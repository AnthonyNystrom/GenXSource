Imports Genetibase.Shared.Controls

Friend NotInheritable Class MainForm
    Protected Overrides Sub OnFormClosed(ByVal e As System.Windows.Forms.FormClosedEventArgs)
        MyBase.OnFormClosed(e)

        RemoveHandler _blendForm.BlendColorChanged, New EventHandler(Of ColorEventArgs)(AddressOf _blendForm_BlendColorChanged)
        _blendForm.Dispose()
        RemoveHandler _paletteForm.GradientBeginChanged, New EventHandler(Of ColorEventArgs)(AddressOf _paletteForm_GradientBeginChanged)
        RemoveHandler _paletteForm.GradientEndChanged, New EventHandler(Of ColorEventArgs)(AddressOf _paletteForm_GradientEndChanged)
        _paletteForm.Dispose()
        NuGenUISnap.UnregisterExternalReferenceForm(Me)
    End Sub

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        MyBase.OnLoad(e)
        NuGenUISnap.RegisterExternalReferenceForm(Me)
        _blendForm.Show()
        _paletteForm.Show()
    End Sub

    Private Sub _blendForm_BlendColorChanged(ByVal sender As Object, ByVal e As ColorEventArgs)
        _colorPanel.SelectedColor = e.Color
    End Sub

    Private Sub _paletteForm_GradientBeginChanged(ByVal sender As Object, ByVal e As ColorEventArgs)
        _blendForm.SetGradientBegin(e.Color)
    End Sub

    Private Sub _paletteForm_GradientEndChanged(ByVal sender As Object, ByVal e As ColorEventArgs)
        _blendForm.SetGradientEnd(e.Color)
    End Sub

    Dim _blendForm As BlendForm
    Dim _paletteForm As PaletteForm

    Sub New()
        Me.InitializeComponent()

        _blendForm = New BlendForm
        AddHandler Me._blendForm.BlendColorChanged, New EventHandler(Of ColorEventArgs)(AddressOf _blendForm_BlendColorChanged)
        _blendForm.Location = New Point(MyBase.Right, MyBase.Top)
        _paletteForm = New PaletteForm
        AddHandler _paletteForm.GradientBeginChanged, New EventHandler(Of ColorEventArgs)(AddressOf _paletteForm_GradientBeginChanged)
        AddHandler _paletteForm.GradientEndChanged, New EventHandler(Of ColorEventArgs)(AddressOf _paletteForm_GradientEndChanged)
        _paletteForm.Location = New Point((MyBase.Left - _paletteForm.Width), MyBase.Top)
    End Sub
End Class
