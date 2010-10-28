Public Class NuGenFunctionApproximation_Main_Form

    Private Sub ChiSquareToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChiSquareToolStripMenuItem.Click
        Dim next_form As New NuGenFunctionApproximation_ChiSqureForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub LeastSquaresToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LeastSquaresToolStripMenuItem.Click
        Dim next_form As New NuGenFunctionApproximation_LeastSquareForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub LinearLeastSquareToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LinearLeastSquareToolStripMenuItem.Click
        Dim next_form As New NuGenFunctionApproximation_LinearLeastSquareForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ParabolicLeastSquareToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ParabolicLeastSquareToolStripMenuItem.Click
        Dim next_form As New NuGenFunctionApproximation_ParabolicLeastSquareForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ParametricLeastSquareToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ParametricLeastSquareToolStripMenuItem.Click
        Dim next_form As New NuGenFunctionApproximation_ParametricLeastSquareForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub PolynomialRegressionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PolynomialRegressionToolStripMenuItem.Click
        Dim next_form As New NuGenFunctionApproximation_PolynomialRegressionForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub SigmaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SigmaToolStripMenuItem.Click
        Dim next_form As New NuGenFunctionApproximation_SigmaForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub
End Class