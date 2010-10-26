/* -----------------------------------------------
 * NuGenOptionSpin.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Properties;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// <seealso cref="DomainUpDown"/>
	/// </summary>
	[ToolboxItem(false)]
	[DefaultEvent("SelectedIndexChanged")]
	[DefaultProperty("Items")]
	[NuGenSRDescription("Description_OptionSpin")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenOptionSpin : NuGenSpinBase
	{
		/*
		 * Items
		 */

		private List<string> _items;

		/// <summary>
		/// Gets or sets a collection of assigned options.
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("Genetibase.Shared.Design.NuGenStringCollectionEditor", typeof(UITypeEditor))]
		[NuGenSRCategory("Category_Data")]
		[NuGenSRDescription("Description_Spin_Items")]
		public List<string> Items
		{
			get
			{
				if (_items == null)
				{
					_items = new List<string>();
				}

				return _items;
			}
		}

		/*
		 * SelectedIndex
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"/>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectedIndex
		{
			get
			{
				return this.Items.IndexOf(this.Text);
			}
			set
			{
				this.Text = this.Items[value];
			}
		}

		/*
		 * SelectedItem
		 */

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SelectedItem
		{
			get
			{
				int itemIndex = this.SelectedIndex;

				if (itemIndex >= 0)
				{
					return this.Items[itemIndex];
				}

				return null;
			}
			set
			{
				int itemIndex = this.Items.IndexOf(value);

				if (itemIndex >= 0)
				{
					this.Text = this.Items[itemIndex];
				}
			}
		}

		private static readonly object _selectedItemChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SelectedItem"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_OptionSpin_SelectedItemChanged")]
		public event EventHandler SelectedItemChanged
		{
			add
			{
				this.Events.AddHandler(_selectedItemChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_selectedItemChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenOptionSpin.SelectedItemChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSelectedItemChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_selectedItemChanged, e);
		}

		/*
		 * Text
		 */

		/// <summary>
		/// Gets or sets the text associated with this <see cref="NuGenOptionSpin"/>.
		/// </summary>
		/// <value></value>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				this.EditBox.Text = value;
			}
		}

		/*
		 * OnDownButtonClick
		 */

		/// <summary>
		/// Displays the next option in the <see cref="Items"/> list.
		/// </summary>
		public override void OnDownButtonClick()
		{
			if (_currentItem < this.Items.Count - 1)
			{
				this.Text = this.Items[++_currentItem];
			}
		}

		/*
		 * OnUpButtonClick
		 */

		/// <summary>
		/// Displays the previous option in the <see cref="Items"/> list.
		/// </summary>
		public override void OnUpButtonClick()
		{
			if (_currentItem > 0)
			{
				this.Text = this.Items[--_currentItem];
			}
		}

		/*
		 * OnEditBoxTextChanged
		 */

		/// <summary>
		/// </summary>
		public override void OnEditBoxTextChanged()
		{
			this.OnSelectedItemChanged(EventArgs.Empty);
		}

		private int _currentItem = -1;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenOptionSpin"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenSpinRenderer"/><para/>
		/// <see cref="INuGenButtonStateTracker"/><para/>
		/// <see cref="INuGenControlStateTracker"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenOptionSpin(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
