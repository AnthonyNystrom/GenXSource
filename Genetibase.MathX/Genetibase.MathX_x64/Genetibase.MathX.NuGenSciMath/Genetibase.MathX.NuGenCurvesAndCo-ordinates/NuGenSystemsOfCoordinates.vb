Imports System.Math


Namespace Genetibase.MathX.NuGenHyperbolicTrignometricFunction

    ''' <summary> Systems of Coordinates </summary>

    Public Class NuGenSystemsOfCoordinates

#Region "COORDINATE CONVERSION."

        ''' <summary> [40400] Rectangular to Polar conversion </summary>
        Public Shared Sub RECTPOL(ByVal X As Decimal, ByVal Y As Decimal, ByRef U As Decimal, ByRef V As Decimal)
            U = Sqrt(X * X + Y * Y)

            ' GUARD AGAINST AMBIGUOUS VECTOR
            If Y = 0 Then Y = (0.1) ^ 30

            ' GUARD AGAINST DIVIDE BY ZERO
            If X = 0 Then X = (0.1) ^ 30

            ' SOME BASICS REQUIRE A SIMPLE ARGUMENT
            Dim W As Decimal = Y / X
            V = Atan(W)

            REM CHECK QUADRANT AND ADJUST
            If X < 0 Then V = V + 3.1415926535
            If V < 0 Then V = V + 6.2831853072
            Return
        End Sub

        ''' <summary> [40450] Polar to Rectangular conversion </summary>
        Public Shared Sub POLRECT(ByVal U As Decimal, ByVal V As Decimal, ByRef X As Decimal, ByRef Y As Decimal)
            X = U * Cos(V)
            Y = U * Sin(V)
        End Sub

#End Region

#Region "COMPLEX VARIABLE OPERATIONS (+, -, X AND /)"

        ''' <summary> [40300] Complex number addition </summary>
        Public Shared Sub ZADD(ByRef X() As Decimal, ByRef Y() As Decimal)
            X(3) = X(1) + X(2)
            Y(3) = Y(1) + Y(2)
        End Sub

        ''' <summary> [40350] Complex number subtraction </summary>
        Public Shared Sub ZSUB(ByRef X() As Decimal, ByRef Y() As Decimal)
            X(3) = X(1) - X(2)
            Y(3) = Y(1) - Y(2)
        End Sub

        ''' <summary> [40500] Polar multiplication </summary>
        Public Shared Sub ZPOLMLT(ByVal U() As Decimal, ByVal V() As Decimal, ByRef UT As Decimal, ByRef VT As Decimal)
            UT = U(1) * U(2)
            VT = V(1) + V(2)

            If VT >= 6.2831853072 Then VT = VT - 6.2831853072
        End Sub

        ''' <summary> [40550] Polar division </summary>
        Public Shared Sub ZPOLDIV(ByVal U() As Decimal, ByVal V() As Decimal, ByRef UT As Decimal, ByRef VT As Decimal)
            UT = U(1) / U(2)
            VT = V(1) - V(2)

            If VT < 0 Then VT = VT + 6.2831853072
        End Sub

        ''' <summary> [40600] Rectangular complex number multiplication </summary>
        Public Shared Sub ZRECTMLT(ByVal XA() As Decimal, ByVal YA() As Decimal, ByRef X As Decimal, ByRef Y As Decimal)

            Dim U(2) As Decimal, V(2) As Decimal, UT As Decimal, VT As Decimal
            ' RECTANGULAR TO POLAR CONVERSION
            RECTPOL(XA(1), YA(1), U(1), V(1)) ' 40400
            ' RECTANGULAR TO POLAR CONVERSION
            RECTPOL(XA(2), YA(2), U(2), V(2)) ' 40400
            ' POLAR MULTIPLICATION
            ZPOLMLT(U, V, UT, VT) ' 40500
            ' POLAR TO RECTANGULAR CONVERSION
            POLRECT(UT, VT, X, Y) ' 40450
        End Sub

        ''' <summary> [40800] Rectangular complex number division </summary>
        Public Shared Sub ZRECTDIV(ByVal X() As Decimal, ByVal Y() As Decimal, ByRef XT As Decimal, ByRef YT As Decimal)

            Dim U(2) As Decimal, V(2) As Decimal, UT As Decimal, VT As Decimal
            ' RECTANGULAR TO POLAR CONVERSION
            RECTPOL(X(1), Y(1), U(1), V(1))  ' 40400
            ' RECTANGULAR TO POLAR CONVERSION
            RECTPOL(X(2), Y(2), U(2), V(2))  ' 40400
            ' POLAR COMPLEX NUMBER DIVISION
            ZPOLDIV(U, V, UT, VT) ' 40550
            ' POLAR TO RECTANGULAR CONVERSION
            POLRECT(UT, VT, XT, YT) ' 40450
        End Sub

        ''' <summary> [41100] Polar power </summary>
        Public Shared Sub ZPOLPOW(ByVal U As Decimal, ByVal V As Decimal, ByVal N As Decimal, ByRef U1 As Decimal, ByRef V1 As Decimal)
            U1 = U ^ N
            V1 = N * V
            V1 = V1 - 6.2831853072 * Int(V1 / 6.2831853072)
        End Sub

        ''' <summary> [41150] Polar (first) root </summary>
        Public Shared Sub ZPOLRT(ByVal N As Int32, ByVal U As Decimal, ByVal V As Decimal, ByRef U1 As Decimal, ByRef V1 As Decimal)
            U1 = U ^ (1 / N)
            V1 = V / N
        End Sub

#End Region

#Region "POWERS AND ROOTS OF Z=X+I*Y"

        ''' <summary> [41200] Rectangular complex number power </summary>
        Public Shared Sub ZRECTPOW(ByVal N As Int32, ByRef X As Decimal, ByRef Y As Decimal)

            Dim U As Decimal, V As Decimal
            'RECTANGULAR TO POLAR CONVERSION
            RECTPOL(X, Y, U, V) ' 40400
            'POLAR POWER
            ZPOLPOW(U, V, N, U, V) ' 41100
            'POLAR TO RECTANGULAR CONVERSION
            POLRECT(U, V, X, Y) ' 40450
        End Sub

        ''' <summary> [41300] Rectangular complex number root </summary>
        Public Shared Sub ZRECTRT(ByVal M As Int32, ByVal N As Int32, ByRef X As Decimal, ByRef Y As Decimal)

            Dim U As Decimal, V As Decimal, U1 As Decimal, V1 As Decimal
            ' RECTANGULAR TO POLAR CONVERSION
            RECTPOL(X, Y, U, V) ' 40400
            ' POLAR (FIRST) ROOT
            ZPOLRT(N, U, V, U1, V1)  '41150
            U = U1
            ' FIND M ORDER ROOT
            ' M=1 CORRESPONDS TO THE FIRST ROOT
            V = V1 + 6.2831853072 * (M - 1) / N
            ' POLAR TO RECTANGULAR CONVERSION
            POLRECT(U, V, X, Y) ' 40450
        End Sub

#End Region

#Region "COORDINATE CHANGE"

        ''' <summary> [41400] Spherical to Rectangular (cartesian) conversion </summary>
        Public Shared Sub SPRRECT(ByVal U As Decimal, ByVal V As Decimal, ByVal W As Decimal, ByRef X As Decimal, ByRef Y As Decimal, ByRef Z As Decimal)
            X = U * (Sin(W)) * Cos(V)
            Y = U * (Sin(W)) * Sin(V)
            Z = U * Cos(W)
        End Sub

        ''' <summary> [41450] Rectangular (cartesian) to Spherical conversion </summary>
        Public Shared Sub RECTSPR(ByVal X As Decimal, ByVal Y As Decimal, ByVal Z As Decimal, ByRef U As Decimal, ByRef V As Decimal, ByRef W As Decimal)
            ' RECTANGULAR TO POLAR CONVERSION
            RECTPOL(X, Y, U, V) ' 40400
            ' RECTANGULAR TO POLAR CONVERSION
            RECTPOL(U, Z, U, W) ' 40400

            If W > 1.5707963268 Then W = W - 6.28318553072
            W = 1.5707963268 - W
        End Sub

#End Region

    End Class

End Namespace