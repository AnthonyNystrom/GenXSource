/* -----------------------------------------------
 * NuGenImageExport.cs
 * Copyright © 2006 Alex Nesterov
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

namespace Genetibase.ApplicationBlocks.ImageExport
{
	/// <summary>
	/// Provides functionality to export images to various formats.
	/// </summary>
	internal class NuGenImageExport : NuGenEventInitiator
	{
		#region Events

		/*
		 * Progress
		 */

		private static readonly object EventProgress = new object();

		/// <summary>
		/// Occurs while process is going on.
		/// </summary>
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

		/// <summary>
		/// Raises the <see cref="E:Progress"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
		protected virtual void OnProgress(CancelEventArgs e)
		{
			EventHandler<CancelEventArgs> handler = (EventHandler<CancelEventArgs>)this.Events[EventProgress];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// Exports the specified image to the specified file formats and types.
		/// </summary>
		/// <param name="image">Specifies the image to export.</param>
		/// <param name="imageType">Specifies image types (i.e. color, grayscale, monochrome).</param>
		/// <param name="resolution">Specifies the resolution for the image.</param>
		/// <param name="fileFormat">Specifies file formats (i.e. BMP, JPEG, etc).</param>
		/// <param name="path">Specifies destination directory.</param>
		/// <param name="filename">Specifies the string that is used in the filename pattern.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para><paramref name="image"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="path"/> is <see langword="null"/> or empty string.</para>
		/// -or-
		/// <para><paramref name="filename"/> is <see langword="null"/> or empty string.</para>
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// Width or height of the <paramref name="resolution"/> structure are not positive.
		/// </exception>
		public void Export(Image image, NuGenImageType imageType, NuGenImageFileFormat fileFormat, Size resolution, string path, string filename)
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
					(Bitmap)image,
					fileFormat,
					path,
					string.Format("{0}_Color", filename)
					);
			}

			if ((imageType & NuGenImageType.Grayscale) != 0)
			{
				this.ExportToFileFormat(
					NuGenControlPaint.GetGrayscaleBitmap((Bitmap)image),
					fileFormat,
					path,
					string.Format("{0}_Grayscale", filename)
					);
			}

			if ((imageType & NuGenImageType.Monochrome) != 0)
			{
				this.ExportToFileFormat(
					NuGenControlPaint.GetMonochromeBitmap((Bitmap)image),
					fileFormat,
					path,
					string.Format("{0}_Monochrome", filename)
					);
			}
		}

		#endregion

		#region Methods.Private

		private void ExportToFileFormat(Bitmap bmp, NuGenImageFileFormat fileFormat, string path, string filename)
		{
			Debug.Assert(bmp != null, "bmp != null");

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			if ((fileFormat & NuGenImageFileFormat.Bmp) != 0)
			{
				bmp.Save(this.GetUniqueFileName(path, filename, "bmp", 0), ImageFormat.Bmp);
				
				if (!this.ShouldContinue())
				{
					return;
				}
			}

			if ((fileFormat & NuGenImageFileFormat.Emf) != 0)
			{
				bmp.Save(this.GetUniqueFileName(path, filename, "emf", 0), ImageFormat.Emf);

				if (!this.ShouldContinue())
				{
					return;
				}
			}

			if ((fileFormat & NuGenImageFileFormat.Exif) != 0)
			{
				bmp.Save(this.GetUniqueFileName(path, filename, "exif", 0), ImageFormat.Exif);

				if (!this.ShouldContinue())
				{
					return;
				}
			}

			if ((fileFormat & NuGenImageFileFormat.Gif) != 0)
			{
				bmp.Save(this.GetUniqueFileName(path, filename, "gif", 0), ImageFormat.Gif);

				if (!this.ShouldContinue())
				{
					return;
				}
			}

			if ((fileFormat & NuGenImageFileFormat.Jpeg) != 0)
			{
				bmp.Save(this.GetUniqueFileName(path, filename, "jpg", 0), ImageFormat.Jpeg);

				if (!this.ShouldContinue())
				{
					return;
				}
			}

			if ((fileFormat & NuGenImageFileFormat.Png) != 0)
			{
				bmp.Save(this.GetUniqueFileName(path, filename, "png", 0), ImageFormat.Png);

				if (!this.ShouldContinue())
				{
					return;
				}
			}

			if ((fileFormat & NuGenImageFileFormat.Tiff) != 0)
			{
				bmp.Save(this.GetUniqueFileName(path, filename, "tiff", 0), ImageFormat.Tiff);

				if (!this.ShouldContinue())
				{
					return;
				}
			}

			if ((fileFormat & NuGenImageFileFormat.Wmf) != 0)
			{
				bmp.Save(this.GetUniqueFileName(path, filename, "wmf", 0), ImageFormat.Wmf);

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

		#endregion
	}
}
