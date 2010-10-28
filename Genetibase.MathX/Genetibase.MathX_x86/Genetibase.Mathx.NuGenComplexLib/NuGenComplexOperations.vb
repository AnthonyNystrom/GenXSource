Public Class NuGenComplexOperations

    Private Sub AdditionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdditionToolStripMenuItem.Click
        Complex_mathematic_groupbox.Visible = True

    End Sub

    Private Sub Complex_add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Complex_add.Click
        Dim obj_first, obj_second, obj_result As New Genetibase.Mathx.NuGenComplexLib.NuGenComplexLib

        If complex_first_no.Text = "" Or complex_second_no.Text = "" Then
            MsgBox("Please Enter The Number")
        Else
            obj_first = obj_first.stringToCplxNum(complex_first_no.Text)
            obj_second = obj_second.stringToCplxNum(complex_second_no.Text)

            obj_result = obj_first.Add(obj_second)
            complex_result.Text = ""
            complex_result.Text = obj_result.cplxNumToString(, , )

        End If
    End Sub

    Private Sub complex_mult_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles complex_mult.Click
        Dim obj_first, obj_second, obj_result As New Genetibase.Mathx.NuGenComplexLib.NuGenComplexLib

        If complex_first_no.Text = "" Or complex_second_no.Text = "" Then
            MsgBox("Please Enter The Number")
        Else
            obj_first = obj_first.stringToCplxNum(complex_first_no.Text)
            obj_second = obj_second.stringToCplxNum(complex_second_no.Text)

            obj_result = obj_first.Multiply(obj_second)
            complex_result.Text = ""
            complex_result.Text = obj_result.cplxNumToString(, , )

        End If
    End Sub

    Private Sub complex_sub_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles complex_sub.Click
        Dim obj_first, obj_second, obj_result As New Genetibase.Mathx.NuGenComplexLib.NuGenComplexLib

        If complex_first_no.Text = "" Or complex_second_no.Text = "" Then
            MsgBox("Please Enter The Number")
        Else
            obj_first = obj_first.stringToCplxNum(complex_first_no.Text)
            obj_second = obj_second.stringToCplxNum(complex_second_no.Text)

            obj_result = obj_first.Subtract(obj_second)
            complex_result.Text = ""
            complex_result.Text = obj_result.cplxNumToString(, , )

        End If
    End Sub

    Private Sub complex_div_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles complex_div.Click
        Dim obj_first, obj_second, obj_result As New Genetibase.Mathx.NuGenComplexLib.NuGenComplexLib

        If complex_first_no.Text = "" Or complex_second_no.Text = "" Then
            MsgBox("Please Enter The Number")
        Else
            obj_first = obj_first.stringToCplxNum(complex_first_no.Text)
            obj_second = obj_second.stringToCplxNum(complex_second_no.Text)

            obj_result = obj_first.Divide(obj_second)
            complex_result.Text = ""
            complex_result.Text = obj_result.cplxNumToString(, , )

        End If
    End Sub

    Private Sub complex_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles complex_clear.Click
        complex_first_no.Text = ""
        complex_second_no.Text = ""
        complex_result.Text = ""

        complex_first_no.Focus()



    End Sub

    Private Sub SubtractionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SubtractionToolStripMenuItem.Click
        Complex_mathematic_groupbox.Visible = True

    End Sub

    Private Sub MultiplicationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MultiplicationToolStripMenuItem.Click
        Complex_mathematic_groupbox.Visible = True

    End Sub

    Private Sub DivisionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DivisionToolStripMenuItem.Click
        Complex_mathematic_groupbox.Visible = True

    End Sub

    Private Sub NumericalOperationMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericalOperationMenuToolStripMenuItem.Click
        Dim next_form As New NuGen_Numeric_menu
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenuToolStripMenuItem.Click
        Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        Me.Visible = False
        next_form.Visible = True
    End Sub
End Class