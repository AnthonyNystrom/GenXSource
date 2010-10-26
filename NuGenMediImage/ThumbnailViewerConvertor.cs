namespace Genetibase.NuGenMediImage
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Globalization;

    /// <summary>Provides a type converter to convert <see cref="T:System.Windows.Forms.ImageViewer"></see> values to and from various other representations.</summary>
    public class ThumbnailViewerConverter : TypeConverter
    {
        /// <summary>Returns whether this converter can convert an object of one type to the type of this converter.</summary>
        /// <returns>true if this object can perform the conversion; otherwise, false.</returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="T:System.Type"></see> that represents the type you wish to convert from.</param>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string text1 = value as string;
            ThumbnailViewer viewer1 = (ThumbnailViewer)context.PropertyDescriptor.GetValue(context.Instance);

            if (text1 != null)
            {
                text1 = text1.Trim();
                if (text1.Length != 0)
                {
                    if (culture == null)
                    {
                        culture = CultureInfo.CurrentCulture;
                    }
                    char ch1 = culture.TextInfo.ListSeparator[0];
                    string[] textArray1 = text1.Split(new char[] { ch1 });

                    viewer1.Collapsed = Boolean.Parse(textArray1[0]);
                    viewer1.Visible = Boolean.Parse(textArray1[1]);
                    viewer1.ShowBrowseButton = Boolean.Parse(textArray1[2]);
                    viewer1.ThumbnailFileType = (ThumbnailFileType)int.Parse(textArray1[3]);
                }
                return viewer1;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if (value is ThumbnailViewer)
            {
                if (destinationType == typeof(string))
                {
                    ThumbnailViewer ImageViewer1 = (ThumbnailViewer)value;
                    if (culture == null)
                    {
                        culture = CultureInfo.CurrentCulture;
                    }
                    string text1 = culture.TextInfo.ListSeparator + " ";
                    TypeConverter converter1 = TypeDescriptor.GetConverter(typeof(Boolean));
                    string[] textArray1 = new string[4];
                    int num1 = 0;
                    textArray1[num1++] = converter1.ConvertToString(context, culture, ImageViewer1.Visible);
                    textArray1[num1++] = converter1.ConvertToString(context, culture, ImageViewer1.Collapsed);
                    textArray1[num1++] = converter1.ConvertToString(context, culture, ImageViewer1.ShowBrowseButton);
                    textArray1[num1++] = converter1.ConvertToString(context, culture, ImageViewer1.ThumbnailFileType);
                    return string.Join(text1, textArray1);
                }
                if (destinationType == typeof(InstanceDescriptor))
                {
                    ThumbnailViewer ImageViewer2 = (ThumbnailViewer)value;
                    return new InstanceDescriptor(typeof(ThumbnailViewer).GetConstructor(new Type[] { typeof(bool), typeof(bool), typeof(bool),typeof(ThumbnailFileType) }), new object[] { ImageViewer2.Visible, ImageViewer2.Collapsed,ImageViewer2.ShowBrowseButton,ImageViewer2.ThumbnailFileType});
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (propertyValues == null)
            {
                throw new ArgumentNullException("propertyValues");
            }
            ThumbnailViewer ImageViewer1 = (ThumbnailViewer)context.PropertyDescriptor.GetValue(context.Instance);

            ImageViewer1.Visible = (bool)propertyValues["Visible"];
            ImageViewer1.Collapsed =(bool)propertyValues["Collapsed"];
            ImageViewer1.ShowBrowseButton = (bool)propertyValues["ShowBrowseButton"];
            ImageViewer1.ThumbnailFileType = (ThumbnailFileType)propertyValues["ThumbnailFileType"];  


            return ImageViewer1;
        }

        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(ThumbnailViewer), attributes).Sort(new string[] { "All", "Left", "Top", "Right", "Visible" });
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

    }
}

