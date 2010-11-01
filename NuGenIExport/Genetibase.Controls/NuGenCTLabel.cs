/* -----------------------------------------------
 * NuGenCTLabel.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NuGenControlPaint = Genetibase.Shared.Drawing.NuGenControlPaint;

using Genetibase.Controls.Design;
using Genetibase.Design;
using Genetibase.Shared;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Text;
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

namespace Genetibase.Controls
{
	/// <summary>
	/// Flexible label with ClearType text rendering.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenCTLabelDesigner))]
	[ToolboxItem(false)]
	public class NuGenCTLabel : UserControl
	{
		#region Declarations

		private System.ComponentModel.Container components = null;

		#endregion

		#region Properties.Appearance
		
		/*
		 * BackgroundColor
		 */

		/// <summary>
		/// Determines the background color for the control.
		/// </summary>
		private Color internalBackgroundColor = SystemColors.Control;

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		[Browsable(true)]
		[Category("Appearance")]
		[DefaultValue(typeof(Color), "Control")]
		[Description("Determines the background color for the control.")]
		public virtual Color BackgroundColor
		{
			get { return this.internalBackgroundColor; }
			set 
			{
				if (this.internalBackgroundColor != value) 
				{
					this.internalBackgroundColor = value;
					this.OnBackgroundColorChanged(EventArgs.Empty);
					this.BackColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.BackgroundTransparency), value);
					this.Refresh();
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenCTLabel.BackgroundColorChanged"/> event identifier.
		/// </summary>
		private static readonly object EventBackgroundColorChanged = new object();

		/// <summary>
		/// Event fired when the value of BackgroundColor property is changed on Control.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Event fired when the value of BackgroundColor property is changed on Control.")]
		public event EventHandler BackgroundColorChanged
		{
			add { this.Events.AddHandler(NuGenCTLabel.EventBackgroundColorChanged, value); }
			remove { this.Events.RemoveHandler(NuGenCTLabel.EventBackgroundColorChanged, value); }
		}

		/// <summary>
		/// Raises the <c>Genetibase.UI.NuGenCTLabel.BackgroundColorChanged</c> event.
		/// </summary>
		/// <param name="e">An <c>System.EventArgs</c> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackgroundColorChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[NuGenCTLabel.EventBackgroundColorChanged];
			
			if (handler != null)
			{
				handler(this, e);
			}
		}

		/*
		 * BackgroundTransparency
		 */

		/// <summary>
		/// Determines the background transparency level for the control.
		/// </summary>
		private int internalBackgroundTransparency = 0;

		/// <summary>
		/// Gets or sets the background transparency level for the control.
		/// </summary>
		[Browsable(true)]
		[Category("Appearance")]
		[DefaultValue(0)]
		[Description("Determines the background transparency level for the control.")]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual int BackgroundTransparency
		{
			get { return this.internalBackgroundTransparency; }
			set 
			{
				if (this.internalBackgroundTransparency != value) 
				{
					this.internalBackgroundTransparency = value;
					this.OnBackgroundTransparencyChanged(EventArgs.Empty);
					this.BackColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(value), this.BackgroundColor);
					this.Refresh();
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenCTLabel.BackgroundTransparencyChanged"/> event identifier.
		/// </summary>
		private static readonly object EventBackgroundTransparencyChanged = new object();
		
		/// <summary>
		/// Event fired when the value of BackgroundTransparency property is changed on Control.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Event fired when the value of BackgroundTransparency property is changed on Control.")]
		public event EventHandler BackgroundTransparencyChanged
		{
			add { this.Events.AddHandler(NuGenCTLabel.EventBackgroundTransparencyChanged, value); }
			remove { this.Events.RemoveHandler(NuGenCTLabel.EventBackgroundTransparencyChanged, value); }
		}

		/// <summary>
		/// Raises the <c>Genetibase.UI.NuGenCTLabel.BackgroundTransparencyChanged</c> event.
		/// </summary>
		/// <param name="e">An <c>System.EventArgs</c> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackgroundTransparencyChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[NuGenCTLabel.EventBackgroundTransparencyChanged];
			
			if (handler != null)
			{
				handler(this, e);
			}
		}

		/*
		 * BorderColor
		 */

		/// <summary>
		/// Determines the border color for the control.
		/// </summary>
		private Color internalBorderColor = Color.Black;

		/// <summary>
		/// Gets or sets the border color for the control.
		/// </summary>
		[Browsable(true)]
		[Category("Appearance")]
		[Description("Determines the border color for the control.")]
		public virtual Color BorderColor
		{
			get { return this.internalBorderColor; }
			set 
			{
				if (this.internalBorderColor != value)
				{
					this.internalBorderColor = value;
					this.OnBorderColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenCTLabel.BorderColorChanged"/> event identifier.
		/// </summary>
		private static readonly object EventBorderColorChanged = new object();

		/// <summary>
		/// Event fired when the value of BorderColor property is changed on Control.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Event fired when the value of BorderColor property is changed on Control.")]
		public event EventHandler BorderColorChanged
		{
			add { this.Events.AddHandler(NuGenCTLabel.EventBorderColorChanged, value); }
			remove { this.Events.RemoveHandler(NuGenCTLabel.EventBorderColorChanged, value); }
		}

		/// <summary>
		/// Raises the <c>Genetibase.UI.NuGenCTLabel.BorderColorChanged</c> event.
		/// </summary>
		/// <param name="e">An <c>System.EventArgs</c> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBorderColorChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[NuGenCTLabel.EventBorderColorChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/*
		 * BorderStyle
		 */

		/// <summary>
		/// Determines the border style for the control.
		/// </summary>
		private NuGenBorderStyle internalBorderStyle = NuGenBorderStyle.None;

		/// <summary>
		/// Gets or sets the border style for the control.
		/// </summary>
		[Browsable(true)]
		[Category("Appearance")]
		[DefaultValue(NuGenBorderStyle.None)]
		[Description("Determines the border style for the control.")]
		public new virtual NuGenBorderStyle BorderStyle
		{
			get { return this.internalBorderStyle; }
			set 
			{
				if (this.internalBorderStyle != value)
				{
					this.internalBorderStyle = value;
					this.OnBorderStyleChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenCTLabel.BorderStyleChanged"/> event identifier.
		/// </summary>
		private static readonly object EventBorderStyleChanged = new object();

		/// <summary>
		/// Event fired when the value of BorderStyle property is changed on Control.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Event fired when the value of BorderStyle property is changed on Control.")]
		public event EventHandler BorderStyleChanged
		{
			add { this.Events.AddHandler(NuGenCTLabel.EventBorderStyleChanged, value); }
			remove { this.Events.RemoveHandler(NuGenCTLabel.EventBorderStyleChanged, value); }
		}

		/// <summary>
		/// Raises the <c>Genetibase.UI.NuGenCTLabel.BorderStyleChanged</c> event.
		/// </summary>
		/// <param name="e">An <c>System.EventArgs</c> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBorderStyleChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[NuGenCTLabel.EventBorderStyleChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/*
		 * ForegroundColor
		 */

		/// <summary>
		/// Determines the foreground color for the control.
		/// </summary>
		private Color internalForegroundColor = SystemColors.ControlText;

		/// <summary>
		/// Gets or sets the foreground color for the control.
		/// </summary>
		[Browsable(true)]
		[Category("Appearance")]
		[DefaultValue(typeof(Color), "ControlText")]
		[Description("Determines the foreground color for the control.")]
		public virtual Color ForegroundColor
		{
			get { return this.internalForegroundColor; }
			set 
			{
				if (this.internalForegroundColor != value) 
				{
					this.internalForegroundColor = value;
					this.OnForegroundColorChanged(EventArgs.Empty);
					this.ForeColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.ForegroundTransparency), value);
					this.Refresh();
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenCTLabel.ForegroundColorChanged"/> event identifier.
		/// </summary>
		private static readonly object EventForegroundColorChanged = new object();

		/// <summary>
		/// Event fired when the value of ForegroundColor property is changed on Control.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Event fired when the value of ForegroundColor property is changed on Control.")]
		public event EventHandler ForegroundColorChanged
		{
			add { this.Events.AddHandler(NuGenCTLabel.EventForegroundColorChanged, value); }
			remove { this.Events.RemoveHandler(NuGenCTLabel.EventForegroundColorChanged, value); }
		}

		/// <summary>
		/// Raises the <c>Genetibase.UI.NuGenCTLabel.ForegroundColorChanged</c> event.
		/// </summary>
		/// <param name="e">An <c>System.EventArgs</c> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnForegroundColorChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[NuGenCTLabel.EventForegroundColorChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/*
		 * ForegroundTransparency
		 */

		/// <summary>
		/// Determines the foreground transparency level for the control.
		/// </summary>
		private int internalForegroundTransparency = 0;

		/// <summary>
		/// Gets or sets the foreground transparency level for the control.
		/// </summary>
		[Browsable(true)]
		[Category("Appearance")]
		[DefaultValue(0)]
		[Description("Determines the foreground transparency level for the control.")]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual int ForegroundTransparency
		{
			get { return this.internalForegroundTransparency; }
			set 
			{
				if (this.internalForegroundTransparency != value) 
				{
					this.internalForegroundTransparency = value;
					this.OnForegroundTransparencyChanged(EventArgs.Empty);
					this.ForeColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(value), this.ForegroundColor);
					this.Refresh();
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenCTLabel.ForegroundTransparencyChanged"/> event identifier.
		/// </summary>
		private static readonly object EventForegroundTransparencyChanged = new object();

		/// <summary>
		/// Event fired when the value of ForegroundTransparency property is changed on Control.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Event fired when the value of ForegroundTransparency property is changed on Control.")]
		public event EventHandler ForegroundTransparencyChanged
		{
			add { this.Events.AddHandler(NuGenCTLabel.EventForegroundTransparencyChanged, value); }
			remove { this.Events.RemoveHandler(NuGenCTLabel.EventForegroundTransparencyChanged, value); }
		}

		/// <summary>
		/// Raises the <c>Genetibase.UI.NuGenCTLabel.ForegroudnTransparencyChanged</c> event.
		/// </summary>
		/// <param name="e">An <c>System.EventArgs</c> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnForegroundTransparencyChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[NuGenCTLabel.EventForegroundTransparencyChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/*
		 * TextAlignment
		 */

		/// <summary>
		/// Determines the text alignment for the control.
		/// </summary>
		private StringAlignment internalTextAlignment = StringAlignment.Center;

		/// <summary>
		/// Gets or sets the text alignment for the control.
		/// </summary>
		[Browsable(true)]
		[Category("Appearance")]
		[DefaultValue(StringAlignment.Center)]
		[Description("Determines the text alignment for the control.")]
		public virtual StringAlignment TextAlignment
		{
			get { return this.internalTextAlignment; }
			set 
			{
				if (this.internalTextAlignment != value)
				{
					this.internalTextAlignment = value;
					this.OnTextAlignmentChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenCTLabel.TextAlignmentChanged"/> event identifier.
		/// </summary>
		private static readonly object EventTextAlignmentChanged = new object();

		/// <summary>
		/// Event fired when the value of TextAlignment property is changed on Control.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Event fired when the value of TextAlignment property is changed on Control.")]
		public event EventHandler TextAlignmentChanged
		{
			add { this.Events.AddHandler(NuGenCTLabel.EventTextAlignmentChanged, value); }
			remove { this.Events.RemoveHandler(NuGenCTLabel.EventTextAlignmentChanged, value); }
		}

		/// <summary>
		/// Raises the <c>Genetibase.UI.NuGenCTLabel.TextAlignmentChanged</c> event.
		/// </summary>
		/// <param name="e">An <c>System.EventArgs</c> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnTextAlignmentChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[NuGenCTLabel.EventTextAlignmentChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/*
		 * TextLineAlignment
		 */

		/// <summary>
		/// Determines the text line alignment for the control.
		/// </summary>
		private StringAlignment internalTextLineAlignment = StringAlignment.Center;

		/// <summary>
		/// Gets or sets the text line alignment for the control.
		/// </summary>
		[Browsable(true)]
		[Category("Appearance")]
		[DefaultValue(StringAlignment.Center)]
		[Description("Determines the text line alignment for the control.")]
		public virtual StringAlignment TextLineAlignment
		{
			get { return this.internalTextLineAlignment; }
			set 
			{
				if (this.internalTextLineAlignment != value)
				{
					this.internalTextLineAlignment = value;
					this.OnTextLineAlignmentChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenCTLabel.TextLineAlignmentChanged"/> event identifier.
		/// </summary>
		private static readonly object EventTextLineAlignmentChanged = new object();

		/// <summary>
		/// Event fired when the value of TextLineAlignment property is changed on Control.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Event fired when the value of TextLineAlignment property is changed on Control.")]
		public event EventHandler TextLineAlignmentChanged
		{
			add { this.Events.AddHandler(NuGenCTLabel.EventTextLineAlignmentChanged, value); }
			remove { this.Events.RemoveHandler(NuGenCTLabel.EventTextLineAlignmentChanged, value); }
		}

		/// <summary>
		/// Raises the <c>Genetibase.UI.NuGenCTLabel.TextLineAlignmentChanged</c> event.
		/// </summary>
		/// <param name="e">An <c>System.EventArgs</c> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnTextLineAlignmentChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[NuGenCTLabel.EventTextLineAlignmentChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/*
		 * TextOrientation
		 */

		/// <summary>
		/// Determines the text orientation for the control.
		/// </summary>
		private NuGenOrientationStyle internalTextOrientation = NuGenOrientationStyle.Horizontal;

		/// <summary>
		/// Gets or sets the text orientation for the control.
		/// </summary>
		[Browsable(true)]
		[Category("Appearance")]
		[DefaultValue(NuGenOrientationStyle.Horizontal)]
		[Description("Determines the text orientation for the control.")]
		public virtual NuGenOrientationStyle TextOrientation
		{
			get { return this.internalTextOrientation; }
			set 
			{
				if (this.internalTextOrientation != value)
				{
					this.internalTextOrientation = value;
					this.OnTextOrientationChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenCTLabel.TextOrientationChanged"/> event identifier.
		/// </summary>
		private static readonly object EventTextOrientationChanged = new object();

		/// <summary>
		/// Event fired when the value of TextOrientation property is changed on Control.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Event fired when the value of TextOrientation property is changed on Control.")]
		public event EventHandler TextOrientationChanged
		{
			add { this.Events.AddHandler(NuGenCTLabel.EventTextOrientationChanged, value); }
			remove { this.Events.RemoveHandler(NuGenCTLabel.EventTextOrientationChanged, value); }
		}

		/// <summary>
		/// Raises the <c>Genetibase.UI.NuGenCTLabel.TextOrientationChanged</c> event.
		/// </summary>
		/// <param name="e">An <c>System.EventArgs</c> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnTextOrientationChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[NuGenCTLabel.EventTextOrientationChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/*
		 * UseClearTypeTextRendering
		 */

		/// <summary>
		/// Indicates whether to use ClearType text rendering.
		/// </summary>
		private bool useClearTypeTextRendering = true;

		/// <summary>
		/// Gets or sets the value indicating whether to use ClearType text rendering.
		/// </summary>
		[Browsable(true)]
		[Category("Appearance")]
		[Description("Indicates whether to use ClearType text rendering.")]
		public bool UseClearTypeTextRendering
		{
			get { return this.useClearTypeTextRendering; }
			set
			{
				if (this.useClearTypeTextRendering != value)
				{
					this.useClearTypeTextRendering = value;
					this.OnUseClearTypeTextRenderingChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenCTLabel.UseClearTypeTextRenderingChanged"/> event identifier.
		/// </summary>
		private static readonly object EventUseClearTypeTextRenderingChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:NuGenCTLabel.UseClearTypeTextRendering"/> property changes.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Occurs when the value of the UseClearTypeTextRendering property changes.")]
		public event EventHandler UseClearTypeTextRenderingChanged
		{
			add { this.Events.AddHandler(NuGenCTLabel.EventUseClearTypeTextRenderingChanged, value); }
			remove { this.Events.RemoveHandler(NuGenCTLabel.EventUseClearTypeTextRenderingChanged, value); }
		}

		/// <summary>
		/// Raises the <see cref="E:NuGenCTLabel.UseClearTypeTextRenderingChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected virtual void OnUseClearTypeTextRenderingChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[NuGenCTLabel.EventUseClearTypeTextRenderingChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#region Properties.Behavior

		/*
		 * AutoSize
		 */

		/// <summary>
		/// Indicates whether this <see cref="T:NuGenCTLabel"/> automatically sets its bounds according
		/// to the text being displayed.
		/// </summary>
		private bool autoSize = false;

		/// <summary>
		/// Gets or sets the value indicating whether this <see cref="T:NuGenCTLabel"/> automatically
		/// sets its bounds according to the text being displayed.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[DefaultValue(false)]
		[Description("Indicates whether this label automatically sets its bounds according to the text being displayed.")]
		public override bool AutoSize
		{
			get { return this.autoSize; }
			set
			{
				if (this.autoSize != value)
				{
					this.autoSize = value;
					this.OnAutoSizeChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenCTLabel.AutoSizeChanged"/> event identifier.
		/// </summary>
		private static readonly object EventAutoSizeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:NuGenCTLabel.AutoSize"/> property changes.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Occurs when the value of the AutoSize property changes.")]
		public new event EventHandler AutoSizeChanged
		{
			add { this.Events.AddHandler(NuGenCTLabel.EventAutoSizeChanged, value); }
			remove { this.Events.RemoveHandler(NuGenCTLabel.EventAutoSizeChanged, value); }
		}

		/// <summary>
		/// Raises the <see cref="E:NuGenCTLabel.AutoSizeChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected new virtual void OnAutoSizeChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[NuGenCTLabel.EventAutoSizeChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/*
		 * EatLine
		 */

		/// <summary>
		/// If <c>true</c> replaces the tail symbols which cannot be displayed due to the label width with "...".
		/// Takes affect only if WordWrap property is <c>false</c>.
		/// </summary>
		private bool internalEatLine = true;

		/// <summary>
		/// Gets or sets the value indicating whether to eat the tail symbols which cannot
		/// be displayed due to the label width.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[DefaultValue(true)]
		[Description("If true replaces the tail symbols which cannot be dispalyed with \"...\". Takes affect only if WordWrap property is false.")]
		public virtual bool EatLine
		{
			get { return this.internalEatLine; }
			set 
			{
				if (this.internalEatLine != value) 
				{
					this.internalEatLine = value;
					this.OnEatLineChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenCTLabel.EatLineChanged"/> event identifier.
		/// </summary>
		private static readonly object EventEatLineChanged = new object();

		/// <summary>
		/// Event fired when the value of EatLine property is changed on Control.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Event fired when the value of EatLine property is changed on Control.")]
		public event EventHandler EatLineChanged
		{
			add { this.Events.AddHandler(NuGenCTLabel.EventEatLineChanged, value); }
			remove { this.Events.RemoveHandler(NuGenCTLabel.EventEatLineChanged, value); }
		}

		/// <summary>
		/// Raises the <c>Genetibase.UI.NuGenCTLabel.EatLineChanged</c> event.
		/// </summary>
		/// <param name="e">An <c>System.EventArgs</c> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnEatLineChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[NuGenCTLabel.EventEatLineChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/*
		 * WordWrap
		 */

		/// <summary>
		/// Indicates whether lines are automatically word-wrapped.
		/// </summary>
		private bool internalWordWrap = false;

		/// <summary>
		/// Gets or sets the value indicating whether lines are automatically word-wrapped.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[DefaultValue(false)]
		[Description("Indicates whether lines are automatically word-wrapped.")]
		public virtual bool WordWrap
		{
			get { return this.internalWordWrap; }
			set 
			{
				if (this.internalWordWrap != value)
				{
					this.internalWordWrap = value;
					this.OnWordWrapChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenCTLabel.WordWrapChanged"/> event identifier.
		/// </summary>
		private static readonly object EventWordWrapChanged = new object();

		/// <summary>
		/// Event fired when the value of WordWrap property is changed on Control.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Event fired when the value of WordWrap property is changed on Control.")]
		public event EventHandler WordWrapChanged
		{
			add { this.Events.AddHandler(NuGenCTLabel.EventWordWrapChanged, value); }
			remove { this.Events.RemoveHandler(NuGenCTLabel.EventWordWrapChanged, value); }
		}

		/// <summary>
		/// Raises the <c>Genetibase.UI.NuGenCTLabel.WordWrapChanged</c> event.
		/// </summary>
		/// <param name="e">An <c>System.EventArgs</c> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnWordWrapChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[NuGenCTLabel.EventWordWrapChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#region Properties.Public.Overriden

		/*
		 * Text
		 */

		/// <summary>
		/// The text contained in the control.
		/// </summary>
		private string internalText = "";

		/// <summary>
		/// Gets or sets the text for the control.
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get { return this.internalText; }
			set 
			{
				if (this.internalText != value)
				{
					this.internalText = value;
					this.OnTextChanged(EventArgs.Empty);

					if (this.AutoSize)
					{
						Graphics g = Graphics.FromHwnd(this.Handle);
						Size textSize = g.MeasureString(value, this.Font).ToSize();

						if (this.TextOrientation == NuGenOrientationStyle.Horizontal)
						{
							this.Size = textSize;
						}
						else
						{
							this.Size = new Size(textSize.Height, textSize.Width);
						}
					}

					this.Refresh();
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenCTLabel.TextChanged"/> event identifier.
		/// </summary>
		private static readonly object EventTextChanged = new object();

		/// <summary>
		/// Event fired when the value of Text property is changed on Control.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Event fired when the value of Text property is changed on Control.")]
		public new event EventHandler TextChanged;

		/// <summary>
		/// Raises the <c>Genetibase.UI.NuGenCTLabel.TextChanged</c> event.
		/// </summary>
		/// <param name="e">An <c>System.EventArgs</c> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected override void OnTextChanged(EventArgs e)
		{
			if (this.TextChanged != null)
			{
				this.TextChanged(this, e);
			}
		}

		#endregion

		#region Properties.Protected.Virtual

		/*
		 * StringProcessor
		 */

		private INuGenStringProcessor stringProcessor = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenStringProcessor StringProcessor
		{
			get
			{
				if (this.stringProcessor == null)
				{
					this.stringProcessor = new NuGenStringProcessor();
				}

				return this.stringProcessor;
			}
		}

		#endregion

		#region Methods.Protected.Overriden

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
			
			if (this.UseClearTypeTextRendering) 
			{
				// High quality text drawing.
				g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			}

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

			StringFormat sf = new StringFormat();

			sf.Alignment = this.TextAlignment;
			sf.LineAlignment = this.TextLineAlignment;

			if (this.WordWrap == false) 
			{
				sf.FormatFlags = StringFormatFlags.NoWrap;
			}
			
			if (this.TextOrientation == NuGenOrientationStyle.Vertical)
			{
				sf.FormatFlags = StringFormatFlags.DirectionVertical;
			}

			if (this.AutoSize == false && this.WordWrap == false && this.EatLine == true) 
			{
				Debug.Assert(this.StringProcessor != null, "this.StringProcessor != null");
				
				bufferString = this.StringProcessor.EatLine(
					this.Text,
					this.Font,
					(this.TextOrientation == NuGenOrientationStyle.Vertical)
						? tweakedRectangle.Height
						: tweakedRectangle.Width,
					g
				);
			}
			else
			{
				bufferString = this.Text;
			}

			using (SolidBrush sb = new SolidBrush(this.ForeColor)) 
			{
				g.DrawString(bufferString, this.Font, sb, tweakedRectangle, sf);
			}
			
			/*
			 * Border
			 */
			
			NuGenControlPaint.DrawBorder(g, this.ClientRectangle, NuGenControlPaint.ColorFromArgb(this.ForegroundTransparency, this.BorderColor), this.BorderStyle);
		}

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

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCTLabel"/> class.
		/// </summary>
		public NuGenCTLabel()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			//
			// NuGenCTLabel
			//
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);
		}

		#endregion

		#region Dispose
		
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#endregion

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		[DebuggerStepThrough()]
		private void InitializeComponent()
		{
			// 
			// NuGenCTLabel
			// 
			this.Name = "NuGenCTLabel";
			this.Size = new System.Drawing.Size(120, 24);

		}
		#endregion
	}
}
