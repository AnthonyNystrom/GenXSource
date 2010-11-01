/* -----------------------------------------------
 * NuGenTreeNodeCollectionEditor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// </summary>
	internal sealed partial class NuGenTreeNodeCollectionEditor : CollectionEditor
	{
		#region Methods.Protected.Overridden

		/*
		 * CreatCollectionForm
		 */

		/// <summary>
		/// Creates a new form to display and edit the current collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.ComponentModel.Design.CollectionEditor.CollectionForm"></see> to provide as the user interface for editing the collection.
		/// </returns>
		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			return new EditorForm(this);
		}

		/*
		 * CreateCollectionItemType
		 */

		/// <summary>
		/// Gets the data type that this collection contains.
		/// </summary>
		/// <returns>
		/// The data type of the items in the collection, or an <see cref="T:System.Object"></see> if no Item property can be located on the collection.
		/// </returns>
		protected override Type CreateCollectionItemType()
		{
			return typeof(NuGenTreeNode);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeNodeCollectionEditor"/> class.
		/// </summary>
		public NuGenTreeNodeCollectionEditor()
			: base(typeof(TreeNodeCollection))
		{
		}

		#endregion
	}
}
