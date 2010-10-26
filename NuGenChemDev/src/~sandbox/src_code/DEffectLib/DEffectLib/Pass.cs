using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;

namespace DEffectLib
{
    class Pass
    {
        VShader vShader;
        string[] vProcesses;
        PShader pShader;
        string[] pProcesses;

        public Pass(VShader vshader, PShader pshader)
        {
            this.vShader = vshader;
            this.pShader = pshader;
        }

        public static Pass ParseXmlElement(XmlElement el, Dictionary<string, Shader> shaders)
        {
            XmlElement vshEl = (XmlElement)el.SelectSingleNode("vshader");
            XmlElement pshEl = (XmlElement)el.SelectSingleNode("pshader");
            string vsh = vshEl.InnerText;
            string psh = pshEl.InnerText;

            string vshProcesses = vshEl.GetAttribute("processes");
            string pshPRocesses = pshEl.GetAttribute("processes");

            string[] vProcesses = null;
            int idx = 0;
            if (vshProcesses != null && vshProcesses.Length > 0)
            {
                MatchCollection matches = Regex.Matches(vshProcesses, "[a-zA-Z0-9]");
                if (matches != null && matches.Count > 0)
                {
                    vProcesses = new string[matches.Count];
                    foreach (Match match in matches)
                    {
                        vProcesses[idx++] = match.Groups[0].Value;
                    }
                }
            }
            string[] pProcesses = null;
            idx = 0;
            if (pshPRocesses != null && pshPRocesses.Length > 0)
            {
                MatchCollection matches = Regex.Matches(pshPRocesses, "[a-zA-Z0-9]");
                if (matches != null && matches.Count > 0)
                {
                    pProcesses = new string[matches.Count];
                    foreach (Match match in matches)
                    {
                        pProcesses[idx++] = match.Groups[0].Value;
                    }
                }
            }

            Shader vshader = null;
            Shader pshader = null;

            shaders.TryGetValue(vsh, out vshader);
            shaders.TryGetValue(psh, out pshader);

            Pass pass = new Pass((VShader)vshader, (PShader)pshader);
            pass.vProcesses = vProcesses;
            pass.pProcesses = pProcesses;
            return pass;
        }

        public void Compile(/*out string structures*/)
        {
            //vShader.Compile();
        }
    }
}