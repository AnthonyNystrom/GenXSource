Public Class NuGenSeries_ChebyshevForm

    Private Sub SeriesMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeriesMenuToolStripMenuItem.Click
        Dim next_form As New NuGenNumericUseOfSeries_MainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        degree_m.Text = ""
        degree_m1.Text = ""

        range_x0.Text = ""
        Label2.Text = ""
        Label3.Text = ""
        Label2.Text = "Series Of Coefficient Is ->"
        Label3.Text = "Polynomial Coefficient Is ->"
        degree_m.Focus()

    End Sub

    Private Sub Chebyshev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chebyshev.Click

        Dim a(Val(degree_m1.Text)), b(Val(degree_m1.Text), Val(degree_m1.Text)), c(Val(degree_m1.Text)) As Decimal
        Dim i, j As Integer
        Dim str As String = Nothing


        If degree_m.Text = "" Or degree_m1.Text = "" Or range_x0.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            i = 0
            Do While i < Val(degree_m1.Text)
                Str = InputBox("Enter Polynomial Coefficient C()")
                c(i) = Val(Str)
                Str = Nothing

                i = i + 1

            Loop

            i = 0
            Do While i < Val(degree_m1.Text)
                j = 0
                Do While j < Val(degree_m1.Text)

                    str = InputBox("Enter Polynomial Coefficient B( , )")
                    b(i, j) = Val(str)
                    str = Nothing
                    j = j + 1
                Loop

                i = i + 1

            Loop
            Genetibase.MathX.NuGenAlgebricEquations.NuGenNumericalUseOfSeries.CHEBECON(Val(degree_m.Text), Val(degree_m1.Text), Val(range_x0.Text), a, b, c)

            Label2.Text = String.Concat(Label2.Text, " { ")

            i = 0
            Do While i < Val(degree_m1.Text)
                Label2.Text = String.Concat(Label2.Text, a(i).ToString)
                i = i + 1

                If i < Val(degree_m1.Text) Then
                    Label2.Text = String.Concat(Label2.Text, " ,")

                End If

            Loop
            Label2.Text = String.Concat(Label2.Text, "  } ")


            Label3.Text = String.Concat(Label3.Text, " { ")

            i = 0
            Do While i < Val(degree_m1.Text)
                Label3.Text = String.Concat(Label3.Text, c(i).ToString)
                i = i + 1
                If i < Val(degree_m1.Text) Then
                    Label2.Text = String.Concat(Label2.Text, " ,")

                End If
            Loop
            Label3.Text = String.Concat(Label3.Text, " } ")
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