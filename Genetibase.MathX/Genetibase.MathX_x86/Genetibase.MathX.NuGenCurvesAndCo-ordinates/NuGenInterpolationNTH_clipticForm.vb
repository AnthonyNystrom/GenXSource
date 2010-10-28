Public Class NuGenInterpolationNTH_clipticForm

    Private Sub completeeliptical_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles completeeliptical.Click
        Dim a(2), b(2), e1, e2 As Decimal
        Dim n As Integer
        n = 0

        If k_val.Text = "" Or e_val.Text = "" Then

            MsgBox("Please Enter The Numbers")
        Else
            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenInterpolation1.CLIPTIC(Val(k_val.Text), Val(e_val.Text), a, b, n, e1, e2)



            res_e1.Text = e1.ToString
            res_e2.Text = e2.ToString

        End If
    End Sub

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        k_val.Text = ""
        e_val.Text = ""

        res_e1.Text = ""
        res_e2.Text = ""

        k_val.Focus()

    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Dim next_form As New NuGenCurvesAndCoordinate_MainMenu
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub NTHMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NTHMenuToolStripMenuItem.Click
        Dim next_form As New NuGenNTH_MainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenuToolStripMenuItem.Click
        Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        Me.Visible = False
        next_form.Visible = True
    End Sub
End Class