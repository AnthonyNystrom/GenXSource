Public Class NuGenPolynomial_AdditionForm

    Private Sub addition_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addition.Click

       

        Dim i, value As Integer
        Dim str As String
        str = Nothing


        If a_val.Text = "" Or b_val.Text = "" Then
            MsgBox("Please Enter The Value")
        Else


            If Val(a_val.Text) > Val(b_val.Text) Then
                value = Val(b_val.Text)
            Else
                value = Val(a_val.Text)
            End If


            Dim a1(value), a2(value), a3(value) As Decimal

            i = 0
            Do While i < value

                str = InputBox("Enter Value For A1")
                a1(i) = Val(str)
                str = Nothing
                i = i + 1
            Loop
            str = Nothing
            i = 0
            Do While i < value

                str = InputBox("Enter Value For A2")
                a2(i) = Val(str)
                str = Nothing
                i = i + 1
            Loop

            a3 = Genetibase.MathX.NuGenAlgebricEquations.NuGenPolynomial.Addition(Val(a_val.Text), Val(b_val.Text), a1, a2)

            Label3.Text = String.Concat(Label3.Text, "{ ")
            i = 0
            Do While i < value
                Label3.Text = String.Concat(Label3.Text, a3(i))
                
                i = i + 1
                If i < value Then
                    Label3.Text = String.Concat(Label3.Text, " , ")
                Else
                    Label3.Text = String.Concat(Label3.Text, " } ")

                End If

            Loop



        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        a_val.Text = ""
        b_val.Text = ""
        Label3.Text = ""
        Label3.Text = "Addition Result Is ->"

        a_val.Focus()

    End Sub

    Private Sub NuGenPolynomial_AdditionForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GroupBox1.Visible = False
        GroupBox2.Visible = False
        GroupBox3.Visible = False
    End Sub

    Private Sub Multiplication_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Multiplication.Click

        Dim i, value, val1, val2 As Integer
        Dim str As String
        str = Nothing


        If a_mul.Text = "" Or b_mul.Text = "" Then
            MsgBox("Please Enter The Value")
        Else
            value = Val(a_mul.Text) + Val(b_mul.Text)
            val1 = Val(a_mul.Text)
            val2 = Val(b_mul.Text)

            Dim a1(val1), a2(val2), a3(value) As Decimal

            i = 0
            Do While i < val1

                str = Interaction.InputBox("Enter The Value For Multiplicant")

                a1(i) = Val(str)
                str = Nothing
                i = i + 1
            Loop
            str = Nothing
            i = 0
            Do While i < val2

                str = Interaction.InputBox("Enter The Value For Multiplier")
                a2(i) = Val(str)
                str = Nothing
                i = i + 1
            Loop

            a3 = Genetibase.MathX.NuGenAlgebricEquations.NuGenPolynomial.Multiplication(Val(a_mul.Text), Val(b_mul.Text), a1, a2)

            Label4.Text = String.Concat(Label4.Text, "{ ")
            i = 0
            Do While i < value
                Label4.Text = String.Concat(Label4.Text, a3(i))

                i = i + 1
                If i < value Then
                    Label4.Text = String.Concat(Label4.Text, " , ")
                Else
                    Label4.Text = String.Concat(Label4.Text, " } ")

                End If

            Loop



        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        a_mul.Text = ""
        b_mul.Text = ""
        Label4.Text = ""
        Label4.Text = "Result Of Functionality Is ->"
        a_val.Focus()

    End Sub

    Private Sub AllRootsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllRootsToolStripMenuItem.Click
        GroupBox2.Visible = False
        GroupBox3.Visible = False
        GroupBox1.Visible = True
    End Sub

    Private Sub ComplexRootToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComplexRootToolStripMenuItem.Click
        GroupBox1.Visible = False
        GroupBox3.Visible = False
        GroupBox2.Visible = True
    End Sub

    Private Sub GroupBox2_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox2.Enter

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        a_div.Text = ""
        b_div.Text = ""
        quotient.Text = ""
        dividend.Text = ""

        a_div.Focus()

    End Sub

    Private Sub Division_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Division.Click
        Dim i, value, val1, val2 As Integer
        Dim str As String
        str = Nothing


        If a_div.Text = "" Or b_div.Text = "" Then
            MsgBox("Please Enter The Value")
        Else
            value = Val(a_div.Text) - Val(b_div.Text)
            val1 = Val(a_div.Text)
            val2 = Val(b_div.Text)

            Dim a1(val1), a2(val2) As Decimal
            Dim a3(2) As Object

            i = 0
            Do While i < val1

                str = Interaction.InputBox("Enter The Value For Divident")

                a1(i) = Val(str)
                str = Nothing
                i = i + 1
            Loop
            str = Nothing
            i = 0
            Do While i < val2

                str = Interaction.InputBox("Enter The Value For Divider")
                a2(i) = Val(str)
                str = Nothing
                i = i + 1
            Loop

            a3 = Genetibase.MathX.NuGenAlgebricEquations.NuGenPolynomial.Division(Val(a_div.Text), Val(b_div.Text), a1, a2)
            quotient.Text = a3(0).ToString
            dividend.Text = a3(1).ToString

        End If
    End Sub

    Private Sub BairstowComplexRootToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BairstowComplexRootToolStripMenuItem.Click
        GroupBox1.Visible = False
        GroupBox2.Visible = False
        GroupBox3.Visible = True
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub GroupBox3_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox3.Enter

    End Sub

    Private Sub AlgebricEquationMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlgebricEquationMenuToolStripMenuItem.Click
        Dim next_form As New NuGenAlgerbicEquation_mainForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub Label14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label14.Click

    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class