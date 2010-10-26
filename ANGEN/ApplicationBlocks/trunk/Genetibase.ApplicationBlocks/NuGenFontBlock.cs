/* -----------------------------------------------
 * NuGenFontBlock.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Controls.FontBoxInternals;
using Genetibase.Shared.Controls.SwitcherInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.ApplicationBlocks.Properties;
using Genetibase.ApplicationBlocks.ComponentModel;

namespace Genetibase.ApplicationBlocks
{
	/// <summary>
	/// Represents a control to specify font parameters.
	/// </summary>
	[ToolboxItem(false)]
	[Designer("Genetibase.ApplicationBlocks.Design.NuGenFontBlockDesigner")]
	public class NuGenFontBlock : NuGenControl
	{
		private Font _selectedFont;

		/// <summary>
		/// Gets or sets the currently selected font.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_FontBlock_SelectedFont")]
		public Font SelectedFont
		{
			get
			{
				return _selectedFont = this.BuildFont();
			}
			set
			{
				if (_selectedFont != value)
				{
					_selectedFont = value;
					this.OnSelectedFontChanged(EventArgs.Empty);
					this.ParseFont(value);
				}
			}
		}

		private static readonly object _selectedFontChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SelectedFont"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_FontBlock_SelectedFontChanged")]
		public event EventHandler SelectedFontChanged
		{
			add
			{
				this.Events.AddHandler(_selectedFontChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_selectedFontChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.ApplicationBlocks.NuGenFontBlock.SelectedFontChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected virtual void OnSelectedFontChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_selectedFontChanged, e);
		}

		/// <summary>
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(244, 21);
			}
		}

		private Font BuildFont()
		{
			FontStyle fontStyle = FontStyle.Regular;

			if (_boldButton.Checked) fontStyle |= FontStyle.Bold;
			if (_italicButton.Checked) fontStyle |= FontStyle.Italic;
			if (_underlineButton.Checked) fontStyle |= FontStyle.Underline;

			return new Font((string)_fontBox.SelectedItem, _fontSizeBox.Value, fontStyle);
		}

		private void ParseFont(Font fontToParse)
		{
			if (fontToParse == null || !_fontBox.Items.Contains(fontToParse.Name))
			{
				fontToParse = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular);
			}

			_fontBox.SelectedIndex = _fontBox.Items.IndexOf(fontToParse.Name);
			_fontSizeBox.Value = (int)fontToParse.Size;
			_boldButton.Checked = fontToParse.Bold;
			_italicButton.Checked = fontToParse.Italic;
			_underlineButton.Checked = fontToParse.Underline;
		}

		private void _selectedFont_StyleChanged(object sender, EventArgs e)
		{
			this.OnSelectedFontChanged(EventArgs.Empty);
		}

		private NuGenFontBox _fontBox;
		private NuGenFontSizeBox _fontSizeBox;
		private NuGenSwitchButton
			_boldButton
			, _italicButton
			, _underlineButton
			;
		private NuGenSpacer[] _spacers;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFontBlock"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para><see cref="INuGenButtonStateService"/></para>
		/// <para><see cref="INuGenControlStateService"/></para>
		/// <para><see cref="INuGenComboBoxRenderer"/></para>
		/// <para><see cref="INuGenImageListService"/></para>
		/// <para><see cref="INuGenFontFamiliesProvider"/></para>
		/// <para><see cref="INuGenSwitchButtonLayoutManager"/></para>
		/// <para><see cref="INuGenSwitchButtonRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenFontBlock(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_spacers = new NuGenSpacer[4];

			for (int i = 0; i < _spacers.Length; i++)
			{
				_spacers[i] = new NuGenSpacer();
				_spacers[i].Dock = DockStyle.Right;
				_spacers[i].Width = 3;
			}

			_fontBox = new NuGenFontBox(serviceProvider);
			_fontBox.Dock = DockStyle.Fill;

			_fontSizeBox = new NuGenFontSizeBox(serviceProvider);
			_fontSizeBox.Dock = DockStyle.Right;
			_fontSizeBox.Width = 50;

			_boldButton = new NuGenSwitchButton(serviceProvider);
			_italicButton = new NuGenSwitchButton(serviceProvider);
			_underlineButton = new NuGenSwitchButton(serviceProvider);

			_boldButton.Size = _italicButton.Size = _underlineButton.Size = new Size(21, 21);
			_boldButton.Dock = _italicButton.Dock = _underlineButton.Dock = DockStyle.Right;
			_boldButton.CheckOnClick = _italicButton.CheckOnClick = _underlineButton.CheckOnClick = true;

			_boldButton.Text = Resources.Text_FontBlock_Bold;
			_italicButton.Text = Resources.Text_FontBlock_Italic;
			_underlineButton.Text = Resources.Text_FontBlock_Underline;

			_fontBox.SelectedIndexChanged += _selectedFont_StyleChanged;
			_fontSizeBox.ValueChanged += _selectedFont_StyleChanged;
			_boldButton.CheckedChanged += _selectedFont_StyleChanged;
			_italicButton.CheckedChanged += _selectedFont_StyleChanged;
			_underlineButton.CheckedChanged += _selectedFont_StyleChanged;

			this.Controls.AddRange(
				new Control[]
				{
					_fontBox
					, _spacers[0]
					, _fontSizeBox
					, _spacers[1]
					, _boldButton
					, _spacers[2]
					, _italicButton
					, _spacers[3]
					, _underlineButton
				}
			);
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_fontBox != null)
				{
					_fontBox.SelectedIndexChanged -= _selectedFont_StyleChanged;
				}

				if (_fontSizeBox != null)
				{
					_fontSizeBox.ValueChanged -= _selectedFont_StyleChanged;
				}

				if (_boldButton != null)
				{
					_boldButton.CheckedChanged -= _selectedFont_StyleChanged;
				}

				if (_italicButton != null)
				{
					_italicButton.CheckedChanged -= _selectedFont_StyleChanged;
				}

				if (_underlineButton != null)
				{
					_underlineButton.CheckedChanged -= _selectedFont_StyleChanged;
				}
			}

			base.Dispose(disposing);
		}
	}
}
