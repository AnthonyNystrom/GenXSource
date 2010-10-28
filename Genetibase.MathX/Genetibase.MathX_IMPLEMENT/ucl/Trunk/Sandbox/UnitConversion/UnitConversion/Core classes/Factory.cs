using System;
using System.Collections.Generic;
using System.Text;

namespace UnitConversion
{
    public static class Factory
    {
        public static Quantity u(double value, BaseUnit type)
        {
            return new Quantity(value, type);
        }
    }
}
