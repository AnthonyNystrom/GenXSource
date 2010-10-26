Option Strict On
Option Explicit On

Imports System.Collections.ObjectModel

Namespace DiffPatch
    Public Class DiffCollection(Of T)
        Inherits Collection(Of DiffEdit(Of T))

    End Class
End Namespace

