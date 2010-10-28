Imports System.Math

Namespace NuGenFunctionPlotting

    Public MustInherit Class FunctionPlot
        Public Lines As ArrayList, Points As ArrayList

        Dim X0 As Int32, Y0 As Int32, XM As Int32, YM As Int32 '---- Plot limits
        Dim VA As Int32, HA As Int32 'Vertical angle & Horiz view angle
        Dim SX As Int32, SY As Int32 'Start position
        Dim NR As Int32, L As Int32 '----  No. of steps, Step length

        Sub New(ByVal _X0 As Int32, ByVal _Y0 As Int32, ByVal _XM As Int32, ByVal _YM As Int32, ByVal _VA As Int32, ByVal _HA As Int32, ByVal _SX As Int32, ByVal _SY As Int32, ByVal _NR As Int32, ByVal _L As Int32)
            Lines = New ArrayList
            Points = New ArrayList

            X0 = _X0 : Y0 = _Y0 : XM = _XM : YM = _YM
            VA = _VA : HA = _HA 'Vertical angle & Horiz view angle
            SY = _SY : SX = _SX 'Start position
            NR = _NR : L = _L '----  No. of steps, Step length
        End Sub

        ''' <summary> 3-D grid plot </summary>
        Public Sub ThreeDGridPlot()

            '350:    SCREEN(2)
            Dim X1 As Decimal, Y1 As Decimal, X2 As Decimal, Y2 As Decimal
            Dim X As Decimal, Y As Decimal
            Dim DX1 As Decimal, DY1 As Decimal, DX As Decimal, DY As Decimal

            Dim CV As Decimal, SV As Decimal, CH As Decimal, SH As Decimal
            Dim I As Decimal, J As Decimal
            Dim lin As Line, p1 As Point, p2 As Point

            Dim STX As Decimal, STY As Decimal

            CV = Cos(VA / 57.3) : SV = Sin(VA / 57.3)
            CH = Cos(HA / 57.3) : SH = Sin(HA / 57.3)

            X = SX : Y = SY : DX1 = L * SH * 3 : DY1 = L * SV * CH
            X1 = X : Y1 = Y : DX = L * CH * 3 : DY = L * SV * SH

            STX = (XM - X0) / NR : STY = (YM - Y0) / NR

            p1 = New Point(X, Y)
            p2 = New Point(X + NR * DX1, Y - NR * DY1)
            lin = New Line(p1, p2) ' Line (X, Y)-(X + NR * DX1, Y - NR * DY1)
            Lines.Add(lin)

            For I = X0 To XM Step STX
                p1 = New Point(X, Y)
                Points.Add(p1) ' PSET(X, Y)

                For J = Y0 To YM Step STY
                    Y2 = Y1 - CV * DefinedFunction(I, J)
                    p2 = New Point(X1, Y2)
                    lin = New Line(p1, p2) 'LINE -(X1,Y2)
                    Lines.Add(lin)
                    p1 = p2.DeepCopy()
                    X1 = X1 + DX1 : Y1 = Y1 - DY1
                Next J : X = X - DX : Y = Y - DY : X1 = X : Y1 = Y
            Next I : X = SX : Y = SY

            p1 = New Point(X, Y)
            p2 = New Point(X - NR * DX, Y - NR * DY)
            lin = New Line(p1, p2) ' LINE (X,Y)-(X-NR*DX,Y-NR*DY)
            Lines.Add(lin)

            For J = Y0 To YM Step STY
                p1 = New Point(X, Y)
                Points.Add(p1) ' PSET(X, Y)
                X1 = X : Y1 = Y

                For I = X0 To XM Step STX
                    Y2 = Y1 - CV * DefinedFunction(I, J)
                    p2 = New Point(X1, Y2)
                    lin = New Line(p1, p2) 'LINE -(X1,Y2)
                    Lines.Add(lin)
                    p1 = p2.DeepCopy()
                    X1 = X1 - DX : Y1 = Y1 - DY
                Next I : X = X + DX1 : Y = Y - DY1
            Next J

            'OUT(&H3D9, 14)
        End Sub

        Public MustOverride Function DefinedFunction(ByVal X As Decimal, ByVal Y As Decimal) As Decimal

    End Class

    Public Structure Point
        Public x As Integer
        Public y As Integer

        Public Sub New(ByVal x As Decimal, ByVal y As Decimal)
            Me.x = Int(x)
            Me.y = Int(y)
        End Sub

        Public Overrides Function ToString() As String
            Return "(" + x.ToString() + ", " + y.ToString() + ")"
        End Function

        Public Function DeepCopy() As Point

            Dim newP As New Point(x, y)
            Return newP
        End Function

    End Structure

    Public Structure Line
        Public p1 As Point
        Public p2 As Point

        Public Sub New(ByVal p1 As Point, ByVal p2 As Point)
            Me.p1 = p1
            Me.p2 = p2
        End Sub

        Public Overrides Function ToString() As String
            Return p1.ToString() + " - " + p2.ToString()
        End Function

    End Structure

End Namespace