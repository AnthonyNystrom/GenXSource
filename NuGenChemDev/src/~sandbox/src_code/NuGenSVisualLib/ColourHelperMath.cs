using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace NuGenSVisualLib
{
    class ColourHelperMath
    {
        public class ColourGradient
        {
            public float R, G, B, A;

            public int Sample(float point)
            {
                return Color.FromArgb((int)(A * point), (int)(R * point), (int)(G * point), (int)(B * point)).ToArgb();
            }
        }

        public static void GenerateLinearGradient(Color a, Color b, out ColourGradient gradient)
        {
            // break into component graduations
            gradient = new ColourGradient();
            gradient.R = (float)(b.R - a.R) / 255f;
            gradient.B = (float)(b.B - a.B) / 255f;
            gradient.G = (float)(b.G - a.G) / 255f;
            gradient.A = (float)(b.A - a.A) / 255f;
        }
    }
}