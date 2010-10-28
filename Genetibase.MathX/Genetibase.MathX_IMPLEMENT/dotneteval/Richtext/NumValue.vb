Public Structure NumValue
    Public unit As eUnit
    Public value As Single
    Public Sub Parse(ByVal v As String)
        If v Is Nothing OrElse v.Length = 0 Then
            unit = Globals.eUnit.pixels
            value = Nothing
        Else
            If v.Chars(v.Length - 1) = "%"c Then
                v = v.Substring(0, v.Length - 1)
                unit = Globals.eUnit.percent
            Else
                unit = Globals.eUnit.pixels
            End If
            Try
                value = Xml.XmlConvert.ToSingle(v)
            Catch ex As Exception
                value = 0
            End Try
        End If
    End Sub

    Public Overrides Function ToString() As String
        Select Case unit
            Case Globals.eUnit.percent
                Return value.ToString & "%"
            Case Globals.eUnit.pixels
                Return value.ToString
        End Select
    End Function
End Structure