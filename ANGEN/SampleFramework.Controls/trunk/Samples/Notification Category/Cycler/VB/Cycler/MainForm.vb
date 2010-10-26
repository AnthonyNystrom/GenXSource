Imports Genetibase.Shared.Controls
Imports System.Diagnostics

Public Class MainForm
    Private Sub UpdateProgressBarValue(ByVal progressBar As NuGenProgressBar)
        Debug.Assert(Not progressBar Is Nothing, "progressBar != null")

        If progressBar.Value = progressBar.Maximum Then
            progressBar.Value = progressBar.Minimum
        End If

        progressBar.Value += 1
    End Sub

    Private Sub _goButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _goButton.Click
        _timer.Enabled = Not _timer.Enabled

        If _timer.Enabled Then
            _goButton.Text = "&Stop"
        Else
            _goButton.Text = "&Start"
        End If
    End Sub

    Private Sub _horzTrackBar_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _horzTrackBar.ValueChanged
        _timer.Interval = 500 / _horzTrackBar.Value
    End Sub

    Private Sub _scrollBar_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _scrollBar.ValueChanged
        _vertProgressBar.Value = _scrollBar.Maximum - _scrollBar.Value
    End Sub

    Private Sub _timer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _timer.Tick
        Me.UpdateProgressBarValue(_horzProgressBar)
        Me.UpdateProgressBarValue(_horzProgressBar2)
    End Sub

    Public Sub New()
        Me.InitializeComponent()
    End Sub
End Class
