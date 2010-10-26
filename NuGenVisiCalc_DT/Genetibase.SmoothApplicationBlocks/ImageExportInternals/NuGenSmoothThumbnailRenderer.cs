/* -----------------------------------------------
 * NuGenSmoothThumbnailRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using res = Genetibase.SmoothApplicationBlocks.Properties.Resources;

using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Drawing;
using Genetibase.SmoothControls;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothApplicationBlocks.ImageExportInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothThumbnailRenderer : NuGenSmoothRenderer, INuGenThumbnailRenderer
	{
		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawBorder(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawBorder(
				paintParams.Graphics
				, NuGenControlPaint.BorderRectangle(paintParams.Bounds)
				, paintParams.State
			);
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawCWRotateButton(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			NuGenImagePaintParams imagePaintParams = new NuGenImagePaintParams(paintParams);

			switch (imagePaintParams.State)
			{
				case NuGenControlState.Hot:
				case NuGenControlState.Pressed:
				{
					imagePaintParams.Image = res.RotateCW_Hot;
					break;
				}
				default:
				{
					imagePaintParams.Image = res.RotateCW_Normal;
					break;
				}
			}

			this.DrawImage(imagePaintParams);
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawCCWRotateButton(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			NuGenImagePaintParams imagePaintParams = new NuGenImagePaintParams(paintParams);

			switch (imagePaintParams.State)
			{
				case NuGenControlState.Hot:
				case NuGenControlState.Pressed:
				{
					imagePaintParams.Image = res.RotateCCW_Hot;
					break;
				}
				default:
				{
					imagePaintParams.Image = res.RotateCCW_Normal;
					break;
				}
			}

			this.DrawImage(imagePaintParams);
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawToolBarSeparator(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Rectangle bounds = paintParams.Bounds;

			this.DrawLine(
				paintParams.Graphics
				, NuGenControlPaint.RectTCCorner(bounds)
				, NuGenControlPaint.RectBCCorner(bounds)
				, paintParams.State
			);
		}

		/// <summary>
		/// </summary>
		/// <param name="clientRectangle"></param>
		/// <returns></returns>
		public Font GetFont(Rectangle clientRectangle)
		{
			return new Font(
				"Verdana"
				, Math.Min(clientRectangle.Width * 0.20f, clientRectangle.Height * 0.20f)
				, FontStyle.Bold
			);
		}

		/// <summary>
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public Color GetForeColor(NuGenControlState state)
		{
			return Color.FromArgb(50, this.ColorManager.GetBorderColor(state));
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public Image GetGridModeImage()
		{
			return res.Grid_Mode;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public Image GetLoupeModeImage()
		{
			return res.Loupe_Mode;
		}

		/// <summary>
		/// </summary>
		public Image GetRotateCWImage()
		{
			return res.RotateCW;
		}

		/// <summary>
		/// </summary>
		public Image GetRotateCCWImage()
		{
			return res.RotateCCW;
		}

		/// <summary>
		/// </summary>
		public Image GetZoomInImage()
		{
			return res.ZoomIn;
		}

		/// <summary>
		/// </summary>
		public Image GetZoomOutImage()
		{
			return res.ZoomOut;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothThumbnailRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// 	<see cref="INuGenSmoothColorManager"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothThumbnailRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
