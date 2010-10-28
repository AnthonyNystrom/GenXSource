Imports System.ComponentModel
Imports System.Drawing

Public Class Series

    Private _index As Integer
    Private _label As String = "newSeries"
    Private _color As Color = Drawing.Color.Red

    <Browsable(False)> _
    Public Property Index() As Integer
        Get
            Return _index
        End Get
        Set(ByVal value As Integer)
            If (_index <> value) Then
                _index = value
            End If
        End Set
    End Property

    <DefaultValue("newSeries")> _
    Public Property Label() As String
        Get
            Return _label
        End Get
        Set(ByVal value As String)
            If (_label <> value) Then
                _label = value
            End If
        End Set
    End Property


    <TypeConverter(GetType(ColorConverter))> _
    Public Property Color() As Color
        Get
            Return _color
        End Get
        Set(ByVal value As Color)
            If (_color <> value) Then
                _color = value
            End If
        End Set
    End Property

    Public Sub New(ByVal index As Integer, ByVal label As String, ByVal color As Color)
        _index = index
        _label = label
        _color = color
    End Sub

    Public Sub New(ByVal label As String, ByVal color As Color)
        _index = -1
        _label = label
        _color = color
    End Sub

    Public Sub New(ByVal label As String)
        _index = -1
        _label = label
        _color = Drawing.Color.Black
    End Sub

    Public Overrides Function ToString() As String
        Return _label
    End Function

End Class
