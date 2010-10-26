/* -----------------------------------------------
 * NuGenBlendSelectorDesigner.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenBlendSelectorDesigner"/>.
	/// </summary>
	public class NuGenBlendSelectorDesigner : ControlDesigner
	{
		/// <summary>
		/// Allows a designer to change or remove items from the set of events that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="events">The events for the class of the component.</param>
		protected override void PostFilterEvents(IDictionary events)
		{
			events.Remove("FontChanged");
			events.Remove("ForeColorChanged");
			events.Remove("TextChanged");
			base.PostFilterEvents(events);
		}

		/// <summary>
		/// Allows a designer to change or remove items from the set of properties that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="properties">The properties for the class of the component.</param>
		protected override void PostFilterProperties(IDictionary properties)
		{
			properties.Remove("Font");
			properties.Remove("ForeColor");
			properties.Remove("Text");

			base.PostFilterProperties(properties);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenBlendSelectorDesigner"/> class.
		/// </summary>
		public NuGenBlendSelectorDesigner()
		{
		}
	}
}
