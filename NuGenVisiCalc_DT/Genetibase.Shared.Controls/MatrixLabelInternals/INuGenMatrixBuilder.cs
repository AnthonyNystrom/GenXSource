/* -----------------------------------------------
 * INuGenMatrixBuilder.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.MatrixLabelInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenMatrixBuilder
	{
		/// <summary>
		/// Set <see langword="true"/> to highlight a dot in the matrix. A char is rendered as a set
		/// of highlighted dots.
		/// </summary>
		void BuildCharMatrix(Char c, Boolean[,] charMatrix);
	}
}
