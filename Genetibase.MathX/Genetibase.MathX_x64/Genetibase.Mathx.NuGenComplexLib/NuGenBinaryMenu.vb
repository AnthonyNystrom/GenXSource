Public Class NuGenBinaryMenu

    Private Sub BinaryOrToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Visible = False
        Dim next_form As New NuGenBinaryOperations
        next_form.Visible = True

        next_form.Binary_logical.Visible = True
        next_form.Binary_not.Visible = False
    End Sub

    Private Sub BinaryAdditionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BinaryAdditionToolStripMenuItem.Click
        Me.Visible = False
        Dim next_form As New NuGenBinaryOperations
        next_form.Visible = True

        next_form.Binary_Mathematical_operations.Visible = True
        next_form.Binary_logical.Visible = False
        next_form.Binary_not.Visible = False


    End Sub

    Private Sub BinaryNotToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BinaryNotToolStripMenuItem.Click
        Me.Visible = False
        Dim next_form As New NuGenBinaryOperations
        next_form.Visible = True

        ' next_form.Addition.Visible = False

        next_form.Binary_Mathematical_operations.Visible = False
        next_form.Binary_logical.Visible = True
        next_form.Binary_not.Visible = False


    End Sub

    Private Sub BinaryAndToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BinaryAndToolStripMenuItem.Click
        Me.Visible = False
        Dim next_form As New NuGenBinaryOperations
        next_form.Visible = True


        next_form.Binary_Mathematical_operations.Visible = False
        next_form.Binary_logical.Visible = False
        next_form.Binary_not.Visible = True
    End Sub

    Private Sub BinaryXorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Visible = False
        Dim next_form As New NuGenBinaryOperations
        next_form.Visible = True

        next_form.Binary_logical.Visible = True
        next_form.Binary_not.Visible = False
    End Sub

    Private Sub BinaryNandToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Visible = False
        Dim next_form As New NuGenBinaryOperations
        next_form.Visible = True

        next_form.Binary_logical.Visible = True
        next_form.Binary_not.Visible = False
    End Sub

    Private Sub BinaryNorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Visible = False
        Dim next_form As New NuGenBinaryOperations
        next_form.Visible = True

        next_form.Binary_logical.Visible = True
        next_form.Binary_not.Visible = False
    End Sub

    Private Sub BinaryNXorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Visible = False
        Dim next_form As New NuGenBinaryOperations
        next_form.Visible = True

        next_form.Binary_logical.Visible = True
        next_form.Binary_not.Visible = False
    End Sub

    Private Sub MenuStrip1_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click

    End Sub

    Private Sub ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem2.Click
        Dim next_form As New NuGenBinary_conversions
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub BinaryOperationsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BinaryOperationsToolStripMenuItem.Click

    End Sub

    Private Sub NuMericalOprationsMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NuMericalOprationsMenuToolStripMenuItem.Click
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