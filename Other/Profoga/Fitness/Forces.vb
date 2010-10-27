Module Forces

    Public Function Coulomb(ByVal charge1 As SByte, ByVal charge2 As SByte, ByVal distanceSquared As Double) As Double
        Const kc As Double = 8988000000.0
        Const charge As Double = -1.60217653E-19
        Return kc * charge1 * charge * charge2 * charge / distanceSquared / 10000000000 ' 
    End Function

    Public LenardJonesC12 As Generic.Dictionary(Of String, Double)
    Public LenardJonesC6 As Generic.Dictionary(Of String, Double)
    Public LenardJonesHydrogen As Generic.Dictionary(Of String, Double)

    Public Function LenardJones(ByVal element1 As String, ByVal element2 As String, ByVal distanceSquared As Double) As Double
        Dim A As Double = LenardJonesC12(element1 & element2)
        Dim B As Double = LenardJonesC6(element1 & element2)
        Return A / distanceSquared ^ 6 - B / distanceSquared ^ 3
        ' A / distance ^ 12 - B / distance ^ 6
    End Function

    Public Function Hydrogen(ByVal element1 As String, ByVal element2 As String, ByVal distanceSquared As Double) As Double
        Dim A As Double = LenardJonesHydrogen(element1 & element2 & "C12")
        Dim B As Double = LenardJonesHydrogen(element1 & element2 & "C10")
        Return A / distanceSquared ^ 6 - B / distanceSquared ^ 5
    End Function

    Public Function Sulfide(ByVal distanceSquared As Double) As Double
        Return 0
    End Function

    Sub New()
        LenardJonesC12 = New Generic.Dictionary(Of String, Double)
        LenardJonesC6 = New Generic.Dictionary(Of String, Double)
        LenardJonesHydrogen = New Generic.Dictionary(Of String, Double)

        LenardJonesC6("CC") = 1127.684
        LenardJonesC6("CN") = 783.3452
        LenardJonesC6("CO") = 633.7542
        LenardJonesC6("CS") = 1476.364
        LenardJonesC6("CH") = 226.9102
        LenardJonesC6("NC") = 783.3452
        LenardJonesC6("NN") = 546.7653
        LenardJonesC6("NO") = 445.9175
        LenardJonesC6("NS") = 1036.932
        LenardJonesC6("NH") = 155.9833
        LenardJonesC6("OC") = 633.7542
        LenardJonesC6("ON") = 445.9175
        LenardJonesC6("OO") = 368.6774
        LenardJonesC6("OS") = 854.6872
        LenardJonesC6("OH") = 124.0492
        LenardJonesC6("SC") = 1476.364
        LenardJonesC6("SN") = 1036.932
        LenardJonesC6("SO") = 854.6872
        LenardJonesC6("SS") = 1982.756
        LenardJonesC6("SH") = 290.0756
        LenardJonesC6("HC") = 226.9102
        LenardJonesC6("HN") = 155.9833
        LenardJonesC6("HO") = 124.0492
        LenardJonesC6("HS") = 290.0756
        LenardJonesC6("HH") = 46.73839

        LenardJonesC12("CC") = 1272653
        LenardJonesC12("CN") = 610155.1
        LenardJonesC12("CO") = 588883.8
        LenardJonesC12("CS") = 1569268
        LenardJonesC12("CH") = 88604.24
        LenardJonesC12("NC") = 610155.1
        LenardJonesC12("NN") = 266862.2
        LenardJonesC12("NO") = 249961.4
        LenardJonesC12("NS") = 721128.6
        LenardJonesC12("NH") = 39093.66
        LenardJonesC12("OC") = 588883.8
        LenardJonesC12("ON") = 249961.4
        LenardJonesC12("OO") = 230584.4
        LenardJonesC12("OS") = 675844.1
        LenardJonesC12("OH") = 38919.64
        LenardJonesC12("SC") = 1569268
        LenardJonesC12("SN") = 721128.6
        LenardJonesC12("SO") = 675844.1
        LenardJonesC12("SS") = 1813147
        LenardJonesC12("SH") = 126821.3
        LenardJonesC12("HC") = 88604.24
        LenardJonesC12("HN") = 39093.66
        LenardJonesC12("HO") = 38919.64
        LenardJonesC12("HS") = 126821.3
        LenardJonesC12("HH") = 1908.578

        LenardJonesHydrogen("OHC12") = 75570
        LenardJonesHydrogen("OHC10") = 23850
        LenardJonesHydrogen("NHC12") = 75570
        LenardJonesHydrogen("NHC10") = 23850
        LenardJonesHydrogen("SHC12") = 2657200
        LenardJonesHydrogen("SHC10") = 354290


    End Sub
End Module
