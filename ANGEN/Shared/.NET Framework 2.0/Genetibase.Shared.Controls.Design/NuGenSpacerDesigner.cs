/* -----------------------------------------------
 * NuGenSpacerDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Design;

using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenSpacer"/>.
	/// </summary>
	public class NuGenSpacerDesigner : ControlDesigner
	{
		/// <summary>
		/// Initializes the designer with the specified component.
		/// </summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent"></see> to associate the designer with. This component must always be an instance of, or derive from, <see cref="T:System.Windows.Forms.Control"></see>.</param>
		public override void Initialize(System.ComponentModel.IComponent component)
		{
			_spacer = component as NuGenSpacer;
			base.Initialize(component);
		}

		/// <summary>
		/// Receives a call when the control that the designer is managing has painted its surface so the designer can paint any additional adornments on top of the control.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> the designer can use to draw on the control.</param>
		protected override void OnPaintAdornments(PaintEventArgs e)
		{
			if (_spacer != null)
			{
				NuGenDesignerRenderer.DrawAdornments(e.Graphics, _spacer.ClientRectangle);
			}
		}

		/// <summary>
		/// Allows a designer to change or remove items from the set of events that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="events">The events for the class of the component.</param>
		protected override void PostFilterEvents(IDictionary events)
		{
			events.Remove("CausesValidationChanged");
			events.Remove("ChangeUICues");
			events.Remove("FontChanged");
			events.Remove("ForeColorChanged");
			events.Remove("Scroll");
			events.Remove("TabIndexChanged");
			events.Remove("TabStopChanged");

			base.PostFilterEvents(events);
		}

		/// <summary>
		/// Allows a designer to change or remove items from the set of properties that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="properties">The properties for the class of the component.</param>
		protected override void PostFilterProperties(IDictionary properties)
		{
			properties.Remove("CausesValidation");
			properties.Remove("Font");
			properties.Remove("ForeColor");
			properties.Remove("TabIndex");
			properties.Remove("TabStop");
			properties.Remove("Text");

			base.PostFilterProperties(properties);
		}

		private NuGenSpacer _spacer;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSpacerDesigner"/> class.
		/// </summary>
		public NuGenSpacerDesigner()
		{
		}
	}
}
