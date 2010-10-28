Option Strict Off
Option Explicit On


Namespace Genetibase.MathX.NuGenTrignometricOperations

    ''' <summary> Trigonometry Functions </summary>
    Public Class NuGenTrigonometry

        ''' <summary> Converts the angles in degrees to radians </summary>
        Public Shared Function ConvertDegreesToRadians(ByVal degrees As Decimal) As Decimal
            ' VB only works in radians
            ConvertDegreesToRadians = CDbl(degrees) * CDbl(Math.PI) / 180
        End Function

        ''' <summary> Converts the angles in radians to degrees </summary>
        Public Shared Function ConvertRadiansToDegrees(ByVal radians As Decimal) As Decimal
            ConvertRadiansToDegrees = CDbl(radians) * 180 / CDbl(Math.PI)
        End Function

        ''' <summary> Angles -- input and output in degrees </summary>
        Public Shared Function HypotenuseFromLengthOpposedAngle(ByVal opp As Decimal, ByVal opposedAngle As Decimal) As Decimal
            '       -- internal work done in radians
            HypotenuseFromLengthOpposedAngle = CDbl(opp) / System.Math.Sin(CDbl(ConvertDegreesToRadians(opposedAngle)))
        End Function

        ''' <summary> LengthFromHypotenuseOpposedAngle </summary>
        Public Shared Function LengthFromHypotenuseOpposedAngle(ByVal hypotenuse As Decimal, ByVal opposedAngle As Decimal) As Decimal
            LengthFromHypotenuseOpposedAngle = CDbl(hypotenuse) * System.Math.Sin(CDbl(ConvertDegreesToRadians(opposedAngle)))
        End Function

        ''' <summary> HypotenuseFromlengthAdjacenteAngle </summary>
        Public Shared Function HypotenuseFromlengthAdjacenteAngle(ByVal adj As Decimal, ByVal adjacenteAngle As Decimal) As Decimal
            HypotenuseFromlengthAdjacenteAngle = CDbl(adj) / System.Math.Cos(CDbl(ConvertDegreesToRadians(adjacenteAngle)))
        End Function

        ''' <summary> LengthFromHypotenuseAdjacenteAngle </summary>
        Public Shared Function LengthFromHypotenuseAdjacenteAngle(ByVal hypotenuse As Decimal, ByVal adjacenteAngle As Decimal) As Decimal
            LengthFromHypotenuseAdjacenteAngle = CDbl(hypotenuse) * System.Math.Cos(CDbl(ConvertDegreesToRadians(adjacenteAngle)))
        End Function

        ''' <summary> LengthFromOpposedAdjacenteAngle </summary>
        Public Shared Function LengthFromOpposedAdjacenteAngle(ByVal opposed As Decimal, ByVal adjacenteAngle As Decimal) As Decimal
            LengthFromOpposedAdjacenteAngle = CDbl(opposed) / System.Math.Tan(CDbl(ConvertDegreesToRadians(adjacenteAngle)))
        End Function

        ''' <summary> LengthFromOpposedOpposedAngle </summary>
        Public Shared Function LengthFromOpposedOpposedAngle(ByVal opposed As Decimal, ByVal opposedAngle As Decimal) As Decimal
            LengthFromOpposedOpposedAngle = CDbl(opposed) * System.Math.Tan(CDbl(ConvertDegreesToRadians(opposedAngle)))
        End Function

        ''' <summary> AngleFromLegths </summary>
        Public Shared Function AngleFromLegths(ByVal opposed As Decimal, ByVal adjacente As Decimal) As Decimal
            AngleFromLegths = ConvertRadiansToDegrees(System.Math.Atan(CDbl(opposed) / CDbl(adjacente)))
        End Function

        ''' <summary> AngleFromHypotenuseOpposed </summary>
        Public Shared Function AngleFromHypotenuseOpposed(ByVal opposed As Decimal, ByVal hypotenuse As Decimal) As Decimal

            Dim X As Decimal = CDbl(opposed) / CDbl(hypotenuse) ' X = an extra variable to leave the derived math function in its simplest form
            AngleFromHypotenuseOpposed = ConvertRadiansToDegrees(System.Math.Atan(CDbl(X) / System.Math.Sqrt(CDbl(-X) * CDbl(X) + 1)))
        End Function

    End Class

End Namespace