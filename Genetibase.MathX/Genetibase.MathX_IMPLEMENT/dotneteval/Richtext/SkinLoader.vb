Public Class SkinLoader
    Private mXr As Xml.XmlReader

    Public Sub Load(ByVal Skin As Skin, ByVal Stream As IO.Stream)
        mXr = New Xml.XmlTextReader(Stream)

        Do While mXr.Read
            Select Case mXr.NodeType
                Case Xml.XmlNodeType.Element
                    Trace.WriteLine("<" & mXr.Name & ">")
                Case Xml.XmlNodeType.EndElement
                    Trace.WriteLine("</" & mXr.Name & ">")
            End Select
        Loop
    End Sub
End Class
