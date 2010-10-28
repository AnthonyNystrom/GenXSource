Public Class NuGenComplexMethod_QudraticForm

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim a(2) As Decimal
        Dim x1, x2, y1, y2 As Decimal


        If a1.Text = "" Or a2.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            a(0) = Val(a1.Text)
            a(1) = Val(a2.Text)
            Genetibase.MathX.NuGenAlgebricEquations.NuGenEquationRoots2.QUADRAT(a, x1, x2, y1, y2)

            r1.Text = String.Concat(r1.Text, x1.ToString, " + I", y1.ToString)
            r2.Text = String.Concat(r2.Text, x2.ToString, " + I", y2.ToString)
            GroupBox2.Visible = True

        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        a2.Text = ""
        a1.Text = ""
        r1.Text = ""
        r2.Text = ""
        GroupBox2.Visible = False
        a2.Focus()
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