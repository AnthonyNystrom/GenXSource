/* -----------------------------------------------
 * NuGenSmoothPictureBoxDesigner.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Design;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.SmoothControls.Design
{
	internal sealed class NuGenSmoothPictureBoxDesigner : ControlDesigner
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
					_actionLists.Add(
						new NuGenSmoothPictureBoxActionList((NuGenSmoothPictureBox)base.Component)
					);
				}

				return _actionLists;
			}
		}

		/// <summary>
		/// Initializes the designer with the specified component.
		/// </summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent"></see> to associate the designer with. This component must always be an instance of, or derive from, <see cref="T:System.Windows.Forms.Control"></see>.</param>
		public override void Initialize(IComponent component)
		{
			_pictureBox = (NuGenSmoothPictureBox)component;
			base.Initialize(component);
		}

		/// <summary>
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
		protected override void OnPaintAdornments(PaintEventArgs e)
		{
			NuGenDesignerRenderer.DrawAdornments(e.Graphics, new Rectangle(0, 0, _pictureBox.Width, _pictureBox.Height));
		}

		private NuGenSmoothPictureBox _pictureBox;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPictureBoxDesigner"/> class.
		/// </summary>
		public NuGenSmoothPictureBoxDesigner()
		{
		}
	}
}
