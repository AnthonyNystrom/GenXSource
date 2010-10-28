Public Class NuGenDerivatives_Form

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        err_limit.Text = ""
        adj_mul.Text = ""
        par_deri.Text = ""
        m_val.Text = ""

        Label2.Text = Nothing

        Label2.Text = String.Concat(Label2.Text, "Derivative is ->")

        err_limit.Focus()



    End Sub

    Private Sub Chebyshev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chebyshev.Click

        Dim x(Val(par_deri.Text)) As Decimal
        Dim i As Integer
        Dim str As String = Nothing


        If err_limit.Text = "" Or adj_mul.Text = "" Or par_deri.Text = "" Or m_val.Text = "" Then
            MsgBox("Please Enter The Value")
        Else
            i = 0
            Do While i < Val(par_deri.Text)
                str = InputBox("Enter The Series For X()")
                x(i) = Val(str)
                str = Nothing
                i = i + 1
            Loop

            Genetibase.MathX.NuGenNumericDerivatives.NuGenNumericalDerivatives.STEEPDA(Val(par_deri.Text), Val(err_limit.Text), Val(m_val.Text), Val(adj_mul.Text), x, 0)

            Label2.Text = String.Concat(Label2.Text, " { ")

            i = 0
            Do While (i < Val(par_deri.Text))
                Label2.Text = String.Concat(Label2.Text, x(i).ToString)

                i = i + 1
                If i < Val(par_deri.Text) Then
                    Label2.Text = String.Concat(Label2.Text, " ,")
                Else
                    Label2.Text = String.Concat(Label2.Text, " }")

                End If
            Loop
        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim x(Val(par_deri.Text)) As Decimal
        Dim i As Integer
        Dim str As String = Nothing


        If err_limit.Text = "" Or adj_mul.Text = "" Or par_deri.Text = "" Or m_val.Text = "" Then
            MsgBox("Please Enter The Value")
        Else
            i = 0
            Do While i < Val(par_deri.Text)
                str = InputBox("Enter The Series For X()")
                x(i) = Val(str)
                str = Nothing
                i = i + 1
            Loop

            Genetibase.MathX.NuGenNumericDerivatives.NuGenNumericalDerivatives.STEEPDS(Val(par_deri.Text), Val(err_limit.Text), Val(m_val.Text), Val(adj_mul.Text), x, 0)

            Label2.Text = String.Concat(Label2.Text, " { ")

            i = 0
            Do While (i < Val(par_deri.Text))
                Label2.Text = String.Concat(Label2.Text, x(i).ToString)

                i = i + 1
                If i < Val(par_deri.Text) Then
                    Label2.Text = String.Concat(Label2.Text, " ,")
                Else
                    Label2.Text = String.Concat(Label2.Text, " }")

                End If
            Loop
        End If

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class