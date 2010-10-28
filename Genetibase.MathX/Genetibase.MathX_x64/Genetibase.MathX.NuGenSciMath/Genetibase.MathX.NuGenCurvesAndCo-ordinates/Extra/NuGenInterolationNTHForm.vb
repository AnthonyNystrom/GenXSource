Public Class NuGenInterolationNTHForm

    Private Sub ArcSinX_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ArcSinX.Click

        Dim y As Decimal
        Dim m As Integer
        m = 0

        If x_val.Text = "" Or e_val.Text = "" Then

            MsgBox("Please Enter The Numbers")
        Else
            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenInterpolation1.ARCSINX(Val(x_val.Text), Val(e_val.Text), y, m)

            Result.Text = y.ToString

        End If
    End Sub

    Private Sub Ataniter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ataniter.Click
        Dim y As Decimal
        Dim m As Integer
        m = 0

        If x_val.Text = "" Or e_val.Text = "" Then

            MsgBox("Please Enter The Numbers")
        Else
            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenInterpolation1.ATANITER(Val(x_val.Text), Val(e_val.Text), y, m)

            Result.Text = y.ToString

        End If

    End Sub
End Class