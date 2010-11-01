using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Genetibase.Network.Design
{
    public class FlagsConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            // We don't want convert from a string
            if (sourceType == typeof(string)) { return false; }
            return base.CanConvertFrom(context, sourceType);
        }
    }
}
