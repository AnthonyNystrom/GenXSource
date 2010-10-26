/* -----------------------------------------------
 * NuGenMiniBarControlDesigner.cs
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
	/// </summary>
	public class NuGenMiniBarControlDesigner : ComponentDesigner
	{
		/// <summary>
		/// Gets the design-time verbs supported by the component that is associated with the designer.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.ComponentModel.Design.DesignerVerbCollection"></see> of <see cref="T:System.ComponentModel.Design.DesignerVerb"></see> objects, or null if no designer verbs are available. This default implementation always returns null.</returns>
		public override DesignerVerbCollection Verbs
		{
			get
			{
				DesignerVerbCollection rt = new DesignerVerbCollection(
					new DesignerVerb[]{
										  new DesignerVerb("Move Up",new EventHandler(MoveUp)),
										  new DesignerVerb("Move Down",new EventHandler(MoveDown)),
										  new DesignerVerb("Delete",new EventHandler(Delete))
									  }
					);
				return rt;
			}
		}

		/// <summary>
		/// Prepares the designer to view, edit, and design the specified component.
		/// </summary>
		/// <param name="component">The component for this designer.</param>
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			_ctl = component as NuGenMiniBarControl;
		}

		#region Methods.Verbs

		/// <summary>
		/// Ereignishandler. Verschiebt das aktuelle Element um eine Position nach links
		/// </summary>
		private void MoveUp(object sender, EventArgs e)
		{
			DoVerb(Actions.moveup);
		}
		
		/// <summary>
		/// Ereignishandler. Verschiebt das aktuelle Element um eine Position nach rechts
		/// </summary>
		private void MoveDown(object sender, EventArgs e)
		{
			DoVerb(Actions.movedown);
		}
		
		/// <summary>
		/// Ereignishandler. Löscht das aktuelle Element
		/// </summary>
		private void Delete(object sender, EventArgs e)
		{
			DoVerb(Actions.delete);
		}
		
		/// <summary>
		/// Führt die angegebene Operation aus
		/// </summary>
		/// <param name="ac">Die auszuführende Aktion</param>
		private void DoVerb(Actions ac)
		{
			//controls feststellen
			_owner = _ctl.Owner;
			if (_owner == null)
				return;
			//designer vorbereiten
			IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
			IComponentChangeService chan = (IComponentChangeService)GetService(typeof(IComponentChangeService));
			//rückgängig-aktion anzigen
			DesignerTransaction t = host.CreateTransaction("Item Op");
			//markierungs-service
			ISelectionService sel = (ISelectionService)this.GetService(typeof(ISelectionService));
			using (t)
			{
				//komponentenänderung einleiten
				chan.OnComponentChanging(_owner, null);
				switch (ac)
				{
					case Actions.moveup:
					{
						//control um eine position nach links verschieben
						_owner.Buttons.Move(_ctl, -1);
						break;
					}
					case Actions.movedown:
					{
						//control um eine position nach rechts verschieben
						_owner.Buttons.Move(_ctl, +1);
						break;
					}
					case Actions.delete:
					{
						/*
						//der entwurfszeitinstanz den button entreißen
						//geht jetzt über den destruktor
						_owner.Buttons.Remove(_ctl);*/

						//neue markierung setzen
						ArrayList arr = new ArrayList();
						arr.Add(_owner);
						sel.SetSelectedComponents(arr, SelectionTypes.Replace);
						//die komponente zerstören
						host.DestroyComponent(_ctl);
						break;
					}
					case Actions.updateparent:
					{
						break;
					}
				}
				//komponentenänderung anzeigen
				chan.OnComponentChanged(_owner, null, null, null);
				t.Commit();
			}
			_owner.Refresh();
		}

		#endregion

		private NuGenMiniBarControl _ctl;
		private NuGenMiniBar _owner;

		private enum Actions
		{
			moveup,
			movedown,
			delete,
			updateparent
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMiniBarControlDesigner"/> class.
		/// </summary>
		public NuGenMiniBarControlDesigner()
		{
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Design.ComponentDesigner"></see> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				//controls feststellen
				_owner = _ctl.Owner;
				if (_owner != null)
				{
					_owner.Buttons.Remove(_ctl);
				}
			}

			base.Dispose(disposing);
		}
	}
}
