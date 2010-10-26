/* -----------------------------------------------
 * EditorScrollBar.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Input;

namespace Genetibase.Windows.Controls
{
    /// <summary>
    /// Represents the <see cref="ScrollBar"/> which allows the <see cref="EditorView"/> to scroll its content properly.
    /// </summary>
    public class EditorScrollBar : ScrollBar
    {
        private Boolean _inShiftLeftClick;

        /// <summary>
        /// </summary>
        public event EventHandler<RoutedPropertyChangedEventArgs<Double>> LeftShiftClick;

        /// <summary>
        /// Provides class handling for the <see cref="E:System.Windows.UIElement.PreviewMouseLeftButtonDown"/> event.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            try
            {
                _inShiftLeftClick = (Keyboard.Modifiers | ModifierKeys.Shift) == Keyboard.Modifiers;
                base.OnPreviewMouseLeftButtonDown(e);
            }
            finally
            {
                _inShiftLeftClick = false;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Controls.Primitives.RangeBase.ValueChanged"/> routed event.
        /// </summary>
        /// <param name="oldValue">Old value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value"/> property</param>
        /// <param name="newValue">New value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value"/> property</param>
        protected override void OnValueChanged(Double oldValue, Double newValue)
        {
            base.OnValueChanged(oldValue, newValue);

            if (_inShiftLeftClick)
            {
                EventHandler<RoutedPropertyChangedEventArgs<Double>> leftShiftClick = this.LeftShiftClick;

                if (leftShiftClick != null)
                {
                    leftShiftClick(this, new RoutedPropertyChangedEventArgs<Double>(oldValue, newValue));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorScrollBar"/> class.
        /// </summary>
        public EditorScrollBar()
        {
        }
    }
}
