Option Strict Off
Option Explicit On 

''' <summary> Polynomial Operations </summary>
Public Class NuGenPolynomial

    ''' <summary> Polynomial addition </summary>
    Public Shared Function Addition(ByVal A_PolynomialOrder As Integer, ByVal B_PolynomialOrder As Integer, ByVal A1() As Decimal, ByVal A2() As Decimal) As Decimal()

        Dim i As Integer
        Dim Index As Integer
        Dim MaxOrder, MinOrder As Integer

        If A_PolynomialOrder > B_PolynomialOrder Then
            MaxOrder = A_PolynomialOrder
            MinOrder = B_PolynomialOrder

        Else
            MaxOrder = B_PolynomialOrder
            MinOrder = A_PolynomialOrder
        End If

        'Dim A1(MaxOrder) As Decimal
        'Dim A2(MinOrder) As Decimal
        Dim A3(MinOrder) As Decimal

        For i = 0 To MinOrder
            A3(i) = A1(i) + A2(i)
        Next

        Return A3
    End Function

    ''' <summary> Polynomial division </summary>
    Public Shared Function Division(ByVal A_PolynomialOrder As Integer, ByVal B_PolynomialOrder As Integer, ByVal Dividend() As Decimal, ByVal Divider() As Decimal) As Object()

        Dim i As Integer
        Dim Back As Integer
        Dim Flag As Integer
        Dim Q As Integer
        Dim R As Integer
        Dim D As Integer
        Dim Index As Integer
        Dim QuotientOrder As Integer
        Dim DivisionResponse(2) As Object

        On Error GoTo ErrorRespond
        QuotientOrder = A_PolynomialOrder - B_PolynomialOrder

        'Dim Dividend(A_PolynomialOrder) As Object
        'Dim Divider(B_PolynomialOrder) As Object
        Dim Quotient(QuotientOrder) As Object

        D = A_PolynomialOrder
        R = B_PolynomialOrder
        Q = QuotientOrder

        Do
            Quotient(Q) = Dividend(D) / Divider(R)
            Flag = 0
            Back = 0

            For i = R To 0 Step -1
                Dividend(D) = Dividend(D) - Quotient(Q) * Divider(i)

                If Flag = 0 And Dividend(D) <> 0 Then
                    Back = D
                    Flag = 1
                End If

                D = D - 1
            Next

            If Flag = 1 Then
                Q = Q - (R + 1 + D - Back)
                D = Back

            Else
                Q = Q - (R + 1)
            End If

        Loop While Q >= 0 And D >= R

        DivisionResponse(0) = Quotient
        DivisionResponse(1) = Dividend

        Exit Function
ErrorRespond:
        Throw New Exception("Division is Imposible")
        Exit Function
    End Function

    ''' <summary> Polynomial multiplication </summary>
    Public Shared Function Multiplication(ByVal A_PolynomialOrder As Integer, ByVal B_PolynomialOrder As Integer, ByVal Multiplicand() As Decimal, ByVal Multiplier() As Decimal) As Decimal()

        Dim R As Integer
        Dim P As Integer
        Dim Index As Integer
        Dim ProductOrder As Integer

        ProductOrder = A_PolynomialOrder + B_PolynomialOrder

        'Dim Multiplicand(A_PolynomialOrder) As Object
        'Dim Multiplier(B_PolynomialOrder) As Object
        Dim Product(ProductOrder) As Decimal

        For P = 0 To B_PolynomialOrder
            For R = 0 To A_PolynomialOrder
                Product(P + R) = Product(P + R) + Multiplicand(R) * Multiplier(P)
            Next
        Next

        Return Product

    End Function

End Class
