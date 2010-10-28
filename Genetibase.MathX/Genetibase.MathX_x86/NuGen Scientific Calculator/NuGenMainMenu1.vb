Imports Genetibase.MathX.NuGenMatrixOperations
Imports Genetibase.MathX.NuGenStructures

Public Class NuGenMainMenu1

    Private Sub option_gauss_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles option_matrix.CheckedChanged

    End Sub

    Private Sub Button_next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_next.Click

        If (option_matrix.Checked) Then
            Dim next_form As New Genetibase.MathX.NuGenMatrixOperations.NuGen_operations_Menu
            Me.Visible = False
            next_form.Visible = True
        ElseIf (option_structures.Checked) Then
            Dim next_form As New Genetibase.MathX.NuGenStructures.NuGen_Structures_Main
            Me.Visible = False
            next_form.Visible = True
        End If


    End Sub
End Class