Public Class NuGenFunctionApproximation_PolynomialRegressionForm

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        n_val.Text = ""
        m_val.Text = ""
        z_val.Text = ""

        n_val.Focus()

    End Sub

    Private Sub AlgebricToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlgebricToolStripMenuItem.Click
        Dim next_form As New NuGenFunctionApproximation_Main_Form
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub Polynomial_Regression_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Polynomial_Regression.Click

        Dim j, i As Integer
        Dim str As String = Nothing


        Dim x(Val(n_val.Text) + 1), z((Val(m_val.Text) + 1), (Val(n_val.Text) + 1)) As Decimal

        If n_val.Text = "" Or m_val.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            i = 1
            Do While i < (Val(n_val.Text) + 1)
                str = InputBox("Enter Series X()")

                x(i) = Val(str)
                str = Nothing
                i = i + 1
            Loop



            Genetibase.MathX.NuGenMatrixOperations.NuGenFunctionApproximation.POLYCM(Val(m_val.Text), Val(n_val.Text), x, z)


            z_val.Text = Nothing
            z_val.Text = String.Concat(z_val.Text, " {")

            i = 1
            Do While i < (Val(m_val.Text) + 1)
                j = 1
                z_val.Text = String.Concat(z_val.Text, " (")
                Do While j < (Val(n_val.Text) + 1)

                    z_val.Text = String.Concat(z_val.Text, z(i, j).ToString)


                    j = j + 1
                    If j < (Val(n_val.Text) + 1) Then
                        z_val.Text = String.Concat(z_val.Text, " ,")
                    Else
                        z_val.Text = String.Concat(z_val.Text, ")")
                    End If
                Loop
                i = i + 1
                If i < (Val(n_val.Text) + 1) Then
                    z_val.Text = String.Concat(z_val.Text, " ,")
                Else
                    z_val.Text = String.Concat(z_val.Text, " }")
                End If
            Loop


        End If
    End Sub
End Class