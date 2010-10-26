namespace FirefoxTabControl
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._addTabButton = new System.Windows.Forms.Button();
			this._splitContainer = new System.Windows.Forms.SplitContainer();
			this._tabControl = new Genetibase.Shared.Controls.NuGenTabControl();
			this._tabControl2 = new Genetibase.Shared.Controls.NuGenTabControl();
			this.nuGenTabPage1 = new Genetibase.Shared.Controls.NuGenTabPage();
			this.nuGenTabPage2 = new Genetibase.Shared.Controls.NuGenTabPage();
			this.nuGenTabPage3 = new Genetibase.Shared.Controls.NuGenTabPage();
			this._splitContainer.Panel1.SuspendLayout();
			this._splitContainer.Panel2.SuspendLayout();
			this._splitContainer.SuspendLayout();
			this._tabControl2.SuspendLayout();
			this.SuspendLayout();
			// 
			// _addTabButton
			// 
			this._addTabButton.Location = new System.Drawing.Point(0, 0);
			this._addTabButton.Name = "_addTabButton";
			this._addTabButton.Size = new System.Drawing.Size(75, 23);
			this._addTabButton.TabIndex = 1;
			this._addTabButton.Text = "&Add Tab";
			this._addTabButton.UseVisualStyleBackColor = true;
			this._addTabButton.Click += new System.EventHandler(this._addTabButton_Click);
			// 
			// _splitContainer
			// 
			this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this._splitContainer.Location = new System.Drawing.Point(5, 5);
			this._splitContainer.Name = "_splitContainer";
			// 
			// _splitContainer.Panel1
			// 
			this._splitContainer.Panel1.Controls.Add(this._tabControl);
			this._splitContainer.Panel1.Controls.Add(this._addTabButton);
			// 
			// _splitContainer.Panel2
			// 
			this._splitContainer.Panel2.Controls.Add(this._tabControl2);
			this._splitContainer.Size = new System.Drawing.Size(708, 402);
			this._splitContainer.SplitterDistance = 362;
			this._splitContainer.TabIndex = 3;
			// 
			// _tabControl
			// 
			this._tabControl.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._tabControl.Location = new System.Drawing.Point(0, 43);
			this._tabControl.Name = "_tabControl";
			this._tabControl.Size = new System.Drawing.Size(362, 359);
			this._tabControl.TabIndex = 0;
			// 
			// _tabControl2
			// 
			this._tabControl2.CloseButtonOnTab = false;
			this._tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tabControl2.Location = new System.Drawing.Point(0, 0);
			this._tabControl2.Name = "_tabControl2";
			this._tabControl2.Size = new System.Drawing.Size(342, 402);
			this._tabControl2.TabIndex = 0;
			this._tabControl2.TabPages.Add(this.nuGenTabPage1);
			this._tabControl2.TabPages.Add(this.nuGenTabPage2);
			this._tabControl2.TabPages.Add(this.nuGenTabPage3);
			// 
			// nuGenTabPage1
			// 
			this.nuGenTabPage1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nuGenTabPage1.Location = new System.Drawing.Point(1, 28);
			this.nuGenTabPage1.Name = "nuGenTabPage1";
			this.nuGenTabPage1.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this.nuGenTabPage1.Size = new System.Drawing.Size(338, 371);
			this.nuGenTabPage1.TabIndex = 0;
			this.nuGenTabPage1.Text = "General";
			// 
			// nuGenTabPage2
			// 
			this.nuGenTabPage2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nuGenTabPage2.Location = new System.Drawing.Point(1, 28);
			this.nuGenTabPage2.Name = "nuGenTabPage2";
			this.nuGenTabPage2.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this.nuGenTabPage2.Size = new System.Drawing.Size(338, 371);
			this.nuGenTabPage2.TabIndex = 2;
			this.nuGenTabPage2.Text = "Content";
			// 
			// nuGenTabPage3
			// 
			this.nuGenTabPage3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nuGenTabPage3.Location = new System.Drawing.Point(1, 28);
			this.nuGenTabPage3.Name = "nuGenTabPage3";
			this.nuGenTabPage3.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this.nuGenTabPage3.Size = new System.Drawing.Size(338, 371);
			this.nuGenTabPage3.TabIndex = 4;
			this.nuGenTabPage3.Text = "Network";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(718, 412);
			this.Controls.Add(this._splitContainer);
			this.Name = "MainForm";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.Text = "MainForm";
			this._splitContainer.Panel1.ResumeLayout(false);
			this._splitContainer.Panel2.ResumeLayout(false);
			this._splitContainer.ResumeLayout(false);
			this._tabControl2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		internal System.Windows.Forms.Button _addTabButton;
		internal System.Windows.Forms.SplitContainer _splitContainer;
		internal Genetibase.Shared.Controls.NuGenTabControl _tabControl;
		private Genetibase.Shared.Controls.NuGenTabControl _tabControl2;
		private Genetibase.Shared.Controls.NuGenTabPage nuGenTabPage1;
		private Genetibase.Shared.Controls.NuGenTabPage nuGenTabPage2;
		private Genetibase.Shared.Controls.NuGenTabPage nuGenTabPage3;

	}
}

