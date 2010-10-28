Option Strict Off
Option Explicit On

Namespace Genetibase.Mathx.NuGenComplexLib

    ''' <summary> Signed Logical Functions </summary>
    Public Class NuGenSignedLogical
        Public MaxNumberOfDigits As Object

        ''' <summary> Binary Addition </summary>
        Public Function BinaryAddition(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String

            Dim TempNumber As Object

            TempNumber = BinaryToDecimal(BinaryNumber1) + BinaryToDecimal(BinaryNumber2)

            If TempNumber > (2 ^ MaxNumberOfDigits - 1) Then
                TempNumber = TempNumber - Int(TempNumber / ((2 ^ MaxNumberOfDigits - 1) - 1)) * ((2 ^ MaxNumberOfDigits - 1) + 1)
                'MsgBox "Output Value Exceeds the Value Correspond to the Maximum Number of Digits!", 48
            End If

            BinaryAddition = DecimalToBinary(TempNumber)
        End Function

        ''' <summary> Binary Substraction </summary>
        Public Function BinarySubtraction(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String

            Dim TempNumber As Object

            TempNumber = BinaryToDecimal(BinaryNumber1) - BinaryToDecimal(BinaryNumber2)
            BinarySubtraction = DecimalToBinary(TempNumber)

        End Function

        ''' <summary> Binary Multiplication </summary>
        Public Function BinaryMultiplication(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String

            Dim TempNumber As Object
            Dim Times As Object

            TempNumber = BinaryToDecimal(BinaryNumber1) * BinaryToDecimal(BinaryNumber2)

            If TempNumber > (2 ^ MaxNumberOfDigits - 1) Then
                TempNumber = TempNumber - Int(TempNumber / ((2 ^ MaxNumberOfDigits - 1) - 1)) * ((2 ^ MaxNumberOfDigits - 1) + 1)
                'MsgBox "Output Value Exceeds the Value Correspond to the Maximum Number of Digits!", 48
            End If

            BinaryMultiplication = DecimalToBinary(TempNumber)

        End Function

        ''' <summary> Binary Integer Division </summary>
        Public Function BinaryIntegerDivision(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String

            Dim TempNumber As Object

            TempNumber = Int(BinaryToDecimal(BinaryNumber1) / BinaryToDecimal(BinaryNumber2))
            BinaryIntegerDivision = DecimalToBinary(TempNumber)
        End Function

        ''' <summary> Binary Modulo </summary>
        Public Function BinaryModulo(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String

            Dim TempNumber2, TempNumber1, TempNumber As Object

            TempNumber1 = BinaryToDecimal(BinaryNumber1)
            TempNumber2 = BinaryToDecimal(BinaryNumber2)
            TempNumber = TempNumber1 - (Int(TempNumber1 / TempNumber2)) * TempNumber2

            BinaryModulo = DecimalToBinary(TempNumber)
        End Function

        ''' <summary> Binary Shift Left </summary>
        Public Function BinaryShiftLeft(ByRef BinaryNumber As String) As String

            BinaryNumber = Right(BinaryNumber, Len(BinaryNumber) - 1) & "0"
            BinaryShiftLeft = Right(BinaryNumber, Len(BinaryNumber) - InStr(1, BinaryNumber, "1") + 1)

        End Function

        ''' <summary> Binary Shift Right </summary>
        Public Function BinaryShiftRight(ByRef BinaryNumber As String) As String

            BinaryNumber = "0" & Left(BinaryNumber, Len(BinaryNumber) - 1)
            BinaryShiftRight = Right(BinaryNumber, Len(BinaryNumber) - InStr(1, BinaryNumber, "1") + 1)

        End Function

        ''' <summary> Binary Not </summary>
        Public Function Binary_Not(ByRef BinaryNumber As String) As String

            Dim Index As Object
            Dim TempBinaryNumber As String

            BinaryNumber = New String("0", MaxNumberOfDigits - Len(BinaryNumber)) & BinaryNumber

            For Index = 1 To Len(BinaryNumber)

                If Mid(BinaryNumber, Index, 1) = "0" Then
                    TempBinaryNumber = TempBinaryNumber & "1"

                Else
                    TempBinaryNumber = TempBinaryNumber & "0"
                End If

            Next

            Binary_Not = SetBinaryValue(TempBinaryNumber)

        End Function

        ''' <summary> Binary And </summary>
        Public Function Binary_And(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String

            Dim Index As Short
            Dim Digit As String
            Dim TempBinaryNumber As String

            BinaryNumber2 = New String("0", MaxNumberOfDigits - Len(BinaryNumber2)) & BinaryNumber2
            BinaryNumber1 = New String("0", MaxNumberOfDigits - Len(BinaryNumber1)) & BinaryNumber1

            For Index = 1 To Len(BinaryNumber1)
                Digit = CStr(Mid(BinaryNumber1, Index, 1) And Mid(BinaryNumber2, Index, 1))
                TempBinaryNumber = TempBinaryNumber & Digit
            Next

            Binary_And = SetBinaryValue(TempBinaryNumber)

        End Function

        ''' <summary> Binary Or </summary>
        Public Function Binary_Or(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String

            Dim Index As Short
            Dim Digit As String
            Dim TempBinaryNumber As String

            BinaryNumber2 = New String("0", MaxNumberOfDigits - Len(BinaryNumber2)) & BinaryNumber2
            BinaryNumber1 = New String("0", MaxNumberOfDigits - Len(BinaryNumber1)) & BinaryNumber1

            For Index = 1 To Len(BinaryNumber1)
                Digit = CStr(Mid(BinaryNumber1, Index, 1) Or Mid(BinaryNumber2, Index, 1))
                TempBinaryNumber = TempBinaryNumber & Digit
            Next

            Binary_Or = SetBinaryValue(TempBinaryNumber)

        End Function

        ''' <summary> Binary XOR </summary>
        Public Function Binary_Xor(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String

            Dim Index As Short
            Dim Digit As String
            Dim TempBinaryNumber As String

            BinaryNumber2 = New String("0", MaxNumberOfDigits - Len(BinaryNumber2)) & BinaryNumber2
            BinaryNumber1 = New String("0", MaxNumberOfDigits - Len(BinaryNumber1)) & BinaryNumber1

            For Index = 1 To Len(BinaryNumber1)
                Digit = CStr(Mid(BinaryNumber1, Index, 1) Xor Mid(BinaryNumber2, Index, 1))
                TempBinaryNumber = TempBinaryNumber & Digit
            Next

            Binary_Xor = SetBinaryValue(TempBinaryNumber)
        End Function

        ''' <summary> Binary NAND </summary>
        Public Function Binary_Nand(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String
            Binary_Nand = Binary_Not(Binary_And(BinaryNumber1, BinaryNumber2))
        End Function

        ''' <summary> Binary NOR </summary>
        Public Function Binary_Nor(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String
            Binary_Nor = Binary_Not(Binary_Or(BinaryNumber1, BinaryNumber2))
        End Function

        ''' <summary> Binary NXOR </summary>
        Public Function Binary_NXor(ByRef BinaryNumber1 As String, ByRef BinaryNumber2 As String) As String
            Binary_NXor = Binary_Not(Binary_Xor(BinaryNumber1, BinaryNumber2))
        End Function

        ''' <summary> Binary Positive to Negative </summary>
        Public Function BinaryPositiveToNegative(ByRef BinaryNumber As String) As String

            Dim Carry As Object

            Dim SumBinaryNumber As String
            Dim TempBinaryNumber As String
            Dim Digit1 As String
            Dim Digit2 As String
            Dim Index As Short

            For Index = 1 To Len(BinaryNumber)

                If Mid(BinaryNumber, Index, 1) = "0" Then Digit1 = "1" Else Digit1 = "0"
                TempBinaryNumber = TempBinaryNumber & Digit1
            Next

            BinaryNumber = New String("1", MaxNumberOfDigits - Len(TempBinaryNumber)) & TempBinaryNumber

            For Index = MaxNumberOfDigits To 1 Step -1
                Digit1 = Mid(BinaryNumber, Index, 1)

                If Index = MaxNumberOfDigits Then Digit2 = CStr(1) Else Digit2 = CStr(0)
                SumBinaryNumber = Trim(Str(Digit1 Xor Digit2 Xor Carry)) & SumBinaryNumber
                Carry = (Digit1 And Digit2) Or ((Digit1 Xor Digit2) And Carry)
            Next

            SumBinaryNumber = Str(Carry) & SumBinaryNumber
            BinaryPositiveToNegative = SetBinaryValue(SumBinaryNumber)

        End Function

        ''' <summary> Decimal to Binary </summary>
        Public Function DecimalToBinary(ByRef DecimalNumber As Object) As String

            Dim BinaryNumber As String
            Dim TempDecimalNumber As Object
            Dim FlagNegative As Boolean

            If DecimalNumber < 0 Then
                DecimalNumber = DecimalNumber * -1 : FlagNegative = True
            End If

            Do
                TempDecimalNumber = Int(DecimalNumber / 2)

                If DecimalNumber - (Int(DecimalNumber / 2)) * 2 = 1 Then
                    BinaryNumber = "1" & BinaryNumber

                Else
                    BinaryNumber = "0" & BinaryNumber
                End If

                DecimalNumber = TempDecimalNumber
            Loop While TempDecimalNumber <> 0

            If FlagNegative = False Then
                DecimalToBinary = BinaryNumber

            Else
                BinaryNumber = BinaryPositiveToNegative(BinaryNumber)
                DecimalToBinary = SetBinaryValue(BinaryNumber)
            End If

        End Function

        ''' <summary> Binary to Decimal </summary>
        Public Function BinaryToDecimal(ByRef BinaryNumber As String) As Object

            Dim Index As Short
            Dim Exponent As Short
            Dim DecimalNumber As Object

            For Index = Len(BinaryNumber) To 1 Step -1

                If Mid(BinaryNumber, Index, 1) = "1" Then DecimalNumber = DecimalNumber + 2 ^ Exponent
                Exponent = Exponent + 1
            Next

            BinaryToDecimal = DecimalNumber
        End Function

        ''' <summary> Hexadecimal to Decimal </summary>
        Public Function HexadecimalToDecimal(ByRef HexNumber As String) As Object

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

        ''' <summary> Decimal to Hexadecimal </summary>
        Public Function DecimalToHexadecimal(ByRef DecimalNumber As Object) As String

            Dim BinaryNumber As String
            Dim TempDecimalNumber As Object
            Dim Digit As String
            Dim HexNumber As String
            Dim Index As Short

            BinaryNumber = DecimalToBinary(DecimalNumber)
            BinaryNumber = New String("0", MaxNumberOfDigits - Len(BinaryNumber)) & BinaryNumber

            For Index = Len(BinaryNumber) To 1 Step -4
                TempDecimalNumber = BinaryToDecimal(Mid(BinaryNumber, Index - 4 + 1, 4))

                Select Case TempDecimalNumber

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
            Next

            DecimalToHexadecimal = SetBinaryValue(HexNumber)

        End Function

        ''' <summary> Hexadecimal to Binary </summary>
        Public Function HexadecimalToBinary(ByRef HexNumber As String) As String
            HexadecimalToBinary = DecimalToBinary(HexadecimalToDecimal(HexNumber))
        End Function

        ''' <summary> Binary to Hexadecimal </summary>
        Public Function BinaryToHexadecimal(ByRef BinaryNumber As String) As String
            BinaryToHexadecimal = DecimalToHexadecimal(BinaryToDecimal(BinaryNumber))
        End Function

        ''' <summary> Set Binary Value </summary>
        Public Function SetBinaryValue(ByRef BinaryNumber As Object) As Object

            Dim TempNumber As String
            Dim Digit As String
            Dim Flag As Short
            Dim Index As Short

            BinaryNumber = Right(BinaryNumber, MaxNumberOfDigits)

            For Index = 1 To Len(BinaryNumber)
                Digit = Mid(BinaryNumber, Index, 1)

                If Digit = "0" And Flag = 0 Then GoTo out Else TempNumber = TempNumber & Digit : Flag = 1
out:
            Next

            If Flag = 1 Then
                SetBinaryValue = TempNumber

            Else
                SetBinaryValue = "0"
            End If

        End Function

    End Class

End Namespace