/* -----------------------------------------------
 * NuGenControlPaint.cs
 * Copyright © 2005-2006 Alex Nesterov
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Reflection;
using Genetibase.WinApi;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.Shared.Drawing
{
	/// <summary>
	/// Provides helper methods for graphic operations.
	/// </summary>
	public static partial class NuGenControlPaint
	{
		#region Declarations.Consts

		/// <summary>
		/// Mm in inch.
		/// </summary>
		private const float MM_IN_INCH = 25.4f;

		#endregion

		#region Properties.Public.Static

		/*
		 * AnyBottom
		 */

		/// <summary>
		/// ContentAlignment.BottomRight | ContentAlignment.BottomCenter | ContentAlignment.BottomLeft
		/// </summary>
		public static ContentAlignment AnyBottom
		{
			get
			{
				return 0
					| ContentAlignment.BottomRight
					| ContentAlignment.BottomCenter
					| ContentAlignment.BottomLeft
					;
			}
		}

		/*
		 * AnyCenter
		 */

		/// <summary>
		/// ContentAlignment.BottomCenter | ContentAlignment.MiddleCenter | ContentAlignment.TopCenter
		/// </summary>
		public static ContentAlignment AnyCenter
		{
			get
			{
				return 0
					| ContentAlignment.BottomCenter
					| ContentAlignment.MiddleCenter
					| ContentAlignment.TopCenter
					;
			}
		}

		/*
		 * AnyMiddle
		 */

		/// <summary>
		/// ContentAlignment.MiddleRight | ContentAlignment.MiddleCenter | ContentAlignment.MiddleLeft
		/// </summary>
		public static ContentAlignment AnyMiddle
		{
			get
			{
				return 0
					| ContentAlignment.MiddleRight
					| ContentAlignment.MiddleCenter
					| ContentAlignment.MiddleLeft
					;
			}
		}

		/*
		 * AnyRight
		 */

		/// <summary>
		/// ContentAlignment.BottomRight | ContentAlignment.MiddleRight | ContentAlignment.TopRight
		/// </summary>
		public static ContentAlignment AnyRight
		{
			get
			{
				return 0
					| ContentAlignment.BottomRight
					| ContentAlignment.MiddleRight
					| ContentAlignment.TopRight
					;
			}
		}

		#endregion

		#region Methods.Private

		/// <summary>
		/// Retrieves the <see cref="T:System.Drawing.Imaging.ImageCodecInfo"/> according to the MIME type
		/// specified.
		/// </summary>
		/// <param name="mimeType">Specifies the MIME.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="mimeType"/> is <see langword="null"/>.
		/// </exception>
		private static ImageCodecInfo GetCodecInfo(string mimeType)
		{
			if (mimeType == null)
			{
				throw new ArgumentNullException("mimeType");
			}

			ImageCodecInfo[] codecInfo = ImageCodecInfo.GetImageEncoders();

			int codec = 0;

			for (int i = 0; i < codecInfo.Length; i++)
			{
				if (codecInfo[i].MimeType == mimeType)
				{
					codec = i;
					break;
				}
			}

			return codecInfo[codec];
		}

		/// <summary>
		/// Invokes the SetStyle method of the <see cref="T:Control"/>.
		/// </summary>
		/// <param name="ctrl">Specifies the <see cref="T:System.Windows.Forms.Control"/> to invoke the SetStyle method for.</param>
		/// <param name="ctrlStyles">Control styles to set.</param>
		/// <param name="value"><see langword="true"/> to apply the specified styles to the control;
		/// otherwise, <see langword="false"/>.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="ctrl"/> is <see langword="null"/>.
		/// </exception>
		private static void InvokeSetStyle(Control ctrl, ControlStyles ctrlStyles, bool value)
		{
			if (ctrl == null)
			{
				throw new ArgumentNullException("ctrl");
			}

			NuGenInvoker.InvokeMethod(ctrl, "SetStyle", ctrlStyles, value);
		}

		/// <summary>
		/// Invokes the GetStyle method of the specified <see cref="T:Control"/>.
		/// </summary>
		/// <param name="ctrl">Specifies the <see cref="T:Control"/> to invoke the GetStyle method for.</param>
		/// <param name="ctrlStyles">Specifies the control style to check.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="ctrl"/> is <see langword="null"/>.
		/// </exception>
		private static bool InvokeGetStyle(Control ctrl, ControlStyles ctrlStyles)
		{
			if (ctrl == null)
			{
				throw new ArgumentNullException("ctrl");
			}

			return (bool)NuGenInvoker.InvokeMethod(ctrl, "GetStyle", ctrlStyles);
		}

		#endregion
	}
}
