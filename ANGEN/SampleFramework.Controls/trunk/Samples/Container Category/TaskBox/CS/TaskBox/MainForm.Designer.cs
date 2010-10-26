namespace TaskBox
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.nuGenSmoothPanel1 = new Genetibase.SmoothControls.NuGenSmoothPanel();
			this._taskBox3 = new Genetibase.SmoothControls.NuGenSmoothTaskBox();
			this._spacer2 = new Genetibase.Shared.Controls.NuGenSpacer();
			this._taskBox2 = new Genetibase.SmoothControls.NuGenSmoothTaskBox();
			this._spacer = new Genetibase.Shared.Controls.NuGenSpacer();
			this._taskBox = new Genetibase.SmoothControls.NuGenSmoothTaskBox();
			this._treeView = new Genetibase.Shared.Controls.NuGenTreeView();
			this.nuGenSmoothPanel1.SuspendLayout();
			this._taskBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// nuGenSmoothPanel1
			// 
			this.nuGenSmoothPanel1.Controls.Add(this._taskBox3);
			this.nuGenSmoothPanel1.Controls.Add(this._spacer2);
			this.nuGenSmoothPanel1.Controls.Add(this._taskBox2);
			this.nuGenSmoothPanel1.Controls.Add(this._spacer);
			this.nuGenSmoothPanel1.Controls.Add(this._taskBox);
			this.nuGenSmoothPanel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.nuGenSmoothPanel1.Location = new System.Drawing.Point(0, 0);
			this.nuGenSmoothPanel1.Name = "nuGenSmoothPanel1";
			this.nuGenSmoothPanel1.Size = new System.Drawing.Size(231, 446);
			// 
			// _taskBox3
			// 
			this._taskBox3.Dock = System.Windows.Forms.DockStyle.Top;
			this._taskBox3.Image = ((System.Drawing.Image)(resources.GetObject("_taskBox3.Image")));
			this._taskBox3.Location = new System.Drawing.Point(0, 210);
			this._taskBox3.Name = "_taskBox3";
			this._taskBox3.Size = new System.Drawing.Size(231, 100);
			this._taskBox3.TabIndex = 5;
			this._taskBox3.Text = "RSS";
			// 
			// _spacer2
			// 
			this._spacer2.Dock = System.Windows.Forms.DockStyle.Top;
			this._spacer2.Location = new System.Drawing.Point(0, 205);
			this._spacer2.Name = "_spacer2";
			this._spacer2.Size = new System.Drawing.Size(231, 5);
			// 
			// _taskBox2
			// 
			this._taskBox2.Dock = System.Windows.Forms.DockStyle.Top;
			this._taskBox2.Image = ((System.Drawing.Image)(resources.GetObject("_taskBox2.Image")));
			this._taskBox2.Location = new System.Drawing.Point(0, 105);
			this._taskBox2.Name = "_taskBox2";
			this._taskBox2.Size = new System.Drawing.Size(231, 100);
			this._taskBox2.SmoothAnimation = true;
			this._taskBox2.TabIndex = 2;
			this._taskBox2.Text = "Junk";
			// 
			// _spacer
			// 
			this._spacer.Dock = System.Windows.Forms.DockStyle.Top;
			this._spacer.Location = new System.Drawing.Point(0, 100);
			this._spacer.Name = "_spacer";
			this._spacer.Size = new System.Drawing.Size(231, 5);
			// 
			// _taskBox
			// 
			this._taskBox.Controls.Add(this._treeView);
			this._taskBox.Dock = System.Windows.Forms.DockStyle.Top;
			this._taskBox.Image = ((System.Drawing.Image)(resources.GetObject("_taskBox.Image")));
			this._taskBox.Location = new System.Drawing.Point(0, 0);
			this._taskBox.Name = "_taskBox";
			this._taskBox.Size = new System.Drawing.Size(231, 100);
			this._taskBox.SmoothAnimation = true;
			this._taskBox.TabIndex = 0;
			this._taskBox.Text = "Accounts";
			// 
			// _treeView
			// 
			this._treeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._treeView.Location = new System.Drawing.Point(0, 0);
			this._treeView.Name = "_treeView";
			this._treeView.Size = new System.Drawing.Size(231, 79);
			this._treeView.TabIndex = 1;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(632, 446);
			this.Controls.Add(this.nuGenSmoothPanel1);
			this.Name = "MainForm";
			this.Text = "TaskBox";
			this.nuGenSmoothPanel1.ResumeLayout(false);
			this._taskBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.SmoothControls.NuGenSmoothTaskBox _taskBox;
		private Genetibase.SmoothControls.NuGenSmoothPanel nuGenSmoothPanel1;
		private Genetibase.SmoothControls.NuGenSmoothTaskBox _taskBox3;
		private Genetibase.Shared.Controls.NuGenSpacer _spacer2;
		private Genetibase.SmoothControls.NuGenSmoothTaskBox _taskBox2;
		private Genetibase.Shared.Controls.NuGenSpacer _spacer;
		private Genetibase.Shared.Controls.NuGenTreeView _treeView;
	}
}

