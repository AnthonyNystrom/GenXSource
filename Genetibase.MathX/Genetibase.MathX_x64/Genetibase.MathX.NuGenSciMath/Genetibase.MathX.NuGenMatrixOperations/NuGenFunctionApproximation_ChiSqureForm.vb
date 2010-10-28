Public Class NuGenFunctionApproximation_ChiSqureForm

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        y_val.Text = ""
        m_val.Text = ""
        res_x.Text = ""

        y_val.Focus()

    End Sub

    Private Sub chi_square_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chi_square.Click
        Dim x As Decimal

        If y_val.Text = "" Or m_val.Text = "" Then
            MsgBox("Please Enter The Values")
        Else
            Genetibase.MathX.NuGenMatrixOperations.NuGenFunctionApproximation.CHISQA(Val(m_val.Text), Val(y_val.Text), x)
            res_x.Text = x.ToString()
        End If

    End Sub

    Private Sub SeriesMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeriesMenuToolStripMenuItem.Click
        Dim next_form As New NuGen_operations_Menu
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub BToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BToolStripMenuItem.Click

    End Sub

    Private Sub AlgebricToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlgebricToolStripMenuItem.Click
        Dim next_form As New NuGenFunctionApproximation_Main_Form
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub NuGenFunctionApproximation_ChiSqureForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        'Me.Visible = False
        'next_form.Visible = True

    End Sub
End Class