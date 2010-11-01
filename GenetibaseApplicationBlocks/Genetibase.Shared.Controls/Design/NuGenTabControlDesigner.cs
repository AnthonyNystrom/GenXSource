/* -----------------------------------------------
 * NuGenTabControlDesigner.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenTabControl"/>.
	/// </summary>
	public class NuGenTabControlDesigner : ParentControlDesigner
	{
		#region Declarations.Fields

		private NuGenTabControl _tabControl = null;

		#endregion

		#region Properties.Protected.Overridden

		/*
		 * AllowControlLasso
		 */

		/// <summary>
		/// Gets a value indicating whether selected controls will be re-parented.
		/// </summary>
		/// <value></value>
		/// <returns>true if the controls that were selected by lassoing on the designer's surface will be re-parented to this designer's control.</returns>
		protected override bool AllowControlLasso
		{
			get
			{
				return false;
			}
		}

		#endregion

		#region Properties.Public.Overridden

		/*
		 * ActionLists
		 */

		/// <summary>
		/// Gets the design-time action lists supported by the component associated with the designer.
		/// </summary>
		/// <value></value>
		/// <returns>The design-time action lists supported by the component associated with the designer.</returns>
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection actionLists = new DesignerActionListCollection();

				Debug.Assert(_tabControl != null, "_tabControl != null");
				NuGenTabControlActionList actionList = new NuGenTabControlActionList(_tabControl);
				actionList.AutoShow = true;

				actionLists.Add(actionList);

				return actionLists;
			}
		}

		#endregion

		#region Methods.Public.Overridden

		/*
		 * CanParent
		 */

		/// <summary>
		/// Indicates whether the specified control can be a child of the control managed by this designer.
		/// </summary>
		/// <param name="control">The <see cref="T:System.Windows.Forms.Control"></see> to test.</param>
		/// <returns>
		/// true if the specified control can be a child of the control managed by this designer; otherwise, false.
		/// </returns>
		public override bool CanParent(Control control)
		{
			if (control is NuGenTabPage)
			{
				return !this.Control.Contains(control);
			}

			return false;
		}

		/*
		 * Initialize
		 */

		/// <summary>
		/// Initializes the designer with the specified component.
		/// </summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent"></see> to associate with the designer.</param>
		public override void Initialize(IComponent component)
		{
			Debug.Assert(component != null, "component != null");
			_tabControl = (NuGenTabControl)component;
			base.Initialize(component);
		}

		#endregion

		#region Methods.Protected

		/*
		 * GetSelectedTabPageDesigner
		 */

		/// <summary>
		/// </summary>
		/// <returns></returns>
		protected NuGenTabPageDesigner GetSelectedTabPageDesigner()
		{
			NuGenTabPageDesigner tabPageDesigner = null;
			NuGenTabPage selectedTabPage = _tabControl.SelectedTab;

			if (selectedTabPage != null)
			{
				IDesignerHost host = (IDesignerHost)this.GetService(typeof(IDesignerHost));

				if (host != null)
				{
					tabPageDesigner = host.GetDesigner(selectedTabPage) as NuGenTabPageDesigner;
				}
			}

			return tabPageDesigner;
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * GetHitTest
		 */

		/// <summary>
		/// Indicates whether a mouse click at the specified point should be handled by the control.
		/// </summary>
		/// <param name="point">A <see cref="T:System.Drawing.Point"></see> indicating the position at which the mouse was clicked, in screen coordinates.</param>
		/// <returns>
		/// true if a click at the specified point is to be handled by the control; otherwise, false.
		/// </returns>
		protected override bool GetHitTest(Point point)
		{
			switch (_tabControl.HitTest(point))
			{
				case NuGenTabControlHitResults.TabButtons:
				{
					return true;
				}
				default:
				{
					return false;
				}
			}
		}

		/*
		 * OnDragDrop
		 */

		/// <summary>
		/// Called when a drag-and-drop object is dropped onto the control designer view.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DragEventArgs"></see> that provides data for the event.</param>
		protected override void OnDragDrop(DragEventArgs e)
		{
			NuGenTabPageDesigner designer = this.GetSelectedTabPageDesigner();

			if (designer != null)
			{
				designer.DoDragDrop(e);
			}
			else
			{
				base.OnDragDrop(e);
			}
		}

		/*
		 * OnDragEnter
		 */

		/// <summary>
		/// Called when a drag-and-drop operation enters the control designer view.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DragEventArgs"></see> that provides data for the event.</param>
		protected override void OnDragEnter(DragEventArgs e)
		{
			NuGenTabPageDesigner designer = this.GetSelectedTabPageDesigner();

			if (designer != null)
			{
				designer.DoDragEnter(e);
			}
			else
			{
				base.OnDragEnter(e);
			}
		}

		/*
		 * OnDragLeave
		 */

		/// <summary>
		/// Called when a drag-and-drop operation leaves the control designer view.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that provides data for the event.</param>
		protected override void OnDragLeave(EventArgs e)
		{
			NuGenTabPageDesigner designer = this.GetSelectedTabPageDesigner();

			if (designer != null)
			{
				designer.DoDragLeave(e);
			}
			else
			{
				base.OnDragLeave(e);
			}
		}

		/*
		 * OnDragOver
		 */

		/// <summary>
		/// Called when a drag-and-drop object is dragged over the control designer view.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DragEventArgs"></see> that provides data for the event.</param>
		protected override void OnDragOver(DragEventArgs e)
		{
			NuGenTabPageDesigner designer = this.GetSelectedTabPageDesigner();

			if (designer != null)
			{
				designer.DoDragOver(e);
			}
			else
			{
				base.OnDragOver(e);
			}
		}

		/*
		 * OnGiveFeedback
		 */

		/// <summary>
		/// Receives a call when a drag-and-drop operation is in progress to provide visual cues based on the location of the mouse while a drag operation is in progress.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.GiveFeedbackEventArgs"></see> that provides data for the event.</param>
		protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
			NuGenTabPageDesigner designer = this.GetSelectedTabPageDesigner();

			if (designer != null)
			{
				designer.DoGiveFeedback(e);
			}
			else
			{
				base.OnGiveFeedback(e);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabControlDesigner"/> class.
		/// </summary>
		public NuGenTabControlDesigner()
		{
		}

		#endregion
	}
}
