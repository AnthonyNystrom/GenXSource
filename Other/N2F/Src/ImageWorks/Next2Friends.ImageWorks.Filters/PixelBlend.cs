using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Next2Friends.ImageWorks.Filters
{
    public static class PixelBlend
    {        
        // Choose darkest color 
        public static byte BlendDarken(byte Src, byte Dst)
        {
            return ((Src < Dst) ? Src : Dst);
        }

        // Multiply
        public static byte BlendMultiply(byte Src, byte Dst)
        {
            return (byte)Math.Max(Math.Min((Src / 255.0f * Dst / 255.0f) * 255.0f, 255), 0);
        }

        // Screen
        public static byte BlendScreen(byte Src, byte Dst)
        {
            return (byte)Math.Max(Math.Min(255 - ((255 - Src) / 255.0f * (255 - Dst) / 255.0f) * 255.0f, 255), 0);
        }

        // Choose lightest color 
        public static byte BlendLighten(byte Src, byte Dst)
        {
            return ((Src > Dst) ? Src : Dst);
        }

        // hard light 
        public static byte BlendHardLight(byte Src, byte Dst)
        {
            return ((Src < 128) ? (byte)Math.Max(Math.Min((Src / 255.0f * Dst / 255.0f) * 255.0f * 2, 255), 0) : (byte)Math.Max(Math.Min(255 - ((255 - Src) / 255.0f * (255 - Dst) / 255.0f) * 255.0f * 2, 255), 0));
        }

        // difference 
        public static byte BlendDifference(byte Src, byte Dst)
        {
            return (byte)((Src > Dst) ? Src - Dst : Dst - Src);
        }

        // pin light 
        public static byte BlendPinLight(byte Src, byte Dst)
        {
            return (Src < 128) ? ((Dst > Src) ? Src : Dst) : ((Dst < Src) ? Src : Dst);
        }

        // overlay 
        public static byte BlendOverlay(byte Src, byte Dst)
        {
            return ((Dst < 128) ? (byte)Math.Max(Math.Min((Src / 255.0f * Dst / 255.0f) * 255.0f * 2, 255), 0) : (byte)Math.Max(Math.Min(255 - ((255 - Src) / 255.0f * (255 - Dst) / 255.0f) * 255.0f * 2, 255), 0));
        }

        // exclusion 
        public static byte BlendExclusion(byte Src, byte Dst)
        {
            return (byte)(Src + Dst - 2 * (Dst * Src) / 255f);
        }

        // Soft Light (XFader formula)  
        public static byte BlendSoftLight(byte Src, byte Dst)
        {
            return (byte)Math.Max(Math.Min((Dst * Src / 255f) + Dst * (255 - ((255 - Dst) * (255 - Src) / 255f) - (Dst * Src / 255f)) / 255f, 255), 0);
        }

        // Color Burn 
        public static byte BlendColorBurn(byte Src, byte Dst)
        {
            return (Src == 0) ? (byte)0 : (byte)Math.Max(Math.Min(255 - (((255 - Dst) * 255) / Src), 255), 0);
        }

        // Color Dodge 
        public static byte BlendColorDodge(byte Src, byte Dst)
        {
            return (Src == 255) ? (byte)255 : (byte)Math.Max(Math.Min((Dst * 255) / (255 - Src), 255), 0);
        }   
    }
}
