Public Class NuGenInterpolationMainForm

    Private Sub GeneralIntegrationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GeneralIntegrationToolStripMenuItem.Click
        Dim next_form As New NuGenInterpolationForGeneralIntegration

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub LagrangeDerivativeInterpolationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LagrangeDerivativeInterpolationToolStripMenuItem.Click
        Dim next_form As New NuGenDerivativeInterpolationFom
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub NewtoneInterpolationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewtoneInterpolationToolStripMenuItem.Click
        Dim next_form As New NuGenNewtoneInterpolationForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub LargrangeInterpolationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LargrangeInterpolationToolStripMenuItem.Click
        Dim next_form As New NuGenInterpolationForLargrangeForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub CurvesAndCoordinateMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CurvesAndCoordinateMenuToolStripMenuItem.Click
        Dim next_form As New NuGenCurvesAndCoordinate_MainMenu
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class