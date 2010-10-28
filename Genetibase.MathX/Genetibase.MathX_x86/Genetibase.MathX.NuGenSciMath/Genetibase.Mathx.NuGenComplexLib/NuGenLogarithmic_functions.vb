Imports Genetibase.Mathx.NuGenComplexLib



Public Class NuGenLogarithmic_functions

    Private Sub FactorialToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FactorialToolStripMenuItem.Click
        Factorial.Visible = True
        Logn_group.Visible = False
        Log_group.Visible = False


    End Sub



    Private Sub LogNToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogNToolStripMenuItem.Click
        Factorial.Visible = False
        Logn_group.Visible = True
        Log_group.Visible = False


    End Sub

    Private Sub LogN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogN.Click
        If logn_first_no.Text = "" Or logn_sec_no.Text = "" Then
            MsgBox("Please Enter Number")
        Else
            logn_result.Text = Genetibase.Mathx.NuGenComplexLib.NuGenLogaritmicFunctions.LogN(Val(logn_first_no.Text), Val(logn_sec_no.Text))

        End If
    End Sub

    Private Sub Log_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Log_button.Click
        If log_first_no.Text = "" Then
            MsgBox("Please Enter Number")
        Else
            log_result.Text = Genetibase.Mathx.NuGenComplexLib.NuGenLogaritmicFunctions.Lg(Val(log_first_no.Text))

        End If
    End Sub

    Private Sub LogToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogToolStripMenuItem.Click
        Factorial.Visible = False
        Logn_group.Visible = False
        Log_group.Visible = True
    End Sub

    Private Sub Factorial_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Factorial_button.Click
        If First_no.Text = "" Then
            MsgBox("Please Enter The Number")
        Else
            Second_no.Text = Genetibase.Mathx.NuGenComplexLib.NuGenLogaritmicFunctions.Factorial(Val(First_no.Text))

        End If

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub NumericalMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericalMenuToolStripMenuItem.Click
        Dim next_form As New NuGen_Numeric_menu
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class