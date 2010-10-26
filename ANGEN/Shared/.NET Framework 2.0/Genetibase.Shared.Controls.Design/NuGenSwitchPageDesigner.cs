/* -----------------------------------------------
 * NuGenSwitchPageDesigner.cs
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
	/// Provides additional design-time functionality for the <see cref="NuGenSwitchPage"/>.
	/// </summary>
	public class NuGenSwitchPageDesigner : NuGenPageDesignerBase
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
				return parentDesigner.Component is NuGenSwitcher;
			}

			return false;
		}

		/// <summary>
		/// Adjusts the set of properties the component will expose through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="properties">An <see cref="T:System.Collections.IDictionary"></see> that contains the properties for the class of the component.</param>
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
		/// Initializes a new instance of the <see cref="NuGenSwitchPageDesigner"/> class.
		/// </summary>
		public NuGenSwitchPageDesigner()
		{
		}
	}
}
