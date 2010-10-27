Public Class Critter
    Implements IComparable(Of Critter)

    ' Methods
    Public Sub New(ByVal genomeLength As Integer, ByVal aFitnessFunction As CalculateFitness)
        'Me.Age = 0
        'Me.GenomeLength = 0
        'Me.flag1 = False
        'Me.flag2 = False
        Me.GenomeLength = genomeLength
        Me.Gene = New Byte(Me.GenomeLength - 1) {}
        Me.Gene.Initialize()
        FitnessFunction = aFitnessFunction
    End Sub

    Private CachedFitness As Double = 0

    ' Public Sub Display(ByVal ACanvas As Graphics, ByVal ARect As Rectangle)
    Function Fitness() As Double
        If CachedFitness = 0 Then
            If FitnessFunction IsNot Nothing Then
                CachedFitness = FitnessFunction.Invoke(Gene)
            Else
                CachedFitness = 0
            End If
        End If
        Return CachedFitness
    End Function

    'Dummy Fitness Function:
    'Private Function DummyFitness()
    '    Dim result As Double = 0.5
    '    Dim i As UInt16 = 0
    '    Do While (i < Me.GenomeLength)
    '        If (((i And 1) Xor Me.gene(i)) = 1) Then
    '            result = (result + (0.5 / CType(Me.GenomeLength, Double)))
    '        Else
    '            result = (result - (0.5 / CType(Me.GenomeLength, Double)))
    '        End If
    '        i = CType((i + 1), UInt16)
    '    Loop
    '    Return (Math.Round(CType((result * 1000000), Double)) / 1000000)
    'End Function


    Public FitnessFunction As CalculateFitness

    Public Delegate Function CalculateFitness(ByVal Genome As Byte()) As Double

    Public Sub RandomInit(ByVal r As Random)
        Dim i As Integer = 0
        Do While (i < Me.GenomeLength)
            Me.Gene(i) = r.Next(2) ' IIf((r.NextDouble >= 0.5), CType(1, Byte), CType(0, Byte))
            i += 1
        Loop
    End Sub

    Public Shared Function Sibling(ByVal Father As Critter, ByVal Mother As Critter) As Critter
        Dim result As Critter
        Const MutationRate As Double = 0.5
        Dim r As New Random
        Dim i, j As Integer
        Dim COnum As Integer = (1 + r.Next(10))
        Dim CO As Integer() = New Integer(COnum - 1) {}
        Dim fromMother As Boolean = (r.NextDouble >= 0.5)
        result = New Critter(Mother.GenomeLength, Mother.FitnessFunction)

        'Crossing Over
        For j = 0 To COnum - 1
            CO(j) = r.Next((result.GenomeLength + 2))
            j += 1
        Next

        For i = 0 To result.GenomeLength - 1
            result.Gene(i) = IIf(fromMother, Mother.Gene(i), Father.Gene(i))
            For j = 0 To COnum - 1
                If (i = CO(j)) Then
                    fromMother = Not fromMother
                End If
                j += 1
            Next
        Next
        'mutations
        For i = 1 To r.Next(MutationRate * result.GenomeLength)
            result.Gene(r.Next(result.GenomeLength)) = CType(r.Next(2), Byte)
        Next
        Return result
    End Function

    ' Fields
    'Public Age As Byte
    'Public flag1 As Boolean
    'Public flag2 As Boolean
    Public Gene As Byte()
    Public GenomeLength As Integer

    Public ReadOnly Property GeneText() As String
        Get
            Dim sb As New System.Text.StringBuilder
            For Each oneGene As Byte In Gene
                sb.Append(oneGene.ToString)
            Next
            Return sb.ToString
        End Get
    End Property

    Public Function CompareTo(ByVal other As Critter) As Integer Implements System.IComparable(Of Critter).CompareTo
        Return Me.Fitness.CompareTo(other.Fitness)
    End Function
End Class


