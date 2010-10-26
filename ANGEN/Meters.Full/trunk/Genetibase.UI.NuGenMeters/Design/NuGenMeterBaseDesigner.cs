/* -----------------------------------------------
 * NuGenMeterBaseDesigner.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.UI.NuGenMeters.Design
{
	/// <summary>
    /// Defines a designer for the <c>Genetibase.UI.NuGenMeters.NuGenMeterBase</c> class.
	/// </summary>
	public class NuGenMeterBaseDesigner : NuGenGenericBaseDesigner
	{
		/// <summary>
		/// Allows a designer to change or remove items from the set
		/// of events that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"/>.
		/// </summary>
		/// <param name="events">The events for the class of the component.</param>
		protected override void PostFilterEvents(IDictionary events)
		{
			events.Remove("CategoryNameChanged");
			events.Remove("CounterFormatChanged");
			events.Remove("CounterNameChanged");

			base.PostFilterEvents(events);
		}

		/// <summary>
		/// Allows a designer to change or remove items from the set
		/// of properties that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"/>.
		/// </summary>
		/// <param name="properties">The properties for the class of the component.</param>
		protected override void PostFilterProperties(IDictionary properties)
		{
			properties.Remove("CategoryName");
			properties.Remove("CounterFormat");
			properties.Remove("CounterName");

			base.PostFilterProperties(properties);
		}
	}
}
