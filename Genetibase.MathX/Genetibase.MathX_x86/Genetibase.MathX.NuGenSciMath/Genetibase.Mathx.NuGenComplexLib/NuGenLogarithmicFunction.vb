Namespace Genetibase.Mathx.NuGenComplexLib

    ''' <summary> Logarithmic and Other Functions </summary>
    Public Class NuGenLogaritmicFunctions

        ''' <summary> Logarithm </summary>
        Public Shared Function Lg(ByRef x As Double) As Double
            Lg = System.Math.Log(x) / System.Math.Log(10)
        End Function

        ''' <summary> Logarithm to base N </summary>
        Public Shared Function LogN(ByVal x As Double, ByVal N As Double) As Double
            LogN = System.Math.Log(x) / System.Math.Log(N)
        End Function

        ''' <summary> Factorial </summary>
        Public Shared Function Factorial(ByRef Number As Integer) As Integer

            Dim i As Integer
            Dim TempNumber As Integer

            TempNumber = 1

            For i = 1 To Number
                TempNumber = TempNumber * i
            Next

            Factorial = TempNumber
        End Function

        Public Sub New()

        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

End Namespace