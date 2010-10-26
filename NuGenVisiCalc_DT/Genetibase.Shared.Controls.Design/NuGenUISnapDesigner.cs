/* -----------------------------------------------
 * NuGenUISnapDesigner.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Design;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenUISnap"/>.
	/// </summary>
	public class NuGenUISnapDesigner : ComponentDesigner
	{
		/// <summary>
		/// Prepares the designer to view, edit, and design the specified component.
		/// </summary>
		/// <param name="component">The component for this designer.</param>
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			NuGenUISnap uiSnap = component as NuGenUISnap;

			if (uiSnap != null)
			{
				uiSnap.HostForm = NuGenDesignerUtils.GetHostForm((IDesignerHost)this.GetService(typeof(IDesignerHost)));
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenUISnapDesigner"/> class.
		/// </summary>
		public NuGenUISnapDesigner()
		{
		}
	}
}
