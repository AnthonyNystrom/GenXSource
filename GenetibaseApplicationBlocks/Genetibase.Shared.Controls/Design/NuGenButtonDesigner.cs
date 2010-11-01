/* -----------------------------------------------
 * NuGenButtonDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenButton"/>.
	/// </summary>
	public class NuGenButtonDesigner : ControlDesigner
	{
		#region Methods.Protected.Overridden

		/*
		 * PreFilterProperties
		 */

		/// <summary>
		/// Adjusts the set of properties the component exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="properties">An <see cref="T:System.Collections.IDictionary"></see> containing the properties for the class of the component.</param>
		protected override void PreFilterProperties(IDictionary properties)
		{
			properties.Remove("UseCompatibleTextRendering");
			base.PreFilterProperties(properties);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenButtonDesigner"/> class.
		/// </summary>
		public NuGenButtonDesigner()
		{

		}

		#endregion
	}
}
