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

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Dim next_form As New NuGenCurvesAndCoordinate_MainMenu
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem2.Click
        Dim next_form As New NuGenNTH_MainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenuToolStripMenuItem.Click
        Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        Me.Visible = False
        next_form.Visible = True
    End Sub
End Class