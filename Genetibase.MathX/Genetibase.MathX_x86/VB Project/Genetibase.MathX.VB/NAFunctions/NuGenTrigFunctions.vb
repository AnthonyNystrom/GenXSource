Imports System.Math
''' <summary> Trigonometric functions </summary>

Public Class NuGenTrigFunctions

#Region "SINE AND COSINE"

    ''' <summary> [43210] Sine series </summary>
    Public Shared Function SINE(ByVal X As Decimal) As Decimal

        Dim X1 As Int32, X2 As Decimal, X3 As Decimal, Z As Decimal, Y As Decimal
        Dim N As Int32, A(6) As Decimal
        X1 = 1

        If X < 0 Then X1 = -1
        X3 = Abs(X)
        X2 = 3.1415926535898
        ' REDUCE RANGE
        'INT - OK
        X3 = X3 - 2 * X2 * Int(0.5 * X3 / X2)

        If X3 > X2 Then X1 = -X1
        If X3 > X2 Then X3 = X3 - X2
        If X3 > X2 / 2 Then X3 = X2 - X3
        Z = X3 * X3
        SINDATA(N, A) ' 43300
        SERSUM(N, A, Z, Y) ' 43200
        Y = X1 * X3 * Y
        Return Y
    End Function

    ''' <summary> [43225] Cosine series </summary>
    Public Shared Function COSINE(ByVal X As Decimal) As Decimal

        Dim X1 As Int32, X2 As Decimal, X3 As Decimal, Z As Decimal, Y As Decimal
        Dim N As Int32, A(6) As Decimal
        X2 = 3.1415926535898
        X1 = 1
        ' REDUCE RANGE
        X3 = Abs(X)
        'INT -OK
        X3 = X3 - 2 * X2 * Int(0.5 * X3 / X2)

        If X3 > X2 Then X1 = -1
        If X3 > X2 Then X3 = X3 - X2
        If X3 > X2 / 2 Then X1 = -X1
        If X3 > X2 / 2 Then X3 = X2 - X3
        Z = X3 * X3
        COSDATA(N, A)  '43310
        SERSUM(N, A, Z, Y) ' 43200
        Y = X1 * Y
        Return Y
    End Function

#End Region

#Region "ARCTANGENT"

    ''' <summary> [43245] Arctangent series subroutine </summary>
    Public Shared Function ARCTAN(ByVal X As Decimal) As Decimal

        Dim X1 As Int32, X3 As Decimal, Z As Decimal, Z1 As Decimal, Y As Decimal
        Dim N As Int32, A(10) As Decimal
        X1 = 1

        If X < 0 Then X1 = -1
        X3 = Abs(X)
        Z1 = (X3 - 1) / (X3 + 1)

        If X3 < 0.5 Then Z1 = X3
        Z = Z1 * Z1
        ' GET SERIES COEFFICIENTS
        ARCDATA(N, A) ' 43320
        ' SUM SERIES
        SERSUM(N, A, Z, Y)  ' 43200
        Y = Z1 * Y

        If X3 >= 0.5 Then Y = Y + 0.78539816339745
        Y = X1 * Y
        Return Y
    End Function

#End Region

#Region "NATURAL LOGARITHM AND EXPONENT"

    ''' <summary> [43280] Natural logarithm series subroutine </summary>
    Public Shared Function LNX(ByVal X As Decimal) As Decimal

        Dim X1 As Int32, X2 As Decimal, X3 As Decimal, Z As Decimal, Z1 As Decimal, Y As Decimal, C As Int32
        Dim N As Int32, A(9) As Decimal

        X1 = 1
        C = -1
        X2 = 2.71828182845905
        X3 = X

        If X < 1 Then
            X = 1 / X
            X1 = -1
        End If

        ' REDUCE RANGE
        X = X2 * X

        Do
            C = C + 1
            X = X / X2
        Loop While X > X2

        Z1 = (X - 1.6487212707) / (X + 1.6487212707)
        Z = Z1 * Z1
        LNDATA(N, A) ' 43360
        SERSUM(N, A, Z, Y)  ' 43200
        Y = X1 * (C + Z1 * Y + 0.5)
        X = X3
        Return Y
    End Function

    ''' <summary> [43470] Exponent series subroutine </summary>
    Public Shared Function EXPX(ByVal X As Decimal) As Decimal

        Dim X1 As Int32, X2 As Decimal, X3 As Decimal, Z As Decimal, Z1 As Decimal, Y As Decimal, C As Int32, I As Int32
        Dim N As Int32, A(8) As Decimal
        X1 = 1
        X3 = X

        If X < 0 Then X1 = -1
        X = Abs(X)
        ' REDUCE RANGE
        X2 = Int(X)
        X = X - X2
        ' GET COEFFICIENTS
        EXPDATA(N, A) ' 43380
        Z = X
        ' SUM SERIES
        SERSUM(N, A, Z, Y)  ' 43200

        If X2 >= 1 Then

            For I = 1 To X2
                Y = Y * 2.71828182845905
            Next I

        End If

        If X1 < 0 Then Y = 1 / Y
        X = X3
        Return Y
    End Function

#End Region

#Region "BASE-10 LOGARITHM AND EXPONENT"

    ''' <summary> [43260] Log base ten series subroutine </summary>
    Public Shared Function LOGX(ByVal X As Decimal) As Decimal

        Dim X1 As Int32, X2 As Decimal, X3 As Decimal, Z As Decimal, Z1 As Decimal, Y As Decimal, C As Int32
        Dim N As Int32, A(9) As Decimal
        X1 = 1
        C = -1
        X2 = 10
        X3 = X

        If X < 1 Then
            X = 1 / X
            X1 = -1
        End If

        ' REDUCE RANGE
        X = X2 * X

        Do
            C = C + 1
            X = X / X2
        Loop While X > X2

        Z1 = (X - 3.16227766) / (X + 3.16227766)
        Z = Z1 * Z1
        LOGDATA(N, A) ' 43340
        SERSUM(N, A, Z, Y)  ' 43200
        Y = X1 * (C + Z1 * Y + 0.5)
        X = X3
        Return Y
    End Function

    ''' <summary> [43450] Power of ten series subroutine </summary>
    Public Shared Function TENPOW(ByVal X As Decimal) As Decimal

        Dim X1 As Int32, X2 As Decimal, X3 As Decimal, Z As Decimal, Y As Decimal, I As Int32
        Dim N As Int32, A(9) As Decimal
        X1 = 1
        X3 = X

        If X < 0 Then X1 = -1
        X = Abs(X)
        ' REDUCE RANGE
        X2 = Int(X)
        X = X - X2
        ' GET COEFFICIENTS
        TENDATA(N, A) ' 43400
        Z = X
        ' SUM SERIES
        SERSUM(N, A, Z, Y)  ' 43200
        Y = Y * Y

        If X2 >= 1 Then

            For I = 1 To X2
                Y = Y * 10
            Next I

        End If

        If X1 < 0 Then Y = 1 / Y
        X = X3
        Return Y
    End Function

#End Region

#Region "Data"

    ''' <summary> [43200] Series summation subroutine </summary>
    Private Shared Sub SERSUM(ByVal N As Int32, ByVal A() As Decimal, ByVal Z As Decimal, ByRef Y As Decimal)

        Dim I As Int32
        Y = 0

        For I = N To 0 Step -1
            Y = Z * Y + A(I)
        Next I

    End Sub

    ''' <summary> [43300] Sine series coefficients </summary>
    Private Shared Sub SINDATA(ByRef N As Int32, ByRef A() As Decimal)
        N = 6
        A(0) = 1
        A(1) = -0.166666666667133
        A(2) = 0.00833333333809067
        A(3) = -0.000198412715551283
        A(4) = 0.0000027557589750762
        A(5) = -0.00000002507059876207
        A(6) = 0.000000000164105986683
    End Sub

    ''' <summary> [43310] Cosine series coefficients </summary>
    Private Shared Sub COSDATA(ByRef N As Int32, ByRef A() As Decimal)
        N = 6
        A(0) = 1
        A(1) = -0.4999999999982
        A(2) = 0.04166666664651
        A(3) = -0.001388888805755
        A(4) = 0.000024801428034
        A(5) = -0.0000002754213324
        A(6) = 0.0000000020189405
    End Sub

    ''' <summary> [43320] Arctangent series coefficients </summary>
    Private Shared Sub ARCDATA(ByRef N As Int32, ByRef A() As Decimal)
        N = 10
        A(0) = 1
        A(1) = -0.333333311286
        A(2) = 0.199998774421
        A(3) = -0.142831560376
        A(4) = 0.110840091104
        A(5) = -0.0892291243810001
        A(6) = 0.0703152000330001
        A(7) = -0.04927890803
        A(8) = 0.026879941561
        A(9) = -0.00956838452000001
        A(10) = 0.001605444922
    End Sub

    ''' <summary> [43340] Log base ten series coefficients </summary>
    Private Shared Sub LOGDATA(ByRef N As Int32, ByRef A() As Decimal)
        N = 9
        A(0) = 0.8685889644
        A(1) = 0.2895297117
        A(2) = 0.1737120608
        A(3) = 0.1242584742
        A(4) = 0.093908046
        A(5) = 0.1009301264
        A(6) = -0.0439630355
        A(7) = 0.3920576195
        A(8) = -0.5170494708
        A(9) = 0.4915571108
    End Sub

    ''' <summary> [43360] Natural logarithm series coefficients </summary>
    Private Shared Sub LNDATA(ByRef N As Int32, ByRef A() As Decimal)
        N = 9
        A(0) = 2
        A(1) = 0.66666672443
        A(2) = 0.3999895288
        A(3) = 0.286436047
        A(4) = 0.197959107
        A(5) = 0.628353
        A(6) = -4.54692
        A(7) = 28.117
        A(8) = -86.42
        A(9) = 106.1
    End Sub

    ''' <summary> [43380] Power of e series coefficients </summary>
    Private Shared Sub EXPDATA(ByRef N As Int32, ByRef A() As Decimal)
        N = 8
        A(0) = 1
        A(1) = 0.999999996680001
        A(2) = 0.49999995173
        A(3) = 0.16666704243
        A(4) = 0.04166685027
        A(5) = 0.00832672635
        A(6) = 0.00140836136
        A(7) = 0.00017358267
        A(8) = 0.0000393168
    End Sub

    ''' <summary> [43400] Power of ten series coefficients </summary>
    Private Shared Sub TENDATA(ByRef N As Int32, ByRef A() As Decimal)
        N = 9
        A(0) = 1
        A(1) = 1.1512925485
        A(2) = 0.662737305
        A(3) = 0.2543345675
        A(4) = 0.0732032741
        A(5) = 0.0168603036
        A(6) = 0.0032196227
        A(7) = 0.000554766
        A(8) = 0.0000573305
        A(9) = 0.0000179419
    End Sub

#End Region

End Class
