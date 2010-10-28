Public Class NuGenRootComplexMethod_AllRoots

    Private Sub TextBox7_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Flag.TextChanged

    End Sub
    Private Sub Label9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label9.Click

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        x0.Text = ""
        y0.Text = ""
        b1.Text = ""
        b2.Text = ""
        con_criteria.Text = ""
        max_iteration.Text = ""
        no_root.Text = ""
        Flag.Text = ""



        GroupBox2.Visible = False

        x0.Focus()


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim x(6), y(6) As Decimal
        Dim k, i As Integer
        Dim str As String
        str = Nothing

        If x0.Text = "" Or y0.Text = "" Or b1.Text = "" Or b2.Text = "" Or con_criteria.Text = "" Or max_iteration.Text = "" Or no_root.Text = "" Or Flag.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            Genetibase.MathX.NuGenAlgebricEquations.NuGenEquationRoots2.ALLROOT(Val(x0.Text), Val(b1.Text), Val(y0.Text), Val(b2.Text), Val(con_criteria.Text), Val(max_iteration.Text), Val(no_root.Text), Val(Flag.Text), x, y, k)

            For i = 0 To i <= x.Length Step 1
                str = String.Concat(str, x(i).ToString)
            Next
            Label12.Text = String.Concat(Label12.Text, " ", str)
            'Label12.Text = str

            str = Nothing
            For i = 0 To i <= y.Length Step 1
                str = String.Concat(str, y(i).ToString)
            Next
            Label11.Text = String.Concat(Label11.Text, " ", str)
            'Label11.Text = str

            Label13.Text = String.Concat(Label13.Text, " ", k.ToString)

            GroupBox2.Visible = True



        End If
    End Sub

    Private Sub ComplexMethodMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComplexMethodMenuToolStripMenuItem.Click
        Dim next_form As New NuGenRootsByComplexMethodMainForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub b1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles b1.TextChanged

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