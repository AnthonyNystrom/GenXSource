Option Strict On
Option Explicit On

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DiagramDesigner
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DiagramDesigner))
        Me.DiagramOpenFileDialog = New System.Windows.Forms.OpenFileDialog
        Me.DiagramSaveFileDialog = New System.Windows.Forms.SaveFileDialog
        Me.DiagramPrintDocument = New System.Drawing.Printing.PrintDocument
        Me.DiagramPageSetupDialog = New System.Windows.Forms.PageSetupDialog
        Me.DiagramPrintDialog = New System.Windows.Forms.PrintDialog
        Me.MainToolStripContainer = New System.Windows.Forms.ToolStripContainer
        Me.DiagramViewer = New Genetibase.NugenObjective.Windows.DiagramDesigner.DiagramPageViewer
        Me.MainMenu = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenFromServerMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PublishMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExportMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.PageSetupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PrintPreviewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PrintToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.UndoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RedoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.CutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator
        Me.SelectAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExplorerWindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PropertiesWindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CustomizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ServerMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LockMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.UnlockMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PublishServerDiagramMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.UpdateMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ContentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.IndexToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SearchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StandardToolbar = New System.Windows.Forms.ToolStrip
        Me.NewToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.OpenToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.SaveToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.PrintToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.toolStripSeparator = New System.Windows.Forms.ToolStripSeparator
        Me.CutToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.CopyToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.PasteToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.HelpToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator
        Me.ZoomToolStripDropDownButton = New System.Windows.Forms.ToolStripDropDownButton
        Me.FiftyPercentMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HunderedPercentMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OneFiftyPercentMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TwoHundredPercentMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AddPageToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.ToolBox = New System.Windows.Forms.ToolStrip
        Me.NewElementToolStripSplitButton = New System.Windows.Forms.ToolStripSplitButton
        Me.ObjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RoleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ActionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AddInteractionToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.DeleteToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.NuGenSmoothToolStripManager = New Genetibase.SmoothControls.NuGenSmoothToolStripManager
        Me.DiagramPrintPreview = New Genetibase.SmoothControls.NuGenSmoothPrintPreview
        Me.NuGenSmoothToolTip1 = New Genetibase.SmoothControls.NuGenSmoothToolTip
        Me.MainToolStripContainer.ContentPanel.SuspendLayout()
        Me.MainToolStripContainer.TopToolStripPanel.SuspendLayout()
        Me.MainToolStripContainer.SuspendLayout()
        Me.MainMenu.SuspendLayout()
        Me.StandardToolbar.SuspendLayout()
        Me.ToolBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'DiagramOpenFileDialog
        '
        Me.DiagramOpenFileDialog.DereferenceLinks = False
        '
        'DiagramPrintDocument
        '
        Me.DiagramPrintDocument.OriginAtMargins = True
        '
        'DiagramPageSetupDialog
        '
        Me.DiagramPageSetupDialog.Document = Me.DiagramPrintDocument
        '
        'DiagramPrintDialog
        '
        Me.DiagramPrintDialog.Document = Me.DiagramPrintDocument
        Me.DiagramPrintDialog.UseEXDialog = True
        '
        'MainToolStripContainer
        '
        '
        'MainToolStripContainer.ContentPanel
        '
        Me.MainToolStripContainer.ContentPanel.Controls.Add(Me.DiagramViewer)
        Me.MainToolStripContainer.ContentPanel.Size = New System.Drawing.Size(819, 408)
        Me.MainToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainToolStripContainer.Location = New System.Drawing.Point(0, 0)
        Me.MainToolStripContainer.Name = "MainToolStripContainer"
        Me.MainToolStripContainer.Size = New System.Drawing.Size(819, 457)
        Me.MainToolStripContainer.TabIndex = 3
        Me.MainToolStripContainer.Text = "ToolStripContainer1"
        '
        'MainToolStripContainer.TopToolStripPanel
        '
        Me.MainToolStripContainer.TopToolStripPanel.Controls.Add(Me.MainMenu)
        Me.MainToolStripContainer.TopToolStripPanel.Controls.Add(Me.StandardToolbar)
        Me.MainToolStripContainer.TopToolStripPanel.Controls.Add(Me.ToolBox)
        '
        'DiagramViewer
        '
        Me.DiagramViewer.Diagram = Nothing
        Me.DiagramViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DiagramViewer.EditingEnabled = True
        Me.DiagramViewer.ExplorerWindowVisible = True
        Me.DiagramViewer.Location = New System.Drawing.Point(0, 0)
        Me.DiagramViewer.Name = "DiagramViewer"
        Me.DiagramViewer.PropertiesWindowVisible = True
        Me.DiagramViewer.Size = New System.Drawing.Size(819, 408)
        Me.DiagramViewer.TabIndex = 0
        '
        'MainMenu
        '
        Me.MainMenu.Dock = System.Windows.Forms.DockStyle.None
        Me.MainMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.MainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.ViewToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.ServerMenuItem, Me.HelpToolStripMenuItem})
        Me.MainMenu.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu.Name = "MainMenu"
        Me.MainMenu.Size = New System.Drawing.Size(819, 24)
        Me.MainMenu.TabIndex = 0
        Me.MainMenu.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.OpenToolStripMenuItem, Me.OpenFromServerMenuItem, Me.toolStripSeparator2, Me.SaveToolStripMenuItem, Me.SaveAsToolStripMenuItem, Me.PublishMenuItem, Me.ExportMenuItem, Me.toolStripSeparator3, Me.PageSetupToolStripMenuItem, Me.PrintPreviewToolStripMenuItem, Me.PrintToolStripMenuItem, Me.toolStripSeparator4, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Image = CType(resources.GetObject("NewToolStripMenuItem.Image"), System.Drawing.Image)
        Me.NewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.NewToolStripMenuItem.Text = "&New"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Image = CType(resources.GetObject("OpenToolStripMenuItem.Image"), System.Drawing.Image)
        Me.OpenToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.OpenToolStripMenuItem.Text = "&Open"
        '
        'OpenFromServerMenuItem
        '
        Me.OpenFromServerMenuItem.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.CheckOut
        Me.OpenFromServerMenuItem.Name = "OpenFromServerMenuItem"
        Me.OpenFromServerMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.OpenFromServerMenuItem.Text = "Open &from server..."
        '
        'toolStripSeparator2
        '
        Me.toolStripSeparator2.Name = "toolStripSeparator2"
        Me.toolStripSeparator2.Size = New System.Drawing.Size(179, 6)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Image = CType(resources.GetObject("SaveToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SaveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.SaveToolStripMenuItem.Text = "&Save"
        '
        'SaveAsToolStripMenuItem
        '
        Me.SaveAsToolStripMenuItem.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Save_As
        Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
        Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.SaveAsToolStripMenuItem.Text = "Save &As..."
        '
        'PublishMenuItem
        '
        Me.PublishMenuItem.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Publish_Site
        Me.PublishMenuItem.Name = "PublishMenuItem"
        Me.PublishMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.PublishMenuItem.Text = "Pu&blish to server..."
        '
        'ExportMenuItem
        '
        Me.ExportMenuItem.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Export
        Me.ExportMenuItem.Name = "ExportMenuItem"
        Me.ExportMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.ExportMenuItem.Text = "&Export..."
        '
        'toolStripSeparator3
        '
        Me.toolStripSeparator3.Name = "toolStripSeparator3"
        Me.toolStripSeparator3.Size = New System.Drawing.Size(179, 6)
        '
        'PageSetupToolStripMenuItem
        '
        Me.PageSetupToolStripMenuItem.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Page_Setup
        Me.PageSetupToolStripMenuItem.Name = "PageSetupToolStripMenuItem"
        Me.PageSetupToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.PageSetupToolStripMenuItem.Text = "Page Set&up..."
        '
        'PrintPreviewToolStripMenuItem
        '
        Me.PrintPreviewToolStripMenuItem.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Preview
        Me.PrintPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.PrintPreviewToolStripMenuItem.Name = "PrintPreviewToolStripMenuItem"
        Me.PrintPreviewToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.PrintPreviewToolStripMenuItem.Text = "Print Pre&view"
        '
        'PrintToolStripMenuItem
        '
        Me.PrintToolStripMenuItem.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Print
        Me.PrintToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.PrintToolStripMenuItem.Name = "PrintToolStripMenuItem"
        Me.PrintToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.PrintToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.PrintToolStripMenuItem.Text = "&Print"
        '
        'toolStripSeparator4
        '
        Me.toolStripSeparator4.Name = "toolStripSeparator4"
        Me.toolStripSeparator4.Size = New System.Drawing.Size(179, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources._Exit
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UndoToolStripMenuItem, Me.RedoToolStripMenuItem, Me.toolStripSeparator5, Me.CutToolStripMenuItem, Me.CopyToolStripMenuItem, Me.PasteToolStripMenuItem, Me.toolStripSeparator6, Me.SelectAllToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.EditToolStripMenuItem.Text = "&Edit"
        '
        'UndoToolStripMenuItem
        '
        Me.UndoToolStripMenuItem.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Undo
        Me.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem"
        Me.UndoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
        Me.UndoToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.UndoToolStripMenuItem.Text = "&Undo"
        '
        'RedoToolStripMenuItem
        '
        Me.RedoToolStripMenuItem.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Redo
        Me.RedoToolStripMenuItem.Name = "RedoToolStripMenuItem"
        Me.RedoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Y), System.Windows.Forms.Keys)
        Me.RedoToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.RedoToolStripMenuItem.Text = "&Redo"
        '
        'toolStripSeparator5
        '
        Me.toolStripSeparator5.Name = "toolStripSeparator5"
        Me.toolStripSeparator5.Size = New System.Drawing.Size(147, 6)
        '
        'CutToolStripMenuItem
        '
        Me.CutToolStripMenuItem.Image = CType(resources.GetObject("CutToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CutToolStripMenuItem.Name = "CutToolStripMenuItem"
        Me.CutToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.CutToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.CutToolStripMenuItem.Text = "Cu&t"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Image = CType(resources.GetObject("CopyToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CopyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.CopyToolStripMenuItem.Text = "&Copy"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Image = CType(resources.GetObject("PasteToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.PasteToolStripMenuItem.Text = "&Paste"
        '
        'toolStripSeparator6
        '
        Me.toolStripSeparator6.Name = "toolStripSeparator6"
        Me.toolStripSeparator6.Size = New System.Drawing.Size(147, 6)
        '
        'SelectAllToolStripMenuItem
        '
        Me.SelectAllToolStripMenuItem.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Select_All
        Me.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem"
        Me.SelectAllToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.SelectAllToolStripMenuItem.Text = "Select &All"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExplorerWindowToolStripMenuItem, Me.PropertiesWindowToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(41, 20)
        Me.ViewToolStripMenuItem.Text = "&View"
        '
        'ExplorerWindowToolStripMenuItem
        '
        Me.ExplorerWindowToolStripMenuItem.Checked = True
        Me.ExplorerWindowToolStripMenuItem.CheckOnClick = True
        Me.ExplorerWindowToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ExplorerWindowToolStripMenuItem.Name = "ExplorerWindowToolStripMenuItem"
        Me.ExplorerWindowToolStripMenuItem.Size = New System.Drawing.Size(175, 22)
        Me.ExplorerWindowToolStripMenuItem.Text = "&Explorer Window"
        '
        'PropertiesWindowToolStripMenuItem
        '
        Me.PropertiesWindowToolStripMenuItem.Checked = True
        Me.PropertiesWindowToolStripMenuItem.CheckOnClick = True
        Me.PropertiesWindowToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.PropertiesWindowToolStripMenuItem.Name = "PropertiesWindowToolStripMenuItem"
        Me.PropertiesWindowToolStripMenuItem.Size = New System.Drawing.Size(175, 22)
        Me.PropertiesWindowToolStripMenuItem.Text = "&Properties Window"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CustomizeToolStripMenuItem, Me.OptionsToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ToolsToolStripMenuItem.Text = "&Tools"
        '
        'CustomizeToolStripMenuItem
        '
        Me.CustomizeToolStripMenuItem.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Tools
        Me.CustomizeToolStripMenuItem.Name = "CustomizeToolStripMenuItem"
        Me.CustomizeToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.CustomizeToolStripMenuItem.Text = "&Customize"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Pivot_Table
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.OptionsToolStripMenuItem.Text = "&Options..."
        '
        'ServerMenuItem
        '
        Me.ServerMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LockMenuItem, Me.UnlockMenuItem, Me.PublishServerDiagramMenuItem, Me.UpdateMenuItem})
        Me.ServerMenuItem.Name = "ServerMenuItem"
        Me.ServerMenuItem.Size = New System.Drawing.Size(51, 20)
        Me.ServerMenuItem.Text = "&Server"
        Me.ServerMenuItem.Visible = False
        '
        'LockMenuItem
        '
        Me.LockMenuItem.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Lock
        Me.LockMenuItem.Name = "LockMenuItem"
        Me.LockMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.LockMenuItem.Text = "Lock"
        '
        'UnlockMenuItem
        '
        Me.UnlockMenuItem.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Unlock
        Me.UnlockMenuItem.Name = "UnlockMenuItem"
        Me.UnlockMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.UnlockMenuItem.Text = "Unlock"
        '
        'PublishServerDiagramMenuItem
        '
        Me.PublishServerDiagramMenuItem.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Publish_Site1
        Me.PublishServerDiagramMenuItem.Name = "PublishServerDiagramMenuItem"
        Me.PublishServerDiagramMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.PublishServerDiagramMenuItem.Text = "Publish"
        '
        'UpdateMenuItem
        '
        Me.UpdateMenuItem.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Refresh
        Me.UpdateMenuItem.Name = "UpdateMenuItem"
        Me.UpdateMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.UpdateMenuItem.Text = "Update"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ContentsToolStripMenuItem, Me.IndexToolStripMenuItem, Me.SearchToolStripMenuItem, Me.toolStripSeparator7, Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(40, 20)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'ContentsToolStripMenuItem
        '
        Me.ContentsToolStripMenuItem.Name = "ContentsToolStripMenuItem"
        Me.ContentsToolStripMenuItem.Size = New System.Drawing.Size(129, 22)
        Me.ContentsToolStripMenuItem.Text = "&Contents"
        '
        'IndexToolStripMenuItem
        '
        Me.IndexToolStripMenuItem.Name = "IndexToolStripMenuItem"
        Me.IndexToolStripMenuItem.Size = New System.Drawing.Size(129, 22)
        Me.IndexToolStripMenuItem.Text = "&Index"
        '
        'SearchToolStripMenuItem
        '
        Me.SearchToolStripMenuItem.Name = "SearchToolStripMenuItem"
        Me.SearchToolStripMenuItem.Size = New System.Drawing.Size(129, 22)
        Me.SearchToolStripMenuItem.Text = "&Search"
        '
        'toolStripSeparator7
        '
        Me.toolStripSeparator7.Name = "toolStripSeparator7"
        Me.toolStripSeparator7.Size = New System.Drawing.Size(126, 6)
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(129, 22)
        Me.AboutToolStripMenuItem.Text = "&About..."
        '
        'StandardToolbar
        '
        Me.StandardToolbar.Dock = System.Windows.Forms.DockStyle.None
        Me.StandardToolbar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripButton, Me.OpenToolStripButton, Me.SaveToolStripButton, Me.PrintToolStripButton, Me.toolStripSeparator, Me.CutToolStripButton, Me.CopyToolStripButton, Me.PasteToolStripButton, Me.toolStripSeparator1, Me.HelpToolStripButton, Me.ToolStripSeparator8, Me.ZoomToolStripDropDownButton, Me.AddPageToolStripButton})
        Me.StandardToolbar.Location = New System.Drawing.Point(3, 24)
        Me.StandardToolbar.Name = "StandardToolbar"
        Me.StandardToolbar.Size = New System.Drawing.Size(266, 25)
        Me.StandardToolbar.TabIndex = 1
        Me.StandardToolbar.Text = "ToolStrip1"
        '
        'NewToolStripButton
        '
        Me.NewToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.NewToolStripButton.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Blank
        Me.NewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.NewToolStripButton.Name = "NewToolStripButton"
        Me.NewToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.NewToolStripButton.Text = "&New"
        '
        'OpenToolStripButton
        '
        Me.OpenToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.OpenToolStripButton.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Open
        Me.OpenToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.OpenToolStripButton.Name = "OpenToolStripButton"
        Me.OpenToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.OpenToolStripButton.Text = "&Open"
        '
        'SaveToolStripButton
        '
        Me.SaveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.SaveToolStripButton.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Save
        Me.SaveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveToolStripButton.Name = "SaveToolStripButton"
        Me.SaveToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.SaveToolStripButton.Text = "&Save"
        '
        'PrintToolStripButton
        '
        Me.PrintToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.PrintToolStripButton.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Print
        Me.PrintToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.PrintToolStripButton.Name = "PrintToolStripButton"
        Me.PrintToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.PrintToolStripButton.Text = "&Print"
        '
        'toolStripSeparator
        '
        Me.toolStripSeparator.Name = "toolStripSeparator"
        Me.toolStripSeparator.Size = New System.Drawing.Size(6, 25)
        '
        'CutToolStripButton
        '
        Me.CutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.CutToolStripButton.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Cut
        Me.CutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CutToolStripButton.Name = "CutToolStripButton"
        Me.CutToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.CutToolStripButton.Text = "C&ut"
        '
        'CopyToolStripButton
        '
        Me.CopyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.CopyToolStripButton.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Copy
        Me.CopyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CopyToolStripButton.Name = "CopyToolStripButton"
        Me.CopyToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.CopyToolStripButton.Text = "&Copy"
        '
        'PasteToolStripButton
        '
        Me.PasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.PasteToolStripButton.Image = CType(resources.GetObject("PasteToolStripButton.Image"), System.Drawing.Image)
        Me.PasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.PasteToolStripButton.Name = "PasteToolStripButton"
        Me.PasteToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.PasteToolStripButton.Text = "&Paste"
        '
        'toolStripSeparator1
        '
        Me.toolStripSeparator1.Name = "toolStripSeparator1"
        Me.toolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'HelpToolStripButton
        '
        Me.HelpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.HelpToolStripButton.Image = CType(resources.GetObject("HelpToolStripButton.Image"), System.Drawing.Image)
        Me.HelpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.HelpToolStripButton.Name = "HelpToolStripButton"
        Me.HelpToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.HelpToolStripButton.Text = "He&lp"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 25)
        '
        'ZoomToolStripDropDownButton
        '
        Me.ZoomToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ZoomToolStripDropDownButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FiftyPercentMenuItem, Me.HunderedPercentMenuItem, Me.OneFiftyPercentMenuItem, Me.TwoHundredPercentMenuItem})
        Me.ZoomToolStripDropDownButton.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Zoom
        Me.ZoomToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ZoomToolStripDropDownButton.Name = "ZoomToolStripDropDownButton"
        Me.ZoomToolStripDropDownButton.Size = New System.Drawing.Size(29, 22)
        Me.ZoomToolStripDropDownButton.Text = "ZoomToolStripDropDownButton"
        Me.ZoomToolStripDropDownButton.ToolTipText = "Zoom"
        '
        'FiftyPercentMenuItem
        '
        Me.FiftyPercentMenuItem.Name = "FiftyPercentMenuItem"
        Me.FiftyPercentMenuItem.Size = New System.Drawing.Size(114, 22)
        Me.FiftyPercentMenuItem.Text = "50%"
        '
        'HunderedPercentMenuItem
        '
        Me.HunderedPercentMenuItem.Name = "HunderedPercentMenuItem"
        Me.HunderedPercentMenuItem.Size = New System.Drawing.Size(114, 22)
        Me.HunderedPercentMenuItem.Text = "100%"
        '
        'OneFiftyPercentMenuItem
        '
        Me.OneFiftyPercentMenuItem.Name = "OneFiftyPercentMenuItem"
        Me.OneFiftyPercentMenuItem.Size = New System.Drawing.Size(114, 22)
        Me.OneFiftyPercentMenuItem.Text = "150%"
        '
        'TwoHundredPercentMenuItem
        '
        Me.TwoHundredPercentMenuItem.Name = "TwoHundredPercentMenuItem"
        Me.TwoHundredPercentMenuItem.Size = New System.Drawing.Size(114, 22)
        Me.TwoHundredPercentMenuItem.Text = "200%"
        '
        'AddPageToolStripButton
        '
        Me.AddPageToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.AddPageToolStripButton.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.NewPage
        Me.AddPageToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.AddPageToolStripButton.Name = "AddPageToolStripButton"
        Me.AddPageToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.AddPageToolStripButton.Text = "ToolStripButton1"
        Me.AddPageToolStripButton.ToolTipText = "Add new page"
        '
        'ToolBox
        '
        Me.ToolBox.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolBox.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewElementToolStripSplitButton, Me.AddInteractionToolStripButton, Me.DeleteToolStripButton})
        Me.ToolBox.Location = New System.Drawing.Point(269, 24)
        Me.ToolBox.Name = "ToolBox"
        Me.ToolBox.Size = New System.Drawing.Size(90, 25)
        Me.ToolBox.TabIndex = 2
        Me.ToolBox.Text = "ToolBox"
        '
        'NewElementToolStripSplitButton
        '
        Me.NewElementToolStripSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.NewElementToolStripSplitButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ObjectToolStripMenuItem, Me.StateToolStripMenuItem, Me.RoleToolStripMenuItem, Me.ActionToolStripMenuItem})
        Me.NewElementToolStripSplitButton.Image = CType(resources.GetObject("NewElementToolStripSplitButton.Image"), System.Drawing.Image)
        Me.NewElementToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.NewElementToolStripSplitButton.Name = "NewElementToolStripSplitButton"
        Me.NewElementToolStripSplitButton.Size = New System.Drawing.Size(32, 22)
        '
        'ObjectToolStripMenuItem
        '
        Me.ObjectToolStripMenuItem.Image = CType(resources.GetObject("ObjectToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ObjectToolStripMenuItem.Name = "ObjectToolStripMenuItem"
        Me.ObjectToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ObjectToolStripMenuItem.Text = "Object"
        Me.ObjectToolStripMenuItem.ToolTipText = "Add new Object"
        '
        'StateToolStripMenuItem
        '
        Me.StateToolStripMenuItem.Image = CType(resources.GetObject("StateToolStripMenuItem.Image"), System.Drawing.Image)
        Me.StateToolStripMenuItem.Name = "StateToolStripMenuItem"
        Me.StateToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.StateToolStripMenuItem.Text = "State"
        Me.StateToolStripMenuItem.ToolTipText = "Add new State"
        '
        'RoleToolStripMenuItem
        '
        Me.RoleToolStripMenuItem.Image = CType(resources.GetObject("RoleToolStripMenuItem.Image"), System.Drawing.Image)
        Me.RoleToolStripMenuItem.Name = "RoleToolStripMenuItem"
        Me.RoleToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.RoleToolStripMenuItem.Text = "Role"
        Me.RoleToolStripMenuItem.ToolTipText = "Add new Role"
        '
        'ActionToolStripMenuItem
        '
        Me.ActionToolStripMenuItem.Image = CType(resources.GetObject("ActionToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ActionToolStripMenuItem.Name = "ActionToolStripMenuItem"
        Me.ActionToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ActionToolStripMenuItem.Text = "Action"
        Me.ActionToolStripMenuItem.ToolTipText = "Add new Action"
        '
        'AddInteractionToolStripButton
        '
        Me.AddInteractionToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.AddInteractionToolStripButton.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Shapes_Connect
        Me.AddInteractionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.AddInteractionToolStripButton.Name = "AddInteractionToolStripButton"
        Me.AddInteractionToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.AddInteractionToolStripButton.Text = "Add Interaction"
        Me.AddInteractionToolStripButton.ToolTipText = "Add Interaction"
        '
        'DeleteToolStripButton
        '
        Me.DeleteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.DeleteToolStripButton.Image = Global.Genetibase.NugenObjective.Windows.DiagramDesigner.My.Resources.Resources.Delete
        Me.DeleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DeleteToolStripButton.Name = "DeleteToolStripButton"
        Me.DeleteToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.DeleteToolStripButton.Text = "Delete"
        Me.DeleteToolStripButton.ToolTipText = "Remove"
        '
        'DiagramPrintPreview
        '
        Me.DiagramPrintPreview.AutoScrollMargin = New System.Drawing.Size(0, 0)
        Me.DiagramPrintPreview.AutoScrollMinSize = New System.Drawing.Size(0, 0)
        Me.DiagramPrintPreview.ClientSize = New System.Drawing.Size(632, 446)
        Me.DiagramPrintPreview.Document = Me.DiagramPrintDocument
        Me.DiagramPrintPreview.Icon = CType(resources.GetObject("DiagramPrintPreview.Icon"), System.Drawing.Icon)
        Me.DiagramPrintPreview.Location = New System.Drawing.Point(176, 232)
        Me.DiagramPrintPreview.Name = "DiagramPrintPreview"
        Me.DiagramPrintPreview.SizeGripStyle = System.Windows.Forms.SizeGripStyle.[Auto]
        Me.DiagramPrintPreview.Text = "PrintPreview"
        Me.DiagramPrintPreview.Visible = False
        '
        'NuGenSmoothToolTip1
        '
        Me.NuGenSmoothToolTip1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '
        'DiagramDesigner
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(819, 457)
        Me.Controls.Add(Me.MainToolStripContainer)
        Me.DoubleBuffered = True
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.MainMenu
        Me.Name = "DiagramDesigner"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.MainToolStripContainer.ContentPanel.ResumeLayout(False)
        Me.MainToolStripContainer.TopToolStripPanel.ResumeLayout(False)
        Me.MainToolStripContainer.TopToolStripPanel.PerformLayout()
        Me.MainToolStripContainer.ResumeLayout(False)
        Me.MainToolStripContainer.PerformLayout()
        Me.MainMenu.ResumeLayout(False)
        Me.MainMenu.PerformLayout()
        Me.StandardToolbar.ResumeLayout(False)
        Me.StandardToolbar.PerformLayout()
        Me.ToolBox.ResumeLayout(False)
        Me.ToolBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MainMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents StandardToolbar As System.Windows.Forms.ToolStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveAsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PrintToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PrintPreviewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UndoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RedoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SelectAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CustomizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContentsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents IndexToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SearchToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents OpenToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents SaveToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents PrintToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CutToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents CopyToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents PasteToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents HelpToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolBox As System.Windows.Forms.ToolStrip
    Friend WithEvents AddInteractionToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents DeleteToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents NewElementToolStripSplitButton As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents ObjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RoleToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ActionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DiagramOpenFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents DiagramSaveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ZoomToolStripDropDownButton As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents FiftyPercentMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HunderedPercentMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OneFiftyPercentMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TwoHundredPercentMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddPageToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents PublishMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenFromServerMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ServerMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LockMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UnlockMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PublishServerDiagramMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UpdateMenuItem As System.Windows.Forms.ToolStripMenuItem
	Friend WithEvents PageSetupToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DiagramPrintDocument As System.Drawing.Printing.PrintDocument
    Friend WithEvents DiagramPageSetupDialog As System.Windows.Forms.PageSetupDialog
    Friend WithEvents DiagramPrintDialog As System.Windows.Forms.PrintDialog
    Friend WithEvents MainToolStripContainer As System.Windows.Forms.ToolStripContainer
    Friend WithEvents DiagramViewer As Genetibase.NugenObjective.Windows.DiagramDesigner.DiagramPageViewer
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExplorerWindowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Friend WithEvents PropertiesWindowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Friend WithEvents ExportMenuItem As System.Windows.Forms.ToolStripMenuItem
	Friend WithEvents NuGenSmoothToolStripManager As Genetibase.SmoothControls.NuGenSmoothToolStripManager
    Private WithEvents DiagramPrintPreview As Genetibase.SmoothControls.NuGenSmoothPrintPreview
    Friend WithEvents NuGenSmoothToolTip1 As Genetibase.SmoothControls.NuGenSmoothToolTip

End Class
