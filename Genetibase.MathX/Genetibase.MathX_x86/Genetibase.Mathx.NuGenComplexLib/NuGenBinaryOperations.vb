Imports Genetibase.Mathx.NuGenComplexLib


Public Class NuGenBinaryOperations

    Private Sub clear_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear_button.Click
        no1.Text = ""
        no2.Text = ""
        Result_Addition.Text = ""
        no1.Focus()

    End Sub

    Private Sub operation_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If no1.Text = "" Or no2.Text = "" Then
            MsgBox("Please enter both the numbers to ADD.")

        Else

            Result_Addition.Text = Genetibase.Mathx.NuGenComplexLib.NuGenBinary.BinaryAddition(no1.Text, no2.Text)

        End If


    End Sub

    Private Sub BackToBinaryMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackToBinaryMenuToolStripMenuItem.Click
        Me.Visible = False
        Dim next_form As New NuGenBinaryMenu
        next_form.Visible = True

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub Addition_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Binary_logical.Enter

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        not_no1.Text = ""
        result_not.Text = ""
        not_no1.Focus()

    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If no1.Text = "" Or no2.Text = "" Then
            MsgBox("Please enter both the numbers to AND")

        Else

            Result_Addition.Text = Genetibase.Mathx.NuGenComplexLib.NuGenBinary.Binary_And(no1.Text, no2.Text)

        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If no1.Text = "" Or no2.Text = "" Then
            MsgBox("Please enter both the numbers to OR")

        Else

            Result_Addition.Text = Genetibase.Mathx.NuGenComplexLib.NuGenBinary.Binary_Or(no1.Text, no2.Text)

        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If no1.Text = "" Or no2.Text = "" Then
            MsgBox("Please enter both the numbers to X-OR")

        Else

            Result_Addition.Text = Genetibase.Mathx.NuGenComplexLib.NuGenBinary.Binary_Xor(no1.Text, no2.Text)

        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If no1.Text = "" Or no2.Text = "" Then
            MsgBox("Please enter both the numbers to NAND")

        Else

            Result_Addition.Text = Genetibase.Mathx.NuGenComplexLib.NuGenBinary.Binary_Nand(no1.Text, no2.Text)

        End If
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        If no1.Text = "" Or no2.Text = "" Then
            MsgBox("Please enter both the numbers to XNOR")

        Else

            Result_Addition.Text = Genetibase.Mathx.NuGenComplexLib.NuGenBinary.Binary_NXor(no1.Text, no2.Text)

        End If
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If no1.Text = "" Or no2.Text = "" Then
            MsgBox("Please enter both the numbers to NOR")

        Else

            Result_Addition.Text = Genetibase.Mathx.NuGenComplexLib.NuGenBinary.Binary_Nor(no1.Text, no2.Text)

        End If
    End Sub

    Private Sub NuGenBinaryOperations_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If not_no1.Text = "" Then
            MsgBox("Please enter the numbers to find NOT")

        Else

            result_not.Text = Genetibase.Mathx.NuGenComplexLib.NuGenBinary.Binary_Not(not_no1.Text)

        End If
    End Sub

    Private Sub not_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles not_clear.Click
        not_no1.Text = ""
        result_not.Text = ""
        not_no1.Focus()

    End Sub

    Private Sub MenuStrip1_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        first_no.Text = ""
        second_no.Text = ""
        result_oper.Text = ""
        first_no.Focus()

    End Sub

    Private Sub binary_add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles binary_add.Click

        Dim obj As New Genetibase.Mathx.NuGenComplexLib.NuGenSignedLogical

        If first_no.Text = "" Or second_no.Text = "" Then
            MsgBox("Please enter both the numbers to ADD")

        Else

            result_oper.Text = obj.BinaryAddition(first_no.Text, second_no.Text)

        End If
    End Sub

    Private Sub binary_sub_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles binary_sub.Click
        Dim obj As New Genetibase.Mathx.NuGenComplexLib.NuGenSignedLogical

        If first_no.Text = "" Or second_no.Text = "" Then
            MsgBox("Please enter both the numbers to SUB")

        Else

            result_oper.Text = obj.BinarySubtraction(first_no.Text, second_no.Text)

        End If
    End Sub

    Private Sub binary_mult_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles binary_mult.Click
        Dim obj As New Genetibase.Mathx.NuGenComplexLib.NuGenSignedLogical

        If first_no.Text = "" Or second_no.Text = "" Then
            MsgBox("Please enter both the numbers to Multiply")

        Else

            result_oper.Text = obj.BinaryMultiplication(first_no.Text, second_no.Text)

        End If
    End Sub

    Private Sub binary_div_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles binary_div.Click
        Dim obj As New Genetibase.Mathx.NuGenComplexLib.NuGenSignedLogical

        If first_no.Text = "" Or second_no.Text = "" Then
            MsgBox("Please enter both the numbers to Divide")

        Else

            result_oper.Text = obj.BinaryIntegerDivision(first_no.Text, second_no.Text)

        End If
    End Sub

    Private Sub binary_mod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles binary_mod.Click
        Dim obj As New Genetibase.Mathx.NuGenComplexLib.NuGenSignedLogical

        If first_no.Text = "" Or second_no.Text = "" Then
            MsgBox("Please enter both the numbers to Calculate Modulo")

        Else

            result_oper.Text = obj.BinaryModulo(first_no.Text, second_no.Text)

        End If
    End Sub

    Private Sub NumericMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericMenuToolStripMenuItem.Click
        Dim next_form As New NuGen_Numeric_menu
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub BackToToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackToToolStripMenuItem.Click
        Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        Me.Visible = False
        next_form.Visible = True
    End Sub
End Class