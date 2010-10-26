namespace Nodes
{
    using System;

    public class Attribute
    {
        public Attribute(string name, string val, string ns)
        {
            this.system = false;
            this.name = "";
            this.val = "";
            this.ns = "";
            if ((((name != null) && (val != null)) && ((name.Length > 0) && (val.Length > 0))) && (name.ToUpper() == "CLASS"))
            {
                val = val.Replace("_", "-");
            }
            this.name = name;
            this.val = val;
            this.ns = ns;
        }

        public Attribute(string name, string val, string ns, bool system)
        {
            this.system = false;
            this.name = "";
            this.val = "";
            this.ns = "";
            this.system = system;
            this.name = name;
            this.val = val;
            this.ns = ns;
        }


        public bool system;
        public string name;
        public string val;
        public string ns;
    }
}

