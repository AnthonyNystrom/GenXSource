Imports System.Math

Namespace NumericalAnalysis
    ''' <summary> Equation Roots </summary>

    Public Class NuGenEquationRoots1

        ''' <summary> Function subroutines </summary>
        Public Shared Sub SUBROUTINE_2(ByVal X As Decimal, ByRef Y As Decimal)
            Y = (X - 2) * (X + 1) * (X + 10)
        End Sub

        Public Shared Sub SUBROUTINE_3(ByVal X As Decimal, ByRef Y As Decimal, ByRef Y1 As Decimal)
            Y1 = 5 + 20 * X + 30 * X * X + 20 * X * X * X + 5 * X * X * X * X
            Y = 1 + 5 * X + 10 * X * X + 10 * X * X * X + 5 * X * X * X * X + X * X * X * X * X
        End Sub

        Public Shared Sub SUBROUTINE_4_5_6(ByVal X As Decimal, ByRef Y As Decimal)
            Y = 1 + 5 * X + 10 * X * X + 10 * X * X * X + 5 * X * X * X * X + X * X * X * X * X
        End Sub

        Public Shared Sub SUBROUTINE_7(ByVal X As Decimal, ByRef Y As Decimal, ByRef C As Decimal)
            Y = X - 2 * Sin(X)
            C = 1 - 2 * Cos(X)
            C = -1 / C
        End Sub

        Public Shared Sub SUBROUTINE_9(ByVal X As Decimal, ByRef Y As Decimal, ByRef Y1 As Decimal)
            Y = Sin(X / 2)
            Y1 = 0.5 * Cos(X / 2)
        End Sub

        ''' <summary> Bisection method subroutine 
        ''' This program iteratively seeks the zero
        ''' of a function using the method of interval
        ''' halving until the interval is less than
        ''' E in width.
        ''' It is assumed that the function Y=Y(X)
        ''' is available from the function subroutine
        ''' located at 44300.
        ''' This subroutine requires as input the initial
        ''' range values (X0 and X1), as well as the
        ''' convergence criterion, e.
        ''' The zero must be within the range specified
        ''' or an erroneous value will be returned in X.
        ''' this subroutine returns the estimate of the root
        ''' In X, and the corresponding y value.
        ''' Also returned is the number of steps (M).
        ''' </summary>
        Public Shared Sub BISECT(ByVal X0 As Decimal, ByVal X1 As Decimal, ByVal E As Decimal, ByRef X As Decimal, ByRef Y As Decimal, ByRef M As Int32)
            M = 0

            Dim Y0 As Decimal

            Do
                X = X0

                SUBROUTINE_2(X, Y)
                Y0 = Y
                X = X1

                SUBROUTINE_2(X, Y)
                X = (X0 + X1) / 2

                SUBROUTINE_2(X, Y)
                M = M + 1

                If Y * Y0 = 0 Then Return
                If Y * Y0 < 0 Then X1 = X
                If Y * Y0 > 0 Then X0 = X
            Loop While Abs(X1 - X0) > E

        End Sub

        ''' <summary> Newton's method subroutine 
        ''' This program calculates the zeros of a
        ''' function by newton's method.
        ''' The routine requires an initial guess, X0,
        ''' and a convergence factor, E.
        ''' Also required is a limit on the number
        ''' of iterations, M. The number used is
        ''' returned in N.
        ''' It is assumed that the function and its
        ''' derivative are in the subroutine at 44300.
        ''' </summary>
        Public Shared Sub ZNEWTON(ByVal X0 As Decimal, ByVal E As Decimal, ByVal M As Int32, ByRef X As Decimal, ByRef N As Int32)
            N = 0

            Dim Y As Decimal, Y1 As Decimal

            ' GET Y AND Y1
            Do
                X = X0

                SUBROUTINE_3(X, Y, Y1)
                ' UPDATE ESTIMATE
                X0 = X0 - Y / Y1
                N = N + 1

                If N >= M Then Return
            Loop While Abs(Y / Y1) > E

            X = X0
        End Sub

        ''' <summary> Secant method subroutine 
        ''' This subroutine calculates the zeroes of a
        ''' function using the secant method.
        ''' Two initial guesses are required, X0 and X1.
        ''' the convergence criterion is E.
        ''' The maximum number of iterations is M.
        ''' The number of iterations performed is
        ''' returned in N.
        ''' The result is returned in X.
        ''' The function = Y(X)
        ''' </summary>
        Public Shared Sub SECANT(ByVal X0 As Decimal, ByVal X1 As Decimal, ByVal E As Decimal, ByVal M As Int32, ByRef X As Decimal, ByRef N As Int32)
            N = 0

            Dim Y0 As Decimal, Y As Decimal, Y1 As Decimal

            ' START ITERATION
            Do
                X = X0

                SUBROUTINE_4_5_6(X, Y)
                Y0 = Y
                X = X1

                ' GET NEXT POINT
                SUBROUTINE_4_5_6(X, Y)
                Y1 = Y

                ' CALCULATE NEW ESTIMATE
                ' IF Y1=Y0 THEN THERE WILL BE AN OVERFLOW
                ' GUARD AGAINST THIS ARTIFICIALLY
                If Y1 = Y0 Then Y1 = Y1 + 0.001
                X = (X0 * Y1 - X1 * Y0) / (Y1 - Y0)
                N = N + 1

                ' TEST FOR CONVERGENCE
                If N >= M Then Return
                If Abs(X1 - X0) < E Then Return
                ' UPDATE POSITIONS
                X0 = X1
                X1 = X
            Loop

        End Sub

        ''' <summary> Modified false position subroutine 
        ''' Subroutine uses hamming's modification to
        ''' speed convergence.
        ''' The function = Y(X)
        ''' The two intitial guesses are X0 and X1.
        ''' these two guesses must bracket the zero.
        ''' the convergence criterion is E.
        ''' The maximum number of guesses is M.
        ''' The result is returned in X.
        ''' The number of iterations is returned in N.
        ''' </summary>
        Public Shared Sub REGULA(ByVal X0 As Decimal, ByVal X1 As Decimal, ByVal E As Decimal, ByVal M As Int32, ByRef X As Decimal, ByRef N As Int32)
            N = 0

            Dim Y0 As Decimal, Y As Decimal, Y1 As Decimal

            ' ME X0<X1
            If X0 >= X1 Then
                X = X0
                X0 = X1
                X1 = X
            End If

            X = X0

            ' GET Y0 AND Y1
            SUBROUTINE_4_5_6(X, Y)
            Y0 = Y
            X = X1

            ' INITIAL GUESSES FOR A AND B ARE REQUIRED
            SUBROUTINE_4_5_6(X, Y)
            Y1 = Y

            ' CALCULATE A NEW ESTIMATE, X
            Do

                Dim TestCond As Boolean = False

                Do
                    X = (X0 * Y1 - X1 * Y0) / (Y1 - Y0)
                    ' TEST FOR CONVERGENCE
                    N = N + 1

                    If N >= M Then Return
                    If Abs(X1 - X) < E Then Return

                    ' GET A NEW Y(X) VALUE
                    SUBROUTINE_4_5_6(X, Y)

                    ' APPLY HAMMING'S MODIFICATION
                    If Y1 * Y = 0 Then Return
                    If Y0 * Y > 0 Then
                        TestCond = True

                    Else
                        X1 = X
                        Y1 = Y
                        Y0 = Y0 / 2
                    End If

                Loop Until TestCond = True

                X0 = X
                Y0 = Y
                Y1 = Y1 / 2
            Loop

        End Sub

        ''' <summary> Aitken acceleration subroutine 
        ''' This routine calculates the zeros of a function
        ''' by iteration, and employs aitken acceleration to
        ''' speed up convergence.
        ''' The subroutine requires an initial guess, X0,
        ''' and two convergence factors, C and E.
        ''' E relates to the accuracy of the estimate, and C
        ''' is used to aid the convergence.
        ''' Also required is an iteration limit, M.
        ''' C=-1 is a normal value. If divergence occurs,
        ''' smaller and/or positive values should be tried.
        ''' The result is returned in X.
        ''' The number of iterations is returned in N.
        ''' The function = Y(X)
        ''' </summary>
        Public Shared Sub AITKEN(ByVal X0 As Decimal, ByVal E As Decimal, ByVal C As Decimal, ByVal M As Int32, ByRef X As Decimal, ByRef N As Int32)
            N = 0
            X = X0

            Dim Y As Decimal, X1 As Decimal, X2 As Decimal, K As Decimal

            ' GET Y
            Do

                Dim TestCond As Boolean = False

                Do

                    SUBROUTINE_4_5_6(X, Y)
                    Y = X + C * Y

                    'Console.WriteLine(Y.ToString())
                    ' ARE THERE ENOUGH POINTS FOR ACCELERATION?
                    If N > 0 Then
                        TestCond = True

                    Else
                        X1 = Y
                        X = X1
                        N = N + 1
                    End If

                Loop Until TestCond = True

                X2 = Y
                N = N + 1

                ' GUARD AGAINST A ZERO DENOMINATOR
                If X2 - 2 * X1 + X0 = 0 Then X0 = X0 + 0.001
                ' PERFORM ACCELERATION
                K = (X2 - X1) * (X2 - X1) / (X2 - 2 * X1 + X0)
                X2 = X2 - K

                ' TEST FOR CONVERGENCE
                If N >= M Then Return
                If Abs(K) < E Then Return
                X0 = X1
                X1 = X2
                X = X1
            Loop

        End Sub

        ''' <summary> Aitken-steffenson iteration subroutine 
        ''' This routine calculates the zeros of a function
        ''' by iteration, and employs aitken acceleration to
        ''' speed up convergence.
        ''' The subroutine requires an initial guess, X0,
        ''' and two convergence factors, C and E.
        ''' E relates to the accuracy of the estimate, and C
        ''' is used to aid the convergence.
        ''' Also required is a limit to the number of iterations, M.
        ''' C=-1 is a normal value. If divergence occurs,
        ''' smaller and/or positive values should be tried.
        ''' The result is returned in X.
        ''' The number of iterations is returned in N.
        ''' The function = Y(X)
        ''' </summary>
        Public Shared Sub ASITER(ByVal X0 As Decimal, ByVal E As Decimal, ByVal C As Decimal, ByVal M As Int32, ByRef X As Decimal, ByRef N As Int32)

            Dim Y As Decimal, X1 As Decimal, X2 As Decimal, K As Decimal, M1 As Int32
            N = 0

            Do
                M1 = 0
                X = X0

                ' GET Y
                Dim TestCond As Boolean = False

                Do

                    SUBROUTINE_7(X, Y, C)
                    Y = X + C * Y

                    ' ARE THERE ENOUGH POINTS FOR ACCELERATION?
                    If M1 > 0 Then
                        TestCond = True

                    Else
                        N = N + 1
                        M1 = M1 + 1
                        X = X1
                        X1 = Y
                    End If

                Loop Until TestCond = True

                X2 = Y
                ' PERFORM ACCELERATION
                K = (X2 - 2 * X1 + X0)

                If K = 0 Then K = 0.001
                K = (X1 - X0) * (X1 - X0) / K
                X0 = X0 - K

                ' TEST FOR CONVERGENCE
                If N >= M Then Return
                If Abs(X - X0) < E Then Return
                ' REPEAT PROCESS
            Loop

        End Sub

        ''' <summary> Synthetic division subroutine 
        ''' Assumes real polynomial coefficients.
        ''' Form calculated is A(X)=C(X)/B(X).
        ''' The input polynomial coefficients are
        ''' C(I) and B(I), the result is A(I).
        ''' C(x) is of order N1, B(X) is of order N2.
        ''' Result is of order N1-N2 (at most).
        ''' </summary>
        Public Shared Sub RSYNDIV(ByVal N1 As Int32, ByVal N2 As Int32, ByVal B() As Decimal, ByVal C() As Decimal, ByRef A() As Decimal)

            Dim I As Int32, J As Int32

            For I = N1 To N2 Step -1
                A(I - N2) = C(I) / B(N2)

                If I = N2 Then Return

                For J = 0 To N2
                    C(I - J) = C(I - J) - A(I - N2) * B(N2 - J)
                Next J
            Next I

        End Sub

        ''' <summary> Subroutine for determining additional roots of a function given a set of already established roots 
        ''' Use is restricted to real roots.
        ''' Method applied is newton-raphson iteration.
        ''' The L established roots are A(I).
        ''' The function Y and its derivative are placed in subroutine 44300.
        ''' The initial guess is X0.
        ''' The accuracy criteria is E.
        ''' </summary>
        Public Shared Sub NEXTROOT(ByVal X0 As Decimal, ByVal E As Decimal, ByVal M As Int32, ByVal L As Int32, ByVal A() As Decimal, ByRef X As Decimal, ByRef N As Int32)

            Dim Y As Decimal, Y0 As Decimal, Y1 As Decimal, B As Decimal, X1 As Decimal
            Dim I As Int32
            N = 0

            ' GIVEN X0, FIND F/F'.
            Dim TestCond As Boolean = False

            Do
                X = X0

                SUBROUTINE_9(X, Y, Y1)
                B = Y1 / Y

                For I = 1 To L
                    B = B - 1 / (X0 - A(I))
                Next I

                ' NEWTON-RAPHSON ITERATION
                X1 = X0 - 1 / B
                N = N + 1

                ' TEST FOR CONVERGENCE
                If N >= M Then
                    TestCond = True

                Else

                    If Abs(X1 - X0) < E Then
                        TestCond = True

                    Else
                        X0 = X1
                    End If
                End If

            Loop Until TestCond = True

            X = X1
        End Sub

    End Class

End Namespace
