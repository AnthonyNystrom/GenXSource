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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents rtfText As System.Windows.Forms.RichTextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.rtfText = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'rtfText
        '
        Me.rtfText.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtfText.HideSelection = False
        Me.rtfText.Name = "rtfText"
        Me.rtfText.Size = New System.Drawing.Size(217, 154)
        Me.rtfText.TabIndex = 0
        Me.rtfText.Text = ""
        '
        'frmDocument
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(217, 154)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.rtfText})
        Me.Name = "frmDocument"
        Me.Text = "frmDocument"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public DocumentChanged As Boolean = False
    Public FileName As String = ""

    Public Sub ExecuteCommand(ByVal cmd As String, ByVal data As Object)
        Select Case cmd
            ' Edit Menu
        Case "buttonUndo"
                rtfText.Undo()
            Case "buttonCut"
                rtfText.Cut()
            Case "buttonCopy"
                rtfText.Copy()
            Case "buttonPaste"
                rtfText.Paste()
            Case "buttonDelete"
                rtfText.SelectedText = ""
            Case "buttonSelectAll"
                rtfText.SelectAll()
            Case "buttonFind"
                If Not data Is Nothing Then
                    Dim searchText As String = data.ToString()
                    rtfText.Find(searchText, rtfText.SelectionStart + rtfText.SelectionLength, RichTextBoxFinds.None)
                End If
            Case "buttonFindNext"
                MessageBox.Show("Not implemented yet.")
            Case "buttonReplace"
                MessageBox.Show("Not implemented yet.")
                ' Format menu
            Case "buttonFontBold"
                If rtfText.SelectionFont.Bold Then
                    rtfText.SelectionFont = New Font(rtfText.SelectionFont, (rtfText.SelectionFont.Style And Not (rtfText.SelectionFont.Style And FontStyle.Bold)))
                Else
                    rtfText.SelectionFont = New Font(rtfText.SelectionFont, (rtfText.SelectionFont.Style Or FontStyle.Bold))
                End If
            Case "buttonFontItalic"
                If rtfText.SelectionFont.Italic Then
                    rtfText.SelectionFont = New Font(rtfText.SelectionFont, (rtfText.SelectionFont.Style And Not (rtfText.SelectionFont.Style And FontStyle.Italic)))
                Else
                    rtfText.SelectionFont = New Font(rtfText.SelectionFont, (rtfText.SelectionFont.Style Or FontStyle.Italic))
                End If
            Case "buttonFontUnderline"
                If rtfText.SelectionFont.Underline Then
                    rtfText.SelectionFont = New Font(rtfText.SelectionFont, (rtfText.SelectionFont.Style And Not (rtfText.SelectionFont.Style And FontStyle.Underline)))
                Else
                    rtfText.SelectionFont = New Font(rtfText.SelectionFont, (rtfText.SelectionFont.Style Or FontStyle.Underline))
                End If
            Case "buttonFontStrike"
                If rtfText.SelectionFont.Strikeout Then
                    rtfText.SelectionFont = New Font(rtfText.SelectionFont, (rtfText.SelectionFont.Style And Not (rtfText.SelectionFont.Style And FontStyle.Strikeout)))
                Else
                    rtfText.SelectionFont = New Font(rtfText.SelectionFont, (rtfText.SelectionFont.Style Or FontStyle.Strikeout))
                End If
            Case "buttonAlignLeft"
                rtfText.SelectionAlignment = HorizontalAlignment.Left
            Case "buttonAlignCenter"
                rtfText.SelectionAlignment = HorizontalAlignment.Center
            Case "buttonAlignRight"
                rtfText.SelectionAlignment = HorizontalAlignment.Right
            Case "buttonTextColor"
                If Not data Is Nothing And TypeOf (data) Is Color Then
                    rtfText.SelectionColor = CType(data, Color)
                End If
            Case "comboFont"
                Dim combo As DevComponents.DotNetBar.ComboBoxItem = CType(data, DevComponents.DotNetBar.ComboBoxItem)
                If Not combo.SelectedItem Is Nothing Then
                    Dim f As Font = New Font(combo.SelectedItem.ToString(), rtfText.SelectionFont.Size)
                    rtfText.SelectionFont = f
                End If
            Case "comboFontSize"
                Dim combo As DevComponents.DotNetBar.ComboBoxItem = CType(data, DevComponents.DotNetBar.ComboBoxItem)
                If Not combo.SelectedItem Is Nothing Then
                    Dim f As Font = New Font(rtfText.SelectionFont.Name, Int32.Parse(combo.SelectedItem.ToString()))
                    rtfText.SelectionFont = f
                End If
        End Select
        DocumentChanged = True
        EnableSelectionItems()
    End Sub

    Private Sub frmDocument_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        Dim mdi As frmMain = CType(Me.ParentForm, frmMain)
        mdi.labelPosition.Text = ""
        DisableDocMenuItems()
    End Sub

    Private Sub frmDocument_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Me.DocumentChanged Then
            Dim dlg As DialogResult = MessageBox.Show(Me, "Document '" + Me.FileName + "' has changed. Save changes?", "Notepad", MessageBoxButtons.YesNoCancel)
            If dlg = DialogResult.Cancel Then
                e.Cancel = True
            ElseIf dlg = DialogResult.Yes Then
                If Me.FileName = "" Then CType(Me.MdiParent, frmMain).SaveDocument(Me)
                If Me.FileName <> "" Then SaveFile()
            End If
        End If
    End Sub

    Private Sub frmDocument_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        EnableDocMenuItems()
        UpdateStatusBar()
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

    Private Sub rtfText_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rtfText.SelectionChanged
        EnableSelectionItems()
    End Sub

    Private Sub rtfText_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rtfText.TextChanged
        DocumentChanged = True
    End Sub

    Private Sub rtfText_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles rtfText.MouseDown
        If e.Button <> MouseButtons.Right Then Exit Sub

        Dim mdi As frmMain = CType(Me.ParentForm, frmMain)
        mdi.EditContextMenu()
    End Sub

    Private Sub UpdateStatusBar()
        Dim mdi As frmMain = CType(Me.ParentForm, frmMain)
        mdi.labelPosition.Text = "Ln " + (rtfText.GetLineFromCharIndex(rtfText.SelectionStart) + 1).ToString() + Chr(9) + "Ch " + rtfText.SelectionStart.ToString()
    End Sub

    Private Sub EnableSelectionItems()
        Dim documentUI As IDocumentUI = CType(Me.ParentForm, IDocumentUI)

        UpdateStatusBar()

        If rtfText.SelectionLength = 0 Then
            documentUI.CutEnabled = False
            documentUI.CopyEnabled = False
            documentUI.DeleteEnabled = False
        Else
            documentUI.CutEnabled = True
            documentUI.CopyEnabled = True
            documentUI.DeleteEnabled = True
        End If

        documentUI.BoldChecked = rtfText.SelectionFont.Bold
        documentUI.ItalicChecked = rtfText.SelectionFont.Italic
        documentUI.UnderlineChecked = rtfText.SelectionFont.Underline
        documentUI.StrikethroughChecked = rtfText.SelectionFont.Strikeout
        documentUI.AlignLeftChecked = (rtfText.SelectionAlignment = HorizontalAlignment.Left)
        documentUI.AlignRightChecked = (rtfText.SelectionAlignment = HorizontalAlignment.Right)
        documentUI.AlignCenterChecked = (rtfText.SelectionAlignment = HorizontalAlignment.Center)
        If Not rtfText.SelectionFont Is Nothing Then
            documentUI.SetSelectionFont(rtfText.SelectionFont)
        End If
        documentUI.FontSize = rtfText.SelectionFont.Size
    End Sub

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

        EnableSelectionItems()
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

    Public Sub OpenFile(ByVal filename As String)
        If filename.Substring(filename.Length - 4, 4).ToLower() = ".txt" Then
            rtfText.LoadFile(filename, RichTextBoxStreamType.PlainText)
        Else
            rtfText.LoadFile(filename, RichTextBoxStreamType.RichText)
        End If
        Me.FileName = filename
        DocumentChanged = False
    End Sub
    Public Sub SaveFile()
        If Me.FileName.Substring(Me.FileName.Length - 4, 4).ToLower() = ".txt" Then
            rtfText.SaveFile(Me.FileName, RichTextBoxStreamType.PlainText)
        Else
            rtfText.SaveFile(Me.FileName, RichTextBoxStreamType.RichText)
        End If
        DocumentChanged = False
    End Sub

End Class
