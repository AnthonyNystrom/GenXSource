Imports System.Math

Namespace NumericalAnalysis

    ''' <summary> Interpolation </summary>
    Public Class NuGenInterpolation3

        ''' <summary> Lagrange interpolation </summary>
        Public Shared Sub LAGRANGE_INTERPOLATION(ByRef N As Decimal, ByVal V As Decimal, ByVal L() As Decimal, ByVal X() As Decimal, ByVal Y() As Decimal, ByVal XT As Decimal, ByRef YT As Decimal)

            If XT < X(1) Then
                N = 0
                Return
            End If

            If XT > X(V - N) Then
                N = 0
                Return
            End If

            Dim I As Int32 = 0, J As Int32, K As Int32

            Do
                I = I + 1
            Loop While XT > X(I)

            I = I - 1

            For J = 0 To N
                L(J) = 1
            Next J

            YT = 0

            For K = 0 To N
                For J = 0 To N

                    If Not (J = K) Then
                        L(K) = L(K) * (XT - X(J + I)) / (X(I + K) - X(J + I))
                    End If

                Next J

                YT = YT + L(K) * Y(I + K)
            Next K

        End Sub

        ''' <summary> Newton interpolation </summary>
        Public Shared Sub NEWTON_INTERPOLATION(ByRef N As Decimal, ByVal V As Decimal, ByVal X() As Decimal, ByVal Y() As Decimal, ByVal XT As Decimal, ByRef YT As Decimal, ByRef E As Decimal)

            Dim Y1(V) As Decimal, Y2(V - 1) As Decimal, Y3(V - 2) As Decimal
            N = 1

            If XT < X(1) Then
                N = 0
                Return
            End If

            If XT > X(V - 3) Then
                N = 0
                Return
            End If

            Dim I As Int32

            For I = 1 To V - 1
                Y1(I) = (Y(I + 1) - Y(I)) / (X(I + 1) - X(I))
            Next I

            For I = 1 To V - 2
                Y2(I) = (Y1(I + 1) - Y1(I)) / (X(I + 1) - X(I))
            Next I

            For I = 1 To V - 3
                Y3(I) = (Y2(I + 1) - Y2(I)) / (X(I + 1) - X(I))
            Next I

            I = 0

            Do
                I = I + 1
            Loop While XT > X(I)

            I = I - 1

            Dim A As Decimal = XT - X(I)
            Dim B As Decimal = A * (XT - X(I + 1))
            Dim C As Decimal = B * (XT - X(I + 2))
            YT = Y(I) + A * Y1(I) + B * Y2(I) + C * Y3(I)
            E = C * (XT - X(I + 3)) * YT / 24
        End Sub

        ''' <summary> Akima interpolation </summary>
        Public Shared Sub AKIMA_SPLINE(ByVal V As Decimal, ByVal X() As Decimal, ByVal Y() As Decimal, ByVal XT As Decimal, ByRef YT As Decimal, ByRef I As Int32)

            Dim N As Int32, M(V + 4) As Decimal, Z(V + 1) As Decimal
            N = 1

            If XT < X(1) Then
                N = 0
                Return
            End If

            If XT > X(V - 3) Then
                N = 0
                Return
            End If

            X(0) = 2 * X(1) - X(2)

            'Dim I As Int32
            For I = 1 To V - 1
                M(I + 2) = (Y(I + 1) - Y(I)) / (X(I + 1) - X(I))
            Next I

            M(V + 2) = 2 * M(V + 1) - M(V)
            M(V + 3) = 2 * M(V + 2) - M(V + 1)
            M(2) = 2 * M(3) - M(4)
            M(1) = 2 * M(2) - M(3)

            Dim A As Decimal, B As Decimal

            For I = 1 To V
                A = Abs(M(I + 3) - M(I + 2))
                B = Abs(M(I + 1) - M(I))

                If A + B <> 0 Then
                    Z(I) = (A * M(I + 1) + B * M(I + 2)) / (A + B)

                Else
                    Z(I) = (M(I + 2) + M(I + 1)) / 2
                End If

            Next I

            I = 0

            Do
                I = I + 1
            Loop While XT >= X(I)

            I = I - 1
            B = X(I + 1) - X(I)
            A = XT - X(I)
            YT = Y(I) + Z(I) * A + (3 * M(I + 2) - 2 * Z(I) - Z(I + 1)) * A * A / B
            YT = YT + (Z(I) + Z(I + 1) - 2 * M(I + 2)) * A * A * A / (B * B)
        End Sub

        ''' <summary> Lagrange derivative interpolation </summary>
        Public Shared Sub LAGRANGE_DERIVATIVE_INTERPOLATION(ByVal V As Decimal, ByVal X() As Decimal, ByVal Y() As Decimal, ByRef N As Int32, ByVal XT As Decimal, ByRef YT As Decimal)

            Dim L(9) As Decimal, M(9, 9) As Decimal

            If XT <= X(1) Then
                N = 0
                Return
            End If

            If XT > X(V - N) Then
                N = 0
                Return
            End If

            Dim I As Int32 = 0, J As Int32, K As Int32, P As Int32

            Do
                I = I + 1
            Loop While XT > X(I)

            I = I - 1

            For J = 0 To N
                L(J) = 0

                For K = 0 To N
                    M(J, K) = 1
                Next K
            Next J

            YT = 0

            For K = 0 To N
                For J = 0 To N

                    If J <> K Then

                        For P = 0 To N

                            If P <> K Then
                                If P <> J Then
                                    M(P, K) = M(P, K) * (XT - X(J + I)) / (X(I + K) - X(I + J))

                                Else
                                    M(P, K) = M(P, K) / (X(I + K) - X(I + J))
                                End If
                            End If

                        Next P

                    End If

                Next J

                For P = 0 To N

                    If P <> K Then
                        L(K) = L(K) + M(P, K)
                    End If

                Next P

                YT = YT + L(K) * Y(I + K)
            Next K

        End Sub

        ''' <summary> General integration </summary>
        Public Shared Sub GENERAL_INTEGRATION(ByVal V As Int32, ByVal X1 As Decimal, ByVal X2 As Decimal, ByVal X() As Decimal, ByVal Y() As Decimal, ByRef Z1 As Decimal, ByRef ZT As Decimal)

            Dim Z(V) As Decimal, M(V + 3) As Decimal, X3 As Decimal
            ZT = 0
            Z1 = 0

            If X1 < X(1) Then Return
            If X2 > X(V - 3) Then Return
            If X1 >= X2 Then
                X3 = X1
                X1 = X2
                X2 = X3
                Z1 = 1
            End If

            If X2 = X1 Then Return

            Dim I1 As Decimal, I2 As Decimal
            _46232(V, X, Y, X1, X2, I1)
            _46267(V, X, Y, X1, X2, I2)
            ZT = 4 * I2 / 3 - I1 / 3

            If Z1 <> 0 Then
                ZT = -ZT
                X2 = X1
                X1 = X3
            End If

            Z1 = 1
        End Sub

        '[label 46232]
        Private Shared Sub _46232(ByVal V As Decimal, ByVal X() As Decimal, ByVal Y() As Decimal, ByVal X1 As Decimal, ByVal X2 As Decimal, ByRef I1 As Decimal)

            Dim XT As Decimal, YT As Decimal, D As Decimal, I As Int32
            I1 = 0

            Dim N1 As Int32 = 0
            XT = X1
            AKIMA_SPLINE(V, X, Y, XT, YT, I)

            If X2 <= X(I + 1) Then
                N1 = N1 + 1
                D = YT
                XT = X2
                AKIMA_SPLINE(V, X, Y, XT, YT, I)
                I1 = (YT + D) * (X2 - X1) / 2
                Return
            End If

            Dim J1 As Int32 = I
            I1 = I1 + (YT + Y(I + 1)) * (X(I + 1) - XT) / 2

            Do Until X2 < X(J1 + 3)
                N1 = N1 + 1
                I1 = I1 + (Y(J1 + 1) + Y(J1 + 3)) * (X(J1 + 3) - X(J1 + 1)) / 2
                J1 = J1 + 2
            Loop

            XT = X2
            AKIMA_SPLINE(V, X, Y, XT, YT, I)
            I1 = I1 + (YT + Y(J1 + 1)) * (X2 - X(J1 + 1)) / 2
            N1 = N1 + 1
        End Sub

        '[label 46267]
        Private Shared Sub _46267(ByVal V As Decimal, ByVal X() As Decimal, ByVal Y() As Decimal, ByVal X1 As Decimal, ByVal X2 As Decimal, ByRef I2 As Decimal)

            Dim XT As Decimal, YT As Decimal, D As Decimal, I As Int32
            I2 = 0
            XT = X1
            AKIMA_SPLINE(V, X, Y, XT, YT, I)
            D = YT

            If X2 <= X(I + 1) Then
                XT = X1 + (X2 - X1) / 2
                AKIMA_SPLINE(V, X, Y, XT, YT, I)
                I2 = I2 + (D + YT) * (X2 - X1) / 4
                D = YT
                XT = X2
                AKIMA_SPLINE(V, X, Y, XT, YT, I)
                I2 = I2 + (D + YT) * (X2 - X1) / 4
                Return
            End If

            XT = X1 + (X(I + 1) - X1) / 2

            Dim J1 As Int32 = I
            AKIMA_SPLINE(V, X, Y, XT, YT, I)
            I2 = I2 + (YT + D) * (XT - X1) / 2
            I2 = I2 + (YT + Y(J1 + 1)) * (X(J1 + 1) - XT) / 2

            Do Until X2 < X(J1 + 2)
                I2 = I2 + (Y(J1 + 1) + Y(J1 + 2)) * (X(J1 + 2) - X(J1 + 1)) / 2
                J1 = J1 + 1
            Loop

            XT = X2 - (X2 - X(J1 + 1)) / 2
            AKIMA_SPLINE(V, X, Y, XT, YT, I)
            D = YT
            I2 = I2 + (Y(J1 + 1) + D) * (X2 - XT) / 2
            XT = X2
            AKIMA_SPLINE(V, X, Y, XT, YT, I)
            I2 = I2 + (D + YT) * (X2 - X(J1 + 1)) / 4
        End Sub

    End Class

End Namespace
