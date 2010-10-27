using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Genetibase.Debug
{
	/// <summary>
	/// BufferedTextView Control
	/// </summary>
	public class NuGenTViewCtrl : System.Windows.Forms.UserControl
	{
		#region Private Instance Variables

		private System.Windows.Forms.HScrollBar hScrollBar;
		private System.Windows.Forms.VScrollBar vScrollBar;
		private NuGenTBase NuGenTBase;
		private System.ComponentModel.Container components = null;
		private bool _scrollOnAdd;
		private bool _shiftPressed;
		private int _mouseDownRow;
		#endregion

		#region Constructors
		
		/// <summary>
		/// Initialises an instance of the BufferTextViewControl class
		/// </summary>
		public NuGenTViewCtrl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Set default property values
			_scrollOnAdd = true;
			_shiftPressed = false;
			_mouseDownRow = 0;
			this.Font = new Font("Courier New", 10.0F);
		}

		#endregion
        
		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.hScrollBar = new System.Windows.Forms.HScrollBar();
			this.vScrollBar = new System.Windows.Forms.VScrollBar();
			this.NuGenTBase = new NuGenTBase();
			this.SuspendLayout();
			// 
			// hScrollBar
			// 
			this.hScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.hScrollBar.Location = new System.Drawing.Point(0, 159);
			this.hScrollBar.Name = "hScrollBar";
			this.hScrollBar.Size = new System.Drawing.Size(248, 17);
			this.hScrollBar.TabIndex = 0;
			this.hScrollBar.ValueChanged += new System.EventHandler(this.hScrollBar_ValueChanged);
			// 
			// vScrollBar
			// 
			this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
			this.vScrollBar.Location = new System.Drawing.Point(231, 0);
			this.vScrollBar.Name = "vScrollBar";
			this.vScrollBar.Size = new System.Drawing.Size(17, 159);
			this.vScrollBar.TabIndex = 1;
			this.vScrollBar.ValueChanged += new System.EventHandler(this.vScrollBar_ValueChanged);
			// 
			// NuGenTBase
			// 
			this.NuGenTBase.Dock = System.Windows.Forms.DockStyle.Fill;
			this.NuGenTBase.Location = new System.Drawing.Point(0, 0);
			this.NuGenTBase.Name = "NuGenTBase";
			this.NuGenTBase.Size = new System.Drawing.Size(231, 159);
			this.NuGenTBase.TabIndex = 2;
			this.NuGenTBase.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NuGenTBase_KeyUp);
			this.NuGenTBase.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NuGenTBase_KeyDown);
			this.NuGenTBase.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NuGenTBase_MouseDown);
			// 
			// NuGenTViewCtrl
			// 
			this.Controls.Add(this.NuGenTBase);
			this.Controls.Add(this.vScrollBar);
			this.Controls.Add(this.hScrollBar);
			this.Name = "NuGenTViewCtrl";
			this.Size = new System.Drawing.Size(248, 176);
			this.ResumeLayout(false);

		}
		#endregion
		
		#region Public Properties

		/// <summary>
		/// Gets or sets the property that determines if the control scrolls to
		/// ensure that newly added items are visible
		/// </summary>
		[Description("If true, new items are automatically scrolled into view")]
		[Category("Behavior")]
		[DefaultValue(true)]
		public bool ScrollOnAdd
		{
			get
			{
				return _scrollOnAdd;
			}

			set
			{
				_scrollOnAdd = value;
			}
		}

		/// <summary>
		/// Gets or sets the ForeColor to use to draw selected text
		/// </summary>
		[Description("Forecolor to use to draw selected text")]
		[Category("Appearance")]
		public Color NuGenTSelectorForeColor
		{
			get
			{
				return NuGenTBase.NuGenTSelectorForeColor;
			}

			set
			{
				NuGenTBase.NuGenTSelectorForeColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the BackColor to use to draw selected text
		/// </summary>
		[Description("Backcolor to use to draw selected text")]
		[Category("Appearance")]
		public Color NuGenTSelectorBackColor
		{
			get
			{
				return NuGenTBase.NuGenTSelectorBackColor;
			}

			set
			{
				NuGenTBase.NuGenTSelectorBackColor = value;
			}
		}

		/// <summary>
		/// Gets the details of the state of text selected in the control
		/// </summary>
		[Browsable(false)]
		public NuGenTSelector NuGenTSelector
		{
			get
			{
				return NuGenTBase.NuGenTSelector;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Add a text item to be displayed
		/// </summary>
		/// <param name="value">The text to be displayed</param>
		public void Add(string value)
		{
			Add(value, false);
		}
		
		public void Add(string value, bool h)
		{
			NuGenTBase.Add(value, h);				
			AdjustScrollBar();
			
			if(ScrollOnAdd)
			{
				// Adjust position of the vertical scrollbar to 
				int firstRow = NuGenTBase.NumberItems - (NuGenTBase.CountVisibleRows - 1);
				if(firstRow < 0)
				{
					firstRow = 0;
				}

				vScrollBar.Value = firstRow;
			}
		}

		/// <summary>
		/// Copies the selected text to the clipboard
		/// </summary>
		/// <returns>True if text was copied otherwise false</returns>
		public bool CopyNuGenTSelectorToClipboard()
		{
			return NuGenTBase.CopyNuGenTSelectorToClipboard();
		}

		#endregion

		#region Protected Methods

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);
			AdjustScrollBar();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			hScrollBar.Minimum = 0;
			hScrollBar.Maximum = 1000;
			hScrollBar.SmallChange = 1;
			vScrollBar.Minimum = 0;
			vScrollBar.Maximum = 0;
			vScrollBar.SmallChange = 1;
		}

		#endregion

		#region Private Methods

		private void AdjustScrollBar()
		{
			// Adjust the properties of the scrollbars to reflect changes
			// in visible text area
			vScrollBar.LargeChange = NuGenTBase.CountVisibleRows;
			vScrollBar.Maximum = NuGenTBase.NumberItems;
			hScrollBar.LargeChange = NuGenTBase.CountVisibleColumns;
		}
		
		#endregion

		#region Private Event Handlers

		private void vScrollBar_ValueChanged(object sender, System.EventArgs e)
		{			
			NuGenTBase.TopRow = vScrollBar.Value;
		}

		private void hScrollBar_ValueChanged(object sender, System.EventArgs e)
		{
			NuGenTBase.FirstColumn = hScrollBar.Value;
		}

		private void NuGenTBase_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			int tahti = NuGenTBase.HitTest(e.Y);
			
			if(_shiftPressed)
			{
				if(NuGenTBase.NuGenTSelector.Length > 0)
				{
					int row;

					if(tahti == -1)
					{
						row = NuGenTBase.NumberItems - 1;
					}
					else
					{
						row = tahti;
					}

					int NuGenTSelectorStart;
					int NuGenTSelectorEnd;

					if(row < _mouseDownRow)
					{
						NuGenTSelectorStart = row;
						NuGenTSelectorEnd = _mouseDownRow;	
					}
					else
					{
						NuGenTSelectorStart = _mouseDownRow;
						NuGenTSelectorEnd = row;
					}
					
					NuGenTBase.NuGenTSelector.Start = NuGenTSelectorStart;
					NuGenTBase.NuGenTSelector.Length = (NuGenTSelectorEnd - NuGenTSelectorStart) + 1;
				}
			}
			else
			{
				if(tahti != -1)
				{
					NuGenTBase.NuGenTSelector.Start = tahti;
					NuGenTBase.NuGenTSelector.Length = 1;
					_mouseDownRow = tahti;
				}
			}
		}

		private void NuGenTBase_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.Shift)
			{
				_shiftPressed = true;
			}
		}

		private void NuGenTBase_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			_shiftPressed = false;
		}

		#endregion
	}
}
