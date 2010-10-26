/* -----------------------------------------------
 * NuGenMinibarDesigner.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.WinApi;

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
	/// Provides additional design-time functionality for the <see cref="NuGenMiniBar"/>.
	/// </summary>
	public class NuGenMinibarDesigner : ControlDesigner
	{
		#region Properties.Public.Overridden

		/// <summary>
		/// Gets the collection of components associated with the component managed by the designer.
		/// </summary>
		/// <value></value>
		/// <returns>The components that are associated with the component managed by the designer.</returns>
		public override ICollection AssociatedComponents
		{
			get
			{
				return _owner.Buttons;
			}
		}

		#endregion

		#region Properties.Protected.Overridden

		/// <summary>
		/// Gets the design-time verbs supported by the component that is associated with the designer.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.ComponentModel.Design.DesignerVerbCollection"></see> of <see cref="T:System.ComponentModel.Design.DesignerVerb"></see> objects, or null if no designer verbs are available. This default implementation always returns null.</returns>
		public override DesignerVerbCollection Verbs
		{
			get
			{
				return new DesignerVerbCollection(new DesignerVerb[]
					{
						new DesignerVerb("Add Button",new EventHandler(AddButton)),
						new DesignerVerb("Add Space",new EventHandler(AddSpace)),
						new DesignerVerb("Add Label",new EventHandler(AddLabel)),
						new DesignerVerb("Remove All",new EventHandler(RemoveAll))
					}
				);
			}
		}

		#endregion

		#region Properties.Private

		/// <summary>
		/// Gibt die aktuelle Mausposition in Owner-Clientkoordinaten zurück
		/// </summary>
		private Point OwnerMousePosition
		{
			get
			{
				return _owner.PointToClient(Control.MousePosition);
			}
		}
		/// <summary>
		/// Ruft die Markierung im Windows-Forms-Designer ab oder lagt sie fest
		/// </summary>
		private object PrimarySelection
		{
			get
			{
				ISelectionService sel = (ISelectionService)this.GetService(typeof(ISelectionService));
				return sel.PrimarySelection;
			}
			set
			{
				coll.Clear();
				if (value != null)
				{
					coll.Add(value);
					ISelectionService sel = (ISelectionService)this.GetService(typeof(ISelectionService));
					sel.SetSelectedComponents(coll, SelectionTypes.Replace);
					_owner.Refresh();
				}
			}
		}

		#endregion

		#region Methods.Public.Overridden

		/// <summary>
		/// Initializes the designer with the specified component.
		/// </summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent"></see> to associate the designer with. This component must always be an instance of, or derive from, <see cref="T:System.Windows.Forms.Control"></see>.</param>
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			_owner = component as NuGenMiniBar;
			//Installiert ein Ereignis, wenn die Markierung im Designer geänder wird
			ISelectionService ss = (ISelectionService)this.GetService(typeof(ISelectionService));
			ss.SelectionChanged += new EventHandler(ss_SelectionChanged);
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Indicates whether a mouse click at the specified point should be handled by the control.
		/// </summary>
		/// <param name="point">A <see cref="T:System.Drawing.Point"></see> indicating the position at which the mouse was clicked, in screen coordinates.</param>
		/// <returns>
		/// true if a click at the specified point is to be handled by the control; otherwise, false.
		/// </returns>
		protected override bool GetHitTest(Point point)
		{
			Point pt = _owner.PointToClient(point);
			return _owner.GetHit(pt) && pt.X < _owner.InternalBarWidth;
		}

		/// <summary>
		/// Receives a call when the control that the designer is managing has painted its surface so the designer can paint any additional adornments on top of the control.
		/// </summary>
		/// <param name="pe">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> the designer can use to draw on the control.</param>
		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			object sel = PrimarySelection;
			foreach (NuGenMiniBarControl ctl in _owner.Buttons)
			{
				pe.Graphics.DrawRectangle(Pens.Silver, ctl.ClientRectangle);
				if (ctl == sel)
					DrawSelectionFrame(pe.Graphics, ctl.ClientRectangle);
			}
		}

		/// <summary>
		/// Allows a designer to change or remove items from the set of events that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="events">The events for the class of the component.</param>
		protected override void PostFilterEvents(IDictionary events)
		{
			events.Remove("AutoSizeChanged");
			events.Remove("AutoValidateChanged");
			events.Remove("CausesValidationChanged");
			events.Remove("Scroll");
			events.Remove("Validated");
			events.Remove("Validating");

			base.PostFilterEvents(events);
		}

		/// <summary>
		/// Allows a designer to change or remove items from the set of properties that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="properties">The properties for the class of the component.</param>
		protected override void PostFilterProperties(IDictionary properties)
		{
			properties.Remove("AutoScroll");
			properties.Remove("AutoScrollMargin");
			properties.Remove("AutoScrollMinSizse");
			properties.Remove("AutoSize");
			properties.Remove("AutoSizeMode");
			properties.Remove("AutoValidate");
			properties.Remove("CausesValidate");

			base.PostFilterProperties(properties);
		}

		/// <summary>
		/// Processes Windows messages and optionally routes them to the control.
		/// </summary>
		/// <param name="m">The <see cref="T:System.Windows.Forms.Message"></see> to process.</param>
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WinUser.WM_LBUTTONDOWN)
			{
				int index = _owner.FindButtonIndex(this.OwnerMousePosition);
				if (index != -1)
				{
					this.PrimarySelection = _owner.Buttons[index];
					m.Result = new IntPtr(0);
				}
			}

			base.WndProc(ref m);
		}

		#endregion

		#region Methods.Private

		private void DrawSelectionFrame(Graphics gr, Rectangle bounds)
		{
			ControlPaint.DrawSelectionFrame(
				gr, false, bounds,
				Rectangle.Inflate(bounds, -3, -3), _owner.BackColor);
		}

		#endregion

		#region EventHandlers

		private void ss_SelectionChanged(object sender, EventArgs e)
		{
			_owner.Refresh();
		}

		#endregion

		#region Verbs

		private void AddButton(object sender, EventArgs e)
		{
			AddItem(typeof(NuGenMiniBarButton));
		}

		private void AddSpace(object sender, EventArgs e)
		{
			AddItem(typeof(NuGenMiniBarSpace));
		}

		private void AddLabel(object sender, EventArgs e)
		{
			AddItem(typeof(NuGenMiniBarLabel));
		}

		private void AddItem(Type ty)
		{
			//Designer vorbereiten
			IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
			IComponentChangeService chan = (IComponentChangeService)GetService(typeof(IComponentChangeService));
			//Rückgängig-Aktion anzeigen
			DesignerTransaction t = host.CreateTransaction("Add Item");
			using (t)
			{
				//Komponente wird geändert
				chan.OnComponentChanging(_owner, null);
				//neue Komponente erstellen
				IComponent com = host.CreateComponent(ty);
				//der Entwurfszeitinstanz hinzufügen
				_owner.Buttons.Add((NuGenMiniBarControl)com);
				//Änderungen anzeigen
				chan.OnComponentChanged(_owner, null, null, null);
				t.Commit();
			}
		}

		private void RemoveAll(object sender, EventArgs e)
		{
			//Designer vorbereiten
			IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
			IComponentChangeService chan = (IComponentChangeService)GetService(typeof(IComponentChangeService));
			//Rückgängig-Aktion anzeigen
			DesignerTransaction t = host.CreateTransaction("Remove All");
			using (t)
			{
				//Komponente wird geändert
				chan.OnComponentChanging(_owner, null);
				//Button-Komponenten zerstören
				NuGenMiniBarControl ctl;
				while (_owner.Buttons.Count > 0)
				{
					ctl = _owner.Buttons[0];
					host.DestroyComponent(ctl);
					//nicht vergessen, sonst droht ewiger loop!
					_owner.Buttons.Remove(ctl);
				}
				/*
				//Entwurfszeitinstanz bereinigen
				//geht jetzt über den destruktor von NuGenMiniBarControlDesigner
				_owner.Buttons.Clear();*/

				//Änderungen anzeigen
				chan.OnComponentChanged(_owner, null, null, null);
				t.Commit();
			}
		}

		#endregion

		private static ArrayList coll = new ArrayList();
		private NuGenMiniBar _owner;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMinibarDesigner"/> class.
		/// </summary>
		public NuGenMinibarDesigner()
		{
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to dispose both managed and unmanaged resources; <see langword="false"/> to dispose only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				//beim zerstören des designers wurde die toolbar gelöscht,
				//es müssen alle NuGenMiniBar-Komponenten einzeln zerstört werden
				IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
				NuGenMiniBarControl ctl;
				while (_owner.Buttons.Count > 0)
				{
					ctl = _owner.Buttons[0];
					host.DestroyComponent(ctl);
					//nicht vergessen, sonst droht ewiger loop!
					_owner.Buttons.Remove(ctl);
				}
				//Deinstalliert das Ereignis für  Markierungsänderung
				ISelectionService ss = (ISelectionService)GetService(typeof(ISelectionService));
				if (ss != null)
				{
					ss.SelectionChanged -= new EventHandler(ss_SelectionChanged);
				}
			}

			base.Dispose(disposing);
		}
	}
}
