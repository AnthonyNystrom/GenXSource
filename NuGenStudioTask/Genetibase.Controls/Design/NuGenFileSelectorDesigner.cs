/* -----------------------------------------------
 * NuGenFileSelectorDesigner.cs
 * Author: Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Windows.Forms.Design;

namespace Genetibase.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenFileSelector"/>.
	/// </summary>
	class NuGenFileSelectorDesigner : ControlDesigner
	{
		#region Properties.Public.Overriden

		/// <summary>
		/// Gets the selection rules that indicate the movement capabilities of a component.
		/// </summary>
		/// <value></value>
		public override SelectionRules SelectionRules
		{
			get
			{
				return base.SelectionRules & ~(SelectionRules.BottomSizeable | SelectionRules.TopSizeable);
			}
		}

		#endregion

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFileSelectorDesigner"/> class.
		/// </summary>
		public NuGenFileSelectorDesigner()
		{
		}
		
		#endregion
	}
}
