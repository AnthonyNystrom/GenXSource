Imports System.IO
Imports Genetibase.NuGenObjective.Windows.DiagramClient
Imports System.IO.IsolatedStorage
Imports System.Runtime.Serialization.Formatters.Binary

Namespace My

    ' The following events are availble for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Private m_addIns As New List(Of AddinStatus)
        Private m_loadedAddIns As New Dictionary(Of String, IClientAddin)
        '    Private m_activeAddins As New ActiveAddinCollection

        '    Public ReadOnly Property Addins() As Dictionary(Of String, IClientAddin)
        '        Get
        '            Return m_addIns
        '        End Get
        '    End Property

        Public ReadOnly Property Addins() As List(Of AddinStatus)
            Get
                Return m_addIns
            End Get
        End Property

        Public Sub GetCurrentUserActiveAddIns()
            Try
                Dim isoStore As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForAssembly
                Dim fs As New IsolatedStorageFileStream("GenetibaseDiagramDesignerAddins.sett", FileMode.Open, isoStore)
                Dim bf As New BinaryFormatter()

                Try
                    m_addIns = TryCast(bf.Deserialize(fs), List(Of AddinStatus))
                Catch Ex As System.Runtime.Serialization.SerializationException
                    m_addIns = Nothing
                End Try
                If m_addIns Is Nothing Then
                    m_addIns = New List(Of AddinStatus)
                Else
                End If
                fs.Close()
            Catch Ex As FileNotFoundException
                m_addIns = New List(Of AddinStatus)
            End Try
        End Sub

        Public Sub SaveCurrentUserActiveAddIns()
            Dim isoStore As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForAssembly
            Dim fs As New IsolatedStorageFileStream("GenetibaseDiagramDesignerAddins.sett", FileMode.Create, isoStore)
            Dim bf As New BinaryFormatter()

            bf.Serialize(fs, m_addIns)
            fs.Flush()
            fs.Close()
        End Sub

        Public Sub ActivateAddin(ByVal addIn As AddinStatus, ByVal designer As IDesigner, ByVal rootMenu As ToolStripMenuItem)
            If m_loadedAddIns.ContainsKey(addIn.ClassName) Then
                m_loadedAddIns(addIn.ClassName).Activate(designer, rootMenu)
            End If
        End Sub

        Public Sub DeactivateAddin(ByVal addIn As AddinStatus, ByVal designer As IDesigner, ByVal rootMenu As ToolStripMenuItem)
            If m_loadedAddIns.ContainsKey(addIn.ClassName) Then
                m_loadedAddIns(addIn.ClassName).Deactivate(designer, rootMenu)
            End If
        End Sub

        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            ' Load any add-ins

            Dim addInsLocationPath As String = Path.Combine(My.Application.Info.DirectoryPath, "addins")
            Dim addinsLocation As New DirectoryInfo(addInsLocationPath)

            Try
                GetCurrentUserActiveAddIns()

                Dim addInFiles As FileInfo() = addinsLocation.GetFiles("*-addin.dll")
                For Each fi As FileInfo In addInFiles
                    Dim fiClassFile As String = fi.FullName.Trim & ".txt"
                    Dim className As String
                    Try
                        className = File.ReadAllText(fiClassFile).Trim
                    Catch Ex As FileNotFoundException
                        Continue For
                    End Try
                    If className <> "" Then
                        Dim addIn As IClientAddin = TryCast( _
                                        AppDomain.CurrentDomain.CreateInstanceFromAndUnwrap( _
                                                    fi.FullName, className _
                                        ), _
                                        IClientAddin _
                                    )

                        If addIn IsNot Nothing Then
                            m_loadedAddIns.Add(className, addIn)

                            Dim aStatus As New AddinStatus(addIn.Name, className, "", False)
                            If m_addIns.BinarySearch(aStatus) < 0 Then
                                m_addIns.Add(New AddinStatus(addIn.Name, className, "", False))
                                m_addIns.Sort()
                            End If
                        End If
                    End If
                Next
                ' Clean active addin list
                Dim deletionList As New List(Of AddinStatus)
                For Each i As AddinStatus In m_addIns
                    If Not m_loadedAddIns.ContainsKey(i.ClassName) Then
                        deletionList.Add(i)
                    End If
                Next
                For Each i As AddinStatus In deletionList
                    m_addIns.Remove(i)
                Next
                m_addIns.Sort()
                ' Save settings
                SaveCurrentUserActiveAddIns()
            Catch ex As DirectoryNotFoundException
                m_addIns = New List(Of AddinStatus)
                SaveCurrentUserActiveAddIns()
            Catch ex As FileNotFoundException

            End Try
        End Sub
    End Class

End Namespace

