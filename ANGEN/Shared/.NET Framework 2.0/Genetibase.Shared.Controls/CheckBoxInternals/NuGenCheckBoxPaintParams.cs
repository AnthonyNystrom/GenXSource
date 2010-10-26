/* -----------------------------------------------
 * NuGenCheckBoxPaintParams.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.CheckBoxInternals
{
	/// <summary>
	/// </summary>
	public class NuGenCheckBoxPaintParams : NuGenPaintParams
	{
		private CheckState _checkState;

		/// <summary>
		/// </summary>
		public CheckState CheckState
		{
			get
			{
				return _checkState;
			}
			set
			{
				_checkState = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCheckBoxPaintParams"/> class.
		/// </summary>
		/// <param name="g"></param>
		/// <exception cref="ArgumentNullException"><paramref name="g"/> is <see langword="null"/>.</exception>
		public NuGenCheckBoxPaintParams(Graphics g)
			: base(g)
		{
		}
	}
}
