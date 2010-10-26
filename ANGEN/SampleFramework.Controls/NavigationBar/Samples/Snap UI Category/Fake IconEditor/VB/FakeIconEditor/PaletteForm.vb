Friend NotInheritable Class PaletteForm
    Public Event GradientBeginChanged As EventHandler(Of ColorEventArgs)
    Public Event GradientEndChanged As EventHandler(Of ColorEventArgs)

    Private Sub _colorSelector_MouseClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles _colorSelector.MouseClick
        If (e.Button = MouseButtons.Left) Then
            RaiseEvent GradientBeginChanged(Me, New ColorEventArgs(_colorSelector.SelectedColor))
        ElseIf (e.Button = MouseButtons.Right) Then
            RaiseEvent GradientEndChanged(Me, New ColorEventArgs(_colorSelector.SelectedColor))
        End If
    End Sub

    Private Sub _colorSelector_SelectedColorChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _colorSelector.SelectedColorChanged
        _colorHistory.SelectedColor = _colorSelector.SelectedColor
    End Sub
End Class
