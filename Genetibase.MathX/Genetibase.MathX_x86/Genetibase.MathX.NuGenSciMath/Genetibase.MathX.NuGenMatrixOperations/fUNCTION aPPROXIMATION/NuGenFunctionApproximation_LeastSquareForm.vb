Public Class NuGenFunctionApproximation_LeastSquareForm

    Private Sub SeriesMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeriesMenuToolStripMenuItem.Click

    End Sub

    Private Sub AlgebricToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlgebricToolStripMenuItem.Click
        Dim next_form As New NuGenFunctionApproximation_Main_Form
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        m_val1.Text = ""
        e_val.Text = ""
        n_val.Text = ""
        d_val.Text = ""
        l_val.Text = ""
        c_val.Text = ""

        m_val1.Focus()

    End Sub

    Private Sub least_sqr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles least_sqr.Click

        Dim dt As Decimal
        Dim l, i As Integer
        Dim str As String = Nothing


        Dim x(Val(m_val1.Text) + 1), y(Val(m_val1.Text) + 1), c(Val(n_val.Text) + 1) As Decimal

        If m_val1.Text = "" Or e_val.Text = "" Or n_val.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            i = 1
            Do While i < (Val(m_val1.Text) + 1)
                Str = InputBox("Enter Series X()")

                x(i) = Val(Str)
                Str = Nothing
                i = i + 1
            Loop

            i = 1
            Do While i < (Val(m_val1.Text) + 1)
                str = InputBox("Enter Series y()")

                y(i) = Val(str)
                str = Nothing
                i = i + 1
            Loop

            Genetibase.MathX.NuGenMatrixOperations.NuGenFunctionApproximation.LSQRPOLY(Val(m_val1.Text), Val(e_val.Text), Val(n_val.Text), x, y, c, dt, l)
            c_val.Text = Nothing

            c_val.Text = String.Concat(c_val.Text, " {")

            i = 1
            Do While i < (Val(n_val.Text) + 1)
                c_val.Text = String.Concat(c_val.Text, c(i).ToString)

                i = i + 1
                If i < Val(n_val.Text) Then
                    c_val.Text = String.Concat(c_val.Text, " ,")
                Else
                    c_val.Text = String.Concat(c_val.Text, " }")
                End If
            Loop


        End If

    End Sub
End Class