Imports System.Math

''' <summary> Numerical computation of special distributions </summary>
Public Class NuGenDistributions

    ''' <summary> [42900] Linear random number generator (LINEAR) 
    ''' U=mean, V=spread, D=seed
    ''' </summary>
    Public Shared Sub LINEAR(ByVal U As Decimal, ByVal V As Decimal, ByRef I9 As Int32, ByRef E As Decimal)

        Dim A As Decimal, B As Decimal, C As Decimal, D As Decimal
        I9 = I9 + 1
        A = 3.1415926535898
        B = 2.71828182845905
        C = 1.41421356237309
        D = 1 + Abs(D)
        E = E + (1 + D / B) * C
        E = E * I9
        E = E - A * Int(E / A)
        E = E - Int(E) + 0.018

        If E > 0.1 Then E = E + 0.009000001
        If E > 0.2 Then E = E - 0.002
        If E > 0.3 Then E = E - 0.005000001
        If E > 0.4 Then E = E - 0.005000001
        If E > 0.5 Then E = E - 0.015
        E = V * (E - 0.5) + U
    End Sub

    ''' <summary> [42925] Normal distribution by central limit theorem (NORMAL) 
    ''' U=mean, V=standard deviation, E=random no. generated
    ''' </summary>
    Public Shared Sub NORMAL(ByVal U As Decimal, ByVal V As Decimal, ByRef E As Decimal)

        Dim I9 As Int32
        E = 0

        For I9 = 1 To 48
            E = E + Rnd(0.9990001) - 0.5
        Next I9

        E = V * E / 2 + U
    End Sub

    ''' <summary> [42950] Poisson random number generator (POISSON) 
    ''' Input parameter U
    '''</summary>
    Public Shared Sub POISSON(ByVal U As Decimal, ByRef E As Decimal)

        Dim Y As Decimal, Y1 As Decimal, X As Decimal, X1 As Decimal
        X = Rnd(0.9990001) * Exp(U)
        X1 = 1
        Y1 = 1
        Y = 0

        Do Until X1 > X
            Y = Y + 1
            Y1 = Y1 * U / Y
            X1 = X1 + Y1
        Loop

        If Y > 0 Then Y = Y - (X1 - X) / Y1
        E = Y
    End Sub

    ''' <summary> [42975] Binomial random number generator (BINOMIAL)
    ''' B=probability per trial, N=number of trials
    ''' </summary>
    Public Shared Sub BINOMIAL(ByVal B As Decimal, ByVal N As Int32, ByRef E As Decimal)

        Dim K As Int32, Y1 As Decimal
        E = 0

        For K = 1 To N
            Y1 = Rnd(0.9990001)

            If Y1 < B Then E = E + 1
        Next K

    End Sub

    ''' <summary> [43000] Exponential random number generator (EXPONENT)
    ''' U=mean
    ''' </summary>
    Public Shared Sub EXPONENT(ByVal U As Decimal, ByVal E As Decimal)

        Dim X As Decimal
        X = Rnd(0.9990001)
        E = -U * Log(1 - X)
    End Sub

    ''' <summary> [43025] Fermi random number generator (FERMI)
    ''' U=inflection point, V=spread of transition region
    ''' </summary>
    Public Shared Sub FERMI(ByVal U As Decimal, ByVal V As Decimal, ByRef E As Decimal)

        Dim Y As Decimal, Y1 As Decimal, X As Decimal, A As Decimal, B As Decimal
        X = Rnd(0.9990001)
        Y = 1
        A = Exp(4 * U / V)
        B = (X - 1) * Log(1 + A)

        Dim TestCond As Boolean = False

        Do
            Y1 = B + Log(A + Exp(Y))

            If Abs((Y - Y1) / Y) < 0.001 Then
                TestCond = True

            Else
                Y = Y1
            End If

        Loop Until TestCond = True

        E = V * Y1 / 4
    End Sub

    ''' <summary> [43050] Cauchy random number generator (CAUCHY)
    ''' U=mean
    ''' </summary>
    Public Shared Sub CAUCHY(ByVal U As Decimal, ByRef E As Decimal)

        Dim X As Decimal
        X = Rnd(0.9990001)
        E = U * Sin(1.5707963267 * X) / Cos(1.5707963267 * X)
    End Sub

    ''' <summary> [43075] Gamma (N=2) random number generator (GAMMA)
    ''' B=input parameter
    ''' </summary>
    Public Shared Sub GAMMA(ByVal B As Decimal, ByRef E As Decimal)

        Dim Y As Decimal, Y1 As Decimal, X As Decimal
        Y = 1
        X = Rnd(0.9990001)

        Dim TestCond As Boolean = False

        Do
            Y1 = -Log((1 - X) / (1 + Y))

            If Abs((Y1 - Y) / Y) < 0.001 Then
                TestCond = True

            Else
                Y = Y1
            End If

        Loop Until TestCond = True

        E = B * Y1
    End Sub

    ''' <summary> [43100] Beta random number generator (BETA)
    ''' Input parameters are A and B
    ''' A is restricted to A=1 and A=2
    ''' Guard against divide by zero
    ''' </summary>
    Public Shared Sub BETA(ByVal A As Int32, ByVal B As Decimal, ByRef E As Decimal)

        Dim Y As Decimal, Y1 As Decimal, X As Decimal

        If B <= 0 Then
            E = 1
            Return
        End If

        ' B>0
        If A > 2 Then Return
        X = Rnd(0.9990001)

        If A <> 2 Then
            E = 1 - (1 - X) ^ (1 / B)
            Return
        End If

        Y = 1

        Dim TestCond As Boolean = False

        Do
            Y1 = 1 - ((1 - X) / (1 + B * Y)) ^ (1 / B)

            If Abs((Y - Y1) / Y) < 0.001 Then
                TestCond = True

            Else
                Y = Y1
            End If

        Loop Until TestCond = True

        E = Y1
    End Sub

    ''' <summary> [43150] Weibull random number genrator (WEIBULL)
    ''' Input parameters are U and V
    ''' </summary>
    Public Shared Sub WEIBULL(ByVal U As Decimal, ByVal V As Decimal, ByRef E As Decimal)

        Dim X As Decimal
        X = Rnd(0.9990001)
        E = U * ((Log(1 / (1 - X))) ^ (1 / V))
    End Sub

End Class
