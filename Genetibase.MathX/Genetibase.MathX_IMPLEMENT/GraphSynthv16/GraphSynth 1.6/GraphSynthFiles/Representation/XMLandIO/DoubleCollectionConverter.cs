using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

namespace GraphSynth
{
    public class DoubleCollectionConverter : System.ComponentModel.TypeConverter
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
                List<Double> items = new List<Double>();
                char[] charSeparators = new char[] { ',', '(', ')', ' ', ':', ';', '/', '\\', '\'', '\"' };

                string[] results = (value as string).Split(charSeparators);

                for (int i = 0; i < results.GetLength(0); i++)
                {
                    if (results[i] != "")
                    {
                        try
                        { items.Add(Double.Parse(results[i].Trim())); }
                        catch
                        { items.Add(0.0); }
                    }
                }
                return items;
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
            if (value.GetType() == typeof(List<Double>))
            {
                List<Double> values = (List<Double>)value;
                string text = "";
                if (values.Count > 0)
                    text = values[0].ToString();
                if (values.Count > 1)
                {
                    for (int i = 1; i < values.Count; i++)
                        text += ", " + values[i].ToString();
                }
                return text;
            }
            else
                return null;
        }
    }

}
