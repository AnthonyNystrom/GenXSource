/* -----------------------------------------------
 * NuGenTextBox.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenTextBox
	{
		/// <summary>
		/// </summary>
		protected class PromptTextBox : TextBox
		{
			#region Properties.Public

			/*
			 * FocusSelect
			 */

			private bool _focusSelect = true;

			/// <summary>
			/// Gets or sets the value indicating whether to automatically select the text when the
			/// text box receives the focus.
			/// </summary>
			public bool FocusSelect
			{
				get
				{
					return _focusSelect;
				}
				set
				{
					if (_focusSelect != value)
					{
						_focusSelect = value;
						this.OnFocusSelectChanged(EventArgs.Empty);
					}
				}
			}

			private static readonly object _focusSelectChanged = new object();

			/// <summary>
			/// Occurs when the value of the <see cref="FocusSelect"/> property changes.
			/// </summary>
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
			 * InvalidTextBackColor
			 */

			private Color _invalidTextBackColor = Color.Empty;

			/// <summary>
			/// Gets or sets the color used as a background color of the control when user input
			/// does not match the chosen pattern.<para/>
			/// <seealso cref="ValidTextBackColor"/>
			/// <seealso cref="UseColors"/>
			/// <seealso cref="Pattern"/>
			/// <seealso cref="PatternMode"/>
			/// </summary>
			public Color InvalidTextBackColor
			{
				get
				{
					if (_invalidTextBackColor == Color.Empty)
					{
						return this.DefaultInvalidTextBackColor;
					}

					return _invalidTextBackColor;
				}
				set
				{
					if (_invalidTextBackColor != value)
					{
						_invalidTextBackColor = value;
						this.OnInvalidTextBackColorChanged(EventArgs.Empty);
						this.Invalidate();
					}
				}
			}

			/// <summary>
			/// </summary>
			/// <returns></returns>
			internal bool ShouldSerializeInvalidTextBackColor()
			{
				return this.InvalidTextBackColor != this.DefaultInvalidTextBackColor;
			}

			/// <summary>
			/// </summary>
			internal void ResetInvalidTextBackColor()
			{
				this.InvalidTextBackColor = this.DefaultInvalidTextBackColor;
			}

			/// <summary>
			/// </summary>
			protected virtual Color DefaultInvalidTextBackColor
			{
				get
				{
					return Color.LightGreen;
				}
			}

			private static readonly object _invalidTextBackColorChanged = new object();

			/// <summary>
			/// Occurs when the value of the <see cref="InvalidTextBackColor"/> property changes.
			/// </summary>
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
			/// <param name="e"></param>
			protected virtual void OnInvalidTextBackColorChanged(EventArgs e)
			{
				Debug.Assert(this.Initiator != null, "this.Initiator != null");
				this.Initiator.InvokePropertyChanged(_invalidTextBackColorChanged, e);
			}

			/*
			 * IsTextValid
			 */

			/// <summary>
			/// Returns <see langword="true"/> if the value of the <see cref="Text"/> property matches
			/// the <see cref="Pattern"/>.
			/// </summary>
			public bool IsTextValid
			{
				get
				{
					if (_regex != null)
					{
						return _regex.IsMatch(this.Text);
					}

					return true;
				}
			}

			/*
			 * PatternMode
			 */

			private NuGenTextBoxPatternMode _patternMode = NuGenTextBoxPatternMode.None;

			/// <summary>
			/// Gets or sets the type of the pattern to use.
			/// <seealso cref="Pattern"/>
			/// </summary>
			public NuGenTextBoxPatternMode PatternMode
			{
				get
				{
					return _patternMode;
				}
				set
				{
					if (_patternMode != value)
					{
						_patternMode = value;
						this.OnPatternModeChanged(EventArgs.Empty);
						this.SetPatternRegex();
						this.SetBackColor(this.IsTextValid);
					}
				}
			}

			private static readonly object _patternModeChanged = new object();

			/// <summary>
			/// Occurs when the value of the <see cref="PatternMode"/> property changes.
			/// </summary>
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
			/// <param name="e"></param>
			protected virtual void OnPatternModeChanged(EventArgs e)
			{
				Debug.Assert(this.Initiator != null, "this.Initiator != null");
				this.Initiator.InvokePropertyChanged(_patternModeChanged, e);
			}

			/*
			 * Pattern
			 */

			private string _pattern = "";

			/// <summary>
			/// Gets or sets the pattern to verify user input against.
			/// Used only when <see cref="PatternMode"/> property equals to
			/// <see cref="NuGenTextBoxPatternMode.CharacterCollection"/>,
			/// <see cref="NuGenTextBoxPatternMode.WildcardPattern"/>, or
			/// <see cref="NuGenTextBoxPatternMode.RegexPattern"/>.<para/>
			/// </summary>
			/// <example>
			/// <code>
			/// // Example 1 - a collection of characters ("a", "b", "c", "D", "E", "F", "1", "2", "3", "@").
			/// PromptTextBox textBox = new PromptTextBox();
			/// textBox.Pattern = "abcDEF123@";
			/// textBox.PatternMode = NuGenTextBoxPatternMode.CharacterCollection;
			/// 
			/// // Example 2 - a wildcard pattern (e-mail address).
			/// PromptTextBox textBox2 = new PromptTextBox();
			/// textBox2.PatternString = "*@*.com";
			/// textBox2.Pattern = NuGenTextBoxPatternMode.WildcardPattern;
			/// 
			/// // Example 3 - a regular expression (only digits).
			/// PromptTextBox textBox3 = new PromptTextBox();
			/// textBox3.PatternString = "^\d+$";
			/// textBox3.Pattern = NuGenTextBoxPatternMode.RegexPattern;
			/// </code>
			/// </example>
			[Description("Determines the pattern to verify user input against. Affects behavior only if PatternMode is set to CharacterCollection, WildcardPattern, or RegexPattern.")]
			public string Pattern
			{
				get
				{
					return _pattern;
				}
				set
				{
					if (_pattern != value)
					{
						_pattern = value;
						this.OnPatternChanged(EventArgs.Empty);
						this.SetPatternRegex();
						this.SetBackColor(this.IsTextValid);
					}
				}
			}

			private static readonly object _patternChanged = new object();

			/// <summary>
			/// Occurs when the value of the <see cref="Pattern"/> property changes.
			/// </summary>
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
			/// <param name="e"></param>
			protected virtual void OnPatternChanged(EventArgs e)
			{
				Debug.Assert(this.Initiator != null, "this.Initiator != null");
				this.Initiator.InvokePropertyChanged(_patternChanged, e);
			}

			/*
			 * PromptFont
			 */

			private Font _promptFont;

			/// <summary>
			/// Gets or sets the font to use when displaying the <see cref="PromptText"/>.
			/// </summary>
			public Font PromptFont
			{
				get
				{
					return _promptFont;
				}
				set
				{
					if (_promptFont != value)
					{
						_promptFont = value;
						this.OnPromptFontChanged(EventArgs.Empty);
						this.Invalidate();
					}
				}
			}

			private static readonly object _promptFontChanged = new object();

			/// <summary>
			/// Occurs when the value of the <see cref="PromptFont"/> property changes.
			/// </summary>
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

			private Color _promptForeColor = SystemColors.GrayText;

			/// <summary>
			/// Gets or sets the fore color to use when displaying the <see cref="PromptText"/>.
			/// </summary>
			public Color PromptForeColor
			{
				get
				{
					return _promptForeColor;
				}
				set
				{
					if (_promptForeColor != value)
					{
						_promptForeColor = value;
						this.OnPromptForeColorChanged(EventArgs.Empty);
						this.Invalidate();
					}
				}
			}

			private static readonly object _promptForeColorChanged = new object();

			/// <summary>
			/// Occurs when the value of the <see cref="PromptForeColor"/> property changes.
			/// </summary>
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

			private string _promptText = "";

			/// <summary>
			/// Gets or sets the prompt text to display if <see cref="Text"/> = "".
			/// </summary>
			/// <exception cref="ArgumentNullException"><para><paramref name="value"/> is <see langword="null"/>.</para></exception>
			public string PromptText
			{
				get
				{
					return _promptText;
				}
				set
				{
					if (value == null)
					{
						throw new ArgumentNullException("value");
					}

					string newPromptText = value.Trim();

					if (newPromptText != _promptText)
					{
						_promptText = newPromptText;
						this.OnPromptTextChanged(EventArgs.Empty);
						this.Invalidate();
					}
				}
			}

			private static readonly object _promptTextChanged = new object();

			/// <summary>
			/// Occurs when the value of the <see cref="PromptText"/> property changes.
			/// </summary>
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

			/*
			 * TextValidated
			 */

			/// <summary>
			/// Returns the value of the <see cref="Text"/> property if it is valid; otherwise, empty string 
			/// is returned.
			/// <seealso cref="Pattern"/>
			/// <seealso cref="PatternMode"/>
			/// </summary>
			public string TextValidated
			{
				get
				{
					if (this.IsTextValid)
					{
						return this.Text;
					}
					else
					{
						return "";
					}
				}
			}

			/*
			 * UseColors
			 */

			private bool _useColors;

			/// <summary>
			/// Gets or sets the value indicating whether the background color of the control should change
			/// to show that user's input does or does not match the chosen pattern.<para/>
			/// <seealso cref="InvalidTextBackColor"/>
			/// <seealso cref="ValidTextBackColor"/>
			/// <seealso cref="PatternMode"/>
			/// <seealso cref="Pattern"/>
			/// </summary>
			public bool UseColors
			{
				get
				{
					return _useColors;
				}
				set
				{
					if (_useColors != value)
					{
						_useColors = value;
						this.OnUseColorsChanged(EventArgs.Empty);
						this.SetBackColor(this.IsTextValid);
					}
				}
			}

			private static readonly object _useColorsChanged = new object();

			/// <summary>
			/// Occurs when the value of the <see cref="UseColors"/> property changes.
			/// </summary>
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
			/// <param name="e"></param>
			protected virtual void OnUseColorsChanged(EventArgs e)
			{
				Debug.Assert(this.Initiator != null, "this.Initiator != null");
				this.Initiator.InvokePropertyChanged(_useColorsChanged, e);
			}

			/*
			 * ValidTextBackColor
			 */

			private Color _validTextBackColor = Color.Empty;

			/// <summary>
			/// Gets or sets the color used as a background color of the control when user input matches
			/// the chosen pattern.<para/>
			/// <seealso cref="InvalidTextBackColor"/>
			/// <seealso cref="UseColors"/>
			/// <seealso cref="PatternMode"/>
			/// <seealso cref="Pattern"/>
			/// </summary>
			public Color ValidTextBackColor
			{
				get
				{
					if (_validTextBackColor == Color.Empty)
					{
						return this.DefaultValidTextBackColor;
					}

					return _validTextBackColor;
				}
				set
				{
					if (_validTextBackColor != value)
					{
						_validTextBackColor = value;
						this.OnValidTextBackColorChanged(EventArgs.Empty);
						this.SetBackColor(this.IsTextValid);
					}
				}
			}

			/// <summary>
			/// </summary>
			/// <returns></returns>
			internal bool ShouldSerializeValidTextBackColor()
			{
				return this.ValidTextBackColor != this.DefaultValidTextBackColor;
			}

			/// <summary>
			/// </summary>
			internal void ResetValidTextBackColor()
			{
				this.ValidTextBackColor = this.DefaultValidTextBackColor;
			}

			/// <summary>
			/// </summary>
			protected virtual Color DefaultValidTextBackColor
			{
				get
				{
					return Color.LightPink;
				}
			}

			private static readonly object _validTextBackColorChanged = new object();

			/// <summary>
			/// Occurs when the value of the <see cref="ValidTextBackColor"/> property changes.
			/// </summary>
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
			/// <param name="e"></param>
			protected virtual void OnValidTextBackColorChanged(EventArgs e)
			{
				Debug.Assert(this.Initiator != null, "this.Initiator != null");
				this.Initiator.InvokePropertyChanged(_validTextBackColorChanged, e);
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

			#endregion

			#region Methods.Protected

			/*
			 * AddBeginningOfStringAndEndOfStringMetacharacters
			 */

			/// <summary>
			/// </summary>
			/// <param name="str"></param>
			/// <returns></returns>
			/// <exception cref="ArgumentNullException"><para><paramref name="str"/> is <see langword="null"/>.</para></exception>
			protected static string AddBeginningOfStringAndEndOfStringMetacharacters(string str)
			{
				if (str == null)
				{
					throw new ArgumentNullException("str");
				}

				str = "^(" + str + ")$";
				return str;
			}

			/*
			 * AddCharacterCollectionMetacharacters
			 */

			/// <summary>
			/// </summary>
			/// <param name="str"></param>
			/// <returns></returns>
			/// <exception cref="ArgumentNullException"><para><paramref name="str"/> is <see langword="null"/>.</para></exception>
			protected static string AddCharacterCollectionMetacharacters(string str)
			{
				if (str == null)
				{
					throw new ArgumentNullException("str");
				}

				if (str.Length != 0)
				{
					// I don't like this condition but new Regex("^{[]+}$") results in:
					// An unhandled exception of type 'System.ArgumentException' occurred in system.dll
					// Additional information: parsing "^{[]+}$" - Unterminated [] set.
					str = "[" + str + "]+";
				}

				return str;
			}

			/*
			 * EscapeRegularExpressionsOperators
			 */

			/// <summary>
			/// </summary>
			/// <param name="str"></param>
			/// <returns></returns>
			/// <exception cref="ArgumentNullException"><para><paramref name="str"/> is <see langword="null"/>.</para></exception>
			protected string EscapeRegularExpressionsOperators(string str)
			{
				if (str == null)
				{
					throw new ArgumentNullException("str");
				}

				str = str.Replace("\\", "\\\\");   // it is crucial to replace "\" first
				str = str.Replace(".", "\\.");
				str = str.Replace("^", "\\^");
				str = str.Replace("$", "\\$");
				str = str.Replace("+", "\\+");
				str = str.Replace("|", "\\|");
				str = str.Replace("{", "\\{");
				str = str.Replace("}", "\\}");
				str = str.Replace("[", "\\[");
				str = str.Replace("]", "\\]");
				str = str.Replace("(", "\\(");
				str = str.Replace(")", "\\)");

				if (this.PatternMode != NuGenTextBoxPatternMode.WildcardPattern)
				{
					str = str.Replace("*", "\\*");
					str = str.Replace("?", "\\?");
				}

				return str;
			}

			/*
			 * Regex
			 */

			/// <summary>
			/// </summary>
			/// <param name="pattern"></param>
			/// <returns></returns>
			protected static Regex GetRegex(string pattern)
			{
				if (pattern == null)
				{
					return null;
				}

				return new Regex(pattern);
			}

			/// <summary>
			/// </summary>
			protected void SetPatternRegex()
			{
				_regex = PromptTextBox.GetRegex(this.GetRegexString());
			}

			/// <summary>
			/// </summary>
			/// <returns></returns>
			protected string GetRegexString()
			{
				string str = "";

				switch (this.PatternMode)
				{
					case NuGenTextBoxPatternMode.None:
					{
						str = "";
						str = AddBeginningOfStringAndEndOfStringMetacharacters(str);
						break;
					}
					case NuGenTextBoxPatternMode.SmallLetters:
					{
						str = "[a-z]+";
						str = AddBeginningOfStringAndEndOfStringMetacharacters(str);
						break;
					}
					case NuGenTextBoxPatternMode.CapitalLetters:
					{
						str = "[A-Z]+";
						str = AddBeginningOfStringAndEndOfStringMetacharacters(str);
						break;
					}
					case NuGenTextBoxPatternMode.Digits:
					{
						str = "[0-9]+";
						str = AddBeginningOfStringAndEndOfStringMetacharacters(str);
						break;
					}
					case NuGenTextBoxPatternMode.NonAlphaNumericCharacters:
					str = "[^a-zA-Z0-9]+";
					{
						str = AddBeginningOfStringAndEndOfStringMetacharacters(str);
						break;
					}
					case NuGenTextBoxPatternMode.WildcardPattern:
					{
						str = this.Pattern;
						str = EscapeRegularExpressionsOperators(str);
						str = ReplaceWildcardCharacters(str);
						str = AddBeginningOfStringAndEndOfStringMetacharacters(str);
						break;
					}
					case NuGenTextBoxPatternMode.CharacterCollection:
					{
						str = this.Pattern;
						str = EscapeRegularExpressionsOperators(str);
						str = AddCharacterCollectionMetacharacters(str);
						str = AddBeginningOfStringAndEndOfStringMetacharacters(str);
						break;
					}
					case NuGenTextBoxPatternMode.RegexPattern:
					{
						str = this.Pattern;
						break;
					}
					case NuGenTextBoxPatternMode.All:
					{
						str = ".+";
						str = AddBeginningOfStringAndEndOfStringMetacharacters(str);
						break;
					}
				}

				return str;
			}

			/*
			 * ReplaceWildcardCharacters
			 */

			/// <summary>
			/// </summary>
			/// <param name="str"></param>
			/// <returns></returns>
			/// <exception cref="ArgumentNullException"><para><paramref name="str"/> is <see langword="null"/>.</para></exception>
			protected static string ReplaceWildcardCharacters(string str)
			{
				if (str == null)
				{
					throw new ArgumentNullException("str");
				}

				str = str.Replace("*", "(.+)");
				str = str.Replace("?", "(.)");

				return str;
			}

			/*
			 * SetBackColor
			 */

			/// <summary>
			/// </summary>
			/// <param name="isTextValid"></param>
			protected void SetBackColor(bool isTextValid)
			{
				if (this.UseColors)
				{
					if (this.Text.Length == 0)
					{
						this.BackColor = Color.FromKnownColor(KnownColor.Window);
					}
					else
					{
						if (isTextValid)
						{
							this.BackColor = ValidTextBackColor;
						}
						else
						{
							this.BackColor = InvalidTextBackColor;
						}
					}
				}
				else
				{
					this.BackColor = Color.FromKnownColor(KnownColor.Window);
				}
			}

			#endregion

			#region Methods.Protected.Overridden

			/*
			 * OnEnter
			 */

			/// <summary>
			/// Raises the <see cref="E:System.Windows.Forms.Control.Enter"></see> event.
			/// </summary>
			/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
			protected override void OnEnter(EventArgs e)
			{
				if (this.Text.Length > 0 && this.FocusSelect)
				{
					this.SelectAll();
				}

				base.OnEnter(e);
			}

			/*
			 * OnTextAlignChanged
			 */

			/// <summary>
			/// Raises the <see cref="E:System.Windows.Forms.TextBox.TextAlignChanged"></see> event.
			/// </summary>
			/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
			protected override void OnTextAlignChanged(EventArgs e)
			{
				base.OnTextAlignChanged(e);
				this.Invalidate();
			}

			/*
			 * WndProc
			 */

			/// <summary>
			/// Processes Windows messages.
			/// </summary>
			/// <param name="m"></param>
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			protected override void WndProc(ref Message m)
			{
				switch (m.Msg)
				{
					case WinUser.WM_SETFOCUS:
					{
						_drawPrompt = false;
						break;
					}
					case WinUser.WM_KILLFOCUS:
					{
						_drawPrompt = true;
						break;
					}
					case WinUser.WM_USER + 7441:
					{
						this.SetBackColor(this.IsTextValid);
						break;
					}
				}

				base.WndProc(ref m);

				// Only draw the prompt on the WM_PAINT event and when the Text property is empty.
				if (
					m.Msg == WinUser.WM_PAINT
					&& _drawPrompt
					&& this.Text.Length == 0
					&& !this.GetStyle(ControlStyles.UserPaint)
					)
				{
					using (Graphics g = Graphics.FromHwnd(this.Handle))
					{
						this.DrawTextPrompt(g);
					}
				}
			}

			#endregion

			#region Methods.Protected.Virtual

			/*
			 * DrawTextPrompt
			 */

			/// <summary>
			/// Draws the PromptText in the TextBox.ClientRectangle using the PromptFont and PromptForeColor
			/// </summary>
			/// <param name="g">The Graphics region to draw the prompt on</param>
			protected virtual void DrawTextPrompt(Graphics g)
			{
				TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.Top | TextFormatFlags.EndEllipsis;

				TextRenderer.DrawText(
					g,
					this.PromptText,
					this.PromptFont,
					this.ClientRectangle,
					this.PromptForeColor,
					this.BackColor,
					flags
				);
			}

			#endregion

			private bool _drawPrompt = true;
			private Regex _regex;

			/// <summary>
			/// Initializes a new instance of the <see cref="PromptTextBox"/> class.
			/// </summary>
			public PromptTextBox()
			{
				this.PromptFont = this.Font;
			}
		}
	}
}
