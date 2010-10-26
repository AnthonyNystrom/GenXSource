/* -----------------------------------------------
 * NuGenCTLabel.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using gdi = Genetibase.WinApi.Gdi32;
using win = Genetibase.WinApi.WinUser;

using Genetibase.Shared.Design;
using Genetibase.Shared;
using Genetibase.Shared.Drawing;
using Genetibase.UI.NuGenMeters.ComponentModel;
using Genetibase.UI.NuGenMeters.Design;

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

namespace Genetibase.UI.NuGenMeters
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

		/// <summary>
		/// Determines the background color for the control.
		/// </summary>
		private Color backgroundColor = SystemColors.Control;

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "Control")]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("BackgroundColorDescription")]
		public virtual Color BackgroundColor
		{
			get
			{
				return this.backgroundColor;
			}
			set
			{
				if (this.backgroundColor != value)
				{
					this.backgroundColor = value;
					this.OnBackgroundColorChanged(EventArgs.Empty);
					this.BackColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.BackgroundTransparency), value);
					this.Refresh();
				}
			}
		}

		private static readonly object EventBackgroundColorChanged = new object();

		/// <summary>
		/// Occurs when the value of BackgroundColor property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("BackgroundColorChangedDescription")]
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenCTLabel.BackgroundColorChanged"/> event.
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

		/// <summary>
		/// Determines the background transparency level for the control.
		/// </summary>
		private int backgroundTransparency = 0;

		/// <summary>
		/// Gets or sets the background transparency level for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(0)]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("BackgroundTransparencyDescription")]
		public virtual int BackgroundTransparency
		{
			get
			{
				return this.backgroundTransparency;
			}
			set
			{
				if (this.backgroundTransparency != value)
				{
					this.backgroundTransparency = value;
					this.OnBackgroundTransparencyChanged(EventArgs.Empty);
					this.BackColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(value), this.BackgroundColor);
					this.Refresh();
				}
			}
		}

		private static readonly object EventBackgroundTransparencyChanged = new object();

		/// <summary>
		/// Occurs when the value of <see cref="P:Genetibase.UI.NuGenMeters.NuGenCTLabel.BackgroundTransparency"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("BackgroundTransparencyChangedDescription")]
		public event EventHandler BackgroundTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(EventBackgroundTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventBackgroundTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenCTLabel.BackgroundTransparencyChanged"/>
		/// event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackgroundTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBackgroundTransparencyChanged, e);
		}

		/*
		 * BorderColor
		 */

		/// <summary>
		/// Determines the border color for the control.
		/// </summary>
		private Color borderColor = Color.Black;

		/// <summary>
		/// Gets or sets the border color for the control.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("BorderColorDescription")]
		public virtual Color BorderColor
		{
			get
			{
				return this.borderColor;
			}
			set
			{
				if (this.borderColor != value)
				{
					this.borderColor = value;
					this.OnBorderColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventBorderColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenCTLabel.BorderColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("BorderColorChangedDescription")]
		public event EventHandler BorderColorChanged
		{
			add
			{
				this.Events.AddHandler(EventBorderColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventBorderColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenCTLabel.BorderColorChanged"/> event.
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

		/// <summary>
		/// Determines the border style for the control.
		/// </summary>
		private NuGenBorderStyle borderStyle = NuGenBorderStyle.None;

		/// <summary>
		/// Gets or sets the border style for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenBorderStyle.None)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("BorderStyleDescription")]
		public new virtual NuGenBorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (this.borderStyle != value)
				{
					this.borderStyle = value;
					this.OnBorderStyleChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventBorderStyleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenCTLabel.BorerStyle"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("BorderStyleChangedDescription")]
		public event EventHandler BorderStyleChanged
		{
			add
			{
				this.Events.AddHandler(EventBorderStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventBorderStyleChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenCTLabel.BorderStyleChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBorderStyleChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBorderStyleChanged, e);
		}

		/*
		 * ForegroundColor
		 */

		/// <summary>
		/// Determines the foreground color for the control.
		/// </summary>
		private Color foregroundColor = SystemColors.ControlText;

		/// <summary>
		/// Gets or sets the foreground color for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "ControlText")]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("ForegroundColorDescription")]
		public virtual Color ForegroundColor
		{
			get
			{
				return this.foregroundColor;
			}
			set
			{
				if (this.foregroundColor != value)
				{
					this.foregroundColor = value;
					this.OnForegroundColorChanged(EventArgs.Empty);
					this.ForeColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.ForegroundTransparency), value);
					this.Refresh();
				}
			}
		}

		private static readonly object EventForegroundColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenCTLabel.ForegroundColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ForegroundColorChangedDescription")]
		public event EventHandler ForegroundColorChanged
		{
			add
			{
				this.Events.AddHandler(EventForegroundColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventForegroundColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenCTLabel.ForegroundColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnForegroundColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventForegroundColorChanged, e);
		}

		/*
		 * ForegroundTransparency
		 */

		/// <summary>
		/// Determines the foreground transparency level for the control.
		/// </summary>
		private int foregroundTransparency = 0;

		/// <summary>
		/// Gets or sets the foreground transparency level for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(0)]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("ForegroundTransparencyDescription")]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual int ForegroundTransparency
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

		private static readonly object EventForegroundTransparencyChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenCTLable.ForegroundTransparency"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ForegroundTransparencyChangedDescription")]
		public event EventHandler ForegroundTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(EventForegroundTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventForegroundTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenCTLabel.ForegroundTransparencyChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnForegroundTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventForegroundTransparencyChanged, e);
		}

		/*
		 * TextAlign
		 */

		private ContentAlignment textAlign = ContentAlignment.MiddleCenter;

		/// <summary>
		/// Gets or sets text alignment for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("TextAlignDescription")]
		public ContentAlignment TextAlign
		{
			get
			{
				return this.textAlign;
			}
			set
			{
				if (this.textAlign != value)
				{
					this.textAlign = value;
					this.OnTextAlignChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object EventTextAlignChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenCTLabel.TextAlign"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("TextAlignChangedDescription")]
		public event EventHandler TextAlignChanged
		{
			add
			{
				this.Events.AddHandler(EventTextAlignChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventTextAlignChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:NuGenCTLabel.TextAlignChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnTextAlignChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventTextAlignChanged, e);
		}

		/*
		 * TextOrientation
		 */

		/// <summary>
		/// Determines the text orientation for the control.
		/// </summary>
		private NuGenOrientationStyle textOrientation = NuGenOrientationStyle.Horizontal;

		/// <summary>
		/// Gets or sets the text orientation for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenOrientationStyle.Horizontal)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("TextOrientationDescription")]
		public virtual NuGenOrientationStyle TextOrientation
		{
			get
			{
				return this.textOrientation;
			}
			set
			{
				if (this.textOrientation != value)
				{
					this.textOrientation = value;
					this.OnTextOrientationChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventTextOrientationChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenCTLabel.TextOrientation"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("TextOrientationChangedDescription")]
		public event EventHandler TextOrientationChanged
		{
			add
			{
				this.Events.AddHandler(EventTextOrientationChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventTextOrientationChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenCTLabel.TextOrientationChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnTextOrientationChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventTextOrientationChanged, e);
		}

		#endregion

		#region Properties.Behavior

		/*
		 * EatLine
		 */

		private bool eatLine = true;

		/// <summary>
		/// Gets or sets the value indicating whether to eat the tail symbols which cannot
		/// be displayed due to the label width.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("BehaviorCategory")]
		[NuGenSRDescription("EatLineDescription")]
		public virtual bool EatLine
		{
			get
			{
				return this.eatLine;
			}
			set
			{
				if (this.eatLine != value)
				{
					this.eatLine = value;
					this.OnEatLineChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object EventEatLineChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenCTLabel.EatLine"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("EatLineChangedDescription")]
		public event EventHandler EatLineChanged
		{
			add
			{
				this.Events.AddHandler(EventEatLineChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventEatLineChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenCTLabel.EatLineChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnEatLineChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventEatLineChanged, e);
		}

		/*
		 * WordWrap
		 */

		/// <summary>
		/// Indicates whether lines are automatically word-wrapped.
		/// </summary>
		private bool wordWrap = false;

		/// <summary>
		/// Gets or sets the value indicating whether lines are automatically word-wrapped.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("BehaviorCategory")]
		[NuGenSRDescription("WordWrapDescription")]
		public virtual bool WordWrap
		{
			get
			{
				return this.wordWrap;
			}
			set
			{
				if (this.wordWrap != value)
				{
					this.wordWrap = value;
					this.OnWordWrapChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object EventWordWrapChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenCTLabel.WordWrap"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("WordWrapChangedDescription")]
		public event EventHandler WordWrapChanged
		{
			add
			{
				this.Events.AddHandler(EventWordWrapChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventWordWrapChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenCTLabel.WordWrapChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnWordWrapChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventWordWrapChanged, e);
		}

		#endregion

		#region Properties.Public.Overriden

		/*
		 * Text
		 */

		/// <summary>
		/// The text contained in the control.
		/// </summary>
		private string text = "";

		/// <summary>
		/// Gets or sets the text for the control.
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				if (this.text != value)
				{
					this.text = value;
					this.OnTextChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventTextChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenCTLabel.Text"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("TextChangedDescription")]
		public new event EventHandler TextChanged
		{
			add
			{
				this.Events.AddHandler(EventTextChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventTextChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.TextChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected override void OnTextChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventTextChanged, e);
		}

		#endregion

		#region Properties.Protected.Overriden

		/// <summary>
		/// Gets the default size for this <see cref="T:Genetibase.UI.NuGenMeters.NuGenCTLabel"/>.
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
		private const int PEN_WIDTH = 1;

		/// <summary>
		/// Draws the control.
		/// </summary>
		/// <param name="e">Provides data for the <c>System.Windows.Forms.Control.Paint</c> event.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			string bufferString = "";

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
						this.text,
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
			if (m.Msg == win.WM_NCHITTEST)
				m.Result = (IntPtr)win.HTTRANSPARENT;
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
		protected virtual void InvokePropertyChanged(object key, EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[key];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <c>Genetibase.UI.NuGenCTLabel</c> class.
		/// </summary>
		public NuGenCTLabel()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);
		}

		#endregion
	}
}
