Imports Genetibase.MathX.NuGenMatrixOperations


Public Class NuGen_Matrix

    Public a(,), b(,), c(,) As Decimal


    Private Sub ok_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ok_button.Click
        If (text3_0.Text = "" Or text3_1.Text = "" Or text3_2.Text = "" Or text3_3.Text = "" Or text3_4.Text = "" Or text3_5.Text = "" Or text3_6.Text = "" Or text3_7.Text = "" Or text3_8.Text = "" Or text1_3_0.Text = "" Or text1_3_1.Text = "" Or text1_3_2.Text = "" Or text1_3_3.Text = "" Or text1_3_4.Text = "" Or text1_3_5.Text = "" Or text1_3_6.Text = "" Or text1_3_7.Text = "" Or text1_3_8.Text = "") Then
            MsgBox("Please Enter Both The Metrices Correctly.")

        Else
            Dim temp(8) As Decimal

            temp(0) = Val(text3_0.Text)
            temp(1) = Val(text3_1.Text)
            temp(2) = Val(text3_2.Text)
            temp(3) = Val(text3_3.Text)
            temp(4) = Val(text3_4.Text)
            temp(5) = Val(text3_5.Text)
            temp(6) = Val(text3_6.Text)
            temp(7) = Val(text3_7.Text)
            temp(8) = Val(text3_8.Text)


            a = Genetibase.MathX.NuGenMatrixOperations.NuGenMatrixOperation.InitMatrix(3, 3, temp)


            temp(0) = Val(text1_3_0.Text)
            temp(1) = Val(text1_3_1.Text)
            temp(2) = Val(text1_3_2.Text)
            temp(3) = Val(text1_3_3.Text)
            temp(4) = Val(text1_3_4.Text)
            temp(5) = Val(text1_3_5.Text)
            temp(6) = Val(text1_3_6.Text)
            temp(7) = Val(text1_3_7.Text)
            temp(8) = Val(text1_3_8.Text)


            b = Genetibase.MathX.NuGenMatrixOperations.NuGenMatrixOperation.InitMatrix(3, 3, temp)


            mat_add.Enabled = True
            mat_sub.Enabled = True




        End If
    End Sub

    Private Sub mat_add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mat_add.Click

        c = Genetibase.MathX.NuGenMatrixOperations.NuGenMatrixOperation.MATADD(3, 3, a, b)

        display_results()

    End Sub

    Public Sub display_results()

        Table_output.Visible = True

        text_o_0.Text = c(1, 1).ToString
        text_o_1.Text = c(1, 2).ToString
        text_o_2.Text = c(1, 3).ToString

        text_o_3.Text = c(2, 1).ToString
        text_o_4.Text = c(2, 2).ToString
        text_o_5.Text = c(2, 3).ToString

        text_o_6.Text = c(3, 1).ToString
        text_o_7.Text = c(3, 2).ToString
        text_o_8.Text = c(3, 3).ToString

    End Sub

    Private Sub mat_sub_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mat_sub.Click

        Genetibase.MathX.NuGenMatrixOperations.NuGenMatrixOperation.MATSUB(3, 3, a, b, c)

        display_results()

    End Sub

    Private Sub clear_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear_button.Click

        text3_0.Text = ""
        text3_1.Text = ""
        text3_2.Text = ""
        text3_3.Text = ""
        text3_4.Text = ""
        text3_5.Text = ""
        text3_6.Text = ""
        text3_7.Text = ""
        text3_8.Text = ""

        text1_3_0.Text = ""
        text1_3_1.Text = ""
        text1_3_2.Text = ""
        text1_3_3.Text = ""
        text1_3_4.Text = ""
        text1_3_5.Text = ""
        text1_3_6.Text = ""
        text1_3_7.Text = ""
        text1_3_8.Text = ""

        text3_0.Focus()

        mat_add.Enabled = False
        mat_sub.Enabled = False


    End Sub

    Private Sub exitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub mat_mult_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mat_mult.Click

    End Sub

    Private Sub mat_div_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mat_div.Click

    End Sub

    Private Sub mat_equ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mat_equ.Click

    End Sub

    Private Sub mathematicalOperationsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class