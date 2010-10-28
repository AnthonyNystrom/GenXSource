Option Strict Off
Option Explicit On

Namespace Genetibase.MathX.NuGenTrignometricOperations


    ''' <summary> Trigonometric Functions </summary>
    Class NuGenTrigonometricFunctions

        ''' <summary> Secant </summary>
        Public Shared Function Sec(ByVal x As Double) As Double
            Sec = 1 / System.Math.Cos(x)
        End Function

        ''' <summary> Cosecant </summary>
        Public Shared Function Cosec(ByVal x As Double) As Double
            Cosec = 1 / System.Math.Sin(x)
        End Function

        ''' <summary> Cotangent </summary>
        Public Shared Function Cotan(ByVal x As Double) As Double
            Cotan = 1 / System.Math.Tan(x)
        End Function

        ''' <summary> Inverse Sine </summary>
        Public Shared Function Arcsin(ByVal x As Double) As Double

            If System.Math.Abs(x) = 1 Then
                Arcsin = System.Math.Atan(1) * System.Math.Sign(x) * 2

            Else
                Arcsin = System.Math.Atan(x / System.Math.Sqrt(-x * x + 1))
            End If

        End Function

        ''' <summary> Inverse Cosine </summary>
        Public Shared Function Arccos(ByVal x As Double) As Double

            If System.Math.Abs(x) = 1 Then
                Arccos = System.Math.Atan(1) * (1 - System.Math.Sign(x)) * 4

            Else
                Arccos = System.Math.Atan(-x / System.Math.Sqrt(-x * x + 1)) + 2 * System.Math.Atan(1)
            End If

        End Function

        ''' <summary> Inverse Secant </summary>
        Public Shared Function Arcsec(ByVal x As Double) As Double

            If System.Math.Abs(x) = 1 Then
                Arcsec = System.Math.Atan(1) * System.Math.Sign(x) * 4

            Else
                Arcsec = System.Math.Atan(x / System.Math.Sqrt(x * x - 1)) + System.Math.Sign((x) - 1) * (2 * System.Math.Atan(1))
            End If

        End Function

        ''' <summary> Inverse Cosecant </summary>
        Public Shared Function Arccosec(ByVal x As Double) As Double

            If System.Math.Abs(x) = 1 Then
                Arccosec = System.Math.Atan(1) * (System.Math.Sign(x) * 4 - 2)

            Else
                Arccosec = System.Math.Atan(x / System.Math.Sqrt(x * x - 1)) + (System.Math.Sign(x) - 1) * (2 * System.Math.Atan(1))
            End If

        End Function

        ''' <summary> Inverse Cotangent </summary>
        Public Shared Function Arccotan(ByVal x As Double) As Double
            Arccotan = System.Math.Atan(x) + 2 * System.Math.Atan(1)
        End Function

    End Class

End Namespace