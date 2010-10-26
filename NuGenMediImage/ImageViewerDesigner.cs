using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace Genetibase.NuGenMediImage
{
    class ImageViewerDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);

            string[] propertiesToHide = { "AutoSize" 
            ,"AccessibleName"
            ,"AccessibleDescription"
            ,"AccessibleRole"
        };

            foreach (string propname in propertiesToHide)
            {
                PropertyDescriptor prop = (PropertyDescriptor)properties[propname];
                if (prop != null)
                {
                    AttributeCollection runtimeAttributes = prop.Attributes;
                    // make a copy of the original attributes
                    // but make room for one extra attribute
                    Attribute[] attrs = new Attribute[runtimeAttributes.Count + 1];
                    runtimeAttributes.CopyTo(attrs, 0);
                    attrs[runtimeAttributes.Count] = new BrowsableAttribute(false);
                    prop = TypeDescriptor.CreateProperty(this.GetType(),
                             propname, prop.PropertyType, attrs);
                    properties[propname] = prop;
                }
            }
        }
    }
}
