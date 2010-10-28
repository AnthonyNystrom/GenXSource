
Public MustInherit Class opCode
   Implements iEvalValue

   Protected mValueDelegate As ValueDelegate
   Protected Delegate Function ValueDelegate() As Object

   Delegate Sub RunDelegate()

   Protected Sub New()

   End Sub

   Protected Sub RaiseEventValueChanged(ByVal sender As Object, ByVal e As EventArgs)
      RaiseEvent ValueChanged(sender, e)
   End Sub

   MustOverride ReadOnly Property SystemType() As System.Type Implements iEvalValue.SystemType

   Public Function CanReturn(ByVal type As System.Type) As Boolean
      Return True
   End Function


   Public Overridable ReadOnly Property value() As Object Implements iEvalValue.Value
      Get
         Return mValueDelegate()
      End Get
   End Property

   Protected Friend Sub Convert(ByVal tokenizer As tokenizer, ByRef param1 As opCode, ByVal SystemType As System.Type)
      If Not param1.SystemType Is SystemType Then
         If param1.CanReturn(SystemType) Then
            param1 = New opCodeConvert(tokenizer, param1, SystemType)
         Else
            tokenizer.RaiseError("Cannot convert " & param1.SystemType.Name & " into " & SystemType.Name)
         End If
      End If
   End Sub

   Protected Shared Sub ConvertToSystemType(ByRef param1 As iEvalValue, ByVal SystemType As Type)
      If Not param1.SystemType Is SystemType Then
         If SystemType Is GetType(Object) Then
            'ignore
         Else
            param1 = New opCodeSystemTypeConvert(param1, SystemType)
         End If
      End If
   End Sub

   Protected Sub SwapParams(ByRef Param1 As opCode, ByRef Param2 As opCode)
      Dim swp As opCode = Param1
      Param1 = Param2
      Param2 = swp
   End Sub

   Public Event ValueChanged(ByVal Sender As Object, ByVal e As System.EventArgs) Implements iEvalValue.ValueChanged
End Class

Friend Class opCodeVariable
   Inherits opCode

   WithEvents mVariable As EvalVariable

   Sub New(ByVal variable As EvalVariable)
      mVariable = variable
   End Sub

   Public Overrides ReadOnly Property Value() As Object
      Get
         Return mVariable
      End Get
   End Property

   Public Overrides ReadOnly Property SystemType() As System.Type
      Get
         Return mVariable.SystemType
      End Get
   End Property

   Private Sub mVariable_ValueChanged(ByVal Sender As Object, ByVal e As System.EventArgs) Handles mVariable.ValueChanged
      MyBase.RaiseEventValueChanged(Sender, e)
   End Sub
End Class

Friend Class opCodeImmediate
   Inherits opCode

   Private mValue As Object
   Private mSystemType As System.Type

   Sub New(ByVal SystemType As System.Type, ByVal value As Object)
      mSystemType = SystemType
      mValue = value
   End Sub

   Public Overrides ReadOnly Property Value() As Object
      Get
         Return mValue
      End Get
   End Property


   Public Overrides ReadOnly Property SystemType() As System.Type
      Get
         Return mSystemType
      End Get
   End Property
End Class

Friend Class opCodeUnary
   Inherits opCode

   WithEvents mParam1 As opCode
   Private mSystemType As System.Type

   Sub New(ByVal tt As eTokenType, ByVal param1 As opCode)
      mParam1 = param1
      Dim v1Type As System.Type = mParam1.SystemType

      Select Case tt
         Case eTokenType.operator_not
            If v1Type Is GetType(Boolean) Then
               mValueDelegate = AddressOf BOOLEAN_NOT
               mSystemType = GetType(Boolean)
            End If
         Case eTokenType.operator_minus
            If v1Type Is GetType(Double) Then
               mValueDelegate = AddressOf NUM_CHGSIGN
               mSystemType = GetType(Double)
            End If
      End Select
   End Sub

   Private Function BOOLEAN_NOT() As Object
      Return Not DirectCast(mParam1.value, Boolean)
   End Function

   Private Function NUM_CHGSIGN() As Object
      Return -DirectCast(mParam1.value, Double)
   End Function

   Public Overrides ReadOnly Property SystemType() As System.Type
      Get
         Return mSystemType
      End Get
   End Property

   Private Sub mParam1_ValueChanged(ByVal Sender As Object, ByVal e As System.EventArgs) Handles mParam1.ValueChanged
      MyBase.RaiseEventValueChanged(Sender, e)
   End Sub
End Class

Friend Class opCodeConvert
   Inherits opCode
   WithEvents mParam1 As iEvalValue
   Private mSystemType As System.Type

   Sub New(ByVal tokenizer As tokenizer, ByVal param1 As iEvalValue, ByVal SystemType As System.Type)
      mParam1 = param1
      If SystemType Is GetType(Boolean) Then
         mValueDelegate = AddressOf TBool
         mSystemType = GetType(Boolean)
      ElseIf SystemType Is GetType(Date) Then
         mValueDelegate = AddressOf TDate
         mSystemType = GetType(Date)
      ElseIf SystemType Is GetType(Double) Then
         mValueDelegate = AddressOf TNum
         mSystemType = GetType(Double)
      ElseIf SystemType Is GetType(String) Then
         mValueDelegate = AddressOf TStr
         mSystemType = GetType(String)
      Else
         tokenizer.RaiseError("Cannot convert " & param1.SystemType.Name & " to " & SystemType.Name)
      End If
   End Sub

   Private Function TBool() As Object
      Return CBool(mParam1.Value)
   End Function

   Private Function TDate() As Object
      Return CDate(mParam1.Value)
   End Function

   Private Function TNum() As Object
      Return CDbl(mParam1.Value)
   End Function

   Private Function TStr() As Object
      Return CStr(mParam1.Value)
   End Function

   Private Function TValue() As Object
      Return DirectCast(mParam1.Value, iEvalValue).Value
   End Function

   Public Overrides ReadOnly Property SystemType() As System.Type
      Get
         Return mSystemType
      End Get
   End Property

   Private Sub mParam1_ValueChanged(ByVal Sender As Object, ByVal e As System.EventArgs) Handles mParam1.ValueChanged
      MyBase.RaiseEventValueChanged(Sender, e)
   End Sub
End Class

Friend Class opCodeSystemTypeConvert
   Inherits opCode
   WithEvents mParam1 As iEvalValue
   Private mSystemType As System.Type

   Sub New(ByVal param1 As iEvalValue, ByVal Type As System.Type)
      mParam1 = param1
      mValueDelegate = AddressOf [CType]
      mSystemType = Type
   End Sub

   Private Function [CType]() As Object
      Return System.Convert.ChangeType(mParam1.Value, mSystemType)
   End Function

   Public Overrides ReadOnly Property systemType() As System.Type
      Get
         Return mSystemType
      End Get
   End Property

   Private Sub mParam1_ValueChanged(ByVal Sender As Object, ByVal e As System.EventArgs) Handles mParam1.ValueChanged
      MyBase.RaiseEventValueChanged(Sender, e)
   End Sub
End Class

Friend Class opCodeIf
   Inherits opCode

   WithEvents mParam1 As opCode
   WithEvents mParam2 As opCode
   WithEvents mParam3 As opCode
   Private mSystemType As System.Type

   Public Sub New(ByVal tokenizer As tokenizer, ByVal param1 As opCode, ByVal param2 As opCode, ByVal param3 As opCode)
      MyBase.Convert(tokenizer, param1, GetType(Boolean))
      mParam1 = param1
      mParam2 = param2
      mParam3 = param3
      If mParam2.SystemType Is mParam3.SystemType Then
         mSystemType = mParam2.SystemType
      Else
         tokenizer.RaiseError("The IF operator cannot return heterogeneous types.")
      End If
   End Sub

   Public Overrides ReadOnly Property Value() As Object
      Get
         Dim res As Object
            If mParam1.value.Equals(True) Then
                res = mParam2.value
            Else
                res = mParam3.value
            End If
            Return res
      End Get
   End Property

   Public Overrides ReadOnly Property SystemType() As System.Type
      Get
         Return mSystemType
      End Get
   End Property

   Private Sub mParam1_ValueChanged(ByVal Sender As Object, ByVal e As System.EventArgs) Handles mParam1.ValueChanged
      MyBase.RaiseEventValueChanged(Sender, e)
   End Sub

   Private Sub mParam2_ValueChanged(ByVal Sender As Object, ByVal e As System.EventArgs) Handles mParam2.ValueChanged
      MyBase.RaiseEventValueChanged(Sender, e)
   End Sub

   Private Sub mParam3_ValueChanged(ByVal Sender As Object, ByVal e As System.EventArgs) Handles mParam3.ValueChanged
      MyBase.RaiseEventValueChanged(Sender, e)
   End Sub
End Class

Friend Class opCodeBinary
   Inherits opCode

   WithEvents mParam1 As opCode
   WithEvents mParam2 As opCode
   Private mSystemType As System.Type

   Public Sub New(ByVal tokenizer As tokenizer, ByVal param1 As opCode, ByVal tt As eTokenType, ByVal param2 As opCode)
      mParam1 = param1
      mParam2 = param2

      Dim v1Type As System.Type = mParam1.SystemType
      Dim v2Type As System.Type = mParam2.SystemType

      Select Case tt
         Case eTokenType.operator_plus
            If v1Type Is GetType(Double) And v2Type Is GetType(Double) Then
               mValueDelegate = AddressOf NUM_PLUS_NUM
               mSystemType = GetType(Double)
            ElseIf v1Type Is GetType(Double) And v2Type Is GetType(Date) Then
               SwapParams(mParam1, mParam2)
               mValueDelegate = AddressOf DATE_PLUS_NUM
               mSystemType = GetType(Date)
            ElseIf v1Type Is GetType(Date) And v2Type Is GetType(Double) Then
               mValueDelegate = AddressOf DATE_PLUS_NUM
               mSystemType = GetType(Date)
            ElseIf mParam1.CanReturn(GetType(String)) And mParam2.CanReturn(GetType(String)) Then
               Convert(tokenizer, mParam1, GetType(String))
               Convert(tokenizer, mParam2, GetType(String))
               mValueDelegate = AddressOf STR_CONCAT_STR
               mSystemType = GetType(String)
            End If
         Case eTokenType.operator_minus
            If v1Type Is GetType(Double) And v2Type Is GetType(Double) Then
               mValueDelegate = AddressOf NUM_MINUS_NUM
               mSystemType = GetType(Double)
            ElseIf v1Type Is GetType(Date) And v2Type Is GetType(Double) Then
               mValueDelegate = AddressOf DATE_MINUS_NUM
               mSystemType = GetType(Date)
            ElseIf v1Type Is GetType(Date) And v2Type Is GetType(Date) Then
               mValueDelegate = AddressOf DATE_MINUS_DATE
               mSystemType = GetType(Double)
            End If
         Case eTokenType.operator_mul
            If v1Type Is GetType(Double) And v2Type Is GetType(Double) Then
               mValueDelegate = AddressOf NUM_MUL_NUM
               mSystemType = GetType(Double)
            End If
         Case eTokenType.operator_div
            If v1Type Is GetType(Double) And v2Type Is GetType(Double) Then
               mValueDelegate = AddressOf NUM_DIV_NUM
               mSystemType = GetType(Double)
            End If
         Case eTokenType.operator_percent
            If v1Type Is GetType(Double) And v2Type Is GetType(Double) Then
               mValueDelegate = AddressOf NUM_PERCENT_NUM
               mSystemType = GetType(Double)
            End If
         Case eTokenType.operator_and, eTokenType.operator_or
            Convert(tokenizer, mParam1, GetType(Boolean))
            Convert(tokenizer, mParam2, GetType(Boolean))
            Select Case tt
               Case eTokenType.operator_or
                  mValueDelegate = AddressOf BOOL_OR_BOOL
                  mSystemType = GetType(Boolean)
               Case eTokenType.operator_and
                  mValueDelegate = AddressOf BOOL_AND_BOOL
                  mSystemType = GetType(Boolean)
            End Select
         Case eTokenType.operator_concat
            Convert(tokenizer, mParam1, GetType(String))
            Convert(tokenizer, mParam2, GetType(String))
            mValueDelegate = AddressOf STR_CONCAT_STR

         Case eTokenType.operator_eq, eTokenType.operator_ge, eTokenType.operator_gt, eTokenType.operator_le, eTokenType.operator_lt, eTokenType.operator_ne
            If v1Type Is GetType(Double) Or v2Type Is GetType(Double) Then
               Convert(tokenizer, mParam1, GetType(Double))
               Convert(tokenizer, mParam2, GetType(Double))
               Select Case tt
                  Case eTokenType.operator_eq : mValueDelegate = AddressOf NUM_EQ_NUM
                  Case eTokenType.operator_ge : mValueDelegate = AddressOf NUM_GE_NUM
                  Case eTokenType.operator_gt : mValueDelegate = AddressOf NUM_GT_NUM
                  Case eTokenType.operator_le : mValueDelegate = AddressOf NUM_LE_NUM
                  Case eTokenType.operator_lt : mValueDelegate = AddressOf NUM_LT_NUM
                  Case eTokenType.operator_ne : mValueDelegate = AddressOf NUM_NE_NUM
               End Select
               mSystemType = GetType(Boolean)
            ElseIf mParam1.CanReturn(GetType(String)) And mParam2.CanReturn(GetType(String)) Then
               Convert(tokenizer, mParam1, GetType(String))
               Convert(tokenizer, mParam1, GetType(String))
               Select Case tt
                  Case eTokenType.operator_eq : mValueDelegate = AddressOf STR_EQ_STR
                  Case eTokenType.operator_ge : mValueDelegate = AddressOf STR_GE_STR
                  Case eTokenType.operator_gt : mValueDelegate = AddressOf STR_GT_STR
                  Case eTokenType.operator_le : mValueDelegate = AddressOf STR_LE_STR
                  Case eTokenType.operator_lt : mValueDelegate = AddressOf STR_LT_STR
                  Case eTokenType.operator_ne : mValueDelegate = AddressOf STR_NE_STR
               End Select

               mSystemType = GetType(Boolean)
            End If

      End Select

      If mValueDelegate Is Nothing Then
         tokenizer.RaiseError( _
            "Cannot apply the operator " & tt.ToString.Replace("operator_", "") & _
            " on " & v1Type.ToString & _
            " and " & v2Type.ToString)
      End If
   End Sub

   Private Function BOOL_AND_BOOL() As Object
      Return DirectCast(mParam1.value, Boolean) And DirectCast(mParam2.value, Boolean)
   End Function

   Private Function BOOL_OR_BOOL() As Object
      Return DirectCast(mParam1.value, Boolean) Or DirectCast(mParam2.value, Boolean)
   End Function

   Private Function BOOL_XOR_BOOL() As Object
      Return DirectCast(mParam1.value, Boolean) Xor DirectCast(mParam2.value, Boolean)
   End Function

   Private Function NUM_EQ_NUM() As Object
      Return DirectCast(mParam1.value, Double) = DirectCast(mParam2.value, Double)
   End Function

   Private Function NUM_LT_NUM() As Object
      Return DirectCast(mParam1.value, Double) < DirectCast(mParam2.value, Double)
   End Function

   Private Function NUM_GT_NUM() As Object
      Return DirectCast(mParam1.value, Double) > DirectCast(mParam2.value, Double)
   End Function

   Private Function NUM_GE_NUM() As Object
      Return DirectCast(mParam1.value, Double) >= DirectCast(mParam2.value, Double)
   End Function

   Private Function NUM_LE_NUM() As Object
      Return DirectCast(mParam1.value, Double) <= DirectCast(mParam2.value, Double)
   End Function

   Private Function NUM_NE_NUM() As Object
      Return DirectCast(mParam1.value, Double) <> DirectCast(mParam2.value, Double)
   End Function

   Private Function NUM_PLUS_NUM() As Object
      Return DirectCast(mParam1.value, Double) + DirectCast(mParam2.value, Double)
   End Function

   Private Function NUM_MUL_NUM() As Object
      Return DirectCast(mParam1.value, Double) * DirectCast(mParam2.value, Double)
   End Function

   Private Function NUM_MINUS_NUM() As Object
      Return DirectCast(mParam1.value, Double) - DirectCast(mParam2.value, Double)
   End Function

   Private Function DATE_PLUS_NUM() As Object
      Return DirectCast(mParam1.value, Date).AddDays(DirectCast(mParam2.value, Double))
   End Function

   Private Function DATE_MINUS_DATE() As Object
      Return DirectCast(mParam1.value, Date).Subtract(DirectCast(mParam2.value, Date)).TotalDays
   End Function

   Private Function DATE_MINUS_NUM() As Object
      Return DirectCast(mParam1.value, Date).AddDays(-DirectCast(mParam2.value, Double))
   End Function

   Private Function STR_CONCAT_STR() As Object
      Return mParam1.value.ToString & mParam2.value.ToString
   End Function

   Private Function STR_EQ_STR() As Object
      Return DirectCast(mParam1.value, String) = DirectCast(mParam2.value, String)
   End Function

   Private Function STR_LT_STR() As Object
      Return DirectCast(mParam1.value, String) < DirectCast(mParam2.value, String)
   End Function

   Private Function STR_GT_STR() As Object
      Return DirectCast(mParam1.value, String) > DirectCast(mParam2.value, String)
   End Function

   Private Function STR_GE_STR() As Object
      Return DirectCast(mParam1.value, String) >= DirectCast(mParam2.value, String)
   End Function

   Private Function STR_LE_STR() As Object
      Return DirectCast(mParam1.value, String) <= DirectCast(mParam2.value, String)
   End Function

   Private Function STR_NE_STR() As Object
      Return DirectCast(mParam1.value, String) <> DirectCast(mParam2.value, String)
   End Function

   Private Function NUM_DIV_NUM() As Object
      Return DirectCast(mParam1.value, Double) / DirectCast(mParam2.value, Double)
   End Function

   Private Function NUM_PERCENT_NUM() As Object
      Return DirectCast(mParam2.value, Double) * (DirectCast(mParam1.value, Double) / 100)
   End Function

   Public Overrides ReadOnly Property SystemType() As System.Type
      Get
         Return mSystemType
      End Get
   End Property

   Private Sub mParam1_ValueChanged(ByVal Sender As Object, ByVal e As System.EventArgs) Handles mParam1.ValueChanged
      MyBase.RaiseEventValueChanged(Sender, e)
   End Sub

   Private Sub mParam2_ValueChanged(ByVal Sender As Object, ByVal e As System.EventArgs) Handles mParam2.ValueChanged
      MyBase.RaiseEventValueChanged(Sender, e)
   End Sub
End Class

Public Class opCodeGetVariable
   Inherits opCode

   WithEvents mParam1 As iEvalValue

   Sub New(ByVal value As iEvalValue)
      mParam1 = value
   End Sub


   Public Overrides ReadOnly Property Value() As Object
      Get
         Return mParam1.Value
      End Get
   End Property

   Public Overrides ReadOnly Property SystemType() As System.Type
      Get
         Return mParam1.SystemType
      End Get
   End Property

   Private Sub mParam1_ValueChanged(ByVal Sender As Object, ByVal e As System.EventArgs) Handles mParam1.ValueChanged
      MyBase.RaiseEventValueChanged(Sender, e)
   End Sub

End Class

Public Class opCodeCallMethod
   Inherits opCode

   Private mBaseObject As Object
   Private mBaseSystemType As System.Type
   WithEvents mBaseValue As iEvalValue  ' for the events only
   Private mBaseValueObject As Object

   Private mMethod As System.Reflection.MemberInfo
   Private mParams As iEvalValue()
   Private mParamValues As Object()

   Private mResultSystemType As System.Type
   WithEvents mResultValue As iEvalValue  ' just for some

   Friend Sub New(ByVal baseObject As Object, ByVal method As System.Reflection.MemberInfo, ByVal params As IList)
      If params Is Nothing Then params = New iEvalValue() {}
      Dim paramInfo() As Reflection.ParameterInfo = Nothing

      If TypeOf method Is Reflection.PropertyInfo Then
         With DirectCast(method, Reflection.PropertyInfo)
            mResultSystemType = DirectCast(method, Reflection.PropertyInfo).PropertyType
            paramInfo = .GetIndexParameters()
         End With
         mValueDelegate = AddressOf GetProperty
      ElseIf TypeOf method Is Reflection.MethodInfo Then
         With DirectCast(method, Reflection.MethodInfo)
            mResultSystemType = .ReturnType
            paramInfo = .GetParameters()
         End With
         mValueDelegate = AddressOf GetMethod
      ElseIf TypeOf method Is Reflection.FieldInfo Then
         With DirectCast(method, Reflection.FieldInfo)
            mResultSystemType = .FieldType
            paramInfo = New Reflection.ParameterInfo() {}
         End With
         mValueDelegate = AddressOf GetField
      End If

      Dim newParams(paramInfo.Length - 1) As iEvalValue
      Dim newParamValues(paramInfo.Length - 1) As Object

      params.CopyTo(newParams, 0)

      For i As Integer = params.Count To paramInfo.Length - 1
         Dim param As Reflection.ParameterInfo = paramInfo(i)
         Dim defValue As Object = param.DefaultValue
         newParams(i) = New EvalVariable(defValue, defValue.GetType)
      Next

      For Each p As iEvalValue In newParams
         AddHandler p.ValueChanged, AddressOf mParamsValueChanged
      Next

      mParams = newParams
      mParamValues = newParamValues
      mBaseObject = baseObject
      mMethod = method

      If TypeOf mBaseObject Is iEvalValue Then
         If TypeOf mBaseObject Is iEvalValue Then
            With DirectCast(mBaseObject, iEvalValue)
               mBaseSystemType = .SystemType
               mBaseSystemType = .SystemType
            End With
         Else
            mBaseSystemType = mBaseObject.GetType()
         End If
      Else
         mBaseSystemType = mBaseObject.GetType
      End If

      For i As Integer = 0 To mParams.Length - 1
         If i < paramInfo.Length Then
            ConvertToSystemType(mParams(i), paramInfo(i).ParameterType)
         End If
      Next

      If GetType(iEvalValue).IsAssignableFrom(mResultSystemType) Then
         mResultValue = DirectCast(InternalValue(), iEvalValue)
         If TypeOf mResultValue Is iEvalValue Then
            With DirectCast(mResultValue, iEvalValue)
               mResultSystemType = .SystemType
            End With
         ElseIf mResultValue Is Nothing Then
            mResultSystemType = GetType(Object)
         Else
            Dim v As Object = mResultValue.Value
            If v Is Nothing Then
               mResultSystemType = GetType(Object)
            Else
               mResultSystemType = v.GetType
            End If
         End If
      Else
         mResultSystemType = SystemType
      End If
   End Sub

   Protected Friend Shared Function GetNew(ByVal tokenizer As tokenizer, ByVal baseObject As Object, ByVal method As System.Reflection.MemberInfo, ByVal params As IList) As opCode
      Dim o As opCode
      o = New opCodeCallMethod(baseObject, method, params)
      Dim newType As Type = Evaluator.ShouldConvert(o.SystemType)
      If newType Is Nothing Then
         Return o
      Else
         Return New opCodeConvert(tokenizer, o, newType)
      End If
   End Function

   Private Function GetProperty() As Object
      Dim res As Object = DirectCast(mMethod, Reflection.PropertyInfo).GetValue(mBaseValueObject, mParamValues)
      Return res
   End Function

   Private Function GetMethod() As Object
      Dim res As Object = DirectCast(mMethod, Reflection.MethodInfo).Invoke(mBaseValueObject, mParamValues)
      Return res
   End Function

   Private Function GetField() As Object
      Dim res As Object = DirectCast(mMethod, Reflection.FieldInfo).GetValue(mBaseValueObject)
      Return res
   End Function

   Private Function InternalValue() As Object
      For i As Integer = 0 To mParams.Length - 1
         mParamValues(i) = mParams(i).Value
      Next
      If TypeOf mBaseObject Is iEvalValue Then
         mBaseValue = DirectCast(mBaseObject, iEvalValue)
         mBaseValueObject = mBaseValue.Value
      Else
         mBaseValueObject = mBaseObject
      End If
      Return MyBase.mValueDelegate()
   End Function

   Public Overrides ReadOnly Property Value() As Object
      Get
         Dim res As Object = InternalValue()
         If TypeOf res Is iEvalValue Then
            mResultValue = DirectCast(res, iEvalValue)
            res = mResultValue.Value
         End If
         Return res
      End Get
   End Property

   Public Overrides ReadOnly Property SystemType() As System.Type
      Get
         Return mResultSystemType
      End Get
   End Property

   Private Sub mParamsValueChanged(ByVal Sender As Object, ByVal e As System.EventArgs)
      MyBase.RaiseEventValueChanged(Sender, e)
   End Sub

   Private Sub mBaseVariable_ValueChanged(ByVal Sender As Object, ByVal e As System.EventArgs) Handles mBaseValue.ValueChanged
      MyBase.RaiseEventValueChanged(Sender, e)
   End Sub

   Private Sub mResultVariable_ValueChanged(ByVal Sender As Object, ByVal e As System.EventArgs) Handles mResultValue.ValueChanged
      MyBase.RaiseEventValueChanged(Sender, e)
   End Sub
End Class

Public Class opCodeGetArrayEntry
   Inherits opCode

   WithEvents mArray As opCode

   Private mParams As iEvalValue()
   Private mValues As Integer()
   Private mResultSystemType As System.Type

   Public Sub New(ByVal array As opCode, ByVal params As IList)
      Dim newParams(params.Count - 1) As iEvalValue
      Dim newValues(params.Count - 1) As Integer
      params.CopyTo(newParams, 0)
      mArray = array
      mParams = newParams
      mValues = newValues
      mResultSystemType = array.SystemType.GetElementType
   End Sub

   Public Overrides ReadOnly Property Value() As Object
      Get
         Dim res As Object
         Dim arr As Array = DirectCast(mArray.value, Array)
         For i As Integer = 0 To mValues.Length - 1
            mValues(i) = CInt(mParams(i).Value)
         Next
         res = arr.GetValue(mValues)
         Return res
      End Get
   End Property

   Public Overrides ReadOnly Property SystemType() As System.Type
      Get
         Return mResultSystemType
      End Get
   End Property

   Private Sub mBaseVariable_ValueChanged(ByVal Sender As Object, ByVal e As System.EventArgs) Handles mArray.ValueChanged
      MyBase.RaiseEventValueChanged(Sender, e)
   End Sub

End Class


