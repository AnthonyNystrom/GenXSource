/* -----------------------------------------------
 * NuGenKeys.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Provides methods to process <see cref="Keys"/> data and perform other key related operations.
	/// </summary>
	public static class NuGenKeys
	{
		/// <summary>
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		public static Boolean IsArrowKey(Keys keyData)
		{
			Keys key = keyData & Keys.KeyCode;

			if (key == Keys.Up || key == Keys.Down || key == Keys.Left || key == Keys.Right)
			{
				return true;
			}

			return false;
		}
	}
}
