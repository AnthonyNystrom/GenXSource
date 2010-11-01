/* -----------------------------------------------
 * NuGenOptionSpin.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Design;
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
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenOptionSpin : NuGenSpinBase
	{
		#region Declarations.Fields

		private int _currentItem = -1;

		#endregion

		#region Properties.Data

		/*
		 * Items
		 */

		private List<string> _items;

		/// <summary>
		/// Gets or sets a collection of assigned options.
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor(typeof(NuGenStringCollectionEditor), typeof(UITypeEditor))]
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

		#endregion

		#region Properties.Public

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

		#endregion

		#region Methods.Public.Overridden

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
		}

		#endregion

		#region Constructors

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

		#endregion
	}
}
