Imports System
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Enum
Imports System.IO
Imports System.Text
Imports Genetibase.UI.CShItem
Imports Genetibase.UI.SystemImageListManager

<DefaultProperty("StartUpDirectory"), DefaultEvent("StartUpDirectoryChanged")> _
Public Class ExpTree
    Inherits System.Windows.Forms.UserControl

    Private Root As TreeNode

    Public Event StartUpDirectoryChanged(ByVal newVal As StartDir)

    Public Event ExpTreeNodeSelected(ByVal SelPath As String, ByVal Item As CShItem)

    Private m_refresh As Boolean = False    'flag to force refreshing of Dirs for Refresh Method
    Private EnableEventPost As Boolean = True 'flag to supress ExpTreeNodeSelected raising during refresh and ExpandANode

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        SystemImageListManager.SetTreeViewImageList(tv1, False)

        AddHandler StartUpDirectoryChanged, AddressOf OnStartUpDirectoryChanged

        OnStartUpDirectoryChanged(m_StartUpDirectory)

    End Sub

    'UserControl1 overrides dispose to clean up the component list.
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
    Friend WithEvents tv1 As System.Windows.Forms.TreeView
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.tv1 = New System.Windows.Forms.TreeView()
        Me.SuspendLayout()
        '
        'tv1
        '
        Me.tv1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tv1.HideSelection = False
        Me.tv1.ImageIndex = -1
        Me.tv1.Name = "tv1"
        Me.tv1.SelectedImageIndex = -1
        Me.tv1.Size = New System.Drawing.Size(200, 264)
        Me.tv1.TabIndex = 0
        '
        'ExpTree
        '
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.tv1})
        Me.Name = "ExpTree"
        Me.Size = New System.Drawing.Size(200, 264)
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "   Public Properties"

#Region "   StartupDir"

    Public Enum StartDir As Integer
        Desktop = &H0
        MyComputer = &H11
        Programs = &H2
        Controls = &H3
        Printers = &H4
        Personal = &H5
        Favorites = &H6
        Startup = &H7
        Recent = &H8
        SendTo = &H9
        StartMenu = &HB
        MyDocuments = &HC
        'MyMusic = &HD
        'MyVideo = &HE
        DesktopDirectory = &H10
        Internet_Cache = &H20
        Cookies = &H21
        History = &H22
        Windows = &H24
        System = &H25
        Program_Files = &H26
        MyPictures = &H27
        Systemx86 = &H29
        AdminTools = &H30
    End Enum

    Private m_StartUpDirectory As StartDir = StartDir.Desktop

    <Category("Misc"), _
     Description("Sets the Initial Directory of the Tree"), _
     DefaultValue(StartDir.Desktop), Browsable(True)> _
    Public Property StartUpDirectory() As StartDir
        Get
            Return m_StartUpDirectory
        End Get
        Set(ByVal Value As StartDir)
            If Array.IndexOf(Value.GetValues(Value.GetType), Value) >= 0 Then
                m_StartUpDirectory = Value
                RaiseEvent StartUpDirectoryChanged(Value)
            Else
                Throw New ApplicationException("Invalid Initial StartUpDirectory")
            End If
        End Set
    End Property
#End Region

#Region "       RootItem"
    '<Summary>
    ' RootItem is a Run-Time only Property
    ' Setting this Item via an External call results in
    '  re-setting the entire tree to be rooted in the 
    '  input CShItem
    ' The new CShItem must be a valid CShItem of some kind
    '  of Folder (File Folder or System Folder)
    ' Attempts to set it using a non-Folder CShItem are ignored
    '</Summary>
    <Browsable(False)> _
    Public Property RootItem() As CShItem
        Get
            Return Root.Tag
        End Get
        Set(ByVal Value As CShItem)
            If Value.IsFolder Then
                If Not IsNothing(Root) Then
                    ClearTree()
                End If
                Root = New TreeNode(Value.DisplayName)
                BuildTree(Value.GetDirectories(m_refresh))
                Root.ImageIndex = SystemImageListManager.GetIconIndex(Value, False)
                Root.SelectedImageIndex = Root.ImageIndex
                Root.Tag = Value
                tv1.Nodes.Add(Root)
                Root.Expand()
                tv1.SelectedNode = Root
            End If
        End Set
    End Property
#End Region

#Region "       SelectedItem"
    <Browsable(False)> _
    Public ReadOnly Property SelectedItem() As CShItem
        Get
            If Not IsNothing(tv1.SelectedNode) Then
                Return tv1.SelectedNode.Tag
            Else
                Return Nothing
            End If
        End Get
    End Property
#End Region

#End Region

#Region "   Public Methods"
    '''</Summary>RefreshTree Method thanks to Calum McLellan</Summary>
    Public Sub RefreshTree(Optional ByVal root As CShItem = Nothing)
        'Set refresh variable for BeforeExpand method
        m_refresh = True
        EnableEventPost = False
        Dim node As TreeNode = Me.tv1.SelectedNode
        Me.tv1.BeginUpdate()
        Dim path As String = CType(node.Tag, CShItem).Path
        'Set root node
        If IsNothing(root) Then
            Me.RootItem = Me.RootItem
        Else
            Me.RootItem = root
        End If
        'Try to expand the node
        If Not Me.ExpandANode(path) Then
            Dim testNode As TreeNode
            Dim nodeList As New ArrayList()
            While Not IsNothing(node.Parent)
                nodeList.Add(node.Parent)
                node = node.Parent
            End While

            For Each node In nodeList
                path = CType(node.Tag, CShItem).Path
                If Me.ExpandANode(path) Then Exit For
            Next
        End If
        'Reset refresh variable for BeforeExpand method
        Me.tv1.EndUpdate()
        m_refresh = False
        EnableEventPost = True
        'We suppressed EventPosting during refresh, so give it one now
        tv1_AfterSelect(Me, New TreeViewEventArgs(tv1.SelectedNode))
    End Sub

    Public Function ExpandANode(ByVal newPath As String) As Boolean
        ExpandANode = False     'assume failure
        Dim newItem As CShItem
        Try
            newItem = New CShItem(newPath)
            If Not newItem.IsFolder Then Exit Function
        Catch
            Exit Function
        End Try
        Return ExpandANode(newItem)
    End Function

    Public Function ExpandANode(ByVal newItem As CShItem) As Boolean
        ExpandANode = False     'assume failure
        Dim baseNode As TreeNode = Root
        baseNode.Expand() 'Ensure base is filled in
        'do the drill down -- Node to expand must be included in tree
        Dim cp As cPidl = newItem.clsPidl     'the cPidl rep of the PIDL to be found
        Dim testNode As TreeNode
        Dim lim As Integer = cp.ItemCount - CType(baseNode.Tag, CShItem).clsPidl.ItemCount
        Do While lim > 0
            For Each testNode In baseNode.Nodes
                If cp.StartsWith(CType(testNode.Tag, CShItem).clsPidl) Then
                    baseNode = testNode
                    baseNode.Expand()
                    lim -= 1
                    GoTo NEXTLEV
                End If
            Next
            Exit Function   'if fall thru then can't find it, return False
NEXTLEV:
        Loop
        'after falling thru here, we have found & expanded the node
        Me.tv1.HideSelection = False
        Me.Select()
        Me.tv1.SelectedNode = baseNode
        Return True
    End Function
#End Region

#Region "   Initial Dir Set Handler"

    Private Sub OnStartUpDirectoryChanged(ByVal newVal As StartDir)
        If Not IsNothing(Root) Then
            ClearTree()
        End If
        Dim L1 As ArrayList
        Dim special As CShItem
        If CType(Val(m_StartUpDirectory), StartDir) = StartDir.Desktop Then
            special = GetDeskTop()
        Else
            special = New CShItem(CType(Val(m_StartUpDirectory), ShellDll.CSIDL))
        End If
        Root = New TreeNode(special.DisplayName)
        BuildTree(special.GetDirectories)
        Root.ImageIndex = SystemImageListManager.GetIconIndex(special, False)
        Root.SelectedImageIndex = Root.ImageIndex
        Root.Tag = special
        tv1.Nodes.Add(Root)
        Root.Expand()
    End Sub

    Private Function BuildTree(ByVal L1 As ArrayList)
        L1.Sort()
        Dim CSI As CShItem
        For Each CSI In L1
            Dim T As TreeNode = MakeNode(CSI)
            If CSI.HasSubFolders Then
                Dim T2 As New TreeNode(" : ")
                T.Nodes.Add(T2)
            End If
            Root.Nodes.Add(T)
            'CSI.DebugDump()
        Next

    End Function

    Private Function MakeNode(ByVal fi As CShItem) As TreeNode
        Dim node As New TreeNode(fi.DisplayName)
        node.Tag = fi
        node.ImageIndex = SystemImageListManager.GetIconIndex(fi, False)
        node.SelectedImageIndex = SystemImageListManager.GetIconIndex(fi, True)
        Return node
    End Function

    Private Sub ClearTree()
        tv1.Nodes.Clear()
        Root = Nothing
    End Sub
#End Region

#Region "   TreeView BeforeExpand Event"

    Private Sub tv1_BeforeExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles tv1.BeforeExpand
        If e.Node.Nodes.Count = 1 AndAlso e.Node.Nodes(0).Text.Equals(" : ") Then
            Dim oldCursor As Cursor = Cursor
            Cursor = Cursors.WaitCursor
            e.Node.Nodes.Clear()
            Dim CSI As CShItem = e.Node.Tag
            Dim D As ArrayList = CSI.GetDirectories(m_refresh)
            If D.Count > 0 Then
                D.Sort()    'uses the class comparer
                Dim item As CShItem
                For Each item In D
                    Dim newNode As TreeNode = MakeNode(item)
                    If item.HasSubFolders Then
                        Dim T2 As New TreeNode(" : ")
                        newNode.Nodes.Add(T2)
                    End If
                    e.Node.Nodes.Add(newNode)
                Next
            End If
            Cursor = oldCursor
        End If
    End Sub
#End Region

#Region "   TreeView AfterSelect Event"
    Private Sub tv1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tv1.AfterSelect
        Dim node As TreeNode = e.Node
        Dim CSI As CShItem = e.Node.Tag
        If CSI.GetDirectories.Count < 1 Then
            node.Nodes.Clear()
        End If
        If EnableEventPost Then 'turned off during Refresh
            If CSI.Path.StartsWith(":") Then
                RaiseEvent ExpTreeNodeSelected(CSI.DisplayName, CSI)
            Else
                RaiseEvent ExpTreeNodeSelected(CSI.Path, CSI)
            End If
        End If
    End Sub
#End Region
End Class
