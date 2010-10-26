/* -----------------------------------------------
 * NuGenRadioButtonDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Design;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenRadioButton"/>.
	/// </summary>
	public class NuGenRadioButtonDesigner : ControlDesigner
	{
		/// <summary>
		/// Gets the selection rules that indicate the movement capabilities of a component.
		/// </summary>
		/// <value></value>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.Design.SelectionRules"></see> values.</returns>
		public override SelectionRules SelectionRules
		{
			get
			{
				if (_radioButton != null && _radioButton.AutoSize)
				{
					return base.SelectionRules & ~SelectionRules.AllSizeable;
				}

				return base.SelectionRules;
			}
		}

		/// <summary>
		/// Initializes the designer with the specified component.
		/// </summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent"></see> to associate the designer with. This component must always be an instance of, or derive from, <see cref="T:System.Windows.Forms.Control"></see>.</param>
		public override void Initialize(IComponent component)
		{
			_radioButton = component as NuGenRadioButton;
			base.Initialize(component);
		}

		/// <summary>
		/// Receives a call when the control that the designer is managing has painted its surface so the designer can paint any additional adornments on top of the control.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> the designer can use to draw on the control.</param>
		protected override void OnPaintAdornments(PaintEventArgs e)
		{
			if (_radioButton != null && !_radioButton.AutoSize)
			{
				NuGenDesignerRenderer.DrawAdornments(e.Graphics, _radioButton.ClientRectangle);
			}
		}

		private NuGenRadioButton _radioButton;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRadioButtonDesigner"/> class.
		/// </summary>
		public NuGenRadioButtonDesigner()
		{
		}
	}
}
