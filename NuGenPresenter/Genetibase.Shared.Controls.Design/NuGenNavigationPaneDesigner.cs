/* -----------------------------------------------
 * NuGenNavigationPaneDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenNavigationPane"/>.
	/// </summary>
	public class NuGenNavigationPaneDesigner : NuGenPageDesignerBase
	{
		/// <summary>
		/// Indicates if this designer's control can be parented by the control of the specified designer.
		/// </summary>
		/// <param name="parentDesigner">The <see cref="T:System.ComponentModel.Design.IDesigner"></see> that manages the control to check.</param>
		/// <returns>
		/// true if the control managed by the specified designer can parent the control managed by this designer; otherwise, false.
		/// </returns>
		public override bool CanBeParentedTo(IDesigner parentDesigner)
		{
			if (parentDesigner != null)
			{
				return parentDesigner.Component is NuGenNavigationBar;
			}

			return false;
		}

		/// <summary>
		/// Allows a designer to change or remove items from the set of properties that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="properties">The properties for the class of the component.</param>
		protected override void PostFilterProperties(IDictionary properties)
		{
			properties.Remove("AutoSize");
			properties.Remove("AutoSizeMode");
			properties.Remove("Dock");
			properties.Remove("Location");
			properties.Remove("Size");

			base.PostFilterProperties(properties);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenNavigationPaneDesigner"/> class.
		/// </summary>
		public NuGenNavigationPaneDesigner()
		{
		}
	}
}
