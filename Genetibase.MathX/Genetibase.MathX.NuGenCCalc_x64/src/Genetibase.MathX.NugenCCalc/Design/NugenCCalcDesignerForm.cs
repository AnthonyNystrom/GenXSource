using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Genetibase.MathX.NugenCCalc.Design.Controls;

namespace Genetibase.MathX.NugenCCalc.Design
{
	/// <summary>
	/// Summary description for NugenCCalcDesignerForm.
	/// </summary>
	public class NugenCCalcDesignerForm : System.Windows.Forms.Form
	{
		private NugenCCalc2D _component;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Panel leftPanel;
		private System.Windows.Forms.Panel rightpanel;
		private Genetibase.MathX.NugenCCalc.Design.Controls.Dashboard dashboard;
		private System.Windows.Forms.Button btClose;
		private System.Windows.Forms.Panel bottomPanel;
		private System.Windows.Forms.GroupBox groupBox1;

		private PropertyView _currentView;


		public NugenCCalcDesignerForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		public NugenCCalcDesignerForm(NugenCCalc2D component) : this()
		{
			_component = component;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NugenCCalcDesignerForm));
			this.leftPanel = new System.Windows.Forms.Panel();
			this.dashboard = new Genetibase.MathX.NugenCCalc.Design.Controls.Dashboard();
			this.rightpanel = new System.Windows.Forms.Panel();
			this.btClose = new System.Windows.Forms.Button();
			this.bottomPanel = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.leftPanel.SuspendLayout();
			this.bottomPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// leftPanel
			// 
			this.leftPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.leftPanel.Controls.Add(this.dashboard);
			this.leftPanel.Location = new System.Drawing.Point(0, 0);
			this.leftPanel.Name = "leftPanel";
			this.leftPanel.Size = new System.Drawing.Size(160, 396);
			this.leftPanel.TabIndex = 5;
			// 
			// dashboard
			// 
			this.dashboard.BackColor = System.Drawing.Color.DarkKhaki;
			this.dashboard.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dashboard.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.dashboard.Location = new System.Drawing.Point(0, 0);
			this.dashboard.Name = "dashboard";
			this.dashboard.Size = new System.Drawing.Size(160, 396);
			this.dashboard.TabIndex = 0;
			this.dashboard.OnDashboardItemChange += new Genetibase.MathX.NugenCCalc.Design.Controls.DashboardItemChangeHandler(this.dashboard_OnDashboardItemChange);
			// 
			// rightpanel
			// 
			this.rightpanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.rightpanel.BackColor = System.Drawing.Color.DarkKhaki;
			this.rightpanel.Location = new System.Drawing.Point(160, 0);
			this.rightpanel.Name = "rightpanel";
			this.rightpanel.Size = new System.Drawing.Size(408, 396);
			this.rightpanel.TabIndex = 6;
			// 
			// btClose
			// 
			this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btClose.BackColor = System.Drawing.SystemColors.Control;
			this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btClose.Location = new System.Drawing.Point(488, 12);
			this.btClose.Name = "btClose";
			this.btClose.TabIndex = 7;
			this.btClose.Text = "Close";
			this.btClose.Click += new System.EventHandler(this.btClose_Click);
			// 
			// bottomPanel
			// 
			this.bottomPanel.BackColor = System.Drawing.Color.DarkKhaki;
			this.bottomPanel.Controls.Add(this.groupBox1);
			this.bottomPanel.Controls.Add(this.btClose);
			this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomPanel.Location = new System.Drawing.Point(0, 390);
			this.bottomPanel.Name = "bottomPanel";
			this.bottomPanel.Size = new System.Drawing.Size(568, 40);
			this.bottomPanel.TabIndex = 8;
			// 
			// groupBox1
			// 
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(568, 8);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			// 
			// NugenCCalcDesignerForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.DarkKhaki;
			this.ClientSize = new System.Drawing.Size(568, 430);
			this.Controls.Add(this.bottomPanel);
			this.Controls.Add(this.rightpanel);
			this.Controls.Add(this.leftPanel);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "NugenCCalcDesignerForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "NugenCCalc 2D Component";
			this.Load += new System.EventHandler(this.NugenCCalcDesignerForm_Load);
			this.leftPanel.ResumeLayout(false);
			this.bottomPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		private void SelectView(string action)
		{
			PropertyView newView = null;
			switch(action)
			{
				case "Source Properties":
					newView = new SourceView(this._component);
					break;
				case "Code Expression Repository":
					newView = new ExpressionRepositoryView(this._component);
					break;
				case "Equation Repository":
					newView = new EquationRepositoryView(this._component);
					break;
				case "Destination Properties":
					newView = new DestinationView(this._component);
					break;
			}

			this.rightpanel.Controls.Add(newView);
			newView.Dock = DockStyle.Fill;
			if (_currentView != null)
			{
				_currentView.SaveData();
				_currentView.Dispose();
				_currentView = null;
			}
			_currentView = newView;
			_currentView.OnViewStatusChange +=new ViewStatusChangeHandler(_currentView_OnViewStatusChange);
		}

		private void _currentView_OnViewStatusChange(object sender, StringArgs s)
		{
			//this.statusBar.Panels[0].Text = s.Data;
		}

		private void dashboard_OnDashboardItemChange(object sender, Genetibase.MathX.NugenCCalc.Design.StringArgs s)
		{
			SelectView(s.Data);
		}


		private void btClose_Click(object sender, System.EventArgs e)
		{
			if (_currentView != null)
			{
				_currentView.SaveData();
				_currentView.Dispose();
				_currentView = null;
			}
			this.Close();
		}

		private void NugenCCalcDesignerForm_Load(object sender, System.EventArgs e)
		{
			SelectView("Source Properties");
		}

	}
}
