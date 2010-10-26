using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DEffectLib
{
    public class ShaderLine
    {
        string rawData;

        Input[] inputDependancies;
        Param[] paramDependancies;
        Output[] outputDependancies;

        object[] breakdown;

        public ShaderLine(string rawData)
        {
            this.rawData = rawData;
        }

        public void ProcessAndVerify(Shader shader, Dictionary<string, Param> _params)
        {
            // parse line for dependancies
            int idx = 0;

            // inputs
            List<Input> inRefs = new List<Input>();
            MatchCollection matches = Regex.Matches(rawData, @"(IN.)([a-zA-Z0-9]*)");
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    string value = match.Groups[2].Value;
                    // validate reference
                    Input input;
                    if (!shader.inputs.TryGetValue(value, out input))
                        throw new Exception();
                    inRefs.Add(input);
                }
            }
            inputDependancies = inRefs.ToArray();
            inRefs.Clear();

            // outputs

            // params
            List<Param> pRefs = new List<Param>();
            matches = Regex.Matches(rawData, @"(PARAM.)([a-zA-Z0-9]*)");
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    string value = match.Groups[2].Value;
                    // validate reference
                    Param param;
                    if (!_params.TryGetValue(value, out param))
                        throw new Exception();
                    pRefs.Add(param);
                }
            }
            paramDependancies = pRefs.ToArray();
            pRefs.Clear();
        }
    }
}