/* -----------------------------------------------
 * NuGenRibbonManagerDesigner.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenInterface.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenRibbonManager"/>.
	/// </summary>
	internal class NuGenRibbonManagerDesigner : ComponentDesigner
	{
		#region Declarations.Fields

		private NuGenRibbonManager _RibbonManager = null;
		private DesignerVerb _AddRibbonControlVerb = null;
		private IComponentChangeService _ComponentChangeService = null;

		#endregion

		#region Properties.Public.Overriden

		/*
		 * ActionLists
		 */

		private DesignerActionListCollection _ActionLists = null;

		/// <summary>
		/// Gets the design-time action lists supported by the component associated with the designer.
		/// </summary>
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (_ActionLists == null)
				{
					_ActionLists = new DesignerActionListCollection();
					_ActionLists.Add(new NuGenRibbonManagerActionList(this));
				}

				return _ActionLists;
			}
		}

		#endregion

		#region Methods.Public.Overriden

		/*
		 * Initialize
		 */

		/// <summary>
		/// </summary>
		public override void Initialize(IComponent component)
		{
			Debug.Assert(component != null, "component != null");
			base.Initialize(component);

			if (component == null)
			{
				return;
			}

			_RibbonManager = (NuGenRibbonManager)component;
			_RibbonManager.HostForm = this.GetHostForm();

			_ComponentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			Debug.Assert(_ComponentChangeService != null, "this.componentChangeService != null");

			if (_ComponentChangeService != null)
			{
				_ComponentChangeService.ComponentChanged += this.OnComponentChanged;
			}
		}

		/*
		 * InitializeNewComponent
		 */

		/// <summary>
		/// Initializes a newly created component.
		/// </summary>
		/// <param name="defaultValues">A name/value dictionary of default values to apply to properties. May be null if no default values are specified.</param>
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			this.AddRibbonControl();
		}

		/*
		 * Verbs
		 */

		private DesignerVerbCollection _Verbs = null;

		/// <summary>
		/// Gets the design-time verbs supported by the component that is associated with the designer.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// A <see cref="DesignerVerbCollection"/> of <see cref="DesignerVerb"/> objects, or null if no
		/// designer verbs are available. This default implementation always returns null.
		/// </returns>
		public override DesignerVerbCollection Verbs
		{
			get
			{
				if (_Verbs == null)
				{
					_Verbs = new DesignerVerbCollection();
					_AddRibbonControlVerb = new DesignerVerb("Add RibbonControl", this.OnAddRibbonControl);
					_Verbs.Add(_AddRibbonControlVerb);
				}

				_AddRibbonControlVerb.Enabled = this.ShouldAddNewRibbonControl();

				return _Verbs;
			}
		}

		#endregion

		#region Methods.Protected

		/*
		 * AddRibbonControl
		 */

		/// <summary>
		/// Adds a new <see cref="T:Genetibase.UI.NuGenInterface.NuGenRibbonControl"/> to the host form if
		/// it doesn't already contain another <see cref="T:Genetibase.UI.NuGenInterface.NuGenRibbonControl"/>
		/// instance.
		/// </summary>
		protected void AddRibbonControl()
		{
			IDesignerHost host = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			Debug.Assert(host != null, "host != null");

			if (host == null)
			{
				return;
			}

			Form hostForm = this.GetHostForm(host);
			Debug.Assert(hostForm != null, "hostForm != null");

			if (hostForm != null && this.ShouldAddNewRibbonControl(hostForm))
			{
				DesignerTransaction transaction = host.CreateTransaction("Add RibbonControl");
				NuGenRibbonControl ribbonControl = (NuGenRibbonControl)host.CreateComponent(
					typeof(NuGenRibbonControl)
				);

				hostForm.Controls.Add(ribbonControl);
				transaction.Commit();
			}
		}

		/*
		 * GetHostForm
		 */

		/// <summary>
		/// Retrieves the host <see cref="Form"/> for the associated
		/// <see cref="NuGenRibbonManager"/>.
		/// </summary>
		/// <returns></returns>
		protected Form GetHostForm()
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			Debug.Assert(designerHost != null, "designerHost != null");

			return this.GetHostForm(designerHost);
		}

		/// <summary>
		/// Retrieves host <see cref="Form"/> for the associated
		/// <see cref="NuGenRibbonManager"/>.
		/// </summary>
		/// <param name="designerHost">Specifies <see cref="IDesignerHost"/>
		/// to retrieve the host <see cref="Form"/> from.</param>
		protected Form GetHostForm(IDesignerHost designerHost)
		{
			if (designerHost == null)
			{
				throw new ArgumentNullException("designerHost");
			}

			return (Form)designerHost.RootComponent;
		}

		/*
		 * ShouldAddNewRibbonControl
		 */

		/// <summary>
		/// Gets the value indicating whether a new
		/// <see cref="T:Genetibase.UI.NuGenInterface.NuGenRibbonControl"/> may be added to the
		/// <paramref name="hostForm"/>.
		/// </summary>
		protected bool ShouldAddNewRibbonControl()
		{
			return this.ShouldAddNewRibbonControl(this.GetHostForm());
		}

		/// <summary>
		/// Gets the value indicating whether a new
		/// <see cref="NuGenRibbonControl"/> may be added to the
		/// <paramref name="hostForm"/>.
		/// </summary>
		/// <param name="hostForm">Specifies the <see cref="Form"/> to check.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="hostForm"/> is <see langword="null"/>.
		/// </exception>
		protected bool ShouldAddNewRibbonControl(Form hostForm)
		{
			if (hostForm == null)
			{
				throw new ArgumentNullException("hostForm");
			}

			foreach (Control ctrl in hostForm.Controls)
			{
				if (ctrl is NuGenRibbonControl)
				{
					return false;
				}
			}

			return true;
		}

		#endregion

		#region Methods.Protected.Overriden

		/*
		 * Dispose
		 */

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Design.ComponentDesigner"></see> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (_ComponentChangeService != null)
			{
				_ComponentChangeService.ComponentChanged -= this.OnComponentChanged;
			}
		}

		#endregion

		#region EventHandlers

		private void OnAddRibbonControl(object sender, EventArgs e)
		{
			this.AddRibbonControl();
		}

		private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (_AddRibbonControlVerb != null)
			{
				_AddRibbonControlVerb.Enabled = this.ShouldAddNewRibbonControl();
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRibbonManagerDesigner"/> class.
		/// </summary>
		public NuGenRibbonManagerDesigner()
		{

		}

		#endregion
	}
}
