Imports System.Runtime.InteropServices
Imports System.IO
Imports System.IO.FileSystemInfo
Imports System.Text
Imports Genetibase.UI.ShellDll
Imports System.Windows.Forms


Public Class CShItem
    Implements IDisposable, IComparable

#Region "   Shared Private Fields"
    'This class has occasion to refer to the TypeName as reported by
    ' SHGetFileInfo. It needs to compare this to the string
    ' (in English) "System Folder"
    'on non-English systems, we do not know, in the general case,
    ' what the equivalent string is to compare against
    'The following variable is set by Sub New() to the string that
    ' corresponds to "System Folder" on the current machine
    ' Sub New() depends on the existance of My Computer(CSIDL.DRIVES),
    ' to determine what the equivalent string is
    Private Shared m_strSystemFolder As String
    'My Computer is also commonly used (though not internally),
    ' so save & expose its name on the current machine
    Private Shared m_strMyComputer As String
    Private Shared m_strMyDocuments As String
    ' The DesktopBase is set up via Sub New() (one time only) and
    '  disposed of only when DesktopBase is finally disposed of
    Private Shared DesktopBase As CShItem

    'We can avoid an extra SHGetFileInfo call once this is set up
    Private Shared OpenFolderIconIndex As Integer = -1

    ' It is also useful to know if the OS is XP or above.  
    ' Set up in Sub New() to avoid multiple calls to find this info
    Private Shared XPorAbove As Boolean

#End Region

#Region "   Instance Private Fields"
    'm_Folder and m_Pidl must be released/freed at Dispose time
    Private m_Folder As IShellFolder    'if item is a folder, contains the Folder interface for this instance
    Private m_Pidl As IntPtr            'The Absolute PIDL for this item (not retained for files)
    Private m_DisplayName As String = ""
    Private m_Path As String
    Private m_TypeName As String
    Private m_Parent As CShItem = Nothing
    Private m_IconIndexNormal As Integer   'index into the System Image list for Normal icon
    Private m_IconIndexOpen As Integer 'index into the SystemImage list for Open icon
    Private m_IsBrowsable As Boolean
    Private m_IsFileSystem As Boolean
    Private m_IsFolder As Boolean
    Private m_HasSubFolders As Boolean
    Private m_IsLink As Boolean
    Private m_IsDisk As Boolean
    Private m_IsShared As Boolean
    Private m_IsHidden As Boolean

    Private m_SortFlag As Integer = 0 'Used in comparisons

    Private m_Directories As ArrayList

    'The following elements are only filled in on demand
    Private m_XtrInfo As Boolean = False
    Private m_LastWriteTime As DateTime
    Private m_CreationTime As DateTime
    Private m_LastAccessTime As DateTime
    Private m_Length As Long

    'Indicates whether DisplayName, TypeName, SortFlag have been set up
    Private m_HasDispType As Boolean = False

    'Holds a byte() representation of m_PIDL -- filled when needed
    Private m_cPidl As cPidl

    'Flags for Dispose state
    Private m_IsDisposing As Boolean
    Private m_Disposed As Boolean

#End Region

#Region "   Destructor"
    ''' <summary>
    ''' Summary of Dispose.
    ''' </summary>
    ''' 
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        ' Take yourself off of the finalization queue
        ' to prevent finalization code for this object
        ' from executing a second time.
        GC.SuppressFinalize(Me)
    End Sub
    ''' <summary>
    ''' Summary of Dispose.
    ''' </summary>
    ''' <param name=disposing></param>
    ''' 
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        ' Allow your Dispose method to be called multiple times,
        ' but throw an exception if the object has been disposed.
        ' Whenever you do something with this class, 
        ' check to see if it has been disposed.
        If Not (m_Disposed) Then
            ' If disposing equals true, dispose all managed 
            ' and unmanaged resources.
            m_Disposed = True
            If (disposing) Then
            End If
            ' Release unmanaged resources. If disposing is false,
            ' only the following code is executed. 
            If Not IsNothing(m_Folder) Then
                Marshal.ReleaseComObject(m_Folder)
            End If
            If Not m_Pidl.Equals(IntPtr.Zero) Then
                Marshal.FreeCoTaskMem(m_Pidl)
            End If
        Else
            Throw New Exception("CShItem Disposed more than once")
        End If
    End Sub

    ' This Finalize method will run only if the 
    ' Dispose method does not get called.
    ' By default, methods are NotOverridable. 
    ' This prevents a derived class from overriding this method.
    ''' <summary>
    ''' Summary of Finalize.
    ''' </summary>
    ''' 
    Protected Overrides Sub Finalize()
        ' Do not re-create Dispose clean-up code here.
        ' Calling Dispose(false) is optimal in terms of
        ' readability and maintainability.
        Dispose(False)
    End Sub

#End Region

#Region "   Constructors"
    '<param>parent -- the CShitem of the parent</param>
    ''' <summary>
    ''' Private Constructor, creates new CShItem from the item's parent folder and
    '''  the item's PIDL relative to that folder.</summary>
    ''' <param name=folder>the folder interface of the parent</param>
    ''' <param name=pidl>the Relative PIDL of this item</param>
    ''' <param name=parent>the CShitem of the parent</param>
    ''' 
    Private Sub New(ByVal folder As IShellFolder, ByVal pidl As IntPtr, ByVal parent As CShItem)
        If IsNothing(DesktopBase) Then
            DesktopBase = New CShItem() 'This initializes the Desktop folder
        End If
        m_Parent = parent
        m_Pidl = concatPidls(parent.PIDL, pidl)

        'Get some attributes
        SetUpAttributes(folder, pidl)

        'Set unfetched value for IconIndex....
        m_IconIndexNormal = -1
        m_IconIndexOpen = -1
        'finally, set up my Folder
        If m_IsFolder Then
            Dim HR As Integer
            HR = folder.BindToObject(pidl, IntPtr.Zero, IID_IShellFolder, m_Folder)
            If HR <> NOERROR Then
                Marshal.ThrowExceptionForHR(HR)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Private Constructor. Creates CShItem of the Desktop
    ''' </summary>
    ''' 
    Private Sub New()           'only used when desktopfolder has not been intialized
        If Not IsNothing(DesktopBase) Then
            Throw New Exception("Attempt to initialize CShItem for second time")
        End If

        Dim HR As Integer
        'firstly determine what the local machine calls a "System Folder" and "My Computer"
        Dim tmpPidl As IntPtr
        HR = SHGetSpecialFolderLocation(0, CSIDL.DRIVES, tmpPidl)
        Dim shfi As New SHFILEINFO()
        Dim dwflag As Integer = SHGFI.DISPLAYNAME Or _
                                SHGFI.TYPENAME Or _
                                SHGFI.PIDL
        Dim dwAttr As Integer = 0
        SHGetFileInfo(tmpPidl, dwAttr, shfi, cbFileInfo, dwflag)
        m_strSystemFolder = shfi.szTypeName
        m_strMyComputer = shfi.szDisplayName
        Marshal.FreeCoTaskMem(tmpPidl)
        'set OS version info
        XPorAbove = ShellDll.IsXpOrAbove()

        'With That done, now set up Desktop CShItem
        m_Path = "::{" & DesktopGUID.ToString & "}"
        m_IsFolder = True
        m_HasSubFolders = True
        m_IsBrowsable = False
        HR = SHGetDesktopFolder(m_Folder)
        m_Pidl = GetSpecialFolderLocation(IntPtr.Zero, CSIDL.DESKTOP)
        dwflag = SHGFI.DISPLAYNAME Or _
                 SHGFI.TYPENAME Or _
                 SHGFI.SYSICONINDEX Or _
                 SHGFI.PIDL
        dwAttr = 0
        Dim H As IntPtr = SHGetFileInfo(m_Pidl, dwAttr, shfi, cbFileInfo, dwflag)
        m_DisplayName = shfi.szDisplayName
        m_TypeName = strSystemFolder   'not returned correctly by SHGetFileInfo
        m_IconIndexNormal = shfi.iIcon
        m_IconIndexOpen = shfi.iIcon
        m_HasDispType = True

        'also get local name for "My Documents"
        Dim pchEaten As Integer
        tmpPidl = IntPtr.Zero
        HR = m_Folder.ParseDisplayName(Nothing, Nothing, "::{450d8fba-ad25-11d0-98a8-0800361b1103}", _
                 pchEaten, tmpPidl, Nothing)
        shfi = New SHFILEINFO()
        dwflag = SHGFI.DISPLAYNAME Or _
                                SHGFI.TYPENAME Or _
                                SHGFI.PIDL
        dwAttr = 0
        SHGetFileInfo(tmpPidl, dwAttr, shfi, cbFileInfo, dwflag)
        m_strMyDocuments = shfi.szDisplayName
        Marshal.FreeCoTaskMem(tmpPidl)
        'this must be done after getting "My Documents" string
        m_SortFlag = ComputeSortFlag()
    End Sub

    ''' <summary>Create instance based on a non-desktop CSIDL.
    ''' Will create based on any CSIDL Except the DeskTop CSIDL</summary>
    ''' <param name=ID>Value from CSIDL enumeration denoting the folder to create this CShItem of.</param>
    ''' 
    Sub New(ByVal ID As CSIDL)
        If IsNothing(DesktopBase) Then
            DesktopBase = New CShItem() 'This initializes the Desktop folder
        End If
        Dim HR As Integer
        If ID = CSIDL.MYDOCUMENTS Then
            Dim pchEaten As Integer
            HR = DesktopBase.m_Folder.ParseDisplayName(Nothing, Nothing, "::{450d8fba-ad25-11d0-98a8-0800361b1103}", _
                     pchEaten, m_Pidl, Nothing)
        Else
            HR = SHGetSpecialFolderLocation(0, ID, m_Pidl)
        End If
        If HR = NOERROR Then
            Dim pParent As IShellFolder
            Dim relPidl As IntPtr = IntPtr.Zero
            Dim itemCnt As Integer = PidlCount(m_Pidl)
            If itemCnt = 1 Then         'parent is desktop
                HR = SHGetDesktopFolder(pParent)
                relPidl = m_Pidl
            Else
                Dim tmpPidl As IntPtr
                tmpPidl = TrimPidl(m_Pidl, relPidl)
                HR = DesktopBase.m_Folder.BindToObject(tmpPidl, IntPtr.Zero, IID_IShellFolder, pParent)
                If Not HR = NOERROR Then Marshal.ThrowExceptionForHR(HR)
                Marshal.FreeCoTaskMem(tmpPidl)
            End If
            'Get the Attributes
            SetUpAttributes(pParent, relPidl)
            'Set unfetched value for IconIndex....
            m_IconIndexNormal = -1
            m_IconIndexOpen = -1
            'finally, set up my Folder
            If m_IsFolder Then
                HR = pParent.BindToObject(relPidl, IntPtr.Zero, IID_IShellFolder, m_Folder)
                If HR <> NOERROR Then
                    Marshal.ThrowExceptionForHR(HR)
                End If
            End If
            Marshal.ReleaseComObject(pParent)
            'if itemCnt=1 then relPidl is same as m_Pidl, don't release
            If itemCnt > 1 Then Marshal.FreeCoTaskMem(relPidl)
        Else
            Marshal.ThrowExceptionForHR(HR)
        End If
    End Sub

    ''' <summary>Create a new CShItem based on a Path Must be a valid FileSystem Path</summary>
    ''' <param name=path></param>
    ''' 
    Sub New(ByVal path As String)
        If IsNothing(DesktopBase) Then
            DesktopBase = New CShItem() 'This initializes the Desktop folder
        End If
        'Removal of following code allows Path(GUID) of Special FOlders to serve
        '  as a valid Path for CShItem creation (part of Calum's refresh code needs this
        'If Not Directory.Exists(path) AndAlso Not File.Exists(path) Then
        '    Throw New Exception("CShItem -- Invalid Path specified")
        'End If
        Dim HR As Integer
        HR = DesktopBase.m_Folder.ParseDisplayName(0, IntPtr.Zero, path, 0, m_Pidl, 0)
        If Not HR = NOERROR Then Marshal.ThrowExceptionForHR(HR)
        Dim pParent As IShellFolder
        Dim relPidl As IntPtr = IntPtr.Zero
        Dim itemCnt As Integer = PidlCount(m_Pidl)
        If itemCnt = 1 Then         'parent is desktop
            HR = SHGetDesktopFolder(pParent)
            relPidl = m_Pidl
        Else
            Dim tmpPidl As IntPtr
            tmpPidl = TrimPidl(m_Pidl, relPidl)
            HR = DesktopBase.m_Folder.BindToObject(tmpPidl, IntPtr.Zero, IID_IShellFolder, pParent)
            If Not HR = NOERROR Then Marshal.ThrowExceptionForHR(HR)
            Marshal.FreeCoTaskMem(tmpPidl)
        End If
        'Get the Attributes
        SetUpAttributes(pParent, relPidl)
        'Set unfetched value for IconIndex....
        m_IconIndexNormal = -1
        m_IconIndexOpen = -1
        'finally, set up my Folder
        If m_IsFolder Then
            HR = pParent.BindToObject(relPidl, IntPtr.Zero, IID_IShellFolder, m_Folder)
            If HR <> NOERROR Then
                Marshal.ThrowExceptionForHR(HR)
            End If
        End If
        Marshal.ReleaseComObject(pParent)
        'if itemCnt=1 then relPidl is same as m_Pidl, don't release
        If itemCnt > 1 Then Marshal.FreeCoTaskMem(relPidl)
    End Sub

    ''' <summary>Get the base attributes of the folder/file that this CShItem represents</summary>
    ''' <param name=folder>Parent Folder of this Item</param>
    ''' <param name=pidl>Relative Pidl of this Item.</param>
    ''' 
    Private Sub SetUpAttributes(ByVal folder As IShellFolder, ByVal pidl As IntPtr)
        Dim attrFlag As SFGAO
        attrFlag = SFGAO.BROWSABLE
        attrFlag = attrFlag Or SFGAO.FILESYSTEM
        attrFlag = attrFlag Or SFGAO.HASSUBFOLDER
        attrFlag = attrFlag Or SFGAO.FOLDER
        attrFlag = attrFlag Or SFGAO.LINK
        attrFlag = attrFlag Or SFGAO.SHARE
        attrFlag = attrFlag Or SFGAO.HIDDEN
        'Note: for GetAttributesOf, we must provide an array, in almost all cases with 1 element
        Dim aPidl(0) As IntPtr
        aPidl(0) = pidl
        folder.GetAttributesOf(1, aPidl, attrFlag)
        m_IsBrowsable = CBool(attrFlag And SFGAO.BROWSABLE)
        m_IsFileSystem = CBool(attrFlag And SFGAO.FILESYSTEM)
        m_HasSubFolders = CBool(attrFlag And SFGAO.HASSUBFOLDER)
        m_IsFolder = CBool(attrFlag And SFGAO.FOLDER)
        m_IsLink = CBool(attrFlag And SFGAO.LINK)
        m_IsShared = CBool(attrFlag And SFGAO.SHARE)
        m_IsHidden = CBool(attrFlag And SFGAO.HIDDEN)
        'Get the Path
        'Dim strr As New STRRET()
        Dim strr As IntPtr = Marshal.AllocCoTaskMem(MAX_PATH * 2 + 4)
        Marshal.WriteInt32(strr, 0, 0)
        Dim buf As New StringBuilder(MAX_PATH)
        Dim itemflags As SHGDN = SHGDN.FORPARSING
        folder.GetDisplayNameOf(pidl, itemflags, strr)
        Dim HR As Integer = StrRetToBuf(strr, pidl, buf, MAX_PATH)
        Marshal.FreeCoTaskMem(strr)     'now done with it
        If HR = NOERROR Then
            m_Path = buf.ToString
            If m_IsFolder AndAlso m_IsFileSystem AndAlso XPorAbove Then
                If File.Exists(m_Path) Then
                    m_IsFolder = False
                End If
            End If
            If m_Path.Length = 3 AndAlso m_Path.Substring(1).Equals(":\") Then
                m_IsDisk = True
            End If
        Else
            Marshal.ThrowExceptionForHR(HR)
        End If
    End Sub

#End Region

#Region "   Icomparable -- for default Sorting"

    ''' <summary>Computes the Sort key of this CShItem, based on its attributes</summary>
    ''' 
    Private Function ComputeSortFlag() As Integer
        Dim rVal As Integer = 0
        If m_IsDisk Then rVal = &H100000
        If m_TypeName.Equals(strSystemFolder) Then
            If Not m_IsBrowsable Then
                rVal = rVal Or &H10000
                If m_strMyDocuments.Equals(m_DisplayName) Then
                    rVal = rVal Or &H1
                End If
            Else
                rVal = rVal Or &H1000
            End If
        End If
        If m_IsFolder Then rVal = rVal Or &H100
        Return rVal
    End Function

    '''<Summary> CompareTo(obj as object)
    '''  Compares obj to this instance based on SortFlag-- obj must be a CShItem</Summary>
    '''<SortOrder>  (low)Disks,non-browsable System Folders,
    '''              browsable System Folders, 
    '''               Directories, Files, Nothing (high)</SortOrder>
    Public Overridable Overloads Function CompareTo(ByVal obj As Object) As Integer _
            Implements IComparable.CompareTo
        If IsNothing(obj) Then Return 1 'non-existant is always low
        Dim Other As CShItem = obj
        If Not m_HasDispType Then SetDispType()
        Dim cmp As Integer = Other.SortFlag - m_SortFlag 'Note the reversal
        If cmp <> 0 Then
            Return cmp
        Else
            If m_IsDisk Then 'implies that both are
                Return String.Compare(m_Path, Other.Path)
            Else
                Return String.Compare(m_DisplayName, Other.DisplayName)
            End If
        End If
    End Function
#End Region

#Region "   Properties"

#Region "       Shared Properties"
    Public Shared ReadOnly Property strMyComputer() As String
        Get
            Return m_strMyComputer
        End Get
    End Property

    Public Shared ReadOnly Property strSystemFolder() As String
        Get
            Return m_strSystemFolder
        End Get
    End Property

#End Region

#Region "       Normal Properties"
    Public ReadOnly Property PIDL() As IntPtr
        Get
            Return m_Pidl
        End Get
    End Property

    Public ReadOnly Property Path() As String
        Get
            Return m_Path
        End Get
    End Property
    Public ReadOnly Property Parent() As CShItem
        Get
            Return m_Parent
        End Get
    End Property

    Public ReadOnly Property IsBrowsable() As Boolean
        Get
            Return m_IsBrowsable
        End Get
    End Property
    Public ReadOnly Property IsFileSystem() As Boolean
        Get
            Return m_IsFileSystem
        End Get
    End Property
    Public ReadOnly Property IsFolder() As Boolean
        Get
            Return m_IsFolder
        End Get
    End Property
    Public ReadOnly Property HasSubFolders() As Boolean
        Get
            Return m_HasSubFolders
        End Get
    End Property
    Public ReadOnly Property IsDisk() As Boolean
        Get
            Return m_IsDisk
        End Get
    End Property
    Public ReadOnly Property IsLink() As Boolean
        Get
            Return m_IsLink
        End Get
    End Property
    Public ReadOnly Property IsShared() As Boolean
        Get
            Return m_IsShared
        End Get
    End Property
    Public ReadOnly Property IsHidden() As Boolean
        Get
            Return m_IsHidden
        End Get
    End Property

#End Region

#Region "       Filled on Demand Properties"
#Region "           Filled based on m_HasDispType"
    ''' <summary>
    ''' Set DisplayName, TypeName, and SortFlag when actually needed
    ''' </summary>
    ''' 
    Private Sub SetDispType()
        'Get Displayname, TypeName
        Dim shfi As New SHFILEINFO()
        Dim dwflag As Integer = SHGFI.DISPLAYNAME Or _
                                SHGFI.TYPENAME Or _
                                SHGFI.PIDL
        Dim dwAttr As Integer = 0
        If m_IsFileSystem And Not m_IsFolder Then
            dwflag = dwflag Or SHGFI.USEFILEATTRIBUTES
            dwAttr = FILE_ATTRIBUTE_NORMAL
        End If
        Dim H As IntPtr = SHGetFileInfo(m_Pidl, dwAttr, shfi, cbFileInfo, dwflag)
        m_DisplayName = shfi.szDisplayName
        m_TypeName = shfi.szTypeName
        'fix DisplayName
        If m_DisplayName.Equals("") Then
            m_DisplayName = m_Path
        End If
        'Fix TypeName
        'If m_IsFolder And m_TypeName.Equals("File") Then
        '    m_TypeName = "File Folder"
        'End If
        m_SortFlag = ComputeSortFlag()
        m_HasDispType = True
    End Sub

    Public ReadOnly Property DisplayName() As String
        Get
            If Not m_HasDispType Then SetDispType()
            Return m_DisplayName
        End Get
    End Property

    Private ReadOnly Property SortFlag() As Integer
        Get
            If Not m_HasDispType Then SetDispType()
            Return m_SortFlag
        End Get
    End Property

    Public ReadOnly Property TypeName() As String
        Get
            If Not m_HasDispType Then SetDispType()
            Return m_TypeName
        End Get
    End Property
#End Region

#Region "           IconIndex properties"
    Public ReadOnly Property IconIndexNormal() As Integer
        Get
            If m_IconIndexNormal < 0 Then
                If Not m_HasDispType Then SetDispType()
                Dim shfi As New SHFILEINFO()
                Dim dwflag As Integer = SHGFI.PIDL Or _
                                        SHGFI.SYSICONINDEX
                Dim dwAttr As Integer = 0
                If m_IsFileSystem And Not m_IsFolder Then
                    dwflag = dwflag Or SHGFI.USEFILEATTRIBUTES
                    dwAttr = FILE_ATTRIBUTE_NORMAL
                End If
                Dim H As IntPtr = SHGetFileInfo(m_Pidl, dwAttr, shfi, cbFileInfo, dwflag)
                m_IconIndexNormal = shfi.iIcon
            End If
            Return m_IconIndexNormal
        End Get
    End Property
    ' IconIndexOpen is Filled on demand
    Public ReadOnly Property IconIndexOpen() As Integer
        Get
            If m_IconIndexOpen < 0 Then
                If Not m_HasDispType Then SetDispType()
                If Not m_IsDisk And m_IsFileSystem And m_IsFolder Then
                    If OpenFolderIconIndex < 0 Then
                        Dim dwflag As Integer = SHGFI.SYSICONINDEX Or SHGFI.PIDL
                        Dim shfi As New SHFILEINFO()
                        Dim H As IntPtr = SHGetFileInfo(m_Pidl, 0, _
                                          shfi, cbFileInfo, _
                                          dwflag Or SHGFI.OPENICON)
                        m_IconIndexOpen = shfi.iIcon
                        'If m_TypeName.Equals("File Folder") Then
                        '    OpenFolderIconIndex = shfi.iIcon
                        'End If
                    Else
                        m_IconIndexOpen = OpenFolderIconIndex
                    End If
                Else
                    m_IconIndexOpen = m_IconIndexNormal
                End If
            End If
            Return m_IconIndexOpen
        End Get
    End Property
#End Region

#Region "           FileInfo type Information"

    ''' <summary>
    ''' Obtains information available from FileInfo.
    ''' </summary>
    ''' 
    Private Sub FillDemandInfo()
        If Not m_IsDisk And m_IsFileSystem And Not m_IsFolder Then
            'in this case, it's a file
            If File.Exists(m_Path) Then
                Dim fi As New FileInfo(m_Path)
                m_LastWriteTime = fi.LastWriteTime
                m_LastAccessTime = fi.LastAccessTime
                m_CreationTime = fi.CreationTime
                m_Length = fi.Length
                m_XtrInfo = True
            End If
        Else
            If m_IsFileSystem And m_IsFolder Then
                If Directory.Exists(m_Path) Then
                    Dim di As New DirectoryInfo(m_Path)
                    m_LastWriteTime = di.LastWriteTime
                    m_LastAccessTime = di.LastAccessTime
                    m_CreationTime = di.CreationTime
                    m_XtrInfo = True
                End If
            End If
        End If
    End Sub

    Public ReadOnly Property LastWriteTime() As DateTime
        Get
            If Not m_XtrInfo Then
                FillDemandInfo()
            End If
            Return m_LastWriteTime
        End Get
    End Property
    Public ReadOnly Property LastAccessTime() As DateTime
        Get
            If Not m_XtrInfo Then
                FillDemandInfo()
            End If
            Return m_LastAccessTime
        End Get
    End Property
    Public ReadOnly Property CreationTime() As DateTime
        Get
            If Not m_XtrInfo Then
                FillDemandInfo()
            End If
            Return m_CreationTime
        End Get
    End Property
    Public ReadOnly Property Length() As Long
        Get
            If Not m_XtrInfo Then
                FillDemandInfo()
            End If
            Return m_Length
        End Get
    End Property
#End Region

#Region "           cPidl information"
    'Public ReadOnly Property pidlStartsWith(ByVal other As CShItem) As Boolean
    '    Get
    '        If IsNothing(m_cPidl) Then
    '            m_cPidl = New cPidl(m_Pidl)
    '        End If
    '        If IsNothing(other.m_cPidl) Then
    '            other.m_cPidl = New cPidl(other.PIDL)
    '        End If
    '        Return m_cPidl.StartsWith(other.m_cPidl)
    '    End Get
    'End Property
    Public ReadOnly Property clsPidl() As cPidl
        Get
            If IsNothing(m_cPidl) Then
                m_cPidl = New cPidl(m_Pidl)
            End If
            Return m_cPidl
        End Get
    End Property
#End Region

#End Region

#End Region

#Region "   Public Methods"

#Region "       Shared Public Methods"
    ''' <summary>
    ''' If not initialized, then build DesktopBase
    ''' once done, or if initialized already,
    ''' </summary>
    '''<returns>The DesktopBase CShItem representing the desktop</returns>
    ''' 
    Public Shared Function GetDeskTop() As CShItem
        If IsNothing(DesktopBase) Then
            DesktopBase = New CShItem()
        End If
        Return DesktopBase
    End Function

#Region "      AllFolderWalk"
    '''<Summary> 
    '''The WalkAllCallBack delegate defines the signature of 
    '''the routine to be passed to DirWalker
    ''' Usage:  dim d1 as new CshItem.WalkAllCallBack(addressof yourroutine)
    '''   Callback function receives a CShItem for each file & Directory in
    '''   Starting Directory and each sub-directory of this directory and
    '''   each sub-dir of each sub-dir ....
    '''</Summary>
    Public Delegate Function WalkAllCallBack(ByVal info As CShItem, _
                                             ByVal UserLevel As Integer, _
                                             ByVal Tag As Integer) _
                                             As Boolean
    '''</Summary>
    '''AllFolderWalk recursively walks down directories from cStart, calling its
    '''   callback routine, WalkAllCallBack, for each Directory and File encountered, including those in
    '''   cStart.  UserLevel is incremented by 1 for each level of dirs that DirWalker
    '''  recurses thru.  Tag in an Integer that is simply passed, unmodified to the 
    '''  callback, with each CShItem encountered, both File & Directory CShItems.
    ''' </Summary>
    ''' <param name=cStart></param>
    ''' <param name=cback></param>
    ''' <param name=UserLevel></param>
    ''' <param name=Tag></param>
    ''' 
    Public Shared Function AllFolderWalk(ByVal cStart As CShItem, _
                                          ByVal cback As WalkAllCallBack, _
                                          ByVal UserLevel As Integer, _
                                          ByVal Tag As Integer) _
                                          As Boolean
        If Not IsNothing(cStart) AndAlso cStart.IsFolder Then
            Dim cItem As CShItem
            'first processes all files in this directory
            For Each cItem In cStart.GetFiles
                If Not cback(cItem, UserLevel, Tag) Then
                    Return False        'user said stop
                End If
            Next
            'then process all dirs in this directory, recursively
            For Each cItem In cStart.GetDirectories
                If Not cback(cItem, UserLevel + 1, Tag) Then
                    Return False        'user said stop
                Else
                    If Not AllFolderWalk(cItem, cback, UserLevel + 1, Tag) Then
                        Return False
                    End If
                End If
            Next
            Return True
        Else        'Invalid call
            Throw New ApplicationException("AllFolderWalk -- Invalid Start Directory")
        End If
    End Function
#End Region

#End Region

#Region "       Instance Methods"
    ''' <summary>
    ''' Returns the Directories of this sub-folder as an ArrayList of CShitems
    ''' </summary>
    ''' <returns>An ArrayList of CShItems. May return an empty ArrayList if there are none.</returns>
    ''' <remarks>New version of code thanks to Calum McLellan for refresh code</remarks>
    Public Function GetDirectories(Optional ByVal refresh As Boolean = False) As ArrayList
        If m_IsFolder Then
            If refresh Then 'Build the list
                m_Directories = GetContents(SHCONTF.FOLDERS Or SHCONTF.INCLUDEHIDDEN)
                Return m_Directories
            ElseIf IsNothing(m_Directories) Then 'Build the list
                m_Directories = GetContents(SHCONTF.FOLDERS Or SHCONTF.INCLUDEHIDDEN)
                Return m_Directories
            Else 'Built this once, just returned saved list
                Return m_Directories
            End If
        Else 'if it is not a Folder, then return empty arraylist
            Return New ArrayList()
        End If
    End Function

    ''' <summary>
    ''' Returns the Files of this sub-folder as an
    '''   ArrayList of CShitems
    ''' Note: we do not keep the arraylist of files, Generate it each time
    ''' </summary>
    ''' <returns>An ArrayList of CShItems. May return an empty ArrayList if there are none.</returns>
    ''' 
    Public Function GetFiles() As ArrayList
        If m_IsFolder Then
            Return GetContents(SHCONTF.NONFOLDERS Or SHCONTF.INCLUDEHIDDEN)
        Else
            Return New ArrayList()
        End If
    End Function
    ''' <summary>
    ''' Returns the Files of this sub-folder, filtered by a filtering string, as an
    '''   ArrayList of CShitems
    ''' Note: we do not keep the arraylist of files, Generate it each time
    ''' </summary>
    ''' <param name=Filter>A filter string (for example: *.Doc)</param>
    ''' <returns>An ArrayList of CShItems. May return an empty ArrayList if there are none.</returns>
    ''' 
    Public Function GetFiles(ByVal Filter As String) As ArrayList
        If m_IsFolder Then
            Dim dummy As New ArrayList()
            Dim fileentries() As String
            fileentries = Directory.GetFiles(m_Path, Filter)
            Dim vFile As String
            For Each vFile In fileentries
                dummy.Add(New CShItem(vFile))
            Next
            Return dummy
        Else
            Return New ArrayList()
        End If
    End Function

    ''' <summary>
    ''' Returns the Directories and Files of this sub-folder as an
    '''   ArrayList of CShitems
    ''' Note: we do not keep the arraylist of files, Generate it each time
    ''' </summary>
    ''' <returns>An ArrayList of CShItems. May return an empty ArrayList if there are none.</returns>
    Public Function GetItems() As ArrayList
        Dim rVal As New ArrayList()
        If m_IsFolder Then
            rVal.AddRange(Me.GetContents(SHCONTF.FOLDERS Or SHCONTF.NONFOLDERS Or SHCONTF.INCLUDEHIDDEN))
            rVal.Sort()
            Return rVal
        Else
            Return rVal
        End If
    End Function

    ''' <summary>
    ''' Summary of ToString.
    ''' </summary>
    ''' 
    Public Overrides Function ToString() As String
        Return m_DisplayName
    End Function

#Region "  Debug Dumper"
    ''' <summary>
    ''' Summary of DebugDump.
    ''' </summary>
    ''' 
    Public Sub DebugDump()
        Debug.WriteLine("DisplayName = " & m_DisplayName)
        Debug.WriteLine("PIDL        = " & m_Pidl.ToString)
        Debug.WriteLine(vbTab & "Path        = " & m_Path)
        Debug.WriteLine(vbTab & "TypeName    = " & m_TypeName)
        Debug.WriteLine(vbTab & "iIconNormal = " & m_IconIndexNormal)
        Debug.WriteLine(vbTab & "iIconSelect = " & m_IconIndexOpen)
        Debug.WriteLine(vbTab & "IsBrowsable = " & m_IsBrowsable)
        Debug.WriteLine(vbTab & "IsFileSystem= " & m_IsFileSystem)
        Debug.WriteLine(vbTab & "IsFolder    = " & m_IsFolder)
        Debug.WriteLine(vbTab & "IsLink    = " & m_IsLink)
        If m_IsFolder Then
            If Not IsNothing(m_Directories) Then
                Debug.WriteLine(vbTab & "Directory Count = " & m_Directories.Count)
            Else
                Debug.WriteLine(vbTab & "Directory Count Not yet set")
            End If
        End If
    End Sub
#End Region

#End Region

#End Region

#Region "   Private Methods"

#Region "       GetContents"
    '<Summary>
    ''' Returns the requested Items of this sub-folder as an ArrayList of CShitems
    ''' </summary>
    ''' <param name=flags>A set of one or more SHCONTF flags indicating which items to return</param>
    ''' 
    Private Function GetContents(ByVal flags As SHCONTF) As ArrayList
        Dim rVal As New ArrayList()
        Dim HR As Integer
        Dim IEnum As IEnumIDList
        HR = m_Folder.EnumObjects(0, flags, IEnum)
        If HR = NOERROR Then
            Dim item As IntPtr = IntPtr.Zero
            Dim itemCnt As Integer
            HR = IEnum.GetNext(1, item, itemCnt)
            If HR = NOERROR Then
                Do While itemCnt > 0 AndAlso Not item.Equals(IntPtr.Zero)
                    'there is no setting to exclude folders from enumeration,
                    ' just one to include non-folders
                    ' so we have to screen the results to return only
                    '  non-folders if folders are not wanted
                    If Not CBool(flags And SHCONTF.FOLDERS) Then 'don't want folders. see if this is one
                        Dim attrFlag As SFGAO
                        attrFlag = attrFlag Or SFGAO.FOLDER Or _
                                                SFGAO.STREAM
                        'Note: for GetAttributesOf, we must provide an array, in almost all cases with 1 element
                        Dim aPidl(0) As IntPtr
                        aPidl(0) = item
                        m_Folder.GetAttributesOf(1, aPidl, attrFlag)
                        If Not XPorAbove Then
                            If CBool(attrFlag And SFGAO.FOLDER) Then 'Don't need it
                                GoTo SKIPONE
                            End If
                        Else         'XP or above
                            If CBool(attrFlag And SFGAO.FOLDER) AndAlso _
                               Not CBool(attrFlag And SFGAO.STREAM) Then
                                GoTo SKIPONE
                            End If
                        End If
                    End If
                    rVal.Add(New CShItem(m_Folder, item, Me))
SKIPONE:            Marshal.FreeCoTaskMem(item) 'if New kept it, it kept a copy
                    item = IntPtr.Zero
                    itemCnt = 0
                    Application.DoEvents()
                    HR = IEnum.GetNext(1, item, itemCnt)
                Loop
            Else
                If HR <> 1 Then GoTo HRError '1 means no more
            End If
        Else : GoTo HRError
        End If
        'Normal Exit
NORMAL: If Not IsNothing(IEnum) Then
            Marshal.ReleaseComObject(IEnum)
        End If
        rVal.TrimToSize()
        Return rVal

        ' Error Exit for all Com errors
HRError:  'not ready disks will return the following error
        If HR = &HFFFFFFFF800704C7 Then
            GoTo NORMAL
        ElseIf HR = &HFFFFFFFF80070015 Then
            GoTo NORMAL
            'unavailable net resources will return these
        ElseIf HR = &HFFFFFFFF80040E96 Or HR = &HFFFFFFFF80040E19  Then
            GoTo NORMAL
        ElseIf HR = &HFFFFFFFF80004001 Then 'Certain "Not Implemented" features will return this
            GoTo NORMAL
        End If
        If Not IsNothing(IEnum) Then Marshal.ReleaseComObject(IEnum)
#If Debug Then
        Marshal.ThrowExceptionForHR(HR)
#End If
        Return New ArrayList()  'sometimes it is a non-fatal error,ignored
    End Function
#End Region

#Region "       Really nasty Pidl manipulation"
    ''' <summary>
    ''' Get Size in bytes of the first (possibly only)
    '''  SHItem in an ID list.  Note: the full size of
    '''   the item is the sum of the sizes of all SHItems
    '''   in the list!!
    ''' </summary>
    ''' <param name=pidl></param>
    ''' 
    Private Shared Function ItemIDSize(ByVal pidl As IntPtr) As Integer
        If Not pidl.Equals(IntPtr.Zero) Then
            Dim b(1) As Byte
            Marshal.Copy(pidl, b, 0, 2)
            Return b(1) * 256 + b(0)
        Else
            Return 0
        End If
    End Function

    ''' <summary>
    ''' computes the actual size of the ItemIDList pointed to by pidl
    ''' </summary>
    ''' <param name=pidl>The pidl pointing to an ItemIDList</param>
    '''<returns> Returns actual size of the ItemIDList, less the terminating nulnul</returns> 
    Private Shared Function ItemIDListSize(ByVal pidl As IntPtr) As Integer
        If Not pidl.Equals(IntPtr.Zero) Then
            Dim i As Integer = ItemIDSize(pidl)
            Dim b As Integer = Marshal.ReadByte(pidl, i) + (Marshal.ReadByte(pidl, i + 1) * 256)
            Do While b > 0
                i += b
                b = Marshal.ReadByte(pidl, i) + (Marshal.ReadByte(pidl, i + 1) * 256)
            Loop
            Return i
        Else : Return 0
        End If
    End Function
    ''' <summary>
    ''' Counts the total number of SHItems in input pidl
    ''' </summary>
    ''' <param name=pidl>The pidl to obtain the count for</param>
    ''' <returns> Returns the count of SHItems pointed to by pidl</returns> 
    Private Shared Function PidlCount(ByVal pidl As IntPtr) As Integer
        If Not pidl.Equals(IntPtr.Zero) Then
            Dim cnt As Integer = 0
            Dim i As Integer = 0
            Dim b As Integer = Marshal.ReadByte(pidl, i) + (Marshal.ReadByte(pidl, i + 1) * 256)
            Do While b > 0
                cnt += 1
                i += b
                b = Marshal.ReadByte(pidl, i) + (Marshal.ReadByte(pidl, i + 1) * 256)
            Loop
            Return cnt
        Else : Return 0
        End If

    End Function
    ''' <summary>
    ''' Concatenates the contents of two pidls into a new Pidl (ended by 00)
    ''' allocating CoTaskMem to hold the result,
    ''' placing the concatenation (followed by 00) into the
    ''' allocated Memory,
    ''' and returning an IntPtr pointing to the allocated mem
    ''' </summary>
    ''' <param name=pidl1>IntPtr to a well formed SHItemIDList or IntPtr.Zero</param>
    ''' <param name=pidl2>IntPtr to a well formed SHItemIDList or IntPtr.Zero</param>
    ''' <returns>Returns a ptr to an ItemIDList containing the 
    '''   concatenation of the two (followed by the req 2 zeros
    '''   Caller must Free this pidl when done with it</returns> 
    Private Shared Function concatPidls(ByVal pidl1 As IntPtr, ByVal pidl2 As IntPtr) As IntPtr
        Dim cb1 As Integer, cb2 As Integer
        cb1 = ItemIDListSize(pidl1)
        cb2 = ItemIDListSize(pidl2)
        Dim rawCnt As Integer = cb1 + cb2
        If (rawCnt) > 0 Then
            Dim b(rawCnt + 1) As Byte
            If cb1 > 0 Then
                Marshal.Copy(pidl1, b, 0, cb1)
            End If
            If cb2 > 0 Then
                Marshal.Copy(pidl2, b, cb1, cb2)
            End If
            Dim rVal As IntPtr = Marshal.AllocCoTaskMem(cb1 + cb2 + 2)
            b(rawCnt) = 0 : b(rawCnt + 1) = 0
            Marshal.Copy(b, 0, rVal, rawCnt + 2)
            Return rVal
        Else
            Return IntPtr.Zero
        End If
    End Function

    ''' <summary>
    ''' Returns an ItemIDList with the last ItemID trimed off
    '''  This is necessary since I cannot get SHBindToParent to work 
    '''  It's purpose is to generate an ItemIDList for the Parent of a
    '''  Special Folder which can then be processed with DesktopBase.BindToObject,
    '''  yeilding a Folder for the parent of the Special Folder
    '''  It also creates and passes back a RELATIVE pidl for this item
    ''' </summary>
    ''' <param name=pidl>A pointer to a well formed ItemIDList. The PIDL to trim</param>
    ''' <param name=relPidl>BYREF IntPtr which will point to a new relative pidl
    '''        containing the contents of the last ItemID in the ItemIDList
    '''        terminated by the required 2 nulls.</param>
    ''' <returns> an ItemIDList with the last element removed.
    '''  Caller must Free this item when through with it
    '''  Also returns the new relative pidl in the 2nd parameter
    '''   Caller must Free this pidl as well, when through with it
    '''</returns>
    Private Shared Function TrimPidl(ByVal pidl As IntPtr, ByRef relPidl As IntPtr) As IntPtr
        Dim cb As Integer = ItemIDListSize(pidl)
        Dim b(cb + 1) As Byte
        Marshal.Copy(pidl, b, 0, cb)
        Dim prev As Integer = 0
        Dim i As Integer = b(0) + (b(1) * 256)
        Do While i < cb AndAlso b(i) <> 0
            prev = i
            i += b(i) + (b(i + 1) * 256)
        Loop
        If (prev + 1) < cb Then
            'first set up the relative pidl
            b(cb) = 0
            b(cb + 1) = 0
            Dim cb1 As Integer = b(prev) + (b(prev + 1) * 256)
            relPidl = Marshal.AllocCoTaskMem(cb1 + 2)
            Marshal.Copy(b, prev, relPidl, cb1 + 2)
            b(prev) = 0 : b(prev + 1) = 0
            Dim rVal As IntPtr = Marshal.AllocCoTaskMem(prev + 2)
            Marshal.Copy(b, 0, rVal, prev + 2)
            Return rVal
        Else
            Return IntPtr.Zero
        End If
    End Function

#Region "   DumpPidl Routines"
    ''' <summary>
    ''' Dumps, to the Debug output, the contents of the mem block pointed to by
    ''' a PIDL. Depends on the internal structure of a PIDL
    ''' </summary>
    ''' <param name=pidl>The IntPtr(a PIDL) pointing to the block to dump</param>
    ''' 
    Private Shared Sub DumpPidl(ByVal pidl As IntPtr)
        Dim cb As Integer = ItemIDListSize(pidl)
        Debug.WriteLine("PIDL " & pidl.ToString & " contains " & cb & " bytes")
        If cb > 0 Then
            Dim b(cb + 1) As Byte
            Marshal.Copy(pidl, b, 0, cb + 1)
            Dim pidlCnt As Integer = 1
            Dim i As Integer = b(0) + (b(1) * 256)
            Dim curB As Integer = 0
            Do While i > 0
                Debug.Write("ItemID #" & pidlCnt & " Length = " & i)
                DumpHex(b, curB, curB + i - 1)
                pidlCnt += 1
                curB += i
                i = b(curB) + (b(curB + 1) * 256)
            Loop
        End If
    End Sub

    '''<Summary>Dump a portion or all of a Byte Array to Debug output</Summary>
    '''<param name =b>A single dimension Byte Array</param>
    '''<param name =sPos>Optional start index of area to dump (default = 0)</param>
    '''<param name =b>Optional last index position to dump (default = end of array)/param>
    '''<Remarks>
    '''</Remarks>
    Private Shared Sub DumpHex(ByVal b() As Byte, _
                            Optional ByVal sPos As Integer = 0, _
                            Optional ByVal ePos As Integer = 0)
        Dim D As Decoder = Encoding.UTF8.GetDecoder()
        If ePos = 0 Then ePos = b.Length - 1
        Dim j As Integer
        Dim curB As Integer = sPos
        Dim sTmp As String
        For j = 0 To ePos - sPos
            If j Mod 16 = 0 Then
                Debug.WriteLine("")
                Debug.Write(Format(j + sPos, "000#") & "). ")
            End If
            If b(curB) < 16 Then
                sTmp = "0" & Hex(b(curB))
            Else
                sTmp = Hex(b(curB))
            End If
            Debug.Write(sTmp) : Debug.Write(" ")
            curB += 1
        Next
        Debug.WriteLine("")
    End Sub
#End Region
#End Region

#Region "   Debug & Test Routines -- commented out"

#Region "       Attribute dumper"
    'Private Shared Sub DumpPidlAtts(ByVal folder As IShellFolder, ByVal pidl As IntPtr, ByVal attr As SFGAO)
    '    Dim strr As IntPtr = Marshal.AllocCoTaskMem(MAX_PATH * 2 + 4)
    '    Marshal.WriteInt32(strr, 0, 0)
    '    Dim buf As New StringBuilder(260)
    '    Dim itemflags As SHGDN = SHGDN.FORPARSING
    '    folder.GetDisplayNameOf(pidl, itemflags, strr)
    '    Dim HR As Integer = StrRetToBuf(strr, pidl, buf, MAX_PATH)
    '    If buf.ToString.StartsWith("::") Then
    '        itemflags = SHGDN.FORADDRESSBAR
    '        Marshal.WriteInt32(strr, 0, 0)
    '        folder.GetDisplayNameOf(pidl, itemflags, strr)
    '        HR = StrRetToBuf(strr, pidl, buf, MAX_PATH)
    '    End If
    '    Marshal.FreeCoTaskMem(strr)     'now done with it
    '    Debug.WriteLine(vbCrLf & buf.ToString)
    '    Debug.WriteLine("SFGAO.FOLDER:  " & CBool((attr And SFGAO.FOLDER)))
    '    Debug.WriteLine("SFGAO.STREAM:  " & CBool((attr And SFGAO.STREAM)))
    '    Debug.WriteLine("SFGAO.HASSUBFOLDER:  " & CBool((attr And SFGAO.HASSUBFOLDER)))
    '    Debug.WriteLine("SFGAO.FILESYSANCESTOR:  " & CBool((attr And SFGAO.FILESYSANCESTOR)))
    '    'Debug.WriteLine("SFGAO.COMPRESSED:  " & CBool((attr And SFGAO.COMPRESSED)))
    'End Sub
#End Region

#End Region

#End Region

#Region "   cPidl Class"
    '''<Summary>cPile class contains a Byte() representation of a PIDL and
    ''' certain Methods and Properties for comparing one cPidl to another</Summary>
    Public Class cPidl
        Implements IEnumerable

#Region "       Private Fields"
        Dim m_bytes() As Byte   'The local copy of the PIDL
        Dim m_ItemCount As Integer      'the # of ItemIDs in this ItemIDList (PIDL)
#End Region

#Region "       Constructor"
        Sub New(ByVal pidl As IntPtr)
            Dim cb As Integer = ItemIDListSize(pidl)
            If cb > 0 Then
                ReDim m_bytes(cb + 1)
                Marshal.Copy(pidl, m_bytes, 0, cb + 1)
                'DumpPidl(pidl)
            Else
                ReDim m_bytes(0)  'This is the DeskTop (we hope)
            End If
            m_ItemCount = PidlCount(pidl)
        End Sub
#End Region

#Region "       Public Properties"
        Public ReadOnly Property PidlBytes() As Byte()
            Get
                Return m_bytes
            End Get
        End Property

        Public ReadOnly Property Length() As Integer
            Get
                Return m_bytes.Length
            End Get
        End Property

        Public ReadOnly Property ItemCount() As Integer
            Get
                Return m_ItemCount
            End Get
        End Property

#End Region

#Region "       Public Methods"

        '''<Summary>Returns True if input cPidl's content exactly match the 
        ''' contents of this instance</Summary>
        Public Function IsEqual(ByVal other As cPidl) As Boolean
            IsEqual = False     'assume not
            If other.Length <> Me.Length Then Exit Function
            Dim ob() As Byte = other.PidlBytes
            Dim i As Integer
            For i = 0 To Me.Length - 1  'note: we look at nulnul also
                If ob(i) <> m_bytes(i) Then Exit Function
            Next
            Return True         'all equal on fall thru
        End Function

        '''<Summary>returns True if the beginning of pidlA matches PidlB exactly for pidlB's entire length</Summary>
        Public Shared Function StartsWith(ByVal pidlA As IntPtr, ByVal pidlB As IntPtr) As Boolean
            Return cPidl.StartsWith(New cPidl(pidlA), New cPidl(pidlB))
        End Function

        '''<Summary>returns True if the beginning of A matches B exactly for B's entire length</Summary>
        Public Shared Function StartsWith(ByVal A As cPidl, ByVal B As cPidl) As Boolean
            Return A.StartsWith(B)
        End Function

        '''<Summary>Returns true if the CPidl input parameter exactly matches the
        ''' beginning of this instance of CPidl</Summary>
        Public Function StartsWith(ByVal cp As cPidl) As Boolean
            Dim b() As Byte = cp.PidlBytes
            If b.Length > m_bytes.Length Then Return False 'input is longer
            Dim i As Integer
            For i = 0 To b.Length - 3 'allow for nulnul at end of cp.PidlBytes
                If b(i) <> m_bytes(i) Then Return False
            Next
            Return True
        End Function
#End Region

#Region "       GetEnumerator"
        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return New ICPidlEnumerator(m_bytes)
        End Function
#End Region

#Region "       PIDL enumerator Class"
        Private Class ICPidlEnumerator
            Implements IEnumerator

            Private m_sPos As Integer   'the first index in the current PIDL
            Private m_ePos As Integer   'the last index in the current PIDL
            Private m_bytes() As Byte   'the local copy of the PIDL
            Private m_NotEmpty As Boolean = False 'the desktop PIDL is zero length

            Sub New(ByVal b() As Byte)
                m_bytes = b
                If b.Length > 0 Then m_NotEmpty = True
                m_sPos = -1 : m_ePos = -1
            End Sub

            Public ReadOnly Property Current() As Object Implements System.Collections.IEnumerator.Current
                Get
                    If m_sPos < 0 Then Throw New InvalidOperationException("ICPidlEnumerator --- attempt to get Current with invalidated list")
                    Dim b(m_ePos - m_sPos) As Byte
                    Array.Copy(m_bytes, m_sPos, b, 0, b.Length)
                    Return b
                End Get
            End Property

            Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
                If m_NotEmpty Then
                    If m_sPos < 0 Then
                        m_sPos = 0 : m_ePos = -1
                    Else
                        m_sPos = m_ePos + 1
                    End If
                    If m_bytes.Length < m_sPos + 1 Then Throw New InvalidCastException("Malformed PIDL")
                    Dim cb As Integer = m_bytes(m_sPos) + m_bytes(m_sPos + 1) * 256
                    If cb = 0 Then
                        Return False 'have passed all back
                    Else
                        m_ePos += cb
                    End If
                Else
                    m_sPos = 0 : m_ePos = 0
                    Return False        'in this case, we have exhausted the list of 0 ITEMIDs
                End If
                Return True
            End Function

            Public Sub Reset() Implements System.Collections.IEnumerator.Reset
                m_sPos = -1 : m_ePos = -1
            End Sub
        End Class
#End Region

    End Class
#End Region

End Class