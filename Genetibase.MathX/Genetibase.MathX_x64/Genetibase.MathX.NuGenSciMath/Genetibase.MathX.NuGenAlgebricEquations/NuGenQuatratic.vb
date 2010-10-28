Imports System

Namespace Genetibase.MathX.NuGenAlgebricEquations


    ''' <summary> Quadratic </summary>
    Public Class NuGenQuadratic

        Private a As Double
        Private b As Double
        Private c As Double

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal a As Double, ByVal b As Double, ByVal c As Double)

            Me.a = a
            Me.b = b
            Me.c = c
        End Sub 'New

        ''' <summary> Positive solution </summary>
        Public Function solutionPos() As Double
            Return (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a)
        End Function 'solutionPos

        ''' <summary> Negative solution </summary>
        Public Function solutionNeg() As Double
            Return (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a)
        End Function 'solutionNeg

    End Class 'Quadratic

End Namespace