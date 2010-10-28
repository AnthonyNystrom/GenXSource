Public Class NuGenAlgerbicEquation_mainForm

   

    Private Sub BackToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub BisectionMethodsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BisectionMethodsToolStripMenuItem.Click
        Dim next_form As New NuGenEquationRoots
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ComplexMethodsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComplexMethodsToolStripMenuItem.Click
        Dim next_form As New NuGenRootsByComplexMethodMainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub NumericUseOfSeriesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUseOfSeriesToolStripMenuItem.Click
        Dim next_form As New NuGenNumericUseOfSeries_MainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub PolynomialToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PolynomialToolStripMenuItem.Click
        Dim next_form As New NuGenPolynomial_AdditionForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
End Class