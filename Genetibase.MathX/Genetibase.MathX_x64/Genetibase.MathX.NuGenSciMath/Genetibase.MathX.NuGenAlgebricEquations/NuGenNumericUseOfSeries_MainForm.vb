Public Class NuGenNumericUseOfSeries_MainForm

    Private Sub AllRootsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllRootsToolStripMenuItem.Click
        Dim next_form As New NuGenSeries_AsymptoticForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ComplexRootToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComplexRootToolStripMenuItem.Click
        Dim next_form As New NuGenSeries_Bessel1Form
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub BesselFunctionSeriesSubroutineToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BesselFunctionSeriesSubroutineToolStripMenuItem.Click
        Dim next_form As New NuGenSeries_BesslserForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ChebyshevEconomizationSeriesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChebyshevEconomizationSeriesToolStripMenuItem.Click
        Dim next_form As New NuGenSeries_ChebyshevForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ChebychevSeriesCoefficientToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChebychevSeriesCoefficientToolStripMenuItem.Click
        Dim next_form As New NuGenSeries_ChiSqureForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub HornersShiftingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HornersShiftingToolStripMenuItem.Click
        Dim next_form As New NuGenSeries_HornerForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub InverseNormalDistributionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InverseNormalDistributionToolStripMenuItem.Click
        Dim next_form As New NuGenSeries_InvnormForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ReciprocalPowerSeriesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReciprocalPowerSeriesToolStripMenuItem.Click
        Dim next_form As New NuGenSeries_ReciProForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ReversionSeriesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReversionSeriesToolStripMenuItem.Click
        Dim next_form As New NuGenSeries_ReversionSeriesForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub SineProductSeriesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SineProductSeriesToolStripMenuItem.Click
        Dim next_form As New NuGenSeries_SineProductForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub BairstowComplexRootToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub AlgebricEquationMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlgebricEquationMenuToolStripMenuItem.Click
        Dim next_form As New NuGenAlgerbicEquation_mainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class