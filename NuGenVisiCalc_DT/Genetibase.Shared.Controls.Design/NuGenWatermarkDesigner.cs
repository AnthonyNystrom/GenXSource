/* -----------------------------------------------
 * NuGenWatermarkDesigner.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenWatermark"/>.
	/// </summary>
	public class NuGenWatermarkDesigner : NuGenPanelDesigner
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
					_actionLists = base.ActionLists;

					if (_actionLists == null)
					{
						_actionLists = new DesignerActionListCollection();
					}

					_actionLists.Add(new NuGenWatermarkActionList((NuGenWatermark)base.Component));
				}

				return _actionLists;
			}
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
			events.Remove("FontChanged");
			events.Remove("ForeColorChanged");
			events.Remove("Scroll");
			events.Remove("TabIndexChanged");
			events.Remove("TabStopChanged");
			events.Remove("Validated");
			events.Remove("Validating");
			events.Remove("Load");

			base.PostFilterEvents(events);
		}

		/// <summary>
		/// Allows a designer to change or remove items from the set of properties that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="properties">The properties for the class of the component.</param>
		protected override void PostFilterProperties(IDictionary properties)
		{
			properties.Remove("AutoValidate");
			properties.Remove("AutoScrollMargin");
			properties.Remove("AutoScrollMinSize");
			properties.Remove("AutoSize");
			properties.Remove("AutoSizeMode");
			properties.Remove("CausesValidation");
			properties.Remove("Font");
			properties.Remove("ForeColor");
			properties.Remove("TabIndex");
			properties.Remove("TabStop");

			base.PostFilterProperties(properties);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWatermarkDesigner"/> class.
		/// </summary>
		public NuGenWatermarkDesigner()
		{
		}
	}
}
