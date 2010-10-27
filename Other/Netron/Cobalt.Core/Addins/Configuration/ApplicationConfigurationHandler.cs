using System;
using System.Reflection;
using System.Configuration;
using System.Collections.Generic;

using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Collections;


namespace Netron.Cobalt
{
    public class AddinsSectionGroup : ConfigurationSectionGroup
    { 
    
    }

    // Define a custom section.
    // The Addin type alows to define a custom section 
    // programmatically.
    public sealed class AddinSection : ConfigurationSection
    {
        // The collection (property bag) that conatains 
        // the section properties.
        private static ConfigurationPropertyCollection _Properties;

        // Internal flag to disable 
        // property setting.
        private static bool _ReadOnly;

        // The FileName property.
        private static readonly ConfigurationProperty mLocation =
            new ConfigurationProperty("Location", typeof(string), "", ConfigurationPropertyOptions.IsRequired);

        private static readonly ConfigurationProperty mType =
            new ConfigurationProperty("Type", typeof(string), "", ConfigurationPropertyOptions.IsRequired);

        
        

        // Addin constructor.
        public AddinSection()
        {
            // Property initialization
            _Properties = new ConfigurationPropertyCollection();

            _Properties.Add(mLocation);

            _Properties.Add(mType);
        }


        // This is a key customization. 
        // It returns the initialized property bag.
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _Properties;
            }
        }


        private new bool IsReadOnly
        {
            get
            {
                return _ReadOnly;
            }
        }

        // Use this to disable property setting.
        private void ThrowIfReadOnly(string propertyName)
        {
            if (IsReadOnly)
                throw new ConfigurationErrorsException(
                    "The property " + propertyName + " is read only.");
        }


        // Customizes the use of Addin
        // by setting _ReadOnly to false.
        // Remember you must use it along with ThrowIfReadOnly.
        protected override object GetRuntimeObject()
        {
            // To enable property setting just assign true to
            // the following flag.
            _ReadOnly = true;
            return base.GetRuntimeObject();
        }


        [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|",   MinLength = 1)]
        public string FileName
        {
            get
            {
                return (string)this["Location"];
            }
            set
            {
                // With this you disable the setting.
                // Renemmber that the _ReadOnly flag must
                // be set to true in the GetRuntimeObject.
                ThrowIfReadOnly("Location");
                this["Location"] = value;
            }
        }

        [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|", MinLength = 1)]
        public string Type
        {
            get
            {
                return (string)this["Type"];
            }
            set
            {
                // With this you disable the setting.
                // Renemmber that the _ReadOnly flag must
                // be set to true in the GetRuntimeObject.
                ThrowIfReadOnly("Type");
                this["Type"] = value;
            }
        }


    }

	
}




