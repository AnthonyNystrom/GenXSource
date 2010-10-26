using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DEffectLib
{
    /// <summary>
    /// Encapsulates an effect's parameter
    /// </summary>
    public class Param
    {
        public static Dictionary<string, string> KnownTypes = new Dictionary<string, string>();

        string name;
        string type;
        string map;
        object defaultValue;

        public string Name
        {
            get { return name; }
        }

        public string Type
        {
            get { return type; }
        }

        public string Map
        {
            get { return map; }
        }

        public object DefaultValue
        {
            get { return defaultValue; }
        }

        public static Param ParseXmlElement(XmlElement el)
        {
            Param param = new Param();
            param.name = el.Attributes["name"].InnerText;
            param.type = el.Attributes["type"].InnerText;
            param.defaultValue = el.GetAttribute("value");
            return param;
        }

        public void ProcessAndVerify()
        {
            // verify type
            if (!KnownTypes.ContainsKey(type))
                throw new Exception();
        }

        public static void InitKnownTypes()
        {
            if (KnownTypes.Count == 0)
            {
                KnownTypes["FLOAT"] = null;
                KnownTypes["FLOAT2"] = null;
                KnownTypes["FLOAT3"] = null;
                KnownTypes["FLOAT4"] = null;
                KnownTypes["MATRIX3x3"] = null;
                KnownTypes["MATRIX4x4"] = null;
            }
        }
    }
}
