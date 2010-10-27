Public Class Population

    Public Critters As Generic.List(Of Critter)
    Public Generation As Integer

    Public ReadOnly Property GenomeLength() As Integer
        Get
            Return _GenomeLength
        End Get
    End Property
    Private _GenomeLength As Integer

    Public Sub Grow(ByVal by As Double)
        GrowTo(CInt(Me.Critters.Count * (1 + by)))
    End Sub

    Public Sub GrowTo(ByVal finalPopulation As Integer)
        Dim popul As Integer = Me.Critters.Count
        Dim r As New Random
        Dim mother As Integer
        Dim father As Integer
        Dim baby As Critter
        Dim i As Integer = 0
        While Me.Critters.Count < finalPopulation
            mother = r.Next(popul)
            father = r.Next(popul)
            baby = Critter.Sibling(Critters(father), Critters(mother))
            Critters.Add(baby)
            i += 1
        End While
        Generation += 1
    End Sub

    Public Sub Kill(ByVal by As Double)
        Dim popul As Integer = Me.Critters.Count
        KillTo(popul * (1 - by))
    End Sub

    Public Sub KillTo(ByVal finalPopulation As Integer)
        Dim popul As Integer = Me.Critters.Count
        Critters.Sort()
        While Critters.Count > finalPopulation
            Critters.RemoveAt(0)
        End While
    End Sub

    Public Sub Randomize()
        Dim r As New System.Random
        For Each c As Critters.Critter In Me.Critters
            c.RandomInit(r)
        Next
    End Sub

    Public Function AverageFitness() As Double
        Dim sum As Double = 0
        For Each c As Critters.Critter In Me.Critters
            sum += c.Fitness
        Next
        Return sum / Me.Critters.Count
    End Function

    Public Function MaxFitness() As Double
        Dim max As Double = 0
        Dim f As Double
        For Each c As Critters.Critter In Me.Critters
            f = c.Fitness
            If f > max Then max = f
        Next
        Return f
    End Function

    Public Sub New(ByVal theGenomeLength As Integer, ByVal individuals As Integer, ByVal aFitnessFunction As Critter.CalculateFitness)
        _GenomeLength = theGenomeLength
        Critters = New List(Of Critter)
        For i As Integer = 1 To individuals
            Critters.Add(New Critter(GenomeLength, aFitnessFunction))
        Next
        Generation = 0
    End Sub
End Class
