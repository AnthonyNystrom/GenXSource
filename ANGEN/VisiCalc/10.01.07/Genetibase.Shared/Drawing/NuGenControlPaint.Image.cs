/* -----------------------------------------------
 * NuGenControlPaint.Image.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace Genetibase.Shared.Drawing
{
	partial class NuGenControlPaint
	{
		private static readonly TraceSwitch _traceSwitch = new TraceSwitch(typeof(NuGenControlPaint).Name, typeof(NuGenControlPaint).FullName);
		private static readonly bool _errorEnabled = _traceSwitch.TraceError;
		private static readonly bool _infoEnabled = _traceSwitch.TraceInfo;

		/*
		 * GetBitmapFromRect
		 */

		/// <summary>
		/// Gets the partial bitmap from the specified <see cref="Bitmap"/>
		/// according to the specified clip rectangle.<para/>Note: Uses unsafe code.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="sourceBitmap"/> is <see langword="null"/>.
		/// </exception>
		public static Bitmap GetBitmapFromRect(Bitmap sourceBitmap, Rectangle rect)
		{
			if (sourceBitmap == null)
			{
				throw new ArgumentNullException("sourceBitmap");
			}

			Bitmap bufferBmp;
			PixelFormat pixelFormat = PixelFormat.Format32bppPArgb;

			if (_infoEnabled)
			{
				Trace.TraceInformation("rect = {0}", rect);
			}

			try
			{
				bufferBmp = new Bitmap(rect.Width, rect.Height, pixelFormat);
			}
			catch (Exception e)
			{
				if (_errorEnabled)
				{
					Trace.TraceError("Error while creating bufferBmp: {0}", e.Message);
				}

				throw;
			}

			BitmapData oneBmpData, twoBmpData;
			Rectangle oneBmpRectangle = new Rectangle(0, 0, rect.Width, rect.Height);
			ImageLockMode imgLockMode = ImageLockMode.WriteOnly;

			try
			{
				oneBmpData = bufferBmp.LockBits(
					oneBmpRectangle,
					imgLockMode,
					pixelFormat
					);
			}
			catch (Exception e)
			{
				if (_errorEnabled)
				{
					Trace.TraceError("Error occured while bufferBmp.LockBits operation: {0}", e.Message);
				}

				throw;
			}

			imgLockMode = ImageLockMode.ReadOnly;

			try
			{
				twoBmpData = sourceBitmap.LockBits(
					rect,
					imgLockMode,
					pixelFormat
					);
			}
			catch (Exception e)
			{
				if (_errorEnabled)
				{
					Trace.TraceError("Error occured while bmp.LockBits operation: {0}", e.Message);
				}

				throw;
			}

			Point size = new Point(rect.Width, rect.Height);

			int y = 0;

			unsafe
			{
				for (y = 0; y < size.Y; y++)
				{
					byte* ro1 = (byte*)oneBmpData.Scan0 + (y * oneBmpData.Stride);
					byte* ro2 = (byte*)twoBmpData.Scan0 + (y * twoBmpData.Stride);

					for (int x = 0; x < size.X * 4; x++)
					{
						ro1[x] = ro2[x];
					}
				}
			}

			try
			{
				bufferBmp.UnlockBits(oneBmpData);
			}
			catch (Exception e)
			{
				if (_errorEnabled)
				{
					Trace.TraceError("Error occured while bufferBmp.UnlockBits operation: {0}", e.Message);
				}

				throw;
			}

			try
			{
				sourceBitmap.UnlockBits(twoBmpData);
			}
			catch (Exception e)
			{
				if (_errorEnabled)
				{
					Trace.TraceError("Error occured while bmp.UnlockBits operation: {0}", e.Message);
				}

				throw;
			}

			if (_infoEnabled)
			{
				Trace.TraceInformation("Operation completed successfully.");
			}

			return bufferBmp;
		}

		/*
		 * GetDesaturatedImage
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="sourceBitmap"/> is <see langword="null"/>.
		/// </exception>
		public static Image GetDesaturatedImage(Image sourceImage)
		{
			if (sourceImage == null)
			{
				throw new ArgumentNullException("sourceImage");
			}

			Bitmap bufferBmp = new Bitmap(sourceImage);
			Rectangle bufferBmpRectangle = new Rectangle(0, 0, bufferBmp.Width, bufferBmp.Height);
			ImageLockMode imgLockMode = ImageLockMode.ReadWrite;
			PixelFormat pixelFormat = PixelFormat.Format32bppArgb;

			if (_infoEnabled)
			{
				Trace.TraceInformation("bufferBmpRectangle = {0}", bufferBmpRectangle.ToString());
			}

			BitmapData bmpData = null;

			try
			{
				bmpData = bufferBmp.LockBits(bufferBmpRectangle, imgLockMode, pixelFormat);
			}
			catch (Exception e)
			{
				if (_errorEnabled)
				{
					Trace.TraceError("Error occured while bufferBmp.LockBits operation: {0}", e.Message);
				}

				throw;
			}

			unsafe
			{
				uint* scan0 = (uint*)bmpData.Scan0;

				for (int height = 0; height < bufferBmp.Height; height++)
				{
					for (int width = 0; width < bufferBmp.Width; width++)
					{
						int block1 = (bmpData.Stride * height) / 4;
						uint block2 = scan0[block1 + width];
						uint block3 = (block2 >> 0x10) & 0xff;
						uint block4 = (block2 >> 8) & 0xff;
						uint block5 = block2 & 0xff;
						uint block6 = (block3 + block4 + block5) / 3;
						scan0[block1 + width] = ((block6 << 0x10) + (block6 << 8)) + block6;
					}
				}
			}

			try
			{
				bufferBmp.UnlockBits(bmpData);
			}
			catch (Exception e)
			{
				if (_errorEnabled)
				{
					Trace.TraceError("Error occured while bufferBmp.UnlockBits operation: {0}", e.Message);
				}

				throw;
			}

			return bufferBmp;
		}

		/*
		 * GetMonochromeImage
		 */

		/// <summary>
		/// Converts the specified <see cref="Bitmap"/> to monochrome image.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="sourceImage"/> is <see langword="null"/>.
		/// </exception>
		public static Image GetMonochromeImage(Image sourceImage)
		{
			if (sourceImage == null)
			{
				throw new ArgumentNullException("sourceImage");
			}

			Bitmap bufferBmp = new Bitmap(sourceImage);
			BitmapData bmpData = bufferBmp.LockBits(new Rectangle(0, 0, bufferBmp.Width, bufferBmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

			unsafe
			{
				uint* scan0 = (uint*)bmpData.Scan0;

				for (int height = 0; height < bufferBmp.Height; height++)
				{
					for (int width = 0; width < bufferBmp.Width; width++)
					{
						int block1 = (bmpData.Stride * height) / 4;
						uint block2 = scan0[block1 + width];
						uint block3 = (block2 >> 0x10) & 0xff;
						uint block4 = (block2 >> 8) & 0xff;
						uint block5 = block2 & 0xff;
						uint block6 = ((block3 + block4) + block5) / 3;

						if (block6 >= 0x80)
						{
							block6 = 0xff;
						}
						else
						{
							block6 = 0;
						}

						scan0[block1 + width] = ((block6 << 0x10) + (block6 << 8)) + block6;
					}
				}
			}

			bufferBmp.UnlockBits(bmpData);
			return bufferBmp;
		}

		/*
		 * GetImage
		 */

		/// <summary>
		/// Retrieves an image with the specified name from resources.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="typeToSpecifyAssemblyContainingImage"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="imageName"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public static Bitmap GetImage(Type typeToSpecifyAssemblyContainingImage, string imageName)
		{
			if (typeToSpecifyAssemblyContainingImage == null)
			{
				throw new ArgumentNullException("typeToSpecifyAssemblyContainingImage");
			}

			if (imageName == null)
			{
				throw new ArgumentNullException("imageName");
			}

			return NuGenControlPaint.GetImage(typeToSpecifyAssemblyContainingImage.Module.Assembly, imageName);
		}

		/// <summary>
		/// Retrieves an image with the specified name from resources.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para>
		///		<paramref name="assemblyContainingImage"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="imageName"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public static Bitmap GetImage(string assemblyContainingImage, string imageName)
		{
			if (assemblyContainingImage == null)
			{
				throw new ArgumentNullException("assemblyContainingImage");
			}

			if (imageName == null)
			{
				throw new ArgumentNullException("imageName");
			}

			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (Assembly a in assemblies)
			{
				string aName = a.GetName().Name;

				if (aName == assemblyContainingImage)
				{
					return NuGenControlPaint.GetImage(a, imageName);
				}
			}

			return null;
		}

		/// <summary>
		/// Retrieves an image with the specified name from resources.
		/// </summary>
		/// 
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="assemblyContainingImage"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="imageName"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public static Bitmap GetImage(Assembly assemblyContainingImage, string imageName)
		{
			if (assemblyContainingImage == null)
			{
				throw new ArgumentNullException("assemblyContainingImage");
			}

			if (imageName == null)
			{
				throw new ArgumentNullException("imageName");
			}

			try
			{
				Stream stream = assemblyContainingImage.GetManifestResourceStream(imageName);

				if (stream != null)
				{
					Bitmap bmp = new Bitmap(stream);
					NuGenControlPaint.MakeBackgroundTransparent(bmp);
					return bmp;
				}
			}
			finally
			{
			}

			return null;
		}

		/*
		 * GetThumbnail
		 */

		/// <summary>
		/// Gets a thumbnail for the specified image.
		/// </summary>
		///
		/// <exception cref="ArgumentNullException">
		/// <paramref name="sourceImage"/> is <see langword="null"/>.
		/// </exception>
		/// 
		/// <exception cref="ArgumentException">
		/// Width or height of the <paramref name="thumbnail"/> structure are not positive.
		/// </exception>
		public static Image GetThumbnail(Image sourceImage, Size thumbnailSize)
		{
			if (sourceImage == null)
			{
				throw new ArgumentNullException("sourceImage");
			}

			if (thumbnailSize.Height < 1 || thumbnailSize.Width < 1)
			{
				throw new ArgumentException(
					string.Format(CultureInfo.InvariantCulture, Properties.Resources.Argument_InvalidResolutionStruct, "thumbnailSize")
				);
			}

			Image img = new Bitmap(thumbnailSize.Width, thumbnailSize.Height);

			using (Graphics g = Graphics.FromImage(img))
			{
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.PixelOffsetMode = PixelOffsetMode.HighQuality;
				g.DrawImage(sourceImage, 0, 0, thumbnailSize.Width, thumbnailSize.Height);
			}

			return img;
		}

		/// <summary>
		/// Gets a thumbnail for the specified image.
		/// </summary>
		/// 
		/// <exception cref="T:System.ArgumentNullException">
		/// <para>
		///		<paramref name="sourceImage"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="outputPath"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="outputPath"/> is an empty string.
		/// </para>
		/// </exception>
		public static void GetThumbnail(Image sourceImage, Size thumbnailSize, string outputPath)
		{
			if (sourceImage == null)
			{
				throw new ArgumentNullException("sourceImage");
			}

			if (string.IsNullOrEmpty("outputPath"))
			{
				throw new ArgumentNullException("outputPath");
			}

			EncoderParameters encoderParams = new EncoderParameters(1);
			ImageCodecInfo jpgCodecInfo = NuGenControlPaint.GetCodecInfo("image/jpeg");
			encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);

			using (Image img = NuGenControlPaint.GetThumbnail(sourceImage, thumbnailSize))
			{
				img.Save(outputPath, jpgCodecInfo, encoderParams);
			}
		}

		/*
		 * ImageBoundsFromContentAlignment
		 */

		/// <summary>
		/// </summary>
		/// <param name="imageSize"></param>
		/// <param name="fitRectangle"></param>
		/// <param name="contentAlignment"></param>
		/// <returns></returns>
		public static Rectangle ImageBoundsFromContentAlignment(Size imageSize, Rectangle fitRectangle, ContentAlignment contentAlignment)
		{
			int cx = 0;
			int cy = 0;

			switch (contentAlignment)
			{
				case ContentAlignment.BottomCenter:
				{
					cx = fitRectangle.Left + (fitRectangle.Width - imageSize.Width) / 2;
					cy = fitRectangle.Bottom - imageSize.Height;

					break;
				}
				case ContentAlignment.BottomLeft:
				{
					cx = fitRectangle.Left;
					cy = fitRectangle.Bottom - imageSize.Height;

					break;
				}
				case ContentAlignment.BottomRight:
				{
					cx = fitRectangle.Right - imageSize.Width;
					cy = fitRectangle.Bottom - imageSize.Height;

					break;
				}
				case ContentAlignment.MiddleLeft:
				{
					cx = fitRectangle.Left;
					cy = fitRectangle.Top + (fitRectangle.Height - imageSize.Height) / 2;

					break;
				}
				case ContentAlignment.MiddleRight:
				{
					cx = fitRectangle.Right - imageSize.Width;
					cy = fitRectangle.Top + (fitRectangle.Height - imageSize.Height) / 2;

					break;
				}
				case ContentAlignment.TopCenter:
				{
					cx = fitRectangle.Left + (fitRectangle.Width - imageSize.Width) / 2;
					cy = fitRectangle.Top;

					break;
				}
				case ContentAlignment.TopLeft:
				{
					cx = fitRectangle.Left;
					cy = fitRectangle.Top;

					break;
				}
				case ContentAlignment.TopRight:
				{
					cx = fitRectangle.Right - imageSize.Width;
					cy = fitRectangle.Top;

					break;
				}
				default:
				{
					cx = fitRectangle.Left + (fitRectangle.Width - imageSize.Width) / 2;
					cy = fitRectangle.Top + (fitRectangle.Height - imageSize.Height) / 2;

					break;
				}
			}

			return new Rectangle(new Point(cx, cy), imageSize);
		}

		/*
		 * MakeBackgroundTransparent
		 */

		/// <summary>
		/// Makes the transparent background for the specified <see cref="T:System.Drawing.Bitmap"/>.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="targetBitmap"/> is <see langword="null"/>.
		/// </exception>
		public static void MakeBackgroundTransparent(Bitmap targetBitmap)
		{
			if (targetBitmap == null)
			{
				throw new ArgumentNullException("targetBitmap");
			}

			Color startColor = targetBitmap.GetPixel(0, targetBitmap.Height - 1);
			targetBitmap.MakeTransparent();

			Color endColor = Color.FromArgb(0, startColor);
			targetBitmap.SetPixel(0, targetBitmap.Height - 1, endColor);
		}

		/*
		 * ScaleToFit
		 */

		/// <summary>
		/// Calculates the destination rectangle for the image of the specified size to fit the
		/// specified rectangle.
		/// </summary>
		public static Rectangle ScaleToFit(Rectangle targetRect, Size imageSize)
		{
			if (!targetRect.IsEmpty)
			{
				Rectangle resultRect = new Rectangle(targetRect.Location, targetRect.Size);

				if (resultRect.Height * imageSize.Width > resultRect.Width * imageSize.Height)
				{
					resultRect.Height = resultRect.Width * imageSize.Height / imageSize.Width;
					resultRect.Y += (targetRect.Height - resultRect.Height) / 2;
				}
				else
				{
					resultRect.Width = resultRect.Height * imageSize.Width / imageSize.Height;
					resultRect.X += (targetRect.Width - resultRect.Width) / 2;
				}

				return resultRect;
			}

			return Rectangle.Empty;
		}
	}
}
