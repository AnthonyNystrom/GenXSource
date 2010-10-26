/* -----------------------------------------------
 * NuGenMatrixLabelDesigner.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenMatrixLabel"/>.
	/// </summary>
	public class NuGenMatrixLabelDesigner : ControlDesigner 
	{
		/// <summary>
		/// Allows a designer to change or remove items from the set of events that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="events">The events for the class of the component.</param>
		protected override void PostFilterEvents(System.Collections.IDictionary events)
		{
			events.Remove("AutoValidateChanged");
			events.Remove("CausesValidationChanged");
			events.Remove("ChangeUICues");
			events.Remove("FontChanged");
			events.Remove("ForeColorChanged");
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
		protected override void PostFilterProperties(System.Collections.IDictionary properties)
		{
			properties.Remove("AutoScroll");
			properties.Remove("AutoScrollMargin");
			properties.Remove("AutoScrollMinSize");
			properties.Remove("AutoSizeMode");
			properties.Remove("AutoValidate");
			properties.Remove("CausesValidation");
			properties.Remove("Font");
			properties.Remove("ForeColor");

			base.PostFilterProperties(properties);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMatrixLabelDesigner"/> class.
		/// </summary>
		public NuGenMatrixLabelDesigner()
		{
		}
	}
}
