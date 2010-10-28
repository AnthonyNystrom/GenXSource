Imports Genetibase.MathX.NuGenLinearRegression

Public Class N_uGenLinearRegressionForm

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles reg_forth.TextChanged

    End Sub
    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click

    End Sub

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        reg_first.Text = ""
        reg_second.Text = ""
        reg_third.Text = ""
        reg_forth.Text = ""

        slope.Text = ""
        Intercept.Text = ""
        Forecast.Text = ""
        coefficent.Text = ""

        reg_first.Focus()

    End Sub

    Private Sub result_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles result.Click

        Dim obj(4) As Double

        obj(0) = Val(reg_first.Text)
        obj(1) = Val(reg_second.Text)
        obj(2) = Val(reg_third.Text)
        obj(3) = Val(reg_forth.Text)

        Dim obj1 As New Object

        Dim result(4) As Double


        obj1 = Genetibase.MathX.NuGenLinearRegression.NuGenLinearRegression.LRegress(obj, result)

        slope.Text = result(0).ToString
        Intercept.Text = result(1).ToString
        Forecast.Text = result(2).ToString
        coefficent.Text = result(3).ToString


    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub

    Private Sub BackToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
End Class