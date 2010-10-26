/* -----------------------------------------------
 * NuGenPropertyGrid.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.PropertyGridInternals;
using Genetibase.Shared.Reflection;
using Genetibase.Shared.Windows;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// <seealso cref="PropertyGrid"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenPropertyGrid), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenPropertyGrid : PropertyGrid
	{
		#region Properties.Services

		/*
		 * Renderer
		 */

		private INuGenPropertyGridRenderer _renderer;

		/// <summary>
		/// </summary>
		protected INuGenPropertyGridRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenPropertyGridRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenPropertyGridRenderer>();
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
			this.StateTracker.Enabled(this.Enabled);
			base.OnEnabledChanged(e);
		}

		#endregion

		#region EventHandlers

		private void doccomment_Paint(object sender, PaintEventArgs e)
		{
			Debug.Assert(this.Renderer != null, "this.Renderer != null");

			NuGenPaintParams paintParams = new NuGenPaintParams(e.Graphics);
			paintParams.Bounds = _doccomment.Properties["ClientRectangle"].GetValue<Rectangle>();
			paintParams.State = this.StateTracker.GetControlState();

			this.Renderer.DrawDocComment(paintParams);
		}

		#endregion

		private NuGenFieldInfo _doccomment;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPropertyGrid"/> class.
		/// </summary>
		public NuGenPropertyGrid()
			: this(NuGenServiceManager.PropertyGridServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPropertyGrid"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Provides:<para/>
		/// <see cref="INuGenControlStateService"/><para/>
		/// <see cref="INuGenPropertyGridRenderer"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenPropertyGrid(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;

			this.ToolStripRenderer = new ToolStripProfessionalRenderer(new NuGenVisualStudioColorTable());

			NuGenInvoker invoker = new NuGenInvoker(this);

			_doccomment = invoker.Fields["doccomment"];
			_doccomment.Events["Paint"].AddHandler(new PaintEventHandler(this.doccomment_Paint));
			_doccomment.Fields["m_labelDesc"].Properties["BackColor"].SetValue(Color.Transparent);
			_doccomment.Fields["m_labelTitle"].Properties["BackColor"].SetValue(Color.Transparent);
		}
	}
}
