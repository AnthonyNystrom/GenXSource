/* -----------------------------------------------
 * EditorViewHelper.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Genetibase.Windows.Controls.Editor;
using Genetibase.Windows.Controls.Editor.View;

namespace Genetibase.Windows.Controls
{
    internal class EditorViewHelper : TextAreaHelper
    {
        private const Double _overlap = 0.2;
        private const Double _roundness = 0.6;
        private IEditorArea _textView;

        public EditorViewHelper(IEditorArea textView) : base(textView)
        {
            if (textView == null)
            {
                throw new ArgumentNullException("textView");
            }
            _textView = textView;
        }

        public Geometry GetMarkerGeometry(Span span)
        {
            if (span == null)
            {
                throw new ArgumentNullException("span");
            }
            if ((span.Start < 0) || (span.End > _textView.TextBuffer.Length))
            {
                throw new ArgumentOutOfRangeException("span");
            }
            if (span.IsEmpty)
            {
                return null;
            }
            PathGeometry geometry = new PathGeometry();
            geometry.FillRule = FillRule.Nonzero;
            IList<ITextLine> textLines = _textView.TextLines;
            if (textLines.Count == 0)
            {
                return null;
            }
            if ((span.Start > base.LastRenderedCharacter) || (span.End < base.FirstRenderedCharacter))
            {
                return null;
            }
            ITextLine item = null;
            ITextLine textLineClosestTo = null;
            if (span.Start < base.FirstRenderedCharacter)
            {
                item = textLines[0];
            }
            if (span.End > base.LastRenderedCharacter)
            {
                textLineClosestTo = textLines[textLines.Count - 1];
            }
            if (item == null)
            {
                item = base.GetTextLineClosestTo(span.Start);
            }
            if (textLineClosestTo == null)
            {
                textLineClosestTo = base.GetTextLineClosestTo(span.End);
            }
            if (item == textLineClosestTo)
            {
                foreach (TextBounds bounds in item.GetTextBounds(span))
                {
                    RectangleGeometry geometry2 = new RectangleGeometry(new Rect(bounds.Left - 0.2, bounds.Top - 1, bounds.Width + 0.4, bounds.Height + 1), 0.6, 0.6);
                    geometry2.Freeze();
                    geometry.AddGeometry(geometry2);
                }
            }
            else
            {
                foreach (TextBounds bounds2 in item.GetTextBounds(span))
                {
                    RectangleGeometry geometry3 = new RectangleGeometry(new Rect(bounds2.Left - 0.2, bounds2.Top - 1, bounds2.Width + 0.4, bounds2.Height + 1), 0.6, 0.6);
                    geometry3.Freeze();
                    geometry.AddGeometry(geometry3);
                }
                Int32 num = textLines.IndexOf(item) + 1;
                Int32 index = textLines.IndexOf(textLineClosestTo);
                for (Int32 i = num; i < index; i++)
                {
                    ITextLine line3 = textLines[i];
                    RectangleGeometry geometry4 = new RectangleGeometry(new Rect(-0.2, line3.VerticalOffset - 0.2, line3.Width, line3.Height + 0.4), 0.6, 0.6);
                    geometry4.Freeze();
                    geometry.AddGeometry(geometry4);
                }
                foreach (TextBounds bounds3 in textLineClosestTo.GetTextBounds(span))
                {
                    RectangleGeometry geometry5 = new RectangleGeometry(new Rect(bounds3.Left - 0.2, bounds3.Top - 1, bounds3.Width + 0.4, bounds3.Height + 1), 0.6, 0.6);
                    geometry5.Freeze();
                    geometry.AddGeometry(geometry5);
                }
            }
            geometry.Freeze();
            return geometry.GetOutlinedPathGeometry();
        }
    }
}
