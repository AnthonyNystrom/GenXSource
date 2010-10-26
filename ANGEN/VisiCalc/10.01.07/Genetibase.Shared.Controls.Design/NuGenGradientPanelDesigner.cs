/* -----------------------------------------------
 * NuGenGradientPanelDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenGradientPanel"/>.
	/// </summary>
	internal sealed class NuGenGradientPanelDesigner : NuGenWatermarkDesigner
	{
		private DesignerActionListCollection _actionLists;

		/// <summary>
		/// </summary>
		/// <value></value>
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

					_actionLists.Add(new NuGenGradientPanelActionList((NuGenGradientPanel)base.Component));
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
			events.Remove("Load");
			events.Remove("Scroll");
			events.Remove("TabIndexChanged");
			events.Remove("TabStopChanged");

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
			properties.Remove("Font");
			properties.Remove("ForeColor");
			properties.Remove("TabIndex");
			properties.Remove("TabStop");

			base.PostFilterProperties(properties);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenGradientPanelDesigner"/> class.
		/// </summary>
		public NuGenGradientPanelDesigner()
		{
		}
	}
}
