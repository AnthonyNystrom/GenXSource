using System;
using System.Windows.Forms;
using Netron.Diagramming.Core;
using Netron.Neon.WinFormsUI;
namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for CodeTab.
	/// </summary>
	public class OutputForm : DockContent
    {
        public NOutput Output;
		#region Fields

        private ContextMenu menu;
		#endregion

		#region Properties



       

		public string OutputText
		{
			get{return this.Output.Text;}
			set{this.Output.Text = value;}
		}

		

		#endregion

		#region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:OutputForm"/> class.
        /// </summary>
        /// <include file="CodeDoc\DockContent.xml" path="//CodeDoc/Class[@name=&quot;DockHContent&quot;]/Constructor[@name=&quot;()&quot;]/*"/>
		public OutputForm()
		{
			InitializeComponent();
            this.Output.AddChannel("Exception");
			this.Output.AddChannel("Info");
				
		}
		#endregion

		#region Methods

		private void OnClearAll(object sender, EventArgs e)
		{
			ClearAll();
		}

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputForm));
            this.menu = new System.Windows.Forms.ContextMenu();
            this.Output = new NOutput();
            this.SuspendLayout();
            // 
            // Output
            // 
            this.Output.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Output.Current = "Default";
            this.Output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Output.Image = null;
            this.Output.Label = "";
            this.Output.Location = new System.Drawing.Point(0, 0);
            this.Output.Name = "Output";
            this.Output.ShowBottomPanel = false;
            this.Output.Size = new System.Drawing.Size(280, 462);
            this.Output.TabIndex = 1;
            // 
            // OutputForm
            // 
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(280, 462);
            this.Controls.Add(this.Output);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OutputForm";
            this.TabText = "Output";
            this.ResumeLayout(false);

		}

		public void ClearAll()
		{
            this.Output.Text = "";
		}

		#endregion
	}
}
