Imports Genetibase.UI
Imports System.IO
Imports System.Threading

Public NotInheritable Class frmVids
    Inherits System.Windows.Forms.Form

    Dim testTime As New DateTime(1, 1, 1, 0, 0, 0)

    Private LastSelectedCSI As CShItem
    Private Shared Event1 As New ManualResetEvent(True)

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'Add any initialization after the InitializeComponent() call
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)

        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If

        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents panelEx1 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents lv1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeaderName As System.Windows.Forms.ColumnHeader
    Friend WithEvents expTree1 As ExpTree
    Friend WithEvents uIButton1 As Janus.Windows.EditControls.UIButton
    Friend WithEvents panelEx3 As DevComponents.DotNetBar.PanelEx
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.panelEx1 = New DevComponents.DotNetBar.PanelEx
        Me.panelEx3 = New DevComponents.DotNetBar.PanelEx
        Me.uIButton1 = New Janus.Windows.EditControls.UIButton
        Me.expTree1 = New ExpTree
        Me.lv1 = New System.Windows.Forms.ListView
        Me.ColumnHeaderName = New System.Windows.Forms.ColumnHeader
        Me.panelEx1.SuspendLayout()
        Me.SuspendLayout()
        '
        'panelEx1
        '
        Me.panelEx1.AntiAlias = True
        Me.panelEx1.Controls.Add(Me.panelEx3)
        Me.panelEx1.Controls.Add(Me.uIButton1)
        Me.panelEx1.Controls.Add(Me.expTree1)
        Me.panelEx1.Controls.Add(Me.lv1)
        Me.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelEx1.Location = New System.Drawing.Point(0, 0)
        Me.panelEx1.Name = "panelEx1"
        Me.panelEx1.Size = New System.Drawing.Size(798, 568)
        Me.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
        Me.panelEx1.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile
        Me.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.panelEx1.Style.GradientAngle = 90
        Me.panelEx1.TabIndex = 0
        '
        'panelEx3
        '
        Me.panelEx3.AntiAlias = True
        Me.panelEx3.Location = New System.Drawing.Point(8, 348)
        Me.panelEx3.Name = "panelEx3"
        Me.panelEx3.Size = New System.Drawing.Size(780, 188)
        Me.panelEx3.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.panelEx3.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.panelEx3.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
        Me.panelEx3.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile
        Me.panelEx3.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.panelEx3.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.panelEx3.Style.GradientAngle = 90
        Me.panelEx3.Style.WordWrap = True
        Me.panelEx3.TabIndex = 9
        Me.panelEx3.Text = "It is important to note that if you are going to use NuGenDVM's Speech Recognitio" & _
        "n Technology  you must train your system to your voice. This is done within the " & _
        "Windows Control Panel under Speech. Please do this on a regular basis (1 or more" & _
        " times every two weeks) if you do not train the Speech Recognition System, NuGen" & _
        "DVM will not recognize your voice accurately. Not All areas within NuGenDVM are " & _
        "covered in these videos. Please experiment; and if you have any questions, pleas" & _
        "e feel free to contact support. You can find Genetitbase, Inc. contact/support i" & _
        "nformation under ""Support"" within the Main Navigation Bar. The system is much mo" & _
        "re feature rich and robust than we can show you without our direct assistance, t" & _
        "herefore these videos are designed to give you a brief overview of NuGenDVM's ca" & _
        "pabilities. "
        '
        'uIButton1
        '
        Me.uIButton1.BackColor = System.Drawing.SystemColors.Control
        Me.uIButton1.Location = New System.Drawing.Point(712, 540)
        Me.uIButton1.Name = "uIButton1"
        Me.uIButton1.Size = New System.Drawing.Size(76, 23)
        Me.uIButton1.TabIndex = 7
        Me.uIButton1.Text = "Exit"
        Me.uIButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003
        '
        'expTree1
        '
        Me.expTree1.BackColor = System.Drawing.SystemColors.Control
        Me.expTree1.Location = New System.Drawing.Point(8, 540)
        Me.expTree1.Name = "expTree1"
        Me.expTree1.Size = New System.Drawing.Size(236, 20)
        Me.expTree1.StartUpDirectory = ExpTree.StartDir.Desktop
        Me.expTree1.TabIndex = 6
        Me.expTree1.Visible = False
        '
        'lv1
        '
        Me.lv1.Activation = System.Windows.Forms.ItemActivation.TwoClick
        Me.lv1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lv1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeaderName})
        Me.lv1.Dock = System.Windows.Forms.DockStyle.Top
        Me.lv1.Location = New System.Drawing.Point(0, 0)
        Me.lv1.MultiSelect = False
        Me.lv1.Name = "lv1"
        Me.lv1.Size = New System.Drawing.Size(798, 340)
        Me.lv1.TabIndex = 5
        '
        'ColumnHeaderName
        '
        Me.ColumnHeaderName.Text = "Name"
        Me.ColumnHeaderName.Width = 750
        '
        'frmVids
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(798, 568)
        Me.ControlBox = False
        Me.Controls.Add(Me.panelEx1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmVids"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Demonstration Videos - Double Click to View"
        Me.panelEx1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub frmVids_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            SystemImageListManager.SetListViewImageList(lv1, True, False)
            SystemImageListManager.SetListViewImageList(lv1, False, False)

            Dim cDir As New CShItem(Application.StartupPath & "\Help\Videos\")

            If cDir.IsFolder Then
                expTree1.RootItem = cDir
            End If

            Me.Focus()

        Catch

        End Try

    End Sub

#Region "   ExplorerTree Event Handling"

    Private Sub AfterNodeSelect(ByVal pathName As String, ByVal CSI As CShItem) Handles expTree1.ExpTreeNodeSelected

        Try

            Dim dirList As New ArrayList
            Dim fileList As New ArrayList
            Dim TotalItems As Integer
            LastSelectedCSI = CSI

            If CSI.DisplayName.Equals(CShItem.strMyComputer) Then
                dirList = CSI.GetDirectories 'avoid re-query since only has dirs

            Else
                dirList = CSI.GetDirectories
                fileList = CSI.GetFiles
            End If

            'SetUpComboBox(CSI)
            TotalItems = dirList.Count + fileList.Count

            If TotalItems > 0 Then

                Dim item As CShItem
                dirList.Sort()
                fileList.Sort()

                'Me.Text = pathName
                'sbr1.Text = pathName & "                 " & _
                '            dirList.Count & " Directories " & fileList.Count & " Files"
                Dim combList As New ArrayList(TotalItems)
                combList.AddRange(dirList)
                combList.AddRange(fileList)
                Event1.WaitOne()
                'Build the ListViewItems & add to lv1
                lv1.BeginUpdate()
                lv1.Items.Clear()

                For Each item In combList

                    Dim lvi As New ListViewItem(item.DisplayName)
                    With lvi

                        If Not item.IsDisk And item.IsFileSystem And Not item.IsFolder Then
                            If item.Length > 1024 Then
                                .SubItems.Add(Format(item.Length / 1024, "#,### KB"))

                            Else
                                .SubItems.Add(Format(item.Length, "##0 Bytes"))
                            End If

                        Else
                            .SubItems.Add("")
                        End If

                        .SubItems.Add(item.TypeName)

                        If item.IsDisk Then
                            .SubItems.Add("")

                        Else

                            If item.LastWriteTime = testTime Then '"#1/1/0001 12:00:00 AM#" is empty
                                .SubItems.Add("")

                            Else
                                .SubItems.Add(item.LastWriteTime)
                            End If
                        End If

                        '.ImageIndex = SystemImageListManager.GetIconIndex(item, False)
                        .Tag = item
                    End With
                    lv1.Items.Add(lvi)
                Next

                lv1.EndUpdate()
                LoadLV1Images()

            Else
                lv1.Items.Clear()
                'sbr1.Text = pathName & " Has No Items"
            End If

        Catch

        End Try

    End Sub

#End Region

#Region "   IconIndex Loading Thread"

    Private Sub LoadLV1Images()

        Try

            Dim ts As New ThreadStart(AddressOf DoLoadLv)
            Dim ot As New Thread(ts)
            ot.ApartmentState = ApartmentState.STA
            Event1.Reset()
            ot.Start()

        Catch

        End Try

    End Sub

    Private Sub DoLoadLv()

        Try

            Dim lvi As ListViewItem

            For Each lvi In lv1.Items
                lvi.ImageIndex = SystemImageListManager.GetIconIndex(lvi.Tag, False)
            Next

            Event1.Set()

        Catch

        End Try

    End Sub

#End Region

    Private Sub uIButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uIButton1.Click

        Try
            Me.Close()

        Catch
        End Try

    End Sub

    Private Sub lv1_ItemActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles lv1.ItemActivate

        Try

            Dim path As String = Application.StartupPath & "\Help\Videos\"
            Dim il As ListViewItem = lv1.SelectedItems.Item(0)
            Dim exename As String = il.Text
            System.Diagnostics.Process.Start(path & exename)
            il = Nothing
            path = Nothing
            exename = Nothing

        Catch
        End Try

    End Sub

End Class
