Public Class NuGenPolarOperationsForm

    Private Sub SystemOfCoordinateMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SystemOfCoordinateMenuToolStripMenuItem.Click
        Dim next_form As New NuGenSystemOfCoordinateMainForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub multiplication_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles multiplication.Click
        Dim u(2), v(2) As Decimal
        Dim ut, vt As Decimal


        If u1.Text = "" Or u2.Text = "" Or v1.Text = "" Or v2.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            u(0) = Val(u1.Text)
            u(1) = Val(u2.Text)
            v(0) = Val(v1.Text)
            v(1) = Val(v2.Text)

            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenSystemsOfCoordinates.ZPOLMLT(u, v, ut, vt)
            res_ut.Text = ut.ToString
            res_vt.Text = vt.ToString


        End If
    End Sub

    Private Sub Division_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Division.Click
        Dim u(2), v(2) As Decimal
        Dim ut, vt As Decimal


        If u1.Text = "" Or u2.Text = "" Or v1.Text = "" Or v2.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            u(0) = Val(u1.Text)
            u(1) = Val(u2.Text)
            v(0) = Val(v1.Text)
            v(1) = Val(v2.Text)

            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenSystemsOfCoordinates.ZPOLDIV(u, v, ut, vt)
            res_ut.Text = ut.ToString
            res_vt.Text = vt.ToString


        End If
    End Sub

    Private Sub Clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Clear.Click
        u1.Text = ""
        u2.Text = ""
        v1.Text = ""
        v2.Text = ""

        u1.Focus()
    End Sub

    Private Sub polarPower_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles polarPower.Click
        Dim u, v As Decimal
        Dim ut, vt, n As Decimal


        If u_val.Text = "" Or v_val.Text = "" Or n_val.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            u = Val(u_val.Text)
            v = Val(v_val.Text)
            n = Val(n_val.Text)

            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenSystemsOfCoordinates.ZPOLPOW(u, v, n, ut, vt)
            res_ut.Text = ut.ToString
            res_vt.Text = vt.ToString


        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim u, v As Decimal
        Dim ut, vt, n As Decimal

        If u_val.Text = "" Or v_val.Text = "" Or n_val.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            u = Val(u_val.Text)
            v = Val(v_val.Text)
            n = Val(n_val.Text)

            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenSystemsOfCoordinates.ZPOLPOW(u, v, n, ut, vt)
            res_ut.Text = ut.ToString
            res_vt.Text = vt.ToString

        End If
    End Sub

    Private Sub MenuStrip1_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub

    Private Sub NuGenPolarOperationsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GroupBox2.Visible = False
        pow_rt.Visible = False

    End Sub

    Private Sub PolarPowerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PolarPowerToolStripMenuItem.Click
        GroupBox2.Visible = False
        pow_rt.Visible = True
    End Sub

    Private Sub PolarRootToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PolarRootToolStripMenuItem.Click
        GroupBox2.Visible = False
        pow_rt.Visible = True
    End Sub

    Private Sub PolarMultiplicationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PolarMultiplicationToolStripMenuItem.Click
        pow_rt.Visible = False
        GroupBox2.Visible = True

    End Sub

    Private Sub PolarDivisionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PolarDivisionToolStripMenuItem.Click
        pow_rt.Visible = False
        GroupBox2.Visible = True

    End Sub
End Class