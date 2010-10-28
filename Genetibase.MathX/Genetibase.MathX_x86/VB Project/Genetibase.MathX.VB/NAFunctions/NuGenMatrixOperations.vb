Imports System.Math

''' <summary> Vector and matrix operations </summary>
Public Class NuGenMatrixOperations

    ''' <summary> Initialize a matrix from one dimensional array </summary>
    Public Shared Function InitMatrix(ByVal rowsCount As Int32, ByVal colsCount As Int32, ByVal elements As Decimal()) As Decimal(,)

        Dim i As Int32, j As Int32

        If (rowsCount * colsCount <> elements.Length) Then
            Throw New Exception("Invalid parameters!")
        End If

        Dim matrix(rowsCount, colsCount) As Decimal

        For i = 0 To rowsCount
            For j = 0 To colsCount
                matrix(i, j) = elements(i * colsCount + j)
            Next j
        Next i

        Return matrix
    End Function

#Region " VECTOR OPERATIONS (+, -, X, *)"

    ''' <summary> [41500] Vector addition subroutine (C=A+B) </summary>
    Public Shared Sub VECTADD(ByVal N As Int32, ByVal A() As Decimal, ByVal B() As Decimal, ByRef C() As Decimal)

        Dim I As Int32

        For I = 1 To N
            C(I) = A(I) + B(I)
        Next I

    End Sub

    ''' <summary> [41550] Vector substraction subroutine (C=A-B) </summary>
    Public Shared Sub VECTSUB(ByVal N As Int32, ByVal A() As Decimal, ByVal B() As Decimal, ByRef C() As Decimal)

        Dim I As Int32

        For I = 1 To N
            C(I) = A(I) - B(I)
        Next I

    End Sub

    ''' <summary> [41600] Vector dot product subroutine (C=A.B) </summary>
    Public Shared Sub VECTDOT(ByVal N As Int32, ByVal A() As Decimal, ByVal B() As Decimal, ByRef C As Decimal)

        Dim I As Int32
        C = 0

        For I = 1 To N
            C = C + A(I) * B(I)
        Next I

    End Sub

    ''' <summary> [41650] Vector cross product subroutine (C=A X B) </summary>
    Public Shared Sub VECTCURL(ByVal N As Int32, ByVal A() As Decimal, ByVal B() As Decimal, ByRef C() As Decimal)
        C(1) = A(2) * B(3) - A(3) * B(2)
        C(2) = A(3) * B(1) - A(1) * B(3)
        C(3) = A(1) * B(2) - A(2) * B(1)
    End Sub

    ''' <summary> [41700] Vector length subroutine </summary>
    Public Shared Sub VECTLEN(ByVal N As Int32, ByVal A() As Decimal, ByRef L As Decimal)

        Dim I As Int32
        L = 0

        For I = 1 To N
            L = L + A(I) * A(I)
        Next I

        L = Sqrt(L)
    End Sub

    ''' <summary> [41750] Vector angle subroutine ( angle between A and B) </summary>
    Public Shared Sub VECTANGL(ByVal N As Int32, ByVal A() As Decimal, ByVal B() As Decimal, ByRef AT As Decimal)

        Dim C As Decimal, LA As Decimal, LB As Decimal, E As Decimal
        ' FIND DOT PRODUCT
        VECTDOT(N, A, B, C) ' 41600
        ' FIND LENGTH OF A
        VECTLEN(N, A, LA) ' 41700
        ' FIND LENGTH OF B
        VECTLEN(N, B, LB) ' 41700
        E = C / (LB * LA) + (0.1) ^ 30
        E = Sqrt(1 - E * E) / E
        AT = Atan(E)

        If C < 0 Then AT = 3.1415926536 - AT
    End Sub

#End Region

#Region "MATRIX SUMS AND PRODUCTS (ELEMENTARY OPERATIONS)"

    ''' <summary> [41800] Matrix addition subroutine (C=A+B) </summary>
    Public Shared Sub MATADD(ByVal M As Int32, ByVal N As Int32, ByVal A(,) As Decimal, ByVal B(,) As Decimal, ByRef C(,) As Decimal)

        Dim I As Int32, J As Int32

        For I = 1 To M
            For J = 1 To N
                C(I, J) = A(I, J) + B(I, J)
            Next J
        Next I

    End Sub

    ''' <summary> Matrix addition function (C=A+B) </summary>
    Public Shared Function MATADD(ByVal M As Int32, ByVal N As Int32, ByVal A(,) As Decimal, ByVal B(,) As Decimal) As Decimal(,)

        Dim C(,) As Decimal
        Dim I As Int32, J As Int32

        For I = 1 To M
            For J = 1 To N
                C(I, J) = A(I, J) + B(I, J)
            Next J
        Next I

        Return C
    End Function

    ''' <summary> [41850] Matrix substraction subroutine (C=A-B) </summary>
    Public Shared Sub MATSUB(ByVal M As Int32, ByVal N As Int32, ByVal A(,) As Decimal, ByVal B(,) As Decimal, ByRef C(,) As Decimal)

        Dim I As Int32, J As Int32

        For I = 1 To M
            For J = 1 To N
                C(I, J) = A(I, J) - B(I, J)
            Next J
        Next I

    End Sub

    ''' <summary> [41900] Matrix multiplication subroutine (C=A X B) </summary>
    Public Shared Sub MATMULT(ByVal M1 As Int32, ByVal N1 As Int32, ByVal N2 As Int32, ByVal A(,) As Decimal, ByVal B(,) As Decimal, ByRef C(,) As Decimal)

        Dim I As Int32, J As Int32, K As Int32

        For I = 1 To M1
            For J = 1 To N2
                C(I, J) = 0

                For K = 1 To N1
                    C(I, J) = C(I, J) + A(I, K) * B(K, J)
                Next K
            Next J
        Next I

    End Sub

    ''' <summary> [41950] Matrix transpose subroutine (B=transpose(A)) </summary>
    Public Shared Sub MATTRANS(ByVal M As Int32, ByVal N As Int32, ByVal A(,) As Decimal, ByRef B(,) As Decimal)

        Dim I As Int32, J As Int32

        For I = 1 To N
            For J = 1 To M
                B(I, J) = A(J, I)
            Next J
        Next I

    End Sub

    ''' <summary> [42000] Diagonal matrix creation subroutine (matrix A(i,j) is the identity matrix times B) </summary>
    Public Shared Sub MATDIAG(ByVal N As Int32, ByVal B As Decimal, ByRef A(,) As Decimal)

        Dim I As Int32, J As Int32

        For I = 1 To N
            For J = 1 To N
                A(I, J) = 0

                If I = J Then A(I, J) = B
            Next J
        Next I

    End Sub

#End Region

#Region "OTHER MATRIX OPERATIONS (MATRIX CLEARING, DIAGONAL MATRIX CREATION, ROW MANIPULATIONS)"

    ''' <summary> [42100] Matrix save subroutine
    ''' N1, N2 are input indices
    ''' </summary>
    Public Shared Sub MATSAV(ByVal N1 As Int32, ByVal N2 As Int32, ByVal sourceT(,) As Decimal, ByRef destT(,) As Decimal)

        Dim I1 As Int32, I2 As Int32

        For I1 = 1 To N1
            For I2 = 1 To N2
                destT(I1, I2) = sourceT(I1, I2)
            Next I2
        Next I1

    End Sub

    ''' <summary> [42200] Scalar B X matrix A subroutine
    ''' N1, N2 and N3 are input indices
    ''' </summary>
    Public Shared Sub MATSCALE(ByVal N1 As Int32, ByVal N2 As Int32, ByVal B As Decimal, ByRef A(,) As Decimal)

        Dim I1 As Int32, I2 As Int32

        For I1 = 1 To N1
            For I2 = 1 To N2
                A(I1, I2) = B * A(I1, I2)
            Next I2
        Next I1

    End Sub

    ''' <summary> [42225] Matrix A clear subroutine
    ''' N1, N2 and N3 are input indices
    ''' use array.Clear() instead
    ''' </summary>

    ''' <summary> [42250] Row switching subroutine
    ''' Rows N1 and N2 are interchanged
    ''' </summary>
    Public Shared Sub MATSWCH(ByVal N As Int32, ByVal N1 As Int32, ByVal N2 As Int32, ByRef A(,) As Decimal)

        Dim J As Int32, B As Decimal

        For J = 1 To N
            B = A(N1, J)
            A(N1, J) = A(N2, J)
            A(N2, J) = B

        Next J

    End Sub

    ''' <summary> [42275] Row multiplication/add subroutine 
    ''' B times row N1 added to N2
    ''' </summary>
    Public Shared Sub MATRMAD(ByVal N As Int32, ByVal N1 As Int32, ByVal N2 As Int32, ByVal B As Decimal, ByRef A(,) As Decimal)

        Dim J As Int32

        For J = 1 To N
            A(N2, J) = A(N2, J) + B * A(N1, J)
        Next J

    End Sub

    ''' <summary> [42300] Cofactor K subroutine
    ''' Input matrix size is N X N
    ''' Matrix A(i,j) IN, matrix B(i,j) OUT
    ''' </summary>
    Public Shared Sub MATCOFAT(ByVal N As Int32, ByVal K As Int32, ByVal A(,) As Decimal, ByRef B(,) As Decimal)

        Dim I As Int32, J As Int32

        ' FIRST SHIFT UP ONE ROW
        For I = 2 To N
            For J = 1 To N
                B(I - 1, J) = A(I, J)
            Next J
        Next I

        For I = 1 To N - 1
            For J = K To N

                If K = N Then '??????

                Else
                    B(I, J) = B(I, J + 1)
                End If

            Next J
        Next I

    End Sub

#End Region

#Region "DETERMINANTS (TO RANK 4)"

    ''' <summary> [42350] Matrix determinant subroutine 
    ''' Finds determinant for up to a 4 X 4 matrix
    ''' </summary>
    Public Shared Function MATDET(ByVal N As Int32, ByVal A(,) As Decimal) As Decimal

        Dim D As Decimal, D1 As Decimal, K As Int32, B(N + 1, N + 1) As Decimal

        If N >= 5 Then Return 0
        If N < 2 Then
            ' ********************
            ' FIRST ORDER DETERMINANT
            D = A(1, 1)
            Return D
        End If

        If N < 3 Then
            ' ********************
            ' SECOND ORDER DETERMINANT
            D = A(1, 1) * A(2, 2) - A(1, 2) * A(2, 1)
            Return D
        End If

        If N < 4 Then
            ' ********************
            ' THIRD ORDER DETERMINANT
            D = ThirdOrderDet(A)
            Return D
        End If

        ' ********************
        ' FOURTH ORDER DETERMINANT
        ' D1 WILL BE THE DETERMINANT
        D1 = 0

        ' FIND DETERMINANT OF EACH COFACTOR
        For K = 1 To 4
            ' GET COFACTOR K
            MATCOFAT(N, K, A, B) ' 42300
            ' COFACTOR RETURNED IN B
            ' GET DET(B)
            D = ThirdOrderDet(B) ' 42363
            D1 = D1 + A(1, K) * D
            ' REVERSE SIGN FOR NEXT COFACTOR
            D1 = -D1
        Next K

        D = D1
        Return D
    End Function

    ''' <summary> [42363] Third order determinant function </summary>
    Private Shared Function ThirdOrderDet(ByVal A(,) As Decimal) As Decimal

        Dim D As Decimal
        D = A(1, 1) * (A(2, 2) * A(3, 3) - A(2, 3) * A(3, 2))
        D = D - A(1, 2) * (A(2, 1) * A(3, 3) - A(2, 3) * A(3, 1))
        D = D + A(1, 3) * (A(2, 1) * A(3, 2) - A(2, 2) * A(3, 1))
        Return D
    End Function

#End Region

#Region "MATRIX INVERSION"

    ''' <summary> Matrix inversion subroutine (Gauss-Jordan elimination)
    ''' Matrix A is input, matrix B is output
    ''' Dim A=N X N temporary Dim B=N X 2N
    ''' First create matrix with A on the left and I on the right
    ''' </summary>
    Public Shared Sub MATINV(ByVal N As Int32, ByVal A(,) As Decimal, ByRef B(,) As Decimal)

        Dim I As Int32, J As Int32, K As Int32, MaxI As Int32
        Dim BTemp(N, 2 * N) As Decimal
        Dim BT As Decimal

        'FIRST CREATE MATRIX WITH A ON THE LEFT AND I ON THE RIGHT
        For I = 1 To N
            For J = 1 To N
                BTemp(I, J + N) = 0
                BTemp(I, J) = A(I, J)
            Next J

            BTemp(I, I + N) = 1
        Next I

        'PERFORM ROW ORIENTED OPERATIONS TO CONVERT THE LEFT HAND
        'SIDE OF B TO THE IDENTITY MATRIX. THE INVERSE OF A WILL
        'THEN BE ON THE RIGHT.
        For K = 1 To N

            If K = N Then ' GoTo 42424

            Else
                MaxI = K

                'FIND MAXIMUM ELEMENT
                For I = K + 1 To N

                    If Abs(BTemp(I, K)) > Abs(BTemp(MaxI, K)) Then MaxI = I
                Next I

                If MaxI = K Then 'GoTo 42424

                Else

                    For J = K To 2 * N
                        BT = BTemp(K, J)
                        BTemp(K, J) = BTemp(MaxI, J)
                        BTemp(MaxI, J) = BT
                    Next J

                End If
            End If

            ' DIVIDE ROW K [label 42424:]
            For J = K + 1 To 2 * N
                BTemp(K, J) = BTemp(K, J) / BTemp(K, K)
            Next J

            If K <> 1 Then 'else GoTo 42434

                For I = 1 To K - 1
                    For J = K + 1 To 2 * N
                        BTemp(I, J) = BTemp(I, J) - BTemp(I, K) * BTemp(K, J)
                    Next J
                Next I

            End If

            'what happens if N =1
            If K = 1 Or K <> N Then 'else GoTo 42441

                '[42434:]
                For I = K + 1 To N
                    For J = K + 1 To 2 * N
                        BTemp(I, J) = BTemp(I, J) - BTemp(I, K) * BTemp(K, J)
                    Next J
                Next I

            End If

            'For I = 1 To N
            '    If I <> K Then
            '        For J = K + 1 To 2 * N
            '            BTemp(I, J) = BTemp(I, J) - BTemp(I, K) * BTemp(K, J)
            '        Next J
            '    End If
            'Next I
        Next K

        'RETRIEVE INVERSE FROM THE RIGHT SIDE OF B
        '[42441:]
        For I = 1 To N
            For J = 1 To N
                B(I, J) = BTemp(I, J + N)
            Next J
        Next I

    End Sub

#End Region

#Region "EIGENVALUE"

    ''' <summary> [42700] Eigenvalue (power method) subroutine (AX=LX)
    ''' A is the N X N matrix
    ''' B is an arbitrary vector
    ''' E is the relative error chosen
    ''' D=count of the number of iterations
    ''' </summary>
    Public Shared Sub EIGENPOW(ByVal N As Int32, ByVal A(,) As Decimal, ByVal E As Decimal, ByVal D1 As Int32, ByRef B(,) As Decimal, ByRef AT As Decimal, ByRef D As Int32)

        Dim I As Int32
        Dim C(N + 1, N + 1) As Decimal, BT As Decimal

        ' GENERATE ARBITRARY NORMALIZED VECTOR B(I,1)
        For I = 1 To N
            B(I, 1) = 1 / Sqrt(N)
        Next I

        ' B = LAST EIGENVALUE ESTIMATE
        ' A = CURRENT EIGENVALUE ESTIMATE
        ' PICK AN INITIAL VALUE FOR THE EIGENVALUE GUESS
        BT = 1
        D = 0

        ' START ITERATION
        Do
            AT = 0
            MATMULT(N, N, 1, A, B, C) ' 41900

            ' CONVERT C OUTPUT TO B
            For I = 1 To N
                B(I, 1) = C(I, 1)
                AT = AT + B(I, 1) * B(I, 1)
            Next I

            D = D + 1
            AT = Sqrt(AT)

            ' NORMALIZE VECTOR
            For I = 1 To N
                B(I, 1) = B(I, 1) / AT
            Next I

            If Abs((AT - BT) / AT) < E Then Return
            BT = AT

            If D > D1 Then Return
        Loop

    End Sub

#End Region

#Region "MATRIX EXPONENTIATION"

    ''' <summary> [42800] Matrix exponent subroutine
    ''' Inputs to the subroutine are the matrix A, matrix C, size N, number of terms K2 and variable X
    ''' </summary>
    Public Shared Sub MATEXP(ByVal N As Int32, ByVal A(,) As Decimal, ByVal K2 As Int32, ByVal X As Decimal, ByRef C(,) As Decimal)

        Dim I As Int32, J As Int32, K1 As Int32
        Dim BT As Decimal
        Dim B(N + 1, N + 1) As Decimal, D(N + 1, N + 1) As Decimal, E(N + 1, N + 1) As Decimal

        ' GUARD AGAINST DIVIDE BY ZERO
        If X = 0 Then X = 0.0000000000001

        ' INITIALIZE STORAGE MATRIX D(I,J)
        For I = 1 To N
            For J = 1 To N
                D(I, J) = 0
            Next J
        Next I

        ' K2 IS THE NUMBER OF TERMS TO BE CALCULATED
        K1 = 0
        ' CREATE IDENTITY MATRIX IN B
        MATDIAG(N, 1, B) '42000
        'MOVE B TO C
        MATSAV(N, N, B, C) ' 42100
        ' ADD C TO D
        MATADD(N, N, D, C, D)

        Dim TestCond As Boolean = False

        Do
            K1 = K1 + 1

            If K1 >= K2 Then
                TestCond = True

            Else
                ' SCALE MATRIX A BY X/K1
                BT = X / K1
                MATSCALE(N, N, BT, A) ' 42200
                ' MULTIPLY A TIMES B
                MATMULT(N, N, N, A, B, C) ' 41900
                ' ADD RESULT TO MATRIX D
                MATADD(N, N, D, C, D)
                ' MOVE C TO B
                MATSAV(N, N, C, B) ' 42100
                ' RETURN MATRIX A TO ORIGINAL CONDITION
                BT = K1 / X
                MATSCALE(N, N, BT, A) ' 42200
            End If

        Loop Until TestCond = True

        ' CONTINUE SUMMATION
        ' MOVE RESULT IN D TO C
        MATSAV(N, N, D, C) ' 42100
        ' RETURN TO CALLING PROGRAM
    End Sub

#End Region

End Class
