Imports Genetibase.MathX.NuGenTrignometricOperations

Public Class NuGenTrigFunctions

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End

        'Exit
    End Sub

    Private Sub ArcTangentToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ArcTangentToolStripMenuItem.Click

    End Sub

    Private Sub trig_arctan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_arctan.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigonometry.AngleFromHypotenuseOpposed(Val(trig_first_no.Text), Val(trig_second_no.Text))

    End Sub

    Private Sub trig_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_clear.Click

        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigonometry.LengthFromOpposedOpposedAngle(Val(trig_first_no.Text), Val(trig_second_no.Text))
        


    End Sub

    Private Sub trig_exponent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_exponent.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigonometry.AngleFromLegths(Val(trig_first_no.Text), Val(trig_second_no.Text))


    End Sub

    Private Sub trig_natural_log_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_natural_log.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigonometry.HypotenuseFromlengthAdjacenteAngle(Val(trig_first_no.Text), Val(trig_second_no.Text))
    End Sub

    Private Sub trig_log_base_ten_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_log_base_ten.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigonometry.HypotenuseFromLengthOpposedAngle(Val(trig_first_no.Text), Val(trig_second_no.Text))
    End Sub

    Private Sub trig_cos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_cos.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigonometry.LengthFromHypotenuseAdjacenteAngle(Val(trig_first_no.Text), Val(trig_second_no.Text))
    End Sub

    Private Sub trig_sin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_sin.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigonometry.LengthFromHypotenuseOpposedAngle(Val(trig_first_no.Text), Val(trig_second_no.Text))
    End Sub

    Private Sub trig_pow_ten_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_pow_ten.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigonometry.LengthFromOpposedAdjacenteAngle(Val(trig_first_no.Text), Val(trig_second_no.Text))
    End Sub

    Private Sub CosineToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CosineToolStripMenuItem.Click

    End Sub

    Private Sub NuGenTrigFunctions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub LogBaseTenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogBaseTenToolStripMenuItem.Click

    End Sub

    Private Sub PowerOfTenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PowerOfTenToolStripMenuItem.Click

    End Sub

    Private Sub trig_result_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_result.TextChanged

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        trig_first_no.Text = ""
        trig_second_no.Text = ""
        trig_result.Text = ""

        trig_first_no.Focus()
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