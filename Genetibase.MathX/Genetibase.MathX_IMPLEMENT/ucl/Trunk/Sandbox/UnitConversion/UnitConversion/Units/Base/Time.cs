using System;
using System.Collections.Generic;
using System.Text;

namespace UnitConversion
{
        public class s : BaseUnit
        {
            public s()
            {
                _type = UnitType.Time;
                _name = "s";
                _multiplier = 1.0;
                _constant = 0.0;
            }

        }

        public class hr : BaseUnit
        {
            public hr()
            {
                _type = UnitType.Time;
                _name = "hr";
                _multiplier = 3600.0;
                _constant = 0.0;
            }

        }
}
