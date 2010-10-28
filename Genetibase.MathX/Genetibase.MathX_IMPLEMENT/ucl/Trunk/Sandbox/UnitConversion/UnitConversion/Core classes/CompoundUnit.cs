using System;
using System.Collections.Generic;
using System.Text;

namespace UnitConversion
{
    public class CompoundUnit : BaseUnit
    {
        internal Dictionary<UnitType, int> _types = new Dictionary<UnitType,int>();

        public Dictionary<UnitType, int> types
        {
            get { return _types; }
            set { _types = value; }
        }

    }
}
