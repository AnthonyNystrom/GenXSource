Public Class Progress

    Private OriginalPopulation As Integer
    Private ProgressData As Data.DataTable

    Public Sub InitializePopulation(ByVal ChromosomeLength As Integer, ByVal PopulationLength As Integer)
        OriginalPopulation = PopulationLength
        Base.Population = New ProFoGA.Critters.Population(ChromosomeLength, OriginalPopulation, AddressOf Base.ProtCalc.RealFitness)
        Base.Population.Randomize()
        InitializeProgressData()
        DisplayPopulation()
    End Sub

    Sub InitializeProgressData()
        ProgressData = New Data.DataTable("ProFoGA")
        ProgressData.Columns.Add(New Data.DataColumn("Generation", GetType(Integer)))
        ProgressData.Columns.Add(New Data.DataColumn("MaximumFitness", GetType(Double)))
        ProgressData.Columns.Add(New Data.DataColumn("AverageFitness", GetType(Double)))
    End Sub

    Sub NotReadyYet()
        'Dim running As Boolean = True
        'While running
        'DisplayPopulation(pop, 0, 0)
        'Console.SetCursorPosition(0, Console.WindowHeight - 2)
        'Console.ForegroundColor = ConsoleColor.White
        'Console.WriteLine("[F3][F4] to grow/kill by 0.2, [F5][F6] to grow/kill to 40/20")
        'Console.Write("[F9] to randomize, [F10] to exit")
        'Console.ResetColor()
        'ky = Console.ReadKey
        'Console.Clear()
        'Select Case ky.Key
        '    Case ConsoleKey.F3
        'pop.Grow(0.2)
        '    Case ConsoleKey.F4
        'pop.Kill(0.2)
        '    Case ConsoleKey.F5
        'pop.GrowTo(40)
        '    Case ConsoleKey.F6
        'pop.KillTo(20)
        '    Case ConsoleKey.F9
        'pop.Randomize()
        'Case ConsoleKey.F10
        '    Console.WriteLine()
        '    running = False
        'End Select
        'End While

        'Dim f As New Fitness.Calculator
        'Dim angles(pop.GenomeLength / 4 - 1) As Double
        'For i As Integer = 0 To angles.Length - 1
        '    With pop.Critters(pop.Critters.Count - 1)
        '        angles(i) = 22.5 * (.Gene(i * 4) + .Gene(i * 4 + 1) * 2 + .Gene(i * 4 + 2) * 4 + .Gene(i * 4 + 3) * 8)
        '    End With
        'Next

        'f.Init(ProteinAminoAcids, angles)
        'f.SaveProtein("C:\out.cml")

    End Sub

    Private Sub btnGrow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrow.Click
        Me.Cursor = Cursors.WaitCursor
        Base.Population.Grow(0.2)
        DisplayPopulation()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub btnKill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKill.Click
        Me.Cursor = Cursors.WaitCursor
        Base.Population.Kill(0.2)
        DisplayPopulation()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Me.lbPopulation.SelectedIndex < 0 Then Exit Sub
        Dim angles() As Double
        My.Forms.Display.Loaded = False
        Dim c As Critters.Critter
        c = Base.Population.Critters(Me.lbPopulation.SelectedIndex)
        angles = Base.ProtCalc.AnglesFromGene(c.Gene)
        Base.ProtCalc.Conformation(angles)
        'My.Forms.Display.Aminos = Base.ProtCalc.ProteinAminoAcids
        My.Forms.Display.Loaded = True
        My.Forms.Display.Show()
    End Sub

    Private Sub DisplayPopulation()
        Dim maxFit, avgFit As Double
        lbPopulation.Items.Clear()
        For Each c As Critters.Critter In Base.Population.Critters
            lbPopulation.Items.Add(c.GeneText)
        Next
        maxFit = Math.Abs(Base.Population.MaxFitness)
        avgFit = Math.Abs(Base.Population.AverageFitness)

        lblPopulationNumber.Text = Base.Population.Critters.Count
        lblMaxFitnessNumber.Text = maxFit.ToString("e")
        lblAvgFitnessNumber.Text = avgFit.ToString("e")
        lblGenerationNumber.Text = Base.Population.Generation

        Dim newRow As Data.DataRow = ProgressData.NewRow
        newRow.Item("Generation") = Base.Population.Generation
        newRow.Item("MaximumFitness") = maxFit
        newRow.Item("AverageFitness") = avgFit
        ProgressData.Rows.Add(newRow)
    End Sub

    Private Sub btnStep_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStep.Click
        Me.Cursor = Cursors.WaitCursor
        Base.Population.GrowTo(OriginalPopulation * 1.5)
        Base.Population.KillTo(OriginalPopulation)
        DisplayPopulation()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Application.Exit()
    End Sub

    Private Sub lbPopulation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbPopulation.SelectedIndexChanged
        Me.btnDisplay.Enabled = Me.lbPopulation.SelectedIndex >= 0
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If dlgExport.ShowDialog = Windows.Forms.DialogResult.OK Then
            ProgressData.WriteXml(dlgExport.FileName)
        End If
    End Sub
End Class