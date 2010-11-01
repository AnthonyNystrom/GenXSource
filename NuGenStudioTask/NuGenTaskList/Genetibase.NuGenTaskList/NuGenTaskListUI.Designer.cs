/* -----------------------------------------------
 * NuGenTaskListUI.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System.ComponentModel;
using System.Windows.Forms;

namespace Genetibase.NuGenTaskList
{
	partial class NuGenTaskListUI
	{
		#region Declarations

		private IContainer _Components = null;
		private Panel _TopLayoutPanel = null;
		private ToolStrip _ToolStrip = null;
		private SplitContainer _SplitContainer = null;
		private NuGenTaskTreeView _TaskTreeView = null;
		private NuGenTaskEditBox _TaskEditBox = null;
		private ToolStripSplitButton _NewTaskSplitButton = null;
		private ToolStripMenuItem _NewTaskMenuItem = null;
		private ToolStripMenuItem _NewFolderMenuItem = null;
		private ToolStripSplitButton _SortBySplitButton = null;
		private ToolStripMenuItem _DescriptionMenuItem = null;
		private ToolStripMenuItem _PriorityMenuItem = null;
		private ToolStripMenuItem _CompletedMenuItem = null;

		#endregion

		#region Dispose

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_Components != null)
				{
					_Components.Dispose();
				}
			}

			base.Dispose(disposing);
		}

		#endregion

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			_Components = new System.ComponentModel.Container();
			_TopLayoutPanel = new System.Windows.Forms.Panel();
			_ToolStrip = new System.Windows.Forms.ToolStrip();
			_NewTaskSplitButton = new System.Windows.Forms.ToolStripSplitButton();
			_NewTaskMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			_NewFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			_SortBySplitButton = new System.Windows.Forms.ToolStripSplitButton();
			_PriorityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			_CompletedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			_DescriptionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			_SplitContainer = new System.Windows.Forms.SplitContainer();
			_TaskTreeView = new Genetibase.NuGenTaskList.NuGenTaskTreeView();
			_TaskEditBox = new Genetibase.NuGenTaskList.NuGenTaskEditBox();
			_TopLayoutPanel.SuspendLayout();
			_ToolStrip.SuspendLayout();
			_SplitContainer.Panel1.SuspendLayout();
			_SplitContainer.Panel2.SuspendLayout();
			_SplitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// topLayoutPanel
			// 
			_TopLayoutPanel.Controls.Add(_ToolStrip);
			_TopLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
			_TopLayoutPanel.Location = new System.Drawing.Point(0, 0);
			_TopLayoutPanel.Name = "topLayoutPanel";
			_TopLayoutPanel.Size = new System.Drawing.Size(265, 27);
			_TopLayoutPanel.TabIndex = 0;
			// 
			// toolStrip
			// 
			_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            _NewTaskSplitButton,
            _SortBySplitButton});
			_ToolStrip.Location = new System.Drawing.Point(0, 0);
			_ToolStrip.Name = "toolStrip";
			_ToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			_ToolStrip.Size = new System.Drawing.Size(265, 25);
			_ToolStrip.TabIndex = 0;
			_ToolStrip.Text = "toolStrip1";
			// 
			// newTaskSplitButton
			// 
			_NewTaskSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            _NewTaskMenuItem,
            _NewFolderMenuItem});
			_NewTaskSplitButton.Image = global::Genetibase.NuGenTaskList.Properties.Resources.Create_New;
			_NewTaskSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			_NewTaskSplitButton.Name = "newTaskSplitButton";
			_NewTaskSplitButton.Size = new System.Drawing.Size(85, 22);
			_NewTaskSplitButton.Text = "New &Task";
			_NewTaskSplitButton.ButtonClick += new System.EventHandler(_NewTaskMenuItem_Click);
			// 
			// newTaskMenuItem
			// 
			_NewTaskMenuItem.Image = global::Genetibase.NuGenTaskList.Properties.Resources.Create_New;
			_NewTaskMenuItem.Name = "newTaskMenuItem";
			_NewTaskMenuItem.Size = new System.Drawing.Size(139, 22);
			_NewTaskMenuItem.Text = "New &Task";
			_NewTaskMenuItem.Click += new System.EventHandler(_NewTaskMenuItem_Click);
			// 
			// newFolderMenuItem
			// 
			_NewFolderMenuItem.Image = global::Genetibase.NuGenTaskList.Properties.Resources.Create_New_Folder;
			_NewFolderMenuItem.Name = "newFolderMenuItem";
			_NewFolderMenuItem.Size = new System.Drawing.Size(139, 22);
			_NewFolderMenuItem.Text = "New &Folder";
			_NewFolderMenuItem.Click += new System.EventHandler(_NewFolderMenuItem_Click);
			// 
			// sortBySplitButton
			// 
			_SortBySplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            _PriorityMenuItem,
            _CompletedMenuItem,
            _DescriptionMenuItem});
			_SortBySplitButton.Image = global::Genetibase.NuGenTaskList.Properties.Resources.Sort_AZ;
			_SortBySplitButton.ImageTransparentColor = System.Drawing.Color.White;
			_SortBySplitButton.Name = "sortBySplitButton";
			_SortBySplitButton.Size = new System.Drawing.Size(74, 22);
			_SortBySplitButton.Text = "&Sort By";
			// 
			// priorityMenuItem
			// 
			_PriorityMenuItem.Image = global::Genetibase.NuGenTaskList.Properties.Resources.Sort_Priority;
			_PriorityMenuItem.Name = "priorityMenuItem";
			_PriorityMenuItem.Size = new System.Drawing.Size(152, 22);
			_PriorityMenuItem.Text = "&Priority";
			_PriorityMenuItem.Click += new System.EventHandler(_PriorityMenuItem_Click);
			// 
			// completedMenuItem
			// 
			_CompletedMenuItem.Image = global::Genetibase.NuGenTaskList.Properties.Resources.Sort_Completed;
			_CompletedMenuItem.Name = "completedMenuItem";
			_CompletedMenuItem.Size = new System.Drawing.Size(152, 22);
			_CompletedMenuItem.Text = "&Completed";
			_CompletedMenuItem.Click += new System.EventHandler(_CompletedMenuItem_Click);
			// 
			// descriptionMenuItem
			// 
			_DescriptionMenuItem.Image = global::Genetibase.NuGenTaskList.Properties.Resources.Sort_Description;
			_DescriptionMenuItem.Name = "descriptionMenuItem";
			_DescriptionMenuItem.Size = new System.Drawing.Size(152, 22);
			_DescriptionMenuItem.Text = "&Description";
			_DescriptionMenuItem.Click += new System.EventHandler(_DescriptionMenuItem_Click);
			// 
			// splitContainer
			// 
			_SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			_SplitContainer.Location = new System.Drawing.Point(0, 27);
			_SplitContainer.Name = "splitContainer";
			_SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer.Panel1
			// 
			_SplitContainer.Panel1.Controls.Add(_TaskTreeView);
			// 
			// splitContainer.Panel2
			// 
			_SplitContainer.Panel2.Controls.Add(_TaskEditBox);
			_SplitContainer.Size = new System.Drawing.Size(265, 373);
			_SplitContainer.SplitterDistance = 260;
			_SplitContainer.TabIndex = 1;
			// 
			// taskTreeView
			// 
			_TaskTreeView.AllowDrop = true;
			_TaskTreeView.CheckBoxes = true;
			_TaskTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			_TaskTreeView.HideSelection = false;
			_TaskTreeView.ImageIndex = 0;
			_TaskTreeView.Location = new System.Drawing.Point(0, 0);
			_TaskTreeView.Name = "taskTreeView";
			_TaskTreeView.SelectedImageIndex = 0;
			_TaskTreeView.ShowLines = false;
			_TaskTreeView.Size = new System.Drawing.Size(265, 165);
			_TaskTreeView.TabIndex = 0;
			// 
			// taskEditBox
			// 
			_TaskEditBox.BackColor = System.Drawing.SystemColors.Window;
			_TaskEditBox.Dock = System.Windows.Forms.DockStyle.Fill;
			_TaskEditBox.Enabled = false;
			_TaskEditBox.Location = new System.Drawing.Point(0, 0);
			_TaskEditBox.Multiline = true;
			_TaskEditBox.Name = "taskEditBox";
			_TaskEditBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			_TaskEditBox.Size = new System.Drawing.Size(265, 204);
			_TaskEditBox.TabIndex = 0;
			// 
			// NuGenTaskListUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(_SplitContainer);
			this.Controls.Add(_TopLayoutPanel);
			this.Name = "NuGenTaskListUI";
			this.Size = new System.Drawing.Size(265, 400);
			_TopLayoutPanel.ResumeLayout(false);
			_TopLayoutPanel.PerformLayout();
			_ToolStrip.ResumeLayout(false);
			_ToolStrip.PerformLayout();
			_SplitContainer.Panel1.ResumeLayout(false);
			_SplitContainer.Panel2.ResumeLayout(false);
			_SplitContainer.Panel2.PerformLayout();
			_SplitContainer.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
