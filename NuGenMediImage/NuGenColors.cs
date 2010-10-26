using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Genetibase.NuGenMediImage
{
    internal class NuGenColors
    {

        //// The single instance allowed
        //private static NuGenColors _colorConfig = null;


        //// The only way to get the object's instance
        //public static NuGenColors GetColorConfig()
        //{
        //    if (_colorConfig == null)
        //    {
        //        _colorConfig = new NuGenColors();
        //    }
        //    return _colorConfig;
        //}

        public Color TabBarBackColor;
        public Color MultiPaneBackColor;
        public Color TabPageColor1;
        public Color TabPageColor2;
        public Color TabPageColor3;
        public Color TabPageColor4;
        public Color Color6;
        public Color TabPageBorderColor;
        public Color RibbonGroupBottomColor1;
        public Color RibbonGroupBottomColor2;
        public Color RibbonGroupBackColor;
        public Color ProgressBarColor;

        public NuGenColors()
        {
            TabBarBackColor = Color.FromArgb(83, 83, 83);
            MultiPaneBackColor = Color.FromArgb(83, 83, 83);

            TabPageColor1 = Color.FromArgb(210, 214, 221);
            TabPageColor2 = Color.FromArgb(193, 198, 207);            
            TabPageColor3 = Color.FromArgb(180, 187, 197);
            TabPageColor4 = Color.FromArgb(231, 240, 241);

            Color6 = Color.FromArgb(190, 190, 190);            
            
            TabPageBorderColor = Color.FromArgb(231, 233, 237);           

            RibbonGroupBottomColor1 = Color.FromArgb(182, 184, 184);
            RibbonGroupBottomColor2 = Color.FromArgb(157, 159, 159);

            ProgressBarColor = Color.DarkOrange;

            RibbonGroupBackColor = SystemColors.Control;
        }
    }
}
