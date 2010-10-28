Option Strict Off
Option Explicit On

Namespace Genetibase.Mathx.NuGenComplexLib

    ''' <summary> The error class </summary>
    Public Class NuGenErrorLib

        'local variables to hold property value(s)
        Private lngNumber As Integer 'local copy
        Private strDescription As String 'local copy
        Private strSource As String

        Public Property Number() As Integer
            Get
                'used when retrieving value of a property, on the right side of an assignment.
                'Syntax: Variable = X.MaxFileSize
                Number = lngNumber
            End Get
            Set(ByVal Value As Integer)
                'used when assigning a value to the property, on the left side of an assignment.
                'Syntax: X.MaxFileSize = 5
                lngNumber = Value
            End Set
        End Property

        Public Property Description() As String
            Get
                'used when retrieving value of a property, on the right side of an assignment.
                'Syntax: Variable = X.MaxFileSize
                Description = strDescription
            End Get
            Set(ByVal Value As String)
                'used when assigning a value to the property, on the left side of an assignment.
                'Syntax: X.MaxFileSize = 5
                strDescription = Value
            End Set
        End Property

        Public Property Source() As String
            Get
                'used when retrieving value of a property, on the right side of an assignment.
                'Syntax: Variable = X.MaxFileSize
                Source = strSource
            End Get
            Set(ByVal Value As String)
                'used when assigning a value to the property, on the left side of an assignment.
                'Syntax: X.MaxFileSize = 5
                strSource = Value
            End Set
        End Property

        Public Sub Clear()
            lngNumber = 0
            strDescription = ""
            strSource = ""
        End Sub

    End Class

End Namespace
