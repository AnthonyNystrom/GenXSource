<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DiagramPageViewer
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container
		Me.MainContainer = New Genetibase.SmoothControls.NuGenSmoothSplitContainer
		Me.CurrentPageViewer = New Genetibase.NugenObjective.Windows.DiagramClient.PageViewerPanel
		Me.PageSelector = New Genetibase.SmoothControls.NuGenSmoothToolStrip
		Me.ToolsContainer = New Genetibase.SmoothControls.NuGenSmoothSplitContainer
		Me.Explorers = New Genetibase.SmoothControls.NuGenSmoothTabControl
		Me.PageExplorer = New Genetibase.SmoothControls.NuGenSmoothTabPage
		Me.PageExplorerTreeView = New System.Windows.Forms.TreeView
		Me.DiagramExplorer = New Genetibase.SmoothControls.NuGenSmoothTabPage
		Me.DiagramExplorerTreeView = New System.Windows.Forms.TreeView
		Me.Properties = New Genetibase.SmoothControls.NuGenSmoothPropertyGrid
		Me.PagesContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.PageRemoveContextMenu = New System.Windows.Forms.ToolStripMenuItem
		Me.PageRenameContextMenu = New System.Windows.Forms.ToolStripMenuItem
		Me.MainContainer.Panel1.SuspendLayout()
		Me.MainContainer.Panel2.SuspendLayout()
		Me.MainContainer.SuspendLayout()
		Me.ToolsContainer.Panel1.SuspendLayout()
		Me.ToolsContainer.Panel2.SuspendLayout()
		Me.ToolsContainer.SuspendLayout()
		Me.Explorers.SuspendLayout()
		Me.PageExplorer.SuspendLayout()
		Me.DiagramExplorer.SuspendLayout()
		Me.PagesContextMenuStrip.SuspendLayout()
		Me.SuspendLayout()
		'
		'MainContainer
		'
		Me.MainContainer.Dock = System.Windows.Forms.DockStyle.Fill
		Me.MainContainer.Location = New System.Drawing.Point(0, 0)
		Me.MainContainer.Name = "MainContainer"
		'
		'MainContainer.Panel1
		'
		Me.MainContainer.Panel1.Controls.Add(Me.CurrentPageViewer)
		Me.MainContainer.Panel1.Controls.Add(Me.PageSelector)
		'
		'MainContainer.Panel2
		'
		Me.MainContainer.Panel2.Controls.Add(Me.ToolsContainer)
		Me.MainContainer.Panel2MinSize = 0
		Me.MainContainer.Size = New System.Drawing.Size(654, 340)
		Me.MainContainer.SplitterDistance = 465
		Me.MainContainer.TabIndex = 0
		Me.MainContainer.TabStop = False
		'
		'CurrentPageViewer
		'
		Me.CurrentPageViewer.BackColor = System.Drawing.SystemColors.Window
		Me.CurrentPageViewer.Dock = System.Windows.Forms.DockStyle.Fill
		Me.CurrentPageViewer.Location = New System.Drawing.Point(0, 0)
		Me.CurrentPageViewer.Name = "CurrentPageViewer"
		Me.CurrentPageViewer.Page = Nothing
		Me.CurrentPageViewer.SelectedElement = Nothing
		Me.CurrentPageViewer.Size = New System.Drawing.Size(465, 315)
		Me.CurrentPageViewer.TabIndex = 1
		'
		'PageSelector
		'
		Me.PageSelector.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.PageSelector.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
		Me.PageSelector.Location = New System.Drawing.Point(0, 315)
		Me.PageSelector.Name = "PageSelector"
		Me.PageSelector.Size = New System.Drawing.Size(465, 25)
		Me.PageSelector.TabIndex = 0
		Me.PageSelector.Text = "ToolStrip1"
		'
		'ToolsContainer
		'
		Me.ToolsContainer.Dock = System.Windows.Forms.DockStyle.Fill
		Me.ToolsContainer.DrawBorder = False
		Me.ToolsContainer.Location = New System.Drawing.Point(0, 0)
		Me.ToolsContainer.Name = "ToolsContainer"
		Me.ToolsContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
		'
		'ToolsContainer.Panel1
		'
		Me.ToolsContainer.Panel1.Controls.Add(Me.Explorers)
		'
		'ToolsContainer.Panel2
		'
		Me.ToolsContainer.Panel2.Controls.Add(Me.Properties)
		Me.ToolsContainer.Size = New System.Drawing.Size(185, 340)
		Me.ToolsContainer.SplitterDistance = 167
		Me.ToolsContainer.TabIndex = 0
		Me.ToolsContainer.TabStop = False
		'
		'Explorers
		'
		Me.Explorers.CloseButtonOnTab = False
		Me.Explorers.Dock = System.Windows.Forms.DockStyle.Fill
		Me.Explorers.Location = New System.Drawing.Point(0, 0)
		Me.Explorers.Name = "Explorers"
		Me.Explorers.Size = New System.Drawing.Size(185, 167)
		Me.Explorers.TabIndex = 2
		Me.Explorers.TabPages.Add(Me.PageExplorer)
		Me.Explorers.TabPages.Add(Me.DiagramExplorer)
		'
		'PageExplorer
		'
		Me.PageExplorer.Controls.Add(Me.PageExplorerTreeView)
		Me.PageExplorer.Location = New System.Drawing.Point(4, 22)
		Me.PageExplorer.Name = "PageExplorer"
		Me.PageExplorer.Padding = New System.Windows.Forms.Padding(3)
		Me.PageExplorer.Size = New System.Drawing.Size(177, 141)
		Me.PageExplorer.TabIndex = 1
		Me.PageExplorer.Text = "Page Explorer"
		'
		'PageExplorerTreeView
		'
		Me.PageExplorerTreeView.Dock = System.Windows.Forms.DockStyle.Fill
		Me.PageExplorerTreeView.HideSelection = False
		Me.PageExplorerTreeView.Location = New System.Drawing.Point(3, 3)
		Me.PageExplorerTreeView.Name = "PageExplorerTreeView"
		Me.PageExplorerTreeView.Size = New System.Drawing.Size(171, 135)
		Me.PageExplorerTreeView.TabIndex = 0
		'
		'DiagramExplorer
		'
		Me.DiagramExplorer.Controls.Add(Me.DiagramExplorerTreeView)
		Me.DiagramExplorer.Location = New System.Drawing.Point(4, 22)
		Me.DiagramExplorer.Name = "DiagramExplorer"
		Me.DiagramExplorer.Padding = New System.Windows.Forms.Padding(3)
		Me.DiagramExplorer.Size = New System.Drawing.Size(158, 141)
		Me.DiagramExplorer.TabIndex = 0
		Me.DiagramExplorer.Text = "Diagram Explorer"
		'
		'DiagramExplorerTreeView
		'
		Me.DiagramExplorerTreeView.Dock = System.Windows.Forms.DockStyle.Fill
		Me.DiagramExplorerTreeView.HideSelection = False
		Me.DiagramExplorerTreeView.Location = New System.Drawing.Point(3, 3)
		Me.DiagramExplorerTreeView.Name = "DiagramExplorerTreeView"
		Me.DiagramExplorerTreeView.Size = New System.Drawing.Size(152, 135)
		Me.DiagramExplorerTreeView.TabIndex = 0
		'
		'Properties
		'
		Me.Properties.Dock = System.Windows.Forms.DockStyle.Fill
		Me.Properties.Location = New System.Drawing.Point(0, 0)
		Me.Properties.Name = "Properties"
		Me.Properties.Size = New System.Drawing.Size(185, 169)
		Me.Properties.TabIndex = 1
		'
		'PagesContextMenuStrip
		'
		Me.PagesContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PageRemoveContextMenu, Me.PageRenameContextMenu})
		Me.PagesContextMenuStrip.Name = "ContextMenuStrip"
		Me.PagesContextMenuStrip.Size = New System.Drawing.Size(125, 48)
		'
		'PageRemoveContextMenu
		'
		Me.PageRemoveContextMenu.Name = "PageRemoveContextMenu"
		Me.PageRemoveContextMenu.Size = New System.Drawing.Size(124, 22)
		Me.PageRemoveContextMenu.Text = "Remove"
		'
		'PageRenameContextMenu
		'
		Me.PageRenameContextMenu.Name = "PageRenameContextMenu"
		Me.PageRenameContextMenu.Size = New System.Drawing.Size(124, 22)
		Me.PageRenameContextMenu.Text = "Rename"
		'
		'DiagramPageViewer
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.Controls.Add(Me.MainContainer)
		Me.Name = "DiagramPageViewer"
		Me.Size = New System.Drawing.Size(654, 340)
		Me.MainContainer.Panel1.ResumeLayout(False)
		Me.MainContainer.Panel1.PerformLayout()
		Me.MainContainer.Panel2.ResumeLayout(False)
		Me.MainContainer.ResumeLayout(False)
		Me.ToolsContainer.Panel1.ResumeLayout(False)
		Me.ToolsContainer.Panel2.ResumeLayout(False)
		Me.ToolsContainer.ResumeLayout(False)
		Me.Explorers.ResumeLayout(False)
		Me.PageExplorer.ResumeLayout(False)
		Me.DiagramExplorer.ResumeLayout(False)
		Me.PagesContextMenuStrip.ResumeLayout(False)
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents MainContainer As Genetibase.SmoothControls.NuGenSmoothSplitContainer
	Friend WithEvents ToolsContainer As Genetibase.SmoothControls.NuGenSmoothSplitContainer
	Friend WithEvents Explorers As Genetibase.SmoothControls.NuGenSmoothTabControl
	Friend WithEvents PageExplorer As Genetibase.SmoothControls.NuGenSmoothTabPage
    Friend WithEvents PageExplorerTreeView As System.Windows.Forms.TreeView
	Friend WithEvents DiagramExplorer As Genetibase.SmoothControls.NuGenSmoothTabPage
    Friend WithEvents DiagramExplorerTreeView As System.Windows.Forms.TreeView
	Friend WithEvents Properties As Genetibase.SmoothControls.NuGenSmoothPropertyGrid
	Friend WithEvents PageSelector As Genetibase.SmoothControls.NuGenSmoothToolStrip
    Friend WithEvents CurrentPageViewer As Genetibase.NugenObjective.Windows.DiagramClient.PageViewerPanel
    Friend WithEvents PagesContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents PageRemoveContextMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PageRenameContextMenu As System.Windows.Forms.ToolStripMenuItem

End Class
