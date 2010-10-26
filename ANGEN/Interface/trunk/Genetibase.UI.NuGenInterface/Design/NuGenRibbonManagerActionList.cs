/* -----------------------------------------------
 * NuGenRibbonManagerActionList.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.UI.NuGenInterface;
using Genetibase.UI.NuGenInterface.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.UI.NuGenInterface.Design
{
	/// <summary>
	/// Defines a list of items used to create a smart tag panel for the
	/// <see cref="NuGenRibbonManager"/>.
	/// </summary>
	internal class NuGenRibbonManagerActionList : DesignerActionList
	{
		#region Declarations

		private NuGenRibbonManagerDesigner _Designer = null;

		#endregion

		#region Properties.Public

		/*
		 * ColorScheme
		 */

		/// <summary>
		/// Gets or sets the <see cref="T:Genetibase.UI.NuGenInterface.NuGenColorScheme"/> to use.
		/// </summary>
		public NuGenColorScheme ColorScheme
		{
			get
			{
				Debug.Assert(base.Component is NuGenRibbonManager, "base.Component is NuGenRibbonManager");
				return ((NuGenRibbonManager)base.Component).ColorScheme;
			}
			set
			{
				Debug.Assert(base.Component is NuGenRibbonManager, "base.Component is NuGenRibbonManager");

				if (base.Component is NuGenRibbonManager)
				{
					NuGenRibbonManager target = (NuGenRibbonManager)base.Component;
					TypeDescriptor.GetProperties(target)["ColorScheme"].SetValue(target, value);
				}
			}
		}

		#endregion

		#region Methods.Public.Overriden

		/*
		 * GetSortedActionItems
		 */

		/// <summary>
		/// Returns the collection of <see cref="T:System.ComponentModel.Design.DesignerActionItem"></see> objects contained in the list.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.ComponentModel.Design.DesignerActionItem"></see> array that contains the items in this list.
		/// </returns>
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection actionItems = new DesignerActionItemCollection();

			actionItems.Add(new DesignerActionPropertyItem(
				"ColorScheme",
				Resources.ColorSchemeDisplayName,
				Resources.AppearanceCategory,
				Resources.ColorSchemeDescription)
				);

			return actionItems;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRibbonManagerActionList"/> class.
		/// </summary>
		/// <param name="designer">Specifies the <see cref="T:NuGenRibbonManagerDesigner"/> to create
		/// smart tags for.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="designer"/> is <see langword="null"/>.
		/// </exception>
		public NuGenRibbonManagerActionList(NuGenRibbonManagerDesigner designer)
			: base(designer.Component)
		{
			if (designer == null)
			{
				throw new ArgumentNullException("designer");
			}

			_Designer = designer;
		}

		#endregion
	}
}
