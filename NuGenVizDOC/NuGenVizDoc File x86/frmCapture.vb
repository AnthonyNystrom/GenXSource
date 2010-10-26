

Public Class frmCapture

    Private Sub frmCapture_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try
            Me.nuGenScreenCap1.Dispose()
            GC.Collect()
        Catch
        End Try
    End Sub
End Class
