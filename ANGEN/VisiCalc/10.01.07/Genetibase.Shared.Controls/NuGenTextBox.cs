/* -----------------------------------------------
 * NuGenTextBox.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
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
	[ToolboxItem(false)]
	[DefaultEvent("TextChanged")]
	[DefaultProperty("Text")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenTextBoxDesigner")]
	[NuGenSRDescription("Description_TextBox")]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenTextBox : UserControl
	{
		#region Properties.Appearance

		/*
		 * DrawBorder
		 */

		private bool _drawBorder = true;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_TextBox_DrawBorder")]
		public bool DrawBorder
		{
			get
			{
				return _drawBorder;
			}
			set
			{
				if (_drawBorder != value)
				{
					_drawBorder = value;
					this.OnDrawBorderChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _drawBorderChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="DrawBorder"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TextBox_DrawBorderChanged")]
		public event EventHandler DrawBorderChanged
		{
			add
			{
				this.Events.AddHandler(_drawBorderChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_drawBorderChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="DrawBorderChanged"/> event.
		/// </summary>
		protected virtual void OnDrawBorderChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_drawBorderChanged, e);
		}

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
				Debug.Assert(_textBoxInternal != null, "_textBoxInternal != null");
				return _textBoxInternal.PromptFont;
			}
			set
			{
				Debug.Assert(_textBoxInternal != null, "_textBoxInternal != null");
				_textBoxInternal.PromptFont = value;
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
				Debug.Assert(_textBoxInternal != null, "_textBoxInternal != null");
				return _textBoxInternal.PromptForeColor;
			}
			set
			{
				Debug.Assert(_textBoxInternal != null, "_textBoxInternal != null");
				_textBoxInternal.PromptForeColor = value;
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
				Debug.Assert(_textBoxInternal != null, "_textBoxInternal != null");
				return _textBoxInternal.PromptText;
			}
			set
			{
				Debug.Assert(_textBoxInternal != null, "_textBoxInternal != null");
				_textBoxInternal.PromptText = value;
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
				Debug.Assert(_textBoxInternal != null, "_textBoxInternal != null");
				return _textBoxInternal.FocusSelect;
			}
			set
			{
				Debug.Assert(_textBoxInternal != null, "_textBoxInternal != null");
				_textBoxInternal.FocusSelect = value;
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

		/*
		 * PasswordChar
		 */

		/// <summary>
		/// Gets or sets the character used to mast characters of a password in the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue('\0')]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TextBox_PasswordChar")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public char PasswordChar
		{
			get
			{
				return _textBoxInternal.PasswordChar;
			}
			set
			{
				_textBoxInternal.PasswordChar = value;
			}
		}

		/*
		 * ReadOnly
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_PromptTextBox_ReadOnly")]
		public bool ReadOnly
		{
			get
			{
				return _textBoxInternal.ReadOnly;
			}
			set
			{
				_textBoxInternal.ReadOnly = value;
			}
		}

		private static readonly object _readOnlyChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ReadOnly"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PromptTextBox_ReadOnlyChanged")]
		public event EventHandler ReadOnlyChanged
		{
			add
			{
				this.Events.AddHandler(_readOnlyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_readOnlyChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ReadOnlyChanged"/> event.
		/// </summary>
		protected virtual void OnReadOnlyChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_readOnlyChanged, e);
		}

		/*
		 * UseSystemPasswordChar
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the text in the control should appear as the default password characters.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TextBox_UseSystemPasswordChar")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public bool UseSystemPasswordChar
		{
			get
			{
				return _textBoxInternal.UseSystemPasswordChar;
			}
			set
			{
				_textBoxInternal.UseSystemPasswordChar = value;
			}
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
				return _textBoxInternal.InvalidTextBackColor;
			}
			set
			{
				_textBoxInternal.InvalidTextBackColor = value;
			}
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		protected bool ShouldSerializeInivalidTextBackColor()
		{
			return _textBoxInternal.ShouldSerializeInvalidTextBackColor();
		}

		/// <summary>
		/// </summary>
		protected void ResetInvalidTextBackColor()
		{
			_textBoxInternal.ResetInvalidTextBackColor();
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
				return _textBoxInternal.PatternMode;
			}
			set
			{
				_textBoxInternal.PatternMode = value;
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
				return _textBoxInternal.Pattern;
			}
			set
			{
				_textBoxInternal.Pattern = value;
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
				return _textBoxInternal.UseColors;
			}
			set
			{
				_textBoxInternal.UseColors = value;
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
				return _textBoxInternal.ValidTextBackColor;
			}
			set
			{
				_textBoxInternal.ValidTextBackColor = value;
			}
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		protected bool ShouldSerializeValidTextBackColor()
		{
			return _textBoxInternal.ShouldSerializeValidTextBackColor();
		}

		/// <summary>
		/// </summary>
		protected void ResetValidTextBackColor()
		{
			_textBoxInternal.ResetValidTextBackColor();
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

		#region Properties.NonBrowsable

		/// <summary>
		/// Gets or sets a value indicating the currently selected text in the control.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SelectedText
		{
			get
			{
				return _textBoxInternal.SelectedText;
			}
			set
			{
				_textBoxInternal.SelectedText = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of characters selected in the text box.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"/>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]		
		public int SelectionLength
		{
			get
			{
				return _textBoxInternal.SelectionLength;
			}
			set
			{
				_textBoxInternal.SelectionLength = value;
			}
		}

		/// <summary>
		/// Gets or sets the starting point of text selected in the text box.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"/>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectionStart
		{
			get
			{
				return _textBoxInternal.SelectionStart;
			}
			set
			{
				_textBoxInternal.SelectionStart = value;
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

				Debug.Assert(_textBoxInternal != null, "_textBoxInternal != null");
				_textBoxInternal.BackColor = value;
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

				Debug.Assert(_textBoxInternal != null, "_textBoxInternal != null");
				_textBoxInternal.ForeColor = value;
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
				return _textBoxInternal.Text;
			}
			set
			{
				_textBoxInternal.Text = value;
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
				base.Padding = value;
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * Initiator
		 */

		private INuGenEventInitiatorService _initiator;

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

		private INuGenServiceProvider _serviceProvider;

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

		private INuGenControlStateTracker _stateTracker;

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

		#region Methods.Public

		/*
		 * Select
		 */

		/// <summary>
		/// Selects a range of text in the text box.
		/// </summary>
		/// <param name="start">The position of the first character in the current text selection within the text box.</param>
		/// <param name="length">The number of characters to select.</param>
		/// <exception cref="ArgumentOutOfRangeException"/>
		public void Select(int start, int length)
		{
			_textBoxInternal.Select(start, length);
		}

		/*
		 * SelectAll
		 */

		/// <summary>
		/// Selects all text in the text box.
		/// </summary>
		public void SelectAll()
		{
			_textBoxInternal.SelectAll();
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

			NuGenBorderPaintParams paintParams = new NuGenBorderPaintParams(e.Graphics);
			paintParams.Bounds = this.ClientRectangle;
			paintParams.DrawBorder = this.DrawBorder;
			paintParams.State = this.StateTracker.GetControlState();

			this.Renderer.DrawBorder(paintParams);
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

			this.Height = _textBoxInternal.Height + this.Padding.Top + this.Padding.Bottom;
			this.Width = _textBoxInternal.Width + this.Padding.Left + this.Padding.Right;
		}

		#endregion

		#region Methods.Private

		private void SubscribeEvents(PromptTextBox textBox)
		{
			Debug.Assert(textBox != null, "textBox != null");

			textBox.BackColorChanged += _textBox_BackColorChanged;
			textBox.FocusSelectChanged += _textBox_FocusSelectChanged;
			textBox.InvalidTextBackColorChanged += _textBox_InvalidTextBackColorChanged;
			textBox.PatternChanged += _textBox_PatternChanged;
			textBox.PatternModeChanged += _textBox_PatternModeChanged;
			textBox.PromptFontChanged += _textBox_PromptFontChanged;
			textBox.PromptForeColorChanged += _textBox_PromptForeColorChanged;
			textBox.PromptTextChanged += _textBox_PromptTextChanged;
			textBox.ReadOnlyChanged += _textBox_ReadOnlyChanged;
			textBox.SizeChanged += _textBox_SizeChanged;
			textBox.TextChanged += _textBox_TextChanged;
			textBox.UseColorsChanged += _textBox_UseColorsChanged;
			textBox.ValidTextBackColorChanged += _textBox_ValidTextBackColorChanged;
		}

		private void UnsubscribeEvents(PromptTextBox textBox)
		{
			Debug.Assert(textBox != null, "textBox != null");

			textBox.BackColorChanged -= _textBox_BackColorChanged;
			textBox.FocusSelectChanged -= _textBox_FocusSelectChanged;
			textBox.InvalidTextBackColorChanged -= _textBox_InvalidTextBackColorChanged;
			textBox.PatternChanged -= _textBox_PatternChanged;
			textBox.PatternModeChanged -= _textBox_PatternModeChanged;
			textBox.PromptFontChanged -= _textBox_PromptFontChanged;
			textBox.PromptForeColorChanged -= _textBox_PromptForeColorChanged;
			textBox.PromptTextChanged -= _textBox_PromptTextChanged;
			textBox.ReadOnlyChanged -= _textBox_ReadOnlyChanged;
			textBox.SizeChanged -= _textBox_SizeChanged;
			textBox.TextChanged -= _textBox_TextChanged;
			textBox.UseColorsChanged -= _textBox_UseColorsChanged;
			textBox.ValidTextBackColorChanged -= _textBox_ValidTextBackColorChanged;
		}

		#endregion

		#region EventHandlers.TextBoxInternal

		private void _textBox_BackColorChanged(object sender, EventArgs e)
		{
			this.BackColor = _textBoxInternal.BackColor;
		}

		private void _textBox_FocusSelectChanged(object sender, EventArgs e)
		{
			this.OnFocusSelectChanged(e);
		}

		private void _textBox_InvalidTextBackColorChanged(object sender, EventArgs e)
		{
			this.OnInvalidTextBackColorChanged(e);
		}

		private void _textBox_PatternChanged(object sender, EventArgs e)
		{
			this.OnPatternChanged(e);
		}

		private void _textBox_PatternModeChanged(object sender, EventArgs e)
		{
			this.OnPatternModeChanged(e);
		}

		private void _textBox_PromptFontChanged(object sender, EventArgs e)
		{
			this.OnPromptFontChanged(e);
		}

		private void _textBox_PromptForeColorChanged(object sender, EventArgs e)
		{
			this.OnPromptForeColorChanged(e);
		}

		private void _textBox_PromptTextChanged(object sender, EventArgs e)
		{
			this.OnPromptTextChanged(e);
		}

		private void _textBox_ReadOnlyChanged(object sender, EventArgs e)
		{
			this.OnReadOnlyChanged(e);
		}

		private void _textBox_SizeChanged(object sender, EventArgs e)
		{
			this.OnSizeChanged(e);
		}

		private void _textBox_TextChanged(object sender, EventArgs e)
		{
			this.OnTextChanged(e);
		}

		private void _textBox_UseColorsChanged(object sender, EventArgs e)
		{
			this.OnUseColorsChanged(e);
		}

		private void _textBox_ValidTextBackColorChanged(object sender, EventArgs e)
		{
			this.OnValidTextBackColorChanged(e);
		}

		#endregion

		private PromptTextBox _textBoxInternal = new PromptTextBox();

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

			_textBoxInternal.BorderStyle = BorderStyle.None;
			_textBoxInternal.Dock = DockStyle.Fill;

			this.SubscribeEvents(_textBoxInternal);

			base.Padding = this.Padding;

			this.BackColor = SystemColors.Window;
			this.Controls.Add(_textBoxInternal);
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to dispose both managed and unmanaged resources; <see langword="false"/> to dispose only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_textBoxInternal != null)
				{
					this.UnsubscribeEvents(_textBoxInternal);
					_textBoxInternal = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
