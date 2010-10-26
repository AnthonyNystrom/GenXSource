using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Genetibase.NuGenMediImage
{
    internal class NuGenColorsStatic
    {       
        public static Color TabBarBackColor;
        public static Color MultiPaneBackColor;
        public static Color TabPageColor1;
        public static Color TabPageColor2;
        public static Color TabPageColor3;
        public static Color TabPageColor4;
        public static Color Color6;
        public static Color TabPageBorderColor;
        public static Color RibbonGroupBottomColor1;
        public static Color RibbonGroupBottomColor2;
        public static Color RibbonGroupBackColor;
        public static Color ProgressBarColor;        

        static NuGenColorsStatic()
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
