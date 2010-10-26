Imports System.Globalization

Public Class MainForm
    Private Sub _timer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _timer.Tick
        _timeMatrix.Text = Me.GetTimeMatrixText()
        _dateMatrix.Text = Me.GetDateMatrixText()
    End Sub

    Private Sub _timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _timer2.Tick
        _counter += 1
        _counterMatrix.Text = _counter.ToString("0000", CultureInfo.CurrentUICulture)

        If _counter = 10000 Then
            _counter = 0
        End If
    End Sub

    Private Function GetDateMatrixText() As String
        Return DateTime.Now.ToShortDateString()
    End Function

    Private Function GetTimeMatrixText() As String
        Return DateTime.Now.ToLongTimeString()
    End Function

    Dim _counter As Int32

    Sub New()
        Me.InitializeComponent()
        _timeMatrix.Text = Me.GetTimeMatrixText()
        _dateMatrix.Text = Me.GetDateMatrixText()
        _timer.Enabled = True
    End Sub
End Class
