/* -----------------------------------------------
 * NuGenLinkLabelDesigner.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenLinkLabel"/>.
	/// </summary>
	public class NuGenLinkLabelDesigner : NuGenLabelDesigner
	{
		private DesignerActionListCollection _actionLists;

		/// <summary>
		/// Gets the design-time action lists supported by the component associated with the designer.
		/// </summary>
		/// <value></value>
		/// <returns>The design-time action lists supported by the component associated with the designer.</returns>
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (_actionLists == null)
				{
					_actionLists = new DesignerActionListCollection();
					_actionLists.Add(new NuGenLinkLabelActionList(_linkLabel));
				}

				return _actionLists;
			}
		}

		/// <summary>
		/// Initializes the designer with the specified component.
		/// </summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent"></see> to associate the designer with. This component must always be an instance of, or derive from, <see cref="T:System.Windows.Forms.Control"></see>.</param>
		public override void Initialize(IComponent component)
		{
			_linkLabel = (NuGenLinkLabel)component;
			base.Initialize(component);
		}

		/// <summary>
		/// Allows a designer to change or remove items from the set of events that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="events">The events for the class of the component.</param>
		protected override void PostFilterEvents(IDictionary events)
		{
			events.Remove("AutoValidateChanged");
			events.Remove("CausesValidationChanged");
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
			properties.Remove("AutoScrollMinSize");
			properties.Remove("AutoScrollMargin");
			properties.Remove("AutoValidate");
			properties.Remove("CausesValidation");
			properties.Remove("Cursor");
			properties.Remove("ForeColor");
			properties.Remove("Validated");
			properties.Remove("Validating");

			base.PostFilterProperties(properties);
		}

		private NuGenLinkLabel _linkLabel;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLinkLabelDesigner"/> class.
		/// </summary>
		public NuGenLinkLabelDesigner()
		{
		}
	}
}
