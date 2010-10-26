Imports Agilix.Ink
Imports Agilix.Ink.Scribble
Imports Genetibase.UI
Imports System.IO


Public Class frmNuGenPrim


    Private Const TITLE As String = "NuGenVizDOC "
    Private fFileName As String
    Private fFilePath As String
    Private fFileNumber As Integer = 0
    Private Sub frmNuGenPrim_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        '<EhHeader>
        Try
            '</EhHeader>
            ScribbleBox1.Dispose()
            ScribbleBox2.Dispose()

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    'Private Sub frmUI_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    '    'DSTBLPROJECTS.Update()
    'End Sub
    Private Sub InsertShape(ByVal shape As Agilix.Ink.ShapeElement)

        '<EhHeader>
        Try

            '</EhHeader>
            Dim Format As New FormatCommand
            ScribbleBox1.Scribble.GetCommandStatus(Format)

            If (Format.State.ColorValid) Then
                shape.LineColor = Format.State.Color
            End If

            ScribbleBox1.StylusMode = New InsertShapeMode(shape)

            '<EhFooter>
        Catch
        End Try '</EhFooter>

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
                'Text = fFileName + " - " + TITLE
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
            'Text = fFileName + " - " + TITLE
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

        '<EhHeader>
        Try
            '</EhHeader>
            fFilePath = ""
            fFileNumber = fFileNumber + 1
            fFileName = "Note" + (fFileNumber).ToString()
            'Text = fFileName + " - " + TITLE
            Text = "New Project" & " Not Saved" & " - " & TITLE

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    'Save the current note to a file
    Private Sub SaveAs(ByVal path As String)

        '<EhHeader>
        Try

            '</EhHeader>
            Dim dialog As New SaveFileDialog
            dialog.Title = "Save As"
            dialog.OverwritePrompt = True
            dialog.CheckPathExists = True
            dialog.DefaultExt = "vdl"

            If fFilePath = Nothing Then
                dialog.FileName = fFileName

            Else
                dialog.FileName = fFilePath
            End If

            dialog.Filter() = TITLE + "file (*.vdl)|*.vdl|Ink Serialized Format (*.isf)|*.isf|Mhtml (*.mht)|*.mht|Image (*.png)|*.png|Rich text (*.rtf)|*.rtf|Plain text (*.txt)|*.txt"

            If dialog.ShowDialog() = DialogResult.OK Then
                fFileName = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName)

                'Text = fFileName + " - " + TITLE
                If (dialog.FilterIndex > 1) Then
                    Export(dialog.FileName, dialog.FilterIndex)

                Else
                    fFilePath = dialog.FileName
                    Save(fFilePath)
                End If
            End If

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        '<EhHeader>
        Try
            '</EhHeader>
            NewFileName()

            Dim document As New Document
            Dim page As New Page(document, Stationery.CreateStockStationeryWithTitle(StationeryStockType.Standard, 19050))

            document.Pages.Add(page)
            ScribbleBox1.Scribble.Document = document

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click

        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            If (CheckDirty() = DialogResult.Cancel) Then
                Return
            End If

            Dim dialog As New OpenFileDialog
            dialog.Title = "Open " + TITLE + " File"
            dialog.CheckFileExists = True
            dialog.Filter() = TITLE + " files (*.vdl)|*.vdl|All files (*.*)|*.*"

            If dialog.ShowDialog() = DialogResult.OK Then
                Open(dialog.FileName)
                Me.Text = "Project" & " - " & dialog.FileName & " - " & TITLE

            Else
                Return
            End If

            panelEx1.Visible = False
            rulerControl1.Visible = True
            'rulerControl1.BringToFront()
            rulerControl2.Visible = True
            rulerControl2.BringToFront()
            ScribbleBox1.BringToFront()
            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Sub ImportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportToolStripMenuItem.Click

        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            'If (CheckDirty() = DialogResult.Cancel) Then
            '    Return
            'End If
            Dim dialog As New OpenFileDialog
            dialog.Title = "Import File"
            dialog.CheckFileExists = True
            dialog.Filter = "Windows Journal Note (*.jnt)|*.jnt|Ink Serialized Format (*.isf)|*.isf|Image|*.bmp;*.gif;*.png;*.jpg;*.wmf"

            If dialog.ShowDialog() = DialogResult.OK Then
                Import(dialog.FileName, dialog.FilterIndex)

            Else
                Return
            End If


            panelEx1.Visible = False
            rulerControl1.Visible = True
            rulerControl2.Visible = True
            ScribbleBox1.BringToFront()
            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click

        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            Dim dialog As New SaveFileDialog
            dialog.Title = "Save As"
            dialog.OverwritePrompt = True
            dialog.CheckPathExists = True
            dialog.DefaultExt = "vdl"
            dialog.Filter() = TITLE + "file (*.vdl)|*.vdl|Ink Serialized Format (*.isf)|*.isf|Mhtml (*.mht)|*.mht|Image (*.png)|*.png|Rich text (*.rtf)|*.rtf|Plain text (*.txt)|*.txt"

            If dialog.ShowDialog() = DialogResult.OK Then
                fFileName = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName)

                If (dialog.FilterIndex > 1) Then
                    Export(dialog.FileName, dialog.FilterIndex)

                Else
                    fFilePath = dialog.FileName
                    Save(fFilePath)
                End If

                Me.Text = "Project" & " - " & dialog.FileName & " - " & TITLE
            End If

            Application.DoEvents()

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Sub CloseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Hide()
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

        '<EhHeader>
        Try

            '</EhHeader>
            Dim dialog As New Dialogs.DefineFlags
            dialog.Colors = ScribbleBox1.Toolbar.ColorCollection
            dialog.Flags = ScribbleBox1.Toolbar.Flags

            If dialog.ShowDialog(Me) = DialogResult.OK Then
                ScribbleBox1.Toolbar.RefreshFlagsToolbar()
            End If

            '<EhFooter>
        Catch
        End Try '</EhFooter>

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
        ScribbleBox1.Scribble.Stationery = Stationery.CreateStockStationery(StationeryStockType.SmallGrid)
    End Sub

    Private Sub GridToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridToolStripMenuItem1.Click
        ScribbleBox1.Scribble.Stationery = Stationery.CreateStockStationery(StationeryStockType.Grid)
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

    Private Sub CAPTUREToolStripMenuItem1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)

        '<EhHeader>
        Try
            '</EhHeader>
            Hide()

            Dim frmCap As New frmCapture
            frmCap.ShowDialog()
            Show()
            ScribbleBox1.Scribble.Paste()
            frmCap.Dispose()
            frmCap = Nothing

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub CheckEdit1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit1.CheckedChanged

        '<EhHeader>
        Try

            '</EhHeader>
            Select Case CheckEdit1.Checked

                Case True
                    Me.ShowInTaskbar = True

                Case False
                    Me.ShowInTaskbar = False
            End Select

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Sub UiButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UiButton1.Click

        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()



            If panelEx1.Visible = True Then
                MsgBox("You need to have an active project open before capturing")
                Return
            End If

            uiPanel0.Hide()
            uiPanel0.Refresh()
            uiPanel2.Hide()
            uiPanel2.Refresh()

            Me.Hide()

            Dim frmCt As New frmDwnCnt(CInt(SpinEdit1.Value))
            frmCt.ShowDialog()

            If Not frmCt.DialogResult = Windows.Forms.DialogResult.Cancel Then
                frmCt.Close()
                frmCt.Dispose()
                frmCt = Nothing

                Dim frmCap As New frmCapture
                frmCap.nuGenScreenCap1.CoordsColor = UiColorButton1.SelectedColor
                frmCap.ShowDialog()
                ScribbleBox1.Scribble.Paste()
                ScribbleBox2.Scribble.Paste()
                ScribbleBox2.Scribble.ZoomOut()
                ScribbleBox2.Scribble.ZoomOut()
                ScribbleBox2.Scribble.ZoomOut()
                frmCap.Dispose()
                frmCap = Nothing
                Me.Show()

            Else
                frmCt.Dispose()
                frmCt = Nothing
                Me.Show()
            End If

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Sub Command0_Click(ByVal sender As Object, ByVal e As Janus.Windows.UI.CommandBars.CommandEventArgs) Handles Command0.Click
        Me.Opacity = 1
        UiButton2.Text = "UI Opacity - 100%"
    End Sub

    Private Sub Command1_Click(ByVal sender As Object, ByVal e As Janus.Windows.UI.CommandBars.CommandEventArgs) Handles Command1.Click
        Me.Opacity = 0.9
        UiButton2.Text = "UI Opacity - 90%"
    End Sub

    Private Sub Command2_Click(ByVal sender As Object, ByVal e As Janus.Windows.UI.CommandBars.CommandEventArgs) Handles Command2.Click
        Me.Opacity = 0.8
        UiButton2.Text = "UI Opacity - 80%"
    End Sub

    Private Sub Command3_Click(ByVal sender As Object, ByVal e As Janus.Windows.UI.CommandBars.CommandEventArgs) Handles Command3.Click
        Me.Opacity = 0.7
        UiButton2.Text = "UI Opacity - 70%"
    End Sub

    Private Sub Command4_Click(ByVal sender As Object, ByVal e As Janus.Windows.UI.CommandBars.CommandEventArgs) Handles Command4.Click
        Me.Opacity = 0.6
        UiButton2.Text = "UI Opacity - 60%"
    End Sub

    Private Sub Command5_Click(ByVal sender As Object, ByVal e As Janus.Windows.UI.CommandBars.CommandEventArgs) Handles Command5.Click
        Me.Opacity = 0.5
        UiButton2.Text = "UI Opacity - 50%"
    End Sub

    Private Sub Command6_Click(ByVal sender As Object, ByVal e As Janus.Windows.UI.CommandBars.CommandEventArgs) Handles Command6.Click
        Me.Opacity = 0.4
        UiButton2.Text = "UI Opacity - 40%"
    End Sub

    Private Sub Command7_Click(ByVal sender As Object, ByVal e As Janus.Windows.UI.CommandBars.CommandEventArgs) Handles Command7.Click
        Me.Opacity = 0.3
        UiButton2.Text = "UI Opacity - 30%"
    End Sub

    Private Sub Command8_Click(ByVal sender As Object, ByVal e As Janus.Windows.UI.CommandBars.CommandEventArgs) Handles Command8.Click
        Me.Opacity = 0.2
        UiButton2.Text = "UI Opacity - 20%"
    End Sub

    Private Sub Command9_Click(ByVal sender As Object, ByVal e As Janus.Windows.UI.CommandBars.CommandEventArgs) Handles Command9.Click
        Me.Opacity = 0.1
        UiButton2.Text = "UI Opacity - 10%"
    End Sub

    Private Sub ExplorerBar1_ItemClick(ByVal sender As Object, ByVal e As Janus.Windows.ExplorerBar.ItemEventArgs) Handles ExplorerBar1.ItemClick

        '<EhHeader>
        Try

            '</EhHeader>
            Select Case e.Item.Key

                Case "HSToolbar"

                    Select Case Me.ScribbleBox1.Toolbar.Visible

                        Case True
                            Me.ScribbleBox1.Toolbar.Visible = False

                        Case False
                            Me.ScribbleBox1.Toolbar.Visible = True
                    End Select

                Case "HSCHToolbar"

                    Select Case Me.ScribbleBox2.Toolbar.Visible

                        Case True
                            Me.ScribbleBox2.Toolbar.Visible = False

                        Case False
                            Me.ScribbleBox2.Toolbar.Visible = True
                    End Select

                Case "CHZoomLevel"

                    Select Case Me.ScribbleBox2.ShowZoom

                        Case True
                            Me.ScribbleBox2.ShowZoom = False

                        Case False
                            Me.ScribbleBox2.ShowZoom = True
                    End Select

                    'Case "NEProjects"
                    '    Application.DoEvents()
                    '    Dim frmPEditNew As New frmProjectCE
                    '    frmPEditNew.ShowDialog()
                    '    frmPEditNew.Dispose()
                    '    frmPEditNew = Nothing
                    '    DSTBLPROJECTS.Fill()
                    'Case "SNow"
                    '    Dim cindex As Integer = GridEX1.CurrentRow.RowIndex
                    '    DSTBLPROJECTS.Update()
                    '    GridEX1.MoveToRowIndex(cindex)
                    '    cindex = Nothing
            End Select

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click

        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            Dim aboutdia As New Genetibase.UI.NuGenAbout.AboutBox
            aboutdia.ShowDialog()
            aboutdia.Dispose()
            aboutdia = Nothing

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    'Private Sub TSTBLPROJECTS_AfterChanges(ByVal sender As Object, ByVal e As C1.Data.RowChangeEventArgs)
    '    Me.Text = "Project " & TSTBLPROJECTS.CurrentRow.Item("strProjectIdentity") & " - " & TITLE
    'End Sub
    'Private Sub TSTBLPROJECTS_PositionChanged(ByVal sender As Object, ByVal e As C1.Data.PositionChangeEventArgs)
    '    Me.Text = "Project " & TSTBLPROJECTS.CurrentRow.Item("strProjectIdentity") & " - " & TITLE
    'End Sub
    Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click

        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            Dim previewer As New NuGenC1ScribblePreviewer
            previewer.ShowPrintDialog = True
            previewer.DoPreviewDialog(ScribbleBox1.Scribble)
            previewer = Nothing

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        '_license = LicenseManager.Validate(GetType(Form1), Me)
        InitializeComponent()
        'ApplicationWaitCursor.Cursor = Cursors.WaitCursor
        'ApplicationWaitCursor.Delay = New TimeSpan(0, 0, 0, 0, 250)
        ' Add any initialization after the InitializeComponent() call.
    End Sub



    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click

        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            If fFilePath = Nothing Then
                Application.DoEvents()

                Dim dialog As New SaveFileDialog
                dialog.Title = "Save As"
                dialog.OverwritePrompt = True
                dialog.CheckPathExists = True
                dialog.DefaultExt = "vdl"
                dialog.Filter() = TITLE + "file (*.vdl)|*.vdl|Ink Serialized Format (*.isf)|*.isf|Mhtml (*.mht)|*.mht|Image (*.png)|*.png|Rich text (*.rtf)|*.rtf|Plain text (*.txt)|*.txt"

                If dialog.ShowDialog() = DialogResult.OK Then
                    fFileName = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName)

                    If (dialog.FilterIndex > 1) Then
                        Export(dialog.FileName, dialog.FilterIndex)

                    Else
                        fFilePath = dialog.FileName
                        Save(fFilePath)
                    End If

                    Me.Text = "Project" & " - " & dialog.FileName & " - " & TITLE
                End If

                Application.DoEvents()

            Else
                Save(fFilePath)
            End If

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Sub NewToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click

        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            If (CheckDirty() = DialogResult.Cancel) Then
                Return
            End If



            NewFileName()

            Dim document As New Document
            Dim page As New Page(document, Stationery.CreateStockStationery(StationeryStockType.Blank))

            document.Pages.Add(page)
            ScribbleBox1.Scribble.Document = document


            panelEx1.Visible = False
            rulerControl1.Visible = True
            rulerControl2.Visible = True
            ScribbleBox1.BringToFront()
            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Function CheckDirty() As DialogResult

        '<EhHeader>
        Try

            '</EhHeader>
            Dim result As New DialogResult
            result = DialogResult.Yes

            If ScribbleBox1.Scribble.Modified = True Then
                result = MessageBox.Show("Do you want to save this Project?", TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Information)

                Select Case (result)

                    Case DialogResult.Yes

                        If fFilePath = Nothing Then
                            Application.DoEvents()

                            Dim dialog As New SaveFileDialog
                            dialog.Title = "Save As"
                            dialog.OverwritePrompt = True
                            dialog.CheckPathExists = True
                            dialog.DefaultExt = "vdl"
                            dialog.Filter() = TITLE + "file (*.vdl)|*.vdl|Ink Serialized Format (*.isf)|*.isf|Mhtml (*.mht)|*.mht|Image (*.png)|*.png|Rich text (*.rtf)|*.rtf|Plain text (*.txt)|*.txt"

                            If dialog.ShowDialog() = DialogResult.OK Then
                                fFileName = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName)

                                If (dialog.FilterIndex > 1) Then
                                    Export(dialog.FileName, dialog.FilterIndex)

                                Else
                                    fFilePath = dialog.FileName
                                    Save(fFilePath)
                                End If
                            End If

                            Application.DoEvents()

                        Else
                            Save(fFilePath)
                        End If

                    Case DialogResult.No
                    Case DialogResult.Cancel
                        ' Do nothing
                End Select

            End If

            Return result

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Function

    Private Sub frmNuGenPrim_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        '<EhHeader>
        Try
            '</EhHeader>
            CheckDirty()

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Sub BlankToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlankToolStripMenuItem.Click

        '<EhHeader>
        Try
            '</EhHeader>
            ScribbleBox1.Scribble.Stationery = Stationery.CreateStockStationery(StationeryStockType.Blank)

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Sub SampleDocumentToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SampleDocumentToolStripMenuItem.Click

        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            If (CheckDirty() = DialogResult.Cancel) Then
                Return
            End If

            Open(Application.StartupPath & "\Sample\Sample.vdl")
            Text = "Project" & " - " & Application.StartupPath & "\Sample\Sample.vdl" & " - " & TITLE

            panelEx1.Visible = False
            rulerControl1.Visible = True
            rulerControl2.Visible = True
            ScribbleBox1.BringToFront()

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    'Private Sub CustomDesignerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomDesignerToolStripMenuItem.Click

    '    '<EhHeader>
    '    Try
    '        '</EhHeader>
    '        Application.DoEvents()

    '        Dim sbuilder As New Form1
    '        sbuilder.ShowDialog()
    '        sbuilder.Dispose()
    '        sbuilder = Nothing

    '        '<EhFooter>
    '    Catch
    '    End Try '</EhFooter>

    'End Sub

    Private Sub CustomToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomToolStripMenuItem.Click
        Application.DoEvents()
        openFileDialog1.Filter = "NuGenVizDOC Background Files (*.xml)|*.xml"
        openFileDialog1.InitialDirectory = Application.StartupPath & "\Backgrounds"
        openFileDialog1.ShowDialog()

        If (openFileDialog1.FileName.Length > 0) Then

            Dim filein As New FileStream(openFileDialog1.FileName, FileMode.Open)
            ScribbleBox1.Stationery.Load(filein)
            ScribbleBox1.Scribble.Stationery = New Stationery
            ScribbleBox1.Scribble.Stationery.Load(filein)
            ScribbleBox1.Scribble.Refresh()
            filein = Nothing
        End If

    End Sub

    Private Sub SubmitABugToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SubmitABugToolStripMenuItem.Click

        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()
            'MenuStrip1.Enabled = False
            EditToolStripMenuItem.Enabled = False
            InsertToolStripMenuItem.Enabled = False
            PaperToolStripMenuItem.Enabled = False
            FileToolStripMenuItem.Enabled = False
            Me.uiPanel0.Enabled = False
            Me.uiPanel2.Enabled = False
            Me.rulerControl1.Hide()
            Me.rulerControl2.Hide()
            Me.webBrowser1.Visible = True
            Me.webBrowser1.BringToFront()
            Me.uIBReturnTo.Visible = True
            Me.uIBReturnTo.Dock = DockStyle.Top
            Me.UiButton1.BringToFront()
            Me.webBrowser1.Url = New System.Uri("http://www.genetibase.com/featuresubmission.php", System.UriKind.Absolute)

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    'Private Sub SubmitAFeatureToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '    '<EhHeader>
    '    Try
    '        '</EhHeader>
    '        Application.DoEvents()
    '        'MenuStrip1.Enabled = False
    '        EditToolStripMenuItem.Enabled = False
    '        InsertToolStripMenuItem.Enabled = False
    '        PaperToolStripMenuItem.Enabled = False
    '        FileToolStripMenuItem.Enabled = False
    '        Me.uiPanel0.Enabled = False
    '        Me.uiPanel2.Enabled = False
    '        Me.rulerControl1.Hide()
    '        Me.rulerControl2.Hide()
    '        Me.webBrowser1.Visible = True
    '        Me.webBrowser1.BringToFront()
    '        Me.uIBReturnTo.Visible = True
    '        Me.uIBReturnTo.Dock = DockStyle.Top
    '        Me.UiButton1.BringToFront()
    '        Me.webBrowser1.Url = New System.Uri("http://www.genetibase.com/bugsubmission.php", System.UriKind.Absolute)

    '        '<EhFooter>
    '    Catch
    '    End Try '</EhFooter>

    'End Sub

    Private Sub CheckForUpdatesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckForUpdatesToolStripMenuItem.Click

        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()
            MsgBox("No Updates Available At This Time")

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Sub uIBReturnTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uIBReturnTo.Click

        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()
            'MenuStrip1.Enabled = False
            EditToolStripMenuItem.Enabled = True
            InsertToolStripMenuItem.Enabled = True
            PaperToolStripMenuItem.Enabled = True
            FileToolStripMenuItem.Enabled = True
            Me.uiPanel0.Enabled = True
            Me.uiPanel2.Enabled = True
            Me.webBrowser1.SendToBack()
            Me.rulerControl1.Show()
            Me.rulerControl2.Show()
            Me.webBrowser1.Visible = False
            Me.uIBReturnTo.SendToBack()
            Me.uIBReturnTo.Visible = False

            If panelEx1.Visible Then
                rulerControl1.Visible = False
                rulerControl2.Visible = False
                'MenuStrip1.Enabled = False
                EditToolStripMenuItem.Enabled = False
                InsertToolStripMenuItem.Enabled = False
                PaperToolStripMenuItem.Enabled = False
            End If

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub



    Private Sub frmNuGenPrim_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '<EhHeader>
        Try
            '</EhHeader>
            picPictureBox1.ImageLocation = Application.StartupPath & "\Resources\NuGenVizCap_LOGO_VEC.png"

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Sub ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem2.Click

        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()
            CheckDirty()
            rulerControl1.Visible = False
            rulerControl2.Visible = False
            ScribbleBox1.SendToBack()
            panelEx1.Visible = True

            Dim document As New Document
            Dim page As New Page(document, Stationery.CreateStockStationery(StationeryStockType.Blank))

            document.Pages.Add(page)
            ScribbleBox1.Scribble.Document = document
            Text = "NuGenVizDOC"

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub



    Private Sub llbNewProject_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            If (CheckDirty() = DialogResult.Cancel) Then
                Return
            End If

            'panelEx1.Visible = False
            rulerControl1.Visible = True
            rulerControl2.Visible = True
            'ScribbleBox1.BringToFront()
            NewFileName()

            Dim document As New Document
            Dim page As New Page(document, Stationery.CreateStockStationery(StationeryStockType.Blank))

            document.Pages.Add(page)
            ScribbleBox1.Scribble.Document = document
            panelEx1.Visible = False
            rulerControl1.Visible = True
            'rulerControl1.BringToFront()
            rulerControl2.Visible = True
            rulerControl2.BringToFront()
            ScribbleBox1.BringToFront()
            'MenuStrip1.Enabled = False
            EditToolStripMenuItem.Enabled = True
            InsertToolStripMenuItem.Enabled = True
            PaperToolStripMenuItem.Enabled = True
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub llbOpenProject_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            If (CheckDirty() = DialogResult.Cancel) Then
                Return
            End If

            Dim dialog As New OpenFileDialog
            dialog.Title = "Open " + TITLE + " File"
            dialog.CheckFileExists = True
            dialog.Filter() = TITLE + " files (*.vdl)|*.vdl|All files (*.*)|*.*"

            If dialog.ShowDialog() = DialogResult.OK Then
                Open(dialog.FileName)
                Me.Text = "Project" & " - " & dialog.FileName & " - " & TITLE

            Else
                Return
            End If

            panelEx1.Visible = False
            rulerControl1.Visible = True
            'rulerControl1.BringToFront()
            rulerControl2.Visible = True
            rulerControl2.BringToFront()
            ScribbleBox1.BringToFront()
            'MenuStrip1.Enabled = False
            EditToolStripMenuItem.Enabled = True
            InsertToolStripMenuItem.Enabled = True
            PaperToolStripMenuItem.Enabled = True
            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub llbImport_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            'If (CheckDirty() = DialogResult.Cancel) Then
            '    Return
            'End If
            Dim dialog As New OpenFileDialog
            dialog.Title = "Import File"
            dialog.CheckFileExists = True
            dialog.Filter = "Windows Journal Note (*.jnt)|*.jnt|Ink Serialized Format (*.isf)|*.isf|Image|*.bmp;*.gif;*.png;*.jpg;*.wmf"

            If dialog.ShowDialog() = DialogResult.OK Then
                Import(dialog.FileName, dialog.FilterIndex)

            Else
                Return
            End If

            panelEx1.Visible = False
            rulerControl1.Visible = True
            'rulerControl1.BringToFront()
            rulerControl2.Visible = True
            rulerControl2.BringToFront()
            ScribbleBox1.BringToFront()
            'MenuStrip1.Enabled = False
            EditToolStripMenuItem.Enabled = True
            InsertToolStripMenuItem.Enabled = True
            PaperToolStripMenuItem.Enabled = True
            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub frmNuGenPrim_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        '<EhHeader>
        Try
            '</EhHeader>
            TopMost = False
            'uiPanel0.AutoHide = True
            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    'Private Sub panelEx1_ChangeUICues(ByVal sender As Object, ByVal e As System.Windows.Forms.UICuesEventArgs) Handles panelEx1.ChangeUICues
    '    If panelEx1.Visible = True Then
    '        MenuStrip1.Enabled = False
    '    Else
    '        MenuStrip1.Enabled = True
    '    End If
    'End Sub



    Private Sub panelEx1_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelEx1.VisibleChanged
        '<EhHeader>
        Try
            '</EhHeader>
            If panelEx1.Visible = True Then
                'MenuStrip1.Enabled = False
                EditToolStripMenuItem.Enabled = False
                InsertToolStripMenuItem.Enabled = False
                PaperToolStripMenuItem.Enabled = False
            Else
                EditToolStripMenuItem.Enabled = True
                InsertToolStripMenuItem.Enabled = True
                PaperToolStripMenuItem.Enabled = True
            End If
            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub



    Private Sub ButtonItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            If (CheckDirty() = DialogResult.Cancel) Then
                Return
            End If

            'panelEx1.Visible = False
            rulerControl1.Visible = True
            rulerControl2.Visible = True
            'ScribbleBox1.BringToFront()
            NewFileName()

            Dim document As New Document
            Dim page As New Page(document, Stationery.CreateStockStationery(StationeryStockType.Blank))

            document.Pages.Add(page)
            ScribbleBox1.Scribble.Document = document
            panelEx1.Visible = False
            rulerControl1.Visible = True
            'rulerControl1.BringToFront()
            rulerControl2.Visible = True
            rulerControl2.BringToFront()
            ScribbleBox1.BringToFront()
            'MenuStrip1.Enabled = True
            EditToolStripMenuItem.Enabled = True
            InsertToolStripMenuItem.Enabled = True
            PaperToolStripMenuItem.Enabled = True
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub ButtonItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            If (CheckDirty() = DialogResult.Cancel) Then
                Return
            End If

            Dim dialog As New OpenFileDialog
            dialog.Title = "Open " + TITLE + " File"
            dialog.CheckFileExists = True
            dialog.Filter() = TITLE + " files (*.vdl)|*.vdl|All files (*.*)|*.*"

            If dialog.ShowDialog() = DialogResult.OK Then
                Open(dialog.FileName)
                Me.Text = "Project" & " - " & dialog.FileName & " - " & TITLE

            Else
                Return
            End If

            panelEx1.Visible = False
            rulerControl1.Visible = True
            'rulerControl1.BringToFront()
            rulerControl2.Visible = True
            rulerControl2.BringToFront()
            ScribbleBox1.BringToFront()
            'MenuStrip1.Enabled = True
            EditToolStripMenuItem.Enabled = True
            InsertToolStripMenuItem.Enabled = True
            PaperToolStripMenuItem.Enabled = True
            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub ButtonItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            'If (CheckDirty() = DialogResult.Cancel) Then
            '    Return
            'End If
            Dim dialog As New OpenFileDialog
            dialog.Title = "Import File"
            dialog.CheckFileExists = True
            dialog.Filter = "Windows Journal Note (*.jnt)|*.jnt|Ink Serialized Format (*.isf)|*.isf|Image|*.bmp;*.gif;*.png;*.jpg;*.wmf"

            If dialog.ShowDialog() = DialogResult.OK Then
                Import(dialog.FileName, dialog.FilterIndex)

            Else
                Return
            End If

            panelEx1.Visible = False
            rulerControl1.Visible = True
            'rulerControl1.BringToFront()
            rulerControl2.Visible = True
            rulerControl2.BringToFront()
            ScribbleBox1.BringToFront()
            'MenuStrip1.Enabled = True
            EditToolStripMenuItem.Enabled = True
            InsertToolStripMenuItem.Enabled = True
            PaperToolStripMenuItem.Enabled = True
            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    'Private Sub ButtonItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    '<EhHeader>
    '    Try
    '        '</EhHeader>
    '        Application.DoEvents()

    '        Dim aboutdia As New Genetibase.UI.NuGenAbout.AboutBox
    '        aboutdia.ShowDialog()
    '        aboutdia.Dispose()
    '        aboutdia = Nothing

    '        '<EhFooter>
    '    Catch
    '    End Try '</EhFooter>
    'End Sub

    Private Sub ButtonItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()
            'MenuStrip1.Enabled = False
            EditToolStripMenuItem.Enabled = False
            InsertToolStripMenuItem.Enabled = False
            PaperToolStripMenuItem.Enabled = False
            Me.uiPanel0.Enabled = False
            Me.uiPanel2.Enabled = False
            Me.rulerControl1.Hide()
            Me.rulerControl2.Hide()
            Me.webBrowser1.Visible = True
            Me.webBrowser1.BringToFront()
            Me.uIBReturnTo.Visible = True
            Me.uIBReturnTo.Dock = DockStyle.Top
            Me.UiButton1.BringToFront()
            Me.webBrowser1.Url = New System.Uri("http://www.genetibase.com/featuresubmission.php", System.UriKind.Absolute)

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Sub ButtonItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()
            'MenuStrip1.Enabled = False
            EditToolStripMenuItem.Enabled = False
            InsertToolStripMenuItem.Enabled = False
            PaperToolStripMenuItem.Enabled = False
            Me.uiPanel0.Enabled = False
            Me.uiPanel2.Enabled = False
            Me.rulerControl1.Hide()
            Me.rulerControl2.Hide()
            Me.webBrowser1.Visible = True
            Me.webBrowser1.BringToFront()
            Me.uIBReturnTo.Visible = True
            Me.uIBReturnTo.Dock = DockStyle.Top
            Me.UiButton1.BringToFront()
            Me.webBrowser1.Url = New System.Uri("http://www.genetibase.com/bugsubmission.php", System.UriKind.Absolute)

            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub ButtonItem10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            If (CheckDirty() = DialogResult.Cancel) Then
                Return
            End If

            Open(Application.StartupPath & "\Sample\Sample.vdl")
            Text = "Project" & " - " & Application.StartupPath & "\Sample\Sample.vdl" & " - " & TITLE

            panelEx1.Visible = False
            rulerControl1.Visible = True
            rulerControl2.Visible = True
            ScribbleBox1.BringToFront()

            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub llbNewProject_LinkClicked_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            If (CheckDirty() = DialogResult.Cancel) Then
                Return
            End If



            NewFileName()

            Dim document As New Document
            Dim page As New Page(document, Stationery.CreateStockStationery(StationeryStockType.Blank))

            document.Pages.Add(page)
            ScribbleBox1.Scribble.Document = document


            panelEx1.Visible = False
            rulerControl1.Visible = True
            rulerControl2.Visible = True
            ScribbleBox1.BringToFront()
            'MenuStrip1.Enabled = True
            EditToolStripMenuItem.Enabled = True
            InsertToolStripMenuItem.Enabled = True
            PaperToolStripMenuItem.Enabled = True
            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub llbOpenProject_LinkClicked_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            If (CheckDirty() = DialogResult.Cancel) Then
                Return
            End If

            Dim dialog As New OpenFileDialog
            dialog.Title = "Open " + TITLE + " File"
            dialog.CheckFileExists = True
            dialog.Filter() = TITLE + " files (*.vdl)|*.vdl|All files (*.*)|*.*"

            If dialog.ShowDialog() = DialogResult.OK Then
                Open(dialog.FileName)
                Me.Text = "Project" & " - " & dialog.FileName & " - " & TITLE

            Else
                Return
            End If

            panelEx1.Visible = False
            rulerControl1.Visible = True
            'rulerControl1.BringToFront()
            rulerControl2.Visible = True
            rulerControl2.BringToFront()
            ScribbleBox1.BringToFront()
            'MenuStrip1.Enabled = True
            EditToolStripMenuItem.Enabled = True
            InsertToolStripMenuItem.Enabled = True
            PaperToolStripMenuItem.Enabled = True
            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub llbImport_LinkClicked_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            'If (CheckDirty() = DialogResult.Cancel) Then
            '    Return
            'End If
            Dim dialog As New OpenFileDialog
            dialog.Title = "Import File"
            dialog.CheckFileExists = True
            dialog.Filter = "Windows Journal Note (*.jnt)|*.jnt|Ink Serialized Format (*.isf)|*.isf|Image|*.bmp;*.gif;*.png;*.jpg;*.wmf"

            If dialog.ShowDialog() = DialogResult.OK Then
                Import(dialog.FileName, dialog.FilterIndex)

            Else
                Return
            End If


            panelEx1.Visible = False
            rulerControl1.Visible = True
            rulerControl2.Visible = True
            ScribbleBox1.BringToFront()
            'MenuStrip1.Enabled = True
            EditToolStripMenuItem.Enabled = True
            InsertToolStripMenuItem.Enabled = True
            PaperToolStripMenuItem.Enabled = True
            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub llbOpenSample_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            If (CheckDirty() = DialogResult.Cancel) Then
                Return
            End If

            Open(Application.StartupPath & "\Sample\Sample.vdl")
            Text = "Project" & " - " & Application.StartupPath & "\Sample\Sample.vdl" & " - " & TITLE

            panelEx1.Visible = False
            rulerControl1.Visible = True
            rulerControl2.Visible = True
            ScribbleBox1.BringToFront()
            'MenuStrip1.Enabled = True
            EditToolStripMenuItem.Enabled = True
            InsertToolStripMenuItem.Enabled = True
            PaperToolStripMenuItem.Enabled = True

            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub llbAbout_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()

            Dim aboutdia As New Genetibase.UI.NuGenAbout.AboutBox
            aboutdia.ShowDialog()
            aboutdia.Dispose()
            aboutdia = Nothing

            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub llbCaptureTutorial_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Process.Start(Application.StartupPath & "\Resources\NuGenVizDOC_Ct.EXE")
            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub llbShapeGesture_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Process.Start(Application.StartupPath & "\Resources\NuGenVizDOC_SGT.EXE")
            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub llbActiveCapture_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Process.Start(Application.StartupPath & "\Resources\NuGenVizDOC_ACPT.EXE")
            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub llbSubmitA_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()
            'MenuStrip1.Enabled = False
            EditToolStripMenuItem.Enabled = False
            InsertToolStripMenuItem.Enabled = False
            PaperToolStripMenuItem.Enabled = False
            Me.uiPanel0.Enabled = False
            Me.uiPanel2.Enabled = False
            Me.rulerControl1.Hide()
            Me.rulerControl2.Hide()
            Me.webBrowser1.Visible = True
            Me.webBrowser1.BringToFront()
            Me.uIBReturnTo.Visible = True
            Me.uIBReturnTo.Dock = DockStyle.Top
            Me.UiButton1.BringToFront()
            Me.webBrowser1.Url = New System.Uri("http://www.genetibase.com/featuresubmission.php?page=nugenvizdoc", System.UriKind.Absolute)

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Sub llbSubmitA1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()
            'MenuStrip1.Enabled = False
            EditToolStripMenuItem.Enabled = False
            InsertToolStripMenuItem.Enabled = False
            PaperToolStripMenuItem.Enabled = False
            Me.uiPanel0.Enabled = False
            Me.uiPanel2.Enabled = False
            Me.rulerControl1.Hide()
            Me.rulerControl2.Hide()
            Me.webBrowser1.Visible = True
            Me.webBrowser1.BringToFront()
            Me.uIBReturnTo.Visible = True
            Me.uIBReturnTo.Dock = DockStyle.Top
            Me.UiButton1.BringToFront()
            Me.webBrowser1.Url = New System.Uri("http://www.genetibase.com/bugsubmission.php?page=nugenvizdoc", System.UriKind.Absolute)

            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        Try
            Application.Exit()
        Catch
        End Try
    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        '<EhHeader>
        Try
            '</EhHeader>
            Process.Start(Application.StartupPath & "\Resources\NuGenVizDOC_SPET.EXE")
            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub



    Private Sub SaveAsExportingTutorialToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsExportingTutorialToolStripMenuItem.Click
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()
            Process.Start(Application.StartupPath & "\Resources\NuGenVizDOC_SPET.EXE")
            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub CaptureTutorialToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CaptureTutorialToolStripMenuItem.Click
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()
            Process.Start(Application.StartupPath & "\Resources\NuGenVizDOC_Ct.EXE")
            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub ShapeGestureTutorialToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShapeGestureTutorialToolStripMenuItem.Click
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()
            Process.Start(Application.StartupPath & "\Resources\NuGenVizDOC_SGT.EXE")
            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub

    Private Sub DragAndDropActiveCapturePaletteTutorialToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DragAndDropActiveCapturePaletteTutorialToolStripMenuItem.Click
        '<EhHeader>
        Try
            '</EhHeader>
            Application.DoEvents()
            Process.Start(Application.StartupPath & "\Resources\NuGenVizDOC_ACPT.EXE")
            '<EhFooter>
        Catch
        End Try '</EhFooter>
    End Sub


End Class
