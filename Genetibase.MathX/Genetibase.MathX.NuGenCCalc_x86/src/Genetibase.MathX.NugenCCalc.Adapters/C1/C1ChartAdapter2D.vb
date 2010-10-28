Imports System
Imports System.Reflection
Imports System.Collections
Imports System.Drawing

Imports Genetibase.MathX.Core
Imports Genetibase.MathX.NugenCCalc.Adapters.IChartAdapter

Namespace C1



    <ToolboxBitmap(GetType(C1ChartAdapter2D), "Win.C1Chart.C1Chart.bmp")> _
    Public Class C1ChartAdapter2D
        Implements IChartAdapter2D, IChartAdapter.IChartAdapter

        Public Event ScopeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Implements IChartAdapter.IChartAdapter2D.ScopeChanged

        Public Event SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Implements IChartAdapter.IChartAdapter2D.SizeChanged

#Region "Constants"
        'Chart2DTypeEnum

        Const Chart2DTypeEnumPolar = 4
        Const Chart2DTypeEnumXYPlot = 0
        'LinePatternEnum
        Const LinePatternEnumNone = 5


        'SymbolShapeEnum
        Const SymbolShapeEnumDot = 2
#End Region

#Region "Private Fields"

        Private _chart
        Private _maxX As Double
        Private _maxY As Double
        Private _minX As Double
        Private _minY As Double
        Private _plotAreaSize As Size
        Private _scaleX As Double
        Private _scaleY As Double
#End Region

#Region "Public Properties"

        Public ReadOnly Property MaxX() As Double Implements IChartAdapter.IChartAdapter2D.MaxX
            Get
                Return Me._maxX
            End Get
        End Property

        Public ReadOnly Property MaxY() As Double Implements IChartAdapter.IChartAdapter2D.MaxY
            Get
                Return Me._maxY
            End Get
        End Property

        Public ReadOnly Property MinX() As Double Implements IChartAdapter.IChartAdapter2D.MinX
            Get
                Return Me._minX
            End Get
        End Property

        Public ReadOnly Property MinY() As Double Implements IChartAdapter.IChartAdapter2D.MinY
            Get
                Return Me._minY
            End Get
        End Property


        Public ReadOnly Property ChartControl() As Object Implements IChartAdapter.IChartAdapter2D.ChartControl
            Get
                Return Me._chart
            End Get
        End Property


        Public ReadOnly Property PlotAreaSize() As System.Drawing.Size Implements IChartAdapter.IChartAdapter2D.PlotAreaSize
            Get
                Dim areaSize As Size
                areaSize = CType(Me._chart.ChartArea.Size, Size)
                If (areaSize.Equals(New Size(-1, -1))) Then
                    Return Me._chart.Size
                End If
                Return Me._chart.ChartArea.Size
            End Get
        End Property
#End Region





        Public Sub New()


        End Sub

        Public Sub New(ByVal c1Chart As Object)
            _chart = c1Chart
            Me.SetChartControl(c1Chart)
        End Sub





        Private Sub _chart_PaintPlotArea(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)
            Dim size1 As Size = Me.PlotAreaSize
            If (Not Me._plotAreaSize.Equals(size1)) Then
                Me._plotAreaSize = size1
                Me.OnSizeChanged()
            End If
            Dim num1 As Double = Me._chart.ChartArea.AxisX.Min
            Dim num2 As Double = Me._chart.ChartArea.AxisX.Max
            Dim num3 As Double = Me._chart.ChartArea.AxisY.Min
            Dim num4 As Double = Me._chart.ChartArea.AxisY.Max
            Dim num5 As Double = Me._chart.ChartArea.AxisX.ScrollBar.Scale
            Dim num6 As Double = Me._chart.ChartArea.AxisY.ScrollBar.Scale
            If ((((Me._minX <> num1) OrElse (Me._maxX <> num2)) OrElse ((Me._minY <> num3) OrElse (Me._maxY <> num4))) OrElse ((Me._scaleX <> num5) OrElse (Me._scaleY <> num6))) Then
                Me._minX = num1
                Me._maxX = num2
                Me._minY = num3
                Me._maxY = num4
                Me._scaleX = num5
                Me._scaleY = num6
                Me.OnScopeChanged()
            End If
        End Sub


        Private Sub OnScopeChanged()
            RaiseEvent ScopeChanged(Me, New EventArgs)
        End Sub


        Private Sub OnSizeChanged()
            RaiseEvent SizeChanged(Me, New EventArgs)
        End Sub






        Private Function SeriesToSeriesIndex(ByVal series As Series) As Integer
            Dim num1 As Integer = -1
            Dim num2 As Integer
            For num2 = 0 To Me._chart.ChartGroups.Item(0).ChartData.SeriesList.Count - 1
                If (Me._chart.ChartGroups.Item(0).ChartData.SeriesList.Item(num2).Label Is Series.Label) Then
                    num1 = num2
                    series.Index = num2
                    Exit For
                End If
            Next num2
            If (num1 = -1) Then
                Dim series1 = [Assembly].LoadWithPartialName("C1.Win.C1Chart, Culture=neutral, PublicKeyToken=a22e16972c085838").CreateInstance("C1.Win.C1Chart.ChartDataSeries")
                series1.Label = seriesName
                series1.LineStyle.Pattern = LinePatternEnumNone
                series1.SymbolStyle.Shape = SymbolShapeEnumDot
                series1.SymbolStyle.Size = 1
                series1.Label = series.Label
                series1.LineStyle.Color = series.Color
                series1.SymbolStyle.Color = series.Color
                'series1.LineStyle.Pattern = 0
                num1 = Me._chart.ChartGroups.Item(0).ChartData.SeriesList.Add(series1)
            End If
            Return num1
        End Function


        Private Function SeriesNameToSeriesIndex(ByVal seriesName As String) As Integer
            Dim num1 As Integer = -1
            Dim num2 As Integer
            For num2 = 0 To Me._chart.ChartGroups.Item(0).ChartData.SeriesList.Count - 1
                If (Me._chart.ChartGroups.Item(0).ChartData.SeriesList.Item(num2).Label Is seriesName) Then
                    num1 = num2
                    Exit For
                End If
            Next num2
            If (num1 = -1) Then
                Dim series1 = [Assembly].LoadWithPartialName("C1.Win.C1Chart, Culture=neutral, PublicKeyToken=a22e16972c085838").CreateInstance("C1.Win.C1Chart.ChartDataSeries")
                series1.Label = seriesName
                series1.LineStyle.Pattern = LinePatternEnumNone
                series1.SymbolStyle.Shape = SymbolShapeEnumDot
                series1.SymbolStyle.Size = 1
                num1 = Me._chart.ChartGroups.Item(0).ChartData.SeriesList.Add(series1)
            End If
            Return num1
        End Function





        Private Sub DataToSeries(ByVal series As Object, ByVal points As Point2D())
            Dim pointdArray1 As Point2D() = points
            Dim num1 As Integer
            Dim xArray As Array = Array.CreateInstance(GetType(Double), pointdArray1.Length)
            Dim yArray As Array = Array.CreateInstance(GetType(Double), pointdArray1.Length)

            For num1 = 0 To pointdArray1.Length - 1
                Dim pointd1 As Point2D = pointdArray1(num1)
                xArray(num1) = pointd1.X
                yArray(num1) = pointd1.Y
            Next num1
            series.X.CopyDataIn(xArray)
            series.Y.CopyDataIn(yArray)
        End Sub


        Private Sub DoubleArrayIntoSeries(ByVal doubleArray As Double(), ByVal series As Object)
            Dim num1 As Double = 1
            Dim num2 As Double = Me._chart.ChartArea.AxisX.Min
            Dim num3 As Double = Me._chart.ChartArea.AxisX.Max
            If (num3 <> num2) Then
                num1 = ((num3 - num2) / CType((doubleArray.Length - 1), Double))
            End If
            Dim xArray As Array = Array.CreateInstance(GetType(Double), doubleArray.Length)
            Dim yArray As Array = Array.CreateInstance(GetType(Double), doubleArray.Length)
            Dim num4 As Integer
            For num4 = 0 To doubleArray.Length - 1
                xArray(num4) = ((num4 * num1) + num2)
                yArray(num4) = doubleArray(num4)
            Next num4
            series.X.CopyDataIn(xArray)
            series.Y.CopyDataIn(yArray)
        End Sub


        Public Sub ClearChartSeries() Implements IChartAdapter.IChartAdapter2D.ClearChartSeries
            Me._chart.ChartGroups.Item(0).ChartData.SeriesList.RemoveAll()
        End Sub


        Public Function Validate(ByVal obj As Object) As Boolean Implements IChartAdapter.IChartAdapter.Validate
            If (obj.GetType.Name = "C1Chart" And obj.GetType.Assembly.ImageRuntimeVersion(1) = "1") Then
                Return True
            End If
            Return False
        End Function

        Public Function GetSeriesNames() As String() Implements IChartAdapter.IChartAdapter2D.GetSeriesNames
            Dim list1 As New ArrayList
            Dim series1
            For Each series1 In Me._chart.ChartGroups.Item(0).ChartData.SeriesList
                list1.Add(series1.Label)
            Next
            Return CType(list1.ToArray(GetType(String)), String())
        End Function

        Public Function GetSeries() As Series() Implements IChartAdapter.IChartAdapter2D.GetSeries
            Dim list1 As New ArrayList
            Dim index As Integer
            index = 0
            For Each series1 In Me._chart.ChartGroups.Item(0).ChartData.SeriesList
                list1.Add(New Series(index, series1.Label, series1.LineStyle.Color))
                index = index + 1
            Next
            Return CType(list1.ToArray(GetType(Series)), Series())
        End Function

        Public Sub SetChartControl(ByVal chartControl As Object) Implements IChartAdapter.IChartAdapter2D.SetChartControl
            If Not Me.Validate(chartControl) Then
                Throw New ArgumentException("Parameter is not valid.", "chartControl")
            End If
            Me._chart = chartControl
            Me._minX = Me._chart.ChartArea.AxisX.Min
            Me._maxX = Me._chart.ChartArea.AxisX.Max
            Me._minY = Me._chart.ChartArea.AxisY.Min
            Me._maxY = Me._chart.ChartArea.AxisY.Max
            Me._scaleX = Me._chart.ChartArea.AxisX.ScrollBar.Scale
            Me._scaleY = Me._chart.ChartArea.AxisY.ScrollBar.Scale
            Me._plotAreaSize = Me.PlotAreaSize

            Dim chartType As Type = Me._chart.GetType()
            chartType.GetEvent("PaintPlotArea").AddEventHandler(_chart, New System.Windows.Forms.PaintEventHandler(AddressOf Me._chart_PaintPlotArea))
        End Sub

        Public Sub SetAxes(ByVal minX As Double, ByVal maxX As Double, ByVal minY As Double, ByVal maxY As Double) Implements IChartAdapter.IChartAdapter2D.SetAxes
            Me._chart.ChartArea.AxisX.Min = minX
            Me._chart.ChartArea.AxisX.Max = maxX
            Me._chart.ChartArea.AxisY.Min = minY
            Me._chart.ChartArea.AxisY.Max = maxY
            Me._minX = minX
            Me._maxX = maxX
            Me._minY = minY
            Me._maxY = maxY
        End Sub

        Public Overloads Sub Plot(ByVal values() As Double, ByVal seriesIndex As Integer) Implements IChartAdapter.IChartAdapter2D.Plot
            Dim series1
            Me._chart.ChartGroups.Item(0).ChartType = Chart2DTypeEnumXYPlot
            If ((seriesIndex >= 0) AndAlso (seriesIndex < Me._chart.ChartGroups.Item(0).ChartData.SeriesList.Count)) Then
                series1 = Me._chart.ChartGroups.Item(0).ChartData.SeriesList.Item(seriesIndex)
                series1.PointData.Clear()
                Me.DoubleArrayIntoSeries(values, series1)
            Else
                series1 = [Assembly].LoadWithPartialName("C1.Win.C1Chart, Culture=neutral, PublicKeyToken=a22e16972c085838").CreateInstance("C1.Win.C1Chart.ChartDataSeries")
                series1.LineStyle.Pattern = LinePatternEnumNone
                series1.SymbolStyle.Shape = SymbolShapeEnumDot
                series1.SymbolStyle.Size = 1
                Me._chart.ChartGroups.Item(0).ChartData.SeriesList.Add(series1)
                Me.DoubleArrayIntoSeries(values, series1)
            End If
        End Sub

        Public Overloads Function Plot(ByVal values() As Double, ByVal seriesName As String) As Integer Implements IChartAdapter.IChartAdapter2D.Plot
            Dim num1 As Integer = Me.SeriesNameToSeriesIndex(seriesName)
            Me.Plot(values, num1)
            Return num1
        End Function


        Public Overloads Sub Plot(ByVal values() As Double, ByVal series As Series) Implements IChartAdapter.IChartAdapter2D.Plot

            Dim num1 As Integer = Me.SeriesToSeriesIndex(series)
            Me.Plot(values, num1)

        End Sub

        Public Overloads Sub Plot(ByVal points() As Core.Point2D, ByVal series As Series) Implements IChartAdapter.IChartAdapter2D.Plot
            Dim num1 As Integer = Me.SeriesToSeriesIndex(series)
            Me.Plot(points, num1)
        End Sub

        Public Overloads Sub Plot(ByVal points() As Core.Point2D, ByVal seriesIndex As Integer) Implements IChartAdapter.IChartAdapter2D.Plot
            Dim series1
            Me._chart.ChartGroups.Item(0).ChartType = Chart2DTypeEnumXYPlot
            If ((seriesIndex >= 0) AndAlso (seriesIndex < Me._chart.ChartGroups.Item(0).ChartData.SeriesList.Count)) Then
                series1 = Me._chart.ChartGroups.Item(0).ChartData.SeriesList.Item(seriesIndex)
                series1.PointData.Clear()
                Me.DataToSeries(series1, points)
            Else
                series1 = [Assembly].LoadWithPartialName("C1.Win.C1Chart, Culture=neutral, PublicKeyToken=a22e16972c085838").CreateInstance("C1.Win.C1Chart.ChartDataSeries")
                series1.LineStyle.Pattern = LinePatternEnumNone
                series1.SymbolStyle.Shape = SymbolShapeEnumDot
                series1.SymbolStyle.Size = 1
                Me._chart.ChartGroups.Item(0).ChartData.SeriesList.Add(series1)
                Me.DataToSeries(series1, points)
            End If
        End Sub

        Public Overloads Function Plot(ByVal points() As Core.Point2D, ByVal seriesName As String) As Integer Implements IChartAdapter.IChartAdapter2D.Plot
            Dim num1 As Integer = Me.SeriesNameToSeriesIndex(seriesName)
            Me.Plot(points, num1)
            Return num1
        End Function



        Public Overloads Sub PlotPolar(ByVal values() As Double, ByVal seriesIndex As Integer) Implements IChartAdapter.IChartAdapter2D.PlotPolar
            If (Not values Is Nothing) Then
                Me.Plot(values, seriesIndex)
                Me._chart.ChartGroups.Item(0).ChartType = Chart2DTypeEnumPolar
            End If
        End Sub

        Public Overloads Sub PlotPolar(ByVal values() As Double, ByVal series As Series) Implements IChartAdapter.IChartAdapter2D.PlotPolar
            If (Not values Is Nothing) Then
                Me.Plot(values, series)
                Me._chart.ChartGroups.Item(0).ChartType = Chart2DTypeEnumPolar
            End If
        End Sub

        Public Overloads Function PlotPolar(ByVal values() As Double, ByVal seriesName As String) As Integer Implements IChartAdapter.IChartAdapter2D.PlotPolar
            Dim num1 As Integer = -1
            If (Not values Is Nothing) Then
                Me.Plot(values, seriesName)
                Me._chart.ChartGroups.Item(0).ChartType = Chart2DTypeEnumPolar
            End If
            Return num1
        End Function

        Public Overloads Sub PlotPolar(ByVal points() As Core.Point2D, ByVal seriesIndex As Integer) Implements IChartAdapter.IChartAdapter2D.PlotPolar
            If (Not points Is Nothing) Then
                Me.Plot(points, seriesIndex)
                Me._chart.ChartGroups.Item(0).ChartType = Chart2DTypeEnumPolar
            End If
        End Sub

        Public Overloads Sub PlotPolar(ByVal points() As Core.Point2D, ByVal series As Series) Implements IChartAdapter.IChartAdapter2D.PlotPolar
            If (Not points Is Nothing) Then
                Me.Plot(points, series)
                Me._chart.ChartGroups.Item(0).ChartType = Chart2DTypeEnumPolar
            End If
        End Sub

        Public Overloads Function PlotPolar(ByVal points() As Core.Point2D, ByVal seriesName As String) As Integer Implements IChartAdapter.IChartAdapter2D.PlotPolar
            Dim num1 As Integer = -1
            If (Not points Is Nothing) Then
                Me.Plot(points, seriesName)
                Me._chart.ChartGroups.Item(0).ChartType = Chart2DTypeEnumPolar
            End If
            Return num1
        End Function

    End Class

End Namespace