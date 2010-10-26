/* -----------------------------------------------
 * ICaretPosition.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Windows.Controls.Editor.Text.View
{
	/// <summary>
	/// </summary>
	public interface ICaretPosition
	{
		/// <summary>
		/// </summary>
		Int32 CharacterIndex
		{
			get;
		}

		/// <summary>
		/// </summary>
		CaretPlacement Placement
		{
			get;
		}

		/// <summary>
		/// </summary>
		Int32 TextInsertionIndex
		{
			get;
		}
	}
}
