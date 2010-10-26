/* -----------------------------------------------
 * IUndoManager.cs
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
	public interface IUndoManager
	{
		/// <summary>
		/// </summary>
		event EventHandler<UndoEventArgs> Redone;
		/// <summary>
		/// </summary>
		event EventHandler<UndoEventArgs> Undone;

		/// <summary>
		/// </summary>
		void AddUndoUnit(IUndoUnit undoUnit);
		/// <summary>
		/// </summary>
		void Clear();
		/// <summary>
		/// </summary>
		void Redo();
		/// <summary>
		/// </summary>
		void Undo();

		/// <summary>
		/// </summary>
		Boolean CanRedo
		{
			get;
		}
		/// <summary>
		/// </summary>
		Boolean CanUndo
		{
			get;
		}
		/// <summary>
		/// </summary>
		IUndoUnit TopRedoUnit
		{
			get;
		}
		/// <summary>
		/// </summary>
		IUndoUnit TopUndoUnit
		{
			get;
		}
	}
}
