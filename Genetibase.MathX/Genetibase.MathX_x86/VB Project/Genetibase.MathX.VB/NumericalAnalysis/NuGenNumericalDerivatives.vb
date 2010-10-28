Imports System.Math

Namespace NumericalAnalysis

    ''' <summary> Numerical Derivatives </summary>
    Public Class NuGenNumericalDerivatives

        ''' <summary> Function subroutine </summary>
        Protected Shared Sub FUNCTION_SUBROUTINE(ByVal X() As Decimal, ByRef Y As Decimal, ByRef D() As Decimal)
            Y = Sin(X(1)) + 2 * Cos(X(2)) - Sin(X(3))
            D(1) = Cos(X(1))
            D(2) = -2 * Sin(X(2))
            D(3) = Cos(X(3))
        End Sub

        ''' <summary> Steepest descent optimization subroutine 
        ''' This program find the local maximum or minimum
        ''' of an L-dimensional function using the method
        ''' of steepest descent, or the gradient.
        ''' The function, Y(X(1),X(2)...), is placed in the
        ''' function subroutine, along with the L derivatives
        ''' of F, D(I).
        ''' The routine seeks using an internally adjusted
        ''' multiplier, K. The search is made until an error
        ''' limit, E, is reached.
        ''' The user must supply initial values for the X(I),
        ''' As well as K (initial) and E. The program returns
        ''' the locally optimum X(I) set.
        ''' Remember to dimension X(I) in the calling program.
        ''' The program needs three values of Y to get started.
        ''' </summary>
        Public Shared Sub STEEPDS(ByVal L As Int32, ByVal E As Decimal, ByVal M As Int32, ByVal K As Decimal, ByRef X() As Decimal, ByRef N As Int32)

            Dim maxL As Int32
            maxL = Max(L, 3)

            Dim J As Int32, I As Int32, YT As Decimal, Y(maxL) As Decimal, D(maxL) As Decimal, X1(L) As Decimal
            N = 0

            ' START INITIAL PROBE
            For J = 1 To 3

                ' OBTAIN Y AND D(I)
                FUNCTION_SUBROUTINE(X, YT, D) ' 44300
                Y(J) = YT
                ' UPDATE X(I)
                MAGNITUDE(L, K, D, X, X1, Y)  ' 47735
            Next J

            ' WE NOW HAVE A HISTORY TO BASE THE SUBSEQUENT SEARCH ON
            ' ACCELERATE SEARCH IF APPROACH IS MONOTONIC
            Do

                If (Y(3) - Y(2)) / (Y(2) - Y(1)) > 0 Then K = K * 1.2

                ' DECELERATE IF HEADING THE WRONG WAY
                If Y(3) < Y(2) Then K = K / 2

                ' UPDATE THE Y(I) IF VALUE HAS DECREASED
                If Y(3) > Y(2) Then
                    Y(1) = Y(2)
                    Y(2) = Y(3)

                Else

                    ' RESTORE THE X(I)
                    For I = 1 To L
                        X(I) = X1(I)
                    Next I

                End If

                ' OBTAIN NEW VALUES
                FUNCTION_SUBROUTINE(X, YT, D) ' 44300
                Y(3) = YT
                ' UPDATE X(I)
                MAGNITUDE(L, K, D, X, X1, Y)  ' 47735
                ' CHECK FOR CONVERGENCE
                N = N + 1

                If N >= M Then Return
                If Abs(Y(3) - Y(2)) < E Then Return
                ' TRY ANOTHER ITERATION
            Loop

        End Sub

        ''' <summary> Find the magnitude of the gradient </summary>
        Private Shared Sub MAGNITUDE(ByVal L As Int32, ByVal K As Decimal, ByRef D() As Decimal, ByRef X() As Decimal, ByRef X1() As Decimal, ByRef Y() As Decimal)

            Dim YT As Decimal, DT As Decimal
            Dim I As Int32
            DT = 0

            For I = 1 To L
                DT = DT + D(I) * D(I)
            Next I

            DT = Sqrt(DT)

            ' UPDATE THE X(I)
            For I = 1 To L
                ' SAVE OLD VALUES
                X1(I) = X(I)
                X(I) = X(I) + K * D(I) / DT
            Next I

            FUNCTION_SUBROUTINE(X, YT, D) ' 44300 
            Y(3) = YT
        End Sub

        ''' <summary> Steepest descent optimization subroutine 
        ''' This program find the local maximum or minimum
        ''' of an L-dimensional function using the method
        ''' of steepest descent, or the gradient.
        ''' The function, Y(X(1),X(2)...), is placed in the
        ''' function subroutine. Finite differences are used to
        ''' calculate the L partial derivatives.
        ''' Of F, D(I).
        ''' The routine seeks using an internally adjusted
        ''' multiplier, K. The search is made until an error
        ''' limit, E, is reached.
        ''' The user must supply initial values for the X(I),
        ''' As well as K (initial) and E. The program returns
        ''' the locally optimum X(I) set.
        ''' Remember to dimension X(I) in the calling program.
        ''' </summary>
        Public Shared Sub STEEPDA(ByVal L As Int32, ByVal E As Decimal, ByVal M As Int32, ByVal K As Decimal, ByRef X() As Decimal, ByRef N As Int32)

            Dim maxL As Int32
            maxL = Max(L, 3)

            Dim J As Int32, I As Int32, DT As Decimal, YT As Decimal, Y(maxL) As Decimal, D(maxL) As Decimal, X1(L) As Decimal
            N = 0
            ' THE PROGRAM NEEDS THREE VALUES OF Y TO GET STARTED.
            ' GENERATE STARTING D(I) VALUES.
            ' THESE ARE NOT EVEN GOOD GUESSES, AND SLOW THE PROGRAM A LITTLE.
            DT = 1
            D(1) = 1 / Sqrt(L)

            For I = 2 To L
                D(I) = D(I - 1)
            Next I

            ' START INITIAL PROBE
            For J = 1 To 3

                ' OBTAIN Y
                FUNCTION_SUBROUTINE(X, YT, D) ' 44300 
                Y(J) = YT
                ' OBTAIN APPROXIMATIONS TO THE D(I)
                FINITE_DIFFERENCES(L, K, YT, DT, Y, D, X) '47866
                ' UPDATE X(I)
                MAGNITUDE2(L, D, DT)  '47849
                UpdateX(L, K, DT, D, X, X1, Y) ' 47856
            Next J

            Do

                ' WE NOW HAVE A HISTORY TO BASE THE SUBSEQUENT SEARCH ON
                ' ACCELERATE SEARCH IF APPROACH IS MONOTONIC
                If (Y(3) - Y(2)) / (Y(2) - Y(1)) > 0 Then K = K * 1.2

                ' DECELERATE IF HEADING THE WRONG WAY
                If Y(3) < Y(2) Then K = K / 2

                ' UPDATE THE Y(I) IF Y(3)>Y(2)
                If Y(3) > Y(2) Then
                    Y(1) = Y(2)
                    Y(2) = Y(3)

                Else

                    ' RESTORE THE X(I)
                    For I = 1 To L
                        X(I) = X1(I)
                    Next I

                End If

                ' OBTAIN NEW VALUES
                FUNCTION_SUBROUTINE(X, YT, D) ' 44300 
                Y(3) = YT
                FINITE_DIFFERENCES(L, K, YT, DT, Y, D, X) '47866

                ' IF D=0 THEN THE PRECISION LIMIT OF THE COMPUTER HAS BEEN REACHED
                If DT = 0 Then Return
                ' UPDATE X(I)
                UpdateX(L, K, DT, D, X, X1, Y) ' 47856
                ' CHECK FOR CONVERGENCE
                N = N + 1

                If N >= M Then Return
                If Abs(Y(3) - Y(2)) < E Then Return
                ' TRY ANOTHER ITERATION
            Loop

        End Sub

        ''' <summary> Find the magnitude of the gradient </summary>
        Private Shared Sub MAGNITUDE2(ByVal L As Int32, ByRef D() As Decimal, ByRef DT As Decimal)

            Dim I As Int32
            DT = 0

            For I = 1 To L
                DT = DT + D(I) * D(I)
            Next I

            DT = Sqrt(DT)
        End Sub

        ''' <summary> Update the X(I) </summary>
        Private Shared Sub UpdateX(ByVal L As Int32, ByVal K As Decimal, ByVal DT As Decimal, ByRef D() As Decimal, ByRef X() As Decimal, ByRef X1() As Decimal, ByRef Y() As Decimal)

            Dim I As Int32, YT As Decimal

            For I = 1 To L
                ' SAVE OLD VALUES
                X1(I) = X(I)
                X(I) = X(I) + K * D(I) / DT
            Next I

            FUNCTION_SUBROUTINE(X, YT, D) ' 44300 
            Y(3) = YT
        End Sub

        ''' <summary> Finite differences subroutine for the D(I) approximation
        ''' Look ahead one half interval
        ''' </summary>
        Private Shared Sub FINITE_DIFFERENCES(ByVal L As Int32, ByVal K As Decimal, ByVal YT As Decimal, ByVal DT As Decimal, ByVal Y() As Decimal, ByVal D() As Decimal, ByRef X() As Decimal)

            Dim I As Int32, A As Decimal, B As Decimal

            For I = 1 To L
                ' SAVE X(I)
                A = X(I)
                ' FIND INCREMENT
                B = D(I) * K / (2 * DT)
                ' MOVE INCREMENT IN X(I)
                X(I) = X(I) + B

                ' OBTAIN Y
                FUNCTION_SUBROUTINE(X, YT, D) ' 44300 

                ' GUARD AGAINST DIVIDE BY ZERO NEAR MAXIMUM
                If B = 0 Then B = 0.00000000001
                ' UPDATE D(I)
                D(I) = (YT - Y(3)) / B

                ' GUARD AGAINST LOCKED UP DERIVATIVE
                If D(I) = 0 Then D(I) = 0.00001
                ' RESTORE X(I) AND Y
                X(I) = A
                YT = Y(3)
            Next I

            ' OBTAIN D
            ' GOSUB 47849
        End Sub

    End Class

End Namespace
