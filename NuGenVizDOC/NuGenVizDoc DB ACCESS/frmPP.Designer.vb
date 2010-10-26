<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPP
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPP))
        Me.C1PrintPreviewControl1 = New C1.Win.C1Preview.C1PrintPreviewControl
        Me.C1PrintPreviewDialog1 = New C1.Win.C1Preview.C1PrintPreviewDialog
        CType(Me.C1PrintPreviewControl1.PreviewPane, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.C1PrintPreviewControl1.SuspendLayout()
        CType(Me.C1PrintPreviewDialog1.PrintPreviewControl.PreviewPane, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.C1PrintPreviewDialog1.PrintPreviewControl.SuspendLayout()
        Me.C1PrintPreviewDialog1.SuspendLayout()
        Me.SuspendLayout()
        '
        'C1PrintPreviewControl1
        '
        resources.ApplyResources(Me.C1PrintPreviewControl1, "C1PrintPreviewControl1")
        Me.C1PrintPreviewControl1.Name = "C1PrintPreviewControl1"
        '
        'C1PrintPreviewControl1.PreviewPane
        '
        Me.C1PrintPreviewControl1.PreviewPane.ExportOptions.Content = New C1.Win.C1Preview.ExporterOptions(-1) {}
        Me.C1PrintPreviewControl1.PreviewPane.IntegrateExternalTools = True
        Me.C1PrintPreviewControl1.PreviewPane.PrintingLayout = True
        resources.ApplyResources(Me.C1PrintPreviewControl1.PreviewPane, "C1PrintPreviewControl1.PreviewPane")
        '
        'C1PrintPreviewDialog1
        '
        resources.ApplyResources(Me.C1PrintPreviewDialog1, "C1PrintPreviewDialog1")
        Me.C1PrintPreviewDialog1.Name = "C1PrintPreviewDialog"
        '
        'C1PrintPreviewDialog1.PrintPreviewControl
        '
        '
        'C1PrintPreviewDialog1.PrintPreviewControl.PreviewPane
        '
        Me.C1PrintPreviewDialog1.PrintPreviewControl.PreviewPane.ExportOptions.Content = New C1.Win.C1Preview.ExporterOptions(-1) {}
        Me.C1PrintPreviewDialog1.PrintPreviewControl.PreviewPane.IntegrateExternalTools = True
        resources.ApplyResources(Me.C1PrintPreviewDialog1.PrintPreviewControl.PreviewPane, "C1PrintPreviewDialog1.PrintPreviewControl.PreviewPane")
        resources.ApplyResources(Me.C1PrintPreviewDialog1.PrintPreviewControl, "C1PrintPreviewDialog1.PrintPreviewControl")
        Me.C1PrintPreviewDialog1.SizeGripStyle = System.Windows.Forms.SizeGripStyle.[Auto]
        '
        'frmPP
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.C1PrintPreviewControl1)
        Me.DoubleBuffered = True
        Me.Name = "frmPP"
        CType(Me.C1PrintPreviewControl1.PreviewPane, System.ComponentModel.ISupportInitialize).EndInit()
        Me.C1PrintPreviewControl1.ResumeLayout(False)
        Me.C1PrintPreviewControl1.PerformLayout()
        CType(Me.C1PrintPreviewDialog1.PrintPreviewControl.PreviewPane, System.ComponentModel.ISupportInitialize).EndInit()
        Me.C1PrintPreviewDialog1.PrintPreviewControl.ResumeLayout(False)
        Me.C1PrintPreviewDialog1.PrintPreviewControl.PerformLayout()
        Me.C1PrintPreviewDialog1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents C1PrintPreviewControl1 As C1.Win.C1Preview.C1PrintPreviewControl
    Friend WithEvents C1PrintPreviewDialog1 As C1.Win.C1Preview.C1PrintPreviewDialog
End Class
