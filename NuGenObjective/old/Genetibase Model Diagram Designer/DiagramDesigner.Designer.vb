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
        Me.MainMenu = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenFromServerMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PublishMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.PageSetupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PrintToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PrintPreviewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
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
        Me.DiagramOpenFileDialog = New System.Windows.Forms.OpenFileDialog
        Me.DiagramSaveFileDialog = New System.Windows.Forms.SaveFileDialog
        Me.DiagramPrintPreviewDialog = New System.Windows.Forms.PrintPreviewDialog
        Me.DiagramPrintDocument = New System.Drawing.Printing.PrintDocument
        Me.DiagramPageSetupDialog = New System.Windows.Forms.PageSetupDialog
        Me.DiagramPrintDialog = New System.Windows.Forms.PrintDialog
        Me.MainToolStripContainer = New System.Windows.Forms.ToolStripContainer
        Me.DiagramViewer = New Genetibase.NugenObjective.Windows.DiagramDesigner.DiagramPageViewer
        Me.MainMenu.SuspendLayout()
        Me.StandardToolbar.SuspendLayout()
        Me.ToolBox.SuspendLayout()
        Me.MainToolStripContainer.ContentPanel.SuspendLayout()
        Me.MainToolStripContainer.TopToolStripPanel.SuspendLayout()
        Me.MainToolStripContainer.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu
        '
        Me.MainMenu.Dock = System.Windows.Forms.DockStyle.None
        Me.MainMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.MainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.ViewToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.ServerMenuItem, Me.HelpToolStripMenuItem})
        Me.MainMenu.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu.Name = "MainMenu"
        Me.MainMenu.Size = New System.Drawing.Size(550, 24)
        Me.MainMenu.TabIndex = 0
        Me.MainMenu.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.OpenToolStripMenuItem, Me.OpenFromServerMenuItem, Me.toolStripSeparator2, Me.SaveToolStripMenuItem, Me.SaveAsToolStripMenuItem, Me.PublishMenuItem, Me.toolStripSeparator3, Me.PageSetupToolStripMenuItem, Me.PrintToolStripMenuItem, Me.PrintPreviewToolStripMenuItem, Me.toolStripSeparator4, Me.ExitToolStripMenuItem})
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
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.NewToolStripMenuItem.Text = "&New"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Image = CType(resources.GetObject("OpenToolStripMenuItem.Image"), System.Drawing.Image)
        Me.OpenToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.OpenToolStripMenuItem.Text = "&Open"
        '
        'OpenFromServerMenuItem
        '
        Me.OpenFromServerMenuItem.Image = CType(resources.GetObject("OpenFromServerMenuItem.Image"), System.Drawing.Image)
        Me.OpenFromServerMenuItem.Name = "OpenFromServerMenuItem"
        Me.OpenFromServerMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.OpenFromServerMenuItem.Text = "Open &from server..."
        '
        'toolStripSeparator2
        '
        Me.toolStripSeparator2.Name = "toolStripSeparator2"
        Me.toolStripSeparator2.Size = New System.Drawing.Size(168, 6)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Image = CType(resources.GetObject("SaveToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SaveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.SaveToolStripMenuItem.Text = "&Save"
        '
        'SaveAsToolStripMenuItem
        '
        Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
        Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.SaveAsToolStripMenuItem.Text = "Save &As"
        '
        'PublishMenuItem
        '
        Me.PublishMenuItem.Image = CType(resources.GetObject("PublishMenuItem.Image"), System.Drawing.Image)
        Me.PublishMenuItem.Name = "PublishMenuItem"
        Me.PublishMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.PublishMenuItem.Text = "P&ublish to server..."
        '
        'toolStripSeparator3
        '
        Me.toolStripSeparator3.Name = "toolStripSeparator3"
        Me.toolStripSeparator3.Size = New System.Drawing.Size(168, 6)
        '
        'PageSetupToolStripMenuItem
        '
        Me.PageSetupToolStripMenuItem.Name = "PageSetupToolStripMenuItem"
        Me.PageSetupToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.PageSetupToolStripMenuItem.Text = "Page Set&up..."
        '
        'PrintToolStripMenuItem
        '
        Me.PrintToolStripMenuItem.Image = CType(resources.GetObject("PrintToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PrintToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.PrintToolStripMenuItem.Name = "PrintToolStripMenuItem"
        Me.PrintToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.PrintToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.PrintToolStripMenuItem.Text = "&Print"
        '
        'PrintPreviewToolStripMenuItem
        '
        Me.PrintPreviewToolStripMenuItem.Image = CType(resources.GetObject("PrintPreviewToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PrintPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.PrintPreviewToolStripMenuItem.Name = "PrintPreviewToolStripMenuItem"
        Me.PrintPreviewToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.PrintPreviewToolStripMenuItem.Text = "Print Pre&view"
        '
        'toolStripSeparator4
        '
        Me.toolStripSeparator4.Name = "toolStripSeparator4"
        Me.toolStripSeparator4.Size = New System.Drawing.Size(168, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
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
        Me.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem"
        Me.UndoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
        Me.UndoToolStripMenuItem.Size = New System.Drawing.Size(139, 22)
        Me.UndoToolStripMenuItem.Text = "&Undo"
        '
        'RedoToolStripMenuItem
        '
        Me.RedoToolStripMenuItem.Name = "RedoToolStripMenuItem"
        Me.RedoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Y), System.Windows.Forms.Keys)
        Me.RedoToolStripMenuItem.Size = New System.Drawing.Size(139, 22)
        Me.RedoToolStripMenuItem.Text = "&Redo"
        '
        'toolStripSeparator5
        '
        Me.toolStripSeparator5.Name = "toolStripSeparator5"
        Me.toolStripSeparator5.Size = New System.Drawing.Size(136, 6)
        '
        'CutToolStripMenuItem
        '
        Me.CutToolStripMenuItem.Image = CType(resources.GetObject("CutToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CutToolStripMenuItem.Name = "CutToolStripMenuItem"
        Me.CutToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.CutToolStripMenuItem.Size = New System.Drawing.Size(139, 22)
        Me.CutToolStripMenuItem.Text = "Cu&t"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Image = CType(resources.GetObject("CopyToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CopyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(139, 22)
        Me.CopyToolStripMenuItem.Text = "&Copy"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Image = CType(resources.GetObject("PasteToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(139, 22)
        Me.PasteToolStripMenuItem.Text = "&Paste"
        '
        'toolStripSeparator6
        '
        Me.toolStripSeparator6.Name = "toolStripSeparator6"
        Me.toolStripSeparator6.Size = New System.Drawing.Size(136, 6)
        '
        'SelectAllToolStripMenuItem
        '
        Me.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem"
        Me.SelectAllToolStripMenuItem.Size = New System.Drawing.Size(139, 22)
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
        Me.ExplorerWindowToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.ExplorerWindowToolStripMenuItem.Text = "&Explorer Window"
        '
        'PropertiesWindowToolStripMenuItem
        '
        Me.PropertiesWindowToolStripMenuItem.Checked = True
        Me.PropertiesWindowToolStripMenuItem.CheckOnClick = True
        Me.PropertiesWindowToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.PropertiesWindowToolStripMenuItem.Name = "PropertiesWindowToolStripMenuItem"
        Me.PropertiesWindowToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
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
        Me.CustomizeToolStripMenuItem.Name = "CustomizeToolStripMenuItem"
        Me.CustomizeToolStripMenuItem.Size = New System.Drawing.Size(123, 22)
        Me.CustomizeToolStripMenuItem.Text = "&Customize"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(123, 22)
        Me.OptionsToolStripMenuItem.Text = "&Options"
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
        Me.LockMenuItem.Image = CType(resources.GetObject("LockMenuItem.Image"), System.Drawing.Image)
        Me.LockMenuItem.Name = "LockMenuItem"
        Me.LockMenuItem.Size = New System.Drawing.Size(109, 22)
        Me.LockMenuItem.Text = "Lock"
        '
        'UnlockMenuItem
        '
        Me.UnlockMenuItem.Image = CType(resources.GetObject("UnlockMenuItem.Image"), System.Drawing.Image)
        Me.UnlockMenuItem.Name = "UnlockMenuItem"
        Me.UnlockMenuItem.Size = New System.Drawing.Size(109, 22)
        Me.UnlockMenuItem.Text = "Unlock"
        '
        'PublishServerDiagramMenuItem
        '
        Me.PublishServerDiagramMenuItem.Image = CType(resources.GetObject("PublishServerDiagramMenuItem.Image"), System.Drawing.Image)
        Me.PublishServerDiagramMenuItem.Name = "PublishServerDiagramMenuItem"
        Me.PublishServerDiagramMenuItem.Size = New System.Drawing.Size(109, 22)
        Me.PublishServerDiagramMenuItem.Text = "Publish"
        '
        'UpdateMenuItem
        '
        Me.UpdateMenuItem.Image = CType(resources.GetObject("UpdateMenuItem.Image"), System.Drawing.Image)
        Me.UpdateMenuItem.Name = "UpdateMenuItem"
        Me.UpdateMenuItem.Size = New System.Drawing.Size(109, 22)
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
        Me.ContentsToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ContentsToolStripMenuItem.Text = "&Contents"
        '
        'IndexToolStripMenuItem
        '
        Me.IndexToolStripMenuItem.Name = "IndexToolStripMenuItem"
        Me.IndexToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.IndexToolStripMenuItem.Text = "&Index"
        '
        'SearchToolStripMenuItem
        '
        Me.SearchToolStripMenuItem.Name = "SearchToolStripMenuItem"
        Me.SearchToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.SearchToolStripMenuItem.Text = "&Search"
        '
        'toolStripSeparator7
        '
        Me.toolStripSeparator7.Name = "toolStripSeparator7"
        Me.toolStripSeparator7.Size = New System.Drawing.Size(115, 6)
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.AboutToolStripMenuItem.Text = "&About..."
        '
        'StandardToolbar
        '
        Me.StandardToolbar.Dock = System.Windows.Forms.DockStyle.None
        Me.StandardToolbar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripButton, Me.OpenToolStripButton, Me.SaveToolStripButton, Me.PrintToolStripButton, Me.toolStripSeparator, Me.CutToolStripButton, Me.CopyToolStripButton, Me.PasteToolStripButton, Me.toolStripSeparator1, Me.HelpToolStripButton, Me.ToolStripSeparator8, Me.ZoomToolStripDropDownButton, Me.AddPageToolStripButton})
        Me.StandardToolbar.Location = New System.Drawing.Point(3, 24)
        Me.StandardToolbar.Name = "StandardToolbar"
        Me.StandardToolbar.Size = New System.Drawing.Size(264, 25)
        Me.StandardToolbar.TabIndex = 1
        Me.StandardToolbar.Text = "ToolStrip1"
        '
        'NewToolStripButton
        '
        Me.NewToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.NewToolStripButton.Image = CType(resources.GetObject("NewToolStripButton.Image"), System.Drawing.Image)
        Me.NewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.NewToolStripButton.Name = "NewToolStripButton"
        Me.NewToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.NewToolStripButton.Text = "&New"
        '
        'OpenToolStripButton
        '
        Me.OpenToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.OpenToolStripButton.Image = CType(resources.GetObject("OpenToolStripButton.Image"), System.Drawing.Image)
        Me.OpenToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.OpenToolStripButton.Name = "OpenToolStripButton"
        Me.OpenToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.OpenToolStripButton.Text = "&Open"
        '
        'SaveToolStripButton
        '
        Me.SaveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.SaveToolStripButton.Image = CType(resources.GetObject("SaveToolStripButton.Image"), System.Drawing.Image)
        Me.SaveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveToolStripButton.Name = "SaveToolStripButton"
        Me.SaveToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.SaveToolStripButton.Text = "&Save"
        '
        'PrintToolStripButton
        '
        Me.PrintToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.PrintToolStripButton.Image = CType(resources.GetObject("PrintToolStripButton.Image"), System.Drawing.Image)
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
        Me.CutToolStripButton.Image = CType(resources.GetObject("CutToolStripButton.Image"), System.Drawing.Image)
        Me.CutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CutToolStripButton.Name = "CutToolStripButton"
        Me.CutToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.CutToolStripButton.Text = "C&ut"
        '
        'CopyToolStripButton
        '
        Me.CopyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.CopyToolStripButton.Image = CType(resources.GetObject("CopyToolStripButton.Image"), System.Drawing.Image)
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
        Me.ZoomToolStripDropDownButton.Image = CType(resources.GetObject("ZoomToolStripDropDownButton.Image"), System.Drawing.Image)
        Me.ZoomToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ZoomToolStripDropDownButton.Name = "ZoomToolStripDropDownButton"
        Me.ZoomToolStripDropDownButton.Size = New System.Drawing.Size(29, 22)
        Me.ZoomToolStripDropDownButton.Text = "ZoomToolStripDropDownButton"
        Me.ZoomToolStripDropDownButton.ToolTipText = "Zoom"
        '
        'FiftyPercentMenuItem
        '
        Me.FiftyPercentMenuItem.Name = "FiftyPercentMenuItem"
        Me.FiftyPercentMenuItem.Size = New System.Drawing.Size(103, 22)
        Me.FiftyPercentMenuItem.Text = "50%"
        '
        'HunderedPercentMenuItem
        '
        Me.HunderedPercentMenuItem.Name = "HunderedPercentMenuItem"
        Me.HunderedPercentMenuItem.Size = New System.Drawing.Size(103, 22)
        Me.HunderedPercentMenuItem.Text = "100%"
        '
        'OneFiftyPercentMenuItem
        '
        Me.OneFiftyPercentMenuItem.Name = "OneFiftyPercentMenuItem"
        Me.OneFiftyPercentMenuItem.Size = New System.Drawing.Size(103, 22)
        Me.OneFiftyPercentMenuItem.Text = "150%"
        '
        'TwoHundredPercentMenuItem
        '
        Me.TwoHundredPercentMenuItem.Name = "TwoHundredPercentMenuItem"
        Me.TwoHundredPercentMenuItem.Size = New System.Drawing.Size(103, 22)
        Me.TwoHundredPercentMenuItem.Text = "200%"
        '
        'AddPageToolStripButton
        '
        Me.AddPageToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.AddPageToolStripButton.Image = CType(resources.GetObject("AddPageToolStripButton.Image"), System.Drawing.Image)
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
        Me.ToolBox.Location = New System.Drawing.Point(3, 49)
        Me.ToolBox.Name = "ToolBox"
        Me.ToolBox.Size = New System.Drawing.Size(88, 25)
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
        Me.ObjectToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.ObjectToolStripMenuItem.Text = "Object"
        Me.ObjectToolStripMenuItem.ToolTipText = "Add new Object"
        '
        'StateToolStripMenuItem
        '
        Me.StateToolStripMenuItem.Image = CType(resources.GetObject("StateToolStripMenuItem.Image"), System.Drawing.Image)
        Me.StateToolStripMenuItem.Name = "StateToolStripMenuItem"
        Me.StateToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.StateToolStripMenuItem.Text = "State"
        Me.StateToolStripMenuItem.ToolTipText = "Add new State"
        '
        'RoleToolStripMenuItem
        '
        Me.RoleToolStripMenuItem.Image = CType(resources.GetObject("RoleToolStripMenuItem.Image"), System.Drawing.Image)
        Me.RoleToolStripMenuItem.Name = "RoleToolStripMenuItem"
        Me.RoleToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.RoleToolStripMenuItem.Text = "Role"
        Me.RoleToolStripMenuItem.ToolTipText = "Add new Role"
        '
        'ActionToolStripMenuItem
        '
        Me.ActionToolStripMenuItem.Image = CType(resources.GetObject("ActionToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ActionToolStripMenuItem.Name = "ActionToolStripMenuItem"
        Me.ActionToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.ActionToolStripMenuItem.Text = "Action"
        Me.ActionToolStripMenuItem.ToolTipText = "Add new Action"
        '
        'AddInteractionToolStripButton
        '
        Me.AddInteractionToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.AddInteractionToolStripButton.Image = CType(resources.GetObject("AddInteractionToolStripButton.Image"), System.Drawing.Image)
        Me.AddInteractionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.AddInteractionToolStripButton.Name = "AddInteractionToolStripButton"
        Me.AddInteractionToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.AddInteractionToolStripButton.Text = "Add Interaction"
        Me.AddInteractionToolStripButton.ToolTipText = "Add Interaction"
        '
        'DeleteToolStripButton
        '
        Me.DeleteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.DeleteToolStripButton.Image = CType(resources.GetObject("DeleteToolStripButton.Image"), System.Drawing.Image)
        Me.DeleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DeleteToolStripButton.Name = "DeleteToolStripButton"
        Me.DeleteToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.DeleteToolStripButton.Text = "Delete"
        Me.DeleteToolStripButton.ToolTipText = "Remove"
        '
        'DiagramOpenFileDialog
        '
        Me.DiagramOpenFileDialog.DefaultExt = "diagram"
        Me.DiagramOpenFileDialog.DereferenceLinks = False
        Me.DiagramOpenFileDialog.Filter = "Diagram Files|*.diagram|Server Diagram Files|*.serverdiagram|All files|*.*"
        '
        'DiagramSaveFileDialog
        '
        Me.DiagramSaveFileDialog.DefaultExt = "diagram"
        Me.DiagramSaveFileDialog.Filter = "Diagram Files|*.diagram|All files|*.*"
        '
        'DiagramPrintPreviewDialog
        '
        Me.DiagramPrintPreviewDialog.AutoScrollMargin = New System.Drawing.Size(0, 0)
        Me.DiagramPrintPreviewDialog.AutoScrollMinSize = New System.Drawing.Size(0, 0)
        Me.DiagramPrintPreviewDialog.ClientSize = New System.Drawing.Size(400, 300)
        Me.DiagramPrintPreviewDialog.Document = Me.DiagramPrintDocument
        Me.DiagramPrintPreviewDialog.Enabled = True
        Me.DiagramPrintPreviewDialog.Icon = CType(resources.GetObject("DiagramPrintPreviewDialog.Icon"), System.Drawing.Icon)
        Me.DiagramPrintPreviewDialog.MainMenuStrip = Me.MainMenu
        Me.DiagramPrintPreviewDialog.Name = "DiagramPrintPreviewDialog"
        Me.DiagramPrintPreviewDialog.Visible = False
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
        Me.MainToolStripContainer.ContentPanel.Size = New System.Drawing.Size(550, 337)
        Me.MainToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainToolStripContainer.Location = New System.Drawing.Point(0, 0)
        Me.MainToolStripContainer.Name = "MainToolStripContainer"
        Me.MainToolStripContainer.Size = New System.Drawing.Size(550, 411)
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
        Me.DiagramViewer.Size = New System.Drawing.Size(550, 337)
        Me.DiagramViewer.TabIndex = 0
        '
        'DiagramDesigner
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(550, 411)
        Me.Controls.Add(Me.MainToolStripContainer)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.MainMenu
        Me.Name = "DiagramDesigner"
        Me.Text = "DigramDesigner"
        Me.MainMenu.ResumeLayout(False)
        Me.MainMenu.PerformLayout()
        Me.StandardToolbar.ResumeLayout(False)
        Me.StandardToolbar.PerformLayout()
        Me.ToolBox.ResumeLayout(False)
        Me.ToolBox.PerformLayout()
        Me.MainToolStripContainer.ContentPanel.ResumeLayout(False)
        Me.MainToolStripContainer.TopToolStripPanel.ResumeLayout(False)
        Me.MainToolStripContainer.TopToolStripPanel.PerformLayout()
        Me.MainToolStripContainer.ResumeLayout(False)
        Me.MainToolStripContainer.PerformLayout()
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
    Friend WithEvents DiagramPrintPreviewDialog As System.Windows.Forms.PrintPreviewDialog
    Friend WithEvents PageSetupToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DiagramPrintDocument As System.Drawing.Printing.PrintDocument
    Friend WithEvents DiagramPageSetupDialog As System.Windows.Forms.PageSetupDialog
    Friend WithEvents DiagramPrintDialog As System.Windows.Forms.PrintDialog
    Friend WithEvents MainToolStripContainer As System.Windows.Forms.ToolStripContainer
    Friend WithEvents DiagramViewer As Genetibase.NugenObjective.Windows.DiagramDesigner.DiagramPageViewer
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExplorerWindowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PropertiesWindowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
