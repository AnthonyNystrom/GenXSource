using System;
using System.IO; 
using Microsoft.CSharp;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using Netron.Diagramming.Core;
namespace Netron.Cobalt
{
	/// <summary>
	/// An extension which executes scripts
	/// </summary>
	public class ScriptExecutor
	{
		#region Constants
		private const string constGenerateAssemblyName = "Script.dll";
        private const string constReferences = "Netron.Diagramming.Win.dll,Netron.Diagramming.Core.dll,Netron.Cobalt.IDE.exe,System.Windows.Forms.dll,System.dll,System.Xml.dll,System.Drawing.dll";
        private const string constBaseNamespace = "Netron.Cobalt";
        private const string constClassName = "ScriptTemplate";
		private const string constRunMethod = "Run";

		#endregion

		#region Events
		public event EventHandler<StringEventArgs> OnRunOutput;
		#endregion

        #region Properties

        /// <summary>
        /// the MainForm field
        /// </summary>
        private MainForm mMainForm;
        /// <summary>
        /// Gets or sets the MainForm
        /// </summary>
        public MainForm MainForm
        {
            get { return mMainForm; }
            set { mMainForm = value; }
        }

        #endregion

        public ScriptExecutor(MainForm mainForm)
		{
            this.mMainForm = mainForm;
		}

		private void Application_OnRunScript(object source, StringEventArgs e)
		{
			this.ExecuteCode( e.Data );
		}

		private void RaiseOnRunOutput(string data)
		{
			if(OnRunOutput!=null)
                OnRunOutput(this, new StringEventArgs(data));
		}

		/// <summary>
		/// Generate a class file for the code
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		private string MakeCode( string code )
		{

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Cobalt.IDE.HTTP.Guide.Framework.ScriptTemplate.cs");
			if(stream ==null)
                throw new System.IO.FileLoadException("Loading of resource 'Netron.Cobalt.Guide.Framework.ScriptTemplate.cs' gave problems.");
			StreamReader reader = new StreamReader(stream);
			string template = reader.ReadToEnd();
			reader.Close();
			stream.Close();
			return template.Replace("//CODE", code);
		}

	

		/// <summary>
		/// Executes the script
		/// </summary>
		/// <param name="code"></param>
		public void ExecuteCode( string code )
		{
			// We will be using the C# code compiler
			using( CSharpCodeProvider provider = new CSharpCodeProvider() )
			{
                //ICodeCompiler compiler = CodeDomProvider.CreateProvider(CSharpCodeProvider.GetLanguageFromExtension(".cs"));


                string[] references = constReferences.Split(',');

				CompilerParameters options = new CompilerParameters();
				options.GenerateInMemory = true;
				options.GenerateExecutable = false;
				options.IncludeDebugInformation = true;
				options.OutputAssembly = constGenerateAssemblyName;
				options.ReferencedAssemblies.AddRange(references);

				// Make the source code file
				string fullSource = this.MakeCode( code );

				// Compile it and make an in-memory assembly
                CompilerResults results = provider.CompileAssemblyFromSource(options, fullSource);

				// Show the errors and quit, if any
				if( results.Errors.HasErrors )
				{
					foreach( CompilerError error in results.Errors )
					{
						RaiseOnRunOutput( error.ErrorText.ToString()  + " [Line " + error.Line + "]" +  Environment.NewLine);
					}

					return;
				}

				// Show compiler outputs
				foreach( string output in results.Output )
				{
					RaiseOnRunOutput( output );
				}

				// Get the class that we have generated using the script and
				// call the public method which contains user's script
				try
				{
					// Get compiled assembly 
					Assembly assembly = results.CompiledAssembly;

					// Get the class which was compiled
					Type type = assembly.GetType( constBaseNamespace + "." + constClassName );

					// Invoke the public method which contains user's script
					ISample obj = Activator.CreateInstance( type ) as ISample;
                    obj.DiagramControl = Application.Diagram;
                    //MethodInfo method = type.GetMethod(constRunMethod);
					//method.Invoke( obj, null );
                    obj.Run();
                    Application.Tabs.Diagram.Show();
				}
				catch( Exception x )
				{
					RaiseOnRunOutput( "Error occured while executing your script:");
					RaiseOnRunOutput( x.ToString() );
				}
				
			}
		}

	}
}
