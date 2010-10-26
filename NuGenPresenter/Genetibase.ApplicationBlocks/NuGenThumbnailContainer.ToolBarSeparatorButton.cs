/* -----------------------------------------------
 * NuGenThumbnailContainer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;	

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenThumbnailContainer
	{
		private sealed class ToolBarSeparatorButton : NuGenControl
		{
			private static readonly Size _defaultSize = new Size(6, 24);

			protected override Size DefaultSize
			{
				get
				{
					return _defaultSize;
				}
			}

			private INuGenThumbnailRenderer _renderer;

			private INuGenThumbnailRenderer Renderer
			{
				get
				{
					if (_renderer == null)
					{
						Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
						_renderer = this.ServiceProvider.GetService<INuGenThumbnailRenderer>();

						if (_renderer == null)
						{
							throw new NuGenServiceNotFoundException<INuGenThumbnailRenderer>();
						}
					}

					return _renderer;
				}
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				NuGenPaintParams paintParams = new NuGenPaintParams(e.Graphics);
				paintParams.Bounds = this.ClientRectangle;
				paintParams.State = this.StateTracker.GetControlState();
				this.Renderer.DrawToolBarSeparator(paintParams);
			}

			protected override void OnRightToLeftChanged(EventArgs e)
			{
				base.OnRightToLeftChanged(e);

				if (this.RightToLeft == RightToLeft.Yes)
				{
					this.Dock = DockStyle.Right;
				}
				else
				{
					this.Dock = DockStyle.Left;
				}
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="ToolBarSeparatorButton"/> class.
			/// </summary>
			/// <param name="serviceProvider"><para>Requires:</para>
			/// 	<para><see cref="INuGenControlStateService"/></para>
			///		<para><see cref="INuGenThumbnailRenderer"/></para>
			/// </param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			public ToolBarSeparatorButton(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this.Dock = DockStyle.Left;
			}
		}
	}
}
