Public Class NuGenComplexMethod_RootNumForm

    Private Sub ComplexMethodMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComplexMethodMenuToolStripMenuItem.Click
        Dim next_form As New NuGenRootsByComplexMethodMainForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim nr As Decimal
        Dim a As Integer



        If x0.Text = "" Or y0.Text = "" Or radius.Text = "" Or no_eval.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
           
            Genetibase.MathX.NuGenAlgebricEquations.NuGenEquationRoots2.ROOTNUM(Val(x0.Text), Val(y0.Text), Val(radius.Text), Val(no_eval.Text), nr, a)

            no_root.Text = nr.ToString
            a_val.Text = a.ToString
            GroupBox2.Visible = True

        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        x0.Text = ""
        y0.Text = ""
        radius.Text = ""
        no_eval.Text = ""
        no_root.Text = ""
        a_val.Text = ""

        GroupBox2.Visible = False

        x0.Focus()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub AlgerbricFunctionMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlgerbricFunctionMenuToolStripMenuItem.Click
        Dim next_form As New NuGenAlgerbicEquation_mainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenuToolStripMenuItem.Click
        Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        Me.Visible = False
        next_form.Visible = True
    End Sub
End Class