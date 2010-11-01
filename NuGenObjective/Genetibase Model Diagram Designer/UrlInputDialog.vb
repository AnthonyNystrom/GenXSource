Imports System.Collections.Specialized
Imports Genetibase.NugenObjective.Windows.DiagramClient
Imports System.Net

Friend Class UrlInputDialog
    Private m_serverStatus As ServerStatus
    Private m_serverFiles As ArrayList
    Private m_selectedURL As String

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

    Private Sub SaveUsedUrls()
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

    Private Sub _okButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _okButton.Click
        Dim result As DialogResult
        If _mode = UrlInputDialogMode.CheckOut Then

            If Not CheckServerStatus() Then
                Exit Sub
            End If


            If m_selectedURL = "" Then
                MsgBox("Please select the file to check out.")
                Exit Sub
            Else
                Dim selectedItemIndex As Integer
                If Not _urlCombo.Items.Contains(m_selectedURL) Then
                    selectedItemIndex = _urlCombo.Items.Add(m_selectedURL)
                Else
                    selectedItemIndex = _urlCombo.Items.IndexOf(m_selectedURL)
                End If
                _urlCombo.SelectedIndex = selectedItemIndex

                result = System.Windows.Forms.DialogResult.OK
            End If
        End If
        If _mode = UrlInputDialogMode.Publish Then
            If Not CheckServerStatus() Then
                Exit Sub
            Else
                result = System.Windows.Forms.DialogResult.OK
            End If
        End If
        SaveUsedUrls()
        DialogResult = result
        Hide()
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

    Private Function CheckServerStatus() As Boolean
        If _urlCombo.Text = "" Then
            Return False
        End If
        Dim serverPathBuilder As New UriBuilder(_urlCombo.Text)
        Dim serverPath As String
        Dim isFileUrl As Boolean

        If serverPathBuilder.Path.EndsWith("/") Then
            isFileUrl = False
            serverPath = serverPathBuilder.ToString
            serverPathBuilder.Path &= "get.serverstatus"
        Else
            isFileUrl = True
            serverPathBuilder.Path = serverPathBuilder.Path.Remove(serverPathBuilder.Path.LastIndexOf("/"c))
            serverPath = serverPathBuilder.ToString
            serverPathBuilder.Path &= "/get.serverstatus"
        End If

        Dim serverStatusChecker As New ServerStatusChecker


        Try
            m_serverStatus = Nothing
            m_serverStatus = serverStatusChecker.GetServerStatus(serverPathBuilder.ToString)
            CheckServerStatus = True
        Catch Ex As WebException When Ex.Status = WebExceptionStatus.NameResolutionFailure Or Ex.Status = WebExceptionStatus.ProxyNameResolutionFailure
            MsgBox( _
                String.Format( _
                    "Could not find a server called {0}. Please check the server URL, and try again.", _
                    serverPath _
                ) _
            )
            CheckServerStatus = False
        Catch Ex As WebException When Ex.Status = WebExceptionStatus.ConnectFailure
            MsgBox( _
                String.Format( _
                    "Could not find a NuGenObjective Server at {0}. Please check the server URL path, and try again.", _
                    serverPath _
                ) _
            )
            CheckServerStatus = False
        Catch Ex As WebException When Ex.Status = WebExceptionStatus.ProtocolError
            Dim msg As String
            If isFileUrl Then
                msg = "The specified file could not be found on the server. Please check the URL and try again."
            Else
                msg = "There does not seem to be a NuGenObjective Server at the specified URL. Please check the URL and try again."
            End If
            MsgBox(msg)
            CheckServerStatus = False
        Catch Ex As WebException
            CheckServerStatus = False
        Finally
            If m_serverStatus IsNot Nothing AndAlso m_serverStatus.Files.Count > 0 Then
                m_serverFiles = New ArrayList(m_serverStatus.Files)
                dgFilesOnServer.DataSource = m_serverFiles
                dgFilesOnServer.Refresh()
            Else
                m_serverFiles = New ArrayList
                dgFilesOnServer.DataSource = m_serverFiles
                dgFilesOnServer.Refresh()
            End If
        End Try
    End Function

    Private Sub _goButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _goButton.Click
        m_selectedURL = ""
        CheckServerStatus()
    End Sub

    Private Sub _urlCombo_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _urlCombo.SelectedValueChanged
        CheckServerStatus()
    End Sub

    Private Sub dgFilesOnServer_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgFilesOnServer.CurrentCellChanged
        Dim selectedItem As ServerStatusFile = TryCast(m_serverFiles(dgFilesOnServer.CurrentCell.RowNumber), ServerStatusFile)
        If selectedItem IsNot Nothing Then
            m_selectedURL = selectedItem.FileURL
        Else
            m_selectedURL = ""
        End If
    End Sub
End Class