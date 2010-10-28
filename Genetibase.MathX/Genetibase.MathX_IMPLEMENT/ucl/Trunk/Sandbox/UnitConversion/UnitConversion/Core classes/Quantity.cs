using System;
using System.Collections.Generic;
using System.Text;

namespace UnitConversion
{
    public class Quantity
    {
        private BaseUnit _unit;
        private double _value;

        public double value
        {
            get { return _value; }
            set { _value = value; }
        }
	
        public BaseUnit unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        public string shortString()
        {
            return string.Format("{0} {1}", value.ToString(), unit.name);
        }

        public static Quantity operator +(Quantity q1, Quantity q2)
        {
            double resultInSI = q1.unit.getValueInSI(q1.value) + q2.unit.getValueInSI(q2.value);
            double result = q1.unit.getValueFromSI(resultInSI);
            return new Quantity(result, q1.unit);
        }

        public Quantity() { }

        public Quantity(double value, BaseUnit unit)
        {
            _unit = unit;
            _value = value;
        }
    }
}
