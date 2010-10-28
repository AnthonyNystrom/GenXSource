Imports Genetibase.MathX.Core

Namespace IChartAdapter

    Public Interface IChartAdapter2D
        Inherits IChartAdapter

        Sub SetAxes(ByVal minX As Double, ByVal maxX As Double, ByVal minY As Double, ByVal maxY As Double)
        Sub ClearChartSeries()

        Function GetSeriesNames() As String()
        Function GetSeries() As Series()

        Function Plot(ByVal values() As Double, ByVal seriesName As String) As Integer
        Function Plot(ByVal points() As Point2D, ByVal seriesName As String) As Integer
        Sub Plot(ByVal values() As Double, ByVal seriesIndex As Integer)
        Sub Plot(ByVal points() As Point2D, ByVal seriesIndex As Integer)


        Sub Plot(ByVal values() As Double, ByVal series As Series)
        Sub Plot(ByVal points() As Point2D, ByVal series As Series)

        Function PlotPolar(ByVal points() As Point2D, ByVal seriesName As String) As Integer
        Sub PlotPolar(ByVal points() As Point2D, ByVal seriesIndex As Integer)
        Sub PlotPolar(ByVal points() As Point2D, ByVal series As Series)

        Function PlotPolar(ByVal values() As Double, ByVal seriesName As String) As Integer
        Sub PlotPolar(ByVal values() As Double, ByVal seriesIndex As Integer)
        Sub PlotPolar(ByVal values() As Double, ByVal series As Series)

        ReadOnly Property PlotAreaSize() As Drawing.Size

        ReadOnly Property MinX() As Double
        ReadOnly Property MaxX() As Double
        ReadOnly Property MinY() As Double
        ReadOnly Property MaxY() As Double

    End Interface

End Namespace
