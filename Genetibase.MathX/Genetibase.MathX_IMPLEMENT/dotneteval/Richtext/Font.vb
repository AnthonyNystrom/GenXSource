Imports System
Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Drawing
Imports System.Text.RegularExpressions

Public Class Font
    Inherits skinElement

    Public Size As Integer
    Public Color As Color
    Public Face As String

    Friend Overrides Sub SaveXMLAttributes(ByVal writer As XmlWriter)
        If Size > 0 Then writer.WriteAttributeString("size", Size.ToString)
        If Not Color.Equals(Color.Empty) Then writer.WriteAttributeString("color", Size.ToString)
        If Face Is Nothing Then writer.WriteAttributeString("face", Face)
    End Sub

    Friend Overrides Sub SaveXML(ByVal writer As System.Xml.XmlWriter)
    End Sub

    Protected Overrides Function parseXmlAttribute(ByVal name As String, ByVal value As String) As Boolean
        Select Case name
            Case "color"
                If value.Chars(0) = "#"c Then
                    Color = Drawing.Color.FromArgb(CInt("&h" & value.Substring(1)))
                Else
                    Color = Drawing.Color.FromName(value)
                End If
                Return True
            Case "size"
                Size = CInt(value)
                Return True
            Case "face"
                Face = value
                Return True
            Case Else
                Return MyBase.parseXmlAttribute(name, value)
        End Select
    End Function
End Class
