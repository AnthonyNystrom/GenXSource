/* -----------------------------------------------
 * NuGenDirectorySelectorDesigner.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Design;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenDirectorySelector"/>.
	/// </summary>
	public class NuGenDirectorySelectorDesigner : ControlDesigner
	{
		/// <summary>
		/// Receives a call when the control that the designer is managing has painted
		/// its surface so the designer can paint any additional adornments on top of
		/// the control.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
		protected override void OnPaintAdornments(PaintEventArgs e)
		{
			Control ctrl = this.Control;

			if (ctrl != null)
			{
				NuGenDesignerRenderer.DrawAdornments(e.Graphics, ctrl.ClientRectangle);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDirectorySelectorDesigner"/> class.
		/// </summary>
		public NuGenDirectorySelectorDesigner()
		{
		}
	}
}
