Imports Genetibase.Shared.Controls
Imports System.Windows.Forms

Public Class MainForm
    Private Sub _commandManager_ApplicationCommandUpdate(ByVal sender As System.Object, ByVal e As NuGenApplicationCommandEventArgs) Handles _commandManager.ApplicationCommandUpdate
        Dim owner As Control = CType(sender, Control)
        Dim Command As NuGenApplicationCommand = e.ApplicationCommand
        Dim item As ToolStripItem = CType(e.Item, ToolStripItem)

        Select Case Command.ApplicationCommandName
            Case "New"
                e.ApplicationCommand.Enabled = _enableNewCheckBox.Checked
            Case "FileCopy"
                e.ApplicationCommand.Enabled = _enableGlobalCopyCheckBox.Checked
            Case "ContextCopy"
                e.ApplicationCommand.Enabled = _enableLocalCopyCheckBox.Checked
            Case "Save"
                e.ApplicationCommand.Visible = _showSaveCheckBox.Checked
            Case "TrackBar"
                _progressBar.Value = _trackBar.Value
        End Select
    End Sub

    Sub New()
        Me.InitializeComponent()
        Me.SetStyle(ControlStyles.Opaque, True)
    End Sub
End Class
