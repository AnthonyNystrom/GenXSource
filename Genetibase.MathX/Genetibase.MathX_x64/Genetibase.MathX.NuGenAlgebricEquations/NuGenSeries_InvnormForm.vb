Public Class NuGenSeries_InvnormForm

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        y_val.Text = ""
        x_val.Text = ""

        y_val.Focus()

    End Sub

    Private Sub Inverse_normal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Inverse_normal.Click
        Dim x As Decimal
        If y_val.Text = "" Then
            MsgBox("Please Enter The Value")
        Else
            Genetibase.MathX.NuGenAlgebricEquations.NuGenNumericalUseOfSeries.INVNORM(x, Val(y_val.Text))

            x_val.Text = x.ToString
        End If
    End Sub

    Private Sub SeriesMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeriesMenuToolStripMenuItem.Click
        Dim next_form As New NuGenNumericUseOfSeries_MainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub BToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BToolStripMenuItem.Click

    End Sub

    Private Sub AlgebricToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlgebricToolStripMenuItem.Click
        Dim next_form As New NuGenAlgerbicEquation_mainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenuToolStripMenuItem.Click
        Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        Me.Visible = False
        next_form.Visible = True
    End Sub
End Class