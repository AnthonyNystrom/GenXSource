using System;
using System.Collections.Generic;
using System.Text;

namespace UnitConversion
{
    public enum UnitType 
    {
        Mass,
        Length,
        Time,
        Angle,
    }

    public class BaseUnit
    {
        internal UnitType _type;
        internal string _name;
        internal double _multiplier;
        internal double _constant;


        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        public double multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }


        public double constant
        {
            get { return _constant; }
            set { _constant = value; }
        }

        public UnitType type
        {
            get { return _type; }
            set { _type = value; }
        }

        public double getValueFromSI(double value)
        {
            return value / multiplier - constant;
        }

        public double getValueInSI(double value)
        {
            return (value + constant)*multiplier;
        }

        public static CompoundUnit operator /(BaseUnit x, BaseUnit y)
        {
            CompoundUnit unit = new CompoundUnit();
            unit.name = string.Format("{0}/{1}", x.name, y.name);
            unit.multiplier = x.multiplier / y.multiplier;
            unit.types.Add(x.type, 1);
            unit.types.Add(y.type, -1);
            return unit;
        }

    }
	
	
}
