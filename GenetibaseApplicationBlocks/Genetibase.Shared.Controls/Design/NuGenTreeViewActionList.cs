/* -----------------------------------------------
 * NuGenTreeViewActionList.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Properties;
using Genetibase.Shared.Design;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// </summary>
	public class NuGenTreeViewActionList : DesignerActionList
	{
		#region Declarations.Fields

		private NuGenTreeViewDesigner _designer;

		#endregion

		#region Properties.Public

		/*
		 * ImageList
		 */

		/// <summary>
		/// </summary>
		public ImageList ImageList
		{
			get
			{
				return ((TreeView)base.Component).ImageList;
			}
			set
			{
				TypeDescriptor.GetProperties(base.Component)["ImageList"].SetValue(base.Component, value);
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * InvokeNodesDialog
		 */

		/// <summary>
		/// </summary>
		public void InvokeNodesDialog()
		{
			NuGenCollectionEditorServiceContext.EditValue(_designer, base.Component, "Nodes");
		}

		#endregion

		#region Methods.Public.Overridden

		/// <summary>
		/// Returns the collection of <see cref="T:System.ComponentModel.Design.DesignerActionItem"></see> objects contained in the list.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.ComponentModel.Design.DesignerActionItem"></see> array that contains the items in this list.
		/// </returns>
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection collection = new DesignerActionItemCollection();
			
			collection.Add(new DesignerActionMethodItem(
				this,
				"InvokeNodesDialog",
				Resources.ActionList_TreeView_EditNodes,
				Resources.Category_Properties,
				Resources.Description_TreeView_EditNodes,
				true)
			);
			
			collection.Add(new DesignerActionPropertyItem(
				"ImageList",
				Resources.ActionList_TreeView_ImageList,
				Resources.Category_Properties,
				Resources.Description_TreeView_ImageList)
			);

			return collection;

		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeViewActionList"/> class.
		/// </summary>
		public NuGenTreeViewActionList(NuGenTreeViewDesigner designer)
			: base(designer.Component)
		{
			_designer = designer;
		}

		#endregion
	}
}
