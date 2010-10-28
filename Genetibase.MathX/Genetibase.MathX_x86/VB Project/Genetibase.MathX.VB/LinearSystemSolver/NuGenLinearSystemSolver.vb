Option Strict Off
Option Explicit On 

''' <summary>Solves the linear equation system [A]{x}={b} for {x}
''' and calculates the determinant of matrix [A] 
''' </summary>
Public Class NuGenLinearSystemSolver

    'Define Global variables
    Private System_DIM As Short 'Dimension of the linear system
    Private Matrix_A(10, 10) As Decimal 'Max. Matrix dimensions 10x10 for the interface needs, but can be increased here
    Private Triangular_A(10, 11) As Decimal 'Triangularized Matrix A
    Private Array_B(10) As Decimal 'Array of the constants, {B}
    Private Solution_Problem As Boolean 'Determines whether the system was solved or not
    Private Result As ResultStructure

    Public Structure ResultStructure
        Public Solutions() As Object 'Array of the Solutions {x}

        Dim Determinant As Decimal
    End Structure

    ''' <summary> Solves the linear equation </summary>
    Public Function SOLVE(ByVal Matrix_A(,) As Decimal, ByVal Array_B() As Decimal, ByVal System_DIM As Integer) As ResultStructure
        Call Build_Triangular_Matrix()
        Return Back_Substitution()
    End Function

    ''' <summary> Builds Triangular Matrix </summary>
    Private Sub Build_Triangular_Matrix()

        Dim message As String
        Dim multiplier_1 As Decimal
        Dim temporary_1 As Decimal
        Dim line_1 As Integer
        Dim k As Integer
        Dim m As Integer
        Dim n As Integer
        'Uses Gauss elimination method in order to build a triangular matrix from the matrix [A]
        'Triangularized Matrix Triangular_A is (System_DIM X System_DIM+1) because it also includes
        'the array {b} with the constants:
        '[ a11 a12 a13 | b1 ]
        '[ a21 a22 a23 | b2 ]
        '[ a31 a32 a33 | b3 ] etc

        On Error GoTo errhandler 'In case the system cannot be solved (Determinant = 0)

        Solution_Problem = False

        'Assign values from matrix [A]
        For n = 1 To 10
            For m = 1 To 10
                Triangular_A(m, n) = Matrix_A(m, n)
            Next
        Next

        'Assign values from array {b}
        For n = 1 To System_DIM
            Triangular_A(n, System_DIM + 1) = Array_B(n)
        Next n

        'Triangularize the matrix
        For k = 1 To System_DIM - 1

            'Bring a non-zero element first by changes lines if necessary
            If Triangular_A(k, k) = 0 Then

                For n = k To System_DIM

                    If Triangular_A(n, k) <> 0 Then
                        line_1 = n : Exit For
                    End If 'Finds line_1 with non-zero element

                Next n

                'Change line k with line_1
                For m = k To System_DIM
                    temporary_1 = Triangular_A(k, m)
                    Triangular_A(k, m) = Triangular_A(line_1, m)
                    Triangular_A(line_1, m) = temporary_1
                Next m

            End If

            'For other lines, make a zero element by using:
            'Ai1=Aij-A11*(Aij/A11)
            'and change all the line using the same formula for other elements
            For n = k + 1 To System_DIM

                If Triangular_A(n, k) <> 0 Then 'if it is zero, stays as it is
                    multiplier_1 = Triangular_A(n, k) / Triangular_A(k, k)

                    For m = k To System_DIM + 1
                        Triangular_A(n, m) = Triangular_A(n, m) - Triangular_A(k, m) * multiplier_1
                    Next m

                End If

            Next n
        Next k

        Exit Sub

errhandler:

        message = "An error occured during the solution process. Make sure that the system is stable and can be solved."
        Throw New Exception(message)
        Solution_Problem = True

    End Sub

    ''' <summary> Calculates the Solution array {x} using back substitution </summary>
    Private Function Back_Substitution() As ResultStructure

        Dim m As Integer
        Dim sum_1 As Decimal
        Dim n As Integer

        If Solution_Problem = True Then Exit Function 'No point in back substituting to find the solution {x}

        'First, calculate last xi (for i = System_DIM)
        Result.Solutions(System_DIM) = Triangular_A(System_DIM, System_DIM + 1) / Triangular_A(System_DIM, System_DIM)

        'Back substitution for the other xi:
        For n = 1 To System_DIM - 1
            sum_1 = 0

            For m = 1 To n
                sum_1 = sum_1 + Result.Solutions(System_DIM + 1 - m) * Triangular_A(System_DIM - n, System_DIM + 1 - m)
            Next m

            Result.Solutions(System_DIM - n) = (Triangular_A(System_DIM - n, System_DIM + 1) - sum_1) / Triangular_A(System_DIM - n, System_DIM - n)
        Next n

        'Calculate Determinant of matrix [A]
        'It is the product of the diagonal elements of the triangular matrix
        Result.Determinant = 1 'Initialize the product

        For n = 1 To System_DIM
            Result.Determinant = Result.Determinant * Triangular_A(n, n)
        Next n

        Return Result
    End Function

End Class
