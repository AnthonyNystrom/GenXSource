using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DEffectLib
{
    /// <summary>
    /// Encapsulates a shader input
    /// </summary>
    public class Input
    {
        string name;
        string type;
        string map;

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

        public static Input ParseXmlElement(XmlElement el)
        {
            Input input = new Input();
            input.name = el.Attributes["name"].InnerText;
            input.map = el.Attributes["map"].InnerText;
            input.type = el.Attributes["type"].InnerText;
            return input;
        }
    }

    public class Output
    {
        string name;
        string type;
        string map;
    }
}