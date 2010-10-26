/* -----------------------------------------------
 * NuGenAlignSelector.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.RadioButtonInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.Shared.Controls.ComponentModel;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a control to select content alignment.
	/// </summary>
	[ToolboxItem(false)]
	public partial class NuGenAlignSelector : NuGenControl
	{
		private static readonly object _alignmentAccepted = new object();

		/// <summary>
		/// Occurs when the user accepts the selected alignment.
		/// </summary>
		[Browsable(false)]
		public event EventHandler AlignmentAccepted
		{
			add
			{
				this.Events.AddHandler(_alignmentAccepted, value);
			}
			remove
			{
				this.Events.RemoveHandler(_alignmentAccepted, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Shared.Controls.NuGenAlignSelector.AlignmentAccepted"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnAlignmentAccepted(EventArgs e)
		{
			this.Initiator.InvokeEventHandler(_alignmentAccepted, e);
		}

		private static readonly object _alignmentCanceled = new object();

		/// <summary>
		/// Occurs when the user cancels the selected alignment.
		/// </summary>
		[Browsable(false)]
		public event EventHandler AlignmentCanceled
		{
			add
			{
				this.Events.AddHandler(_alignmentCanceled, value);
			}
			remove
			{
				this.Events.RemoveHandler(_alignmentCanceled, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Shared.Controls.NuGenAlignSelector.AlignmentCanceled"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnAlignmentCanceled(EventArgs e)
		{
			this.Initiator.InvokeEventHandler(_alignmentCanceled, e);
		}

		private ContentAlignment _alignment = ContentAlignment.MiddleCenter;

		/// <summary>
		/// Gets or sets the currently selected align style.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_AlignSelector_Alignment")]
		public ContentAlignment Alignment
		{
			get
			{
				return _alignment;
			}
			set
			{
				if (_alignment != value)
				{
					_alignment = value;
					this.OnAlignmentChanged(EventArgs.Empty);
					this.SetAlignment(value);
				}
			}
		}

		private static readonly object _alignmentChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Alignment"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_AlignSelector_AlignmentChanged")]
		public event EventHandler AlignmentChanged
		{
			add
			{
				this.Events.AddHandler(_alignmentChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_alignmentChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Shared.Controls.NuGenAlignSelector.AlignmentChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnAlignmentChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_alignmentChanged, e);
		}

		internal List<Switcher> Switchers
		{
			get
			{
				return _switchers;
			}
		}

		/// <summary>
		/// </summary>
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Return)
			{
				this.OnAlignmentAccepted(EventArgs.Empty);
			}
			else if (keyData == Keys.Escape)
			{
				this.OnAlignmentCanceled(EventArgs.Empty);
			}

			return base.ProcessDialogKey(keyData);
		}

		private void SetAlignment(ContentAlignment alignment)
		{
			foreach (Switcher switcher in _switchers)
			{
				if (switcher.AssociatedAlignment == alignment)
				{
					switcher.Checked = true;
					break;
				}
			}
		}

		private void _switcher_CheckedChanged(object sender, EventArgs e)
		{
			Switcher switcher = (Switcher)sender;

			if (switcher.Checked)
			{
				this.Alignment = switcher.AssociatedAlignment;
			}
		}

		private TableLayoutPanel _layoutPanel;
		private List<Switcher> _switchers;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenAlignSelector"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		///		<para><see cref="INuGenButtonStateService"/></para>
		///		<para><see cref="INuGenControlStateService"/></para>	
		///		<para><see cref="INuGenRadioButtonLayoutManager"/></para>
		///		<para><see cref="INuGenRadioButtonRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenAlignSelector(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.BackColor = Color.Transparent;

			_switchers = new List<Switcher>();

			_layoutPanel = new TableLayoutPanel();
			_layoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

			for (int i = 0; i < 3; i++)
			{
				_layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
				_layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
			}

			_layoutPanel.Dock = DockStyle.Fill;
			_layoutPanel.Parent = this;

			ContentAlignment[] alignStyleCollection = NuGenEnum.ToArray<ContentAlignment>();

			int column = 0;
			int row = 0;

			foreach (ContentAlignment alignStyle in alignStyleCollection)
			{
				Switcher switcher = new Switcher(serviceProvider, alignStyle);
				_switchers.Add(switcher);
				switcher.CheckedChanged += _switcher_CheckedChanged;
				_layoutPanel.Controls.Add(switcher, column, row);

				column++;

				if (column == 3)
				{
					column = 0;
					row++;
				}
			}

			this.SetAlignment(ContentAlignment.MiddleCenter);
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_switchers != null)
				{
					foreach (Switcher switcher in _switchers)
					{
						switcher.CheckedChanged -= _switcher_CheckedChanged;
					}

					_switchers.Clear();
					_switchers = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
