Public Class frmCapHis

    Private Sub frmCapHis_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim SnapUI As New Genetibase.UI.NuGenUISnap(Me)
        SnapUI.StickOnMove = True
        SnapUI.StickToScreen = True
        SnapUI.StickToOther = True
        Me.ScribbleBox1.Scribble.ZoomOut()
        Me.ScribbleBox1.Scribble.ZoomOut()

    End Sub
End Class