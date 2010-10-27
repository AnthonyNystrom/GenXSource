using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;

namespace MapServiceAPI
{
    internal class Utility
    {
        static public string ColorToRGB(Color c)
        {
            SolidColorBrush s = new SolidColorBrush(c);
            return "#" + s.Color.R.ToString("x2") + s.Color.G.ToString("x2") + s.Color.B.ToString("x2");
        }

        static public Color GetThisColor(string colorString)
        {
            colorString = colorString.ToLower();
            colorString = colorString[0].ToString().ToUpper() + colorString.Substring(1, colorString.Length - 1);
            Type colorType = (typeof(System.Windows.Media.Colors));
            if (colorType.GetProperty(colorString) != null)
            {
                object o = colorType.InvokeMember(colorString, BindingFlags.GetProperty, null, null, null);
                if (o != null)
                {
                    return (Color)o;
                }
            }
            return Colors.Black;
        }
    }
}
