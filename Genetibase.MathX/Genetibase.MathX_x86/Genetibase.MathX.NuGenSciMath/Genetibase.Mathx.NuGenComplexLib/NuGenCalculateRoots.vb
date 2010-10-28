Option Strict Off
Option Explicit On

Namespace Genetibase.Mathx.NuGenComplexLib


    ''' <summary> Find roots </summary>
    Class NuGenRoots

        ''' <summary> Function </summary>
        Private Shared Function F(ByRef x As Decimal, ByRef Root As Decimal, ByRef Value As Decimal) As Decimal
            F = x ^ Root - Value
        End Function

        ''' <summary> Find roots </summary>
        Public Shared Function FindRoots(ByRef Root As Decimal, ByRef Value As Decimal) As Decimal

            Dim Middle As Decimal
            Dim Times As Integer
            Dim RightEdge As Decimal
            Dim LeftEdge As Decimal
            LeftEdge = 0
            RightEdge = 10000000

            For Times = 1 To 10000
                Middle = (LeftEdge + RightEdge) / 2

                If F(LeftEdge, Root, Value) > 0 And F(Middle, Root, Value) > 0 Then LeftEdge = Middle
                If F(LeftEdge, Root, Value) > 0 And F(Middle, Root, Value) <= 0 Then RightEdge = Middle
                If F(LeftEdge, Root, Value) <= 0 And F(Middle, Root, Value) > 0 Then RightEdge = Middle
                If F(LeftEdge, Root, Value) <= 0 And F(Middle, Root, Value) <= 0 Then LeftEdge = Middle
            Next

            FindRoots = (LeftEdge + RightEdge) / 2
        End Function

    End Class

End Namespace