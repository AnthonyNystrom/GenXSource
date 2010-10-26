/* -----------------------------------------------
 * NuGenSwitchButtonDesigner.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Genetibase.Shared.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenSwitchButton"/>.
	/// </summary>
	public class NuGenSwitchButtonDesigner : ControlDesigner
	{
		/// <summary>
		/// Initializes the designer with the specified component.
		/// </summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent"></see> to associate the designer with. This component must always be an instance of, or derive from, <see cref="T:System.Windows.Forms.Control"></see>.</param>
		public override void Initialize(IComponent component)
		{
			_switchButton = component as NuGenSwitchButton;
			base.Initialize(component);
		}

		/// <summary>
		/// Receives a call when the control that the designer is managing has painted its surface so the designer can paint any additional adornments on top of the control.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> the designer can use to draw on the control.</param>
		protected override void OnPaintAdornments(PaintEventArgs e)
		{
			if (_switchButton != null)
			{
				NuGenDesignerRenderer.DrawAdornments(e.Graphics, _switchButton.ClientRectangle);
			}
		}

		private NuGenSwitchButton _switchButton;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSwitchButtonDesigner"/> class.
		/// </summary>
		public NuGenSwitchButtonDesigner()
		{
		}
	}
}
