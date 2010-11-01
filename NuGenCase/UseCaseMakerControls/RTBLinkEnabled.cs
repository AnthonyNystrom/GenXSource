using System;
using System.ComponentModel;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;

namespace UseCaseMakerControls
{
	public class LinkEnabledRTB : RichTextBox
	{
		#region Members
		//Members exposed via properties
		private SepararatorCollection mSeparators = new SepararatorCollection();  
		private HighLightDescriptorCollection mHighlightDescriptors = new HighLightDescriptorCollection();
		private bool mCaseSesitive = false;
		private bool mFilterAutoComplete = false;
		private string lastTokenClicked = "";

		//Internal use members
		private bool mAutoCompleteShown = false;
		private bool mParsing = false;
		private bool mIgnoreLostFocus = false;
		private ToolTip mTooltip = new ToolTip();
		private AutoCompleteForm mAutoCompleteForm = new AutoCompleteForm();

		// Events
		public event MouseOverTokenEventHandler				MouseOverToken;
		public event ItemTextChangedEventHandler			ItemTextChanged;
		public event ItemClickEventHandler					ItemClick;
		public event ItemTextEnterEventHandler				ItemTextEnter;
		public event ItemTextSelectionChangedEventHandler	ItemTextSelectionChanged;

		//Undo/Redo members
		private ArrayList mUndoList = new ArrayList();
		private Stack mRedoStack = new Stack();
		private bool mIsUndo = false;
		private UndoRedoInfo mLastInfo = new UndoRedoInfo("", new Win32.POINT(), 0);
		private int mMaxUndoRedoSteps = 50;

		#endregion

		#region Constructors
		void LinkEnableRTB()
		{
			DetectUrls = false;
			mTooltip.AutoPopDelay = 3000;
			mTooltip.InitialDelay = 1000;
			mTooltip.ReshowDelay = 1000;
			mTooltip.ShowAlways = false;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Determines if token recognition is case sensitive.
		/// </summary>
		[Category("Behavior")]
		public bool CaseSensitive 
		{ 
			get 
			{ 
				return mCaseSesitive; 
			}
			set 
			{ 
				mCaseSesitive = value;
			}
		}

		/// <summary>
		/// Sets whether or not to remove items from the Autocomplete window as the user types...
		/// </summary>
		[Category("Behavior")]
		public bool FilterAutoComplete 
		{
			get 
			{
				return mFilterAutoComplete;
			}
			set 
			{
				mFilterAutoComplete = value;
			}
		}

		/// <summary>
		/// Set the maximum amount of Undo/Redo steps.
		/// </summary>
		[Category("Behavior")]
		public int MaxUndoRedoSteps 
		{
			get 
			{
				return mMaxUndoRedoSteps;
			}
			set
			{
				mMaxUndoRedoSteps = value;
			}
		}

		/// <summary>
		/// A collection of charecters. a token is every string between two separators.
		/// </summary>
		/// 
		public SepararatorCollection Separators 
		{
			get 
			{
				return mSeparators;
			}
			set
			{
				mSeparators = value;
			}
		}
		
		/// <summary>
		/// The collection of highlight descriptors.
		/// </summary>
		/// 
		public HighLightDescriptorCollection HighlightDescriptors 
		{
			get 
			{
				return mHighlightDescriptors;
			}
			set
			{
				mHighlightDescriptors = value;
			}
		}

		public ToolTip ToolTip
		{
			get
			{
				return mTooltip;
			}
		}

		public string LastTokenClicked
		{
			get
			{
				return lastTokenClicked;
			}
		}
		#endregion

		#region Public Methods
		public void ParseNow()
		{
			this.OnTextChanged(new EventArgs());
		}
		#endregion

		#region Protected Methods
		protected virtual void OnMouseOverToken(MouseOverTokenEventArgs e)
		{
			if(MouseOverToken != null)
			{
				MouseOverToken(this,e);
			}
		}

		protected virtual void OnItemTextChanged(ItemTextChangedEventArgs e)
		{
			if(ItemTextChanged != null)
			{
				ItemTextChanged(this,e);
			}
		}

		protected virtual void OnItemClick(MouseOverTokenEventArgs e)
		{
			if(ItemClick != null)
			{
				ItemClick(this,e);
			}
		}

		protected virtual void OnItemTextEnter(ItemTextChangedEventArgs e)
		{
			if(ItemTextEnter != null)
			{
				ItemTextEnter(this,e);
			}
		}

		protected virtual void OnItemTextSelectionChanged(ItemTextChangedEventArgs e)
		{
			if(ItemTextSelectionChanged  != null)
			{
				ItemTextSelectionChanged(this,e);
			}
		}
		#endregion

		#region Overriden methods

		/// <summary>
		/// The on text changed overrided. Here we parse the text into RTF for the highlighting.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTextChanged(EventArgs e)
		{
			if(mParsing) return;

			if(this.HighlightDescriptors.Count == 0)
			{
				base.OnTextChanged(e);
				OnItemTextChanged(new ItemTextChangedEventArgs(this));
				return;
			}

			mParsing = true;
			Win32.SendMessage(Handle,Win32.WM_SETREDRAW,0,(IntPtr)0);
			base.OnTextChanged(e);

			if (!mIsUndo)
			{
				mRedoStack.Clear();
				mUndoList.Insert(0,mLastInfo);
				this.LimitUndo();
				mLastInfo = new UndoRedoInfo(Text, GetScrollPos(), SelectionStart);
			}
			
			//Save scroll bar an cursor position, changeing the RTF moves the cursor and scrollbars to top positin
			Win32.POINT scrollPos = GetScrollPos();
			int cursorLoc = SelectionStart;

			this.Select(0,this.Text.Length);
			this.SelectionFont = Parent.Font;
			this.SelectionColor = SystemColors.WindowText;

			int lastPos;
			int tokenLength;
			string token;
			foreach(HighlightDescriptor hd in this.HighlightDescriptors)
			{
				token = hd.Token.Replace("\t"," ");
				token = token.Replace("\v",".");
				lastPos = 0;
				tokenLength = token.Length;
				while(true)
				{
					lastPos = this.Find(token,lastPos,RichTextBoxFinds.WholeWord);
					if(lastPos == -1 || lastPos >= this.Text.Length)
					{
						break;
					}
					this.Select(lastPos,tokenLength);
					this.SelectionColor = hd.Color;
					this.Select(lastPos+tokenLength,1);
					this.SelectionColor = SystemColors.WindowText;
					lastPos += 1;
				}
			}

			//Restore cursor and scrollbars location.
			this.SelectionStart = cursorLoc;
			this.SelectionLength = 0;

			SetScrollPos(scrollPos);
			Win32.SendMessage(Handle,Win32.WM_SETREDRAW,1,(IntPtr)0);
			Invalidate();
			
			if (mAutoCompleteShown)
			{
				if (mFilterAutoComplete)
				{
					SetAutoCompleteItems();
					SetAutoCompleteSize();
					SetAutoCompleteLocation(false);
				}
				SetBestSelectedAutoCompleteItem();
			}

			mParsing = false;

			OnItemTextChanged(new ItemTextChangedEventArgs(this));
		}

		protected override void OnVScroll(EventArgs e)
		{
			if (mParsing) return;
			base.OnVScroll(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			HideAutoCompleteForm();
			base.OnMouseDown (e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			string token = CurrentToken(e);
			if(token != string.Empty)
			{
				MouseOverTokenEventArgs mot = new MouseOverTokenEventArgs(
					e.Button,
					e.Clicks,
					e.X,
					e.Y,
					e.Delta,
					this,
					token,
					true);
				OnMouseOverToken(mot);
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			string token = CurrentToken(e);
			if(token != string.Empty)
			{
				lastTokenClicked = token;
			}
			MouseOverTokenEventArgs mot = new MouseOverTokenEventArgs(
				e.Button,
				e.Clicks,
				e.X,
				e.Y,
				e.Delta,					
				this,
				token,
				(token != string.Empty) ? true : false);
			OnItemClick(mot);
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);

			this.OnItemTextEnter(new ItemTextChangedEventArgs(this));
		}

		protected override void OnSelectionChanged(EventArgs e)
		{
			base.OnSelectionChanged(e);
			this.OnItemTextSelectionChanged(new ItemTextChangedEventArgs(this));
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(msg.Msg == Win32.WM_KEYDOWN || msg.Msg == Win32.WM_SYSKEYDOWN)
			{
				if(mAutoCompleteShown &&
					(keyData == Keys.Tab ||
					keyData == Keys.Enter ||
					keyData == Keys.Space))
				{
					AcceptAutoCompleteItem();
					return true;
				}
			}
			return base.ProcessCmdKey (ref msg, keyData);
		}

		/// <summary>
		/// Taking care of Keyboard events
		/// </summary>
		/// <param name="m"></param>
		/// <remarks>
		/// Since even when overriding the OnKeyDown methoed and not calling the base function 
		/// you don't have full control of the input, I've decided to catch windows messages to handle them.
		/// </remarks>
		protected override void WndProc(ref Message m)
		{
			switch(m.Msg)
			{
//				case Win32.WM_PAINT:
//				{
//					//Don't draw the control while parsing to avoid flicker.
//					if(mParsing)
//					{
//						return;
//					}
//					break;
//				}
				case Win32.WM_KEYDOWN:
				case Win32.WM_SYSKEYDOWN:
				{
					if(mAutoCompleteShown)
					{
						switch((Keys)(int)m.WParam)
						{
							case Keys.Down:
							{
								if(mAutoCompleteForm.Items.Count != 0)
								{
									mAutoCompleteForm.SelectedIndex = (mAutoCompleteForm.SelectedIndex + 1) % mAutoCompleteForm.Items.Count;
								}
								return;
							}
							case Keys.Up:
							{
								if(mAutoCompleteForm.Items.Count != 0)
								{
									if(mAutoCompleteForm.SelectedIndex < 1)
									{
										mAutoCompleteForm.SelectedIndex = mAutoCompleteForm.Items.Count - 1;
									}
									else
									{
										mAutoCompleteForm.SelectedIndex--;
									}
								}
								return;
							}
							case Keys.Escape:
							{
								HideAutoCompleteForm();
								return;
							}
						}
					}
					else
					{
						if(((Keys)(int)m.WParam == Keys.Space) && 
							Control.ModifierKeys == Keys.Control)
						{
							if(this.HighlightDescriptors.Count > 0)
							{
								ShowAutoComplete();
							}
						} 
						else if(((Keys)(int)m.WParam == Keys.Z) &&
							Control.ModifierKeys == Keys.Control)
						{
							Undo();
							return;
						}
						else if(((Keys)(int)m.WParam == Keys.Y) && 
							Control.ModifierKeys == Keys.Control)
						{
							Redo();
							return;
						}
					}
					break;
				}
				case Win32.WM_CHAR:
				{
					switch((Keys)(int)m.WParam)
					{
						case Keys.Space:
							if(Control.ModifierKeys == Keys.Control)
							{
								return;
							}
							break;
						case Keys.Back:
						case Keys.Delete:
							if(this.TagCount() % 2 == 0)
							{
								HideAutoCompleteForm();
							}
							break;
					}
					if((int)m.WParam == '\"')
					{
						int counter = this.TagCount() + 1;
						if(mAutoCompleteShown && counter % 2 == 0)
						{
							HideAutoCompleteForm();
						}
						else if(!mAutoCompleteShown && counter % 2 != 0)
						{
							if(this.HighlightDescriptors.Count > 0)
							{
								ShowAutoComplete();
							}
						}
					}
					if((int)m.WParam == '.')
					{
						if(mAutoCompleteShown)
						{
							HideAutoCompleteForm();
						}
					}
				}
				break;
			}
			base.WndProc(ref m);
		}

		/// <summary>
		/// Hides the AutoComplete form when losing focus on textbox.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLostFocus(EventArgs e)
		{
			if(!mIgnoreLostFocus)
			{
				HideAutoCompleteForm();
			}
			base.OnLostFocus (e);
		}
		#endregion

		#region Private Methods
		private int TagCount()
		{
			int counter = 0;
			char [] chars = this.Text.ToCharArray();
			foreach(char c in chars)
			{
				if(c == '\"')
				{
					counter += 1;
				}
			}

			return counter;
		}

		private string CurrentToken(MouseEventArgs e)
		{
			bool found = false;

			if(this.Text.Length == 0)
			{
				return string.Empty;
			}
			
			char [] separators = this.Separators.GetAsCharArray();

			int charIndex;

			string c = string.Empty;
			string token = string.Empty;
			string compositeToken = string.Empty;
			string retValue = string.Empty;
			int tokenEndIndex;

			// Composite token
			found = false;
			charIndex = GetCharIndexFromPosition(new Point(e.X,e.Y));
			while(charIndex >= 0 && charIndex < this.Text.Length)
			{
				c = this.Text.Substring(charIndex,1);
				if(c == "\"")
				{
					found = true;
					break;
				}
				charIndex -= 1;
			}

			if(found)
			{
				charIndex += 1;
				
				if(c == "\"")
				{
					tokenEndIndex = this.Text.IndexOf("\"",charIndex);
					if(tokenEndIndex == -1)
					{
						tokenEndIndex = this.Text.Length;
					}
					
					compositeToken = "\"" + this.Text.Substring(charIndex,tokenEndIndex - charIndex) + "\"";
					compositeToken = compositeToken.Replace(" ","\t");
					compositeToken = compositeToken.Replace(".","\v");
				}
			}

			// Single word token
			charIndex = GetCharIndexFromPosition(new Point(e.X,e.Y));
			while(charIndex >= 0 && charIndex < this.Text.Length)
			{
				c = this.Text.Substring(charIndex,1);
				if(c.IndexOfAny(separators) != -1)
				{
					found = true;
					break;
				}
				charIndex -= 1;
			}

			charIndex += 1;

			if(charIndex > this.Text.Length)
			{
				return string.Empty;
			}

			tokenEndIndex = this.Text.IndexOfAny(separators,charIndex);
			if(tokenEndIndex == -1)
			{
				tokenEndIndex = this.Text.Length;
			}
			
			token = this.Text.Substring(charIndex,tokenEndIndex - charIndex);

			foreach(HighlightDescriptor hd in this.HighlightDescriptors)
			{
				if(hd.Token == token)
				{
					token = token.Replace("\"","");
					retValue = token;
					break;
				}
				if(hd.Token == compositeToken)
				{
					compositeToken = compositeToken.Replace("\t"," ");
					compositeToken = compositeToken.Replace("\v",".");
					compositeToken = compositeToken.Replace("\"","");
					retValue = compositeToken;
					break;
				}
			}

			return retValue;
		}
		#endregion

		#region Undo/Redo Code
		public new bool CanUndo 
		{
			get 
			{
				return mUndoList.Count > 0;
			}
		}
		public new bool CanRedo
		{
			get 
			{
				return mRedoStack.Count > 0;
			}
		}

		private void LimitUndo()
		{
			while (mUndoList.Count > mMaxUndoRedoSteps)
			{
				mUndoList.RemoveAt(mMaxUndoRedoSteps);
			}
		}

		public new void Undo()
		{
			if (!CanUndo)
				return;
			mIsUndo = true;
			mRedoStack.Push(new UndoRedoInfo(Text, GetScrollPos(), SelectionStart));
			UndoRedoInfo info = (UndoRedoInfo)mUndoList[0];
			mUndoList.RemoveAt(0);
			Text = info.Text;
			SelectionStart = info.CursorLocation;
			SetScrollPos(info.ScrollPos);
			mLastInfo = info;
			mIsUndo = false;
		}
		public new void Redo()
		{
			if (!CanRedo)
				return;
			mIsUndo = true;
			mUndoList.Insert(0,new UndoRedoInfo(Text, GetScrollPos(), SelectionStart));
			LimitUndo();
			UndoRedoInfo info = (UndoRedoInfo)mRedoStack.Pop();
			Text = info.Text;
			SelectionStart = info.CursorLocation;
			SetScrollPos(info.ScrollPos);
			mIsUndo = false;
		}

		private class UndoRedoInfo
		{
			public UndoRedoInfo(string text, Win32.POINT scrollPos, int cursorLoc)
			{
				Text = text;
				ScrollPos = scrollPos;
				CursorLocation = cursorLoc;
			}
			public readonly Win32.POINT ScrollPos;
			public readonly int CursorLocation;
			public readonly string Text;
		}
		#endregion

		#region AutoComplete functions

		/// <summary>
		/// Entry point to autocomplete mechanism.
		/// Tries to complete the current word. if it fails it shows the AutoComplete form.
		/// </summary>
		private void CompleteWord()
		{
			int curTokenStartIndex = Text.LastIndexOfAny(mSeparators.GetAsCharArray(), Math.Min(SelectionStart, Text.Length - 1))+1;
			int curTokenEndIndex= Text.IndexOfAny(mSeparators.GetAsCharArray(), SelectionStart);
			if (curTokenEndIndex == -1) 
			{
				curTokenEndIndex = Text.Length;
			}
			string curTokenString = Text.Substring(curTokenStartIndex, Math.Max(curTokenEndIndex - curTokenStartIndex,0)).ToUpper();
			
			string token = null;
			foreach (HighlightDescriptor hd in mHighlightDescriptors)
			{
				if (hd.UseForAutoComplete && hd.Token.ToUpper().StartsWith(curTokenString))
				{
					if (token == null)
					{
						token = hd.Token;
					}
					else
					{
						token = null;
						break;
					}
				}
			}
			if (token == null)
			{
				ShowAutoComplete();
			}
			else
			{
				SelectionStart = curTokenStartIndex;
				SelectionLength = curTokenEndIndex - curTokenStartIndex;
				SelectedText = token;
				SelectionStart = SelectionStart + SelectionLength;
				SelectionLength = 0;
			}
		}

		/// <summary>
		/// replace the current word of the cursor with the one from the AutoComplete form and closes it.
		/// </summary>
		/// <returns>If the operation was succesful</returns>
		private bool AcceptAutoCompleteItem()
		{		
			if (mAutoCompleteForm.SelectedItem == null)
			{
				return false;
			}
			
			int curTokenStartIndex = Text.LastIndexOfAny(mSeparators.GetAsCharArray(), Math.Min(SelectionStart, Text.Length - 1)) + 1;
			if(SelectionStart > 0)
			{
				int tmpTokenStartIndex = Text.LastIndexOfAny(mSeparators.GetAsCharArray(), Math.Min(SelectionStart - 1, Text.Length - 1)) + 1;
				if(tmpTokenStartIndex < curTokenStartIndex)
				{
					curTokenStartIndex = tmpTokenStartIndex;
				}
			}
			int curTokenEndIndex= Text.IndexOfAny(mSeparators.GetAsCharArray(), SelectionStart);
			if(curTokenEndIndex == -1) 
			{
				curTokenEndIndex = Text.Length;
			}
			SelectionStart = Math.Max(curTokenStartIndex, 0);
			SelectionLength = Math.Max(0,curTokenEndIndex - curTokenStartIndex);
			if(SelectedText[0] != '\"')
			{
				SelectedText = "\"";
			}
			else
			{
				SelectedText = "";
			}
			SelectedText += mAutoCompleteForm.SelectedItem;
			SelectionStart = SelectionStart + SelectionLength;
			SelectionLength = 0;

			HideAutoCompleteForm();
			return true;
		}

		/// <summary>
		/// Finds the and sets the best matching token as the selected item in the AutoCompleteForm.
		/// </summary>
		private void SetBestSelectedAutoCompleteItem()
		{
			if(this.Text.Length == 0)
			{
				return;
			}

			int curTokenStartIndex = Text.LastIndexOfAny(mSeparators.GetAsCharArray(), Math.Min(SelectionStart, Text.Length - 1)) + 1;
			if(SelectionStart > 0)
			{
				int tmpTokenStartIndex = Text.LastIndexOfAny(mSeparators.GetAsCharArray(), Math.Min(SelectionStart - 1, Text.Length - 1)) + 1;
				if(tmpTokenStartIndex < curTokenStartIndex)
				{
					curTokenStartIndex = tmpTokenStartIndex;
				}
			}
			int curTokenEndIndex  = Text.IndexOfAny(mSeparators.GetAsCharArray(), SelectionStart);
			if(curTokenEndIndex == -1)
			{
				curTokenEndIndex = Text.Length;
			}
			string curTokenString = Text.Substring(curTokenStartIndex, Math.Max(curTokenEndIndex - curTokenStartIndex,0)).ToUpper();
			if(curTokenString.Length > 0)
			{
				if(curTokenString[0] != '\"')
				{
					curTokenString = "\"" + curTokenString;
				}
			}
			
			if((mAutoCompleteForm.SelectedItem != null) && 
				mAutoCompleteForm.SelectedItem.ToUpper().StartsWith(curTokenString))
			{
				return;
			}

			int target = 0;
			for(int index = mAutoCompleteForm.Items.Count - 1; index >= 0; index--)
			{
				if(curTokenString.CompareTo(mAutoCompleteForm.Items[index]) > 0)
				{
					target = index + 1;
					break;
				}
				else if(curTokenString.CompareTo(mAutoCompleteForm.Items[index]) == 0)
				{
					target = index;
					break;
				}
			}

			if(target > 0 && target == mAutoCompleteForm.Items.Count)
			{
				target = mAutoCompleteForm.Items.Count - 1;
			}

			mAutoCompleteForm.SelectedIndex = target;
		}

		/// <summary>
		/// Sets the items for the AutoComplete form.
		/// </summary>
		private void SetAutoCompleteItems()
		{
			mAutoCompleteForm.Items.Clear();
			string filterString = "";
			if (mFilterAutoComplete)
			{
			
				int filterTokenStartIndex = Text.LastIndexOfAny(mSeparators.GetAsCharArray(), Math.Min(SelectionStart, Text.Length - 1))+1;
				int filterTokenEndIndex= Text.IndexOfAny(mSeparators.GetAsCharArray(), SelectionStart);
				if (filterTokenEndIndex == -1) 
				{
					filterTokenEndIndex = Text.Length;
				}
				filterString = Text.Substring(filterTokenStartIndex, filterTokenEndIndex - filterTokenStartIndex).ToUpper();
			}
		
			foreach (HighlightDescriptor hd in mHighlightDescriptors)
			{
				if (hd.Token.ToUpper().StartsWith(filterString) && hd.UseForAutoComplete)
				{
					string sub = hd.Token;
					sub = sub.Replace("\t"," ");
					sub = sub.Replace("\v",".");
					mAutoCompleteForm.Items.Add(sub);
				}
			}
			mAutoCompleteForm.UpdateView();
		}
		
		/// <summary>
		/// Sets the size. the size is limited by the MaxSize property in the form itself.
		/// </summary>
		private void SetAutoCompleteSize()
		{
			mAutoCompleteForm.Height = Math.Min(
				Math.Max(mAutoCompleteForm.Items.Count, 1) * mAutoCompleteForm.ItemHeight + 4, 
				mAutoCompleteForm.MaximumSize.Height);
		}

		/// <summary>
		/// closes the AutoCompleteForm.
		/// </summary>
		private void HideAutoCompleteForm()
		{
			mAutoCompleteForm.Visible = false;
			mAutoCompleteShown = false;
		}
		
		/// <summary>
		/// Sets the location of the AutoComplete form, maiking sure it's on the screen where the cursor is.
		/// </summary>
		/// <param name="moveHorizontly">determines wheather or not to move the form horizontly.</param>
		private void SetAutoCompleteLocation(bool moveHorizontly)
		{
			Point cursorLocation = GetPositionFromCharIndex(SelectionStart);
			Screen screen = Screen.FromControl(this); // Screen.FromPoint(cursorLocation);
			Point optimalLocation = new Point(PointToScreen(cursorLocation).X-15, (int)(PointToScreen(cursorLocation).Y + Font.Size*2 + 2));
			Rectangle desiredPlace = new Rectangle(optimalLocation , mAutoCompleteForm.Size);
			desiredPlace.Width = 152;
			if (desiredPlace.Left < screen.Bounds.Left) 
			{
				desiredPlace.X = screen.Bounds.Left;
			}
			if (desiredPlace.Right > screen.Bounds.Right)
			{
				desiredPlace.X -= (desiredPlace.Right - screen.Bounds.Right);
			}
			if (desiredPlace.Bottom > screen.Bounds.Bottom)
			{
				desiredPlace.Y = cursorLocation.Y - 2 - desiredPlace.Height;
			}
			if (!moveHorizontly)
			{
				desiredPlace.X = mAutoCompleteForm.Left;
			}

			mAutoCompleteForm.Bounds = desiredPlace;
		}

		/// <summary>
		/// Shows the Autocomplete form.
		/// </summary>
		public void ShowAutoComplete()
		{
			SetAutoCompleteItems();
			SetAutoCompleteSize();
			SetAutoCompleteLocation(true);
			mIgnoreLostFocus = true;
			mAutoCompleteForm.Visible = true;
			SetBestSelectedAutoCompleteItem();
			mAutoCompleteShown = true;
			Focus();
			mIgnoreLostFocus = false;
		}

		#endregion 

		#region Scrollbar positions functions
		/// <summary>
		/// Sends a win32 message to get the scrollbars' position.
		/// </summary>
		/// <returns>a POINT structore containing horizontal and vertical scrollbar position.</returns>
		private unsafe Win32.POINT GetScrollPos()
		{
			Win32.POINT res = new Win32.POINT();
			IntPtr ptr = new IntPtr(&res);
			Win32.SendMessage(Handle, Win32.EM_GETSCROLLPOS, 0, ptr);
			return res;

		}

		/// <summary>
		/// Sends a win32 message to set scrollbars position.
		/// </summary>
		/// <param name="point">a POINT conatining H/Vscrollbar scrollpos.</param>
		private unsafe void SetScrollPos(Win32.POINT point)
		{
			IntPtr ptr = new IntPtr(&point);
			Win32.SendMessage(Handle, Win32.EM_SETSCROLLPOS, 0, ptr);

		}
		#endregion
	}
}
