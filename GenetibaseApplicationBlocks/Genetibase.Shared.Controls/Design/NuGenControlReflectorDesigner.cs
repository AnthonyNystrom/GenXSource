/* -----------------------------------------------
 * NuGenControlReflectorDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenControlReflector"/>.
	/// </summary>
	public class NuGenControlReflectorDesigner : ControlDesigner
	{
		#region Declarations.Fields

		private NuGenControlReflector _ctrlReflector;

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
			_ctrlReflector = component as NuGenControlReflector;
			base.Initialize(component);
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnPaintAdornments
		 */

		/// <summary>
		/// Receives a call when the control that the designer is managing has painted
		/// its surface so the designer can paint any additional adornments on top of
		/// the control.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
		protected override void OnPaintAdornments(PaintEventArgs e)
		{
			if (_ctrlReflector != null)
			{
				Graphics g = e.Graphics;

				using (Pen pen = new Pen(SystemColors.ControlDarkDark))
				{
					pen.DashStyle = DashStyle.Dash;
					e.Graphics.DrawRectangle(pen, NuGenControlPaint.BorderRectangle(_ctrlReflector.ClientRectangle));
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenControlReflectorDesigner"/> class.
		/// </summary>
		public NuGenControlReflectorDesigner()
		{

		}

		#endregion
	}
}
