Imports System
Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Drawing
Imports System.Text.RegularExpressions

Public MustInherit Class skinElement
    Private mId As String

    Protected Overridable Function parseXmlAttribute(ByVal name As String, ByVal value As String) As Boolean
        Select Case name
            Case "id"
                mId = value
                Return True
        End Select
    End Function

    Protected Overridable Sub parseXmlText(ByVal value As String)
        ' do nothing but overridable
    End Sub

    Protected Overridable Function parseXmlElement(ByVal name As String) As skinElement
        Return Nothing
    End Function

    Protected Overridable Sub parseComplete()
        ' do nothing but overridable
    End Sub

    Friend MustOverride Sub SaveXML(ByVal writer As XmlWriter)

    Friend Overridable Sub SaveXMLAttributes(ByVal writer As XmlWriter)
        If Len(mId) > 0 Then writer.WriteAttributeString("id", mId)
    End Sub

    Protected Friend Shared Sub LoadXML(ByVal el As skinElement, ByVal reader As XmlTextReader)
        If Not el Is Nothing Then
            Dim i As Integer
            Dim name As String = reader.Name
            For i = 0 To reader.AttributeCount - 1
                reader.MoveToAttribute(i)
                If Not el.parseXmlAttribute(reader.Name, reader.Value) Then
                    MsgBox(String.Format("Error in skin, line:{0} column:{1}" & vbCrLf & _
                        "Unknown attribute {2} in tag {3}", reader.LineNumber, reader.LinePosition, reader.Name, name))
                End If
            Next i
            reader.MoveToElement() 'Moves the reader back to the element node.
        End If
        If reader.IsEmptyElement Then
            If Not el Is Nothing Then el.parseComplete()
            Exit Sub
        End If 'XmlTextReader        
        While (reader.Read())
            Select Case reader.NodeType
                Case XmlNodeType.Text
                    If Not el Is Nothing Then el.parseXmlText(reader.Value)
                Case XmlNodeType.Element
                    Dim el2 As skinElement = Nothing
                    If Not el Is Nothing Then
                        el2 = el.parseXmlElement(reader.Name)
                    End If
                    LoadXML(el2, reader)
                Case XmlNodeType.EndElement
                    If Not el Is Nothing Then el.parseComplete()
                    Exit Sub
            End Select
            If reader.EOF Then Exit While
        End While
    End Sub

End Class
