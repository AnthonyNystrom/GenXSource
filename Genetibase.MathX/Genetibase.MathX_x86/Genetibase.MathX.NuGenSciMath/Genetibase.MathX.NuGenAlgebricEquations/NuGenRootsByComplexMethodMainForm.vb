Public Class NuGenRootsByComplexMethodMainForm

    Private Sub UsingNewtonsMethodToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UsingNewtonsMethodToolStripMenuItem.Click
        Dim next_form As New NuGenComplexMethod_LinForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

    Private Sub AllRootsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllRootsToolStripMenuItem.Click
        Dim next_form As New NuGenRootComplexMethod_AllRoots

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub BairstowComplexRootToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BairstowComplexRootToolStripMenuItem.Click
        Dim next_form As New NuGenComplexMethod_BairstowForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ComplexRootToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComplexRootToolStripMenuItem.Click
        Dim next_form As New NuGenComplexMethod_cznewtonForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ParabolicRootToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ParabolicRootToolStripMenuItem.Click
        Dim next_form As New NuGenComplexMethod_MuellerForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ParabolicRoot1ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ParabolicRoot1ToolStripMenuItem.Click
        Dim next_form As New NuGenComplexMethod_Mueller2_Form
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub QuadraticRootToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles QuadraticRootToolStripMenuItem.Click
        Dim next_form As New NuGenComplexMethod_QudraticForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ComplexRootNumberToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComplexRootNumberToolStripMenuItem.Click
        Dim next_form As New NuGenComplexMethod_RootNumForm
        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ComplexRootZCircleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComplexRootZCircleToolStripMenuItem.Click
        Dim next_form As New NuGenComplexMethod_ZCircleForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ComplexRootsUsingMuellersMethodToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComplexRootsUsingMuellersMethodToolStripMenuItem.Click
        Dim next_form As New NuGenComplexMethod_ZMuellerForm

        Me.Visible = False
        next_form.Visible = True
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'End
    End Sub

    Private Sub AlgebricEquationMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlgebricEquationMenuToolStripMenuItem.Click
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