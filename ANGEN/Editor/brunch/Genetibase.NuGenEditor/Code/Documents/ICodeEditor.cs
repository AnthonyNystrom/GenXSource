/* -----------------------------------------------
 * ICodeEditor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Windows.Controls.Code.Documents
{
	/// <summary>
	/// </summary>
	public interface ICodeEditor : IDisposable
	{
		/// <summary>
		/// </summary>
		void ClearSelection();
		/// <summary>
		/// </summary>
		void EnsureCaretVisible();
		/// <summary>
		/// </summary>
		void EnsureSpanVisible(Int32 start, Int32 length);
		/// <summary>
		/// </summary>
		Boolean Find(String findText, Boolean matchCase, Boolean searchReverse);
		/// <summary>
		/// </summary>
		void Focus();
		/// <summary>
		/// </summary>
		Int32 GetLengthOfLineFromLineNumber(Int32 value);
		/// <summary>
		/// </summary>
		Int32 GetLineNumberFromPosition(Int32 value);
		/// <summary>
		/// </summary>
		Int32 GetLineOffsetFromPosition(Int32 value);
		/// <summary>
		/// </summary>
		Int32 GetStartOfLineFromLineNumber(Int32 value);
		/// <summary>
		/// </summary>
		Int32 GetStartOfNextLineFromPosition(Int32 value);
		/// <summary>
		/// </summary>
		void GotoLine(Int32 lineNumber);
		/// <summary>
		/// </summary>
		void MoveLineToCenterOfView(Int32 lineNumber);
		/// <summary>
		/// </summary>
		Boolean Replace(String findText, String replaceText, Boolean matchCase);
		/// <summary>
		/// </summary>
		Boolean ReplaceAll(String findText, String replaceText, Boolean matchCase);
		/// <summary>
		/// </summary>
		void Select(Int32 start, Int32 length);
		/// <summary>
		/// </summary>
		void SelectCurrentWord();

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
		Int32 CaretPosition
		{
			get;
			set;
		}
		/// <summary>
		/// </summary>
		Boolean IsSelectionEmpty
		{
			get;
		}
		/// <summary>
		/// </summary>
		Int32 SelectionLength
		{
			get;
		}
		/// <summary>
		/// </summary>
		Int32 SelectionStart
		{
			get;
		}
		/// <summary>
		/// </summary>
		String Text
		{
			get;
		}
	}
}
