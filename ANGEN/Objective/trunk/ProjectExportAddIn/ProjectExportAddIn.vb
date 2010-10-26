Option Strict On
Option Explicit On

Imports Genetibase.NuGenObjective
Imports Genetibase.NuGenObjective.Windows.DiagramClient
Imports System.Windows.Forms
Imports System.IO

Public Class ProjectExportAddIn
    Implements IClientAddin

    Private m_designer As IDesigner
    Private WithEvents m_menuItem As ToolStripItem

    Public ReadOnly Property Name() As String Implements IClientAddin.Name
        Get
            Return "Project Export Add-in"
        End Get
    End Property

    Public Sub Initialize(ByVal designer As IDesigner, ByVal rootAddinMenu As System.Windows.Forms.ToolStripMenuItem) Implements IClientAddin.Activate
        m_designer = designer
        m_menuItem = rootAddinMenu.DropDownItems.Add("Export to Microsoft Project...")
    End Sub

    Public Sub Deactivate(ByVal designer As IDesigner, ByVal rootAddinMenu As System.Windows.Forms.ToolStripMenuItem) Implements IClientAddin.Deactivate
        rootAddinMenu.DropDownItems.Remove(m_menuItem)
    End Sub

    Public Sub Invoke(ByVal client As ClientDiagram) Implements IClientAddin.Invoke
        Dim fileName As String = m_designer.SaveFileDialog(".mpx", "MPX Files|*.mpx")
        If fileName <> "" Then
            Dim fs As New FileStream(fileName, FileMode.Create)
            Dim sw As New StreamWriter(fs, Text.Encoding.ASCII)


            ' Write the MPX header
            sw.WriteLine("MPX, Genetibase Diagram Microsoft Project Export Add-in, 1.0, ANSI")

            ' Write Task fields
            ' We will write interaction names as task names,
            ' and a dummy duration
            sw.WriteLine("60, name, duration")

            ' Write tasks
            For Each obj As ModelObject In client.CurrentDiagram.System.Objects
                For Each int As Interaction In obj.Interactions
                    ' write a task
                    sw.WriteLine("70, {0}, 0d", int.Name)
                    ' Write its details as a task note
                    sw.WriteLine("71, ""Moves the object {0} into the state {1}. Can be performed by the role {2} through the action {3}""", int.Object.Name, int.State.Name, int.Role.Name, int.Action.Name)
                Next
            Next

            ' Flush and close file
            sw.Flush()
            sw.Close()
            MsgBox("Done")
        End If
    End Sub

    Private Sub m_menuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_menuItem.Click
        Invoke(m_designer.DiagramClient)
    End Sub
End Class
