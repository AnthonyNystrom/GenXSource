Public Class NuGenSeries_ChiSqureForm

    Private Sub SeriesMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeriesMenuToolStripMenuItem.Click
        Dim next_form As New NuGenNumericUseOfSeries_MainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        degree.Text = ""
        x_val.Text = ""
        acc_para.Text = ""
        res_y.Text = ""

        degree.Focus()

    End Sub

    Private Sub Chi_square_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chi_square.Click
        Dim y As Decimal
        If degree.Text = "" Or x_val.Text = "" Or acc_para.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            Genetibase.MathX.NuGenAlgebricEquations.NuGenNumericalUseOfSeries.CHISQ(Val(degree.Text), Val(x_val.Text), Val(acc_para.Text), y)
            res_y.Text = y.ToString
        End If


    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub AlgebricToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlgebricToolStripMenuItem.Click
        Dim next_form As New NuGenAlgerbicEquation_mainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class