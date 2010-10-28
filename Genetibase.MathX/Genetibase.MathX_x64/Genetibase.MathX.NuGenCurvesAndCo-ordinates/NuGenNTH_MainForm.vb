Public Class NuGenNTH_MainForm

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub HyperbolicTrignometricFunctionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HyperbolicTrignometricFunctionsToolStripMenuItem.Click
        Dim next_form As New NuGenInterpolationForNTH_IntegerOrderBesselForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Dim next_form As New NuGenCurvesAndCoordinate_MainMenu
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub SystemOfCoordinatesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SystemOfCoordinatesToolStripMenuItem.Click
        Dim next_form As New NuGenInterpolationNTH_clipticForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub InterpolationUsingLagrangeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InterpolationUsingLagrangeToolStripMenuItem.Click
        Dim next_form As New NuGenInterPolationNthForhermiteForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub InterpolationUsingNTHToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InterpolationUsingNTHToolStripMenuItem.Click
        Dim next_form As New NuGenInterolationNTHForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub InterpolationUsingCordicToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InterpolationUsingCordicToolStripMenuItem.Click
        Dim next_form As New NuGenInterolationNTHForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenuToolStripMenuItem.Click
        Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        Me.Visible = False
        next_form.Visible = True
    End Sub
End Class