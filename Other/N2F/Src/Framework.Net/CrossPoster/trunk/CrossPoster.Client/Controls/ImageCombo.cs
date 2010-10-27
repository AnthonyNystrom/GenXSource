/* ------------------------------------------------
 * ImageCombo.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Next2Friends.CrossPoster.Client.Controls
{
    sealed class ImageCombo : ComboBox
    {
        /// <summary>
        /// Creates a new instance of the <code>ImageCombo</code> class.
        /// </summary>
        public ImageCombo()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        [DefaultValue(null)]
        public ImageList ImageList { get; set; }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();

            // Check if it is an item from the Items collection...
            if (e.Index < 0)
            {
                using (var sb = new SolidBrush(e.ForeColor))
                {
                    // Not an item, draw the text (indented).
                    e.Graphics.DrawString(
                        Text, e.Font, sb,
                        e.Bounds.Left + GetItemTextOffset(),
                        e.Bounds.Top);
                }
            }
            else
            {
                var item = Items[e.Index] as ImageComboItem;

                // Check if item is an ImageComboItem.
                if (item != null)
                {
                    Color forecolor = (item.ForeColor != Color.FromKnownColor(KnownColor.Transparent)) ? item.ForeColor : e.ForeColor;
                    Font font = item.Mark ? new Font(e.Font, FontStyle.Bold) : e.Font;

                    using (var sb = new SolidBrush(forecolor))
                    {
                        // -1: no image.
                        if (item.ImageIndex != -1 && ImageList != null)
                        {
                            // Draw image, then draw text next to it.
                            ImageList.Draw(e.Graphics, e.Bounds.Left, e.Bounds.Top, item.ImageIndex);
                            e.Graphics.DrawString(item.Text, font, sb, e.Bounds.Left + ImageList.ImageSize.Width, e.Bounds.Top);
                        }
                        else
                        {
                            // Draw text (indented).
                            e.Graphics.DrawString(item.Text, font, sb, e.Bounds.Left + GetItemTextOffset(), e.Bounds.Top);
                        }
                    }

                }
                else
                {
                    using (var sb = new SolidBrush(e.ForeColor))
                    {
                        // It is not an ImageComboItem, draw it.
                        e.Graphics.DrawString(Items[e.Index].ToString(), e.Font, sb, e.Bounds.Left + GetItemTextOffset(), e.Bounds.Top);
                    }
                }

            }

            base.OnDrawItem(e);
        }

        private Int32 GetItemTextOffset()
        {
            return ImageList != null ? ImageList.ImageSize.Width : 0;
        }
    }
}
