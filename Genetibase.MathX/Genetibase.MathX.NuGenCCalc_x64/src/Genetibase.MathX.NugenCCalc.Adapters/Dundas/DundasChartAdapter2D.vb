Imports System
Imports System.Reflection
Imports System.Collections
Imports System.Drawing
Imports System.Windows.Forms

Imports Genetibase.MathX.Core
Imports Genetibase.MathX.NugenCCalc.Adapters.IChartAdapter

Namespace Dundas
    <ToolboxBitmap(GetType(DundasChartAdapter2D), "Charting.WinControl.DundasChart.ico")> _
    Public Class DundasChartAdapter2D
        Implements IChartAdapter2D, IChartAdapter.IChartAdapter

        ' Events
        Public Event ScopeChanged(ByVal sender As Object, ByVal args As EventArgs) Implements IChartAdapter.IChartAdapter2D.ScopeChanged
        Public Event SizeChanged(ByVal sender As Object, ByVal args As EventArgs) Implements IChartAdapter.IChartAdapter2D.SizeChanged

        ' Constants
        Dim MarkerStyleCircle = 2



        ' Methods
        Public Sub New()

        End Sub
        Public Sub New(ByVal dundasChart As Object)
            Me.SetChartControl(dundasChart)
        End Sub


        Private Sub _chart_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) 'ChartPaintEventArgs)

            Dim size1 As Size = Me.PlotAreaSize
            If (Me._plotAreaSize.Height <> size1.Height Or Me._plotAreaSize.Width <> size1.Width) Then
                Me._plotAreaSize = size1
                Me.OnSizeChanged()
            End If

            Dim num1 As Double = Me._chart.ChartAreas.Item(0).AxisX.Minimum
            Dim num2 As Double = Me._chart.ChartAreas.Item(0).AxisX.Maximum
            Dim num3 As Double = Me._chart.ChartAreas.Item(0).AxisY.Minimum
            Dim num4 As Double = Me._chart.ChartAreas.Item(0).AxisY.Maximum
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
            If (((Me._minX <> num1) OrElse (Me._maxX <> num2)) OrElse ((Me._minY <> num3) OrElse (Me._maxY <> num4))) Then
                Me._minX = num1
                Me._maxX = num2
                Me._minY = num3
                Me._maxY = num4
                Me.OnScopeChanged()
            End If

        End Sub

        Private Sub DoubleArrayIntoSeries(ByVal doubleArray As Double(), ByVal series As Object)
            Dim num1 As Double = 1
            Dim num2 As Double = Me._chart.ChartAreas.Item(0).AxisX.Minimum
            Dim num3 As Double = Me._chart.ChartAreas.Item(0).AxisX.Maximum
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
                series.Points.AddXY(((num4 * num1) + num2), doubleArray(num4))
                num4 += 1
            Loop
        End Sub

        Public Sub ClearChartSeries() Implements IChartAdapter.IChartAdapter2D.ClearChartSeries
            Me._chart.Series.Clear()
        End Sub

        Public Function GetSeriesNames() As String() Implements IChartAdapter2D.GetSeriesNames
            Dim list1 As New ArrayList
            Dim series1 'As Series
            For Each series1 In Me._chart.Series
                list1.Add(series1.Name)
            Next
            Return CType(list1.ToArray(GetType(String)), String())
        End Function

        Public Function GetSeries() As Series() Implements IChartAdapter.IChartAdapter2D.GetSeries
            Dim list1 As New ArrayList
            Dim index As Integer
            index = 0
            For Each series1 In Me._chart.Series
                list1.Add(New Series(index, series1.Name, series1.Color))
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
            Dim num1 As Integer = Me.SeriesToSeriesIndex(series)
            Me.Plot(values, num1)
        End Sub

        Public Sub Plot(ByVal points() As Core.Point2D, ByVal series As Series) Implements IChartAdapter.IChartAdapter2D.Plot
            Dim num1 As Integer = Me.SeriesToSeriesIndex(series)
            Me.Plot(points, num1)
        End Sub

        Public Sub Plot(ByVal points As Point2D(), ByVal seriesIndex As Integer) Implements IChartAdapter2D.Plot

            Dim series1 'As Series
            Me.SetMinMaxFields()


            Dim chartType As Type = Me._chart.GetType()
            chartType.GetEvent("Paint").AddEventHandler(_chart, New System.Windows.Forms.PaintEventHandler(AddressOf Me._chart_Paint))

            'AddHandler Me._chart.PrePaint, New PaintEventHandler(AddressOf Me._chart_PrePaint)


            If ((seriesIndex >= 0) AndAlso (seriesIndex < Me._chart.Series.Count)) Then
                series1 = Me._chart.Series.Item(seriesIndex)
                series1.Points.Clear()
            Else
                series1 = [Assembly].LoadWithPartialName("DundasWinChart, Culture=neutral, PublicKeyToken=a4c03dce5db22406").CreateInstance("Dundas.Charting.WinControl.Series")
                series1.Type = 0
                series1.XValueType = 1
                series1.YValueType = 1
                series1.MarkerStyle = MarkerStyleCircle
                series1.MarkerSize = 2
                series1.MarkerColor = Color.Black
                Me._chart.Series.Add(series1)
            End If
            Dim pointdArray1 As Point2D() = points
            Dim num1 As Integer = 0
            Do While (num1 < pointdArray1.Length)
                Dim pointd1 As Point2D = pointdArray1(num1)
                series1.Points.AddXY(pointd1.X, pointd1.Y)
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
            Me.SetMinMaxFields()

            Dim chartType As Type = Me._chart.GetType()
            chartType.GetEvent("Paint").AddEventHandler(_chart, New System.Windows.Forms.PaintEventHandler(AddressOf Me._chart_Paint))

            'AddHandler Me._chart.PrePaint, New PaintEventHandler(AddressOf Me._chart_PrePaint)

            If ((seriesIndex >= 0) AndAlso (seriesIndex < Me._chart.Series.Count)) Then
                series1 = Me._chart.Series.Item(seriesIndex)
                series1.Points.Clear()
            Else
                series1 = [Assembly].LoadWithPartialName("DundasWinChart, Culture=neutral, PublicKeyToken=a4c03dce5db22406").CreateInstance("Dundas.Charting.WinControl.Series")
                series1.Type = 0
                series1.XValueType = 1
                series1.YValueType = 1
                series1.MarkerStyle = MarkerStyleCircle
                series1.MarkerSize = 2
                series1.MarkerColor = Color.Black
                Me._chart.Series.Add(series1)
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


        Private Function SeriesToSeriesIndex(ByVal series As Series) As Integer
            Dim num1 As Integer = -1
            Dim num2 As Integer
            For num2 = 0 To Me._chart.Series.Count - 1
                If (Me._chart.Series.Item(num2).Name Is series.Label) Then
                    num1 = num2
                    series.Index = num2
                    Exit For
                End If
            Next num2
            If (num1 = -1) Then
                Dim series1 = [Assembly].LoadWithPartialName("DundasWinChart, Culture=neutral, PublicKeyToken=a4c03dce5db22406").CreateInstance("Dundas.Charting.WinControl.Series")

                series1.Name = series.Label
                series1.Type = 0
                series1.XValueType = 1
                series1.YValueType = 1
                series1.MarkerStyle = MarkerStyleCircle
                series1.MarkerSize = 2
                series1.MarkerColor = series.Color
                num1 = Me._chart.Series.Add(series1)
            End If
            Return num1
        End Function



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
                Dim series1 = [Assembly].LoadWithPartialName("DundasWinChart, Culture=neutral, PublicKeyToken=a4c03dce5db22406").CreateInstance("Dundas.Charting.WinControl.Series")
                series1.Name = seriesName
                series1.Type = 0
                series1.XValueType = 1
                series1.YValueType = 1
                series1.MarkerStyle = MarkerStyleCircle
                series1.MarkerSize = 2
                series1.MarkerColor = Color.Black
                num1 = Me._chart.Series.Add(series1)
            End If
            Return num1
        End Function
        Public Sub SetAxes(ByVal minX As Double, ByVal maxX As Double, ByVal minY As Double, ByVal maxY As Double) Implements IChartAdapter2D.SetAxes
            Me.SetMinMaxFields()

            Dim chartType As Type = Me._chart.GetType()
            chartType.GetEvent("Paint").AddEventHandler(_chart, New System.Windows.Forms.PaintEventHandler(AddressOf Me._chart_Paint))

            'AddHandler Me._chart.PrePaint, New PaintEventHandler(AddressOf Me._chart_PrePaint)

            Me._chart.ChartAreas.Item(0).AxisX.Minimum = minX
            Me._chart.ChartAreas.Item(0).AxisX.Maximum = maxX
            Me._chart.ChartAreas.Item(0).AxisY.Minimum = minY
            Me._chart.ChartAreas.Item(0).AxisY.Maximum = maxY
        End Sub
        Public Sub SetChartControl(ByVal chartControl As Object) Implements IChartAdapter2D.SetChartControl
            If Not Me.Validate(chartControl) Then
                Throw New ArgumentException("Parameter is not valid.", "chartControl")
            End If
            Me._chart = chartControl
        End Sub
        Private Sub SetMinMaxFields()
            Me._minX = Me._chart.ChartAreas.Item(0).AxisX.Minimum
            Me._maxX = Me._chart.ChartAreas.Item(0).AxisX.Maximum
            Me._minY = Me._chart.ChartAreas.Item(0).AxisY.Minimum
            Me._maxY = Me._chart.ChartAreas.Item(0).AxisY.Maximum
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
        End Sub
        Public Function Validate(ByVal obj As Object) As Boolean Implements IChartAdapter.IChartAdapter.Validate
            If (obj.GetType.FullName = "Dundas.Charting.WinControl.Chart") Then
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
                Dim size1 As Size = New Size
                Dim num1 As Double = ((Me._chart.Height * Me._chart.ChartAreas.Item(0).Position.Height) / 100.0!)
                Dim num2 As Double = ((Me._chart.Width * Me._chart.ChartAreas.Item(0).Position.Width) / 100.0!)
                Dim num3 As Double = ((num1 * Me._chart.ChartAreas.Item(0).InnerPlotPosition.Height) / 100)
                Dim num4 As Double = ((num2 * Me._chart.ChartAreas.Item(0).InnerPlotPosition.Width) / 100)
                size1.Height = CType(num3, Integer)
                size1.Width = CType(num4, Integer)
                If (size1.Width = 0 And size1.Height = 0) Then
                    size1 = Me._chart.Size
                End If
                Return size1
            End Get
        End Property

        ' Fields
        Private _chart As Object
        Private _maxX As Double
        Private _maxY As Double
        Private _minX As Double
        Private _minY As Double
        Private _plotAreaSize As Size

    End Class
End Namespace
