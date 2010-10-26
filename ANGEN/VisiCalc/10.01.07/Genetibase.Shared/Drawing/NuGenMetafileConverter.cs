/* -----------------------------------------------
 * NuGenMetafileConverter.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
	/// Provides methods to convert a <see cref="Metafile"/> to stream and vice versa.
	/// </summary>
	public static class NuGenMetafileConverter
	{
		#region Methods.Public.Static

		/*
		 * BytesToMetafile
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="bytes"/> is <see langword="null"/>.
		/// </exception>
		public static Metafile BytesToMetafile(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}

			MemoryStream stream = new MemoryStream(bytes);
			return (Metafile)Image.FromStream(stream);
		}

		/*
		 * MetafileToBytes
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="metafile"/> is <see langword="null"/>.
		/// </exception>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public static byte[] MetafileToBytes(Metafile metafile)
		{
			if (metafile == null)
			{
				throw new ArgumentNullException("metafile");
			}

			MemoryStream stream = new MemoryStream();
			NuGenMetafileSaver.Save(stream, metafile);
			stream.Flush();
			
			byte[] buffer = stream.GetBuffer();
			stream.Close();
			
			return buffer;
		}

		/*
		 * MetafileToString
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="metafileToConvert"/> is <see langword="null"/>.
		/// </exception>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public static string MetafileToString(Metafile metafileToConvert)
		{
			if (metafileToConvert == null)
			{
				throw new ArgumentNullException("metafileToConvert");
			}

			byte[] buffer = NuGenMetafileConverter.MetafileToBytes(metafileToConvert);
			return Convert.ToBase64String(buffer);
		}

		/*
		 * StringToMetafile
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///	<paramref name="stringToConstructMetafileFrom"/> is <see langword="null"/>.
		///	</exception>
		public static Metafile StringToMetafile(string stringToConstructMetafileFrom)
		{
			if (stringToConstructMetafileFrom == null)
			{
				throw new ArgumentNullException("stringToConstructMetafileFrom");
			}

			byte[] buffer = Convert.FromBase64String(stringToConstructMetafileFrom);
			return NuGenMetafileConverter.BytesToMetafile(buffer);
		}

		#endregion
	}
}
