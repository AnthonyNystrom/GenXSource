using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DEffectLib
{
    class Technique
    {
        string name;
        Pass[] passes;

        public string Name
        {
            get { return name; }
        }

        public static Technique ParseXmlElement(XmlElement el, Dictionary<string, Shader> shaders)
        {
            Technique tech = new Technique();
            tech.name = el.Attributes["name"].InnerText;

            // parse passes
            XmlNodeList passEls = el.SelectNodes("pass");
            tech.passes = new Pass[passEls.Count];
            for (int i = 0; i < passEls.Count; i++)
            {
                tech.passes[i] = Pass.ParseXmlElement((XmlElement)passEls[i], shaders);
            }

            return tech;
        }

        public string ProduceCode()
        {
            // run through dependancies


            // write params

            // write I/O structures
            return null;
        }
    }
}