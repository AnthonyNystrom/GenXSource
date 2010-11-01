namespace Nodes
{
    using System;

    public class FontStyle
    {
        static FontStyle()
        {
            FontStyle.NONE = 0;
            FontStyle.BOLD = 2;
            FontStyle.ITALIC = 4;
            FontStyle.UNDERLINE = 8;
        }


        public static int NONE;
        public static int BOLD;
        public static int ITALIC;
        public static int UNDERLINE;
    }
}

