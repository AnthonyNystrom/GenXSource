/* -----------------------------------------------
 * NuGenCTLabel.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Design;
using Genetibase.Shared;
using Genetibase.Shared.Drawing;
using Genetibase.Meters.ComponentModel;
using Genetibase.Meters.Design;
using Genetibase.WinApi;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Meters
{
	/// <summary>
	/// Flexible label with ClearType text rendering.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenCTLabelDesigner))]
	[ToolboxItem(false)]
	public class NuGenCTLabel : UserControl
	{
		#region Properties.Appearance

		/*
		 * BackgroundColor
		 */

		private Color _backgroundColor;

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "Control")]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_BackgroundColor")]
		public virtual Color BackgroundColor
		{
			get
			{
				if (_backgroundColor == Color.Empty)
				{
					return SystemColors.Control;
				}

				return _backgroundColor;
			}
			set
			{
				if (_backgroundColor != value)
				{
					_backgroundColor = value;
					this.OnBackgroundColorChanged(EventArgs.Empty);
					this.BackColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.BackgroundTransparency), value);
					this.Refresh();
				}
			}
		}

		private static readonly Object EventBackgroundColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of BackgroundColor property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_BackgroundColorChanged")]
		public event EventHandler BackgroundColorChanged
		{
			add
			{
				this.Events.AddHandler(EventBackgroundColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventBackgroundColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenCTLabel.BackgroundColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackgroundColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBackgroundColorChanged, e);
		}

		/*
		 * BackgroundTransparency
		 */

		private Int32 _backgroundTransparency;

		/// <summary>
		/// Gets or sets the background transparency level for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(0)]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_BackgroundTransparency")]
		public virtual Int32 BackgroundTransparency
		{
			get
			{
				return _backgroundTransparency;
			}
			set
			{
				if (_backgroundTransparency != value)
				{
					_backgroundTransparency = value;
					this.OnBackgroundTransparencyChanged(EventArgs.Empty);
					this.BackColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(value), this.BackgroundColor);
					this.Refresh();
				}
			}
		}

		private static readonly Object _backgroundTransparencyChanged = new Object();

		/// <summary>
		/// Occurs when the value of <see cref="P:Genetibase.Meters.NuGenCTLabel.BackgroundTransparency"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_BackgroundTransparencyChanged")]
		public event EventHandler BackgroundTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(_backgroundTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_backgroundTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenCTLabel.BackgroundTransparencyChanged"/>
		/// event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackgroundTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_backgroundTransparencyChanged, e);
		}

		/*
		 * BorderColor
		 */

		private Color _borderColor;

		/// <summary>
		/// Gets or sets the border color for the control.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_BorderColor")]
		public virtual Color BorderColor
		{
			get
			{
				if (_borderColor == Color.Empty)
				{
					return Color.Black;
				}

				return _borderColor;
			}
			set
			{
				if (_borderColor != value)
				{
					_borderColor = value;
					this.OnBorderColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _borderColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenCTLabel.BorderColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_BorderColorChanged")]
		public event EventHandler BorderColorChanged
		{
			add
			{
				this.Events.AddHandler(_borderColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_borderColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenCTLabel.BorderColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBorderColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBackgroundColorChanged, e);
		}

		/*
		 * BorderStyle
		 */

		private NuGenBorderStyle _borderStyle = NuGenBorderStyle.None;

		/// <summary>
		/// Gets or sets the border style for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenBorderStyle.None)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_BorderStyle")]
		public new virtual NuGenBorderStyle BorderStyle
		{
			get
			{
				return _borderStyle;
			}
			set
			{
				if (_borderStyle != value)
				{
					_borderStyle = value;
					this.OnBorderStyleChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _borderStyleChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenCTLabel.BorerStyle"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_BorderStyleChanged")]
		public event EventHandler BorderStyleChanged
		{
			add
			{
				this.Events.AddHandler(_borderStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_borderStyleChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenCTLabel.BorderStyleChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBorderStyleChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_borderStyleChanged, e);
		}

		/*
		 * ForegroundColor
		 */

		private Color _foregroundColor;

		/// <summary>
		/// Gets or sets the foreground color for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "ControlText")]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ForegroundColor")]
		public virtual Color ForegroundColor
		{
			get
			{
				if (_foregroundColor == Color.Empty)
				{
					return SystemColors.ControlText;
				}

				return _foregroundColor;
			}
			set
			{
				if (_foregroundColor != value)
				{
					_foregroundColor = value;
					this.OnForegroundColorChanged(EventArgs.Empty);
					this.ForeColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.ForegroundTransparency), value);
					this.Refresh();
				}
			}
		}

		private static readonly Object _foregroundColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenCTLabel.ForegroundColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ForegroundColorChanged")]
		public event EventHandler ForegroundColorChanged
		{
			add
			{
				this.Events.AddHandler(_foregroundColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_foregroundColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenCTLabel.ForegroundColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnForegroundColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_foregroundColorChanged, e);
		}

		/*
		 * ForegroundTransparency
		 */

		private Int32 foregroundTransparency;

		/// <summary>
		/// Gets or sets the foreground transparency level for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(0)]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ForegroundTransparency")]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual Int32 ForegroundTransparency
		{
			get
			{
				return this.foregroundTransparency;
			}
			set
			{
				if (this.foregroundTransparency != value)
				{
					this.foregroundTransparency = value;
					this.OnForegroundTransparencyChanged(EventArgs.Empty);
					this.ForeColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(value), this.ForegroundColor);
					this.Refresh();
				}
			}
		}

		private static readonly Object _foregroundTransparencyChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenCTLable.ForegroundTransparency"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ForegroundTransparencyChanged")]
		public event EventHandler ForegroundTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(_foregroundTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_foregroundTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenCTLabel.ForegroundTransparencyChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnForegroundTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_foregroundTransparencyChanged, e);
		}

		/*
		 * TextAlign
		 */

		private ContentAlignment _textAlign = ContentAlignment.MiddleCenter;

		/// <summary>
		/// Gets or sets text alignment for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_TextAlign")]
		public ContentAlignment TextAlign
		{
			get
			{
				return _textAlign;
			}
			set
			{
				if (_textAlign != value)
				{
					_textAlign = value;
					this.OnTextAlignChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly Object _textAlignChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenCTLabel.TextAlign"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TextAlignChanged")]
		public event EventHandler TextAlignChanged
		{
			add
			{
				this.Events.AddHandler(_textAlignChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_textAlignChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:NuGenCTLabel.TextAlignChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnTextAlignChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_textAlignChanged, e);
		}

		/*
		 * TextOrientation
		 */

		private NuGenOrientationStyle _textOrientation = NuGenOrientationStyle.Horizontal;

		/// <summary>
		/// Gets or sets the text orientation for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenOrientationStyle.Horizontal)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_TextOrientation")]
		public virtual NuGenOrientationStyle TextOrientation
		{
			get
			{
				return _textOrientation;
			}
			set
			{
				if (_textOrientation != value)
				{
					_textOrientation = value;
					this.OnTextOrientationChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _textOrientationChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenCTLabel.TextOrientation"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TextOrientationChanged")]
		public event EventHandler TextOrientationChanged
		{
			add
			{
				this.Events.AddHandler(_textOrientationChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_textOrientationChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenCTLabel.TextOrientationChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnTextOrientationChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_textOrientationChanged, e);
		}

		#endregion

		#region Properties.Behavior

		/*
		 * EatLine
		 */

		private Boolean _eatLine = true;

		/// <summary>
		/// Gets or sets the value indicating whether to eat the tail symbols which cannot
		/// be displayed due to the label width.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_EatLine")]
		public virtual Boolean EatLine
		{
			get
			{
				return _eatLine;
			}
			set
			{
				if (_eatLine != value)
				{
					_eatLine = value;
					this.OnEatLineChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Object _eatLineChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenCTLabel.EatLine"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_EatLineChanged")]
		public event EventHandler EatLineChanged
		{
			add
			{
				this.Events.AddHandler(_eatLineChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_eatLineChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenCTLabel.EatLineChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnEatLineChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_eatLineChanged, e);
		}

		/*
		 * WordWrap
		 */

		private Boolean _wordWrap;

		/// <summary>
		/// Gets or sets the value indicating whether lines are automatically word-wrapped.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_WordWrap")]
		public virtual Boolean WordWrap
		{
			get
			{
				return _wordWrap;
			}
			set
			{
				if (_wordWrap != value)
				{
					_wordWrap = value;
					this.OnWordWrapChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Object _wordWrapChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenCTLabel.WordWrap"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_WordWrapChanged")]
		public event EventHandler WordWrapChanged
		{
			add
			{
				this.Events.AddHandler(_wordWrapChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_wordWrapChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenCTLabel.WordWrapChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnWordWrapChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_wordWrapChanged, e);
		}

		#endregion

		#region Properties.Public.Overriden

		/*
		 * Text
		 */

		/// <summary>
		/// The text contained in the control.
		/// </summary>
		private String _text = "";

		/// <summary>
		/// Gets or sets the text for the control.
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override String Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (this._text != value)
				{
					this._text = value;
					this.OnTextChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _textChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenCTLabel.Text"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TextChanged")]
		public new event EventHandler TextChanged
		{
			add
			{
				this.Events.AddHandler(_textChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_textChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.TextChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected override void OnTextChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_textChanged, e);
		}

		#endregion

		#region Properties.Protected.Overriden

		/// <summary>
		/// Gets the default size for this <see cref="T:Genetibase.Meters.NuGenCTLabel"/>.
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(120, 24);
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Specifies the pen width to draw the border.
		/// </summary>
		private const Int32 PEN_WIDTH = 1;

		/// <summary>
		/// Draws the control.
		/// </summary>
		/// <param name="e">Provides data for the <c>System.Windows.Forms.Control.Paint</c> event.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			String bufferString = "";

			// High quality text drawing.
			g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

			// Define a tweaked rectangle for that the border was visible.
			Rectangle tweakedRectangle = new Rectangle(
				this.ClientRectangle.X,
				this.ClientRectangle.Y,
				this.ClientRectangle.Width - PEN_WIDTH,
				this.ClientRectangle.Height - PEN_WIDTH
				);

			/*
			 * Text
			 */

			using (StringFormat sf = NuGenControlPaint.CreateStringFormat(this, this.TextAlign, this.EatLine, true))
			{
				if (this.WordWrap == false)
				{
					sf.FormatFlags |= StringFormatFlags.NoWrap;
				}

				if (this.TextOrientation == NuGenOrientationStyle.Vertical)
				{
					sf.FormatFlags |= StringFormatFlags.DirectionVertical;
				}

				using (SolidBrush sb = new SolidBrush(this.ForeColor))
				{
					g.DrawString(
						this._text,
						this.Font,
						sb,
						tweakedRectangle,
						sf
					);
				}
			}

			/*
			 * Border
			 */

			NuGenControlPaint.DrawBorder(g, this.ClientRectangle, NuGenControlPaint.ColorFromArgb(this.ForegroundTransparency, this.BorderColor), this.BorderStyle);
		}

		/*
		 * WndProc
		 */

		/// <summary>
		/// Makes the control transparent for mouse events.
		/// </summary>
		/// <param name="m">Windows message.</param>
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WinUser.WM_NCHITTEST)
				m.Result = (IntPtr)WinUser.HTTRANSPARENT;
			else
				base.WndProc(ref m);
		}

		#endregion

		#region Methods.Protected.Virtual

		/// <summary>
		/// Invokes event handlers specified by the <paramref name="key"/>.
		/// <param name="key">Specifies the event handlers to invoke.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		/// </summary>
		protected virtual void InvokePropertyChanged(Object key, EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[key];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCTLabel"/> class.
		/// </summary>
		public NuGenCTLabel()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);
		}
	}
}
