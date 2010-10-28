

Imports System
Imports System.Reflection
Imports System.Collections
Imports System.Drawing

Imports Genetibase.MathX.Core
Imports Genetibase.MathX.NugenCCalc.Adapters.IChartAdapter


Namespace TeeChart


    Public Delegate Sub TwoObjEventHandler(ByVal a, ByVal b)


    <ToolboxBitmap(GetType(TeeChartAdapter2D), "Steema.TeeChart.Images.TChart.bmp")> _
    Public Class TeeChartAdapter2D
        Implements IChartAdapter2D, IChartAdapter.IChartAdapter

        Private Shared _subscriber As EventSubscriber

        ' Events
        Public Event SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Implements IChartAdapter.IChartAdapter2D.SizeChanged
        Public Event ScopeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Implements IChartAdapter.IChartAdapter2D.ScopeChanged


        ' Methods
        Public Sub New()

        End Sub
        Public Sub New(ByVal teeChartControl As Object)
            Me.SetChartControl(teeChartControl)
        End Sub


        Public Sub _chart_BeforeDraw(ByVal ParamArray args() As Object)


            Dim size1 As Size = Me.PlotAreaSize
            If (Me._plotAreaSize.Height <> size1.Height Or Me._plotAreaSize.Width <> size1.Width) Then
                Me._plotAreaSize = size1
                Me.OnSizeChanged()
            End If
            Dim num1 As Double = Me._chart.get_Axes.get_Bottom.get_Minimum
            Dim num2 As Double = Me._chart.get_Axes.get_Bottom.get_Maximum
            Dim num3 As Double = Me._chart.get_Axes.get_Left.get_Minimum
            Dim num4 As Double = Me._chart.get_Axes.get_Left.get_Maximum
            Dim num5 As Double = Me._chart.get_Aspect.get_Zoom
            If (((Me._minX <> num1) OrElse (Me._maxX <> num2)) OrElse (((Me._minY <> num3) OrElse (Me._maxY <> num4)) OrElse (Me._zoom <> num5))) Then
                Me._minX = num1
                Me._maxX = num2
                Me._minY = num3
                Me._maxY = num4
                Me._zoom = num5
                Me.OnScopeChanged()
            End If

        End Sub


        Private Sub DoubleArrayIntoSeries(ByVal doubleArray As Double(), ByVal xySeries As Object)
            Dim num1 As Double = 1
            Dim num2 As Double = Me._chart.get_Axes.get_Bottom.get_Minimum
            Dim num3 As Double = Me._chart.get_Axes.get_Bottom.get_Maximum
            If (num2 <> num3) Then
                num1 = ((num3 - num2) / CType((doubleArray.Length - 1), Double))
            End If
            Dim num4 As Integer
            For num4 = 0 To doubleArray.Length - 1
                xySeries.Add(((num4 * num1) + num2), doubleArray(num4))
            Next num4
        End Sub
        Private Sub DoubleArrayIntoSeriesPolar(ByVal doubleArray As Double(), ByVal polarSeries As Object)
            Dim num1 As Double = 1
            Dim num2 As Double = Me._chart.get_Axes.get_Bottom.get_Minimum
            Dim num3 As Double = Me._chart.get_Axes.get_Bottom.get_Maximum
            If (num2 <> num3) Then
                num1 = ((num3 - num2) / CType((doubleArray.Length - 1), Double))
            End If
            Dim num4 As Integer
            For num4 = 0 To doubleArray.Length - 1
                polarSeries.Add(((num4 * num1) + num2), doubleArray(num4))
            Next num4
        End Sub


        Public Sub ClearChartSeries() Implements IChartAdapter.IChartAdapter2D.ClearChartSeries
            Me._chart.get_Series.Clear()
        End Sub

        Public Function GetSeriesNames() As String() Implements IChartAdapter2D.GetSeriesNames

            Dim list1 As New ArrayList

            Dim series1 ' = [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Series")

            For Each series1 In Me._chart.get_Series
                list1.Add(series1.get_Title)
            Next

            Return CType(list1.ToArray(GetType(String)), String())
        End Function


        Public Function GetSeries() As Series() Implements IChartAdapter.IChartAdapter2D.GetSeries
            Dim list1 As New ArrayList
            Dim series1 ' = [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Series")
            Dim index As Integer
            index = 0
            For Each series1 In Me._chart.get_Series
                list1.Add(New Series(index, series1.get_Title, series1.Color))
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
            Dim series1 ' = [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Series")

            If (((seriesIndex >= 0) AndAlso (seriesIndex < Me._chart.get_Series.Count)) AndAlso (Not Me._chart.get_Series.get_Item(seriesIndex).GetType.Name = "Polar")) Then
                series1 = Me._chart.get_Series.get_Item(seriesIndex)
                series1.Clear()
            Else
                Dim points1 = [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Points")
                points1.get_Pointer.set_Style(0)
                points1.get_Pointer.set_HorizSize(1)
                points1.get_Pointer.set_VertSize(1)
                points1.get_Pointer.get_Pen.set_Visible(False)
                series1 = Me._chart.get_Series.Add(points1)
            End If
            Dim pointdArray1 As Point2D() = points
            Dim num1 As Integer
            For num1 = 0 To pointdArray1.Length - 1
                Dim pointd1 As Point2D = pointdArray1(num1)
                series1.Add(pointd1.X, pointd1.Y)
            Next num1
        End Sub
        Public Function Plot(ByVal points As Point2D(), ByVal seriesName As String) As Integer Implements IChartAdapter2D.Plot
            Dim num1 As Integer = Me.SeriesNameToSeriesIndex(seriesName)
            Me.Plot(points, num1)
            Return num1
        End Function

        Public Sub Plot(ByVal values As Double(), ByVal seriesIndex As Integer) Implements IChartAdapter2D.Plot
            Dim series1 ' = [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Series")
            If (((seriesIndex >= 0) AndAlso (seriesIndex < Me._chart.get_Series.Count)) AndAlso (Not Me._chart.get_Series.get_Item(seriesIndex).GetType.Name = "Polar")) Then
                series1 = Me._chart.get_Series.get_Item(seriesIndex)
                series1.Clear()
            Else
                Dim points1 = [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Points")
                points1.get_Pointer.set_Style(0)
                points1.get_Pointer.set_HorizSize(1)
                points1.get_Pointer.set_VertSize(1)
                points1.get_Pointer.get_Pen.set_Visible(False)
                series1 = Me._chart.get_Series.Add(points1)
            End If
            Me.DoubleArrayIntoSeries(values, series1)
        End Sub
        Public Function Plot(ByVal values As Double(), ByVal seriesName As String) As Integer Implements IChartAdapter2D.Plot
            Dim num1 As Integer = Me.SeriesNameToSeriesIndex(seriesName)
            Me.Plot(values, num1)
            Return num1
        End Function

        Public Sub PlotPolar(ByVal points As Point2D(), ByVal seriesIndex As Integer) Implements IChartAdapter2D.PlotPolar
            Dim series1 ' = [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Series")
            If (((seriesIndex >= 0) AndAlso (seriesIndex < Me._chart.get_Series.Count)) AndAlso (Me._chart.get_Series.get_Item(seriesIndex).GetType.Name = "Polar")) Then
                series1 = Me._chart.get_Series.get_Item(seriesIndex)
                series1.Clear()
            Else
                Dim polar1 = [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Polar")
                polar1.get_Pointer.set_Style(0)
                polar1.get_Pointer.set_HorizSize(1)
                polar1.get_Pointer.set_VertSize(1)
                polar1.get_Pointer.get_Pen.set_Visible(False)
                series1 = Me._chart.get_Series.Add(polar1)
            End If
            Dim pointdArray1 As Point2D() = points
            Dim num1 As Integer
            For num1 = 0 To pointdArray1.Length - 1
                Dim pointd1 As Point2D = pointdArray1(num1)
                series1.Add(pointd1.X, pointd1.Y)
            Next num1
        End Sub


        Public Sub PlotPolar(ByVal points As Point2D(), ByVal series As Series) Implements IChartAdapter2D.PlotPolar
            Dim series1 ' = [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Series")
            If (((series.Index >= 0) AndAlso (series.Index < Me._chart.get_Series.Count)) AndAlso (Me._chart.get_Series.get_Item(series.Index).GetType.Name = "Polar")) Then
                series1 = Me._chart.get_Series.get_Item(series.Index)
                series1.Clear()
            Else
                Dim polar1 = [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Polar")
                polar1.get_Pointer.set_Style(0)
                polar1.get_Pointer.set_HorizSize(1)
                polar1.get_Pointer.set_VertSize(1)
                polar1.get_Pointer.get_Pen.set_Visible(False)
                series1 = Me._chart.get_Series.Add(polar1)
            End If
            Dim pointdArray1 As Point2D() = points
            Dim num1 As Integer
            For num1 = 0 To pointdArray1.Length - 1
                Dim pointd1 As Point2D = pointdArray1(num1)
                series1.Add(pointd1.X, pointd1.Y)
            Next num1
        End Sub

        Public Function PlotPolar(ByVal points As Point2D(), ByVal seriesName As String) As Integer Implements IChartAdapter2D.PlotPolar
            Dim num1 As Integer = Me.SeriesNameToSeriesIndexPolar(seriesName)
            Me.PlotPolar(points, num1)
            Return num1
        End Function

        Public Sub PlotPolar(ByVal values As Double(), ByVal seriesIndex As Integer) Implements IChartAdapter2D.PlotPolar
            Dim series1 '= [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Series")
            If (((seriesIndex >= 0) AndAlso (seriesIndex < Me._chart.get_Series.Count)) AndAlso (Me._chart.get_Series.get_Item(seriesIndex).GetType.Name = "Polar")) Then
                series1 = Me._chart.get_Series.get_Item(seriesIndex)
                series1.Clear()
            Else
                Dim polar1 = [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Polar")
                polar1.get_Pointer.set_Style(0)
                polar1.get_Pointer.set_HorizSize(1)
                polar1.get_Pointer.set_VertSize(1)
                polar1.get_Pointer.get_Pen.set_Visible(False)
                series1 = Me._chart.get_Series.Add(polar1)
            End If
            Me.DoubleArrayIntoSeriesPolar(values, series1)
        End Sub

        Public Sub PlotPolar(ByVal values As Double(), ByVal series As Series) Implements IChartAdapter2D.PlotPolar
            Dim series1 '= [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Series")
            If (((series.Index >= 0) AndAlso (series.Index < Me._chart.get_Series.Count)) AndAlso (Me._chart.get_Series.get_Item(series.Index).GetType.Name = "Polar")) Then
                series1 = Me._chart.get_Series.get_Item(series.Index)
                series1.Clear()
            Else
                Dim polar1 = [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Polar")
                polar1.get_Pointer.set_Style(0)
                polar1.get_Pointer.set_HorizSize(1)
                polar1.get_Pointer.set_VertSize(1)
                polar1.get_Pointer.get_Pen.set_Visible(False)
                series1 = Me._chart.get_Series.Add(polar1)
            End If
            Me.DoubleArrayIntoSeriesPolar(values, series1)
        End Sub

        Public Function PlotPolar(ByVal values As Double(), ByVal seriesName As String) As Integer Implements IChartAdapter2D.PlotPolar
            Dim num1 As Integer = Me.SeriesNameToSeriesIndexPolar(seriesName)
            Me.PlotPolar(values, num1)
            Return num1
        End Function

        Private Function SeriesNameToSeriesIndex(ByVal seriesName As String) As Integer
            Dim num1 As Integer = -1
            Dim num2 As Integer
            For num2 = 0 To Me._chart.get_Series.Count - 1
                If ((Me._chart.get_Series.get_Item(num2).get_Title Is seriesName) AndAlso (Not Me._chart.get_Series.get_Item(num2).GetType.Name = "Polar")) Then
                    num1 = num2
                    Exit For
                End If
            Next num2
            If (num1 = -1) Then
                Dim points1 = [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Points")
                points1.set_Title(seriesName)
                points1.get_Pointer.set_Style(0)
                points1.get_Pointer.set_HorizSize(1)
                points1.get_Pointer.set_VertSize(1)
                points1.get_Pointer.get_Pen.set_Visible(False)
                Me._chart.get_Series.Add(points1)
                num1 = Me._chart.get_Series.IndexOf(points1)
            End If
            Return num1
        End Function

        Private Function SeriesToSeriesIndex(ByVal series As Series) As Integer
            Dim num1 As Integer = -1
            Dim num2 As Integer
            For num2 = 0 To Me._chart.get_Series.Count - 1
                If (Me._chart.get_Series.get_Item(num2).get_Title Is series.Label) Then
                    num1 = num2
                    series.Index = num2
                    Exit For
                End If
            Next num2
            If (num1 = -1) Then
                Dim points1 = [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Points")
                points1.set_Title(series.Label)
                points1.get_Pointer.set_Style(0)
                points1.get_Pointer.set_HorizSize(1)
                points1.get_Pointer.set_VertSize(1)
                points1.get_Pointer.get_Pen.set_Visible(False)
                Me._chart.get_Series.Add(points1)
                num1 = Me._chart.get_Series.IndexOf(points1)
            End If
            Return num1
        End Function




        Private Function SeriesNameToSeriesIndexPolar(ByVal seriesName As String) As Integer
            Dim num1 As Integer = -1
            Dim num2 As Integer
            For num2 = 0 To Me._chart.get_Series.Count - 1
                If ((Me._chart.get_Series.get_Item(num2).get_Title Is seriesName) AndAlso (Me._chart.get_Series.get_Item(num2).GetType.Name = "Polar")) Then
                    num1 = num2
                    Exit For
                End If
            Next num2
            If (num1 = -1) Then
                Dim polar1 = [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Polar")
                polar1.set_Title(seriesName)
                polar1.get_Pointer.set_Style(0)
                polar1.get_Pointer.set_HorizSize(1)
                polar1.get_Pointer.set_VertSize(1)
                polar1.get_Pointer.get_Pen.set_Visible(False)
                Me._chart.get_Series.Add(polar1)
                num1 = Me._chart.get_Series.IndexOf(polar1)
            End If
            Return num1
        End Function


        Public Sub SetAxes(ByVal minX As Double, ByVal maxX As Double, ByVal minY As Double, ByVal maxY As Double) Implements IChartAdapter2D.SetAxes

            Me._chart.get_Axes.get_Bottom.set_Minimum(minX)
            Me._chart.get_Axes.get_Bottom.set_Maximum(maxX)
            Me._chart.get_Axes.get_Bottom.set_Automatic(False)
            Me._chart.get_Axes.get_Bottom.set_AutomaticMinimum(False)
            Me._chart.get_Axes.get_Bottom.set_AutomaticMaximum(False)
            Me._chart.get_Axes.get_Left.set_Minimum(minY)
            Me._chart.get_Axes.get_Left.set_Maximum(maxY)
            Me._chart.get_Axes.get_Left.set_Automatic(False)
            Me._chart.get_Axes.get_Left.set_AutomaticMinimum(False)
            Me._chart.get_Axes.get_Left.set_AutomaticMaximum(False)
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
            Me._minX = Me._chart.get_Axes.get_Bottom.get_Minimum
            Me._maxX = Me._chart.get_Axes.get_Bottom.get_Maximum
            Me._minY = Me._chart.get_Axes.get_Left.get_Minimum
            Me._maxY = Me._chart.get_Axes.get_Left.get_Maximum
            Me._zoom = Me._chart.get_Aspect.get_Zoom
            Me._plotAreaSize = Me.PlotAreaSize

            'If _subscriber Is Nothing Then
            '    _subscriber = New EventSubscriber(_chart.GetType().GetEvent("BeforeDraw"))
            'End If

            '_subscriber.Subscribe(_chart, New CustomEventHandler(AddressOf _chart_BeforeDraw))




        End Sub

        Public Function Validate(ByVal obj As Object) As Boolean Implements IChartAdapter.IChartAdapter.Validate
            If (obj.GetType().Name = "TChart") Then
                Return True
            End If
            Return False

        End Function


        ' Properties
        Public ReadOnly Property ChartControl() As Object Implements IChartAdapter2D.ChartControl
            Get
                Return Me._chart
            End Get
        End Property
        Public ReadOnly Property MaxX() As Double Implements IChartAdapter2D.MaxX
            Get
                Return Me._maxX
            End Get
        End Property
        Public ReadOnly Property MaxY() As Double Implements IChartAdapter2D.MaxY
            Get
                Return Me._maxY
            End Get
        End Property
        Public ReadOnly Property MinX() As Double Implements IChartAdapter2D.MinX
            Get
                Return Me._minX
            End Get
        End Property
        Public ReadOnly Property MinY() As Double Implements IChartAdapter2D.MinY
            Get
                Return Me._minY
            End Get
        End Property
        Public ReadOnly Property PlotAreaSize() As Size Implements IChartAdapter2D.PlotAreaSize
            Get
                Dim rectangle1 As Rectangle = Me._chart.get_Chart.ChartRect
                If (rectangle1.Size.Height = 0 And rectangle1.Size.Width = 0) Then
                    rectangle1.Size = Me._chart.Size
                End If
                Return rectangle1.Size
            End Get
        End Property

        ' Fields
        Private _chart As Object
        Private _maxX As Double
        Private _maxY As Double
        Private _minX As Double
        Private _minY As Double
        Private _plotAreaSize As Size
        Private _zoom As Double


    End Class

End Namespace
