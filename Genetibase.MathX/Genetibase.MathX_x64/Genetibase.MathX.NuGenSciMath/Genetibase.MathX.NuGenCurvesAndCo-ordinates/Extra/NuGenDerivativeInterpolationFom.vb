Public Class NuGenDerivativeInterpolationFom

    Private Sub Result_show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Result_show.Click
        Dim obj_x(4), obj_y(4), yt As Decimal
        Dim n As Int32


        If x_1.Text = "" Or x_2.Text = "" Or x_3.Text = "" Or x_4.Text = "" Or y_1.Text = "" Or y_2.Text = "" Or y_3.Text = "" Or y_4.Text = "" Or x1_val.Text = "" Or X2_val.Text = "" Or index_val.Text = "" Then
            MsgBox("Please Enter All The Values")
        Else
            obj_x(0) = Val(x_1.Text)
            obj_x(1) = Val(x_2.Text)
            obj_x(2) = Val(x_3.Text)
            obj_x(3) = Val(x_4.Text)


            obj_y(0) = Val(y_1.Text)
            obj_y(1) = Val(y_2.Text)
            obj_y(2) = Val(y_3.Text)
            obj_y(3) = Val(y_4.Text)


            n = Val(X2_val.Text)
            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenInterpolation3.LAGRANGE_DERIVATIVE_INTERPOLATION(Val(index_val.Text), obj_x, obj_y, n, Val(x1_val.Text), yt)

            result.Text = yt.ToString

        End If



    End Sub

    Private Sub BackToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackToolStripMenuItem.Click

    End Sub

    Private Sub InterpolationForLargrangMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InterpolationForLargrangMenuToolStripMenuItem.Click
        Dim next_form As New NuGenInterpolationMainForm

        Me.Visible = False
        next_form.Visible = True

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub
End Class