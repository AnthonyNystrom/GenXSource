/* -----------------------------------------------
 * NuGenGenericBaseDesigner.cs
 * Copyright � 2006 Alex Nesterov
 * --------------------------------------------- */

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.UI.NuGenMeters.Design
{
	/// <summary>
	/// Defines a designer for the <c>Genetibase.UI.NuGenMeters.NuGenGenericBase</c> class.
	/// </summary>
	public class NuGenGenericBaseDesigner : ControlDesigner
	{
		#region Declarations

		private NuGenGenericBase nuGenMeter = null;
		private DesignerVerb resetVerb = null;

		#endregion

		#region Properties.Protected.Overriden

		/// <summary>
		/// Allows a designer to change or remove items from the set
		/// of events that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"/>.
		/// </summary>
		/// <param name="events">The events for the class of the component.</param>
		protected override void PostFilterEvents(IDictionary events)
		{
			events.Remove("BackColorChanged");
			events.Remove("FontChanged");
			events.Remove("ForeColorChanged");
			events.Remove("Load");
			events.Remove("RightToLeftChanged");
			events.Remove("Validated");
			events.Remove("Validating");

			base.PostFilterEvents(events);
		}

		/// <summary>
		/// Allows a designer to change or remove items from the set
		/// of properties that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"/>.
		/// </summary>
		/// <param name="properties">The properties for the class of the component.</param>
		protected override void PostFilterProperties(IDictionary properties)
		{
			properties.Remove("BackColor");
			properties.Remove("Font");
			properties.Remove("ForeColor");
			properties.Remove("RightToLeft");

			base.PostFilterProperties(properties);
		}

		#endregion

		#region Properties.Public.Overriden

		/// <summary>
		/// Gets the design-time verbs supported by the component that is associated with the designer.
		/// </summary>
		public override DesignerVerbCollection Verbs
		{
			get
			{
				return new DesignerVerbCollection(new DesignerVerb[] { this.resetVerb });
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Initializes the designer with the specified component.
		/// </summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent"/> to associate the designer with. This component must always be an instance of, or derive from, <see cref="T:System.Windows.Forms.Control"/> .</param>
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.nuGenMeter = (NuGenGenericBase)component;
		}

		#endregion

		#region EventHandlers

		/// <summary>
		/// Handles the Click event of the Reset verb.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		private void Reset_Click(object sender, EventArgs e)
		{
			this.nuGenMeter.Reset();
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenGenericBaseDesigner"/> class.
		/// </summary>
		public NuGenGenericBaseDesigner()
		{
			this.resetVerb = new DesignerVerb("Reset", new EventHandler(this.Reset_Click));
		}

		#endregion
	}
}
