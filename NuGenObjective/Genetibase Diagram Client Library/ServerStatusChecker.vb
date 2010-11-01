Option Strict On
Option Explicit On

Imports System.Xml
Imports System.Xml.Serialization

Public Class ServerStatusChecker
    Private _server As ServerStatus

    Public Function GetServerStatus(ByVal serverUrl As String) As ServerStatus
        Dim xs As New XmlSerializer(GetType(ServerStatus))
        Dim xr As XmlReader = XmlReader.Create(serverUrl)
        Dim result As ServerStatus

        result = TryCast(xs.Deserialize(xr), ServerStatus)

        Return result
    End Function
End Class
