/* -----------------------------------------------
 * NuGenScrollButtonDesigner.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenScrollButton"/>.
	/// </summary>
	public class NuGenScrollButtonDesigner : ControlDesigner
	{
		/// <summary>
		/// Allows a designer to change or remove items from the set of events that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="events">The events for the class of the component.</param>
		protected override void PostFilterEvents(IDictionary events)
		{
			events.Remove("AutoSizeChanged");
			events.Remove("BindingContextChanged");
			events.Remove("CausesValidationChanged");
			events.Remove("TextChanged");
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
			properties.Remove("AutoEllipsis");
			properties.Remove("AutoSize");
			properties.Remove("AutoSizeMode");
			properties.Remove("CausesValidation");
			properties.Remove("DialogResult");
			properties.Remove("FlatAppearance");
			properties.Remove("Font");
			properties.Remove("ForeColor");
			properties.Remove("Image");
			properties.Remove("ImageAlign");
			properties.Remove("ImageIndex");
			properties.Remove("ImageKey");
			properties.Remove("ImageList");
			properties.Remove("Text");
			properties.Remove("TextAlign");
			properties.Remove("TextImageRelation");
			properties.Remove("UseMnemonic");
			properties.Remove("UseVisualStyleBackColor");

			base.PostFilterProperties(properties);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenScrollButtonDesigner"/> class.
		/// </summary>
		public NuGenScrollButtonDesigner()
		{
		}
	}
}
