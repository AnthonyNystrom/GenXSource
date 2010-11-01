/* -----------------------------------------------
 * NuGenPictureBoxDesigner.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Drawing;

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenPictureBox"/>.
	/// </summary>
	public class NuGenPictureBoxDesigner : ControlDesigner
	{
		#region Declarations.Fields

		private NuGenPictureBox _pictureBox = null;

		#endregion

		#region Properties.Public.Overridden

		/*
		 * ActionLists
		 */

		private DesignerActionListCollection _actionLists = null;

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
					_actionLists.Add(new NuGenPictureBoxActionList(this));
				}

				return _actionLists;
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
			if (component is NuGenPictureBox)
			{
				_pictureBox = (NuGenPictureBox)component;
			}

			base.Initialize(component);
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnPaintAdornments
		 */

		/// <summary>
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
		protected override void OnPaintAdornments(PaintEventArgs e)
		{
			if (_pictureBox != null)
			{
				using (Pen pen = new Pen(SystemColors.ControlDarkDark))
				{
					pen.DashStyle = DashStyle.Dash;
					e.Graphics.DrawRectangle(
						pen,
						NuGenControlPaint.BorderRectangle(new Rectangle(0, 0, _pictureBox.Width, _pictureBox.Height))
					);
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPictureBoxDesigner"/> class.
		/// </summary>
		private NuGenPictureBoxDesigner()
		{
		}

		#endregion
	}
}
