namespace SampleFramework
{
	partial class Browser
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Browser));
			this._treeViewImageList = new System.Windows.Forms.ImageList(this.components);
			this._vertSplitContainer = new Genetibase.SmoothControls.NuGenSmoothSplitContainer();
			this._samplesTreeView = new Genetibase.Shared.Controls.NuGenTreeView();
			this._horzSplitContainer = new Genetibase.SmoothControls.NuGenSmoothSplitContainer();
			this._descriptionTextBox = new System.Windows.Forms.TextBox();
			this._imagePictureBox = new Genetibase.SmoothControls.NuGenSmoothPictureBox();
			this._toolStrip = new Genetibase.SmoothControls.NuGenSmoothToolStrip();
			this._runButton = new System.Windows.Forms.ToolStripButton();
			this._csSampleButton = new System.Windows.Forms.ToolStripButton();
			this._vbSampleButton = new System.Windows.Forms.ToolStripButton();
			this._browseButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this._docButton = new System.Windows.Forms.ToolStripButton();
			this.linkLabel1 = new Genetibase.Shared.Controls.LinkLabel();
			this._vertSplitContainer.Panel1.SuspendLayout();
			this._vertSplitContainer.Panel2.SuspendLayout();
			this._vertSplitContainer.SuspendLayout();
			this._horzSplitContainer.Panel1.SuspendLayout();
			this._horzSplitContainer.Panel2.SuspendLayout();
			this._horzSplitContainer.SuspendLayout();
			this._toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// _treeViewImageList
			// 
			this._treeViewImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_treeViewImageList.ImageStream")));
			this._treeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
			this._treeViewImageList.Images.SetKeyName(0, "Folder.png");
			this._treeViewImageList.Images.SetKeyName(1, "FolderOpen.png");
			this._treeViewImageList.Images.SetKeyName(2, "Sample.png");
			// 
			// _vertSplitContainer
			// 
			this._vertSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this._vertSplitContainer.Location = new System.Drawing.Point(0, 25);
			this._vertSplitContainer.Name = "_vertSplitContainer";
			// 
			// _vertSplitContainer.Panel1
			// 
			this._vertSplitContainer.Panel1.Controls.Add(this._samplesTreeView);
			// 
			// _vertSplitContainer.Panel2
			// 
			this._vertSplitContainer.Panel2.Controls.Add(this._horzSplitContainer);
			this._vertSplitContainer.Size = new System.Drawing.Size(366, 241);
			this._vertSplitContainer.SplitterDistance = 121;
			this._vertSplitContainer.TabIndex = 1;
			// 
			// _samplesTreeView
			// 
			this._samplesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._samplesTreeView.ImageIndex = 0;
			this._samplesTreeView.ImageList = this._treeViewImageList;
			this._samplesTreeView.Location = new System.Drawing.Point(0, 0);
			this._samplesTreeView.Name = "_samplesTreeView";
			this._samplesTreeView.Size = new System.Drawing.Size(121, 241);
			this._samplesTreeView.TabIndex = 0;
			this._samplesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._samplesTreeView_AfterSelect);
			// 
			// _horzSplitContainer
			// 
			this._horzSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this._horzSplitContainer.Location = new System.Drawing.Point(0, 0);
			this._horzSplitContainer.Name = "_horzSplitContainer";
			this._horzSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// _horzSplitContainer.Panel1
			// 
			this._horzSplitContainer.Panel1.Controls.Add(this._descriptionTextBox);
			// 
			// _horzSplitContainer.Panel2
			// 
			this._horzSplitContainer.Panel2.Controls.Add(this._imagePictureBox);
			this._horzSplitContainer.Size = new System.Drawing.Size(241, 241);
			this._horzSplitContainer.SplitterDistance = 64;
			this._horzSplitContainer.TabIndex = 0;
			// 
			// _descriptionTextBox
			// 
			this._descriptionTextBox.BackColor = System.Drawing.SystemColors.Window;
			this._descriptionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._descriptionTextBox.Location = new System.Drawing.Point(0, 0);
			this._descriptionTextBox.Multiline = true;
			this._descriptionTextBox.Name = "_descriptionTextBox";
			this._descriptionTextBox.ReadOnly = true;
			this._descriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._descriptionTextBox.Size = new System.Drawing.Size(241, 64);
			this._descriptionTextBox.TabIndex = 0;
			// 
			// _imagePictureBox
			// 
			this._imagePictureBox.BackColor = System.Drawing.SystemColors.Window;
			this._imagePictureBox.DisplayMode = Genetibase.Shared.Controls.NuGenDisplayMode.ActualSize;
			this._imagePictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._imagePictureBox.Location = new System.Drawing.Point(0, 0);
			this._imagePictureBox.Name = "_imagePictureBox";
			this._imagePictureBox.Padding = new System.Windows.Forms.Padding(1);
			this._imagePictureBox.Size = new System.Drawing.Size(241, 173);
			this._imagePictureBox.TabIndex = 0;
			this._imagePictureBox.ZoomFactor = 1;
			// 
			// _toolStrip
			// 
			this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._runButton,
            this._csSampleButton,
            this._vbSampleButton,
            this._browseButton,
            this.toolStripSeparator1,
            this._docButton,
            this.linkLabel1});
			this._toolStrip.Location = new System.Drawing.Point(0, 0);
			this._toolStrip.Name = "_toolStrip";
			this._toolStrip.Size = new System.Drawing.Size(366, 25);
			this._toolStrip.TabIndex = 0;
			this._toolStrip.Text = "nuGenSmoothToolStrip1";
			// 
			// _runButton
			// 
			this._runButton.Enabled = false;
			this._runButton.Image = ((System.Drawing.Image)(resources.GetObject("_runButton.Image")));
			this._runButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._runButton.Name = "_runButton";
			this._runButton.Size = new System.Drawing.Size(23, 22);
			this._runButton.Click += new System.EventHandler(this._runButton_Click);
			// 
			// _csSampleButton
			// 
			this._csSampleButton.Enabled = false;
			this._csSampleButton.Image = ((System.Drawing.Image)(resources.GetObject("_csSampleButton.Image")));
			this._csSampleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._csSampleButton.Name = "_csSampleButton";
			this._csSampleButton.Size = new System.Drawing.Size(23, 22);
			this._csSampleButton.Click += new System.EventHandler(this._csSampleButton_Click);
			// 
			// _vbSampleButton
			// 
			this._vbSampleButton.Enabled = false;
			this._vbSampleButton.Image = ((System.Drawing.Image)(resources.GetObject("_vbSampleButton.Image")));
			this._vbSampleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._vbSampleButton.Name = "_vbSampleButton";
			this._vbSampleButton.Size = new System.Drawing.Size(23, 22);
			this._vbSampleButton.Click += new System.EventHandler(this._vbSampleButton_Click);
			// 
			// _browseButton
			// 
			this._browseButton.Enabled = false;
			this._browseButton.Image = ((System.Drawing.Image)(resources.GetObject("_browseButton.Image")));
			this._browseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._browseButton.Name = "_browseButton";
			this._browseButton.Size = new System.Drawing.Size(23, 22);
			this._browseButton.Click += new System.EventHandler(this._browseButton_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// _docButton
			// 
			this._docButton.Image = ((System.Drawing.Image)(resources.GetObject("_docButton.Image")));
			this._docButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._docButton.Name = "_docButton";
			this._docButton.Size = new System.Drawing.Size(23, 22);
			// 
			// linkLabel1
			// 
			this.linkLabel1.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel1.Image")));
			this.linkLabel1.Margin = new System.Windows.Forms.Padding(2, 1, 0, 2);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(112, 16);
			this.linkLabel1.Target = "http://www.genetibase.com/";
			this.linkLabel1.Text = "Genetibase, Inc.";
			this.linkLabel1.ToolTipText = "http://www.genetibase.com/";
			// 
			// Browser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(366, 266);
			this.Controls.Add(this._vertSplitContainer);
			this.Controls.Add(this._toolStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Browser";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this._vertSplitContainer.Panel1.ResumeLayout(false);
			this._vertSplitContainer.Panel2.ResumeLayout(false);
			this._vertSplitContainer.ResumeLayout(false);
			this._horzSplitContainer.Panel1.ResumeLayout(false);
			this._horzSplitContainer.Panel1.PerformLayout();
			this._horzSplitContainer.Panel2.ResumeLayout(false);
			this._horzSplitContainer.ResumeLayout(false);
			this._toolStrip.ResumeLayout(false);
			this._toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Genetibase.SmoothControls.NuGenSmoothToolStrip _toolStrip;
		private System.Windows.Forms.ToolStripButton _runButton;
		private System.Windows.Forms.ToolStripButton _csSampleButton;
		private System.Windows.Forms.ToolStripButton _vbSampleButton;
		private System.Windows.Forms.ToolStripButton _browseButton;
		private Genetibase.SmoothControls.NuGenSmoothSplitContainer _vertSplitContainer;
		private Genetibase.SmoothControls.NuGenSmoothSplitContainer _horzSplitContainer;
		private Genetibase.Shared.Controls.NuGenTreeView _samplesTreeView;
		private System.Windows.Forms.TextBox _descriptionTextBox;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton _docButton;

		private Browser()
		{
			this.InitializeComponent();
		}

		private System.Windows.Forms.ImageList _treeViewImageList;
		private Genetibase.SmoothControls.NuGenSmoothPictureBox _imagePictureBox;
		private Genetibase.Shared.Controls.LinkLabel linkLabel1;
	}
}
