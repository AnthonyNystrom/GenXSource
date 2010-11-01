Option Strict On
Option Explicit On

Imports System.IO

Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim path As New DirectoryInfo(Request.PhysicalApplicationPath)
        Dim files() As DirectoryInfo = path.GetDirectories("diagrams\*")

        If files.Length = 0 Then
            lblMessage.Text = "There are no diagrams uploaded."
        Else
            lblMessage.Text = "Please select a diagram below."
            For Each f As DirectoryInfo In files
                'Dim hl As New HyperLink
                'hl.Text = f.Name
                'hl.NavigateUrl = Server.UrlPathEncode("ShowDiagram.aspx?file=" & f.Name)
                If Not ( _
                    (f.Attributes And FileAttributes.Hidden) = FileAttributes.Hidden _
                        OrElse _
                    (f.Attributes And FileAttributes.System) = FileAttributes.System _
                    ) Then
                    Dim li As New ListItem(f.Name, Server.UrlPathEncode("ShowDiagram.aspx?file=" & f.Name & ".ocidml"))

                    lstDiagrams.Items.Add(li)

                    'Dim tc As New TableCell
                    'tc.Controls.Add(hl)

                    'Dim tr As New TableRow()
                    'tr.Cells.Add(tc)

                    'tblDiagrams.Rows.Add(tr)
                End If
            Next
        End If
    End Sub
End Class
