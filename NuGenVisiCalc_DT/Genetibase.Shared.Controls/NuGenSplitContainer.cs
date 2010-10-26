/* -----------------------------------------------
 * NuGenSplitContainer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.SplitContainerInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// <seealso cref="SplitContainer"/>
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSplitContainer : SplitContainer
	{
		#region Properties.Appearance

		/*
		 * DrawBorder
		 */

		private bool _drawBorder = true;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_SplitContainer_DrawBorder")]
		public bool DrawBorder
		{
			get
			{
				return _drawBorder;
			}
			set
			{
				if (_drawBorder != value)
				{
					_drawBorder = value;
					this.OnDrawBorderChanged(EventArgs.Empty);
					this.Invalidate(true);
				}
			}
		}

		private static readonly object _drawBorderChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="DrawBorder"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_SplitContainer_DrawBorderChanged")]
		public event EventHandler DrawBorderChanged
		{
			add
			{
				this.Events.AddHandler(_drawBorderChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_drawBorderChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="DrawBorderChanged"/> event.
		/// </summary>
		protected virtual void OnDrawBorderChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_drawBorderChanged, e);
		}

		#endregion

		#region Properties.Hidden

		/*
		 * BorderStyle
		 */

		/// <summary>
		/// Do not use this property. Any value will not affect the appearance.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new BorderStyle BorderStyle
		{
			get
			{
				return base.BorderStyle;
			}
			set
			{
				base.BorderStyle = value;
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * Initiator
		 */

		private INuGenEventInitiatorService _initiator;

		/// <summary>
		/// </summary>
		protected virtual INuGenEventInitiatorService Initiator
		{
			get
			{
				if (_initiator == null)
				{
					_initiator = new NuGenEventInitiatorService(this, this.Events);
				}

				return _initiator;
			}
		}

		/*
		 * Renderer
		 */

		private INuGenSplitContainerRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenSplitContainerRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenSplitContainerRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenSplitContainerRenderer>();
					}
				}

				return _renderer;
			}
		}

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider = null;

		/// <summary>
		/// </summary>
		protected INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		/*
		 * StateTracker
		 */

		private INuGenControlStateTracker _stateTracker;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenControlStateTracker StateTracker
		{
			get
			{
				if (_stateTracker == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					INuGenControlStateService stateService = this.ServiceProvider.GetService<INuGenControlStateService>();

					if (stateService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenControlStateService>();
					}

					_stateTracker = stateService.CreateStateTracker();
					Debug.Assert(_stateTracker != null, "_stateTracker != null");
				}

				return _stateTracker;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnEnabledChanged
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");
			this.StateTracker.Enabled(this.Enabled);
			base.OnEnabledChanged(e);
		}

		/*
		 * OnPaintBackground
		 */

		/// <summary>
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			Debug.Assert(this.Renderer != null, "this.Renderer != null");
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");

			NuGenPaintParams paintParams = new NuGenPaintParams(e.Graphics);
			paintParams.Bounds = this.ClientRectangle;
			paintParams.State = this.StateTracker.GetControlState();

			this.Renderer.DrawBackground(paintParams);
		}

		#endregion

		#region EventHandlers.Panels

		private void panel_Paint(object sender, PaintEventArgs e)
		{
			Debug.Assert(this.Renderer != null, "this.Renderer != null");
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");

			NuGenBorderPaintParams paintParams = new NuGenBorderPaintParams(e.Graphics);
			paintParams.Bounds = ((Control)sender).ClientRectangle;
			paintParams.DrawBorder = this.DrawBorder;
			paintParams.State = this.StateTracker.GetControlState();

			this.Renderer.DrawPanel(paintParams);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSplitContainer"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenSplitContainerRenderer"/><para/>
		/// <see cref="INuGenControlStateService"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenSplitContainer(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;

			ControlStyles panelStyles = 0
				| ControlStyles.AllPaintingInWmPaint
				| ControlStyles.Opaque
				| ControlStyles.OptimizedDoubleBuffer
				;

			NuGenControlPaint.SetStyle(this.Panel1, panelStyles, true);
			NuGenControlPaint.SetStyle(this.Panel2, panelStyles, true);

			this.Panel1.Paint += this.panel_Paint;
			this.Panel2.Paint += this.panel_Paint;
		}

		#endregion
	}
}
