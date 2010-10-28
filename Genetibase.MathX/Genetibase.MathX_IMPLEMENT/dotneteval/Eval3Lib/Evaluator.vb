Imports System.Reflection
Imports System.Data

Public Class Evaluator
   Friend mEnvironmentFunctionsList As ArrayList
   Public iEvalValue As Boolean
   Public ReadOnly Syntax As eParserSyntax
   Public ReadOnly CaseSensitive As Boolean

   Sub New(Optional ByVal syntax As eParserSyntax = eParserSyntax.Vb, Optional ByVal caseSensitive As Boolean = False)
      Me.Syntax = Syntax
      Me.CaseSensitive = CaseSensitive
      mEnvironmentFunctionsList = New ArrayList
   End Sub

   Public Sub AddEnvironmentFunctions(ByVal obj As Object)
      If obj Is Nothing Then Exit Sub
      If Not mEnvironmentFunctionsList.Contains(obj) Then
         mEnvironmentFunctionsList.Add(obj)
      End If
   End Sub

   Public Sub RemoveEnvironmentFunctions(ByVal obj As Object)
      If mEnvironmentFunctionsList.Contains(obj) Then
         mEnvironmentFunctionsList.Remove(obj)
      End If
   End Sub

   Public Function Parse(ByVal str As String) As opCode
      Dim parser As New parser(Me)
      Dim tokenizer As New tokenizer(parser, Syntax)
      Return parser.Parse(str, tokenizer)
   End Function

   Public Function ParseText(ByVal str As String) As opCode
      Dim parser As New parser(Me)
      Dim tokenizer As New tokenizer(parser, Syntax)
      Return parser.ParseText(str, tokenizer)
   End Function


   Public Shared Function ConvertToString(ByVal value As Object) As String
      If TypeOf value Is String Then
         Return DirectCast(value, String)
      ElseIf value Is Nothing Then
         Return String.Empty
      ElseIf TypeOf value Is Date Then
         Dim d As Date = DirectCast(value, Date)
         If d.TimeOfDay.TotalMilliseconds > 0 Then
            Return d.ToString
         Else
            Return d.ToShortDateString()
         End If
      ElseIf TypeOf value Is Decimal Then
         Dim d As Decimal = DirectCast(value, Decimal)
         If (d Mod 1) <> 0 Then
            Return d.ToString("#,##0.00")
         Else
            Return d.ToString("#,##0")
         End If
      ElseIf TypeOf value Is Double Then
         Dim d As Double = DirectCast(value, Double)
         If (d Mod 1) <> 0 Then
            Return d.ToString("#,##0.00")
         Else
            Return d.ToString("#,##0")
         End If
      ElseIf TypeOf value Is Object Then
         Return value.ToString
      End If
      Return String.Empty
   End Function

   Public Class parserException
      Inherits Exception
      Public ReadOnly formula As String
      Public ReadOnly pos As Integer

      Friend Sub New(ByVal str As String, ByVal formula As String, ByVal line As Integer, ByVal column As Integer)
         MyBase.New(str)
         Me.formula = formula
         Me.pos = pos
      End Sub

   End Class



   Friend Shared Function varEq(ByVal v1 As String, ByVal v2 As String) As Boolean
      Dim lv1, lv2 As Integer
      If v1 Is Nothing Then lv1 = 0 Else lv1 = v1.Length
      If v2 Is Nothing Then lv2 = 0 Else lv2 = v2.Length

      If lv1 <> lv2 Then Return False
      If lv1 = 0 Then Return True

      Dim c1, c2 As Char

      For i As Integer = 0 To lv1 - 1
         c1 = v1.Chars(i)
         c2 = v2.Chars(i)
         Select Case c1
            Case "a"c To "z"c
               If c2 <> c1 AndAlso Asc(c2) <> (Asc(c1) - 32) Then
                  Return False
               End If
            Case "A"c To "Z"c
               If c2 <> c1 AndAlso Asc(c2) <> (Asc(c1) + 32) Then
                  Return False
               End If
            Case "-"c, "_"c, "."c
               If c2 <> c1 AndAlso c2 <> "_"c AndAlso c2 <> "."c Then
                  Return False
               End If
            Case "_"c
               If c2 <> c1 AndAlso c2 <> "-"c Then
                  Return False
               End If
            Case Else
               If c2 <> c1 Then Return False
         End Select
      Next
      Return True
   End Function

   Friend Shared Function ShouldConvert(ByVal t As Type) As Type
      If t Is GetType(Single) _
              Or t Is GetType(Double) _
              Or t Is GetType(Decimal) _
              Or t Is GetType(Int16) _
              Or t Is GetType(Int32) _
              Or t Is GetType(Int64) _
              Or t Is GetType(Byte) _
              Or t Is GetType(UInt16) _
              Or t Is GetType(UInt32) _
              Or t Is GetType(UInt64) _
              Then
         Return GetType(Double) ' GetType(Double
      ElseIf t Is GetType(Date) Then
         Return GetType(Date) 'GetType(Date
      ElseIf t Is GetType(Boolean) Then
         Return GetType(Boolean) 'GetType(Boolean
      ElseIf t Is GetType(String) Then
         Return GetType(String) 'GetType(String
      Else
         Return Nothing 'GetType(Object
      End If
   End Function

   Friend Shared Function FindVariableInBag(ByVal bag As iVariableBag, ByVal varname As String) As iEvalValue
      Return bag.GetVariable(varname)
   End Function

   Public Shared Function GetNativeType(ByVal t As String) As Type

      Select Case t
         Case "number"
            Return GetType(Double)
         Case "string"
            Return GetType(String)
         Case "boolean"
            Return GetType(Boolean)
         Case "date", "datetime"
            Return GetType(Date)
         Case Else
            Throw New Exception("The type " & t & " is not a script engine native type")
      End Select

   End Function

   Public Shared Function GetDefaultValue(ByVal t As Type) As Object
      If t Is GetType(Double) Then
         Return 0.0
      ElseIf t Is GetType(String) Then
         Return String.Empty
      ElseIf t Is GetType(Boolean) Then
         Return False
      ElseIf t Is GetType(Date) Then
         Return New Date
      Else
         Return Nothing
      End If
   End Function
End Class
