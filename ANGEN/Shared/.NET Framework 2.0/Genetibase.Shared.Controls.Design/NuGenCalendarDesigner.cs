/* -----------------------------------------------
 * NuGenCalendarDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenCalendar"/>.
	/// </summary>
	public class NuGenCalendarDesigner : ControlDesigner
	{
		private DesignerActionListCollection actionLists;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCalendarDesigner"/> class.
		/// </summary>
		public NuGenCalendarDesigner()
		{
		}

		/// <summary>
		/// Gets the design-time action lists supported by the component associated with the designer.
		/// </summary>
		/// <value></value>
		/// <returns>The design-time action lists supported by the component associated with the designer.</returns>
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (null == actionLists)
				{
					actionLists = new DesignerActionListCollection();
					actionLists.Add(
						new NuGenCalendarActionList(this.Component));
				}
				return actionLists;
			}
		}

		/// <summary>
		/// Gets the selection rules that indicate the movement capabilities of a component.
		/// </summary>
		/// <value></value>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.Design.SelectionRules"></see> values.</returns>
		public override SelectionRules SelectionRules
		{
			get
			{
				// Remove all manual resizing of the control
				SelectionRules selectionRules = base.SelectionRules;
				selectionRules = SelectionRules.Visible | SelectionRules.AllSizeable | SelectionRules.Moveable;
				return selectionRules;
			}
		}

		/// <summary>
		/// Allows a designer to change or remove items from the set of properties that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="properties">The properties for the class of the component.</param>
		protected override void PostFilterProperties(System.Collections.IDictionary properties)
		{
			properties.Remove("BackColor");
			properties.Remove("Font");
			properties.Remove("BackgroundImage");
			properties.Remove("ForeColor");
			properties.Remove("Text");
			properties.Remove("RightToLeft");
			properties.Remove("ImeMode");
			properties.Remove("Padding");
			properties.Remove("BackgroundImageLayout");

			base.PostFilterProperties(properties);
		}
	}
}
