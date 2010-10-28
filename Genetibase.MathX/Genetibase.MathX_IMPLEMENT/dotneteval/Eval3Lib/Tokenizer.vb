Public Class tokenizer
    Private mString As String
    Private mLen As Integer
    Private mPos As Integer
    Private mCurChar As Char
    Private mParser As parser
    Private mSyntax As eParserSyntax
    Private mLine, mColumn As Integer

    Public line, column As Integer

    Public type As eTokenType
    Public value As New System.Text.StringBuilder

    Protected Friend Sub New(ByVal Parser As parser, Optional ByVal syntax As eParserSyntax = eParserSyntax.Vb)
        mSyntax = syntax
        mParser = Parser
        mPos = -1
    End Sub

    Public Sub Start(ByVal str As String)
        mString = str
        mLen = str.Length
        mPos = 0
        mCurChar = Nothing
        mLine = 1
        mColumn = 0
        NextChar()   ' start the machine
    End Sub

    Public Sub RaiseError(ByVal msg As String, Optional ByVal ex As Exception = Nothing)
        If TypeOf ex Is Evaluator.parserException Then
            msg &= ". " & ex.Message
        Else
            msg &= " " & " at line " & line & " character " & column
            If Not ex Is Nothing Then
                msg &= ". " & ex.Message
            End If
        End If
        'in fact startpos is probably already gone too far
        Throw New Evaluator.parserException(msg, Me.mString, line, column)
    End Sub

    Public Sub RaiseUnexpectedToken(Optional ByVal msg As String = Nothing)
        If Len(msg) = 0 Then
            msg = ""
        Else
            msg &= "; "
        End If
        RaiseError(msg & "Unexpected " & type.ToString().Replace("_"c, " "c) & " : " & value.ToString)
    End Sub

    Public Sub RaiseWrongOperator(ByVal tt As eTokenType, ByVal ValueLeft As Object, ByVal valueRight As Object, Optional ByVal msg As String = Nothing)
        If Len(msg) > 0 Then
            msg.Replace("[card]", tt.GetType.ToString)
            msg &= ". "
        End If
        msg = "Cannot apply the operator " & tt.ToString
        If ValueLeft Is Nothing Then
            msg &= " on nothing"
        Else
            msg &= " on a " & ValueLeft.GetType.ToString()
        End If
        If Not valueRight Is Nothing Then
            msg &= " and a " & valueRight.GetType.ToString()
        End If
        RaiseError(msg)
    End Sub

    Private Function IsOp() As Boolean
        Return mCurChar = "+"c _
        Or mCurChar = "-"c _
        Or mCurChar = "–"c _
        Or mCurChar = "%"c _
        Or mCurChar = "/"c _
        Or mCurChar = "("c _
        Or mCurChar = ")"c _
        Or mCurChar = "."c
    End Function

    Public Sub NextToken()
        value.Length = 0
        type = eTokenType.none
        Do
            line = mLine
            column = mColumn
            Select Case mCurChar
                Case Nothing
                    type = eTokenType.end_of_file
                Case ControlChars.Cr
                    NextChar()
                    ' we ignore lf just after CR
                    If mCurChar = ControlChars.Lf Then NextChar()
                    type = eTokenType.end_of_line
                Case "0"c To "9"c
                    ParseNumber()
                Case "-"c, "–"c
                    NextChar()
                    type = eTokenType.operator_minus
                Case "+"c
                    NextChar()
                    type = eTokenType.operator_plus
                Case "*"c
                    NextChar()
                    type = eTokenType.operator_mul
                Case "/"c
                    NextChar()
                    If mCurChar = "/"c And mSyntax = eParserSyntax.cSharp Then
                        NextChar()
                        skipEndOfLineComment()
                    Else
                        type = eTokenType.operator_div
                    End If
                Case ":"c
                    NextChar()
                    If mCurChar = "="c Then
                        NextChar()
                        type = eTokenType.colon_equal
                    Else
                        type = eTokenType.colon
                    End If
                Case "%"c
                    NextChar()
                    type = eTokenType.operator_percent
                Case "("c
                    NextChar()
                    type = eTokenType.open_parenthesis
                Case ")"c
                    NextChar()
                    type = eTokenType.close_parenthesis
                Case "?"c
                    NextChar()
                    type = eTokenType.question_mark
                Case "<"c
                    NextChar()
                    If mCurChar = "="c Then
                        NextChar()
                        type = eTokenType.operator_le
                    ElseIf mCurChar = ">"c Then
                        NextChar()
                        type = eTokenType.operator_ne
                    Else
                        type = eTokenType.operator_lt
                    End If
                Case ">"c
                    NextChar()
                    If mCurChar = "="c Then
                        NextChar()
                        type = eTokenType.operator_ge
                    Else
                        type = eTokenType.operator_gt
                    End If
                Case ","c
                    NextChar()
                    type = eTokenType.comma
                Case "="c
                    NextChar()
                    type = eTokenType.operator_eq
                Case "."c
                    NextChar()
                    type = eTokenType.dot
                Case """"c
                    ParseText(True)
                    type = eTokenType.value_string
                Case "#"c
                    ParseDate()
                Case "["c
                    NextChar()
                    type = eTokenType.open_bracket
                Case "'"c
                    If mSyntax = eParserSyntax.Vb Then
                        NextChar()
                        skipEndOfLineComment()
                    Else
                        ParseIdentifier()
                    End If
                Case "!"c
                    If mSyntax = eParserSyntax.cSharp Then
                        type = eTokenType.operator_not
                    Else
                        ParseIdentifier()
                    End If
                Case "&"c
                    NextChar()
                    If mSyntax = eParserSyntax.Vb Then
                        type = eTokenType.operator_concat
                    ElseIf mSyntax = eParserSyntax.cSharp Then
                        If mCurChar = "&" Then
                            NextChar()
                            type = eTokenType.operator_and
                        Else
                            ParseIdentifier()
                        End If
                    End If
                Case "|"c
                    If mSyntax = eParserSyntax.cSharp Then
                        NextChar()
                        If mCurChar = "|" Then
                            NextChar()
                            type = eTokenType.operator_or
                        End If
                    Else
                        ParseIdentifier()
                    End If
                Case "]"c
                    NextChar()
                    type = eTokenType.close_bracket
                Case Chr(0) To " "c
                    ' do nothing
                Case Else
                    ParseIdentifier()
            End Select
            If type <> eTokenType.none Then Exit Do
            NextChar()
        Loop
    End Sub

    Private Sub NextChar()
        If mPos < mLen Then
            Dim oldChar As Char = mCurChar
            If oldChar = ControlChars.Cr Then
                mLine += 1
                mColumn = 1
            Else
                mColumn += 1
            End If

            mCurChar = mString.Chars(mPos)
            mPos += 1
            If mCurChar = Chr(147) Or mCurChar = Chr(148) Then
                mCurChar = """"c
            End If
            If mCurChar = Chr(145) Or mCurChar = Chr(146) Then
                mCurChar = "'"c
            End If
            If mCurChar = vbLf Then
                If oldChar = ControlChars.Cr Then
                    ' CRLF (ignore the LF)
                    NextChar()
                    mColumn = 1
                Else
                    ' LF alone...
                    mCurChar = ControlChars.Cr
                End If
            End If
        Else
            mCurChar = Nothing
        End If
    End Sub

    Private Sub ParseNumber()
        type = eTokenType.value_number
        While mCurChar >= "0"c And mCurChar <= "9"c
            value.Append(mCurChar)
            NextChar()
        End While
        If mCurChar = "."c Then
            value.Append(mCurChar)
            NextChar()
            While mCurChar >= "0"c And mCurChar <= "9"c
                value.Append(mCurChar)
                NextChar()
            End While
        End If
    End Sub

    Private Sub ParseIdentifier()
        While (mCurChar >= "0"c And mCurChar <= "9"c) _
                Or (mCurChar >= "a"c And mCurChar <= "z"c) _
                Or (mCurChar >= "A"c And mCurChar <= "Z"c) _
                Or (mCurChar >= "A"c And mCurChar <= "Z"c) _
                Or (mCurChar >= Chr(128)) _
                Or (mCurChar = "_"c) Or (mCurChar = "@"c)
            value.Append(mCurChar)
            NextChar()
        End While
        type = eTokenType.none
        If mSyntax = eParserSyntax.Vb Then
            Select Case value.ToString
                Case "and"
                    type = eTokenType.operator_and
                Case "or"
                    type = eTokenType.operator_or
                Case "not"
                    type = eTokenType.operator_not
                Case "true", "yes"
                    type = eTokenType.value_true
                Case "false", "no"
                    type = eTokenType.value_false
            End Select
        ElseIf mSyntax = eParserSyntax.cSharp Then
            Select Case value.ToString
                Case "true"
                    type = eTokenType.value_true
                Case "false"
                    type = eTokenType.value_false
            End Select
        End If
        If type = eTokenType.none Then type = eTokenType.value_identifier
    End Sub

    Friend Function ParseText(ByVal InQuote As Boolean) As opCode
        Dim OriginalChar As Char
        Dim curString As opCode = Nothing
        If InQuote Then
            OriginalChar = mCurChar
            NextChar()
        End If

        Do While mCurChar <> Nothing
            If InQuote AndAlso mCurChar = OriginalChar Then
                NextChar()
                If mCurChar = OriginalChar Then
                    value.Append(mCurChar)
                Else
                    'End of String
                    InQuote = False
                    Exit Do
                End If
            ElseIf mCurChar = "%"c Then
                NextChar()
                If mCurChar = "["c Then
                    NextChar()
                    Dim valueBefore As System.Text.StringBuilder = value
                    If valueBefore.Length > 0 Then
                        Dim opCodeBefore As opCode = New opCodeImmediate(GetType(String), valueBefore.ToString)
                        If curString Is Nothing Then
                            curString = opCodeBefore
                        Else
                            curString = New opCodeBinary(Me, curString, eTokenType.operator_concat, opCodeBefore)
                        End If
                    End If
                    Me.value.Length = 0
                    Me.NextToken() ' restart the tokenizer for the subExpr
                    Dim subExpr As opCode = Nothing
                    Try
                        subExpr = mParser.ParseExpr(Nothing, ePriority.none)
                        If curString Is Nothing Then
                            curString = subExpr
                        Else
                            curString = New opCodeBinary(Me, curString, eTokenType.operator_concat, subExpr)
                        End If
                    Catch ex As Exception
                        ' XML don't like < and >
                        Me.value.Append("[Error " & ex.Message & "]")
                    End Try
                    Me.value.Length = 0
                Else
                    value.Append("%"c)
                End If
            Else
                value.Append(mCurChar)
                NextChar()
            End If
        Loop
        If InQuote Then
            RaiseError("Incomplete string, missing " & OriginalChar & "; String started")
        Else
            If value.Length = 0 Then
                If curString Is Nothing Then
                    Return New opCodeImmediate(GetType(String), String.Empty)
                Else
                    Return curString
                End If
            Else
                Dim lastString As opCode = New opCodeImmediate(GetType(String), value.ToString)
                If curString Is Nothing Then
                    Return lastString
                Else
                    Return New opCodeBinary(Me, curString, eTokenType.operator_concat, lastString)
                End If
            End If
        End If
        Return Nothing
    End Function

    Private Sub ParseDate()
        NextChar() ' eat the #
        Dim zone As Integer = 0
        While (mCurChar >= "0"c And mCurChar <= "9"c) Or (mCurChar = "/"c) Or (mCurChar = ":"c) Or (mCurChar = " "c)
            value.Append(mCurChar)
            NextChar()
        End While
        If mCurChar <> "#" Then
            RaiseError("Invalid date should be #dd/mm/yyyy#")
        Else
            NextChar()
        End If
        type = eTokenType.value_date
    End Sub

    Public Function RestOfLine() As String
        value.Length = 0
        Do While mCurChar <> vbCr And mCurChar <> vbLf And mCurChar <> Nothing
            value.Append(mCurChar)
            NextChar()
        Loop
        type = eTokenType.end_of_line
        Return value.ToString
    End Function

    Sub skipEndOfLineComment()
        RestOfLine()
        type = eTokenType.end_of_line
    End Sub

    Public Sub parseEndOfLine(Optional ByVal mandatory As Boolean = False)
        If type = eTokenType.end_of_line Or type = eTokenType.end_of_file Then
            While type = eTokenType.end_of_line
                NextToken()
            End While
        ElseIf mandatory Then
            Me.RaiseUnexpectedToken("End of line expected")
        End If
    End Sub
End Class
