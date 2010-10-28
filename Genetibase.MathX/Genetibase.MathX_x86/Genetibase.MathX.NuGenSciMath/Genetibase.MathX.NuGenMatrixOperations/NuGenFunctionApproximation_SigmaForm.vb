Public Class NuGenFunctionApproximation_SigmaForm

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
        m_val.Text = ""
        d_val.Text = ""

        m_val.Focus()

    End Sub

    Private Sub sigma_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sigma.Click

        Dim i As Integer
        Dim str As String = Nothing
        Dim sd As Decimal



        Dim x(Val(n_val.Text) + 1), y(Val(n_val.Text) + 1), d(Val(n_val.Text) + 1) As Decimal

        If n_val.Text = "" Or m_val.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            i = 1
            Do While i < (Val(n_val.Text) + 1)
                str = InputBox("Enter Original Data Set  X()")

                x(i) = Val(str)
                str = Nothing
                i = i + 1
            Loop

            i = 1
            Do While i < (Val(n_val.Text) + 1)
                str = InputBox("Enter Original Data Set  Y()")

                y(i) = Val(str)
                str = Nothing
                i = i + 1
            Loop

            i = 1
            Do While i < (Val(n_val.Text) + 1)
                str = InputBox("Enter Polynomial Coefficient  D()")

                d(i) = Val(str)
                str = Nothing
                i = i + 1
            Loop

            Genetibase.MathX.NuGenMatrixOperations.NuGenFunctionApproximation.Sigma(Val(m_val.Text), Val(n_val.Text), d, x, y, sd)

            d_val.Text = sd.ToString

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