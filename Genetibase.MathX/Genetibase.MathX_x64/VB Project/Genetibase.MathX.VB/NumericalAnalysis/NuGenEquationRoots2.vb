Imports System.Math

Namespace NumericalAnalysis

    ''' <summary> Equation Roots </summary>
    Public Class NuGenEquationRoots2

        ''' <summary> Function subroutines </summary>
        Private Shared Sub SUBROUTINE_1_2_6_7(ByVal X As Decimal, ByVal Y As Decimal, ByRef U As Decimal, ByRef V As Decimal)
            U = X * X - Y * Y + 1
            V = 2 * X * Y
        End Sub

        Private Shared Sub SUBROUTINE_3(ByVal X As Decimal, ByVal Y As Decimal, ByRef U As Decimal, ByRef V As Decimal, _
            ByRef U1 As Decimal, ByRef V1 As Decimal, ByRef U2 As Decimal, ByRef V2 As Decimal)
            U = X * X - Y * Y + 1
            V = 2 * X * Y
            U1 = 2 * X
            U2 = -2 * Y
            V1 = 2 * Y
            V2 = 2 * X
        End Sub

        Private Shared Sub SUBROUTINE_4(ByVal X As Decimal, ByRef Y As Decimal)
            Y = (X + 1) * (X + 1) * (X + 1) * (X + 1) * (X + 1)
        End Sub

        Private Shared Sub SUBROUTINE_5(ByVal X As Decimal, ByVal Y As Decimal, ByRef W As Decimal)
            W = (X + 1) * (X + 1) * (X + 1) * (X + 1) * (X + 1) * (Y - 1) * (Y - 1) * (Y - 1) * (Y - 1) * (Y - 1)
        End Sub

        ''' <summary> Complex root counting subroutine 
        ''' This routine calculates the number of complex
        ''' roots within a circle of radius W centered
        ''' on (X0,Y0) by counting (U,V) transistions
        ''' around the circumference.
        ''' The input parameters are
        '''    W - the radius of the circle
        '''    (X0,Y0) - the center of the circle
        '''    M - the number of evaluation points per quadrant
        ''' The routine returns the number of roots found, N
        ''' and the number A, where A<>0 indicates a failure
        ''' in the algorithm.
        ''' It is assumed that the function is complex in the
        ''' domain being searched (U and V both have transitions).
        ''' It is also assumed that the sector spacing is close
        ''' enough to catch all transistions.
        ''' Note that N(I) must be dimensioned to 4M in the
        ''' calling program.
        ''' Observe that U(X,Y) and V(X,Y) are expected to be
        ''' found in the functions subroutine.
        ''' </summary>
        Public Shared Sub ROOTNUM(ByVal X0 As Int32, ByVal Y0 As Int32, ByVal W As Int32, ByVal M As Int32, ByRef NR As Int32, ByRef A As Decimal)

            Dim I As Int32, X As Decimal, Y As Decimal, U As Decimal, V As Decimal, N(4 * M) As Int32

            A = 3.14159 / (2 * M)

            ' START CALCULATION BY ESTABLISHING THE N(I) ARRAY
            For I = 1 To 4 * M
                X = W * Cos(A * (I - 1)) + X0
                Y = W * Sin(A * (I - 1)) + Y0

                SUBROUTINE_1_2_6_7(X, Y, U, V) ' 44300

                If U >= 0 Then If V >= 0 Then N(I) = 1
                If U < 0 Then If V >= 0 Then N(I) = 2
                If U < 0 Then If V < 0 Then N(I) = 3
                If U >= 0 Then If V < 0 Then N(I) = 4
            Next I

            ' COUNT COMPLETE CYCLES COUNTERCLOCKWISE
            NR = N(1)
            A = 0

            For I = 2 To 4 * M

                If NR = N(I) Then

                Else

                    If NR <> 4 Then If NR = N(I) + 1 Then A = A - 1
                    If NR = 1 Then If N(I) = 4 Then A = A - 1
                    If NR = 4 Then If N(I) = 1 Then A = A + 1
                    If NR + 1 = N(I) Then A = A + 1
                    NR = N(I)
                End If

            Next I

            ' COMPLETE CIRCLE
            If NR <> 4 Then If NR = N(1) + 1 Then A = A - 1
            If NR = 4 Then If N(1) = 1 Then A = A + 1
            If NR = 1 Then If N(1) = 4 Then A = A - 1
            If NR + 1 = N(1) Then A = A + 1
            A = Abs(A)
            'INT - OK
            NR = Int(A / 4)
            'INT - OK
            A = A - 4 * Int(A / 4)
        End Sub

        ''' <summary> Complex root search subroutine 
        ''' This program searches for the complex roots
        ''' of an analytical function by encircling the
        ''' zero and estimating where it is. The circle
        ''' is subsequently tightened by a factor E, and
        ''' a new estimate made.
        ''' The inputs to the subroutine are
        '''  (X0,Y0) - the initial guesses
        '''  W - the initial radius of the search circle
        '''  E - the factor by which the circle is reduced
        '''  N - the number of iterations
        '''  M - the number of evaluation points per quadrant
        ''' the results is returned in Z=X+IY (X,Y).
        ''' Also, the number of iterations performed, or
        ''' in progress, is returned in K.
        ''' X(I),Y(I),U(I) and V(I) must be dimensioned
        ''' in the calling program to 4M.
        ''' It is assumed that the function is decomposed
        ''' into its real and imaginary parts, U(X,Y) and
        ''' V(X,Y), and that these are accessible by a call
        ''' to the function subroutine which returns U and V.
        ''' Start calculation by finding the evaluation points
        ''' </summary>
        Public Shared Sub ZCIRCLE(ByVal X0 As Decimal, ByVal Y0 As Decimal, ByVal W As Decimal, ByVal E As Decimal, ByVal M As Int32, ByVal N As Int32, ByRef ZX As Decimal, ByRef ZY As Decimal, ByRef K As Int32)

            Dim A As Decimal, I As Int32, J As Int32, UT As Decimal, VT As Decimal
            Dim X(4 * M) As Decimal, Y(4 * M) As Decimal, U(4 * M) As Decimal, V(4 * M) As Decimal
            Dim X1 As Decimal, Y1 As Decimal, X2 As Decimal, Y2 As Decimal, X3 As Decimal, Y3 As Decimal, X4 As Decimal, Y4 As Decimal
            Dim B1 As Decimal, B2 As Decimal
            Dim M1 As Decimal, M2 As Decimal, M3 As Decimal, M4 As Decimal
            M = M * 4
            K = 1
            A = 6.28319 / M

            Do

                For I = 1 To M
                    X(I) = W * Cos(A * (I - 1)) + X0
                    Y(I) = W * Sin(A * (I - 1)) + Y0
                Next I

                ' DETERMINE THE CORRESPONDING U(I) AND(I)
                For I = 1 To M
                    ZX = X(I)
                    ZY = Y(I)

                    SUBROUTINE_1_2_6_7(ZX, ZY, UT, VT) ' 44300
                    U(I) = UT
                    V(I) = VT
                Next I

                ' FIND THE POSITION AT WHICH U CHANGES SIGN IN THE
                ' COUNTERCLOCKWISE DIRECTION
                I = 1
                UT = U(I)

                Dim TestCond As Int32 = 0

                Do
                    AUXILLIARY_SUBROUTINE(M, I, J) '46885

                    If UT * U(I) < 0 Then
                        TestCond = 1 ' 46785

                    Else

                        ' GUARD AGAINST INFINITE LOOP
                        If I = 1 Then
                            ' INFINITE LOOP ENCOUNTERED, RETURN
                            ZX = 0
                            ZY = 0
                            Return
                        End If
                    End If

                Loop While TestCond = 0

                ' TRANSITION FOUND
                M1 = I
                ' SEARCH FOR THE OTHER TRANSITION, STARTING
                ' ON THE OTHER SIDE OF THE CIRCLE
                I = M1 + M / 2

                If I > M Then I = I - M
                J = I
                UT = U(I)
                ' FLIP DIRECTIONS ALTERNATELY
                TestCond = 0

                Do
                    AUXILLIARY_SUBROUTINE(M, I, J) '46885

                    If UT * U(I) < 0 Then TestCond = 1 '46801
                    If UT * U(J) < 0 Then TestCond = 2 '46804
                    If TestCond = 0 Then

                        ' TEST FOR INFINITE LOOP
                        If (I = M1 + M / 2) Or (J = M1 + M / 2) Then
                            ' INFINITE LOOP ENCOUNTERED, RETURN
                            ZX = 0
                            ZY = 0
                            Return
                        End If
                    End If

                Loop While TestCond = 0

                If TestCond = 1 Then
                    ' TRANSITION FOUND
                    M3 = I

                Else 'TestCond = 2 

                    ' TRANSITION FOUND
                    If J = M Then J = 0
                    M3 = J + 1
                End If

                ' M1 AND M3 HAVE BEEN DETERMINED. NOW FOR M2 AND M4.
                ' NOW FOR THE V TRANSITIONS
                I = M1 + M / 4

                If I > M Then I = I - M
                J = I
                VT = V(I)
                TestCond = 0

                Do
                    AUXILLIARY_SUBROUTINE(M, I, J) '46885

                    If VT * V(I) < 0 Then TestCond = 1 '46820
                    If VT * V(J) < 0 Then TestCond = 2 '46822

                    ' AGAIN, GUARD AGAINST THE INFINITE LOOP
                    If TestCond = 0 Then
                        If (I = M1 + M / 4) Or (J = M1 + M / 4) Then
                            ' INFINITE LOOP ENCOUNTERED, RETURN
                            ZX = 0
                            ZY = 0
                            Return
                        End If
                    End If

                Loop While TestCond = 0

                If TestCond = 1 Then
                    ' M2 HAS BEEN FOUND
                    M2 = I

                Else 'TestCond = 2 

                    If J = M Then J = 0
                    M2 = J + 1
                End If

                ' M2 HAS BEEN FOUND. NOW FOR M4
                I = M2 + M / 2

                If I > M Then I = I - M
                J = I
                VT = V(I)
                TestCond = 0

                Do
                    AUXILLIARY_SUBROUTINE(M, I, J) '46885

                    If UT * V(I) < 0 Then TestCond = 1 ' 46836
                    If VT * V(J) < 0 Then TestCond = 2 ' 46838

                    ' GUARD AGAINST THE INFINITE LOOP AGAIN
                    If TestCond = 0 Then
                        If (I = M2 + M / 2) Or (J = M2 + M / 2) Then
                            ' INFINITE LOOP ENCOUNTERED, RETURN
                            ZX = 0
                            ZY = 0
                            Return
                        End If
                    End If

                Loop While TestCond = 0

                If TestCond = 1 Then
                    M4 = I

                Else 'TestCond = 2 

                    If J = M Then J = 0
                    M4 = J + 1
                End If

                ' ALL THE INTERSECTIONS HAVE BEEN DETERMINED
                ' INTERPOLATE TO FIND THE FOUR (X,Y) COORDINATES
                I = M1
                AUXILLIARY_SUBROUTINE_INTERPOLATION(M, I, X, Y, U, V, J, ZX, ZY) ' 46891
                X1 = ZX
                Y1 = ZY
                I = M2
                AUXILLIARY_SUBROUTINE_INTERPOLATION(M, I, X, Y, U, V, J, ZX, ZY) ' 46891
                X2 = ZX
                Y2 = ZY
                I = M3
                AUXILLIARY_SUBROUTINE_INTERPOLATION(M, I, X, Y, U, V, J, ZX, ZY) ' 46891
                X3 = ZX
                Y3 = ZY
                I = M4
                AUXILLIARY_SUBROUTINE_INTERPOLATION(M, I, X, Y, U, V, J, ZX, ZY) ' 46891
                X4 = ZX
                Y4 = ZY

                ' CALCULATE THE INTERSECTION OF THE LINES
                ' GUARD AGAINST A DIVIDE BY ZERO
                If X1 <> X3 Then
                    M1 = (Y3 - Y1) / (X3 - X1)

                    If X2 <> X4 Then
                        M2 = (Y2 - Y4) / (X2 - X4)

                    Else
                        M2 = 100000000.0#
                    End If

                Else
                    ZX = X1
                    ZY = (Y1 + Y3) / 2
                    M2 = (Y2 - Y4) / (X2 - X4)
                End If

                B1 = Y1 - M1 * X1
                B2 = Y2 - M2 * X2
                ZX = -(B1 - B2) / (M1 - M2)
                ZY = (M1 * B2 + M2 * B1) / (M1 + M2)

                ' IS ANOTHER ITERATION IN ORDER?
                If K = N Then Return
                X0 = ZX
                Y0 = ZY
                K = K + 1
                W = W * E
            Loop

        End Sub

        ''' <summary> Auxilliary subroutine </summary>
        Private Shared Sub AUXILLIARY_SUBROUTINE(ByVal M As Int32, ByRef I As Int32, ByRef J As Int32)
            I = I + 1
            J = J - 1

            If I > M Then I = I - M
            If J < 1 Then J = J + M
        End Sub

        ''' <summary> Auxilliary subroutine for interpolation </summary>
        Private Shared Sub AUXILLIARY_SUBROUTINE_INTERPOLATION(ByVal M As Int32, ByVal I As Int32, ByVal X() As Decimal, ByVal Y() As Decimal, ByVal U() As Decimal, ByVal V() As Decimal, ByRef J As Int32, ByRef XT As Decimal, ByRef YT As Decimal)
            J = I - 1

            If J < 1 Then J = J + M
            ' REGULA FALSI INTERPOLATION FOR THE ZERO
            XT = (X(I) * U(J) + X(J) * U(I)) / (U(I) + U(J))
            YT = (Y(I) * V(J) + Y(J) * V(I)) / (V(I) + V(J))
        End Sub

        ''' <summary> Complex root seeking using newton's method 
        ''' This routine uses the complex domain form of
        ''' newton's method for iteratively searching for roots.
        ''' It is assumed that the function and its first partial
        ''' derivatives are available from the functions subroutine
        ''' in the form  F(Z) = u(X,Y) + i V(X,Y).
        ''' The required derivatives are dU/dX and dU/dY.
        ''' The inputs to the subroutine are the initial guess, X0, Y0,
        ''' the convergence criteria, E, and the maximum number of
        ''' iterations to be performed, N.
        ''' The resulting approximation to the root is returned in
        ''' (X,Y), and the number of iterations in K.
        ''' </summary>
        Public Shared Sub CZNEWTON(ByVal X0 As Decimal, ByVal Y0 As Decimal, ByVal E As Decimal, ByVal N As Int32, ByRef X As Decimal, ByRef Y As Decimal, ByRef K As Int32)

            Dim U As Decimal, V As Decimal, U1 As Decimal, V1 As Decimal, U2 As Decimal, V2 As Decimal, A As Decimal
            K = 0

            Do
                K = K + 1
                ' GET U, V AND THE DERIVATIVES
                X = X0
                Y = Y0

                SUBROUTINE_3(X, Y, U, V, U1, V1, U2, V2) ' 44300
                A = U1 * U1 + U2 * U2
                X = X0 + (V * U2 - U * U1) / A
                Y = Y0 - (V * U1 + U * U2) / A

                ' CHECK FOR CONVERGENCE IN EUCLIDEAN SPACE
                If (X0 - X) * (X0 - X) + (Y0 - Y) * (Y0 - Y) <= E * E Then Return
                If K >= N Then Return
                X0 = X
                Y0 = Y
            Loop

        End Sub

        ''' <summary> Parabolic root seeking subroutine 
        ''' This program iteratively seeks the root of a
        ''' function by fitting a parabola to three points
        ''' and calculating the nearest root as described in
        ''' becket and hurt, numerical calculations and algorithms.
        ''' The subroutine inputs are
        '''     X0 - the initial guess
        '''     D - a bound on the error in this guess
        '''     E - the convergence criteria
        '''     N - the maximum number of iterations
        ''' The program returns the value of the root found, X,
        ''' and the number of iterations performed, K.
        ''' it is assumed that the function Y(X) is available
        ''' in the functions subroutine.
        ''' Set up the three evaluation points
        ''' </summary>
        Public Shared Sub MUELLER(ByVal X0 As Decimal, ByVal D As Decimal, ByVal E As Decimal, ByVal N As Int32, ByRef X As Decimal, ByRef K As Int32)

            Dim X1 As Decimal, X2 As Decimal, X3 As Decimal, L1 As Decimal, D1 As Decimal
            Dim E1 As Decimal, E2 As Decimal, E3 As Decimal
            Dim A1 As Decimal, C1 As Decimal, B As Decimal, L As Decimal

            K = 1
            X3 = X0
            X1 = X3 - D
            X2 = X3 + D

            ' CALCULATE MUELLER PARAMETERS
            ' GUARD AGAINST DIVIDE BY ZERO
            If X2 - X1 = 0 Then X2 = X2 * (1.0000001)

            Do

                If X2 - X1 = 0 Then X2 = X2 + 0.0000001
                L1 = (X3 - X2) / (X2 - X1)
                D1 = (X3 - X1) / (X2 - X1)

                If Not (K > 1) Then

                    ' GET VALUES OF FUNCTION
                    SUBROUTINE_4(X1, E1) ' 44300
                    SUBROUTINE_4(X2, E2) ' 44300
                End If

                SUBROUTINE_4(X3, E3) ' 44300
                A1 = L1 * L1 * E1 - D1 * D1 * E2 + (L1 + D1) * E3
                C1 = L1 * (L1 * E1 - D1 * E2 + E3)
                B = A1 * A1 - 4 * D1 * C1 * E3

                ' TEST FOR COMPLEX ROOT, MEANING THE PARABOLA IS INVERTED
                If B < 0 Then B = 0

                ' CHOOSE CLOSEST ROOT
                If A1 < 0 Then A1 = A1 - Sqrt(B)
                If A1 > 0 Then A1 = A1 + Sqrt(B)

                ' GUARD AGAINST A DIVIDE BY ZERO
                If Abs(A1) + Abs(B) = 0 Then A1 = 4 * D1 * E3

                ' CALCULATE RELATIVE DISTANCE OF NEXT GUESS
                ' GUARD AGAINST DIVIDE BY ZERO
                If A1 = 0 Then A1 = 0.0000001
                L = -2 * D1 * E3 / A1
                ' CALCULATE NEXT ESTIMATE
                X = X3 + L * (X3 - X2)

                ' TEST FOR CONVERGENCE
                If Abs(X - X3) < E Then Return

                ' TEST FOR NUMBER OF ITERATIONS
                If K >= N Then Return
                ' OTHERWISE, MAKE ANOTHER PASS
                K = K + 1
                ' SAVE SOME CALCULATIONS:
                X1 = X2
                X2 = X3
                X3 = X
                E1 = E2
                E2 = E3
            Loop

        End Sub

        ''' <summary> Parabolic root seeking subroutine 
        ''' This program iteratively seeks the root of a
        ''' function by fitting a parabola to three points
        ''' and calculating the nearest root as described in
        ''' becket and hurt, numerical calculations and algorithms.
        ''' the subroutine inputs are
        '''     X0,Y0 - the initial guess
        '''     B1,B2 - a bound on the error in this guess
        '''     E - the convergence criteria
        '''     N - the maximum number of iterations
        ''' the program returns the value of the root found, (X,Y),
        ''' and the number of iterations performed, K.
        ''' It is assumed that the function U(X,Y) is available
        ''' in the functions subroutine.
        ''' Set up the three evaluation points
        ''' </summary>
        Public Shared Sub MUELLER2(ByVal X0 As Decimal, ByVal B1 As Decimal, ByVal Y0 As Decimal, ByVal B2 As Decimal, ByVal E As Decimal, ByVal N As Int32, ByRef X As Decimal, ByRef Y As Decimal, ByRef K As Int32)

            Dim W As Decimal, L1 As Decimal, D1 As Decimal, L As Decimal
            Dim X1 As Decimal, X2 As Decimal, X3 As Decimal
            Dim E1 As Decimal, E2 As Decimal, E3 As Decimal
            Dim Y1 As Decimal, Y2 As Decimal, Y3 As Decimal
            'Dim A1 As Decimal, C1 As Decimal, B As Decimal
            K = 1

            Do
                X3 = X0
                X1 = X3 - B1
                X2 = X3 + B1

                ' CALCULATE MUELLER PARAMETERS
                ' GUARD AGAINST DIVIDE BY ZERO
                If X2 - X1 = 0 Then X2 = X2 * (1.0000001#)
                If X2 - X1 = 0 Then X2 = X2 + 0.0000001
                L1 = (X3 - X2) / (X2 - X1)
                D1 = (X3 - X1) / (X2 - X1)
                ' GET VALUES OF FUNCTION
                Y = Y0
                X = X1

                SUBROUTINE_5(X, Y, W) ' 44300
                E1 = W
                X = X2

                SUBROUTINE_5(X, Y, W) ' 44300
                E2 = W
                X = X3

                SUBROUTINE_5(X, Y, W) ' 44300
                E3 = W
                UTILITY_SUBROUTINE(L1, E1, E2, E3, D1, L) '  47111
                ' CALCULATE NEW X ESTIMATE
                B1 = L * (X3 - X2)
                X = X3 + B1

                ' TEST FOR CONVERGENCE
                If Abs(B1) + Abs(B2) < E Then Return
                X0 = X
                ' REPEAT FOR THE Y DIRECTION
                Y3 = Y0
                Y1 = Y3 - B2
                Y2 = Y3 + B2

                ' CALCULATE MUELLER PARAMETERS
                ' GUARD AGAINST A DIVIDE BY ZERO
                If Y2 - Y1 = 0 Then Y2 = Y2 * (1.0000001#)
                If Y2 - Y1 = 0 Then Y2 = Y2 + 0.0000001
                L1 = (Y3 - Y2) / (Y2 - Y1)
                D1 = (Y3 - Y1) / (Y2 - Y1)
                ' GET VALUES OF FUNCTION
                Y = Y1

                SUBROUTINE_5(X, Y, W) ' 44300
                E1 = W
                Y = Y2

                SUBROUTINE_5(X, Y, W) ' 44300
                E2 = W
                Y = Y3

                SUBROUTINE_5(X, Y, W) ' 44300
                E3 = W
                UTILITY_SUBROUTINE(L1, E1, E2, E3, D1, L) ' 47111
                ' CALCULATE NEW Y ESTIMATE
                B2 = L * (Y3 - Y2)
                Y = Y3 + B2

                ' TEST FOR CONVERGENCE
                If Abs(B1) + Abs(B2) < E Then Return

                ' TEST FOR NUMBER OF ITERATIONS
                If K >= N Then Return
                Y0 = Y
                K = K + 1
                ' START ANOTHER PASS
            Loop

        End Sub

        ''' <summary> Utility subroutine  </summary>
        Private Shared Sub UTILITY_SUBROUTINE(ByVal L1 As Decimal, ByVal E1 As Decimal, ByVal E2 As Decimal, ByVal E3 As Decimal, ByVal D1 As Decimal, ByRef L As Decimal)

            Dim B As Decimal, A1 As Decimal, C1 As Decimal
            A1 = L1 * L1 * E1 - D1 * D1 * E2 + (L1 + D1) * E3
            C1 = L1 * (L1 * E1 - D1 * E2 + E3)
            B = A1 * A1 - 4 * D1 * C1 * E3

            ' TEST FOR COMPLEX ROOT, MEANING THE PARABOLA IS INVERTED
            If B < 0 Then B = 0

            ' CHOOSE CLOSEST ROOT
            If A1 < 0 Then A1 = A1 - Sqrt(B)
            If A1 > 0 Then A1 = A1 + Sqrt(B)

            ' GUARD AGAINST A DIVIDE BY ZERO
            If Abs(A1) + Abs(B) = 0 Then A1 = 4 * D1 * E3

            ' CALCULATE RELATIVE DISTANCE OF NEXT GUESS
            ' GUARD AGAINST DIVIDE BY ZERO
            If A1 = 0 Then A1 = 0.0000001
            L = -2 * D1 * E3 / A1
        End Sub

        ''' <summary> Mueller's method for complex roots 
        ''' This program uses the parabolic fitting technique
        ''' associated with Mueller's method, but does it in
        ''' the complex domain.
        ''' The inputs to the subroutine are the initial
        ''' guess, (X0,Y0), the convergence criteria, E,
        ''' and the maximum number of iterations, N.
        ''' Also required are bounds on the initial guess, B1 and B2.
        ''' returned is the new estimate, (X,Y), and the
        ''' number of iterations performed, K.
        ''' It is assumed that the function F(Z) = U(X,Y)+IV(X,Y)
        ''' Is available in the functions subroutine.
        ''' Start calculations
        ''' </summary>
        Public Shared Sub ZMUELLER(ByVal X0 As Decimal, ByVal B1 As Decimal, ByVal Y0 As Decimal, ByVal B2 As Decimal, ByVal E As Decimal, ByVal N As Int32, ByRef X As Decimal, ByRef Y As Decimal, ByRef K As Int32)

            Dim X1 As Decimal, X2 As Decimal, X3 As Decimal
            Dim E1 As Decimal, E2 As Decimal
            Dim L1 As Decimal, L2 As Decimal
            Dim C1 As Decimal, C2 As Decimal
            Dim D As Decimal, D1 As Decimal, D2 As Decimal
            Dim A As Decimal, A1 As Decimal, A2 As Decimal
            Dim Y1 As Decimal, Y2 As Decimal, Y3 As Decimal
            Dim U1 As Decimal, V1 As Decimal, U2 As Decimal, V2 As Decimal, U3 As Decimal, V3 As Decimal, B As Decimal
            K = 1
            X3 = X0
            Y3 = Y0
            X1 = X3 - B1
            Y1 = Y3 - B2
            X2 = X3 + B1
            Y2 = Y3 + B2

            Do
                D = (X2 - X1) * (X2 - X1) + (Y2 - Y1) * (Y2 - Y1)

                ' AVOID DIVIDE BY ZERO
                If D = 0 Then D = 0.0000001
                L1 = (X3 - X2) * (X2 - X1) + (Y3 - Y2) * (Y2 - Y1)
                L1 = L1 / D
                L2 = (X2 - X1) * (Y3 - Y2) - (X3 - X2) * (Y2 - Y1)
                L2 = L2 / D
                D1 = (X3 - X1) * (X2 - X1) + (Y3 - Y1) * (Y2 - Y1)
                D1 = D1 / D
                D2 = (X2 - X1) * (Y3 - Y1) - (X3 - X1) * (Y2 - Y1)
                D2 = D2 / D

                ' GET FUNCTION VALUES
                SUBROUTINE_1_2_6_7(X1, Y1, U1, V1) '44300
                SUBROUTINE_1_2_6_7(X2, Y2, U2, V2) '44300
                SUBROUTINE_1_2_6_7(X3, Y3, U3, V3) '44300
                'Console.WriteLine(X3)
                'Console.WriteLine(U3)
                'Console.WriteLine(Y3)
                'Console.WriteLine(V3)
                ' CALCULATE MUELLER PARAMETERS
                E1 = U1 * (L1 * L1 - L2 * L2) - 2 * V1 * L1 * L2 - U2 * (D1 * D1 - D2 * D2)
                E1 = E1 + 2 * V2 * D1 * D2 + U3 * (L1 + D1) - V3 * (L2 + D2)
                E2 = 2 * L1 * L2 * U1 + V1 * (L1 * L1 - L2 * L2) - 2 * D1 * D2 * U2 - V2 * (D1 * D1 - D2 * D2)
                E2 = E2 + U3 * (L2 + D2) + V3 * (L1 + D1)
                C1 = L1 * L1 * U1 - L1 * L2 * V1 - D1 * L1 * U2 + L1 * D2 * V2 + U3 * L1
                C1 = C1 - U1 * L2 * L2 - V1 * L1 * L2 + U2 * L2 * D2 + V2 * D1 * L2 - V3 * L2
                C2 = U1 * L1 * L2 + V1 * L1 * L1 - U2 * D2 * L1 - V2 * D1 * L1 + V3 * L1
                C2 = C2 + L1 * L2 * U1 - L2 * L2 * V1 - D1 * L2 * U2 + D2 * L2 * V2 + U3 * L2
                B1 = E1 * E1 - E2 * E2 - 4 * (U3 * D1 * C1 - U3 * D2 * C2 - V3 * D2 * C1 - V3 * D1 * C2)
                B2 = 2 * E1 * E2 - 4 * (U3 * D2 * C1 + U3 * D1 * C2 + V3 * D1 * C1 - V3 * D2 * C2)

                ' GUARD AGAINST A DIVIDE BY ZERO
                If B1 = 0 Then B1 = 0.0000001
                A = Atan(B2 / B1)
                A = A / 2
                B = Sqrt(Sqrt(B1 * B1 + B2 * B2))
                B1 = B * Cos(A)
                B2 = B * Sin(A)
                A1 = (E1 + B1) * (E1 + B1) + (E2 + B2) * (E2 + B2)
                A2 = (E1 - B1) * (E1 - B1) + (E2 - B2) * (E2 - B2)

                If A1 > A2 Then
                    A1 = E1 + B1
                    A2 = E2 + B2

                Else
                    A1 = E1 - B1
                    A2 = E2 - B2
                End If

                A = A1 * A1 + A2 * A2
                L1 = A1 * D1 * U3 - A1 * D2 * V3 + A2 * U3 * D2 + A2 * V3 * D1

                ' GUARD AGAINST DIVIDE BY ZERO
                If A = 0 Then A = 0.0000001
                L1 = -2 * L1 / A
                L2 = -D1 * U3 * A2 + D2 * V3 * A2 + A1 * U3 * D2 + A1 * V3 * D1
                L2 = -2 * L2 / A
                ' CALCULATE NEW ESTIMATE
                X = X3 + L1 * (X3 - X2) - L2 * (Y3 - Y2)
                Y = Y3 + L2 * (X3 - X2) + L1 * (Y3 - Y2)

                ' TEST FOR CONVERGENCE
                If Abs(X - X0) + Abs(Y - Y0) < E Then Return

                ' TEST FOR NUMBER OF ITERATIONS
                If K >= N Then Return
                ' CONTINUE
                K = K + 1
                '????????????
                'Console.WriteLine(X)
                'Console.WriteLine(Y)
                X0 = X
                Y0 = Y
                X1 = X2
                Y1 = Y2
                X2 = X3
                Y2 = Y3
                X3 = X
                Y3 = Y
            Loop

        End Sub

        ''' <summary> General root determination subroutine 
        ''' The routine attempts to calculate the several roots of a
        ''' given series or function by repeatedly using the
        ''' Zmueller subroutine and 'oving the roots already found
        ''' by division.
        ''' The input to the subroutine are
        '''     X0,Y0 - the initial guess
        '''     B1,B2 - the bounds on this guess
        '''     E - the convergence criteria
        '''     N - the maximum number of iterations per root
        '''     N2 - the number of roots being sought
        '''     N3 - a flag indicating a function F(Z) (1)
        '''          Or a polynomial (2)
        ''' The program returns the n2 roots found, X(I),Y(I)
        ''' and the last number of iterations used, K.
        ''' If K=0 then N3 was in error
        ''' start calculations
        ''' </summary>
        Public Shared Sub ALLROOT(ByVal X0 As Decimal, ByVal B1 As Decimal, ByVal Y0 As Decimal, ByVal B2 As Decimal, ByVal E As Decimal, ByVal N As Int32, ByVal N2 As Int32, ByVal N3 As Int32, ByRef X() As Decimal, ByRef Y() As Decimal, ByRef K As Int32)

            Dim J1 As Int32, X4 As Decimal, Y4 As Decimal
            Dim M As Int32, A(5) As Decimal
            Dim XT As Decimal, YT As Decimal
            K = 0

            If (N3 < 1) Or (N3 > 2) Then Return

            J1 = 0
            ' SAVE THE INITIAL GUESS
            X4 = X0
            Y4 = Y0

            ' IF N3=2 THEN GET THE SERIES COEFFICIENTS
            If N3 = 2 Then COEFFICIENTS_SUBROUTINE(M, A) ' 47322

            Do

                ' TEST FOR COMPLETION
                If J1 = N2 Then Return
                ' ZMUELLER
                VARIANT_MUELLER(M, B1, B2, J1, A, N, N3, X, Y, X0, Y0, XT, YT, K)  ' 47344
                J1 = J1 + 1
                X(J1) = XT
                Y(J1) = YT
                X0 = X4
                Y0 = Y4
                ' TRY ANOTHER PASS
            Loop

        End Sub

        ''' <summary> Coefficients subroutine </summary>
        Private Shared Sub COEFFICIENTS_SUBROUTINE(ByRef M As Int32, ByRef A() As Decimal)
            M = 5
            A(0) = 0
            A(1) = 24
            A(2) = -50
            A(3) = 35
            A(4) = -10
            A(5) = 1
        End Sub

        ''' <summary> Variant on mueller's method for complex roots
        ''' This program uses the parabolic fitting technique
        ''' associated with Mueller's method, but does it in
        ''' the complex domain.
        ''' The inputs to the subroutine are the initial
        ''' guess, (X0,Y0), the convergence criteria, E,
        ''' and the maximum number of iterations, N.
        ''' Also required are bounds on the initial guess, B1 and B2.
        ''' Returned is the new estimate, (X,Y), and the
        ''' number of iterations performed, K.
        ''' It is assumed that the function F(Z) = U(X,Y)+IV(X,Y)
        ''' is available in the functions subroutine.
        ''' Start calculations
        ''' </summary>
        Private Shared Sub VARIANT_MUELLER(ByVal M As Int32, ByVal B1 As Decimal, ByVal B2 As Decimal, ByVal J1 As Int32, ByVal A() As Decimal, ByVal N As Int32, ByVal N3 As Int32, ByVal X() As Decimal, ByVal Y() As Decimal, ByVal X0 As Decimal, ByVal Y0 As Decimal, ByRef XT As Decimal, ByRef YT As Decimal, ByRef K As Int32)

            Dim X1 As Decimal, X2 As Decimal, X3 As Decimal
            Dim Y1 As Decimal, Y2 As Decimal, Y3 As Decimal
            Dim E1 As Decimal, E2 As Decimal
            Dim L1 As Decimal, L2 As Decimal
            Dim C1 As Decimal, C2 As Decimal
            Dim D As Decimal, D1 As Decimal, D2 As Decimal
            Dim AT As Decimal, A1 As Decimal, A2 As Decimal
            Dim U1 As Decimal, V1 As Decimal, U2 As Decimal, V2 As Decimal, U3 As Decimal, V3 As Decimal
            Dim B As Decimal
            K = 1
            X3 = X0
            Y3 = Y0
            X1 = X3 - B1
            Y1 = Y3 - B2
            X2 = X3 + B1
            Y2 = Y3 + B2

            Do
                D = (X2 - X1) * (X2 - X1) + (Y2 - Y1) * (Y2 - Y1)

                ' AVOID DIVIDE BY ZERO
                If D = 0 Then D = 0.0000001
                L1 = (X3 - X2) * (X2 - X1) + (Y3 - Y2) * (Y2 - Y1)
                L1 = L1 / D
                L2 = (X2 - X1) * (Y3 - Y2) - (X3 - X2) * (Y2 - Y1)
                L2 = L2 / D
                D1 = (X3 - X1) * (X2 - X1) + (Y3 - Y1) * (Y2 - Y1)
                D1 = D1 / D
                D2 = (X2 - X1) * (Y3 - Y1) - (X3 - X1) * (Y2 - Y1)
                D2 = D2 / D
                ' GET FUNCTION VALUES
                SUPERVISOR_SUBROUTINE(M, J1, A, X, Y, N3, X1, Y1, U1, V1)  ' 47431
                SUPERVISOR_SUBROUTINE(M, J1, A, X, Y, N3, X2, Y2, U2, V2)  ' 47431
                SUPERVISOR_SUBROUTINE(M, J1, A, X, Y, N3, X3, Y3, U3, V3)  ' 47431
                ' CALCULATE MUELLER PARAMETERS
                E1 = U1 * (L1 * L1 - L2 * L2) - 2 * V1 * L1 * L2 - U2 * (D1 * D1 - D2 * D2)
                E1 = E1 + 2 * V2 * D1 * D2 + U3 * (L1 + D1) - V3 * (L2 + D2)
                E2 = 2 * L1 * L2 * U1 + V1 * (L1 * L1 - L2 * L2) - 2 * D1 * D2 * U2 - V2 * (D1 * D1 - D2 * D2)
                E2 = E2 + U3 * (L2 + D2) + V3 * (L1 + D1)
                C1 = L1 * L1 * U1 - L1 * L2 * V1 - D1 * L1 * U2 + L1 * D2 * V2 + U3 * L1
                C1 = C1 - U1 * L2 * L2 - V1 * L1 * L2 + U2 * L2 * D2 + V2 * D1 * L2 - V3 * L2
                C2 = U1 * L1 * L2 + V1 * L1 * L1 - U2 * D2 * L1 - V2 * D1 * L1 + V3 * L1
                C2 = C2 + L1 * L2 * U1 - L2 * L2 * V1 - D1 * L2 * U2 + D2 * L2 * V2 + U3 * L2
                B1 = E1 * E1 - E2 * E2 - 4 * (U3 * D1 * C1 - U3 * D2 * C2 - V3 * D2 * C1 - V3 * D1 * C2)
                B2 = 2 * E1 * E2 - 4 * (U3 * D2 * C1 + U3 * D1 * C2 + V3 * D1 * C1 - V3 * D2 * C2)

                ' GUARD AGAINST A DIVIDE BY ZERO
                If B1 = 0 Then B1 = 0.0000001
                AT = Atan(B2 / B1)
                AT = AT / 2
                B = Sqrt(Sqrt(B1 * B1 + B2 * B2))
                B1 = B * Cos(AT)
                B2 = B * Sin(AT)
                A1 = (E1 + B1) * (E1 + B1) + (E2 + B2) * (E2 + B2)
                A2 = (E1 - B1) * (E1 - B1) + (E2 - B2) * (E2 - B2)

                If A1 > A2 Then
                    A1 = E1 + B1
                    A2 = E2 + B2

                Else
                    A1 = E1 - B1
                    A2 = E2 - B2
                End If

                AT = A1 * A1 + A2 * A2
                L1 = A1 * D1 * U3 - A1 * D2 * V3 + A2 * U3 * D2 + A2 * V3 * D1

                ' GUARD AGAINST DIVIDE BY ZERO
                If AT = 0 Then AT = 0.0000001
                L1 = -2 * L1 / AT
                L2 = -D1 * U3 * A2 + D2 * V3 * A2 + A1 * U3 * D2 + A1 * V3 * D1
                L2 = -2 * L2 / AT
                ' CALCULATE NEW ESTIMATE
                XT = X3 + L1 * (X3 - X2) - L2 * (Y3 - Y2)
                YT = Y3 + L2 * (X3 - X2) + L1 * (Y3 - Y2)

                ' TEST FOR CONVERGENCE
                If Abs(XT - X0) + Abs(YT - Y0) < E Then Return

                ' TEST FOR NUMBER OF ITERATIONS
                If K >= N Then Return
                ' CONTINUE
                K = K + 1
                X0 = XT
                Y0 = YT
                X1 = X2
                Y1 = Y2
                X2 = X3
                Y2 = Y3
                X3 = XT
                Y3 = YT
            Loop

        End Sub

        ''' <summary> Supervisor subroutine </summary>
        Private Shared Sub SUPERVISOR_SUBROUTINE(ByVal M As Int32, ByVal J1 As Int32, ByVal A() As Decimal, ByVal X() As Decimal, ByVal Y() As Decimal, ByVal N3 As Decimal, ByVal XT As Decimal, ByVal YT As Decimal, ByRef U As Decimal, ByRef V As Decimal)

            Dim U5 As Decimal, A4 As Decimal, J2 As Int32

            ' DO WE GO TO THE FUNCTIONS SUBROUTINE OR TO THE SERIES SUBROUTINE?
            If N3 = 1 Then SUBROUTINE_1_2_6_7(XT, YT, U, V) ' 44300
            If N3 = 2 Then
                NuGenNumericalUseOfSeries.CMPLXSER(M, XT, YT, A, U, V) ' 44950
            End If

            If J1 = 0 Then Return

            ' DIVIDE BY THE J1 ROOTS ALREADY FOUND
            For J2 = 1 To J1
                U5 = U
                U = (XT - X(J2)) * U + (YT - Y(J2)) * V
                V = (XT - X(J2)) * V - (YT - Y(J2)) * U5
                A4 = (XT - X(J2)) * (XT - X(J2)) + (YT - Y(J2)) * (YT - Y(J2))

                ' GUARD AGAINST DIVIDE BY ZERO
                If A4 = 0 Then A4 = 0.0000001
                V = V / A4
                U = U / A4
            Next J2

            ' RETURN TO ZMUELLER
        End Sub

        ''' <summary> Quadratic root subroutine
        ''' This program calculates the two roots of
        ''' a given second order polynomial using
        ''' the quadratic equation evaluated in a
        ''' manner which minimizes round off error.
        ''' The polynomial is assumed to be of
        ''' the form
        '''      Y = A(2)*X*X +A(1)*X +A(0)
        ''' The two roots are returned as 
        ''' R1 = X1 + I Y1
        ''' R2 = X2 + I Y2
        ''' </summary>
        Public Shared Sub QUADRAT(ByVal A() As Decimal, ByRef X1 As Decimal, ByRef X2 As Decimal, ByRef Y1 As Decimal, ByRef Y2 As Decimal)

            Dim AT As Decimal, B As Decimal, C As Decimal

            ' TEST FOR A(2)=0
            If A(2) <> 0 Then
                AT = A(1) * A(1) - 4 * A(2) * A(0)
                B = Sqrt(Abs(AT))

                ' ESTABLISH SIGN
                If A(1) = 0 Then C = 1
                If A(1) <> 0 Then C = Abs(A(1)) / A(1)

                ' DETERMINE THE FIRST ROOT
                ' CHECK IF ROOT IS COMPLEX
                If AT > 0 Then
                    X1 = -C * (Abs(A(1)) + B) / (2 * A(2))
                    Y1 = 0

                Else
                    X1 = -C * Abs(A(1)) / (2 * A(2))
                    Y1 = -C * B / (2 * A(2))
                End If

                ' CALCULATE THE SECOND ROOT
                C = X1 * X1 + Y1 * Y1

                If C <> 0 Then
                    C = A(0) / (C * A(2))
                    X2 = X1 * C
                    Y2 = -Y1 * C
                    Return

                Else
                    X2 = 1000000.0 * 1000000.0 * 1000000.0
                    Return
                End If

            Else

                ' TEST FOR A(1)=0
                If A(1) <> 0 Then
                    X1 = -A(0) / A(1)
                    Y1 = 0
                    X2 = X1
                    Y2 = Y1
                    Return

                Else
                    X1 = 0
                    X2 = 0
                    Y1 = 0
                    Y2 = 0
                    Return
                End If
            End If

        End Sub

        ''' <summary> Polynomial complex roots subroutine
        ''' uses Lin's method as described in the reference
        ''' a practical guide to computer methods for engineers by shoup.
        ''' The input polynomial coefficients are A(0) through A(M).
        ''' M is the order of the polynomial.
        ''' Initial guesses for A and B are required.
        ''' The results are returned in X1,Y1 and X2,Y2.
        ''' X is the real part, and Y is the imaginary.
        ''' The maxinum number of iterations is N.
        ''' The number of iterations is returned in K.
        ''' The convergence criterion is E.
        ''' If necessary, dimension A(I), B(I) and C(I) in the calling program.
        ''' </summary>
        Public Shared Sub LIN(ByVal M As Int32, ByVal A() As Decimal, ByVal E As Decimal, ByVal N As Int32, ByVal AT As Decimal, ByVal B As Decimal, ByRef X1 As Decimal, ByRef X2 As Decimal, ByRef Y1 As Decimal, ByRef Y2 As Decimal, ByRef K As Int32)

            Dim I As Int32, J As Int32, C(M) As Decimal, CT As Decimal, A1 As Decimal
            Dim B1 As Decimal
            Dim BT(M) As Decimal

            ' NORMALIZE THE A(I) SERIES
            For I = 0 To M
                C(I) = A(I) / A(M)
            Next I

            ' START ITERATION
            ' SET INITIAL GUESS FOR THE QUADRATIC COEFFICIENTS
            BT(0) = 0
            BT(1) = 0

            Dim TestCond2 As Boolean = False

            Do

                Dim TestCond1 As Boolean = False

                Do
                    BT(M - 1) = C(M - 1) - AT
                    BT(M - 2) = C(M - 2) - AT * BT(M - 1) - B

                    For J = 3 To M
                        BT(M - J) = C(M - J) - AT * BT(M + 1 - J) - B * BT(M + 2 - J)
                    Next J

                    ' GUARD AGAINST DIVIDE BY ZERO
                    If BT(2) <> 0 Then
                        TestCond1 = True

                    Else
                        AT = AT + 0.0000001
                        B = B - 0.0000001
                    End If

                Loop Until TestCond1 = True

                A1 = (C(1) - B * BT(3)) / BT(2)
                B1 = C(0) / BT(2)
                K = K + 1

                ' TEST FOR THE NUMBER OF ITERATIONS
                If K >= N Then
                    TestCond2 = True

                Else

                    ' TEST FOR CONVERGENCE
                    If Abs(AT - A1) + Abs(B - B1) < E * E Then
                        TestCond2 = True

                    Else
                        AT = A1
                        B = B1
                    End If
                End If

                ' RETURN FOR NEXT ITERATION
            Loop Until TestCond2 = True

            AT = A1
            B = B1
            CT = AT * AT - 4 * B

            ' IS THERE AN IMAGINARY PART
            If CT > 0 Then
                'Console.WriteLine("cv")
                Y1 = 0
                Y2 = Y1
                X1 = -AT + Sqrt(CT)
                X2 = -AT - Sqrt(CT)

            Else
                Y1 = Sqrt(-CT)
                Y2 = -Y1
                X1 = -AT
                X2 = X1
            End If

            X1 = X1 / 2
            X2 = X2 / 2
            Y1 = Y1 / 2
            Y2 = Y2 / 2
        End Sub

        ''' <summary> Bairstow complex root subroutine
        ''' This subroutine finds the complex conjugate roots
        ''' of a polynomial having real coefficients.
        ''' See computer methods for science and engineering
        ''' by R.L. Lafara.
        ''' order of input series is M >= 4.
        ''' Series coefficients are A(I).
        ''' Initial guesses A and B are required.
        ''' E is the convergence factor.
        ''' Subroutine returns X1,Y1 and X2,Y2.
        ''' N is the maximum number of iterations.
        ''' K is the number of iterations performed.
        ''' If necessary, dimension A(I),B(I), C(I) and D(I)
        ''' in the calling program.
        ''' Use normalized series, C(I)
        ''' </summary>
        Public Shared Sub BAIRSTOW(ByVal M As Int32, ByVal A() As Decimal, ByVal E As Decimal, ByVal N As Int32, ByVal PI As Decimal, ByVal B As Decimal, ByRef X1 As Decimal, ByRef X2 As Decimal, ByRef Y1 As Decimal, ByRef Y2 As Decimal, ByRef K As Int32)

            Dim I As Int32, J As Int32, C(M) As Decimal, BT(M) As Decimal, D(M) As Decimal, AT As Decimal, CT As Decimal
            Dim A1 As Decimal, B1 As Decimal, D2 As Decimal, DT As Decimal

            For I = 0 To M
                C(I) = A(I) / A(M)
            Next I

            ' CHOSE INITIAL ESTIMATES FOR A AND B
            K = 0
            BT(M) = 1

            Dim TestCond As Boolean = False

            Do
                ' START ITERATION SEQUENCE
                BT(M - 1) = C(M - 1) - AT

                For J = 2 To M - 1
                    BT(M - J) = C(M - J) - AT * BT(M + 1 - J) - B * BT(M + 2 - J)
                Next J

                BT(0) = C(0) - B * BT(2)
                D(M - 1) = -1
                D(M - 2) = -BT(M - 1) + AT

                For J = 3 To M - 1
                    D(M - J) = -BT(M + 1 - J) - AT * D(M + 1 - J) - B * D(M + 2 - J)
                Next J

                D(0) = -B * D(2)
                D2 = -BT(2) - B * D(3)
                DT = D(1) * D2 - D(0) * D(2)
                A1 = -BT(1) * D2 + BT(0) * D(2)
                A1 = A1 / DT
                B1 = -D(1) * BT(0) + D(0) * BT(1)
                B1 = B1 / DT
                AT = AT + A1
                B = B + B1
                K = K + 1

                ' TEST FOR THE NUMBER OF ITERATIONS
                If K >= N Then
                    TestCond = True

                Else

                    ' TEST FOR CONVERGENCE
                    If Abs(A1) + Abs(B1) > E * E Then

                    Else
                        TestCond = True
                    End If
                End If

            Loop Until TestCond = True

            ' EXTRACT ROOTS FROM QUADRATIC EQUATION
            CT = AT * AT - 4 * B

            ' TEST TO SEE IF A COMPLEX ROOT
            If CT > 0 Then
                X1 = -AT + Sqrt(CT)
                X2 = -AT - Sqrt(CT)
                Y1 = 0
                Y2 = Y1

            Else
                X1 = -AT
                X2 = X1
                Y1 = Sqrt(-CT)
                Y2 = -Y1
            End If

            X1 = X1 / 2
            X2 = X2 / 2
            Y1 = Y1 / 2
            Y2 = Y2 / 2
        End Sub

    End Class

End Namespace
