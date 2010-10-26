/* -----------------------------------------------
 * CodeEditor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Genetibase.Windows.Controls.Code.UserInterface;
using Genetibase.Windows.Controls.Data.Text;
using Genetibase.Windows.Controls.Editor;
using Genetibase.Windows.Controls.Editor.Text.View;
using Genetibase.Windows.Controls.Framework.Commands;
using Genetibase.Windows.Controls.Logic.Text.Find;
using Genetibase.Windows.Controls.Logic.Text.Undo;
using System.Windows.Input;
using System.ComponentModel;

namespace Genetibase.Windows.Controls.Code.Documents
{
	/// <summary>
	/// </summary>
	public sealed class CodeEditor : ICodeEditor, IDisposable
	{
		private CodeOptionsModel codeOptionsModel;
		private ICommandManager commandManager;
		private IEditorCommands editorCommands;
		private IFindLogic findLogic;
		private IEditorView editorView;
		private IEditorViewHost editorViewHost;
		private IUndoManager undoManager;

		/// <summary>
		/// </summary>
		public event EventHandler LostFocus;

		internal CodeEditor(
			ICommandManager commandManager
			, IEditorView editorView
			, IEditorViewHost editorViewHost
			, IEditorCommands editorCommands
			, IUndoManager undoManager
			, IFindLogic findLogic
			, CodeOptionsModel codeOptionsModel
			)
		{
			this.commandManager = commandManager;
			this.editorView = editorView;
			this.editorViewHost = editorViewHost;
			this.findLogic = findLogic;
			this.editorCommands = editorCommands;
			this.undoManager = undoManager;
			FrameworkElement element = this.Element;
			element.PreviewLostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.Editor_LostFocus);
			element.LostFocus += new RoutedEventHandler(this.Editor_LostFocus);
			if (this.commandManager != null)
			{
				this.commandManager.CommandExecuting += new CommandExecutionEventHandler(this.Editor_LostFocus);
			}
			this.editorView.Background = Brushes.White;
			TextFormattingRunProperties.DefaultProperties.SetTypeface(new Typeface(FontName));
			TextFormattingRunProperties.DefaultProperties.SetFontRenderingEmSize(FontSize);
			this.editorViewHost.LineNumberGutterForegroundColor = Colors.Black;
			this.editorViewHost.LineNumberGutterTypeface = new Typeface(FontName);
			this.editorViewHost.LineNumberGutterFontSize = FontSize;
			this.editorView.Invalidate();
			this.codeOptionsModel = codeOptionsModel;
			this.codeOptionsModel.PropertyChanged += new PropertyChangedEventHandler(this.CodeOptionsModel_PropertyChanged);
			this.UpdateOptions();
		}

		private static Boolean Clamp(ref Int32 value, Int32 maxValue)
		{
			if (value > maxValue)
			{
				value = maxValue;
				return true;
			}
			return false;
		}

		private Boolean ClampToLength(ref Int32 offset)
		{
			return Clamp(ref offset, Math.Max(0, this.TextBuffer.Length - 1));
		}

		private Boolean ClampToLineCount(ref Int32 line)
		{
			return Clamp(ref line, Math.Max(0, this.TextBuffer.LineCount - 1));
		}

		/// <summary>
		/// </summary>
		public void ClearUndoStack()
		{
			this.undoManager.Clear();
		}

		private void CodeOptionsModel_PropertyChanged(Object sender, EventArgs e)
		{
			this.UpdateOptions();
		}

		/// <summary>
		/// </summary>
		public void Dispose()
		{
			FrameworkElement element = this.Element;
			element.PreviewLostKeyboardFocus -= new KeyboardFocusChangedEventHandler(this.Editor_LostFocus);
			element.LostFocus -= new RoutedEventHandler(this.Editor_LostFocus);
			if (this.commandManager != null)
			{
				this.commandManager.CommandExecuting -= new CommandExecutionEventHandler(this.Editor_LostFocus);
			}
			this.codeOptionsModel.PropertyChanged -= new PropertyChangedEventHandler(this.CodeOptionsModel_PropertyChanged);
		}

		private void Editor_LostFocus(Object sender, EventArgs e)
		{
			if (this.LostFocus != null)
			{
				this.LostFocus(this, e);
			}
		}

		/// <summary>
		/// </summary>
		public void Focus()
		{
			this.editorView.VisualElement.Focus();
		}

		void ICodeEditor.ClearSelection()
		{
			this.TextView.Selection.Clear();
		}

		void ICodeEditor.EnsureCaretVisible()
		{
			this.TextView.Caret.EnsureVisible();
		}

		void ICodeEditor.EnsureSpanVisible(Int32 start, Int32 length)
		{
			TextSpan span = new TextSpan(this.TextBuffer, start, length);
			new TextViewHelper(this.TextView).EnsureSpanVisible(span, 0, 0);
		}

		Boolean ICodeEditor.Find(String findText, Boolean matchCase, Boolean searchReverse)
		{
			Int32 characterIndex = this.editorView.Caret.Position.CharacterIndex;
			if (((this.TextView.Selection.Count > 0) && (this.TextView.Selection.ActiveSpan != null)) && (this.TextView.Selection.ActiveSpan.Length > 0))
			{
				if (searchReverse)
				{
					characterIndex = this.TextView.Selection.ActiveSpan.End - 1;
				}
				else
				{
					characterIndex = this.TextView.Selection.ActiveSpan.End;
				}
			}
			this.findLogic.MatchCase = matchCase;
			this.findLogic.SearchString = findText;
			this.findLogic.SearchReverse = searchReverse;
			TextSpan span = this.findLogic.FindNext(characterIndex, false);
			if (span != null)
			{
				ICodeEditor editor = this;
				editor.ClearSelection();
				editor.Select(span.Start, span.Length);
				editor.EnsureSpanVisible(span.Start, span.Length);
				editor.CaretPosition = span.End;
				editor.EnsureCaretVisible();
				return true;
			}
			return false;
		}

		Int32 ICodeEditor.GetLengthOfLineFromLineNumber(Int32 value)
		{
			if (this.ClampToLineCount(ref value))
			{
				return 0;
			}
			return this.TextBuffer.GetLengthOfLineFromLineNumber(value);
		}

		Int32 ICodeEditor.GetLineNumberFromPosition(Int32 value)
		{
			if (this.ClampToLength(ref value))
			{
				return this.TextBuffer.LineCount;
			}
			return this.TextBuffer.GetLineNumberFromPosition(value);
		}

		Int32 ICodeEditor.GetLineOffsetFromPosition(Int32 value)
		{
			this.ClampToLength(ref value);
			return this.TextBuffer.GetLineOffsetFromPosition(value);
		}

		Int32 ICodeEditor.GetStartOfLineFromLineNumber(Int32 value)
		{
			this.ClampToLineCount(ref value);
			return this.TextBuffer.GetStartOfLineFromLineNumber(value);
		}

		Int32 ICodeEditor.GetStartOfNextLineFromPosition(Int32 value)
		{
			this.ClampToLength(ref value);
			return this.TextBuffer.GetStartOfNextLineFromPosition(value);
		}

		void ICodeEditor.GotoLine(Int32 lineNumber)
		{
			this.ClampToLineCount(ref lineNumber);
			this.EditorCommands.GotoLine(lineNumber);
		}

		void ICodeEditor.MoveLineToCenterOfView(Int32 lineNumber)
		{
			this.ClampToLineCount(ref lineNumber);
			ITextView textView = this.TextView;
			Double num = textView.TotalContentHeight / ((Double)textView.TextBuffer.LineCount);
			Int32 num2 = lineNumber;
			Double num3 = num * (num2 - 1);
			Double lineOffset = textView.ViewportHeight / 2;
			if (lineOffset > num3)
			{
				textView.DisplayLine(0, 0, ViewRelativePosition.Top);
			}
			else
			{
				textView.DisplayLine(num2, lineOffset, ViewRelativePosition.Top);
			}
		}

		Boolean ICodeEditor.Replace(String findText, String replaceText, Boolean matchCase)
		{
			if (this.TextView.Selection.Count == 1)
			{
				Int32 start = this.TextView.Selection.ActiveSpan.Start;
				this.findLogic.MatchCase = matchCase;
				this.findLogic.SearchString = findText;
				this.findLogic.SearchReverse = false;
				TextSpan span = this.findLogic.FindNext(start, false);
				if ((span != null) && this.TextView.Selection.ActiveSpan.Equals(span))
				{
					this.EditorCommands.ReplaceSelection(replaceText);
				}
			}
			ICodeEditor editor = this;
			return editor.Find(findText, matchCase, false);
		}

		Boolean ICodeEditor.ReplaceAll(String findText, String replaceText, Boolean matchCase)
		{
			return (this.EditorCommands.ReplaceAllMatches(findText, replaceText, matchCase, false) > 0);
		}

		void ICodeEditor.Select(Int32 start, Int32 length)
		{
			TextSpan span = new TextSpan(this.TextBuffer, start, length);
			this.TextView.Selection.Select(span);
		}

		void ICodeEditor.SelectCurrentWord()
		{
			this.EditorCommands.SelectCurrentWord();
		}

		private void UpdateOptions()
		{
			this.EditorCommands.ConvertTabsToSpace = this.codeOptionsModel.ConvertTabsToSpace;
			this.TextView.TabSize = this.codeOptionsModel.TabSize;
		}

		internal IEditorCommands EditorCommands
		{
			get
			{
				return this.editorCommands;
			}
		}

		/// <summary>
		/// </summary>
		public FrameworkElement Element
		{
			get
			{
				return this.editorViewHost.HostControl;
			}
		}

		/// <summary>
		/// </summary>
		public static String FontName
		{
			get
			{
				switch (CultureInfo.CurrentCulture.ThreeLetterWindowsLanguageName)
				{
					case "ENU":
					return "Courier New";

					case "JPN":
					return "MS Gothic";

					case "KOR":
					return "Dotum Che";

					case "CHS":
					return "NSimSun";

					case "CHT":
					return "MingLiu";
				}
				return "Courier New";
			}
		}

		/// <summary>
		/// </summary>
		public static Double FontSize
		{
			get
			{
				return 12;
			}
		}

		Boolean ICodeEditor.CanRedo
		{
			get
			{
				return this.undoManager.CanRedo;
			}
		}

		Boolean ICodeEditor.CanUndo
		{
			get
			{
				return this.undoManager.CanUndo;
			}
		}

		Int32 ICodeEditor.CaretPosition
		{
			get
			{
				return this.TextView.Caret.Position.CharacterIndex;
			}
			set
			{
				this.ClampToLength(ref value);
				this.TextView.Caret.MoveTo(value, CaretPlacement.LeftOfCharacter);
			}
		}

		Boolean ICodeEditor.IsSelectionEmpty
		{
			get
			{
				return this.TextView.Selection.IsEmpty;
			}
		}

		Int32 ICodeEditor.SelectionLength
		{
			get
			{
				return this.TextView.Selection.ActiveSpan.Length;
			}
		}

		Int32 ICodeEditor.SelectionStart
		{
			get
			{
				return this.TextView.Selection.ActiveSpan.Start;
			}
		}

		String ICodeEditor.Text
		{
			get
			{
				return this.TextBuffer.GetText();
			}
		}

		/// <summary>
		/// </summary>
		public Boolean ReadOnly
		{
			get
			{
				return this.editorViewHost.IsReadOnly;
			}
			set
			{
				this.editorViewHost.IsReadOnly = value;
			}
		}

		/// <summary>
		/// </summary>
		public String Text
		{
			get
			{
				return this.TextBuffer.GetText();
			}
			set
			{
				this.TextBuffer.Replace(0, this.TextBuffer.Length, value);
			}
		}

		/// <summary>
		/// </summary>
		public TextBuffer TextBuffer
		{
			get
			{
				return this.editorView.TextBuffer;
			}
		}

		internal ITextView TextView
		{
			get
			{
				return this.editorView;
			}
		}

		/// <summary>
		/// </summary>
		public Thickness VerticalScrollBarMargin
		{
			get
			{
				return this.editorViewHost.VerticalScrollBarMargin;
			}
			set
			{
				this.editorViewHost.VerticalScrollBarMargin = value;
			}
		}
	}
}
