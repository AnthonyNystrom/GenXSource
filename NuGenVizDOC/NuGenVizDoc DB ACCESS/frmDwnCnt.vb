

Public Class frmDwnCnt

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        '<EhHeader>
        Try

            '</EhHeader>
            If Not cntvaluex = 0 Then
                Me.Label1.Text = "Capture in: " & cntvaluex.ToString
                cntvaluex = cntvaluex - 1

            Else
                DialogResult = Windows.Forms.DialogResult.OK
            End If

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

    Private Sub frmDwnCnt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '<EhHeader>
        Try
            '</EhHeader>
            Label1.Text = "Capture in: " & cntvaluex.ToString

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub



    Private Sub butCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butCancel.Click

        '<EhHeader>
        Try
            '</EhHeader>
            DialogResult = Windows.Forms.DialogResult.Cancel

            '<EhFooter>
        Catch
        End Try '</EhFooter>

    End Sub

End Class
