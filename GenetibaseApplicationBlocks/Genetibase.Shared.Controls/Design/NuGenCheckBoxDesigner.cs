/* -----------------------------------------------
 * NuGenCheckBoxDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional functionality for the <see cref="NuGenCheckBox"/>.
	/// </summary>
	public class NuGenCheckBoxDesigner : ControlDesigner
	{
		#region Declarations.Fields

		private NuGenCheckBox _checkBox = null;

		#endregion

		#region Properties.Public

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
				if (_checkBox != null)
				{
					if (_checkBox.AutoSize)
					{
						return base.SelectionRules & ~SelectionRules.AllSizeable;
					}
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
		public override void Initialize(IComponent component)
		{
			if (component is NuGenCheckBox)
			{
				_checkBox = (NuGenCheckBox)component;
			}

			base.Initialize(component);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCheckBoxDesigner"/> class.
		/// </summary>
		public NuGenCheckBoxDesigner()
		{

		}

		#endregion
	}
}
