Imports Genetibase.MathX.NuGenTrignometricOperations

Public Class NuGenMainTrigForm

    Private Sub BinaryNotToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BinaryNotToolStripMenuItem.Click
        Dim Next_form As New NuGenTrigFunctions
        Me.Visible = False
        Next_form.Visible = True
    End Sub

    Private Sub BinaryAdditionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BinaryAdditionToolStripMenuItem.Click
        Dim Next_form As New NuGenTrignometricFunction
        Me.Visible = False
        Next_form.Visible = True

    End Sub

    Private Sub ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem2.Click
        Dim Next_form As New NuGenTrignometrys
        Me.Visible = False
        Next_form.Visible = True
    End Sub

    Private Sub NuGenMainTrigForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub BinaryAndToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class