Public Interface iEvalValue
    ReadOnly Property Value() As Object
    Event ValueChanged(ByVal Sender As Object, ByVal e As EventArgs)
   ReadOnly Property SystemType() As System.Type
End Interface

