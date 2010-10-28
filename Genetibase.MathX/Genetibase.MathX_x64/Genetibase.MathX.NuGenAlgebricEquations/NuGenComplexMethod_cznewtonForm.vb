Public Class NuGenComplexMethod_cznewtonForm

    Private Sub ComplexMethodMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComplexMethodMenuToolStripMenuItem.Click
        Dim next_form As New NuGenRootsByComplexMethodMainForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim x, y As Decimal
        Dim k As Integer


        If x0.Text = "" Or y0.Text = "" Or con_criteria.Text = "" Or max_iteration.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            Genetibase.MathX.NuGenAlgebricEquations.NuGenEquationRoots2.CZNEWTON(Val(x0.Text), Val(y0.Text), Val(con_criteria.Text), Val(max_iteration.Text), x, y, k)

            val_x.Text = x.ToString
            val_y.Text = y.ToString
            val_k.Text = k.ToString

           
            GroupBox2.Visible = True
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        x0.Text = ""
        y0.Text = ""
       
        con_criteria.Text = ""
        max_iteration.Text = ""
        val_x.Text = ""
        val_y.Text = ""
        val_k.Text = ""



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