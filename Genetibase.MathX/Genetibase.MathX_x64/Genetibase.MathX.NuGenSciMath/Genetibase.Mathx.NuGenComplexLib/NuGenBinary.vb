Option Strict Off
Option Explicit On


Namespace Genetibase.Mathx.NuGenComplexLib

    ''' <summary> Logical Functions </summary>
    Class NuGenBinary

        ''' <summary> Binary Addition </summary>
        Public Shared Function BinaryAddition(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String

            Dim TempNumber As Object
            TempNumber = BinaryToDecimal(BinaryNumber1) + BinaryToDecimal(BinaryNumber2)
            BinaryAddition = DecimalToBinary(TempNumber)
        End Function

        ''' <summary> Binary Not </summary>
        Public Shared Function Binary_Not(ByRef BinaryNumber As String) As String

            Dim Index As Integer
            Dim TempBinaryNumber As String

            For Index = 1 To Len(BinaryNumber)

                If Mid(BinaryNumber, Index, 1) = "0" Then
                    TempBinaryNumber = TempBinaryNumber & "1"

                Else
                    TempBinaryNumber = TempBinaryNumber & "0"
                End If

            Next

            TempBinaryNumber = Right(TempBinaryNumber, Len(TempBinaryNumber) - InStr(1, TempBinaryNumber, "1") + 1)
            Binary_Not = SetBinaryValue(TempBinaryNumber)
        End Function

        ''' <summary> Binary And </summary>
        Public Shared Function Binary_And(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String

            Dim Index As Short
            Dim Digit As String
            Dim TempBinaryNumber As String

            BinaryNumber2 = New String("0", 40 - Len(BinaryNumber2)) & BinaryNumber2
            BinaryNumber1 = New String("0", 40 - Len(BinaryNumber1)) & BinaryNumber1

            For Index = 1 To Len(BinaryNumber1)
                Digit = CStr(Mid(BinaryNumber1, Index, 1) And Mid(BinaryNumber2, Index, 1))
                TempBinaryNumber = TempBinaryNumber & Digit
            Next

            TempBinaryNumber = Right(TempBinaryNumber, Len(TempBinaryNumber) - InStr(1, TempBinaryNumber, "1") + 1)
            Binary_And = SetBinaryValue(TempBinaryNumber)

        End Function

        ''' <summary> Binary Or </summary>
        Public Shared Function Binary_Or(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String

            Dim Index As Short
            Dim Digit As String
            Dim TempBinaryNumber As String

            BinaryNumber2 = New String("0", 40 - Len(BinaryNumber2)) & BinaryNumber2
            BinaryNumber1 = New String("0", 40 - Len(BinaryNumber1)) & BinaryNumber1

            For Index = 1 To Len(BinaryNumber1)
                Digit = CStr(Mid(BinaryNumber1, Index, 1) Or Mid(BinaryNumber2, Index, 1))
                TempBinaryNumber = TempBinaryNumber & Digit
            Next

            TempBinaryNumber = Right(TempBinaryNumber, Len(TempBinaryNumber) - InStr(1, TempBinaryNumber, "1") + 1)
            Binary_Or = SetBinaryValue(TempBinaryNumber)

        End Function

        ''' <summary> Binary Xor </summary>
        Public Shared Function Binary_Xor(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String

            Dim Index As Short
            Dim Digit As String
            Dim TempBinaryNumber As String

            BinaryNumber2 = New String("0", 40 - Len(BinaryNumber2)) & BinaryNumber2
            BinaryNumber1 = New String("0", 40 - Len(BinaryNumber1)) & BinaryNumber1

            For Index = 1 To Len(BinaryNumber1)
                Digit = CStr(Mid(BinaryNumber1, Index, 1) Xor Mid(BinaryNumber2, Index, 1))
                TempBinaryNumber = TempBinaryNumber & Digit
            Next

            TempBinaryNumber = Right(TempBinaryNumber, Len(TempBinaryNumber) - InStr(1, TempBinaryNumber, "1") + 1)
            Binary_Xor = SetBinaryValue(TempBinaryNumber)

        End Function

        ''' <summary> Binary Nand </summary>
        Public Shared Function Binary_Nand(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String
            Binary_Nand = Binary_Not(Binary_And(BinaryNumber1, BinaryNumber2))
        End Function

        ''' <summary> Binary Nor </summary>
        Public Shared Function Binary_Nor(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String
            Binary_Nor = Binary_Not(Binary_Or(BinaryNumber1, BinaryNumber2))
        End Function

        ''' <summary> Binary NXor </summary>
        Public Shared Function Binary_NXor(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String
            Binary_NXor = Binary_Not(Binary_Xor(BinaryNumber1, BinaryNumber2))
        End Function

        ''' <summary> Decimal To Binary </summary>
        Public Shared Function DecimalToBinary(ByRef DecimalNumber As Object) As String

            Dim BinaryNumber As String
            Dim TempDecimalNumber As Object

            Do
                TempDecimalNumber = Int(DecimalNumber / 2)

                If DecimalNumber - (Int(DecimalNumber / 2)) * 2 = 1 Then
                    BinaryNumber = "1" & BinaryNumber

                Else
                    BinaryNumber = "0" & BinaryNumber
                End If

                DecimalNumber = TempDecimalNumber
            Loop While TempDecimalNumber <> 0

            DecimalToBinary = BinaryNumber

        End Function

        ''' <summary> Binary To Decimal </summary>
        Public Shared Function BinaryToDecimal(ByRef BinaryNumber As String) As Object

            Dim Index As Short
            Dim Exponent As Short
            Dim DecimalNumber As Object

            For Index = Len(BinaryNumber) To 1 Step -1

                If Mid(BinaryNumber, Index, 1) = "1" Then DecimalNumber = DecimalNumber + 2 ^ Exponent
                Exponent = Exponent + 1
            Next

            BinaryToDecimal = DecimalNumber
        End Function

        ''' <summary> Hexadecimal To Decimal </summary>
        Public Shared Function HexadecimalToDecimal(ByRef HexNumber As String) As Object

            Dim HexDigit As String
            Dim Index As Short
            Dim Exponent As Short
            Dim DecimalDigit As Object
            Dim DecimalNumber As Object

            For Index = Len(HexNumber) To 1 Step -1
                HexDigit = Mid(HexNumber, Index, 1)

                Select Case HexDigit

                    Case "0" To "9"
                        DecimalDigit = Val(HexDigit)

                    Case "A", "a"
                        DecimalDigit = 10

                    Case "B", "b"
                        DecimalDigit = 11

                    Case "C", "c"
                        DecimalDigit = 12

                    Case "D", "d"
                        DecimalDigit = 13

                    Case "E", "e"
                        DecimalDigit = 14

                    Case "F", "f"
                        DecimalDigit = 15
                End Select

                DecimalNumber = DecimalNumber + (DecimalDigit * 16 ^ Exponent)
                Exponent = Exponent + 1
            Next

            HexadecimalToDecimal = DecimalNumber

        End Function

        ''' <summary> Decimal To Hexadecimal </summary>
        Public Shared Function DecimalToHexadecimal(ByRef DecimalNumber As Object) As String

            Dim HexNumber As String
            Dim TempDecimalNumber As Object
            Dim Digit As String

            Do
                TempDecimalNumber = Int(DecimalNumber / 16)

                Select Case DecimalNumber - (Int(DecimalNumber / 16)) * 16

                    Case 0 : Digit = "0"
                    Case 1 : Digit = "1"
                    Case 2 : Digit = "2"
                    Case 3 : Digit = "3"
                    Case 4 : Digit = "4"
                    Case 5 : Digit = "5"
                    Case 6 : Digit = "6"
                    Case 7 : Digit = "7"
                    Case 8 : Digit = "8"
                    Case 9 : Digit = "9"
                    Case 10 : Digit = "A"
                    Case 11 : Digit = "B"
                    Case 12 : Digit = "C"
                    Case 13 : Digit = "D"
                    Case 14 : Digit = "E"
                    Case 15 : Digit = "F"
                End Select

                HexNumber = Digit & HexNumber
                DecimalNumber = TempDecimalNumber
            Loop While TempDecimalNumber <> 0

            DecimalToHexadecimal = HexNumber

        End Function

        ''' <summary> Hexadecimal To Binary </summary>
        Public Shared Function HexadecimalToBinary(ByRef HexNumber As String) As String
            HexadecimalToBinary = DecimalToBinary(HexadecimalToDecimal(HexNumber))
        End Function

        ''' <summary> Binary To Hexadecimal </summary>
        Public Shared Function BinaryToHexadecimal(ByRef BinaryNumber As String) As String
            BinaryToHexadecimal = DecimalToHexadecimal(BinaryToDecimal(BinaryNumber))
        End Function

        ''' <summary> Set Binary Value </summary>
        Private Shared Function SetBinaryValue(ByRef BinaryNumber As String) As String

            Dim TempNumber As String
            Dim Digit As String
            Dim Flag As Short
            Dim Index As Short

            For Index = 1 To Len(BinaryNumber)
                Digit = Mid(BinaryNumber, Index, 1)

                If Digit = "0" And Flag = 0 Then

                    'GoTo out
                Else
                    TempNumber = TempNumber & Digit
                    Flag = 1
                End If

                'out:
            Next

            If Flag = 1 Then SetBinaryValue = TempNumber Else SetBinaryValue = "0"

        End Function

    End Class

End Namespace