
Imports Genetibase.MathX.Distribution
Imports Genetibase.MathX.NuGenAlgebricEquations
Imports Genetibase.MathX.NuGenComplexLib
Imports Genetibase.MathX.NuGenCurvesAndCo_ordinates
Imports Genetibase.MathX.NuGenLinearRegression
Imports Genetibase.MathX.NuGenMatrix
Imports Genetibase.MathX.NuGenMatrixOperations
Imports Genetibase.MathX.NuGenNumericDerivatives
Imports Genetibase.MathX.NuGenStatistic
Imports Genetibase.MathX.NuGenStructures
Imports Genetibase.MathX.NuGenTrignometricOperations
Imports Genetibase.MathX.NuGenUtils


'Namespace NuGenSciCalc_name

    Public Class NuGenSciCalc

        Private Sub option_derivatives_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        End Sub

        Private Sub exitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exitToolStripMenuItem.Click
        'End
        End Sub

        Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
            Dim next_form As New Genetibase.MathX.NuGenComplexLib.NuGen_Numeric_menu
            Me.Visible = False
            next_form.Visible = True
        End Sub

        Private Sub mathematicalOperationsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mathematicalOperationsToolStripMenuItem.Click
            Dim next_form As New Genetibase.MathX.NuGenMatrixOperations.NuGen_operations_Menu
            Me.Visible = False
            next_form.Visible = True
        End Sub

        Private Sub NuGenDistributionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NuGenDistributionToolStripMenuItem.Click
            Dim next_form As New Genetibase.MathX.Distribution.NuGen_Distribution
            Me.Visible = False
            next_form.Visible = True
        End Sub

        Private Sub NuGenStatisticsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NuGenStatisticsToolStripMenuItem.Click
            Dim next_form As New Genetibase.MathX.NuGenStatistic.NuGenStatistic_Form
            Me.Visible = False
            next_form.Visible = True
        End Sub

        Private Sub NuGenCurvesAndCoordinatesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NuGenCurvesAndCoordinatesToolStripMenuItem.Click
            Dim next_form As New Genetibase.MathX.NuGenCurvesAndCo_ordinates.NuGenCurvesAndCoordinate_MainMenu
            Me.Visible = False
            next_form.Visible = True
        End Sub

        Private Sub NuGenAlgebricEquationsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NuGenAlgebricEquationsToolStripMenuItem.Click
            Dim next_form As New Genetibase.MathX.NuGenAlgebricEquations.NuGenAlgerbicEquation_mainForm
            Me.Visible = False
            next_form.Visible = True
        End Sub

        Private Sub NuGenLinearRegressionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NuGenLinearRegressionToolStripMenuItem.Click
            Dim next_form As New Genetibase.MathX.NuGenLinearRegression.N_uGenLinearRegressionForm
            Me.Visible = False
            next_form.Visible = True
        End Sub

        Private Sub NuGenDerivativesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NuGenDerivativesToolStripMenuItem.Click
            Dim next_form As New Genetibase.MathX.NuGenNumericDerivatives.NuGenDerivatives_Form
            Me.Visible = False
            next_form.Visible = True
        End Sub

        Private Sub NuGenStructuresToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NuGenStructuresToolStripMenuItem.Click
            Dim next_form As New Genetibase.MathX.NuGenStructures.NuGen_Structures_Main
            Me.Visible = False
            next_form.Visible = True
        End Sub

        Private Sub NuGenTrignometricOperationsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NuGenTrignometricOperationsToolStripMenuItem.Click
            Dim next_form As New Genetibase.MathX.NuGenTrignometricOperations.NuGenMainTrigForm
            Me.Visible = False
            next_form.Visible = True
        End Sub

        Private Sub NuGenSciCalc_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        End Sub
    End Class

'End Namespace
