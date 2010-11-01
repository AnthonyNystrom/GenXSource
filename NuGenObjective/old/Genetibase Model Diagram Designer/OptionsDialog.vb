Imports System.Windows.Forms
Imports System.ComponentModel
Imports Genetibase.NuGenObjective.Windows.DiagramClient
Imports Genetibase.NuGenObjective

Public Class OptionsDialog

    Private WithEvents m_source As BindingSource
    Private m_sourceIsBinding As Boolean
    Private m_designer As DiagramDesigner

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ' Activate/Deactivate addins
        For Each aStatus As AddinStatus In My.Application.Addins
            If aStatus.Active Then
                My.Application.ActivateAddin(aStatus, Designer, Designer.ToolsToolStripMenuItem)
            Else
                My.Application.DeactivateAddin(aStatus, Designer, Designer.ToolsToolStripMenuItem)
            End If
        Next
        ' Save settings
        My.Application.SaveCurrentUserActiveAddIns()
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        ' Reload settings
        My.Application.GetCurrentUserActiveAddIns()
        Me.Close()
    End Sub

    Private Sub OptionsDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set up BindingSource over Addins collection
        ' so that we can receive events when data changes
        m_sourceIsBinding = True
        m_source = New BindingSource
        m_source.DataSource = My.Application.Addins
        m_sourceIsBinding = False

        ' Bind DataGrid to BindingSource
        ' and set up columns
        With AddInsGrid
            .AutoGenerateColumns = True
            .DataSource = My.Application.Addins
            With .Columns("Name")
                .DisplayIndex = 0
                .ReadOnly = True
            End With
            With .Columns("Active")
                .DisplayIndex = 1
            End With
        End With
    End Sub

    'Private Sub m_source_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_source.CurrentItemChanged
    '    If Not m_sourceIsBinding Then
    '        Dim aStatus As AddinStatus = TryCast(m_source.Current, AddinStatus)
    '        If aStatus IsNot Nothing Then
    '            If aStatus.Active Then
    '                My.Application.ActivateAddin(aStatus, Designer, Designer.ToolsToolStripMenuItem)
    '            Else
    '                My.Application.DeactivateAddin(aStatus, Designer, Designer.ToolsToolStripMenuItem)
    '            End If
    '        End If
    '    End If
    'End Sub

    Public Property Designer() As DiagramDesigner
        Get
            Return m_designer
        End Get
        Set(ByVal value As DiagramDesigner)
            m_designer = value
        End Set
    End Property
End Class
