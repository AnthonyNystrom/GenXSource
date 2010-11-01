Option Strict On
Option Explicit On

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports Genetibase.NuGenObjective

' This code was conceived and written by Gustavo Arranhado, who
' is a great graphic artist. I have just cleaned it up a bit
' and translated it to VB from the original C#.
Public NotInheritable Class DrawingHelper
    Private Shared Sub PlotRoundRectangle(ByVal rectangle As Rectangle, ByVal size As Single, ByVal gp As GraphicsPath)
        With gp
            .AddArc(rectangle.X, rectangle.Y, size, size, 180, 90)
            .AddArc(rectangle.X + rectangle.Width - size, rectangle.Y, size, size, 270, 90)
            .AddArc(rectangle.X + rectangle.Width - size, rectangle.Y + rectangle.Height - size, size, size, 0, 90)
            .AddArc(rectangle.X, rectangle.Y + rectangle.Height - size, size, size, 90, 90)
            .CloseFigure()
        End With
    End Sub

    Public Shared Sub DrawRoundRectangle( _
                                ByVal g As Graphics, _
                                ByVal pen As Pen, _
                                ByVal rectangle As Rectangle, _
                                ByVal radius As Single _
                            )
        Dim size As Single = radius * 2.0!

        Dim gp As GraphicsPath = New GraphicsPath()
        Using gp
            PlotRoundRectangle(rectangle, size, gp)
            g.DrawPath(pen, gp)
        End Using
    End Sub

    Public Shared Sub FillRoundRectangle( _
                        ByVal g As Graphics, _
                        ByVal brush As Brush, _
                        ByVal rectangle As Rectangle, _
                        ByVal radius As Single _
                    )
        Dim size As Single = radius * 2.0!

        Dim gp As GraphicsPath = New GraphicsPath()
        Using gp
            PlotRoundRectangle(rectangle, size, gp)
            g.FillPath(brush, gp)
        End Using
    End Sub

    Public Shared Function ElementSize( _
                                ByVal g As Graphics, _
                                ByVal font As Font, _
                                ByVal text As String _
                            ) As Size
        Dim size As SizeF = g.MeasureString(text, font)

        ' Resize element by the size of text
        Return New Size(CInt(size.Width + 20.0!), CInt(size.Height + 20.0!))
    End Function

    Public Shared Sub DrawElement( _
                        ByVal g As Graphics, _
                        ByVal font As Font, _
                        ByVal outlinecolor As Color, _
                        ByVal gradientStartColor As Color, _
                        ByVal gradientEndColor As Color, _
                        ByVal textcolor As Color, _
                        ByVal colormode As LinearGradientMode, _
                        ByVal roundradius As Single, _
                        ByVal selected As Boolean, _
                        ByVal text As String, _
                        ByVal rect As Rectangle _
                    )

        Using brush As New SolidBrush(Color.FromArgb(48, 0, 0, 0))
            FillRoundRectangle( _
                g, _
                brush, _
                New Rectangle(rect.X + 1, rect.Y + 1, rect.Width, rect.Height), _
                roundradius _
            )
        End Using


        Using brush As New SolidBrush(Color.FromArgb(32, 0, 0, 0))
            FillRoundRectangle( _
                g, _
                brush, _
                New Rectangle(rect.X + 2, rect.Y + 2, rect.Width, rect.Height), _
                roundradius _
            )
        End Using


        Using brush As New SolidBrush(Color.FromArgb(16, 0, 0, 0))
            FillRoundRectangle( _
                g, _
                brush, _
                New Rectangle(rect.X + 3, rect.Y + 3, rect.Width, rect.Height), _
                roundradius _
            )
        End Using

        Using brush As New LinearGradientBrush(rect, gradientStartColor, gradientEndColor, colormode)
            FillRoundRectangle( _
                g, _
                brush, _
                rect, _
                roundradius _
            )
        End Using

        Dim sf As New StringFormat
        sf.Alignment = StringAlignment.Center
        sf.LineAlignment = StringAlignment.Center

        Using Brush As New SolidBrush(textcolor)
            g.DrawString( _
                text, _
                font, _
                Brush, _
                rect, _
                sf _
            )
        End Using

        If selected Then
            Using pen As New Pen(outlinecolor, 2.0!)
                DrawRoundRectangle( _
                    g, _
                    pen, _
                    rect, _
                    roundradius _
                )
            End Using
        Else
            Using pen As New Pen(Color.FromArgb(64, outlinecolor), 1.0!)
                DrawRoundRectangle( _
                    g, _
                    pen, _
                    rect, _
                    roundradius _
                )
            End Using
        End If
        sf.Dispose()
    End Sub

    Public Shared Function LineLineIntersection( _
            ByVal line1Point1 As Point, _
            ByVal line1Point2 As Point, _
            ByVal line2Point1 As Point, _
            ByVal line2Point2 As Point _
        ) As Point

        Dim result As Point

        Dim A1 As Integer = line1Point2.Y - line1Point1.Y
        Dim B1 As Integer = line1Point1.X - line1Point2.X
        Dim C1 As Integer = A1 * line1Point1.X + B1 * line1Point1.Y

        Dim A2 As Integer = line2Point2.Y - line2Point1.Y
        Dim B2 As Integer = line2Point1.X - line2Point2.X
        Dim C2 As Integer = A2 * line2Point1.X + B2 * line2Point1.Y

        Dim det As Double = CDbl(A1 * B2 - A2 * B1)

        If (det = 0) Then
            result = Point.Empty
        Else
            result = New Point( _
                        CInt(CDbl(B2 * C1 - B1 * C2) / det), _
                        CInt(CDbl(A1 * C2 - A2 * C1) / det) _
                    )
        End If

        Return result
    End Function

    Public Shared Function PageSize(ByVal page As Page) As Size
        Dim result As New Size(0, 0)

        If page IsNot Nothing Then
            With page
                If .Roles.Count > 0 Then
                    result.Width = 192
                ElseIf .States.Count > 0 Then
                    result.Width = 48
                Else
                    result.Width = 15
                End If
                result.Width += (144 * .States.Count) + 128

                result.Height = (Math.Max(.Roles.Count, .Actions.Count) * 64) + 192
            End With
        End If
        Return result
    End Function

    Private Sub New()

    End Sub
End Class
