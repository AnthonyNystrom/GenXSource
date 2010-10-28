Namespace Genetibase.MathX.NuGenHyperbolicTrignometricFunction

    '''<summary>Hyperbolic Trigonometric Functions </summary>
    Class NuGenHyperbolicTrigonometricFunctions

        ''' <summary>Hyperbolic Sine </summary>
        Public Shared Function HSin(ByVal x As Double) As Double
            HSin = (System.Math.Exp(x) - System.Math.Exp(-x)) / 2
        End Function

        ''' <summary> Hyperbolic Cosine </summary>
        Public Shared Function HCos(ByVal x As Double) As Double
            HCos = (System.Math.Exp(x) + System.Math.Exp(-x)) / 2
        End Function

        ''' <summary> Hyperbolic Tangent </summary>
        Public Shared Function HTan(ByVal x As Double) As Double
            HTan = (System.Math.Exp(x) - System.Math.Exp(-x)) / (System.Math.Exp(x) + System.Math.Exp(-x))
        End Function

        ''' <summary> Hyperbolic Secant </summary>
        Public Shared Function HSec(ByVal x As Double) As Double
            HSec = 2 / (System.Math.Exp(x) + System.Math.Exp(-x))
        End Function

        ''' <summary> Hyperbolic Cosecant </summary>
        Public Shared Function HCosec(ByVal x As Double) As Double
            HCosec = 2 / (System.Math.Exp(x) - System.Math.Exp(-x))
        End Function

        ''' <summary> Hyperbolic Cotangent </summary>
        Public Shared Function HCotan(ByVal x As Double) As Double
            HCotan = (System.Math.Exp(x) + System.Math.Exp(-x)) / (System.Math.Exp(x) - System.Math.Exp(-x))
        End Function

        ''' <summary> Inverse Hyperbolic Sine </summary>
        Public Shared Function HArcsin(ByVal x As Double) As Double
            HArcsin = System.Math.Log(x + System.Math.Sqrt(x * x + 1))
        End Function

        ''' <summary> Inverse Hyperbolic Cosine </summary>
        Public Shared Function HArccos(ByVal x As Double) As Double
            HArccos = System.Math.Log(x + System.Math.Sqrt(x * x - 1))
        End Function

        ''' <summary> Inverse Hyperbolic Tangent </summary>
        Public Shared Function HArctan(ByVal x As Double) As Double
            HArctan = System.Math.Log((1 + x) / (1 - x)) / 2
        End Function

        ''' <summary> Inverse Hyperbolic Secant </summary>
        Public Shared Function HArcsec(ByVal x As Double) As Double
            HArcsec = System.Math.Log((System.Math.Sqrt(-x * x + 1) + 1) / x)
        End Function

        ''' <summary> Inverse Hyperbolic Cosecant </summary>
        Public Shared Function HArccosec(ByVal x As Double) As Double
            HArccosec = System.Math.Log((System.Math.Sign(x) * System.Math.Sqrt(x * x + 1) + 1) / x)
        End Function

        ''' <summary> Inverse Hyperbolic Cotangent </summary>
        Public Shared Function HArccotan(ByVal x As Double) As Double
            HArccotan = System.Math.Log((x + 1) / (x - 1)) / 2
        End Function

    End Class

End Namespace
