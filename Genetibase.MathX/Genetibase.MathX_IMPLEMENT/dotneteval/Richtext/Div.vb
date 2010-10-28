Imports System
Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Drawing
Imports System.Text.RegularExpressions

Public Class div
    Inherits visualElement
    Friend mContent As ArrayList
    Friend mOrientation As eOrientation

    Public Sub New(ByVal RichText As RichText)
        MyBase.New(RichText)
        mContent = New ArrayList
    End Sub
    Public Enum eOrientation
        horizontal
        vertical
    End Enum
    Friend Overrides Sub SetRectangle(ByVal g As Graphics, ByVal rectangle As Rectangle)
        If mRichtext.mReorganize = False AndAlso mRectangle.Equals(rectangle) Then Exit Sub
        MyBase.SetRectangle(g, rectangle)
        If mContent Is Nothing Then Exit Sub
        Dim nb As Integer = mContent.Count
        Dim r As Rectangle
        Dim el As visualElement
        Const GUESSMIN As Integer = 16
        Dim guess As Integer
        ' Calculate the wanted size
        Dim Fixed, totalFixed As Double
        Dim totalGuess As Double
        For i As Integer = 0 To nb - 1
            el = CType(mContent(i), visualElement)
            Select Case mOrientation
                Case eOrientation.horizontal
                    If TypeOf el Is layer Then
                        ' if it is a layer we don't add this to the current div
                    ElseIf el.XmlHeight > 0 Then
                        totalFixed += el.XmlHeight
                    Else
                        el.GetIdealSize(g, mOrientation, mRectangle)
                        guess = el.mRectangle.Height
                        If guess < GUESSMIN Then guess = GUESSMIN
                        el.mRectangle.Height = guess 'temporary it will be changed again using the zoom
                        totalGuess += guess
                    End If
                Case eOrientation.vertical
                    If el.XmlWidth > 0 Then
                        totalFixed += el.XmlWidth
                    Else
                        el.GetIdealSize(g, mOrientation, mRectangle)
                        guess = el.mRectangle.Width
                        If guess < GUESSMIN Then guess = GUESSMIN
                        el.mRectangle.Width = guess 'temporary it will be changed again using the zoom
                        totalGuess += guess
                    End If
            End Select
        Next i

        ' Calculate the appriopriate zoom
        Dim zoomGuess As Double

        If totalGuess = 0 Then
            zoomGuess = 0
        Else
            Select Case mOrientation
                Case eOrientation.horizontal
                    zoomGuess = (mRectangle.Height - totalFixed) / totalGuess
                Case eOrientation.vertical
                    zoomGuess = (mRectangle.Width - totalFixed) / totalGuess
            End Select
        End If

        ' Apply the zoom
        Dim pos1 As Integer
        Dim siz As Integer
        Select Case mOrientation
            Case eOrientation.horizontal
                pos1 = mRectangle.Y
            Case eOrientation.vertical
                pos1 = mRectangle.X
        End Select

        For i As Integer = 0 To nb - 1
            el = CType(mContent(i), visualElement)
            If TypeOf el Is layer Then
                ' if it is a layer we don't add this to the current div
                el.SetRectangle(g, rectangle)
            Else
                r = mRectangle
                Select Case mOrientation
                    Case eOrientation.horizontal
                        If el.XmlHeight > 0 Then
                            siz = CInt(el.XmlHeight)
                        Else
                            siz = CInt(zoomGuess * el.Rectangle.Height)
                        End If
                        r.Y = pos1
                        r.Height = siz
                        el.SetRectangle(g, r)
                        pos1 += siz
                    Case eOrientation.vertical
                        If el.XmlWidth > 0 Then
                            siz = CInt(el.XmlWidth)
                        Else
                            siz = CInt(zoomGuess * el.Rectangle.Width)
                        End If
                        r.X = pos1
                        r.Width = siz
                        el.SetRectangle(g, r)
                        pos1 += siz
                End Select
            End If
        Next i
    End Sub
    Public Property Orientation() As eOrientation
        Get
            Return mOrientation
        End Get
        Set(ByVal Value As eOrientation)
            If Not Value = mOrientation Then
                mOrientation = Value
                If Not mRichText Is Nothing Then mRichText.RaiseXmlChanged()
            End If
        End Set
    End Property
    Friend Overrides Sub Paint(ByVal g As Graphics, ByVal ClipRectangle As Rectangle)
        MyBase.Paint(g, ClipRectangle)
        If mContent Is Nothing Then Exit Sub
        Dim el As visualElement
        For i As Integer = 0 To mContent.Count - 1
            el = DirectCast(mContent(i), visualElement)
            el.Paint(g, ClipRectangle)
        Next
    End Sub
    Protected Overrides Function parseXmlAttribute(ByVal name As String, ByVal value As String) As Boolean
        Select Case name
            Case "orientation"
                mOrientation = CType([Enum].Parse(GetType(eOrientation), value), eOrientation)
                Return True
            Case Else
                Return MyBase.parseXmlAttribute(name, value)
        End Select
    End Function
    Friend Overrides Sub SaveXML(ByVal writer As System.Xml.XmlWriter)
        writer.WriteStartElement("div")
        MyBase.SaveXMLAttributes(writer)
        writer.WriteAttributeString("orientation", Me.mOrientation.ToString)

        Dim e As visualElement
        For i As Integer = 0 To mContent.Count - 1
            e = DirectCast(mContent(i), visualElement)
            'For Each e As element In mContent
            e.SaveXML(writer)
        Next
        writer.WriteEndElement()
    End Sub
    Protected Overrides Function parseXmlElement(ByVal name As String) As skinElement
        Dim NewElement As visualElement
        Select Case name
            Case "p"
                NewElement = New paragraph(mrichtext)
                NewElement.Attach(Me, Integer.MaxValue)
                Return NewElement
            Case "img"
                NewElement = New image(mRichText)
                NewElement.Attach(Me, Integer.MaxValue)
                Return NewElement
            Case "div"
                NewElement = New div(mrichtext)
                NewElement.Attach(Me, Integer.MaxValue)
                Return NewElement
            Case "layer"
                NewElement = New layer(mRichText)
                NewElement.Attach(Me, Integer.MaxValue)
                Return NewElement
            Case Else
                Return MyBase.parseXmlElement(name)
        End Select
    End Function
    Protected Overrides Sub parseComplete()
        If Not Me.mDiv Is Nothing AndAlso mContent.Count = 0 Then
            ' we don't really like being here empty
            Me.mDiv.mContent.Remove(Me)
            Me.mDiv = Nothing
        End If
        Exit Sub
    End Sub
    Protected Overrides Sub parseXmlText(ByVal value As String)
        ' div should not contain text
        ' however being me, I am flexible
        Dim p As New paragraph(mrichtext, value)
        p.Attach(Me, Integer.MaxValue)
    End Sub
End Class
