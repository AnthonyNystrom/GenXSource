Namespace IChartAdapter

    Public Interface IChartAdapter
        Function Validate(ByVal obj As Object) As Boolean
        Sub SetChartControl(ByVal chartControl As Object)
        ReadOnly Property ChartControl() As Object
        Event SizeChanged As EventHandler
        Event ScopeChanged As EventHandler
    End Interface

End Namespace
