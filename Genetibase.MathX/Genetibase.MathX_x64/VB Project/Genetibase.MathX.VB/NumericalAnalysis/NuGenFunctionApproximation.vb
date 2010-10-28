Imports System.Math

Namespace NumericalAnalysis

    ''' <summary> Function Aproximations </summary>
    Public Class NuGenFunctionApproximation

        ''' <summary> Matrix multiplication subroutine 
        ''' C=A X B   A is M1 by N1   B is M2 by N2   C is M1 by N2
        ''' N1 = M2
        ''' </summary>
        Public Shared Sub MATMULT(ByVal M1 As Int32, ByVal N1 As Int32, ByVal N2 As Int32, ByVal A(,) As Decimal, ByVal B(,) As Decimal, ByRef C(,) As Decimal)

            Dim I As Int32
            Dim J As Int32
            Dim K As Int32

            For I = 1 To M1
                For J = 1 To N2
                    C(I, J) = 0

                    For K = 1 To N1
                        C(I, J) = C(I, J) + A(I, K) * B(K, J)
                    Next K
                Next J
            Next I

        End Sub

        ''' <summary> Matrix transpose subroutine 
        ''' B=TRANSPOSE(A)
        ''' </summary>
        Public Shared Sub MATTRANS(ByVal M As Int32, ByVal N As Int32, ByVal SourceT(,) As Decimal, ByRef TransposeT(,) As Decimal)

            Dim I As Int32
            Dim J As Int32

            For I = 1 To N
                For J = 1 To M
                    TransposeT(I, J) = SourceT(J, I)
                Next J
            Next I

        End Sub

        ''' <summary> Linear least squares subroutine
        ''' The input data set is (X(M),Y(M)).
        ''' The number of data points is N.
        ''' The number of different points must be greater than one.
        ''' X(M) and Y(M) must be dimensioned in the calling program.
        ''' The subroutine also calculates the unbiased estimate
        ''' of the standard deviation, D.
        ''' The returned parameters are A,B and D.
        ''' </summary>
        Public Shared Sub LSTSQR1(ByVal N As Int32, ByVal X() As Decimal, ByVal Y() As Decimal, ByRef A As Decimal, ByRef B As Decimal, ByRef D As Decimal)

            Dim A1 As Decimal = 0
            Dim A2 As Decimal = 0
            Dim B0 As Decimal = 0
            Dim B1 As Decimal = 0
            Dim D1 As Decimal
            Dim M As Int32

            For M = 0 To N - 1
                A1 = A1 + X(M)
                A2 = A2 + X(M) * X(M)
                B0 = B0 + Y(M)
                B1 = B1 + Y(M) * X(M)
            Next M

            A1 = A1 / N
            A2 = A2 / N
            B0 = B0 / N
            B1 = B1 / N
            D = A1 * A1 - A2
            A = A1 * B1 - A2 * B0
            A = A / D
            B = A1 * B0 - B1
            B = B / D
            '********************
            'EVALUATION OF STANDARD DEVIATION (UNBIASED ESTIMATE)
            D = 0

            For M = 0 To N - 1
                D1 = Y(M) - A - B * X(M)
                D = D + D1 * D1
            Next M

            D = Sqrt(D / (N - 2))
        End Sub

        ''' <summary> Parabolic least squares subroutine 
        ''' The input data set is (X(M),Y(M)).
        ''' The number of data points is N.
        ''' The number of different points must be greater than three.
        ''' X(M) and Y(M) must be dimensioned in the calling program.
        ''' The subroutine also calculates the unbiased estimate
        ''' of the standard deviation, D.
        ''' The returned parameters are A,B,C and D.
        ''' </summary>
        Public Shared Sub LSTSQR2(ByVal N As Int32, ByVal X() As Decimal, ByVal Y() As Decimal, ByRef A As Decimal, ByRef B As Decimal, ByRef C As Decimal, ByRef D As Decimal)

            Dim A0 As Decimal = 1
            Dim A1 As Decimal = 0
            Dim A2 As Decimal = 0
            Dim A3 As Decimal = 0
            Dim A4 As Decimal = 0
            Dim B0 As Decimal = 0
            Dim B1 As Decimal = 0
            Dim B2 As Decimal = 0
            Dim D1 As Decimal
            Dim M As Int32

            For M = 0 To N - 1
                A1 = A1 + X(M)
                A2 = A2 + X(M) * X(M)
                A3 = A3 + X(M) * X(M) * X(M)
                A4 = A4 + X(M) * X(M) * X(M) * X(M)
                B0 = B0 + Y(M)
                B1 = B1 + Y(M) * X(M)
                B2 = B2 + Y(M) * X(M) * X(M)
            Next M

            A1 = A1 / N
            A2 = A2 / N
            A3 = A3 / N
            A4 = A4 / N
            B0 = B0 / N
            B1 = B1 / N
            B2 = B2 / N
            D = A0 * (A2 * A4 - A3 * A3) - A1 * (A1 * A4 - A3 * A2) + A2 * (A1 * A3 - A2 * A2)
            A = B0 * (A2 * A4 - A3 * A3) + B1 * (A3 * A2 - A1 * A4) + B2 * (A1 * A3 - A2 * A2)
            A = A / D
            B = B0 * (A3 * A2 - A1 * A4) + B1 * (A0 * A4 - A2 * A2) + B2 * (A2 * A1 - A0 * A3)
            B = B / D
            C = B0 * (A1 * A3 - A2 * A2) + B1 * (A1 * A2 - A0 * A3) + B2 * (A0 * A2 - A1 * A1)
            C = C / D
            '********************
            'EVALUATION OF STANDARD DEVIATION (UNBIASED ESTIMATE)
            D = 0

            For M = 0 To N - 1
                D1 = Y(M) - A - B * X(M) - C * X(M) * X(M)
                D = D + D1 * D1
            Next M

            D = Sqrt(D / (N - 3))
        End Sub

        ''' <summary> Coefficient matrix generation subroutine for the one dimensional polynomial regression 
        ''' The input data set consists of N pairs of
        ''' (X(I),Y(I)) values.
        ''' The regression order is N.
        ''' The matrix returned, Z, is M rows by N+1 columns.
        ''' Dimension this matrix in the calling program.
        ''' Recall that the regression routine input will use N+1,.
        ''' You must set N to N+1 before entering it.
        ''' </summary>
        Public Shared Sub POLYCM(ByVal M As Int32, ByVal N As Int32, ByVal X() As Decimal, ByRef Z(,) As Decimal)

            Dim I As Int32
            Dim J As Int32
            Dim B As Decimal

            For I = 1 To M
                B = 1

                For J = 1 To N + 1
                    'Z IS M ROWS BY N+1 COLUMNS
                    Z(I, J) = B
                    B = B * X(I)
                Next J
            Next I

        End Sub

        ''' <summary> Least squares fitting subroutine 
        ''' General subroutine for multidimensional,
        ''' nonlinear regression.
        ''' The equation fitted has the form
        ''' Y=D(1)X1 + D(2)X2 +  ... + D(N)XN
        ''' Change in notation is due to a dimension conflict.
        ''' The coefficients are returned by the program in D(I).
        ''' The XI can be simple powers of X, or functions.
        ''' Note that the XI are assumed to be independent.
        ''' The measured responses are Y(I)- there are m of them.
        ''' Y is M row column vector, and Z(I,J) is a
        ''' M row by N column matrix.
        ''' M>=N
        ''' The subroutine inputs are Y(I), Z(I,J), M and N.
        ''' The working matrices within the program are A(I,J),
        ''' B(I,J) and C(I,J).
        ''' The subroutine calls several other matrix operation
        ''' routines to perform the calculation.
        ''' Dimension A,B,C,Y and Z in the calling program.
        ''' </summary>
        Public Shared Sub LEASTSQR(ByVal M As Int32, ByVal N As Int32, ByVal Y() As Decimal, ByVal Z(,) As Decimal, ByRef A(,) As Decimal, ByRef B(,) As Decimal, ByRef C(,) As Decimal, ByRef D() As Decimal)

            Dim I As Int32
            Dim J As Int32
            Dim TransposeZ(N, M) As Decimal
            Dim MultipleZTZ(N, N) As Decimal
            Dim Inverse(N, N) As Decimal
            Dim Product(N, M) As Decimal
            Dim ColVector(M, 1) As Decimal
            Dim ResultT(N, 1) As Decimal

            'FIND TRANSPOSE OF Z AND PUT RESULT IN TransposeZ
            MATTRANS(M, N, Z, TransposeZ)

            'MULTIPLY TransposeZ AND Z
            'Z IS M BY N => TransposeZ IS N BY M
            MATMULT(N, M, N, TransposeZ, Z, MultipleZTZ)
            'RESULT IS IN MultipleZTZ, AN N BY N MATRIX.. 

            'THEN FIND INVERSE
            NuGenMatrixOperations.MATINV(N, MultipleZTZ, Inverse)

            'MULTIPLY INVERSE AND Z(TRANSPOSE)
            'Inverse IS N BY N, TransposeZ IS N BY M
            MATMULT(N, N, M, Inverse, TransposeZ, Product)
            'PRODUCT IS N BY M

            'MULTIPLY PRODUCT AND ColVector
            'ColVector IS A COLUMN VECTOR (M BY 1)
            For I = 1 To M
                ColVector(I, 1) = Y(I)
            Next I

            'PRODUCT IS N BY M.   ColVector IS M BY 1
            MATMULT(N, M, 1, Product, ColVector, ResultT)
            'ResultT is N BY 1

            'REGRESSION COEFFICIENTS ARE IN ResultT(I,1)
            'MOVE THEM TO D(I)
            For I = 1 To N
                D(I) = ResultT(I, 1)
            Next I

        End Sub

        ''' <summary> Standard deviation subroutine 
        ''' This subroutine calculates the standard
        ''' deviation for a polynomial fit.
        ''' The inputs are the number of data points, M,
        ''' The degree of the fit, N,
        ''' The polynomial coefficients, D(I),
        ''' The original data set, X(I), Y(I).
        ''' </summary>
        Public Shared Sub Sigma(ByVal M As Int32, ByVal N As Int32, ByVal D() As Decimal, ByVal X() As Decimal, ByVal Y() As Decimal, ByRef SD As Decimal)
            SD = 0

            Dim I As Int32
            Dim J As Int32
            Dim YT As Decimal
            Dim B As Decimal

            For I = 1 To M
                YT = 0
                B = 1

                For J = 1 To N + 1
                    YT = YT + D(J) * B
                    B = B * X(I)
                Next J

                SD = SD + (YT - Y(I)) * (YT - Y(I))
            Next I

            If M - N - 1 > 0 Then
                SD = SD / (M - N - 1)
                SD = Sqrt(SD)

            Else ' in this case all the calculation before is for nothing
                SD = 0
            End If

        End Sub

        ''' <summary> Coefficient matrix generation subroutine for multiple nonlinear regression 
        ''' Also calculates the standard deviation, D, even
        ''' though there is some redundant computing.
        ''' The maximum number of dimensions is 9.
        ''' The input data set consists of m data sets of the form
        '''      Y(I), X(I,1), X(I,2),......, X(I,L)
        ''' The number of dimensions is L.
        ''' The order of the fit to each dimension is M(J).
        ''' The result is an (M1+1)*(M2+1)...*(ML+1)+1
        ''' column by M row matrix, Z.
        ''' This matrix is arranged as follows ( example- L=2, M(1)=2, M(2)=2)
        '''   1  X1  X1*X1  X2  X2*X1  X2*X1*X1  X2*X2  X2*X2*X1  X2*X2*X1*X1
        ''' This matrix should be dimensioned in the calling program
        ''' as should also the X(A,J) matrix of data values.
        ''' Calculate the total number of dimensions
        ''' </summary>
        Public Shared Sub MLTNLREG(ByVal L As Int32, ByVal MI As Int32, ByVal M() As Int32, ByVal X(,) As Decimal, ByVal Y() As Decimal, ByRef D() As Decimal, ByRef Z(,) As Decimal, ByRef SD As Decimal)

            Dim I As Int32
            Dim K As Int32
            Dim N As Int32
            Dim J As Int32
            Dim B As Decimal
            Dim YT As Decimal

            N = 1

            For I = 1 To L
                N = N * (M(I) + 1)
            Next I

            SD = 0

            For I = 1 To MI

                'BRANCH ACCORDING TO DIMENSION
                'RETURN IF DIMENSION IS GREATER THAN 9
                If L <= 0 Then
                    L = 0
                    Return
                End If

                If L > 9 Then
                    L = 0
                    Return
                End If

                J = 0
                B = 1

                SubSetCalculation(L, I, Z, X, M, J, B)

                'ARRAY GENERATED FOR ROW I
                YT = 0

                For K = 1 To N
                    YT = YT + D(K) * Z(I, K)
                Next K

                SD = SD + (Y(I) - YT) * (Y(I) - YT)
            Next I

            'CALCULATE STANDARD DEVIATION
            If MI - N > 0 Then '43836
                SD = SD / (MI - N)
                SD = Sqrt(SD)

            Else
                SD = 0
            End If

        End Sub

        Private Shared Sub SubSetCalculation(ByVal L As Int32, ByVal I As Int32, ByRef Z(,) As Decimal, ByRef X(,) As Decimal, ByVal M() As Int32, ByRef J As Int32, ByRef B As Decimal)

            Dim I1 As Int32
            Dim C As Decimal

            C = B

            For I1 = 0 To M(L)

                If L = 1 Then
                    J = J + 1
                    Z(I, J) = B

                Else

                    SubSetCalculation(L - 1, I, Z, X, M, J, B)
                End If

                B = B * X(I, L)
            Next I1

            B = C
        End Sub

        ''' <summary> Least squares polynomial fitting subroutine 
        ''' This program least squares fits a polynomial to input data.
        ''' forsythe orthogonal polynomials are used in the fitting.
        ''' The number of data points is N.
        ''' The data is input to the subroutine in X(I),Y(I) pairs.
        ''' The coefficients are returned in C(I).
        ''' The smoothed data is returned in V(I).
        ''' The order of the fit is specified by M.
        ''' The standard deviation of the fit is returned in D.
        ''' There are two options available by use of the parameter E.
        ''' If E=0 the fit is to order M.
        ''' If E>0 the order of fit increases towards M, but 
        ''' will stop if the relative standard deviation does not 
        ''' decrease by more than E between successive fits.
        ''' The order of the fit then obtained is L.
        ''' The arrays X,Y,V,A,B,C,C2,D,E and F Must be dimensioned.
        ''' A(I) and B(I) are simply work arrays
        ''' </summary>
        Public Shared Sub LSQRPOLY(ByVal M As Int32, ByVal Er As Decimal, ByVal N As Int32, ByVal X() As Decimal, ByVal Y() As Decimal, ByRef C() As Decimal, ByRef DT As Decimal, ByRef L As Int32)

            Dim I As Int32
            Dim N1 As Int32 = M + 1
            Dim V1 As Decimal = 10000000.0#
            Dim A(N1) As Decimal
            Dim B(N1) As Decimal
            Dim F(N1) As Decimal
            Dim V(N) As Decimal
            Dim D(N) As Decimal
            Dim E(N) As Decimal
            Dim C2(N1) As Decimal '

            Dim W As Decimal, A1 As Decimal, B1 As Decimal, C1 As Decimal, D1 As Decimal, F1 As Decimal
            Dim A2 As Decimal, B2 As Decimal, E2 As Decimal, F2 As Decimal, L2 As Decimal, V2 As Decimal, VT As Decimal

            'INITIALIZE THE ARRAYS
            For I = 1 To N1
                A(I) = 0
                B(I) = 0
                F(I) = 0
            Next I

            For I = 1 To N
                V(I) = 0
                D(I) = 0
            Next I

            D1 = Sqrt(N)
            W = D1

            For I = 1 To N
                E(I) = 1 / W
            Next I

            F1 = D1
            A1 = 0

            For I = 1 To N
                A1 = A1 + X(I) * E(I) * E(I)
            Next I

            C1 = 0

            For I = 1 To N
                C1 = C1 + Y(I) * E(I)
            Next I

            B(1) = 1 / F1
            F(1) = B(1) * C1

            For I = 1 To N
                V(I) = V(I) + E(I) * C1
            Next I

            M = 1

            Dim TestCond As Boolean = False

            Do

                'SAVE LATEST RESULTS
                For I = 1 To L
                    C2(I) = C(I)
                Next I

                L2 = L
                V2 = VT
                F2 = F1
                A2 = A1
                F1 = 0

                For I = 1 To N
                    B1 = E(I)
                    E(I) = (X(I) - A2) * E(I) - F2 * D(I)
                    D(I) = B1
                    F1 = F1 + E(I) * E(I)
                Next I

                F1 = Sqrt(F1)

                For I = 1 To N
                    E(I) = E(I) / F1
                Next I

                A1 = 0

                For I = 1 To N
                    A1 = A1 + X(I) * E(I) * E(I)
                Next I

                C1 = 0

                For I = 1 To N
                    C1 = C1 + E(I) * Y(I)
                Next I

                M = M + 1
                I = 0

                Do
                    L = M - I
                    B2 = B(L)
                    D1 = 0

                    If L > 1 Then D1 = B(L - 1)
                    D1 = D1 - A2 * B(L) - F2 * A(L)
                    B(L) = D1 / F1
                    A(L) = B2
                    I = I + 1
                Loop While (I <> M)

                For I = 1 To N
                    V(I) = V(I) + E(I) * C1
                Next I

                For I = 1 To N1
                    F(I) = F(I) + B(I) * C1
                    C(I) = F(I)
                Next I

                VT = 0

                For I = 1 To N
                    VT = VT + (V(I) - Y(I)) * (V(I) - Y(I))
                Next I

                ' NOTE THE DIVISION IS BY THE NUMBER OF DEGREES OF FREEDOM
                VT = Sqrt(VT / (N - L - 1)) ' L here is allways = 1
                L = M

                If Er <> 0 Then

                    'TEST FOR MIMIMAL IMPROVEMENT OR 'IF ERROR IS LARGER, QUIT
                    If (Abs(V1 - VT) / VT < Er) Or (Er * VT > Er * V1) Then
                        'SEQUENCE HAS BEEN ABORTED
                        'RECOVER LAST VALUES
                        L = L2
                        VT = V2

                        For I = 1 To L
                            C(I) = C2(I)
                        Next I

                        TestCond = True

                    Else
                        V1 = VT
                    End If
                End If

                If M = N1 Then TestCond = True
            Loop Until TestCond = True

            'SHIFT THE C(I) DOWN SO C(0) IS THE CONSTANT TERM
            For I = 1 To L
                C(I - 1) = C(I)
            Next I

            C(L) = 0
            'L IS THE ORDER OF THE POLYNOMIAL FITTED
            L = L - 1
            DT = VT
        End Sub

        ''' <summary> Multi-dimensional polynomial regression iteration subroutine 
        ''' This program supervises the calling of several
        ''' other subroutines in order to iteratively
        ''' fit least squares polynomials in more than
        ''' one dimension.
        ''' The program repeatedly calculates improved coefficients
        ''' until the standard deviation is no longer reduced.
        ''' The inputs to the subroutine are the number of
        ''' dimensions, L, the degree of fit for each
        ''' dimension, M(I), and the input data, X(I,J) and Y(I).
        ''' The coefficients are returned in D(I), with the
        ''' standard deviation in D.
        ''' Also returned is the number of iterations tried, L1.
        ''' The original Y(I) values are saved in Y1(I).
        ''' The current coefficients are stored in D1(I).
        ''' </summary>
        Public Shared Sub REGITER(ByVal L As Int32, ByVal M As Int32, ByVal N As Int32, ByVal ML() As Int32, _
            ByVal X(,) As Decimal, ByVal Y As Decimal(), ByRef D() As Decimal, ByRef SD As Decimal, ByRef L1 As Int32)

            Dim I As Int32, J As Int32, K As Int32
            Dim Z(M, N) As Decimal, Y1(M) As Decimal, D1(N) As Decimal, A(M, M) As Decimal, B(M, 2 * M) As Decimal, C(M, M) As Decimal
            Dim D1T As Decimal, BT As Decimal, YT As Decimal

            L1 = 0

            'SAVE Y(I)
            For I = 1 To M
                Y1(I) = Y(I)
            Next I

            'ZERO D1(I)
            For I = 1 To N
                D1(I) = 0
            Next I

            'SET THE INITIAL STANDARD DEVIATION HIGH
            D1T = 10000000.0#

            'GO TO COEFFICIENTS SUBROUTINE
            Dim TestCond As Boolean = False

            Do
                MLTNLREG(L, M, ML, X, Y, D, Z, SD) '[43800]
                'GO TO REGRESSION SUBROUTINE
                LEASTSQR(M, N, Y, Z, A, B, C, D) '[43650]
                'GET STANDARD DEVIATION
                MLTNLREG(L, M, ML, X, Y, D, Z, SD) '[43800]

                'IF STANDARD DEVIATION IS DECREASING, CONTINUE
                If D1T > SD Then
                    'SAVE THE STANDARD DEVIATION
                    D1T = SD
                    L1 = L1 + 1

                    'AUGMENT COEFFICIENT MATRIX
                    For I = 1 To N
                        D(I) = D1(I) + D(I)
                        D1(I) = D(I)
                    Next I

                    'RESTORE Y(I)
                    For I = 1 To M
                        Y(I) = Y1(I)
                    Next I

                    'REDUCE Y(I) ACCORDING TO THE D(I)
                    For I = 1 To M
                        J = 0
                        BT = 1

                        SubSetCalculation(L, I, Z, X, ML, J, BT)
                        'ARRAY GENERATED FOR ROW I
                        YT = 0

                        For K = 1 To N
                            YT = YT + D(K) * Z(I, K)
                        Next K

                        Y(I) = Y(I) - YT
                    Next I

                    'WE NOW HAVE A SET OF ERROR VALUES
                Else
                    TestCond = True
                End If

            Loop Until TestCond = True

            '*********
            'TERMINATE ITERATION
            For I = 1 To N
                D(I) = D1(I)
            Next I

            'RESTORE Y(I)
            For I = 1 To M
                Y(I) = Y1(I)
            Next I

            'GET THE FINAL STANDARD DEVIATION
            MLTNLREG(L, M, ML, X, Y, D, Z, SD) ' 43800
        End Sub

        ''' <summary> Parametric least squares curve fit subroutine 
        ''' This program least squares fits a function to a set of
        ''' data values by successively (parameter by parameter) reducing the variance.
        ''' Convergence depends on the initial values.- convergence is not assured.
        ''' N pairs of data values, (X(I),Y(I)), are given.
        ''' There are L parameters, A(J), to be optimized across.
        ''' Required are initial values for the parameter A(L) and E.
        ''' Another important parameter which affects stability is E1,
        ''' Which is initially converted to E1(L) for the first intervals.
        ''' the parameters are multiplied by (1-E1(I)) on each pass.
        ''' Dimension X(I),Y(I),A(I) and E1(I) in the calling program.
        ''' </summary>
        Public Shared Sub PARAFIT(ByVal N As Int32, ByVal L As Int32, ByVal Er1 As Decimal, ByVal E As Decimal, ByRef X() As Int32, ByRef Y() As Decimal, ByRef A() As Decimal, ByRef D As Decimal, ByRef M As Int32)

            Dim I As Int32
            Dim J As Int32
            Dim A0 As Decimal, L2 As Decimal
            Dim M1 As Decimal, M0 As Decimal
            Dim E1(L) As Decimal

            For I = 1 To L
                E1(I) = Er1
            Next I

            M = 0

            'SET UP TEST RESIDUAL
            Dim L1 As Decimal = 1000000.0!

            'MAKE SWEEP THROUGH ALL PARAMETERS
            Do

                For I = 1 To L '[44257]
                    A0 = A(I)
                    'GET VALUE OF RESIDUAL
                    A(I) = A0

                    Do '[44261]
                        _44286(L, N, X, Y, A, D, L2)
                        'STORE RESULT IN M0
                        M0 = L2
                        'REPEAT FOR M1
                        A(I) = A0 * (1 - E1(I))
                        _44286(L, N, X, Y, A, D, L2)
                        M1 = L2

                        'CHANGE INTERVAL SIZE IF CALLED FOR
                        'IF VARIANCE WAS INCREASED, HALVE E1(I)
                        If M1 > M0 Then E1(I) = -E1(I) / 2

                        'IF VARIANCE WAS REDUCED, INCREASE STEP SIZE BY INCREASING E1(I)
                        If M1 < M0 Then E1(I) = 1.2 * E1(I)

                        'IF VARIANCE HAS INCREASED, TRY TO REDUCE IT
                        If M1 > M0 Then A(I) = A0
                    Loop While M1 > M0 'If M1 > M0 Then GoTo 44261

                Next I

                'END OF A COMPLETE PASS
                'TEST FOR CONVERGENCE
                M = M + 1

                If L2 = 0 Then Return
                If Abs((L1 - L2) / L2) < E Then Return
                'IF THIS POINT IS REACHED, ANOTHER PASS IS CALLED FOR
                L1 = L2
            Loop ' 44257

        End Sub

        ''' <summary> Residual generation subroutine </summary>
        Private Shared Sub _44286(ByVal L As Int32, ByVal N As Int32, ByRef X() As Int32, ByRef Y() As Decimal, ByRef A() As Decimal, ByRef D As Decimal, ByRef L2 As Decimal)

            Dim J As Int32
            Dim YT As Decimal
            Dim XT As Int32

            L2 = 0

            For J = 1 To N
                XT = X(J)
                'OBTAIN FUNCTION
                YT = _44300(A, XT)
                L2 = L2 + (Y(J) - YT) * (Y(J) - YT)
            Next J

            D = Sqrt(L2 / (N - L))
        End Sub

        ''' <summary> Functions subroutine </summary>
        Private Shared Function _44300(ByRef A() As Decimal, ByVal X As Int32) As Decimal

            Dim Y As Decimal
            Y = A(1) * Exp(-(X - A(2)) * (X - A(2)) / A(3))
            Return Y
        End Function

        ''' <summary> Chi-square cummulative distribution approximation 
        ''' Reference- statistics manual
        ''' crow, maxfield and davis (dover, 1960).
        ''' the input value is Y, the probability.
        ''' The output value is the corresponding
        ''' chi-square statistic.
        ''' </summary>
        Public Shared Sub CHISQA(ByVal M As Int32, ByVal Y As Decimal, ByRef X As Decimal)

            Dim Z As Decimal
            X = Y

            'GUARD AGAINST 0 DISCONTINUITY
            If X = 0 Then
                X = Exp(-100)
            End If

            Dim TestCond08 As Boolean = False
            Dim TestCond13 As Boolean = False

            If X > 0.5 Then
                TestCond08 = True

            Else
                X = -Log(X)
                'REGRESSED TABLE CORRECTION
                Z = -0.803 + 1.312 * X - 0.2118 * X * X + 0.016 * X * X * X
                TestCond13 = True
            End If

            If TestCond08 = True Then
                X = 1 - X

                'GUARD AGAINST 0 DISCONTINUITY
                If X = 0 Then
                    TestCond13 = False

                Else
                    X = -Log(X)
                    Z = 0.803 - 1.312 * X + 0.2118 * X * X - 0.016 * X * X * X
                    TestCond13 = True
                End If
            End If

            If TestCond13 = True Then
                X = 2 / (9 * M)
                X = 1 - X + Z * Sqrt(X)
            End If

            X = M * X * X * X
        End Sub

    End Class

End Namespace