Imports Genetibase.MathX.NuGenTrignometricOperations

Public Class NuGenTrignometricFunction

    Private Sub SeriesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeriesToolStripMenuItem.Click

    End Sub

    Private Sub ArcTangentToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ArcTangentToolStripMenuItem.Click

    End Sub

    Private Sub PowerOfTenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PowerOfTenToolStripMenuItem.Click

    End Sub

    Private Sub Trig_groupbox_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Trig_groupbox.Enter

    End Sub

    Private Sub trig_arccos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_arccos.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigonometricFunctions.Arccos(Val(trig_first_no.Text))
    End Sub

    Private Sub trig_arccosec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_arccosec.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigonometricFunctions.Arccosec(Val(trig_first_no.Text))
    End Sub

    Private Sub trig_arccotan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_arccotan.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigonometricFunctions.Arccotan(Val(trig_first_no.Text))
    End Sub

    Private Sub trig_arcsec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_arcsec.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigonometricFunctions.Arcsec(Val(trig_first_no.Text))
    End Sub

    Private Sub trig_arcsin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_arcsin.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigonometricFunctions.Arcsin(Val(trig_first_no.Text))
    End Sub

    Private Sub trig_Cosec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_Cosec.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigonometricFunctions.Cosec(Val(trig_first_no.Text))
    End Sub

    Private Sub trig_cotan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_cotan.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigonometricFunctions.Cotan(Val(trig_first_no.Text))
    End Sub

    Private Sub trig_Sec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_Sec.Click
        trig_result.Text = Genetibase.MathX.NuGenTrignometricOperations.NuGenTrigonometricFunctions.Sec(Val(trig_first_no.Text))
    End Sub

    Private Sub trig_Clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trig_Clear.Click
        trig_first_no.Text = ""
        trig_result.Text = ""

        trig_first_no.Focus()

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub BackToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackToolStripMenuItem.Click

    End Sub

    Private Sub NuGenTrignometricFunction_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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