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
		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			return new EditorForm(this);
		}

		protected override Type CreateCollectionItemType()
		{
			return typeof(NuGenTreeNode);
		}

		public NuGenTreeNodeCollectionEditor()
			: base(typeof(TreeNodeCollection))
		{
		}
	}
}
