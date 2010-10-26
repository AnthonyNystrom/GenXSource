/* -----------------------------------------------
 * ImageExportService.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Drawing;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Globalization;

namespace Genetibase.ApplicationBlocks.ImageExportInternals
{
	/// <summary>
	/// Provides functionality to export images to various formats.
	/// </summary>
	internal sealed class ImageExportService : NuGenEventInitiator
	{
		private static readonly object EventProgress = new object();

		public event EventHandler<CancelEventArgs> Progress
		{
			add
			{
				this.Events.AddHandler(EventProgress, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventProgress, value);
			}
		}

		private void OnProgress(CancelEventArgs e)
		{
			EventHandler<CancelEventArgs> handler = (EventHandler<CancelEventArgs>)this.Events[EventProgress];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		public void ExportWithWatermark(
			Image image
			, NuGenImageType imageType
			, NuGenImageFileFormat fileFormat
			, Size resolution
			, int watermarkCount
			, Font watermarkFont
			, Color watermarkColor
			, ContentAlignment watermarkAlignment
			, string path
			, string filename
			)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}

			if (watermarkFont == null)
			{
				throw new ArgumentNullException("watermarkFont");
			}

			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}

			if (string.IsNullOrEmpty(filename))
			{
				throw new ArgumentNullException("filename");
			}

			using (Graphics g = Graphics.FromImage(image))
			using (SolidBrush sb = new SolidBrush(watermarkColor))
			{
				string text = watermarkCount.ToString(CultureInfo.CurrentCulture);
				Size watermarkSize = g.MeasureString(text, watermarkFont).ToSize();
				Rectangle watermarkBounds = NuGenControlPaint.ImageBoundsFromContentAlignment(
					watermarkSize
					, new Rectangle(new Point(0, 0), image.Size)
					, watermarkAlignment
				);
				g.DrawString(text, watermarkFont, sb, watermarkBounds);
			}

			this.Export(image, imageType, fileFormat, resolution, path, filename);
		}

		public void Export(
			Image image
			, NuGenImageType imageType
			, NuGenImageFileFormat fileFormat
			, Size resolution
			, string path
			, string filename
			)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}

			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}

			if (string.IsNullOrEmpty(filename))
			{
				throw new ArgumentNullException("filename");
			}

			image = NuGenControlPaint.GetThumbnail(image, resolution);

			if ((imageType & NuGenImageType.Color) != 0)
			{
				this.ExportToFileFormat(
					image,
					fileFormat,
					path,
					string.Format("{0}_Color", filename)
				);
			}

			if ((imageType & NuGenImageType.Grayscale) != 0)
			{
				this.ExportToFileFormat(
					NuGenControlPaint.GetDesaturatedImage(image),
					fileFormat,
					path,
					string.Format("{0}_Grayscale", filename)
				);
			}

			if ((imageType & NuGenImageType.Monochrome) != 0)
			{
				this.ExportToFileFormat(
					NuGenControlPaint.GetMonochromeImage(image),
					fileFormat,
					path,
					string.Format("{0}_Monochrome", filename)
				);
			}
		}

		private void ExportToFileFormat(Image img, NuGenImageFileFormat fileFormat, string path, string filename)
		{
			Debug.Assert(img != null, "bmp != null");

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			if ((fileFormat & NuGenImageFileFormat.Bmp) != 0)
			{
				img.Save(this.GetUniqueFileName(path, filename, "bmp", 0), ImageFormat.Bmp);

				if (!this.ShouldContinue())
				{
					return;
				}
			}

			if ((fileFormat & NuGenImageFileFormat.Emf) != 0)
			{
				img.Save(this.GetUniqueFileName(path, filename, "emf", 0), ImageFormat.Emf);

				if (!this.ShouldContinue())
				{
					return;
				}
			}

			if ((fileFormat & NuGenImageFileFormat.Exif) != 0)
			{
				img.Save(this.GetUniqueFileName(path, filename, "exif", 0), ImageFormat.Exif);

				if (!this.ShouldContinue())
				{
					return;
				}
			}

			if ((fileFormat & NuGenImageFileFormat.Gif) != 0)
			{
				img.Save(this.GetUniqueFileName(path, filename, "gif", 0), ImageFormat.Gif);

				if (!this.ShouldContinue())
				{
					return;
				}
			}

			if ((fileFormat & NuGenImageFileFormat.Jpeg) != 0)
			{
				img.Save(this.GetUniqueFileName(path, filename, "jpg", 0), ImageFormat.Jpeg);

				if (!this.ShouldContinue())
				{
					return;
				}
			}

			if ((fileFormat & NuGenImageFileFormat.Png) != 0)
			{
				img.Save(this.GetUniqueFileName(path, filename, "png", 0), ImageFormat.Png);

				if (!this.ShouldContinue())
				{
					return;
				}
			}

			if ((fileFormat & NuGenImageFileFormat.Tiff) != 0)
			{
				img.Save(this.GetUniqueFileName(path, filename, "tiff", 0), ImageFormat.Tiff);

				if (!this.ShouldContinue())
				{
					return;
				}
			}

			if ((fileFormat & NuGenImageFileFormat.Wmf) != 0)
			{
				img.Save(this.GetUniqueFileName(path, filename, "wmf", 0), ImageFormat.Wmf);

				if (!this.ShouldContinue())
				{
					return;
				}
			}
		}

		private string GetCurrentFileName(string path, string filename, string extension, int postfix)
		{
			return Path.Combine(path, string.Concat(filename, postfix > 0 ? "_" + postfix.ToString() : "", ".", extension));
		}

		private string GetUniqueFileName(string path, string filename, string extension, int defaultCount)
		{
			string fullName = this.GetCurrentFileName(path, filename, extension, defaultCount);

			if (File.Exists(fullName))
			{
				fullName = this.GetUniqueFileName(path, filename, extension, ++defaultCount);
			}

			return fullName;
		}

		private bool ShouldContinue()
		{
			CancelEventArgs e = new CancelEventArgs();
			this.OnProgress(e);
			return !e.Cancel;
		}
	}
}
