Friend NotInheritable Class BlendForm
    Public Event BlendColorChanged As EventHandler(Of ColorEventArgs)

    Public Sub SetGradientBegin(ByVal gradientBegin As Color)
        _blendSelector.UpperColor = gradientBegin
    End Sub

    Public Sub SetGradientEnd(ByVal gradientEnd As Color)
        _blendSelector.LowerColor = gradientEnd
    End Sub

    Private Sub _blendSelector_ColorChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _blendSelector.ColorChanged, _blendSelector.ColorChanging
        RaiseEvent BlendColorChanged(Me, New ColorEventArgs(Me._blendSelector.SelectedColor))
    End Sub
End Class