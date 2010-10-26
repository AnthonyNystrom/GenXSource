#region gnu_license
/*
    Crystal Controls - C# control library containing the following tools:
        CrystalControl - base class
        CrystalGradientControl - a control that can either have a gradient background or be totally transparent.
        CrystalLabel - a homegrown label that can have a gradient or transparent background.
        CrystalTrackBar - a homegrown trackbar that can have a gradient or transparent background.
        CrystalToolStripTrackBar - a host for CrystalTrackBar that allows it to work in a ToolStrip.
    Copyright (C) 2006 Richard Guion
    Attilan Software Factory: http://www.attilan.com
    Contact: richard@attilan.com

    Version 0.67, Alpha build release
        This is a work in progress: USE AT YOUR OWN RISK!  Interfaces/Methods may change!
 
    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
    http://www.gnu.org
 */
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.VisualStyles;
//using Attilan.Crystal.Tools;

/// <summary>
/// This internal class helps us locate the resource for ToolboxBitmap items.
/// http://www.bobpowell.net/toolboxbitmap.htm
/// </summary>
internal class resfinder
{
}

namespace Attilan.Crystal.Controls
{
    /// <summary>
    /// Base class for all Crystal controls, derives from ScrollableControl.
    /// </summary>
    [System.ComponentModel.DesignerCategory("code")]
    [ToolboxItem(false)]
    public class CrystalControl : ScrollableControl
    {
        #region Gui

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// A base control class that provides either a gradient or transparent background.
    /// A gradient background may make your application more colorful.
    /// A transparent background is useful if your Form's background has a bitmap image,
    /// and also if you are hosting the control within a toolstrip (see CrystalToolStripTrackBar).
    /// </summary>
    [System.ComponentModel.DesignerCategory("code")]
    [ToolboxItem(false)]
    public class CrystalGradientControl : CrystalControl
    {
        private const int TransparentBit = 0x00000020; //WS_EX_TRANSPARENT 
        private Color _Color1 = Color.LightBlue;
        private Color _Color2 = Color.SteelBlue;
        private float _ColorAngle = 90f;
        private bool _transparentMode = false;

        #region Fields

        /// <summary>
        /// The first color of the gradient.  If TransparencyMode is true, the gradient will be ignored.
        /// </summary>
        [
        Browsable(true),
        Category("Appearance"),
        DefaultValue(1),
        Description("Indicates the first color for the gradient effect. If TransparencyMode is true, the gradient will be ignored.")
        ]
        public Color Color1
        {
            get { return _Color1; }
            set
            {
                if (value != _Color1)
                {
                    _Color1 = value;
                    this.InvalidateEx();
                }
            }
        }

        /// <summary>
        /// The second color of the gradient.  If TransparencyMode is true, the gradient will be ignored.
        /// </summary>
        [
        Browsable(true),
        Category("Appearance"),
        DefaultValue(1),
        Description("Indicates the second color for the gradient effect. If TransparencyMode is true, the gradient will be ignored.")
        ]
        public Color Color2
        {
            get { return _Color2; }
            set
            {
                if (value != _Color2)
                {
                    _Color2 = value;
                    this.InvalidateEx();
                }
            }
        }

        /// <summary>
        /// The angle of the gradient.
        /// </summary>
        [
        Browsable(true),
        Category("Appearance"),
        DefaultValue(180f),
        Description("Indicates the angle of the color gradient.")
        ]
        public float ColorAngle
        {
            get { return _ColorAngle; }
            set
            {
                if (value != _ColorAngle)
                {
                    _ColorAngle = value;
                    this.InvalidateEx();
                }
            }
        }
        /// <summary>
        /// Determines whether the control will be in transparent or non-transparent mode.
        /// When set to true, Color1 and Color2 will be ignored.
        /// </summary>
        [
        Browsable(true),
        Category("Appearance"),
        DefaultValue(false),
        Description("Indicates that the background of this control should be totally transparent to allow the background of the parent window.")
        ]
        public bool TransparentMode
        {
            get { return _transparentMode; }
            set
            {
                if (value != _transparentMode)
                {
                    _transparentMode = value;
                    base.RecreateHandle();
                }
            }
        }

        #endregion

        #region Constructors, Destructors

        /// <summary>
        /// Constructor for CrystalGradientControl
        /// </summary>
        public CrystalGradientControl()
        {
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Returns the Creation bits for the controls.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (TransparentMode)
                    cp.ExStyle |= TransparentBit; // add WS_EX_TRANSPARENT 
                else
                    cp.ExStyle &= TransparentBit; // subtract WS_EX_TRANSPARENT 
                return cp;
            }
        }

        /// <summary>
        /// Paints the background for the control.  If the TransparencyMode is true,
        /// no background is painted.
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (TransparentMode)
                return;

            // Getting the graphics object
            Graphics g = pevent.Graphics;

            // Creating the rectangle for the gradient
            Rectangle rBackground = new Rectangle(0, 0, this.Width, this.Height);

            // Creating the lineargradient
            System.Drawing.Drawing2D.LinearGradientBrush bBackground
                = new System.Drawing.Drawing2D.LinearGradientBrush(rBackground, _Color1, _Color2, _ColorAngle);

            // Draw the gradient onto the form
            g.FillRectangle(bBackground, rBackground);

            // Disposing of the resources held by the brush
            bBackground.Dispose();
        }

        #endregion

        /// <summary>
        /// Invalidates the control in transparency mode by calling the parent to invalidate the given rect.
        /// If Transparency mode is false, the control's rect is invalidated.
        /// </summary>
        /// <param name="rc">The rectangle to invalidate.</param>
        protected virtual void InvalidateEx(Rectangle rc)
        {
            if (TransparentMode)
            {
                if (Parent == null)
                {
                    //System.Console.WriteLine("InvalidateEx: Parent is NULL");
                    return;
                }

                Parent.Invalidate(rc, true);

                // THIS IS CRITICAL
                // If you don't update the parent here, then the rect may
                // get invalidated after you have repainted the control.
                Parent.Update();
            }
            else
                Invalidate(rc);
        }

        /// <summary>
        /// Invalidates the control in transparency mode by calling the parent to invalidate the given rect.
        /// If Transparency mode is false, the control's rect is invalidated.
        /// </summary>
        /// <param name="rc">The rectangle to invalidate.</param>
        protected virtual void InvalidateEx(RectangleF rc)
        {
            Rectangle newRect = new Rectangle();
            newRect.X = Convert.ToInt32(rc.X);
            newRect.Y = Convert.ToInt32(rc.Y);
            newRect.Width = Convert.ToInt32(rc.Width);
            newRect.Height = Convert.ToInt32(rc.Height);

            InvalidateEx(newRect);
        }

        /// <summary>
        /// Invalidates the entire rectangle for this control.
        /// </summary>
        protected virtual void InvalidateEx()
        {
            if (TransparentMode)
            {
                Rectangle rc = new Rectangle(this.Location, this.Size);
                InvalidateEx(rc);
            }
            else
                Invalidate();
        }

        /// <summary>
        /// Forces OnPaint to be called for this control.
        /// </summary>
        protected virtual void RedrawControl()
        {
            Invalidate();
            Update();
        }
    }

}