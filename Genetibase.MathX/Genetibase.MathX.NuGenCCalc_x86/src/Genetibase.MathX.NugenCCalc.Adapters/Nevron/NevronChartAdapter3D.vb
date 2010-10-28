Imports System.Drawing
Imports System.Reflection
Imports System.Collections
Imports System.Windows.Forms

Imports Genetibase.MathX.Core
Imports Genetibase.MathX.NugenCCalc.Adapters.IChartAdapter

Namespace Nevron
    <ToolboxBitmap(GetType(NevronChartAdapter3D), "NChart.NChartControl.bmp")> _
       Public Class NevronChartAdapter3D
        Implements IChartAdapter3D, IChartAdapter.IChartAdapter

        ' Events
        Public Event ScopeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Implements IChartAdapter.IChartAdapter3D.ScopeChanged
        Public Event SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Implements IChartAdapter.IChartAdapter3D.SizeChanged

        'Constants
        Dim StandardAxisPrimaryX = 2
        Dim StandardAxisPrimaryY = 0
        Dim StandardAxisDepth = 4

        Dim RenderDeviceGDI = 0
        Dim RenderDeviceOpenGL = 1

        Dim AxisScaleModeNumeric = 1
        Dim PointStyleSphere = 6
        Dim DataLabelsModeNone = 0

        Dim PredefinedProjectionPerspectiveTilted = 15


        ' Methods
        Public Sub New()

        End Sub
        Public Sub New(ByVal nevronChartControl As Object)
            Me.SetChartControl(nevronChartControl)
        End Sub


        Private Sub _chart3d_Paint(ByVal sender As Object, ByVal e As PaintEventArgs)
            Dim num1 As Double = Me._chart3d.get_Charts.Item(0).Axis(StandardAxisPrimaryX).NumericScale.Min
            Dim num2 As Double = Me._chart3d.get_Charts.Item(0).Axis(StandardAxisPrimaryX).NumericScale.Max
            Dim num3 As Double = Me._chart3d.get_Charts.Item(0).Axis(StandardAxisPrimaryY).NumericScale.Min
            Dim num4 As Double = Me._chart3d.get_Charts.Item(0).Axis(StandardAxisPrimaryY).NumericScale.Max
            Dim num5 As Double = Me._chart3d.get_Charts.Item(0).Axis(StandardAxisDepth).NumericScale.Min
            Dim num6 As Double = Me._chart3d.get_Charts.Item(0).Axis(StandardAxisDepth).NumericScale.Max
            Dim num7 As Double = Me._chart3d.get_Charts.Item(0).View.Zoom
            If ((((Me._minX <> num1) OrElse (Me._maxX <> num2)) OrElse ((Me._minY <> num3) OrElse (Me._maxY <> num4))) OrElse (((Me._minZ <> num5) OrElse (Me._maxZ <> num6)) OrElse (Me._zoom <> num7))) Then
                Me._minX = num1
                Me._maxX = num2
                Me._minY = num3
                Me._maxY = num4
                Me._minZ = num5
                Me._maxZ = num6
                Me._zoom = num7
                Me.OnScopeChanged()
            End If
        End Sub


        Private Sub _chart3d_SizeChanged(ByVal sender As Object, ByVal e As EventArgs)
            Me.OnSizeChanged()
        End Sub
        Private Sub OnScopeChanged()
            RaiseEvent ScopeChanged(Me, New EventArgs)
        End Sub
        Private Sub OnSizeChanged()
            RaiseEvent SizeChanged(Me, New EventArgs)
        End Sub


        Public Sub PlotPoints(ByVal points As Point3D()) Implements IChartAdapter3D.PlotPoints

            If (Not points Is Nothing) Then
                Dim chart1 = Me._chart3d.get_Charts.Item(0)
                Me._chart3d.get_Settings.RenderDevice = RenderDeviceGDI
                chart1.Series.Clear()
                chart1.Axis(StandardAxisPrimaryX).Title = "X"
                chart1.Axis(StandardAxisPrimaryY).Title = "Z"
                chart1.Axis(StandardAxisDepth).Title = "Y"
                chart1.Axis(StandardAxisPrimaryX).ScaleMode = AxisScaleModeNumeric
                chart1.Axis(StandardAxisPrimaryY).ScaleMode = AxisScaleModeNumeric
                chart1.Axis(StandardAxisDepth).ScaleMode = AxisScaleModeNumeric
                chart1.Axis(StandardAxisPrimaryX).NumericScale.AutoMax = True
                chart1.Axis(StandardAxisPrimaryX).NumericScale.AutoMin = True
                chart1.Axis(StandardAxisPrimaryY).NumericScale.AutoMax = True
                chart1.Axis(StandardAxisPrimaryY).NumericScale.AutoMin = True
                chart1.Axis(StandardAxisDepth).NumericScale.AutoMax = True
                chart1.Axis(StandardAxisDepth).NumericScale.AutoMin = True
                Dim series1 = [Assembly].LoadWithPartialName("Nevron.NChartCore, Culture=neutral, PublicKeyToken=2961c51bb98125d2").CreateInstance("Nevron.NChart.NPointSeries")
                series1.UseXValues = True
                series1.UseZValues = True
                series1.PointStyle = PointStyleSphere
                series1.Size = 1.0!
                series1.DataLabels.Mode = DataLabelsModeNone
                Dim num1 As Integer = 0
                Do While (num1 < points.Length)
                    series1.AddPoint(points(num1).Z, points(num1).X, points(num1).Y)
                    num1 += 1
                Loop
                chart1.Series.Add(series1)
                Me._chart3d.get_Settings.RenderDevice = RenderDeviceOpenGL
                chart1.Width = 60.0!
                chart1.Depth = 60.0!
                chart1.Height = 25.0!
                chart1.View.SetPredefinedProjection(PredefinedProjectionPerspectiveTilted)

                Dim tdo = [Assembly].LoadWithPartialName("Nevron.GraphicsCore, Culture=neutral, PublicKeyToken=19b73d3fb4eb0a72").CreateInstance("Nevron.GraphicsCore.NTrackballDragOperation")
                Me._chart3d.get_InteractivityOperations.Add(tdo)

            End If

        End Sub
        Public Sub PlotSurface(ByVal gridLeftBottom As Point2D, ByVal gridRightTop As Point2D, ByVal values(,) As Double) Implements IChartAdapter3D.PlotSurface
            If (Not values Is Nothing) Then
                Dim chart1 = Me._chart3d.get_Charts.Item(0)
                Me._chart3d.get_Settings.RenderDevice = RenderDeviceGDI
                chart1.Series.Clear()
                chart1.Axis(StandardAxisPrimaryX).Title = "X"
                chart1.Axis(StandardAxisPrimaryY).Title = "Z"
                chart1.Axis(StandardAxisDepth).Title = "Y"
                chart1.Axis(StandardAxisPrimaryX).ScaleMode = AxisScaleModeNumeric
                chart1.Axis(StandardAxisPrimaryY).ScaleMode = AxisScaleModeNumeric
                chart1.Axis(StandardAxisDepth).ScaleMode = AxisScaleModeNumeric
                chart1.Axis(StandardAxisPrimaryX).NumericScale.AutoMax = True
                chart1.Axis(StandardAxisPrimaryX).NumericScale.AutoMin = True
                chart1.Axis(StandardAxisPrimaryY).NumericScale.AutoMax = True
                chart1.Axis(StandardAxisPrimaryY).NumericScale.AutoMin = True
                chart1.Axis(StandardAxisDepth).NumericScale.AutoMax = True
                chart1.Axis(StandardAxisDepth).NumericScale.AutoMin = True
                Dim series1 = [Assembly].LoadWithPartialName("Nevron.NChartCore, Culture=neutral, PublicKeyToken=2961c51bb98125d2").CreateInstance("Nevron.NChart.NMeshSurfaceSeries")
                Dim num1 As Integer = values.GetLength(0)
                Dim num2 As Integer = values.GetLength(1)
                Dim num3 As Double = ((gridRightTop.X - gridLeftBottom.X) / CType((num1 - 1), Double))
                Dim num4 As Double = ((gridRightTop.Y - gridLeftBottom.Y) / CType((num2 - 1), Double))
                series1.Data.SetGridSize(num1, num2)
                Dim num5 As Integer = 0
                Do While (num5 < num1)
                    Dim num6 As Integer = 0
                    Do While (num6 < num2)
                        series1.Data.SetValue(num5, num6, values(num5, num6), ((num5 * num3) + gridLeftBottom.X), ((num6 * num4) + gridLeftBottom.Y))
                        num6 += 1
                    Loop
                    num5 += 1
                Loop
                series1.SmoothPalette = True
                chart1.Series.Add(series1)
                Me._chart3d.get_Settings.RenderDevice = RenderDeviceOpenGL
                chart1.Width = 60.0!
                chart1.Depth = 60.0!
                chart1.Height = 25.0!
                chart1.View.SetPredefinedProjection(PredefinedProjectionPerspectiveTilted)

                Dim tdo = [Assembly].LoadWithPartialName("Nevron.GraphicsCore, Culture=neutral, PublicKeyToken=19b73d3fb4eb0a72").CreateInstance("Nevron.GraphicsCore.NTrackballDragOperation")
                Me._chart3d.get_InteractivityOperations.Add(tdo)

            End If

        End Sub

        Public Sub SetChartControl(ByVal chartControl As Object) Implements IChartAdapter3D.SetChartControl

            If Not Me.Validate(chartControl) Then
                Throw New ArgumentException("Parameter is not valid.", "chartControl")
            End If
            Me._chart3d = chartControl
            Me._minX = Me._chart3d.get_Charts.Item(0).Axis(StandardAxisPrimaryX).NumericScale.Min
            Me._maxX = Me._chart3d.get_Charts.Item(0).Axis(StandardAxisPrimaryX).NumericScale.Max
            Me._minY = Me._chart3d.get_Charts.Item(0).Axis(StandardAxisPrimaryY).NumericScale.Min
            Me._maxY = Me._chart3d.get_Charts.Item(0).Axis(StandardAxisPrimaryY).NumericScale.Max
            Me._minZ = Me._chart3d.get_Charts.Item(0).Axis(StandardAxisDepth).NumericScale.Min
            Me._maxZ = Me._chart3d.get_Charts.Item(0).Axis(StandardAxisDepth).NumericScale.Max
            Me._zoom = Me._chart3d.get_Charts.Item(0).View.Zoom


            Dim chartType As Type = Me._chart3d.GetType()
            chartType.GetEvent("Paint").AddEventHandler(Me._chart3d, New PaintEventHandler(AddressOf Me._chart3d_Paint))
            chartType.GetEvent("SizeChanged").AddEventHandler(Me._chart3d, New EventHandler(AddressOf Me._chart3d_SizeChanged))
            '            AddHandler Me._chart3d.Paint, New PaintEventHandler(AddressOf Me._chart3d_Paint)
            '            AddHandler Me._chart3d.SizeChanged, New EventHandler(AddressOf Me._chart3d_SizeChanged)

        End Sub


        Public Function Validate(ByVal obj As Object) As Boolean Implements IChartAdapter.IChartAdapter.Validate
            If (obj.GetType.Name = "NChartControl") Then
                Return True
            End If
            Return False
        End Function

        ' Properties
        Public ReadOnly Property ChartControl() As Object Implements IChartAdapter3D.ChartControl
            Get
                Return Me._chart3d
            End Get
        End Property


        ' Fields
        Private _chart3d As Object
        Private _maxX As Double
        Private _maxY As Double
        Private _maxZ As Double
        Private _minX As Double
        Private _minY As Double
        Private _minZ As Double
        Private _zoom As Double

    End Class

End Namespace