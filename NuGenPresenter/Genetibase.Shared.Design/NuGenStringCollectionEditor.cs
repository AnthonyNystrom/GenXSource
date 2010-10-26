/* -----------------------------------------------
 * NuGenStringCollectionEditor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Text;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// Provides functionality to edit string collections during desing-time.
	/// </summary>
	public partial class NuGenStringCollectionEditor : CollectionEditor
	{
		/// <summary>
		/// Creates a new form to display and edit the current collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.ComponentModel.Design.CollectionEditor.CollectionForm"></see> to provide as the user interface for editing the collection.
		/// </returns>
		protected override CollectionForm CreateCollectionForm()
		{
			return new EditorForm(this);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenStringCollectionEditor"/> class.
		/// </summary>
		public NuGenStringCollectionEditor(Type targetCollectionType)
			: base(targetCollectionType)
		{
		}
	}
}
