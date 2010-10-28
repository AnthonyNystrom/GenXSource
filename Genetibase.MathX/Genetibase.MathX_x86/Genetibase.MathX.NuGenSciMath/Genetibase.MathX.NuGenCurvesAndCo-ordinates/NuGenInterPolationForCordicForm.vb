Public Class NuGenInterPolationForCordicForm

    Private Sub curve_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles curve_clear.Click
        first_no.Text = ""
        result.Text = ""

        first_no.Focus()

    End Sub

    Private Sub CosH_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CosH.Click
        If first_no.Text = "" Then
            MsgBox("Please Enter The Number")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenInterpolation2.COSH(Val(first_no.Text))
        End If

    End Sub

    Private Sub SinH_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SinH.Click
        If first_no.Text = "" Then
            MsgBox("Please Enter The Number")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenInterpolation2.SINH(Val(first_no.Text))
        End If
    End Sub

    Private Sub TanH_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TanH.Click
        If first_no.Text = "" Then
            MsgBox("Please Enter The Number")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenInterpolation2.TANH(Val(first_no.Text))
        End If
    End Sub

    Private Sub InvCosH_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvCosH.Click
        If first_no.Text = "" Then
            MsgBox("Please Enter The Number")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenInterpolation2.INVCOSH(Val(first_no.Text))
        End If
    End Sub

    Private Sub InvSinH_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvSinH.Click
        If first_no.Text = "" Then
            MsgBox("Please Enter The Number")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenInterpolation2.INVSINH(Val(first_no.Text))
        End If
    End Sub

    Private Sub InvTanH_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvTanH.Click
        If first_no.Text = "" Then
            MsgBox("Please Enter The Number")
        Else
            result.Text = Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenInterpolation2.INVTANH(Val(first_no.Text))
        End If
    End Sub

    Private Sub CurvesAndCoordinateMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CurvesAndCoordinateMenuToolStripMenuItem.Click
        Dim next_form As New NuGenCurvesAndCoordinate_MainMenu
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class