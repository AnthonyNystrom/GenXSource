Option Strict On
Option Explicit On 

Imports System
Imports System.ComponentModel
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Xml.Serialization
Imports Microsoft.SharePoint
Imports Microsoft.SharePoint.Utilities
Imports Microsoft.SharePoint.WebPartPages

'Description for GenetibaseDiagramPageWebPart.
<DefaultProperty("Text"), ToolboxData("<{0}:GenetibaseDiagramPageWebPart runat=server></{0}:GenetibaseDiagramPageWebPart>"), XmlRoot(Namespace:="genetibase.com")> _
Public Class GenetibaseDiagramPageWebPart
    Inherits Microsoft.SharePoint.WebPartPages.WebPart

    Private m_diagramServer As String = ""
    Private m_diagramName As String = ""
    Private m_diagramPageName As String = ""

    <Browsable(True), _
    Category("Genetibase"), _
    DefaultValue(""), _
    WebPartStorage(Storage.Shared), _
    FriendlyName("Diagram Server"), _
    Description("Please provide the URL of a Gentibase diagram server (e.g. http://diagrams.genetibase.com)")> _
    Public Property DiagramServer() As String
        Get
            Return m_diagramServer
        End Get
        Set(ByVal Value As String)
            m_diagramServer = Value
        End Set
    End Property

    <Browsable(True), _
    Category("Genetibase"), _
    DefaultValue(""), _
    WebPartStorage(Storage.Shared), _
    FriendlyName("Diagram Name"), _
    Description("Please provide the name of a Gentibase diagram (e.g. sample.diagram")> _
    Public Property DiagramName() As String
        Get
            Return m_diagramName
        End Get
        Set(ByVal Value As String)
            m_diagramName = Value
        End Set
    End Property

    <Browsable(True), _
    Category("Genetibase"), _
    DefaultValue(""), _
    WebPartStorage(Storage.Shared), _
    FriendlyName("Diagram Page"), _
    Description("Please provide the name of a page in the diagram (e.g. Page1")> _
    Public Property DiagramPage() As String
        Get
            Return m_diagramPageName
        End Get
        Set(ByVal Value As String)
            m_diagramPageName = Value
        End Set
    End Property

    'Render this Web Part to the output parameter specified.
    Protected Overrides Sub RenderWebPart(ByVal output As System.Web.UI.HtmlTextWriter)
        Try
            If m_diagramName.Length = 0 _
                    OrElse _
                m_diagramServer.Length = 0 _
                    OrElse _
                m_diagramPageName.Length = 0 Then

                Dim lbl As New Label
                lbl.CssClass = "ms-vb"
                lbl.Text = "Please provide the URL of a diagram server, a diagram name, and a page to display."
                Controls.Add(lbl)
            Else
                Dim img As New Image
                With img
                    .Width = New Unit(Me.ContainerWidth)
                    .AlternateText = m_diagramName
                    .ImageUrl = HttpUtility.UrlPathEncode( _
                                String.Format( _
                                    "{0}/{1}?page={2}", _
                                    m_diagramServer, _
                                    m_diagramName, _
                                    m_diagramPageName _
                                ) _
                                )
                    Controls.Add(img)
                End With
            End If

            Me.RenderChildren(output)
        Catch ex As Exception
            output.Write("An error occured. Details are: " & ex.ToString)
        End Try
    End Sub
End Class
