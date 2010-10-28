Public Class NuGenInterpolationMainForm

    Private Sub GeneralIntegrationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GeneralIntegrationToolStripMenuItem.Click
        Dim next_form As New NuGenInterpolationForGeneralIntegration

        Me.Visible = False
        next_form.Visible = True
    End Sub
End Class