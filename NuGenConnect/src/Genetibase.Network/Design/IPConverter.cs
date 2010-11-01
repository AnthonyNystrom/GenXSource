using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Reflection;

namespace Genetibase.Network.Design
{
    public class IPConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            // We don't want convert from a string
            if (sourceType == typeof(string)) { return false; }
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string) || destinationType == typeof(InstanceDescriptor)) { return true; }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value is Ras.Ras.RASIPADDR && (destinationType == typeof(string)))
            {
                Ras.Ras.RASIPADDR i = (Ras.Ras.RASIPADDR)value;
                return string.Format("{0}.{1}.{2}.{3}", i.a.ToString(), i.b.ToString(), i.c.ToString(), i.d.ToString());
            }
            else if (value is Ras.Ras.RASIPADDR && (destinationType == typeof(InstanceDescriptor)))
            {
                Ras.Ras.RASIPADDR i = (Ras.Ras.RASIPADDR)value;
                byte[] values = new byte[4];
                Type[] types = new Type[4];
                types[0] = types[1] = types[2] = types[3] = typeof(byte);
                values[0] = i.a; values[1] = i.b; values[2] = i.c; values[3] = i.d;
                ConstructorInfo ci = typeof(Ras.Ras.RASIPADDR).GetConstructor(types);
                return new InstanceDescriptor(ci, values);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

}
