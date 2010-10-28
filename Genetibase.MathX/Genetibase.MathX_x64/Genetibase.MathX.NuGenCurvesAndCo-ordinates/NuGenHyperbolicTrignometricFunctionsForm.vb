Imports Genetibase.MathX.NuGenCurvesAndCo_ordinates

Public Class NuGenHyperbolicTrignometricFunctionsForm

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HArcTan.Click
        If first_no.Text = "" Then
            MsgBox("Please Enter The Angle")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenHyperbolicTrigonometricFunctions.HArctan(Val(first_no.Text))
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HCoSec.Click
        If first_no.Text = "" Then
            MsgBox("Please Enter The Angle")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenHyperbolicTrigonometricFunctions.HCosec(Val(first_no.Text))
        End If
    End Sub

    Private Sub harccos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles harccos.Click

        If first_no.Text = "" Then
            MsgBox("Please Enter The Angle")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenHyperbolicTrigonometricFunctions.HArccos(Val(first_no.Text))
        End If

    End Sub

    Private Sub HArcCoSec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HArcCoSec.Click
        If first_no.Text = "" Then
            MsgBox("Please Enter The Angle")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenHyperbolicTrigonometricFunctions.HArccosec(Val(first_no.Text))
        End If
    End Sub

    Private Sub HArcCoTan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HArcCoTan.Click
        If first_no.Text = "" Then
            MsgBox("Please Enter The Angle")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenHyperbolicTrigonometricFunctions.HArccotan(Val(first_no.Text))
        End If
    End Sub

    Private Sub HArcSec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HArcSec.Click
        If first_no.Text = "" Then
            MsgBox("Please Enter The Angle")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenHyperbolicTrigonometricFunctions.HArcsec(Val(first_no.Text))
        End If
    End Sub

    Private Sub HArcSin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HArcSin.Click
        If first_no.Text = "" Then
            MsgBox("Please Enter The Angle")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenHyperbolicTrigonometricFunctions.HArcsin(Val(first_no.Text))
        End If
    End Sub

    Private Sub HCos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HCos.Click
        If first_no.Text = "" Then
            MsgBox("Please Enter The Angle")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenHyperbolicTrigonometricFunctions.HCos(Val(first_no.Text))
        End If
    End Sub

    Private Sub HCoTan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HCoTan.Click
        If first_no.Text = "" Then
            MsgBox("Please Enter The Angle")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenHyperbolicTrigonometricFunctions.HCotan(Val(first_no.Text))
        End If
    End Sub

    Private Sub HSec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HSec.Click
        If first_no.Text = "" Then
            MsgBox("Please Enter The Angle")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenHyperbolicTrigonometricFunctions.HSec(Val(first_no.Text))
        End If
    End Sub

    Private Sub HSin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HSin.Click
        If first_no.Text = "" Then
            MsgBox("Please Enter The Angle")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenHyperbolicTrigonometricFunctions.HSin(Val(first_no.Text))
        End If
    End Sub

    Private Sub HTan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HTan.Click
        If first_no.Text = "" Then
            MsgBox("Please Enter The Angle")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenHyperbolicTrigonometricFunctions.HTan(Val(first_no.Text))
        End If
    End Sub

    Private Sub curve_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles curve_clear.Click
        first_no.Text = ""
        result.Text = ""

        first_no.Focus()
    End Sub

    Private Sub NuGenHyperbolicTrignometricFunctionsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub CurvesAndCoordinateMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CurvesAndCoordinateMenuToolStripMenuItem.Click
        Dim next_form As New NuGenCurvesAndCoordinate_MainMenu
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenuToolStripMenuItem.Click
        Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        Me.Visible = False
        next_form.Visible = True
    End Sub
End Class