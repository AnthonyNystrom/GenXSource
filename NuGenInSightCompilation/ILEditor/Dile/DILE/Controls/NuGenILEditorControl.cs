using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Dile.Disassemble;
using Dile.Disassemble.ILCodes;
using ILEditor.Properties;
using Dile.UI.Debug;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Dile.Controls
{
	public partial class NuGenILEditorControl : RichTextBox
	{
		private const int IndentationSize = 4;

		[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
		public struct CHARFORMAT2
		{
			public int cbSize;
			public int dwMask;
			public int dwEffects;
			public int yHeight;
			public int yOffset;
			public int crTextColor;
			public byte bCharSet;
			public byte bPitchAndFamily;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string szFaceName;
			public short wWeight;
			public short sSpacing;
			public int crBackColor;
			public int lcid;
			public int dwReserved;
			public short sStyle;
			public short wKerning;
			public byte bUnderlineType;
			public byte bAnimation;
			public byte bRevAuthor;
			public byte bReserved1;
		}

		[DllImport("user32.dll")]
		static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		private static List<NuGenOpCodeItem> opCodeItems = null;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public static List<NuGenOpCodeItem> OpCodeItems
		{
			get
			{
				return NuGenILEditorControl.opCodeItems;
			}

			set
			{
				NuGenILEditorControl.opCodeItems = value;
			}
		}

		private static List<string> opCodeNames = null;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public static List<string> OpCodeNames
		{
			get
			{
				return NuGenILEditorControl.opCodeNames;
			}

			set
			{
				NuGenILEditorControl.opCodeNames = value;
			}
		}

		private bool showKeywords = false;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public bool ShowKeywords
		{
			get
			{
				return showKeywords;
			}

			set
			{
				showKeywords = value;
			}
		}

		private bool refreshKeyword = true;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public bool RefreshKeyword
		{
			get
			{
				return refreshKeyword;
			}

			set
			{
				refreshKeyword = value;
			}
		}

		private NuGenParser parser = new NuGenParser(string.Empty);
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public NuGenParser Parser
		{
			get
			{
				return parser;
			}

			set
			{
				parser = value;
			}
		}

		private NuGenIMultiLine codeObject = null;
		public NuGenIMultiLine CodeObject
		{
			get
			{
				return codeObject;
			}

			set
			{
				codeObject = value;
			}
		}

		private static readonly NuGenDefaultLine defaultLine = new NuGenDefaultLine();
		private static NuGenDefaultLine DefaultLine
		{
			get
			{
				return defaultLine;
			}
		}

		private bool isRedrawDisabled = false;
		private bool IsRedrawDisabled
		{
			get
			{
				return isRedrawDisabled;
			}
			set
			{
				isRedrawDisabled = value;
			}
		}

		private NuGenBaseLineDescriptor previousCurrentLine;
		private NuGenBaseLineDescriptor PreviousCurrentLine
		{
			get
			{
				return previousCurrentLine;
			}
			set
			{
				previousCurrentLine = value;
			}
		}

		private NuGenBaseLineDescriptor currentLine = null;
		public NuGenBaseLineDescriptor CurrentLine
		{
			get
			{
				return currentLine;
			}
			set
			{
				PreviousCurrentLine = currentLine;
				currentLine = value;
			}
		}

		private List<NuGenBaseLineDescriptor> specialLines = new List<NuGenBaseLineDescriptor>();
		public List<NuGenBaseLineDescriptor> SpecialLines
		{
			get
			{
				return specialLines;
			}
		}

		public NuGenILEditorControl()
		{
			InitializeComponent();
			SelectionIndent = 70;
			keywordListBox.Parent = this;
			ReadOnly = true;
			BackColor = Color.White;

			if (!DesignMode)
			{
				Initialize();

				keywordListBox.BeginUpdate();
				keywordListBox.Items.Clear();
				NuGenOpCodeItem[] opCodeItemsArray = OpCodeItems.ToArray();
				Array.Sort(opCodeItemsArray);
				keywordListBox.Items.AddRange(opCodeItemsArray);
				keywordListBox.EndUpdate();
			}
		}

		public void ShowCodeObject(NuGenIMultiLine codeObject)
		{
			CodeObject = codeObject;
			StringBuilder ilCodeString = new StringBuilder();

			for (int ilCodeIndex = 0; ilCodeIndex < CodeObject.CodeLines.Count; ilCodeIndex++)
			{
				NuGenCodeLine codeLine = CodeObject.CodeLines[ilCodeIndex];
				string text = codeLine.Text;

				for (int indentationIndex = 0; indentationIndex < codeLine.Indentation; indentationIndex++)
				{
					ilCodeString.Append("    ");
				}

				ilCodeString.AppendLine(codeLine.Text);
			}

			ilCodeString = ilCodeString.Replace("\0", string.Empty);
			Text = ilCodeString.ToString();
		}

		private static string GetReferencedMscorlibPath()
		{
			string result = string.Empty;
			bool found = false;
			AssemblyName[] referencedAssemblies = System.Reflection.Assembly.GetExecutingAssembly().GetReferencedAssemblies();
			int referencedAssembliesIndex = 0;

			while (!found && referencedAssembliesIndex < referencedAssemblies.Length)
			{
				AssemblyName assemblyName = referencedAssemblies[referencedAssembliesIndex++];

				found = (assemblyName.Name == "mscorlib");

				if (found)
				{
					result = System.Reflection.Assembly.Load(assemblyName).Location;
				}
			}

			return result;
		}

		private static void LoadOpCodeItems()
		{
			string line;

			OpCodeItems = new List<NuGenOpCodeItem>();
			using (StringReader reader = new StringReader(Resources.OpCodeItems))
			{
				do
				{
					line = reader.ReadLine();

					if (line != null)
					{
						short opCodeValue = Convert.ToInt16(line);
						string description = reader.ReadLine();

						NuGenOpCodeItem opCodeItem = new NuGenOpCodeItem();
						opCodeItem.OpCodeValue = opCodeValue;
						opCodeItem.Description = description;

						OpCodeItems.Add(opCodeItem);
					}

				} while (line != null);
			}

			OpCodeNames = new List<string>();
			using (StringReader reader = new StringReader(Resources.OpCodeNames))
			{
				do
				{
					line = reader.ReadLine();

					if (line != null)
					{
						OpCodeNames.Add(line);
					}

				} while (line != null);
			}
		}

		//private static void LoadOpCodeItems()
		//{
		//  OpCodeItems = new List<OpCodeItem>();
		//  OpCodeNames = new List<string>();

		//  string mscorlibPath = GetReferencedMscorlibPath();
		//  string mscorlibXmlPath = Path.Combine(Path.GetDirectoryName(mscorlibPath), "mscorlib.xml");
		//  XPathDocument xmlCommentDocument = new XPathDocument(mscorlibXmlPath);
		//  XPathNavigator navigator = xmlCommentDocument.CreateNavigator();
		//  XPathNodeIterator iterator = navigator.Select(string.Format("/doc/members/member[starts-with(translate(@name, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'f:system.reflection.emit.opcodes.')]"));

		//  Dictionary<string, string> descriptions = new Dictionary<string, string>();

		//  while (iterator.MoveNext())
		//  {
		//    string opCodeName = iterator.Current.GetAttribute("name", string.Empty).Substring(33).ToLower();
		//    descriptions[opCodeName] = iterator.Current.Value;
		//  }

		//  foreach (FieldInfo fieldInfo in typeof(OpCodes).GetFields())
		//  {
		//    if (fieldInfo.FieldType == typeof(OpCode))
		//    {
		//      OpCode opCode = (OpCode)fieldInfo.GetValue(fieldInfo.ReflectedType);
		//      string opCodeName = opCode.Name.ToLower();
		//      string description = string.Empty;

		//      if (descriptions.ContainsKey(opCodeName))
		//      {
		//        description = descriptions[opCodeName];
		//      }

		//      OpCodeItems.Add(new OpCodeItem(opCode, description));
		//      OpCodeNames.Add(opCode.Name);
		//    }
		//  }
		//}

		public static void Initialize()
		{
			if (OpCodeItems == null)
			{
				//OpCodeItems = new List<OpCodeItem>();
				//OpCodeNames = new List<string>();
				LoadOpCodeItems();
			}
		}

		private Point GetScrollPosition()
		{
			Point result = new Point();
			IntPtr resultPointer = IntPtr.Zero;

			try
			{
				resultPointer = Marshal.AllocHGlobal(Marshal.SizeOf(result));

				SendMessage(Handle, NuGenConstants.EM_GETSCROLLPOS, IntPtr.Zero, resultPointer);
				result = (Point)Marshal.PtrToStructure(resultPointer, typeof(Point));
			}
			catch
			{
				throw;
			}
			finally
			{
				if (resultPointer != IntPtr.Zero)
				{
					Marshal.DestroyStructure(resultPointer, typeof(Point));
					Marshal.FreeHGlobal(resultPointer);
				}
			}

			return result;
		}

		private void SetScrollPosition(Point position)
		{
			IntPtr positionPointer = IntPtr.Zero;

			try
			{
				positionPointer = Marshal.AllocHGlobal(Marshal.SizeOf(position));
				Marshal.StructureToPtr(position, positionPointer, false);

				SendMessage(Handle, NuGenConstants.EM_SETSCROLLPOS, IntPtr.Zero, positionPointer);
			}
			catch
			{
				throw;
			}
			finally
			{
				if (positionPointer != IntPtr.Zero)
				{
					Marshal.DestroyStructure(positionPointer, typeof(Point));
					Marshal.FreeHGlobal(positionPointer);
				}
			}
		}

		private void SetLineColor(int position, string line, int indentationCount, NuGenBaseLineDescriptor lineDescriptor)
		{
			int indentation = indentationCount * IndentationSize;
			position += indentation;
			line = line.Substring(indentation);

			SelectionStart = position;
			SelectionLength = line.Length;
			SetColor(lineDescriptor.BackColor, lineDescriptor.ForeColor);
		}

		private void SetColor(Color backColor)
		{
			CHARFORMAT2 charFormat = new CHARFORMAT2();
			charFormat.cbSize = Marshal.SizeOf(typeof(CHARFORMAT2));
			charFormat.dwMask = NuGenConstants.CFM_BACKCOLOR;
			charFormat.crBackColor = MakeColorRef(backColor.R, backColor.G, backColor.B);

			IntPtr lparam = IntPtr.Zero;

			try
			{
				lparam = Marshal.AllocHGlobal(charFormat.cbSize);
				IntPtr wparam = new IntPtr(NuGenConstants.SCF_SELECTION);

				Marshal.StructureToPtr(charFormat, lparam, false);

				SendMessage(Handle, NuGenConstants.EM_SETCHARFORMAT, wparam, lparam);
			}
			catch
			{
				throw;
			}
			finally
			{
				if (lparam != IntPtr.Zero)
				{
					Marshal.DestroyStructure(lparam, typeof(CHARFORMAT2));
					Marshal.FreeHGlobal(lparam);
				}
			}
		}

		private void SetColor(Color backColor, Color foreColor)
		{
			CHARFORMAT2 charFormat = new CHARFORMAT2();
			charFormat.cbSize = Marshal.SizeOf(typeof(CHARFORMAT2));
			charFormat.dwMask = NuGenConstants.CFM_BACKCOLOR | NuGenConstants.CFM_COLOR;
			charFormat.crBackColor = MakeColorRef(backColor.R, backColor.G, backColor.B);
			charFormat.crTextColor = MakeColorRef(foreColor.R, foreColor.G, foreColor.B);

			IntPtr lparam = IntPtr.Zero;

			try
			{
				lparam = Marshal.AllocHGlobal(charFormat.cbSize);
				IntPtr wparam = new IntPtr(NuGenConstants.SCF_SELECTION);

				Marshal.StructureToPtr(charFormat, lparam, false);

				SendMessage(Handle, NuGenConstants.EM_SETCHARFORMAT, wparam, lparam);
			}
			catch
			{
				throw;
			}
			finally
			{
				if (lparam != IntPtr.Zero)
				{
					Marshal.DestroyStructure(lparam, typeof(CHARFORMAT2));
					Marshal.FreeHGlobal(lparam);
				}
			}
		}

		private static int MakeColorRef(byte r, byte g, byte b)
		{
			return (int)(((uint)r) | (((uint)g) << 8) | (((uint)b) << 16));
		}

		protected override void OnHScroll(EventArgs e)
		{
			if (!IsRedrawDisabled)
			{
				base.OnHScroll(e);
			}
		}

		protected override void OnVScroll(EventArgs e)
		{
			if (!IsRedrawDisabled)
			{
				base.OnVScroll(e);
			}
		}

		private void RefreshCurrentLine(bool isFormattingCleared, bool keepScrollPosition)
		{
			int position = 0;
			string line = string.Empty;
			NuGenBaseILCode ilCode = null;
			bool isScrollingNeeded = false;
			int selectionStart = SelectionStart;
			int selectionLength = SelectionLength;
			int firstVisibleCharacter = -1;
			int lastVisibleCharacter = -1;
			Point scrollPosition = new Point();

			if (CurrentLine != null)
			{
				position = FindPositionOfILCodeByOffset(CurrentLine.InstructionOffset, out line, out ilCode);
				firstVisibleCharacter = GetCharIndexFromPosition(new Point(0, 0));
				lastVisibleCharacter = GetCharIndexFromPosition(new Point(ClientSize.Width, ClientSize.Height));
				isScrollingNeeded = (position < firstVisibleCharacter || position > lastVisibleCharacter);
			}

			if (!isScrollingNeeded && keepScrollPosition)
			{
				scrollPosition = GetScrollPosition();
			}

			if (!isFormattingCleared && PreviousCurrentLine != null && CodeObject != null)
			{
				string previousLine;
				NuGenBaseILCode previousILCode;
				int previousPosition = FindPositionOfILCodeByOffset(PreviousCurrentLine.InstructionOffset, out previousLine, out previousILCode);
				int indentation = (previousILCode == null ? 0 : previousILCode.Indentation);

				SetLineColor(previousPosition, previousLine, indentation, DefaultLine);
			}

			if (CurrentLine != null && CodeObject != null)
			{
				int indentation = (ilCode == null ? 0 : ilCode.Indentation);
				SetLineColor(position, line, indentation, CurrentLine);
			}

			if (keepScrollPosition)
			{
				if (isScrollingNeeded)
				{
					SelectionLength = 0;
					ScrollToCaret();
				}
				else
				{
					SelectionLength = selectionLength;
					SelectionStart = selectionStart;
					SetScrollPosition(scrollPosition);
				}
			}
		}

		public int RefreshSpecialLines()
		{
			int result = -1;

			foreach (NuGenBaseLineDescriptor lineDescriptor in SpecialLines)
			{
				string line;
				NuGenBaseILCode ilCode;
				int position = FindPositionOfILCodeByOffset(lineDescriptor.InstructionOffset, out line, out ilCode);
				int indentation = (ilCode == null ? 0 : ilCode.Indentation);

				if (lineDescriptor.ScrollToOffset)
				{
					result = position;
				}

				SetLineColor(position, line, indentation, lineDescriptor);
			}

			return result;
		}

		public void RefreshControl(bool forceFormatRefresh)
		{
			RefreshControl(forceFormatRefresh, true);
		}

		public void RefreshControl(bool forceFormatRefresh, uint scrollToOffset)
		{
			RefreshControl(forceFormatRefresh, false);

			string line;
			NuGenBaseILCode ilCode;
			int position = FindPositionOfILCodeByOffset(Convert.ToInt32(scrollToOffset), out line, out ilCode);
			SelectionLength = 0;
			SelectionStart = position;
			ScrollToCaret();
		}

		private void RefreshControl(bool forceFormatRefresh, bool keepScrollPosition)
		{
			DisableRedraw();
			bool isFormattingCleared = false;
			NuGenMethodDefinition methodDefinition = CodeObject as NuGenMethodDefinition;
			bool displayBreakpoints = (methodDefinition != null && NuGenProject.Instance.HasBreakpointsInMethod(methodDefinition));
			bool displaySpecialLines = (SpecialLines.Count > 0);
			Point scrollPosition = new Point();
			int selectionStart = 0;
			int selectionLength = 0;
			bool restoreScrollPosition = true;

			if (keepScrollPosition && (forceFormatRefresh || displayBreakpoints || displaySpecialLines))
			{
				scrollPosition = GetScrollPosition();
				selectionStart = SelectionStart;
				selectionLength = SelectionLength;
				SelectAll();
				SetColor(DefaultLine.BackColor);
				isFormattingCleared = true;
			}

			if (displaySpecialLines)
			{
				int newSelectionStart = RefreshSpecialLines();

				if (newSelectionStart >= 0)
				{
					selectionStart = newSelectionStart;
					selectionLength = 0;
					restoreScrollPosition = false;
				}
			}

			if (displayBreakpoints)
			{
				RefreshBreakpoints(methodDefinition);
			}

			if (keepScrollPosition && (forceFormatRefresh || displayBreakpoints || displaySpecialLines))
			{
				SelectionStart = selectionStart;
				SelectionLength = selectionLength;

				if (restoreScrollPosition)
				{
					SetScrollPosition(scrollPosition);
				}
			}

			RefreshCurrentLine(isFormattingCleared, keepScrollPosition);

			EnableRedraw();
			Refresh();
		}

		private void RefreshBreakpoints(NuGenMethodDefinition methodDefinition)
		{
			foreach (NuGenFunctionBreakpointInformation functionBreakpoint in NuGenProject.Instance.FunctionBreakpoints)
			{
				if (functionBreakpoint.MethodDefinition == methodDefinition)
				{
					RefreshBreakpoint(functionBreakpoint);
				}
			}
		}

		private void RefreshBreakpoint(NuGenFunctionBreakpointInformation breakpoint)
		{
			if (CurrentLine == null || breakpoint.Offset != CurrentLine.InstructionOffset)
			{
				int breakpointOffset = Convert.ToInt32(breakpoint.Offset);
				NuGenBreakpointLine breakpointLine = new NuGenBreakpointLine(breakpoint.State, breakpointOffset);
				string line;
				NuGenBaseILCode ilCode;

				int position = FindPositionOfILCodeByOffset(breakpointOffset, out line, out ilCode);

				if (ilCode != null)
				{
					SetLineColor(position, line, ilCode.Indentation, breakpointLine);
				}
			}
		}

		public void UpdateBreakpoint(NuGenFunctionBreakpointInformation breakpoint)
		{
			DisableRedraw();
			int selectionStart = SelectionStart;
			int selectionLength = SelectionLength;

			RefreshBreakpoint(breakpoint);

			SelectionStart = selectionStart;
			SelectionLength = selectionLength;

			EnableRedraw();
			Refresh();
		}

		protected override void WndProc(ref Message m)
		{
			if (!IsRedrawDisabled || (m.Msg != 0x000F && m.Msg != 0x0014 && m.Msg != 0x0085))
			{
				base.WndProc(ref m);

				if (!DesignMode && m.Msg == 0x000F && CodeObject != null)
				{
					Point firstCharPosition = GetPositionFromCharIndex(0);

					if (firstCharPosition.X >= 0)
					{
						using (Graphics graphics = CreateGraphics())
						{
							Font regularFont = Font;
							Font boldFont = new Font(Font, FontStyle.Bold);
							Font boldItalicFont = new Font(Font, FontStyle.Bold | FontStyle.Italic);
							int charIndex = 1;
							int firstVisibleCharacter = GetCharIndexFromPosition(new Point(0, 0));
							int lastVisibleCharacter = GetCharIndexFromPosition(new Point(ClientSize.Width, ClientSize.Height));

							if (lastVisibleCharacter == 0)
							{
								lastVisibleCharacter = Text.Length;
							}

							for (int ilCodeIndex = 0; ilCodeIndex < CodeObject.CodeLines.Count && charIndex < lastVisibleCharacter; ilCodeIndex++)
							{
								NuGenCodeLine codeLine = CodeObject.CodeLines[ilCodeIndex];

								if (charIndex >= firstVisibleCharacter && charIndex <= lastVisibleCharacter && codeLine is NuGenBaseILCode)
								{
									Point position = GetPositionFromCharIndex(charIndex);
									NuGenBaseILCode ilCode = (NuGenBaseILCode)codeLine;

									Brush brush = Brushes.Black;
									Font currentFont = regularFont;

									graphics.DrawString(ilCode.Address, currentFont, brush, 0 + firstCharPosition.X - SelectionIndent, position.Y);
								}

								charIndex += codeLine.Text.Length + 1 + codeLine.Indentation * IndentationSize;
							}
						}
					}
				}
			}
		}

		protected void EnableRedraw()
		{
			Message m = new Message();
			m.HWnd = this.Handle;
			m.LParam = IntPtr.Zero;
			m.Msg = 0x000B; //wm_setredraw
			m.WParam = new IntPtr(1);
			WndProc(ref m);

			IsRedrawDisabled = false;
		}

		protected void DisableRedraw()
		{
			Message m = new Message();
			m.HWnd = this.Handle;
			m.LParam = IntPtr.Zero;
			m.Msg = 0x000B; //wm_setredraw
			m.WParam = new IntPtr(0);
			WndProc(ref m);

			IsRedrawDisabled = true;
		}

		private string GetLine(int charPosition)
		{
			string result = string.Empty;
			bool found = false;
			int tempCharIndex = 0;
			int lineIndex = 0;

			while (!found && lineIndex < Lines.Length)
			{
				string line = Lines[lineIndex++];

				if (charPosition >= tempCharIndex && charPosition <= tempCharIndex + line.Length)
				{
					found = true;
					result = line;
				}
				else
				{
					tempCharIndex += line.Length + 1;
				}
			}

			return result;
		}

		protected override void OnTextChanged(EventArgs e)
		{
			if (!IsRedrawDisabled)
			{
				base.OnTextChanged(e);

				DisableRedraw();

				int selectionStart = SelectionStart;
				int selectionLength = SelectionLength;
				Font regularFont = new Font(Font, FontStyle.Regular);
				Font boldFont = new Font(Font, FontStyle.Bold);

				Parser = new NuGenParser(Text);
				SelectAll();
				SelectionIndent = 70;
				SelectionColor = Color.Black;
				SelectionFont = regularFont;

				foreach (NuGenComment comment in Parser.Comments)
				{
					SelectionStart = comment.StartPosition;

					if (comment.EndPosition == 0)
					{
						SelectionLength = selectionStart - comment.StartPosition;
					}
					else
					{
						SelectionLength = comment.Length;
					}

					SelectionColor = Color.Green;
				}

				if (!DesignMode)
				{
					foreach (NuGenWord word in Parser.Words)
					{
						if (OpCodeNames.Contains(word.WordBuilder.ToString()) && word.IsFirstWordInLine)
						{
							SelectionStart = word.StartPosition;
							SelectionLength = word.Length;
							SelectionFont = boldFont;
						}
					}
				}

				SelectionStart = selectionStart;
				SelectionLength = selectionLength;

				EnableRedraw();
				Invalidate();
			}
		}

		private void SetKeyword(string keywordHint)
		{
			int keywordHintIndex = keywordListBox.FindString(keywordHint);

			if (keywordHintIndex != -1)
			{
				keywordListBox.SelectedIndex = keywordHintIndex;
			}
		}

		private void ShowKeywordsListBox(Point position, string keywordHint)
		{
			position.Y += 20;
			keywordListBox.Location = position;
			SetKeyword(keywordHint);
			keywordListBox.Show();
			keywordListBox.Refresh();
		}

		private void SendKeyDownMessage(IntPtr handle, Keys keys)
		{
			SendMessage(handle, NuGenConstants.WM_KEYDOWN, new IntPtr((int)keys), new IntPtr(0x20000000));
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			bool result = false;
			RefreshKeyword = true;

			if (msg.Msg == NuGenConstants.WM_KEYDOWN || msg.Msg == NuGenConstants.WM_SYSKEYDOWN)
			{
				ShowKeywords = ((keyData == (Keys.Control | Keys.Space)) || (keyData == (Keys.Control | Keys.J)));

				if (ShowKeywords)
				{
					result = true;
				}

				if (!ShowKeywords && keywordListBox.Visible)
				{
					switch (keyData)
					{
						case Keys.Up:
							if (keywordListBox.SelectedIndex > 0)
							{
								keywordListBox.SelectedIndex--;
							}
							result = true;
							RefreshKeyword = false;
							break;

						case Keys.Down:
							if (keywordListBox.SelectedIndex < keywordListBox.Items.Count - 1)
							{
								keywordListBox.SelectedIndex++;
							}
							result = true;
							RefreshKeyword = false;
							break;

						case Keys.PageUp:
							keywordListBox.SelectedIndex -= (keywordListBox.SelectedIndex < 8 ? keywordListBox.SelectedIndex : 7);
							result = true;
							RefreshKeyword = false;
							break;

						case Keys.PageDown:
							keywordListBox.SelectedIndex += (keywordListBox.Items.Count - keywordListBox.SelectedIndex < 8 ? keywordListBox.Items.Count - keywordListBox.SelectedIndex - 1 : 7);
							result = true;
							RefreshKeyword = false;
							break;

						case Keys.Enter:
						case Keys.Tab:
							result = true;
							RefreshKeyword = false;
							break;
					}
				}

				if (keyData == (Keys.Control | Keys.Tab))
				{
					SendKeyDownMessage(Parent.Handle, keyData);
				}
			}

			if (!result)
			{
				result = base.ProcessCmdKey(ref msg, keyData);
			}

			return result;
		}

		private int FindPositionOfILCodeByOffset(int offset)
		{
			string line;
			NuGenBaseILCode ilCode;

			return FindPositionOfILCodeByOffset(offset, out line, out ilCode);
		}

		private int FindPositionOfILCodeByOffset(int offset, out string line, out NuGenBaseILCode ilCode)
		{
			int result = 0;
			int codeLineIndex = 0;
			int lineIndex = 0;
			bool found = false;
			line = string.Empty;
			ilCode = null;

			while (!found && codeLineIndex < CodeObject.CodeLines.Count)
			{
				NuGenCodeLine codeLine = CodeObject.CodeLines[codeLineIndex];
				ilCode = codeLine as NuGenBaseILCode;

				if (ilCode != null && ilCode.Offset >= offset)
				{
					line = Lines[lineIndex];
					found = true;
				}
				else
				{
					for (int lineNumber = 0; lineNumber < codeLine.TextLineNumber + 1; lineNumber++)
					{
						result += Lines[lineIndex].Length + 1;
						lineIndex++;
					}
				}

				codeLineIndex++;
			}

			return result;
		}

		public NuGenBaseILCode GetILCodeAtMouseCursor()
		{
			int mouseCharIndex = GetCharIndexFromPosition(PointToClient(Cursor.Position));

			return FindILCodeByIndex(mouseCharIndex);
		}

		private NuGenBaseILCode FindILCodeByIndex(int position)
		{
			NuGenBaseILCode result = null;
			int lineIndex = 0;
			int codeLineIndex = 0;
			bool found = false;

			while (!found && codeLineIndex < CodeObject.CodeLines.Count)
			{
				NuGenCodeLine codeLine = CodeObject.CodeLines[codeLineIndex];
				position -= Lines[lineIndex].Length + 1;

				for (int lineNumber = 0; lineNumber < codeLine.TextLineNumber; lineNumber++)
				{
					lineIndex++;
					position -= Lines[lineIndex].Length + 1;
				}

				if (position < 0)
				{
					result = codeLine as NuGenBaseILCode;
					found = true;
				}

				lineIndex++;
				codeLineIndex++;
			}

			return result;
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			NuGenWord hintWord = Parser.FindWordByExactPosition(SelectionStart);

			if (keywordListBox.Visible && RefreshKeyword && hintWord != null)
			{
				SetKeyword(hintWord.WordBuilder.ToString());
			}
			else if (ShowKeywords)
			{
				if (hintWord == null)
				{
					hintWord = Parser.FindWordByPosition(SelectionStart);

					if (hintWord == null || (hintWord != null && hintWord.LineNumber != GetLineFromCharIndex(SelectionStart)))
					{
						ShowKeywordsListBox(GetPositionFromCharIndex(SelectionStart), string.Empty);
					}
				}
				else if (hintWord.IsFirstWordInLine)
				{
					ShowKeywordsListBox(GetPositionFromCharIndex(hintWord.StartPosition), hintWord.WordBuilder.ToString());
				}
			}

			if (keywordListBox.Visible && e.KeyData == Keys.Escape)
			{
				keywordListBox.Visible = false;
			}

			if (!e.Alt && !e.Control && !e.Shift && (e.KeyData == Keys.Tab || e.KeyData == Keys.Space || e.KeyData == Keys.Enter) && keywordListBox.Visible)
			{
				DisableRedraw();

				if (hintWord != null)
				{
					Select(hintWord.StartPosition, hintWord.Length);
				}

				SelectedText = ((NuGenOpCodeItem)keywordListBox.SelectedItem).OpCode.Name;
				keywordListBox.Visible = false;
				EnableRedraw();
			}

			base.OnKeyUp(e);
		}

		public void SetBreakpointAtSelection()
		{
			NuGenMethodDefinition methodDefinition = CodeObject as NuGenMethodDefinition;

			if (methodDefinition != null)
			{
				NuGenBaseILCode ilCode = FindILCodeByIndex(SelectionStart);

				if (ilCode != null)
				{
					NuGenFunctionBreakpointInformation breakpointInformation = NuGenBreakpointHandler.Instance.AddRemoveBreakpoint(methodDefinition, ilCode.Offset, false);

					if (breakpointInformation != null)
					{
						UpdateBreakpoint(breakpointInformation);
						NuGenProject.Instance.IsSaved = false;
					}
				}
			}
		}

		public void SetRunToCursorAtSelection()
		{
			NuGenMethodDefinition methodDefinition = CodeObject as NuGenMethodDefinition;

			if (methodDefinition != null)
			{
				NuGenBaseILCode ilCode = FindILCodeByIndex(SelectionStart);

				if (ilCode != null)
				{
					NuGenBreakpointHandler.Instance.RunToCursor(methodDefinition, ilCode.Offset, false);
				}
			}
		}
	}
}