using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DEffectLib
{
    public abstract class Shader
    {
        string id;
        protected internal Dictionary<string, Input> inputs;
        ShaderProcess[] processes;

        public string ID
        {
            get { return id; }
        }

        protected static void ParseXml(XmlElement el, Shader shader)
        {
            shader.id = el.Attributes["id"].InnerText;

            // parse inputs
            XmlNodeList inputEls = el.SelectNodes("input");
            shader.inputs = new Dictionary<string,Input>(inputEls.Count);
            for (int i = 0; i < inputEls.Count; i++)
            {
                Input input = Input.ParseXmlElement((XmlElement)inputEls[i]);
                shader.inputs.Add(input.Name, input);
            }

            // parse processes
            XmlNodeList processEls = el.SelectNodes("process");
            shader.processes = new ShaderProcess[processEls.Count];
            for (int i = 0; i < processEls.Count; i++)
            {
                shader.processes[i] = ShaderProcess.ParseXmlElement((XmlElement)processEls[i]);
            }
        }

        public virtual void ProcessAndVerify(Dictionary<string, Param> _params)
        {
            // validate inputs

            // validate processes
            foreach (ShaderProcess process in processes)
            {
                process.ProcessAndVerify(this, _params);
            }
        }
    }

    class VShader : Shader
    {
        public static VShader ParseXml(XmlElement el)
        {
            VShader shader = new VShader();
            Shader.ParseXml(el, shader);
            return shader;
        }
    }

    class PShader : Shader
    {
        public static PShader ParseXml(XmlElement el)
        {
            PShader shader = new PShader();
            Shader.ParseXml(el, shader);
            return shader;
        }
    }
}