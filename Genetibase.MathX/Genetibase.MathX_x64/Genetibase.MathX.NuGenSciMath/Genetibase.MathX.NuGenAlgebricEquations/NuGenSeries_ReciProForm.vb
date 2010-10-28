Public Class NuGenSeries_ReciProForm

    Private Sub SeriesMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeriesMenuToolStripMenuItem.Click
        Dim next_form As New NuGenNumericUseOfSeries_MainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        degree_m.Text = ""
        degree_n.Text = ""

        Label2.Text = Nothing
        Label2.Text = String.Concat(Label2.Text, "Series Of Coefficient Is ->")

        degree_n.Focus()

    End Sub

    Private Sub Reciprocal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Reciprocal.Click
        Dim value, i As Integer
        value = Val(degree_n.Text) + (Val(degree_m.Text) - Val(degree_n.Text))
        Dim str As String = Nothing

        Dim a(value), b(value) As Decimal

        If degree_m.Text = "" Or degree_n.Text = "" Then
            MsgBox("Please Enter The Value")
        Else
            i = 0
            Do While i < Val(degree_n.Text)
                str = InputBox("Enter The value for Series Coefficent A()")
                a(i) = str
                str = Nothing
                i = i + 1
            Loop
            Genetibase.MathX.NuGenAlgebricEquations.NuGenNumericalUseOfSeries.RECIPRO(Val(degree_n.Text), Val(degree_m.Text), a, b)

            Label2.Text = String.Concat(Label2.Text, " { ")
            i = 0
            Do While i < value
                Label2.Text = String.Concat(Label2.Text, b(i))
                i = i + 1

                If i < value Then
                    Label2.Text = String.Concat(Label2.Text, " ,")
                Else
                    Label2.Text = String.Concat(Label2.Text, " }")
                End If
            Loop

        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub AlgebricToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlgebricToolStripMenuItem.Click
        Dim next_form As New NuGenAlgerbicEquation_mainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class