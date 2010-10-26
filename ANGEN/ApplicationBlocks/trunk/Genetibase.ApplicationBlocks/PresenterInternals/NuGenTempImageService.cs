/* -----------------------------------------------
 * TempImageService.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.Properties;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.PresenterInternals
{
	/// <summary>
	/// Provides functionality to work with temporary images. Call <see cref="Dispose()"/> to delete the temporary files.
	/// </summary>
	public sealed class NuGenTempImageService : INuGenTempImageService
	{
		/// <summary>
		/// </summary>
		/// <returns></returns>
		public IList<Image> GetTempImageCollection()
		{
			IList<Image> imageCollection = new List<Image>();

			string directoryName;
			
			if (this.GetTempImageDirectory(out directoryName))
			{
				DirectoryInfo dirInfo = new DirectoryInfo(directoryName);
				
				if (dirInfo.Exists)
				{
					FileInfo[] tempImageFiles;

					try
					{
						tempImageFiles = dirInfo.GetFiles(
							string.Format("*.{0}", Resources.Text_Presenter_TempFileExt)
							, SearchOption.TopDirectoryOnly
						);
					}
					catch (DirectoryNotFoundException)
					{
						return imageCollection;
					}

					if (tempImageFiles != null && tempImageFiles.Length > 0)
					{
						foreach (FileInfo fi in tempImageFiles)
						{
							if (fi.Exists)
							{
								imageCollection.Add(Image.FromFile(Path.Combine(directoryName, fi.Name)));
							}
						}
					}
				}
			}

			return imageCollection;
		}

		private bool GetTempImageDirectory(out string directoryName)
		{
			DirectoryInfo dirInfo = new DirectoryInfo(GetTempFolderName());

			if (dirInfo.Exists)
			{
				directoryName = dirInfo.FullName;
				return true;
			}

			directoryName = null;
			return false;
		}

		/// <summary>
		/// </summary>
		/// <param name="imageToSave"></param>
		/// <exception cref="ArgumentNullException"><paramref name="imageToSave"/> is <see langword="null"/>.</exception>
		public void SaveTempImage(Image imageToSave)
		{
			if (imageToSave == null)
			{
				throw new ArgumentNullException("imageToSave");
			}

			DirectoryInfo dirInfo = new DirectoryInfo(_tempPath);
			Debug.Write("NuGenTempImageService::SaveTempImage:\tAttempting to save to ");
			Debug.WriteLine(_tempPath);

			if (!dirInfo.Exists)
			{
				Debug.Write("NuGenTempImageService::SaveTempImage:\tDirectory does not exist. Creating...");
				dirInfo.Create();
				Debug.WriteLine(" ok");
			}
			else
			{
				Debug.WriteLine("NuGenTempImageService::SaveTempImage:\tDirectory exists.");
			}

			Debug.Write("NuGenTempImageService::SaveTempImage:\tSaving the image at ");

			string path;

			do
			{
				path = Path.Combine(_tempPath, GetTempFileName());
			} while (new FileInfo(path).Exists);
			
			Debug.WriteLine(path);

			imageToSave.Save(path, ImageFormat.Bmp);
			Debug.WriteLine("NuGenTempImageService::SaveTempImage:\tOperation succeeded.");
		}

		private static string GetTempFolderName()
		{
			return Path.Combine(Path.GetTempPath(), Resources.Text_Presenter_TempFolderPrefix);
		}

		private static string GetTempFileName()
		{
			return string.Format("{0}.{1}", GetTempName(), Resources.Text_Presenter_TempFileExt);
		}

		private static int _fileCounter = 0;

		private static string GetTempName()
		{
			_fileCounter++;
			return _fileCounter.ToString("0000");
		}

		private static readonly string _tempPath;

		static NuGenTempImageService()
		{
			_tempPath = GetTempFolderName();
			Debug.Write("NuGenTempImageService::.cctor:\t_tempPath = ");
			Debug.WriteLine(_tempPath);
			DirectoryInfo dirInfo = new DirectoryInfo(_tempPath);
			Debug.Write("NuGenTempImageService::.cctor:\tCreating the folder at _tempPath...");
			dirInfo.Create();
			Debug.WriteLine(" ok");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTempImageService"/> class.
		/// </summary>
		public NuGenTempImageService()
		{
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private bool _disposed;

		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				_disposed = true;

				DirectoryInfo dirInfo;

				try
				{
					dirInfo = new DirectoryInfo(_tempPath);
				}
				catch (Exception e)
				{
					Debug.WriteLine("NuGenTempImageService::Dispose(bool):\tException has been thrown while initializing DirectoryInfo.");
					Debug.WriteLine(e.ToString());
					return;
				}

				if (dirInfo != null && dirInfo.Exists)
				{
					Debug.Write("NuGenTempImageService::Dispose(bool):\tDeleting the temporary directory...");
					
					try
					{
						dirInfo.Delete(true);
					}
					catch (Exception e)
					{
						Debug.WriteLine("NuGenTempImageService::Dispose(bool):\tException has been thrown while deleting the temporary directory.");
						Debug.WriteLine(e.ToString());
						return;
					}

					Debug.WriteLine(" ok");
				}
			}
		}
	}
}
