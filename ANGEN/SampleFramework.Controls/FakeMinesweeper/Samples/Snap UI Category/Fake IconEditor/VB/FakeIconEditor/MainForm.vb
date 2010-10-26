Imports Genetibase.Shared.Controls

Public Class MainForm
    Protected Overrides Sub OnFormClosed(ByVal e As System.Windows.Forms.FormClosedEventArgs)
        MyBase.OnFormClosed(e)
        _blendForm.Dispose()
        _paletteForm.Dispose()
        NuGenUISnap.UnregisterExternalReferenceForm(Me)
    End Sub

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        MyBase.OnLoad(e)
        NuGenUISnap.RegisterExternalReferenceForm(Me)
        _blendForm.Show()
        _paletteForm.Show()
    End Sub

    Dim _blendForm As BlendForm
    Dim _paletteForm As PaletteForm

    Sub New()
        Me.InitializeComponent()

        _blendForm = New BlendForm()
        _blendForm.Location = New Point(Me.Right, Me.Top)

        _paletteForm = New PaletteForm()
        _paletteForm.Location = New Point(Me.Left - _paletteForm.Width, Me.Top)
    End Sub
End Class
