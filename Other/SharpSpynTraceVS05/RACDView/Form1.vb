Public Class Form1
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
    Friend WithEvents nuGenOInternal1 As Genetibase.Debug.NuGenOInternal
    Friend WithEvents btnExceptionTest As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents NuGenOInternal2 As Genetibase.Debug.NuGenOInternal
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnExceptionTest = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.NuGenOInternal2 = New Genetibase.Debug.NuGenOInternal
        Me.SuspendLayout()
        '
        'btnExceptionTest
        '
        Me.btnExceptionTest.Location = New System.Drawing.Point(96, 448)
        Me.btnExceptionTest.Name = "btnExceptionTest"
        Me.btnExceptionTest.Size = New System.Drawing.Size(128, 23)
        Me.btnExceptionTest.TabIndex = 8
        Me.btnExceptionTest.Text = "Exception Test"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(16, 448)
        Me.Button2.Name = "Button2"
        Me.Button2.TabIndex = 7
        Me.Button2.Text = "Button2"
        '
        'NuGenOInternal2
        '
        Me.NuGenOInternal2.Location = New System.Drawing.Point(8, 8)
        Me.NuGenOInternal2.Name = "NuGenOInternal2"
        Me.NuGenOInternal2.Size = New System.Drawing.Size(680, 344)
        Me.NuGenOInternal2.TabIndex = 9
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(736, 566)
        Me.Controls.Add(Me.NuGenOInternal2)
        Me.Controls.Add(Me.btnExceptionTest)
        Me.Controls.Add(Me.Button2)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

#End Region
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click


        Trace.WriteLine("lkjwerlwkjer")
        Debug.Assert(True, "message", "message")

    End Sub



    Private Sub btnExceptionTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExceptionTest.Click
        Try
            Dim f As Form1
            f.Show()

        Catch eo As Exception
            Debug.Assert(False, eo.ToString, eo.ToString)
        End Try
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.NuGenOInternal2.StartLogging()
    End Sub
End Class
