/* -----------------------------------------------
 * NuGenImageConverter.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Genetibase.Shared.Drawing
{
	/// <summary>
	/// Provides methods to convert an <see cref="T:Image"/> to stream and vice versa.
	/// </summary>
	public static class NuGenImageConverter
	{
		#region Methods.Public.Static

		/*
		 * BytesToImage
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="bytes"/> is <see langword="null"/>.
		/// </exception>
		public static Image BytesToImage(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}

			MemoryStream stream = new MemoryStream(bytes);
			return Image.FromStream(stream);
		}

		/*
		 * ImageToBytes
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="image"/> is <see langword="null"/>.
		/// </exception>
		public static byte[] ImageToBytes(Image image)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}

			TypeConverter converter = TypeDescriptor.GetConverter(typeof(Image));
			return (byte[])converter.ConvertTo(image, typeof(byte[]));
		}

		/*
		 * ImageToString
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///	<paramref name="image"/> is <see langword="null"/>.
		/// </exception>
		public static string ImageToString(Image image)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}

			byte[] buffer = ImageToBytes(image);
			return Convert.ToBase64String(buffer);
		}

		/*
		 * StringToImage
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="stringToConstructImageFrom"/> is <see langword="null"/>.</para></exception>
		public static Image StringToImage(string stringToConstructImageFrom)
		{
			if (stringToConstructImageFrom == null)
			{
				throw new ArgumentNullException("stringToConstructImageFrom");
			}

			byte[] buffer = Convert.FromBase64String(stringToConstructImageFrom);
			return BytesToImage(buffer);
		}

		#endregion
	}
}
