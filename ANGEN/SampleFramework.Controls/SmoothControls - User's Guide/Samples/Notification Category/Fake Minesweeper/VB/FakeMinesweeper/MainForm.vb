Public Class MainForm
    Private Sub _timer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _timer.Tick
        Dim hundreds As Int32 = _counter / 100
        Dim tempNum As Int32 = _counter - hundreds * 100
        Dim tens As Int32 = tempNum / 10
        tempNum = tempNum - tens * 10

        _segmentOne.Value = hundreds
        _segmentTwo.Value = tens
        _segmentThree.Value = tempNum

        _counter += 1

        If _counter = 1000 Then
            _counter = 0
        End If
    End Sub

    Private Sub _goButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _goButton.Click
        _timer.Enabled = True
    End Sub

    Dim _counter As Int32

    Sub New()
        Me.InitializeComponent()
    End Sub
End Class
