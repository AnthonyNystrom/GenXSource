<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProjectCE
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        Disposer()
        Application.DoEvents()
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim GridEXLayout1 As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProjectCE))
        Me.TSTBLPROJECTS = New C1.Data.C1DataTableSource
        Me.DSTBLPROJECTS = New C1.Data.C1DataSet
        Me.UiGroupBox1 = New Janus.Windows.EditControls.UIGroupBox
        Me.GridEX1 = New Janus.Windows.GridEX.GridEX
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DeleteProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CalendarCombo2 = New Janus.Windows.CalendarCombo.CalendarCombo
        Me.CalendarCombo1 = New Janus.Windows.CalendarCombo.CalendarCombo
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Scribble1 = New Agilix.Ink.Scribble.Scribble
        Me.EditBox7 = New Janus.Windows.GridEX.EditControls.EditBox
        Me.EditBox6 = New Janus.Windows.GridEX.EditControls.EditBox
        Me.EditBox5 = New Janus.Windows.GridEX.EditControls.EditBox
        Me.EditBox4 = New Janus.Windows.GridEX.EditControls.EditBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.EditBox3 = New Janus.Windows.GridEX.EditControls.EditBox
        Me.EditBox2 = New Janus.Windows.GridEX.EditControls.EditBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.UiButton2 = New Janus.Windows.EditControls.UIButton
        Me.UiButton5 = New Janus.Windows.EditControls.UIButton
        Me.UiButton4 = New Janus.Windows.EditControls.UIButton
        Me.UiButton3 = New Janus.Windows.EditControls.UIButton
        Me.UiButton1 = New Janus.Windows.EditControls.UIButton
        CType(Me.DSTBLPROJECTS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UiGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UiGroupBox1.SuspendLayout()
        CType(Me.GridEX1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TSTBLPROJECTS
        '
        Me.TSTBLPROJECTS.DataSet = Me.DSTBLPROJECTS
        Me.TSTBLPROJECTS.TableView = "TBLProjects"
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
        'UiGroupBox1
        '
        Me.UiGroupBox1.Controls.Add(Me.GridEX1)
        Me.UiGroupBox1.Controls.Add(Me.CalendarCombo2)
        Me.UiGroupBox1.Controls.Add(Me.CalendarCombo1)
        Me.UiGroupBox1.Controls.Add(Me.Label9)
        Me.UiGroupBox1.Controls.Add(Me.Label8)
        Me.UiGroupBox1.Controls.Add(Me.Scribble1)
        Me.UiGroupBox1.Controls.Add(Me.EditBox7)
        Me.UiGroupBox1.Controls.Add(Me.EditBox6)
        Me.UiGroupBox1.Controls.Add(Me.EditBox5)
        Me.UiGroupBox1.Controls.Add(Me.EditBox4)
        Me.UiGroupBox1.Controls.Add(Me.Label7)
        Me.UiGroupBox1.Controls.Add(Me.Label6)
        Me.UiGroupBox1.Controls.Add(Me.Label5)
        Me.UiGroupBox1.Controls.Add(Me.Label4)
        Me.UiGroupBox1.Controls.Add(Me.Label3)
        Me.UiGroupBox1.Controls.Add(Me.Label2)
        Me.UiGroupBox1.Controls.Add(Me.EditBox3)
        Me.UiGroupBox1.Controls.Add(Me.EditBox2)
        Me.UiGroupBox1.Controls.Add(Me.Panel1)
        resources.ApplyResources(Me.UiGroupBox1, "UiGroupBox1")
        Me.UiGroupBox1.FrameStyle = Janus.Windows.EditControls.FrameStyle.None
        Me.UiGroupBox1.Name = "UiGroupBox1"
        Me.UiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2003
        '
        'GridEX1
        '
        Me.GridEX1.AllowDelete = Janus.Windows.GridEX.InheritableBoolean.[True]
        Me.GridEX1.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.[False]
        Me.GridEX1.AlternatingColors = True
        Me.GridEX1.CardWidth = 288
        Me.GridEX1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.GridEX1.DataSource = Me.TSTBLPROJECTS
        resources.ApplyResources(Me.GridEX1, "GridEX1")
        GridEXLayout1.DataSource = Me.TSTBLPROJECTS
        GridEXLayout1.IsCurrentLayout = True
        GridEXLayout1.Key = "NuGenVizDOC_CV"
        resources.ApplyResources(GridEXLayout1, "GridEXLayout1")
        Me.GridEX1.Layouts.AddRange(New Janus.Windows.GridEX.GridEXLayout() {GridEXLayout1})
        Me.GridEX1.Name = "GridEX1"
        Me.GridEX1.SaveSettings = False
        Me.GridEX1.View = Janus.Windows.GridEX.View.CardView
        Me.GridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeleteProjectToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        resources.ApplyResources(Me.ContextMenuStrip1, "ContextMenuStrip1")
        '
        'DeleteProjectToolStripMenuItem
        '
        Me.DeleteProjectToolStripMenuItem.Name = "DeleteProjectToolStripMenuItem"
        resources.ApplyResources(Me.DeleteProjectToolStripMenuItem, "DeleteProjectToolStripMenuItem")
        '
        'CalendarCombo2
        '
        Me.CalendarCombo2.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.TSTBLPROJECTS, "datEntDate", True))
        resources.ApplyResources(Me.CalendarCombo2, "CalendarCombo2")
        '
        '
        '
        Me.CalendarCombo2.DropDownCalendar.FirstMonth = New Date(2006, 4, 1, 0, 0, 0, 0)
        Me.CalendarCombo2.DropDownCalendar.Name = ""
        Me.CalendarCombo2.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2003
        Me.CalendarCombo2.Name = "CalendarCombo2"
        Me.CalendarCombo2.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2003
        '
        'CalendarCombo1
        '
        Me.CalendarCombo1.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.TSTBLPROJECTS, "datEtmStart", True))
        resources.ApplyResources(Me.CalendarCombo1, "CalendarCombo1")
        '
        '
        '
        Me.CalendarCombo1.DropDownCalendar.FirstMonth = New Date(2006, 4, 1, 0, 0, 0, 0)
        Me.CalendarCombo1.DropDownCalendar.Name = ""
        Me.CalendarCombo1.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2003
        Me.CalendarCombo1.Name = "CalendarCombo1"
        Me.CalendarCombo1.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2003
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        '
        'Scribble1
        '
        Me.Scribble1.AllowDrop = True
        resources.ApplyResources(Me.Scribble1, "Scribble1")
        Me.Scribble1.BackColor = System.Drawing.Color.White
        Me.Scribble1.DataBindings.Add(New System.Windows.Forms.Binding("DocumentData", Me.TSTBLPROJECTS, "memDocument", True))
        Me.Scribble1.ForceSwapFont = New System.Drawing.Font("Times New Roman", 10.0!)
        Me.Scribble1.Name = "Scribble1"
        '
        'EditBox7
        '
        Me.EditBox7.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TSTBLPROJECTS, "intProjectCodeLanguageKey", True))
        resources.ApplyResources(Me.EditBox7, "EditBox7")
        Me.EditBox7.Name = "EditBox7"
        Me.EditBox7.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
        Me.EditBox7.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        '
        'EditBox6
        '
        Me.EditBox6.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TSTBLPROJECTS, "intProjectUILanguageKey", True))
        resources.ApplyResources(Me.EditBox6, "EditBox6")
        Me.EditBox6.Name = "EditBox6"
        Me.EditBox6.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
        Me.EditBox6.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        '
        'EditBox5
        '
        Me.EditBox5.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TSTBLPROJECTS, "intProjectPlatformKey", True))
        resources.ApplyResources(Me.EditBox5, "EditBox5")
        Me.EditBox5.Name = "EditBox5"
        Me.EditBox5.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
        Me.EditBox5.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        '
        'EditBox4
        '
        Me.EditBox4.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TSTBLPROJECTS, "memProjectNotes", True))
        resources.ApplyResources(Me.EditBox4, "EditBox4")
        Me.EditBox4.Multiline = True
        Me.EditBox4.Name = "EditBox4"
        Me.EditBox4.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
        Me.EditBox4.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'EditBox3
        '
        Me.EditBox3.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TSTBLPROJECTS, "strProjectVersion", True))
        resources.ApplyResources(Me.EditBox3, "EditBox3")
        Me.EditBox3.Name = "EditBox3"
        Me.EditBox3.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
        Me.EditBox3.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        '
        'EditBox2
        '
        Me.EditBox2.BackColor = System.Drawing.SystemColors.Info
        Me.EditBox2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TSTBLPROJECTS, "strProjectIdentity", True))
        resources.ApplyResources(Me.EditBox2, "EditBox2")
        Me.EditBox2.Name = "EditBox2"
        Me.EditBox2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
        Me.EditBox2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.UiButton2)
        Me.Panel1.Controls.Add(Me.UiButton5)
        Me.Panel1.Controls.Add(Me.UiButton4)
        Me.Panel1.Controls.Add(Me.UiButton3)
        Me.Panel1.Controls.Add(Me.UiButton1)
        resources.ApplyResources(Me.Panel1, "Panel1")
        Me.Panel1.Name = "Panel1"
        '
        'UiButton2
        '
        resources.ApplyResources(Me.UiButton2, "UiButton2")
        Me.UiButton2.Name = "UiButton2"
        Me.UiButton2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003
        '
        'UiButton5
        '
        resources.ApplyResources(Me.UiButton5, "UiButton5")
        Me.UiButton5.Name = "UiButton5"
        Me.UiButton5.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003
        '
        'UiButton4
        '
        resources.ApplyResources(Me.UiButton4, "UiButton4")
        Me.UiButton4.Name = "UiButton4"
        Me.UiButton4.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003
        '
        'UiButton3
        '
        resources.ApplyResources(Me.UiButton3, "UiButton3")
        Me.UiButton3.Name = "UiButton3"
        Me.UiButton3.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003
        '
        'UiButton1
        '
        resources.ApplyResources(Me.UiButton1, "UiButton1")
        Me.UiButton1.Name = "UiButton1"
        Me.UiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003
        '
        'frmProjectCE
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ControlBox = False
        Me.Controls.Add(Me.UiGroupBox1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmProjectCE"
        Me.ShowInTaskbar = False
        CType(Me.DSTBLPROJECTS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UiGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UiGroupBox1.ResumeLayout(False)
        Me.UiGroupBox1.PerformLayout()
        CType(Me.GridEX1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DSTBLPROJECTS As C1.Data.C1DataSet
    Friend WithEvents TSTBLPROJECTS As C1.Data.C1DataTableSource
    Friend WithEvents UiGroupBox1 As Janus.Windows.EditControls.UIGroupBox
    Friend WithEvents UiButton4 As Janus.Windows.EditControls.UIButton
    Friend WithEvents UiButton3 As Janus.Windows.EditControls.UIButton
    Friend WithEvents UiButton1 As Janus.Windows.EditControls.UIButton
    Friend WithEvents EditBox3 As Janus.Windows.GridEX.EditControls.EditBox
    Friend WithEvents EditBox2 As Janus.Windows.GridEX.EditControls.EditBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents EditBox4 As Janus.Windows.GridEX.EditControls.EditBox
    Friend WithEvents Scribble1 As Agilix.Ink.Scribble.Scribble
    Friend WithEvents UiButton2 As Janus.Windows.EditControls.UIButton
    Friend WithEvents EditBox7 As Janus.Windows.GridEX.EditControls.EditBox
    Friend WithEvents EditBox6 As Janus.Windows.GridEX.EditControls.EditBox
    Friend WithEvents EditBox5 As Janus.Windows.GridEX.EditControls.EditBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents DeleteProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents UiButton5 As Janus.Windows.EditControls.UIButton
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents CalendarCombo2 As Janus.Windows.CalendarCombo.CalendarCombo
    Friend WithEvents CalendarCombo1 As Janus.Windows.CalendarCombo.CalendarCombo
    Friend WithEvents GridEX1 As Janus.Windows.GridEX.GridEX
End Class
