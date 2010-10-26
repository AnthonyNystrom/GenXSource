using System.Text;

namespace Genetibase.VisUI.Scripting
{
    class BooScriptBuilder
    {
        public static void InsertFunction(string preamble, string name, string _params, string code, ref StringBuilder script)
        {
            script.AppendLine(string.Format("{0} def {1}({2}):", preamble, name, _params));
            script.Append(code);
        }

        public static void CreateClass(string[] imports, string name, string parentClass,
                                       out StringBuilder script)
        {
            script = new StringBuilder();
            if (imports != null)
            {
                foreach (string import in imports)
                {
                    script.AppendLine(import);
                }
            }

            script.AppendLine(string.Format("class {0}({1}):", name, parentClass));
        }

        public static void InsertMember(string name, string type, ref StringBuilder script)
        {
            script.Append(name);
            script.Append(" as ");
            script.AppendLine(type);
        }
    }
}