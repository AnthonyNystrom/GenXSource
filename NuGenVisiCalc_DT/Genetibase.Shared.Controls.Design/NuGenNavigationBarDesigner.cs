/* -----------------------------------------------
 * NuGenNavigationBarDesigner.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenNavigationBar"/>.
	/// </summary>
	public class NuGenNavigationBarDesigner : NuGenPageHostDesignerBase
	{
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
			if (control is NuGenNavigationPane)
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
			base.Initialize(component);

			_navigationBar = component as NuGenNavigationBar;

			if (_navigationBar != null)
			{
				_navigationBar.SelectedButtonChanged += _navigationBar_SelectedButtonChanged;
			}

			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));

			if (selectionService != null)
			{
				selectionService.SelectionChanged += _selectionService_SelectionChanged;
			}
		}

		/*
		 * GetSelectedNavigationPaneDesigner
		 */

		/// <summary>
		/// </summary>
		protected NuGenNavigationPaneDesigner GetSelectedNavigationPaneDesigner()
		{
			NuGenNavigationPaneDesigner navigationPaneDesigner = null;
			NuGenNavigationPane selectedNavigationPane = _navigationBar.SelectedNavigationPane;

			if (selectedNavigationPane != null)
			{
				IDesignerHost host = (IDesignerHost)this.GetService(typeof(IDesignerHost));

				if (host != null)
				{
					navigationPaneDesigner = host.GetDesigner(selectedNavigationPane) as NuGenNavigationPaneDesigner;
				}
			}

			return navigationPaneDesigner;
		}

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
			switch (_navigationBar.HitTest(point))
			{
				case NuGenNavigationBarHitResult.Buttons:
				case NuGenNavigationBarHitResult.Grip:
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
			NuGenNavigationPaneDesigner designer = this.GetSelectedNavigationPaneDesigner();

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
			NuGenNavigationPaneDesigner designer = this.GetSelectedNavigationPaneDesigner();

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
			NuGenNavigationPaneDesigner designer = this.GetSelectedNavigationPaneDesigner();

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
			NuGenNavigationPaneDesigner designer = this.GetSelectedNavigationPaneDesigner();

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
			NuGenNavigationPaneDesigner designer = this.GetSelectedNavigationPaneDesigner();

			if (designer != null)
			{
				designer.DoGiveFeedback(e);
			}
			else
			{
				base.OnGiveFeedback(e);
			}
		}

		/*
		 * PostFilterEvents
		 */

		/// <summary>
		/// Allows a designer to add to the set of events that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="events">The events for the class of the component.</param>
		protected override void PostFilterEvents(IDictionary events)
		{
			events.Remove("AutoSizeChanged");
			events.Remove("CausesValidationChanged");
			events.Remove("Load");
			events.Remove("Scroll");

			base.PostFilterEvents(events);
		}

		/*
		 * PostFilterProperties
		 */

		/// <summary>
		/// Adjusts the set of properties the component will expose through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="properties">An <see cref="T:System.Collections.IDictionary"></see> that contains the properties for the class of the component.</param>
		protected override void PostFilterProperties(IDictionary properties)
		{
			properties.Remove("AutoSize");
			properties.Remove("AutoSizeMode");
			properties.Remove("AutoScroll");
			properties.Remove("AutoScrollMargin");
			properties.Remove("AutoScrollMinSize");
			properties.Remove("AutoValidate");
			properties.Remove("CausesValidation");

			base.PostFilterProperties(properties);
		}

		private static NuGenNavigationPane GetNavigationPaneOfComponent(object component)
		{
			if (!(component is Control))
			{
				return null;
			}

			Control parent = (Control)component;

			while (parent != null && !(parent is NuGenNavigationPane))
			{
				parent = parent.Parent;
			}

			return (NuGenNavigationPane)parent;
		}

		private void _navigationBar_SelectedButtonChanged(object sender, EventArgs e)
		{
			ISelectionService service = (ISelectionService)this.GetService(typeof(ISelectionService));

			if (service != null)
			{
				ICollection selectedComponents = service.GetSelectedComponents();
				bool flag = false;

				foreach (object selectedComponent in selectedComponents)
				{
					NuGenNavigationPane navigationPaneOfComponent = NuGenNavigationBarDesigner.GetNavigationPaneOfComponent(selectedComponent);

					if (
						navigationPaneOfComponent != null
						&& navigationPaneOfComponent.Parent == _navigationBar
						&& navigationPaneOfComponent == _navigationBar.SelectedNavigationPane
						)
					{
						flag = true;
						break;
					}
				}

				if (!flag)
				{
					service.SetSelectedComponents(new object[] { base.Component });
				}
			}
		}

		private void _selectionService_SelectionChanged(object sender, EventArgs e)
		{
			ISelectionService service = (ISelectionService)this.GetService(typeof(ISelectionService));

			if (service != null)
			{
				ICollection selectedComponents = service.GetSelectedComponents();

				foreach (object selectedComponent in selectedComponents)
				{
					NuGenNavigationPane navigationPaneOfComponent = NuGenNavigationBarDesigner.GetNavigationPaneOfComponent(selectedComponent);

					if (navigationPaneOfComponent != null && navigationPaneOfComponent.Parent == _navigationBar)
					{
						_navigationBar.SelectedNavigationPane = navigationPaneOfComponent;
						break;
					}
				}
			}
		}

		private NuGenNavigationBar _navigationBar;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenNavigationBarDesigner"/> class.
		/// </summary>
		public NuGenNavigationBarDesigner()
		{
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Design.ParentControlDesigner"></see>, and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_navigationBar != null)
				{
					_navigationBar.SelectedButtonChanged -= _navigationBar_SelectedButtonChanged;
				}

				ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));

				if (selectionService != null)
				{
					selectionService.SelectionChanged -= _selectionService_SelectionChanged;
				}
			}

			base.Dispose(disposing);
		}
	}
}
