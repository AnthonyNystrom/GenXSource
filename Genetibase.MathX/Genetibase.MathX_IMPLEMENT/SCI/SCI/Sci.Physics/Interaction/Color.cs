/* *****************************************************************************
 * AUTHOR       : Coskun OBA
 * EMAIL        : oba.coskun@hotmail.com
 * 
 * DATE         : JANUARY 2007
 * *****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Sci.Physics.Interaction
{
    #region ColorFactory
    public sealed class ColorFactory
    {
        private Hashtable colors = new Hashtable();

        private static readonly ColorFactory instance = new ColorFactory();

        public static ColorFactory GetColourFactory() { return instance; }

        public Color GetColour(ColorCharge key)
        {
            Color color = (Color)colors[key];
            if (color == null)
            {
                switch (key)
                {
                    case ColorCharge.WHITE: color = new White(); break;
                    case ColorCharge.RED: color = new Red(); break;
                    case ColorCharge.GREEN: color = new Green(); break;
                    case ColorCharge.BLUE: color = new Blue(); break;
                    case ColorCharge.ANTIRED: color = new AntiRed(); break;
                    case ColorCharge.ANTIGREEN: color = new AntiGreen(); break;
                    case ColorCharge.ANTIBLUE: color = new AntiBlue(); break;
                }
                colors.Add(key, color);
            }
            return color;
        }
    }
    #endregion

    #region ColorCharge
    public enum ColorCharge
    {
        WHITE,
        RED,
        GREEN,
        BLUE,
        ANTIRED,
        ANTIGREEN,
        ANTIBLUE
    }
    #endregion

    #region Color
    public abstract class Color
    {
        protected ColorCharge charge = ColorCharge.WHITE;

        public Color(ColorCharge charge) { this.charge = charge; }

        public ColorCharge Charge { get { return charge; } }

        public override bool Equals(object obj)
        {
            return (obj is Color) ? charge.Equals(((Color)(obj)).charge) : false;
        }
        public override int GetHashCode() { return charge.GetHashCode(); }
        public override string ToString() { return charge.ToString(); }
    }
    #endregion
    #region White
    public sealed class White : Color
    {
        public White() : base(ColorCharge.WHITE) { }
    }
    #endregion
    #region Red
    public sealed class Red : Color
    {
        public Red() : base(ColorCharge.RED) { }
    }
    #endregion
    #region Green
    public sealed class Green : Color
    {
        public Green() : base(ColorCharge.GREEN) { }
    }
    #endregion
    #region Blue
    public sealed class Blue : Color
    {
        public Blue() : base(ColorCharge.BLUE) { }
    }
    #endregion

    #region AntiColor
    public abstract class AntiColor : Color
    {
        public AntiColor(ColorCharge charge) : base(charge){}
    }
    #endregion
    #region AntiRed
    public sealed class AntiRed : AntiColor
    {
        public AntiRed() : base(ColorCharge.ANTIRED) { }
    }
    #endregion
    #region AntiGreen
    public sealed class AntiGreen : AntiColor
    {
        public AntiGreen() : base(ColorCharge.ANTIGREEN) { }
    }
    #endregion
    #region AntiBlue
    public sealed class AntiBlue : AntiColor
    {
        public AntiBlue() : base(ColorCharge.ANTIBLUE) { }
    }
    #endregion
}
