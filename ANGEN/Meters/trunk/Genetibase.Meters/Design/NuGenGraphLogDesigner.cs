/* -----------------------------------------------
 * NuGenGraphLogDesigner.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Windows.Forms.Design;

namespace Genetibase.Meters.Design
{
	/// <summary>
    /// Defines a designer for <see cref="Genetibase.Meters.NuGenGraphLog"/> class.
	/// </summary>
	public class NuGenGraphLogDesigner : NuGenGraphGenericDesigner
	{
		/// <summary>
		/// Allows a designer to change or remove items from the set
		/// of events that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"/>.
		/// </summary>
		/// <param name="events">The events for the class of the component.</param>
		protected override void PostFilterEvents(IDictionary events)
		{
			events.Remove("CategoryNameChanged");
			events.Remove("CouunterNameChanged");
			events.Remove("InstanceNameChanged");
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
			properties.Remove("CounterName");
			properties.Remove("InstanceName");
			base.PostFilterProperties(properties);
		}
	}
}
