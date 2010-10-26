using System.Collections.Generic;

namespace Genetibase.NuGenDEMVis
{
    class ProgramArgs
    {
        readonly Dictionary<string, object> args;

        /// <summary>
        /// Initializes a new instance of the ProgramArgs class.
        /// </summary>
        public ProgramArgs(string[] args)
        {
            this.args = new Dictionary<string, object>();
            ParseArgs(args);
        }

        private void ParseArgs(string[] args)
        {
            // assume all are switches for now
            for (int i = 0; i < args.Length; i++)
            {
                this.args.Add(args[i], null);
            }
        }

        public bool CheckSwitch(string key)
        {
            return args.ContainsKey(key);
        }
    }
}