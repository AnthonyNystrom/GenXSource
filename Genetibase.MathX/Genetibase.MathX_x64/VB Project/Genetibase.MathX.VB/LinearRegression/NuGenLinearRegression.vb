Option Strict Off
Option Explicit On 

''' <summary> Linear Regression </summary>
Public Class NuGenLinearRegression

    ''' <summary> Linear Regression </summary>
    Public Shared Function LRegress(ByRef Values() As Double, ByRef ResultsCopyTo() As Double) As Object

        Dim X As Short
        Dim Y() As Double
        Dim intLoop As Short
        Dim n As Short
        Dim q1 As Double
        Dim q2 As Double
        Dim q3 As Double
        Dim XY As Double
        Dim XSquared As Double
        Dim YSquared As Double
        Dim XSum As Double
        Dim YSum As Double
        Dim XSquaredSum As Double
        Dim YSquaredSum As Double
        Dim XYSum As Double
        X = UBound(Values)
        ReDim Y(X)

        For intLoop = 1 To X
            Y(intLoop) = Values(intLoop) 'Copy values to X
        Next intLoop

        For intLoop = 1 To X
            XSum = XSum + intLoop
            YSum = YSum + Y(intLoop)
            XSquaredSum = XSquaredSum + (intLoop * intLoop)
            YSquaredSum = YSquaredSum + (Y(intLoop) * Y(intLoop))
            XYSum = XYSum + (Y(intLoop) * intLoop)
        Next intLoop

        n = X 'Number of periods in calculation
        q1 = (XYSum - ((XSum * YSum) / n))
        q2 = (XSquaredSum - ((XSum * XSum) / n))
        q3 = (YSquaredSum - ((YSum * YSum) / n))
        ResultsCopyTo(1) = (q1 / q2) '= (0.1 * (XYSum - (3 * YSum))) 'Slope
        ResultsCopyTo(2) = (YSum - ResultsCopyTo(1) * XSum) / n 'Intercept
        ResultsCopyTo(3) = (((n + 1) * ResultsCopyTo(1)) + ResultsCopyTo(2)) 'Forecast
        ResultsCopyTo(4) = (q1 * q1) / (q2 * q3) 'Coefficient of determination (R-Squared)
    End Function

End Class
