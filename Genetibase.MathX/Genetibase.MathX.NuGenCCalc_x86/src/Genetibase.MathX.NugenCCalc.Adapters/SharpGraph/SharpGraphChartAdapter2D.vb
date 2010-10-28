Imports System
Imports System.Reflection
Imports System.Collections
Imports System.Drawing
Imports System.Windows.Forms

Imports Genetibase.MathX.Core
Imports Genetibase.MathX.NugenCCalc.Adapters.IChartAdapter

Namespace SharpGraph

    <ToolboxBitmap(GetType(SharpGraphChartAdapter2D), "DataDynamics.SharpGraph.Windows.ico")> _
    Public Class SharpGraphChartAdapter2D
        Implements IChartAdapter2D, IChartAdapter.IChartAdapter

        ' Events
        Public Event ScopeChanged(ByVal sender As Object, ByVal args As EventArgs) Implements IChartAdapter2D.ScopeChanged
        Public Event SizeChanged(ByVal sender As Object, ByVal args As EventArgs) Implements IChartAdapter2D.SizeChanged

        ' Constants
        Dim ChartTypeScatter = 17
        Dim MarkerStylePoint = 1

        Dim AxisTypeCategorical = 0
        Dim AxisTypeNumerical = 1



        ' Methods
        Public Sub New()

        End Sub
        Public Sub New(ByVal sharpgraphChart As Object)
            Me.SetChartControl(sharpgraphChart)
        End Sub


        Private Sub _chart_Paint(ByVal sender As Object, ByVal e As PaintEventArgs)
            Dim size1 As Size = Me.PlotAreaSize
            If (Me._plotAreaSize.Height <> size1.Height Or Me._plotAreaSize.Width <> size1.Width) Then
                Me._plotAreaSize = size1
                Me.OnSizeChanged()
            End If
            Dim num1 As Double = Me._chart.ChartAreas.Item(0).Axes.Item("AxisX").Min
            Dim num2 As Double = Me._chart.ChartAreas.Item(0).Axes.Item("AxisX").Max
            Dim num3 As Double = Me._chart.ChartAreas.Item(0).Axes.Item("AxisY").Min
            Dim num4 As Double = Me._chart.ChartAreas.Item(0).Axes.Item("AxisY").Max
            If Double.IsNaN(num1) Then
                num1 = 0
            End If
            If Double.IsNaN(num2) Then
                num2 = 0
            End If
            If Double.IsNaN(num3) Then
                num3 = 0
            End If
            If Double.IsNaN(num4) Then
                num4 = 0
            End If
            Dim num5 As Double = Me._chart.ChartAreas.Item(0).Axes.Item("AxisX").Scale
            Dim num6 As Double = Me._chart.ChartAreas.Item(0).Axes.Item("AxisY").Scale
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


        Private Sub DoubleArrayIntoSeries(ByVal doubleArray As Double(), ByVal series As Object)
            Dim num1 As Double = 1
            Dim num2 As Double = Me._chart.ChartAreas.Item(0).Axes.Item("AxisX").Min
            Dim num3 As Double = Me._chart.ChartAreas.Item(0).Axes.Item("AxisX").Max
            If Double.IsNaN(num2) Then
                num2 = 0
            End If
            If Double.IsNaN(num3) Then
                num3 = 0
            End If
            If (num2 <> num3) Then
                num1 = ((num3 - num2) / CType((doubleArray.Length - 1), Double))
            End If
            Dim num4 As Integer = 0
            Do While (num4 < doubleArray.Length)
                series.Points.AddXY(((num4 * num1) + num2), New Double() {doubleArray(num4)})
                num4 += 1
            Loop
        End Sub


        Public Sub ClearChartSeries() Implements IChartAdapter.IChartAdapter2D.ClearChartSeries
            Me._chart.Series.Clear()
        End Sub

        Public Function GetSeriesNames() As String() Implements IChartAdapter2D.GetSeriesNames
            Dim list1 As New ArrayList
            Dim series1 ' As Series
            For Each series1 In Me._chart.Series
                list1.Add(series1.Name)
            Next
            Return CType(list1.ToArray(GetType(String)), String())
        End Function

        Public Function GetSeries() As Series() Implements IChartAdapter.IChartAdapter2D.GetSeries
            Dim list1 As New ArrayList
            Dim series1 ' As Series
            Dim index As Integer
            index = 0
            For Each series1 In Me._chart.Series
                list1.Add(New Series(index, series1.Name, New Color))
                index += 1
            Next
            Return CType(list1.ToArray(GetType(Series)), Series())
        End Function

        Private Sub OnScopeChanged()
            RaiseEvent ScopeChanged(Me, New EventArgs)
        End Sub
        Private Sub OnSizeChanged()
            RaiseEvent SizeChanged(Me, New EventArgs)
        End Sub

        Public Sub Plot(ByVal values() As Double, ByVal series As Series) Implements IChartAdapter.IChartAdapter2D.Plot

        End Sub

        Public Sub Plot(ByVal points() As Core.Point2D, ByVal series As Series) Implements IChartAdapter.IChartAdapter2D.Plot

        End Sub

        Public Sub Plot(ByVal points As Point2D(), ByVal seriesIndex As Integer) Implements IChartAdapter2D.Plot
            Dim series1 'As Series
            If ((seriesIndex >= 0) AndAlso (seriesIndex < Me._chart.Series.Count)) Then
                series1 = Me._chart.Series.Item(seriesIndex)
                series1.Points.Clear()
            Else
                series1 = [Assembly].LoadWithPartialName("SharpGraph.Windows, Culture=neutral, PublicKeyToken=d5075c155abe504c").CreateInstance("DataDynamics.SharpGraph.Windows.Series")
                series1.Type = ChartTypeScatter
                Me._chart.ChartAreas.Item(0).Axes.Item("AxisX").AxisType = AxisTypeCategorical
                Me._chart.ChartAreas.Item(0).Axes.Item("AxisY").AxisType = AxisTypeNumerical
                Me._chart.Series.Add(series1)
                series1.Marker = [Assembly].LoadWithPartialName("SharpGraph.Windows, Culture=neutral, PublicKeyToken=d5075c155abe504c").CreateInstance("DataDynamics.SharpGraph.Windows.Marker")
                series1.Marker.Style = MarkerStylePoint
                series1.Marker.Size = 2
                series1.Marker.Label.Format = ""
            End If
            Dim pointdArray1 As Point2D() = points
            Dim num1 As Integer = 0
            Do While (num1 < pointdArray1.Length)
                Dim pointd1 As Point2D = pointdArray1(num1)
                series1.Points.AddXY(pointd1.X, New Double() {pointd1.Y})
                num1 += 1
            Loop
            Me._chart.Refresh()
        End Sub
        Public Function Plot(ByVal points As Point2D(), ByVal seriesName As String) As Integer Implements IChartAdapter2D.Plot
            Dim num1 As Integer = Me.SeriesNameToSeriesIndex(seriesName)
            Me.Plot(points, num1)
            Return num1
        End Function



        Public Sub Plot(ByVal values As Double(), ByVal seriesIndex As Integer) Implements IChartAdapter2D.Plot
            Dim series1 'As Series
            If ((seriesIndex >= 0) AndAlso (seriesIndex < Me._chart.Series.Count)) Then
                series1 = Me._chart.Series.Item(seriesIndex)
                series1.Points.Clear()
            Else
                series1 = [Assembly].LoadWithPartialName("SharpGraph.Windows, Culture=neutral, PublicKeyToken=d5075c155abe504c").CreateInstance("DataDynamics.SharpGraph.Windows.Series")
                series1.Type = ChartTypeScatter
                Me._chart.ChartAreas.Item(0).Axes.Item("AxisX").AxisType = AxisTypeCategorical
                Me._chart.ChartAreas.Item(0).Axes.Item("AxisY").AxisType = AxisTypeNumerical
                Me._chart.Series.Add(series1)
                series1.Marker = [Assembly].LoadWithPartialName("SharpGraph.Windows, Culture=neutral, PublicKeyToken=d5075c155abe504c").CreateInstance("DataDynamics.SharpGraph.Windows.Marker")
                series1.Marker.Style = MarkerStylePoint
                series1.Marker.Size = 2
                series1.Marker.Label.Format = ""
            End If
            Me.DoubleArrayIntoSeries(values, series1)
            Me._chart.Refresh()
        End Sub
        Public Function Plot(ByVal values As Double(), ByVal seriesName As String) As Integer Implements IChartAdapter2D.Plot
            Dim num1 As Integer = Me.SeriesNameToSeriesIndex(seriesName)
            Me.Plot(values, num1)
            Return num1
        End Function



        Public Sub PlotPolar(ByVal points As Point2D(), ByVal seriesIndex As Integer) Implements IChartAdapter2D.PlotPolar

        End Sub
        Public Function PlotPolar(ByVal points As Point2D(), ByVal seriesName As String) As Integer Implements IChartAdapter2D.PlotPolar
            Return 0
        End Function
        Public Sub PlotPolar(ByVal values As Double(), ByVal seriesIndex As Integer) Implements IChartAdapter2D.PlotPolar

        End Sub
        Public Function PlotPolar(ByVal values As Double(), ByVal seriesName As String) As Integer Implements IChartAdapter2D.PlotPolar
            Return 0
        End Function


        Public Sub PlotPolar(ByVal values As Double(), ByVal series As Series) Implements IChartAdapter2D.PlotPolar

        End Sub

        Public Sub PlotPolar(ByVal points As Point2D(), ByVal series As Series) Implements IChartAdapter2D.PlotPolar

        End Sub

        Private Function SeriesNameToSeriesIndex(ByVal seriesName As String) As Integer
            Dim num1 As Integer = -1
            Dim num2 As Integer = 0
            Do While (num2 < Me._chart.Series.Count)
                If (Me._chart.Series.Item(num2).Name Is seriesName) Then
                    num1 = num2
                    Exit Do
                End If
                num2 += 1
            Loop
            If (num1 = -1) Then
                Dim series1 = [Assembly].LoadWithPartialName("SharpGraph.Windows, Culture=neutral, PublicKeyToken=d5075c155abe504c").CreateInstance("DataDynamics.SharpGraph.Windows.Series")
                If (seriesName Is "") Then
                    seriesName = ("Series #" & Me._chart.Series.Count)
                End If
                series1.Name = seriesName
                series1.Type = ChartTypeScatter
                Me._chart.ChartAreas.Item(0).Axes.Item("AxisX").AxisType = AxisTypeCategorical
                Me._chart.ChartAreas.Item(0).Axes.Item("AxisY").AxisType = AxisTypeNumerical
                num1 = Me._chart.Series.Add(series1)
                series1.Marker = [Assembly].LoadWithPartialName("SharpGraph.Windows, Culture=neutral, PublicKeyToken=d5075c155abe504c").CreateInstance("DataDynamics.SharpGraph.Windows.Marker")
                series1.Marker.Style = MarkerStylePoint
                series1.Marker.Size = 2
                series1.Marker.Label.Format = ""
            End If
            Return num1
        End Function

        Public Sub SetAxes(ByVal minX As Double, ByVal maxX As Double, ByVal minY As Double, ByVal maxY As Double) Implements IChartAdapter2D.SetAxes
            Me._chart.ChartAreas.Item(0).Axes.Item("AxisX").Min = minX
            Me._chart.ChartAreas.Item(0).Axes.Item("AxisX").Max = maxX
            Me._chart.ChartAreas.Item(0).Axes.Item("AxisY").Min = minY
            Me._chart.ChartAreas.Item(0).Axes.Item("AxisY").Max = maxY
            Me._minX = minX
            Me._maxX = maxX
            Me._minY = minY
            Me._maxY = maxY
        End Sub



        Public Sub SetChartControl(ByVal chartControl As Object) Implements IChartAdapter2D.SetChartControl
            If Not Me.Validate(chartControl) Then
                Throw New ArgumentException("Parameter is not valid.", "chartControl")
            End If
            Me._chart = chartControl
            Me._minX = Me._chart.ChartAreas.Item(0).Axes.Item("AxisX").Min
            Me._maxX = Me._chart.ChartAreas.Item(0).Axes.Item("AxisX").Max
            Me._minY = Me._chart.ChartAreas.Item(0).Axes.Item("AxisY").Min
            Me._maxY = Me._chart.ChartAreas.Item(0).Axes.Item("AxisY").Max
            If Double.IsNaN(Me._minX) Then
                Me._minX = 0
            End If
            If Double.IsNaN(Me._maxX) Then
                Me._maxX = 0
            End If
            If Double.IsNaN(Me._minY) Then
                Me._minY = 0
            End If
            If Double.IsNaN(Me._maxY) Then
                Me._maxY = 0
            End If
            Me._scaleX = Me._chart.ChartAreas.Item(0).Axes.Item("AxisX").Scale
            Me._scaleY = Me._chart.ChartAreas.Item(0).Axes.Item("AxisY").Scale
            Me._plotAreaSize = Me.PlotAreaSize

            Dim chartType As Type = Me._chart.GetType()
            chartType.GetEvent("Paint").AddEventHandler(_chart, New PaintEventHandler(AddressOf Me._chart_Paint))
            'AddHandler Me._chart.Paint, New PaintEventHandler(AddressOf Me._chart_Paint)


            Me._chart.ChartAreas.Item(0).Axes.Item("AxisX").AxisType = AxisTypeCategorical
            Me._chart.ChartAreas.Item(0).Axes.Item("AxisY").AxisType = AxisTypeNumerical

        End Sub



        Public Function Validate(ByVal obj As Object) As Boolean Implements IChartAdapter2D.Validate
            If (obj.GetType.Name = "SharpGraph") Then
                Return True
            End If
            Return False
        End Function

        ' Properties
        Public ReadOnly Property ChartControl() As Object Implements IChartAdapter2D.ChartControl
            Get
                Return _chart
            End Get
        End Property
        Public ReadOnly Property MaxX() As Double Implements IChartAdapter2D.MaxX
            Get
                Return _maxX
            End Get
        End Property
        Public ReadOnly Property MaxY() As Double Implements IChartAdapter2D.MaxY
            Get
                Return _maxY
            End Get
        End Property
        Public ReadOnly Property MinX() As Double Implements IChartAdapter2D.MinX
            Get
                Return _minX
            End Get
        End Property
        Public ReadOnly Property MinY() As Double Implements IChartAdapter2D.MinY
            Get
                Return _minY
            End Get
        End Property
        Public ReadOnly Property PlotAreaSize() As Size Implements IChartAdapter2D.PlotAreaSize
            Get
                Return Me._chart.Size
            End Get
        End Property

        ' Fields
        Private _chart As Object
        Private _maxX As Double
        Private _maxY As Double
        Private _minX As Double
        Private _minY As Double
        Private _plotAreaSize As Size
        Private _scaleX As Double
        Private _scaleY As Double



    End Class

End Namespace
