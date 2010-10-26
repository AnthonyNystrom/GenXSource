/* -----------------------------------------------
 * IEditorCommands.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Windows.Controls.Data.Text;

namespace Genetibase.Windows.Controls.Editor.Text.View
{
	/// <summary>
	/// </summary>
	public interface IEditorCommands
	{
		/// <summary>
		/// </summary>
		void CopySelection();
		/// <summary>
		/// </summary>
		void CutSelection();
		/// <summary>
		/// </summary>
		void DeleteCharacterToLeft();
		/// <summary>
		/// </summary>
		void DeleteCharacterToRight();
		/// <summary>
		/// </summary>
		Boolean DeleteSelection();
		/// <summary>
		/// </summary>
		void DeleteWordToLeft();
		/// <summary>
		/// </summary>
		void DeleteWordToRight();
		/// <summary>
		/// </summary>
		void ExtendSelection(Int32 newEnd);
		/// <summary>
		/// </summary>
		void GotoLine(Int32 lineNumber);
		/// <summary>
		/// </summary>
		void IndentSelection();
		/// <summary>
		/// </summary>
		void InsertNewLine();
		/// <summary>
		/// </summary>
		void InsertTab();
		/// <summary>
		/// </summary>
		void InsertText(String text);
		/// <summary>
		/// </summary>
		void MoveCaretAndExtendSelection(ITextLine textLine, Double horizontalOffset);
		/// <summary>
		/// </summary>
		void MoveCharacterLeft(Boolean select);
		/// <summary>
		/// </summary>
		void MoveCharacterRight(Boolean select);
		/// <summary>
		/// </summary>
		void MoveCurrentLineToBottom();
		/// <summary>
		/// </summary>
		void MoveCurrentLineToTop();
		/// <summary>
		/// </summary>
		void MoveLineDown(Boolean select);
		/// <summary>
		/// </summary>
		void MoveLineUp(Boolean select);
		/// <summary>
		/// </summary>
		void MoveToEndOfDocument(Boolean select);
		/// <summary>
		/// </summary>
		void MoveToEndOfLine(Boolean select);
		/// <summary>
		/// </summary>
		void MoveToNextWord(Boolean select);
		/// <summary>
		/// </summary>
		void MoveToPreviousWord(Boolean select);
		/// <summary>
		/// </summary>
		void MoveToStartOfDocument(Boolean select);
		/// <summary>
		/// </summary>
		void MoveToStartOfLine(Boolean select);
		/// <summary>
		/// </summary>
		void PageDown(Boolean select);
		/// <summary>
		/// </summary>
		void PageUp(Boolean select);
		/// <summary>
		/// </summary>
		void Paste();
		/// <summary>
		/// </summary>
		void Redo();
		/// <summary>
		/// </summary>
		void RemovePreviousTab();
		/// <summary>
		/// </summary>
		Int32 ReplaceAllMatches(String searchText, String replaceText, Boolean matchCase, Boolean matchWholeWord);
		/// <summary>
		/// </summary>
		void ReplaceSelection(String text);
		/// <summary>
		/// </summary>
		void ReplaceText(Int32 start, Int32 length, String text);
		/// <summary>
		/// </summary>
		void ResetSelection();
		/// <summary>
		/// </summary>
		void ScrollDownAndMoveCaretIfNecessary();
		/// <summary>
		/// </summary>
		void ScrollUpAndMoveCaretIfNecessary();
		/// <summary>
		/// </summary>
		void SelectAll();
		/// <summary>
		/// </summary>
		void SelectCurrentWord();
		/// <summary>
		/// </summary>
		void SelectLine(Int32 lineNumber, Boolean extendSelection);
		/// <summary>
		/// </summary>
		void TextInput(TextInputState state, String text);
		/// <summary>
		/// </summary>
		void Undo();
		/// <summary>
		/// </summary>
		void UnindentSelection();

		/// <summary>
		/// </summary>
		Boolean CanPaste
		{
			get;
		}
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
		Boolean ConvertTabsToSpace
		{
			get;
			set;
		}
		/// <summary>
		/// </summary>
		Boolean OverwriteMode
		{
			get;
			set;
		}
		/// <summary>
		/// </summary>
		Span ProvisionalCompositionSpan
		{
			get;
		}
		/// <summary>
		/// </summary>
		ITextView TextView
		{
			get;
		}
	}


}
