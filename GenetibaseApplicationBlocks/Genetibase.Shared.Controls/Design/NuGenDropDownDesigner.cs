/* -----------------------------------------------
 * NuGenDropDownDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenDropDown"/>.
	/// </summary>
	public class NuGenDropDownDesigner : ControlDesigner
	{
		#region Declarations.Fields

		private NuGenDropDown _dropDown;

		#endregion

		#region Properties.Public.Overridden

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
				if (_dropDown != null && _dropDown.AutoSize)
				{
					return base.SelectionRules & ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
				}

				return base.SelectionRules;
			}
		}

		#endregion

		#region Methods.Public.Overridden

		/*
		 * Initialize
		 */

		/// <summary>
		/// Initializes the designer with the specified component.
		/// </summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent"></see> to associate the designer with. This component must always be an instance of, or derive from, <see cref="T:System.Windows.Forms.Control"></see>.</param>
		public override void Initialize(System.ComponentModel.IComponent component)
		{
			_dropDown = component as NuGenDropDown;
			base.Initialize(component);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDropDownDesigner"/> class.
		/// </summary>
		public NuGenDropDownDesigner()
		{

		}

		#endregion
	}
}
