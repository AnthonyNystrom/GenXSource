Public Class PrintDemo
    Inherits System.Windows.Forms.Form

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
	Friend WithEvents Button1 As System.Windows.Forms.Button
	Friend WithEvents ServiceController1 As System.ServiceProcess.ServiceController
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.Button1 = New System.Windows.Forms.Button
		Me.ServiceController1 = New System.ServiceProcess.ServiceController
		Me.SuspendLayout()
		'
		'Button1
		'
		Me.Button1.Location = New System.Drawing.Point(8, 8)
		Me.Button1.Name = "Button1"
		Me.Button1.TabIndex = 2
		Me.Button1.Text = "Button1"
		'
		'PrintDemo
		'
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.ClientSize = New System.Drawing.Size(640, 430)
		Me.Controls.Add(Me.Button1)
		Me.Name = "PrintDemo"
		Me.Text = "PrintDemo"
		Me.ResumeLayout(False)

	End Sub

#End Region

	Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

	End Sub
End Class
