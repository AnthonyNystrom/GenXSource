/* -----------------------------------------------
 * NuGenNativeGrfx.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Drawing
{
	/// <summary>
	/// Provides a double-buffered <see cref="Graphics"/> used for GDI drawing.
	/// </summary>
	public sealed class NuGenNativeGrfx : IDisposable
	{
		/// <summary>
		/// Returns the <see cref="Graphics"/> used as a buffer.
		/// </summary>
		public Graphics Graphics
		{
			get
			{
				return _memGrfx;
			}
		}

		/// <summary>
		/// Renders the buffered content.
		/// </summary>
		public void DrawToTargetGraphics()
		{
			_targetGrfx.DrawImageUnscaled(_memBmp, new Point(0, 0));
		}

		private Graphics _targetGrfx;
		private Graphics _memGrfx;
		private Bitmap _memBmp;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenNativeGrfx"/> class.
		/// </summary>
		public NuGenNativeGrfx(IntPtr hDC, Size size)
		{
			_targetGrfx = Graphics.FromHdc(hDC);
			_memBmp = new Bitmap(size.Width, size.Height);
			_memGrfx = Graphics.FromImage(_memBmp);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (_memGrfx != null)
			{
				_memGrfx.Dispose();
				_memGrfx = null;
			}

			if (_memBmp != null)
			{
				_memBmp.Dispose();
				_memBmp = null;
			}

			if (_targetGrfx != null)
			{
				_targetGrfx.Dispose();
				_targetGrfx = null;
			}
		}
	}
}
