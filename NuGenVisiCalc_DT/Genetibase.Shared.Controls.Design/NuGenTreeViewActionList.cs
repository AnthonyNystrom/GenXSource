/* -----------------------------------------------
 * NuGenTreeViewActionList.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using ctrlProps = Genetibase.Shared.Controls.Properties;
using designProps = Genetibase.Shared.Controls.Design.Properties;

using Genetibase.Shared.ComponentModel;
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
		/*
		 * ImageList
		 */

		/// <summary>
		/// </summary>
		public ImageList ImageList
		{
			get
			{
				return this.TreeViewProperties["ImageList"].GetValue<ImageList>();
			}
			set
			{
				this.TreeViewProperties["ImageList"].SetValue(value);
			}
		}

		private NuGenPropertyDescriptorCollection _treeViewProperties;

		private NuGenPropertyDescriptorCollection TreeViewProperties
		{
			get
			{
				if (_treeViewProperties == null)
				{
					_treeViewProperties = NuGenTypeDescriptor.Instance(base.Component).Properties;
				}

				return _treeViewProperties;
			}
		}

		/*
		 * InvokeButtonsDialog
		 */

		/// <summary>
		/// </summary>
		public void InvokeButtonsDialog()
		{
			NuGenCollectionEditorServiceContext.EditValue(_designer, base.Component, "Nodes");
		}

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
			DesignerActionItemCollection collection = new DesignerActionItemCollection();
			
			collection.Add(new DesignerActionMethodItem(
				this,
				"InvokeButtonsDialog",
				designProps.Resources.ActionList_TreeView_EditNodes,
				ctrlProps.Resources.Category_Properties,
				designProps.Resources.Description_TreeView_EditNodes,
				true)
			);
			
			collection.Add(new DesignerActionPropertyItem(
				"ImageList",
				designProps.Resources.ActionList_TreeView_ImageList,
				ctrlProps.Resources.Category_Properties,
				ctrlProps.Resources.Description_TreeView_ImageList)
			);

			return collection;
		}

		private NuGenTreeViewDesigner _designer;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeViewActionList"/> class.
		/// </summary>
		public NuGenTreeViewActionList(NuGenTreeViewDesigner designer)
			: base(designer.Component)
		{
			_designer = designer;
		}
	}
}
