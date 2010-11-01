/* -----------------------------------------------
 * NuGenCTLabelDesigner.cs
 * Author: Alex Nesterov
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenCTLabel"/>.
	/// </summary>
	class NuGenCTLabelDesigner : ControlDesigner
	{
		#region Methods.Protected.Overriden

		/// <summary>
		/// Allows a designer to change or remove items from the set
		/// of events that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"/>.
		/// </summary>
		/// <param name="events">The events for the class of the component.</param>
		protected override void PostFilterEvents(IDictionary events)
		{
			events.Remove("BackColorChanged");
			events.Remove("FontChanged");
			events.Remove("ForeColorChanged");
			events.Remove("Load");
			events.Remove("RightToLeftChanged");
			events.Remove("Validated");
			events.Remove("Validating");
			base.PostFilterEvents(events);
		}
		
		/// <summary>
		/// Allows a designer to change or remove items from the set
		/// of properties that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"/>.
		/// </summary>
		/// <param name="properties">The properties for the class of the component.</param>
		protected override void PostFilterProperties(IDictionary properties)
		{
			properties.Remove("BackColor");
			properties.Remove("BackgroundImage");
			properties.Remove("CausesValidation");
			properties.Remove("Font");
			properties.Remove("ForeColor");
			properties.Remove("RightToLeft");
			base.PostFilterProperties(properties);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCTLabelDesigner"/> class.
		/// </summary>
		public NuGenCTLabelDesigner()
		{
		}

		#endregion
	}
}
