/* -----------------------------------------------
 * NuGenTabPage.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls.Design;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Controls
{
	/// <summary>
	/// </summary>
	[Designer(typeof(NuGenTabPageDesigner))]
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenTabPage : UserControl
	{
		#region Properties.Public

		/*
		 * TabButtonImage
		 */

		private Image _tabButtonImage = null;

		/// <summary>
		/// Gets or sets the <see cref="Image"/> that the associated <see cref="NuGenTabButton"/> displays.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		public Image TabButtonImage
		{
			get
			{
				return _tabButtonImage;
			}
			set
			{
				if (_tabButtonImage != value)
				{
					_tabButtonImage = value;
					this.OnTabButtonImageChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _tabButtonImageChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="TabButtonImage"/> property changes.
		/// </summary>
		public event EventHandler TabButtonImageChanged
		{
			add
			{
				this.Events.AddHandler(_tabButtonImageChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tabButtonImageChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TabButtonImageChanged"/> event.
 		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnTabButtonImageChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_tabButtonImageChanged, e);
		}

		#endregion

		#region Properties.Public.Overriden

		/*
		 * Text
		 */

		/// <summary>
		/// </summary>
		/// <value></value>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		/// <summary>
		/// Occurs when the tab page text changes.
		/// </summary>
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		#endregion

		#region Properties.Protected

		/*
		 * TabRenderer
		 */

		private INuGenTabRenderer _tabRenderer = null;

		/// <summary>
		/// </summary>
		protected INuGenTabRenderer TabRenderer
		{
			get
			{
				if (_tabRenderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_tabRenderer = this.ServiceProvider.GetService<INuGenTabRenderer>();

					if (_tabRenderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTabRenderer>();
					}
				}

				return _tabRenderer;
			}
		}

		#endregion

		#region Properties.Protected.Overriden

		/*
		 * DefaultSize
		 */

		private Size _defaultSize = Size.Empty;

		/// <summary>
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				if (_defaultSize == Size.Empty)
				{
					_defaultSize = new Size(250, 250);
				}

				return _defaultSize;
			}
		}

		#endregion
		
		#region Properties.Protected.Virtual

		/*
		 * Initiator
		 */

		private INuGenEventInitiatorService _initiator = null;

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
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			this.TabRenderer.DrawTabPage(new NuGenTabPagePaintParams(e.Graphics, this.ClientRectangle));
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabPage"/> class.
		/// </summary>
		public NuGenTabPage()
			: this(new NuGenTabControlServiceProvider())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabPage"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenTabRenderer"/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenTabPage(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;

			this.Dock = DockStyle.Fill;
			this.DoubleBuffered = true;
			this.Padding = new Padding(0, 1, 0, 0);
		}

		#endregion
	}
}
