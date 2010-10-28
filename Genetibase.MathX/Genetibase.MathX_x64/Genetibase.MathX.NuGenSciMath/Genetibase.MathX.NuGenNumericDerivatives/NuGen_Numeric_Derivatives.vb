Imports Genetibase.MathX.NuGenNumericDerivatives


Public Class NuGen_Numeric_Derivatives

    Private Sub NuGen_Numeric_Derivatives_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button_steepda_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_steepda.Click
        If (text_l.Text = "" Or text_e.Text = "" Or text_k.Text = "" Or text_x.Text = "") Then

            MsgBox("Please Enter All The Values")

        Else

            Dim x(4) As Decimal
            Dim N As Integer


            Genetibase.MathX.NuGenNumericDerivatives.NuGenNumericalDerivatives.STEEPDA(Val(text_l.Text), Val(text_e.Text), 0, Val(text_k.Text), x, N)

            text_res.Text = N.ToString()


        End If

    End Sub

    Private Sub Button_steepds_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_steepds.Click
        If (text_l.Text = "" Or text_e.Text = "" Or text_k.Text = "" Or text_x.Text = "") Then

            MsgBox("Please Enter All The Values")

        Else

            Dim x(4) As Decimal
            Dim N As Integer


            Genetibase.MathX.NuGenNumericDerivatives.NuGenNumericalDerivatives.STEEPDS(Val(text_l.Text), Val(text_e.Text), 0, Val(text_k.Text), x, N)

            text_res.Text = N.ToString()


        End If
    End Sub

    Private Sub exitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exitToolStripMenuItem.Click
        End
    End Sub
End Class