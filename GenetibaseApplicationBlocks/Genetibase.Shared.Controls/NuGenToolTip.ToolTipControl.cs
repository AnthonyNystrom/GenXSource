/* -----------------------------------------------
 * NuGenToolTip.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ToolTipInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenToolTip
	{
		private sealed class ToolTipControl : NuGenLayeredWindow
		{
			#region Properties.Services

			/*
			 * LayoutManager
			 */

			private INuGenToolTipLayoutManager _layoutManager;

			private INuGenToolTipLayoutManager LayoutManager
			{
				get
				{
					if (_layoutManager == null)
					{
						Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
						_layoutManager = this.ServiceProvider.GetService<INuGenToolTipLayoutManager>();

						if (_layoutManager == null)
						{
							throw new NuGenServiceNotFoundException<INuGenToolTipLayoutManager>();
						}
					}

					return _layoutManager;
				}
			}

			/*
			 * Renderer
			 */

			private INuGenToolTipRenderer _renderer;

			private INuGenToolTipRenderer Renderer
			{
				get
				{
					if (_renderer == null)
					{
						Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
						_renderer = this.ServiceProvider.GetService<INuGenToolTipRenderer>();

						if (_renderer == null)
						{
							throw new NuGenServiceNotFoundException<INuGenToolTipRenderer>();
						}
					}

					return _renderer;
				}
			}

			/*
			 * ServiceProvider
			 */

			private INuGenServiceProvider _serviceProvider;

			private INuGenServiceProvider ServiceProvider
			{
				get
				{
					return _serviceProvider;
				}
			}

			#endregion

			#region Methods.Protected.Overridden

			/*
			 * OnPaint
			 */

			protected override void OnPaint(PaintEventArgs e)
			{
				Graphics g = e.Graphics;
				Size shadowSize = this.LayoutManager.GetShadowSize();

				Rectangle bodyBounds = new Rectangle(
					this.ClientRectangle.Left,
					this.ClientRectangle.Top,
					this.ClientRectangle.Width - shadowSize.Width,
					this.ClientRectangle.Height - shadowSize.Height
				);

				NuGenPaintParams bodyPaintParams = new NuGenPaintParams(this, g, bodyBounds, NuGenControlState.Normal);
				
				this.Renderer.DrawBackground(bodyPaintParams);
				this.Renderer.DrawBorder(bodyPaintParams);
			}

			#endregion

			#region Methods.Public

			public void Show(NuGenToolTipInfo tooltipInfo, Point location)
			{
				if (tooltipInfo == null)
				{
					throw new ArgumentNullException("tooltipInfo");
				}

				base.Show(location);
			}

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="ToolTipControl"/> class.
			/// </summary>
			/// <param name="serviceProvider">
			/// <para>Requires:</para>
			/// <para><see cref="INuGenToolTipRenderer"/></para>
			/// </param>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
			/// </exception>
			public ToolTipControl(INuGenServiceProvider serviceProvider)
			{
				if (serviceProvider == null)
				{
					throw new ArgumentNullException("serviceProvider");
				}

				_serviceProvider = serviceProvider;
			}

			#endregion
		}
	}
}
