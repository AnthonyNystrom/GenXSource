/* -----------------------------------------------
 * NuGenNavigationPane.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.NavigationBarInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	[Designer("Genetibase.Shared.Controls.Design.NuGenNavigationPaneDesigner")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenNavigationPane : NuGenControl
	{
		/*
		 * NavigationButtonImage
		 */

		private Image _navigationButtonImage;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_NavigationBar_NavigationButtonImage")]
		public Image NavigationButtonImage
		{
			get
			{
				return _navigationButtonImage;
			}
			set
			{
				if (_navigationButtonImage != value)
				{
					_navigationButtonImage = value;
					this.OnNavigationButtonImageChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _navigationButtonImageChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="NavigationButtonImage"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_NavigationBar_NavigationButtonImageChanged")]
		public event EventHandler NavigationButtonImageChanged
		{
			add
			{
				this.Events.AddHandler(_navigationButtonImageChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_navigationButtonImageChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenNavigationPane.NavigationButtonImageChanged"/> event.
		/// </summary>
		protected virtual void OnNavigationButtonImageChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_navigationButtonImageChanged, e);
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
		[NuGenSRCategory("Category_PropertyChanged")]
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
		 * Renderer
		 */

		private INuGenNavigationBarRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenNavigationBarRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenNavigationBarRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenNavigationBarRenderer>();
					}
				}

				return _renderer;
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
			NuGenPaintParams paintParams = new NuGenPaintParams(e.Graphics);
			paintParams.Bounds = this.ClientRectangle;
			paintParams.State = this.StateTracker.GetControlState();
			this.Renderer.DrawBackground(paintParams);
			this.Renderer.DrawNavigationPaneBorder(paintParams);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenNavigationPane"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// <para><see cref="INuGenControlStateTracker"/></para>
		/// <para><see cref="INuGenNavigationBarRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenNavigationPane(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.Dock = DockStyle.Fill;
		}
	}
}
