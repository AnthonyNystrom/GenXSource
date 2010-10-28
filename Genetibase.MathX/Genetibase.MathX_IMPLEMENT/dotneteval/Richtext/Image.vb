Imports System
Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Drawing
Imports System.Text.RegularExpressions

Public Class image
    Inherits visualElement

    Private mImageName As String
    'Private mImage As Drawing.Image
    Public ImageRectangle As Rectangle

    Public Sub New(ByVal RichText As RichText)
        MyBase.New(RichText)
    End Sub

    Public ReadOnly Property Image() As Drawing.Image
        Get
            If Len(mImageName) = 0 Then
                Return Nothing
            Else
                Try
                    Return Skin.GetImage(mImageName)
                Catch ex As Exception
                    Return Nothing
                    'ignore
                End Try
            End If
        End Get
    End Property

    Protected Overrides Function parseXmlAttribute(ByVal name As String, ByVal value As String) As Boolean
        Select Case name
            Case "src"
                ImageName = value
                Return True
            Case Else
                Return MyBase.parseXmlAttribute(name, value)
        End Select
    End Function

    Friend Overrides Sub Paint(ByVal g As Graphics, ByVal ClipRectangle As Rectangle)
        MyBase.Paint(g, ClipRectangle)
        If Not Image Is Nothing Then
            If ImageRectangle.Width > 0 And ImageRectangle.Height > 0 Then
                g.DrawImage(Image, ImageRectangle)
            End If
        End If
    End Sub

    Friend Overrides Sub GetIdealSize(ByVal g As Graphics, ByVal orientation As div.eOrientation, ByVal fitinto As Rectangle)
        ' By default we want 100%
        If Image Is Nothing Then
            mRectangle.Width = 10
            mRectangle.Height = 10
        Else
            Dim idealSize As Size = Image.Size
            If idealSize.Width = 0 Then idealSize.Width = 1
            If idealSize.Height = 0 Then idealSize.Height = 1

            Dim zoomx As Double = fitinto.Width / idealSize.Width
            Dim zoomy As Double = fitinto.Height / idealSize.Height
            If zoomx > zoomy Then zoomx = zoomy ' take the smallest zoom
            mRectangle.Width = CInt(idealSize.Width * zoomx)
            mRectangle.Height = CInt(idealSize.Height * zoomx)
        End If
    End Sub

    Friend Overrides Sub SetRectangle(ByVal g As System.Drawing.Graphics, ByVal rectangle As System.Drawing.Rectangle)
        MyBase.SetRectangle(g, rectangle)
        If Image Is Nothing Then Exit Sub
        Dim ImageSize As Size
        Dim ratio As Double

        If XmlWidth > 0 And XmlHeight > 0 Then
            ' fixed ratio
            ImageSize = New Size(CInt(XmlWidth), CInt(XmlHeight))
        Else
            ' image ration
            ImageSize = Image.Size
        End If

        If ImageSize.Width = 0 Then ImageSize.Width = 1
        If ImageSize.Height = 0 Then ImageSize.Height = 1

        Dim zoomx As Double = mRectangle.Width / ImageSize.Width
        Dim zoomy As Double = mRectangle.Height / ImageSize.Height
        If zoomx > zoomy Then zoomx = zoomy ' take the smallest zoom
        ImageRectangle = mRectangle
        ImageRectangle.Width = CInt(ImageSize.Width * zoomx)
        ImageRectangle.Height = CInt(ImageSize.Height * zoomx)
        SetLocation()
    End Sub

    Private Sub SetLocation()
        If ImageRectangle.Width < mRectangle.Width Then
            Select Case mAlign
                Case eAlign.topleft, eAlign.bottomleft, eAlign.left
                    ImageRectangle.X = mRectangle.X
                Case eAlign.topright, eAlign.bottomright, eAlign.right
                    ImageRectangle.X = mRectangle.X + mRectangle.Width - ImageRectangle.Width
                Case Else
                    ImageRectangle.X = mRectangle.X + (mRectangle.Width - ImageRectangle.Width) \ 2
            End Select
        End If
        If ImageRectangle.Height < mRectangle.Height Then
            Select Case mAlign
                Case eAlign.top, eAlign.topleft, eAlign.topright
                    ImageRectangle.Y = mRectangle.Y
                Case eAlign.bottom, eAlign.bottomleft, eAlign.bottomright
                    ImageRectangle.Y = mRectangle.Y + (mRectangle.Height - ImageRectangle.Height)
                Case Else
                    ImageRectangle.Y = mRectangle.Y + (mRectangle.Height - ImageRectangle.Height) \ 2
            End Select
        End If
    End Sub

    Public Property ImageName() As String
        Get
            Return mImageName
        End Get
        Set(ByVal Value As String)
            mImageName = Value
        End Set
    End Property

    Friend Overrides Sub SaveXML(ByVal writer As System.Xml.XmlWriter)
        writer.WriteStartElement("img")
        MyBase.SaveXMLAttributes(writer)
        writer.WriteAttributeString("src", ImageName)
        writer.WriteEndElement()
    End Sub

    Public Overrides Property Align() As eAlign
        Get
            Return MyBase.Align
        End Get
        Set(ByVal Value As eAlign)
            If Not Value = MyBase.Align Then
                MyBase.Align = Value
                SetLocation()
            End If
        End Set
    End Property
End Class
