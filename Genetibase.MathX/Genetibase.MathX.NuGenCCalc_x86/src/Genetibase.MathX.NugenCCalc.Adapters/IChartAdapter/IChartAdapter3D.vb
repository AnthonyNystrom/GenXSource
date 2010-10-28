Imports Genetibase.MathX.Core

Namespace IChartAdapter

    Public Interface IChartAdapter3D
        Inherits IChartAdapter

        Sub PlotSurface(ByVal gridLeftBottom As Point2D, ByVal gridRightTop As Point2D, ByVal values(,) As Double)
        Sub PlotPoints(ByVal points() As Point3D)

    End Interface

End Namespace
