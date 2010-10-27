/* ------------------------------------------------
 * ImageComboItem.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Next2Friends.CrossPoster.Client.Controls
{
    sealed class ImageComboItem
    {        
        public ImageComboItem()
        {
        }

        public ImageComboItem(string text)
            : this(text, -1)
        {
        }

        public ImageComboItem(string text, int imageIndex)
            : this(text, imageIndex, false)
        {
        }

        public ImageComboItem(string text, int imageIndex, bool mark)
            : this(text, imageIndex, false, Color.FromKnownColor(KnownColor.Transparent))
        {
        }

        public ImageComboItem(string text, int imageIndex, bool mark, Color foreColor)
            : this(text, imageIndex, mark, foreColor, null)
        {
        }

        public ImageComboItem(string text, int imageIndex, bool mark, Color foreColor, Object tag)
        {
            ForeColor = foreColor;
            ImageIndex = imageIndex;
            Mark = mark;
            Tag = tag;
            Text = text;
        }

        public Color ForeColor { get; set; }
        public Int32 ImageIndex { get; set; }
        public Boolean Mark { get; set; }
        public Object Tag { get; set; }
        public String Text { get; set; }
        public override String ToString()
        {
            return Text;
        }
    }
}
