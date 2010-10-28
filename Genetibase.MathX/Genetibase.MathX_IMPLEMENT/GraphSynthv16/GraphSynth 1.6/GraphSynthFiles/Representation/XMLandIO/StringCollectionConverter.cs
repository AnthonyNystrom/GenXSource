using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

namespace GraphSynth
{
    public class StringCollectionConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            else return false;
        }

        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                return StringCollectionConverter.convert((string)value);
            }
            else
                return null;
        }

        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
        {
            if ((sourceType == typeof(List<string>)) || (sourceType == typeof(string[])))
                return true;
            else return false;
        }

        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture, object value, Type s)
        {
            if (value.GetType() == typeof(List<string>))
            {
                return StringCollectionConverter.convert((List<string>) value);
            }
            else
                return null;
        }


        //
        public static List<string> convert(string value)
        {
            List<string> items = new List<string>();
            char[] charSeparators = new char[] { ',', '(', ')', ' ', ':', ';', '.', '/', '\\', '\'', '\"' };

            string[] results = value.Split(charSeparators);

            for (int i = 0; i < results.GetLength(0); i++)
            {
                if (results[i] != "")
                    items.Add(results[i].Trim());
            }
            return items;
        }
        public static string convert(List<string> values)
        {
            string text = "";
            if (values.Count > 0)
                text = values[0];
            if (values.Count > 1)
            {
                for (int i = 1; i < values.Count; i++)
                    text += ", " + values[i];
            }
            return text;
        }
    }

}
