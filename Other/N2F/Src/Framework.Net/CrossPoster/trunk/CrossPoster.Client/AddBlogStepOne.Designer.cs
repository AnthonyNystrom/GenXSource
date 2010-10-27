namespace Next2Friends.CrossPoster.Client
{
    partial class AddBlogStepOne
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Blogger", 0, 0);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("WordPress", 1, 1);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("LiveJournal", 2, 2);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddBlogStepOne));
            this._blogList = new System.Windows.Forms.TreeView();
            this._blogImages = new System.Windows.Forms.ImageList(this.components);
            this._cancelButton = new System.Windows.Forms.Button();
            this._nextButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _blogList
            // 
            this._blogList.FullRowSelect = true;
            this._blogList.HideSelection = false;
            this._blogList.ImageIndex = 0;
            this._blogList.ImageList = this._blogImages;
            this._blogList.Location = new System.Drawing.Point(11, 12);
            this._blogList.Name = "_blogList";
            treeNode1.ImageIndex = 0;
            treeNode1.Name = "_bloggerNode";
            treeNode1.SelectedImageIndex = 0;
            treeNode1.Tag = "Blogger";
            treeNode1.Text = "Blogger";
            treeNode2.ImageIndex = 1;
            treeNode2.Name = "_wordpressNode";
            treeNode2.SelectedImageIndex = 1;
            treeNode2.Tag = "WordPress";
            treeNode2.Text = "WordPress";
            treeNode3.ImageIndex = 2;
            treeNode3.Name = "_ljNode";
            treeNode3.SelectedImageIndex = 2;
            treeNode3.Tag = "LiveJournal";
            treeNode3.Text = "LiveJournal";
            this._blogList.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this._blogList.SelectedImageIndex = 0;
            this._blogList.ShowLines = false;
            this._blogList.ShowPlusMinus = false;
            this._blogList.ShowRootLines = false;
            this._blogList.Size = new System.Drawing.Size(199, 187);
            this._blogList.TabIndex = 1;
            // 
            // _blogImages
            // 
            this._blogImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_blogImages.ImageStream")));
            this._blogImages.TransparentColor = System.Drawing.Color.Transparent;
            this._blogImages.Images.SetKeyName(0, "Blogger.png");
            this._blogImages.Images.SetKeyName(1, "WordPress.png");
            this._blogImages.Images.SetKeyName(2, "LiveJournal.png");
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(135, 207);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 2;
            this._cancelButton.Text = "&Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
            // 
            // _nextButton
            // 
            this._nextButton.Location = new System.Drawing.Point(54, 207);
            this._nextButton.Name = "_nextButton";
            this._nextButton.Size = new System.Drawing.Size(75, 23);
            this._nextButton.TabIndex = 3;
            this._nextButton.Text = "&Next >";
            this._nextButton.UseVisualStyleBackColor = true;
            this._nextButton.Click += new System.EventHandler(this._nextButton_Click);
            // 
            // AddBlogStepOne
            // 
            this.AcceptButton = this._nextButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(222, 238);
            this.Controls.Add(this._nextButton);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._blogList);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddBlogStepOne";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Blog Engine";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView _blogList;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _nextButton;
        private System.Windows.Forms.ImageList _blogImages;
    }
}