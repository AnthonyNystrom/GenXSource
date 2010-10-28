Public Class NuGenFunctionApproximation_ParametricLeastSquareForm

    Private Sub AlgebricToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlgebricToolStripMenuItem.Click
        Dim next_form As New NuGenFunctionApproximation_Main_Form
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        n_val.Text = ""
        l_val.Text = ""
        e_val.Text = ""
        e1_val.Text = ""

        a_val.Text = ""
        d_val.Text = ""

        n_val.Focus()


    End Sub

    Private Sub parametric_least_square_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles parametric_least_square.Click
        Dim d As Decimal
        Dim i As Integer
        Dim str As String = Nothing


        Dim y(Val(l_val.Text)), a(Val(l_val.Text)) As Decimal
        Dim x(Val(l_val.Text)) As Integer

        If n_val.Text = "" Or l_val.Text = "" Or e_val.Text = "" Or e1_val.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            i = 0
            Do While i < (Val(n_val.Text))
                str = InputBox("Enter Series X()")

                x(i) = Val(str)
                str = Nothing
                i = i + 1
            Loop

            i = 0
            Do While i < (Val(n_val.Text))
                str = InputBox("Enter Series y()")

                y(i) = Val(str)
                str = Nothing
                i = i + 1
            Loop

            Genetibase.MathX.NuGenMatrixOperations.NuGenFunctionApproximation.PARAFIT(Val(n_val.Text), Val(l_val.Text), Val(e1_val.Text), Val(e_val.Text), x, y, a, d, 0)

            d_val.Text = d.ToString

            a_val.Text = Nothing
            a_val.Text = String.Concat(a_val.Text, " {")

            i = 0
            Do While i < Val(n_val.Text)
                a_val.Text = String.Concat(a_val.Text, a(i).ToString)
                i = i + 1
                If i < Val(n_val.Text) Then
                    a_val.Text = String.Concat(a_val.Text, " ,")
                Else
                    a_val.Text = String.Concat(a_val.Text, " }")
                End If
            Loop





        End If


        
    End Sub

    Private Sub SeriesMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeriesMenuToolStripMenuItem.Click
        Dim next_form As New NuGen_operations_Menu
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class