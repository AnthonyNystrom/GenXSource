Imports Genetibase.MathX.NuGenMatrixOperations

Public Class NuGen_Vector_operations

    Public a(3), b(3), c(3) As Decimal

    Private Sub Button_add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_add.Click

        Label5.Visible = False

        Genetibase.MathX.NuGenMatrixOperations.NuGenMatrixOperation.VECTADD(3, a, b, c)

        display_results()

    End Sub

    Private Sub Button_ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ok.Click

        If (Text_a_1.Text = "" Or Text_a_2.Text = "" Or Text_a_3.Text = "" Or Text_b_1.Text = "" Or Text_b_2.Text = "" Or Text_b_3.Text = "") Then
            MsgBox("Please Enter All The Values For Both The Vectors")
        Else
            Button_add.Enabled = True
            Button_sub.Enabled = True
            Button_cross.Enabled = True
            Button_dot.Enabled = True
            Button_angle.Enabled = True

            form_vectors()

            Label5.Visible = False

        End If
    End Sub

    Public Sub form_vectors()

        a(1) = Val(Text_a_1.Text)
        a(2) = Val(Text_a_2.Text)
        a(3) = Val(Text_a_3.Text)

        b(1) = Val(Text_b_1.Text)
        b(2) = Val(Text_b_2.Text)
        b(3) = Val(Text_b_3.Text)

    End Sub

    Public Sub display_results()

        Text_1.Text = c(1).ToString
        Text_2.Text = c(2).ToString
        Text_3.Text = c(3).ToString

        Button_add.Enabled = False
        Button_sub.Enabled = False
        Button_cross.Enabled = False
        Button_dot.Enabled = False
        Button_angle.Enabled = False

        form_vectors()

        Vector_out.Visible = True

        Label5.Visible = False

    End Sub



    Private Sub exitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub Button_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_clear.Click

        Label5.Visible = False
        Text_a_1.Text = ""
        Text_a_2.Text = ""
        Text_a_3.Text = ""
        Text_b_1.Text = ""
        Text_b_2.Text = ""
        Text_b_3.Text = ""

        Text_a_1.Focus()

    End Sub

    Private Sub NuGen_Vector_operations_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Text_a_1.Focus()

    End Sub

    Private Sub Button_sub_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_sub.Click

        Label5.Visible = False

        Genetibase.MathX.NuGenMatrixOperations.NuGenMatrixOperation.VECTSUB(3, a, b, c)

        display_results()

    End Sub

    Private Sub Button_cross_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_cross.Click

        Label5.Visible = False

        Genetibase.MathX.NuGenMatrixOperations.NuGenMatrixOperation.VECTCURL(3, a, b, c)

        display_results()

    End Sub

    Private Sub Button_dot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_dot.Click

        Dim temp As Decimal

        Vector_out.Visible = False

        Genetibase.MathX.NuGenMatrixOperations.NuGenMatrixOperation.VECTDOT(3, a, b, temp)

        Label5.Text = "A . B =   " + temp.ToString

        Label5.Visible = True

        ' display_results()

    End Sub

    Private Sub Button_angle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_angle.Click

        Dim temp As Decimal

        Vector_out.Visible = False

        Genetibase.MathX.NuGenMatrixOperations.NuGenMatrixOperation.VECTANGL(3, a, b, temp)

        Label5.Text = "A angle B =   " + temp.ToString

        Label5.Visible = True

    End Sub

    Private Sub mathematicalOperationsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mathematicalOperationsToolStripMenuItem.Click
        Dim next_form As New NuGen_operations_Menu
        Me.Visible = False
        next_form.Visible = True

    End Sub

    Private Sub functionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles functionsToolStripMenuItem.Click

    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class