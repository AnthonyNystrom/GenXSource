/* -----------------------------------------------
 * IEditorArea.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Windows;
using System.Windows.Media;
using Genetibase.Windows.Controls.Editor;
using Genetibase.Windows.Controls.Editor.View;

namespace Genetibase.Windows.Controls
{
    /// <summary>
    /// </summary>
    public interface IEditorArea : ITextArea, IPropertyOwner
    {
        /// <summary>
        /// </summary>
        Brush Background { get; set; }

        /// <summary>
        /// </summary>
        FrameworkElement VisualElement { get; }
    }
}
