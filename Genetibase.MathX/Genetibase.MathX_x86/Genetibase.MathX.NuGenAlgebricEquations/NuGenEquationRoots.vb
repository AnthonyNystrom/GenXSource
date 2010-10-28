Imports Genetibase.MathX.NuGenAlgebricEquations

Public Class NuGenEquationRoots

    Private Sub alg_equ_aitkn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles alg_equ_aitkn.Click
        If alg_equ_first_no.Text = "" Or alg_equ_second_no.Text = "" Or alg_equ_third_no.Text = "" Or alg_equ_fourth_no.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            Dim obj1 As Decimal
            Dim obj2 As Integer

            Genetibase.MathX.NuGenAlgebricEquations.NumericalAnalysis.NuGenEquationRoots1.AITKEN(Val(alg_equ_first_no.Text), Val(alg_equ_second_no.Text), Val(alg_equ_third_no.Text), Val(alg_equ_fourth_no.Text), obj1, obj2)

            alg_equ_result_no.Text = (obj1.ToString)
        End If
    End Sub

    Private Sub alg_equ_asiter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles alg_equ_asiter.Click


        If alg_equ_first_no.Text = "" Or alg_equ_second_no.Text = "" Or alg_equ_third_no.Text = "" Or alg_equ_fourth_no.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            Dim obj1 As Decimal
            Dim obj2 As Integer

            Genetibase.MathX.NuGenAlgebricEquations.NumericalAnalysis.NuGenEquationRoots1.ASITER(Val(alg_equ_first_no.Text), Val(alg_equ_second_no.Text), Val(alg_equ_third_no.Text), Val(alg_equ_fourth_no.Text), obj1, obj2)

            alg_equ_result_no.Text = (obj1.ToString)
        End If
    End Sub

    Private Sub alg_equ_bisect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles alg_equ_bisect.Click
        
    End Sub

    Private Sub alg_equ_nextroot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)


        If alg_equ_first_no.Text = "" Or alg_equ_second_no.Text = "" Or alg_equ_third_no.Text = "" Or alg_equ_fourth_no.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
        Dim obj1 As Decimal
        Dim obj2 As Integer

            'Genetibase.MathX.NuGenAlgebricEquations.NumericalAnalysis.NuGenEquationRoots1.NEXTROOT(Val(alg_equ_first_no.Text), Val(alg_equ_second_no.Text), Val(alg_equ_third_no.Text), Val(alg_equ_fourth_no.Text), obj1, obj2)

            alg_equ_result_no.Text = (obj1.ToString)
        End If

    End Sub

    Private Sub alg_equ_regula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles alg_equ_regula.Click

        If alg_equ_first_no.Text = "" Or alg_equ_second_no.Text = "" Or alg_equ_third_no.Text = "" Or alg_equ_fourth_no.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            Dim obj1 As Decimal
            Dim obj2 As Integer

            Genetibase.MathX.NuGenAlgebricEquations.NumericalAnalysis.NuGenEquationRoots1.REGULA(Val(alg_equ_first_no.Text), Val(alg_equ_second_no.Text), Val(alg_equ_third_no.Text), Val(alg_equ_fourth_no.Text), obj1, obj2)

            alg_equ_result_no.Text = (obj1.ToString)
        End If
    End Sub

    Private Sub alg_euqrsyndiv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles alg_euqrsyndiv.Click

        If alg_equ_first_no.Text = "" Or alg_equ_second_no.Text = "" Or alg_equ_third_no.Text = "" Or alg_equ_fourth_no.Text = "" Then
            MsgBox("Please Enter The Values")
        Else

        Dim obj1 As Decimal
        Dim obj2 As Integer

        'Genetibase.MathX.NuGenAlgebricEquations.NumericalAnalysis.NuGenEquationRoots1.RSYNDIV(Val(alg_equ_first_no.Text), Val(alg_equ_second_no.Text), Val(alg_equ_third_no.Text), Val(alg_equ_fourth_no.Text), obj1, obj2)

        alg_equ_result_no.Text = (obj1.ToString)
        End If
    End Sub

    Private Sub alg_equ_secant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles alg_equ_secant.Click


        If alg_equ_first_no.Text = "" Or alg_equ_second_no.Text = "" Or alg_equ_third_no.Text = "" Or alg_equ_fourth_no.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
        Dim obj1 As Decimal
        Dim obj2 As Integer

        Genetibase.MathX.NuGenAlgebricEquations.NumericalAnalysis.NuGenEquationRoots1.SECANT(Val(alg_equ_first_no.Text), Val(alg_equ_second_no.Text), Val(alg_equ_third_no.Text), Val(alg_equ_fourth_no.Text), obj1, obj2)

        alg_equ_result_no.Text = (obj1.ToString)
        End If
    End Sub

    Private Sub alg_equ_znewton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles alg_equ_znewton.Click

        If alg_equ_first_no.Text = "" Or alg_equ_second_no.Text = "" Or alg_equ_third_no.Text = "" Or alg_equ_fourth_no.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
        alg_equ_fourth_no.Text = ""


        Dim obj1 As Decimal
        Dim obj2 As Integer

        Genetibase.MathX.NuGenAlgebricEquations.NumericalAnalysis.NuGenEquationRoots1.ZNEWTON(Val(alg_equ_first_no.Text), Val(alg_equ_second_no.Text), Val(alg_equ_third_no.Text), obj1, obj2)


        alg_equ_result_no.Text = (obj1.ToString)
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Dim next_form As New NuGenAlgerbicEquation_mainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub alg_equ_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles alg_equ_clear.Click

        alg_equ_first_no.Text = ""
        alg_equ_second_no.Text = ""
        alg_equ_third_no.Text = ""
        alg_equ_fourth_no.Text = ""
        alg_equ_result_no.Text = ""
        alg_equ_first_no.Focus()


    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenuToolStripMenuItem.Click
        Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        Me.Visible = False
        next_form.Visible = True
    End Sub
End Class