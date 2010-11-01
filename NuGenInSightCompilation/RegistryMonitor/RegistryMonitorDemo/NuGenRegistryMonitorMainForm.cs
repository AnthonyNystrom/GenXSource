using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Data;
using RegistryUtils;
using Microsoft.Win32;

namespace RegistryMonitorUI
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class NuGenRegistryMonitorMainForm : System.Windows.Forms.Form
    {
		private Janus.Windows.EditControls.UIButton buttonStart;
        private Janus.Windows.EditControls.UIButton buttonStop;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.TextBox textRegistryKey;

		private RegistryMonitor registryMonitor;
        private Janus.Windows.EditControls.UIComboBox comboBoxRegistryHives;
        private Label label2;
        private Label label1;
        private Panel panel1;
        private Janus.Windows.UI.Tab.UITab uiTab1;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NuGenRegistryMonitorMainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			comboBoxRegistryHives.DisplayMember = "Name";
			comboBoxRegistryHives.Items.Add(Registry.CurrentUser.Name);
			comboBoxRegistryHives.Items.Add(Registry.LocalMachine.Name);
			comboBoxRegistryHives.SelectedValue = Registry.CurrentUser.Name;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
				StopRegistryMonitor();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NuGenRegistryMonitorMainForm));
            this.comboBoxRegistryHives = new Janus.Windows.EditControls.UIComboBox();
            this.textRegistryKey = new System.Windows.Forms.TextBox();
            this.buttonStart = new Janus.Windows.EditControls.UIButton();
            this.buttonStop = new Janus.Windows.EditControls.UIButton();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.uiTab1 = new Janus.Windows.UI.Tab.UITab();
            this.uiTabPage1 = new Janus.Windows.UI.Tab.UITabPage();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).BeginInit();
            this.uiTab1.SuspendLayout();
            this.uiTabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxRegistryHives
            // 
            this.comboBoxRegistryHives.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.comboBoxRegistryHives.Location = new System.Drawing.Point(100, 28);
            this.comboBoxRegistryHives.Name = "comboBoxRegistryHives";
            this.comboBoxRegistryHives.Size = new System.Drawing.Size(208, 20);
            this.comboBoxRegistryHives.TabIndex = 1;
            // 
            // textRegistryKey
            // 
            this.textRegistryKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textRegistryKey.Location = new System.Drawing.Point(100, 60);
            this.textRegistryKey.Name = "textRegistryKey";
            this.textRegistryKey.Size = new System.Drawing.Size(618, 20);
            this.textRegistryKey.TabIndex = 3;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(20, 100);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 4;
            this.buttonStart.Text = "Start";
            this.buttonStart.Click += new System.EventHandler(this.OnButtonStartClick);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(108, 100);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 5;
            this.buttonStop.Text = "Stop";
            this.buttonStop.Click += new System.EventHandler(this.OnButtonStopClick);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.IntegralHeight = false;
            this.listBox1.Location = new System.Drawing.Point(17, 141);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(701, 404);
            this.listBox1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(232)))), ((int)(((byte)(247)))));
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(20, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Registry key:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(232)))), ((int)(((byte)(247)))));
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(20, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Registry hive:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.uiTab1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(736, 567);
            this.panel1.TabIndex = 7;
            // 
            // uiTab1
            // 
            this.uiTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTab1.Location = new System.Drawing.Point(0, 0);
            this.uiTab1.Name = "uiTab1";
            this.uiTab1.ShowTabs = false;
            this.uiTab1.Size = new System.Drawing.Size(736, 567);
            this.uiTab1.TabIndex = 7;
            this.uiTab1.TabPages.AddRange(new Janus.Windows.UI.Tab.UITabPage[] {
            this.uiTabPage1});
            this.uiTab1.VisualStyle = Janus.Windows.UI.Tab.TabVisualStyle.Office2007;
            // 
            // uiTabPage1
            // 
            this.uiTabPage1.Controls.Add(this.listBox1);
            this.uiTabPage1.Controls.Add(this.buttonStop);
            this.uiTabPage1.Controls.Add(this.label1);
            this.uiTabPage1.Controls.Add(this.textRegistryKey);
            this.uiTabPage1.Controls.Add(this.comboBoxRegistryHives);
            this.uiTabPage1.Controls.Add(this.label2);
            this.uiTabPage1.Controls.Add(this.buttonStart);
            this.uiTabPage1.Location = new System.Drawing.Point(1, 1);
            this.uiTabPage1.Name = "uiTabPage1";
            this.uiTabPage1.Size = new System.Drawing.Size(734, 565);
            this.uiTabPage1.TabStop = true;
            this.uiTabPage1.Text = "New Tab";
            // 
            // NuGenRegistryMonitorMainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(736, 567);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "NuGenRegistryMonitorMainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Registry Monitor";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).EndInit();
            this.uiTab1.ResumeLayout(false);
            this.uiTabPage1.ResumeLayout(false);
            this.uiTabPage1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void OnButtonStartClick(object sender, EventArgs e)
		{
			string keyName = string.Format("{0}\\{1}", comboBoxRegistryHives.SelectedValue.ToString(), textRegistryKey.Text);

			listBox1.Items.Add("Monitoring \"" + keyName + "\" started");
			registryMonitor = new RegistryMonitor(keyName);
            registryMonitor.RegChanged += new EventHandler(OnRegChanged);
            registryMonitor.Error += new System.IO.ErrorEventHandler(OnError);
            registryMonitor.Start();

			buttonStart.Enabled = false;
			buttonStop.Enabled = true;
		}

		private void OnButtonStopClick(object sender, EventArgs e)
		{
			StopRegistryMonitor();
		}

		private void StopRegistryMonitor()
		{
			if (registryMonitor != null)
			{
				registryMonitor.Stop();
				registryMonitor.RegChanged -= new EventHandler(OnRegChanged);
				registryMonitor.Error -= new System.IO.ErrorEventHandler(OnError);
				registryMonitor = null;

				buttonStart.Enabled = true;
				buttonStop.Enabled = false;

				listBox1.Items.Add("Monitoring stopped");
			}
		}

		private void OnRegChanged(object sender, EventArgs e)
		{
		    if (listBox1.InvokeRequired)
		    {
		        listBox1.BeginInvoke(new EventHandler(OnRegChanged), new object[] { sender, e});
		        return;
		    }
		    
			listBox1.Items.Add("Registry has changed");
		}

		private void OnError(object sender, ErrorEventArgs e)
		{
            if (listBox1.InvokeRequired)
            {
                listBox1.BeginInvoke(new ErrorEventHandler(OnError), new object[] { sender, e });
                return;
            }

            listBox1.Items.Add("ERROR: " + e.GetException().Message);
			StopRegistryMonitor();
		}
	}
}
