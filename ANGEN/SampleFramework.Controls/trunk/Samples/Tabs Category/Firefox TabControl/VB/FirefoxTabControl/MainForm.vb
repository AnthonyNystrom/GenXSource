Imports FirefoxTabControl.My.Resources
Imports System.Globalization

Public Class MainForm
    Private Sub AddTabPage()
        _tabPageCount += 1
        _tabControl.TabPages.Add(String.Format(CultureInfo.CurrentUICulture, "Tab {0}", _tabPageCount)).TabButtonImage = Resources.Blank
    End Sub

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.AddTabPage()
    End Sub

    Private Sub _addTabButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _addTabButton.Click
        Me.AddTabPage()
    End Sub

    Dim _tabPageCount As Int32

    Sub New()
        Me.InitializeComponent()
    End Sub
End Class
