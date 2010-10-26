/* -----------------------------------------------
 * NuGenPanelActionList.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.Design.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// </summary>
	public class NuGenPanelActionList : DesignerActionList
	{
		/// <summary>
		/// </summary>
		public DockStyle Dock
		{
			get
			{
				return this.PanelProperties["Dock"].GetValue<DockStyle>();
			}
			set
			{
				this.PanelProperties["Dock"].SetValue(value);
			}
		}

		private NuGenPropertyDescriptorCollection _panelProperties;

		private NuGenPropertyDescriptorCollection PanelProperties
		{
			get
			{
				if (_panelProperties == null)
				{
					_panelProperties = NuGenTypeDescriptor.Instance(_panel).Properties;
				}

				return _panelProperties;
			}
		}

		/// <summary>
		/// Returns the collection of <see cref="T:System.ComponentModel.Design.DesignerActionItem"></see> objects contained in the list.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.ComponentModel.Design.DesignerActionItem"></see> array that contains the items in this list.
		/// </returns>
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection items = new DesignerActionItemCollection();
			items.Add(new DesignerActionPropertyItem("Dock", Resources.ActionList_Panel_Dock));
			return items;
		}

		private ScrollableControl _panel;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPanelActionList"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="panel"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenPanelActionList(ScrollableControl panel)
			: base(panel)
		{
			if (panel == null)
			{
				throw new ArgumentNullException("panel");
			}

			_panel = panel;
		}
	}
}
