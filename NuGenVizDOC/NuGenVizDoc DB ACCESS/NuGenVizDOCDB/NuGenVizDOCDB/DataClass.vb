Imports System
Imports System.ComponentModel
Imports System.Collections
Imports System.Diagnostics
Imports System.IO
Imports System.Configuration
Imports C1.Data
Imports NuGenVizDOCDB.DataObjects






' C1SchemaDef container class: main class of the data library.
' Contains the C1SchemaDef component managing the data schema. 
Public Class DataClass
    Inherits System.ComponentModel.Component

#Region " Component Designer generated code "

    ' C1SchemaDef component managing the data schema.
    ' Needs to be public in order to allow access to the library.
    Public WithEvents SchemaDef1 As C1.Data.C1SchemaDef
    Public Sub New(ByVal Container As System.ComponentModel.IContainer)
        MyClass.New()
        'Required for Windows.Forms Class Composition Designer support
        Container.Add(Me)
    End Sub

    Public Sub New()
        MyBase.New()
        'This call is required by the Component Designer.
        InitializeComponent()
    End Sub

    'Component overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)

        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If

        MyBase.Dispose(disposing)
    End Sub

    Friend WithEvents table_TBLProjectCodeLanguage As C1.Data.C1TableLogic
    Friend WithEvents table_TBLProjectPlatform As C1.Data.C1TableLogic
    Friend WithEvents table_TBLProjects As C1.Data.C1TableLogic
    Friend WithEvents table_TBLProjectUILanguage As C1.Data.C1TableLogic
    Friend WithEvents dataset_DSTBLPROJECTS As C1.Data.C1DataSetLogic
    Friend WithEvents table_TBLStates As C1.Data.C1TableLogic
    Friend WithEvents table_TBLUsers As C1.Data.C1TableLogic

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer
    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DataClass))
        Me.SchemaDef1 = New C1.Data.C1SchemaDef
        Me.table_TBLProjectCodeLanguage = New C1.Data.C1TableLogic
        Me.table_TBLProjectPlatform = New C1.Data.C1TableLogic
        Me.table_TBLProjects = New C1.Data.C1TableLogic
        Me.table_TBLProjectUILanguage = New C1.Data.C1TableLogic
        Me.dataset_DSTBLPROJECTS = New C1.Data.C1DataSetLogic
        Me.table_TBLStates = New C1.Data.C1TableLogic
        Me.table_TBLUsers = New C1.Data.C1TableLogic
        CType(Me.SchemaDef1, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'SchemaDef1
        '
        Me.SchemaDef1.DataObjectsAssemblyFlags = CType((C1.Data.DataObjectsAssemblyFlags.DataSetNamespaces Or C1.Data.DataObjectsAssemblyFlags.StaticNameFields), C1.Data.DataObjectsAssemblyFlags)
        Me.SchemaDef1.DummyImage = CType(resources.GetObject("SchemaDef1.DummyImage"), System.Drawing.Bitmap)
        Me.SchemaDef1.SerializedSchemaXML = Me.SchemaDef1.GetSchemaFromResources(resources, "SchemaDef1", Me)
        Me.SchemaDef1.SerializedDesignTimeOptionsXML = Me.SchemaDef1.GetDesignTimeOptionsFromResources(resources, "SchemaDef1", Me)
        '
        'table_TBLProjectCodeLanguage
        '
        Me.table_TBLProjectCodeLanguage.SchemaComponent = Me.SchemaDef1
        Me.table_TBLProjectCodeLanguage.SchemaPersist = Nothing
        Me.table_TBLProjectCodeLanguage.Table = "TBLProjectCodeLanguage"
        '
        'table_TBLProjectPlatform
        '
        Me.table_TBLProjectPlatform.SchemaComponent = Me.SchemaDef1
        Me.table_TBLProjectPlatform.SchemaPersist = Nothing
        Me.table_TBLProjectPlatform.Table = "TBLProjectPlatform"
        '
        'table_TBLProjects
        '
        Me.table_TBLProjects.SchemaComponent = Me.SchemaDef1
        Me.table_TBLProjects.SchemaPersist = Nothing
        Me.table_TBLProjects.Table = "TBLProjects"
        '
        'table_TBLProjectUILanguage
        '
        Me.table_TBLProjectUILanguage.SchemaComponent = Me.SchemaDef1
        Me.table_TBLProjectUILanguage.SchemaPersist = Nothing
        Me.table_TBLProjectUILanguage.Table = "TBLProjectUILanguage"
        '
        'dataset_DSTBLPROJECTS
        '
        Me.dataset_DSTBLPROJECTS.DataSetDef = "DSTBLPROJECTS"
        Me.dataset_DSTBLPROJECTS.SchemaComponent = Me.SchemaDef1
        Me.dataset_DSTBLPROJECTS.SchemaPersist = Nothing
        '
        'table_TBLStates
        '
        Me.table_TBLStates.SchemaComponent = Me.SchemaDef1
        Me.table_TBLStates.SchemaPersist = Nothing
        Me.table_TBLStates.Table = "TBLStates"
        '
        'table_TBLUsers
        '
        Me.table_TBLUsers.SchemaComponent = Me.SchemaDef1
        Me.table_TBLUsers.SchemaPersist = Nothing
        Me.table_TBLUsers.Table = "TBLUsers"
        CType(Me.SchemaDef1, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

#End Region


    Private Sub SchemaDef1_CreateSchema(ByVal sender As Object, ByVal e As System.EventArgs) Handles SchemaDef1.CreateSchema
        Dim commonpath As String = System.IO.Directory.GetCurrentDirectory
        'MsgBox("Provider=Microsoft.Jet.OLEDB.4.0;Password="";Data Source=" & commonpath & "\NuGenVizDocDB.mdb;Persist Security Info=True")
        SchemaDef1.Schema.Connections("Connection").ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & commonpath & "\NuGenVizDocDB.mdb;Persist Security Info=True"
        'Provider=Microsoft.Jet.OLEDB.4.0;Password="";Data Source=C:\Documents and Settings\Anthony Nystrom\Desktop\NuGenVizDoc\bin\NuGenVizDocDB.mdb;Persist Security Info=True
        commonpath = Nothing
    End Sub
End Class
