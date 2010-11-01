
Imports System.Xml
Imports Genetibase.NuGenObjective

Partial Class ShowDiagram
    Inherits System.Web.UI.Page

    Private Function CreatePathToDiagram(ByVal rootPath As String, ByVal fileName As String) As String
        Dim result As String = IO.Path.Combine(rootPath, "diagrams")
        result = IO.Path.Combine(result, fileName)
        If result.EndsWith(".ocidml", StringComparison.InvariantCultureIgnoreCase) Then
            result = result.Remove(result.LastIndexOf(".ocidml", StringComparison.InvariantCultureIgnoreCase))
        End If
        result = IO.Path.Combine(result, "current.ocidml")
        Return result
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Overrides Sub CreateChildControls()
        MyBase.CreateChildControls()

        Dim fileName As String = Request.QueryString("file")
        Dim pageName As String = Request.QueryString("page")

        If fileName <> "" Then
            Try
                Dim xd As New XmlDocument()
                Dim filePath As String = CreatePathToDiagram(Request.PhysicalApplicationPath, fileName)

                xd.Load(filePath)

                Dim nsmgr As XmlNamespaceManager = New XmlNamespaceManager(xd.NameTable)
                nsmgr.AddNamespace("gd", Diagram.RootNameSpace)
                Dim pList As XmlNodeList = xd.SelectNodes("/gd:Diagram/gd:Pages/gd:Page", nsmgr)

                If pageName Is Nothing Then
                    If pList.Count > 0 Then
                        pageName = pList(0).SelectSingleNode("./gd:Key", nsmgr).InnerText
                    End If
                End If

                For Each page As XmlNode In pList
                    'Dim tc As New TableCell
                    Dim hl As New HyperLink
                    Dim pageTitle As String = page.SelectSingleNode("./gd:Name", nsmgr).InnerText
                    Dim pageKey As String = page.SelectSingleNode("./gd:Key", nsmgr).InnerText


                    'hl.Text = "[" & pageTitle & "]"
                    hl.Text = pageTitle

                    If pageKey <> pageName Then
                        hl.NavigateUrl = Server.UrlPathEncode( _
                                            String.Format( _
                                                "ShowDiagram.aspx?file={0}&page={1}", _
                                                fileName, _
                                                pageKey _
                                            ) _
                                    )
                    Else
                        hl.CssClass = "current"
                    End If

                    pageTabs.Controls.Add(hl)
                Next

                Dim imageURL As String = fileName
                If Not imageURL.EndsWith(".ocidml", StringComparison.InvariantCultureIgnoreCase) Then
                    imageURL &= ".ocidml"
                End If
                If pageName <> "" Then
                    imageURL &= "?page=" & pageName
                End If
                imgPage.ImageUrl = Server.UrlPathEncode(imageURL)
            Catch ex As Exception

            End Try
        Else
            imgPage.Visible = False
        End If
    End Sub
End Class
