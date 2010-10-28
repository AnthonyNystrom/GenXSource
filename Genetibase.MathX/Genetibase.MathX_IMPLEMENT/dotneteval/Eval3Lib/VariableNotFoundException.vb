Public Class VariableNotFoundException
    Inherits Exception
    Public ReadOnly VariableName As String

    Public Sub New(ByVal variableName As String, _
       Optional ByVal innerException As Exception = Nothing)
        MyBase.New(variableName & " was not found", Nothing)
        Me.VariableName = variableName
    End Sub
End Class
