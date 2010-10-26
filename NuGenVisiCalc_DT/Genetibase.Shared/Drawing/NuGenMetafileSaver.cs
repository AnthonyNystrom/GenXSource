/* -----------------------------------------------
 * NuGenMetafileSaver.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security;
using System.Security.Permissions;

namespace Genetibase.Shared.Drawing
{
	/// <summary>
	/// Provides methods to save specified <see cref="T:Metafile"/> instances.
	/// </summary>
	public static class NuGenMetafileSaver
	{
		#region Methods.Public.Static

		/// <summary>
		/// Saves the specified <see cref="Metafile"/> to the specified <see cref="Stream"/>.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="streamToSaveTo"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="metafileToSave"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public static void Save(Stream streamToSaveTo, Metafile metafileToSave)
		{
			if (streamToSaveTo == null)
			{
				throw new ArgumentNullException("streamToSaveTo");
			}

			if (metafileToSave == null)
			{
				throw new ArgumentNullException("metafileToSave");
			}

			Metafile bufferMetafile = null;

			using (Bitmap bitmap = new Bitmap(1, 1))
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					IntPtr ptr = graphics.GetHdc();
					bufferMetafile = new Metafile(streamToSaveTo, ptr);
					graphics.ReleaseHdc(ptr);
				}
			}

			using (Graphics graphics = Graphics.FromImage(bufferMetafile))
			{
				graphics.DrawImage(metafileToSave, 0, 0);
			}
		}

		/// <summary>
		/// Saves the specified <see cref="Metafile"/> at the specified path.
		/// </summary>
		/// 
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="path"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="path"/> is an empty string.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="metafileToSave"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public static void Save(string path, Metafile metafileToSave)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}

			if (metafileToSave == null)
			{
				throw new ArgumentNullException("metafileToSave");
			}

			FileStream stream = null;

			try
			{
				stream = new FileStream(path, FileMode.Create);
				Save(stream, metafileToSave);
			}
			catch
			{
				throw;
			}
			finally
			{
				if (stream != null)
				{
					stream.Flush();
					stream.Close();
				}
			}
		}

		#endregion
	}
}
