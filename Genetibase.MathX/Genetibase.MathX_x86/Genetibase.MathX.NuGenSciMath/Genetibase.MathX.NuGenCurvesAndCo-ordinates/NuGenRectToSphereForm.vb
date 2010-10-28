Public Class NuGenRectToSphereForm

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles u_val.TextChanged

    End Sub
    Private Sub Label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click

    End Sub

    Private Sub NuGenRectToSphereForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GroupBox1.Visible = False
        GroupBox2.Visible = False

    End Sub

    Private Sub RectangularToPhereToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RectangularToPhereToolStripMenuItem.Click
        GroupBox1.Visible = True
        GroupBox2.Visible = False

    End Sub

    Private Sub SphereToRectangularToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SphereToRectangularToolStripMenuItem.Click
        GroupBox2.Visible = True
        GroupBox1.Visible = False
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim x, y, z As Decimal

        If u_val1.Text = "" Or v_val1.Text = "" Or w_val1.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenSystemsOfCoordinates.SPRRECT(Val(u_val.Text), Val(v_val.Text), Val(w_val.Text), x, y, z)
            x_val1.Text = x.ToString
            y_val1.Text = y.ToString
            z_val1.Text = z.ToString

        End If

    End Sub

    Private Sub Rec_Sphere_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rec_Sphere.Click
        Dim u, v, w As Decimal

        If x_val.Text = "" Or y_val.Text = "" Or z_val.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenSystemsOfCoordinates.SPRRECT(Val(x_val.Text), Val(y_val.Text), Val(z_val.Text), u, v, w)
            u_val.Text = u.ToString
            v_val.Text = v.ToString
            w_val.Text = w.ToString

        End If
    End Sub

    Private Sub SystemOfCoordinateMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SystemOfCoordinateMenuToolStripMenuItem.Click
        Dim next_form As New NuGenSystemOfCoordinateMainForm

        Me.Visible = False
        next_form.Visible = True

    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Dim next_form As New NuGenCurvesAndCoordinate_MainMenu
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        u_val1.Text = ""
        v_val1.Text = ""
        w_val1.Text = ""

        x_val1.Text = ""
        y_val1.Text = ""
        z_val1.Text = ""
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        u_val.Text = ""
        v_val.Text = ""
        w_val.Text = ""

        x_val.Text = ""
        y_val.Text = ""
        z_val.Text = ""
        x_val.Focus()

    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class