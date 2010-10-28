Imports System
Imports System.Reflection
Imports System.Collections
Imports System.Drawing
Imports System.Windows.Forms

Imports Genetibase.MathX.Core
Imports Genetibase.MathX.NugenCCalc.Adapters.IChartAdapter

Namespace Xceed
    <ToolboxBitmap(GetType(XceedChartAdapter2D), "Chart.ChartControl.bmp")> _
        Public Class XceedChartAdapter2D
        Implements IChartAdapter2D, IChartAdapter.IChartAdapter

        ' Events
        Public Event ScopeChanged(ByVal sender As Object, ByVal args As EventArgs) Implements IChartAdapter.IChartAdapter2D.ScopeChanged
        Public Event SizeChanged(ByVal sender As Object, ByVal args As EventArgs) Implements IChartAdapter.IChartAdapter2D.SizeChanged

        ' Constants
        Dim MarginModeStretch = 2
        Dim MarginModeFit = 1
        Dim MarginModeNone = 0


        ' Methods
        Public Sub New()

        End Sub
        Public Sub New(ByVal xceedChartControl As Object)
            Me.SetChartControl(xceedChartControl)
        End Sub


        Private Sub _chart_Paint(ByVal sender As Object, ByVal e As PaintEventArgs)
            Dim size1 As Size = Me.PlotAreaSize
            If (Me._plotAreaSize.Height <> size1.Height Or Me._plotAreaSize.Width <> size1.Width) Then
                Me._plotAreaSize = size1
                Me.OnSizeChanged()
            End If
            Dim num1 As Double = Me._chart.get_Charts.get_Item(0).Axis(2).get_NumericScale.get_Min
            Dim num2 As Double = Me._chart.get_Charts.get_Item(0).Axis(2).get_NumericScale.get_Max
            Dim num3 As Double = Me._chart.get_Charts.get_Item(0).Axis(0).get_NumericScale.get_Min
            Dim num4 As Double = Me._chart.get_Charts.get_Item(0).Axis(0).get_NumericScale.get_Max
            Dim num5 As Double = Me._chart.get_Charts.get_Item(0).get_View.get_Zoom
            If (((Me._minX <> num1) OrElse (Me._maxX <> num2)) OrElse (((Me._minY <> num3) OrElse (Me._maxY <> num4)) OrElse (Me._zoom <> num5))) Then
                Me._minX = num1
                Me._maxX = num2
                Me._minY = num3
                Me._maxY = num4
                Me._zoom = num5
                Me.OnScopeChanged()
            End If
        End Sub

        Private Function AddNewSeries(ByVal seriesName As String) As Integer
            Dim series1 = [Assembly].LoadWithPartialName("Xceed.Chart.Core, Culture=neutral, PublicKeyToken=ba83ff368b7563c6").CreateInstance("Xceed.Chart.Core.PointSeries")
            If seriesName <> "" Then
                series1.set_Name(seriesName)
            End If
            series1.set_PointStyle(7)
            series1.set_Size(0.25!)
            series1.get_DataLabels.set_Mode(0)
            Return Me._chart.get_Charts.get_Item(0).get_Series.Add(series1)
        End Function

        Private Function AddNewSeries(ByVal series As Series) As Integer
            Dim series1 = [Assembly].LoadWithPartialName("Xceed.Chart.Core, Culture=neutral, PublicKeyToken=ba83ff368b7563c6").CreateInstance("Xceed.Chart.Core.PointSeries")
            If seriesName <> "" Then
                series1.set_Name(series.Label)
            End If
            series1.set_PointStyle(7)
            series1.set_Size(0.25!)
            series1.get_DataLabels.set_Mode(0)
            Return Me._chart.get_Charts.get_Item(0).get_Series.Add(series1)
        End Function

        Private Function AddNewSeriesPolar(ByVal seriesName As String) As Integer
            Dim series1 = [Assembly].LoadWithPartialName("Xceed.Chart.Core, Culture=neutral, PublicKeyToken=ba83ff368b7563c6").CreateInstance("Xceed.Chart.Core.PolarSeries")
            If seriesName <> "" Then
                series1.set_Name(seriesName)
            End If

            series1.get_DataLabels.set_Mode(0)

            Return Me._chart.get_Charts.get_Item(0).get_Series.Add(series1)

        End Function
        Private Sub DoubleArrayIntoSeries(ByVal doubleArray As Double(), ByVal xySeries As Object)
            Dim num1 As Double = 1
            Dim num2 As Double = Me._chart.get_Charts.get_Item(0).Axis(2).get_NumericScale.get_Min
            Dim num3 As Double = Me._chart.get_Charts.get_Item(0).Axis(2).get_NumericScale.get_Max
            If (num3 <> num2) Then
                num1 = ((num3 - num2) / CType((doubleArray.Length - 1), Double))
            End If
            Dim num4 As Integer = 0
            Do While (num4 < doubleArray.Length)
                xySeries.AddXY(doubleArray(num4), ((num4 * num1) + num2))
                num4 += 1
            Loop
        End Sub


        Private Sub DoubleArrayIntoSeriesPolar(ByVal doubleArray As Double(), ByVal polarSeries As Object)
            Dim num1 As Double = 1
            Dim num2 As Double = Me._chart.get_Charts.get_Item(0).Axis(2).get_NumericScale.get_Min
            Dim num3 As Double = Me._chart.get_Charts.get_Item(0).Axis(2).get_NumericScale.get_Max
            If (num3 <> num2) Then
                num1 = ((num3 - num2) / CType((doubleArray.Length - 1), Double))
            End If
            Dim num4 As Integer = 0
            Do While (num4 < doubleArray.Length)
                polarSeries.AddPolar(doubleArray(num4), ((num4 * num1) + num2))
                num4 += 1
            Loop
        End Sub

        Public Sub ClearChartSeries() Implements IChartAdapter.IChartAdapter2D.ClearChartSeries
            Me._chart.get_Charts.get_Item(0).get_Series.Clear()
        End Sub

        Public Function GetSeriesNames() As String() Implements IChartAdapter2D.GetSeriesNames
            Dim list1 As New ArrayList
            Dim series1
            For Each series1 In Me._chart.get_Charts.get_Item(0).get_Series
                list1.Add(series1.get_Name)
            Next
            Return CType(list1.ToArray(GetType(String)), String())
        End Function

        Public Function GetSeries() As Series() Implements IChartAdapter.IChartAdapter2D.GetSeries
            Dim list1 As New ArrayList
            Dim series1
            Dim index As Integer
            index = 0
            For Each series1 In Me._chart.get_Charts.get_Item(0).get_Series
                list1.Add(New Series(index, series1.get_Name, New Color))
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

            Dim series1 '= [Assembly].LoadWithPartialName("Xceed.Chart.Core, Culture=neutral, PublicKeyToken=ba83ff368b7563c6").CreateInstance("Xceed.Chart.Core.XYScatterSeries")
            Me._chart.get_Charts.get_Item(0).Axis(2).set_ScaleMode(1)
            Me._chart.get_Charts.get_Item(0).Axis(0).set_ScaleMode(1)

            'MessageBox.Show(Me._chart.get_Charts.get_Item(0).get_Series.get_Item(seriesIndex).GetType.BaseType.Name)

            If (((seriesIndex >= 0) AndAlso (seriesIndex < Me._chart.get_Charts.get_Item(0).get_Series.Count)) AndAlso (Me._chart.get_Charts.get_Item(0).get_Series.get_Item(seriesIndex).GetType.BaseType.Name = "XYScatterSeries")) Then
                series1 = Me._chart.get_Charts.get_Item(0).get_Series.get_Item(seriesIndex)
                series1.get_Values.Clear()
                series1.get_XValues.Clear()
            Else
                series1 = Me._chart.get_Charts.get_Item(0).get_Series(AddNewSeries(""))
                series1.set_PointStyle(7)
                series1.set_Size(0.25!)
                series1.get_DataLabels.set_Mode(0)
            End If
            series1.set_UseXValues(True)
            Dim pointdArray1 As Point2D() = points
            Dim num1 As Integer = 0
            Do While (num1 < pointdArray1.Length)
                Dim pointd1 As Point2D = pointdArray1(num1)
                series1.AddXY(pointd1.Y, pointd1.X)
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

            Dim series1 '= [Assembly].LoadWithPartialName("Xceed.Chart.Core, Culture=neutral, PublicKeyToken=ba83ff368b7563c6").CreateInstance("Xceed.Chart.Core.XYScatterSeries")
            Me._chart.get_Charts.get_Item(0).Axis(2).set_ScaleMode(1)
            Me._chart.get_Charts.get_Item(0).Axis(0).set_ScaleMode(1)

            ' Dim ind = Me._chart.get_Charts.get_Item(0).get_Series.Count
            ' Dim typ = Me._chart.get_Charts.get_Item(0).get_Series.get_Item(seriesIndex).GetType.BaseType.Name

            If (((seriesIndex >= 0) AndAlso (seriesIndex < Me._chart.get_Charts.get_Item(0).get_Series.Count)) AndAlso (Me._chart.get_Charts.get_Item(0).get_Series.get_Item(seriesIndex).GetType.BaseType.Name = "XYScatterSeries")) Then
                series1 = Me._chart.get_Charts.get_Item(0).get_Series.get_Item(seriesIndex)
                series1.get_Values.Clear()
                series1.get_XValues.Clear()
            Else
                series1 = Me._chart.get_Charts.get_Item(0).get_Series(AddNewSeries(""))
                series1.set_PointStyle(7)
                series1.set_Size(0.25!)
                series1.get_DataLabels.set_Mode(0)
            End If
            series1.set_UseXValues(True)
            Me.DoubleArrayIntoSeries(values, series1)
            Me._chart.Refresh()

        End Sub

        Public Function Plot(ByVal values As Double(), ByVal seriesName As String) As Integer Implements IChartAdapter2D.Plot
            Dim num1 As Integer = Me.SeriesNameToSeriesIndex(seriesName)
            Me.Plot(values, num1)
            Return num1
        End Function


        Public Sub PlotPolar(ByVal values As Double(), ByVal series As Series) Implements IChartAdapter2D.PlotPolar
            Dim series1 '= [Assembly].LoadWithPartialName("Xceed.Chart.Core, Culture=neutral, PublicKeyToken=ba83ff368b7563c6").CreateInstance("Xceed.Chart.Core.PolarSeries")
            Me._chart.get_Charts.get_Item(0).Axis(2).set_ScaleMode(1)
            Me._chart.get_Charts.get_Item(0).Axis(0).set_ScaleMode(1)
            If (((series.Index >= 0) AndAlso (series.Index < Me._chart.get_Charts.get_Item(0).get_Series.Count)) AndAlso (Me._chart.get_Charts.get_Item(0).get_Series.get_Item(seriesIndex).GetType.Name = "PolarSeries")) Then
                series1 = Me._chart.get_Charts.get_Item(0).get_Series.get_Item(series.Index)
                series1.get_Values.Clear()
            Else
                series1 = Me._chart.get_Charts.get_Item(0).get_Series(AddNewSeriesPolar(""))
            End If
            Me.DoubleArrayIntoSeriesPolar(values, series1)
            Me._chart.Refresh()
        End Sub

        Public Sub PlotPolar(ByVal points As Point2D(), ByVal series As Series) Implements IChartAdapter2D.PlotPolar

            Dim series1 '= [Assembly].LoadWithPartialName("Xceed.Chart.Core, Culture=neutral, PublicKeyToken=ba83ff368b7563c6").CreateInstance("Xceed.Chart.Core.PolarSeries")
            Me._chart.get_Charts.get_Item(0).Axis(2).set_ScaleMode(1)
            Me._chart.get_Charts.get_Item(0).Axis(0).set_ScaleMode(1)
            If (((series.Index >= 0) AndAlso (series.Index < Me._chart.get_Charts.get_Item(0).get_Series.Count)) AndAlso (Me._chart.get_Charts.get_Item(0).get_Series.get_Item(seriesIndex).GetType.Name = "PolarSeries")) Then
                series1 = Me._chart.get_Charts.get_Item(0).get_Series.get_Item(series.Index)
                series1.get_Values.Clear()
            Else
                series1 = Me._chart.get_Charts.get_Item(0).get_Series(AddNewSeriesPolar(""))
            End If
            Dim pointdArray1 As Point2D() = points
            Dim num1 As Integer = 0
            Do While (num1 < pointdArray1.Length)
                Dim pointd1 As Point2D = pointdArray1(num1)
                series1.AddPolar(pointd1.Y, pointd1.X)
                num1 += 1
            Loop
            Me._chart.Refresh()
        End Sub

        Public Sub PlotPolar(ByVal points As Point2D(), ByVal seriesIndex As Integer) Implements IChartAdapter2D.PlotPolar

            Dim series1 '= [Assembly].LoadWithPartialName("Xceed.Chart.Core, Culture=neutral, PublicKeyToken=ba83ff368b7563c6").CreateInstance("Xceed.Chart.Core.PolarSeries")
            Me._chart.get_Charts.get_Item(0).Axis(2).set_ScaleMode(1)
            Me._chart.get_Charts.get_Item(0).Axis(0).set_ScaleMode(1)
            If (((seriesIndex >= 0) AndAlso (seriesIndex < Me._chart.get_Charts.get_Item(0).get_Series.Count)) AndAlso (Me._chart.get_Charts.get_Item(0).get_Series.get_Item(seriesIndex).GetType.Name = "PolarSeries")) Then
                series1 = Me._chart.get_Charts.get_Item(0).get_Series.get_Item(seriesIndex)
                series1.get_Values.Clear()
            Else
                series1 = Me._chart.get_Charts.get_Item(0).get_Series(AddNewSeriesPolar(""))
            End If
            Dim pointdArray1 As Point2D() = points
            Dim num1 As Integer = 0
            Do While (num1 < pointdArray1.Length)
                Dim pointd1 As Point2D = pointdArray1(num1)
                series1.AddPolar(pointd1.Y, pointd1.X)
                num1 += 1
            Loop
            Me._chart.Refresh()

        End Sub

        Public Function PlotPolar(ByVal points As Point2D(), ByVal seriesName As String) As Integer Implements IChartAdapter2D.PlotPolar
            Dim num1 As Integer = Me.SeriesNameToSeriesIndexPolar(seriesName)
            Me.PlotPolar(points, num1)
            Return num1
        End Function

        Public Sub PlotPolar(ByVal values As Double(), ByVal seriesIndex As Integer) Implements IChartAdapter2D.PlotPolar

            Dim series1 '= [Assembly].LoadWithPartialName("Xceed.Chart.Core, Culture=neutral, PublicKeyToken=ba83ff368b7563c6").CreateInstance("Xceed.Chart.Core.PolarSeries")
            Me._chart.get_Charts.get_Item(0).Axis(2).set_ScaleMode(1)
            Me._chart.get_Charts.get_Item(0).Axis(0).set_ScaleMode(1)
            If (((seriesIndex >= 0) AndAlso (seriesIndex < Me._chart.get_Charts.get_Item(0).get_Series.Count)) AndAlso (Me._chart.get_Charts.get_Item(0).get_Series.get_Item(seriesIndex).GetType.Name = "PolarSeries")) Then
                series1 = Me._chart.get_Charts.get_Item(0).get_Series.get_Item(seriesIndex)
                series1.get_Values.Clear()
            Else
                series1 = Me._chart.get_Charts.get_Item(0).get_Series(AddNewSeriesPolar(""))
            End If
            Me.DoubleArrayIntoSeriesPolar(values, series1)
            Me._chart.Refresh()

        End Sub

        Public Function PlotPolar(ByVal values As Double(), ByVal seriesName As String) As Integer Implements IChartAdapter2D.PlotPolar
            Dim num1 As Integer = Me.SeriesNameToSeriesIndexPolar(seriesName)
            Me.PlotPolar(values, num1)
            Return num1
        End Function


        Private Function SeriesNameToSeriesIndex(ByVal seriesName As String) As Integer
            Dim num1 As Integer = -1
            Dim num2 As Integer = 0
            Do While (num2 < Me._chart.get_Charts.get_Item(0).get_Series.Count)
                If (Me._chart.get_Charts.get_Item(0).get_Series.get_Item(num2).get_Name Is seriesName) Then
                    num1 = num2
                    Exit Do
                End If
                num2 += 1
            Loop
            If (num1 = -1) Then
                num1 = Me.AddNewSeries(seriesName)
            End If
            Return num1
        End Function


        Private Function SeriesToSeriesIndex(ByVal series As Series) As Integer
            Dim num1 As Integer = -1
            Dim num2 As Integer
            For num2 = 0 To Me._chart.get_Charts.get_Item(0).get_Series.Count - 1
                If (Me._chart.get_Charts.get_Item(0).get_Series.get_Item(num2).get_Name Is series.Label) Then
                    num1 = num2
                    series.Index = num2
                    Exit For
                End If
            Next num2
            If (num1 = -1) Then
                AddNewSeries(series)
            End If
            Return num1
        End Function



        Private Function SeriesNameToSeriesIndexPolar(ByVal seriesName As String) As Integer

            Dim num1 As Integer = -1
            Dim num2 As Integer = 0
            Do While (num2 < Me._chart.get_Charts.get_Item(0).get_Series.Count)
                If ((Me._chart.get_Charts.get_Item(0).get_Series.get_Item(num2).get_Name Is seriesName) AndAlso (Me._chart.get_Charts.get_Item(0).get_Series.get_Item(num2).GetType.BaseType.Name = "PolarSeries")) Then
                    num1 = num2
                    Exit Do
                End If
                num2 += 1
            Loop
            If (num1 = -1) Then
                num1 = AddNewSeriesPolar("")
            End If
            Return num1

        End Function

        Public Sub SetAxes(ByVal minX As Double, ByVal maxX As Double, ByVal minY As Double, ByVal maxY As Double) Implements IChartAdapter2D.SetAxes
            Me._chart.get_Charts.get_Item(0).Axis(2).set_ScaleMode(1)
            Me._chart.get_Charts.get_Item(0).Axis(2).get_NumericScale.set_AutoMax(False)
            Me._chart.get_Charts.get_Item(0).Axis(2).get_NumericScale.set_AutoMin(False)
            Me._chart.get_Charts.get_Item(0).Axis(2).get_NumericScale.set_Min(minX)
            Me._chart.get_Charts.get_Item(0).Axis(2).get_NumericScale.set_Max(maxX)
            Me._chart.get_Charts.get_Item(0).Axis(0).set_ScaleMode(1)
            Me._chart.get_Charts.get_Item(0).Axis(0).get_NumericScale.set_AutoMax(False)
            Me._chart.get_Charts.get_Item(0).Axis(0).get_NumericScale.set_AutoMin(False)
            Me._chart.get_Charts.get_Item(0).Axis(0).get_NumericScale.set_Min(minY)
            Me._chart.get_Charts.get_Item(0).Axis(0).get_NumericScale.set_Max(maxY)
            Me._minX = minX
            Me._maxX = maxX
            Me._minY = minY
            Me._maxY = maxY
            Me._chart.Refresh()
        End Sub

        Public Sub SetChartControl(ByVal chartControl As Object) Implements IChartAdapter2D.SetChartControl

            If Not Me.Validate(chartControl) Then
                Throw New ArgumentException("Parameter is not valid.", "chartControl")
            End If
            Me._chart = chartControl
            Me._minX = Me._chart.get_Charts.get_Item(0).Axis(2).get_NumericScale.get_Min
            Me._maxX = Me._chart.get_Charts.get_Item(0).Axis(2).get_NumericScale.get_Max
            Me._minY = Me._chart.get_Charts.get_Item(0).Axis(0).get_NumericScale.get_Min
            Me._maxY = Me._chart.get_Charts.get_Item(0).Axis(0).get_NumericScale.get_Max
            Me._zoom = Me._chart.get_Charts.get_Item(0).get_View.get_Zoom
            Me._plotAreaSize = Me.PlotAreaSize

            Dim chartType As Type = Me._chart.GetType()
            chartType.GetEvent("Paint").AddEventHandler(_chart, New PaintEventHandler(AddressOf Me._chart_Paint))

        End Sub

        Public Function Validate(ByVal obj As Object) As Boolean Implements IChartAdapter.IChartAdapter.Validate
            If (obj.GetType.Name = "ChartControl") Then
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
                Dim single1 As Single = Me._chart.Charts.Item(0).Margins.Top
                Dim single2 As Single = Me._chart.Charts.Item(0).Margins.Bottom
                Dim single3 As Single = Me._chart.Charts.Item(0).Margins.Right
                Dim single4 As Single = Me._chart.Charts.Item(0).Margins.Left
                Dim num1 As Double = Me._chart.Height
                Dim num2 As Double = Me._chart.Width
                Dim num3 As Double = 0
                Dim num4 As Double = 0
                Dim num5 As Double = Me._chart.Charts.Item(0).Height
                Dim num6 As Double = Me._chart.Charts.Item(0).Width
                If (Me._chart.Charts.Item(0).MarginMode = MarginModeStretch) Then
                    num3 = ((num1 * (single2 - single1)) / 100)
                    num4 = ((num2 * (single3 - single4)) / 100)
                Else
                    If (Me._chart.Charts.Item(0).MarginMode = MarginModeFit) Then
                        If (Me._chart.Width > Me._chart.Height) Then
                            num3 = ((num1 * (single2 - single1)) / 100)
                            num4 = ((num6 / num5) * num3)
                        Else
                            num4 = ((num2 * (single3 - single4)) / 100)
                            num3 = ((num5 / num6) * num4)
                        End If
                    Else
                        If (Me._chart.Charts.Item(0).MarginMode = MarginModeNone) Then
                            If (Me._chart.Width > Me._chart.Height) Then
                                num3 = ((num1 * num5) / 100)
                                num4 = ((num6 / num5) * num3)
                            Else
                                num4 = ((num2 * num6) / 100)
                                num3 = ((num5 / num6) * num4)
                            End If
                        End If
                    End If
                End If
                size1.Height = CType(num3, Integer)
                size1.Width = CType(num4, Integer)
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
        Private _zoom As Double

    End Class

End Namespace
