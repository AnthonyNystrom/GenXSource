Imports System.Math

Namespace Genetibase.MathX.NuGenHyperbolicTrignometricFunction

    ''' <summary> Interpolation </summary>
    Public Class NuGenInterpolation2

        ''' <summary> Trigonometric cordic subroutine
        ''' This subroutine calculates the sine and cosine
        ''' of an angle using the cordic rotation method.
        ''' The input angle is A.
        ''' The sine is returned in U, and the cosine in V.
        ''' Remember to dimension W(I) and A(I) in the calling program
        ''' If the angle is zero, set functions and return.
        ''' </summary>
        Public Shared Sub TRIGCORD(ByVal A() As Decimal, ByRef AT As Decimal, ByRef U As Decimal, ByRef V As Decimal)

            If AT = 0 Then
                U = 0
                V = 1
                Return
            End If

            Dim I As Int32, N As Int32, P As Decimal, A0 As Decimal
            ' GET THE TANGENT COEFFICIENTS
            TRIG_CORDIC_COEFFICIENT(N, A, P, A0)  '45518

            ' DETERMINE THE WEIGHTS, W(I)1060
            Dim W(N) As Decimal, Z As Decimal
            TRIG_CORDIC_WEIGHTS(N, AT, A0, W, Z) '45506

            Dim U0 As Decimal = P
            Dim V0 As Decimal = 0
            Dim U1 As Decimal
            Dim V1 As Decimal

            ' PERFORM THE ROTATIONS UP TO THE RESIDUAL
            For I = 1 To N
                ' UPDATE U0 AND V0
                U1 = U0 - W(I) * A(I) * V0
                V1 = V0 + W(I) * A(I) * U0
                U0 = U1
                V0 = V1
            Next I

            ' PERFORM THE RESIDUAL ROTATION USING Z
            ' USE U0 AND V0 AS DUMMY VARIABLES
            U0 = 1 - Z * Z / 2
            V0 = Z * (1 + Z * Z / 3)
            ' U AND V ARE THE FINAL RESULTS
            V = U0 * (U1 - V0 * V1)
            U = U0 * (V1 + V0 * U1)
            ' U=SIN(A)
            ' V=COS(A)
        End Sub

        ''' <summary>  Trig cordic weights subroutine
        ''' The weights are W(I)= plus or minus 1
        ''' The input angle is A
        ''' </summary>
        Public Shared Sub TRIG_CORDIC_WEIGHTS(ByVal N As Int32, ByVal AT As Decimal, ByVal A0 As Decimal, ByRef W() As Decimal, ByRef Z As Decimal)

            Dim I As Int32
            Z = AT

            For I = 1 To N
                W(I) = -1

                If Z > 0 Then W(I) = 1
                Z = Z - W(I) * A0
                A0 = A0 / 2
            Next I

            ' Z IS THE RESIDUAL ANGLE
        End Sub

        ''' <summary>  Trig cordic coefficient subroutine
        ''' For case N=12
        ''' </summary>
        Public Shared Sub TRIG_CORDIC_COEFFICIENT(ByRef N As Int32, ByRef A() As Decimal, ByRef P As Decimal, ByRef A0 As Decimal)
            N = 12
            ' THE TANGENTS ARE GIVEN IN A(I)
            A(1) = 1
            A(2) = 0.414213562373095
            A(3) = 0.198912367379658
            A(4) = 0.0984914033571642
            A(5) = 0.0491268497694672
            A(6) = 0.0245486221089254
            A(7) = 0.0122724623795663
            A(8) = 0.0061360001576234
            A(9) = 0.00306797120142267
            A(10) = 0.00153398199108867
            A(11) = 0.000766990544343093
            A(12) = 0.000383495215771441
            ' P REPRESENTS P(N)
            P = 0.636619787972041
            ' A0 IS PI/4
            A0 = 0.785398163397448
        End Sub

        ''' <summary> Inverse trigonometric cordic subroutine
        ''' This subroutine calculates the angle corresponding
        ''' to a given sine, cosine or tangent using the
        ''' cordic rotation method.
        ''' The input is U=SIN(A), V=COS(A), or W=TAN(A).
        ''' The returned value is A.
        ''' Ember to dimension W(I) and A(I) in the calling program.
        ''' Translate the U,V,W inputs
        ''' </summary>
        Public Shared Sub INVCORD(ByVal A() As Decimal, ByVal WT() As Decimal, ByVal W As Decimal, ByVal U As Decimal, ByVal V As Decimal, ByRef Ar As Decimal)

            Dim bFound As Boolean = False

            If Not (U = 0) Then

                ' INVERSE SINE IS WANTED
                If Abs(U) >= 0.0001 Then V = Sqrt(1 - U * U)
                If Abs(U) < 0.0001 Then V = 1 - U * U / 2 - U * U * U * U / 8
                bFound = True
            End If

            If Not (V = 0) Then

                ' INVERSE COSINE IS WANTED
                If Abs(V) >= 0.0001 Then U = Sqrt(1 - V * V)
                If Abs(V) < 0.0001 Then U = 1 - V * V / 2 - V * V * V * V / 8
                bFound = True
            End If

            If Not (W = 0) Then

                ' INVERSE TANGENT IS WANTED
                If Abs(W) <= 10000 Then U = 1 / Sqrt(1 + 1 / (W * W))
                If Abs(W) > 10000 Then U = 1 - 1 / (2 * W * W) + 3 / (8 * W * W * W * W)
                If Abs(U) >= 0.0001 Then V = Sqrt(1 - U * U)
                If Abs(U) < 0.0001 Then V = 1 - U * U / 2 - U * U * U * U / 8
                U = U * Abs(W) / W
                bFound = True
            End If

            If bFound = False Then
                Ar = 0
                Return
            End If

            ' GET COEFFICIENTS
            Dim N As Int32, P As Decimal, A0 As Decimal
            TRIG_CORDIC_COEFFICIENT(N, A, P, A0) '45616

            ' TEST FOR SPECIAL VALUES
            If Not ((Abs(U) < 1) Or (Abs(V) > 0)) Then
                ' SPECIAL CASE FOUND
                Ar = 2 * A0 * Abs(U) / U
                Return
            End If

            If Not ((Abs(V) < 1) Or (Abs(U) > 0)) Then
                Ar = 0
                Return
            End If

            ' SWITCH U WITH V AND INITIALIZE
            Ar = U
            U = V
            V = Ar

            Dim U0 As Decimal = P
            Dim U1 As Decimal = U0
            Dim V0 As Decimal = 0
            Dim V1 As Decimal = V0

            ' PERFORM THE ROTATIONS UP TO THE RESIDUAL
            Dim I As Int32

            For I = 1 To N
                ' IS ROTATION TO BE PLUS OR MINUS?
                WT(I) = -1

                If V0 < V Then WT(I) = 1
                ' UPDATE U0 AND V0
                U1 = U0 - WT(I) * A(I) * V0
                V1 = V0 + WT(I) * A(I) * U0
                U0 = U1
                V0 = V1
            Next I

            ' THE SET OF W(I) WEIGHTS HAVE NOW BEEN DETERMINED
            ' PERFORM THE RESIDUAL ANGLE APPROXIMATION
            Dim Z As Decimal = V * U1 - U * V1
            Z = Z + Z * Z * Z / 6

            ' ASSEMBLE RESULTS
            For I = 1 To N
                Z = Z + WT(I) * A0
                A0 = A0 / 2
            Next I

            ' RESULT IS IN Z
            Ar = Z
        End Sub

        ''' <summary>  Modified cordic exponential subroutine
        ''' This program takes an input value and returns Y=EXP(X).
        ''' X may be any positive or negative value.
        ''' Remember to dimension A(I) and W(I) in the calling program.
        ''' </summary>
        Public Shared Sub EXPCORD(ByVal X As Decimal, ByRef Y As Decimal) ' ByVal A() As Decimal, ByVal W() As Decimal)

            ' GET COEFFICIENTS
            Dim N As Int32, Z As Decimal, A(9) As Decimal, E As Decimal, W(9) As Decimal
            EXPONENTIAL_COEFFICIENTS(N, E, A) ' 45684

            ' REDUCE THE RANGE OF X
            'INT - OK
            Dim K As Int32 = Int(X)
            X = X - K
            ' DETERMINE THE WEIGHTING COEFFICIENTS, W(I)
            WEIGHT_DETERMINATION(N, X, W, Z) '45673
            ' CACLULATE PRODUCTS
            Y = 1

            Dim I As Int32

            For I = 1 To N

                If W(I) > 0 Then Y = Y * A(I)
            Next I

            ' PERFORM RESIDUAL MULTIPLICATION
            Y = Y * (1 + Z * (1 + Z / 2 * (1 + Z / 3 * (1 + Z / 4))))

            ' ACCOUNT FOR FACTOR EXP(K)
            If K < 0 Then E = 1 / E
            If Not (Abs(K) < 1) Then

                For I = 1 To Abs(K)
                    Y = Y * E
                Next I

                ' RESTORE X
                X = X + K
            End If

        End Sub

        ''' <summary>  Weight determination subroutine </summary>
        Public Shared Sub WEIGHT_DETERMINATION(ByVal N As Int32, ByVal X As Decimal, ByRef W() As Decimal, ByRef Z As Decimal)

            Dim I As Int32
            Dim A As Decimal = 0.5
            Z = X

            For I = 1 To N
                W(I) = 0

                If Z > A Then W(I) = 1
                Z = Z - W(I) * A
                A = A / 2
            Next I

        End Sub

        ''' <summary> Exponential coefficients subroutine </summary>
        Public Shared Sub EXPONENTIAL_COEFFICIENTS(ByRef N As Int32, ByRef E As Decimal, ByRef A() As Decimal)
            N = 9
            E = 2.71828182845905
            A(1) = 1.64872127070013
            A(2) = 1.28402541668774
            A(3) = 1.13314845306683
            A(4) = 1.06449445891786
            A(5) = 1.0317434074991
            A(6) = 1.01574770858669
            A(7) = 1.00784309720645
            A(8) = 1.00391388933835
            A(9) = 1.001955033591
        End Sub

        ''' <summary> Modified cordic natural logarithm subroutine
        ''' This program takes an input value and returns Y=LN(X).
        ''' X may be any positive value.
        ''' Remember to dimension A(I) and W(I) in the calling program.
        ''' </summary>
        Public Shared Function LNCORDIC(ByVal X As Decimal) As Decimal

            Dim Y As Decimal

            ' GET COEFFICIENTS
            Dim N As Int32, A(15) As Decimal, E As Decimal
            LNCORDIC_EC(N, E, A) ' 45770

            Dim W(N) As Decimal

            ' IF X<=0 THEN AN ERROR EXISTS, RETURN
            If X <= 0 Then Return 0

            Dim K As Int32 = 0

            ' REDUCE THE RANGE OF X
            While Not (X < E)
                ' DIVIDE OUT A POWER OF E
                K = K + 1
                X = X / E
            End While

            ' TEST IF X>=1. IF SO GO TO NEXT STEP
            ' OTHERWISE, BRING X TO >1
            While Not (X >= 1)
                K = K - 1
                X = X * E
            End While

            ' DETERMINE THE WEIGHTING COEFFICIENTS, W(I)
            Dim Z As Decimal
            LNCORDIC_WD(N, X, A, Z, W) ' 45761
            ' CALCULATE RESIDUAL FACTOR BASED ON Z
            ' WANT LN(Z), WHERE Z IS NEAR UNITY
            Z = Z - 1
            Z = Z * (1 - (Z / 2) * (1 + (Z / 3) * (1 - Z / 4)))

            ' ASSEMBLE RESULTS
            Dim AT As Decimal = 1 / 2
            Dim I As Int32

            For I = 1 To N
                Z = Z + W(I) * AT
                AT = AT / 2
            Next I

            ' Z IS NOW THE MANTISSA, K THE CHARACTERISTIC
            Y = K + Z
            Return Y
        End Function

        ''' <summary> Weight determination subroutine </summary>
        Public Shared Sub LNCORDIC_WD(ByVal N As Int32, ByVal X As Decimal, ByVal A() As Decimal, ByRef Z As Decimal, ByRef W() As Decimal)

            Dim I As Int32
            Z = X

            For I = 1 To N
                W(I) = 0

                If Z > A(I) Then W(I) = 1
                If W(I) = 1 Then Z = Z / A(I)
            Next I

        End Sub

        ''' <summary> Exponential coefficients subroutine </summary>
        Public Shared Sub LNCORDIC_EC(ByRef N As Int32, ByRef E As Decimal, ByRef A() As Decimal)
            N = 15
            E = 2.71828182845905
            A(1) = 1.64872127070013
            A(2) = 1.28402541668774
            A(3) = 1.13314845306683
            A(4) = 1.06449445891786
            A(5) = 1.0317434074991
            A(6) = 1.01574770858669
            A(7) = 1.00784309720645
            A(8) = 1.00391388933835
            A(9) = 1.001955033591
            A(10) = 1.00097703949242
            A(11) = 1.00048840047869
            A(12) = 1.00024417042975
            A(13) = 1.00012207776338
            A(14) = 1.00006103701893
            A(15) = 1.00003051804379
        End Sub

        ''' <summary> Hyperbolic sine subroutine
        ''' This program uses the definition of the
        ''' hyperbolic sine and the modified cordic
        ''' exponential subroutine to approximate
        ''' ARCSINH(X) over the entire range of real X.
        ''' The input to the subroutine is X.
        ''' The returned value is Y=ARCSINH(X).
        ''' </summary>
        Public Shared Function SINH(ByVal X As Decimal) As Decimal

            ' START CALCULATION
            Dim Y As Decimal

            ' IS X SMALL ENOUGH TO CAUSE ROUND OFF ERROR?
            If Not (Abs(X) < 0.35) Then
                ' CALCULATE SINH(X) USING EXPONENTIAL DEFINITION
                ' GET EXP(X)
                EXPCORD(X, Y) ' 45650
                ' CALCULATE SINH(X)
                Y = (Y - (1 / Y)) / 2
                Return Y
            End If

            ' SERIES APPROXIMATION
            Dim Z As Decimal = 1
            Y = 1

            Dim I As Int32

            For I = 1 To 8
                Z = Z * X * X / ((2 * I) * (2 * I + 1))
                Y = Y + Z
            Next I

            Y = X * Y
            Return Y
        End Function

        ''' <summary> Hyperbolic cosine subroutine
        ''' This program uses the definition of the
        ''' hyperbolic cosine and the modified cordic
        ''' exponential subroutine to approximate
        ''' ARCOSH(X) over the entire range of real X.
        ''' The returned value is Y=ARCOSH(X).
        ''' </summary>
        Public Shared Function COSH(ByVal X As Decimal) As Decimal

            ' START CALCULATION
            ' GET EXP(X)
            Dim Y As Decimal
            EXPCORD(X, Y) ' 45650
            Y = (Y + (1 / Y)) / 2
            Return Y
        End Function

        ''' <summary> Hyperbolic tangent subroutine
        ''' This program uses the definition
        '''   TAN(X)=SINH(X)/COSH(X)
        ''' To calculate the hyperbolic tangent.
        ''' The input is X.
        ''' The output is Y=TANH(X).
        ''' Start calculation
        ''' Get SINH(X)
        ''' </summary>
        Public Shared Function TANH(ByVal X As Decimal) As Decimal

            Dim sinhX As Decimal, coshX As Decimal, Y As Decimal
            sinhX = SINH(X) '45800
            coshX = COSH(X) ' 45825
            Y = sinhX / coshX
            Return Y
        End Function

        ''' <summary> ARCSINH(X) subroutine
        ''' This routine calculates the inverse
        ''' hyperbolic sine using the modified
        ''' cordic natural logarithm subroutine.
        ''' The input is X.
        ''' The output is Y=ARCSINH(X).
        ''' </summary>
        Public Shared Function INVSINH(ByVal X As Decimal)

            ' START CALCULATION
            ' TEST FOR ZERO ARGUMENT
            If X = 0 Then
                Return 0
            End If

            Dim Y As Decimal
            Dim signX As Int32 = Math.Sign(X) ' (X / Abs(X))
            X = Abs(X)
            X = X + Sqrt(X * X + 1)
            ' GET LOGARITHM
            Y = LNCORDIC(X) ' 45725
            ' INSERT SIGN
            Y = signX * Y
            Return Y
        End Function

        ''' <summary> ARCCOSH(X) subroutine
        ''' This routine calculates the inverse
        ''' hyperbolic cosine using the modified
        ''' cordic natural logarithm subroutine.
        ''' The input is X.
        ''' The output is Y=ARCOSH(X).
        ''' </summary>
        Public Shared Function INVCOSH(ByVal X As Decimal) As Decimal

            Dim Y As Decimal

            ' BEGIN CALCULATION
            ' TEST FOR ARGUMENT LESS THAN OR EQUAL TO UNITY
            If X <= 1 Then
                Return 0
            End If

            X = Abs(X)
            X = X + Sqrt(X - 1) * Sqrt(X + 1)
            ' GET LOGARITHM
            Y = LNCORDIC(X) ' 45725
            Return Y
        End Function

        ''' <summary> ARCTANH(X) subroutine
        ''' This program calculates the inverse
        ''' hyperbolic tangent using the modified
        ''' cordic natural logarithm subroutine.
        ''' The input is X.
        ''' The output is Y=ARCTANH(X).
        ''' </summary>
        Public Shared Function INVTANH(ByVal X As Decimal) As Decimal

            Dim Y As Decimal

            ' START CALCULATION
            ' TEST FOR X>= +/- 1
            If Abs(X) >= 1 Then
                Y = (X / Abs(X)) * 1000000.0# * 1000000.0# * 1000000.0#
                Return Y
            End If

            ' TEST FOR ZERO ARGUMENT
            If X <> 0 Then
                Return 0
            End If

            X = (1 + X) / (1 - X)
            ' GET LOGARITHM
            Y = LNCORDIC(X) ' 45725
            Return Y
        End Function

    End Class

End Namespace
