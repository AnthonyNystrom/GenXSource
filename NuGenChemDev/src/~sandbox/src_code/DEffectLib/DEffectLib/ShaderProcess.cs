using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DEffectLib
{
    class ShaderProcess
    {
        string name;
        string mode;
        ShaderLine[] lines;

        public static ShaderProcess ParseXmlElement(XmlElement el)
        {
            ShaderProcess process = new ShaderProcess();

            process.name = el.Attributes["name"].InnerText;
            process.mode = el.GetAttribute("mode");
            
            // process lines
            XmlNodeList lineEls = el.SelectNodes("line");
            process.lines = new ShaderLine[lineEls.Count];
            for (int i = 0; i < lineEls.Count; i++)
            {
                process.lines[i] = new ShaderLine(lineEls[i].InnerText);
            }

            return process;
        }

        public void ProcessAndVerify(Shader shader, Dictionary<string, Param> _params)
        {
            // validate lines
            foreach (ShaderLine line in lines)
            {
                line.ProcessAndVerify(shader, _params);
            }
        }
    }
}