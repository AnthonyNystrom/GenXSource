Public Class NuGenCurvesAndCoordinate_MainMenu

    Private Sub HyperbolicTrignometricFunctionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HyperbolicTrignometricFunctionsToolStripMenuItem.Click
        Dim next_form As New NuGenHyperbolicTrignometricFunctionsForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub SystemOfCoordinatesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SystemOfCoordinatesToolStripMenuItem.Click
        Dim next_form As New NuGenSystemOfCoordinateMainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub InterpolationUsingLagrangeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InterpolationUsingLagrangeToolStripMenuItem.Click
        Dim next_form As New NuGenInterpolationMainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub InterpolationUsingNTHToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InterpolationUsingNTHToolStripMenuItem.Click
        Dim next_form As New NuGenNTH_MainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub InterpolationUsingCordicToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InterpolationUsingCordicToolStripMenuItem.Click
        Dim next_form As New NuGenInterPolationForCordicForm
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