using System.Drawing;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.Ast;
using Scintilla;
using Scintilla.Enums;

namespace Genetibase.VisUI.Scripting
{
    public class ScriptError
    {
        public int Line;
        public int Column;
        public string Description;

        public ScriptError(int line, int column, string description)
        {
            Line = line;
            Column = column;
            Description = description;
        }


        public ScriptError(LexicalInfo info, string description)
        {
            Line = info.Line;
            Column = info.Column;
            Description = description;
        }
    }

    public class ScriptDocument
    {
        ScriptError[] errors;
        ScriptError[] warnings;

        BooScript script;
        ScintillaControl editor;

        public ScriptDocument(ScintillaControl editor)
        {
            this.editor = editor;
            script = new BooScript(null, null);

            editor.MarkerDefine(0, MarkerSymbol.ShortArrow);
            //editor.MarkerSetForegroundColor(1, Color.Red.ToArgb());
        }

        public bool Compile()
        {
            script.Script = editor.Text;
            bool result = script.Compile();

            SetContext(script);

            return result;
        }

        private void SetContext(BooScript script)
        {
            errors = null;
            if (script.Errors != null)
            {
                errors = new ScriptError[script.Errors.Count];
                for (int i = 0; i < errors.Length; i++)
                {
                    errors[i] = new ScriptError(script.Errors[i].LexicalInfo, script.Errors[i].Message);
                    editor.MarkerAdd(errors[i].Line, 0);
                }
            }
            warnings = null;
            if (script.Warnings != null)
            {
                warnings = new ScriptError[script.Warnings.Count];
                for (int i = 0; i < warnings.Length; i++)
                {
                    warnings[i] = new ScriptError(script.Warnings[i].LexicalInfo, script.Warnings[i].Message);
                }
            }

            editor.Update();
        }

        public ScriptError[] Errors
        {
            get { return errors; }
        }

        public ScriptError[] Warnings
        {
            get { return warnings; }
        }

        public void GotoLine(int line)
        {
            editor.GotoLine(line);
        }
    }
}