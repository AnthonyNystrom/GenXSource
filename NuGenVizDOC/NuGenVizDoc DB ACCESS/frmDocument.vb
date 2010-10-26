Public Class frmDocument
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

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
    Friend WithEvents ScribbleBox1 As Agilix.Ink.Scribble.ScribbleBox
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDocument))
        Me.ScribbleBox1 = New Agilix.Ink.Scribble.ScribbleBox
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
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
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ScribbleBox1
        '
        Me.ScribbleBox1.BackColor = System.Drawing.SystemColors.Control
        Me.ScribbleBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ScribbleBox1.ForceSwapFont = New System.Drawing.Font("Times New Roman", 10.0!)
        Me.ScribbleBox1.HighlightElementColor = System.Drawing.Color.FromArgb(CType(CType(189, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.ScribbleBox1.Location = New System.Drawing.Point(0, 25)
        Me.ScribbleBox1.Name = "ScribbleBox1"
        Me.ScribbleBox1.Size = New System.Drawing.Size(557, 436)
        Me.ScribbleBox1.TabColor = System.Drawing.SystemColors.ControlDark
        Me.ScribbleBox1.TabIndex = 0
        Me.ScribbleBox1.Text = "ScribbleBox1"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripButton, Me.OpenToolStripButton, Me.SaveToolStripButton, Me.PrintToolStripButton, Me.toolStripSeparator, Me.CutToolStripButton, Me.CopyToolStripButton, Me.PasteToolStripButton, Me.toolStripSeparator1, Me.HelpToolStripButton})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(557, 25)
        Me.ToolStrip1.TabIndex = 2
        Me.ToolStrip1.Text = "ToolStrip1"
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
        'frmDocument
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(557, 461)
        Me.Controls.Add(Me.ScribbleBox1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "frmDocument"
        Me.Text = "frmDocument"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public DocumentChanged As Boolean = False
    Public FileName As String = ""

    'Public Sub ExecuteCommand(ByVal cmd As String, ByVal data As Object)
    '    Select Case cmd
    '        ' Edit Menu
    '        Case "buttonUndo"
    '            rtfText.Undo()
    '        Case "buttonCut"
    '            rtfText.Cut()
    '        Case "buttonCopy"
    '            rtfText.Copy()
    '        Case "buttonPaste"
    '            rtfText.Paste()
    '        Case "buttonDelete"
    '            rtfText.SelectedText = ""
    '        Case "buttonSelectAll"
    '            rtfText.SelectAll()
    '        Case "buttonFind"
    '            If Not data Is Nothing Then
    '                Dim searchText As String = data.ToString()
    '                rtfText.Find(searchText, rtfText.SelectionStart + rtfText.SelectionLength, RichTextBoxFinds.None)
    '            End If
    '        Case "buttonFindNext"
    '            MessageBox.Show("Not implemented yet.")
    '        Case "buttonReplace"
    '            MessageBox.Show("Not implemented yet.")
    '            ' Format menu
    '        Case "buttonFontBold"
    '            If rtfText.SelectionFont.Bold Then
    '                rtfText.SelectionFont = New Font(rtfText.SelectionFont, (rtfText.SelectionFont.Style And Not (rtfText.SelectionFont.Style And FontStyle.Bold)))
    '            Else
    '                rtfText.SelectionFont = New Font(rtfText.SelectionFont, (rtfText.SelectionFont.Style Or FontStyle.Bold))
    '            End If
    '        Case "buttonFontItalic"
    '            If rtfText.SelectionFont.Italic Then
    '                rtfText.SelectionFont = New Font(rtfText.SelectionFont, (rtfText.SelectionFont.Style And Not (rtfText.SelectionFont.Style And FontStyle.Italic)))
    '            Else
    '                rtfText.SelectionFont = New Font(rtfText.SelectionFont, (rtfText.SelectionFont.Style Or FontStyle.Italic))
    '            End If
    '        Case "buttonFontUnderline"
    '            If rtfText.SelectionFont.Underline Then
    '                rtfText.SelectionFont = New Font(rtfText.SelectionFont, (rtfText.SelectionFont.Style And Not (rtfText.SelectionFont.Style And FontStyle.Underline)))
    '            Else
    '                rtfText.SelectionFont = New Font(rtfText.SelectionFont, (rtfText.SelectionFont.Style Or FontStyle.Underline))
    '            End If
    '        Case "buttonFontStrike"
    '            If rtfText.SelectionFont.Strikeout Then
    '                rtfText.SelectionFont = New Font(rtfText.SelectionFont, (rtfText.SelectionFont.Style And Not (rtfText.SelectionFont.Style And FontStyle.Strikeout)))
    '            Else
    '                rtfText.SelectionFont = New Font(rtfText.SelectionFont, (rtfText.SelectionFont.Style Or FontStyle.Strikeout))
    '            End If
    '        Case "buttonAlignLeft"
    '            rtfText.SelectionAlignment = HorizontalAlignment.Left
    '        Case "buttonAlignCenter"
    '            rtfText.SelectionAlignment = HorizontalAlignment.Center
    '        Case "buttonAlignRight"
    '            rtfText.SelectionAlignment = HorizontalAlignment.Right
    '        Case "buttonTextColor"
    '            If Not data Is Nothing And TypeOf (data) Is Color Then
    '                rtfText.SelectionColor = CType(data, Color)
    '            End If
    '        Case "comboFont"
    '            Dim combo As DevComponents.DotNetBar.ComboBoxItem = CType(data, DevComponents.DotNetBar.ComboBoxItem)
    '            If Not combo.SelectedItem Is Nothing Then
    '                Dim f As Font = New Font(combo.SelectedItem.ToString(), rtfText.SelectionFont.Size)
    '                rtfText.SelectionFont = f
    '            End If
    '        Case "comboFontSize"
    '            Dim combo As DevComponents.DotNetBar.ComboBoxItem = CType(data, DevComponents.DotNetBar.ComboBoxItem)
    '            If Not combo.SelectedItem Is Nothing Then
    '                Dim f As Font = New Font(rtfText.SelectionFont.Name, Int32.Parse(combo.SelectedItem.ToString()))
    '                rtfText.SelectionFont = f
    '            End If
    '    End Select
    '    DocumentChanged = True
    '    EnableSelectionItems()
    'End Sub

    Private Sub frmDocument_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        Dim mdi As frmMain = CType(Me.ParentForm, frmMain)
        mdi.labelPosition.Text = ""
        DisableDocMenuItems()
    End Sub

    'Private Sub frmDocument_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
    '    If Me.DocumentChanged Then
    '        Dim dlg As DialogResult = MessageBox.Show(Me, "Document '" + Me.FileName + "' has changed. Save changes?", "Notepad", MessageBoxButtons.YesNoCancel)
    '        If dlg = DialogResult.Cancel Then
    '            e.Cancel = True
    '        ElseIf dlg = DialogResult.Yes Then
    '            If Me.FileName = "" Then CType(Me.MdiParent, frmMain).SaveDocument(Me)
    '            If Me.FileName <> "" Then SaveFile()
    '        End If
    '    End If
    'End Sub

    Private Sub frmDocument_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        EnableDocMenuItems()
        'UpdateStatusBar()
    End Sub

    Private Sub frmDocument_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Deactivate
        DisableDocMenuItems()
    End Sub

    Private Sub frmDocument_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged
        If Me.Visible Then
            EnableDocMenuItems()
        Else
            DisableDocMenuItems()
        End If
    End Sub

    'Private Sub rtfText_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    EnableSelectionItems()
    'End Sub

    Private Sub rtfText_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        DocumentChanged = True
    End Sub

    Private Sub rtfText_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If e.Button <> Windows.Forms.MouseButtons.Right Then Exit Sub

        Dim mdi As frmMain = CType(Me.ParentForm, frmMain)
        mdi.EditContextMenu()
    End Sub

    'Private Sub UpdateStatusBar()
    '    Dim mdi As frmMain = CType(Me.ParentForm, frmMain)
    '    mdi.labelPosition.Text = "Ln " + (rtfText.GetLineFromCharIndex(rtfText.SelectionStart) + 1).ToString() + Chr(9) + "Ch " + rtfText.SelectionStart.ToString()
    'End Sub

    'Private Sub EnableSelectionItems()
    '    Dim documentUI As IDocumentUI = CType(Me.ParentForm, IDocumentUI)

    '    UpdateStatusBar()

    '    If rtfText.SelectionLength = 0 Then
    '        documentUI.CutEnabled = False
    '        documentUI.CopyEnabled = False
    '        documentUI.DeleteEnabled = False
    '    Else
    '        documentUI.CutEnabled = True
    '        documentUI.CopyEnabled = True
    '        documentUI.DeleteEnabled = True
    '    End If

    '    documentUI.BoldChecked = rtfText.SelectionFont.Bold
    '    documentUI.ItalicChecked = rtfText.SelectionFont.Italic
    '    documentUI.UnderlineChecked = rtfText.SelectionFont.Underline
    '    documentUI.StrikethroughChecked = rtfText.SelectionFont.Strikeout
    '    documentUI.AlignLeftChecked = (rtfText.SelectionAlignment = HorizontalAlignment.Left)
    '    documentUI.AlignRightChecked = (rtfText.SelectionAlignment = HorizontalAlignment.Right)
    '    documentUI.AlignCenterChecked = (rtfText.SelectionAlignment = HorizontalAlignment.Center)
    '    If Not rtfText.SelectionFont Is Nothing Then
    '        documentUI.SetSelectionFont(rtfText.SelectionFont)
    '    End If
    '    documentUI.FontSize = rtfText.SelectionFont.Size
    'End Sub

    Private Sub EnableDocMenuItems()
        If Not Me.Visible Then Exit Sub

        Dim documentUI As IDocumentUI = CType(Me.ParentForm, IDocumentUI)

        ' Disabled Edit items
        documentUI.PasteEnabled = True
        documentUI.SelectAllEnabled = True
        documentUI.FindEnabled = True
        documentUI.FindNextEnabled = True
        documentUI.ReplaceEnabled = True

        ' Disable Format items
        documentUI.AlignLeftEnabled = True
        documentUI.AlignRightEnabled = True
        documentUI.AlignCenterEnabled = True
        documentUI.BoldEnabled = True
        documentUI.ItalicEnabled = True
        documentUI.UnderlineEnabled = True
        documentUI.StrikethroughEnabled = True
        documentUI.TextColorEnabled = True
        documentUI.FontSelectionEnabled = True
        documentUI.FontSizeEnabled = True

        'EnableSelectionItems()
    End Sub

    Private Sub DisableDocMenuItems()
        Dim documentUI As IDocumentUI = CType(Me.ParentForm, IDocumentUI)

        ' Disabled Edit items
        documentUI.CutEnabled = False
        documentUI.CopyEnabled = False
        documentUI.PasteEnabled = False
        documentUI.DeleteEnabled = False
        documentUI.SelectAllEnabled = False
        documentUI.FindEnabled = False
        documentUI.FindNextEnabled = False
        documentUI.ReplaceEnabled = False

        ' Disable Format items
        documentUI.AlignLeftEnabled = False
        documentUI.AlignRightEnabled = False
        documentUI.AlignCenterEnabled = False
        documentUI.BoldEnabled = False
        documentUI.ItalicEnabled = False
        documentUI.UnderlineEnabled = False
        documentUI.StrikethroughEnabled = False
        documentUI.TextColorEnabled = False
        documentUI.FontSelectionEnabled = False
        documentUI.FontSizeEnabled = False
    End Sub

    'Public Sub OpenFile(ByVal filename As String)
    '    If filename.Substring(filename.Length - 4, 4).ToLower() = ".txt" Then
    '        rtfText.LoadFile(filename, RichTextBoxStreamType.PlainText)
    '    Else
    '        rtfText.LoadFile(filename, RichTextBoxStreamType.RichText)
    '    End If
    '    Me.FileName = filename
    '    DocumentChanged = False
    'End Sub
    'Public Sub SaveFile()
    '    If Me.FileName.Substring(Me.FileName.Length - 4, 4).ToLower() = ".txt" Then
    '        rtfText.SaveFile(Me.FileName, RichTextBoxStreamType.PlainText)
    '    Else
    '        rtfText.SaveFile(Me.FileName, RichTextBoxStreamType.RichText)
    '    End If
    '    DocumentChanged = False
    'End Sub

End Class
