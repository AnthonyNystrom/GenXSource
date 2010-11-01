/* -----------------------------------------------
 * NuGenTextBox.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.Design;
using Genetibase.Shared.Controls.TextBoxInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// A text box which supports prompt text and input mask verification.
	/// </summary>
	[DefaultEvent("TextChanged")]
	[DefaultProperty("Text")]
	[Designer(typeof(NuGenTextBoxDesigner))]
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenTextBox : UserControl
	{
		#region Properties.Appearance

		/*
		 * PromptFont
		 */

		/// <summary>
		/// Gets or sets the font to use when displaying the <see cref="PromptText"/>.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_PromptTextBox_PromptFont")]
		public Font PromptFont
		{
			get
			{
				Debug.Assert(this.TextBoxInternal != null, "this.TextBoxInternal != null");
				return this.TextBoxInternal.PromptFont;
			}
			set
			{
				Debug.Assert(this.TextBoxInternal != null, "this.TextBoxInternal != null");
				this.TextBoxInternal.PromptFont = value;
			}
		}

		private static readonly object _promptFontChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="PromptFont"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PromptTextBox_PromptFontChanged")]
		public event EventHandler PromptFontChanged
		{
			add
			{
				this.Events.AddHandler(_promptFontChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_promptFontChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="PromptFontChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPromptFontChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_promptFontChanged, e);
		}

		/*
		 * PromptForeColor
		 */

		/// <summary>
		/// Gets or sets the fore color to use when displaying the <see cref="PromptText"/>.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "GrayText")]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_PromptTextBox_PromptForeColor")]
		public Color PromptForeColor
		{
			get
			{
				Debug.Assert(this.TextBoxInternal != null, "this.TextBoxInternal != null");
				return this.TextBoxInternal.PromptForeColor;
			}
			set
			{
				Debug.Assert(this.TextBoxInternal != null, "this.TextBoxInternal != null");
				this.TextBoxInternal.PromptForeColor = value;
			}
		}

		private static readonly object _promptForeColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="PromptForeColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PromptTextBox_PromptForeColorChanged")]
		public event EventHandler PromptForeColorChanged
		{
			add
			{
				this.Events.AddHandler(_promptForeColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_promptForeColorChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="PromptForeColorChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPromptForeColorChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_promptForeColorChanged, e);
		}

		/*
		 * PromptText
		 */

		/// <summary>
		/// Gets or sets the prompt text to display if <see cref="Text"/> = "".
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_PromptTextBox_PromptText")]
		public string PromptText
		{
			get
			{
				Debug.Assert(this.TextBoxInternal != null, "this.TextBoxInternal != null");
				return this.TextBoxInternal.PromptText;
			}
			set
			{
				Debug.Assert(this.TextBoxInternal != null, "this.TextBoxInternal != null");
				this.TextBoxInternal.PromptText = value;
			}
		}

		private static readonly object _promptTextChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="PromptText"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PromptTextBox_PromptTextChanged")]
		public event EventHandler PromptTextChanged
		{
			add
			{
				this.Events.AddHandler(_promptTextChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_promptTextChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="PromptTextChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPromptTextChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_promptTextChanged, e);
		}

		#endregion

		#region Properties.Behavior

		/*
		 * FocusSelect
		 */

		/// <summary>
		/// Gets or sets the value indicating whether to select the text when this text box receives the focus.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_PromptTextBox_FocusSelect")]
		public bool FocusSelect
		{
			get
			{
				Debug.Assert(this.TextBoxInternal != null, "this.TextBoxInternal != null");
				return this.TextBoxInternal.FocusSelect;
			}
			set
			{
				Debug.Assert(this.TextBoxInternal != null, "this.TextBoxInternal != null");
				this.TextBoxInternal.FocusSelect = value;
			}
		}

		private static readonly object _focusSelectChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="FocusSelect"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PromptTextBox_FocusSelectChanged")]
		public event EventHandler FocusSelectChanged
		{
			add
			{
				this.Events.AddHandler(_focusSelectChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_focusSelectChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="FocusSelectChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnFocusSelectChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_focusSelectChanged, e);
		}

		#endregion

		#region Properties.Regex

		/*
		 * InvalidTextBackColor
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Regex")]
		[NuGenSRDescription("Description_PromptTextBox_InvalidTextBackColor")]
		public Color InvalidTextBackColor
		{
			get
			{
				return this.TextBoxInternal.InvalidTextBackColor;
			}
			set
			{
				this.TextBoxInternal.InvalidTextBackColor = value;
			}
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		protected bool ShouldSerializeInivalidTextBackColor()
		{
			return this.TextBoxInternal.ShouldSerializeInvalidTextBackColor();
		}

		/// <summary>
		/// </summary>
		protected void ResetInvalidTextBackColor()
		{
			this.TextBoxInternal.ResetInvalidTextBackColor();
		}

		private static readonly object _invalidTextBackColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="InvalidTextBackColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PromptTextBox_InvalidTextBackColorChanged")]
		public event EventHandler InvalidTextBackColorChanged
		{
			add
			{
				this.Events.AddHandler(_invalidTextBackColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_invalidTextBackColorChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="InvalidTextBackColorChanged"/> event.
		/// </summary>
		protected virtual void OnInvalidTextBackColorChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_invalidTextBackColorChanged, e);
		}

		/*
		 * PatternMode
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenTextBoxPatternMode.None)]
		[NuGenSRCategory("Category_Regex")]
		[NuGenSRDescription("Description_PromptTextBox_PatternMode")]
		public NuGenTextBoxPatternMode PatternMode
		{
			get
			{
				return this.TextBoxInternal.PatternMode;
			}
			set
			{
				this.TextBoxInternal.PatternMode = value;
			}
		}

		private static readonly object _patternModeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="PatternMode"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PromptTextBox_PatternModeChanged")]
		public event EventHandler PatternModeChanged
		{
			add
			{
				this.Events.AddHandler(_patternModeChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_patternModeChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="PatternModeChanged"/> event.
		/// </summary>
		protected virtual void OnPatternModeChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_patternModeChanged, e);
		}

		/*
		 * Pattern
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Regex")]
		[NuGenSRDescription("Description_PromptTextBox_Pattern")]
		public string Pattern
		{
			get
			{
				return this.TextBoxInternal.Pattern;
			}
			set
			{
				this.TextBoxInternal.Pattern = value;
			}
		}

		private static readonly object _patternChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Pattern"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PromptTextBox_PatternChanged")]
		public event EventHandler PatternChanged
		{
			add
			{
				this.Events.AddHandler(_patternChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_patternChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="PatternChanged"/> event.
		/// </summary>
		protected virtual void OnPatternChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_patternChanged, e);
		}

		/*
		 * UseColors
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Regex")]
		[NuGenSRDescription("Description_PromptTextBox_UseColors")]
		public bool UseColors
		{
			get
			{
				return this.TextBoxInternal.UseColors;
			}
			set
			{
				this.TextBoxInternal.UseColors = value;
			}
		}

		private static readonly object _useColorsChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="UseColors"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PromptTextBox_UseColorsChanged")]
		public event EventHandler UseColorsChanged
		{
			add
			{
				this.Events.AddHandler(_useColorsChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_useColorsChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="UseColorsChanged"/> event.
		/// </summary>
		protected virtual void OnUseColorsChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_useColorsChanged, e);
		}

		/*
		 * ValidTextBackColor
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Regex")]
		[NuGenSRDescription("Description_PromptTextBox_ValidTextBackColor")]
		public Color ValidTextBackColor
		{
			get
			{
				return this.TextBoxInternal.ValidTextBackColor;
			}
			set
			{
				this.TextBoxInternal.ValidTextBackColor = value;
			}
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		protected bool ShouldSerializeValidTextBackColor()
		{
			return this.TextBoxInternal.ShouldSerializeValidTextBackColor();
		}

		/// <summary>
		/// </summary>
		protected void ResetValidTextBackColor()
		{
			this.TextBoxInternal.ResetValidTextBackColor();
		}

		private static readonly object _validTextBackColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ValidTextBackColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PromptTextBox_ValidTextBackColorChanged")]
		public event EventHandler ValidTextBackColorChanged
		{
			add
			{
				this.Events.AddHandler(_validTextBackColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_validTextBackColorChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ValidTextBackColorChanged"/> event.
		/// </summary>
		protected virtual void OnValidTextBackColorChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_validTextBackColorChanged, e);
		}

		#endregion

		#region Properties.Hidden

		/*
		 * Padding
		 */

		private static readonly Padding _padding = new Padding(4);

		/// <summary>
		/// Do not use this property. Any value will not affect the appearance.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Padding Padding
		{
			get
			{
				return _padding;
			}
			set
			{
				base.Padding = _padding;
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * Initiator
		 */

		private INuGenEventInitiatorService _initiator = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenEventInitiatorService Initiator
		{
			get
			{
				if (_initiator == null)
				{
					_initiator = new NuGenEventInitiatorService(this, this.Events);
				}

				return _initiator;
			}
		}

		/*
		 * Renderer
		 */

		private INuGenTextBoxRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenTextBoxRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenTextBoxRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTextBoxRenderer>();
					}
				}

				return _renderer;
			}
		}

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider = null;

		/// <summary>
		/// </summary>
		protected INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		/*
		 * StateTracker
		 */

		private INuGenControlStateTracker _stateTracker = null;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenControlStateTracker StateTracker
		{
			get
			{
				if (_stateTracker == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					INuGenControlStateService stateService = this.ServiceProvider.GetService<INuGenControlStateService>();

					if (stateService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenControlStateTracker>();
					}

					_stateTracker = stateService.CreateStateTracker();
					Debug.Assert(_stateTracker != null, "_stateTracker != null");
				}

				return _stateTracker;
			}
		}

		#endregion

		#region Properties.Public.Overridden

		/*
		 * BackColor
		 */

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[DefaultValue(typeof(Color), "Window")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;

				Debug.Assert(this.TextBoxInternal != null, "this.TextBoxInternal != null");
				this.TextBoxInternal.BackColor = value;
			}
		}

		/*
		 * ForeColor
		 */

		/// <summary>
		/// Gets or sets the foreground color of the control.
		/// </summary>
		/// <value></value>
		/// <returns>The foreground <see cref="T:System.Drawing.Color"></see> of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultForeColor"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;

				Debug.Assert(this.TextBoxInternal != null, "this.TextBoxInternal != null");
				this.TextBoxInternal.ForeColor = value;
			}
		}

		/*
		 * Text
		 */

		/// <summary>
		/// Gets or sets the text this text box displays.
		/// </summary>
		/// <value></value>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override string Text
		{
			get
			{
				return this.TextBoxInternal.Text;
			}
			set
			{
				this.TextBoxInternal.Text = value;
			}
		}

		private static readonly object _textChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Text"/> property changes.
		/// </summary>
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
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
		/// Will bubble the <see cref="TextChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTextChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_textChanged, e);
		}

		#endregion

		#region Properties.Protected.Virtual

		/*
		 * TextBoxInternal
		 */

		private PromptTextBox _textBoxInternal = null;

		/// <summary>
		/// </summary>
		protected virtual PromptTextBox TextBoxInternal
		{
			get
			{
				if (_textBoxInternal == null)
				{
					_textBoxInternal = new PromptTextBox();
				}

				return _textBoxInternal;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnEnabledChanged
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");
			this.StateTracker.Enabled(this.Enabled);
			base.OnEnabledChanged(e);
		}

		/*
		 * OnGotFocus
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnGotFocus(EventArgs e)
		{
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");
			this.StateTracker.GotFocus();
			base.OnGotFocus(e);
		}

		/*
		 * OnLostFocus
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnLostFocus(EventArgs e)
		{
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");
			this.StateTracker.LostFocus();
			base.OnLostFocus(e);
		}

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Debug.Assert(this.Renderer != null, "this.Renderer != null");
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");

			this.Renderer.DrawBorder(new NuGenPaintParams(this, e.Graphics, this.ClientRectangle, this.StateTracker.GetControlState()));
		}

		/*
		 * OnSizeChanged
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			this.Height = this.TextBoxInternal.Height + this.Padding.Top + this.Padding.Bottom;
			this.Width = this.TextBoxInternal.Width + this.Padding.Left + this.Padding.Right;
		}

		#endregion

		#region Methods.Private

		/*
		 * SubscribeEvents
		 */

		private void SubscribeEvents(PromptTextBox textBox)
		{
			Debug.Assert(textBox != null, "textBox != null");

			textBox.BackColorChanged += delegate
			{
				this.BackColor = this.TextBoxInternal.BackColor;
			};

			textBox.BackColorChanged += delegate
			{
				this.BackColor = _textBoxInternal.BackColor;
			};

			textBox.FocusSelectChanged += delegate(object sender, EventArgs e)
			{
				this.OnFocusSelectChanged(e);
			};

			textBox.InvalidTextBackColorChanged += delegate(object sender, EventArgs e)
			{
				this.OnInvalidTextBackColorChanged(e);
			};

			textBox.PatternModeChanged += delegate(object sender, EventArgs e)
			{
				this.OnPatternModeChanged(e);
			};

			textBox.PatternChanged += delegate(object sender, EventArgs e)
			{
				this.OnPatternChanged(e);
			};

			textBox.PromptFontChanged += delegate(object sender, EventArgs e)
			{
				this.OnPromptFontChanged(e);
			};

			textBox.PromptForeColorChanged += delegate(object sender, EventArgs e)
			{
				this.OnPromptForeColorChanged(e);
			};

			textBox.PromptTextChanged += delegate(object sender, EventArgs e)
			{
				this.OnPromptTextChanged(e);
			};

			textBox.SizeChanged += delegate
			{
				this.OnSizeChanged(EventArgs.Empty);
			};

			textBox.TextChanged += delegate(object sender, EventArgs e)
			{
				this.OnTextChanged(e);
			};

			textBox.UseColorsChanged += delegate(object sender, EventArgs e)
			{
				this.OnUseColorsChanged(e);
			};

			textBox.ValidTextBackColorChanged += delegate(object sender, EventArgs e)
			{
				this.OnValidTextBackColorChanged(e);
			};
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTextBox"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenControlStateService"/><para/>
		/// <see cref="INuGenTextBoxRenderer"/><para/>
		/// </param>
		public NuGenTextBox(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;

			this.TextBoxInternal.BorderStyle = BorderStyle.None;
			this.TextBoxInternal.Dock = DockStyle.Fill;
			this.SubscribeEvents(this.TextBoxInternal);

			base.Padding = this.Padding;

			this.BackColor = SystemColors.Window;
			this.Controls.Add(this.TextBoxInternal);
		}

		#endregion
	}
}
