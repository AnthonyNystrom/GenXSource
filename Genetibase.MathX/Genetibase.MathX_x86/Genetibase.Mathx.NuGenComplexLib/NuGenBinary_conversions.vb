Imports Genetibase.Mathx.NuGenComplexLib


Public Class NuGenBinary_conversions

    Private Sub BackToToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackToToolStripMenuItem.Click
        Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub BackToBinaryMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackToBinaryMenuToolStripMenuItem.Click
        Me.Visible = False
        Dim next_form As New NuGenBinaryMenu
        next_form.Visible = True

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub binary_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles binary_clear.Click
        first_no.Text = ""
        result_oper.Text = ""
        first_no.Focus()

    End Sub

    Private Sub binary_p_to_n_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles binary_p_to_n.Click

        Dim obj As New Genetibase.Mathx.NuGenComplexLib.NuGenSignedLogical

        If first_no.Text = "" Then
            MsgBox("Please Enter Number.")

        Else
            result_oper.Text = obj.BinaryPositiveToNegative(first_no.Text)
        End If

    End Sub

    Private Sub binary_shift_left_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles binary_shift_left.Click
        Dim obj As New Genetibase.Mathx.NuGenComplexLib.NuGenSignedLogical

        If first_no.Text = "" Then
            MsgBox("Please Enter Number.")

        Else
            result_oper.Text = obj.BinaryShiftLeft(first_no.Text)
        End If
    End Sub

    Private Sub binary_shift_right_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles binary_shift_right.Click
        Dim obj As New Genetibase.Mathx.NuGenComplexLib.NuGenSignedLogical

        If first_no.Text = "" Then
            MsgBox("Please Enter Number.")

        Else
            result_oper.Text = obj.BinaryShiftRight(first_no.Text)
        End If
    End Sub

    Private Sub binary_hexa_to_binary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles binary_hexa_to_binary.Click
        Dim obj As New Genetibase.Mathx.NuGenComplexLib.NuGenSignedLogical

        If first_no.Text = "" Then
            MsgBox("Please Enter Number.")

        Else
            result_oper.Text = obj.HexadecimalToBinary(first_no.Text)
        End If
    End Sub

    Private Sub binary_hexa_to_deci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles binary_hexa_to_deci.Click
        Dim obj As New Genetibase.Mathx.NuGenComplexLib.NuGenSignedLogical

        If first_no.Text = "" Then
            MsgBox("Please Enter Number.")

        Else
            result_oper.Text = obj.HexadecimalToDecimal(first_no.Text)
        End If
    End Sub

    Private Sub binary_to_deci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles binary_to_deci.Click
        Dim obj As New Genetibase.Mathx.NuGenComplexLib.NuGenSignedLogical

        If first_no.Text = "" Then
            MsgBox("Please Enter Number.")

        Else
            result_oper.Text = obj.BinaryToDecimal(first_no.Text)
        End If
    End Sub

    Private Sub Binary_to_hexa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Binary_to_hexa.Click
        Dim obj As New Genetibase.Mathx.NuGenComplexLib.NuGenSignedLogical

        If first_no.Text = "" Then
            MsgBox("Please Enter Number.")

        Else
            result_oper.Text = obj.BinaryToHexadecimal(first_no.Text)
        End If
    End Sub

    Private Sub binary_deci_to_binary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles binary_deci_to_binary.Click
        Dim obj As New Genetibase.Mathx.NuGenComplexLib.NuGenSignedLogical

        If first_no.Text = "" Then
            MsgBox("Please Enter Number.")

        Else
            result_oper.Text = obj.DecimalToBinary(first_no.Text)
        End If
    End Sub

    Private Sub binary_deci_to_hexa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles binary_deci_to_hexa.Click
        Dim obj As New Genetibase.Mathx.NuGenComplexLib.NuGenSignedLogical

        If first_no.Text = "" Then
            MsgBox("Please Enter Number.")

        Else
            result_oper.Text = obj.DecimalToHexadecimal(first_no.Text)
        End If
    End Sub

    Private Sub NuGenNumericMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NuGenNumericMenuToolStripMenuItem.Click
        Dim next_form As New NuGen_Numeric_menu
        Me.Visible = False
        next_form.Visible = True
    End Sub
End Class