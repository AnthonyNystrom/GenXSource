Imports Agilix.Ink
Imports Agilix.Ink.Scribble
Imports System.Drawing.Printing
Imports System.Drawing.Drawing2D

Public Class Form11024
    Private Const TITLE As String = "NuGenVizDoc Viewer"

#Region " Fields "
    Private fFileName As String
    Private fFilePath As String
    Private fFileNumber As Integer = 0
    Private fPrintPage As Integer
    Private fPrintRectangle As Integer
    Private fPrintPhysicalRectangles As New ArrayList
#End Region

#Region " Load/Save user settings "
    Private Sub Form_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\Agilix\Ink\Scribble")
        ScribbleBox1.ColorCollectionData = key.GetValue("Colors", "").ToString()
        ScribbleBox1.WritingInstrumentsData = key.GetValue("WritingInstruments", "")
        ScribbleBox1.FlagsData = key.GetValue("Flags", "").ToString()
        ScribbleBox1.Toolbar.RefreshFlagsToolbar()
    End Sub

    Private Sub Application_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\Agilix\Ink\Scribble")
        key.SetValue("CustomColors", ScribbleBox1.ColorCollectionData)
        key.SetValue("WritingInstruments", ScribbleBox1.WritingInstrumentsData)
        key.SetValue("Flags", ScribbleBox1.FlagsData)
        key.Close()

        ' Give the user the chance to save or cancel if note is dirty
        If CheckDirty() = DialogResult.Cancel Then
            e.Cancel = True
        End If
    End Sub
#End Region

    '#Region " File Menu "
    '    Private Sub NewMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewMenuItem.Click
    '        If (CheckDirty() = DialogResult.Cancel) Then
    '            Return
    '        End If
    '        NewFileName()

    '        Dim document As New Document
    '        Dim page As New Page(document, Stationery.CreateStockStationeryWithTitle(StationeryStockType.Standard, 19050))

    '        document.Pages.Add(page)
    '        ScribbleBox1.Scribble.Document = document
    '    End Sub

    '    Private Sub OpenMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenMenuItem.Click
    '        Dim dialog As New OpenFileDialog
    '        dialog.Title = "Open " + TITLE + " File"
    '        dialog.CheckFileExists = True
    '        dialog.Filter() = TITLE + " files (*.ant|*.ant|All files (*.*)|*.*"
    '        If dialog.ShowDialog() = DialogResult.OK Then
    '            Open(dialog.FileName)
    '        End If
    '    End Sub

    '    Private Sub ImportMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportMenuItem.Click
    '        If CheckDirty() = DialogResult.Cancel Then
    '            Return
    '        End If
    '        Dim dialog As New OpenFileDialog
    '        dialog.Title = "Import File"
    '        dialog.CheckFileExists = True
    '        dialog.Filter = "Windows Journal Note (*.jnt)|*.jnt|Ink Serialized Format (*.isf)|*.isf|Image|*.bmp;*.gif;*.png;*.jpg;*.wmf"
    '        If dialog.ShowDialog() = DialogResult.OK Then
    '            Import(dialog.FileName, dialog.FilterIndex)
    '        End If
    '    End Sub

    '    Private Sub SaveMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveMenuItem.Click
    '        If (fFilePath.ToString = "") Or (fFilePath.Length = 0) Then
    '            SaveAsMenuItem_Click(sender, e)
    '        Else
    '            Save(fFilePath)
    '        End If
    '    End Sub

    '    Private Sub SaveAsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsMenuItem.Click
    '        Dim dialog As New SaveFileDialog
    '        dialog.Title = "Save As"
    '        dialog.OverwritePrompt = True
    '        dialog.CheckPathExists = True
    '        dialog.DefaultExt = "ant"
    '        If (fFilePath.ToString = "") Or (fFilePath.Length = 0) Then
    '            dialog.FileName = fFileName
    '        Else
    '            dialog.FileName = fFilePath
    '        End If
    '        dialog.Filter() = TITLE + "file (*.ant)|*.ant|Ink Serialized Format (*.isf)|*.isf|Mhtml (*.mht)|*.mht|Image (*.png)|*.png|Rich text (*.rtf)|*.rtf|Plain text (*.txt)|*.txt"
    '        If dialog.ShowDialog() = DialogResult.OK Then
    '            fFileName = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName)
    '            Text = fFileName + " - " + TITLE
    '            If (dialog.FilterIndex > 1) Then
    '                Export(dialog.FileName, dialog.FilterIndex)
    '            Else
    '                fFilePath = dialog.FileName
    '                Save(fFilePath)
    '            End If
    '        End If
    '    End Sub

    '    Private Sub PrintMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintMenuItem.Click
    '        Dim doc As New System.Drawing.Printing.PrintDocument
    '        AddHandler doc.PrintPage, AddressOf PrintPage
    '        ScribbleBox1.Document.WaitForBackgroundAnalysis()
    '        doc.DocumentName = fFileName
    '        doc.PrinterSettings.FromPage = 1
    '        doc.PrinterSettings.ToPage = ScribbleBox1.Document.Pages.Count
    '        doc.PrinterSettings.MaximumPage = ScribbleBox1.Document.Pages.Count

    '        Dim dialog As New PrintDialog
    '        dialog.AllowSelection = False
    '        dialog.AllowSomePages = True
    '        dialog.ShowNetwork = True
    '        dialog.Document = doc
    '        doc.OriginAtMargins = True
    '        If dialog.ShowDialog(Me) = DialogResult.OK Then
    '            fPrintPhysicalRectangles.Clear()
    '            fPrintPage = dialog.PrinterSettings.FromPage - 1
    '            fPrintRectangle = 0
    '            doc.Print()
    '        End If
    '    End Sub

    '    Private Sub ExitMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitMenuItem.Click
    '        Close()
    '    End Sub
    '#End Region

    '#Region " Edit Menu "
    '    Private Sub EditMenu_Popup(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '        Dim menu As Menu = sender
    '        If menu Is Nothing Then
    '            Return
    '        End If

    '        menu.MenuItems(0).Enabled = ScribbleBox1.Toolbar.CommandTarget.GetCommandStatus(New UndoCommand)
    '        menu.MenuItems(1).Enabled = ScribbleBox1.Toolbar.CommandTarget.GetCommandStatus(New RedoCommand)
    '        ' 2 - Seperator
    '        menu.MenuItems(3).Enabled = ScribbleBox1.Toolbar.CommandTarget.GetCommandStatus(New CutCommand)
    '        menu.MenuItems(4).Enabled = ScribbleBox1.Toolbar.CommandTarget.GetCommandStatus(New CopyCommand)
    '        menu.MenuItems(5).Enabled = menu.MenuItems(4).Enabled
    '        menu.MenuItems(6).Enabled = ScribbleBox1.Toolbar.CommandTarget.GetCommandStatus(New PasteCommand)
    '        menu.MenuItems(7).Enabled = ScribbleBox1.Toolbar.CommandTarget.GetCommandStatus(New DeleteCommand)
    '        ' 8 - Seperator
    '        ' 9 - Find, always enabled
    '        ' 10 - Find next, always enabled
    '        ' 11 - Seperator
    '        ' 12 - Define Flags, always enabled
    '        menu.MenuItems(13).Enabled = ScribbleBox1.Toolbar.CommandTarget.GetCommandStatus(New BringToFrontCommand)
    '        menu.MenuItems(14).Enabled = ScribbleBox1.Toolbar.CommandTarget.GetCommandStatus(New SendToBackCommand)
    '        menu.MenuItems(15).Enabled = ScribbleBox1.Toolbar.CommandTarget.GetCommandStatus(New GroupCommand)
    '        menu.MenuItems(16).Enabled = ScribbleBox1.Toolbar.CommandTarget.GetCommandStatus(New UngroupCommand)
    '        menu.MenuItems(17).Enabled = ScribbleBox1.Toolbar.CommandTarget.GetCommandStatus(New FormatCommand)
    '    End Sub
    '    Private Sub UndoMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UndoMenuItem.Click
    '        ScribbleBox1.Scribble.Undo()
    '    End Sub

    '    Private Sub RedoMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RedoMenuItem.Click
    '        ScribbleBox1.Scribble.Redo()
    '    End Sub

    '    Private Sub CutMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutMenuItem.Click
    '        ScribbleBox1.Scribble.Cut()
    '    End Sub
    '    Private Sub CopyMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyMenuItem.Click
    '        ScribbleBox1.Scribble.Copy()
    '    End Sub
    '    Private Sub CopyAsTextMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyAsTextMenuItem.Click
    '        ScribbleBox1.Scribble.CopyAsText()
    '    End Sub
    '    Private Sub PasteMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteMenuItem.Click
    '        ScribbleBox1.Scribble.Paste()
    '    End Sub
    '    Private Sub DeleteMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteMenuItem.Click
    '        ScribbleBox1.Scribble.Delete()
    '    End Sub
    '    Private Sub FindMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindMenuItem.Click
    '        ScribbleBox1.Scribble.Find()
    '    End Sub
    '    Private Sub FindNextMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindNextMenuItem.Click
    '        ScribbleBox1.Scribble.FindNext()
    '    End Sub
    '    Private Sub DefineMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DefineMenuItem.Click
    '        Dim dialog As New Dialogs.DefineFlags
    '        dialog.Colors = ScribbleBox1.Toolbar.ColorCollection
    '        dialog.Flags = ScribbleBox1.Toolbar.Flags
    '        If dialog.ShowDialog(Me) = DialogResult.OK Then
    '            ScribbleBox1.Toolbar.RefreshFlagsToolbar()
    '        End If
    '    End Sub
    '    Private Sub BringToFrontMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BringToFrontMenuItem.Click
    '        ScribbleBox1.Scribble.BringElementsToFront()
    '    End Sub
    '    Private Sub SendToBackMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SendToBackMenuItem.Click
    '        ScribbleBox1.Scribble.SendElementsToBack()
    '    End Sub
    '    Private Sub GroupMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupMenuItem.Click
    '        ScribbleBox1.Scribble.Group()
    '    End Sub
    '    Private Sub UnGroupMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UnGroupMenuItem.Click
    '        ScribbleBox1.Scribble.Ungroup()
    '    End Sub
    '    Private Sub ConvertToTextMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConvertToTextMenuItem.Click
    '        ScribbleBox1.Scribble.ConvertToText()
    '    End Sub
    '    Private Sub FormatMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FormatMenuItem.Click
    '        ScribbleBox1.Scribble.Format()
    '    End Sub

    '#End Region

    '#Region " Paper Menu "
    '    Private Sub BlankMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlankMenuItem.Click
    '        ScribbleBox1.Scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Blank, 19050)
    '    End Sub
    '    Private Sub NarrowCollegeMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NarrowCollegeMenuItem.Click
    '        ScribbleBox1.Scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Narrow, 19050)
    '    End Sub
    '    Private Sub StandardMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StandardMenuItem.Click
    '        ScribbleBox1.Scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Standard, 19050)
    '    End Sub
    '    Private Sub WideMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WideMenuItem.Click
    '        ScribbleBox1.Scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Wide, 19050)
    '    End Sub
    '    Private Sub SmallGridMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SmallGridMenuItem.Click
    '        ScribbleBox1.Scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.SmallGrid, 19050)
    '    End Sub
    '    Private Sub GridMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridMenuItem.Click
    '        ScribbleBox1.Scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Grid, 19050)
    '    End Sub
    '#End Region

    '#Region " Insert Shape Menu "
    '    Private Sub RectangleMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RectangleMenuItem.Click
    '        Dim rectangle As New RectangleElement(ScribbleBox1.Scribble.Page, System.Drawing.Rectangle.Empty)
    '        InsertShape(rectangle)
    '    End Sub

    '    Private Sub ElipseMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ElipseMenuItem.Click
    '        Dim ellipse As New EllipseElement(ScribbleBox1.Scribble.Page, System.Drawing.Rectangle.Empty)
    '        InsertShape(ellipse)
    '    End Sub
    '    Private Sub TriangleMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TriangleMenuItem.Click
    '        Dim triangle As New TriangleElement(ScribbleBox1.Scribble.Page, System.Drawing.Rectangle.Empty, TriangleType.IsoscelesUp)
    '        InsertShape(triangle)
    '    End Sub
    '    Private Sub YieldSignMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles YieldSignMenuItem.Click
    '        Dim triangle As New TriangleElement(ScribbleBox1.Scribble.Page, System.Drawing.Rectangle.Empty, TriangleType.IsoscelesDown)
    '        InsertShape(triangle)
    '    End Sub
    '    Private Sub DiamondMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiamondMenuItem.Click
    '        Dim diamond As New DiamondElement(ScribbleBox1.Scribble.Page, System.Drawing.Rectangle.Empty)
    '        InsertShape(diamond)
    '    End Sub
    '    Private Sub StarMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StarMenuItem.Click
    '        Dim star As New StarElement(ScribbleBox1.Scribble.Page, System.Drawing.Rectangle.Empty)
    '        InsertShape(star)
    '    End Sub
    '    Private Sub LineMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LineMenuItem.Click
    '        Dim line As New LineElement(ScribbleBox1.Scribble.Page, System.Drawing.Rectangle.Empty)
    '        InsertShape(line)
    '    End Sub

    '    Private Sub ArrowMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ArrowMenuItem.Click
    '        Dim line As New LineElement(ScribbleBox1.Scribble.Page, System.Drawing.Rectangle.Empty)
    '        line.EndArrow = True
    '        InsertShape(line)
    '    End Sub
    '    Private Sub PictureMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureMenuItem.Click
    '        ScribbleBox1.Scribble.InsertPicture()
    '    End Sub
    '#End Region

    '#Region " Help Menu "
    '    Private Sub AboutMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutMenuItem.Click
    '        Dim dialog As New Agilix.Ink.Dialogs.About
    '        dialog.ShowDialog(Me)
    '    End Sub
    '#End Region

#Region " Private Subroutines "
    Private Sub InsertShape(ByVal shape As Agilix.Ink.ShapeElement)
        Dim Format As New FormatCommand
        ScribbleBox1.Scribble.GetCommandStatus(Format)
        If (Format.State.ColorValid) Then
            shape.LineColor = Format.State.Color
        End If
        ScribbleBox1.StylusMode = New InsertShapeMode(shape)
    End Sub

    Private Sub Save(ByVal path As String)
        Try
            If (path = "") Or (path.Length = 0) Then
                SaveAs(path)
            Else
                Dim fs As System.IO.FileStream
                fs = New System.IO.FileStream(path, IO.FileMode.Create, IO.FileAccess.ReadWrite)
                ScribbleBox1.Scribble.Document.Save(fs)
                ScribbleBox1.Scribble.Modified = False
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
            ScribbleBox1.Scribble.Document = doc
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
                Case 1 : ScribbleBox1.Scribble.Document = Converters.Journal.ImportScribble(fs, Nothing)
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
                Case 4 : document = Converters.RichText.ImportScribble(fs, New System.Drawing.Size(18850, 25000), Nothing)
                Case 5 : document = Converters.PlainText.ImportScribble(fs, New System.Drawing.Size(18850, 25000), Nothing)
                Case 6 : document = Converters.PrintJob.ImportScribble(fs, Nothing)
            End Select
            fs.Close()
            ScribbleBox1.Scribble.Modified = True
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
                Case 2 : Converters.Isf.ExportPage(ScribbleBox1.Scribble.Document, ScribbleBox1.Scribble.PageIndex, fs)
                Case 3 : Converters.Mhtml.ExportScribble(ScribbleBox1.Scribble.Document, fs)
                Case 4 : Converters.Image.ExportPage(ScribbleBox1.Scribble.Document, ScribbleBox1.Scribble.PageIndex, fs)
                Case 5 : Converters.RichText.ExportScribble(ScribbleBox1.Scribble.Document, fs)
                Case 6 : Converters.PlainText.ExportScribble(ScribbleBox1.Scribble.Document, fs)
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
        If ScribbleBox1.Scribble.Modified = True Then
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
        renderer = New Renderer(ScribbleBox1.Scribble.Document)

        ' Paginate the first time through
        Dim PPage As Integer
        If fPrintPhysicalRectangles.Count = 0 Then
            For PPage = doc.PrinterSettings.FromPage - 1 To Math.Min(ScribbleBox1.Document.Pages.Count - 1, doc.PrinterSettings.ToPage - 1)
                ' Adjust the size of the margins
                Dim pagePaper As Rectangle
                pagePaper = GetAdjustedSize(ScribbleBox1.Document.Pages(PPage), paper)
                pagePaper.Location = New System.Drawing.Point(0, 0)

                fPrintPhysicalRectangles.Add(renderer.GetPhysicalPages(PPage, pagePaper.Size, pagePaper.Size))
            Next PPage
        End If
        If fPrintPage >= Math.Min(ScribbleBox1.Document.Pages.Count, doc.PrinterSettings.ToPage) Then
            Return
        End If

        Dim clipRectangles() As Rectangle
        clipRectangles = (fPrintPhysicalRectangles(fPrintPage))
        Dim clipRectangle As Rectangle = clipRectangles(fPrintRectangle)

        Dim adjustedPaper As Rectangle
        adjustedPaper = GetAdjustedSize(ScribbleBox1.Document.Pages(fPrintPage), paper)

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

        renderer.Draw(e.Graphics, fPrintPage, clipRectangle, ScribbleBox1.Document.Pages(fPrintPage).Size)

        e.Graphics.Restore(state)

        fPrintRectangle += 1
        If fPrintRectangle <> clipRectangles.Length Then
            e.HasMorePages = True
        Else
            fPrintRectangle = 0
            fPrintPage += 1
            If (fPrintPage < Math.Min(ScribbleBox1.Document.Pages.Count, doc.PrinterSettings.ToPage)) Then
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


    'Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem1.Click
    '    Dim frmCap As New frmCapture

    '    frmCap.ShowDialog()
    '    Me.ScribbleBox1.Scribble.Paste()
    '    frmCap.Dispose()
    '    frmCap = Nothing
    'End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        If (CheckDirty() = DialogResult.Cancel) Then
            Return
        End If
        NewFileName()

        Dim document As New Document
        Dim page As New Page(document, Stationery.CreateStockStationeryWithTitle(StationeryStockType.Standard, 19050))

        document.Pages.Add(page)
        ScribbleBox1.Scribble.Document = document
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        Dim dialog As New OpenFileDialog
        dialog.Title = "Open " + TITLE + " File"
        dialog.CheckFileExists = True
        dialog.Filter() = TITLE + " files (*.ant|*.ant|All files (*.*)|*.*"
        If dialog.ShowDialog() = DialogResult.OK Then
            Open(dialog.FileName)
        End If
    End Sub

    Private Sub ImportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportToolStripMenuItem.Click
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

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        If (fFilePath.ToString = "") Or (fFilePath.Length = 0) Then
            SaveAsToolStripMenuItem_Click(sender, e)
        Else
            Save(fFilePath)
        End If
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click
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

    Private Function PreparePrintDocument() As PrintDocument
        Dim doc As New System.Drawing.Printing.PrintDocument
        AddHandler doc.PrintPage, AddressOf PrintPage

        'ScribbleBox1.Document.WaitForBackgroundAnalysis()
        'doc.DocumentName = fFileName
        'doc.PrinterSettings.FromPage = 1
        'doc.PrinterSettings.ToPage = ScribbleBox1.Document.Pages.Count
        'doc.PrinterSettings.MaximumPage = ScribbleBox1.Document.Pages.Count
        'fPrintPhysicalRectangles.Clear()
        'fPrintPage = doc.PrinterSettings.FromPage - 1
        'fPrintRectangle = 0
        'Dim dialog As New PrintDialog
        'dialog.AllowSelection = False
        'dialog.AllowSomePages = True
        'dialog.ShowNetwork = True
        'dialog.Document = doc
        'doc.OriginAtMargins = True
        'doc.Print()
        Return doc
    End Function

    Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click
        'Dim doc As New System.Drawing.Printing.PrintDocument
        'AddHandler doc.PrintPage, AddressOf PrintPage
        'PrintPage(doc, e)
        'ScribbleBox1.Document.WaitForBackgroundAnalysis()
        'doc.DocumentName = fFileName
        'doc.PrinterSettings.FromPage = 1
        'doc.PrinterSettings.ToPage = ScribbleBox1.Document.Pages.Count
        'doc.PrinterSettings.MaximumPage = ScribbleBox1.Document.Pages.Count

        'Dim dialog As New PrintDialog
        'dialog.AllowSelection = False
        'dialog.AllowSomePages = True
        'dialog.ShowNetwork = True
        'dialog.Document = doc
        'doc.OriginAtMargins = True

        'Dim printform As New frmPP

        'fPrintPhysicalRectangles.Clear()
        'fPrintPage = dialog.PrinterSettings.FromPage - 1
        'fPrintRectangle = 0
        'printform.C1PrintPreviewControl1.Document = doc


        'printform.Show()

        'doc.Print()
        'If dialog.ShowDialog(Me) = DialogResult.OK Then

        '    doc.Print()
        'End If
        'PrintPage(doc)
        'Dim formprint As New frmPP

        'formprint.Show()
        'Dim FF As New Form_Container
        'Dim FFF As New frmPP

        'F.Height = 1024
        'F.Width = 768
        'FF.Prepare(New Drawing.Point(10, 20), FFF)
        'FFF.C1PrintPreviewControl1.Document = PreparePrintDocument()
        'FFF.Show()
        C1PrintPreviewDialog1.Document = PreparePrintDocument()
        C1PrintPreviewDialog1.ShowDialog()
    End Sub

    Private Sub CloseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub UndoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UndoToolStripMenuItem.Click
        ScribbleBox1.Scribble.Undo()
    End Sub

    Private Sub RedoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RedoToolStripMenuItem.Click
        ScribbleBox1.Scribble.Redo()
    End Sub

    Private Sub CutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutToolStripMenuItem.Click
        ScribbleBox1.Scribble.Cut()
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        ScribbleBox1.Scribble.Copy()
    End Sub

    Private Sub CopyAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyAsToolStripMenuItem.Click
        ScribbleBox1.Scribble.CopyAsText()
    End Sub

    Private Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        ScribbleBox1.Scribble.Paste()
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        ScribbleBox1.Scribble.Delete()
    End Sub

    Private Sub FindToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindToolStripMenuItem.Click
        ScribbleBox1.Scribble.Find()
    End Sub

    Private Sub FindNextToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindNextToolStripMenuItem.Click
        ScribbleBox1.Scribble.FindNext()
    End Sub

    Private Sub DefineToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DefineToolStripMenuItem.Click
        Dim dialog As New Dialogs.DefineFlags
        dialog.Colors = ScribbleBox1.Toolbar.ColorCollection
        dialog.Flags = ScribbleBox1.Toolbar.Flags
        If dialog.ShowDialog(Me) = DialogResult.OK Then
            ScribbleBox1.Toolbar.RefreshFlagsToolbar()
        End If
    End Sub

    Private Sub BringToFrontToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BringToFrontToolStripMenuItem.Click
        ScribbleBox1.Scribble.BringElementsToFront()
    End Sub

    Private Sub SendToBackToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SendToBackToolStripMenuItem.Click
        ScribbleBox1.Scribble.SendElementsToBack()
    End Sub

    Private Sub GroupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupToolStripMenuItem.Click
        ScribbleBox1.Scribble.Group()
    End Sub

    Private Sub UnGroupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UnGroupToolStripMenuItem.Click
        ScribbleBox1.Scribble.Ungroup()
    End Sub

    Private Sub ConvertToTextToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConvertToTextToolStripMenuItem.Click
        ScribbleBox1.Scribble.ConvertToText()
    End Sub

    Private Sub FormatToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FormatToolStripMenuItem.Click
        ScribbleBox1.Scribble.Format()
    End Sub

    Private Sub GridToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridToolStripMenuItem.Click
        ScribbleBox1.Scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.SmallGrid, 19050)
    End Sub

    Private Sub GridToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridToolStripMenuItem1.Click
        ScribbleBox1.Scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Grid, 19050)
    End Sub

    Private Sub RectangleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RectangleToolStripMenuItem.Click
        Dim rectangle As New RectangleElement(ScribbleBox1.Scribble.Page, System.Drawing.Rectangle.Empty)
        InsertShape(rectangle)
    End Sub

    Private Sub ElipseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ElipseToolStripMenuItem.Click
        Dim ellipse As New EllipseElement(ScribbleBox1.Scribble.Page, System.Drawing.Rectangle.Empty)
        InsertShape(ellipse)
    End Sub

    Private Sub TriangleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TriangleToolStripMenuItem.Click
        Dim triangle As New TriangleElement(ScribbleBox1.Scribble.Page, System.Drawing.Rectangle.Empty, TriangleType.IsoscelesUp)
        InsertShape(triangle)
    End Sub

    Private Sub YieldToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles YieldToolStripMenuItem.Click
        Dim triangle As New TriangleElement(ScribbleBox1.Scribble.Page, System.Drawing.Rectangle.Empty, TriangleType.IsoscelesDown)
        InsertShape(triangle)
    End Sub

    Private Sub DiamondToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiamondToolStripMenuItem.Click
        Dim diamond As New DiamondElement(ScribbleBox1.Scribble.Page, System.Drawing.Rectangle.Empty)
        InsertShape(diamond)
    End Sub

    Private Sub StarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StarToolStripMenuItem.Click
        Dim star As New StarElement(ScribbleBox1.Scribble.Page, System.Drawing.Rectangle.Empty)
        InsertShape(star)
    End Sub

    Private Sub LineToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LineToolStripMenuItem.Click
        Dim line As New LineElement(ScribbleBox1.Scribble.Page, System.Drawing.Rectangle.Empty)
        InsertShape(line)
    End Sub

    Private Sub ArrowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ArrowToolStripMenuItem.Click
        Dim line As New LineElement(ScribbleBox1.Scribble.Page, System.Drawing.Rectangle.Empty)
        line.EndArrow = True
        InsertShape(line)
    End Sub

    Private Sub PictureToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureToolStripMenuItem.Click
        ScribbleBox1.Scribble.InsertPicture()
    End Sub

    Private Sub CAPTUREToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CAPTUREToolStripMenuItem1.Click
        Dim frmCap As New frmCapture

        frmCap.ShowDialog()
        Me.ScribbleBox1.Scribble.Paste()
        frmCap.Dispose()
        frmCap = Nothing
    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Me.NuGenRuler1.Hide()
    End Sub

    Private Sub ShowTopRulerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowTopRulerToolStripMenuItem.Click
        Me.NuGenRuler1.Show()
    End Sub

    Private Sub HideLeftRulerToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideLeftRulerToolStripMenuItem1.Click
        Me.NuGenRuler2.Hide()
    End Sub

    Private Sub ShowLeftRulerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowLeftRulerToolStripMenuItem.Click
        Me.NuGenRuler2.Show()
    End Sub

    Private Sub HideToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideToolStripMenuItem.Click

    End Sub

    Private Sub ShowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowToolStripMenuItem.Click

    End Sub

    Private Sub HideToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideToolStripMenuItem1.Click

    End Sub

    Private Sub ShowToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowToolStripMenuItem1.Click

    End Sub

    Private Sub HideToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideToolStripMenuItem2.Click

    End Sub

    Private Sub ShowToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowToolStripMenuItem2.Click

    End Sub

    Private Sub HideToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideToolStripMenuItem3.Click

    End Sub

    Private Sub ShowToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowToolStripMenuItem3.Click

    End Sub
End Class