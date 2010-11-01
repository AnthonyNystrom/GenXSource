/* -----------------------------------------------
 * NuGenResolutionSpin.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using System;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.ImageExport
{
	/// <summary>
	/// <see cref="T:NumericUpDown"/> control with tweaked <see cref="P:Maximum"/> and <see cref="P:Minimum"/>
	/// properties.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	internal class NuGenResolutionSpin : NumericUpDown
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenResolutionSpin"/> class.
		/// </summary>
		public NuGenResolutionSpin()
		{
			this.Minimum = 0;
			this.Maximum = int.MaxValue;
			this.TextAlign = HorizontalAlignment.Right;
		}

		#endregion
	}
}
