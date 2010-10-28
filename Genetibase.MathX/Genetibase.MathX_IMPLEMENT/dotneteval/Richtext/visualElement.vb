Imports System
Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Drawing
Imports System.Text.RegularExpressions

Public MustInherit Class visualElement
    Inherits skinElement
    Private mTag As String
    Private mMargins As Margins
    Private mXmlWidth As NumValue
    Private mXmlHeight As NumValue

    Protected mRichText As RichText
    Protected mDiv As div
    Protected mAlign As eAlign = eAlign.center
    Friend mRectangle As Rectangle
    Private mTooltip As String

    Public Property Tooltip() As String
        Get
            If mTooltip Is Nothing Then
                If mDiv Is Nothing Then
                    Return Nothing
                Else
                    Return mDiv.Tooltip
                End If
            Else
                Return mTooltip
            End If
        End Get
        Set(ByVal Value As String)
            mTooltip = Value
        End Set
    End Property

    Public Property Tag() As String
        Get
            If mTag Is Nothing Then
                If mDiv Is Nothing Then
                    Return Nothing
                Else
                    Return mDiv.Tag
                End If
            Else
                Return mTag
            End If
        End Get
        Set(ByVal Value As String)
            mTag = Value
        End Set
    End Property

    Public Overridable Property Align() As eAlign
        Get
            Return mAlign
        End Get
        Set(ByVal Value As eAlign)
            If Not Value = mAlign Then
                mAlign = Value
                If Not mRichText Is Nothing Then mRichText.RaiseXmlChanged()
            End If
        End Set
    End Property

    Public ReadOnly Property Margins() As Margins
        Get
            Return mMargins
        End Get
    End Property

    Sub New(ByVal RichText As RichText)
        mRichText = RichText
        mMargins = New Margins
    End Sub

    Public Sub Attach(ByVal div As div, ByVal idx As Integer)
        If Not mDiv Is Nothing Then mDetach()
        If Not div Is Nothing Then
            mDiv = div
            If idx > mDiv.mContent.Count Then idx = mDiv.mContent.Count
            If idx < 0 Then idx = 0
            mDiv.mContent.Insert(idx, Me)
        End If
        If Not TypeOf div Is layer Then ForceReorganize()
        If Not mRichText Is Nothing Then mRichText.RaiseXmlChanged()
    End Sub

    Friend Sub ForceReorganize()
        ' Force to reorganize the control on next paint
        Me.mRectangle.Width = -1
        Me.mRectangle.Height = -1
        If Not mDiv Is Nothing Then
            mDiv.ForceReorganize()
        Else
            mRichText.RaiseRepaintNeeded()
        End If
    End Sub

    Private Sub mDetach()
        Dim d As div = mDiv
        If Not d Is Nothing Then
            mDiv = Nothing
            d.mContent.Remove(Me)
            d.ForceReorganize()
            mRichText.RaiseXmlChanged()
        End If
    End Sub

    Public Sub Delete()
        Dim parentdiv As div = mDiv
        Dim idx As Integer
        If TypeOf Me Is div Then
            Dim d As div = DirectCast(Me, div)
            If (parentdiv Is Nothing) Then
                ' we can't delete the root unless it contains only one item
                mDetach()
                If d.mContent.Count = 1 Then
                    mRichText.mRoot = DirectCast(d.mContent(0), visualElement)
                End If
            Else
                mDetach()
                While d.mContent.Count > 0
                    DirectCast(d.mContent(0), visualElement).Attach(parentdiv, idx)
                    idx += 1
                End While
                idx = parentdiv.mContent.IndexOf(Me)
            End If
        Else
            mDetach()
        End If
        mRichText.RaiseXmlChanged()
    End Sub

    Public ReadOnly Property XmlWidth() As Single
        Get
            Select Case mXmlWidth.unit
                Case Globals.eUnit.pixels
                    Return mXmlWidth.value
                Case Globals.eUnit.percent
                    If Not mDiv Is Nothing Then
                        Return (mXmlWidth.value * mDiv.Rectangle.Width) / 100.0!
                    End If
            End Select
        End Get
    End Property

    Public ReadOnly Property XmlHeight() As Single
        Get
            Select Case mXmlWidth.unit
                Case Globals.eUnit.pixels
                    Return mXmlWidth.value
                Case Globals.eUnit.percent
                    If Not mDiv Is Nothing Then
                        Return (mXmlHeight.value * mDiv.Rectangle.Height) / 100.0!
                    End If
            End Select
        End Get
    End Property

    Public Sub addElement(ByVal e As visualElement, ByVal pos As ePosition)
        mRichText.BeginUpdates()
        Try
            e.mDetach()
            Dim o As div.eOrientation
            Select Case pos
                Case ePosition.ABove, ePosition.Below
                    o = div.eOrientation.horizontal
                Case ePosition.OnTheLeft, ePosition.OnTheRight
                    o = div.eOrientation.vertical
            End Select
            Dim curDiv As div = mDiv
            If curDiv Is Nothing Then
                curDiv = New div(mRichText)
                Me.Attach(curDiv, Integer.MaxValue)
                mRichText.mRoot = curDiv
            End If
            Dim idx As Integer = curDiv.mContent.IndexOf(Me)
            If (curDiv.mContent.Count > 1) AndAlso curDiv.mOrientation <> o Then
                Dim newDiv As div
                curDiv.mContent.Remove(Me)
                newDiv = New div(mRichText)
                newDiv.Attach(curDiv, idx)
                newDiv.mOrientation = o
                newDiv.mContent.Add(Me)
                mDiv = newDiv
                curDiv = newDiv
                idx = 0
            Else
                curDiv.mOrientation = o
            End If
            If pos = ePosition.Below Or pos = ePosition.OnTheRight Then
                idx += 1
            End If
            e.Attach(curDiv, idx)
            If Not mRichText Is Nothing Then mRichText.RaiseXmlChanged()
        Finally
            mRichText.EndUpdates()
        End Try
    End Sub

    Public Function AddNewElement(ByVal type As eElementType, ByVal pos As ePosition) As visualElement
        Dim e As visualElement
        Select Case type
            Case eElementType.Image
                e = New image(mRichText)
            Case eElementType.Text
                e = New paragraph(mRichText, "New paragraph")
        End Select
        addElement(e, pos)
    End Function

    Friend Overridable Sub SetRectangle(ByVal g As Graphics, ByVal rectangle As Rectangle)
        Dim LeftMargin As Integer = mMargins.Left
        Dim RightMargin As Integer = mMargins.Right
        Dim TopMargin As Integer = mMargins.Top
        Dim BottomMargin As Integer = mMargins.Bottom
        If mRichText.ShowBorders Then
            If LeftMargin < 2 Then LeftMargin = 2
            If RightMargin < 2 Then RightMargin = 2
            If TopMargin < 2 Then TopMargin = 2
            If BottomMargin < 2 Then BottomMargin = 2
        End If
        mRectangle = rectangle
        mRectangle.X += LeftMargin
        mRectangle.Y += TopMargin
        mRectangle.Width -= LeftMargin + RightMargin
        mRectangle.Height -= TopMargin + BottomMargin
        If mRectangle.Width < 0 Then mRectangle.Width = 0
        If mRectangle.Height < 0 Then mRectangle.Height = 0
    End Sub

    Friend Overridable Sub Paint(ByVal g As Graphics, ByVal ClipRectangle As Rectangle)
        If mRichText.ShowBorders Then
            Dim p As Pen
            If TypeOf Me Is div Then
                p = Pens.Red
            ElseIf TypeOf Me Is image Then
                p = Pens.Green
            ElseIf TypeOf Me Is paragraph Then
                p = Pens.Blue
            End If

            If mRichText.SelectedElement Is Me Then
                g.DrawRectangle(p, mRectangle.X - 1, mRectangle.Y - 1, _
                   mRectangle.Width + 1, mRectangle.Height + 1)
            End If
        End If
    End Sub

    Friend Overridable Sub GetIdealSize(ByVal g As Graphics, ByVal orientation As div.eOrientation, ByVal fitinto As Rectangle)
        ' By default we want 100%
        mRectangle.Width = fitinto.Width \ 2
        mRectangle.Height = fitinto.Height \ 2
    End Sub

    Friend ReadOnly Property Rectangle() As Rectangle
        Get
            Return mRectangle
        End Get
    End Property

    Protected Overrides Function parseXmlAttribute(ByVal name As String, ByVal value As String) As Boolean
        Select Case name
            Case "width"
                mXmlWidth.Parse(value)
                Return True
            Case "height"
                mXmlHeight.Parse(value)
                Return True
            Case "align"
                mAlign = CType([Enum].Parse(GetType(eAlign), value), eAlign)
                Return True
            Case "tag"
                mTag = value
                Return True
            Case "tooltip"
                Tooltip = value
                Return True
            Case Else
                Return MyBase.parseXmlAttribute(name, value)
        End Select
    End Function

    'Friend MustOverride Sub SaveXML(ByVal writer As XmlWriter)

    Friend Overrides Sub SaveXMLAttributes(ByVal writer As XmlWriter)
        mMargins.SaveXMLAttributes(writer)
        If mXmlWidth.value > 0 Then writer.WriteAttributeString("width", mXmlWidth.ToString)
        If mXmlHeight.value > 0 Then writer.WriteAttributeString("height", mXmlHeight.ToString)

        If mAlign <> eAlign.default Then writer.WriteAttributeString("align", mAlign.ToString)
        If Len(Tooltip) > 0 Then writer.WriteAttributeString("tooltip", Me.Tooltip)
        If Len(mTag) > 0 Then writer.WriteAttributeString("tag", mTag)
    End Sub

    Protected Overrides Function parseXmlElement(ByVal name As String) As skinElement
        If name = "margins" Then
            Return mMargins
        Else
            Return MyBase.parseXmlElement(name)
        End If
    End Function
End Class
