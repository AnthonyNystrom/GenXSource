<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.pasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.copyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.cutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.customizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.optionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.selectAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.toolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.printPreviewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.printToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.exitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.redoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.undoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.editToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.commandSave = New Genetibase.[Shared].Controls.NuGenApplicationCommand(Me.components)
        Me._commandManager = New Genetibase.[Shared].Controls.NuGenCommandManager2Ex(Me.components)
        Me.commandFileCopy = New Genetibase.[Shared].Controls.NuGenApplicationCommand(Me.components)
        Me.copyToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.commandExit = New Genetibase.[Shared].Controls.NuGenApplicationCommand(Me.components)
        Me.commandTrackBar = New Genetibase.[Shared].Controls.NuGenApplicationCommand(Me.components)
        Me._trackBar = New Genetibase.SmoothControls.SmoothTrackBar
        Me.commandNew = New Genetibase.[Shared].Controls.NuGenApplicationCommand(Me.components)
        Me.newToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.newToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.commandContextCopy = New Genetibase.[Shared].Controls.NuGenApplicationCommand(Me.components)
        Me.copyToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me._contextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.contentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.helpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.indexToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.searchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.aboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.openToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.saveToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.printToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.cutToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.pasteToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.helpToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.toolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.fileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.openToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator = New System.Windows.Forms.ToolStripSeparator
        Me.saveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.saveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.toolStripContainer1 = New System.Windows.Forms.ToolStripContainer
        Me._enableLocalCopyCheckBox = New Genetibase.SmoothControls.NuGenSmoothCheckBox
        Me._enableGlobalCopyCheckBox = New Genetibase.SmoothControls.NuGenSmoothCheckBox
        Me._enableNewCheckBox = New Genetibase.SmoothControls.NuGenSmoothCheckBox
        Me._toolStrip = New System.Windows.Forms.ToolStrip
        Me.toolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator
        Me.toolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator
        Me._showSaveCheckBox = New Genetibase.SmoothControls.NuGenSmoothCheckBox
        Me._bkgndPanel = New Genetibase.SmoothControls.NuGenSmoothPanel
        Me._statusStrip = New System.Windows.Forms.StatusStrip
        Me._progressBar = New Genetibase.SmoothControls.SmoothProgressBar
        Me._menuStrip = New System.Windows.Forms.MenuStrip
        Me._toolstripManager = New Genetibase.SmoothControls.NuGenSmoothToolStripManager
        Me._contextMenuStrip.SuspendLayout()
        Me.toolStripContainer1.SuspendLayout()
        Me._toolStrip.SuspendLayout()
        Me._bkgndPanel.SuspendLayout()
        Me._statusStrip.SuspendLayout()
        Me._menuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'pasteToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.pasteToolStripMenuItem, Nothing)
        Me.pasteToolStripMenuItem.Image = CType(resources.GetObject("pasteToolStripMenuItem.Image"), System.Drawing.Image)
        Me.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem"
        Me.pasteToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.pasteToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.pasteToolStripMenuItem.Text = "&Paste"
        '
        'copyToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.copyToolStripMenuItem, Me.commandFileCopy)
        Me.copyToolStripMenuItem.Image = CType(resources.GetObject("copyToolStripMenuItem.Image"), System.Drawing.Image)
        Me.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.copyToolStripMenuItem.Name = "copyToolStripMenuItem"
        Me.copyToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.copyToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.copyToolStripMenuItem.Text = "&Copy"
        '
        'cutToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.cutToolStripMenuItem, Nothing)
        Me.cutToolStripMenuItem.Image = CType(resources.GetObject("cutToolStripMenuItem.Image"), System.Drawing.Image)
        Me.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cutToolStripMenuItem.Name = "cutToolStripMenuItem"
        Me.cutToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.cutToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.cutToolStripMenuItem.Text = "Cu&t"
        '
        'toolStripSeparator4
        '
        Me.toolStripSeparator4.Name = "toolStripSeparator4"
        Me.toolStripSeparator4.Size = New System.Drawing.Size(149, 6)
        '
        'customizeToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.customizeToolStripMenuItem, Nothing)
        Me.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem"
        Me.customizeToolStripMenuItem.Size = New System.Drawing.Size(134, 22)
        Me.customizeToolStripMenuItem.Text = "&Customize"
        '
        'toolsToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.toolsToolStripMenuItem, Nothing)
        Me.toolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.customizeToolStripMenuItem, Me.optionsToolStripMenuItem})
        Me.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem"
        Me.toolsToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.toolsToolStripMenuItem.Text = "&Tools"
        '
        'optionsToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.optionsToolStripMenuItem, Nothing)
        Me.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem"
        Me.optionsToolStripMenuItem.Size = New System.Drawing.Size(134, 22)
        Me.optionsToolStripMenuItem.Text = "&Options"
        '
        'selectAllToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.selectAllToolStripMenuItem, Nothing)
        Me.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem"
        Me.selectAllToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.selectAllToolStripMenuItem.Text = "Select &All"
        '
        'toolStripSeparator3
        '
        Me.toolStripSeparator3.Name = "toolStripSeparator3"
        Me.toolStripSeparator3.Size = New System.Drawing.Size(149, 6)
        '
        'toolStripSeparator2
        '
        Me.toolStripSeparator2.Name = "toolStripSeparator2"
        Me.toolStripSeparator2.Size = New System.Drawing.Size(148, 6)
        '
        'printPreviewToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.printPreviewToolStripMenuItem, Nothing)
        Me.printPreviewToolStripMenuItem.Image = CType(resources.GetObject("printPreviewToolStripMenuItem.Image"), System.Drawing.Image)
        Me.printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem"
        Me.printPreviewToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
        Me.printPreviewToolStripMenuItem.Text = "Print Pre&view"
        '
        'printToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.printToolStripMenuItem, Nothing)
        Me.printToolStripMenuItem.Image = CType(resources.GetObject("printToolStripMenuItem.Image"), System.Drawing.Image)
        Me.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.printToolStripMenuItem.Name = "printToolStripMenuItem"
        Me.printToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.printToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
        Me.printToolStripMenuItem.Text = "&Print"
        '
        'exitToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.exitToolStripMenuItem, Me.commandExit)
        Me.exitToolStripMenuItem.Name = "exitToolStripMenuItem"
        Me.exitToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
        Me.exitToolStripMenuItem.Text = "E&xit"
        '
        'redoToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.redoToolStripMenuItem, Nothing)
        Me.redoToolStripMenuItem.Name = "redoToolStripMenuItem"
        Me.redoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Y), System.Windows.Forms.Keys)
        Me.redoToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.redoToolStripMenuItem.Text = "&Redo"
        '
        'undoToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.undoToolStripMenuItem, Nothing)
        Me.undoToolStripMenuItem.Name = "undoToolStripMenuItem"
        Me.undoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
        Me.undoToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.undoToolStripMenuItem.Text = "&Undo"
        '
        'editToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.editToolStripMenuItem, Nothing)
        Me.editToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.undoToolStripMenuItem, Me.redoToolStripMenuItem, Me.toolStripSeparator3, Me.cutToolStripMenuItem, Me.copyToolStripMenuItem, Me.pasteToolStripMenuItem, Me.toolStripSeparator4, Me.selectAllToolStripMenuItem})
        Me.editToolStripMenuItem.Name = "editToolStripMenuItem"
        Me.editToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.editToolStripMenuItem.Text = "&Edit"
        '
        'commandSave
        '
        Me.commandSave.ApplicationCommandName = "Save"
        Me.commandSave.CommandManager = Me._commandManager
        Me.commandSave.Description = Nothing
        Me.commandSave.UIItems.Add(Me.saveToolStripButton)
        Me.commandSave.UIItems.Add(Me.saveToolStripMenuItem)
        '
        '_commandManager
        '
        Me._commandManager.ApplicationCommands.AddRange(New Genetibase.[Shared].Controls.NuGenApplicationCommand() {Me.commandExit, Me.commandFileCopy, Me.commandTrackBar, Me.commandNew, Me.commandContextCopy, Me.commandSave})
        Me._commandManager.ContextMenus.Add(Me._contextMenuStrip)
        '
        'commandFileCopy
        '
        Me.commandFileCopy.ApplicationCommandName = "FileCopy"
        Me.commandFileCopy.CommandManager = Me._commandManager
        Me.commandFileCopy.Description = Nothing
        Me.commandFileCopy.UIItems.Add(Me.copyToolStripMenuItem)
        Me.commandFileCopy.UIItems.Add(Me.copyToolStripButton)
        '
        'copyToolStripButton
        '
        Me._commandManager.SetApplicationCommand(Me.copyToolStripButton, Me.commandFileCopy)
        Me.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.copyToolStripButton.Image = CType(resources.GetObject("copyToolStripButton.Image"), System.Drawing.Image)
        Me.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.copyToolStripButton.Name = "copyToolStripButton"
        Me.copyToolStripButton.Size = New System.Drawing.Size(23, 30)
        Me.copyToolStripButton.Text = "&Copy"
        '
        'commandExit
        '
        Me.commandExit.ApplicationCommandName = "Exit"
        Me.commandExit.CommandManager = Me._commandManager
        Me.commandExit.Description = Nothing
        Me.commandExit.UIItems.Add(Me.exitToolStripMenuItem)
        '
        'commandTrackBar
        '
        Me.commandTrackBar.ApplicationCommandName = "TrackBar"
        Me.commandTrackBar.CommandManager = Me._commandManager
        Me.commandTrackBar.Description = Nothing
        Me.commandTrackBar.UIItems.Add(Me._trackBar)
        '
        '_trackBar
        '
        Me._commandManager.SetApplicationCommand(Me._trackBar, Me.commandTrackBar)
        Me._trackBar.LargeChange = 5
        Me._trackBar.Maximum = 100
        Me._trackBar.Minimum = 0
        Me._trackBar.Name = "_trackBar"
        Me._trackBar.Size = New System.Drawing.Size(100, 30)
        Me._trackBar.SmallChange = 10
        Me._trackBar.TickStyle = System.Windows.Forms.TickStyle.BottomRight
        Me._trackBar.Value = 0
        '
        'commandNew
        '
        Me.commandNew.ApplicationCommandName = "New"
        Me.commandNew.CommandManager = Me._commandManager
        Me.commandNew.Description = Nothing
        Me.commandNew.UIItems.Add(Me.newToolStripMenuItem)
        Me.commandNew.UIItems.Add(Me.newToolStripButton)
        '
        'newToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.newToolStripMenuItem, Me.commandNew)
        Me.newToolStripMenuItem.Image = CType(resources.GetObject("newToolStripMenuItem.Image"), System.Drawing.Image)
        Me.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.newToolStripMenuItem.Name = "newToolStripMenuItem"
        Me.newToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.newToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
        Me.newToolStripMenuItem.Text = "&New"
        '
        'newToolStripButton
        '
        Me._commandManager.SetApplicationCommand(Me.newToolStripButton, Me.commandNew)
        Me.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.newToolStripButton.Image = CType(resources.GetObject("newToolStripButton.Image"), System.Drawing.Image)
        Me.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.newToolStripButton.Name = "newToolStripButton"
        Me.newToolStripButton.Size = New System.Drawing.Size(23, 30)
        Me.newToolStripButton.Text = "&New"
        '
        'commandContextCopy
        '
        Me.commandContextCopy.ApplicationCommandName = "ContextCopy"
        Me.commandContextCopy.CommandManager = Me._commandManager
        Me.commandContextCopy.Description = Nothing
        Me.commandContextCopy.UIItems.Add(Me.copyToolStripMenuItem1)
        '
        'copyToolStripMenuItem1
        '
        Me._commandManager.SetApplicationCommand(Me.copyToolStripMenuItem1, Me.commandContextCopy)
        Me.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1"
        Me.copyToolStripMenuItem1.Size = New System.Drawing.Size(110, 22)
        Me.copyToolStripMenuItem1.Text = "Copy"
        '
        '_contextMenuStrip
        '
        Me._contextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.copyToolStripMenuItem1})
        Me._contextMenuStrip.Name = "_contextMenuStrip"
        Me._contextMenuStrip.Size = New System.Drawing.Size(111, 26)
        '
        'contentsToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.contentsToolStripMenuItem, Nothing)
        Me.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem"
        Me.contentsToolStripMenuItem.Size = New System.Drawing.Size(129, 22)
        Me.contentsToolStripMenuItem.Text = "&Contents"
        '
        'helpToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.helpToolStripMenuItem, Nothing)
        Me.helpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.contentsToolStripMenuItem, Me.indexToolStripMenuItem, Me.searchToolStripMenuItem, Me.toolStripSeparator5, Me.aboutToolStripMenuItem})
        Me.helpToolStripMenuItem.Name = "helpToolStripMenuItem"
        Me.helpToolStripMenuItem.Size = New System.Drawing.Size(40, 20)
        Me.helpToolStripMenuItem.Text = "&Help"
        '
        'indexToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.indexToolStripMenuItem, Nothing)
        Me.indexToolStripMenuItem.Name = "indexToolStripMenuItem"
        Me.indexToolStripMenuItem.Size = New System.Drawing.Size(129, 22)
        Me.indexToolStripMenuItem.Text = "&Index"
        '
        'searchToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.searchToolStripMenuItem, Nothing)
        Me.searchToolStripMenuItem.Name = "searchToolStripMenuItem"
        Me.searchToolStripMenuItem.Size = New System.Drawing.Size(129, 22)
        Me.searchToolStripMenuItem.Text = "&Search"
        '
        'toolStripSeparator5
        '
        Me.toolStripSeparator5.Name = "toolStripSeparator5"
        Me.toolStripSeparator5.Size = New System.Drawing.Size(126, 6)
        '
        'aboutToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.aboutToolStripMenuItem, Nothing)
        Me.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem"
        Me.aboutToolStripMenuItem.Size = New System.Drawing.Size(129, 22)
        Me.aboutToolStripMenuItem.Text = "&About..."
        '
        'openToolStripButton
        '
        Me._commandManager.SetApplicationCommand(Me.openToolStripButton, Nothing)
        Me.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.openToolStripButton.Image = CType(resources.GetObject("openToolStripButton.Image"), System.Drawing.Image)
        Me.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.openToolStripButton.Name = "openToolStripButton"
        Me.openToolStripButton.Size = New System.Drawing.Size(23, 30)
        Me.openToolStripButton.Text = "&Open"
        '
        'saveToolStripButton
        '
        Me._commandManager.SetApplicationCommand(Me.saveToolStripButton, Me.commandSave)
        Me.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.saveToolStripButton.Image = CType(resources.GetObject("saveToolStripButton.Image"), System.Drawing.Image)
        Me.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.saveToolStripButton.Name = "saveToolStripButton"
        Me.saveToolStripButton.Size = New System.Drawing.Size(23, 30)
        Me.saveToolStripButton.Text = "&Save"
        '
        'printToolStripButton
        '
        Me._commandManager.SetApplicationCommand(Me.printToolStripButton, Nothing)
        Me.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.printToolStripButton.Image = CType(resources.GetObject("printToolStripButton.Image"), System.Drawing.Image)
        Me.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.printToolStripButton.Name = "printToolStripButton"
        Me.printToolStripButton.Size = New System.Drawing.Size(23, 30)
        Me.printToolStripButton.Text = "&Print"
        '
        'cutToolStripButton
        '
        Me._commandManager.SetApplicationCommand(Me.cutToolStripButton, Nothing)
        Me.cutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cutToolStripButton.Image = CType(resources.GetObject("cutToolStripButton.Image"), System.Drawing.Image)
        Me.cutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cutToolStripButton.Name = "cutToolStripButton"
        Me.cutToolStripButton.Size = New System.Drawing.Size(23, 30)
        Me.cutToolStripButton.Text = "C&ut"
        '
        'pasteToolStripButton
        '
        Me._commandManager.SetApplicationCommand(Me.pasteToolStripButton, Nothing)
        Me.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.pasteToolStripButton.Image = CType(resources.GetObject("pasteToolStripButton.Image"), System.Drawing.Image)
        Me.pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.pasteToolStripButton.Name = "pasteToolStripButton"
        Me.pasteToolStripButton.Size = New System.Drawing.Size(23, 30)
        Me.pasteToolStripButton.Text = "&Paste"
        '
        'helpToolStripButton
        '
        Me._commandManager.SetApplicationCommand(Me.helpToolStripButton, Nothing)
        Me.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.helpToolStripButton.Image = CType(resources.GetObject("helpToolStripButton.Image"), System.Drawing.Image)
        Me.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.helpToolStripButton.Name = "helpToolStripButton"
        Me.helpToolStripButton.Size = New System.Drawing.Size(23, 30)
        Me.helpToolStripButton.Text = "He&lp"
        '
        'toolStripStatusLabel1
        '
        Me._commandManager.SetApplicationCommand(Me.toolStripStatusLabel1, Nothing)
        Me.toolStripStatusLabel1.Name = "toolStripStatusLabel1"
        Me.toolStripStatusLabel1.Size = New System.Drawing.Size(49, 17)
        Me.toolStripStatusLabel1.Text = "Progress"
        '
        'fileToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.fileToolStripMenuItem, Nothing)
        Me.fileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.newToolStripMenuItem, Me.openToolStripMenuItem, Me.toolStripSeparator, Me.saveToolStripMenuItem, Me.saveAsToolStripMenuItem, Me.toolStripSeparator1, Me.printToolStripMenuItem, Me.printPreviewToolStripMenuItem, Me.toolStripSeparator2, Me.exitToolStripMenuItem})
        Me.fileToolStripMenuItem.Name = "fileToolStripMenuItem"
        Me.fileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.fileToolStripMenuItem.Text = "&File"
        '
        'openToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.openToolStripMenuItem, Nothing)
        Me.openToolStripMenuItem.Image = CType(resources.GetObject("openToolStripMenuItem.Image"), System.Drawing.Image)
        Me.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.openToolStripMenuItem.Name = "openToolStripMenuItem"
        Me.openToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.openToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
        Me.openToolStripMenuItem.Text = "&Open"
        '
        'toolStripSeparator
        '
        Me.toolStripSeparator.Name = "toolStripSeparator"
        Me.toolStripSeparator.Size = New System.Drawing.Size(148, 6)
        '
        'saveToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.saveToolStripMenuItem, Me.commandSave)
        Me.saveToolStripMenuItem.Image = CType(resources.GetObject("saveToolStripMenuItem.Image"), System.Drawing.Image)
        Me.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.saveToolStripMenuItem.Name = "saveToolStripMenuItem"
        Me.saveToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.saveToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
        Me.saveToolStripMenuItem.Text = "&Save"
        '
        'saveAsToolStripMenuItem
        '
        Me._commandManager.SetApplicationCommand(Me.saveAsToolStripMenuItem, Nothing)
        Me.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem"
        Me.saveAsToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
        Me.saveAsToolStripMenuItem.Text = "Save &As"
        '
        'toolStripSeparator1
        '
        Me.toolStripSeparator1.Name = "toolStripSeparator1"
        Me.toolStripSeparator1.Size = New System.Drawing.Size(148, 6)
        '
        'toolStripContainer1
        '
        '
        'toolStripContainer1.ContentPanel
        '
        Me.toolStripContainer1.ContentPanel.Size = New System.Drawing.Size(322, 216)
        Me.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.toolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.toolStripContainer1.Name = "toolStripContainer1"
        Me.toolStripContainer1.Size = New System.Drawing.Size(322, 266)
        Me.toolStripContainer1.TabIndex = 2
        Me.toolStripContainer1.Text = "toolStripContainer1"
        '
        '_enableLocalCopyCheckBox
        '
        Me._enableLocalCopyCheckBox.AutoSize = False
        Me._enableLocalCopyCheckBox.Location = New System.Drawing.Point(12, 113)
        Me._enableLocalCopyCheckBox.Name = "_enableLocalCopyCheckBox"
        Me._enableLocalCopyCheckBox.Size = New System.Drawing.Size(268, 24)
        Me._enableLocalCopyCheckBox.TabIndex = 3
        Me._enableLocalCopyCheckBox.Text = "Enable Local Copy"
        Me._enableLocalCopyCheckBox.UseVisualStyleBackColor = False
        '
        '_enableGlobalCopyCheckBox
        '
        Me._enableGlobalCopyCheckBox.AutoSize = False
        Me._enableGlobalCopyCheckBox.Location = New System.Drawing.Point(12, 90)
        Me._enableGlobalCopyCheckBox.Name = "_enableGlobalCopyCheckBox"
        Me._enableGlobalCopyCheckBox.Size = New System.Drawing.Size(268, 24)
        Me._enableGlobalCopyCheckBox.TabIndex = 3
        Me._enableGlobalCopyCheckBox.Text = "Enable Global Copy"
        Me._enableGlobalCopyCheckBox.UseVisualStyleBackColor = False
        '
        '_enableNewCheckBox
        '
        Me._enableNewCheckBox.AutoSize = False
        Me._enableNewCheckBox.Location = New System.Drawing.Point(12, 67)
        Me._enableNewCheckBox.Name = "_enableNewCheckBox"
        Me._enableNewCheckBox.Size = New System.Drawing.Size(268, 24)
        Me._enableNewCheckBox.TabIndex = 2
        Me._enableNewCheckBox.Text = "Enable New"
        Me._enableNewCheckBox.UseVisualStyleBackColor = False
        '
        '_toolStrip
        '
        Me._toolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.newToolStripButton, Me.openToolStripButton, Me.saveToolStripButton, Me.printToolStripButton, Me.toolStripSeparator6, Me.cutToolStripButton, Me.copyToolStripButton, Me.pasteToolStripButton, Me.toolStripSeparator7, Me.helpToolStripButton, Me._trackBar})
        Me._toolStrip.Location = New System.Drawing.Point(0, 24)
        Me._toolStrip.Name = "_toolStrip"
        Me._toolStrip.Size = New System.Drawing.Size(322, 33)
        Me._toolStrip.TabIndex = 0
        Me._toolStrip.Text = "toolStrip1"
        '
        'toolStripSeparator6
        '
        Me.toolStripSeparator6.Name = "toolStripSeparator6"
        Me.toolStripSeparator6.Size = New System.Drawing.Size(6, 33)
        '
        'toolStripSeparator7
        '
        Me.toolStripSeparator7.Name = "toolStripSeparator7"
        Me.toolStripSeparator7.Size = New System.Drawing.Size(6, 33)
        '
        '_showSaveCheckBox
        '
        Me._showSaveCheckBox.AutoSize = False
        Me._showSaveCheckBox.Location = New System.Drawing.Point(12, 136)
        Me._showSaveCheckBox.Name = "_showSaveCheckBox"
        Me._showSaveCheckBox.Size = New System.Drawing.Size(268, 24)
        Me._showSaveCheckBox.TabIndex = 3
        Me._showSaveCheckBox.Text = "Show Save"
        Me._showSaveCheckBox.UseVisualStyleBackColor = False
        '
        '_bkgndPanel
        '
        Me._bkgndPanel.ContextMenuStrip = Me._contextMenuStrip
        Me._bkgndPanel.Controls.Add(Me._statusStrip)
        Me._bkgndPanel.Controls.Add(Me._showSaveCheckBox)
        Me._bkgndPanel.Controls.Add(Me._enableLocalCopyCheckBox)
        Me._bkgndPanel.Controls.Add(Me._enableGlobalCopyCheckBox)
        Me._bkgndPanel.Controls.Add(Me._enableNewCheckBox)
        Me._bkgndPanel.Controls.Add(Me._toolStrip)
        Me._bkgndPanel.Controls.Add(Me._menuStrip)
        Me._bkgndPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._bkgndPanel.DrawBorder = False
        Me._bkgndPanel.Location = New System.Drawing.Point(0, 0)
        Me._bkgndPanel.Name = "_bkgndPanel"
        Me._bkgndPanel.Size = New System.Drawing.Size(322, 266)
        '
        '_statusStrip
        '
        Me._statusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.toolStripStatusLabel1, Me._progressBar})
        Me._statusStrip.Location = New System.Drawing.Point(0, 244)
        Me._statusStrip.Name = "_statusStrip"
        Me._statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode
        Me._statusStrip.Size = New System.Drawing.Size(322, 22)
        Me._statusStrip.TabIndex = 4
        Me._statusStrip.Text = "statusStrip1"
        '
        '_progressBar
        '
        Me._progressBar.MarqueeAnimationSpeed = 100
        Me._progressBar.Maximum = 100
        Me._progressBar.Minimum = 0
        Me._progressBar.Name = "_progressBar"
        Me._progressBar.Padding = New System.Windows.Forms.Padding(1)
        Me._progressBar.Size = New System.Drawing.Size(100, 20)
        Me._progressBar.Step = 10
        Me._progressBar.Style = Genetibase.[Shared].Controls.NuGenProgressBarStyle.Continuous
        Me._progressBar.Value = 0
        '
        '_menuStrip
        '
        Me._menuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.fileToolStripMenuItem, Me.editToolStripMenuItem, Me.toolsToolStripMenuItem, Me.helpToolStripMenuItem})
        Me._menuStrip.Location = New System.Drawing.Point(0, 0)
        Me._menuStrip.Name = "_menuStrip"
        Me._menuStrip.Size = New System.Drawing.Size(322, 24)
        Me._menuStrip.TabIndex = 1
        Me._menuStrip.Text = "menuStrip1"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(322, 266)
        Me.Controls.Add(Me._bkgndPanel)
        Me.Controls.Add(Me.toolStripContainer1)
        Me.Name = "MainForm"
        Me.Text = "MainForm"
        Me._contextMenuStrip.ResumeLayout(False)
        Me.toolStripContainer1.ResumeLayout(False)
        Me.toolStripContainer1.PerformLayout()
        Me._toolStrip.ResumeLayout(False)
        Me._toolStrip.PerformLayout()
        Me._bkgndPanel.ResumeLayout(False)
        Me._bkgndPanel.PerformLayout()
        Me._statusStrip.ResumeLayout(False)
        Me._statusStrip.PerformLayout()
        Me._menuStrip.ResumeLayout(False)
        Me._menuStrip.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents pasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _commandManager As Genetibase.Shared.Controls.NuGenCommandManager2Ex
    Private WithEvents commandExit As Genetibase.Shared.Controls.NuGenApplicationCommand
    Private WithEvents exitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents commandFileCopy As Genetibase.Shared.Controls.NuGenApplicationCommand
    Private WithEvents copyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents copyToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents commandTrackBar As Genetibase.Shared.Controls.NuGenApplicationCommand
    Private WithEvents _trackBar As Genetibase.SmoothControls.SmoothTrackBar
    Private WithEvents commandNew As Genetibase.Shared.Controls.NuGenApplicationCommand
    Private WithEvents newToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents newToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents commandContextCopy As Genetibase.Shared.Controls.NuGenApplicationCommand
    Private WithEvents copyToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents commandSave As Genetibase.Shared.Controls.NuGenApplicationCommand
    Private WithEvents saveToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents saveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _contextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Private WithEvents cutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents toolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents customizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents toolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents optionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents selectAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents toolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents toolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents printPreviewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents printToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents redoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents undoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents editToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents contentsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents helpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents indexToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents searchToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents toolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents aboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents openToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents printToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents cutToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents pasteToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents helpToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents toolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents fileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents openToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents toolStripSeparator As System.Windows.Forms.ToolStripSeparator
    Private WithEvents saveAsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents toolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Private WithEvents _enableLocalCopyCheckBox As Genetibase.SmoothControls.NuGenSmoothCheckBox
    Private WithEvents _enableGlobalCopyCheckBox As Genetibase.SmoothControls.NuGenSmoothCheckBox
    Private WithEvents _enableNewCheckBox As Genetibase.SmoothControls.NuGenSmoothCheckBox
    Private WithEvents _toolStrip As System.Windows.Forms.ToolStrip
    Private WithEvents toolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents toolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _showSaveCheckBox As Genetibase.SmoothControls.NuGenSmoothCheckBox
    Private WithEvents _bkgndPanel As Genetibase.SmoothControls.NuGenSmoothPanel
    Private WithEvents _statusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents _progressBar As Genetibase.SmoothControls.SmoothProgressBar
    Private WithEvents _menuStrip As System.Windows.Forms.MenuStrip
    Private WithEvents _toolstripManager As Genetibase.SmoothControls.NuGenSmoothToolStripManager

End Class
