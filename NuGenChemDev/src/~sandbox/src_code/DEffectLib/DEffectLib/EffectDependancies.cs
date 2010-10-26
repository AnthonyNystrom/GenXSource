using System;
using System.Collections.Generic;
using System.Text;

namespace DEffectLib
{
    class EffectDependancies
    {
        public Dictionary<string, Param> parameters;

        public string Write()
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, Param> param in parameters)
            {
                string end = "";
                if (param.Value.Map != null)
                    end = string.Format(" : {0}");
                builder.AppendLine(string.Format("uniform extern {0} {1}{2};",
                                                 param.Value.Type, param.Value.Name, end));
            }

            return builder.ToString();
        }
    }

    class TechniqueDependancies
    {
        public Dictionary<string, ShaderDependancies> shaders;
    }

    class ShaderDependancies
    {
        public Dictionary<string, Input> inputs;
        public Dictionary<string, Output> outputs;

        public string Write()
        {
            StringBuilder builder = new StringBuilder();

            foreach (KeyValuePair<string, Input> input in inputs)
            {
                builder.AppendLine(string.Format("struct {0} {", ));

                input.Value.

                builder.AppendLine("};");
            }

            return builder.ToString();
        }
    }
}