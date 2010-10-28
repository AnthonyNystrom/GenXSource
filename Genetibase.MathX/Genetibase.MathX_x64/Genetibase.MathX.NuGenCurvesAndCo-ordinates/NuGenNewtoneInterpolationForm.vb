Public Class NuGenNewtoneInterpolationForm

    Private Sub InterpolationLagrangeMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InterpolationLagrangeMenuToolStripMenuItem.Click
        Dim next_form As New NuGenInterpolationMainForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub Result_show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Result_show.Click

        Dim obj_x(4), obj_y(4), e1, n As Decimal

        If x_1.Text = "" Or x_2.Text = "" Or x_3.Text = "" Or x_4.Text = "" Or y_1.Text = "" Or y_2.Text = "" Or y_3.Text = "" Or y_4.Text = "" Or x1_val.Text = "" Or X2_val.Text = "" Or index_val.Text = "" Then
            MsgBox("Please Enter All The Values")
        Else
            n = 1

            obj_x(0) = Val(x_1.Text)
            obj_x(1) = Val(x_2.Text)
            obj_x(2) = Val(x_3.Text)
            obj_x(3) = Val(x_4.Text)


            obj_y(0) = Val(y_1.Text)
            obj_y(1) = Val(y_2.Text)
            obj_y(2) = Val(y_3.Text)
            obj_y(3) = Val(y_4.Text)



            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenInterpolation3.NEWTON_INTERPOLATION(n, Val(index_val.Text), obj_x, obj_y, Val(x1_val.Text), Val(X2_val.Text), e1)

            result.Text = e1.ToString

        End If




    End Sub

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        x_1.Text = ""
        x_2.Text = ""
        x_3.Text = ""
        x_4.Text = ""
        y_1.Text = ""
        y_2.Text = ""
        y_3.Text = ""
        y_4.Text = ""
        x1_val.Text = ""
        X2_val.Text = ""
        index_val.Text = ""

        x1_val.Focus()
    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Dim next_form As New NuGenCurvesAndCoordinate_MainMenu
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenuToolStripMenuItem.Click
        Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        Me.Visible = False
        next_form.Visible = True
    End Sub
End Class