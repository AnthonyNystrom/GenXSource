Imports System.Math

Namespace NumericalAnalysis
    ''' <summary> Interpolation </summary>

    Public Class NuGenInterpolation1

        ''' <summary> NTH root subroutine 
        ''' Uses newton-raphson iteration.
        ''' Reference- hart, computer approximations.
        ''' Exponent is 1/N, input parameter is N
        ''' Note that N Must be an integer.
        ''' Argument is Y, desired accuracy is E.
        ''' Returned value is X.
        ''' Initial value is X0.
        ''' </summary>
        Public Shared Sub NTHROOT(ByVal Y As Int32, ByVal N As Int32, ByVal E As Decimal, ByVal X0 As Int32, ByRef X As Decimal, ByRef M As Int32)

            If N <= 1 Then Return
            If Y < 0 Then Return
            If Int(N) <> N Then Return
            If E <= 0 Then Return
            If Y <= 0 Then
                X = 0
                Return
            End If

            M = 0

            Dim X1 As Decimal, X2 As Decimal, I As Int32

            Do
                ' FIND N-1 POWER OF X0
                X2 = 1

                For I = 1 To N - 1
                    X2 = X2 * X0
                Next I

                ' ITERATE
                X1 = ((N - 1) * X0 + Y / X2) / N
                X = X1
                M = M + 1

                If Abs((X0 - X1) / X1) < E Then Return
                X0 = X1
            Loop

        End Sub

        ''' <summary> General root determination subroutine 
        ''' High accuracy iteration involving the square root.
        ''' Routine decomposes exponent into a binary representation
        ''' and then applies newton-raphson iteration.
        ''' Y is the input, N is the exponent, and X the returned root.
        ''' E is the desired accuracy of the iteration.
        ''' M is the desired number of bits in the representation of N.
        ''' Save Y For returning from subroutine.
        ''' </summary>
        Public Shared Sub GENROOT(ByVal M As Int32, ByVal Y As Decimal, ByVal N As Decimal, ByVal E As Decimal, ByRef X As Decimal, ByRef A() As Decimal)

            ' SAVE N
            Dim N3 As Decimal = N

            If Y < 0 Then Return
            If E <= 0 Then Return
            If Y <= 0 Then
                X = 0
                Return
            End If

            Dim X3 As Decimal = Y

            ' IF THE EXPONENT IS NEGATIVE, INVERT PROBLEM
            If N < 0 Then
                N = -N
                Y = 1 / Y
            End If

            'INT - OK, N1 defined in RTDECOMP
            Dim N1 As Int32 = Int(N)
            ' BREAK N DOWN INTO POWERS OF 1/2
            RTDECOMP(M, N, A) ' 45060
            ' FIND MULTIPLIERS
            MULTIPLYING_FACTORS(M, E, Y, N, X, A, X3, N3, N1) ' 45074
            Y = X3
            N = N3
        End Sub

        ''' <summary> Root decomposition subroutine 
        ''' Decompose root n into a binary representation.
        ''' M is the number of binary digits.
        ''' N is the input decimal number.
        ''' N1 is the integer part of N.
        ''' A(I) is the binary representation of the remaining fraction.
        ''' </summary>
        Public Shared Sub RTDECOMP(ByVal M As Int32, ByVal N As Decimal, ByRef A() As Decimal)

            Dim I As Int32

            'INT - OK
            Dim N1 As Int32 = Int(N)
            Dim N2 As Decimal = N - N1

            ' N2 IS BETWEEN 0 AND 1
            ' DECOMPOSE N2 INTO FRACTIONS
            Dim AT As Decimal = 0.5

            For I = 1 To M
                A(I) = 0

                If AT < N2 Then A(I) = 1
                If AT < N2 Then N2 = N2 - AT
                AT = AT / 2
            Next I

        End Sub

        ''' <summary> Find multiplying factors </summary>
        Public Shared Sub MULTIPLYING_FACTORS(ByVal M As Int32, ByVal E As Decimal, ByVal Y As Decimal, ByVal N As Decimal, ByRef X As Decimal, ByRef A() As Decimal, ByVal X3 As Decimal, ByVal N3 As Decimal, ByVal N1 As Int32)

            Dim I As Int32

            For I = 1 To M
                ' FIND SQUARE ROOT OF Y 
                ' REPLACE Y WITH ITS SQUARE ROOT
                Y = SQROOT(E, Y) ' 45109

                If A(I) = 1 Then
                    ' A(I) IS SET EQUAL TO THE LATEST SQUARE ROOT OF Y
                    A(I) = Y

                Else
                    A(I) = 1
                End If

            Next I

            '' ASSEMBLE RESULTS
            '' RETRIEVE Y
            Y = X3
            '' RETRIEVE N
            N = N3

            ' TAKE CARE OF N1 MULTIPLICATIONS
            Dim X2 As Decimal = 1

            '
            If N1 <> 0 Then

                For I = 1 To N1
                    X2 = X2 * Y
                Next I

            End If

            ' TAKE CARE OF ROOT PORTION
            For I = 1 To M
                X2 = X2 * A(I)
            Next I

            ' THE FINAL ROOT IS X
            X = X2
        End Sub

        ''' <summary> Square root subroutine 
        ''' Uses newton-raphson iteration.
        ''' Called Heron's rule.
        ''' Reference- hart, computer approximations.
        ''' Argument is Y, returned value is X1.
        ''' Desired accuracy is E.
        ''' </summary>
        Public Shared Function SQROOT(ByVal E As Decimal, ByVal Y As Decimal) As Decimal

            Dim X0 As Decimal = 1, X1 As Decimal

            Do
                X1 = (X0 + Y / X0) / 2

                If Abs((X1 - X0) / X1) < E Then Return X1
                X0 = X1
            Loop

        End Function

        ''' <summary> Tangent iteration subroutine
        ''' Uses the inverse tangent.
        ''' Based on Newton-Raphson iteration.
        ''' X is the argument, Y is the result.
        ''' The desired accuracy is E.
        ''' Note, the allowable range of the argument is -PI/2 TO PI/2.
        ''' Initial guess is X0=1
        ''' </summary>
        Public Shared Sub TANITER(ByVal X As Decimal, ByVal E As Decimal, ByRef Y As Decimal)

            Dim X0 As Decimal = 1, X1 As Decimal

            ' CHECK FOR DIVIDE BY ZERO
            If X = 0 Then
                Y = 0
                Return
            End If

            ' CHECK FOR OUT OF BOUNDS
            If Abs(X) >= 3.1415926535 / 2 Then Return
            If E <= 0 Then Return

            ' CAN CALL ARCTANGENT SUBROUTINE HERE.
            Do
                X1 = X0 + (X - Math.Atan(X0)) * (1 + X0 * X0)

                ' TEST FOR ACCURACY
                If Abs((X0 - X1) / X1) < E Then
                    Y = X1
                    Return
                End If

                X0 = X1
            Loop

        End Sub

        ''' <summary> Inverse tangent recursion subroutine
        ''' Uses gauss iteration
        ''' Reference- acton, numerical methods that work
        ''' Argument is X, result is Y
        ''' Desired accuracy is E
        ''' Heron's rule (iteration) for the square root is also used
        ''' A0,A1,B0,B1 are dummy variables
        ''' </summary>
        Public Shared Sub ATANITER(ByVal X As Decimal, ByVal E As Decimal, ByRef Y As Decimal, ByRef M As Int32)

            If E < 0 Then Return

            Dim A0 As Decimal, A1 As Decimal, B0 As Decimal, B1 As Decimal
            M = 0
            Y = 1 + X * X

            ' FIND SQUARE ROOT OF 1/Y
            Dim X1 As Decimal = SQROOT(E, Y) ' 45177
            Dim X2 As Decimal = 1 / X1
            A0 = X2
            B0 = 1

            Dim TestCond As Boolean = False

            Do
                A1 = (A0 + B0) / 2
                Y = A1 * B0
                ' FIND SQUARE ROOT
                X1 = SQROOT(E, Y) ' 45177
                B1 = X1
                ' CHECK ACCURACY
                M = M + 1

                If Abs((A1 - B1) / B1) < E Then
                    TestCond = True

                Else
                    A0 = A1
                    B0 = B1
                End If

            Loop Until TestCond = True

            ' COMPUTE FINAL RESULT
            Y = A1 * B1
            ' OBTAIN SQUARE ROOT
            X1 = SQROOT(E, Y) ' 45177
            Y = X * X2 / X1
        End Sub

        ''' <summary> ARCSIN(X) recursion subroutine
        ''' Input is X (-1<X<1)
        ''' Output is Y=ARCSIN(X)
        ''' Convergence criteria is E
        ''' Reference- COMPUTATIONAL ANALYSIS by HENRICI
        ''' </summary>
        Public Shared Sub ARCSINX(ByVal X As Decimal, ByVal E As Decimal, ByRef Y As Decimal, ByRef M As Int32)
            M = 0

            ' GUARD AGAINST FAILURE
            If E <= 0 Then Return
            If X = 0 Then
                Y = 0
                Return
            End If

            ' CHECK RANGE
            If Abs(X) > 1 Then Return

            Dim U0 As Decimal = X * Sqrt(1 - X * X)
            Dim U1 As Decimal = X
            Dim U2 As Decimal

            Do
                U2 = U1 * Sqrt(2 * U1 / (U1 + U0))
                Y = U2
                M = M + 1

                If Abs(U2 - U1) < E Then Return
                U0 = U1
                U1 = U2
            Loop

        End Sub

        ''' <summary> Complete elliptic integral of the first and second kind 
        ''' The input parameter is K, which should
        ''' be between 0 and 1.
        ''' Technique uses Gauss' formula for the
        ''' arithmogeometrical mean.
        ''' Reference- ball, algorithms for RPN calculators.
        ''' E is A measure of the convergence accuracy.
        ''' Depending on E, A(I) and B(I) May have to be dimensioned
        ''' in the calling program.
        ''' The returned values are e1, the elliptic
        ''' integral of the first kind, and E2,
        ''' the integral of the second kind.
        ''' </summary>
        Public Shared Sub CLIPTIC(ByVal K As Decimal, ByVal E As Decimal, ByVal A() As Decimal, ByVal B() As Decimal, ByRef N As Int32, ByRef E1 As Decimal, ByRef E2 As Decimal)
            A(0) = 1 + K
            B(0) = 1 - K
            N = 0

            If K < 0 Then Return
            If K > 1 Then Return
            If E <= 0 Then Return
            If K >= 1 Then
                E2 = 1
                E1 = 10000 * 10000
                E1 = E1 * E1 * E1 * E1
                Return
            End If

            Do
                N = N + 1
                ' GENERATE IMPROVED VALUES
                A(N) = (A(N - 1) + B(N - 1)) / 2
                B(N) = Sqrt(A(N - 1) * B(N - 1))
            Loop While Abs(A(N) - B(N)) > E

            E1 = 1.5707963268 / A(N)
            E2 = 2

            Dim M As Int32 = 1, I As Int32

            For I = 1 To N

                If Not (Abs(A(I) - B(I)) < 0.000001) Then
                    E2 = E2 - M * (A(I) * A(I) - B(I) * B(I))
                End If

                M = M * 2
            Next I

            E2 = E2 * E1 / 2
        End Sub

        ''' <summary> LN(X) Recursion subroutine 
        ''' Input is X (0<X<1)
        ''' Output is Y=LN(X)
        ''' Convergence criteria is E
        ''' Reference-  computational analysis by henrici
        ''' </summary>
        Public Shared Sub LNX(ByVal X As Decimal, ByVal E As Decimal, ByRef Y As Decimal, ByRef M As Int32)
            M = 0

            ' GUARD AGAINST FAILURE
            If E <= 0 Then Return
            If Not (X < 1) Then
                Y = 0
                Return
            End If

            ' CHECK RANGE
            If X <= 0 Then Return

            Dim U0 As Decimal = (X * X - 1 / (X * X)) / 4
            Dim U1 As Decimal = (X - 1 / X) / 2
            Dim U2 As Decimal

            Do
                U2 = U1 * Sqrt(2 * U1 / (U1 + U0))
                Y = U2
                M = M + 1

                If Abs(U2 - U1) < E Then Return
                U0 = U1
                U1 = U2
            Loop

        End Sub

        ''' <summary> Integer order bessel function subroutine 
        ''' Calculates bessel functions of order 0 through 4
        ''' for X>0.
        ''' Miller's method used, see Henrici
        ''' Argument is X
        ''' Number of steps =M
        ''' Returned results are Y(I)
        ''' </summary>
        Public Shared Sub INTBESSL(ByVal X As Decimal, ByVal E As Decimal, ByRef Y() As Decimal, ByRef M As Int32)

            ' TEST FOR RANGE
            If X <= 0 Then Return
            If M <= 0 Then Return
            Y(0) = 1
            Y(1) = 0

            Dim C As Decimal = 0
            Dim N As Int32 = M

            ' UPDATE RESULTS
            Dim I As Int32

            Do

                For I = 4 To 1 Step -1
                    Y(I) = Y(I - 1)
                Next I

                ' APPLY RECURSION RELATION
                Y(0) = 2 * N * Y(1) / X - Y(2)
                N = N - 1

                If N = 0 Then Exit Do

                'INT - OK
                If Not (Int(N / 2) <> N / 2) Then
                    C = C + 2 * Y(0)
                End If

            Loop

            C = C + Y(0)

            ' SCALE THE RESULTS
            For I = 0 To 4
                Y(I) = Y(I) / C
            Next I

        End Sub

        ''' <summary> Legendre series coefficient evaluation subroutine 
        ''' By means of recursion relation
        ''' the order of the polynomial is N
        ''' The coefficients are returned in A(I)
        ''' Dimension A(I) and B(I,J) in the calling program
        ''' Establish P0 and P1 coefficients
        ''' </summary>
        Public Shared Sub LEGNDRE(ByVal N As Int32, ByVal B(,) As Decimal, ByRef A() As Decimal)

            Dim I As Int32, J As Int32
            B(0, 0) = 1
            B(1, 0) = 0
            B(1, 1) = 1

            ' RETURN IF ORDER IS LESS THAN TWO
            If N < 2 Then Return

            For I = 2 To N
                B(I, 0) = -(I - 1) * B(I - 2, 0) / I

                For J = 1 To I
                    ' BASIC RECURSION RELATION
                    B(I, J) = (I + I - 1) * B(I - 1, J - 1) - (I - 1) * B(I - 2, J)
                    B(I, J) = B(I, J) / I
                Next J
            Next I

            For I = 0 To N
                A(I) = B(N, I)
            Next I

        End Sub

        ''' <summary> Laguerre polynomial coefficient evaluation subroutine
        ''' By means of recursion relation
        ''' the order of the polynomial is N
        ''' The coefficients are returned in A(I)
        ''' Dimension A(I) and B(I,J) in the calling program
        ''' Establish L0 and L1 coefficients
        ''' </summary>
        Public Shared Sub LAGUERR(ByVal N As Int32, ByVal B(,) As Decimal, ByRef A() As Decimal)

            Dim I As Int32, J As Int32
            B(0, 0) = 1
            B(1, 0) = 1
            B(1, 1) = -1

            ' RETURN IF ORDER IS LESS THAN TWO
            If N < 2 Then Return

            For I = 2 To N
                B(I, 0) = (2 * I - 1) * B(I - 1, 0) - (I - 1) * (I - 1) * B(I - 2, 0)

                For J = 1 To I
                    ' BASIC RECURSION RELATION
                    B(I, J) = (2 * I - 1) * B(I - 1, J) - B(I - 1, J - 1) - (I - 1) * (I - 1) * B(I - 2, J)
                Next J
            Next I

            For I = 0 To N
                A(I) = B(N, I)
            Next I

        End Sub

        ''' <summary> Hermite polynomial coefficient evaluation subroutine
        ''' BY means of recursion relation
        ''' the order of the polynomial is N
        ''' The coefficients are returned in A(I)
        ''' Dimension A(I) and B(I,J) in the calling program
        ''' Establish H0 and H1 coefficients
        ''' </summary>
        Public Shared Sub HERMITE(ByVal N As Int32, ByVal B(,) As Decimal, ByRef A() As Decimal)

            Dim I As Int32, J As Int32
            B(0, 0) = 1
            B(1, 0) = 0
            B(1, 1) = 2

            ' RETURN IF ORDER IS LESS THAN TWO
            If N < 2 Then Return

            For I = 2 To N
                B(I, 0) = -2 * (I - 1) * B(I - 2, 0)

                For J = 1 To I
                    ' BASIC RECURSION RELATION
                    B(I, J) = 2 * B(I - 1, J - 1) - 2 * (I - 1) * B(I - 2, J)
                Next J
            Next I

            For I = 0 To N
                A(I) = B(N, I)
            Next I

        End Sub

    End Class

End Namespace
