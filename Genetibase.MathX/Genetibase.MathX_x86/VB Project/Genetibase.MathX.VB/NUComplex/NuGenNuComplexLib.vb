Option Strict On
Option Explicit On 

''' <summary> Complex Number Operations </summary>
Public Class NuGenComplexLib

    'local variable(s) to hold property value(s)
    Private dblReal As Double 'holds numeric value of real part
    Private dblImaginary As Double 'holds numeric value of imaginary part
    Private dblConjugate As Double 'holds numeric value of complex conjugate
    Private dblMagnitude As Double 'holds magnitude (r) of complex number
    Private dblTheta As Double 'holds theta of complex number
    Private mvarError As NuGenErrorLib

    ''' <summary> The imaginary part of the complex number </summary>
    Public Property Imaginary() As Double
        Get
            'used when retrieving value of a property, on the right side of an assignment.
            Imaginary = dblImaginary
        End Get
        Set(ByVal Value As Double)
            'used when assigning a value to the property, on the left side of an assignment.
            dblImaginary = Value
        End Set
    End Property

    ''' <summary> The real part of the complex number </summary>
    Public Property Real() As Double
        Get
            'used when retrieving value of a property, on the right side of an assignment.
            Real = dblReal
        End Get
        Set(ByVal Value As Double)
            'used when assigning a value to the property, on the left side of an assignment.
            dblReal = Value
        End Set
    End Property

    ''' <summary> The magnitude of the complex number </summary>
    Public Property Magnitude() As Double
        Get
            'used when retrieving value of a property, on the right side of an assignment.
            Magnitude = dblMagnitude
        End Get
        Set(ByVal Value As Double)
            'used when assigning a value to the property, on the left side of an assignment.
            dblMagnitude = Value
        End Set
    End Property

    ''' <summary> If Theta is entered directly through this property, the number is assumed to be in radians </summary>
    Public Property Theta() As Double
        Get
            'used when retrieving value of a property, on the right side of an assignment.
            Theta = dblTheta
        End Get
        Set(ByVal Value As Double)
            'used when assigning a value to the property, on the left side of an assignment.
            dblTheta = Value
        End Set
    End Property

    ''' <summary> The conjugate of the complex number </summary>
    Public Property Conjugate() As Double
        Get
            'used when retrieving value of a property, on the right side of an assignment.
            Conjugate = dblConjugate
        End Get
        Set(ByVal Value As Double)
            'used when assigning a value to the property, on the left side of an assignment.
            dblConjugate = Value
        End Set
    End Property

    ''' <summary> The error code </summary>
    Public Property CNError() As NuGenErrorLib
        Get
            'used when retrieving value of a property, on the right side of an assignment.
            CNError = mvarError
        End Get
        Set(ByVal Value As NuGenErrorLib)
            mvarError = Value
        End Set
    End Property

    ''' <summary> Changes the sign of a complex number </summary>
    Public Function ChgSign() As NuGenComplexLib

        Dim CplxResult As NuGenComplexLib
        CplxResult = New NuGenComplexLib
        CplxResult.Real = dblReal * -1
        CplxResult.Conjugate = dblImaginary
        CplxResult.Imaginary = dblImaginary * -1
        ChgSign = CplxResult.GetPolar()
    End Function

    ''' <summary> Inverts the complex number </summary>
    Public Function Invert() As NuGenComplexLib

        Dim CplxResult As NuGenComplexLib
        CplxResult = New NuGenComplexLib
        CplxResult = CplxResult.stringToCplxNum("1+j0", False)
        CplxResult = CplxResult.Divide(Me)
        Invert = CplxResult.GetPolar()
    End Function

    ''' <summary> Raises the complex number to the indicated power using De Moivre's Theorem </summary>
    Public Function Exp(ByRef k As Integer) As NuGenComplexLib

        Dim CplxResult As NuGenComplexLib
        CplxResult = New NuGenComplexLib
        CplxResult.CNError = New NuGenErrorLib

        Try
            'compute new exponentiated rectangular coordinates
            CplxResult.Real = dblMagnitude ^ k * System.Math.Cos(k * dblTheta)
            CplxResult.Imaginary = dblMagnitude ^ k * System.Math.Sin(k * dblTheta)
            CplxResult.Conjugate = dblImaginary * -1
            Exp = CplxResult.GetPolar

        Catch ex As Exception
            CplxResult.CNError.Number = Err.Number
            CplxResult.CNError.Description = Err.Description
            CplxResult.CNError.Source = "Function Exp"
            Exp = CplxResult
            Err.Clear()
        End Try

    End Function

    ''' <summary> Routine to extract the roots of a complex number using De Moivre's Theorem </summary>
    Public Function ExtractRoots(ByRef n As Integer) As NuGenComplexLib()

        Dim CplxRoot() As NuGenComplexLib
        Dim k As Integer

        Try
            'Redimension the array that holds the extracted complex roots
            ReDim CplxRoot(n - 1)

            'Extract the roots using De Moivre's Theorem
            For k = 0 To n - 1
                'Create a complex number object
                CplxRoot(k) = New NuGenComplexLib
                CplxRoot(k).CNError = New NuGenErrorLib
                'First compute the real part of the kth root
                CplxRoot(k).Real = dblMagnitude ^ (1 / n) * System.Math.Cos((dblTheta + k * 2 * System.Math.PI) / n)
                'Compute the imaginary part of the kth root
                CplxRoot(k).Imaginary = dblMagnitude ^ (1 / n) * System.Math.Sin((dblTheta + k * 2 * System.Math.PI) / n)
                CplxRoot(k).Conjugate = CplxRoot(k).Imaginary * -1
                CplxRoot(k).GetPolar()
            Next k

            ExtractRoots = CplxRoot

        Catch ex As Exception
            CplxRoot(0).CNError.Number = Err.Number
            CplxRoot(0).CNError.Description = Err.Description
            CplxRoot(0).CNError.Source = "Function Exp"
            ExtractRoots = CplxRoot
            Err.Clear()
        End Try

    End Function

    ''' <summary> Routine to divide two complex numbers </summary>
    Public Function Divide(ByRef CplxNr As NuGenComplexLib) As NuGenComplexLib

        Dim CplxResult As NuGenComplexLib

        Try
            CplxResult = New NuGenComplexLib
            CplxResult.CNError = New NuGenErrorLib
            'Compute the real part of the complex number
            CplxResult.Real = (dblReal * CplxNr.Real + -1 * dblImaginary * CplxNr.Conjugate) / (CplxNr.Real * CplxNr.Real + -1 * CplxNr.Imaginary * CplxNr.Conjugate)
            'Compute the imaginary part of the complex number
            CplxResult.Imaginary = (dblReal * CplxNr.Conjugate + CplxNr.Real * dblImaginary) / (CplxNr.Real * CplxNr.Real + -1 * CplxNr.Imaginary * CplxNr.Conjugate)
            'Compute the conjugate
            CplxResult.Conjugate = -1 * CplxResult.Imaginary
            Divide = CplxResult.GetPolar()

        Catch ex As Exception
            CplxResult.CNError.Number = Err.Number
            CplxResult.CNError.Description = Err.Description
            CplxResult.CNError.Source = "Function Exp"
            Divide = CplxResult
            Err.Clear()
        End Try

    End Function

    ''' <summary> Routine to multiply two complex numbers </summary>
    Public Function Multiply(ByRef CplxNr As NuGenComplexLib) As NuGenComplexLib

        Dim CplxResult As NuGenComplexLib
        CplxResult = New NuGenComplexLib
        'Find the real part using simple FOIL method
        CplxResult.Real = dblReal * CplxNr.Real + -1 * (dblImaginary * CplxNr.Imaginary)
        'Find the imaginary using simple FOIL method
        CplxResult.Imaginary = dblReal * CplxNr.Imaginary + CplxNr.Real * dblImaginary
        Multiply = CplxResult.GetPolar()
    End Function

    ''' <summary> Routine to subtract two complex numbers </summary>
    Public Function Subtract(ByRef CplxNr As NuGenComplexLib) As NuGenComplexLib

        Dim CplxResult As NuGenComplexLib
        CplxResult = New NuGenComplexLib
        CplxResult.Real = dblReal - CplxNr.Real
        CplxResult.Imaginary = dblImaginary - CplxNr.Imaginary
        CplxResult.Conjugate = CplxResult.Imaginary * -1

        Subtract = CplxResult.GetPolar()
    End Function

    ''' <summary> Routine to add two complex numbers </summary>
    Public Function Add(ByRef CplxNr As NuGenComplexLib) As NuGenComplexLib

        Dim CplxResult As NuGenComplexLib
        CplxResult = New NuGenComplexLib
        CplxResult.Real = dblReal + CplxNr.Real
        CplxResult.Imaginary = dblImaginary + CplxNr.Imaginary
        CplxResult.Conjugate = CplxResult.Imaginary * -1
        Add = CplxResult.GetPolar()
    End Function

    ''' <summary> Calculate the rectangular coordinates- given the polar coordinates </summary>
    Public Function GetRectangular() As NuGenComplexLib
        'We want to put the complex number into the form a + jb
        'the value of a = r*Cos(T) where
        '  a = dblReal
        '  r = dblMagnitude
        '  T = dblTheta
        dblReal = dblMagnitude * System.Math.Cos(dblTheta)
        'the value of b = r*Sin(T) where
        '  b = dblImaginary
        '  r = dblMagnitude
        '  T = dblTheta
        dblImaginary = dblMagnitude * System.Math.Sin(dblTheta)
        GetRectangular = Me
    End Function

    ''' <summary> Calculate polar coordinates </summary>
    Public Function GetPolar() As NuGenComplexLib
        'Convert the expression a + jb from rectangular coordinates to polar coordinates
        'STEP 1 = Compute Magnitude using the formula
        'R = sqrt(a^2 + b^2), as in the complex expression a + jb
        dblMagnitude = System.Math.Sqrt((dblReal ^ 2) + (dblImaginary ^ 2))

        'STEP 2 = Compute the value of Theta using the formula
        ' Theta = ArcTan(b/a) where theta is the angle between the positive real axis and R
        'if the real coordinate (a) is not equal to zero, then Theta is simply the
        'ArcTan(b/a), adjusted for the quadrant the point is in. However, if the Real coordinate
        'is approaching 0 then Theta is approaching either dblpi/2 or -dblpi/2
        If dblReal <> 0 Then
            'find Theta directly
            dblTheta = System.Math.Atan(dblImaginary / dblReal)

        Else 'dblReal = 0

            If dblImaginary > 0 Then
                'if the Imaginary coordinate is positive - the angle is in Quad I
                dblTheta = System.Math.PI / 2

            ElseIf dblImaginary < 0 Then
                'if the Imaginary coordinate is negative - the angle is in Quad III
                dblTheta = -1 * System.Math.PI / 2

            Else
                'Can't have both numbers = 0
                dblTheta = System.Math.Atan(1)
            End If
        End If

        'Adjust Theta based on the quadrant the complex point is in
        If dblImaginary >= 0 Then
            If dblReal >= 0 Then

                'Quadrant I
                'No action - Theta is the angle from the positive real axis to r
            Else 'dblreal is < 0
                'Quadrand II
                'Adjust by adding System.Math.PI radians (or 180 degrees)
                dblTheta = dblTheta + System.Math.PI
            End If

        Else 'dblImaginary is < 0

            If dblReal < 0 Then
                'Quadrant III
                'Adjust by adding System.Math.PI radians (or 180 degrees)
                dblTheta = dblTheta + System.Math.PI

            Else 'dblReal >= 0
                'Quadrant IV
                'Adjust by adding 2pi radians (or 360 degrees)
                dblTheta = dblTheta + 2 * System.Math.PI
            End If
        End If

        GetPolar = Me
    End Function

    ''' <summary> Initialize a new instance of this class </summary>
    Public Sub New()
        MyBase.New()
        mvarError = New NuGenErrorLib
    End Sub

    ''' <summary> Returns the string representation </summary>
    Public Function cplxNumToString(Optional ByRef Polar As Boolean = False, Optional ByRef useRadians As Boolean = True, Optional ByRef intPlaces As Integer = 0) As String

        Dim tempSigDig As Integer
        Dim ImagSign As String
        Dim intInternalPrecision As Short
        intInternalPrecision = 15

        If Polar Then

            'If number of decimal places specified is greater than zero then use intPlaces instead of
            'lngInternalPrecision. If number of decimal places specified is invalid then use the current
            'lngInternalPrecision.
            If intPlaces > 0 And intPlaces <= intInternalPrecision Then
                tempSigDig = intPlaces

            Else
                tempSigDig = intInternalPrecision
            End If

            If useRadians Then
                cplxNumToString = CStr(System.Math.Round(dblMagnitude, tempSigDig)).Trim & " / " & CStr(System.Math.Round(dblTheta, tempSigDig)).Trim & "r"

            Else
                cplxNumToString = CStr(System.Math.Round(dblMagnitude, tempSigDig)).Trim & " / " & CStr(System.Math.Round(dblTheta * 180 / System.Math.PI, tempSigDig)).Trim & "d"
            End If

            'cplxNumToString = CStr(System.Math.Round(dblMagnitude, tempSigDig)).Trim & " / " & CStr(System.Math.Round(dblTheta, tempSigDig)).Trim & "r (" & CStr(System.Math.Round(dblTheta * 180 / System.Math.PI, tempSigDig)).Trim & "d)"
        Else

            'If number of decimal places specified is greater than zero then use intPlaces instead of
            'lngInternalPrecision. If number of decimal places specified is invalid then use the current
            'lngInternalPrecision.
            If intPlaces > 0 And intPlaces <= intInternalPrecision Then
                tempSigDig = intPlaces

            Else
                tempSigDig = intInternalPrecision
            End If

            'Format temporary strings using the number of specified signigicant decimal places
            'specified by the user Note that although the displayed expressions are rounded as
            'specified by the value lngSigDig the actual numerical values are not rounded
            'Format the expression
            If dblImaginary >= 0 Then
                ImagSign = " + "

            Else
                ImagSign = " - "
            End If

            If System.Math.Round(dblReal, tempSigDig) <> 0 And System.Math.Round(System.Math.Abs(dblImaginary), tempSigDig) <> 0 Then
                If System.Math.Round(System.Math.Abs(dblImaginary), tempSigDig) = 1 Or System.Math.Round(System.Math.Abs(dblImaginary), tempSigDig) = -1 Then
                    cplxNumToString = CStr(System.Math.Round(dblReal, tempSigDig)).Trim & ImagSign & "j"

                Else
                    cplxNumToString = CStr(System.Math.Round(dblReal, tempSigDig)).Trim & ImagSign & "j" & CStr(System.Math.Round(System.Math.Abs(dblImaginary), tempSigDig)).Trim
                End If

            ElseIf System.Math.Round(dblReal, tempSigDig) = 0 And System.Math.Round(System.Math.Abs(dblImaginary), tempSigDig) <> 0 Then

                If System.Math.Round(System.Math.Abs(dblImaginary), tempSigDig) = 1 Or System.Math.Round(System.Math.Abs(dblImaginary), tempSigDig) = -1 Then
                    cplxNumToString = ImagSign.Trim & "j"

                Else
                    cplxNumToString = ImagSign.Trim & "j" & CStr(System.Math.Round(System.Math.Abs(dblImaginary), tempSigDig)).Trim
                End If

                If cplxNumToString.StartsWith("+") Then cplxNumToString = cplxNumToString.Remove(0, 1)

            ElseIf System.Math.Round(dblReal, tempSigDig) <> 0 And System.Math.Round(System.Math.Abs(dblImaginary), tempSigDig) = 0 Then
                cplxNumToString = CStr(System.Math.Round(dblReal, tempSigDig)).Trim

            Else
                cplxNumToString = "0"
            End If
        End If

    End Function

    ''' <summary> Routine to parse the complex number expression entered by the user
    ''' It extracts the real and imaginary part of the number and stores them in
    ''' type double variables used in calculations
    ''' The user can enter the complex number in a variety of ways. For example, all of the following
    ''' formats would be acceptable:
    ''' R, +R, -R, jI, +jI, -jI, R+jI, R-jI, +R+jI, +R-jI, -R+jI, -R-jI
    ''' R-j(I=-1), R+j(I=1)
    ''' <summary>
    'Public Sub ParseCplxNr(CNum As NuGenComplexLib, strExpression As String, E As Error)

    ''' <summary> Returns the NuGenComplexLib representation </summary>
    Public Function stringToCplxNum(ByRef strExpression As String, Optional ByRef Polar As Boolean = False) As NuGenComplexLib

        Dim CNum As NuGenComplexLib
        Dim j As Integer
        Dim lastChar As Char        'the last character examined
        Dim imagRep As Char         'the character used as the "j" character
        Dim imagSign As Char        'the sign of the imaginary part
        Dim intExprLen As Integer     'length of the strExpression variable
        Dim strtempReal As String   'real part of complext number (as string)
        Dim strtempImag As String   'imaginary part of complext number (as string)
        Dim intSigns As Integer       'number of signs in the complex number
        Dim intAlpha As Integer       'number of alpha characters in complex number
        Dim intAlphaPos As Integer = -1   'position of the j character
        Dim intSlashPos As Integer    'position of the j character
        Dim intAngle As Integer
        Dim tmpChar As Char
        CNum = New NuGenComplexLib
        CNum.CNError = New NuGenErrorLib
        strExpression = RemoveWhitespace(strExpression)
        intExprLen = strExpression.Length

        'Check for a d (degrees) or an r(radians) at the end of the string
        'if found, shorten the length of the string by one.
        If Polar Then
            If strExpression.EndsWith("d") Or strExpression.EndsWith("D") Then
                intAngle = 2
                intExprLen = intExprLen - 1

            ElseIf strExpression.EndsWith("r") Or strExpression.EndsWith("R") Then
                intAngle = 1
                intExprLen = intExprLen - 1
            End If

            'This routine finds the '/' that separates the magnitude and angle
            For j = 0 To intExprLen - 1

                If strExpression.Chars(j) = "/" Or strExpression.Chars(j) = "\" Then
                    intSlashPos = j
                End If

            Next j

            If intSlashPos < 0 Then
                'if there is no slash, its an invalid polar number
                CNum.CNError.Number = 2004
                CNum.CNError.Description = "Invalid number - polar coordinates."
                CNum.CNError.Source = "Function stringToCplxNumber"
                stringToCplxNum = CNum
                Exit Function

            ElseIf intSlashPos = 0 Then 'the magnitude is assumed to be zero
                CNum.Magnitude = 0

            Else

                Try
                    CNum.Magnitude = CDbl(strExpression.Substring(0, intSlashPos))

                Catch ex As Exception
                    CNum.CNError.Number = 2003
                    CNum.CNError.Description = "Invalid number - polar coordinates."
                    CNum.CNError.Source = "Function stringToCplxNumber"
                    stringToCplxNum = CNum
                    Exit Function
                End Try

                'if the entry to the right of the '/' is not numeric then an invalid number has been entered
                Try
                    CNum.Theta = CDbl(strExpression.Substring(intSlashPos + 1, intExprLen - intSlashPos - 1))

                Catch ex As Exception
                    CNum.CNError.Number = 2003
                    CNum.CNError.Description = "Invalid number - polar coordinates."
                    CNum.CNError.Source = "Function stringToCplxNumber"
                    stringToCplxNum = CNum
                    Exit Function
                End Try

            End If

            If intAngle = 2 Then 'the entered angle is in degrees
                CNum.Theta = CNum.Theta * System.Math.PI / 180
            End If

            stringToCplxNum = CNum.GetRectangular()
            stringToCplxNum.Conjugate = -1 * stringToCplxNum.Imaginary

        Else
            strExpression = strExpression.ToLower

            For Each tmpChar In strExpression

                If Char.IsLetter(tmpChar) Then
                    intAlpha += 1
                    intAlphaPos = j
                    strExpression = strExpression.Replace(tmpChar, "j")

                ElseIf tmpChar = "+" Or tmpChar = "-" Then
                    intSigns += 1
                End If

                j += 1
            Next

            If intAlpha > 1 Or intSigns > 2 Then
                CNum.CNError.Number = 2004
                CNum.CNError.Description = "Invalid number - rectangular coordinates."
                CNum.CNError.Source = "Function stringToCplxNumber"
                stringToCplxNum = CNum
                Exit Function
            End If

            If intAlphaPos < 0 Then
                'There is no imaginary part
                strtempReal = strExpression
                strtempImag = "0"

            Else

                'There is an imaginary part
                If intAlphaPos = 0 Then 'there is no real part and it's positive
                    strtempReal = "0"
                    strtempImag = strExpression

                    If strtempImag.EndsWith("j") Then
                        strtempImag = strtempImag.Replace("j", "1")

                    Else
                        strtempImag = strtempImag.Replace("j", "")
                    End If

                ElseIf intAlphaPos = 1 Then 'there is no real part and the first char is a sign

                    If strExpression.StartsWith("+") Or strExpression.StartsWith("-") Then
                        strtempReal = "0"
                        strtempImag = strExpression

                        If strtempImag.EndsWith("j") Then
                            strtempImag = strtempImag.Replace("j", "1")

                        Else
                            strtempImag = strtempImag.Replace("j", "")
                        End If

                    Else
                        'there is an error because if the second char is a j then the first character must be a sign
                        CNum.CNError.Number = 2004
                        CNum.CNError.Description = "Invalid number - rectangular coordinates."
                        CNum.CNError.Source = "Function stringToCplxNumber"
                        stringToCplxNum = CNum
                        Exit Function
                    End If

                Else
                    'there is a real and imaginary part if intalphapos > 1
                    strtempReal = strExpression.Substring(0, intAlphaPos - 1)
                    strtempImag = strExpression.Substring(intAlphaPos - 1)

                    If strtempImag.EndsWith("j") Then
                        strtempImag = strtempImag.Replace("j", "1")

                    Else
                        strtempImag = strtempImag.Replace("j", "")
                    End If
                End If
            End If

            Try
                CNum.Real = CDbl(strtempReal) 'convert real part to double
                CNum.Imaginary = CDbl(strtempImag) 'convert imaginary part to double
                CNum.Conjugate = -1 * CNum.Imaginary
                stringToCplxNum = CNum.GetPolar()

            Catch ex As Exception
                CNum.CNError.Number = Err.Number
                CNum.CNError.Description = Err.Description
                CNum.CNError.Source = "Conversion to double failed - Function stringToCplxNumber"
                stringToCplxNum = CNum
                Err.Clear()
            End Try

        End If

    End Function

    ''' <summary> Remove whitespace function <summary>
    Private Function RemoveWhitespace(ByRef strExpression As String) As String
        'RemoveWhitespace = Replace(strExpression, " ", "")
        RemoveWhitespace = strExpression.Replace(" ", "")
    End Function

End Class
