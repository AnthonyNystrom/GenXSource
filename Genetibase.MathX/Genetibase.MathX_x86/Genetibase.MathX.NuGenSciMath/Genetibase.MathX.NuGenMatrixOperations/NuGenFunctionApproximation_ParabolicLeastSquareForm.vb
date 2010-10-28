Public Class NuGenFunctionApproximation_ParabolicLeastSquareForm

    Private Sub AlgebricToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlgebricToolStripMenuItem.Click
        Dim next_form As New NuGenFunctionApproximation_Main_Form
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        n_val.Text = ""
        d_val.Text = ""
        a_val.Text = ""
        b_val.Text = ""
        c_val.Text = ""

        n_val.Focus()
    End Sub

    Private Sub linear_least_square_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles linear_least_square.Click
        Dim a, b, c, d As Decimal
        Dim l, i As Integer
        Dim str As String = Nothing


        Dim x(Val(n_val.Text) + 1), y(Val(n_val.Text) + 1) As Decimal

        If n_val.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            i = 0
            Do While i < (Val(n_val.Text) - 1)
                str = InputBox("Enter Series X()")

                x(i) = Val(str)
                str = Nothing
                i = i + 1
            Loop

            i = 1
            Do While i < (Val(n_val.Text) - 1)
                str = InputBox("Enter Series y()")

                y(i) = Val(str)
                str = Nothing
                i = i + 1
            Loop

            Genetibase.MathX.NuGenMatrixOperations.NuGenFunctionApproximation.LSTSQR2(Val(n_val.Text), x, y, a, b, c, d)

            d_val.Text = d.ToString
            a_val.Text = a.ToString
            b_val.Text = b.ToString
            c_val.Text = c.ToString


        End If
    End Sub

    Private Sub SeriesMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeriesMenuToolStripMenuItem.Click
        Dim next_form As New NuGen_operations_Menu
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