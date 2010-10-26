/* -----------------------------------------------
 * NuGenLabelDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Design;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenLabel"/>.
	/// </summary>
	public class NuGenLabelDesigner : ControlDesigner
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
				if (_label != null && _label.AutoSize)
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
			_label = component as NuGenLabel;
			base.Initialize(component);
		}

		/// <summary>
		/// Receives a call when the control that the designer is managing has painted
		/// its surface so the designer can paint any additional adornments on top of
		/// the control.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
		protected override void OnPaintAdornments(PaintEventArgs e)
		{
			if (_label != null && !_label.AutoSize)
			{
				NuGenDesignerRenderer.DrawAdornments(e.Graphics, _label.ClientRectangle);
			}
		}

		/// <summary>
		/// Allows a designer to change or remove items from the set of events that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="events">The events for the class of the component.</param>
		protected override void PostFilterEvents(IDictionary events)
		{
			events.Remove("AutoValidateChanged");
			events.Remove("CausesValidationChanged");
			events.Remove("Scroll");
			events.Remove("Validated");
			events.Remove("Validating");

			base.PostFilterEvents(events);
		}

		/// <summary>
		/// Allows a designer to change or remove items from the set of properties that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="properties">The properties for the class of the component.</param>
		protected override void PostFilterProperties(IDictionary properties)
		{
			properties.Remove("AutoScroll");
			properties.Remove("AutoScrollMargin");
			properties.Remove("AutoScrollMinSize");
			properties.Remove("AutoScrollMinSize");
			properties.Remove("AutoValidate");
			properties.Remove("CausesValidation");
			
			base.PostFilterProperties(properties);
		}

		private NuGenLabel _label;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLabelDesigner"/> class.
		/// </summary>
		public NuGenLabelDesigner()
		{
		}
	}
}
