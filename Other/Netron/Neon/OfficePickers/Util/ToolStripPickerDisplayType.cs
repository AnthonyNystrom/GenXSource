using System;
using System.Collections.Generic;
using System.Text;

namespace Netron.Neon.OfficePickers
{
    /// <summary>
    /// Specifies the display options for the ToolStripColorPicker such as
    /// image, text and underline.
    /// </summary>
    public enum ToolStripPickerDisplayType
    {
        // <summary>
        /// Specifies that only a normal image is to be rendered for this ToolStripColorPicker
        /// </summary>   
        NormalImage,
        /// <summary>
        /// Specifies that both image and text are to be rendered for this ToolStripColorPicker
        /// </summary>
        NormalImageAndText,
        /// <summary>
        /// Specifies that both color under-line and image are to be rendered for this ToolStripColorPicker
        /// </summary>
        UnderLineAndImage,
        /// <summary>
        /// Specifies that both color under-line and text are to be rendered for this ToolStripColorPicker
        /// </summary>
        UnderLineAndText,
        /// <summary>
        /// Specifies that both color under-line, text and image are to be rendered for this ToolStripColorPicker
        /// </summary>
        UnderLineTextAndImage,
        /// <summary>
        /// Specifies that only a color under-line is to be rendered for this ToolStripColorPicker
        /// </summary>
        UnderLineOnly,
        /// <summary>
        /// Specified that neither image, text nor under-line is to be rendered for this ToolStripColorPicker
        /// </summary>
        None,
        /// <summary>
        /// Specifies that only text is to be rendered for this ToolStripColorPicker            
        /// </summary>
        Text
    }
}
