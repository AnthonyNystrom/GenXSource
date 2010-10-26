/* -----------------------------------------------
 * NuGenTabPage.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.TabControlInternals;
using Genetibase.Shared;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	[Designer("Genetibase.Shared.Controls.Design.NuGenTabPageDesigner")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenTabPage : UserControl
	{
		/*
		 * TabButtonImage
		 */

		private Image _tabButtonImage;

		/// <summary>
		/// Gets or sets the <see cref="Image"/> that the associated <see cref="NuGenTabButton"/> displays.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_TabControl_TabButtonImage")]
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
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TabControl_TabButtonImageChanged")]
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
		 * TabRenderer
		 */

		private INuGenTabRenderer _tabRenderer;

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

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider;

		/// <summary>
		/// </summary>
		protected virtual INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

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

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabPage"/> class.
		/// </summary>
		public NuGenTabPage()
			: this(NuGenServiceManager.TabControlServiceProvider)
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

			Dock = DockStyle.Fill;
			DoubleBuffered = true;
			Padding = new Padding(0, 1, 0, 0);
		}
	}
}
