Imports Genetibase.MathX.NuGenMatrixOperations


Public Class NuGen_Linear_System_Solver

    Dim data1(2, 2), data2(2), data_out(2) As Decimal


    Private Sub ok_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ok_button.Click

        If (text1_3_0.Text = "" Or text1_3_1.Text = "" Or text1_3_2.Text = "" Or text3_0.Text = "" Or text3_1.Text = "" Or text3_2.Text = "") Then

            MsgBox("Please Enter Both The Metrices Correctly.")
        Else




            'decimal[,] data2 = new decimal[3, 3];


            data1(0, 0) = Val(text3_0.Text)
            data1(0, 1) = Val(text3_1.Text)
            data1(0, 2) = Val(text3_2.Text)
            data1(1, 0) = Val(text3_3.Text)
            data1(1, 1) = Val(text3_4.Text)
            data1(1, 2) = Val(text3_5.Text)
            data1(2, 0) = Val(text3_6.Text)
            data1(2, 1) = Val(text3_7.Text)
            data1(2, 2) = Val(text3_8.Text)

            data2(0) = Val(text1_3_0.Text)
            data2(1) = Val(text1_3_1.Text)
            data2(2) = Val(text1_3_2.Text)

            Me.Gauss_button.Enabled = True



        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        text1_3_0.Text = ""
        text1_3_1.Text = ""
        text1_3_2.Text = ""


        text3_0.Text = ""
        text3_1.Text = ""
        text3_2.Text = ""
        text3_3.Text = ""
        text3_4.Text = ""
        text3_5.Text = ""
        text3_6.Text = ""
        text3_7.Text = ""
        text3_8.Text = ""

        text3_0.Focus()

        Me.Gauss_button.Enabled = False
    End Sub

    Private Sub Gauss_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Gauss_button.Click

        Dim obj1 As New Genetibase.MathX.NuGenMatrixOperations.NuGenLinearSystemSolver

        Dim var_str As Genetibase.MathX.NuGenMatrixOperations.NuGenLinearSystemSolver.ResultStructure

        var_str = obj1.SOLVE(data1, data2, Val(text_out.Text))

        Label3.Text = "The Determinant is : "

        text_out.Text = var_str.Determinant.ToString()

        Me.Gauss_button.Enabled = True



    End Sub

    Private Sub exitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub mathematicalOperationsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mathematicalOperationsToolStripMenuItem.Click
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