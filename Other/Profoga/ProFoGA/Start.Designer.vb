<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Start
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Start))
        Me.pnlMakeProtein = New System.Windows.Forms.Panel
        Me.splMakeProtein = New System.Windows.Forms.SplitContainer
        Me.lbProtein = New System.Windows.Forms.ListBox
        Me.lbAminoacids = New System.Windows.Forms.ListBox
        Me.btnDone = New System.Windows.Forms.Button
        Me.lblPopulation = New System.Windows.Forms.Label
        Me.numPopulation = New System.Windows.Forms.NumericUpDown
        Me.pnlMakeProtein.SuspendLayout()
        Me.splMakeProtein.Panel1.SuspendLayout()
        Me.splMakeProtein.Panel2.SuspendLayout()
        Me.splMakeProtein.SuspendLayout()
        CType(Me.numPopulation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlMakeProtein
        '
        Me.pnlMakeProtein.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlMakeProtein.Controls.Add(Me.splMakeProtein)
        Me.pnlMakeProtein.Location = New System.Drawing.Point(12, 12)
        Me.pnlMakeProtein.Name = "pnlMakeProtein"
        Me.pnlMakeProtein.Size = New System.Drawing.Size(432, 391)
        Me.pnlMakeProtein.TabIndex = 1
        '
        'splMakeProtein
        '
        Me.splMakeProtein.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splMakeProtein.Location = New System.Drawing.Point(0, 0)
        Me.splMakeProtein.Name = "splMakeProtein"
        '
        'splMakeProtein.Panel1
        '
        Me.splMakeProtein.Panel1.Controls.Add(Me.lbProtein)
        '
        'splMakeProtein.Panel2
        '
        Me.splMakeProtein.Panel2.Controls.Add(Me.lbAminoacids)
        Me.splMakeProtein.Size = New System.Drawing.Size(432, 391)
        Me.splMakeProtein.SplitterDistance = 144
        Me.splMakeProtein.TabIndex = 1
        '
        'lbProtein
        '
        Me.lbProtein.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbProtein.FormattingEnabled = True
        Me.lbProtein.Location = New System.Drawing.Point(0, 0)
        Me.lbProtein.Name = "lbProtein"
        Me.lbProtein.Size = New System.Drawing.Size(144, 381)
        Me.lbProtein.TabIndex = 0
        '
        'lbAminoacids
        '
        Me.lbAminoacids.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbAminoacids.FormattingEnabled = True
        Me.lbAminoacids.Items.AddRange(New Object() {"Ala", "Arg", "Asn", "Asp", "Cys", "Gln", "Glu", "Gly", "His", "Ile", "Leu", "Lys", "Met", "Phe", "Pro", "Ser", "Thr", "Trp", "Tyr", "Val"})
        Me.lbAminoacids.Location = New System.Drawing.Point(0, 0)
        Me.lbAminoacids.Name = "lbAminoacids"
        Me.lbAminoacids.Size = New System.Drawing.Size(284, 381)
        Me.lbAminoacids.TabIndex = 0
        '
        'btnDone
        '
        Me.btnDone.Location = New System.Drawing.Point(369, 408)
        Me.btnDone.Name = "btnDone"
        Me.btnDone.Size = New System.Drawing.Size(75, 23)
        Me.btnDone.TabIndex = 2
        Me.btnDone.Text = "&Done"
        Me.btnDone.UseVisualStyleBackColor = True
        '
        'lblPopulation
        '
        Me.lblPopulation.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblPopulation.AutoSize = True
        Me.lblPopulation.Location = New System.Drawing.Point(12, 413)
        Me.lblPopulation.Name = "lblPopulation"
        Me.lblPopulation.Size = New System.Drawing.Size(60, 13)
        Me.lblPopulation.TabIndex = 4
        Me.lblPopulation.Text = "Population:"
        '
        'numPopulation
        '
        Me.numPopulation.DataBindings.Add(New System.Windows.Forms.Binding("Value", Global.TerraSoft.BioTech.ProFoGA.My.MySettings.Default, "PopulationSize", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.numPopulation.Location = New System.Drawing.Point(78, 409)
        Me.numPopulation.Name = "numPopulation"
        Me.numPopulation.Size = New System.Drawing.Size(78, 20)
        Me.numPopulation.TabIndex = 3
        Me.numPopulation.Value = Global.TerraSoft.BioTech.ProFoGA.My.MySettings.Default.PopulationSize
        '
        'Start
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(456, 443)
        Me.Controls.Add(Me.lblPopulation)
        Me.Controls.Add(Me.numPopulation)
        Me.Controls.Add(Me.btnDone)
        Me.Controls.Add(Me.pnlMakeProtein)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Start"
        Me.Text = "Compose Protein"
        Me.pnlMakeProtein.ResumeLayout(False)
        Me.splMakeProtein.Panel1.ResumeLayout(False)
        Me.splMakeProtein.Panel2.ResumeLayout(False)
        Me.splMakeProtein.ResumeLayout(False)
        CType(Me.numPopulation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlMakeProtein As System.Windows.Forms.Panel
    Friend WithEvents splMakeProtein As System.Windows.Forms.SplitContainer
    Friend WithEvents lbProtein As System.Windows.Forms.ListBox
    Friend WithEvents lbAminoacids As System.Windows.Forms.ListBox
    Friend WithEvents btnDone As System.Windows.Forms.Button
    Friend WithEvents numPopulation As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblPopulation As System.Windows.Forms.Label
End Class
