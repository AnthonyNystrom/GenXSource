Public Class NuGen_Numeric_menu

    Private Sub button_ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_ok.Click
        If (option_roots.Checked) Then
            Dim next_form As New Finding_Roots
            Me.Visible = False
            next_form.Visible = True
        ElseIf (option_binary.Checked) Then

            Dim next_form As New NuGenBinaryMenu
            Me.Visible = False
            next_form.Visible = True

        ElseIf (option_complex.Checked) Then
            Dim next_form As New NuGenComplexOperations
            Me.Visible = False
            next_form.Visible = True

        ElseIf (option_log.Checked) Then
            Dim next_form As New NuGenLogarithmic_functions
            Me.Visible = False
            next_form.Visible = True

        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End

    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class