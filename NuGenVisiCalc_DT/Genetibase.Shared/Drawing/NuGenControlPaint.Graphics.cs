/* -----------------------------------------------
 * NuGenControlPaint.Graphics.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.Shared.Drawing
{
	partial class NuGenControlPaint
	{
		/*
		 * CreateBitmapFromGraphics
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="source"/> is <see langword="null"/>.
		/// </exception>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public static Bitmap CreateBitmapFromGraphics(Graphics source, Rectangle bounds)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			IntPtr hDCSource = source.GetHdc();
			IntPtr hDCDestination = Gdi32.CreateCompatibleDC(hDCSource);
			IntPtr hBitmap = Gdi32.CreateCompatibleBitmap(hDCSource, bounds.Width, bounds.Height);
			Gdi32.SelectObject(hDCDestination, hBitmap);
			Gdi32.BitBlt(hDCDestination, bounds.X, bounds.Y, bounds.Width, bounds.Height, hDCSource, bounds.X, bounds.Y, WinGdi.SRCCOPY);

			Bitmap bitmap = Bitmap.FromHbitmap(hBitmap);

			Gdi32.DeleteObject(hDCDestination);
			Gdi32.DeleteObject(hBitmap);

			source.ReleaseHdc(hDCSource);

			return bitmap;
		}

		/*
		 * GetStyle
		 */

		/// <summary>
		/// Checks if the specified style is set for the specified <see cref="T:System.Windows.Forms.Control"/>.
		/// </summary>
		/// <param name="ctrl">Specifies the <see cref="Control"/> to check the style for.</param>
		/// <param name="flag">Specifies the style to check.</param>
		/// <returns><see langword="true"/> if the specified style is set; otherwise, <see langword="false"/>.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="ctrl"/> is <see langword="null"/>.
		/// </exception>
		public static bool GetStyle(Control ctrl, ControlStyles flag)
		{
			if (ctrl == null)
			{
				throw new ArgumentNullException("ctrl");
			}

			return InvokeGetStyle(ctrl, flag);
		}

		/*
		 * Make180CCWGraphics
		 */

		/// <summary>
		/// Rotates the specified <see cref="Graphics"/> at 180 degrees counter-clock-wise.
		/// </summary>
		/// <param name="g"></param>
		/// <param name="bounds"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// </exception>
		public static void Make180CCWGraphics(Graphics g, Rectangle bounds)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			g.RotateTransform(-180);
			g.TranslateTransform(-bounds.Width + 1, -bounds.Height + 1);
		}

		/*
		 * Make90CCWGraphics
		 */

		/// <summary>
		/// Rotates the specified <see cref="Graphics"/> at 90 degrees counter-clock-wise.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// </exception>
		public static void Make90CCWGraphics(Graphics g, Rectangle bounds)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			g.RotateTransform(-90);
			g.TranslateTransform(-bounds.Height + 1, 0);
		}

		/*
		 * Make90CWGraphics
		 */

		/// <summary>
		/// Rotates the specified <see cref="Graphics"/> at 90 degrees clock-wise.
		/// </summary>
		/// <param name="g"></param>
		/// <param name="bounds"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// </exception>
		public static void Make90CWGraphics(Graphics g, Rectangle bounds)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			g.RotateTransform(90);
			g.TranslateTransform(0, -bounds.Height + 1);
		}

		/*
		 * SetGraphicsDefaultQuality
		 */

		/// <summary>
		/// Apply default quality settings for the specified <see cref="T:System.Drawing.Graphics"/>.
		/// </summary>
		/// <param name="g">Specifies the <see cref="T:System.Drawing.Graphics"/> to set the quality settings for.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="g"/> is <see langword="null"/>.
		/// </exception>
		public static void SetGraphicsDefaultQuality(Graphics g)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			g.PixelOffsetMode = PixelOffsetMode.Default;
			g.SmoothingMode = SmoothingMode.Default;
			g.TextRenderingHint = TextRenderingHint.SystemDefault;
		}

		/*
		 * SetGraphicsHighQuality
		 */

		/// <summary>
		/// Apply high quality settings for the specified <see cref="T:System.Drawing.Graphics"/>.
		/// <see cref="P:System.Drawing.Graphics.PixelOffsetMode"/> = <see cref="F:System.Drawing.Drawing2D.PixelOffsetMode.HighQuality"/>.
		/// <see cref="P:System.Drawing.Graphics.SmoothingMode"/> = <see cref="F:System.Drawing.Drawing2D.SmoothingMode.HighQuality"/>.
		/// <see cref="P:System.Drawing.Graphics.TextRenderingHint"/> = <see cref="F:System.Drawing.Drawing2D.TextRenderingHint.AntiAliasGridFit"/>.
		/// </summary>
		/// <param name="g">Specifies the <see cref="T:System.Drawing.Graphics"/> to set the quality settings for.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="g"/> is <see langword="null"/>.
		/// </exception>
		public static void SetGraphicsHighQuality(Graphics g)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			g.PixelOffsetMode = PixelOffsetMode.HighQuality;
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
		}

		/*
		 * SetGraphicsVeryHighQuality
		 */

		/// <summary>
		/// Apply very high quality settings for the specified <see cref="T:System.Drawing.Graphics"/>.
		/// <see cref="P:System.Drawing.Graphics.PixelOffsetMode"/> = <see cref="F:System.Drawing.Drawing2D.PixelOffsetMode.HighQuality"/>.
		/// <see cref="P:System.Drawing.Graphics.SmoothingMode"/> = <see cref="F:System.Drawing.Drawing2D.SmoothingMode.AntiAlias"/>.
		/// <see cref="P:System.Drawing.Graphics.TextRenderingHint"/> = <see cref="F:System.Drawing.Drawing2D.TextRenderingHint.ClearTypeGridFit"/>.
		/// </summary>
		/// <param name="g">Specifies the <see cref="T:System.Drawing.Graphics"/> to set the quality settings for.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="g"/> is <see langword="null"/>.
		/// </exception>
		public static void SetGraphicsVeryHighQuality(Graphics g)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			g.PixelOffsetMode = PixelOffsetMode.HighQuality;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
		}

		/*
		 * SetStyle
		 */

		/// <summary>
		/// Sets the specified style for the specified <see cref="T:System.Windows.Forms.Control"/>.
		/// </summary>
		/// <param name="ctrl">Specifies the <see cref="T:System.Windows.Forms.Control"/> to set the style for.</param>
		/// <param name="ctrlStyles">Specifies the style to set.</param>
		/// <param name="value"><see langword="true"/> to set the style for the <see cref="T:System.Windows.Forms.Control"/>;
		/// otherwise, <see langword="false"/>.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="ctrl"/> is <see langword="null"/>.
		/// </exception>
		public static void SetStyle(Control ctrl, ControlStyles ctrlStyles, bool value)
		{
			if (ctrl == null)
			{
				throw new ArgumentNullException("ctrl");
			}

			NuGenControlPaint.InvokeSetStyle(ctrl, ctrlStyles, value);
		}
	}
}
