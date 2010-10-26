using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace DEffectLib
{
    /// <summary>
    /// Encapsulates a dynamic effect
    /// </summary>
    public class DEffect
    {
        string name;
        Dictionary<string, Param> _params;
        Dictionary<string, Shader> shaders;
        Dictionary<string, Technique> techniques;

        public static DEffect LoadEffect(string filename)
        {
            using (FileStream fs = File.Open(filename, FileMode.Open))
            {
                return LoadEffect(fs);
            }
        }

        public static DEffect LoadEffect(Stream stream)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(stream);
            return LoadEffect(xml);
        }

        public static DEffect LoadEffect(XmlDocument xml)
        {
            Param.InitKnownTypes();

            DEffect effect = new DEffect();

            XmlElement effectEl = (XmlElement)xml.SelectSingleNode("/effect");
            effect.name = effectEl.Attributes["name"].InnerText;

            // parse params
            XmlNodeList paramEls = xml.SelectNodes("/effect/param");
            effect._params = new Dictionary<string, Param>(paramEls.Count);
            for (int i = 0; i < paramEls.Count; i++)
            {
                Param p = Param.ParseXmlElement((XmlElement)paramEls[i]);
                effect._params[p.Name] = p;
            }

            // parse shaders
            XmlNodeList shaderEls = xml.SelectSingleNode("/effect/shaders").ChildNodes;
            effect.shaders = new Dictionary<string, Shader>(shaderEls.Count);
            for (int i = 0; i < shaderEls.Count; i++)
            {
                Shader s = null;
                if (shaderEls[i].Name == "vshader")
                    s = VShader.ParseXml((XmlElement)shaderEls[i]);
                else if (shaderEls[i].Name == "pshader")
                    s = PShader.ParseXml((XmlElement)shaderEls[i]);
                effect.shaders[s.ID] = s;
            }

            // parse techniques
            XmlNodeList techniqueEls = effectEl.SelectNodes("technique");
            effect.techniques = new Dictionary<string,Technique>(techniqueEls.Count);
            for (int i = 0; i < techniqueEls.Count; i++)
            {
                Technique technique = Technique.ParseXmlElement((XmlElement)techniqueEls[i], effect.shaders);
                effect.techniques.Add(technique.Name, technique);
            }

            effect.ProcessAndVerify();

            return effect;
        }

        public string ProduceEffectForTechnique(string technique)
        {
            // find technique
            Technique tn;
            if (techniques.TryGetValue(technique, out tn))
            {
                return tn.ProduceCode();
            }
            return null;
        }

        protected void ProcessAndVerify()
        {
            // verify params
            for (Dictionary<string, Param>.Enumerator param = _params.GetEnumerator(); param.MoveNext(); )
            {
                param.Current.Value.ProcessAndVerify();
            }

            // verify shaders
            foreach (KeyValuePair<string, Shader> shader in shaders)
            {
                shader.Value.ProcessAndVerify(_params);
            }
        }
    }
}