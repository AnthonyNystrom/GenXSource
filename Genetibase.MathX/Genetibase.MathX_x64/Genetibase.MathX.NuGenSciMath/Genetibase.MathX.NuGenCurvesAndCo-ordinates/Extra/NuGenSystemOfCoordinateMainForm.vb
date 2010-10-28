Public Class NuGenSystemOfCoordinateMainForm

    Private Sub CurvesAndCoordinateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CurvesAndCoordinateToolStripMenuItem.Click

    End Sub

    Private Sub GeneralIntegrationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GeneralIntegrationToolStripMenuItem.Click
        Dim next_form As New NuGenRectToSphereForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub LagrangeDerivativeInterpolationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LagrangeDerivativeInterpolationToolStripMenuItem.Click
        Dim next_form As New NuGenRectToSphereForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub LargrangeInterpolationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LargrangeInterpolationToolStripMenuItem.Click
        Dim next_form As New NuGenComplexOperationsForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub NewtoneInterpolationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewtoneInterpolationToolStripMenuItem.Click
        Dim next_form As New NuGenComplexOperationsForm

        Me.Visible = False
        next_form.Visible = True

    End Sub

    Private Sub PolarDivisionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PolarDivisionToolStripMenuItem.Click
        Dim next_form As New NuGenPolarOperationsForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub PolarMultiplicationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PolarMultiplicationToolStripMenuItem.Click
        Dim next_form As New NuGenPolarOperationsForm

        Me.Visible = False
        next_form.Visible = True

    End Sub

    Private Sub RectangularComplexNoMultiplicationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RectangularComplexNoMultiplicationToolStripMenuItem.Click
        Dim next_form As New NuGenComplexRectOperationsForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub RectangularComplexNoDivisionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RectangularComplexNoDivisionToolStripMenuItem.Click
        Dim next_form As New NuGenComplexRectOperationsForm

        Me.Visible = False
        next_form.Visible = True

    End Sub

    Private Sub PolarPowerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PolarPowerToolStripMenuItem.Click
        Dim next_form As New NuGenPolarOperationsForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub PolarRootToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PolarRootToolStripMenuItem.Click
        Dim next_form As New NuGenPolarOperationsForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub RectangularComplexNoPowerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RectangularComplexNoPowerToolStripMenuItem.Click
        Dim next_form As New NuGenComplexRectOperationsForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub RectangularComplexNoRootToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RectangularComplexNoRootToolStripMenuItem.Click
        Dim next_form As New NuGenComplexRectOperationsForm

        Me.Visible = False
        next_form.Visible = True
    End Sub
End Class