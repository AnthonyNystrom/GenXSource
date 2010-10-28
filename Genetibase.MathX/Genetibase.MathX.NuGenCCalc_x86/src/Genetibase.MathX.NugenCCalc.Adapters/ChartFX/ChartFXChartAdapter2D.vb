Imports System
Imports System.Reflection
Imports System.Collections
Imports System.Drawing
Imports System.Windows.Forms

Imports Genetibase.MathX.Core
Imports Genetibase.MathX.NugenCCalc.Adapters.IChartAdapter


Namespace ChartFX
    <ToolboxBitmap(GetType(ChartFXChartAdapter2D), "Ñharticon.ico")> _
    Public Class ChartFXChartAdapter2D
        Implements IChartAdapter2D, IChartAdapter.IChartAdapter

        Private Shared _subscriber As EventSubscriber


        ' Events
        Public Event ScopeChanged(ByVal sender As Object, ByVal args As EventArgs) Implements IChartAdapter.IChartAdapter2D.ScopeChanged
        Public Event SizeChanged(ByVal sender As Object, ByVal args As EventArgs) Implements IChartAdapter.IChartAdapter2D.SizeChanged

        ' Constants 
        Dim GalleryScatter = 4
        Dim MarkerShapeCircle = 2

        Dim CODValues = 1
        Dim CODXValues = 6

        ' Methods
        Public Sub New()

        End Sub
        Public Sub New(ByVal fxChart As Object)
            Me.SetChartControl(fxChart)
        End Sub


        Private Sub _chart_PrePaint(ByVal ParamArray args() As Object) 'WHEventArgs)
            Dim size1 As Size = Me.PlotAreaSize
            If (Me._plotAreaSize.Height <> size1.Height Or Me._plotAreaSize.Width <> size1.Width) Then
                Me._plotAreaSize = size1
                Me.OnSizeChanged()
            End If
            Dim num1 As Double = Me._chart.AxisX.Min
            Dim num2 As Double = Me._chart.AxisX.Max
            Dim num3 As Double = Me._chart.AxisY.Min
            Dim num4 As Double = Me._chart.AxisY.Max
            Dim num5 As Double = Me._chart.AxisX.ScaleUnit
            Dim num6 As Double = Me._chart.AxisY.ScaleUnit
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

        Public Function GetSeriesNames() As String() Implements IChartAdapter2D.GetSeriesNames
            Dim list1 As New ArrayList
            Dim attributes1 'As SeriesAttributes
            For Each attributes1 In Me._chart.Series
                list1.Add(attributes1.Legend)
            Next
            Return CType(list1.ToArray(GetType(String)), String())
        End Function

        Public Sub ClearChartSeries() Implements IChartAdapter.IChartAdapter2D.ClearChartSeries

        End Sub

        Public Function GetSeries() As Series() Implements IChartAdapter.IChartAdapter2D.GetSeries
            Dim list1 As New ArrayList
            Dim attributes1 'As SeriesAttributes
            Dim index As Integer
            index = 0
            For Each attributes1 In Me._chart.Series
                list1.Add(New Series(index, attributes1.Legend, attributes1.Color))
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
            Dim num1 As Integer = 0
            If ((seriesIndex >= 0) AndAlso (seriesIndex < Me._chart.NSeries)) Then
                num1 = seriesIndex
            Else
                num1 = Me._chart.NSeries
                Me._chart.Series.Item(num1).Gallery = GalleryScatter
                Me._chart.Series.Item(num1).MarkerShape = MarkerShapeCircle
                Me._chart.Series.Item(num1).MarkerSize = 1
            End If
            Me._chart.OpenData(CODValues, 0, -1)
            Me._chart.OpenData(CODXValues, 0, -1)
            Dim num2 As Integer = 0
            Do While (num2 < points.Length)
                Me._chart.XValue.Item(num1, num2) = points(num2).X
                Me._chart.Value.Item(num1, num2) = points(num2).Y
                num2 += 1
            Loop
            Me._chart.CloseData(CODXValues)
            Me._chart.CloseData(CODValues)
        End Sub

        Public Function Plot(ByVal points As Point2D(), ByVal seriesName As String) As Integer Implements IChartAdapter2D.Plot
            Dim num1 As Integer = Me.SeriesNameToSeriesIndex(seriesName)
            Me.Plot(points, num1)
            If (num1 = Me._chart.NSeries) Then
                Me._chart.Series.Item(Me._chart.NSeries).Legend = seriesName
            End If
            Return num1
        End Function


        Public Sub Plot(ByVal values As Double(), ByVal seriesIndex As Integer) Implements IChartAdapter2D.Plot
            Dim num1 As Double = 1
            If (Not Me._chart.AxisX.AutoScale AndAlso Not Me._chart.AxisY.AutoScale) Then
                Dim num2 As Double = Me._chart.AxisX.Min
                num1 = ((Me._chart.AxisX.Max - num2) / CType((values.Length - 1), Double))
            End If
            Dim num4 As Integer = 0
            If ((seriesIndex >= 0) AndAlso (seriesIndex < Me._chart.NSeries)) Then
                num4 = seriesIndex
            Else
                num4 = Me._chart.NSeries
                Me._chart.Series.Item(num4).Gallery = GalleryScatter
                Me._chart.Series.Item(num4).MarkerShape = MarkerShapeCircle
                Me._chart.Series.Item(num4).MarkerSize = 1
            End If
            Me._chart.OpenData(CODValues, 0, -1)
            Me._chart.OpenData(CODXValues, 0, -1)
            Dim num5 As Integer = 0
            Do While (num5 < values.Length)
                Me._chart.XValue.Item(num4, num5) = (num5 * num1)
                Me._chart.Value.Item(num4, num5) = values(num5)
                num5 += 1
            Loop
            Me._chart.CloseData(CODXValues)
            Me._chart.CloseData(CODValues)
        End Sub
        Public Function Plot(ByVal values As Double(), ByVal seriesName As String) As Integer Implements IChartAdapter2D.Plot
            Dim num1 As Integer = Me.SeriesNameToSeriesIndex(seriesName)
            Me.Plot(values, num1)
            If (num1 = Me._chart.NSeries) Then
                Me._chart.Series.Item(Me._chart.NSeries).Legend = seriesName
            End If
            Return num1
        End Function


        Public Sub PlotPolar(ByVal points As Point2D(), ByVal seriesIndex As Integer) Implements IChartAdapter2D.PlotPolar

        End Sub
        Public Function PlotPolar(ByVal points As Point2D(), ByVal seriesName As String) As Integer Implements IChartAdapter2D.PlotPolar
            Return 0
        End Function


        Public Sub PlotPolar(ByVal values As Double(), ByVal seriesIndex As Integer) Implements IChartAdapter2D.PlotPolar

        End Sub

        Public Sub PlotPolar(ByVal values As Double(), ByVal series As Series) Implements IChartAdapter2D.PlotPolar

        End Sub

        Public Sub PlotPolar(ByVal points As Point2D(), ByVal series As Series) Implements IChartAdapter2D.PlotPolar

        End Sub
        Public Function PlotPolar(ByVal values As Double(), ByVal seriesName As String) As Integer Implements IChartAdapter2D.PlotPolar
            Return 0
        End Function


        Private Function SeriesNameToSeriesIndex(ByVal seriesName As String) As Integer
            Dim num1 As Integer = Me._chart.NSeries
            Dim num2 As Integer = 0
            Do While (num2 < Me._chart.Series.Count)
                If (Me._chart.Series.Item(num2).Legend Is seriesName) Then
                    num1 = num2
                    Exit Do
                End If
                If (num2 = (Me._chart.Series.Count - 1)) Then
                    Me._chart.Series.Item(num1).Gallery = GalleryScatter
                    Me._chart.Series.Item(num1).MarkerShape = MarkerShapeCircle
                    Me._chart.Series.Item(num1).MarkerSize = 1
                End If
                num2 += 1
            Loop
            Return num1
        End Function


        Private Function SeriesToSeriesIndex(ByVal series As Series) As Integer
            Dim num1 As Integer = Me._chart.NSeries
            Dim num2 As Integer = 0
            Do While (num2 < Me._chart.Series.Count)
                If (Me._chart.Series.Item(num2).Legend Is series.Label) Then
                    num1 = num2
                    Exit Do
                End If
                If (num2 = (Me._chart.Series.Count - 1)) Then
                    Me._chart.Series.Item(num1).Gallery = GalleryScatter
                    Me._chart.Series.Item(num1).MarkerShape = MarkerShapeCircle
                    Me._chart.Series.Item(num1).MarkerSize = 1
                End If
                num2 += 1
            Loop
            Return num1
        End Function


        Public Sub SetAxes(ByVal minX As Double, ByVal maxX As Double, ByVal minY As Double, ByVal maxY As Double) Implements IChartAdapter2D.SetAxes
            Me._chart.AxisY.Max = maxY
            Me._chart.AxisY.Min = minY
            Me._chart.AxisY.AutoScale = False
            Me._chart.AxisX.Max = maxX
            Me._chart.AxisX.Min = minX
            Me._chart.AxisX.AutoScale = False
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
            Me._minX = Me._chart.AxisX.Min
            Me._maxX = Me._chart.AxisX.Max
            Me._minY = Me._chart.AxisY.Min
            Me._maxY = Me._chart.AxisY.Max
            Me._scaleX = Me._chart.AxisX.ScaleUnit
            Me._scaleY = Me._chart.AxisY.ScaleUnit
            Me._plotAreaSize = Me.PlotAreaSize

            'If _subscriber Is Nothing Then
            '    _subscriber = New EventSubscriber(_chart.GetType().GetEvent("PrePaint"))
            'End If
            '_subscriber.Subscribe(_chart, New CustomEventHandler(AddressOf _chart_PrePaint))

        End Sub


        Public Function Validate(ByVal obj As Object) As Boolean Implements IChartAdapter.IChartAdapter.Validate
            If (obj.GetType.FullName = "SoftwareFX.ChartFX.Chart") Then
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
                size1.Height = Me._chart.Panes.Item(0).Bounds.Height
                size1.Width = Me._chart.Panes.Item(0).Bounds.Width
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
        Private _scaleX As Double
        Private _scaleY As Double


    End Class

End Namespace

