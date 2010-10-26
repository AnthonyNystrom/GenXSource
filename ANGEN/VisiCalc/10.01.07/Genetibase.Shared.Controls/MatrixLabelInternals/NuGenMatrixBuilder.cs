/* -----------------------------------------------
 * NuGenMatrixBuilder.cs
 * Copyright � 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.MatrixLabelInternals
{
	/// <summary>
	/// </summary>
	public class NuGenMatrixBuilder : INuGenMatrixBuilder
	{
		/// <summary>
		/// Set <see langword="true"/> to highlight a dot in the matrix. A char is rendered as a set
		/// of highlighted dots.
		/// </summary>
		/// <param name="c"></param>
		/// <param name="charMatrix"></param>
		public virtual void BuildCharMatrix(Char c, Boolean[,] charMatrix)
		{
			Debug.Assert(charMatrix != null, "charMatrix != null");

			switch (c)
			{
				case 'a':
				case '�': // cyr
				{
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[4, 4] = true;
					break;
				}
				case 'b':
				case '�': // cyr
				{
					charMatrix[0, 0] = true;
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case 'c':
				case '�': // cyr
				{
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case 'd':
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case 'e':
				case '�': // cyr
				case '�': // cyr
				{
					charMatrix[0, 0] = true;
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[0, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[0, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					charMatrix[4, 4] = true;
					break;
				}
				case 'f':
				{
					charMatrix[0, 0] = true;
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[0, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;

					charMatrix[0, 3] = true;

					charMatrix[0, 4] = true;
					break;
				}
				case 'g':
				{
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case 'h':
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[4, 4] = true;
					break;
				}
				case 'i':
				{
					charMatrix[0, 0] = true;
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[2, 1] = true;

					charMatrix[2, 2] = true;

					charMatrix[2, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					charMatrix[4, 4] = true;
					break;
				}
				case 'j':
				{
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[3, 1] = true;

					charMatrix[3, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[3, 3] = true;

					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					break;
				}
				case 'k':
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[3, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[3, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[4, 4] = true;
					break;
				}
				case 'l':
				{
					charMatrix[0, 0] = true;

					charMatrix[0, 1] = true;

					charMatrix[0, 2] = true;

					charMatrix[0, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					charMatrix[4, 4] = true;
					break;
				}
				case 'm':
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[1, 1] = true;
					charMatrix[3, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[4, 4] = true;
					break;
				}
				case 'n':
				{
					charMatrix[0, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[1, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[3, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[4, 4] = true;
					break;
				}
				case 'o':
				case '�': // cyr
				{
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case 'p':
				case '�': // cyr
				{
					charMatrix[0, 0] = true;
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;

					charMatrix[0, 3] = true;

					charMatrix[0, 4] = true;
					break;
				}
				case 'q':
				{
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[3, 3] = true;

					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[4, 4] = true;
					break;
				}
				case 'r':
				{
					charMatrix[0, 0] = true;
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[3, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[4, 4] = true;
					break;
				}
				case 's':
				{
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;

					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;

					charMatrix[4, 3] = true;

					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case 't':
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[2, 1] = true;

					charMatrix[2, 2] = true;

					charMatrix[2, 3] = true;

					charMatrix[2, 4] = true;
					break;
				}
				case 'u':
				{
					charMatrix[0, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case 'v':
				{
					charMatrix[0, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[1, 2] = true;
					charMatrix[3, 2] = true;

					charMatrix[1, 3] = true;
					charMatrix[3, 3] = true;

					charMatrix[2, 4] = true;
					break;
				}
				case 'w':
				{
					charMatrix[0, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[2, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[2, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[1, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case 'x':
				case '�': // cyr
				{
					charMatrix[0, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[1, 1] = true;
					charMatrix[3, 1] = true;

					charMatrix[2, 2] = true;

					charMatrix[1, 3] = true;
					charMatrix[3, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[4, 4] = true;
					break;
				}
				case 'y':
				{
					charMatrix[0, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[1, 1] = true;
					charMatrix[3, 1] = true;

					charMatrix[2, 2] = true;

					charMatrix[2, 3] = true;

					charMatrix[2, 4] = true;
					break;
				}
				case 'z':
				{
					charMatrix[0, 0] = true;
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[3, 1] = true;

					charMatrix[2, 2] = true;

					charMatrix[1, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					charMatrix[4, 4] = true;
					break;
				}
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[0, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[0, 2] = true;
					charMatrix[0, 3] = true;
					charMatrix[0, 4] = true;
					break;
				}
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[1, 1] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 3] = true;
					charMatrix[4, 4] = true;

					charMatrix[4, 0] = true;
					charMatrix[3, 1] = true;
					charMatrix[1, 3] = true;
					charMatrix[0, 4] = true;

					charMatrix[2, 0] = true;
					charMatrix[2, 1] = true;
					charMatrix[2, 3] = true;
					charMatrix[2, 4] = true;
					break;
				}
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[4, 1] = true;

					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;

					charMatrix[4, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[4, 4] = true;

					charMatrix[3, 1] = true;
					charMatrix[2, 2] = true;
					charMatrix[1, 3] = true;
					break;
				}
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[4, 4] = true;

					charMatrix[3, 1] = true;
					charMatrix[2, 2] = true;
					charMatrix[1, 3] = true;

					charMatrix[2, 0] = true;
					break;
				}
				case '�':
				{
					charMatrix[4, 0] = true;
					charMatrix[4, 1] = true;
					charMatrix[4, 2] = true;
					charMatrix[4, 3] = true;
					charMatrix[4, 4] = true;

					charMatrix[3, 1] = true;
					charMatrix[2, 2] = true;
					charMatrix[1, 3] = true;
					charMatrix[0, 4] = true;
					break;
				}
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[0, 1] = true;
					charMatrix[0, 2] = true;
					charMatrix[0, 3] = true;
					charMatrix[0, 4] = true;

					charMatrix[4, 0] = true;
					charMatrix[4, 1] = true;
					charMatrix[4, 2] = true;
					charMatrix[4, 3] = true;
					charMatrix[4, 4] = true;

					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;
					break;
				}
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[1, 1] = true;
					charMatrix[2, 2] = true;

					charMatrix[4, 0] = true;
					charMatrix[3, 1] = true;

					charMatrix[1, 3] = true;
					charMatrix[0, 4] = true;
					break;
				}
				case '�':
				{
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[2, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;

					charMatrix[2, 3] = true;
					charMatrix[2, 4] = true;
					break;
				}
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[1, 3] = true;
					charMatrix[2, 3] = true;
					charMatrix[3, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[4, 4] = true;
					break;
				}
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[0, 1] = true;

					charMatrix[4, 0] = true;
					charMatrix[4, 1] = true;
					charMatrix[4, 2] = true;
					charMatrix[4, 3] = true;
					charMatrix[4, 4] = true;

					charMatrix[0, 2] = true;
					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;
					break;
				}
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[2, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[2, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					charMatrix[4, 4] = true;
					break;
				}
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[1, 3] = true;
					charMatrix[2, 3] = true;
					charMatrix[3, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[4, 4] = true;

					charMatrix[2, 1] = true;
					charMatrix[2, 2] = true;
					break;
				}
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[1, 0] = true;

					charMatrix[1, 1] = true;
					charMatrix[1, 2] = true;
					charMatrix[1, 3] = true;
					charMatrix[1, 4] = true;

					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;

					charMatrix[4, 3] = true;

					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[0, 1] = true;
					charMatrix[0, 2] = true;
					charMatrix[0, 3] = true;
					charMatrix[0, 4] = true;

					charMatrix[4, 0] = true;
					charMatrix[4, 1] = true;
					charMatrix[4, 2] = true;
					charMatrix[4, 3] = true;
					charMatrix[4, 4] = true;

					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[2, 3] = true;
					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					break;
				}
				case '�':
				{
					charMatrix[1, 0] = true;

					charMatrix[1, 1] = true;
					charMatrix[1, 2] = true;
					charMatrix[1, 3] = true;
					charMatrix[1, 4] = true;

					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;

					charMatrix[4, 3] = true;

					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case '�':
				{
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[4, 1] = true;

					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[4, 3] = true;

					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case '�':
				{
					charMatrix[0, 0] = true;
					charMatrix[0, 1] = true;
					charMatrix[0, 2] = true;
					charMatrix[0, 3] = true;
					charMatrix[0, 4] = true;

					charMatrix[1, 2] = true;

					charMatrix[3, 0] = true;

					charMatrix[2, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[2, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[2, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[3, 4] = true;
					
					charMatrix[2, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[2, 4] = true;
					charMatrix[4, 4] = true;
					break;
				}
				case '�':
				{
					charMatrix[4, 0] = true;
					charMatrix[4, 1] = true;
					charMatrix[4, 2] = true;
					charMatrix[4, 3] = true;
					charMatrix[4, 4] = true;

					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;

					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;

					charMatrix[1, 3] = true;
					charMatrix[0, 4] = true;
					break;
				}
				case '1':
				{
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[2, 1] = true;

					charMatrix[2, 2] = true;

					charMatrix[2, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					charMatrix[4, 4] = true;
					break;
				}
				case '2':
				{
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;

					charMatrix[1, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					charMatrix[4, 4] = true;
					break;
				}
				case '3':
				{
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case '4':
				{
					charMatrix[0, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[3, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[3, 3] = true;

					charMatrix[3, 4] = true;
					break;
				}
				case '5':
				{
					charMatrix[0, 0] = true;
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[0, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;

					charMatrix[4, 3] = true;

					charMatrix[0, 4] = true;
					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case '6':
				{
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case '7':
				{
					charMatrix[0, 0] = true;
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;
					charMatrix[4, 0] = true;

					charMatrix[4, 1] = true;

					charMatrix[3, 2] = true;

					charMatrix[2, 3] = true;

					charMatrix[2, 4] = true;
					break;
				}
				case '8':
				{
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case '9':
				{
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[1, 2] = true;
					charMatrix[2, 2] = true;
					charMatrix[3, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[4, 3] = true;

					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;
					break;
				}
				case '0':
				{
					charMatrix[1, 0] = true;
					charMatrix[2, 0] = true;
					charMatrix[3, 0] = true;

					charMatrix[0, 1] = true;
					charMatrix[4, 1] = true;

					charMatrix[0, 2] = true;
					charMatrix[4, 2] = true;

					charMatrix[0, 3] = true;
					charMatrix[4, 3] = true;

					charMatrix[1, 4] = true;
					charMatrix[2, 4] = true;
					charMatrix[3, 4] = true;

					charMatrix[3, 1] = true;
					charMatrix[2, 2] = true;
					charMatrix[1, 3] = true;
					break;
				}
				case ':':
				{
					charMatrix[2, 1] = true;

					charMatrix[2, 3] = true;
					break;
				}
				case '.':
				{
					charMatrix[2, 4] = true;
					break;
				}
				case '/':
				{
					charMatrix[4, 0] = true;
					charMatrix[3, 1] = true;
					charMatrix[2, 2] = true;
					charMatrix[1, 3] = true;
					charMatrix[0, 4] = true;
					break;
				}
				case ',':
				{
					charMatrix[2, 2] = true;
					charMatrix[2, 3] = true;
					charMatrix[1, 4] = true;
					break;
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMatrixBuilder"/> class.
		/// </summary>
		public NuGenMatrixBuilder()
		{
		}
	}
}
