using System.IO;
using System.Reflection;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.IO;
using Boo.Lang.Compiler.Pipelines;
using Genetibase.NuGenRenderCore.Resources;
using System;

namespace Genetibase.VisUI.Scripting
{
    public class BooScript : Resource
    {
        string scriptData;
        CompilerContext context;

        public BooScript(string scriptData, string id)
             : base(id, null, null)
        {
            //this.scriptData = NuGenVisUI.Properties.Resources.WelcomeLayer_boo;
            this.scriptData = scriptData;
        }

        public static BooScript LoadBooScript(string id, string filePath)
        {
            StreamReader reader = new StreamReader(filePath);

            BooScript script = new BooScript(reader.ReadToEnd(), id);
            reader.Dispose();

            return script;
        }

        public bool Compile()
        {
            BooCompiler compiler = new BooCompiler();
            compiler.Parameters.GenerateInMemory = true;
            compiler.Parameters.Input.Add(new StringInput("<script>", scriptData));

            compiler.Parameters.Pipeline = new CompileToMemory();
            context = compiler.Run();

            if (context.GeneratedAssembly == null)
            {
                return false;
            }
            return true;
        }

        public void Run()
        {
        }

        public CompilerErrorCollection Errors
        {
            get { return context.Errors; }
        }

        public CompilerWarningCollection Warnings
        {
            get { return context.Warnings; }
        }

        public string Script
        {
            get { return scriptData; }
            set { scriptData = value; }
        }

        public Assembly GeneratedAssembly
        {
            get { return context.GeneratedAssembly; }
        }

        public override void Dispose()
        {
            
        }
    }
}