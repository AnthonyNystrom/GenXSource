/* -----------------------------------------------
 * NuGenPinpointWindowDesigner.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Genetibase.Shared.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenPinpointWindow"/>.
	/// </summary>
	public class NuGenPinpointWindowDesigner : ControlDesigner
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
				if (_pinpointWindow != null && _pinpointWindow.AutoSize)
				{
					return base.SelectionRules & ~(SelectionRules.AllSizeable);
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
			_pinpointWindow = component as NuGenPinpointWindow;
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
			if (_pinpointWindow != null)
			{
				NuGenDesignerRenderer.DrawAdornments(e.Graphics, _pinpointWindow.ClientRectangle);
			}

			base.OnPaintAdornments(e);
		}

		/// <summary>
		/// Allows a designer to change or remove items from the set of events that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="events">The events for the class of the component.</param>
		protected override void PostFilterEvents(IDictionary events)
		{
			events.Remove("AutoSizeChanged");
			events.Remove("AutoValidateChanged");
			events.Remove("CausesValidationChanged");
			events.Remove("ChangeUICues");
			events.Remove("Load");
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
			properties.Remove("AutoSize");
			properties.Remove("AutoSizeMode");
			properties.Remove("AutoValidate");
			properties.Remove("CausesValidation");

			base.PostFilterProperties(properties);
		}

		private NuGenPinpointWindow _pinpointWindow;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPinpointWindowDesigner"/> class.
		/// </summary>
		public NuGenPinpointWindowDesigner()
		{
		}
	}
}
