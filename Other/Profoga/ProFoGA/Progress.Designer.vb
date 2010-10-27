<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Progress
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Progress))
        Me.pnlPopulation = New System.Windows.Forms.Panel
        Me.lbPopulation = New System.Windows.Forms.ListBox
        Me.btnGrow = New System.Windows.Forms.Button
        Me.btnKill = New System.Windows.Forms.Button
        Me.ssPopulation = New System.Windows.Forms.StatusStrip
        Me.lblPopulation = New System.Windows.Forms.ToolStripStatusLabel
        Me.lblPopulationNumber = New System.Windows.Forms.ToolStripStatusLabel
        Me.lblGeneration = New System.Windows.Forms.ToolStripStatusLabel
        Me.lblGenerationNumber = New System.Windows.Forms.ToolStripStatusLabel
        Me.lblMaxFitness = New System.Windows.Forms.ToolStripStatusLabel
        Me.lblMaxFitnessNumber = New System.Windows.Forms.ToolStripStatusLabel
        Me.lblAvgFitness = New System.Windows.Forms.ToolStripStatusLabel
        Me.lblAvgFitnessNumber = New System.Windows.Forms.ToolStripStatusLabel
        Me.btnDisplay = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnStep = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.dlgExport = New System.Windows.Forms.SaveFileDialog
        Me.pnlPopulation.SuspendLayout()
        Me.ssPopulation.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlPopulation
        '
        Me.pnlPopulation.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlPopulation.Controls.Add(Me.lbPopulation)
        Me.pnlPopulation.Location = New System.Drawing.Point(13, 13)
        Me.pnlPopulation.Name = "pnlPopulation"
        Me.pnlPopulation.Size = New System.Drawing.Size(431, 347)
        Me.pnlPopulation.TabIndex = 0
        '
        'lbPopulation
        '
        Me.lbPopulation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbPopulation.FormattingEnabled = True
        Me.lbPopulation.Location = New System.Drawing.Point(0, 0)
        Me.lbPopulation.Name = "lbPopulation"
        Me.lbPopulation.Size = New System.Drawing.Size(431, 342)
        Me.lbPopulation.TabIndex = 0
        '
        'btnGrow
        '
        Me.btnGrow.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGrow.Location = New System.Drawing.Point(450, 12)
        Me.btnGrow.Name = "btnGrow"
        Me.btnGrow.Size = New System.Drawing.Size(75, 23)
        Me.btnGrow.TabIndex = 1
        Me.btnGrow.Text = "&Grow"
        Me.btnGrow.UseVisualStyleBackColor = True
        '
        'btnKill
        '
        Me.btnKill.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnKill.Location = New System.Drawing.Point(450, 41)
        Me.btnKill.Name = "btnKill"
        Me.btnKill.Size = New System.Drawing.Size(75, 23)
        Me.btnKill.TabIndex = 2
        Me.btnKill.Text = "&Kill"
        Me.btnKill.UseVisualStyleBackColor = True
        '
        'ssPopulation
        '
        Me.ssPopulation.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblPopulation, Me.lblPopulationNumber, Me.lblGeneration, Me.lblGenerationNumber, Me.lblMaxFitness, Me.lblMaxFitnessNumber, Me.lblAvgFitness, Me.lblAvgFitnessNumber})
        Me.ssPopulation.Location = New System.Drawing.Point(0, 363)
        Me.ssPopulation.Name = "ssPopulation"
        Me.ssPopulation.Size = New System.Drawing.Size(537, 22)
        Me.ssPopulation.TabIndex = 3
        '
        'lblPopulation
        '
        Me.lblPopulation.Name = "lblPopulation"
        Me.lblPopulation.Size = New System.Drawing.Size(61, 17)
        Me.lblPopulation.Text = "Population:"
        '
        'lblPopulationNumber
        '
        Me.lblPopulationNumber.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblPopulationNumber.Name = "lblPopulationNumber"
        Me.lblPopulationNumber.Size = New System.Drawing.Size(14, 17)
        Me.lblPopulationNumber.Text = "0"
        '
        'lblGeneration
        '
        Me.lblGeneration.Name = "lblGeneration"
        Me.lblGeneration.Size = New System.Drawing.Size(60, 17)
        Me.lblGeneration.Text = "Generation"
        '
        'lblGenerationNumber
        '
        Me.lblGenerationNumber.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblGenerationNumber.Name = "lblGenerationNumber"
        Me.lblGenerationNumber.Size = New System.Drawing.Size(14, 17)
        Me.lblGenerationNumber.Text = "0"
        '
        'lblMaxFitness
        '
        Me.lblMaxFitness.Name = "lblMaxFitness"
        Me.lblMaxFitness.Size = New System.Drawing.Size(31, 17)
        Me.lblMaxFitness.Text = "Max:"
        '
        'lblMaxFitnessNumber
        '
        Me.lblMaxFitnessNumber.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblMaxFitnessNumber.Name = "lblMaxFitnessNumber"
        Me.lblMaxFitnessNumber.Size = New System.Drawing.Size(14, 17)
        Me.lblMaxFitnessNumber.Text = "0"
        '
        'lblAvgFitness
        '
        Me.lblAvgFitness.Name = "lblAvgFitness"
        Me.lblAvgFitness.Size = New System.Drawing.Size(30, 17)
        Me.lblAvgFitness.Text = "Avg:"
        '
        'lblAvgFitnessNumber
        '
        Me.lblAvgFitnessNumber.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblAvgFitnessNumber.Name = "lblAvgFitnessNumber"
        Me.lblAvgFitnessNumber.Size = New System.Drawing.Size(14, 17)
        Me.lblAvgFitnessNumber.Text = "0"
        '
        'btnDisplay
        '
        Me.btnDisplay.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDisplay.Location = New System.Drawing.Point(450, 308)
        Me.btnDisplay.Name = "btnDisplay"
        Me.btnDisplay.Size = New System.Drawing.Size(75, 23)
        Me.btnDisplay.TabIndex = 4
        Me.btnDisplay.Text = "&Display"
        Me.btnDisplay.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExit.Location = New System.Drawing.Point(450, 337)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 23)
        Me.btnExit.TabIndex = 5
        Me.btnExit.Text = "E&xit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnStep
        '
        Me.btnStep.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStep.Location = New System.Drawing.Point(450, 70)
        Me.btnStep.Name = "btnStep"
        Me.btnStep.Size = New System.Drawing.Size(75, 23)
        Me.btnStep.TabIndex = 6
        Me.btnStep.Text = "&Step"
        Me.btnStep.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExport.Location = New System.Drawing.Point(450, 279)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(75, 23)
        Me.btnExport.TabIndex = 7
        Me.btnExport.Text = "&Export"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'dlgExport
        '
        Me.dlgExport.DefaultExt = "xml"
        Me.dlgExport.FileName = "output.xml"
        Me.dlgExport.Filter = "XML file|*.xml"
        '
        'Progress
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(537, 385)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnStep)
        Me.Controls.Add(Me.ssPopulation)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnDisplay)
        Me.Controls.Add(Me.btnKill)
        Me.Controls.Add(Me.btnGrow)
        Me.Controls.Add(Me.pnlPopulation)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(280, 240)
        Me.Name = "Progress"
        Me.Text = "Progress"
        Me.pnlPopulation.ResumeLayout(False)
        Me.ssPopulation.ResumeLayout(False)
        Me.ssPopulation.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlPopulation As System.Windows.Forms.Panel
    Friend WithEvents btnGrow As System.Windows.Forms.Button
    Friend WithEvents btnKill As System.Windows.Forms.Button
    Friend WithEvents ssPopulation As System.Windows.Forms.StatusStrip
    Friend WithEvents lblPopulation As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblMaxFitness As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblAvgFitness As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents btnDisplay As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents lbPopulation As System.Windows.Forms.ListBox
    Friend WithEvents lblPopulationNumber As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblMaxFitnessNumber As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblAvgFitnessNumber As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents btnStep As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents lblGeneration As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblGenerationNumber As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents dlgExport As System.Windows.Forms.SaveFileDialog
End Class
