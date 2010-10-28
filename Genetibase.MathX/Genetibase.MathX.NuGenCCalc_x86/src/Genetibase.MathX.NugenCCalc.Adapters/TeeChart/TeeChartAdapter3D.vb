Imports System.Drawing
Imports System.Reflection
Imports System.Collections
Imports System.Windows.Forms

Imports Genetibase.MathX.Core
Imports Genetibase.MathX.NugenCCalc.Adapters.IChartAdapter

Namespace TeeChart

    <ToolboxBitmap(GetType(TeeChartAdapter3D), "Steema.TeeChart.Images.TChart.bmp")> _
    Public Class TeeChartAdapter3D
        Implements IChartAdapter3D, IChartAdapter.IChartAdapter

        ' Events
        Public Event ScopeChanged(ByVal sender As Object, ByVal args As EventArgs) Implements IChartAdapter3D.ScopeChanged
        Public Event SizeChanged(ByVal sender As Object, ByVal args As EventArgs) Implements IChartAdapter3D.SizeChanged

        ' Methods
        Public Sub New()

        End Sub
        Public Sub New(ByVal teeChartControl As Object)
            Me.SetChartControl(teeChartControl)
        End Sub

        '    Private Sub _chart3d_BeforeDraw(ByVal sender As Object, ByVal g As Graphics3D)
        '        Dim num1 As Double = Me._chart3d.Axes.Bottom.Minimum
        '        Dim num2 As Double = Me._chart3d.Axes.Bottom.Maximum
        '        Dim num3 As Double = Me._chart3d.Axes.Left.Minimum
        '        Dim num4 As Double = Me._chart3d.Axes.Left.Maximum
        '        Dim num5 As Double = Me._chart3d.Axes.Depth.Minimum
        '        Dim num6 As Double = Me._chart3d.Axes.Depth.Maximum
        '        Dim num7 As Double = Me._chart3d.Aspect.Zoom
        '        If ((((Me._minX <> num1) OrElse (Me._maxX <> num2)) OrElse ((Me._minY <> num3) OrElse (Me._maxY <> num4))) OrElse (((Me._minZ <> num5) OrElse (Me._maxZ <> num6)) OrElse (Me._zoom <> num7))) Then
        '            Me._minX = num1
        '            Me._maxX = num2
        '            Me._minY = num3
        '            Me._maxY = num4
        '            Me._minZ = num5
        '            Me._maxZ = num6
        '            Me._zoom = num7
        '            Me.OnScopeChanged()
        '        End If
        '    End Sub

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
                Me._chart3d.Axes.Bottom.Title.Caption = "X"
                Me._chart3d.Axes.Depth.Title.Caption = "Y"
                Me._chart3d.Axes.DepthTop.Title.Caption = "Y"
                Me._chart3d.Axes.Left.Title.Caption = "Z"
                Me._chart3d.Series.Clear()
                Dim pointsd1 = [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Points3D")
                Dim num1 As Integer = 0
                Do While (num1 < points.Length)
                    pointsd1.Add(points(num1).X, points(num1).Z, points(num1).Y)
                    num1 += 1
                Loop
                pointsd1.LinePen.Visible = False
                pointsd1.Pointer.Style = 0 'PointerStyles.Rectangle
                pointsd1.Pointer.HorizSize = 1
                pointsd1.Pointer.VertSize = 1
                Me._chart3d.Series.Add(pointsd1)
            End If
        End Sub


        Public Sub PlotSurface(ByVal gridLeftBottom As Point2D, ByVal gridRightTop As Point2D, ByVal values(,) As Double) Implements IChartAdapter3D.PlotSurface
            If (Not values Is Nothing) Then
                Me._chart3d.Axes.Bottom.Title.Caption = "X"
                Me._chart3d.Axes.Depth.Title.Caption = "Y"
                Me._chart3d.Axes.DepthTop.Title.Caption = "Y"
                Me._chart3d.Axes.Left.Title.Caption = "Z"
                Me._chart3d.Series.Clear()
                Dim surface1 = [Assembly].LoadWithPartialName("TeeChart, Culture=neutral, PublicKeyToken=9c8126276c77bdb7").CreateInstance("Steema.TeeChart.Styles.Surface")
                surface1.IrregularGrid = True
                Dim num1 As Integer = values.GetLength(0)
                Dim num2 As Integer = values.GetLength(1)
                Dim num3 As Double = ((gridRightTop.X - gridLeftBottom.X) / CType((num1 - 1), Double))
                Dim num4 As Double = ((gridRightTop.Y - gridLeftBottom.Y) / CType((num2 - 1), Double))
                Dim num5 As Integer = 0
                Do While (num5 < num1)
                    Dim num6 As Integer = 0
                    Do While (num6 < num2)
                        surface1.Add(((num5 * num3) + gridLeftBottom.X), values(num5, num6), ((num6 * num4) + gridLeftBottom.Y))
                        num6 += 1
                    Loop
                    num5 += 1
                Loop
                Me._chart3d.Series.Add(surface1)
            End If
        End Sub

        Public Sub SetChartControl(ByVal chartControl As Object) Implements IChartAdapter3D.SetChartControl
            If Not Me.Validate(chartControl) Then
                Throw New ArgumentException("Parameter is not valid.", "chartControl")
            End If
            Me._chart3d = chartControl
            Me._minX = Me._chart3d.Axes.Bottom.Minimum
            Me._maxX = Me._chart3d.Axes.Bottom.Maximum
            Me._minY = Me._chart3d.Axes.Left.Minimum
            Me._maxY = Me._chart3d.Axes.Left.Maximum
            Me._minZ = Me._chart3d.Axes.Depth.Minimum
            Me._maxZ = Me._chart3d.Axes.Depth.Maximum
            Me._zoom = Me._chart3d.Aspect.Zoom


            'AddHandler Me._chart3d.BeforeDraw, New PaintChartEventHandler(AddressOf Me._chart3d_BeforeDraw)



            Dim chartType As Type = Me._chart3d.GetType()
            chartType.GetEvent("SizeChanged").AddEventHandler(_chart3d, New EventHandler(AddressOf Me._chart3d_SizeChanged))
            'AddHandler Me._chart3d.SizeChanged, New EventHandler(AddressOf Me._chart3d_SizeChanged)

        End Sub


        Public Function Validate(ByVal obj As Object) As Boolean Implements IChartAdapter.IChartAdapter.Validate
            If (obj.GetType.Name = "TChart") Then
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