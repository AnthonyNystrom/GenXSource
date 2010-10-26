/* -----------------------------------------------
 * NuGenBevelDesigner.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenBevel"/>.
	/// </summary>
	public class NuGenBevelDesigner : ControlDesigner
	{
		/// <summary>
		/// Allows a designer to change or remove items from the set of events that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="events">The events for the class of the component.</param>
		protected override void PostFilterEvents(IDictionary events)
		{
			events.Remove("AutoSizeChanged");
			events.Remove("AutoValidateChanged");
			events.Remove("BindingContextChanged");
			events.Remove("CausesValidationChanged");
			events.Remove("ChangeUICues");
			events.Remove("FontChanged");
			events.Remove("ForeColorChanged");
			events.Remove("Load");
			events.Remove("QueryAccessiblityHelp");
			events.Remove("QueryAccessiblityHelp");
			events.Remove("QueryContinueDrag");
			events.Remove("Scroll");
			events.Remove("TabIndexChanged");
			events.Remove("TabStopChanged");
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
			properties.Remove("AutoScrollMinSize");
			properties.Remove("AutoScrollMargin");
			properties.Remove("AutoSize");
			properties.Remove("AutoSizeMode");
			properties.Remove("AutoValidate");
			properties.Remove("CausesValidation");
			properties.Remove("TabIndex");
			properties.Remove("TabStop");

			base.PostFilterProperties(properties);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenBevelDesigner"/> class.
		/// </summary>
		public NuGenBevelDesigner()
		{
		}
	}
}
