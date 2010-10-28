Public Class NuGenComplexMethod_ZCircleForm

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim zx, zy As Decimal
        Dim k As Integer



        If x0.Text = "" Or y0.Text = "" Or radius.Text = "" Or no_eval.Text = "" Or no_iteration.Text = "" Or redu_factor.Text = "" Then
            MsgBox("Please Enter The Values")
        Else

            Genetibase.MathX.NuGenAlgebricEquations.NuGenEquationRoots2.ZCIRCLE(Val(x0.Text), Val(y0.Text), Val(radius.Text), Val(redu_factor.Text), Val(no_eval.Text), Val(no_iteration.Text), zx, zy, k)


            zx1.Text = zx.ToString
            zx2.Text = zy.ToString
            k_val.Text = k.ToString

            GroupBox2.Visible = True

        End If
    End Sub

    Private Sub ComplexMethodMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComplexMethodMenuToolStripMenuItem.Click
        Dim next_form As New NuGenRootsByComplexMethodMainForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        x0.Text = ""
        y0.Text = ""
        radius.Text = ""
        no_eval.Text = ""
        no_iteration.Text = ""
        redu_factor.Text = ""
        zx1.Text = ""
        zx2.Text = ""
        k_val.Text = ""

        GroupBox2.Visible = False

        x0.Focus()

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub AlgerbricFunctionMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlgerbricFunctionMenuToolStripMenuItem.Click
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