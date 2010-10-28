Imports System.Math

Namespace MPPC
    ''' <summary> Solves the linear equation </summary>
    Class UnDefJaggedArray
        Dim N As Int32
        Dim store As New ArrayList
        Public Sub New(ByVal _N As Int32)
            N = _N
            store.Capacity = N + 1
        End Sub
        Public Sub SetVal(ByVal R As Int32, ByVal C As Int32, ByVal val As Decimal)
            Dim oneShelf As New ArrayList
            If store.Count <= R Then
                Do While store.Count <= R
                    Dim newShelf As New ArrayList
                    store.Add(newShelf)
                Loop
            End If
            oneShelf = CType(store(R), ArrayList)
            If oneShelf.Count <= C Then
                Do While oneShelf.Count <= C
                    oneShelf.Add(0.0#)
                Loop
            End If
            oneShelf(C) = val
        End Sub
        Public Function GetVal(ByVal R As Int32, ByVal C As Int32) As Decimal
            Dim Y As Decimal
            Dim oneShelf As ArrayList
            If store.Count <= R Then
                Y = 0.0#
            Else
                oneShelf = CType(store(R), ArrayList)
                If oneShelf.Count <= C Then
                    Y = 0.0#
                Else
                    Y = oneShelf(C)
                End If
            End If
            Return Y
        End Function
    End Class

    Class UnDefArray
        Dim store As New ArrayList
        Public Sub SetVal(ByVal C As Int32, ByVal val As Decimal)
            If store.Count <= C Then
                Do While store.Count <= C
                    store.Add(0.0#)
                Loop
            End If
            store(C) = val
        End Sub
        Public Function GetVal(ByVal C As Int32) As Decimal
            Dim Y As Decimal
            If store.Count <= C Then
                Y = 0.0#
            Else
                Y = store(C)
            End If
            Return Y
        End Function
    End Class


    Public Class NuGenMultiplePrecisionPocketCalculator
        Shared MDEZ#(15)
        Public Shared ID As Int32

        Shared Sub New()
            GENERATING_POWERS_OF_10()
        End Sub

#Region "Private methods"
        '[10000]**************************************************************************
        ' GENERATING POWERS OF 10
        Private Shared Sub GENERATING_POWERS_OF_10()
            MDEZ#(0) = 1.0# : MDEZ#(1) = 10.0# : MDEZ#(2) = 100.0# : MDEZ#(3) = 1000.0# : MDEZ#(4) = 10000.0# : MDEZ#(5) = 100000.0# : MDEZ#(6) = 1000000.0# : MDEZ#(7) = 10000000.0# : MDEZ#(8) = 100000000.0# : MDEZ#(9) = 1000000000.0# : MDEZ#(10) = 10000000000.0# : MDEZ#(11) = 100000000000.0# : MDEZ#(12) = 1000000000000.0#
            MDEZ#(13) = 10000000000000.0# : MDEZ#(14) = 100000000000000.0# : MDEZ#(15) = 1.0E+15#
        End Sub
        '[11805] VALUES OF PI, 2*PI, PI/2
        Public Shared Sub VALUES_OF_PI(ByRef MPI#(), ByRef MPI2#(), ByRef MPIM#())
            MPI#(0) = 31415926535897.0# : MPI#(1) = 93238462643383.0# : MPI#(2) = 27950288419716.0#
            MPI#(3) = 93993751058209.0# : MPI#(4) = 74944592307816.0# : MPI#(5) = 40628620899862.0#
            MPI#(6) = 80348253421170.0# : MPI#(7) = 67982148086513.0# : MPI#(8) = 28230664709384.0# : MPI#(9) = 46095505822317.0#
            MPI2#(0) = 62831853071795.0# : MPI2#(1) = 86476925286766.0# : MPI2#(2) = 55900576839433.0#
            MPI2#(3) = 87987502116419.0# : MPI2#(4) = 49889184615632.0# : MPI2#(5) = 81257241799725.0#
            MPI2#(6) = 60696506842341.0# : MPI2#(7) = 35964296173026.0# : MPI2#(8) = 56461329418768.0# : MPI2#(9) = 92191011644634.0#
            MPIM#(0) = 15707963267948.0# : MPIM#(1) = 96619231321691.0# : MPIM#(2) = 63975144209858.0#
            MPIM#(3) = 46996875529104.0# : MPIM#(4) = 87472296153908.0# : MPIM#(5) = 20314310449931.0#
            MPIM#(6) = 40174126710585.0# : MPIM#(7) = 33991074043256.0# : MPIM#(8) = 64115332354692.0# : MPIM#(9) = 23047752911158.0#
        End Sub
        '[12035] VALUES OF TAN(PI/20), TAN(3*PI/20), TAN(PI/10), TAN(PI/5)
        Public Shared Sub VALUES_OF_TAN(ByRef MTP20#(), ByRef MT3P20#(), ByRef MTP5#(), ByRef MTP10#())
            MTP20#(0) = 15838444032453.0# : MTP20#(1) = 62938388830926.0# : MTP20#(2) = 94366411433916.0#
            MTP20#(3) = 21607373329723.0# : MTP20#(4) = 17409950356576.0# : MTP20#(5) = 371427139809.0#
            MTP20#(6) = 59820686711676.0# : MTP20#(7) = 83969760247784.0# : MTP20#(8) = 24624563536012.0# : MTP20#(9) = 74685035858549.0# : MTP20#(10) = 89072371641780.0#
            MT3P20#(0) = 50952544949442.0# : MT3P20#(1) = 88105137069112.0# : MT3P20#(2) = 50657485824525.0#
            MT3P20#(3) = 96664631726152.0# : MT3P20#(4) = 8309180647715.0# : MT3P20#(5) = 33792121810993.0#
            MT3P20#(6) = 83688616264218.0# : MT3P20#(7) = 35378150042534.0# : MT3P20#(8) = 73093818956995.0# : MT3P20#(9) = 98877948947202.0# : MT3P20#(10) = 11474453963664.0#
            MTP5#(0) = 72654252800536.0# : MTP5#(1) = 8858954667574.0# : MTP5#(2) = 80618749616092.0#
            MTP5#(3) = 39296520846275.0# : MTP5#(4) = 663273457493.0# : MTP5#(5) = 91845683088420.0#
            MTP5#(6) = 57752221614009.0# : MTP5#(7) = 14316931718973.0# : MTP5#(8) = 689685296271.0# : MTP5#(9) = 73566758439156.0# : MTP5#(10) = 52126761370862.0#
            MTP10#(0) = 32491969623290.0# : MTP10#(1) = 63261558714122.0# : MTP10#(2) = 15134464954903.0#
            MTP10#(3) = 47152147510030.0# : MTP10#(4) = 78047191366729.0# : MTP10#(5) = 960744948322.0#
            MTP10#(6) = 68773544696505.0# : MTP10#(7) = 4817038709027.0# : MTP10#(8) = 41989556011889.0# : MTP10#(9) = 91361520930611.0# : MTP10#(10) = 30422105682941.0#
        End Sub

        '[10020] *************************************************************************
        ' ROUTINE THAT FURNISHES THE EXPONENT
        ' INPUT  - MAEX#
        ' OUTPUT - MEX
        Private Shared Sub FURNISHES_EXPONENT(ByVal MAEX#, ByRef MB$, ByRef MEX As Int32)
            If MAEX# = 0 Then MEX = 0 : Return
            'CINT - OK
            MEX = CInt(Log(Abs(CSng(MAEX#))) / Log(10)) : MB$ = "1.D" + Str$(MEX) : If Abs(MAEX#) < Val(MB$) Then MEX = MEX - 1
        End Sub
        '[10120] *************************************************************************
        ' ROUTINE THAT BREAKS A INTEGER NUMBER WITH 14 DIGITS INTO TWO NUMBERS            WITH 7 DIGITS
        ' INPUT  - MSEP#
        ' OUTPUT - MS1#,MS2#
        Private Shared Sub BREAKS_INTEGER(ByVal MSEP#, ByRef MS1#, ByRef MS2#)
            MS1# = Fix(MSEP# / MDEZ#(7)) : MS2# = Fix(MSEP# - MS1# * MDEZ#(7))
        End Sub
        '[11260] THIS IS AN AUXILIARY ROUTINE FOR ROUTINE 11020
        ' INPUT  - ML#(ID),ML1#
        ' OUTPUT - SAME
        Private Shared Sub AUXILIARY_NATURAL_LOGARITHM(ByRef ML#(), ByRef ML1#)
            Dim MN9 As Int32, MX6 As Int32, MEX As Int32
            Dim MA1#(ID), MA3#(ID), MA5#(ID), MB1#(ID), MB3#(ID), MC1#(ID), MC3#(ID), MDL#(ID)
            Dim MA2#, MA4#, MA6#, MB2#, MB4#, MC2#, MC4#, MC6#, MAEX#, MB$, MX1#, M1%
11265:      For MN9 = 0 To ID : MA1#(MN9) = ML#(MN9) : MB1#(MN9) = 0 : Next MN9
            MB1#(0) = -MDEZ#(13) : MA2# = ML1# : MB2# = 0
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
            For MN9 = 0 To ID : MA3#(MN9) = MC1#(MN9) : Next MN9
            MA4# = MC2#
11270:      MB1#(0) = -MB1#(0)
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
11275:      For MN9 = 0 To ID : MB3#(MN9) = MC1#(MN9) : Next MN9
            MB4# = MC2#
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
11280:      MX6 = 0
            For MN9 = 0 To ID
                MDL#(MN9) = MC3#(MN9) : MA1#(MN9) = MC3#(MN9) : MB1#(MN9) = MC3#(MN9)
            Next MN9
            MC6# = MC4# : MA2# = MC4# : MB2# = MC4#
11285:      MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
            For MN9 = 0 To ID : MA5#(MN9) = MC1#(MN9) : Next MN9 : MA6# = MC2#
11290:      For MN9 = 0 To ID : MA1#(MN9) = MA5#(MN9) : MB1#(MN9) = MC3#(MN9) : Next MN9
            MA2# = MA6# : MB2# = MC4#
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
11295:      MAEX# = MX6 + MX6 + 1
            FURNISHES_EXPONENT(MAEX#, MB$, MEX) ' 10020
            For MN9 = 0 To ID
                MA1#(MN9) = 0 : MB1#(MN9) = MC1#(MN9)
            Next MN9
            MA1#(0) = MAEX# * MDEZ#(13 - MEX) : MA2# = MEX : MB2# = MC2#
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
11300:      MAEX# = MX6 + MX6 + 3
            FURNISHES_EXPONENT(MAEX#, MB$, MEX) ' 10020
            For MN9 = 0 To ID
                MA3#(MN9) = MC1#(MN9) : MB3#(MN9) = 0
            Next MN9
            MB3#(0) = MAEX# * MDEZ#(13 - MEX) : MA4# = MC2# : MB4# = MEX
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
11305:      MX1# = MC4# - MC6#
            For MN9 = 0 To ID
                If Abs(MC3#(MN9)) = Abs(MDL#(MN9)) Then GoTo 11320
11310:          If Abs(MC3#(MN9)) < Abs(MDL#(MN9)) Then GoTo 11325
11315:          GoTo 11330
11320:      Next MN9
            GoTo 11330
11325:      MX1# = MX1# - 1
11330:      If MX1# + 14 * (ID + 1) < 0 Then GoTo 11345
11335:      For MN9 = 0 To ID : MA1#(MN9) = MDL#(MN9) : MB1#(MN9) = MC3#(MN9) : Next MN9 : MA2# = MC6# : MB2# = MC4#
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
11340:      For MN9 = 0 To ID : MDL#(MN9) = MC1#(MN9) : Next MN9 : MC6# = MC2# : MX6 = MX6 + 1
            GoTo 11290
11345:      M1% = 2
            For MN9 = 0 To ID : MA1#(MN9) = MDL#(MN9) : Next MN9 : MA2# = MC6#
            MULTIPLICATION_BY_INTEGER(M1%, MA1#, MA2#, MC1#, MC2#)    ' 10040
            For MN9 = 0 To ID : ML#(MN9) = MC1#(MN9) : Next MN9 : ML1# = MC2#
        End Sub
        '[11350] NATURAL LOGARITHM OF 2, 10 AND sqrt(2)/2
        Private Shared Sub NATURAL_LOGARITHM_SQRT(ByRef MLN2#(), ByRef MR22#(), ByRef MLN10#())
            MLN2#(0) = 69314718055994.0# : MLN2#(1) = 53094172321214.0# : MLN2#(2) = 58176568075500.0#
            MLN2#(3) = 13436025525412.0# : MLN2#(4) = 6800094933936.0# : MLN2#(5) = 21969694715605.0# : MLN2#(6) = 86332699641868.0#
            MLN2#(7) = 75420014810205.0# : MLN2#(8) = 70685733685520.0# : MLN2#(9) = 23575813055703.0#
            MR22#(0) = 70710678118654.0# : MR22#(1) = 75244008443621.0# : MR22#(2) = 4849039284835.0#
            MR22#(3) = 93768847403658.0# : MR22#(4) = 83398689953662.0# : MR22#(5) = 39231053519425.0# : MR22#(6) = 19376716382078.0#
            MR22#(7) = 63675069231154.0# : MR22#(8) = 56148512462418.0# : MR22#(9) = 2792536860632.0#
            MLN10#(0) = 23025850929940.0# : MLN10#(1) = 45684017991454.0# : MLN10#(2) = 68436420760110.0#
            MLN10#(3) = 14886287729760.0# : MLN10#(4) = 33327900967572.0# : MLN10#(5) = 60967735248023.0# : MLN10#(6) = 59972050895982.0#
            MLN10#(7) = 98341967784042.0# : MLN10#(8) = 28624863340952.0# : MLN10#(9) = 54650828067567.0#
        End Sub
#End Region
        ''' <summary> [10040] Routine for multiplication of a integer number by a multiple precision number 
        ''' INPUT  - M1%,MA1#(ID),MA2#
        ''' OUTPUT - MC1#(ID),MC2#
        ''' </summary>
        Public Shared Sub MULTIPLICATION_BY_INTEGER(ByVal M1%, ByVal MA1#(), ByVal MA2#, ByRef MC1#(), ByRef MC2#)
            Dim MN6 As Int32, MN7 As Int32, MN8 As Int32, MN9 As Int32
            Dim MZ9#(ID)
            If M1% = 0 Then For MN9 = 0 To ID : MC1#(MN9) = 0 : Next MN9 : MC2# = 0 : Return
            MN8 = Sign(M1%) : If Abs(M1%) = 10 Then For MN9 = 0 To ID : MC1#(MN9) = MN8 * MA1#(MN9) : Next MN9 : MC2# = MA2# + 1 : Return
            If Abs(M1%) = 100 Then For MN9 = 0 To ID : MC1#(MN9) = MN8 * MA1#(MN9) : Next MN9 : MC2# = MA2# + 2 : Return
            If Abs(M1%) > 100 Then Throw New Exception("INPUT GREATER THAN 100 IN ROUTINE 10040") : Stop
            For MN9 = 0 To ID : MZ9#(MN9) = M1% * MA1#(MN9) : Next MN9
            For MN9 = ID To 1 Step -1 : MN7 = MN9 - 1
                If Abs(MZ9#(MN9)) < MDEZ#(14) Then
                Else
                    MN8 = Fix(MZ9#(MN9) / MDEZ#(14)) : MZ9#(MN9) = MZ9#(MN9) - MN8 * MDEZ#(14) : MZ9#(MN7) = MZ9#(MN7) + MN8
                End If
            Next MN9
            If Abs(MZ9#(0)) < MDEZ#(14) Then For MN9 = 0 To ID : MC1#(MN9) = MZ9#(MN9) : Next MN9 : MC2# = MA2# : Return
            If Abs(MZ9#(0)) < MDEZ#(15) Then MC2# = MA2# + 1 : MN6 = 10 Else MC2# = MA2# + 2 : MN6 = 100
            Dim MSEP#, MS1#, MS2#
            For MN9 = 0 To ID
                MSEP# = MZ9#(MN9) / MN6
                BREAKS_INTEGER(MSEP#, MS1#, MS2#)   ' 10120
                MC1#(MN9) = MS1# * MDEZ#(7) + MS2# : MN8 = MZ9#(MN9) - MC1#(MN9) * MN6
                If MN9 = ID Then
                Else
                    MN7 = MN9 + 1 : MZ9#(MN7) = MZ9#(MN7) + MN8 * MDEZ#(14)
                End If
            Next MN9
        End Sub
        ''' <summary> [10140] Routine that multiplies two numbers in multiple precision 
        ''' INPUT  - MA1#(ID),MA2#,MB1#(ID),MB2#
        ''' OUTPUT - MC1#(ID),MC2#
        ''' </summary>
        Public Shared Sub MULTIPLIES_TWO_NUMBERS(ByVal MA1#(), ByVal MA2#, ByVal MB1#(), ByVal MB2#, ByRef MC1#(), ByRef MC2#)
            Dim MN2 As Int32, MN3 As Int32, MN4 As Int32, MN5 As Int32, MN6 As Int32, MN7 As Int32, MN8 As Int32, MN9 As Int32
            Dim MSEP#, MS1#, MS2#, MAL#(2 * ID + 1), MBL#(2 * ID + 1), MCL#(ID)

            For MN9 = 0 To ID : MC1#(MN9) = 0 : MCL#(MN9) = 0 : Next MN9 : MC2# = 0
            If MA1#(0) = 0 Or MB1#(0) = 0 Then Return
            MN8 = 0
            For MN9 = 0 To ID
                MN7 = MN8 + 1 : MSEP# = MA1#(MN9)
                BREAKS_INTEGER(MSEP#, MS1#, MS2#) ' 10120
                MAL#(MN8) = MS1# : MAL#(MN7) = MS2#
                MSEP# = MB1#(MN9)
                BREAKS_INTEGER(MSEP#, MS1#, MS2#) ' 10120
                MBL#(MN8) = MS1# : MBL#(MN7) = MS2# : MN8 = MN8 + 2
            Next MN9
            MN9 = 2 * ID + 1 : For MN8 = 0 To MN9
                MN7 = MN9 - MN8 : For MN6 = 0 To MN7
                    MN5 = Int((MN6 + MN8) / 2)
                    If Not (2 * MN5 <> MN6 + MN8) Then
                        MCL#(MN5) = MAL#(MN8) * MBL#(MN6) + MCL#(MN5) : For MN4 = MN5 To 0 Step -1
                            If Abs(MCL#(MN4)) < MDEZ#(14) Then
                                MN4 = 0
                            Else
                                If Abs(MCL#(MN4)) > MDEZ#(14) Then MN3 = Fix(MCL#(MN4) / MDEZ#(14)) Else MN3 = Sign(MCL#(MN4))
                                MN2 = MN4 - 1 : MCL#(MN2) = MCL#(MN2) + MN3 : MCL#(MN4) = MCL#(MN4) - MN3 * MDEZ#(14)
                            End If
                        Next MN4
                    Else
                        MN5 = Int((MN6 + MN8 - 1) / 2) : MSEP# = MAL#(MN8) * MBL#(MN6)
                        BREAKS_INTEGER(MSEP#, MS1#, MS2#) ' 10120
                        MCL#(MN5) = MCL#(MN5) + MS1#
                        For MN4 = MN5 To 0 Step -1
                            If Abs(MCL#(MN4)) < MDEZ#(14) Then
                                MN4 = 0
                            Else
                                If Abs(MCL#(MN4)) > MDEZ#(14) Then MN3 = Fix(MCL#(MN4) / MDEZ#(14)) Else MN3 = Sign(MCL#(MN4))
                                MN2 = MN4 - 1 : MCL#(MN2) = MCL#(MN2) + MN3 : MCL#(MN4) = MCL#(MN4) - MN3 * MDEZ#(14)
                            End If
                        Next MN4 : MN5 = MN5 + 1
                        If MN5 > ID Then
                        Else
                            MCL#(MN5) = MCL#(MN5) + MS2# * MDEZ#(7)
                            For MN4 = MN5 To 0 Step -1
                                If Abs(MCL#(MN4)) < MDEZ#(14) Then
                                    MN4 = 0
                                Else
                                    If Abs(MCL#(MN4)) > MDEZ#(14) Then MN3 = Fix(MCL#(MN4) / MDEZ#(14)) Else MN3 = Sign(MCL#(MN4))
                                    MN2 = MN4 - 1 : MCL#(MN2) = MCL#(MN2) + MN3 : MCL#(MN4) = MCL#(MN4) - MN3 * MDEZ#(14)
                                End If
                            Next MN4
                        End If
                    End If
                Next MN6 : Next MN8
            If Abs(MCL#(0)) < MDEZ#(13) Then
            Else
                MC2# = MA2# + MB2# + 1
                For MN8 = 0 To ID : MC1#(MN8) = MCL#(MN8) : Next MN8 : Return
            End If
            MCL#(0) = MCL#(0) * 10 : For MN8 = 1 To ID
                MN7 = MN8 - 1 : MN6 = Fix(MCL#(MN8) / MDEZ#(13)) : MCL#(MN7) = MCL#(MN7) + MN6
                MCL#(MN8) = (MCL#(MN8) - MN6 * MDEZ#(13)) * 10 : Next MN8
            MC2# = MA2# + MB2#
            For MN8 = 0 To ID : MC1#(MN8) = MCL#(MN8) : Next MN8 : Return
        End Sub
        ''' <summary> [10340] Routine that adds two numbers in multiple precision 
        ''' INPUT  - MA1#(ID),MA2#,MB1#(ID),MB2#
        ''' OUTPUT - MC1#(ID),MC2#
        ''' </summary>
        Public Shared Sub ADDS_TWO_NUMBERS(ByVal MA1#(), ByVal MA2#, ByVal MB1#(), ByVal MB2#, ByRef MC1#(), ByRef MC2#)
            Dim MN3 As Int32, MN6 As Int32, MN9 As Int32
            Dim MN1#, MN2#, MN4#, MN5#, MN7#, MN8#, MCL#(ID), MAEX#, MB$
            Dim MEX As Int32, M7%
            'Dim MSEP#, MS1#, MS2#, MAL#(ID), MBL#(ID), 

10350:      MN5# = 0 : M7% = 0 : If MA1#(0) = 0 Then For MN9 = 0 To ID : MC1#(MN9) = MB1#(MN9) : Next MN9 : MC2# = MB2# : Return
10355:      If MB1#(0) = 0 Then For MN9 = 0 To ID : MC1#(MN9) = MA1#(MN9) : Next MN9 : MC2# = MA2# : Return
10360:      If Abs(MA2# - MB2#) < 14 * (ID + 1) Then
            Else
10365:          If MA2# > MB2# Then
                    For MN9 = 0 To ID : MC1#(MN9) = MA1#(MN9) : Next MN9 : MC2# = MA2# : Return
                Else
                    For MN9 = 0 To ID : MC1#(MN9) = MB1#(MN9) : Next MN9 : MC2# = MB2# : Return
                End If
            End If
10375:      If MA2# > MB2# Then '10610
            Else
10380:          If MA2# < MB2# Then MN5# = MB2# : For MN9 = 0 To ID : MCL#(MN9) = MA1#(MN9) : Next MN9 : GoTo 10615
10385:          For MN9 = 0 To ID : MCL#(MN9) = MA1#(MN9) + MB1#(MN9) : Next MN9 : MN5# = MA2#
10390:          MN7 = 0 : For MN9 = 0 To ID
10395:              If MCL#(MN9) <> 0 Then MN8 = MN9 : MN9 = ID : MN7 = 1
10400:          Next MN9 : If MN7 = 0 Then For MN9 = 0 To ID : MC1#(MN9) = 0 : Next MN9 : MC2# = 0 : Return
10405:          If MN8 = 0 Then GoTo 10500
10410:          MN5# = MN5# - 14 * MN8

10411:          If Abs(MCL#(MN8)) >= MDEZ#(13) Then GoTo 10490
10415:          If MN8 < ID Then GoTo 10430
10420:          MAEX# = MCL#(MN8)
                FURNISHES_EXPONENT(MAEX#, MB$, MEX) ' 10020
                MN9 = 13 - MEX : MC1#(0) = MCL#(ID) * MDEZ#(MN9) : MC2# = MN5# - MN9
10425:          For MN9 = 1 To ID : MC1#(MN9) = 0 : Next MN9 : Return
10430:          MN9 = Sign(MCL#(MN8)) : MN7 = MN8 + 1 : If MN9 = Sign(MCL#(MN7)) Then GoTo 10465
10435:          MCL#(MN8) = MCL#(MN8) - MN9
10440:          If MCL#(MN8) <> 0 Then GoTo 10460
10445:          MN5# = MN5# - 14 : MCL#(MN7) = MCL#(MN7) + MN9 * MDEZ#(14) : MN6 = ID - MN8 : For MN4 = 0 To MN6
10450:              MN3 = MN4 + MN8 : MCL#(MN4) = MCL#(MN3) : Next MN4 : MN6 = MN6 + 1
10455:          For MN4 = MN6 To ID : MCL#(MN4) = 0 : Next MN4 : GoTo 10411
10460:          MCL#(MN7) = MCL#(MN7) + MN9 * MDEZ#(14)

10465:          MAEX# = MCL#(MN8)
                FURNISHES_EXPONENT(MAEX#, MB$, MEX) ' 10020
                MN7 = 13 - MEX : MN9 = MEX + 1 : MN5# = MN5# - MN7
10470:          For MN6 = MN8 + 1 To ID
10475:              MN4# = Fix(MCL#(MN6) / MDEZ#(MN9))
10480:              MN3 = MN6 - 1 : MCL#(MN3) = MCL#(MN3) * MDEZ#(MN7) + MN4# : MCL#(MN6) = MCL#(MN6) - MN4# * MDEZ#(MN9) : Next MN6
10485:          MCL#(ID) = MCL#(ID) * MDEZ#(MN7)

10490:          MN9 = ID - MN8 : For MN6 = 0 To MN9 : MCL#(MN6) = MCL#(MN6 + MN8) : Next MN6
10495:          MN9 = MN9 + 1 : For MN6 = MN9 To ID : MCL#(MN6) = 0 : Next MN6
10500:          For MN9 = ID To 1 Step -1 : If Abs(MCL#(MN9)) < MDEZ#(14) Then GoTo 10515
10505:              MN7 = Fix(MCL#(MN9) / MDEZ#(14))
10510:              MN6 = MN9 - 1 : MCL#(MN6) = MCL#(MN6) + MN7 : MCL#(MN9) = MCL#(MN9) - MN7 * MDEZ#(14)
10515:          Next MN9 : If Abs(MCL#(0)) < MDEZ#(14) Then GoTo 10540
10520:          MAEX# = MCL#(0)
                FURNISHES_EXPONENT(MAEX#, MB$, MEX) ' 10020
                MN9 = MEX - 13 : MN5# = MN5# + MN9 : MN7# = 0 : MN1 = ID
10525:          For MN6 = 0 To MN1
10530:              MCL#(MN6) = MCL#(MN6) + MN7# * MDEZ#(MEX) : MN4# = Fix(MCL#(MN6) / MDEZ#(MN9))
10535:              MN7# = MCL#(MN6) - MN4# * MDEZ#(MN9) : MCL#(MN6) = MN4# : Next MN6

10540:          If Abs(MCL#(0)) >= MDEZ#(13) Then GoTo 10585
10545:          MN9 = Sign(MCL#(0)) : If (MN9 = Sign(MCL#(1)) Or MN9 * Sign(MCL#(1)) = 0) Then GoTo 10566
10550:          MCL#(0) = MCL#(0) - MN9 : If MCL#(0) <> 0 Then GoTo 10565
10555:          MN5# = MN5# - 14 : MCL#(1) = MCL#(1) + MN9 * MDEZ#(14) : For MN6 = 1 To ID
10560:              MN7 = MN6 - 1 : MCL#(MN7) = MCL#(MN6) : Next MN6 : MCL#(ID) = 0 : GoTo 10540
10565:          MCL#(1) = MCL#(1) + MN9 * MDEZ#(14)

10566:          MAEX# = MCL#(0)
                FURNISHES_EXPONENT(MAEX#, MB$, MEX) ' 10020
                MN9 = 13 - MEX : MN7 = MEX + 1 : MN5# = MN5# - MN9
10570:          For MN6 = 1 To ID : MN8# = Fix(MCL#(MN6) / MDEZ#(MN7))
10575:              MN4 = MN6 - 1 : MCL#(MN4) = MCL#(MN4) * MDEZ#(MN9) + MN8# : MCL#(MN6) = MCL#(MN6) - MN8# * MDEZ#(MN7) : Next MN6
10580:          MCL#(ID) = MCL#(ID) * MDEZ#(MN9)
10585:          MN9 = Sign(MCL#(0)) : For MN8 = 1 To ID : If MN9 = Sign(MCL#(MN8)) Or MN9 * Sign(MCL#(MN8)) = 0 Then GoTo 10595
10590:              M7% = 1 : MN7 = MN8 - 1 : MCL#(MN7) = MCL#(MN7) - MN9 : MCL#(MN8) = MCL#(MN8) + MN9 * MDEZ#(14)
10595:          Next MN8
                If M7% = 0 Then
                    For MN9 = 0 To ID : MC1#(MN9) = MCL#(MN9) : Next MN9 : MC2# = MN5# : Return
                Else
                    M7% = 0 : GoTo 10540
                End If
            End If
10610:      MN5# = MA2# : For MN9 = 0 To ID : MCL#(MN9) = MB1#(MN9) : Next MN9

10615:      MN9 = Int(Abs(MA2# - MB2#) / 14)
10620:      MN8 = Abs(MA2# - MB2#) - 14 * MN9 : MN7 = 14 - MN8 : MN6 = ID - MN9 : If MN8 = 0 Then GoTo 10635
10625:      MN4# = 0
            For MN3 = 0 To MN6
                MN2# = Fix(MCL#(MN3) / MDEZ#(MN8))
10630:          MN1# = MCL#(MN3) - MN2# * MDEZ#(MN8) : MCL#(MN3) = MN2# + MN4# * MDEZ#(MN7) : MN4# = MN1#
            Next MN3
10635:      If MA2# <= MB2# Then GoTo 10650
10640:      For MN3 = 0 To MN6 : MN4 = ID - MN3 : MCL#(MN4) = MA1#(MN4) + MCL#(MN4 - MN9) : Next MN3
            If MN9 = 0 Then GoTo 10500
10645:      MN9 = MN9 - 1 : For MN3 = 0 To MN9 : MCL#(MN3) = MA1#(MN3) : Next MN3 : GoTo 10500
10650:      For MN3 = 0 To MN6 : MN4 = ID - MN3 : MCL#(MN4) = MB1#(MN4) + MCL#(MN4 - MN9) : Next MN3 : If MN9 = 0 Then GoTo 10500
10655:      MN9 = MN9 - 1 : For MN3 = 0 To MN9 : MCL#(MN3) = MB1#(MN3) : Next MN3 : GoTo 10500
        End Sub
        ''' <summary> [10660] Routine that divides two numbers in multiple precision 
        ''' INPUT  - MA3#(ID),MA4#,MB3#(ID),MB4#
        ''' OUTPUT - MC3#(ID),MC4#
        ''' </summary>
        Public Shared Sub DIVIDES_TWO_NUMBERS(ByVal MA3#(), ByVal MA4#, ByVal MB3#(), ByVal MB4#, ByRef MC3#(), ByRef MC4#)
            Dim MN8 As Int32, MN9 As Int32, MR#(ID), M1%, MX7#, MA1#(ID), MB1#(ID), MC1#(ID), MA2#, MB2#, MC2#
            Dim MSEP#, MS1#, MS2#
            Dim MX4 As Int32
10674:      If MB3#(0) = 0 Then Throw New Exception("DIVISION BY ZERO IN ROUTINE 10660") : Stop
10675:      If MA3#(0) = 0 Then For MN9 = 0 To ID : MC3#(MN9) = 0 : Next MN9 : MC4# = 0 : Return
10676:      MR#(0) = MDEZ#(13) / MB3#(0)
            If Abs(MR#(0)) = 1 Then
                MR#(0) = Sign(MB3#(0)) * MDEZ#(13) : MX7# = 0
                For MN9 = 1 To ID
                    If MB3#(MN9) <> 0 Then GoTo 10680
                Next MN9
                M1% = Sign(MB3#(0)) : For MN8 = 0 To ID : MC3#(MN8) = M1% * MA3#(MN8) : Next MN8
                MC4# = MA4# - MB4#
                Return
            End If
10677:      MSEP# = MR#(0) * MDEZ#(14)
            BREAKS_INTEGER(MSEP#, MS1#, MS2#) ' 10120
            MR#(0) = MS1# * MDEZ#(7) + MS2# : MX7# = -1
10680:      For MN9 = 1 To ID : MR#(MN9) = 0 : Next MN9

10681:      For MN9 = 0 To ID : MA1#(MN9) = MR#(MN9) : MB1#(MN9) = MB3#(MN9) : Next MN9 : MA2# = MX7# : MB2# = 0
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
10685:      For MN9 = 0 To ID : MA1#(MN9) = 0 : MB1#(MN9) = -MC1#(MN9) : Next MN9 : MA1#(0) = 2 * MDEZ#(13) : MA2# = 0 : MB2# = MC2#
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
10690:      For MN9 = 0 To ID : MA1#(MN9) = MR#(MN9) : MB1#(MN9) = MC1#(MN9) : Next MN9 : MA2# = MX7# : MB2# = MC2#
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) ' 10140
10695:      If ID = 0 Then If Abs(MR#(0) - MC1#(0)) < 3 Then GoTo 10715 Else GoTo 10710
10700:      MX4 = ID - 1
            For MN9 = 0 To MX4
                If MR#(MN9) <> MC1#(MN9) Then GoTo 10710
            Next MN9

10705:      If Abs(MR#(ID) - MC1#(ID)) < 3 Then GoTo 10715
10710:      For MN9 = 0 To ID : MR#(MN9) = MC1#(MN9) : Next MN9 : MX7# = MC2# : GoTo 10681
10715:      If Abs(MA3#(0)) = MDEZ#(13) Then
                For MN9 = 1 To ID
                    If MA3#(MN9) <> 0 Then GoTo 10725
                Next MN9
                M1% = Sign(MA3#(0)) : For MN8 = 0 To ID : MC3#(MN8) = M1% * MR#(MN8) : Next MN8 : MC4# = MX7# + MA4# - MB4# : Return
            End If
10725:      For MN9 = 0 To ID : MA1#(MN9) = MR#(MN9) : MB1#(MN9) = MA3#(MN9) : Next MN9 : MA2# = MX7# : MB2# = MA4#
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) ' 10140
10730:      For MN9 = 0 To ID : MC3#(MN9) = MC1#(MN9) : Next MN9 : MC4# = MC2# - MB4#
        End Sub
        ''' <summary> [10810] Routine that raises a number to an integer power in multiple precision 
        ''' INPUT  - M5%,MA5#(ID),MA6#
        ''' OUTPUT - MC5#(ID),MC6#
        ''' </summary>
        Public Shared Sub RAISES_NUMBER_TO_POWER(ByVal M5%, ByVal MA5#(), ByVal MA6#, ByRef MC5#(), ByRef MC6#)
            Dim MN9 As Int32
            Dim MA1#(ID), MB1#(ID), MC1#(ID), MA3#(ID), MB3#(ID), MC3#(ID)
            Dim MP As UnDefJaggedArray = New UnDefJaggedArray(ID)
            Dim MIP As UnDefArray = New UnDefArray
            Dim MA2#, MB2#, MC2#, M2%
            Dim MB3T#, MC3T#, MA4#, MB4#, MC4#
            Dim MX8 As Int32, MX9 As Int32, MX5 As Int32, MX4 As Int32
10815:      If MA5#(0) <> 0 Then GoTo 10830
10820:      If M5% = 0 Then Throw New Exception("BASIS AND EXPONENT ARE BOTH ZERO IN ROUTINE 10820")
10821:      If M5% < 0 Then Throw New Exception("ZERO RAISED TO NEGATIVE POWER IN ROUTINE 10810") : Stop
10825:      For MN9 = 0 To ID : MC5#(MN9) = 0 : Next MN9 : MC6# = 0 : Return
10830:      If Abs(M5%) > 2047 Then Throw New Exception("EXPONENT GREATER THAN 2047 IN ROUTINE 10820 - USE ROUTINE 11560 INSTEAD")
10835:      If M5% = 0 Then For MN9 = 1 To ID : MC5#(MN9) = 0 : Next MN9 : MC5#(0) = MDEZ#(13) : MC6# = 0 : Return
10840:      If M5% < 0 Then GoTo 10855
10845:      If M5% = 1 Then
                For MN9 = 0 To ID : MC5#(MN9) = MA5#(MN9) : Next MN9 : MC6# = MA6# : Return
            Else
                GoTo 10860
            End If
10855:      If M5% = -1 Then
                For MN9 = 0 To ID : MA3#(MN9) = 0 : MB3#(MN9) = MA5#(MN9) : Next MN9
                MA3#(0) = MDEZ#(13) : MA4# = 0 : MB4# = MA6#
                DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
                For MN9 = 0 To ID : MC5#(MN9) = MC3#(MN9) : Next MN9 : MC6# = MC4# : Return
            End If
10860:      M2% = Abs(M5%)
            For MN9 = 0 To ID : MA1#(MN9) = MA5#(MN9) : MB1#(MN9) = MA5#(MN9) : MP.SetVal(MN9, 0, MA5#(MN9)) : Next MN9
10865:      MIP.SetVal(0, 0) : MX9 = 0 : MX8 = 1 : MA2# = 0 : MB2# = 0
10870:      MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
            For MN9 = 0 To ID : MP.SetVal(MN9, MX8, MC1#(MN9)) : Next MN9 : MIP.SetVal(MX8, MC2#)
10875:      MX4 = 2 ^ MX8 - M2%
            If MX4 < 0 Then
                MX9 = MX9 + 1 : MX8 = MX8 + 1
                If 2 ^ MX8 - M2% > 0 Then
                    GoTo 10895
                Else
                    For MN9 = 0 To ID : MA1#(MN9) = MP.GetVal(MN9, MX9) : MB1#(MN9) = MA1#(MN9) : Next MN9
                    MA2# = MIP.GetVal(MX9) : MB2# = MA2#
                    GoTo 10870
                End If
            End If
10880:      If MX4 > 0 Then GoTo 10895
10885:      If M5% > 0 Then For MN9 = 0 To ID : MC5#(MN9) = MP.GetVal(MN9, MX8) : Next MN9 : MC6# = MIP.GetVal(MX8) + M5% * MA6# : Return
10890:      For MN9 = 0 To ID : MA3#(MN9) = 0 : MB3#(MN9) = MP.GetVal(MN9, MX8) : Next MN9 : MA3#(0) = MDEZ#(13) : MA4# = 0 : MB4# = MIP.GetVal(MX8)
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
            For MN9 = 0 To ID : MC5#(MN9) = MC3#(MN9) : Next MN9 : MC6# = M5% * MA6# : Return
10895:      MX5 = MX8 - 2 : MX9 = 2 ^ (MX5 + 1) : For MN9 = 0 To ID : MA1#(MN9) = MP.GetVal(MN9, MX8 - 1) : Next MN9 : MA2# = MIP.GetVal(MX8 - 1)
10899:      MX4 = 2 ^ MX5 + MX9 - M2%
10900:      If MX4 < 0 Then
                For MN9 = 0 To ID : MB1#(MN9) = MP.GetVal(MN9, MX5) : Next MN9
                MB2# = MIP.GetVal(MX5)
                MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
                For MN9 = 0 To ID : MA1#(MN9) = MC1#(MN9) : Next MN9
                MA2# = MC2# : MX9 = MX9 + 2 ^ MX5 : MX5 = MX5 - 1
                GoTo 10899
            End If
10905:      If MX4 > 0 Then
                MX5 = MX5 - 1
                GoTo 10899
            End If
10910:      For MN9 = 0 To ID : MB1#(MN9) = MP.GetVal(MN9, MX5) : Next MN9 : MB2# = MIP.GetVal(MX5)
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
10915:      If M5% > 0 Then For MN9 = 0 To ID : MC5#(MN9) = MC1#(MN9) : Next MN9 : MC6# = MC2# + M5% * MA6# : Return
10920:      For MN9 = 0 To ID : MA3#(MN9) = 0 : MB3#(MN9) = MC1#(MN9) : Next MN9
            MA3#(0) = MDEZ#(13) : MA4# = 0 : MB4# = MC2#
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
            For MN9 = 0 To ID : MC5#(MN9) = MC3#(MN9) : Next MN9
            MC6# = MC4# + M5% * MA6#
        End Sub
        ''' <summary> [10930] Routine that calculates the square-root in multiple precision 
        ''' INPUT  - MA5#(ID),MA6#
        ''' OUTPUT - MC5#(ID),MC6#
        ''' </summary>
        Public Shared Sub SQUAREROOT(ByVal MA5#(), ByVal MA6#, ByRef MC5#(), ByRef MC6#)
            Dim MN9 As Int32, MK2 As Int32
            Dim MN3#, MA4#, MN5#, MN6#, MIP#(ID), MA3#(ID), MB3#(ID), MC3#(ID)
            Dim MSEP#, MS1#, MS2#
            Dim MB4#, MC3T#, MC4#
            Dim MA1#(ID), MB1#(ID), MC1#(ID), MDL#(ID)
            Dim MA2#, MB2#, MC2#, M1%
            'Dim MB3T#, MC3T#, MA4#, MB4#, MC4#
10935:      MK2 = 14 * (ID + 1) - 2 : If MA5#(0) < 0 Then Throw New Exception("NEGATIVE PARAMETER IN ROUTINE 10930") : Stop
10937:      If MA5#(0) = 0 Then For MN9 = 0 To ID : MC5#(MN9) = 0 : Next MN9 : MC6# = 0 : Return
10940:      MN5# = MA6# / 2
            If Fix(MN5#) <> MN5# Then
                MIP#(0) = Sqrt(MA5#(0)) * MDEZ#(7) : MN3# = (MA6# - 1) / 2
                GoTo 10970
            End If
10945:      If MA5#(0) <> MDEZ#(13) Then GoTo 10965
10950:      MIP#(0) = MA5#(0) : MN3# = MN5#
            For MN9 = 1 To ID
10955:          If MA5#(MN9) <> 0 Then GoTo 10970
10960:      Next MN9
            MC5#(0) = MA5#(0) : For MN9 = 1 To ID : MC5#(MN9) = 0 : Next MN9 : MC6# = MN3# : Return
10965:      MIP#(0) = Sqrt(MA5#(0) * 10) * MDEZ#(6) : MN3# = MN5#
10970:      MSEP# = MIP#(0)
            BREAKS_INTEGER(MSEP#, MS1#, MS2#)   ' 10120
            MIP#(0) = MS1# * MDEZ#(7) + MS2# : For MN9 = 1 To ID : MIP#(MN9) = 0 : Next MN9
10975:      For MN9 = 0 To ID : MA3#(MN9) = MA5#(MN9) : MB3#(MN9) = MIP#(MN9) : Next MN9 : MA4# = MA6# : MB4# = MN3#
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
10980:      For MN9 = 0 To ID : MA1#(MN9) = MC3#(MN9) : MB1#(MN9) = MIP#(MN9) : Next MN9 : MA2# = MC4# : MB2# = MN3#
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) ' 10340
10985:      For MN9 = 0 To ID : MA1#(MN9) = MC1#(MN9) : Next MN9 : MA2# = MC2# : M1% = 5
            MULTIPLICATION_BY_INTEGER(M1%, MA1#, MA2#, MC1#, MC2#)    ' 10040
10990:      For MN9 = 0 To ID : MDL#(MN9) = MC1#(MN9) : Next MN9 : MN6# = MC2# - 1
10995:      For MN9 = 0 To ID : MA1#(MN9) = MDL#(MN9) : MB1#(MN9) = -MIP#(MN9) : Next MN9 : MA2# = MN6# - MN3# : MB2# = 0
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) ' 10340
11000:      For MN9 = 0 To ID : If MC1#(MN9) <> 0 Then GoTo 11015
11005:      Next MN9
11010:      For MN9 = 0 To ID : MC5#(MN9) = MDL#(MN9) : Next MN9 : MC6# = MN6# : Return
11015:      If Abs(MC2#) - MK2 >= 0 Then
                GoTo 11010
            Else
                For MN9 = 0 To ID : MIP#(MN9) = MDL#(MN9) : Next MN9 : MN3# = MN6#
                GoTo 10975
            End If
        End Sub
        ''' <summary> [11020] Routine that calculates the natural logarithm in multiple precision 
        ''' INPUT  - MA7#(ID),MA8#
        ''' OUTPUT - MC7#(ID),MC8#
        ''' </summary>
        Public Shared Sub NATURAL_LOGARITHM(ByVal MA7#(), ByVal MA8#, ByRef MC7#(), ByRef MC8#)
            Dim MA2#, MA4#, MA6#, MB2#, MB4#, MC2#, MC4#, MC6#, MK1#, ML1#, MX8#, MAEX#, MB$, MEX
            Dim MA1#(ID), MA3#(ID), MA5#(ID), MB1#(ID), MB3#(ID), MC1#(ID), MC3#(ID), MC5#(ID), MK#(ID), ML#(ID)
            Dim M1%, M3%, M4%, M5%
            Dim MN9 As Int32, MN8 As Int32
            Dim MLN2#(9), MR22#(9), MLN10#(9)
11025:      MX8# = 0 : If MLN2#(0) = 0 Then NATURAL_LOGARITHM_SQRT(MLN2#, MR22#, MLN10#) '11350
11035:      If Not (MA7#(0) > 0) Then
11040:          Throw New Exception("PARAMETER IS NEGATIVE OR ZERO IN ROUTINE 11020")
            End If
11045:      M3% = 1 : If MA8# <> 0 Then GoTo 11060
11050:      If MA7#(0) > MDEZ#(13) Then GoTo 11065
11051:      For MN9 = 1 To ID
                If MA7#(MN9) = 0 Then
                    GoTo 11055
                Else
                    GoTo 11065
                End If
11055:      Next MN9
            For MN8 = 0 To ID : MC7#(MN8) = 0 : Next MN8 : MC8# = 0
            Return
11060:      If MA8# < 0 Then
                M3% = -1
                For MN9 = 0 To ID
                    MA3#(MN9) = 0 : MB3#(MN9) = MA7#(MN9)
                Next MN9
                MA3#(0) = MDEZ#(13) : MA4# = 0 : MB4# = MA8#
                DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#)
                For MN9 = 0 To ID
                    MK#(MN9) = MA7#(MN9) : MA7#(MN9) = MC3#(MN9)
                Next MN9
                MK1# = MA8# : MA8# = MC4#
            End If
11065:      If MA8# > 35 Then MX8# = MA8# : MA8# = 0
11066:      M4% = Fix(Log(MA7#(0) * 10.0# ^ (MA8# - 13)) / Log(2)) : M5% = M4% + 1
            For MN9 = 1 To ID : MA5#(MN9) = 0 : Next MN9
            MA5#(0) = 2 * MDEZ#(13) : MA6# = 0
            RAISES_NUMBER_TO_POWER(M5%, MA5#, MA6#, MC5#, MC6#)    ' 10810
11070:      If MA8# = MC6# Then GoTo 11085
11075:      If MC6# < MA8# Then GoTo 11115
11080:      GoTo 11120
11085:      For MN9 = 0 To ID
11090:          If MC5#(MN9) = MA7#(MN9) Then GoTo 11110
11095:          If MC5#(MN9) < MA7#(MN9) Then GoTo 11115
11100:          GoTo 11120
11110:      Next MN9
11115:      M4% = M4% + 1 : For MN9 = 0 To ID : MA1#(MN9) = MC5#(MN9) : Next MN9 : M1% = 2 : MA2# = MC6#
            MULTIPLICATION_BY_INTEGER(M1%, MA1#, MA2#, MC1#, MC2#) '10040
            For MN9 = 0 To ID
                MC5#(MN9) = MC1#(MN9)
            Next MN9
            MC6# = MC2#
11120:      M4% = M4% + 1
            For MN9 = 0 To ID
                MA3#(MN9) = MA7#(MN9) : MB3#(MN9) = MC5#(MN9)
            Next MN9
            MA4# = MA8# : MB4# = MC6#
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) '10660
            For MN9 = 0 To ID
                MC7#(MN9) = MC3#(MN9)
            Next MN9
            MC8# = MC4#
11125:      If MC4# >= 0 Then
                Throw New Exception("M GREATER THAN 1 IN ROUTINE 11020 - IM=" + MC4#.ToString())
            End If
11130:      For MN9 = 0 To ID : MA1#(MN9) = MC7#(MN9) : Next MN9
            MA2# = MC8# : M1% = 2
            MULTIPLICATION_BY_INTEGER(M1%, MA1#, MA2#, MC1#, MC2#) '10040
            For MN9 = 0 To ID : ML#(MN9) = MC1#(MN9) : Next MN9
            ML1# = MC2#
11135:      If ML1# < 0 Then
                For MN9 = 0 To ID
                    MC7#(MN9) = ML#(MN9)
                Next MN9
                MC8# = ML1#
            End If
11150:      For MN9 = 0 To ID
                MA3#(MN9) = MC7#(MN9) : MB3#(MN9) = MR22#(MN9)
            Next MN9
            MA4# = MC8# : MB4# = -1
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) '10660
            For MN9 = 0 To ID
                ML#(MN9) = MC3#(MN9)
            Next MN9
            ML1# = MC4#
11200:      If ML1# >= 0 Then GoTo 11240
11205:      For MN9 = 0 To ID : MA1#(MN9) = MC7#(MN9) : Next MN9 : MA2# = MC8# : M1% = 2
            MULTIPLICATION_BY_INTEGER(M1%, MA1#, MA2#, MC1#, MC2#) '10040
            For MN9 = 0 To ID : ML#(MN9) = MC1#(MN9) : Next MN9 : ML1# = MC2#
11210:      If ML1# <> 0 Then GoTo 11230
11215:      If ML#(0) <> MDEZ#(13) Then GoTo 11230
11220:      For MN9 = 1 To ID
                If ML#(MN9) <> 0 Then GoTo 11230
11225:      Next MN9
            ML#(0) = 0
            GoTo 11235
11230:      AUXILIARY_NATURAL_LOGARITHM(ML#, ML1#) '11260
11235:      For MN9 = 0 To ID : MB1#(MN9) = -MLN2#(MN9) : MA1#(MN9) = ML#(MN9) : Next MN9 : MA2# = ML1# : MB2# = -1
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10340
            For MN9 = 0 To ID : ML#(MN9) = MC1#(MN9) : Next MN9 : ML1# = MC2#
            GoTo 11245
11240:      For MN9 = 0 To ID : ML#(MN9) = MC7#(MN9) : Next MN9 : ML1# = MC8#
            AUXILIARY_NATURAL_LOGARITHM(ML#, ML1#) '11260
11245:      MAEX# = M4%
            FURNISHES_EXPONENT(MAEX#, MB$, MEX) ' 10020 
            MA1#(0) = M4% * MDEZ#(13 - MEX) : For MN9 = 1 To ID : MA1#(MN9) = 0 : MB1#(MN9) = MLN2#(MN9) : Next MN9 : MB1#(0) = MLN2#(0) : MA2# = MEX : MB2# = -1
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140 
11250:      For MN9 = 0 To ID : MA1#(MN9) = MC1#(MN9) : MB1#(MN9) = ML#(MN9) : Next MN9 : MA2# = MC2# : MB2# = ML1#
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10340
11255:      For MN9 = 0 To ID : MC7#(MN9) = M3% * MC1#(MN9) : Next MN9 : MC8# = MC2#
            If MX8# = 0 Then
                If M3% = -1 Then
                    For MN8 = 0 To ID : MA7#(MN8) = MK#(MN8) : Next MN8
                    MA8# = MK1#
                    Return
                Else
                    Return
                End If
            End If
11256:      If MLN10#(0) = 0 Then NATURAL_LOGARITHM_SQRT(MLN2#, MR22#, MLN10#) '11350
11257:      For MN9 = 0 To ID : MA1#(MN9) = 0 : MB1#(MN9) = MLN10#(MN9) : Next MN9 : MAEX# = MX8#
            FURNISHES_EXPONENT(MAEX#, MB$, MEX) ' 10020 
            MA1#(0) = MX8# * MDEZ#(13 - MEX) : MA2# = MEX : MB2# = 0
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140 
11258:      For MN9 = 0 To ID : MA1#(MN9) = MC1#(MN9) : MB1#(MN9) = MC7#(MN9) : Next MN9 : MA2# = MC2# : MB2# = MC8#
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10340
11259:      For MN9 = 0 To ID : MC7#(MN9) = M3% * MC1#(MN9) : Next MN9 : MC8# = MC2# : MA8# = MX8#
        End Sub
        ''' <summary> [11360] Routine that calculates the exponential in multiple precision 
        ''' INPUT  - MA5#(ID),MA6#
        ''' OUTPUT - MC5#(ID),MC6#
        ''' </summary>
        Public Shared Sub EXPONENTIAL(ByVal MA5#(), ByVal MA6#, ByRef MC5#(), ByRef MC6#)
            Dim MN4 As Int32, MN5 As Int32, MN6 As Int32, MN7 As Int32, MN8 As Int32, MN9 As Int32, MEX As Int32, MX2 As Int32
            Dim MA2#, MA4#, MB2#, MB4#, MC2#, MC4#, MC8#, MX1#, MX8#, MX9#, M2%, M5%, MAEX#, MSEP#, MS1#, MS2#, MB$
            Dim MA1#(ID), MA3#(ID), MA7#(ID), MB1#(ID), MB3#(ID), MC1#(ID), MC3#(ID), MDL#(ID)
            Dim MLN2#(9), MR22#(9), MLN10#(9), LN2#(ID)
11365:      If MA5#(0) = 0 Then
                For MN9 = 1 To ID
                    MC5#(MN9) = 0
                Next MN9
                MC5#(0) = MDEZ#(13) : MC6# = 0
                Return
            End If
11370:      MX9# = 0 : M5% = 0 : M2% = 0
            If MA5#(0) > 0 Then
                M5% = 1
            Else
                For MN9 = 0 To ID : MA5#(MN9) = -MA5#(MN9) : Next MN9
            End If
11375:      If LN2#(0) = 0 Then NATURAL_LOGARITHM_SQRT(MLN2#, MR22#, MLN10#) '11350
11380:      For MN9 = 0 To ID
                MA3#(MN9) = MA5#(MN9) : MB3#(MN9) = MLN10#(MN9)
            Next MN9
            MA4# = MA6# : MB4# = 0
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
            For MN9 = 0 To ID : MDL#(MN9) = MC3#(MN9) : Next MN9 : MX8# = MC4#
11385:      If MX8# > 0 Then GoTo 11405
11390:      For MN9 = 0 To ID
                MA3#(MN9) = MA5#(MN9) : MB3#(MN9) = MLN2#(MN9)
            Next MN9
            MA4# = MA6# : MB4# = -1
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
            For MN9 = 0 To ID : MDL#(MN9) = MC3#(MN9) : Next MN9 : MX8# = MC4#
11395:      If MX8# >= 0 Then GoTo 11495
11400:      MC8# = 0
            For MN9 = 0 To ID : MA7#(MN9) = MA5#(MN9) : Next MN9 : MX1# = MA6#
            GoTo 11505
11405:      If MX8# > 13 Then
                Throw New Exception("ATTEMPT TO CALCULATE EXPONENTIAL OF A NUMBER GREATER THAN 10^14 IN ROUTINE 11360")
            End If
11410:      MN8 = 13 - MX8# : MX9# = Fix(MDL#(0) / MDEZ#(MN8)) : MA7#(0) = MDL#(0) - MX9# * MDEZ#(MN8)
11415:      For MN9 = 1 To ID : MA7#(MN9) = MDL#(MN9) : Next MN9
11420:      For MN8 = 0 To ID
                If MA7#(MN8) <> 0 Then GoTo 11430
11425:      Next MN8
            For MN9 = 1 To ID : MC5#(MN9) = 0 : Next MN9 : MC5#(0) = MDEZ#(13) : MC6# = 0
            GoTo 11540
11430:      If MN8 <> ID Then GoTo 11445
11435:      MAEX# = MA7#(ID)
            FURNISHES_EXPONENT(MAEX#, MB$, MEX) ' 10020
11440:      MA7#(0) = MA7#(ID) * MDEZ#(13 - MEX)
            For MN9 = 1 To ID : MA7#(MN9) = 0 : Next MN9
            GoTo 11480
11445:      MAEX# = MA7#(MN8)
            FURNISHES_EXPONENT(MAEX#, MB$, MEX) ' 10020
            If MEX = 13 Then GoTo 11470
11450:      MN7 = ID - MN8 - 1 : MN6 = MEX + 1 : MN5 = 13 - MEX
            For MN9 = 0 To MN7
11455:          MN4 = MN9 + MN8 + 1 : MSEP# = MA7#(MN4) / MDEZ#(MN6)
                BREAKS_INTEGER(MSEP#, MS1#, MS2#) '10120
                MA7#(MN4) = MA7#(MN4) - (MS1# * MDEZ#(7) + MS2#) * MDEZ#(MN6) : MA7#(MN9) = MA7#(MN4 - 1) * MDEZ#(MN5) + MS1# * MDEZ#(7) + MS2#
11460:      Next MN9
            MA7#(MN9) = MA7#(MN4) * MDEZ#(MN5)
            If MN8 = 0 Then GoTo 11480
11465:      For MN6 = 1 To MN8 : MN9 = MN9 + 1 : MA7#(MN9) = 0 : Next MN6
            GoTo 11480
11470:      MN7 = ID - MN8 : For MN9 = 0 To MN7 : MA7#(MN9) = MA7#(MN8 + MN9) : Next MN9
11475:      MN5 = MN8 - 1 : For MN6 = 0 To MN5 : MA7#(MN9) = 0 : MN9 = MN9 + 1 : Next MN6
11480:      MX1# = -14 * MN8 + MEX - 13 + MX8#
            If M2% = 1 Then GoTo 11500
11485:      For MN9 = 0 To ID : MA1#(MN9) = MA7#(MN9) : MB1#(MN9) = MLN10#(MN9) : Next MN9 : MA2# = MX1# : MB2# = 0
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
11490:      For MN9 = 0 To ID : MA3#(MN9) = MC1#(MN9) : MB3#(MN9) = MLN2#(MN9) : Next MN9 : MA4# = MC2# : MB4# = -1
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
            For MN9 = 0 To ID : MDL#(MN9) = MC3#(MN9) : Next MN9 : MX8# = MC4#
11495:      M2% = 1 : MN9 = 13 - MX8# : MC8# = Fix(MDL#(0) / MDEZ#(MN9)) : MA7#(0) = MDL#(0) - MC8# * MDEZ#(MN9)
            GoTo 11415
11500:      For MN9 = 0 To ID : MA1#(MN9) = MA7#(MN9) : MB1#(MN9) = MLN2#(MN9) : Next MN9 : MA2# = MX1# : MB2# = -1
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
            For MN9 = 0 To ID : MA7#(MN9) = MC1#(MN9) : Next MN9 : MX1# = MC2#
11505:      MX2 = 1 : For MN9 = 0 To ID : MC5#(MN9) = MA7#(MN9) : MDL#(MN9) = MA7#(MN9) : Next MN9 : MC6# = MX1# : MX8# = MX1#
11510:      For MN9 = 0 To ID : MA1#(MN9) = MDL#(MN9) : MB1#(MN9) = MA7#(MN9) : Next MN9 : MA2# = MX8# : MB2# = MX1#
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
11515:      MX2 = MX2 + 1 : MAEX# = MX2
            FURNISHES_EXPONENT(MAEX#, MB$, MEX) ' 10020
            For MN9 = 0 To ID : MA3#(MN9) = MC1#(MN9) : MB3#(MN9) = 0 : Next MN9 : MB3#(0) = MX2 * MDEZ#(13 - MEX) : MA4# = MC2# : MB4# = MEX
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
            For MN9 = 0 To ID : MA7#(MN9) = MC3#(MN9) : Next MN9 : MX1# = MC4#
11520:      MC4# = MX1# - MC6#
            If Abs(MA7#(0)) < Abs(MC5#(0)) Then MC4# = MC4# - 1
11525:      If MC4# + 14 * (ID + 1) < 0 Then GoTo 11535
11530:      For MN9 = 0 To ID : MA1#(MN9) = MC5#(MN9) : MB1#(MN9) = MA7#(MN9) : Next MN9 : MA2# = MC6# : MB2# = MX1#
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
            For MN9 = 0 To ID : MC5#(MN9) = MC1#(MN9) : Next MN9 : MC6# = MC2#
            GoTo 11510
11535:      For MN9 = 0 To ID : MA1#(MN9) = 0 : MB1#(MN9) = MC5#(MN9) : Next MN9 : MA1#(0) = MDEZ#(13) : MA2# = 0 : MB2# = MC6#
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
            For MN9 = 0 To ID : MC5#(MN9) = MC1#(MN9) : Next MN9 : MC6# = MC2#
11540:      If MC8# = 0 Then GoTo 11550
11545:      MAEX# = 2.0# ^ MC8#
            FURNISHES_EXPONENT(MAEX#, MB$, MEX) ' 10020
            For MN9 = 0 To ID : MA1#(MN9) = 0 : MB1#(MN9) = MC5#(MN9) : Next MN9 : MA1#(0) = MAEX# * MDEZ#(13 - MEX) : MA2# = MEX : MB2# = MC6#
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
            MC6# = MC2# : For MN9 = 0 To ID : MC5#(MN9) = MC1#(MN9) : Next MN9
11550:      MC6# = MC6# + MX9#
            If M5% = 1 Then Return
11555:      For MN9 = 0 To ID
                MA3#(MN9) = 0 : MB3#(MN9) = MC5#(MN9) : MA5#(MN9) = -MA5#(MN9)
            Next MN9 : MA3#(0) = MDEZ#(13) : MA4# = 0 : MB4# = MC6#
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
            For MN9 = 0 To ID : MC5#(MN9) = MC3#(MN9) : Next MN9 : MC6# = MC4#
        End Sub
        ''' <summary> [11560] Routine that calculate powers of a number in multiple precision 
        ''' INPUT  - MA9#(ID),MA10#,MB9#(ID),MB10#
        ''' OUTPUT - MC9#(ID),MC10#
        ''' </summary>
        Public Shared Sub POWERS(ByVal MA9#(), ByVal MA10#, ByVal MB9#(), ByVal MB10#, ByRef MC9#(), ByRef MC10#)
            Dim MN8 As Int32, MN9 As Int32
            Dim MA1#(ID), MB1#(ID), MC1#(ID), MA3#(ID), MB3#(ID), MC3#(ID), MA5#(ID), MC5#(ID), MA7#(ID), MC7#(ID)
            Dim MA2#, MB2#, MC2#, MA4#, MB4#, MC4#, MA6#, MC6#, MA8#, MC8#
11575:      If Abs(MB9#(0)) <> MDEZ#(13) Then GoTo 11595
11580:      If MB10# <> 0 Then
                GoTo 11595
            Else
                For MN9 = 1 To ID
                    If MB9#(MN9) <> 0 Then GoTo 11605
                Next MN9
                If MB9#(0) > 0 Then
                    For MN8 = 0 To ID
                        MC9#(MN8) = MA9#(MN8)
                    Next MN8
                    MC10# = MA10#
                    Return
                End If
            End If
11585:      If MA9#(0) = 0 Then
                Throw New Exception("ZERO RAISED TO -1 IN ROUTINE 11560")
            End If
11590:      For MN9 = 0 To ID : MA3#(MN9) = 0 : MB3#(MN9) = MA9#(MN9) : Next MN9 : MA3#(0) = MDEZ#(13) : MA4# = 0 : MB4# = MA10#
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
            For MN8 = 0 To ID : MC9#(MN8) = MC3#(MN8) : Next MN8 : MC10# = MC4#
            Return
11595:      If MB9#(0) <> 0 Then GoTo 11605
11600:      If MA9#(0) = 0 Then
                Throw New Exception("ZERO RAISED TO ZERO IN ROUTINE 11560")
            Else
                For MN9 = 1 To ID : MC9#(MN9) = 0 : Next MN9 : MC9#(0) = MDEZ#(13) : MC10# = 0
                Return
            End If
11605:      If MA9#(0) = 0 Then
                If MB9#(0) < 0 Then
                    Throw New Exception("ZERO RAISED TO NEGATIVE POWER IN ROUTINE 11560")
                Else
                    For MN9 = 0 To ID : MC9#(MN9) = 0 : Next MN9 : MC10# = 0
                    Return
                End If
            End If
11610:      If MA9#(0) < 0 Then
                Throw New Exception("NEGATIVE BASIS IN ROUTINE 11560")
            End If
11620:      For MN9 = 0 To ID : MA7#(MN9) = MA9#(MN9) : Next MN9 : MA8# = MA10#
            NATURAL_LOGARITHM(MA7#, MA8#, MC7#, MC8#) '11020
            For MN9 = 0 To ID : MA1#(MN9) = MC7#(MN9) : MB1#(MN9) = MB9#(MN9) : Next MN9 : MA2# = MC8# : MB2# = MB10#
11625:      MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
            For MN9 = 0 To ID : MA5#(MN9) = MC1#(MN9) : Next MN9 : MA6# = MC2#
11630:      EXPONENTIAL(MA5#, MA6#, MC5#, MC6#) '11360
            For MN9 = 0 To ID : MC9#(MN9) = MC5#(MN9) : Next MN9 : MC10# = MC6#
        End Sub
        ''' <summary> [11640] Routine that calculates the decimal logarithm 
        ''' INPUT  - MA7#(ID),MA8#
        ''' OUTPUT - MC7#(ID),MC8#
        ''' </summary>
        Public Shared Sub DECIMAL_LOGARITHM(ByVal MA7#(), ByVal MA8#, ByRef MC7#(), ByRef MC8#)
            Dim MN9 As Int32
            Dim MAEX#, MB$, MEX
            Dim MA3#(ID), MA4#, MB3#(ID), MB4#, MC3#(ID), MC4#
            Dim MLN2#(9), MR22#(9), MLN10#(9)
11643:      If MLN10#(0) = 0 Then NATURAL_LOGARITHM_SQRT(MLN2#, MR22#, MLN10#) '11350
11644:      If MA7#(0) <> MDEZ#(13) Then GoTo 11647
11645:      For MN9 = 1 To ID
                If MA7#(MN9) <> 0 Then GoTo 11647
11646:      Next MN9
            MAEX# = MA8#
            FURNISHES_EXPONENT(MAEX#, MB$, MEX) ' 10020
            MC7#(0) = MA8# * MDEZ#(13 - MEX)
            For MN9 = 1 To ID : MC7#(MN9) = 0 : Next MN9 : MC8# = MEX
            Return
11647:      NATURAL_LOGARITHM(MA7#, MA8#, MC7#, MC8#) '11020
            For MN9 = 0 To ID
                MA3#(MN9) = MC7#(MN9) : MB3#(MN9) = MLN10#(MN9)
            Next MN9 : MA4# = MC8# : MB4# = 0
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
11648:      For MN9 = 0 To ID : MC7#(MN9) = MC3#(MN9) : Next MN9 : MC8# = MC4#
        End Sub
        ''' <summary> [11650] Routine that calculates the sine in multiple precision 
        ''' INPUT  - MA5#(ID),MA6#
        ''' OUTPUT - MC5#(ID),MC6#
        ''' </summary>
        Public Shared Sub SINE(ByVal MA5#(), ByVal MA6#, ByRef MC5#(), ByRef MC6#, ByRef MT%)
            Dim M1%, M2%, M3%, M5%
            Dim MN9 As Int32, MN8 As Int32, MN4 As Int32, MN3 As Int32, MN2 As Int32, MN1 As Int32
            Dim MA1#(ID), MA2#, MB1#(ID), MB2#, MC1#(ID), MC2#, MA3#(ID), MA4#, MB3#(ID), MB4#, MC3#(ID), MC4#, MA7#(ID), MIP#(ID), MDL#(ID)
            Dim MAEX#, MB$, MEX, MN5#, MN7#, MX8#
            Dim MPI#(9), MPI2#(9), MPIM#(9)
            Dim MX1#, MX7#, MX9#
11653:      M2% = 1 : M3% = 0 : If MPI#(0) = 0 Then VALUES_OF_PI(MPI#, MPI2#, MPIM#) ' 11805
11654:      If MA5#(0) = 0 Then For MN9 = 0 To ID : MC5#(MN9) = 0 : Next MN9 : MC6# = 0 : Return
11655:      If MA5#(0) < 0 Then M2% = -1 : For MN9 = 0 To ID : MA5#(MN9) = -MA5#(MN9) : Next MN9
11660:      For MN9 = 0 To ID : MA3#(MN9) = MA5#(MN9) : MB3#(MN9) = MPI2#(MN9) : Next MN9 : MA4# = MA6# : MB4# = 0
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
11665:      If MC4# < 0 Then
                For MN9 = 0 To ID : MC1#(MN9) = MA5#(MN9) : Next MN9 : MC2# = MA6#
                GoTo 11710
            End If
11670:      If MC4# >= (ID + 1) * 14 - 1 Then
                Throw New Exception("INCREASE ID - WITH PRESENT PRECISION IT IS IMPOSSIBLE TO CALCULATE SINE")
            End If
11675:      MN1 = Fix((MC4# + 1) / 14)
            If MN1 <> (MC4# + 1) / 14 Then GoTo 11690
11680:      For MN9 = MN1 To ID
                If MC3#(MN9) <> 0 Then
                    MN7# = MC3#(MN9) : MN1 = MN9
                    'Throw New Exception("is this really GoTo 10695??????")
                    'GoTo 10695 replaced with GoTo 11695
                    GoTo 11695
                End If
11685:      Next MN9
            For MN7 = 0 To ID : MC5#(MN7) = 0 : MA5#(MN9) = M2% * MA5#(MN9) : Next MN7 : MC6# = 0
            Return
11690:      MN2 = 13 - (MC4# - MN1 * 14) : MN7# = MC3#(MN1) - (Fix(MC3#(MN1) / MDEZ#(MN2))) * MDEZ#(MN2)
            If MN7# = 0 Then
                MN1 = MN1 + 1
                GoTo 11680
            End If
11695:      MN3 = MN1 : MN8 = ID - MN1 - 1 : MAEX# = MN7#
            FURNISHES_EXPONENT(MAEX#, MB$, MEX) ' 10020
            MA2# = MEX - MN2 : MN2 = MEX + 1 : MN4 = 14 - MN2
            If MN8 < 0 Then
                MN9 = 0
                GoTo 11700
            Else
                For MN9 = 0 To MN8
                    MN3 = MN3 + 1 : MN5# = Fix(MC3#(MN3) / MDEZ#(MN2)) : MA1#(MN9) = MN7# * MDEZ#(MN4) + MN5# : MN7# = MC3#(MN3) - MN5# * MDEZ#(MN2)
                Next MN9
            End If
11700:      MA1#(MN9) = MN7# * MDEZ#(MN4)
            If MN9 <> ID Then
                MN9 = MN9 + 1
                For MN8 = MN9 To ID : MA1#(MN8) = 0 : Next MN8
            End If
11705:      MB2# = 0
            For MN9 = 0 To ID : MB1#(MN9) = MPI2#(MN9) : Next MN9
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
11710:      For MN9 = 0 To ID : MA1#(MN9) = MC1#(MN9) : MB1#(MN9) = -MPI#(MN9) : Next MN9 : MA2# = MC2# : MB2# = 0
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
11715:      If MC1#(0) = 0 Then
                For MN9 = 0 To ID
                    MC5#(MN9) = 0 : MA5#(MN9) = M2% * MA5#(MN9)
                Next MN9
                MC6# = 0
                Return
            End If
11720:      If MC1#(0) > 0 Then
                M5% = -1
                For MN9 = 0 To ID
                    MA1#(MN9) = MC1#(MN9)
                Next MN9
                MA2# = MC2#
                GoTo 11730
            End If
11725:      M5% = 1
11730:      MT% = M2% : For MN9 = 0 To ID : MB1#(MN9) = -MPIM#(MN9) : Next MN9 : MB2# = 0
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
11740:      If MC1#(0) = 0 Then
                For MN9 = 0 To ID : MC5#(MN9) = 0 : MA5#(MN9) = M2% * MA5#(MN9) : Next MN9
                MC5#(0) = M2% * M5% * MDEZ#(13) : MC6# = 0
                Return
            End If
11745:      If MC1#(0) > 0 Then
                MT% = -M2%
                For MN9 = 0 To ID : MB1#(MN9) = -MA1#(MN9) : MA1#(MN9) = MPI#(MN9) : Next MN9
                MB2# = MA2# : MA2# = 0
                ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
                For MN9 = 0 To ID : MA1#(MN9) = MC1#(MN9) : Next MN9 : MA2# = MC2#
            End If
11750:      For MN9 = 0 To ID : MB1#(MN9) = MA1#(MN9) : Next MN9 : MB2# = MA2#
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
11755:      For MN9 = 0 To ID : MIP#(MN9) = MC1#(MN9) : MDL#(MN9) = MA1#(MN9) : MA7#(MN9) = MA1#(MN9) : Next MN9 : MX8# = MC2# : MX9# = MA2# : MX1# = MA2#
11760:      M3% = M3% + 1 : MAEX# = (M3% + M3%) * (M3% + M3% + 1)
            FURNISHES_EXPONENT(MAEX#, MB$, MEX) ' 10020
11765:      For MN9 = 0 To ID : MA1#(MN9) = MIP#(MN9) : MB1#(MN9) = -MA7#(MN9) : Next MN9 : MA2# = MX8# : MB2# = MX1#
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
11770:      For MN9 = 0 To ID : MA3#(MN9) = MC1#(MN9) : MB3#(MN9) = 0 : Next MN9 : MB3#(0) = MAEX# * MDEZ#(13 - MEX) : MA4# = MC2# : MB4# = MEX
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
            For MN9 = 0 To ID : MA7#(MN9) = MC3#(MN9) : Next MN9
            MX1# = MC4#
11775:      MX7# = MX1# - MX9#
            For MN9 = 0 To ID
                If Abs(MA7#(MN9)) > Abs(MDL#(MN9)) Then GoTo 11790
11780:          If Abs(MA7#(MN9)) < Abs(MDL#(MN9)) Then MX7# = MX7# - 1 : GoTo 11790
11785:      Next MN9
11790:      If MX7# + 14 * (ID + 1) < 0 Then GoTo 11800
11795:      For MN9 = 0 To ID : MA1#(MN9) = MDL#(MN9) : MB1#(MN9) = MA7#(MN9) : Next MN9 : MA2# = MX9# : MB2# = MX1#
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
            For MN9 = 0 To ID : MDL#(MN9) = MC1#(MN9) : Next MN9 : MX9# = MC2#
            GoTo 11760
11800:      M1% = M2% * M5% : For MN9 = 0 To ID : MC5#(MN9) = M1% * MDL#(MN9) : MA5#(MN9) = M2% * MA5#(MN9) : Next MN9 : MC6# = MX9#
        End Sub
        ''' <summary> [11820] Routine that calculates cosine in multiple precision 
        ''' INPUT  - MA9#(ID),MA10#
        ''' OUTPUT - MC9#(ID),MC10#
        ''' </summary>
        Public Shared Sub COSINE(ByVal MA9#(), ByVal MA10#, ByRef MC9#(), ByRef MC10#)
            Dim MN9 As Int32
            Dim MA1#(ID), MA2#, MB1#(ID), MB2#, MC1#(ID), MC2#, MA5#(ID), MA6#, MC5#(ID), MC6#
            Dim MT%
            Dim MPI#(9), MPI2#(9), MPIM#(9)
            If MA9#(0) = 0 Then
                For MN9 = 1 To ID : MC9#(MN9) = 0 : Next MN9 : MC9#(0) = MDEZ#(13) : MC10# = 0
                Return
            End If
            If MPI#(0) = 0 Then VALUES_OF_PI(MPI#, MPI2#, MPIM#) ' 11805
            For MN9 = 0 To ID : MA1#(MN9) = MA9#(MN9) : MB1#(MN9) = MPIM#(MN9) : Next MN9 : MA2# = MA10# : MB2# = 0
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
            For MN9 = 0 To ID : MA5#(MN9) = MC1#(MN9) : Next MN9 : MA6# = MC2#
            SINE(MA5#, MA6#, MC5#, MC6#, MT%) '11650
            For MN9 = 0 To ID : MC9#(MN9) = MC5#(MN9) : Next MN9 : MC10# = MC6#
        End Sub
        ''' <summary> [11850] Routine that calculates tangent in multiple precision 
        ''' INPUT  - MA9#(ID),MA10#
        ''' OUTPUT - MC9#(ID),MC10#
        ''' </summary>
        Public Shared Sub TANGENT(ByVal MA9#(), ByVal MA10#, ByRef MC9#(), ByRef MC10#)
            Dim MN9 As Int32
            Dim MT%, MS#, ML#(ID)
            Dim MA1#(ID), MA2#, MB1#(ID), MB2#, MC1#(ID), MC2#, MA3#(ID), MA4#, MB3#(ID), MB4#, MC3#(ID), MC4#, MA5#(ID), MA6#, MC5#(ID), MC6#
            For MN9 = 0 To ID : MA5#(MN9) = MA9#(MN9) : Next MN9 : MA6# = MA10#
            SINE(MA5#, MA6#, MC5#, MC6#, MT%) '11650
            If MC5#(0) = 0 Then
                For MN9 = 0 To ID : MC9#(MN9) = 0 : Next MN9 : MC10# = 0
                Return
            End If
            If MC6# = 0 Then
                If Abs(MC5#(0)) = MDEZ#(13) Then
                    For MN9 = 1 To ID
                        If Not (MC5#(MN9) = 0) Then GoTo 11856
                    Next MN9
                    Throw New Exception("TANGENT IN INFINITE")
                End If
            End If
11856:      For MN9 = 0 To ID : ML#(MN9) = MC5#(MN9) : MA1#(MN9) = MC5#(MN9) : MB1#(MN9) = MC5#(MN9) : Next MN9 : MS# = MC6# : MB2# = MC6# : MA2# = MC6#
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
            For MN9 = 0 To ID : MA1#(MN9) = 0 : MB1#(MN9) = -MC1#(MN9) : Next MN9 : MA1#(0) = MDEZ#(13) : MA2# = 0 : MB2# = MC2#
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
            For MN9 = 0 To ID : MA5#(MN9) = MC1#(MN9) : Next MN9 : MA6# = MC2#
            SQUAREROOT(MA5#, MA6#, MC5#, MC6#) '10930
            For MN9 = 0 To ID : MA3#(MN9) = ML#(MN9) : MB3#(MN9) = MC5#(MN9) : Next MN9 : MA4# = MS# : MB4# = MC6#
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
            For MN9 = 0 To ID : MC9#(MN9) = MT% * Abs(MC3#(MN9)) : Next MN9 : MC10# = MC4#
        End Sub
        ''' <summary> [11870] Routine that calculates arc tangent in multiple precision 
        ''' INPUT  - MA7#(ID),MA8#
        ''' OUTPUT - MC7#(ID),MC8#
        ''' </summary>
        Public Shared Sub ARCTANGENT(ByVal MA7#(), ByVal MA8#, ByRef MC7#(), ByRef MC8#)
            Dim MN9 As Int32
            Dim MA1#(ID), MA2#, MB1#(ID), MB2#, MC1#(ID), MC2#, MA3#(ID), MA4#, MB3#(ID), MB4#, MC3#(ID), MC4#
            Dim M2%, M3%, M5%, MX6#, MX8#, MX9#, MN#(ID), MN8#, MX1#
            Dim MAEX#, MB$, MEX, ML#(ID), MK#(ID)
            Dim MPI#(9), MPI2#(9), MPIM#(9)
            Dim MTP20#(10), MT3P20#(10), MTP5#(10), MTP10#(10)
11875:      If MA7#(0) = 0 Then For MN9 = 0 To ID : MC7#(MN9) = 0 : Next MN9 : MC8# = 0 : Return
11880:      For MN9 = 0 To ID : MK#(MN9) = Abs(MA7#(MN9)) : Next MN9 : MX9# = MA8#
            If MA7#(0) < 0 Then
                M2% = -1
            Else
                M2% = 1
            End If
11883:      If MT3P20#(0) = 0 Then VALUES_OF_TAN(MTP20#, MT3P20#, MTP5#, MTP10#) ' 12035
11885:      M3% = 0
            If MPI#(0) = 0 Then VALUES_OF_PI(MPI#, MPI2#, MPIM#) ' 11805
11886:      If MA8# < 0 Then GoTo 11900
11887:      If MA8# > 0 Then GoTo 11895
11890:      If MK#(0) = MDEZ#(13) Then
                For MN9 = 1 To ID
                    If MK#(MN9) <> 0 Then GoTo 11895
                Next MN9
                GoTo 11915
            End If
11895:      M3% = 1 : For MN9 = 0 To ID : MA3#(MN9) = 0 : MB3#(MN9) = MK#(MN9) : Next MN9 : MA3#(0) = MDEZ#(13) : MA4# = 0 : MB4# = MX9#
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
            For MN9 = 0 To ID : MK#(MN9) = MC3#(MN9) : Next MN9 : MX9# = MC4#
11900:      M5% = 1
            If MA8# < -1 Then GoTo 11980
11905:      For MN9 = 0 To ID
                If MK#(MN9) > MT3P20#(MN9) Then GoTo 11915
11906:          If MK#(MN9) < MT3P20#(MN9) Then GoTo 11940
11910:      Next MN9
            GoTo 11940
11915:      M5% = 2 : For MN9 = 0 To ID : MA1#(MN9) = MK#(MN9) : MB1#(MN9) = MTP5#(MN9) : Next MN9 : MA2# = MX9# : MB2# = -1
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
11920:      For MN9 = 0 To ID : MA1#(MN9) = 0 : MB1#(MN9) = MC1#(MN9) : Next MN9 : MA1#(0) = MDEZ#(13) : MA2# = 0 : MB2# = MC2#
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
11925:      For MN9 = 0 To ID : MB3#(MN9) = MC1#(MN9) : Next MN9 : MB4# = MC2#
11930:      For MN9 = 0 To ID : MA1#(MN9) = MK#(MN9) : MB1#(MN9) = -MTP5#(MN9) : Next MN9 : MA2# = MX9# : MB2# = -1
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
            If MC1#(0) = 0 Then
                For MN9 = 0 To ID : ML#(MN9) = 0 : Next MN9
                MX8# = 0
                GoTo 12010
            End If
11935:      For MN9 = 0 To ID : MA3#(MN9) = MC1#(MN9) : Next MN9 : MA4# = MC2#
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
            For MN9 = 0 To ID : MK#(MN9) = MC3#(MN9) : Next MN9 : MX9# = MC4#
            GoTo 11980
11940:      For MN9 = 0 To ID
                If MK#(MN9) > MTP20#(MN9) Then GoTo 11955
11945:          If MK#(MN9) < MTP20#(MN9) Then GoTo 11980
11950:      Next MN9
            GoTo 11980
11955:      M5% = 3 : For MN9 = 0 To ID : MA1#(MN9) = MK#(MN9) : MB1#(MN9) = MTP10#(MN9) : Next MN9 : MA2# = MX9# : MB2# = -1
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
11960:      For MN9 = 0 To ID : MA1#(MN9) = 0 : MB1#(MN9) = MC1#(MN9) : Next MN9 : MA1#(0) = MDEZ#(13) : MA2# = 0 : MB2# = MC2#
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
11965:      For MN9 = 0 To ID : MB3#(MN9) = MC1#(MN9) : Next MN9 : MB4# = MC2#
11970:      For MN9 = 0 To ID : MA1#(MN9) = MK#(MN9) : MB1#(MN9) = -MTP10#(MN9) : Next MN9 : MA2# = MX9# : MB2# = -1
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
            If MC1#(0) = 0 Then
                For MN9 = 0 To ID : ML#(MN9) = 0 : Next MN9
                MX8# = 0
                GoTo 12010
            End If
11975:      For MN9 = 0 To ID : MA3#(MN9) = MC1#(MN9) : Next MN9 : MA4# = MC2#
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
            For MN9 = 0 To ID : MK#(MN9) = MC3#(MN9) : Next MN9 : MX9# = MC4#
11980:      For MN9 = 0 To ID : ML#(MN9) = MK#(MN9) : MN#(MN9) = MK#(MN9) : MA1#(MN9) = MK#(MN9) : MB1#(MN9) = MK#(MN9) : Next MN9 : MX8# = MX9# : MA2# = MX9# : MB2# = MX9# : MX1# = MX9#
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
            For MN9 = 0 To ID : MK#(MN9) = MC1#(MN9) : Next MN9 : MX9# = MC2# : MX6# = 0
11985:      MX6# = MX6# + 1 : For MN9 = 0 To ID : MA1#(MN9) = -MN#(MN9) : MB1#(MN9) = MK#(MN9) : Next MN9 : MA2# = MX1# : MB2# = MX9#
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
            For MN9 = 0 To ID : MN#(MN9) = MC1#(MN9) : Next MN9 : MX1# = MC2#
11990:      MAEX# = MX6# + MX6# + 1
            FURNISHES_EXPONENT(MAEX#, MB$, MEX) ' 10020
            For MN9 = 0 To ID : MA3#(MN9) = MN#(MN9) : MB3#(MN9) = 0 : Next MN9 : MB3#(0) = MAEX# * MDEZ#(13 - MEX) : MB4# = MEX : MA4# = MX1#
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
11995:      MN8# = MC4# - MX8# : If Abs(MC3#(0)) < Abs(ML#(0)) Then MN8# = MN8# - 1
12000:      If MN8# + 14 * (ID + 1) <= 0 Then GoTo 12010
12005:      For MN9 = 0 To ID : MA1#(MN9) = ML#(MN9) : MB1#(MN9) = MC3#(MN9) : Next MN9 : MA2# = MX8# : MB2# = MC4#
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
            For MN9 = 0 To ID : ML#(MN9) = MC1#(MN9) : Next MN9 : MX8# = MC2#
            GoTo 11985
12010:      If M5% = 1 Then GoTo 12025
            If M5% = 2 Then GoTo 12015
            If M5% = 3 Then GoTo 12020
12015:      For MN9 = 0 To ID : MA1#(MN9) = MPI2#(MN9) : MB1#(MN9) = ML#(MN9) : Next MN9 : MA2# = -1 : MB2# = MX8#
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
            For MN9 = 0 To ID : ML#(MN9) = MC1#(MN9) : Next MN9 : MX8# = MC2#
            GoTo 12025
12020:      For MN9 = 0 To ID : MA1#(MN9) = MPI#(MN9) : MB1#(MN9) = ML#(MN9) : Next MN9 : MA2# = -1 : MB2# = MX8#
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
            For MN9 = 0 To ID : ML#(MN9) = MC1#(MN9) : Next MN9 : MX8# = MC2#
12025:      If M3% = 1 Then For MN9 = 0 To ID : MA1#(MN9) = MPIM#(MN9) : MB1#(MN9) = -ML#(MN9) : Next MN9 : MA2# = 0 : MB2# = MX8#
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
            For MN9 = 0 To ID : ML#(MN9) = MC1#(MN9) : Next MN9 : MX8# = MC2#
12030:      For MN9 = 0 To ID : MC7#(MN9) = M2% * ML#(MN9) : Next MN9 : MC8# = MX8#
        End Sub
        ''' <summary> [12060] Routine that calculates arc sine in multiple precision 
        ''' INPUT  - MA9#(ID),MA10#
        ''' OUTPUT - MC9#(ID),MC10#
        ''' </summary>
        Public Shared Sub ARCSINE(ByVal MA9#(), ByVal MA10#, ByRef MC9#(), ByRef MC10#)
            Dim MN9 As Int32
            Dim M1%
            Dim MA1#(ID), MB1#(ID), MC1#(ID), MA3#(ID), MB3#(ID), MC3#(ID), MA5#(ID), MC5#(ID), MA7#(ID), MC7#(ID)
            Dim MA2#, MB2#, MC2#, MA4#, MB4#, MC4#, MA6#, MC6#, MA8#, MC8#
            Dim MPI#(9), MPI2#(9), MPIM#(9)
12065:      If MA9#(0) = 0 Then
                For MN9 = 0 To ID : MC9#(MN9) = 0 : Next MN9 : MC10# = 0
                Return
            End If
12070:      If MA10# > 0 Then GoTo 12120
12075:      If MA10# < 0 Then GoTo 12090
12080:      If Abs(MA9#(0)) > MDEZ#(13) Then GoTo 12120
12085:      For MN9 = 1 To ID
                If MA9#(MN9) <> 0 Then GoTo 12120
            Next MN9
            M1% = Sign(MA9#(0)) : VALUES_OF_PI(MPI#, MPI2#, MPIM#) ' 11805
            For MN9 = 0 To ID : MC9#(MN9) = M1% * MPIM#(MN9) : Next MN9 : MC10# = 0
            Return
12090:      For MN9 = 0 To ID : MA1#(MN9) = MA9#(MN9) : MB1#(MN9) = MA9#(MN9) : Next MN9 : MA2# = MA10# : MB2# = MA10#
            MULTIPLIES_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#) '10140
12095:      For MN9 = 0 To ID : MA1#(MN9) = 0 : MB1#(MN9) = -MC1#(MN9) : Next MN9 : MA2# = 0 : MA1#(0) = MDEZ#(13) : MB2# = MC2#
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
12100:      For MN9 = 0 To ID : MA5#(MN9) = MC1#(MN9) : Next MN9 : MA6# = MC2#
            SQUAREROOT(MA5#, MA6#, MC5#, MC6#) '10930
12105:      For MN9 = 0 To ID : MA3#(MN9) = MA9#(MN9) : MB3#(MN9) = MC5#(MN9) : Next MN9 : MA4# = MA10# : MB4# = MC6#
            DIVIDES_TWO_NUMBERS(MA3#, MA4#, MB3#, MB4#, MC3#, MC4#) ' 10660
12110:      For MN9 = 0 To ID : MA7#(MN9) = MC3#(MN9) : Next MN9 : MA8# = MC4#
            ARCTANGENT(MA7#, MA8#, MC7#, MC8#) '11870
12115:      For MN9 = 0 To ID : MC9#(MN9) = MC7#(MN9) : Next MN9 : MC10# = MC8# : Return
12120:      Throw New Exception("PARAMETER GREATER THAN 1 IN ROUTINE 12060")
        End Sub
        ''' <summary> [12130] Routine that calculates arc cosine in multiple precision 
        ''' INPUT  - MA9#(ID),MA10#
        ''' OUTPUT - MC9#(ID),MC10#
        ''' </summary>
        Public Shared Sub ARCCOSINE(ByVal MA9#(), ByVal MA10#, ByRef MC9#(), ByRef MC10#)
            Dim MN9 As Int32
            Dim MA1#(ID), MB1#(ID), MC1#(ID)
            Dim MA2#, MB2#, MC2#
            Dim MPI#(9), MPI2#(9), MPIM#(9)
12135:      If MA9#(0) = 0 Then
                VALUES_OF_PI(MPI#, MPI2#, MPIM#) ' 11805
                For MN9 = 0 To ID : MC9#(MN9) = MPIM#(MN9) : Next MN9 : MC10# = 0
                Return
            End If
12140:      If MA10# < 0 Then GoTo 12170
12145:      If MA10# > 0 Then GoTo 12180
12150:      If Abs(MA9#(0)) > MDEZ#(13) Then GoTo 12180
12155:      For MN9 = 1 To ID
                If MA9#(MN9) <> 0 Then GoTo 12180
            Next MN9
12160:      If MA9#(0) > 0 Then
                For MN9 = 0 To ID : MC9#(MN9) = 0 : Next MN9 : MC10# = 0
                Return
            End If
12165:      VALUES_OF_PI(MPI#, MPI2#, MPIM#) ' 11805
            For MN9 = 0 To ID : MC9#(MN9) = MPI#(MN9) : Next MN9 : MC10# = 0
            Return
12170:      ARCSINE(MA9#, MA10#, MC9#, MC10#) ' 12060
            VALUES_OF_PI(MPI#, MPI2#, MPIM#) ' 11805
            For MN9 = 0 To ID : MA1#(MN9) = MPIM#(MN9) : MB1#(MN9) = -MC9#(MN9) : Next MN9 : MA2# = 0 : MB2# = MC10#
            ADDS_TWO_NUMBERS(MA1#, MA2#, MB1#, MB2#, MC1#, MC2#)  '10340
12175:      For MN9 = 0 To ID : MC9#(MN9) = MC1#(MN9) : Next MN9 : MC10# = MC2#
            Return
12180:      Throw New Exception("PARAMETER GREATER THAN 1 IN ROUTINE 12130")
        End Sub

    End Class
End Namespace
