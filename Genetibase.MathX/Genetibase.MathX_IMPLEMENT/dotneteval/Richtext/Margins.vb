Imports System
Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Drawing
Imports System.Text.RegularExpressions

Public Class Margins
    Inherits skinElement

    Public Top As Integer
    Public Left As Integer
    Public Right As Integer
    Public Bottom As Integer

    Friend Overrides Sub SaveXMLAttributes(ByVal writer As XmlWriter)
        If Top = Left And _
           Top = Right And _
           Top = Bottom Then
            If Top > 0 Then
                writer.WriteAttributeString("margin", Top.ToString)
            End If
        Else
            If Top > 0 Then writer.WriteAttributeString("top", Top.ToString)
            If Left > 0 Then writer.WriteAttributeString("left", Left.ToString)
            If Right > 0 Then writer.WriteAttributeString("right", Right.ToString)
            If Bottom > 0 Then writer.WriteAttributeString("bottom", Bottom.ToString)
        End If
    End Sub

    Friend Overrides Sub SaveXML(ByVal writer As System.Xml.XmlWriter)

    End Sub

    Protected Overrides Function parseXmlAttribute(ByVal name As String, ByVal value As String) As Boolean
        Select Case name
            Case "all"
                Top = CInt(value)
                Left = Top
                Right = Top
                Bottom = Top
                Return True
            Case "top"
                Top = CInt(value)
                Return True
            Case "left"
                Left = CInt(value)
                Return True
            Case "right"
                Right = CInt(value)
                Return True
            Case "bottom"
                Bottom = CInt(value)
                Return True
            Case Else
                Return MyBase.parseXmlAttribute(name, value)
        End Select
    End Function
End Class
