Public Class Calculator

    Public Function Calculate() As Double
        Dim allAtoms As New List(Of Aminoacids.Atom)
        For Each oneAmino As Aminoacids.Molecule In ProteinAminoAcids.Values
            allAtoms.AddRange(oneAmino.Atoms.Values)
        Next
        Dim result As Double = 0
        Dim distanceSquared As Double
        Dim atoms() As Aminoacids.Atom
        atoms = allAtoms.ToArray
        For i As Integer = LBound(atoms) To UBound(atoms)
            For j As Integer = i + 1 To UBound(atoms)
                distanceSquared = Aminoacids.Atom.DistanceSquared(atoms(i), atoms(j))
                If atoms(i).Charge <> 0 And atoms(j).Charge <> 0 Then
                    result += Forces.Coulomb(atoms(i).Charge, atoms(j).Charge, distanceSquared)
                End If
                If (atoms(i).Element = "O" Or atoms(i).Element = "N" Or atoms(i).Element = "S") And _
                    atoms(j).Element = "H" Then
                    result += Forces.Hydrogen(atoms(i).Element, atoms(j).Element, distanceSquared)
                End If
                If atoms(i).Element = "S" And atoms(j).Element = "S" Then
                    result += Forces.Sulfide(distanceSquared)
                End If
                result += Forces.LenardJones(atoms(i).Element, atoms(j).Element, distanceSquared)
            Next
        Next
        Return result
    End Function

    Public ProteinAminoAcids As Dictionary(Of Integer, Aminoacids.Molecule)
    Public ProteinAminoacidNames() As String

    Public Sub Conformation(ByVal names() As String, ByVal angles() As Double)
        ProteinAminoacidNames = names
        Conformation(angles)
    End Sub

    Public Sub Conformation(ByVal angles() As Double)
        Dim oneAmino As Aminoacids.Molecule
        Dim i As Integer = 0

        Aminoacids.Molecule.ResetId()
        ProteinAminoAcids = New Dictionary(Of Integer, Aminoacids.Molecule)
        For Each aminoName As String In ProteinAminoacidNames
            oneAmino = Aminoacids.Molecule.Load(aminoName)

            oneAmino.RotateAngC(angles(i * 2))
            oneAmino.RotateAngN(angles(i * 2 + 1))

            If i = 0 Then oneAmino.Move(-oneAmino.Atoms(1).X, -oneAmino.Atoms(1).Y, -oneAmino.Atoms(1).Z)
            ProteinAminoAcids.Add(oneAmino.Id, oneAmino)
            If i >= 1 Then Aminoacids.Molecule.Join(ProteinAminoAcids(oneAmino.Id - 1), oneAmino)
            i += 1
        Next
        'SaveProtein("C:\out.cml")
    End Sub

    Public Sub SaveProtein(ByVal finalfilename As String)
        Dim oneAmino As Aminoacids.Molecule
        Dim ser As New Xml.Serialization.XmlSerializer(GetType(Aminoacids.Molecule), "http://www.xml-cml.org/schema/cml2/core")
        Dim xmlOne, xmlTotal As Xml.XmlDocument
        xmlTotal = New Xml.XmlDocument
        xmlTotal.LoadXml("<molecule xmlns=""http://www.xml-cml.org/schema/cml2/core""><atomArray/><bondArray/></molecule>")
        Dim sb As Text.StringBuilder
        Dim out As IO.StringWriter
        Dim newNode As Xml.XmlDocumentFragment

        Dim nsmgr As New Xml.XmlNamespaceManager(xmlTotal.NameTable)
        nsmgr.AddNamespace("cml", "http://www.xml-cml.org/schema/cml2/core")

        For Each oneAmino In ProteinAminoAcids.Values
            sb = New Text.StringBuilder
            out = New IO.StringWriter(sb)
            xmlOne = New Xml.XmlDocument
            ser.Serialize(out, oneAmino)
            out.Close()
            xmlOne.LoadXml(sb.ToString)
            For Each atomNode As Xml.XmlNode In xmlOne.SelectNodes("/cml:molecule/cml:atomArray/cml:atom", nsmgr)
                newNode = xmlTotal.CreateDocumentFragment
                newNode.InnerXml = atomNode.OuterXml
                xmlTotal.SelectSingleNode("/cml:molecule/cml:atomArray", nsmgr).AppendChild(newNode)
            Next
            For Each bondNode As Xml.XmlNode In xmlOne.SelectNodes("/cml:molecule/cml:bondArray/cml:bond", nsmgr)
                newNode = xmlTotal.CreateDocumentFragment
                newNode.InnerXml = bondNode.OuterXml
                xmlTotal.SelectSingleNode("/cml:molecule/cml:bondArray", nsmgr).AppendChild(newNode)
            Next
        Next
        xmlTotal.Save(finalfilename)
    End Sub

    Public Function RealFitness(ByVal genes() As Byte) As Double
        Dim angles As Double() = AnglesFromGene(genes)
        Me.Conformation(ProteinAminoacidNames, angles)
        Return -Me.Calculate()
    End Function

    Public Function AnglesFromGene(ByVal genes As Byte()) As Double()
        Dim angles(genes.Length / 4 - 1) As Double
        For i As Integer = 0 To angles.Length - 1
            angles(i) = 22.5 * (genes(i * 4) + genes(i * 4 + 1) * 2 + genes(i * 4 + 2) * 4 + genes(i * 4 + 3) * 8)
        Next
        Return angles
    End Function

End Class
