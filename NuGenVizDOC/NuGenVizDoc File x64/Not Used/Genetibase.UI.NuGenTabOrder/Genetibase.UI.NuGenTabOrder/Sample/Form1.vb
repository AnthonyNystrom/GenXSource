
Imports Genetibase.UI
Public Class Form1
    Inherits System.Windows.Forms.Form


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Dim tom = New TabOrder(Me)
        tom.SetTabOrder(TabOrder.TabScheme.AcrossFirst)

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
    Friend WithEvents tabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents groupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents textBox13 As System.Windows.Forms.TextBox
    Friend WithEvents textBox14 As System.Windows.Forms.TextBox
    Friend WithEvents textBox15 As System.Windows.Forms.TextBox
    Friend WithEvents textBox16 As System.Windows.Forms.TextBox
    Friend WithEvents textBox17 As System.Windows.Forms.TextBox
    Friend WithEvents textBox18 As System.Windows.Forms.TextBox
    Friend WithEvents tabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents textBox19 As System.Windows.Forms.TextBox
    Friend WithEvents textBox20 As System.Windows.Forms.TextBox
    Friend WithEvents textBox21 As System.Windows.Forms.TextBox
    Friend WithEvents textBox22 As System.Windows.Forms.TextBox
    Friend WithEvents textBox23 As System.Windows.Forms.TextBox
    Friend WithEvents textBox24 As System.Windows.Forms.TextBox
    Friend WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents btnRandomizeTabOrder As System.Windows.Forms.Button
    Friend WithEvents radioDownFirst As System.Windows.Forms.RadioButton
    Friend WithEvents radioAcrossFirst As System.Windows.Forms.RadioButton
    Friend WithEvents btnAdjustTabOrder As System.Windows.Forms.Button
    Friend WithEvents panel1 As System.Windows.Forms.Panel
    Friend WithEvents textBox7 As System.Windows.Forms.TextBox
    Friend WithEvents textBox8 As System.Windows.Forms.TextBox
    Friend WithEvents textBox9 As System.Windows.Forms.TextBox
    Friend WithEvents textBox10 As System.Windows.Forms.TextBox
    Friend WithEvents textBox11 As System.Windows.Forms.TextBox
    Friend WithEvents textBox12 As System.Windows.Forms.TextBox
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents textBox6 As System.Windows.Forms.TextBox
    Friend WithEvents textBox5 As System.Windows.Forms.TextBox
    Friend WithEvents textBox4 As System.Windows.Forms.TextBox
    Friend WithEvents textBox3 As System.Windows.Forms.TextBox
    Friend WithEvents textBox2 As System.Windows.Forms.TextBox
    Friend WithEvents textBox1 As System.Windows.Forms.TextBox
    Friend WithEvents AddOverrideCheckbox As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.tabControl1 = New System.Windows.Forms.TabControl
        Me.tabPage1 = New System.Windows.Forms.TabPage
        Me.groupBox3 = New System.Windows.Forms.GroupBox
        Me.textBox13 = New System.Windows.Forms.TextBox
        Me.textBox14 = New System.Windows.Forms.TextBox
        Me.textBox15 = New System.Windows.Forms.TextBox
        Me.textBox16 = New System.Windows.Forms.TextBox
        Me.textBox17 = New System.Windows.Forms.TextBox
        Me.textBox18 = New System.Windows.Forms.TextBox
        Me.tabPage2 = New System.Windows.Forms.TabPage
        Me.textBox19 = New System.Windows.Forms.TextBox
        Me.textBox20 = New System.Windows.Forms.TextBox
        Me.textBox21 = New System.Windows.Forms.TextBox
        Me.textBox22 = New System.Windows.Forms.TextBox
        Me.textBox23 = New System.Windows.Forms.TextBox
        Me.textBox24 = New System.Windows.Forms.TextBox
        Me.groupBox2 = New System.Windows.Forms.GroupBox
        Me.btnRandomizeTabOrder = New System.Windows.Forms.Button
        Me.radioDownFirst = New System.Windows.Forms.RadioButton
        Me.radioAcrossFirst = New System.Windows.Forms.RadioButton
        Me.btnAdjustTabOrder = New System.Windows.Forms.Button
        Me.panel1 = New System.Windows.Forms.Panel
        Me.textBox7 = New System.Windows.Forms.TextBox
        Me.textBox8 = New System.Windows.Forms.TextBox
        Me.textBox9 = New System.Windows.Forms.TextBox
        Me.textBox10 = New System.Windows.Forms.TextBox
        Me.textBox11 = New System.Windows.Forms.TextBox
        Me.textBox12 = New System.Windows.Forms.TextBox
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.textBox6 = New System.Windows.Forms.TextBox
        Me.textBox5 = New System.Windows.Forms.TextBox
        Me.textBox4 = New System.Windows.Forms.TextBox
        Me.textBox3 = New System.Windows.Forms.TextBox
        Me.textBox2 = New System.Windows.Forms.TextBox
        Me.textBox1 = New System.Windows.Forms.TextBox
        Me.AddOverrideCheckbox = New System.Windows.Forms.CheckBox
        Me.tabControl1.SuspendLayout()
        Me.tabPage1.SuspendLayout()
        Me.groupBox3.SuspendLayout()
        Me.tabPage2.SuspendLayout()
        Me.groupBox2.SuspendLayout()
        Me.panel1.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabControl1
        '
        Me.tabControl1.Controls.Add(Me.tabPage1)
        Me.tabControl1.Controls.Add(Me.tabPage2)
        Me.tabControl1.Location = New System.Drawing.Point(24, 200)
        Me.tabControl1.Name = "tabControl1"
        Me.tabControl1.SelectedIndex = 0
        Me.tabControl1.Size = New System.Drawing.Size(552, 192)
        Me.tabControl1.TabIndex = 6
        '
        'tabPage1
        '
        Me.tabPage1.Controls.Add(Me.groupBox3)
        Me.tabPage1.Location = New System.Drawing.Point(4, 22)
        Me.tabPage1.Name = "tabPage1"
        Me.tabPage1.Size = New System.Drawing.Size(544, 166)
        Me.tabPage1.TabIndex = 0
        Me.tabPage1.Text = "tabPage1"
        '
        'groupBox3
        '
        Me.groupBox3.Controls.Add(Me.textBox13)
        Me.groupBox3.Controls.Add(Me.textBox14)
        Me.groupBox3.Controls.Add(Me.textBox15)
        Me.groupBox3.Controls.Add(Me.textBox16)
        Me.groupBox3.Controls.Add(Me.textBox17)
        Me.groupBox3.Controls.Add(Me.textBox18)
        Me.groupBox3.Location = New System.Drawing.Point(24, 8)
        Me.groupBox3.Name = "groupBox3"
        Me.groupBox3.Size = New System.Drawing.Size(264, 160)
        Me.groupBox3.TabIndex = 1
        Me.groupBox3.TabStop = False
        Me.groupBox3.Text = "groupBox3"
        '
        'textBox13
        '
        Me.textBox13.Location = New System.Drawing.Point(144, 112)
        Me.textBox13.Name = "textBox13"
        Me.textBox13.TabIndex = 5
        Me.textBox13.Text = "textBox6"
        '
        'textBox14
        '
        Me.textBox14.Location = New System.Drawing.Point(16, 112)
        Me.textBox14.Name = "textBox14"
        Me.textBox14.TabIndex = 4
        Me.textBox14.Text = "textBox5"
        '
        'textBox15
        '
        Me.textBox15.Location = New System.Drawing.Point(144, 72)
        Me.textBox15.Name = "textBox15"
        Me.textBox15.TabIndex = 3
        Me.textBox15.Text = "textBox4"
        '
        'textBox16
        '
        Me.textBox16.Location = New System.Drawing.Point(16, 72)
        Me.textBox16.Name = "textBox16"
        Me.textBox16.TabIndex = 2
        Me.textBox16.Text = "textBox3"
        '
        'textBox17
        '
        Me.textBox17.Location = New System.Drawing.Point(144, 32)
        Me.textBox17.Name = "textBox17"
        Me.textBox17.TabIndex = 1
        Me.textBox17.Text = "textBox2"
        '
        'textBox18
        '
        Me.textBox18.Location = New System.Drawing.Point(16, 32)
        Me.textBox18.Name = "textBox18"
        Me.textBox18.TabIndex = 0
        Me.textBox18.Text = "textBox1"
        '
        'tabPage2
        '
        Me.tabPage2.Controls.Add(Me.textBox19)
        Me.tabPage2.Controls.Add(Me.textBox20)
        Me.tabPage2.Controls.Add(Me.textBox21)
        Me.tabPage2.Controls.Add(Me.textBox22)
        Me.tabPage2.Controls.Add(Me.textBox23)
        Me.tabPage2.Controls.Add(Me.textBox24)
        Me.tabPage2.Location = New System.Drawing.Point(4, 22)
        Me.tabPage2.Name = "tabPage2"
        Me.tabPage2.Size = New System.Drawing.Size(544, 166)
        Me.tabPage2.TabIndex = 1
        Me.tabPage2.Text = "tabPage2"
        Me.tabPage2.Visible = False
        '
        'textBox19
        '
        Me.textBox19.Location = New System.Drawing.Point(176, 104)
        Me.textBox19.Name = "textBox19"
        Me.textBox19.TabIndex = 11
        Me.textBox19.Text = "textBox19"
        '
        'textBox20
        '
        Me.textBox20.Location = New System.Drawing.Point(48, 104)
        Me.textBox20.Name = "textBox20"
        Me.textBox20.TabIndex = 10
        Me.textBox20.Text = "textBox20"
        '
        'textBox21
        '
        Me.textBox21.Location = New System.Drawing.Point(176, 64)
        Me.textBox21.Name = "textBox21"
        Me.textBox21.TabIndex = 9
        Me.textBox21.Text = "textBox21"
        '
        'textBox22
        '
        Me.textBox22.Location = New System.Drawing.Point(48, 64)
        Me.textBox22.Name = "textBox22"
        Me.textBox22.TabIndex = 8
        Me.textBox22.Text = "textBox22"
        '
        'textBox23
        '
        Me.textBox23.Location = New System.Drawing.Point(176, 24)
        Me.textBox23.Name = "textBox23"
        Me.textBox23.TabIndex = 7
        Me.textBox23.Text = "textBox23"
        '
        'textBox24
        '
        Me.textBox24.Location = New System.Drawing.Point(48, 24)
        Me.textBox24.Name = "textBox24"
        Me.textBox24.TabIndex = 6
        Me.textBox24.Text = "textBox24"
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.AddOverrideCheckbox)
        Me.groupBox2.Controls.Add(Me.btnRandomizeTabOrder)
        Me.groupBox2.Controls.Add(Me.radioDownFirst)
        Me.groupBox2.Controls.Add(Me.radioAcrossFirst)
        Me.groupBox2.Controls.Add(Me.btnAdjustTabOrder)
        Me.groupBox2.Location = New System.Drawing.Point(24, 400)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(552, 88)
        Me.groupBox2.TabIndex = 7
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Adjust Tab Order"
        '
        'btnRandomizeTabOrder
        '
        Me.btnRandomizeTabOrder.Location = New System.Drawing.Point(408, 16)
        Me.btnRandomizeTabOrder.Name = "btnRandomizeTabOrder"
        Me.btnRandomizeTabOrder.Size = New System.Drawing.Size(128, 23)
        Me.btnRandomizeTabOrder.TabIndex = 15
        Me.btnRandomizeTabOrder.Text = "Randomize Tab Order"
        '
        'radioDownFirst
        '
        Me.radioDownFirst.Location = New System.Drawing.Point(176, 16)
        Me.radioDownFirst.Name = "radioDownFirst"
        Me.radioDownFirst.TabIndex = 14
        Me.radioDownFirst.Text = "Down First"
        '
        'radioAcrossFirst
        '
        Me.radioAcrossFirst.Checked = True
        Me.radioAcrossFirst.Location = New System.Drawing.Point(56, 16)
        Me.radioAcrossFirst.Name = "radioAcrossFirst"
        Me.radioAcrossFirst.TabIndex = 13
        Me.radioAcrossFirst.TabStop = True
        Me.radioAcrossFirst.Text = "Across First"
        '
        'btnAdjustTabOrder
        '
        Me.btnAdjustTabOrder.Location = New System.Drawing.Point(280, 16)
        Me.btnAdjustTabOrder.Name = "btnAdjustTabOrder"
        Me.btnAdjustTabOrder.Size = New System.Drawing.Size(112, 23)
        Me.btnAdjustTabOrder.TabIndex = 12
        Me.btnAdjustTabOrder.Text = "Adjust Tab Order"
        '
        'panel1
        '
        Me.panel1.Controls.Add(Me.textBox7)
        Me.panel1.Controls.Add(Me.textBox8)
        Me.panel1.Controls.Add(Me.textBox9)
        Me.panel1.Controls.Add(Me.textBox10)
        Me.panel1.Controls.Add(Me.textBox11)
        Me.panel1.Controls.Add(Me.textBox12)
        Me.panel1.Location = New System.Drawing.Point(304, 32)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(296, 152)
        Me.panel1.TabIndex = 5
        '
        'textBox7
        '
        Me.textBox7.Location = New System.Drawing.Point(162, 106)
        Me.textBox7.Name = "textBox7"
        Me.textBox7.TabIndex = 11
        Me.textBox7.Text = "textBox6"
        '
        'textBox8
        '
        Me.textBox8.Location = New System.Drawing.Point(34, 106)
        Me.textBox8.Name = "textBox8"
        Me.textBox8.TabIndex = 10
        Me.textBox8.Text = "textBox5"
        '
        'textBox9
        '
        Me.textBox9.Location = New System.Drawing.Point(162, 66)
        Me.textBox9.Name = "textBox9"
        Me.textBox9.TabIndex = 9
        Me.textBox9.Text = "textBox4"
        '
        'textBox10
        '
        Me.textBox10.Location = New System.Drawing.Point(34, 66)
        Me.textBox10.Name = "textBox10"
        Me.textBox10.TabIndex = 8
        Me.textBox10.Text = "textBox3"
        '
        'textBox11
        '
        Me.textBox11.Location = New System.Drawing.Point(162, 26)
        Me.textBox11.Name = "textBox11"
        Me.textBox11.TabIndex = 7
        Me.textBox11.Text = "textBox2"
        '
        'textBox12
        '
        Me.textBox12.Location = New System.Drawing.Point(34, 26)
        Me.textBox12.Name = "textBox12"
        Me.textBox12.TabIndex = 6
        Me.textBox12.Text = "textBox1"
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.textBox6)
        Me.groupBox1.Controls.Add(Me.textBox5)
        Me.groupBox1.Controls.Add(Me.textBox4)
        Me.groupBox1.Controls.Add(Me.textBox3)
        Me.groupBox1.Controls.Add(Me.textBox2)
        Me.groupBox1.Controls.Add(Me.textBox1)
        Me.groupBox1.Location = New System.Drawing.Point(24, 24)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(264, 160)
        Me.groupBox1.TabIndex = 4
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "groupBox1"
        '
        'textBox6
        '
        Me.textBox6.Location = New System.Drawing.Point(144, 112)
        Me.textBox6.Name = "textBox6"
        Me.textBox6.TabIndex = 5
        Me.textBox6.Text = "textBox6"
        '
        'textBox5
        '
        Me.textBox5.Location = New System.Drawing.Point(16, 112)
        Me.textBox5.Name = "textBox5"
        Me.textBox5.TabIndex = 4
        Me.textBox5.Text = "textBox5"
        '
        'textBox4
        '
        Me.textBox4.Location = New System.Drawing.Point(144, 72)
        Me.textBox4.Name = "textBox4"
        Me.textBox4.TabIndex = 3
        Me.textBox4.TabStop = False
        Me.textBox4.Text = "not a tab stop"
        '
        'textBox3
        '
        Me.textBox3.Location = New System.Drawing.Point(16, 72)
        Me.textBox3.Name = "textBox3"
        Me.textBox3.TabIndex = 2
        Me.textBox3.Text = "textBox3"
        '
        'textBox2
        '
        Me.textBox2.Location = New System.Drawing.Point(144, 32)
        Me.textBox2.Name = "textBox2"
        Me.textBox2.TabIndex = 1
        Me.textBox2.Text = "textBox2"
        '
        'textBox1
        '
        Me.textBox1.Location = New System.Drawing.Point(16, 32)
        Me.textBox1.Name = "textBox1"
        Me.textBox1.TabIndex = 0
        Me.textBox1.Text = "textBox1"
        '
        'AddOverrideCheckbox
        '
        Me.AddOverrideCheckbox.Location = New System.Drawing.Point(280, 56)
        Me.AddOverrideCheckbox.Name = "AddOverrideCheckbox"
        Me.AddOverrideCheckbox.Size = New System.Drawing.Size(224, 24)
        Me.AddOverrideCheckbox.TabIndex = 17
        Me.AddOverrideCheckbox.Text = "Add DownFirst Override to groupBox3"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(624, 502)
        Me.Controls.Add(Me.tabControl1)
        Me.Controls.Add(Me.groupBox2)
        Me.Controls.Add(Me.panel1)
        Me.Controls.Add(Me.groupBox1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.tabControl1.ResumeLayout(False)
        Me.tabPage1.ResumeLayout(False)
        Me.groupBox3.ResumeLayout(False)
        Me.tabPage2.ResumeLayout(False)
        Me.groupBox2.ResumeLayout(False)
        Me.panel1.ResumeLayout(False)
        Me.groupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnAdjustTabOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdjustTabOrder.Click
        Dim c As Control

        For Each c In Controls
            Debug.WriteLine(c.Name)
        Next c

        '// This button lets you explicitly call the tab order manager whenever you want
        '// with a specific scheme.
        '// In most cases, I envision the tab order manager being called at the end of
        '// Form_Load or constructor (as above).
        Dim scheme As TabOrder.TabScheme
        If radioAcrossFirst.Checked Then
            scheme = TabOrder.TabScheme.AcrossFirst
        Else
            scheme = TabOrder.TabScheme.DownFirst
        End If

        Dim tom As New TabOrder(Me)

        '// This method lets you change the scheme for a specified control rather than
        '// inherit from its parents.
        If AddOverrideCheckbox.Checked Then
            tom.SetSchemeForControl(groupBox3, TabOrder.TabScheme.DownFirst)
        End If

        '// This method actually sets the order all the way down the control hierarchy.
        tom.SetTabOrder(scheme)

    End Sub

    '/// <summary>
    '/// Recursively set a random number for TabIndex for all children of this control
    '/// </summary>
    '/// <param name="parent">The control whose children we want to give random tab order to.</param>
    '/// <param name="generator">The random number generator to use.</param>
    Private Sub RandomizeChildren(ByVal parent As Control, ByVal generator As Random)
        Dim c As Control
        For Each c In parent.Controls
            c.TabIndex = generator.Next(100)
            If c.HasChildren Then
                RandomizeChildren(c, generator)
            End If
        Next c
    End Sub

    Private Sub btnRandomizeTabOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRandomizeTabOrder.Click
        RandomizeChildren(Me, New Random)
    End Sub
End Class
