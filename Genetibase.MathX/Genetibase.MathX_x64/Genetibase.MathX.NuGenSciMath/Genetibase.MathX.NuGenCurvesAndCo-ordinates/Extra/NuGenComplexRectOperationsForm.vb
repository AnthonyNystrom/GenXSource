Public Class NuGenComplexRectOperationsForm

    Private Sub SystemOfCoordinateMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SystemOfCoordinateMenuToolStripMenuItem.Click
        Dim next_form As New NuGenSystemOfCoordinateMainForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub addition_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addition.Click
        Dim x(2), y(2) As Decimal
        Dim xt, yt As Decimal


        If x1.Text = "" Or x2.Text = "" Or y1.Text = "" Or y2.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            x(0) = Val(x1.Text)
            x(1) = Val(x2.Text)
            y(0) = Val(y1.Text)
            y(1) = Val(y2.Text)

            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenSystemsOfCoordinates.ZRECTMLT(x, y, xt, yt)
            res_x.Text = xt.ToString
            res_y.Text = yt.ToString


        End If
    End Sub

    Private Sub subtraction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles subtraction.Click
        Dim x(2), y(2) As Decimal
        Dim xt, yt As Decimal


        If x1.Text = "" Or x2.Text = "" Or y1.Text = "" Or y2.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            x(0) = Val(x1.Text)
            x(1) = Val(x2.Text)
            y(0) = Val(y1.Text)
            y(1) = Val(y2.Text)

            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenSystemsOfCoordinates.ZRECTDIV(x, y, xt, yt)
            res_x.Text = xt.ToString
            res_y.Text = yt.ToString


        End If
    End Sub

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        x1.Text = ""
        x2.Text = ""
        y1.Text = ""
        y2.Text = ""

        x1.Focus()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim x, y As Decimal
        Dim n As Integer


        If x_val.Text = "" Or y_val.Text = "" Or n_val.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            x = Val(x_val.Text)
            y = Val(y_val.Text)
            n = Val(n_val.Text)

            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenSystemsOfCoordinates.ZRECTPOW(n, x, y)
            res_x1.Text = x.ToString
            res_y1.Text = y.ToString


        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        x_val.Text = ""
        y_val.Text = ""
        n_val.Text = ""

        x_val.Focus()
    End Sub

    Private Sub RectComplexMultiplicationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RectComplexMultiplicationToolStripMenuItem.Click
        GroupBox3.Visible = False
        GroupBox1.Visible = False
        GroupBox2.Visible = True

    End Sub

    Private Sub RectComplexDivisionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RectComplexDivisionToolStripMenuItem.Click
        GroupBox1.Visible = False
        GroupBox3.Visible = False
        GroupBox2.Visible = True

    End Sub

    Private Sub RectComplexPowerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RectComplexPowerToolStripMenuItem.Click
        GroupBox2.Visible = False
        GroupBox3.Visible = False
        GroupBox1.Visible = True

    End Sub

    Private Sub NuGenComplexRectOperationsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GroupBox1.Visible = False
        GroupBox2.Visible = False
        GroupBox3.Visible = False

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim x, y As Decimal
        Dim n, m As Integer


        If x_rt_val.Text = "" Or y_rt_val.Text = "" Or n_rt_val.Text = "" Or m_rt_val.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            x = Val(x_rt_val.Text)
            y = Val(y_rt_val.Text)
            n = Val(n_rt_val.Text)
            m = Val(m_rt_val.Text)

            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenSystemsOfCoordinates.ZRECTRT(m, n, x, y)
            res_x1.Text = x.ToString
            res_y1.Text = y.ToString


        End If
    End Sub

    Private Sub RectComplexRootToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RectComplexRootToolStripMenuItem.Click
        GroupBox2.Visible = False
        GroupBox1.Visible = False
        GroupBox3.Visible = True

    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub GroupBox2_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox2.Enter

    End Sub
End Class