Imports Agilix.Ink
Imports Agilix.Ink.Scribble
Imports System.Drawing.Printing
Imports System.Drawing.Drawing2D

Public Class Form1
    Inherits System.Windows.Forms.Form
    Private Const TITLE As String = "Agilix Scribbler"

#Region " Fields "
    Private fFileName As String
    Private fFilePath As String
    Private fFileNumber As Integer = 0
    Private fPrintPage As Integer
    Private fPrintRectangle As Integer
    Private fPrintPhysicalRectangles As New ArrayList
#End Region

#Region " Main "
    <System.STAThread()> _
    Public Shared Sub Main(ByVal args() As String)

        ' Use the XP style
        System.Windows.Forms.Application.EnableVisualStyles()
        Application.DoEvents()

        System.Windows.Forms.Application.Run(New Form1)
    End Sub
#End Region

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'Add any initialization after the InitializeComponent() call
        InitializeComponent()
        Me.Icon = New Icon("..\Icon1.ico")

        NewFileName()
        Dim document As New Document
        Dim page As Page
        page = New Page(document, Stationery.CreateStockStationeryWithTitle(StationeryStockType.Standard, 19050))

        document.Pages.Add(page)
        fScribble.Scribble.Document = document
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents fScribble As Agilix.Ink.Scribble.ScribbleBox
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents FileMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents NewMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents OpenMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ImportMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents SaveMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents SaveAsMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem8 As System.Windows.Forms.MenuItem
    Friend WithEvents PrintMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ExitMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem11 As System.Windows.Forms.MenuItem
    Friend WithEvents UndoMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents RedoMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem14 As System.Windows.Forms.MenuItem
    Friend WithEvents CutMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents CopyMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents PasteMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents DeleteMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents CopyAsTextMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem20 As System.Windows.Forms.MenuItem
    Friend WithEvents FindMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents FindNextMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem23 As System.Windows.Forms.MenuItem
    Friend WithEvents DefineMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents BringToFrontMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents SendToBackMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents GroupMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents UnGroupMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem29 As System.Windows.Forms.MenuItem
    Friend WithEvents ConvertToTextMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents FormatMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem32 As System.Windows.Forms.MenuItem
    Friend WithEvents RectangleMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ElipseMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents TriangleMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents YieldSignMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents DiamondMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents StarMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem39 As System.Windows.Forms.MenuItem
    Friend WithEvents LineMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ArrowMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem42 As System.Windows.Forms.MenuItem
    Friend WithEvents PictureMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem44 As System.Windows.Forms.MenuItem
    Friend WithEvents BlankMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents NarrowCollegeMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents StandardMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents WideMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents SmallGridMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents GridMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem51 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem52 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem53 As System.Windows.Forms.MenuItem
    Friend WithEvents AboutMenuItem As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.fScribble = New Agilix.Ink.Scribble.ScribbleBox
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.FileMenuItem = New System.Windows.Forms.MenuItem
        Me.NewMenuItem = New System.Windows.Forms.MenuItem
        Me.OpenMenuItem = New System.Windows.Forms.MenuItem
        Me.ImportMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.SaveMenuItem = New System.Windows.Forms.MenuItem
        Me.SaveAsMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem8 = New System.Windows.Forms.MenuItem
        Me.PrintMenuItem = New System.Windows.Forms.MenuItem
        Me.ExitMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem11 = New System.Windows.Forms.MenuItem
        Me.UndoMenuItem = New System.Windows.Forms.MenuItem
        Me.RedoMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem14 = New System.Windows.Forms.MenuItem
        Me.CutMenuItem = New System.Windows.Forms.MenuItem
        Me.CopyMenuItem = New System.Windows.Forms.MenuItem
        Me.CopyAsTextMenuItem = New System.Windows.Forms.MenuItem
        Me.PasteMenuItem = New System.Windows.Forms.MenuItem
        Me.DeleteMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem20 = New System.Windows.Forms.MenuItem
        Me.FindMenuItem = New System.Windows.Forms.MenuItem
        Me.FindNextMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem23 = New System.Windows.Forms.MenuItem
        Me.DefineMenuItem = New System.Windows.Forms.MenuItem
        Me.BringToFrontMenuItem = New System.Windows.Forms.MenuItem
        Me.SendToBackMenuItem = New System.Windows.Forms.MenuItem
        Me.GroupMenuItem = New System.Windows.Forms.MenuItem
        Me.UnGroupMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem29 = New System.Windows.Forms.MenuItem
        Me.ConvertToTextMenuItem = New System.Windows.Forms.MenuItem
        Me.FormatMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem32 = New System.Windows.Forms.MenuItem
        Me.RectangleMenuItem = New System.Windows.Forms.MenuItem
        Me.ElipseMenuItem = New System.Windows.Forms.MenuItem
        Me.TriangleMenuItem = New System.Windows.Forms.MenuItem
        Me.YieldSignMenuItem = New System.Windows.Forms.MenuItem
        Me.DiamondMenuItem = New System.Windows.Forms.MenuItem
        Me.StarMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem39 = New System.Windows.Forms.MenuItem
        Me.LineMenuItem = New System.Windows.Forms.MenuItem
        Me.ArrowMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem42 = New System.Windows.Forms.MenuItem
        Me.PictureMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem44 = New System.Windows.Forms.MenuItem
        Me.BlankMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem51 = New System.Windows.Forms.MenuItem
        Me.NarrowCollegeMenuItem = New System.Windows.Forms.MenuItem
        Me.StandardMenuItem = New System.Windows.Forms.MenuItem
        Me.WideMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem52 = New System.Windows.Forms.MenuItem
        Me.SmallGridMenuItem = New System.Windows.Forms.MenuItem
        Me.GridMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem53 = New System.Windows.Forms.MenuItem
        Me.AboutMenuItem = New System.Windows.Forms.MenuItem
        Me.SuspendLayout()
        '
        'fScribble
        '
        Me.fScribble.BackColor = System.Drawing.SystemColors.Control
        Me.fScribble.Dock = System.Windows.Forms.DockStyle.Fill
        Me.fScribble.HighlightElementColor = System.Drawing.Color.FromArgb(CType(189, Byte), CType(208, Byte), CType(220, Byte))
        Me.fScribble.Location = New System.Drawing.Point(0, 0)
        Me.fScribble.Name = "fScribble"
        Me.fScribble.Size = New System.Drawing.Size(752, 518)
        Me.fScribble.TabColor = System.Drawing.SystemColors.ControlDark
        Me.fScribble.TabIndex = 0

        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.FileMenuItem, Me.MenuItem11, Me.MenuItem32, Me.MenuItem44, Me.MenuItem53})
        '
        'FileMenuItem
        '
        Me.FileMenuItem.Index = 0
        Me.FileMenuItem.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.NewMenuItem, Me.OpenMenuItem, Me.ImportMenuItem, Me.MenuItem5, Me.SaveMenuItem, Me.SaveAsMenuItem, Me.MenuItem8, Me.PrintMenuItem, Me.ExitMenuItem})
        Me.FileMenuItem.Text = "&File"
        '
        'NewMenuItem
        '
        Me.NewMenuItem.Index = 0
        Me.NewMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlN
        Me.NewMenuItem.Text = "&New"
        '
        'OpenMenuItem
        '
        Me.OpenMenuItem.Index = 1
        Me.OpenMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlO
        Me.OpenMenuItem.Text = "&Open..."
        '
        'ImportMenuItem
        '
        Me.ImportMenuItem.Index = 2
        Me.ImportMenuItem.Text = "&Import..."
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 3
        Me.MenuItem5.Text = "-"
        '
        'SaveMenuItem
        '
        Me.SaveMenuItem.Index = 4
        Me.SaveMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlS
        Me.SaveMenuItem.Text = "&Save"
        '
        'SaveAsMenuItem
        '
        Me.SaveAsMenuItem.Index = 5
        Me.SaveAsMenuItem.Text = "Save &As..."
        '
        'MenuItem8
        '
        Me.MenuItem8.Index = 6
        Me.MenuItem8.Text = "-"
        '
        'PrintMenuItem
        '
        Me.PrintMenuItem.Index = 7
        Me.PrintMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlP
        Me.PrintMenuItem.Text = "&Print..."
        '
        'ExitMenuItem
        '
        Me.ExitMenuItem.Index = 8
        Me.ExitMenuItem.Text = "E&xit"
        '
        'MenuItem11
        '
        Me.MenuItem11.Index = 1
        Me.MenuItem11.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.UndoMenuItem, Me.RedoMenuItem, Me.MenuItem14, Me.CutMenuItem, Me.CopyMenuItem, Me.CopyAsTextMenuItem, Me.PasteMenuItem, Me.DeleteMenuItem, Me.MenuItem20, Me.FindMenuItem, Me.FindNextMenuItem, Me.MenuItem23, Me.DefineMenuItem, Me.BringToFrontMenuItem, Me.SendToBackMenuItem, Me.GroupMenuItem, Me.UnGroupMenuItem, Me.MenuItem29, Me.ConvertToTextMenuItem, Me.FormatMenuItem})
        Me.MenuItem11.Text = "&Edit"
        '
        'UndoMenuItem
        '
        Me.UndoMenuItem.Index = 0
        Me.UndoMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlZ
        Me.UndoMenuItem.Text = "&Undo"
        '
        'RedoMenuItem
        '
        Me.RedoMenuItem.Index = 1
        Me.RedoMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlY
        Me.RedoMenuItem.Text = "&Redo"
        '
        'MenuItem14
        '
        Me.MenuItem14.Index = 2
        Me.MenuItem14.Text = "-"
        '
        'CutMenuItem
        '
        Me.CutMenuItem.Index = 3
        Me.CutMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftX
        Me.CutMenuItem.Text = "Cu&t"
        '
        'CopyMenuItem
        '
        Me.CopyMenuItem.Index = 4
        Me.CopyMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlC
        Me.CopyMenuItem.Text = "&Copy"
        '
        'CopyAsTextMenuItem
        '
        Me.CopyAsTextMenuItem.Index = 5
        Me.CopyAsTextMenuItem.Text = "Cop&y as Text..."
        '
        'PasteMenuItem
        '
        Me.PasteMenuItem.Index = 6
        Me.PasteMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlV
        Me.PasteMenuItem.Text = "&Paste"
        '
        'DeleteMenuItem
        '
        Me.DeleteMenuItem.Index = 7
        Me.DeleteMenuItem.Shortcut = System.Windows.Forms.Shortcut.Del
        Me.DeleteMenuItem.Text = "&Delete"
        '
        'MenuItem20
        '
        Me.MenuItem20.Index = 8
        Me.MenuItem20.Text = "-"
        '
        'FindMenuItem
        '
        Me.FindMenuItem.Index = 9
        Me.FindMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlF
        Me.FindMenuItem.Text = "&Find..."
        '
        'FindNextMenuItem
        '
        Me.FindNextMenuItem.Index = 10
        Me.FindNextMenuItem.Shortcut = System.Windows.Forms.Shortcut.F3
        Me.FindNextMenuItem.Text = "Find &Next"
        '
        'MenuItem23
        '
        Me.MenuItem23.Index = 11
        Me.MenuItem23.Text = "-"
        '
        'DefineMenuItem
        '
        Me.DefineMenuItem.Index = 12
        Me.DefineMenuItem.Text = "Define Fla&gs..."
        '
        'BringToFrontMenuItem
        '
        Me.BringToFrontMenuItem.Index = 13
        Me.BringToFrontMenuItem.Text = "&Bring to Front"
        '
        'SendToBackMenuItem
        '
        Me.SendToBackMenuItem.Index = 14
        Me.SendToBackMenuItem.Text = "Send to Bac&k"
        '
        'GroupMenuItem
        '
        Me.GroupMenuItem.Index = 15
        Me.GroupMenuItem.Text = "&Group"
        '
        'UnGroupMenuItem
        '
        Me.UnGroupMenuItem.Index = 16
        Me.UnGroupMenuItem.Text = "Ungr&oup"
        '
        'MenuItem29
        '
        Me.MenuItem29.Index = 17
        Me.MenuItem29.Text = "-"
        '
        'ConvertToTextMenuItem
        '
        Me.ConvertToTextMenuItem.Index = 18
        Me.ConvertToTextMenuItem.Text = "Con&vert to Text"
        '
        'FormatMenuItem
        '
        Me.FormatMenuItem.Index = 19
        Me.FormatMenuItem.Text = "For&mat..."
        '
        'MenuItem32
        '
        Me.MenuItem32.Index = 2
        Me.MenuItem32.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.RectangleMenuItem, Me.ElipseMenuItem, Me.TriangleMenuItem, Me.YieldSignMenuItem, Me.DiamondMenuItem, Me.StarMenuItem, Me.MenuItem39, Me.LineMenuItem, Me.ArrowMenuItem, Me.MenuItem42, Me.PictureMenuItem})
        Me.MenuItem32.Text = "&Insert"
        '
        'RectangleMenuItem
        '
        Me.RectangleMenuItem.Index = 0
        Me.RectangleMenuItem.Text = "&Rectangle"
        '
        'ElipseMenuItem
        '
        Me.ElipseMenuItem.Index = 1
        Me.ElipseMenuItem.Text = "&Ellipse"
        '
        'TriangleMenuItem
        '
        Me.TriangleMenuItem.Index = 2
        Me.TriangleMenuItem.Text = "&Triangle"
        '
        'YieldSignMenuItem
        '
        Me.YieldSignMenuItem.Index = 3
        Me.YieldSignMenuItem.Text = "&Yield Sign"
        '
        'DiamondMenuItem
        '
        Me.DiamondMenuItem.Index = 4
        Me.DiamondMenuItem.Text = "&Diamond"
        '
        'StarMenuItem
        '
        Me.StarMenuItem.Index = 5
        Me.StarMenuItem.Text = "&Star"
        '
        'MenuItem39
        '
        Me.MenuItem39.Index = 6
        Me.MenuItem39.Text = "-"
        '
        'LineMenuItem
        '
        Me.LineMenuItem.Index = 7
        Me.LineMenuItem.Text = "&Line"
        '
        'ArrowMenuItem
        '
        Me.ArrowMenuItem.Index = 8
        Me.ArrowMenuItem.Text = "&Arrow"
        '
        'MenuItem42
        '
        Me.MenuItem42.Index = 9
        Me.MenuItem42.Text = "-"
        '
        'PictureMenuItem
        '
        Me.PictureMenuItem.Index = 10
        Me.PictureMenuItem.Text = "&Picture..."
        '
        'MenuItem44
        '
        Me.MenuItem44.Index = 3
        Me.MenuItem44.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.BlankMenuItem, Me.MenuItem51, Me.NarrowCollegeMenuItem, Me.StandardMenuItem, Me.WideMenuItem, Me.MenuItem52, Me.SmallGridMenuItem, Me.GridMenuItem})
        Me.MenuItem44.Text = "&Paper"
        '
        'BlankMenuItem
        '
        Me.BlankMenuItem.Index = 0
        Me.BlankMenuItem.Text = "&Blank"
        '
        'MenuItem51
        '
        Me.MenuItem51.Index = 1
        Me.MenuItem51.Text = "-"
        '
        'NarrowCollegeMenuItem
        '
        Me.NarrowCollegeMenuItem.Index = 2
        Me.NarrowCollegeMenuItem.Text = "&Narrow College"
        '
        'StandardMenuItem
        '
        Me.StandardMenuItem.Index = 3
        Me.StandardMenuItem.Text = "&Standard"
        '
        'WideMenuItem
        '
        Me.WideMenuItem.Index = 4
        Me.WideMenuItem.Text = "&Wide"
        '
        'MenuItem52
        '
        Me.MenuItem52.Index = 5
        Me.MenuItem52.Text = "-"
        '
        'SmallGridMenuItem
        '
        Me.SmallGridMenuItem.Index = 6
        Me.SmallGridMenuItem.Text = "S&mall Grid"
        '
        'GridMenuItem
        '
        Me.GridMenuItem.Index = 7
        Me.GridMenuItem.Text = "&Grid"
        '
        'MenuItem53
        '
        Me.MenuItem53.Index = 4
        Me.MenuItem53.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.AboutMenuItem})
        Me.MenuItem53.Text = "&Help"
        '
        'AboutMenuItem
        '
        Me.AboutMenuItem.Index = 0
        Me.AboutMenuItem.Text = "&About..."
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(752, 518)
        Me.Controls.Add(Me.fScribble)
        Me.Menu = Me.MainMenu1
        Me.Name = "Form1"
        Me.ResumeLayout(False)

    End Sub

    Protected Overrides Sub OnGotFocus(ByVal e As EventArgs)
        OnGotFocus(e)
        fScribble.Focus()
    End Sub
#End Region

#Region " Load/Save user settings "
    Private Sub Form_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\Agilix\Ink\Scribble")
        fScribble.ColorCollectionData = key.GetValue("Colors", "").ToString()
        fScribble.WritingInstrumentsData = key.GetValue("WritingInstruments", "")
        fScribble.FlagsData = key.GetValue("Flags", "").ToString()
        fScribble.Toolbar.RefreshFlagsToolbar()
    End Sub

    Private Sub Application_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\Agilix\Ink\Scribble")
        key.SetValue("CustomColors", fScribble.ColorCollectionData)
        key.SetValue("WritingInstruments", fScribble.WritingInstrumentsData)
        key.SetValue("Flags", fScribble.FlagsData)
        key.Close()

        ' Give the user the chance to save or cancel if note is dirty
        If CheckDirty() = DialogResult.Cancel Then
            e.Cancel = True
        End If
    End Sub
#End Region

#Region " File Menu "
    Private Sub NewMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewMenuItem.Click
        If (CheckDirty() = DialogResult.Cancel) Then
            Return
        End If
        NewFileName()

        Dim document As New Document
        Dim page As New Page(document, Stationery.CreateStockStationeryWithTitle(StationeryStockType.Standard, 19050))

        document.Pages.Add(page)
        fScribble.Scribble.Document = document
    End Sub

    Private Sub OpenMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenMenuItem.Click
        Dim dialog As New OpenFileDialog
        dialog.Title = "Open " + TITLE + " File"
        dialog.CheckFileExists = True
        dialog.Filter() = TITLE + " files (*.ant|*.ant|All files (*.*)|*.*"
        If dialog.ShowDialog() = DialogResult.OK Then
            Open(dialog.FileName)
        End If
    End Sub

    Private Sub ImportMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportMenuItem.Click
        If CheckDirty() = DialogResult.Cancel Then
            Return
        End If
        Dim dialog As New OpenFileDialog
        dialog.Title = "Import File"
        dialog.CheckFileExists = True
        dialog.Filter = "Windows Journal Note (*.jnt)|*.jnt|Ink Serialized Format (*.isf)|*.isf|Image|*.bmp;*.gif;*.png;*.jpg;*.wmf"
        If dialog.ShowDialog() = DialogResult.OK Then
            Import(dialog.FileName, dialog.FilterIndex)
        End If
    End Sub

    Private Sub SaveMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveMenuItem.Click
        If (fFilePath.ToString = "") Or (fFilePath.Length = 0) Then
            SaveAsMenuItem_Click(sender, e)
        Else
            Save(fFilePath)
        End If
    End Sub

    Private Sub SaveAsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsMenuItem.Click
        Dim dialog As New SaveFileDialog
        dialog.Title = "Save As"
        dialog.OverwritePrompt = True
        dialog.CheckPathExists = True
        dialog.DefaultExt = "ant"
        If (fFilePath.ToString = "") Or (fFilePath.Length = 0) Then
            dialog.FileName = fFileName
        Else
            dialog.FileName = fFilePath
        End If
        dialog.Filter() = TITLE + "file (*.ant)|*.ant|Ink Serialized Format (*.isf)|*.isf|Mhtml (*.mht)|*.mht|Image (*.png)|*.png|Rich text (*.rtf)|*.rtf|Plain text (*.txt)|*.txt"
        If dialog.ShowDialog() = DialogResult.OK Then
            fFileName = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName)
            Text = fFileName + " - " + TITLE
            If (dialog.FilterIndex > 1) Then
                Export(dialog.FileName, dialog.FilterIndex)
            Else
                fFilePath = dialog.FileName
                Save(fFilePath)
            End If
        End If
    End Sub

    Private Sub PrintMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintMenuItem.Click
        Dim doc As New System.Drawing.Printing.PrintDocument
        AddHandler doc.PrintPage, AddressOf PrintPage
        fScribble.Document.WaitForBackgroundAnalysis()
        doc.DocumentName = fFileName
        doc.PrinterSettings.FromPage = 1
        doc.PrinterSettings.ToPage = fScribble.Document.Pages.Count
        doc.PrinterSettings.MaximumPage = fScribble.Document.Pages.Count

        Dim dialog As New PrintDialog
        dialog.AllowSelection = False
        dialog.AllowSomePages = True
        dialog.ShowNetwork = True
        dialog.Document = doc
        doc.OriginAtMargins = True
        If dialog.ShowDialog(Me) = DialogResult.OK Then
            fPrintPhysicalRectangles.Clear()
            fPrintPage = dialog.PrinterSettings.FromPage - 1
            fPrintRectangle = 0
            doc.Print()
        End If
    End Sub

    Private Sub ExitMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitMenuItem.Click
        Close()
    End Sub
#End Region

#Region " Edit Menu "
    Private Sub EditMenu_Popup(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim menu As Menu = sender
        If menu Is Nothing Then
            Return
        End If

        menu.MenuItems(0).Enabled = fScribble.Toolbar.CommandTarget.GetCommandStatus(New UndoCommand)
        menu.MenuItems(1).Enabled = fScribble.Toolbar.CommandTarget.GetCommandStatus(New RedoCommand)
        ' 2 - Seperator
        menu.MenuItems(3).Enabled = fScribble.Toolbar.CommandTarget.GetCommandStatus(New CutCommand)
        menu.MenuItems(4).Enabled = fScribble.Toolbar.CommandTarget.GetCommandStatus(New CopyCommand)
        menu.MenuItems(5).Enabled = menu.MenuItems(4).Enabled
        menu.MenuItems(6).Enabled = fScribble.Toolbar.CommandTarget.GetCommandStatus(New PasteCommand)
        menu.MenuItems(7).Enabled = fScribble.Toolbar.CommandTarget.GetCommandStatus(New DeleteCommand)
        ' 8 - Seperator
        ' 9 - Find, always enabled
        ' 10 - Find next, always enabled
        ' 11 - Seperator
        ' 12 - Define Flags, always enabled
        menu.MenuItems(13).Enabled = fScribble.Toolbar.CommandTarget.GetCommandStatus(New BringToFrontCommand)
        menu.MenuItems(14).Enabled = fScribble.Toolbar.CommandTarget.GetCommandStatus(New SendToBackCommand)
        menu.MenuItems(15).Enabled = fScribble.Toolbar.CommandTarget.GetCommandStatus(New GroupCommand)
        menu.MenuItems(16).Enabled = fScribble.Toolbar.CommandTarget.GetCommandStatus(New UngroupCommand)
        menu.MenuItems(17).Enabled = fScribble.Toolbar.CommandTarget.GetCommandStatus(New FormatCommand)
    End Sub
    Private Sub UndoMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UndoMenuItem.Click
        fScribble.Scribble.Undo()
    End Sub

    Private Sub RedoMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RedoMenuItem.Click
        fScribble.Scribble.Redo()
    End Sub

    Private Sub CutMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutMenuItem.Click
        fScribble.Scribble.Cut()
    End Sub
    Private Sub CopyMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyMenuItem.Click
        fScribble.Scribble.Copy()
    End Sub
    Private Sub CopyAsTextMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyAsTextMenuItem.Click
        fScribble.Scribble.CopyAsText()
    End Sub
    Private Sub PasteMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteMenuItem.Click
        fScribble.Scribble.Paste()
    End Sub
    Private Sub DeleteMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteMenuItem.Click
        fScribble.Scribble.Delete()
    End Sub
    Private Sub FindMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindMenuItem.Click
        fScribble.Scribble.Find()
    End Sub
    Private Sub FindNextMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindNextMenuItem.Click
        fScribble.Scribble.FindNext()
    End Sub
    Private Sub DefineMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DefineMenuItem.Click
        Dim dialog As New Dialogs.DefineFlags
        dialog.Colors = fScribble.Toolbar.ColorCollection
        dialog.Flags = fScribble.Toolbar.Flags
        If dialog.ShowDialog(Me) = DialogResult.OK Then
            fScribble.Toolbar.RefreshFlagsToolbar()
        End If
    End Sub
    Private Sub BringToFrontMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BringToFrontMenuItem.Click
        fScribble.Scribble.BringElementsToFront()
    End Sub
    Private Sub SendToBackMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SendToBackMenuItem.Click
        fScribble.Scribble.SendElementsToBack()
    End Sub
    Private Sub GroupMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupMenuItem.Click
        fScribble.Scribble.Group()
    End Sub
    Private Sub UnGroupMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UnGroupMenuItem.Click
        fScribble.Scribble.Ungroup()
    End Sub
    Private Sub ConvertToTextMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConvertToTextMenuItem.Click
        fScribble.Scribble.ConvertToText()
    End Sub
    Private Sub FormatMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FormatMenuItem.Click
        fScribble.Scribble.Format()
    End Sub

#End Region

#Region " Paper Menu "
    Private Sub BlankMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlankMenuItem.Click
        fScribble.Scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Blank, 19050)
    End Sub
    Private Sub NarrowCollegeMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NarrowCollegeMenuItem.Click
        fScribble.Scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Narrow, 19050)
    End Sub
    Private Sub StandardMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StandardMenuItem.Click
        fScribble.Scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Standard, 19050)
    End Sub
    Private Sub WideMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WideMenuItem.Click
        fScribble.Scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Wide, 19050)
    End Sub
    Private Sub SmallGridMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SmallGridMenuItem.Click
        fScribble.Scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.SmallGrid, 19050)
    End Sub
    Private Sub GridMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridMenuItem.Click
        fScribble.Scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Grid, 19050)
    End Sub
#End Region

#Region " Insert Shape Menu "
    Private Sub RectangleMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RectangleMenuItem.Click
        Dim rectangle As New RectangleElement(fScribble.Scribble.Page, System.Drawing.Rectangle.Empty)
        InsertShape(rectangle)
    End Sub

    Private Sub ElipseMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ElipseMenuItem.Click
        Dim ellipse As New EllipseElement(fScribble.Scribble.Page, System.Drawing.Rectangle.Empty)
        InsertShape(ellipse)
    End Sub
    Private Sub TriangleMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TriangleMenuItem.Click
        Dim triangle As New TriangleElement(fScribble.Scribble.Page, System.Drawing.Rectangle.Empty, TriangleType.IsoscelesUp)
        InsertShape(triangle)
    End Sub
    Private Sub YieldSignMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles YieldSignMenuItem.Click
        Dim triangle As New TriangleElement(fScribble.Scribble.Page, System.Drawing.Rectangle.Empty, TriangleType.IsoscelesDown)
        InsertShape(triangle)
    End Sub
    Private Sub DiamondMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiamondMenuItem.Click
        Dim diamond As New DiamondElement(fScribble.Scribble.Page, System.Drawing.Rectangle.Empty)
        InsertShape(diamond)
    End Sub
    Private Sub StarMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StarMenuItem.Click
        Dim star As New StarElement(fScribble.Scribble.Page, System.Drawing.Rectangle.Empty)
        InsertShape(star)
    End Sub
    Private Sub LineMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LineMenuItem.Click
        Dim line As New LineElement(fScribble.Scribble.Page, System.Drawing.Rectangle.Empty)
        InsertShape(line)
    End Sub

    Private Sub ArrowMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ArrowMenuItem.Click
        Dim line As New LineElement(fScribble.Scribble.Page, System.Drawing.Rectangle.Empty)
        line.EndArrow = True
        InsertShape(line)
    End Sub
    Private Sub PictureMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureMenuItem.Click
        fScribble.Scribble.InsertPicture()
    End Sub
#End Region

#Region " Help Menu "
    Private Sub AboutMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutMenuItem.Click
        Dim dialog As New Agilix.Ink.Dialogs.About
        dialog.ShowDialog(Me)
    End Sub
#End Region

#Region " Private Subroutines "
    Private Sub InsertShape(ByVal shape As Agilix.Ink.ShapeElement)
        Dim Format As New FormatCommand
        fScribble.Scribble.GetCommandStatus(Format)
        If (format.State.ColorValid) Then
            shape.LineColor = Format.State.Color
        End If
        fScribble.StylusMode = New InsertShapeMode(shape)
    End Sub

    Private Sub Save(ByVal path As String)
        Try
            If (path = "") Or (path.Length = 0) Then
                SaveAs(path)
            Else
                Dim fs As System.IO.FileStream
                fs = New System.IO.FileStream(path, IO.FileMode.Create, IO.FileAccess.ReadWrite)
                fScribble.Scribble.Document.Save(fs)
                fScribble.Scribble.Modified = False
                fFilePath = path
                fFileName = System.IO.Path.GetFileNameWithoutExtension(path)
                Text = fFileName + " - " + TITLE
                fs.Close()
            End If

        Catch ex As Exception
            MessageBox.Show("Unable to save file: " + path + "  " + ex.Message, "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Open an existing file 
    '   path: Path for note
    Private Sub Open(ByVal path As String)
        Try
            Dim fs As System.IO.FileStream
            Dim doc As New Document

            fs = New System.IO.FileStream(path, IO.FileMode.Open)
            doc.Load(fs)
            fFilePath = path
            fFileName = System.IO.Path.GetFileNameWithoutExtension(path)
            Text = fFileName + " - " + TITLE
            fScribble.Scribble.Document = doc
            fs.Close()
        Catch ex As Exception
            MessageBox.Show("Unable to open file: " + path + "  " + ex.Message, "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Import a Journal note file
    '   filename: path of import file
    '   type: type of import
    Private Sub Import(ByVal filename As String, ByVal type As Integer)
        Dim currentCursor As System.Windows.Forms.Cursor = Cursor.Current
        Dim document As Document
        Dim page As Page
        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Try
            Dim fs As System.IO.FileStream
            fs = New System.IO.FileStream(filename, IO.FileMode.Open, IO.FileAccess.Read)
            Select Case (type)
                Case 1 : fScribble.Scribble.Document = Converters.Journal.ImportScribble(fs, Nothing)
                Case 2
                    document = New Document
                    page = New Page(document)
                    document.Pages.Add(page)
                    Converters.Isf.ImportPage(document.Pages(0), fs, Nothing)
                Case 3
                    document = New Document
                    page = New Page(document)
                    document.Pages.Add(page)
                    Converters.Image.ImportPage(document.Pages(0), fs, Nothing)
                Case 4 : document = Converters.RichText.ImportScribble(fs, New Size(18850, 25000), Nothing)
                Case 5 : document = Converters.PlainText.ImportScribble(fs, New Size(18850, 25000), Nothing)
                Case 6 : document = Converters.PrintJob.ImportScribble(fs, Nothing)
            End Select
            fs.Close()
            fScribble.Scribble.Modified = True
        Catch ex As Exception
            MessageBox.Show("Unable to import file: " + filename + " " + ex.Message, "Error importing file", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Cursor.Current = currentCursor
        End Try
    End Sub

    ' Export a file
    '   filename: Filename
    '   type: Type of export
    Private Sub Export(ByVal filename As String, ByVal type As Integer)
        Try
            Dim fs As System.IO.FileStream
            fs = New System.IO.FileStream(filename, IO.FileMode.Create, IO.FileAccess.ReadWrite)
            Select Case (type)
                Case 2 : Converters.Isf.ExportPage(fScribble.Scribble.Document, fScribble.Scribble.PageIndex, fs)
                Case 3 : Converters.Mhtml.ExportScribble(fScribble.Scribble.Document, fs)
                Case 4 : Converters.Image.ExportPage(fScribble.Scribble.Document, fScribble.Scribble.PageIndex, fs)
                Case 5 : Converters.RichText.ExportScribble(fScribble.Scribble.Document, fs)
                Case 6 : Converters.PlainText.ExportScribble(fScribble.Scribble.Document, fs)
            End Select
        Catch ex As Exception
            MessageBox.Show("Unable to save file: " + filename + "  " + ex.Message, "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Create a new filename for an empty note
    Private Sub NewFileName()
        fFilePath = ""
        fFileNumber = fFileNumber + 1
        fFileName = "Note" + (fFileNumber).ToString()
        Text = fFileName + " - " + TITLE
    End Sub

    ' Check the dirty flag and prompt the user to save if empty
    '   returns: DialogResult.Cancel if the user wants to cancel the operation
    Private Function CheckDirty()
        Dim result As New DialogResult
        result = DialogResult.Yes
        If fScribble.Scribble.Modified = True Then
            result = MessageBox.Show("Do you want to save the changes to """ + fFileName + """?", TITLE, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information)
            Select Case (result)
                Case DialogResult.Yes
                    Save(fFilePath)
                Case DialogResult.No
                Case DialogResult.Cancel
                    ' Do nothing
            End Select
        End If
        Return result
    End Function

    Public Function GetDeviceCaps(ByVal hdc As IntPtr, ByVal nIndex As Integer)
    End Function
    Private Sub PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim doc As PrintDocument
        doc = sender

        ' Convert to HiMetric
        Dim paper As Rectangle
        paper = New Rectangle( _
                0, _
                0, _
                e.MarginBounds.Width * 2540 / 100, _
                e.MarginBounds.Height * 2540 / 100)


        Dim renderer As Renderer
        renderer = New Renderer(fScribble.Scribble.Document)

        ' Paginate the first time through
        Dim PPage As Integer
        If fPrintPhysicalRectangles.Count = 0 Then
            For PPage = doc.PrinterSettings.FromPage - 1 To Math.Min(fScribble.Document.Pages.Count - 1, doc.PrinterSettings.ToPage - 1)
                ' Adjust the size of the margins
                Dim pagePaper As Rectangle
                pagePaper = GetAdjustedSize(fScribble.Document.Pages(PPage), paper)
                pagePaper.Location = New Point(0, 0)

                fPrintPhysicalRectangles.Add(renderer.GetPhysicalPages(PPage, pagePaper.Size, pagePaper.Size))
            Next PPage
        End If
        If fPrintPage >= Math.Min(fScribble.Document.Pages.Count, doc.PrinterSettings.ToPage) Then
            Return
        End If

        Dim clipRectangles() As Rectangle
        clipRectangles = (fPrintPhysicalRectangles(fPrintPage))
        Dim clipRectangle As Rectangle = clipRectangles(fPrintRectangle)

        Dim adjustedPaper As Rectangle
        adjustedPaper = GetAdjustedSize(fScribble.Document.Pages(fPrintPage), paper)

        Dim state As GraphicsState
        state = e.Graphics.Save()

        ' The clipRectangle may be larger than what we asked for,
        ' As the GetPhysicalPages call automatically scales to fit the width
        Dim scale As Single
        scale = adjustedPaper.Width / clipRectangle.Width

        ' Set the graphics context to HiMetric, and translate so that the print origin is at (0,0)
        Dim offset As SizeF
        offset = New SizeF(e.Graphics.Transform.OffsetX, e.Graphics.Transform.OffsetY)
        e.Graphics.TranslateTransform(-offset.Width, -offset.Height)
        e.Graphics.PageUnit = GraphicsUnit.Pixel
        e.Graphics.ScaleTransform(e.Graphics.DpiX / 2540.0F * scale, e.Graphics.DpiY / 2540.0F * scale)
        e.Graphics.TranslateTransform((offset.Width * 25.4F + adjustedPaper.X) / scale, (offset.Height * 25.4F + adjustedPaper.Y) / scale, MatrixOrder.Prepend)
        e.Graphics.TranslateTransform(0, -clipRectangle.Top)

        renderer.Draw(e.Graphics, fPrintPage, clipRectangle, fScribble.Document.Pages(fPrintPage).Size)

        e.Graphics.Restore(state)

        fPrintRectangle += 1
        If fPrintRectangle <> clipRectangles.Length Then
            e.HasMorePages = True
        Else
            fPrintRectangle = 0
            fPrintPage += 1
            If (fPrintPage < Math.Min(fScribble.Document.Pages.Count, doc.PrinterSettings.ToPage)) Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
                fPrintPage = 0
            End If
        End If
    End Sub

    ' This function adjusts the printing for 8 1/2 X 11 inch paper, so that print captured pages print out correctly.
    Private Function GetAdjustedSize(ByVal page As Page, ByVal originalSize As Rectangle)
        Dim rect As Rectangle
        rect = New Rectangle(0, 0, 6.5 * 2540, 9 * 2540)
        If Not originalSize.Equals(rect) Then
            Return originalSize
        End If

        If (page.Width <= originalSize.Width) Then
            Return originalSize
        End If

        If Not page.IBinder Is Nothing Then
            ' Return the original IBinder size
            Return New Rectangle(-0.75 * 2540, -0.75 * 2540, 8 * 2540, 10.25 * 2540)
        End If

        ' Center the width horizontally
        Dim newWidth As Integer
        Dim offset As Integer
        newWidth = Math.Min(page.Width, 8 * 2540)
        offset = (originalSize.Width - newWidth) / 2

        Return New Rectangle(offset, originalSize.Y, newWidth, originalSize.Height)
    End Function

    'Save the current note to a file
    Private Sub SaveAs(ByVal path As String)
        Dim dialog As New SaveFileDialog
        dialog.Title = "Save As"
        dialog.OverwritePrompt = True
        dialog.CheckPathExists = True
        dialog.DefaultExt = "ant"
        If (fFilePath.ToString = "") Or (fFilePath.Length = 0) Then
            dialog.FileName = fFileName
        Else
            dialog.FileName = fFilePath
        End If
        dialog.Filter() = TITLE + "file (*.ant)|*.ant|Ink Serialized Format (*.isf)|*.isf|Mhtml (*.mht)|*.mht|Image (*.png)|*.png|Rich text (*.rtf)|*.rtf|Plain text (*.txt)|*.txt"
        If dialog.ShowDialog() = DialogResult.OK Then
            fFileName = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName)
            Text = fFileName + " - " + TITLE
            If (dialog.FilterIndex > 1) Then
                Export(dialog.FileName, dialog.FilterIndex)
            Else
                fFilePath = dialog.FileName
                Save(fFilePath)
            End If
        End If
    End Sub
#End Region

End Class
