Option Strict On
Option Explicit On

Imports System.Xml
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Windows.Forms
Imports Genetibase.NuGenObjective

Public Class LocalDiagram
    Inherits ClientDiagram

    Private m_serverURL As String

    Public Overrides Property ServerUrl() As String
        Get
            Return m_serverURL
        End Get
        Set(ByVal value As String)
            m_serverURL = value
        End Set
    End Property


    Public Overrides Sub Publish()
        If m_serverURL = "" Then
            RaiseMessage("The diagram cannot be published, because the URL of the server has not been provided.", _
                    MessageBoxButtons.OK, _
                    MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        If FileName = "" OrElse Dirty Then
            RaiseMessage("The diagram cannot be published, because it has not been saved locally. Please save the diagram, and then Publish again.", _
                        MessageBoxButtons.OK, _
                        MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim wc As New WebClient
        Dim webStream As Stream = wc.OpenWrite(m_serverURL, "PUT")

        Dim xws As XmlWriterSettings = WriterSettings()
        xws.CloseOutput = True

        Dim xw As XmlWriter = XmlWriter.Create(webStream, xws)
        CurrentDiagram.Save(xw)

        xw.Close()

        wc.Dispose()

        RaiseMessage( _
            "The diagram has been published to the server. You are still working on your local copy.", _
            MessageBoxButtons.OK, _
            MessageBoxIcon.Information _
        )
    End Sub

    Public Overrides Sub Save(ByVal filePath As String)
        Dim xw As XmlWriter = XmlWriter.Create(filePath, WriterSettings)
        CurrentDiagram.Save(xw)
        xw.Flush()
        xw.Close()
        FileName = filePath
        Dirty = False
    End Sub

    Public Shared Function Open(ByVal filePath As String) As LocalDiagram
        Dim result As New LocalDiagram
        Dim xr As XmlReader = XmlReader.Create(filePath, ReaderSettings)

        result.CurrentDiagram = Diagram.Open(xr)
        xr.Close()

        result.FileName = filePath
        result.Dirty = False

        Return result
    End Function

    Public Sub New()
        Dim newDiagram As New Diagram
        newDiagram.AddNewPage().Name = "Page1"
        CurrentDiagram = newDiagram
    End Sub
End Class
