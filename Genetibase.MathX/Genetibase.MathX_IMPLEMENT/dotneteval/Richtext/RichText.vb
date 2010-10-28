Imports System
Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Drawing
Imports System.Text.RegularExpressions

Public Class RichText
    Protected Friend mReorganize As Boolean
    Protected Friend mRoot As visualElement

    Private mXML As String
    Private mError As String
    Private mUpdating As Integer
    Private Shared mErrorFont As Drawing.Font
    Private Shared sb As New System.Text.StringBuilder
    Private mXmlChanged As Boolean
    Private mXmlChangedRaised As Boolean
    Public Event XmlChanged()
    Public Event RepaintNeeded()
    ' for debugging
    Private mShowBorders As Boolean = False
    Private mSelectedElement As visualElement


    Property SelectedElement() As visualElement
        Get
            Return mSelectedElement
        End Get
        Set(ByVal Value As visualElement)
            If Not Value Is mSelectedElement Then
                mSelectedElement = Value
                If mShowBorders Then RaiseRepaintNeeded()
            End If
        End Set
    End Property

    Property ShowBorders() As Boolean
        Get
            Return mShowBorders
        End Get
        Set(ByVal Value As Boolean)
            If Not Value = mShowBorders Then
                mShowBorders = Value
                mReorganize = True
                RaiseRepaintNeeded()
            End If
        End Set
    End Property

    Sub New()
        mRoot = New paragraph(Me, "")
    End Sub

    Public Property XML() As String
        Get
            ' when an outside object ask for the XML we just consider it has unchanged
            If mXmlChanged Then
                ' Rebuild the xml
                Dim sw As New StringWriter
                Dim xw As New System.Xml.XmlTextWriter(sw)
                xw.Formatting = Formatting.Indented
                mRoot.SaveXML(xw)
                mXML = sw.ToString
                mXmlChanged = False
                mXmlChangedRaised = False
            End If
            Return mXML
        End Get
        Set(ByVal Value As String)
            If Not mXML = Value Then
                mXML = Value
                mXmlChangedRaised = False
                BeginUpdates()
                Try
                    mRoot = Nothing
                    mError = Nothing
                    Dim nt As NameTable = New NameTable
                    Dim nsmgr As XmlNamespaceManager = New XmlNamespaceManager(nt)
                    nsmgr.AddNamespace("magic", "urn:soslegal.co.uk/magic")
                    Dim context As XmlParserContext = New XmlParserContext(Nothing, nsmgr, Nothing, XmlSpace.None)
                    Dim Reader As New XmlTextReader(mXML, XmlNodeType.Element, context)

                    Dim div As New div(Me)
                    skinElement.LoadXml(div, Reader)
                    Reader.Close()
                    If div.mContent.Count = 1 Then
                        Dim el As visualElement = CType(div.mContent(0), visualElement)
                        mRoot = el
                    Else
                        mRoot = div
                    End If
                    RaiseXmlChanged()
                Catch ex As Exception
                    mRoot = Nothing
                    mError = ex.Message & vbCrLf & mXML
                Finally
                    EndUpdates()
                End Try
            End If
        End Set
    End Property

    Public Sub Paint(ByVal g As Graphics, ByVal ClientRectangle As Rectangle, ByVal ClipRectangle As Rectangle)
        If mRoot Is Nothing Then
            If Not mError Is Nothing Then
                If mErrorFont Is Nothing Then mErrorFont = New Drawing.Font(FontFamily.GenericMonospace, 10)
                g.DrawString(mError, mErrorFont, Brushes.Black, RectangleF.op_Implicit(ClientRectangle))
            End If
            Exit Sub
        End If
        If mReorganize OrElse Not mRoot.Rectangle.Equals(ClientRectangle) Then
            mRoot.SetRectangle(g, ClientRectangle)
            mReorganize = False
        End If
        mRoot.Paint(g, ClipRectangle)
    End Sub

    Public Function GetTooltip(ByVal pt As Point) As String
        Dim e As visualElement = Me.GetElementAt(pt)
        Dim t As String = e.Tooltip
        If t Is Nothing Then
            If TypeOf (e) Is paragraph Then
                Return DirectCast(e, paragraph).Text()
            ElseIf TypeOf (e) Is image Then
                ' We could return image name but I don't think this is very useful
                ' Return DirectCast(e, RichText.image).ImageName
                Return ""
            Else
                Return ""
            End If
        Else
            Return t
        End If
    End Function

    Public Shared Function escape(ByVal s As String) As String
        If s Is Nothing Then Return String.Empty
        sb.Length = 0
        For i As Integer = 0 To s.Length - 1
            Select Case s.Chars(i)
                Case "&"c
                    sb.Append("&amp;")
                Case "<"c
                    sb.Append("&lt;")
                Case ">"c
                    sb.Append("&gt;")
                Case Else
                    sb.Append(s.Chars(i))
            End Select
        Next
        Return sb.ToString
    End Function

    Public Function GetElementAt(ByVal pt As Point) As visualElement
        Dim e As visualElement = mRoot
        Dim d As div
        If e Is Nothing Then Return Nothing
        Dim found As Boolean
        While TypeOf e Is div
            d = DirectCast(e, div)
            found = False

            For i As Integer = 0 To d.mContent.Count - 1
                e = DirectCast(d.mContent(i), visualElement)

                'For Each e In d.mContent
                If e.Rectangle.Contains(pt) Then
                    found = True
                    Exit For
                End If
            Next
            If Not found Then Return d
        End While
        Return e
    End Function

    Protected Friend Sub RaiseXmlChanged()
        mXmlChanged = True
        If mXmlChangedRaised = False And mUpdating = 0 And Not mRoot Is Nothing Then
            mXmlChangedRaised = True
            RaiseEvent XmlChanged()
            RaiseEvent RepaintNeeded()
        End If
    End Sub

    Protected Friend Sub RaiseRepaintNeeded()
        RaiseEvent RepaintNeeded()
    End Sub

    Friend Sub BeginUpdates()
        mUpdating += 1
    End Sub

    Friend Sub EndUpdates()
        If mUpdating > 0 Then
            mUpdating -= 1
            If mXmlChanged Then RaiseXmlChanged()
        End If
    End Sub

    Public Sub ForceReorganize()
        mReorganize = True
    End Sub

End Class

