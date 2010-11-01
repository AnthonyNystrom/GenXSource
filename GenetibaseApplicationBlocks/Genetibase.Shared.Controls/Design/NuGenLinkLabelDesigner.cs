/* -----------------------------------------------
 * NuGenLinkLabelDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
	public class NuGenLinkLabelDesigner : ControlDesigner
	{
		#region Declarations.Fields

		private NuGenLinkLabel _linkLabel;

		#endregion

		#region Properties.Public.Overridden

		/*
		 * ActionLists
		 */

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
					_actionLists.Add(new NuGenLinkLabelActionList((NuGenLinkLabel)this.Component));
				}

				return _actionLists;
			}
		}

		/*
		 * SelectionRules
		 */

		/// <summary>
		/// Gets the selection rules that indicate the movement capabilities of a component.
		/// </summary>
		/// <value></value>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.Design.SelectionRules"></see> values.</returns>
		public override SelectionRules SelectionRules
		{
			get
			{
				if (_linkLabel != null && _linkLabel.AutoSize)
				{
					return base.SelectionRules & ~SelectionRules.AllSizeable;
				}

				return base.SelectionRules;
			}
		}

		#endregion

		#region Methods.Public.Overridden

		/// <summary>
		/// Initializes the designer with the specified component.
		/// </summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent"></see> to associate the designer with. This component must always be an instance of, or derive from, <see cref="T:System.Windows.Forms.Control"></see>.</param>
		public override void Initialize(IComponent component)
		{
			_linkLabel = component as NuGenLinkLabel;
			base.Initialize(component);
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Adjusts the set of properties the component exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="properties">An <see cref="T:System.Collections.IDictionary"></see> containing the properties for the class of the component.</param>
		protected override void PreFilterProperties(IDictionary properties)
		{
			properties.Remove("Cursor");
			properties.Remove("ForeColor");
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLinkLabelDesigner"/> class.
		/// </summary>
		public NuGenLinkLabelDesigner()
		{

		}

		#endregion
	}
}
