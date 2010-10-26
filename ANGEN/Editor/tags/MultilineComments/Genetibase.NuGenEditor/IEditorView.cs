/* -----------------------------------------------
 * IEditorView.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Genetibase.Windows.Controls.Editor.View;

namespace Genetibase.Windows.Controls
{
    /// <summary>
    /// </summary>
    public interface IEditorView
    {
        /// <summary>
        /// </summary>
        IEditorCommands EditorCommandPrimitives { get; }

        /// <summary>
        /// </summary>
        Thickness HorizontalScrollBarMargin { get; set; }

        /// <summary>
        /// </summary>
        Control HostControl { get; }

        /// <summary>
        /// </summary>
        Boolean IsLineNumberGutterVisible { get; set; }

        /// <summary>
        /// </summary>
        Boolean IsReadOnly { get; set; }

        /// <summary>
        /// </summary>
        Brush LineNumberGutterBackgroundBrush { get; set; }

        /// <summary>
        /// </summary>
        Double LineNumberGutterFontSize { get; set; }

        /// <summary>
        /// </summary>
        Color LineNumberGutterForegroundColor { get; set; }

        /// <summary>
        /// </summary>
        Typeface LineNumberGutterTypeface { get; set; }

        /// <summary>
        /// </summary>
        Brush OverwriteCaretBrush { get; set; }

        /// <summary>
        /// </summary>
        IEditorArea TextView { get; }

        /// <summary>
        /// </summary>
        Thickness VerticalScrollBarMargin { get; set; }
    }
}
