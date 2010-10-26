namespace Genetibase.Shared.Controls.Design
{
    partial class NuGenUIItemsForm
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
            this.labelAssociatedItems = new System.Windows.Forms.Label();
            this.labelCommand = new System.Windows.Forms.Label();
            this.labelCommandName = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.labelAvailableItems = new System.Windows.Forms.Label();
            this.listViewAvailableItems = new System.Windows.Forms.ListView();
            this.columnHeaderAvailableItems = new System.Windows.Forms.ColumnHeader();
            this.listViewAssociatedItems = new System.Windows.Forms.ListView();
            this.columnHeaderAssociatedItems = new System.Windows.Forms.ColumnHeader();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelAssociatedItems
            // 
            this.labelAssociatedItems.AutoSize = true;
            this.labelAssociatedItems.Location = new System.Drawing.Point(3, 0);
            this.labelAssociatedItems.Name = "labelAssociatedItems";
            this.labelAssociatedItems.Size = new System.Drawing.Size(89, 13);
            this.labelAssociatedItems.TabIndex = 1;
            this.labelAssociatedItems.Text = "Associated items:";
            // 
            // labelCommand
            // 
            this.labelCommand.AutoSize = true;
            this.labelCommand.Location = new System.Drawing.Point(12, 9);
            this.labelCommand.Name = "labelCommand";
            this.labelCommand.Size = new System.Drawing.Size(57, 13);
            this.labelCommand.TabIndex = 4;
            this.labelCommand.Text = "Command:";
            // 
            // labelCommandName
            // 
            this.labelCommandName.AutoSize = true;
            this.labelCommandName.Location = new System.Drawing.Point(75, 9);
            this.labelCommandName.Name = "labelCommandName";
            this.labelCommandName.Size = new System.Drawing.Size(94, 13);
            this.labelCommandName.TabIndex = 5;
            this.labelCommandName.Text = "<command name>";
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(297, 261);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(378, 261);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 48);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.labelAvailableItems);
            this.splitContainer1.Panel1.Controls.Add(this.listViewAvailableItems);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listViewAssociatedItems);
            this.splitContainer1.Panel2.Controls.Add(this.labelAssociatedItems);
            this.splitContainer1.Size = new System.Drawing.Size(441, 207);
            this.splitContainer1.SplitterDistance = 207;
            this.splitContainer1.TabIndex = 10;
            // 
            // labelAvailableItems
            // 
            this.labelAvailableItems.AutoSize = true;
            this.labelAvailableItems.Location = new System.Drawing.Point(3, 0);
            this.labelAvailableItems.Name = "labelAvailableItems";
            this.labelAvailableItems.Size = new System.Drawing.Size(80, 13);
            this.labelAvailableItems.TabIndex = 10;
            this.labelAvailableItems.Text = "Available items:";
            // 
            // listViewAvailableItems
            // 
            this.listViewAvailableItems.AllowDrop = true;
            this.listViewAvailableItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewAvailableItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderAvailableItems});
            this.listViewAvailableItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewAvailableItems.Location = new System.Drawing.Point(3, 16);
            this.listViewAvailableItems.Name = "listViewAvailableItems";
            this.listViewAvailableItems.Size = new System.Drawing.Size(201, 188);
            this.listViewAvailableItems.TabIndex = 9;
            this.listViewAvailableItems.UseCompatibleStateImageBehavior = false;
            this.listViewAvailableItems.View = System.Windows.Forms.View.Details;
            this.listViewAvailableItems.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView_DragEnter);
            this.listViewAvailableItems.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView_DragDrop);
            this.listViewAvailableItems.DoubleClick += new System.EventHandler(this.listViewAvailableItems_DoubleClick);
            this.listViewAvailableItems.Resize += new System.EventHandler(this.listViewAvailableItems_Resize);
            this.listViewAvailableItems.DragOver += new System.Windows.Forms.DragEventHandler(this.listView_DragOver);
            this.listViewAvailableItems.DragLeave += new System.EventHandler(this.listView_DragLeave);
            this.listViewAvailableItems.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView_ItemDrag);
            // 
            // listViewAssociatedItems
            // 
            this.listViewAssociatedItems.AllowDrop = true;
            this.listViewAssociatedItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewAssociatedItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderAssociatedItems});
            this.listViewAssociatedItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewAssociatedItems.Location = new System.Drawing.Point(3, 16);
            this.listViewAssociatedItems.Name = "listViewAssociatedItems";
            this.listViewAssociatedItems.Size = new System.Drawing.Size(224, 188);
            this.listViewAssociatedItems.TabIndex = 10;
            this.listViewAssociatedItems.UseCompatibleStateImageBehavior = false;
            this.listViewAssociatedItems.View = System.Windows.Forms.View.Details;
            this.listViewAssociatedItems.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView_DragEnter);
            this.listViewAssociatedItems.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView_DragDrop);
            this.listViewAssociatedItems.DoubleClick += new System.EventHandler(this.listViewAssociatedItems_DoubleClick);
            this.listViewAssociatedItems.Resize += new System.EventHandler(this.listViewAssociatedItems_Resize);
            this.listViewAssociatedItems.DragOver += new System.Windows.Forms.DragEventHandler(this.listView_DragOver);
            this.listViewAssociatedItems.DragLeave += new System.EventHandler(this.listView_DragLeave);
            this.listViewAssociatedItems.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView_ItemDrag);
            // 
            // FormUIItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 296);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelCommandName);
            this.Controls.Add(this.labelCommand);
            this.Name = "FormUIItems";
            this.Text = "UIItem Associations Editor";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAssociatedItems;
        private System.Windows.Forms.Label labelCommand;
        private System.Windows.Forms.Label labelCommandName;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listViewAvailableItems;
        private System.Windows.Forms.ListView listViewAssociatedItems;
        private System.Windows.Forms.Label labelAvailableItems;
        private System.Windows.Forms.ColumnHeader columnHeaderAvailableItems;
        private System.Windows.Forms.ColumnHeader columnHeaderAssociatedItems;
    }
}