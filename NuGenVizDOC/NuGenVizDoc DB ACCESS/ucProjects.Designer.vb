<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucProjects
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim GridEXLayout1 As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucProjects))
        Me.DSTBLPROJECTS = New C1.Data.C1DataSet
        Me.GridEX1 = New Janus.Windows.GridEX.GridEX
        Me.TSTBLPROJECTS = New C1.Data.C1DataTableSource
        CType(Me.DSTBLPROJECTS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridEX1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DSTBLPROJECTS
        '
        Me.DSTBLPROJECTS.CaseSensitive = False
        Me.DSTBLPROJECTS.DataLibrary = "NuGenVizDOCDB"
        Me.DSTBLPROJECTS.DataLibraryUrl = ""
        Me.DSTBLPROJECTS.DataSetDef = "DSTBLPROJECTS"
        Me.DSTBLPROJECTS.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DSTBLPROJECTS.Name = "DSTBLPROJECTS"
        Me.DSTBLPROJECTS.SchemaClassName = "NuGenVizDOCDB.DataClass"
        Me.DSTBLPROJECTS.SchemaDef = Nothing
        '
        'GridEX1
        '
        Me.GridEX1.AlternatingColors = True
        Me.GridEX1.Cursor = System.Windows.Forms.Cursors.Default
        Me.GridEX1.DataSource = Me.TSTBLPROJECTS
        GridEXLayout1.LayoutString = resources.GetString("GridEXLayout1.LayoutString")
        Me.GridEX1.DesignTimeLayout = GridEXLayout1
        Me.GridEX1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridEX1.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular
        Me.GridEX1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GridEX1.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003
        Me.GridEX1.GroupTotals = Janus.Windows.GridEX.GroupTotals.[Default]
        Me.GridEX1.Location = New System.Drawing.Point(0, 0)
        Me.GridEX1.Name = "GridEX1"
        Me.GridEX1.RecordNavigator = True
        Me.GridEX1.Size = New System.Drawing.Size(498, 583)
        Me.GridEX1.TabIndex = 0
        Me.GridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        Me.GridEX1.VisualStyleAreas.CardsStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        Me.GridEX1.VisualStyleAreas.CheckBoxStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        Me.GridEX1.VisualStyleAreas.ControlBorderStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        Me.GridEX1.VisualStyleAreas.EditControlsStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        Me.GridEX1.VisualStyleAreas.GroupByBoxStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        Me.GridEX1.VisualStyleAreas.GroupRowsStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        Me.GridEX1.VisualStyleAreas.HeadersStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        Me.GridEX1.VisualStyleAreas.ScrollBarsStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        Me.GridEX1.VisualStyleAreas.TreeGliphsStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        '
        'TSTBLPROJECTS
        '
        Me.TSTBLPROJECTS.DataSet = Me.DSTBLPROJECTS
        Me.TSTBLPROJECTS.TableView = "TBLProjects"
        '
        'ucProjects
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GridEX1)
        Me.Name = "ucProjects"
        Me.Size = New System.Drawing.Size(498, 583)
        CType(Me.DSTBLPROJECTS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridEX1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GridEX1 As Janus.Windows.GridEX.GridEX
    Friend WithEvents TSTBLPROJECTS As C1.Data.C1DataTableSource
    Friend WithEvents DSTBLPROJECTS As C1.Data.C1DataSet

End Class
