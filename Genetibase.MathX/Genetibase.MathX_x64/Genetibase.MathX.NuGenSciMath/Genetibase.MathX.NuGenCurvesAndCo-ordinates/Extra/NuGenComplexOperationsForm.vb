Public Class NuGenComplexOperationsForm

    Private Sub RectangularToPhereToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub SystemOfCoordinateMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SystemOfCoordinateMenuToolStripMenuItem.Click
        Dim next_form As New NuGenSystemOfCoordinateMainForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addition.Click
        Dim x(3), y(3) As Decimal

        If x1.Text = "" Or x2.Text = "" Or y1.Text = "" Or y2.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            x(0) = Val(x1.Text)
            x(1) = Val(x2.Text)
            y(0) = Val(y1.Text)
            y(1) = Val(y2.Text)

            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenSystemsOfCoordinates.ZADD(x, y)
            res_x.Text = x(3).ToString
            res_y.Text = y(3).ToString


        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        x1.Text = ""
        x2.Text = ""
        y1.Text = ""
        y2.Text = ""

        x1.Focus()

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles subtraction.Click
        Dim x(3), y(3) As Decimal

        If x1.Text = "" Or x2.Text = "" Or y1.Text = "" Or y2.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            x(0) = Val(x1.Text)
            x(1) = Val(x2.Text)
            y(0) = Val(y1.Text)
            y(1) = Val(y2.Text)

            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenSystemsOfCoordinates.ZSUB(x, y)
            res_x.Text = x(3).ToString
            res_y.Text = y(3).ToString


        End If
    End Sub
End Class