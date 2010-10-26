/* -----------------------------------------------
 * NuGenBorderPaintParams.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public class NuGenBorderPaintParams : NuGenPaintParams
	{
		private bool _drawBorder;

		/// <summary>
		/// </summary>
		public bool DrawBorder
		{
			get
			{
				return _drawBorder;
			}
			set
			{
				_drawBorder = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenBorderPaintParams"/> class.
		/// </summary>
		/// <param name="g"></param>
		/// <exception cref="ArgumentNullException"><paramref name="g"/> is <see langword="null"/>.</exception>
		public NuGenBorderPaintParams(Graphics g)
			: base(g)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenBorderPaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="initializeFrom"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenBorderPaintParams(NuGenPaintParams initializeFrom)
			: base(initializeFrom)
		{
		}
	}
}
