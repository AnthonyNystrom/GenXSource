/* -----------------------------------------------
 * NuGenHotKeyPopup.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using ctrls = Genetibase.Shared.Controls;

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.CheckBoxInternals;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.HotKeySelectorInternals;
using Genetibase.Shared.Controls.LabelInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.Properties;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	internal sealed partial class NuGenHotKeyPopup : NuGenPanel
	{
		#region Events

		private static readonly object _selectionAccepted = new object();

		public event EventHandler<NuGenHotKeyEventArgs> SelectionAccepted
		{
			add
			{
				this.Events.AddHandler(_selectionAccepted, value);
			}
			remove
			{
				this.Events.RemoveHandler(_selectionAccepted, value);
			}
		}

		private void OnSelectionAccepted(NuGenHotKeyEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenHotKeyEventArgs>(_selectionAccepted, e);
		}

		private static readonly object _selectionCanceled = new object();

		public event EventHandler SelectionCanceled
		{
			add
			{
				this.Events.AddHandler(_selectionCanceled, value);
			}
			remove
			{
				this.Events.RemoveHandler(_selectionCanceled, value);
			}
		}

		private void OnSelectionCanceled(EventArgs e)
		{
			this.Initiator.InvokeEventHandler(_selectionCanceled, e);
		}

		#endregion

		#region Properties.Protected.Overridden

		private static readonly Size _defaultSize = new Size(184, 105);

		/// <summary>
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return _defaultSize;
			}
		}

		#endregion

		#region Methods.Public

		public void SetSelectedHotKeys(Keys hotKeys)
		{
			_ctrlCheckBox.Checked = (hotKeys & Keys.Control) != Keys.None; 
			_shiftCheckBox.Checked = (hotKeys & Keys.Shift) != Keys.None;
			_altCheckBox.Checked = (hotKeys & Keys.Alt) != Keys.None;
			_keyCombo.SelectedKey = hotKeys & Keys.KeyCode;
		}

		#endregion

		#region Methods.Protected.Overridden

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Return)
			{
				this.OnSelectionAccepted(new NuGenHotKeyEventArgs(this.BuildHotKeys()));
			}
			else if (keyData == Keys.Escape)
			{
				this.OnSelectionCanceled(EventArgs.Empty);
			}

			return base.ProcessDialogKey(keyData);
		}

		#endregion

		#region Methods.Private

		private Keys BuildHotKeys()
		{
			Keys hotKeys = _keyCombo.SelectedKey;

			if (hotKeys == Keys.None)
			{
				return Keys.None;
			}

			if (_ctrlCheckBox.Checked)
			{
				hotKeys |= Keys.Control;
			}

			if (_shiftCheckBox.Checked)
			{
				hotKeys |= Keys.Shift;
			}

			if (_altCheckBox.Checked)
			{
				hotKeys |= Keys.Alt;
			}

			return hotKeys;
		}

		#endregion

		#region EventHandlers

		private void _resetButton_Click(object sender, EventArgs e)
		{
			_ctrlCheckBox.Checked = false;
			_shiftCheckBox.Checked = false;
			_altCheckBox.Checked = false;
			_keyCombo.SelectedIndex = -1;
		}

		#endregion

		private NuGenButton _resetButton;
		private NuGenCheckBox _ctrlCheckBox;
		private NuGenCheckBox _shiftCheckBox;
		private NuGenCheckBox _altCheckBox;
		private KeyCombo _keyCombo;
		private Label _modifiersLabel;
		private Label _keyLabel;
		private FlowLayoutPanel _modifiersLayoutPanel;
		private FlowLayoutPanel _keyLayoutPanel;
		private TableLayoutPanel _tableLayoutPanel;

		private static readonly Padding _offsetMargin = new Padding(12, 3, 3, 3);

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenHotKeyPopup"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// <para><see cref="INuGenButtonStateTracker"/></para>
		/// <para><see cref="INuGenControlStateTracker"/></para>
		/// <para><see cref="INuGenButtonLayoutManager"/></para>
		/// <para><see cref="INuGenButtonRenderer"/></para>
		/// <para><see cref="INuGenCheckBoxLayoutManager"/></para>
		/// <para><see cref="INuGenCheckBoxRenderer"/></para>
		/// <para><see cref="INuGenComboBoxRenderer"/></para>
		/// <para><see cref="INuGenLabelLayoutManager"/></para>
		/// <para><see cref="INuGenLabelRenderer"/></para>
		/// <para><see cref="INuGenPanelRenderer"/></para>
		/// <para><see cref="INuGenControlImageManager"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenHotKeyPopup(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_modifiersLabel = new Label(serviceProvider);
			_modifiersLabel.Text = Resources.Text_HotKeysPopup_Modifiers;

			_keyLabel = new Label(serviceProvider);
			_keyLabel.Text = Resources.Text_HotKeysPopup_Key;

			_ctrlCheckBox = new NuGenCheckBox(serviceProvider);
			_ctrlCheckBox.Text = Resources.Text_HotKey_ctrlCheckBox;

			_shiftCheckBox = new NuGenCheckBox(serviceProvider);
			_shiftCheckBox.Text = Resources.Text_HotKey_shiftCheckBox;
			
			_altCheckBox = new NuGenCheckBox(serviceProvider);
			_altCheckBox.Text = Resources.Text_HotKey_altCheckBox;

			_modifiersLayoutPanel = new FlowLayoutPanel();
			_modifiersLayoutPanel.Dock = DockStyle.Fill;
			_modifiersLayoutPanel.Controls.AddRange(
				new Control[]
				{
					_ctrlCheckBox
					, _shiftCheckBox
					, _altCheckBox
				}
			);
			_modifiersLayoutPanel.Margin = _offsetMargin;

			_keyCombo = new KeyCombo(serviceProvider);
			_keyCombo.Width = 80;

			_resetButton = new NuGenButton(serviceProvider);
			_resetButton.Click += _resetButton_Click;
			_resetButton.Text = Resources.Text_HotKeysPopup_Reset;

			_keyLayoutPanel = new FlowLayoutPanel();
			_keyLayoutPanel.Dock = DockStyle.Fill;
			_keyLayoutPanel.Controls.AddRange(
				new Control[]
				{
					_keyCombo
					, _resetButton
				}
			);
			_keyLayoutPanel.Margin = _offsetMargin;

			_tableLayoutPanel = new TableLayoutPanel();
			_tableLayoutPanel.Dock = DockStyle.Fill;
			_tableLayoutPanel.Parent = this;
			_tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			_tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
			_tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
			_tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
			_tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
			_tableLayoutPanel.Controls.Add(_modifiersLabel, 0, 0);
			_tableLayoutPanel.Controls.Add(_modifiersLayoutPanel, 0, 1);
			_tableLayoutPanel.Controls.Add(_keyLabel, 0, 2);
			_tableLayoutPanel.Controls.Add(_keyLayoutPanel, 0, 3);
		}
	}
}
