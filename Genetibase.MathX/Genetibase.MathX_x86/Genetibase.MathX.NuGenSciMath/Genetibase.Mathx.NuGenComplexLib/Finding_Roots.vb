Imports Genetibase.Mathx.NuGenComplexLib


Public Class Finding_Roots

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles operation_button.Click

        Dim numb, root, result As Decimal
        'Dim obj As Genetibase.Mathx.NuGenComplexLib.NuGenRoots

        If (TextBox1.Text = "" Or TextBox2.Text = "") Then
            MsgBox("Please Enter Both The Values Correctly")
        Else


            numb = Val(TextBox1.Text)
            root = Val(TextBox2.Text)
            result = Genetibase.Mathx.NuGenComplexLib.NuGenRoots.FindRoots(root, numb)
            TextBox3.Text = result

        End If



    End Sub

    'Sub main()

    'End Sub

    Private Sub clear_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear_button.Click
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""



    End Sub

    Private Sub Finding_Roots_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub NumeicalOperationsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumeicalOperationsToolStripMenuItem.Click
        Dim next_form As New NuGen_Numeric_menu
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class