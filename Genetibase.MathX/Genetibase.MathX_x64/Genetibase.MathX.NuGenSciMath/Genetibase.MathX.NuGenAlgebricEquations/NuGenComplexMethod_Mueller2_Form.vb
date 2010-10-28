Public Class NuGenComplexMethod_Mueller2_Form

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim x, y As Decimal
        Dim k As Integer



        If x0.Text = "" Or y0.Text = "" Or b1.Text = "" Or b2.Text = "" Or con_criteria.Text = "" Or max_iteration.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            Genetibase.MathX.NuGenAlgebricEquations.NuGenEquationRoots2.MUELLER2(Val(x0.Text), Val(b1.Text), Val(y0.Text), Val(b2.Text), Val(con_criteria.Text), Val(max_iteration.Text), x, y, k)

            x_val.Text = x.ToString
            y_val.Text = y.ToString
            k_val.Text = k.ToString

            GroupBox2.Visible = True



        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        x0.Text = ""
        y0.Text = ""
        b1.Text = ""
        b2.Text = ""
        max_iteration.Text = ""
        con_criteria.Text = ""
        x_val.Text = ""
        y_val.Text = ""
        k_val.Text = ""

        GroupBox2.Visible = False
        x0.Focus()
    End Sub

    Private Sub ComplexMethodMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComplexMethodMenuToolStripMenuItem.Click
        Dim next_form As New NuGenRootsByComplexMethodMainForm

        Me.Visible = False
        next_form.Visible = True
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