/* -----------------------------------------------
 * NuGenPanelDesigner.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Design;

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenPanel"/>.
	/// </summary>
	public class NuGenPanelDesigner : ParentControlDesigner
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
					_actionLists.Add(new NuGenPanelActionList((ScrollableControl)base.Component));
				}

				return _actionLists;
			}
		}

		/// <summary>
		/// Initializes the designer with the specified component.
		/// </summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent"></see> to associate with the designer.</param>
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			_panel = (ScrollableControl)component;
		}

		/// <summary>
		/// Raises the paint adornments event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
		protected override void OnPaintAdornments(PaintEventArgs e)
		{
			NuGenDesignerRenderer.DrawAdornments(e.Graphics, _panel.ClientRectangle);
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
			events.Remove("Load");
			events.Remove("Scroll");
			events.Remove("TabIndexChanged");
			events.Remove("TabStopChanged");
			events.Remove("Validating");
			events.Remove("Validated");

			base.PostFilterEvents(events);
		}

		/// <summary>
		/// Allows a designer to change or remove items from the set of properties that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="properties">The properties for the class of the component.</param>
		protected override void PostFilterProperties(IDictionary properties)
		{
			properties.Remove("AutoSize");
			properties.Remove("AutoSizeMode");
			properties.Remove("AutoValidate");
			properties.Remove("CausesValidation");
			properties.Remove("Font");
			properties.Remove("ForeColor");
			properties.Remove("TabIndex");
			properties.Remove("TabStop");

			base.PostFilterProperties(properties);
		}

		private ScrollableControl _panel;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPanelDesigner"/> class.
		/// </summary>
		public NuGenPanelDesigner()
		{
		}
	}
}
