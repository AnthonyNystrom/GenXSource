/* -----------------------------------------------
 * IUndoUnit.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Windows.Controls.Logic.Text.Undo
{
	/// <summary>
	/// </summary>
	public interface IUndoUnit
	{
		/// <summary>
		/// </summary>
		void Redo();
		/// <summary>
		/// </summary>
		void Undo();

		/// <summary>
		/// </summary>
		String Description
		{
			get;
		}
	}
}
