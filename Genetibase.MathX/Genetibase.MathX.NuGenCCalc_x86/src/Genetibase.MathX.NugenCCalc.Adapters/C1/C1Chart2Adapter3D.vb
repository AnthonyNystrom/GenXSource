Imports System.Drawing
Imports System.Reflection
Imports System.Collections
Imports System.Windows.Forms

Imports Genetibase.MathX.Core
Imports Genetibase.MathX.NugenCCalc.Adapters.IChartAdapter


Namespace C1

    <ToolboxBitmap(GetType(C1ChartAdapter3D), "Win.C1Chart3D.C1Chart3D.bmp")> _
    Public Class C1Chart2Adapter3D
        Implements IChartAdapter3D, IChartAdapter.IChartAdapter


        ' Events
        Public Event ScopeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Implements IChartAdapter.IChartAdapter3D.ScopeChanged
        Public Event SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Implements IChartAdapter.IChartAdapter3D.SizeChanged

        ' Methods
        Public Sub New()

        End Sub

        Public Sub New(ByVal c1Chart3D As Object)
            Me.SetChartControl(c1Chart3D)
        End Sub


        Private Sub _chart3d_Paint(ByVal sender As Object, ByVal e As PaintEventArgs)
            Dim num1 As Double = Me._chart3d.get_ChartArea.get_AxisX.get_Min
            Dim num2 As Double = Me._chart3d.get_ChartArea.get_AxisX.get_Max
            Dim num3 As Double = Me._chart3d.get_ChartArea.get_AxisY.get_Min
            Dim num4 As Double = Me._chart3d.get_ChartArea.get_AxisY.get_Max
            Dim num5 As Double = Me._chart3d.get_ChartArea.get_AxisZ.get_Min
            Dim num6 As Double = Me._chart3d.get_ChartArea.get_AxisZ.get_Max
            Dim num7 As Double = Me._chart3d.get_ChartArea.get_View.get_ViewportScale
            If ((((Me._minX <> num1) OrElse (Me._maxX <> num2)) OrElse ((Me._minY <> num3) OrElse (Me._maxY <> num4))) OrElse (((Me._minZ <> num5) OrElse (Me._maxZ <> num6)) OrElse (Me._scale <> num7))) Then
                Me._minX = num1
                Me._maxX = num2
                Me._minY = num3
                Me._maxY = num4
                Me._minZ = num5
                Me._maxZ = num6
                Me._scale = num7
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
                Me._chart3d.get_ChartArea.get_AxisX.set_Title("X")
                Me._chart3d.get_ChartArea.get_AxisY.set_Title("Y")
                Me._chart3d.get_ChartArea.get_AxisZ.set_Title("Z")

                Dim group1 = Me._chart3d.get_ChartGroups.get_Item(0)

                group1.get_ChartData.set_Layout(0)
                group1.get_ChartData.get_SetPoint.get_SeriesCollection.Clear()

                Dim pointArray1 = Array.CreateInstance([Assembly].LoadWithPartialName("C1.Win.C1Chart3D.2, Culture=neutral, PublicKeyToken=a22e16972c085838").GetType("C1.Win.C1Chart3D.Chart3DPoint"), (points.Length))
                Dim pointType = [Assembly].LoadWithPartialName("C1.Win.C1Chart3D.2, Culture=neutral, PublicKeyToken=a22e16972c085838").GetType("C1.Win.C1Chart3D.Chart3DPoint")

                Dim num1 As Integer
                For num1 = 0 To points.Length - 1

                    Dim pt = Activator.CreateInstance(pointType)
                    pt.X = points(num1).X
                    pt.Y = points(num1).Y
                    pt.Z = points(num1).Z

                    pointArray1(num1) = pt

                Next num1


                group1.get_ChartData.get_SetPoint.AddSeries(pointArray1)
                group1.get_ChartData.get_SetPoint.get_SeriesCollection.get_Item(0).get_Style.get_SymbolStyle.set_Size(1)
                group1.set_ChartType(2)
                group1.set_ChartType(0)

            End If

        End Sub

        Public Sub PlotSurface(ByVal gridLeftBottom As Point2D, ByVal gridRightTop As Point2D, ByVal values(,) As Double) Implements IChartAdapter3D.PlotSurface
            If (Not values Is Nothing) Then
                Me._chart3d.get_ChartArea.get_AxisX.set_Title("X")
                Me._chart3d.get_ChartArea.get_AxisY.set_Title("Y")
                Me._chart3d.get_ChartArea.get_AxisZ.set_Title("Z")
                Dim num1 As Integer = values.GetLength(0)
                Dim num2 As Integer = values.GetLength(1)
                Dim num3 As Double = ((gridRightTop.X - gridLeftBottom.X) / CType((num1 - 1), Double))
                Dim num4 As Double = ((gridRightTop.Y - gridLeftBottom.Y) / CType((num2 - 1), Double))
                Dim group1 = Me._chart3d.get_ChartGroups.get_Item(0)
                group1.get_ChartData.set_Layout(1)
                group1.get_ChartData.get_SetGrid.set_RowCount(num2)
                group1.get_ChartData.get_SetGrid.set_RowDelta(num4)
                group1.get_ChartData.get_SetGrid.set_RowOrigin(gridLeftBottom.Y)
                group1.get_ChartData.get_SetGrid.set_ColumnCount(num1)
                group1.get_ChartData.get_SetGrid.set_ColumnDelta(num3)
                group1.get_ChartData.get_SetGrid.set_ColumnOrigin(gridLeftBottom.X)
                group1.get_ChartData.get_SetGrid.set_GridData(values)
                group1.set_ChartType(0)
                group1.set_ChartType(2)
            End If

        End Sub

        Public Sub SetChartControl(ByVal chartControl As Object) Implements IChartAdapter3D.SetChartControl
            If Not Me.Validate(chartControl) Then
                Throw New ArgumentException("Parameter is not valid.", "chartControl")
            End If
            Me._chart3d = chartControl
            Me._minX = Me._chart3d.get_ChartArea.get_AxisX.get_Min
            Me._maxX = Me._chart3d.get_ChartArea.get_AxisX.get_Max
            Me._minY = Me._chart3d.get_ChartArea.get_AxisY.get_Min
            Me._maxY = Me._chart3d.get_ChartArea.get_AxisY.get_Max
            Me._minZ = Me._chart3d.get_ChartArea.get_AxisZ.get_Min
            Me._maxZ = Me._chart3d.get_ChartArea.get_AxisZ.get_Max
            Me._scale = Me._chart3d.get_ChartArea.get_View.get_ViewportScale

            Dim chartType As Type = Me._chart3d.GetType()
            chartType.GetEvent("Paint").AddEventHandler(Me._chart3d, New PaintEventHandler(AddressOf Me._chart3d_Paint))
            chartType.GetEvent("SizeChanged").AddEventHandler(Me._chart3d, New EventHandler(AddressOf Me._chart3d_SizeChanged))

        End Sub

        Public Function Validate(ByVal obj As Object) As Boolean Implements IChartAdapter.IChartAdapter.Validate
            If (obj.GetType.Name = "C1Chart3D" And obj.GetType.Assembly.ImageRuntimeVersion(1) = "2") Then
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
        Private _scale As Double


    End Class

End Namespace
