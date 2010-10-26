Imports System
Imports System.ComponentModel
Imports System.Collections
Imports System.Diagnostics
Imports C1.Data
Imports TESTDB.DataObjects

' C1SchemaDef container class: main class of the data library.
' Contains the C1SchemaDef component managing the data schema. 
Public Class DataClass
    Inherits System.ComponentModel.Component

#Region " Component Designer generated code "

    ' C1SchemaDef component managing the data schema.
    ' Needs to be public in order to allow access to the library.
    Public WithEvents SchemaDef1 As C1.Data.C1SchemaDef

    Public Sub New(Container As System.ComponentModel.IContainer)
        MyClass.New()

        'Required for Windows.Forms Class Composition Designer support
        Container.Add(me)
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

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.SchemaDef1 = New C1.Data.C1SchemaDef()
            Me.SchemaDef1.DataObjectsAssemblyFlags = C1.Data.DataObjectsAssemblyFlags.All
    End Sub

#End Region

End Class
