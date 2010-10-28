Imports Genetibase.MathX.NuGenMatrix

Public Class NuGen_operations_Menu

    Private Sub Button_next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_next.Click
        If (option_gauss.Checked) Then

            Dim next_form As New NuGen_Gauss
            Me.Visible = False
            next_form.Visible = True

        ElseIf (option_linear.Checked) Then

            Dim next_form As New NuGen_Linear_System_Solver
            Me.Visible = False
            next_form.Visible = True

        ElseIf (option_Vectors.Checked) Then

            Dim next_form As New NuGen_Vector_operations
            Me.Visible = False
            next_form.Visible = True

        ElseIf (option_matrix.Checked) Then

            Dim next_form As New NuGenMatrix_Math
            Me.Visible = False
            next_form.Visible = True

        ElseIf (option_function.Checked) Then

            Dim next_form As New NuGenFunctionApproximation_Main_Form
            Me.Visible = False
            next_form.Visible = True

        End If
    End Sub

    Private Sub exitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub option_matrix_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles option_matrix.CheckedChanged

    End Sub

    Private Sub NuGen_operations_Menu_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub mathematicalOperationsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class