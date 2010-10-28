Imports System.Math

Namespace NumericalAnalysis

    ''' <summary> Numerical Use of Series </summary>
    Public Class NuGenNumericalUseOfSeries

        ''' <summary> Bessel function series subroutine
        ''' The order is N, the argument X.
        ''' The End SubED value is in Y.
        ''' The number of terms used is End SubED in M.
        ''' E is the convergence criterion
        ''' </summary>
        Public Shared Sub BESSLSER(ByVal N As Int32, ByVal X As Decimal, ByVal E As Decimal, ByRef Y As Decimal, ByRef M As Int32)

            Dim I As Int32, A As Decimal = 1

            If N > 1 Then

                ' CALCULATE N!
                For I = 1 To N
                    A = A * I
                Next I

            End If

            A = 1 / A

            If N <> 0 Then

                ' CALCULATE MULTIPLYING TERM
                For I = 1 To N
                    A = A * X / 2
                Next I

            End If

            Dim B0 As Decimal = 1
            Dim B2 As Decimal = 1
            Dim B1 As Decimal
            M = 0

            Do
                ' ASSEMBLE SERIES SUM
                M = M + 1
                B1 = -(X * X * B0) / (M * (M + N) * 4)
                B2 = B2 + B1
                B0 = B1
                ' TEST FOR CONVERGENCE
            Loop While Abs(B1) > E

            ' FORM FINAL ANSWER
            Y = A * B2
        End Sub

        ''' <summary> Bessel function series coefficient evaluation subroutine
        ''' M+1 is the number of coefficients desired.
        ''' N is the order of the bessel function.
        ''' The coefficients are End SubED IN A(I).
        ''' Dimension A(I) and B(I) in the calling program.
        ''' A1,B1 and B(I) are dummy variables.
        ''' </summary>
        Public Shared Sub BESSEL(ByVal N As Decimal, ByVal M As Int32, ByVal A() As Decimal, ByVal B() As Decimal)

            Dim I As Int32
            Dim A1 As Decimal = 1
            Dim B1 As Decimal = 1

            If N <> 0 Then

                For I = 1 To N
                    B(I - 1) = 0
                    B1 = B1 * I
                    A1 = A1 / 2
                Next I

            End If

            B1 = A1 / B1
            A1 = 1

            For I = 0 To M Step 2
                A(I) = A1 * B1
                A(I + 1) = 0
                A1 = -A1 / ((I + 2) * (N + N + I + 2))
            Next I

            A1 = A1 / 2

            For I = 0 To M
                B(I + N) = A(I)
            Next I

            For I = 0 To N + M
                A(I) = B(I)
            Next I

        End Sub

        ''' <summary> Bessel function asymptotic series subroutine
        ''' This program calculates the zeroth and first order bessel
        ''' functions using an asymptotic series expansion.
        ''' The required input are X and a convergence factor E.
        ''' End SubED are the two bessel functions, J0(X) and J1(X)
        ''' Reference-  algorithms for RPN calculators
        '''   by BALL, J.A.,  WILEY AND SONS,
        ''' CAlculate P and Q polynomials
        ''' P0(X)=M1  P1(X)=M2  Q0(X)=N1  Q1(X)=N2
        ''' </summary>
        Public Shared Sub BESSEL01(ByVal E3 As Decimal, ByVal X As Int32, ByRef J0 As Decimal, ByRef J1 As Decimal, ByRef N As Int32, ByRef E As Decimal)

            Dim A = 1
            Dim A1 = 1
            Dim A2 = 1
            Dim B = 1
            Dim C = 1
            Dim E1 = 1000000.0!
            Dim M = -1
            Dim X1 = 1 / (8 * X)
            X1 = X1 * X1

            Dim M1 = 1
            Dim M2 = 1
            Dim N1 = -1 / (8 * X)
            Dim N2 = -3 * N1
            N = 0

            Dim E2 As Decimal
            Dim E4 As Decimal
            Dim TestCond As Boolean = False

            Do
                M = M + 2
                A = A * M * M
                M = M + 2
                A = A * M * M
                C = C * X1
                A1 = A1 * A2
                A2 = A2 + 1
                A1 = A1 * A2
                A2 = A2 + 1
                E2 = A * C / A1
                E4 = 1 + (M + 2) / M + (M + 2) * (M + 2) / (A2 * 8 * X) + (M + 2) * (M + 4) / (A2 * 8 * X)
                E4 = E4 * E2

                ' TEST FOR DIVERGENCE
                If Abs(E4) > E1 Then
                    TestCond = True

                Else
                    E1 = Abs(E2)
                    M1 = M1 - E2
                    M2 = M2 + E2 * (M + 2) / M
                    N1 = N1 + E2 * (M + 2) * (M + 2) / (A2 * 8 * X)
                    N2 = N2 - E2 * (M + 2) * (M + 4) / (A2 * 8 * X)
                    N = N + 1

                    ' TEST FOR CONVERGENCE CRITERION
                    If E1 < E3 Then TestCond = True
                End If

            Loop Until TestCond = True

            A = 3.1415926536
            E = E2
            B = Sqrt(2 / (A * X))
            J0 = B * (M1 * Cos(X - A / 4) - N1 * Sin(X - A / 4))
            J1 = B * (M2 * Cos(X - 3 * A / 4) - N2 * Sin(X - 3 * A / 4))
        End Sub

        ''' <summary> Series approximation subroutine for LN(X)
        ''' Accuracy better than 6 places for X>=3.
        ''' Accuracy better than 12 places for X>10.
        ''' Advantage is that very large values of the argument can be used
        ''' without fear of overflow.
        ''' Reference-  CRC math tables.
        ''' X is the input, Y is the output
        ''' </summary>
        Public Shared Sub LNX(ByVal X As Decimal, ByRef Y As Decimal)

            Dim X1 As Decimal = 1 / (X * X)
            Y = (X + 0.5) * Log(X) - X * (1 - X1 / 12 + X1 * X1 / 360 - X1 * X1 * X1 / 1260 + X1 * X1 * X1 * X1 / 1680)
            Y = Y + 0.918938533205
        End Sub

        ''' <summary> CHI-SQUARE function subroutine
        ''' This program takes a given degree of freedom, M
        ''' and value, X, and calculates the chi-square
        ''' density distribution function value, Y.
        ''' Reference- texas instruments SR-51 owners manual (1974).
        ''' Subroutine LN(X!) is also called.
        ''' </summary>
        Public Shared Sub CHI_SQR(ByVal M As Int32, ByVal X As Decimal, ByRef Y As Decimal)

            'PERFORM CALCULATION
            Dim M1 As Decimal = M / 2 - 1
            LNX(M1, Y) '44580

            Dim C As Decimal = -X / 2 + (M / 2 - 1) * Log(X) - (M / 2) * Log(2) - Y
            Y = Exp(C)
        End Sub

        ''' <summary> CHI-SQUARE cummulative distribution 
        ''' The program is fairly accurate and calls upon the
        ''' chi-square probability density function subroutine (chi-sqr).
        ''' The input parameter is M, the number of degrees of freedom.
        ''' Also required is the ordinate value. The program End SubS Y,
        ''' The cummulative distribution integral from 0 to X.
        ''' Reference- Hewlett-Packard statistics programs, 1974.
        ''' This program also requires an accuracy parameter, E, to
        ''' determine the level of summation.
        ''' </summary>
        Public Shared Sub CHISQ(ByVal M As Int32, ByVal X As Decimal, ByVal E As Decimal, ByRef Y As Decimal)

            Dim Y1 As Decimal = 1
            Dim X2 As Decimal = X
            Dim M2 As Int32 = M + 2
            X2 = X2 / M2

            Dim TestCond As Boolean = False

            Do
                Y1 = Y1 + X2

                If X2 < E Then
                    TestCond = True

                Else
                    M2 = M2 + 2
                    'THIS FORM IS USED TO AVOID OVERFLOW
                    X2 = X2 * X / M2
                End If

                'LOOP TO CONTINUE SUM
            Loop Until TestCond = True

            'OBTAIN Y, THE PROBABILITY DENSITY FUNCTION
            CHI_SQR(M, X, Y) '44600
            Y = Y1 * Y * 2 * X / M
        End Sub

        ''' <summary> Asymptotic series expansion of the integral of 2 exp(-x*x)/sqrt(pi) - the normalized error function
        ''' THis program detemines the values of the above
        ''' integrand using an asymptotic series which is
        ''' evaluated to the level of maximum accuracy.
        ''' The integral is from 0 to X.
        ''' The input parameter is X>0. The results are
        ''' End SubED in Y and Y1, with the error measure in E.
        ''' The program also End SubS the number of terms used.
        ''' NOTE- the error is roughly equal to
        ''' first term neglected in the series summation.
        ''' Reference-  A SHORT TABLE OF INTEGRALS by B.O. PEIRCE
        '''   GINN AND COMPANY   1957
        ''' </summary>
        Public Shared Sub ASYMERF(ByVal X As Int32, ByRef Y As Decimal, ByRef E As Decimal, ByRef N As Int32)
            N = 1
            Y = 1

            Dim C2 As Decimal = 1 / (2 * X * X), C1 As Decimal

            Do
                Y = Y - C2
                N = N + 2
                C1 = C2
                C2 = -C1 * N / (2 * X * X)
                'TEST FOR DIVERGENCE
                'THE BREAK POINT IS ROUGHLY N=X*X
            Loop Until Abs(C2) > Abs(C1)

            ' CONTINUE SUMMATION
            N = (N + 1) / 2
            E = Exp(-X * X) / (X * 1.77245385090552)

            Dim Y1 As Decimal = Y * E
            Y = 1 - Y1
            E = E * C2
        End Sub

        ''' <summary> Chebychev series coefficient evaluation subroutine
        '''  The order of the polynomial is N.
        '''  The coefficients are End SubED in the
        '''  array B(I,J). I is the degree of the polynomial,
        '''  J is the coefficient order.
        '''  Dimension B(I,J) in the calling program.
        '''  Establish T0 and T1 coefficients
        ''' </summary>
        Public Shared Sub CHEBYSER(ByVal N As Int32, ByRef B(,) As Decimal)

            Dim I As Int32, J As Int32
            B(0, 0) = 1
            B(1, 0) = 0
            B(1, 1) = 1

            ' Return IF ORDER IS LESS THAN TWO
            If N < 2 Then Return

            For I = 2 To N
                For J = 1 To I
                    ' BASIC RECURSION RELATION
                    B(I, J) = B(I - 1, J - 1) + B(I - 1, J - 1) - B(I - 2, J)
                Next J

                B(I, 0) = -B(I - 2, 0)
            Next I

        End Sub

        ''' <summary> Chebyshev economization subroutine 
        ''' Routine takes the input polynomial coefficients, C(I),
        ''' and End SubS the chebyschev series coefficients, A(I).
        ''' The degree of the series passed to the routine is M.
        ''' The degree of the series End SubED is M1.
        ''' The maximum range of X is X0- X0 is used for scaling.
        ''' The chebyschev series coefficient (B(I,J) subroutine is
        ''' called- I is the order of the chebyschev polynomial.
        ''' Note that the input series coefficients are nulled during the process,
        ''' and then set equal to the economized series coefficients.
        ''' The chebyshev series is valid only over the range ABS(X/X0)<=1.
        ''' Dimension A(I),B(I,J),C(I) in the calling program.
        ''' Start by scaling the input coefficients according to C(I)
        ''' </summary>
        Public Shared Sub CHEBECON(ByVal M As Int32, ByVal M1 As Int32, ByVal X0 As Int32, ByRef A() As Decimal, ByRef B(,) As Decimal, ByRef C() As Decimal)

            Dim I As Int32, N As Int32, L As Int32, J As Int32
            Dim B1 As Decimal = X0

            For I = 1 To M
                C(I) = C(I) * B1
                B1 = B1 * X0
            Next I

            ' GET CHEBYSCHEV SERIES COEFFICIENTS.
            ' POLYNOMIAL SERIES IS REDUCED FROM THE HIGHEST ORDER DOWN
            For N = M To 0 Step -1
                CHEBYSER(N, B) '44725
                A(N) = C(N) / B(N, N)

                For L = 0 To N
                    ' CHEBYSCHEV SERIES OF ORDER L IS SUBTRACTED OUT OF THE POLYNOMIAL
                    C(L) = C(L) - A(N) * B(N, L)
                Next L
            Next N

            ' PERFORM TRUNCATION
            For I = 0 To M1
                For J = 0 To I
                    C(J) = C(J) + A(I) * B(I, J)
                Next J
            Next I

            ' CONVERT BACK TO THE INTERVAL X0
            B1 = 1 / X0

            For I = 1 To M1
                C(I) = C(I) * B1
                B1 = B1 / X0
            Next I

        End Sub

        ''' <summary> Series reversion subroutine 
        ''' This program takes a polynomial, Y=A(0) + A(1)X + ..
        ''' and End SubS a polynomial X = B(0) + B(1)Y + ...
        ''' Reference  CRC STANDARD MATHEMATICAL TABLES
        '''              24TH EDITION
        ''' The input series coefficients are A(0),A(1), ETC.
        ''' A(1) must be nonzero.
        ''' The output series coefficients are B(0),B(1),....,B(7).
        ''' The degree of reversion is limited to seven.
        ''' A1,A2,.... are dummy variables.
        ''' </summary>
        Public Shared Sub REVERSE(ByVal N As Int32, ByVal A() As Decimal, ByRef B() As Decimal)

            Dim I As Int32
            Dim A1 As Decimal = A(1)
            Dim A2 As Decimal = A(2)
            B(1) = 1 / A1

            Dim AT As Decimal = 1 / A1
            Dim BT As Decimal = AT * AT
            AT = AT * BT
            B(2) = -A2 / AT

            Dim A3 As Decimal = A(3)
            AT = AT * BT
            B(3) = AT * (2 * A2 * A2 - A1 * A3)

            Dim A4 As Decimal = A(4)
            AT = AT * BT
            B(4) = AT * (5 * A1 * A2 * A3 - A1 * A1 * A4 - 5 * A2 * A2 * A2)

            Dim A5 As Decimal = A(5)
            AT = AT * BT
            B(5) = 6 * A1 * A1 * A2 * A4 + 3 * A1 * A1 * A3 * A3 + 14 * A2 * A2 * A2 * A2
            B(5) = B(5) - A1 * A1 * A1 * A5 - 21 * A1 * A2 * A2 * A3
            B(5) = AT * B(5)

            Dim A6 As Decimal = A(6)
            AT = AT * BT
            B(6) = 7 * A1 * A1 * A1 * A2 * A5 + 7 * A1 * A1 * A1 * A3 * A4 + 84 * A1 * A2 * A2 * A2 * A3
            B(6) = B(6) - A1 * A1 * A1 * A1 * A6 - 28 * A1 * A1 * A2 * A2 * A4
            B(6) = B(6) - 28 * A1 * A1 * A2 * A3 * A3 - 42 * A2 * A2 * A2 * A2 * A2
            B(6) = AT * B(6)

            Dim A7 As Decimal = A(7)
            AT = AT * BT
            B(7) = 8 * A1 * A1 * A1 * A1 * A2 * A6 + 8 * A1 * A1 * A1 * A1 * A3 * A5
            B(7) = B(7) + 4 * A1 * A1 * A1 * A1 * A4 * A4 + 120 * A1 * A1 * A2 * A2 * A2 * A4
            B(7) = B(7) + 180 * A1 * A1 * A2 * A2 * A3 * A3 + 132 * A2 * A2 * A2 * A2 * A2 * A2
            B(7) = B(7) - A1 * A1 * A1 * A1 * A1 * A7 - 36 * A1 * A1 * A1 * A2 * A2 * A5
            B(7) = B(7) - 72 * A1 * A1 * A1 * A2 * A3 * A4 - 12 * A1 * A1 * A1 * A3 * A3 * A3
            B(7) = B(7) - 330 * A1 * A2 * A2 * A2 * A2 * A3
            B(7) = AT * B(7)
            B(0) = 0
            AT = A(0)

            For I = 1 To 7
                B(0) = B(0) - B(I) * AT
                AT = AT * A(0)
            Next I

        End Sub

        ''' <summary> Reciprocal power series subroutine 
        ''' Reference- COMPUTATIONAL ANALYSIS BY HENRICI.
        ''' The input series coefficients are A(I).
        ''' The output series coefficients are B(I).
        ''' The degree of the input polynomial is N..
        ''' The degree of the inverted polynomial is M.
        ''' Dimension A(I) and B(I) in the calling program
        ''' The program will take care of the normalization using L
        ''' </summary>
        Public Shared Sub RECIPRO(ByVal N As Int32, ByVal M As Int32, ByVal A() As Decimal, ByRef B() As Decimal)

            Dim I As Int32, J As Int32
            Dim L As Decimal = A(0)

            For I = 0 To N
                A(I) = A(I) / L
                B(I) = 0
            Next I

            ' CLEAR ARRAYS
            For I = N + 1 To M
                A(I) = 0
                B(I) = 0
            Next I

            ' CALCULATE THE B(I) COEFFICIENTS
            B(0) = 1

            For I = 1 To M
                J = 1

                Do
                    B(I) = B(I) - A(J) * B(I - J)
                    J = J + 1
                Loop While J <= I

            Next I

            ' UN-NORMALIZE THE A(I) AND B(I)
            For I = 0 To M
                A(I) = A(I) * L
                B(I) = B(I) / L
            Next I

        End Sub

        ''' <summary> Horner's shifting rule subroutine 
        ''' This subroutine takes a given quartic
        ''' polynomial and converts it to a taylor expansion.
        ''' The input series coefficients are A(I).
        ''' The expansion point is X0.
        ''' The shifted coefficients are End SubED in B(I).
        ''' C(4,5) must be dimensioned in the calling program.
        ''' </summary>
        Public Shared Sub HORNER(ByVal X0 As Decimal, ByVal A() As Decimal, ByRef B() As Decimal, ByVal C(,) As Decimal)

            Dim J As Int32, I As Int32

            For J = 0 To 4
                C(J, 0) = A(4 - J)
            Next J

            For I = 0 To 4
                C(0, I + 1) = C(0, I)
                J = 1

                Do While J <= 4 - I
                    C(J, I + 1) = X0 * C(J - 1, I + 1) + C(J, I)
                    J = J + 1
                Loop

            Next I

            For I = 0 To 4
                B(4 - I) = C(I, 4 - I + 1)
            Next I

        End Sub

        ''' <summary> Inverse normal distribution subroutine
        ''' This program calculates an approximation
        ''' to the integral of the normal distribution
        ''' function from X to infinity (the tail).
        ''' A rational polynomial is used.
        ''' The input is Y, with the result End SubED in X.
        ''' The accuracy is better than 0.0005 in the range 0<Y<=.5
        ''' Reference- ABRAMOWITZ AND STEGUN
        ''' Define coefficients
        ''' </summary>
        Public Shared Sub INVNORM(ByRef X As Decimal, ByVal Y As Decimal)

            Dim C0 As Decimal = 2.51552
            Dim C1 As Decimal = 0.802853
            Dim C2 As Decimal = 0.010328
            Dim D1 As Decimal = 1.43279
            Dim D2 As Decimal = 0.189269
            Dim D3 As Decimal = 0.001308

            If Y = 0 Then
                X = 10000000000000.0#
                Return
            End If

            Dim Z As Decimal = Sqrt(-Log(Y * Y))
            X = 1 + D1 * Z + D2 * Z * Z + D3 * Z * Z * Z
            X = (C0 + C1 * Z + C2 * Z * Z) / X
            X = Z - X
        End Sub

        ''' <summary> Sine product series subroutine 
        ''' This program calculates an approximation to SIN(X)
        ''' Using repeated products.
        ''' The inputs to the program are the argument, X
        ''' and an error factor, E.
        ''' The approximation is returned in Y.
        ''' </summary>
        Public Shared Sub SINEPROD(ByVal X As Decimal, ByVal E As Decimal, ByRef Y As Decimal, ByRef K As Int32)

            Dim M As Decimal
            Y = X
            K = 1

            Dim L As Decimal = 3.14159265358979
            L = X / L
            L = L * L

            Do
                M = L / (K * K)
                Y = Y * (1 - M)

                If M < E Then Return
                K = K + 1
            Loop

        End Sub

        ''' <summary> Complex series evaluation subroutine 
        ''' The series coefficients are A(I), assumed real.
        ''' The order of the polynomial is M.
        ''' The subroutine uses repeated calls to the
        ''' NTH power (Z^N) complex number subroutine.
        ''' Inputs to the subroutine are X,Y,M, and the A(I).
        ''' Outputs are Z1(real) and Z2(imaginary).
        ''' </summary>
        Public Shared Sub CMPLXSER(ByVal M As Int32, ByVal X As Decimal, ByVal Y As Decimal, ByVal A() As Decimal, ByRef Z1 As Decimal, ByRef Z2 As Decimal)

            Dim N As Int32
            Z1 = A(0)
            Z2 = 0

            ' STORE X AND Y
            Dim A1 As Decimal = X
            Dim A2 As Decimal = Y

            For N = 1 To M
                ' RECALL ORIGINAL X AND Y
                X = A1
                Y = A2
                ' GO TO Z^N SUBROUTINE
                NuGenSystemsOfCoordinates.ZRECTPOW(N, X, Y) ' 41200
                ' FORM PARTIAL SUM
                Z1 = Z1 + A(N) * X
                Z2 = Z2 + A(N) * Y
            Next N

            ' RESTORE X AND Y
            X = A1
            Y = A2
        End Sub

    End Class

End Namespace