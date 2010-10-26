/* -----------------------------------------------
 * NuGenReflectedImageGenerator.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.ControlReflectorInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenReflectedImageGenerator : INuGenReflectedImageGenerator
	{
		#region Declarations.Fields

		private static readonly PixelFormat _pixelFormat = PixelFormat.Format32bppArgb;

		#endregion

		#region INuGenImageGenerator.BuildReflectedImage

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="ctrlBmp"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void BuildReflectedImage(Bitmap ctrlBmp, NuGenReflectStyle reflectStyle, int opacity)
		{
			if (ctrlBmp == null)
			{
				throw new ArgumentNullException("ctrlBmp");
			}

			NuGenReflectedImageGenerator.RotateBitmap(ctrlBmp, reflectStyle);
			BitmapData ctrlBmpData = ctrlBmp.LockBits(
				new Rectangle(0, 0, ctrlBmp.Width, ctrlBmp.Height),
				ImageLockMode.ReadWrite,
				_pixelFormat
				);

			unsafe 
			{
				uint* scan0 = (uint*)ctrlBmpData.Scan0;
				int scanWidth = ctrlBmpData.Stride;
				Size ctrlBmpSize = ctrlBmp.Size;

				switch (reflectStyle)
				{
					case NuGenReflectStyle.Left:
					case NuGenReflectStyle.Right:
					{
						for (int y = 0; y < ctrlBmpData.Height; y++)
						{
							for (int x = 0; x < ctrlBmpData.Width; x++)
							{
								uint currentOpacity = NuGenReflectedImageGenerator.GetCurrentOpacity(ctrlBmpSize, x, reflectStyle, opacity);
								NuGenReflectedImageGenerator.ApplyOpacity(scan0, scanWidth, x, y, currentOpacity);
							}
						}

						break;
					}
					default:
					{
						for (int y = 0; y < ctrlBmpData.Height; y++)
						{
							uint currentOpacity = NuGenReflectedImageGenerator.GetCurrentOpacity(ctrlBmpSize, y, reflectStyle, opacity);

							for (int x = 0; x < ctrlBmpData.Width; x++)
							{
								NuGenReflectedImageGenerator.ApplyOpacity(scan0, scanWidth, x, y, currentOpacity);
							}
						}

						break;
					}
				}
			}

			ctrlBmp.UnlockBits(ctrlBmpData);
		}

		#endregion

		#region INuGenImageGenerator.GetControlImage

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="ctrl"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public Bitmap GetControlImage(Control ctrl)
		{
			if (ctrl == null)
			{
				throw new ArgumentNullException("ctrl");
			}

			Rectangle rect = NuGenReflectedImageGenerator.GetImageRectangle(ctrl);
			Bitmap ctrlBmp = new Bitmap(rect.Width, rect.Height, _pixelFormat);
			ctrl.DrawToBitmap(ctrlBmp, rect);
			return ctrlBmp;
		}

		#endregion

		#region Methods.Private

		/*
		 * ApplyOpacity
		 */

		private static unsafe void ApplyOpacity(uint* scan0, int scanWidth, int x, int y, uint currentOpacity)
		{
			int stride = scanWidth * y / 4;
			uint color = scan0[stride + x];
			uint alpha = (color >> 24) & 0xff;

			if (alpha == 0)
				return;

			color ^= (alpha << 24);
			alpha = (uint)(currentOpacity / 255.0 * alpha);
			color |= (alpha << 24);

			scan0[stride + x] = color;
		}

		/*
		 * GetCurrentOpacity
		 */

		private static uint GetCurrentOpacity(Size bmpSize, int iteration, NuGenReflectStyle reflectStyle, int maxOpacity)
		{
			return (uint)Math.Min(
				255,
				Math.Max(
					0,
					Math.Round(NuGenReflectedImageGenerator.GetRawOpacity(bmpSize, iteration, reflectStyle, maxOpacity))
				)
			);
		}

		/*
		 * GetImageRectangle
		 */

		private static Rectangle GetImageRectangle(Control ctrl)
		{
			Debug.Assert(ctrl != null, "ctrl != null");
			return new Rectangle(0, 0, ctrl.Width, ctrl.Height);
		}

		/*
		 * GetRawOpacity
		 */

		private static float GetRawOpacity(Size bmpSize, int iteration, NuGenReflectStyle reflectStyle, int maxOpacity)
		{
			switch (reflectStyle)
			{
				case NuGenReflectStyle.Left:
				{
					return (255f / bmpSize.Width) * iteration - maxOpacity;
				}
				case NuGenReflectStyle.Right:
				{
					return (255f / bmpSize.Width) * (bmpSize.Width - iteration) - maxOpacity;
				}
				case NuGenReflectStyle.Top:
				{
					return (255f / bmpSize.Height) * iteration - maxOpacity;
				}
				default:
				{
					return (255f / bmpSize.Height) * (bmpSize.Height - iteration) - maxOpacity;
				}
			}
		}

		/*
		 * RotateBitmap
		 */

		private static void RotateBitmap(Bitmap bmpToRotate, NuGenReflectStyle reflectStyle)
		{
			Debug.Assert(bmpToRotate != null, "bmpToRotate != null");

			switch (reflectStyle)
			{
				case NuGenReflectStyle.Left:
				case NuGenReflectStyle.Right:
				{
					bmpToRotate.RotateFlip(RotateFlipType.RotateNoneFlipX);
					break;
				}
				default:
				{
					bmpToRotate.RotateFlip(RotateFlipType.RotateNoneFlipY);
					break;
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenReflectedImageGenerator"/> class.
		/// </summary>
		public NuGenReflectedImageGenerator()
		{

		}

		#endregion
	}
}
