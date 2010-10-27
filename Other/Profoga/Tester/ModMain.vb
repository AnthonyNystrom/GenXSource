Module ModuleMain

    Sub Main()
        Console.WindowHeight = 55
        Console.WindowWidth = 120
        Console.BufferHeight = 1000
        'Try
        Main07()
        'Catch ex As Exception
        'Console.ForegroundColor = ConsoleColor.Red
        'Console.WriteLine(ex.ToString)
        'Console.ResetColor()
        'End Try
        Console.WriteLine("Press [Enter] to exit")
        Console.ReadLine()
    End Sub

    Private Sub Main01()
        Dim ky As System.ConsoleKeyInfo = Console.ReadKey(True)
        Console.WriteLine(ky.Modifiers.ToString)
        Console.WriteLine(ky.Key.ToString)
    End Sub

    Private Sub Main02()
        Dim br As System.Drawing.SolidBrush
        br = New System.Drawing.SolidBrush(System.Drawing.Color.Blue)
    End Sub

    Private Sub Main03()
        Dim pop As New ProFoGA.Critters.Population(40, 40, AddressOf DummyFitness)
        Dim running As Boolean = True
        Dim ky As System.ConsoleKeyInfo
        While running
            DisplayPopulation(pop, 0, 0)
            Console.SetCursorPosition(0, Console.WindowHeight - 2)
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine("[F3][F4] to grow/kill by 0.2, [F5][F6] to grow/kill to 40/20")
            Console.Write("[F9] to randomize, [F10] to exit")
            Console.ResetColor()
            ky = Console.ReadKey
            Console.Clear()
            Select Case ky.Key
                Case ConsoleKey.F3
                    pop.Grow(0.2)
                Case ConsoleKey.F4
                    pop.Kill(0.2)
                Case ConsoleKey.F5
                    pop.GrowTo(40)
                Case ConsoleKey.F6
                    pop.KillTo(20)
                Case ConsoleKey.F9
                    pop.Randomize()
                Case ConsoleKey.F10
                    Console.WriteLine()
                    running = False
            End Select
        End While

    End Sub

    Private Sub Main04()
        Dim pop As New ProFoGA.Critters.Population(80, 100, AddressOf DummyFitness)
        pop.Randomize()
        For generation As Integer = 1 To 5000
            pop.GrowTo(100)
            pop.KillTo(50)
            DisplayPopulation(pop, 0, 0)
            Console.SetCursorPosition(0, Console.WindowHeight - 1)
            Console.ForegroundColor = ConsoleColor.White
            Console.Write("Generation: {0:0000} Average Fitness:{1:0.00000}", generation, pop.AverageFitness)
            Console.ResetColor()


        Next
    End Sub

    Private Sub DisplayPopulation(ByVal pop As Critters.Population, ByVal left As Integer, ByVal top As Integer)
        Dim lin As Integer = 0
        For Each c As Critters.Critter In pop.Critters
            For x As Integer = 0 To c.gene.Length - 1
                Console.SetCursorPosition(left + x, top + lin)
                Console.Write(c.gene(x).ToString)
            Next
            Console.Write(vbTab)
            Console.Write(c.Fitness().ToString("0.00000"))
            lin += 1
        Next
        Console.WriteLine()
        Console.Write("Average Fitness:" & pop.AverageFitness.ToString("0.00000"))
    End Sub

    Private Function DummyFitness(ByVal genes() As Byte) As Double
        Dim result As Double = 0.5
        Dim GenomeLength As Integer = genes.Length
        For i As Integer = 0 To GenomeLength - 1
            If (((i And 1) Xor genes(i)) = 1) Then
                result += 0.5 / GenomeLength
            Else
                result -= 0.5 / GenomeLength
            End If
        Next
        Return (Math.Round(CType((result * 1000000), Double)) / 1000000)
    End Function

    Private Sub Main05()
        Dim amino As New Aminoacids.Molecule
        amino.LoadFlatFile("C:\Documents and Settings\GSchizas\My Documents\Biotech\DISS\AMN\GLU.AMN")
        Dim ser As New Xml.Serialization.XmlSerializer(GetType(Aminoacids.Molecule))
        'Dim ser As New Xml.Serialization.XmlSerializer(GetType(Aminoacids.Atom))

        'For Each atom As Generic.KeyValuePair(Of Integer, Aminoacids.Atom) In amino.Atoms
        '    '    Console.WriteLine(String.Format("{0:0.00000} {1:0.00000} {2:0.00000}", atom.Value.X, atom.Value.Y, atom.Value.Z))
        '    ser.Serialize(Console.Out, atom.Value)
        '    Console.WriteLine()
        'Next
        'Console.WriteLine()
        Dim sb As New Text.StringBuilder
        Dim txtOut As New IO.StringWriter(sb)

        ser.Serialize(txtOut, amino)
        Console.WriteLine(sb.ToString)

        Dim txtIn As New IO.StringReader(sb.ToString)

        'Dim amino2 As Aminoacids.Molecule
        'amino2 = ser.Deserialize(txtIn)

        'Console.WriteLine(amino)
        'Console.WriteLine(amino2)
        'Dim dict As New Generic.List(Of Aminoacids.Bond)
        'Dim ser2 As New Xml.Serialization.XmlSerializer(GetType(Generic.List(Of Aminoacids.Bond)))

        'dict.Add(New Aminoacids.Bond(1, Aminoacids.BondType.Single))
        'dict.Add(New Aminoacids.Bond(2, Aminoacids.BondType.Double))
        'dict.Add(New Aminoacids.Bond(3, Aminoacids.BondType.Single))
        'dict.Add(New Aminoacids.Bond(4, Aminoacids.BondType.Triple))

        'ser2.Serialize(Console.Out, dict)
        'Console.WriteLine()


    End Sub

    Private Sub Main06()
        Dim aminos() As Aminoacids.Molecule
        aminos = New Aminoacids.Molecule(3) {}
        aminos.Initialize()
        For i As Integer = 0 To aminos.Length - 1
            aminos(i) = New Aminoacids.Molecule()
            aminos(i).LoadFlatFile("C:\Documents and Settings\GSchizas\My Documents\Biotech\DISS\AMN\ALA.AMN")
        Next

        Dim ser As New Xml.Serialization.XmlSerializer(GetType(Aminoacids.Molecule()))

        Console.WriteLine()
        ser.Serialize(New IO.StreamWriter("C:\before.xml"), aminos)
        Console.WriteLine()

        For i As Integer = 1 To aminos.Length - 1
            Aminoacids.Molecule.Join(aminos(i - 1), aminos(i))
        Next

        Console.WriteLine()
        ser.Serialize(New IO.StreamWriter("C:\after.xml"), aminos)
        Console.WriteLine()
    End Sub

    Private Sub Main07()
        Dim frm As New Display
        frm.ShowDialog()
    End Sub

    Private Sub Main08()
        'Dim ser As New Xml.Serialization.XmlSerializer(GetType(Aminoacids.Atom))
        'Dim anAtomXml As String = "<atom id=""a1"" elementType=""N"" x3=""0.039000"" y3=""-0.028000"" z3=""0.000000"" role=""peptideN""/>"
        'Dim anAtomStream As New IO.StringReader(anAtomXml)
        'Dim x As Aminoacids.Atom
        'x = ser.Deserialize(anAtomStream)

        'Console.WriteLine(x.SpecialRole)
        Dim ser As New Xml.Serialization.XmlSerializer(GetType(Aminoacids.Molecule), "http://www.xml-cml.org/schema/cml2/core")
        Dim aMoleculeXml, bMoleculeXml As String
        aMoleculeXml = My.Computer.FileSystem.ReadAllText("C:\Documents and Settings\GSchizas\My Documents\Biotech\DISS\Aminoacids\ala.cml")
        Dim aMoleculeStream As New IO.StringReader(aMoleculeXml)

        Dim x As Aminoacids.Molecule
        x = ser.Deserialize(aMoleculeStream)

        aMoleculeStream.Close()

        Dim sb As New System.Text.StringBuilder
        Dim bMoleculeStream As New IO.StringWriter(sb)

        ser.Serialize(bMoleculeStream, x)

        bMoleculeXml = sb.ToString

        Console.WriteLine(aMoleculeXml)
        Console.WriteLine(bMoleculeXml)
    End Sub

    Private Sub Main09()

        Dim names() As String = New String() {"Tyr", "Gly", "Gly", "Phe", "Met"}
        Dim id As Integer = 0
        Dim running As Boolean = True
        Console.WriteLine("Press [F2] to run, [F10] to exit")
        Do
            id += 1
            Select Case Console.ReadKey(True).Key
                Case ConsoleKey.F2
                    RunOnce(names, id)
                Case ConsoleKey.F10
                    running = False
            End Select
        Loop Until Not running
    End Sub

    Private Sub RunOnce(ByVal names As String(), ByVal id As Integer)
        Dim r As New Random
        Dim angles(10) As Double
        For i As Integer = LBound(angles) To UBound(angles)
            angles(i) = r.Next(0, 15) * 22.5
        Next

        Dim f As New Fitness.Calculator
        f.Conformation(names, angles)
        f.SaveProtein(String.Format("C:\out{0:0000}.cml", id))
        Console.WriteLine(String.Format("id: {0:0000} = {1}", id, f.Calculate))
    End Sub

    Private Sub Main10()
        Dim amino As Aminoacids.Molecule
        amino = Aminoacids.Molecule.Load("ala")
    End Sub

    Private Sub Main11()
        For i As Integer = 250 To 260
            Console.WriteLine(Aminoacids.Atom.AtomName(i, 0))
        Next
    End Sub

    Private Sub Main12()
        Dim pop As New ProFoGA.Critters.Population(40, 40, AddressOf RealFitness)
        Dim running As Boolean = True
        Dim ky As System.ConsoleKeyInfo
        While running
            DisplayPopulation(pop, 0, 0)
            Console.SetCursorPosition(0, Console.WindowHeight - 2)
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine("[F3][F4] to grow/kill by 0.2, [F5][F6] to grow/kill to 40/20")
            Console.Write("[F9] to randomize, [F10] to exit")
            Console.ResetColor()
            ky = Console.ReadKey
            Console.Clear()
            Select Case ky.Key
                Case ConsoleKey.F3
                    pop.Grow(0.2)
                Case ConsoleKey.F4
                    pop.Kill(0.2)
                Case ConsoleKey.F5
                    pop.GrowTo(40)
                Case ConsoleKey.F6
                    pop.KillTo(20)
                Case ConsoleKey.F9
                    pop.Randomize()
                Case ConsoleKey.F10
                    Console.WriteLine()
                    running = False
            End Select
        End While
        Dim f As New Fitness.Calculator
        Dim angles(pop.GenomeLength / 4 - 1) As Double
        For i As Integer = 0 To angles.Length - 1
            With pop.Critters(pop.Critters.Count - 1)
                angles(i) = 22.5 * (.gene(i * 4) + .gene(i * 4 + 1) * 2 + .gene(i * 4 + 2) * 4 + .gene(i * 4 + 3) * 8)
            End With
        Next
        f.Conformation(ProteinAminoAcids, angles)
        f.SaveProtein("C:\out.cml")
    End Sub

    Dim ProteinAminoAcids() As String = {"Tyr", "Gly", "Gly", "Phe", "Met"}

    Private Function RealFitness(ByVal genes() As Byte) As Double
        Dim f As New Fitness.Calculator
        Dim angles(genes.Length / 4 - 1) As Double
        For i As Integer = 0 To angles.Length - 1
            angles(i) = 22.5 * (genes(i * 4) + genes(i * 4 + 1) * 2 + genes(i * 4 + 2) * 4 + genes(i * 4 + 3) * 8)
        Next
        f.Conformation(ProteinAminoAcids, angles)
        'Console.WriteLine(String.Format("id: {0:0000} = {1}", id, f.Calculate))
        Return -f.Calculate
    End Function

    Private Sub Main13()
        Dim x As New Hashtable
        x("abc") = "def"
    End Sub


End Module
