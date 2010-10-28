

Namespace Genetibase.MathX.NuGenMatrixOperations

#Region "Source Code of Gauss Elimination"

    ''' <summary> This is Algorithm of Gauss Eliminations
    '''
    ''' a as integer => Ad(a,a)   Ed(a) => X(a)=Elimination(a)
    ''' 
    ''' Ad |3  1   5|     X| ? |     Ed| 5 |
    '''    |2  3   4| *    | ? |   =   | 2 |
    '''    |2  5   8|      | ? |       | 6 |
    ''' 
    ''' or a = something
    ''' 
    ''' Ad |3  1   5   2|     X| ? |     Ed| 5 |
    '''    |2  3   4   5| *    | ? |   =   | 2 |
    '''    |2  5   8   4|      | ? |       | 6 |
    '''    |2  3   4   8|      | ? |       | 4 |
    '''    
    '''    In FuncTion or Sub
    ''' 
    '''    Matris as New Gauss()
    '''    and Then a as integer ' matris row , colon number
    '''    A(,)    two dimension Array =>  A(a,a) as double
    '''    E()     a dimension Array   =>  E(a) as double
    '''    X()     a dimension Array   =>  X(a) as double
    '''    a=3 =>
    '''    A(0,0)=3    :   A(0,1)=1    :   A(0,2)=5        
    '''    A(1,0)=2    :   A(1,1)=3    :   A(1,2)=4        
    '''    A(2,0)=2    :   A(2,1)=5    :   A(2,2)=8
    ''' 
    '''    E(0)=5      :   E(1)=2      :   E(2)=6
    ''' 
    '''    X=matris.ellimination(A,E)
    '''    X(0)=-0,5   :   X(1)=-1,0   :   X(2)=1,5 
    ''' </summary>
    Class NuGenGauss

        ''' <summary> Gauss Elimination </summary>
        Public Function Elimination(ByVal Ad(,) As Double, ByVal Ed() As Double) As Double()
            ReDim Elimination(Ad.GetUpperBound(0))

            Dim i, j, g, t, t1, t2 As Integer
            Dim z, sabit, k As Double
            Dim A(,), E() As Double

            Try
                t = Ad.GetUpperBound(0)
                t1 = Ad.GetUpperBound(1)
                t2 = Ed.GetUpperBound(0)

                If (t = t1 And t = t2) Then

                    ReDim A(t, t), E(t)

                    For i = 1 To t
                        For j = 1 To t
                            A(i, j) = Ad(i, j)
                        Next

                        E(i) = Ed(i)
                    Next

                    For i = 1 To t - 1
                        For j = i + 1 To t
                            z = A(j, i) / A(i, i)
                            E(j) = E(j) - z * E(i)

                            For g = i + 1 To t
                                A(j, g) = A(j, g) - z * A(i, g)
                            Next
                        Next
                    Next

                    For i = t To 1 Step -1

                        sabit = E(i)

                        For j = t To i + 1 Step -1

                            sabit = sabit - A(i, j) * Elimination.GetValue(j)

                        Next

                        k = sabit / A(i, i)

                        Elimination.SetValue(k, i)

                    Next

                Else
                    Throw New Exception("Matrix Size Error")
                End If

            Catch
                Throw New Exception("Matrix Error")
            End Try

        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

#End Region

End Namespace
