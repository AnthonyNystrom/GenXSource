using System;
using Netron.Diagramming.Core;
using Netron.Diagramming.Win;
using System.Windows.Forms;
using System.Drawing;
namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for Scripting.
	/// </summary>
	public  class ScriptTemplate :  ISample
	{
        public event EventHandler<StringEventArgs> OnRunScript;
        public event EventHandler<StringEventArgs> OnRunOutput;


        /// <summary>
        /// the DiagramControl field
        /// </summary>
        private DiagramControl mDiagramControl;
        /// <summary>
        /// Gets or sets the DiagramControl
        /// </summary>
        public DiagramControl DiagramControl
        {
            get { return mDiagramControl; }
            set { mDiagramControl = value; }
        }
	


        public ScriptTemplate()            
		{
				
		}
		#region Methods

		protected void RaiseOnRunScript(string data)
		{
			if(OnRunScript!=null)
				OnRunScript(this, new StringEventArgs(data));
		}

        protected void RaiseOnRunOutput(object source, string data)
		{
			if(OnRunOutput!=null)
                OnRunOutput(source, new StringEventArgs(data));
		}

		

		#endregion

        protected void exec_OnRunOutput(object source, StringEventArgs e)
		{
			RaiseOnRunOutput(source, e.Data);
		}

        public  void Init()
        {
            //INIT
        }

        public  void Run()
        {
            //CODE
        }
    }

}
