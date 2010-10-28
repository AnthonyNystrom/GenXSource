Public Class NuGenSeries_HornerForm

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

        b1.Text = ""
        b2.Text = ""
        b3.Text = ""
        b4.Text = ""

        x_val.Text = ""

        a1.Focus()


    End Sub

    Private Sub horner_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles horner.Click
        Dim a(4), c(4, 5), b(4) As Decimal
        Dim i, j As Integer
        Dim str As String = Nothing

        If a1.Text = "" Or a2.Text = "" Or a3.Text = "" Or a4.Text = "" Or x_val.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            a(0) = Val(a1.Text)
            a(1) = Val(a2.Text)
            a(2) = Val(a3.Text)
            a(3) = Val(a4.Text)

            Do While (i < 4)
                j = 0
                Do While j < 5
                    str = InputBox("Enter Value For C( , )")
                    c(i, j) = Val(str)
                    str = Nothing
                    j = j + 1
                Loop
                i = i + 1
            Loop
            Genetibase.MathX.NuGenAlgebricEquations.NuGenNumericalUseOfSeries.HORNER(Val(x_val.Text), a, b, c)

            b1.Text = b(0).ToString
            b2.Text = b(1).ToString
            b3.Text = b(2).ToString
            b4.Text = b(3).ToString

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