Imports System.Collections.Specialized

Friend Class UrlInputDialog
	Public ReadOnly Property URL() As String
		Get
			Return _urlCombo.Text
		End Get
	End Property

	Private Property URLs() As StringCollection
		Get
			Select Case _mode
				Case UrlInputDialogMode.Publish
					Return My.Settings.Export_URLs
				Case Else
					Return My.Settings.CheckOut_URLs
			End Select
		End Get
		Set(ByVal value As StringCollection)
			Select Case _mode
				Case UrlInputDialogMode.Publish
					My.Settings.Export_URLs = value
				Case Else
					My.Settings.CheckOut_URLs = value
			End Select
		End Set
	End Property

	Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
		MyBase.OnLoad(e)
		Dim urls As StringCollection = Me.URLs

		If Not urls Is Nothing Then
			For Each url As String In urls
				_urlCombo.Items.Add(url)
			Next
		End If
	End Sub

	Private Sub _okButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _okButton.Click
		Dim urls As StringCollection = Me.URLs
		If urls Is Nothing Then
			urls = New StringCollection()
			Me.URLs = urls
		End If
		Dim url As String = _urlCombo.Text
		If Not String.IsNullOrEmpty(url) And Not _urlCombo.Items.Contains(url) Then
			urls.Add(url)
		End If
		My.Settings.Save()
	End Sub

	Dim _mode As UrlInputDialogMode

	Public Sub New(ByVal mode As UrlInputDialogMode)
		Me.InitializeComponent()
		Me.SetStyle(ControlStyles.Opaque, True)
		_mode = mode
		Select Case _mode
			Case UrlInputDialogMode.Publish
				Me.Text = "Publish to server"
			Case Else
				Me.Text = "Open from server"
		End Select
	End Sub
End Class