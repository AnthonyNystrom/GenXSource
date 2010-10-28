Imports Microsoft.VisualBasic.Interaction


Public Class NuGenComplexMethod_BairstowForm

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim a1(Val(m.Text)) As Decimal
        Dim i, j As Integer
        Dim str As String
        str = Nothing
        
        Dim x1, x2, y1, y2, K As Decimal
        If a.Text = "" Or b.Text = "" Or m.Text = "" Or con_criteria.Text = "" Or max_iteration.Text = "" Then

            MsgBox("Please Enter The Number")
        Else
            j = Val(m.Text)
            i = 0
            Do While (i <= j)
                str = Interaction.InputBox("Enter the coefficient")


                a1(i) = Val(str)
                str = Nothing
                i = i + 1

            Loop




            Genetibase.MathX.NuGenAlgebricEquations.NuGenEquationRoots2.BAIRSTOW(Val(m.Text), a1, Val(con_criteria.Text), Val(max_iteration.Text), 0, Val(b.Text), x1, x2, y1, y2, K)

            Label12.Text = String.Concat(Label12.Text, " -> ", x1.ToString)
            Label5.Text = String.Concat(Label5.Text, " ->  ", x2.ToString)
            Label11.Text = String.Concat(Label11.Text, " -> ", y1.ToString)
            Label4.Text = String.Concat(Label4.Text, " ->  ", y2.ToString)
            Label13.Text = String.Concat(Label13.Text, " -> ", K.ToString)
            GroupBox2.Visible = True

        End If
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        a.Text = ""
        b.Text = ""
        m.Text = ""

        con_criteria.Text = ""
        max_iteration.Text = ""

        GroupBox2.Visible = False

        a.Focus()


    End Sub

    Private Sub ComplexMethodMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComplexMethodMenuToolStripMenuItem.Click
        Dim next_form As New NuGenRootsByComplexMethodMainForm

        Me.Visible = False
        next_form.Visible = True
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

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
End Class