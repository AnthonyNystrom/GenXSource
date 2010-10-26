/* -----------------------------------------------
 * NuGenGrfxMode.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Genetibase.Shared.Drawing
{
	/// <summary>
	/// <para>Stores the state of the specified <see cref="Graphics"/> instance. Resets the property values
	/// after <see cref="NuGenGrfxMode.Dispose"/> method was called (invokes protected <see cref="NuGenGrfxMode.ResetGrfx"/> method
	/// internally).</para>
	/// <para>The following properties are supported:</para>
	/// <para><see cref="System.Drawing.Graphics.PixelOffsetMode"/></para>
	/// <para><see cref="System.Drawing.Graphics.SmoothingMode"/></para>
	/// <para><see cref="System.Drawing.Graphics.TextRenderingHint"/></para>
	/// </summary>
	/// <example>
	/// using System.Drawing;
	/// using System.Drawing.Drawing2D;
	/// 
	/// ...
	/// 
	/// private void DrawRoundRectangle(Graphics g)
	/// {
	///		// Suppose g.SmoothingMode = SmoothingMode.None here.
	///		using (NuGenGrfxMode mode = new NuGenGrfxMode(g))
	///		{
	///			g.SmoothingMode = SmoothingMode.AntiAlias;
	///			// Some drawing here using g.
	///		} // mode.Dispose() is called at this point.
	/// 
	///		// g.SmoothingMode = SmoothingMode.None again after mode.Dispose() method was invoked.
	/// }
	/// </example>
	public sealed class NuGenGrfxMode : IDisposable
	{
		/// <summary>
		/// Resets the properties of the specified <see cref="Graphics"/> instance.
		/// </summary>
		private void ResetGrfx()
		{
			if (_grfx != null)
			{
				_grfx.PixelOffsetMode = _oldPixelOffsetMode;
				_grfx.SmoothingMode = _oldSmoothingMode;
				_grfx.TextRenderingHint = _oldTextRenderingHint;
			}
		}

		private Graphics _grfx;
		private PixelOffsetMode _oldPixelOffsetMode;
		private SmoothingMode _oldSmoothingMode;
		private TextRenderingHint _oldTextRenderingHint;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenGrfxMode"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="grfx"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenGrfxMode(Graphics grfx)
		{
			if (grfx == null)
			{
				throw new ArgumentNullException("grfx");
			}

			_grfx = grfx;

			_oldPixelOffsetMode = _grfx.PixelOffsetMode;
			_oldSmoothingMode = _grfx.SmoothingMode;
			_oldTextRenderingHint = _grfx.TextRenderingHint;
		}

		/// <summary>
		/// Resets the properties of the specified <see cref="Graphics"/> instance.
		/// </summary>
		public void Dispose()
		{
			this.ResetGrfx();
		}
	}
}
