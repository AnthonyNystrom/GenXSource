Public Class NuGenSeries_ReversionSeriesForm

    Private Sub SeriesMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeriesMenuToolStripMenuItem.Click
        Dim next_form As New NuGenNumericUseOfSeries_MainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        a1.Text = ""
        a2.Text = ""
        a3.Text = ""
        a4.Text = ""
        a5.Text = ""
        a6.Text = ""
        a7.Text = ""

        b1.Text = ""
        b2.Text = ""
        b3.Text = ""
        b4.Text = ""
        b5.Text = ""
        b6.Text = ""
        b7.Text = ""


        a1.Focus()



    End Sub

    Private Sub horner_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles horner.Click
        Dim a(7), b(7) As Decimal


        If a1.Text = "" Or a2.Text = "" Or a3.Text = "" Or a4.Text = "" Or a5.Text = "" Or a6.Text = "" Or a7.Text = "" Then
            MsgBox("Please enter The Values")
        Else
            a(0) = a1.Text
            a(1) = a2.Text
            a(2) = a3.Text
            a(3) = a4.Text
            a(4) = a5.Text
            a(5) = a6.Text
            a(6) = a7.Text

            Genetibase.MathX.NuGenAlgebricEquations.NuGenNumericalUseOfSeries.REVERSE(0, a, b)
            b1.Text = b(0).ToString
            b2.Text = b(1).ToString
            b3.Text = b(2).ToString
            b4.Text = b(3).ToString
            b5.Text = b(4).ToString
            b6.Text = b(5).ToString
            b7.Text = b(6).ToString

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