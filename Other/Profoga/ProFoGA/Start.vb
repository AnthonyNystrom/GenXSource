Public Class Start

    Private Sub lbAminoacids_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbAminoacids.DoubleClick
        lbProtein.Items.Add(lbAminoacids.SelectedItem)
    End Sub

    Private Sub lbProtein_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbProtein.DoubleClick
        If lbProtein.SelectedIndex >= 0 Then
            lbProtein.Items.Remove(lbProtein.SelectedItem)
        End If
    End Sub

    Private Function AminoacidNames() As String()
        Dim AminoacidList As List(Of String)
        AminoacidList = New List(Of String)
        For Each amino As String In lbProtein.Items
            AminoacidList.Add(amino)
        Next
        Return AminoacidList.ToArray
    End Function

    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click
        ProtCalc = New Fitness.Calculator
        SaveProtein()
        Base.ProtCalc.ProteinAminoacidNames = AminoacidNames()
        My.Forms.Progress.InitializePopulation(lbProtein.Items.Count * 8, Me.numPopulation.Value)
        My.Forms.Progress.Show()
    End Sub

    Private Sub Start_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lbProtein.Items.Clear()
        lbProtein.Items.AddRange(My.Settings.Protein.Split("-"))
    End Sub

    Sub SaveProtein()
        My.Settings.Protein = Join(AminoacidNames, "-")
        My.Settings.Save()
    End Sub

    Private Sub Start_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        SaveProtein()
    End Sub
End Class