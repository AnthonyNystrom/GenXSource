using System;
using System.Collections.Generic;
using System.Text;

namespace UnitConversion
{
    public class m : BaseUnit
    {
        public m()
        {
            _type = UnitType.Length;
            _name = "m";
            _multiplier = 1.0;
            _constant = 0.0;
        }

    }

    public class km : BaseUnit
    {
        public km()
        {
            _type = UnitType.Length;
            _name = "km";
            _multiplier = 1000.0;
            _constant = 0.0;
        }

    }

    public static class x
    {
        public static m m = new m();
        public static km km = new km();

        public static s s = new s();
        public static hr hr = new hr();


    }
}
