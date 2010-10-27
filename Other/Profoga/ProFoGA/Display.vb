Imports System.Drawing

Public Class Display

    'Public Aminos As Generic.Dictionary(Of Integer, Aminoacids.Molecule)
    Public Loaded As Boolean = False

    Private Function AtomPoint(ByVal g As Graphics, ByVal anAtom As Aminoacids.Atom) As Drawing.PointF
        Dim result As New Drawing.PointF

        Dim sx As Integer = CInt(g.ClipBounds.Right - g.ClipBounds.Left)
        Dim sy As Integer = CInt(g.ClipBounds.Bottom - g.ClipBounds.Top)
        Dim centerX As Integer = ((sx / 2) + CType(g.ClipBounds.Left, Integer))
        Dim centerY As Integer = ((sy / 2) + CType(g.ClipBounds.Top, Integer))

        Dim depthX As Integer = (sx / 10)
        Dim depthY As Integer = (sy / 10)
        Dim sz1 As Double
        sz1 = (15 / (15 + anAtom.Z))

        result.X = centerX + sz1 * depthX * anAtom.X
        result.Y = centerY + sz1 * depthY * anAtom.Y
        Return result
    End Function

#Region "Molecule Display"

    Private Sub pbDisplay_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pbDisplay.Paint
        'Dim g As Drawing.Graphics = pbDisplay.CreateGraphics ' e.Graphics
        Dim g As Drawing.Graphics = e.Graphics
        'On Error Resume Next
        If Not Loaded Then
            g.FillRectangle(Brushes.BlanchedAlmond, e.ClipRectangle)
            g.FillRectangle(Brushes.BurlyWood, Rectangle.Inflate(e.ClipRectangle, -10, -10))
            g.FillRectangle(Brushes.Chocolate, Rectangle.Inflate(e.ClipRectangle, -20, -20))
            Exit Sub
        End If

        'Dim sx As Integer = CInt(ACanvas.ClipBounds.Right - ACanvas.ClipBounds.Left)
        'Dim sy As Integer = CInt(ACanvas.ClipBounds.Bottom - ACanvas.ClipBounds.Top)
        'Dim centerX As Integer = ((sx / 2) + CType(ACanvas.ClipBounds.Left, Integer))
        'Dim centerY As Integer = ((sy / 2) + CType(ACanvas.ClipBounds.Top, Integer))
        'Dim depthX As Integer = (sx / 10)
        'Dim depthY As Integer = (sy / 10)

        Dim displayScale As Double
        Dim centerPoint As Drawing.PointF
        displayScale = Math.Sqrt(g.ClipBounds.Height ^ 2 + g.ClipBounds.Width ^ 2) / 500

        Dim atomsToDisplay As New Generic.List(Of Aminoacids.Atom)
        For Each oneAmino As Aminoacids.Molecule In Base.ProtCalc.ProteinAminoAcids.Values
            atomsToDisplay.AddRange(oneAmino.Atoms.Values)
        Next

        For z As Integer = 100 To -100 Step -1
            For Each oneAtom As Aminoacids.Atom In atomsToDisplay
                If ((oneAtom.Z >= (z / 10)) AndAlso (oneAtom.Z < ((z + 1) / 10))) Then
                    centerPoint = AtomPoint(g, oneAtom)

                    For Each aBond As Aminoacids.Bond In oneAtom.Bonds
                        Dim targetAtom As Aminoacids.Atom
                        If aBond.Order <> Aminoacids.BondOrder.Peptide Then
                            targetAtom = Base.ProtCalc.ProteinAminoAcids(oneAtom.MoleculeId).Atoms(aBond.LinkAtom)
                        Else
                            With Base.ProtCalc.ProteinAminoAcids(aBond.LinkMolecule)
                                If oneAtom.Element = "N" Then
                                    targetAtom = .Atoms(.PeptideC)
                                Else
                                    targetAtom = .Atoms(.PeptideN)
                                End If
                            End With
                        End If
                        If (oneAtom.MoleculeId = targetAtom.MoleculeId And _
                            oneAtom.Id > targetAtom.Id) Or _
                            oneAtom.MoleculeId > targetAtom.MoleculeId Then
                            DrawBond(g, centerPoint, targetAtom, aBond.Order, aBond.SpecialAngle)
                        End If
                    Next

                    If oneAtom.Element.Trim <> "*" Then
                        DrawAtom(g, displayScale, centerPoint, oneAtom)
                        DrawAtomText(g, oneAtom, centerPoint)
                    End If
                End If
            Next
        Next
    End Sub

    Private Sub DrawAtom(ByVal g As Drawing.Graphics, ByVal displayScale As Double, ByVal center As Drawing.PointF, ByVal anAtom As Aminoacids.Atom)
        Dim s As Double
        s = 15 * displayScale
        If anAtom.Element.Trim = "H" Then s = 7 * displayScale
        Dim sz1 As Double
        sz1 = (15 / (15 + anAtom.Z))

        Dim b As Brush
        Select Case anAtom.Element.Trim
            Case "C"
                b = Brushes.Gray
            Case "H"
                b = Brushes.Aqua
            Case "O"
                b = Brushes.Red
            Case "N"
                b = Brushes.Navy
            Case "S"
                b = Brushes.Yellow
            Case Else
                b = Brushes.White
        End Select
        Dim ellipseRadius As Single = CSng(s * sz1)

        g.FillEllipse(b, _
            center.X - ellipseRadius / 2, _
            center.Y - ellipseRadius / 2, _
            ellipseRadius, _
            ellipseRadius)
        g.DrawEllipse(Pens.Black, _
            center.X - ellipseRadius / 2, _
            center.Y - ellipseRadius / 2, _
            ellipseRadius, _
            ellipseRadius)
    End Sub

    Private Sub DrawBond(ByVal g As Drawing.Graphics, ByVal center As Drawing.PointF, ByVal bondAtom As Aminoacids.Atom, ByVal bondType As Aminoacids.BondOrder, ByVal SpecialBond As Aminoacids.BondAngle)
        Dim bondPoint As Drawing.PointF
        bondPoint = AtomPoint(g, bondAtom)
        Dim p As Drawing.Pen
        p = New Pen(Color.Black, 1)
        Select Case bondType
            Case Aminoacids.BondOrder.Double
                p.Width = 4
            Case Aminoacids.BondOrder.Triple
                p.Width = 4
                p.Color = Color.White
            Case Aminoacids.BondOrder.Peptide
                p.Width = 8
                p.Color = Color.AliceBlue
        End Select
        If SpecialBond = Aminoacids.BondAngle.Psi Or SpecialBond = Aminoacids.BondAngle.Phi Then
            p.DashStyle = Drawing2D.DashStyle.Dot
        End If
        g.DrawLine(p, center.X, center.Y, bondPoint.X, bondPoint.Y)
    End Sub

    Private Sub DrawAtomText(ByVal g As Graphics, ByVal anAtom As Aminoacids.Atom, ByVal center As PointF)
        Dim atomText As String

        Select Case anAtom.Charge
            Case 1
                atomText = "+"
            Case -1
                atomText = "-"
            Case Else
                atomText = ""
        End Select

        atomText &= Chr(96 + anAtom.MoleculeId.ToString) & anAtom.Id.ToString

        'If anAtom.SpecialRole <> Aminoacids.AtomSpecialRole.None Then atomText &= ":" & anAtom.SpecialRole.ToString("g")

        Dim f As New Font("Tahoma", 8.0!)
        Dim b As Brush = IIf((anAtom.Element.Trim = "N"), Brushes.Silver, Brushes.Black)
        'Dim b As Brush = Brushes.Indigo
        Dim drawingSize As SizeF = g.MeasureString(atomText, f)
        g.DrawString(atomText, f, b, _
            center.X - drawingSize.Width / 2, _
            center.Y - drawingSize.Height / 2, _
            StringFormat.GenericDefault)
    End Sub

#End Region

#Region "Molecule Movement"

    Private Moving As Boolean
    Private ddX, ddY As Integer

    Private Sub pbDisplay_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbDisplay.MouseDown
        Me.ddX = e.X
        Me.ddY = e.Y
        Me.Moving = True
    End Sub

    Private Sub pbDisplay_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbDisplay.MouseUp
        Me.Moving = False
    End Sub

    Private Sub pbDisplay_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbDisplay.MouseMove
        Dim s As Single
        If Loaded AndAlso Me.Moving Then
            Select Case e.Button
                Case Windows.Forms.MouseButtons.Left
                    For Each oneAmino As Aminoacids.Molecule In Base.ProtCalc.ProteinAminoAcids.Values
                        oneAmino.Rotate(0, 0, 0, e.Y - Me.ddY, Me.ddX - e.X, 0, Math.PI / 36)
                    Next
                    'Case Windows.Forms.MouseButtons.Right
                    '    s = Math.Sign(Me.ddX - e.X)
                    '    Base.ProtCalc.ProteinAminoAcids(1).RotateAngC(s * Math.PI / 36)
                    'Case Windows.Forms.MouseButtons.Middle
                    '    s = Math.Sign(Me.ddX - e.X)
                    '    Base.ProtCalc.ProteinAminoAcids(2).RotateAngN(s * Math.PI / 36)
                    'For Each oneAmino As Aminoacids.Molecule In aminos.Values
                    '    oneAmino.Move((Me.ddX - e.X) / 10, (Me.ddY - e.Y) / 10, 0)
                    'Next
            End Select

            'Aminoacids.Amino.Join(amino1, amino2)

            Me.pbDisplay.Invalidate()


            'Aminomol.Join(AminoForm.OneAmino, AminoForm.TwoAmino)
            'If Me.cbShow1.Checked Then
            '    AminoForm.OneAmino.Display(Me.TheDisplay.CreateGraphics)
            'End If
            'If Me.cbShow2.Checked Then
            '    AminoForm.TwoAmino.Display(Me.TheDisplay.CreateGraphics)
            'End If
            Me.ddX = e.X
            Me.ddY = e.Y
        End If

    End Sub

#End Region
End Class