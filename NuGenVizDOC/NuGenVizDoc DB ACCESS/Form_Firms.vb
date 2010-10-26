Public Class Form_Firms
    Dim FF As Form11024
    Dim FHIS As frmCapHis


    Private Sub NavBarItem1_LinkClicked(ByVal sender As Object, ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles NavBarItem1.LinkClicked

        Application.Exit()
    End Sub



    Private Sub NavBarItem4_LinkClicked(ByVal sender As Object, ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs)

        Dim FF As New Form1640


        FF.Show()
    End Sub

    Private Sub NavBarItem5_LinkClicked(ByVal sender As Object, ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles NavBarItem5.LinkClicked

        FF = New Form11024
        FHIS = New frmCapHis


        FF.Show()
        FHIS.Show()
        Me.SimpleButton1.Enabled = True

    End Sub



    Private Sub Form_Firms_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim SnapUI As New Genetibase.UI.NuGenUISnap(Me)
        SnapUI.StickOnMove = True
        SnapUI.StickToScreen = True
        SnapUI.StickToOther = True
        Me.SimpleButton1.Enabled = False
        Me.Focus()
    End Sub

    Private Sub NavBarItem3_LinkClicked(ByVal sender As Object, ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs)

        Dim frmCap As New frmCapture

        frmCap.Show()
    End Sub



    Private Sub NavBarItem7_LinkClicked(ByVal sender As Object, ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles NavBarItem7.LinkClicked
        Try
            FF.Close()
            FHIS.Close()
            Me.SimpleButton1.Enabled = False
        Catch
        End Try

    End Sub

    Private Sub NavBarItem4_LinkClicked1(ByVal sender As Object, ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles NavBarItem4.LinkClicked
        FHIS = New frmCapHis
        FHIS.Show()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Try
            FHIS.Hide()
            FF.Hide()

        Catch
        End Try

        Me.Hide()

        Dim frmCt As New frmDwnCnt(CInt(SpinEdit1.Value))
        frmCt.ShowDialog()

        If Not frmCt.DialogResult = Windows.Forms.DialogResult.Cancel Then

            frmCt.Close()
            frmCt.Dispose()
            frmCt = Nothing

            Dim frmCap As New frmCapture
            frmCap.ShowDialog()
            FF.ScribbleBox1.Scribble.Paste()
            FHIS.ScribbleBox1.Scribble.Paste()
            FHIS.ScribbleBox1.Scribble.ZoomOut()
            FHIS.ScribbleBox1.Scribble.ZoomOut()
            frmCap.Dispose()
            frmCap = Nothing
            Me.Show()

            Try
                FF.Show()
                FHIS.Show()
            Catch
            End Try

        Else

            frmCt.Dispose()
            frmCt = Nothing
            Me.Show()
            FF.Show()
            FHIS.Show()


        End If
    End Sub
End Class