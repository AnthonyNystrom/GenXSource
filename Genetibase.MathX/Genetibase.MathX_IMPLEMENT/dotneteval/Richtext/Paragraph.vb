Imports System
Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Drawing
Imports System.Text.RegularExpressions

Public Class paragraph
    Inherits visualElement

    Private mFont As System.Drawing.Font
    Private mText As String
    Private mBrush As Brush
    Private mTextRectangle As RectangleF
    Private mDrawFormat As StringFormat

    Public ReadOnly Property Font() As System.Drawing.Font
        Get
            Return mFont
        End Get
    End Property

    Public Property Text() As String
        Get
            Return mText
        End Get
        Set(ByVal Value As String)
            If Not mText = Value Then
                mText = Value
                If Not mRichText Is Nothing Then mRichText.RaiseXmlChanged()
            End If
        End Set
    End Property

    Public Property Color() As String
        Get
            If TypeOf mBrush Is SolidBrush Then
                Dim c As Color = CType(mBrush, SolidBrush).Color
                If Not c.Equals(Drawing.Color.Black) Then
                    Return c.Name
                End If
            End If
        End Get
        Set(ByVal Value As String)
            Dim c As Color
            If Len(Value) = 0 Then
                c = System.Drawing.Color.Black
            Else
                Try
                    If Len(Value) > 0 Then
                        ' Quick and dirty parse color
                        If Value.Chars(0) = "#"c Then
                            c = Drawing.Color.FromArgb(CInt("&h" & Value.Substring(1)))
                        Else
                            c = Drawing.Color.FromName(Value)
                        End If
                    End If
                Catch ex As Exception
                    ' ignore
                    c = System.Drawing.Color.Black
                End Try
            End If
            If Not TypeOf mBrush Is SolidBrush _
               OrElse Not DirectCast(mBrush, SolidBrush).Color.Equals(c) Then
                mBrush = New SolidBrush(c)
            End If
            If Not mRichText Is Nothing Then mRichText.RaiseXmlChanged()
        End Set
    End Property

    Friend Overrides Sub Paint(ByVal g As Graphics, ByVal ClipRectangle As Rectangle)
        MyBase.Paint(g, ClipRectangle)
        If mTextRectangle.Width > 0 And mTextRectangle.Height > 0 Then
            'g.DrawRectangle(Pens.Green, mTextRectangle.X, mTextRectangle.Y, _
            '   mTextRectangle.Width, mTextRectangle.Height)
            g.DrawString(mText, mFont, mBrush, mTextRectangle, mDrawFormat)
        End If
    End Sub

    Public Property FontName() As String
        Get
            'context.serialize(wdgFont, mTxt.FontName)
            If mFont Is Nothing Then Return String.Empty
            Dim style As String
            If mFont.Underline Then style &= "underline "
            If mFont.Bold Then style &= "bold "
            If mFont.Italic Then style &= "italic "
            If mFont.Strikeout Then style &= "strikeout "
            If style Is Nothing Then
                If mFont.Size = 10 Then
                    If mFont.Name = "Microsoft Sans Serif" Then
                        Return String.Empty
                    Else
                        Return mFont.Name
                    End If
                Else
                    Return mFont.Name & "," & mFont.Size
                End If
            Else
                Return mFont.Name & "," & mFont.Size & "," & style.Trim
            End If

        End Get
        Set(ByVal Value As String)
            If Value Is Nothing Then Exit Property
            Dim fontparts As String() = Value.Split(","c)
            Dim fontface As String = "Microsoft Sans Serif"
            Dim fontstyle As String = ""
            Dim fontsize As Single = 8.25!

            If fontparts.Length > 0 Then fontface = fontparts(0)
            If fontparts.Length > 1 Then
                Dim fontSizeText As String = fontparts(1)
                If fontSizeText.Length = 0 Then
                    fontsize = 8.25!
                Else
                    Try
                        fontsize = CSng(fontparts(1))
                    Catch ex As Exception
                        fontsize = 8.25!
                    End Try
                End If
            End If
            If fontparts.Length > 2 Then fontstyle = fontparts(2)
            Dim fs As FontStyle
            If fontstyle.IndexOf("bold") >= 0 Then fs = fs Or System.Drawing.FontStyle.Bold
            If fontstyle.IndexOf("italic") >= 0 Then fs = fs Or System.Drawing.FontStyle.Italic
            If fontstyle.IndexOf("strikeout") >= 0 Then fs = fs Or System.Drawing.FontStyle.Strikeout
            If fontstyle.IndexOf("underline") >= 0 Then fs = fs Or System.Drawing.FontStyle.Underline
            Try
                mFont = New Drawing.Font(fontface, fontsize, fs, GraphicsUnit.Point)
            Catch ex As Exception
                ' do nothing
            End Try
            If mFont Is Nothing Then
                mFont = New Drawing.Font(FontFamily.GenericSansSerif, fontsize, Drawing.FontStyle.Regular, GraphicsUnit.Pixel)
            End If

        End Set
    End Property

    Friend Overrides Sub GetIdealSize(ByVal g As Graphics, ByVal orientation As div.eOrientation, ByVal fitinto As Rectangle)
        ' By default we want 100%
        If Len(mText) = 0 Then
            mRectangle.Width = 0
            mRectangle.Height = 0
        Else
            mRectangle = fitinto
            Try
                Dim sizef As SizeF
                mRectangle.Size = fitinto.Size
                Select Case orientation
                    Case div.eOrientation.horizontal
                        sizef.Width = fitinto.Width
                        sizef.Height = fitinto.Height
                    Case div.eOrientation.vertical
                        sizef.Width = fitinto.Width
                        sizef.Height = fitinto.Height
                End Select
                sizef = g.MeasureString(mText, mFont, sizef, mDrawFormat)
                mRectangle.Width = CInt(sizef.Width)
                mRectangle.Height = CInt(sizef.Height)
            Catch ex As Exception
                ' ignore
            End Try
        End If
    End Sub

    Friend Overrides Sub SetRectangle(ByVal g As System.Drawing.Graphics, ByVal rectangle As System.Drawing.Rectangle)
        MyBase.SetRectangle(g, rectangle)
        mTextRectangle = RectangleF.op_Implicit(mRectangle)
        'TextRectangle.Y += mFont.Size / 2
        ' TextRectangle.Height -= mFont.Size
    End Sub

    Public Sub New(ByVal Richtext As RichText, Optional ByVal text As String = Nothing)
        MyBase.New(Richtext)
        If text Is Nothing Then mText = String.Empty Else mText = text
        If mDrawFormat Is Nothing Then mDrawFormat = New StringFormat
        mDrawFormat.FormatFlags = (StringFormatFlags.DisplayFormatControl Or StringFormatFlags.LineLimit)
        mDrawFormat.Trimming = StringTrimming.EllipsisCharacter
        mFont = New Drawing.Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular, GraphicsUnit.Pixel)
        mBrush = New SolidBrush(Drawing.Color.Black)
    End Sub

    Protected Overrides Function parseXmlAttribute(ByVal name As String, ByVal value As String) As Boolean
        Select Case name
            Case "font"
                FontName = value
                Return True
            Case "color"
                Color = value
                Return True
            Case Else
                Return MyBase.parseXmlAttribute(name, value)
        End Select
    End Function

    Friend Overrides Sub SaveXML(ByVal writer As System.Xml.XmlWriter)
        writer.WriteStartElement("p")
        MyBase.SaveXMLAttributes(writer)
        If Len(FontName) > 0 Then writer.WriteAttributeString("font", FontName)
        If Len(Color) > 0 Then writer.WriteAttributeString("color", Color)
        writer.WriteString(mText)
        writer.WriteEndElement()
    End Sub


    Public Overrides Property Align() As eAlign
        Get
            Return MyBase.Align
        End Get
        Set(ByVal Value As eAlign)
            If Not Value = MyBase.Align Then
                MyBase.Align = Value
                SetDrawFormat()
            End If
        End Set
    End Property

    Private Sub SetDrawFormat()
        If mDrawFormat Is Nothing Then mDrawFormat = New StringFormat
        Select Case mAlign
            Case eAlign.top, eAlign.topleft, eAlign.topright
                mDrawFormat.LineAlignment = StringAlignment.Near
            Case eAlign.bottom, eAlign.bottomleft, eAlign.bottomright
                mDrawFormat.LineAlignment = StringAlignment.Far
            Case Else
                mDrawFormat.LineAlignment = StringAlignment.Center
        End Select
        Select Case mAlign
            Case eAlign.topleft, eAlign.bottomleft, eAlign.left
                mDrawFormat.Alignment = StringAlignment.Near
            Case eAlign.topright, eAlign.bottomright, eAlign.right
                mDrawFormat.Alignment = StringAlignment.Far
            Case Else
                mDrawFormat.Alignment = StringAlignment.Center
        End Select
        mDrawFormat.FormatFlags = (StringFormatFlags.DisplayFormatControl Or StringFormatFlags.LineLimit)
        mDrawFormat.Trimming = StringTrimming.EllipsisCharacter
    End Sub

    Protected Overrides Sub parseComplete()
        SetDrawFormat()
    End Sub

    Protected Overrides Sub parseXmlText(ByVal value As String)
        mText &= value
    End Sub
End Class
