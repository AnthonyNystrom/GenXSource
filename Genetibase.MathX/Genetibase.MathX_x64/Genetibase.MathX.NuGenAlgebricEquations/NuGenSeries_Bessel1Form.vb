Public Class NuGenSeries_Bessel1Form

    Private Sub Bessel1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bessel1.Click
        Dim j0, j1, e2 As Decimal

        If x.Text = "" Or con_factor.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            Genetibase.MathX.NuGenAlgebricEquations.NuGenNumericalUseOfSeries.BESSEL01(Val(con_factor.Text), Val(x.Text), j0, j1, 0, e2)

            res_j0.Text = j0.ToString
            res_j1.Text = j1.ToString
            e1.Text = e2.ToString

        End If
    End Sub

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        x.Text = ""
        con_factor.Text = ""


        res_j0.Text = ""
        res_j1.Text = ""
        e1.Text = ""

        x.Focus()


    End Sub

    Private Sub SeriesMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeriesMenuToolStripMenuItem.Click
        Dim next_form As New NuGenNumericUseOfSeries_MainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub BToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BToolStripMenuItem.Click

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub AlgebricToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlgebricToolStripMenuItem.Click
        Dim next_form As New NuGenAlgerbicEquation_mainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub
End Class