Imports System.Text.RegularExpressions
Imports System.Xml

<Serializable(), Xml.Serialization.XmlRoot("molecule")> _
Public Class Molecule
    Implements Xml.Serialization.IXmlSerializable

    Private Shared UniqueId As Integer
    ' Methods
    Public Sub New()
        'Me.DehydrateNitrogenH = 0
        'Me.DehydrateCarbonH = 0
        'Me.DehydrateCarbonO = 0
        System.Threading.Interlocked.Increment(UniqueId)
        Me.Id = UniqueId
    End Sub

    Public Shared Sub ResetId()
        System.Threading.Interlocked.Add(UniqueId, -UniqueId)
    End Sub

    Public Sub DehydrateC()
        Dim aBond As Bond

        'For Each aBond In Me.Atoms(Me.DehydrateCarbonO).Bonds
        '    If Me.Atoms(aBond.LinkAtom).Name = "C" Then Me.PeptideC = aBond.LinkAtom
        'Next

        For Each aBond In Me.Atoms(Me.PeptideC).Bonds
            If Me.Atoms(aBond.LinkAtom).SpecialRole = AtomSpecialRole.SkeletonC Then
                aBond.SpecialAngle = BondAngle.Psi
                Me.BondPsi = aBond
            End If
            If aBond.LinkAtom = Me.DehydrateCarbonO Then
                aBond.LinkAtom = 0
                aBond.Order = BondOrder.Peptide
            End If
        Next
        Me.Atoms.Remove(Me.DehydrateCarbonH)
        Me.Atoms.Remove(Me.DehydrateCarbonO)

        ' Find rotating bond
    End Sub

    Public Sub DehydrateN()
        'Dim linkedH As Atom = Me.Atoms(Me.DehydrateNitrogenH) ' Hydrogen to be removed
        'Me.PeptideN = linkedH.Bonds(0).LinkAtom

        For Each aBond As Bond In Me.Atoms(PeptideN).Bonds
            If Me.Atoms(aBond.LinkAtom).SpecialRole = AtomSpecialRole.SkeletonC Then
                aBond.SpecialAngle = BondAngle.Phi
                Me.BondPhi = aBond
            End If
            If aBond.LinkAtom = Me.DehydrateNitrogenH Then
                aBond.Order = BondOrder.Peptide
                aBond.LinkAtom = 0
                aBond.LinkMolecule = 0
            End If
        Next
        Me.Atoms.Remove(Me.DehydrateNitrogenH)
        'Me.Atoms.Remove(Me.DehydN) '.Name = "*"
    End Sub

    'Public Sub Display(ByVal ACanvas As Graphics)

    Public Sub Move(ByVal dx As Double, ByVal dy As Double, ByVal dz As Double)
        For Each atom As KeyValuePair(Of Integer, Atom) In Me.Atoms
            atom.Value.X += dx
            atom.Value.Y += dy
            atom.Value.Z += dz
        Next
    End Sub

    ''' <summary>
    ''' Loads a aminoacid molecule from a file
    ''' </summary>
    ''' <param name="fname">Filename</param>
    ''' <remarks>File format is as follows:
    ''' 1. Number of atoms
    ''' After that, repeat «Number of atoms» times:
    ''' 2.1 Atom Name
    ''' 2.2 X, Y, Z coordinates
    ''' 2.3 atom number that has bond (6 times)
    ''' After the atoms, there are:
    ''' 3. Which atoms are the ones that should be dehydrated (Nitrogen and two Carbon atoms)
    ''' 4. Number of Double bonds
    ''' Repeat «Number of Double bonds» times
    ''' 5. Atom 1, Atom 2
    ''' 6. Number of charged atoms
    ''' Repeat «Number of charged atoms» times
    ''' 7. Charged atom. The sign of this value is the charge of the atom, the absolute value is the atom id.
    ''' </remarks>
    Public Sub LoadFlatFile(ByVal fname As String)
        'Dim gi As New Globalization.CultureInfo("en-US")
        Dim oneLine As String()
        Dim aminoFile As New IO.StreamReader(fname)
        Dim num As Integer = Integer.Parse(aminoFile.ReadLine)
        Me.Atoms = New Dictionary(Of Integer, Atom)
        Me.Name = IO.Path.GetFileNameWithoutExtension(fname)
        Dim i, k As Integer
        Dim c As SByte

        ' Atoms
        For i = 0 To num - 1
            Dim newAtom As New Atom()
            With newAtom
                .Element = aminoFile.ReadLine.Trim '.PadRight(2)
                .MoleculeId = Me.Id
                oneLine = Regex.Replace(aminoFile.ReadLine, "\s\s+", " ").Trim.Split()
                .X = Double.Parse(oneLine(0), Globalization.CultureInfo.InvariantCulture)
                .Y = Double.Parse(oneLine(1), Globalization.CultureInfo.InvariantCulture)
                .Z = Double.Parse(oneLine(2), Globalization.CultureInfo.InvariantCulture)
                .Id = i + 1
                oneLine = Regex.Replace(aminoFile.ReadLine, "\s\s+", " ").Trim.Split()
                'Me.newAtom.c = New Byte(6) {}
                'Me.newAtom.b = New Byte(6) {}
                'Bonds
                .Bonds = New List(Of Bond)
                Dim bondHash As New Dictionary(Of Integer, Integer)
                For k = 0 To 5
                    c = Byte.Parse(oneLine(k))
                    If c <> 0 Then
                        If bondHash.ContainsKey(c) Then
                            newAtom.Bonds(bondHash(c)).Order += 1
                        Else
                            bondHash.Add(c, k)
                            newAtom.Bonds.Add(New Bond(c, BondOrder.Single))
                        End If
                    End If

                    'Me.Atoms(i).c(k) = c
                    'Me.Atoms(i).b(k) = 1

                Next
                .Charge = 0
            End With
            Me.Atoms.Add(newAtom.Id, newAtom)
        Next
        oneLine = Regex.Replace(aminoFile.ReadLine, "\s\s+", " ").Trim.Split()
        Me.DehydrateNitrogenH = Byte.Parse(oneLine(0))
        Me.DehydrateCarbonH = Byte.Parse(oneLine(1))
        Me.DehydrateCarbonO = Byte.Parse(oneLine(2))

        'Bond type
        Dim n As Byte = Byte.Parse(aminoFile.ReadLine)
        For i = 0 To n - 1
            oneLine = Regex.Replace(aminoFile.ReadLine, "\s\s+", " ").Trim.Split()
            Dim a1 As Byte = Byte.Parse(oneLine(0))
            Dim a2 As Byte = Byte.Parse(oneLine(1))
            'For j = 0 To 5
            '    If (Me.Atoms(a1).c(j) = a2) Then Me.Atoms(a1).b(j) = 2
            '    If (Me.Atoms(a2).c(j) = a1) Then Me.Atoms(a2).b(j) = 2
            'Next
            Dim newBond As Bond
            For j As Integer = 0 To Me.Atoms(a1).Bonds.Count - 1
                If Me.Atoms(a1).Bonds(j).LinkAtom = a2 Then
                    Me.Atoms(a1).Bonds(j).Order += 1
                End If
            Next

            For j As Integer = 0 To Me.Atoms(a2).Bonds.Count - 1
                If Me.Atoms(a2).Bonds(j).LinkAtom = a2 Then
                    newBond = Me.Atoms(a2).Bonds(j)
                    newBond.Order += 1
                    Me.Atoms(a2).Bonds(j) = newBond
                End If
            Next
        Next

        'Charges
        n = Byte.Parse(aminoFile.ReadLine)
        For i = 0 To n - 1
            c = SByte.Parse(aminoFile.ReadLine)
            Me.Atoms(Math.Abs(c)).Charge = Math.Sign(c)
        Next

        If (Me.Atoms(Me.DehydrateCarbonH).Element = "O") Then
            i = Me.DehydrateCarbonH
            Me.DehydrateCarbonH = Me.DehydrateCarbonO
            Me.DehydrateCarbonO = i
        End If
        aminoFile.Close()
    End Sub

    Public Shared Function Load(ByVal name As String) As Molecule
        Dim oneAmino As Molecule
        Dim ser As New Xml.Serialization.XmlSerializer(GetType(Molecule), "http://www.xml-cml.org/schema/cml2/core")
        'filename = IO.Path.Combine(RootFolder, aminoName & ".cml")
        Dim aMoleculeStream As IO.StringReader
        aMoleculeStream = New IO.StringReader(My.Resources.ResourceManager.GetString(name.ToLower))
        oneAmino = ser.Deserialize(aMoleculeStream)
        aMoleculeStream.Close()
        Return oneAmino
    End Function

    Public Sub Rotate(ByVal x1 As Double, ByVal y1 As Double, ByVal z1 As Double, ByVal x2 As Double, ByVal y2 As Double, ByVal z2 As Double, ByVal th As Double, ByVal ParamArray ExcludeAtoms() As Integer)
        If (th < 0) Then th += 2 * Math.PI

        If (th <> 0) Then
            Dim s1 As Double = Math.Cos(th / 2)
            Dim s2 As Double = Math.Sin(th / 2)
            Dim tx As Double = x2 - x1
            Dim ty As Double = y2 - y1
            Dim tz As Double = z2 - z1
            Dim tl As Double = Math.Sqrt(tx * tx + ty * ty + tz * tz)
            If (tl <> 0) Then
                Dim a As Double = s2 * (tx / tl)
                Dim b As Double = s2 * (ty / tl)
                Dim c As Double = s2 * (tz / tl)
                Me.Move(-x1, -y1, -z1)

                For Each atom As KeyValuePair(Of Integer, Atom) In Me.Atoms
                    If Array.BinarySearch(ExcludeAtoms, atom.Value.Id) < 0 Then
                        Dim x As Double = atom.Value.X
                        Dim y As Double = atom.Value.Y
                        Dim z As Double = atom.Value.Z
                        atom.Value.X = (1 - 2 * b * b - 2 * c * c) * x + (2 * a * b - 2 * s1 * c) * y + (2 * a * c + 2 * s1 * b) * z
                        atom.Value.Y = (2 * a * b + 2 * s1 * c) * x + (1 - 2 * a * a - 2 * c * c) * y + (2 * b * c - 2 * s1 * a) * z
                        atom.Value.Z = (2 * a * c - 2 * s1 * b) * x + (2 * b * c + 2 * s1 * a) * y + (1 - 2 * a * a - 2 * b * b) * z
                    End If
                Next
                Me.Move(x1, y1, z1)
            End If
        End If
    End Sub

    Public Sub RotateAngC(ByVal th As Double)
        Dim atom1, atom2 As Atom
        'Find C of peptide
        atom1 = Me.Atoms(Me.PeptideC)
        'Find connected special bond
        atom2 = Me.Atoms(Me.SkeletonC)

        Me.Rotate( _
            atom1.X, atom1.Y, atom1.Z, _
            atom2.X, atom2.Y, atom2.Z, _
            th, Me.PeptideC, Me.PeptideO)
    End Sub

    Public Sub RotateAngN(ByVal th As Double)
        Dim atom1, atom2 As Atom
        'Find N of peptide
        atom1 = Me.Atoms(Me.PeptideN)
        'Find connected special bond
        atom2 = Me.Atoms(Me.SkeletonC)

        Me.Rotate( _
            atom1.X, atom1.Y, atom1.Z, _
            atom2.X, atom2.Y, atom2.Z, _
            th, Me.PeptideN, Me.PeptideH)
    End Sub

    ' Fields
    Public Property Id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            If Me.Atoms IsNot Nothing Then
                For Each oneAtom As Atom In Me.Atoms.Values
                    If oneAtom.MoleculeId = _id Then oneAtom.MoleculeId = value
                    For Each aBond As Bond In oneAtom.Bonds
                        If aBond.LinkMolecule = _id Then aBond.LinkMolecule = value
                    Next
                Next
            End If
            _id = value
        End Set
    End Property
    Public Atoms As Dictionary(Of Integer, Atom)
    Private _id As Integer
    Public Name As String

    Public DehydrateCarbonH As Integer
    Public DehydrateCarbonO As Integer
    Public DehydrateNitrogenH As Integer

    Public PeptideC As Integer
    Public PeptideN As Integer
    Public PeptideO As Integer
    Public PeptideH As Integer

    Public SkeletonC As Integer
    Public SkeletonH As Integer

    Public BondPhi As Bond
    Public BondPsi As Bond

    'Public DisplayOptions As Integer
    'Public num As Byte

    Private Shared Function Theta(ByVal Atom1 As Atom, ByVal Atom2 As Atom, ByVal Atom3 As Atom, ByVal Atom4 As Atom) As Double
        Dim A1, B1, C1, D1 As Double
        Dim A2, B2, C2, D2 As Double
        Dim th, co As Double
        Dim x1, y1, z1, x2, y2, z2, x3, y3, z3, x4, y4, z4 As Double

        x1 = Atom1.X
        y1 = Atom1.Y
        z1 = Atom1.Z

        x2 = Atom2.X
        y2 = Atom2.Y
        z2 = Atom2.Z

        x3 = Atom3.X
        y3 = Atom3.Y
        z3 = Atom3.Z

        x4 = Atom4.X
        y4 = Atom4.Y
        z4 = Atom4.Z

        'A1 * x1 + B1 * y1 + C1 * z1 + D1 = 0
        'A1 * x2 + B1 * y2 + C1 * z2 + D1 = 0
        'A1 * x3 + B1 * y3 + C1 * z3 + D1 = 0

        A1 = y3 * (z1 - z2) + y1 * (z2 - z3) + y2 * (z3 - z1)
        B1 = z3 * (x1 - x2) + z1 * (x2 - x3) + z2 * (x3 - x1)
        C1 = x3 * (y1 - y2) + x1 * (y2 - y3) + x2 * (y3 - y1)
        D1 = -x3 * (y1 * z2 - y2 * z1) - x1 * (y2 * z3 - y3 * z2) - x2 * (y3 * z1 - y1 * z3)

        A2 = y4 * (z2 - z1) + y2 * (z1 - z4) + y1 * (z4 - z2)
        B2 = z4 * (x2 - x1) + z2 * (x1 - x4) + z1 * (x4 - x2)
        C2 = x4 * (y2 - y1) + x2 * (y1 - y4) + x1 * (y4 - y2)
        D2 = -x4 * (y2 * z1 - y1 * z2) - x2 * (y1 * z4 - y4 * z1) - x1 * (y4 * z2 - y2 * z4)

        'A1 = (((y3 * (z1 - z2)) + (y1 * (z2 - z3))) + (y2 * (z3 - z1)))
        'B1 = (((z3 * (x1 - x2)) + (z1 * (x2 - x3))) + (z2 * (x3 - x1)))
        'C1 = (((x3 * (y1 - y2)) + (x1 * (y2 - y3))) + (x2 * (y3 - y1)))
        'D1 = (((-x3 * ((y1 * z2) - (y2 * z1))) - (x1 * ((y2 * z3) - (y3 * z2)))) - (x2 * ((y3 * z1) - (y1 * z3))))
        'A2 = (((y4 * (z2 - z1)) + (y2 * (z1 - z4))) + (y1 * (z4 - z2)))
        'B2 = (((z4 * (x2 - x1)) + (z2 * (x1 - x4))) + (z1 * (x4 - x2)))
        'C2 = (((x4 * (y2 - y1)) + (x2 * (y1 - y4))) + (x1 * (y4 - y2)))
        'D2 = (((-x4 * ((y2 * z1) - (y1 * z2))) - (x2 * ((y1 * z4) - (y4 * z1)))) - (x1 * ((y4 * z2) - (y2 * z4))))

        'co = ((((A1 * A2) + (B1 * B2)) + (C1 * C2)) / (Math.Sqrt((((A1 * A1) + (B1 * B1)) + (C1 * C1))) * Math.Sqrt((((A2 * A2) + (B2 * B2)) + (C2 * C2)))))
        co = (A1 * A2 + B1 * B2 + C1 * C2) / (Math.Sqrt(A1 * A1 + B1 * B1 + C1 * C1) * Math.Sqrt(A2 * A2 + B2 * B2 + C2 * C2))

        th = Math.Acos(Math.Abs(co))
        'co = Math.Abs(co)

        'If (co > 1) Then co = 1
        'If (co < -1) Then co = -1

        'si = Math.Sqrt(1 - co ^ 2)
        'If (co = 0) Then
        '    th = Math.PI / 2
        'Else
        '    th = Math.Atan(si / co)
        'End If
        'If (((((A1 * x4) + (B1 * y4)) + (C1 * z4)) + D1) > 0) Then th = -th
        'If A1 * x4 + B1 * y4 + C1 * z4 + D1 > 0 Then th = -th 'Math.PI + th
        If A1 * x4 + B1 * y4 + C1 * z4 + D1 > 0 Then th = Math.PI + th
        Return th
    End Function

    Public Shared Sub Join(ByRef OneAmino As Molecule, ByRef TwoAmino As Molecule)
        Dim th As Double
        Dim A, B, C, D As Double
        'Dim A1, B1, C1, D1 As Double
        'Dim A2, B2, C2, D2 As Double
        Dim x1, y1, z1, x2, y2, z2 As Double
        Dim x3, y3, z3 As Double
        Dim l1, l2, co As Double
        Dim ser As New Xml.Serialization.XmlSerializer(GetType(Molecule))

        'Move to lock 1st atom
        TwoAmino.Move( _
            (OneAmino.Atoms(OneAmino.PeptideC).X - TwoAmino.Atoms(TwoAmino.DehydrateNitrogenH).X), _
            (OneAmino.Atoms(OneAmino.PeptideC).Y - TwoAmino.Atoms(TwoAmino.DehydrateNitrogenH).Y), _
            (OneAmino.Atoms(OneAmino.PeptideC).Z - TwoAmino.Atoms(TwoAmino.DehydrateNitrogenH).Z))

        'Rotate to lock 2nd atom
        x1 = OneAmino.Atoms(OneAmino.DehydrateCarbonO).X
        y1 = OneAmino.Atoms(OneAmino.DehydrateCarbonO).Y
        z1 = OneAmino.Atoms(OneAmino.DehydrateCarbonO).Z

        x2 = OneAmino.Atoms(OneAmino.PeptideC).X
        y2 = OneAmino.Atoms(OneAmino.PeptideC).Y
        z2 = OneAmino.Atoms(OneAmino.PeptideC).Z

        x3 = TwoAmino.Atoms(TwoAmino.PeptideN).X
        y3 = TwoAmino.Atoms(TwoAmino.PeptideN).Y
        z3 = TwoAmino.Atoms(TwoAmino.PeptideN).Z
        'A = (((y1 * (z2 - z3)) + (y2 * (z3 - z1))) + (y3 * (z1 - z2))))
        'B = (((z1 * (x2 - x3)) + (z2 * (x3 - x1))) + (z3 * (x1 - x2))))
        'C = (((x1 * (y2 - y3)) + (x2 * (y3 - y1))) + (x3 * (y1 - y2))))
        'D = (((-x1 * ((y2 * z3) - (y3 * z2))) - (x2 * ((y3 * z1) - (y1 * z3)))) - (x3 * ((y1 * z2) - (y2 * z1))))

        A = y1 * (z2 - z3) + y2 * (z3 - z1) + y3 * (z1 - z2)
        B = z1 * (x2 - x3) + z2 * (x3 - x1) + z3 * (x1 - x2)
        C = x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2)
        D = -x1 * (y2 * z3 - y3 * z2) - x2 * (y3 * z1 - y1 * z3) - x3 * (y1 * z2 - y2 * z1)

        l1 = Math.Sqrt((x1 - x2) ^ 2 + (y1 - y2) ^ 2 + (z1 - z2) ^ 2)
        l2 = Math.Sqrt((x3 - x2) ^ 2 + (y3 - y2) ^ 2 + (z3 - z2) ^ 2)

        'l1 = Math.Sqrt((x1 - x4) ^ 2 + (y1 - y4) ^ 2 + (z1 - z4) ^ 2) ' ((Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2)) + Math.Pow((z1 - z2), 2)))
        'l2 = Math.Sqrt((x2 - x4) ^ 2 + (y2 - y4) ^ 2 + (z2 - z4) ^ 2) ' ((Math.Pow((x3 - x2), 2) + Math.Pow((y3 - y2), 2)) + Math.Pow((z3 - z2), 2)))

        co = ((x1 - x2) * (x3 - x2) + (y1 - y2) * (y3 - y2) + (z1 - z2) * (z3 - z2)) / (l1 * l2)
        'si = Math.Sqrt(Math.Abs(1 - co ^ 2))

        'If (co = 0) Then
        '    th = Math.PI / 2
        'Else
        '    th = Math.Atan(si / co)
        'End If
        th = Math.Acos(co)
        If (((((x1 - x2) * x3) + ((y1 - y2) * y3)) + ((z1 - z2) * z3)) < ((((x1 - x2) * x2) + ((y1 - y2) * y2)) + ((z1 - z2) * z2))) Then
            th = (Math.PI + th)
        End If

        TwoAmino.Rotate(x2, y2, z2, x2 + A, y2 + B, z2 + C, th)

        '{Find O and H of CO=NH Bond}
        'bond3 = 255
        'bond4 = 255
        'For Each aBond As Bond In OneAmino.Atoms(OneAmino.PeptideC).Bonds
        '    If OneAmino.Atoms(aBond.LinkAtom).Name = "O" AndAlso aBond.LinkAtom <> OneAmino.DehydrateCarbonO Then
        '        bond3 = aBond.LinkAtom
        '    End If
        'Next

        'For Each aBond As Bond In TwoAmino.Atoms(TwoAmino.PeptideN).Bonds
        '    If TwoAmino.Atoms(aBond.LinkAtom).Name = "H" AndAlso aBond.LinkAtom <> TwoAmino.DehydrateNitrogenH Then
        '        bond4 = aBond.LinkAtom
        '    End If
        'Next

        ''Proline - special case, no H bonded to N
        'If (bond4 = 255) Then bond4 = 11 'Really Dirty Hack, but I suppose it will work

        'x1 = OneAmino.Atoms(bond1).X
        'y1 = OneAmino.Atoms(bond1).Y
        'z1 = OneAmino.Atoms(bond1).Z

        'x2 = TwoAmino.Atoms(bond2).X
        'y2 = TwoAmino.Atoms(bond2).Y
        'z2 = TwoAmino.Atoms(bond2).Z

        'x3 = OneAmino.Atoms(bond3).X
        'y3 = OneAmino.Atoms(bond3).Y
        'z3 = OneAmino.Atoms(bond3).Z

        'x4 = TwoAmino.Atoms(bond4).X
        'y4 = TwoAmino.Atoms(bond4).Y
        'z4 = TwoAmino.Atoms(bond4).Z

        'find angle of plane O,C,N and plane H,N,C
        th = Theta(OneAmino.Atoms(OneAmino.PeptideC), TwoAmino.Atoms(TwoAmino.PeptideN), OneAmino.Atoms(OneAmino.PeptideO), TwoAmino.Atoms(TwoAmino.PeptideH))

        'Dim out As IO.StreamWriter
        'out = New IO.StreamWriter("C:\a1.cml") : ser.Serialize(out, OneAmino) : out.Close()
        'out = New IO.StreamWriter("C:\a2.cml") : ser.Serialize(out, TwoAmino) : out.Close()

        TwoAmino.Rotate( _
            OneAmino.Atoms(OneAmino.PeptideC).X, OneAmino.Atoms(OneAmino.PeptideC).Y, OneAmino.Atoms(OneAmino.PeptideC).Z, _
            TwoAmino.Atoms(TwoAmino.PeptideN).X, TwoAmino.Atoms(TwoAmino.PeptideN).Y, TwoAmino.Atoms(TwoAmino.PeptideN).Z, _
            -th)

        'out = New IO.StreamWriter("C:\b1.cml") : ser.Serialize(out, OneAmino) : out.Close()
        'out = New IO.StreamWriter("C:\b2.cml") : ser.Serialize(out, TwoAmino) : out.Close()

        OneAmino.DehydrateC()
        TwoAmino.DehydrateN()

        'Reset Phi and Psi angles
        'Find skeleton Carbon and Nitrogen

        'For Each aBond As Bond In OneAmino.Atoms(OneAmino.BondPsi.LinkAtom).Bonds
        '    If OneAmino.Atoms(aBond.LinkAtom).Name = "H" Then bond5 = aBond.LinkAtom : Exit For
        'Next

        'For Each aBond As Bond In TwoAmino.Atoms(TwoAmino.BondPhi.LinkAtom).Bonds
        '    If TwoAmino.Atoms(aBond.LinkAtom).Name = "H" Then bond6 = aBond.LinkAtom : Exit For
        'Next

        'out = New IO.StreamWriter("C:\c1.cml") : ser.Serialize(out, OneAmino) : out.Close()
        'out = New IO.StreamWriter("C:\c2.cml") : ser.Serialize(out, TwoAmino) : out.Close()

        th = Theta( _
            OneAmino.Atoms(OneAmino.PeptideC), OneAmino.Atoms(OneAmino.SkeletonC), _
            OneAmino.Atoms(OneAmino.PeptideO), OneAmino.Atoms(OneAmino.SkeletonH))
        'th = 0
        OneAmino.RotateAngC(-th + Math.PI)
        '.Rotate( _
        '    OneAmino.Atoms(OneAmino.PeptideC).X, OneAmino.Atoms(OneAmino.PeptideC).Y, OneAmino.Atoms(OneAmino.PeptideC).Z, _
        '    OneAmino.Atoms(OneAmino.SkeletonC).X, OneAmino.Atoms(OneAmino.SkeletonC).Y, OneAmino.Atoms(OneAmino.SkeletonC).Z, _
        '    -th, OneAmino.PeptideC, OneAmino.PeptideO)

        th = Theta( _
            TwoAmino.Atoms(TwoAmino.PeptideN), TwoAmino.Atoms(TwoAmino.SkeletonC), _
            TwoAmino.Atoms(TwoAmino.PeptideH), TwoAmino.Atoms(TwoAmino.SkeletonH))
        'th = 0
        TwoAmino.RotateAngN(th)


        'out = New IO.StreamWriter("C:\d1.cml") : ser.Serialize(out, OneAmino) : out.Close()
        'out = New IO.StreamWriter("C:\d2.cml") : ser.Serialize(out, TwoAmino) : out.Close()

        'TwoAmino.Rotate( _
        '    TwoAmino.Atoms(TwoAmino.PeptideN).X, TwoAmino.Atoms(TwoAmino.PeptideN).Y, TwoAmino.Atoms(TwoAmino.PeptideN).Z, _
        '    TwoAmino.Atoms(TwoAmino.SkeletonC).X, TwoAmino.Atoms(TwoAmino.SkeletonC).Y, TwoAmino.Atoms(TwoAmino.SkeletonC).Z, _
        '    -th, OneAmino.PeptideN, OneAmino.PeptideH)

        For Each aBond As Bond In OneAmino.Atoms(OneAmino.PeptideC).Bonds
            If aBond.Order = BondOrder.Peptide Then
                aBond.LinkAtom = TwoAmino.PeptideN
                aBond.LinkMolecule = TwoAmino.Id
            End If
        Next
        For Each aBond As Bond In TwoAmino.Atoms(TwoAmino.PeptideN).Bonds
            If aBond.Order = BondOrder.Peptide Then
                aBond.LinkAtom = OneAmino.PeptideC
                aBond.LinkMolecule = OneAmino.Id
            End If
        Next
    End Sub

    Public Function GetSchema() As System.Xml.Schema.XmlSchema Implements System.Xml.Serialization.IXmlSerializable.GetSchema
        Dim result As New System.Xml.Schema.XmlSchema
        Return result
    End Function

    Public Sub ReadXml(ByVal reader As System.Xml.XmlReader) Implements System.Xml.Serialization.IXmlSerializable.ReadXml
        '(GetType(Integer), Nothing)
        Me.Atoms = New Dictionary(Of Integer, Atom)
        reader.MoveToContent()
        Dim newAtom As Aminoacids.Atom
        Dim serAtom As New Serialization.XmlSerializer(GetType(Atom), "http://www.xml-cml.org/schema/cml2/core")
        'Dim gi As New Globalization.CultureInfo.invariant
        While reader.Read()
            If reader.NodeType = XmlNodeType.Element Then
                Select Case reader.Name
                    Case "molecule"
                        Me.Id = reader.GetAttribute("id")
                        'Case "dehydrate"
                        '    Me.DehydrateNitrogenH = reader.GetAttribute("N")
                        '    Me.DehydrateCarbonH = reader.GetAttribute("C1")
                        '    Me.DehydrateCarbonO = reader.GetAttribute("C2")
                    Case "atom"
                        newAtom = serAtom.Deserialize(reader.ReadSubtree)
                        Select Case newAtom.SpecialRole
                            Case AtomSpecialRole.PeptideC
                                Me.PeptideC = newAtom.Id
                            Case AtomSpecialRole.PeptideH
                                Me.PeptideH = newAtom.Id
                            Case AtomSpecialRole.PeptideN
                                Me.PeptideN = newAtom.Id
                            Case AtomSpecialRole.PeptideO
                                Me.PeptideO = newAtom.Id
                            Case AtomSpecialRole.SkeletonC
                                Me.SkeletonC = newAtom.Id
                            Case AtomSpecialRole.SkeletonH
                                Me.SkeletonH = newAtom.Id
                            Case AtomSpecialRole.DehydrateCarbonH
                                Me.DehydrateCarbonH = newAtom.Id
                            Case AtomSpecialRole.DehydrateCarbonO
                                Me.DehydrateCarbonO = newAtom.Id
                            Case AtomSpecialRole.DehydrateNitrogenH
                                Me.DehydrateNitrogenH = newAtom.Id
                        End Select
                        newAtom.MoleculeId = Me.Id
                        Me.Atoms.Add(newAtom.Id, newAtom)
                    Case "bond"
                        Dim atomRef() As String = reader.GetAttribute("atomRefs2").Split
                        Dim newBond As Bond
                        newBond = New Bond
                        newBond.LinkAtom = atomRef(1).Substring(1)
                        newBond.LinkMolecule = Me.Id
                        newBond.Order = CInt(reader.GetAttribute("order"))
                        Me.Atoms(atomRef(0).Substring(1)).Bonds.Add(newBond)

                        newBond = New Bond
                        newBond.LinkAtom = atomRef(0).Substring(1)
                        newBond.LinkMolecule = Me.Id
                        newBond.Order = CInt(reader.GetAttribute("order"))
                        Me.Atoms(atomRef(1).Substring(1)).Bonds.Add(newBond)

                End Select
            End If

            'Select Case reader.NodeType
            '    Case XmlNodeType.Element
            '        Console.Write("<{0}>", reader.Name)
            '    Case XmlNodeType.Text
            '        Console.Write(reader.Value)
            '    Case XmlNodeType.CDATA
            '        Console.Write("<![CDATA[{0}]]>", reader.Value)
            '    Case XmlNodeType.ProcessingInstruction
            '        Console.Write("<?{0} {1}?>", reader.Name, reader.Value)
            '    Case XmlNodeType.Comment
            '        Console.Write("<!--{0}-->", reader.Value)
            '    Case XmlNodeType.XmlDeclaration
            '        Console.Write("<?xml version='1.0'?>")
            '    Case XmlNodeType.Document
            '    Case XmlNodeType.DocumentType
            '        Console.Write("<!DOCTYPE {0} [{1}]", reader.Name, reader.Value)
            '    Case XmlNodeType.EntityReference
            '        Console.Write(reader.Name)
            '    Case XmlNodeType.EndElement
            '        Console.Write("</{0}>", reader.Name)
            'End Select
        End While
        'While reader.Read
        '    console.WriteLine(
        'End While
    End Sub

    Public Sub WriteXml(ByVal writer As System.Xml.XmlWriter) Implements System.Xml.Serialization.IXmlSerializable.WriteXml
        writer.WriteAttributeString("id", Me.Id)
        writer.WriteAttributeString("title", Me.Name)

        Dim xsn As New Xml.Serialization.XmlSerializerNamespaces()
        xsn.Add(String.Empty, String.Empty)
        Dim atomSer As New Xml.Serialization.XmlSerializer(GetType(Atom), "http://www.xml-cml.org/schema/cml2/core")
        writer.WriteStartElement("atomArray")
        For Each oneAtom As KeyValuePair(Of Integer, Atom) In Me.Atoms
            atomSer.Serialize(writer, oneAtom.Value, xsn)
        Next
        writer.WriteEndElement()

        writer.WriteStartElement("bondArray")
        For Each oneAtom As KeyValuePair(Of Integer, Atom) In Me.Atoms
            For Each oneBond As Bond In oneAtom.Value.Bonds
                If oneAtom.Value.Id < oneBond.LinkAtom Then
                    writer.WriteStartElement("bond")
                    writer.WriteAttributeString("atomRefs2", oneAtom.Value.Name & " " & Atom.AtomName(oneBond.LinkMolecule, oneBond.LinkAtom))
                    If oneBond.Order = BondOrder.Peptide Then
                        writer.WriteAttributeString("order", 1)
                        writer.WriteAttributeString("role", "peptide")
                    Else
                        writer.WriteAttributeString("order", oneBond.Order.ToString("d"))
                    End If
                    writer.WriteEndElement()
                End If
            Next
        Next
        writer.WriteEndElement()


        'TerraSoft.BioTech.ProFoGA.Aminoacids.Molecule.Atoms
    End Sub

    Private Shared Function AtomName(ByVal moleculeId As Integer, ByVal atomId As Integer) As String

    End Function
End Class