Public Class NuGenInterPolationNthForhermiteForm

    Private Sub Hermitepolynomial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Hermitepolynomial.Click

        Dim b(3, 3) As Decimal
        Dim a(Val(k_val.Text)) As Decimal
        If k_val.Text = "" Then

            MsgBox("Please Enter The Value")
        Else
            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenInterpolation1.HERMITE(Val(k_val.Text), b, a)

            Dim str As String
            Dim i As Integer
            str = Nothing



            For i = 0 To i < Val(k_val.Text) Step 1

                str = String.Concat(str, a(i).ToString)
                str = String.Concat(str, " , ")

            Next
            Label3.Text = String.Concat(Label3.Text, str)


        End If
    End Sub

    Private Sub NuGenInterPolationNthForhermiteForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        k_val.Text = ""
        Label3.Text = ""
        Label3.Text = "Result ->"
        k_val.Focus()

    End Sub

    Private Sub k_val_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles k_val.TextChanged

    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Dim next_form As New NuGenCurvesAndCoordinate_MainMenu
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub NTHMNenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NTHMNenuToolStripMenuItem.Click
        Dim next_form As New NuGenNTH_MainForm
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