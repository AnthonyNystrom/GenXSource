Imports Genetibase.MathX.NuGenTrignometricOperations

Public Class NuGenTrignometrys

    Private Sub ArcTangentToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ArcTangentToolStripMenuItem.Click

    End Sub

    Private Sub Trig_groupbox_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Trig_groupbox.Enter

    End Sub

    Private Sub trig_arctan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_arctan.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigFunctions.ARCTAN(Val(trig_first_no.Text))
    End Sub

    Private Sub trig_exponent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_exponent.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigFunctions.EXPX(Val(trig_first_no.Text))
    End Sub

    Private Sub trig_natural_log_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_natural_log.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigFunctions.LNX(Val(trig_first_no.Text))
    End Sub

    Private Sub trig_log_base_ten_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_log_base_ten.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigFunctions.LOGX(Val(trig_first_no.Text))
    End Sub

    Private Sub trig_cos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_cos.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigFunctions.COSINE(Val(trig_first_no.Text))
    End Sub

    Private Sub trig_sin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_sin.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigFunctions.SINE(Val(trig_first_no.Text))
    End Sub

    Private Sub trig_pow_ten_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_pow_ten.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigFunctions.TENPOW(Val(trig_first_no.Text))
    End Sub

    Private Sub trig_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_clear.Click
        trig_first_no.Text = ""
        trig_result.Text = ""

        trig_first_no.Focus()

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub TrignometricMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrignometricMenuToolStripMenuItem.Click
        Dim next_form As New NuGenMainTrigForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class