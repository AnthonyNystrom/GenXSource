/* -----------------------------------------------
 * NuGenOrientationControlDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the controls that support horizontal and
	/// vertical layout.
	/// </summary>
	public class NuGenOrientationControlDesigner : ControlDesigner
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
					_actionLists.Add(new NuGenOrientationControlActionList(
						(NuGenOrientationControlBase)this.Component)
					);
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
			events.Remove("Load");
			base.PostFilterEvents(events);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenOrientationControlDesigner"/> class.
		/// </summary>
		public NuGenOrientationControlDesigner()
		{
		}
	}
}
