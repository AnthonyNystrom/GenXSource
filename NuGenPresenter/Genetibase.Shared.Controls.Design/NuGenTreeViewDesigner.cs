/* -----------------------------------------------
 * NuGenTreeViewDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenTreeView"/>.
	/// </summary>
	public class NuGenTreeViewDesigner : ControlDesigner
	{
		private DesignerActionListCollection _actionLists;

		/// <summary>
		/// Gets the design-time action lists supported by the component associated with the designer.
		/// </summary>
		/// <value></value>
		/// <returns>The design-time action lists supported by the component associated with the designer.</returns>
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (_actionLists == null)
				{
					_actionLists = new DesignerActionListCollection();
					_actionLists.Add(new NuGenTreeViewActionList(this));
				}

				return _actionLists;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeViewDesigner"/> class.
		/// </summary>
		public NuGenTreeViewDesigner()
		{

		}

	}
}
