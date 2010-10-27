'Stillinger's Dosage Calulator - Calculates injection quantities of drugs for canines
'and felines.
'Platform:  Windows XP
'Compiler:  Microsoft Visual Basic.NET (Standard)
'Copyright (C) 2003, 2004  Jeff Stillinger
'
'This program is free software; you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation; either version 2 of the License, or
'any later version.
'
'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU General Public License for more details.
'
'You should have received a copy of the GNU General Public License
'along with this program; if not, write to the Free Software
'Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
'
'See also the file GPL.TXT that came with this package,
'or http://www.gnu.org/licenses/gpl.txt
'
'GNU GENERAL PUBLIC LICENSE RESTRICTIONS:
'YOU MAY NOT MODIFY THIS SOFTWARE FOR HUMAN USE.
'
'IT IS A VIOLATION OF UNITED STATES FEDERAL LAW TO USE THIS PRODUCT IN A MANOR 
'INCONSISTENT WITH IT'S LABELING.  THIS PRODUCT IS CLEARLY LABELED FOR VTERINARY 
'USE ONLY. YOU MAY NOT REMOVE THE LABEL THAT STATES "FOR VETERINARY USE ONLY". 
'

Public Class Form1
    Inherits System.Windows.Forms.Form
    Public timergo As Double
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
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox4 As System.Windows.Forms.CheckBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox8 As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents CheckBox5 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox6 As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBox7 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox8 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox9 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox10 As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox9 As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents StatusBar1 As System.Windows.Forms.StatusBar
    Friend WithEvents StatusBarPanel1 As System.Windows.Forms.StatusBarPanel
    Friend WithEvents StatusBarPanel2 As System.Windows.Forms.StatusBarPanel
    Friend WithEvents StatusBarPanel3 As System.Windows.Forms.StatusBarPanel
    Friend WithEvents CheckBox11 As System.Windows.Forms.CheckBox
    Friend WithEvents Timer1 As System.Timers.Timer
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents TextBox10 As System.Windows.Forms.TextBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents TextBox11 As System.Windows.Forms.TextBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents TextBox12 As System.Windows.Forms.TextBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents TextBox13 As System.Windows.Forms.TextBox
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents Label53 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents GroupBox11 As System.Windows.Forms.GroupBox
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents TextBox14 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox15 As System.Windows.Forms.TextBox
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents Label60 As System.Windows.Forms.Label
    Friend WithEvents Label61 As System.Windows.Forms.Label
    Friend WithEvents TextBox16 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox17 As System.Windows.Forms.TextBox
    Friend WithEvents Label62 As System.Windows.Forms.Label
    Friend WithEvents TextBox18 As System.Windows.Forms.TextBox
    Friend WithEvents Label63 As System.Windows.Forms.Label
    Friend WithEvents Label64 As System.Windows.Forms.Label
    Friend WithEvents GroupBox13 As System.Windows.Forms.GroupBox
    Friend WithEvents Label77 As System.Windows.Forms.Label
    Friend WithEvents Label78 As System.Windows.Forms.Label
    Friend WithEvents Label79 As System.Windows.Forms.Label
    Friend WithEvents Label80 As System.Windows.Forms.Label
    Friend WithEvents Label81 As System.Windows.Forms.Label
    Friend WithEvents Label82 As System.Windows.Forms.Label
    Friend WithEvents Label69 As System.Windows.Forms.Label
    Friend WithEvents Label70 As System.Windows.Forms.Label
    Friend WithEvents Label71 As System.Windows.Forms.Label
    Friend WithEvents Label73 As System.Windows.Forms.Label
    Friend WithEvents Label74 As System.Windows.Forms.Label
    Friend WithEvents Label75 As System.Windows.Forms.Label
    Friend WithEvents Label76 As System.Windows.Forms.Label
    Friend WithEvents StatusBar2 As System.Windows.Forms.StatusBar
    Friend WithEvents StatusBarPanel4 As System.Windows.Forms.StatusBarPanel
    Friend WithEvents StatusBarPanel5 As System.Windows.Forms.StatusBarPanel
    Friend WithEvents StatusBarPanel6 As System.Windows.Forms.StatusBarPanel
    Friend WithEvents GroupBox12 As System.Windows.Forms.GroupBox
    Friend WithEvents Label65 As System.Windows.Forms.Label
    Friend WithEvents Label66 As System.Windows.Forms.Label
    Friend WithEvents Label67 As System.Windows.Forms.Label
    Friend WithEvents Label68 As System.Windows.Forms.Label
    Friend WithEvents StatusBarPanel7 As System.Windows.Forms.StatusBarPanel
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Form1))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.CheckBox2 = New System.Windows.Forms.CheckBox
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.CheckBox4 = New System.Windows.Forms.CheckBox
        Me.CheckBox3 = New System.Windows.Forms.CheckBox
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.Label51 = New System.Windows.Forms.Label
        Me.Label50 = New System.Windows.Forms.Label
        Me.Label49 = New System.Windows.Forms.Label
        Me.Label48 = New System.Windows.Forms.Label
        Me.Label33 = New System.Windows.Forms.Label
        Me.TextBox13 = New System.Windows.Forms.TextBox
        Me.CheckBox11 = New System.Windows.Forms.CheckBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.Label32 = New System.Windows.Forms.Label
        Me.Label31 = New System.Windows.Forms.Label
        Me.Label30 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label25 = New System.Windows.Forms.Label
        Me.Label24 = New System.Windows.Forms.Label
        Me.Label23 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.CheckBox6 = New System.Windows.Forms.CheckBox
        Me.CheckBox5 = New System.Windows.Forms.CheckBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.TextBox8 = New System.Windows.Forms.TextBox
        Me.TextBox7 = New System.Windows.Forms.TextBox
        Me.TextBox6 = New System.Windows.Forms.TextBox
        Me.GroupBox7 = New System.Windows.Forms.GroupBox
        Me.TextBox5 = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
        Me.TextBox9 = New System.Windows.Forms.TextBox
        Me.CheckBox10 = New System.Windows.Forms.CheckBox
        Me.CheckBox9 = New System.Windows.Forms.CheckBox
        Me.CheckBox8 = New System.Windows.Forms.CheckBox
        Me.CheckBox7 = New System.Windows.Forms.CheckBox
        Me.StatusBar1 = New System.Windows.Forms.StatusBar
        Me.StatusBarPanel1 = New System.Windows.Forms.StatusBarPanel
        Me.StatusBarPanel2 = New System.Windows.Forms.StatusBarPanel
        Me.StatusBarPanel3 = New System.Windows.Forms.StatusBarPanel
        Me.Timer1 = New System.Timers.Timer
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.Label29 = New System.Windows.Forms.Label
        Me.Label28 = New System.Windows.Forms.Label
        Me.TextBox12 = New System.Windows.Forms.TextBox
        Me.Label27 = New System.Windows.Forms.Label
        Me.TextBox11 = New System.Windows.Forms.TextBox
        Me.Label26 = New System.Windows.Forms.Label
        Me.TextBox10 = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.GroupBox10 = New System.Windows.Forms.GroupBox
        Me.Label47 = New System.Windows.Forms.Label
        Me.Label46 = New System.Windows.Forms.Label
        Me.Label45 = New System.Windows.Forms.Label
        Me.Label44 = New System.Windows.Forms.Label
        Me.Label43 = New System.Windows.Forms.Label
        Me.Label42 = New System.Windows.Forms.Label
        Me.Label41 = New System.Windows.Forms.Label
        Me.Label40 = New System.Windows.Forms.Label
        Me.Label39 = New System.Windows.Forms.Label
        Me.Label38 = New System.Windows.Forms.Label
        Me.Label37 = New System.Windows.Forms.Label
        Me.Label36 = New System.Windows.Forms.Label
        Me.Label35 = New System.Windows.Forms.Label
        Me.Label34 = New System.Windows.Forms.Label
        Me.Label52 = New System.Windows.Forms.Label
        Me.Label53 = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox11 = New System.Windows.Forms.GroupBox
        Me.Label70 = New System.Windows.Forms.Label
        Me.Label64 = New System.Windows.Forms.Label
        Me.Label63 = New System.Windows.Forms.Label
        Me.TextBox18 = New System.Windows.Forms.TextBox
        Me.Label62 = New System.Windows.Forms.Label
        Me.TextBox17 = New System.Windows.Forms.TextBox
        Me.TextBox16 = New System.Windows.Forms.TextBox
        Me.Label61 = New System.Windows.Forms.Label
        Me.Label60 = New System.Windows.Forms.Label
        Me.Label59 = New System.Windows.Forms.Label
        Me.Label58 = New System.Windows.Forms.Label
        Me.Label57 = New System.Windows.Forms.Label
        Me.Label56 = New System.Windows.Forms.Label
        Me.TextBox15 = New System.Windows.Forms.TextBox
        Me.TextBox14 = New System.Windows.Forms.TextBox
        Me.Label55 = New System.Windows.Forms.Label
        Me.Label54 = New System.Windows.Forms.Label
        Me.GroupBox13 = New System.Windows.Forms.GroupBox
        Me.Label76 = New System.Windows.Forms.Label
        Me.Label75 = New System.Windows.Forms.Label
        Me.Label74 = New System.Windows.Forms.Label
        Me.Label73 = New System.Windows.Forms.Label
        Me.Label71 = New System.Windows.Forms.Label
        Me.Label69 = New System.Windows.Forms.Label
        Me.Label80 = New System.Windows.Forms.Label
        Me.Label79 = New System.Windows.Forms.Label
        Me.Label78 = New System.Windows.Forms.Label
        Me.Label77 = New System.Windows.Forms.Label
        Me.Label81 = New System.Windows.Forms.Label
        Me.Label82 = New System.Windows.Forms.Label
        Me.StatusBar2 = New System.Windows.Forms.StatusBar
        Me.StatusBarPanel4 = New System.Windows.Forms.StatusBarPanel
        Me.StatusBarPanel5 = New System.Windows.Forms.StatusBarPanel
        Me.StatusBarPanel6 = New System.Windows.Forms.StatusBarPanel
        Me.StatusBarPanel7 = New System.Windows.Forms.StatusBarPanel
        Me.GroupBox12 = New System.Windows.Forms.GroupBox
        Me.Label68 = New System.Windows.Forms.Label
        Me.Label67 = New System.Windows.Forms.Label
        Me.Label66 = New System.Windows.Forms.Label
        Me.Label65 = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        CType(Me.StatusBarPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StatusBarPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StatusBarPanel3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Timer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox10.SuspendLayout()
        Me.GroupBox11.SuspendLayout()
        Me.GroupBox13.SuspendLayout()
        CType(Me.StatusBarPanel4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StatusBarPanel5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StatusBarPanel6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StatusBarPanel7, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox12.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox1.Controls.Add(Me.CheckBox2)
        Me.GroupBox1.Controls.Add(Me.CheckBox1)
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(120, 56)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Animals Weight"
        '
        'CheckBox2
        '
        Me.CheckBox2.Location = New System.Drawing.Point(72, 32)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(40, 16)
        Me.CheckBox2.TabIndex = 2
        Me.CheckBox2.TabStop = False
        Me.CheckBox2.Text = "lbs"
        '
        'CheckBox1
        '
        Me.CheckBox1.Checked = True
        Me.CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox1.Location = New System.Drawing.Point(72, 16)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(40, 16)
        Me.CheckBox1.TabIndex = 1
        Me.CheckBox1.TabStop = False
        Me.CheckBox1.Text = "kg"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(8, 24)
        Me.TextBox1.MaxLength = 5
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(56, 20)
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Text = "000.0"
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox2.Controls.Add(Me.CheckBox4)
        Me.GroupBox2.Controls.Add(Me.CheckBox3)
        Me.GroupBox2.Controls.Add(Me.TextBox2)
        Me.GroupBox2.Location = New System.Drawing.Point(136, 8)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(136, 56)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Dosage"
        '
        'CheckBox4
        '
        Me.CheckBox4.Location = New System.Drawing.Point(72, 32)
        Me.CheckBox4.Name = "CheckBox4"
        Me.CheckBox4.Size = New System.Drawing.Size(56, 16)
        Me.CheckBox4.TabIndex = 3
        Me.CheckBox4.TabStop = False
        Me.CheckBox4.Text = "mg/lb"
        '
        'CheckBox3
        '
        Me.CheckBox3.Checked = True
        Me.CheckBox3.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox3.Location = New System.Drawing.Point(72, 16)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(56, 16)
        Me.CheckBox3.TabIndex = 2
        Me.CheckBox3.TabStop = False
        Me.CheckBox3.Text = "mg/kg"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(8, 24)
        Me.TextBox2.MaxLength = 17
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(56, 20)
        Me.TextBox2.TabIndex = 1
        Me.TextBox2.Text = "0000"
        '
        'GroupBox3
        '
        Me.GroupBox3.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox3.Controls.Add(Me.TextBox3)
        Me.GroupBox3.Location = New System.Drawing.Point(8, 88)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(120, 56)
        Me.GroupBox3.TabIndex = 2
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Drug Order (mg)"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(16, 24)
        Me.TextBox3.MaxLength = 17
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(64, 20)
        Me.TextBox3.TabIndex = 2
        Me.TextBox3.Text = "0000"
        '
        'GroupBox4
        '
        Me.GroupBox4.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox4.Controls.Add(Me.TextBox4)
        Me.GroupBox4.Location = New System.Drawing.Point(136, 88)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(136, 56)
        Me.GroupBox4.TabIndex = 3
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Concentration (mg/ml)"
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(16, 24)
        Me.TextBox4.MaxLength = 17
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(64, 20)
        Me.TextBox4.TabIndex = 3
        Me.TextBox4.Text = "0000"
        '
        'GroupBox5
        '
        Me.GroupBox5.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox5.Controls.Add(Me.Label51)
        Me.GroupBox5.Controls.Add(Me.Label50)
        Me.GroupBox5.Controls.Add(Me.Label49)
        Me.GroupBox5.Controls.Add(Me.Label48)
        Me.GroupBox5.Controls.Add(Me.Label33)
        Me.GroupBox5.Controls.Add(Me.TextBox13)
        Me.GroupBox5.Controls.Add(Me.CheckBox11)
        Me.GroupBox5.Controls.Add(Me.Label16)
        Me.GroupBox5.Controls.Add(Me.Label15)
        Me.GroupBox5.Controls.Add(Me.Label13)
        Me.GroupBox5.Controls.Add(Me.Label12)
        Me.GroupBox5.Controls.Add(Me.Label10)
        Me.GroupBox5.Controls.Add(Me.Label9)
        Me.GroupBox5.Controls.Add(Me.Label8)
        Me.GroupBox5.Controls.Add(Me.Label7)
        Me.GroupBox5.Controls.Add(Me.Label6)
        Me.GroupBox5.Controls.Add(Me.Label5)
        Me.GroupBox5.Location = New System.Drawing.Point(432, 8)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(192, 240)
        Me.GroupBox5.TabIndex = 4
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Recomended Fluid Rates"
        '
        'Label51
        '
        Me.Label51.BackColor = System.Drawing.Color.SandyBrown
        Me.Label51.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label51.Location = New System.Drawing.Point(120, 216)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(64, 16)
        Me.Label51.TabIndex = 16
        '
        'Label50
        '
        Me.Label50.Location = New System.Drawing.Point(8, 216)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(112, 16)
        Me.Label50.TabIndex = 15
        Me.Label50.Text = "Plasma Rate (ml/hr)"
        '
        'Label49
        '
        Me.Label49.BackColor = System.Drawing.Color.SandyBrown
        Me.Label49.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label49.Location = New System.Drawing.Point(120, 144)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(64, 16)
        Me.Label49.TabIndex = 14
        '
        'Label48
        '
        Me.Label48.Location = New System.Drawing.Point(8, 144)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(120, 16)
        Me.Label48.TabIndex = 13
        Me.Label48.Text = "Oxyglobin Dosage (ml)"
        '
        'Label33
        '
        Me.Label33.Location = New System.Drawing.Point(104, 112)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(56, 20)
        Me.Label33.TabIndex = 12
        Me.Label33.Text = "Adjust ggt"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBox13
        '
        Me.TextBox13.Location = New System.Drawing.Point(160, 112)
        Me.TextBox13.MaxLength = 2
        Me.TextBox13.Name = "TextBox13"
        Me.TextBox13.Size = New System.Drawing.Size(24, 20)
        Me.TextBox13.TabIndex = 11
        Me.TextBox13.TabStop = False
        Me.TextBox13.Text = "15"
        Me.TextBox13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'CheckBox11
        '
        Me.CheckBox11.Location = New System.Drawing.Point(24, 112)
        Me.CheckBox11.Name = "CheckBox11"
        Me.CheckBox11.Size = New System.Drawing.Size(48, 20)
        Me.CheckBox11.TabIndex = 10
        Me.CheckBox11.TabStop = False
        Me.CheckBox11.Text = "Sync"
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.SandyBrown
        Me.Label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label16.Location = New System.Drawing.Point(120, 192)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(64, 16)
        Me.Label16.TabIndex = 9
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(8, 192)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(112, 16)
        Me.Label15.TabIndex = 8
        Me.Label15.Text = "Plasma Dosage (ml)"
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.Color.SandyBrown
        Me.Label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label13.Location = New System.Drawing.Point(120, 80)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(64, 16)
        Me.Label13.TabIndex = 7
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(24, 80)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(96, 32)
        Me.Label12.TabIndex = 6
        Me.Label12.Text = "1 Drop every     -> Seconds"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.SandyBrown
        Me.Label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label10.Location = New System.Drawing.Point(120, 168)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(64, 16)
        Me.Label10.TabIndex = 5
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(8, 168)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(112, 16)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Oxyglobin Rate ml/hr"
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.SandyBrown
        Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label8.Location = New System.Drawing.Point(120, 56)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(64, 16)
        Me.Label8.TabIndex = 3
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(8, 56)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(112, 16)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "ml/hr (Pump Setting)"
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.SandyBrown
        Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label6.Location = New System.Drawing.Point(120, 24)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(64, 16)
        Me.Label6.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 24)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(112, 16)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "24 Hr. Maint. Volume"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.MediumBlue
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Label1.Location = New System.Drawing.Point(280, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(144, 16)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Recomended Order (mg)"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.MediumSpringGreen
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(280, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(144, 16)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Pending..."
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.MediumBlue
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Label3.Location = New System.Drawing.Point(280, 80)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(144, 16)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Dosage (ml/cc)"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.MediumSpringGreen
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(280, 96)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(144, 16)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Pending..."
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(704, 408)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(88, 24)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Calculate"
        '
        'GroupBox6
        '
        Me.GroupBox6.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox6.Controls.Add(Me.Label32)
        Me.GroupBox6.Controls.Add(Me.Label31)
        Me.GroupBox6.Controls.Add(Me.Label30)
        Me.GroupBox6.Controls.Add(Me.Label11)
        Me.GroupBox6.Controls.Add(Me.Label25)
        Me.GroupBox6.Controls.Add(Me.Label24)
        Me.GroupBox6.Controls.Add(Me.Label23)
        Me.GroupBox6.Controls.Add(Me.Label22)
        Me.GroupBox6.Controls.Add(Me.CheckBox6)
        Me.GroupBox6.Controls.Add(Me.CheckBox5)
        Me.GroupBox6.Controls.Add(Me.Label21)
        Me.GroupBox6.Controls.Add(Me.Label20)
        Me.GroupBox6.Controls.Add(Me.Label19)
        Me.GroupBox6.Controls.Add(Me.TextBox8)
        Me.GroupBox6.Controls.Add(Me.TextBox7)
        Me.GroupBox6.Controls.Add(Me.TextBox6)
        Me.GroupBox6.Location = New System.Drawing.Point(8, 264)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(416, 112)
        Me.GroupBox6.TabIndex = 13
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Whole Blood Transfusion Volume"
        '
        'Label32
        '
        Me.Label32.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label32.Location = New System.Drawing.Point(8, 88)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(120, 16)
        Me.Label32.TabIndex = 26
        Me.Label32.Text = "Normal PCV: 37 - 55%"
        Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label31
        '
        Me.Label31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.Location = New System.Drawing.Point(232, 72)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(24, 16)
        Me.Label31.TabIndex = 25
        Me.Label31.Text = "%"
        '
        'Label30
        '
        Me.Label30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.Location = New System.Drawing.Point(232, 48)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(24, 16)
        Me.Label30.TabIndex = 24
        Me.Label30.Text = "%"
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(232, 24)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(16, 16)
        Me.Label11.TabIndex = 23
        Me.Label11.Text = "%"
        '
        'Label25
        '
        Me.Label25.BackColor = System.Drawing.Color.MediumSpringGreen
        Me.Label25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.ForeColor = System.Drawing.Color.Black
        Me.Label25.Location = New System.Drawing.Point(256, 80)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(144, 16)
        Me.Label25.TabIndex = 22
        Me.Label25.Text = "Pending..."
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label24
        '
        Me.Label24.BackColor = System.Drawing.Color.MediumBlue
        Me.Label24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.ForeColor = System.Drawing.Color.White
        Me.Label24.Location = New System.Drawing.Point(256, 64)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(144, 16)
        Me.Label24.TabIndex = 21
        Me.Label24.Text = "Infusion Rate/24 (ml/hr)"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label23
        '
        Me.Label23.BackColor = System.Drawing.Color.MediumSpringGreen
        Me.Label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.Color.Black
        Me.Label23.Location = New System.Drawing.Point(256, 40)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(144, 16)
        Me.Label23.TabIndex = 20
        Me.Label23.Text = "Pending..."
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label22
        '
        Me.Label22.BackColor = System.Drawing.Color.MediumBlue
        Me.Label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.ForeColor = System.Drawing.Color.White
        Me.Label22.Location = New System.Drawing.Point(256, 24)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(144, 16)
        Me.Label22.TabIndex = 19
        Me.Label22.Text = "Blood Required (ml)"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CheckBox6
        '
        Me.CheckBox6.Location = New System.Drawing.Point(8, 40)
        Me.CheckBox6.Name = "CheckBox6"
        Me.CheckBox6.Size = New System.Drawing.Size(56, 16)
        Me.CheckBox6.TabIndex = 7
        Me.CheckBox6.TabStop = False
        Me.CheckBox6.Text = "Feline"
        '
        'CheckBox5
        '
        Me.CheckBox5.Checked = True
        Me.CheckBox5.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox5.Location = New System.Drawing.Point(8, 24)
        Me.CheckBox5.Name = "CheckBox5"
        Me.CheckBox5.Size = New System.Drawing.Size(64, 16)
        Me.CheckBox5.TabIndex = 6
        Me.CheckBox5.TabStop = False
        Me.CheckBox5.Text = "Canine"
        '
        'Label21
        '
        Me.Label21.Location = New System.Drawing.Point(112, 72)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(80, 16)
        Me.Label21.TabIndex = 5
        Me.Label21.Text = "Raise  PCV to"
        '
        'Label20
        '
        Me.Label20.Location = New System.Drawing.Point(72, 48)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(120, 16)
        Me.Label20.TabIndex = 4
        Me.Label20.Text = "Patient's Current PCV"
        '
        'Label19
        '
        Me.Label19.Location = New System.Drawing.Point(88, 24)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(104, 16)
        Me.Label19.TabIndex = 3
        Me.Label19.Text = "Doner Animal PCV"
        '
        'TextBox8
        '
        Me.TextBox8.Location = New System.Drawing.Point(192, 72)
        Me.TextBox8.MaxLength = 2
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.Size = New System.Drawing.Size(40, 20)
        Me.TextBox8.TabIndex = 2
        Me.TextBox8.TabStop = False
        Me.TextBox8.Text = "45.5"
        Me.TextBox8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox7
        '
        Me.TextBox7.Location = New System.Drawing.Point(192, 48)
        Me.TextBox7.MaxLength = 2
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.Size = New System.Drawing.Size(40, 20)
        Me.TextBox7.TabIndex = 1
        Me.TextBox7.TabStop = False
        Me.TextBox7.Text = "00"
        Me.TextBox7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox6
        '
        Me.TextBox6.Location = New System.Drawing.Point(192, 24)
        Me.TextBox6.MaxLength = 2
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.Size = New System.Drawing.Size(40, 20)
        Me.TextBox6.TabIndex = 0
        Me.TextBox6.TabStop = False
        Me.TextBox6.Text = "00"
        Me.TextBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBox7
        '
        Me.GroupBox7.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox7.Controls.Add(Me.TextBox5)
        Me.GroupBox7.Location = New System.Drawing.Point(168, 184)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(104, 56)
        Me.GroupBox7.TabIndex = 14
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Oral Tablet (mg)"
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(16, 24)
        Me.TextBox5.MaxLength = 17
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(48, 20)
        Me.TextBox5.TabIndex = 4
        Me.TextBox5.Text = "0000"
        '
        'Label17
        '
        Me.Label17.BackColor = System.Drawing.Color.MediumBlue
        Me.Label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.White
        Me.Label17.Location = New System.Drawing.Point(280, 176)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(144, 16)
        Me.Label17.TabIndex = 15
        Me.Label17.Text = "Number of Pills"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label18
        '
        Me.Label18.BackColor = System.Drawing.Color.MediumSpringGreen
        Me.Label18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(280, 192)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(144, 16)
        Me.Label18.TabIndex = 16
        Me.Label18.Text = "Pending..."
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox8
        '
        Me.GroupBox8.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox8.Controls.Add(Me.TextBox9)
        Me.GroupBox8.Controls.Add(Me.CheckBox10)
        Me.GroupBox8.Controls.Add(Me.CheckBox9)
        Me.GroupBox8.Controls.Add(Me.CheckBox8)
        Me.GroupBox8.Controls.Add(Me.CheckBox7)
        Me.GroupBox8.Location = New System.Drawing.Point(8, 184)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(152, 56)
        Me.GroupBox8.TabIndex = 22
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "Oral Frequency x Days"
        '
        'TextBox9
        '
        Me.TextBox9.Location = New System.Drawing.Point(120, 24)
        Me.TextBox9.MaxLength = 2
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.Size = New System.Drawing.Size(24, 20)
        Me.TextBox9.TabIndex = 5
        Me.TextBox9.Text = "01"
        Me.TextBox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'CheckBox10
        '
        Me.CheckBox10.Location = New System.Drawing.Point(64, 32)
        Me.CheckBox10.Name = "CheckBox10"
        Me.CheckBox10.Size = New System.Drawing.Size(48, 16)
        Me.CheckBox10.TabIndex = 3
        Me.CheckBox10.TabStop = False
        Me.CheckBox10.Text = "QID"
        '
        'CheckBox9
        '
        Me.CheckBox9.Location = New System.Drawing.Point(64, 16)
        Me.CheckBox9.Name = "CheckBox9"
        Me.CheckBox9.Size = New System.Drawing.Size(48, 16)
        Me.CheckBox9.TabIndex = 2
        Me.CheckBox9.TabStop = False
        Me.CheckBox9.Text = "TID"
        '
        'CheckBox8
        '
        Me.CheckBox8.Checked = True
        Me.CheckBox8.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox8.Location = New System.Drawing.Point(8, 32)
        Me.CheckBox8.Name = "CheckBox8"
        Me.CheckBox8.Size = New System.Drawing.Size(48, 16)
        Me.CheckBox8.TabIndex = 1
        Me.CheckBox8.TabStop = False
        Me.CheckBox8.Text = "BID"
        '
        'CheckBox7
        '
        Me.CheckBox7.Location = New System.Drawing.Point(8, 16)
        Me.CheckBox7.Name = "CheckBox7"
        Me.CheckBox7.Size = New System.Drawing.Size(48, 16)
        Me.CheckBox7.TabIndex = 0
        Me.CheckBox7.TabStop = False
        Me.CheckBox7.Text = "SID"
        '
        'StatusBar1
        '
        Me.StatusBar1.Location = New System.Drawing.Point(0, 483)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.StatusBarPanel1, Me.StatusBarPanel2, Me.StatusBarPanel3})
        Me.StatusBar1.ShowPanels = True
        Me.StatusBar1.Size = New System.Drawing.Size(806, 24)
        Me.StatusBar1.SizingGrip = False
        Me.StatusBar1.TabIndex = 23
        Me.StatusBar1.Text = "StatusBar1"
        '
        'StatusBarPanel1
        '
        Me.StatusBarPanel1.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.StatusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.StatusBarPanel1.Text = "Copyright 2003, 2004 by Jeff Stillinger "
        Me.StatusBarPanel1.ToolTipText = "Click to View License to Use."
        Me.StatusBarPanel1.Width = 205
        '
        'StatusBarPanel2
        '
        Me.StatusBarPanel2.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.StatusBarPanel2.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.StatusBarPanel2.Text = "FOR VETERINARY USE ONLY"
        Me.StatusBarPanel2.ToolTipText = "Click to View License to Use."
        Me.StatusBarPanel2.Width = 174
        '
        'StatusBarPanel3
        '
        Me.StatusBarPanel3.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.StatusBarPanel3.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.StatusBarPanel3.Text = "Released under the terms and conditions of the GNU General Pulbic Lincense"
        Me.StatusBarPanel3.ToolTipText = "Click to View License to Use."
        Me.StatusBarPanel3.Width = 427
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.SynchronizingObject = Me
        '
        'GroupBox9
        '
        Me.GroupBox9.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox9.Controls.Add(Me.Label29)
        Me.GroupBox9.Controls.Add(Me.Label28)
        Me.GroupBox9.Controls.Add(Me.TextBox12)
        Me.GroupBox9.Controls.Add(Me.Label27)
        Me.GroupBox9.Controls.Add(Me.TextBox11)
        Me.GroupBox9.Controls.Add(Me.Label26)
        Me.GroupBox9.Controls.Add(Me.TextBox10)
        Me.GroupBox9.Controls.Add(Me.Label14)
        Me.GroupBox9.Location = New System.Drawing.Point(432, 256)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(192, 120)
        Me.GroupBox9.TabIndex = 24
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "Solutions-Dextrose to Fluids"
        '
        'Label29
        '
        Me.Label29.BackColor = System.Drawing.Color.Transparent
        Me.Label29.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.ForeColor = System.Drawing.Color.Black
        Me.Label29.Location = New System.Drawing.Point(112, 16)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(72, 16)
        Me.Label29.TabIndex = 7
        Me.Label29.Text = "Replace (ml) "
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label28
        '
        Me.Label28.BackColor = System.Drawing.SystemColors.Window
        Me.Label28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(112, 31)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(72, 16)
        Me.Label28.TabIndex = 6
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextBox12
        '
        Me.TextBox12.Location = New System.Drawing.Point(72, 88)
        Me.TextBox12.MaxLength = 4
        Me.TextBox12.Name = "TextBox12"
        Me.TextBox12.Size = New System.Drawing.Size(24, 20)
        Me.TextBox12.TabIndex = 5
        Me.TextBox12.TabStop = False
        Me.TextBox12.Text = "2.5"
        '
        'Label27
        '
        Me.Label27.Location = New System.Drawing.Point(16, 88)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(56, 24)
        Me.Label27.TabIndex = 4
        Me.Label27.Text = "Desired Mixture %"
        '
        'TextBox11
        '
        Me.TextBox11.Location = New System.Drawing.Point(72, 56)
        Me.TextBox11.MaxLength = 2
        Me.TextBox11.Name = "TextBox11"
        Me.TextBox11.Size = New System.Drawing.Size(24, 20)
        Me.TextBox11.TabIndex = 3
        Me.TextBox11.TabStop = False
        Me.TextBox11.Text = "50"
        '
        'Label26
        '
        Me.Label26.Location = New System.Drawing.Point(16, 56)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(56, 24)
        Me.Label26.TabIndex = 2
        Me.Label26.Text = "Dextrose Solution"
        '
        'TextBox10
        '
        Me.TextBox10.Location = New System.Drawing.Point(72, 16)
        Me.TextBox10.MaxLength = 4
        Me.TextBox10.Name = "TextBox10"
        Me.TextBox10.Size = New System.Drawing.Size(32, 20)
        Me.TextBox10.TabIndex = 1
        Me.TextBox10.TabStop = False
        Me.TextBox10.Text = "1000"
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(8, 16)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(56, 32)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Fluid Vol. (ml)"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'GroupBox10
        '
        Me.GroupBox10.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox10.Controls.Add(Me.Label47)
        Me.GroupBox10.Controls.Add(Me.Label46)
        Me.GroupBox10.Controls.Add(Me.Label45)
        Me.GroupBox10.Controls.Add(Me.Label44)
        Me.GroupBox10.Controls.Add(Me.Label43)
        Me.GroupBox10.Controls.Add(Me.Label42)
        Me.GroupBox10.Controls.Add(Me.Label41)
        Me.GroupBox10.Controls.Add(Me.Label40)
        Me.GroupBox10.Controls.Add(Me.Label39)
        Me.GroupBox10.Controls.Add(Me.Label38)
        Me.GroupBox10.Controls.Add(Me.Label37)
        Me.GroupBox10.Controls.Add(Me.Label36)
        Me.GroupBox10.Controls.Add(Me.Label35)
        Me.GroupBox10.Controls.Add(Me.Label34)
        Me.GroupBox10.Location = New System.Drawing.Point(8, 384)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(520, 72)
        Me.GroupBox10.TabIndex = 25
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "Shock Dosages - Center Ranges, Crystalloid Fluids"
        '
        'Label47
        '
        Me.Label47.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label47.Location = New System.Drawing.Point(408, 47)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(104, 16)
        Me.Label47.TabIndex = 13
        Me.Label47.Text = "Range 35-50 ml/kg"
        '
        'Label46
        '
        Me.Label46.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label46.Location = New System.Drawing.Point(408, 32)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(104, 16)
        Me.Label46.TabIndex = 12
        Me.Label46.Text = "Range 10-20 ml/kg"
        '
        'Label45
        '
        Me.Label45.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label45.Location = New System.Drawing.Point(152, 47)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(104, 16)
        Me.Label45.TabIndex = 11
        Me.Label45.Text = "Range 70-90 ml/kg"
        '
        'Label44
        '
        Me.Label44.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label44.Location = New System.Drawing.Point(152, 32)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(104, 16)
        Me.Label44.TabIndex = 10
        Me.Label44.Text = "Range 20-40 ml/kg"
        '
        'Label43
        '
        Me.Label43.BackColor = System.Drawing.Color.DarkRed
        Me.Label43.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label43.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label43.ForeColor = System.Drawing.Color.White
        Me.Label43.Location = New System.Drawing.Point(336, 47)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(64, 16)
        Me.Label43.TabIndex = 9
        Me.Label43.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label42
        '
        Me.Label42.BackColor = System.Drawing.Color.DarkRed
        Me.Label42.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label42.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label42.ForeColor = System.Drawing.Color.White
        Me.Label42.Location = New System.Drawing.Point(336, 32)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(64, 16)
        Me.Label42.TabIndex = 8
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label41
        '
        Me.Label41.Location = New System.Drawing.Point(272, 48)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(72, 16)
        Me.Label41.TabIndex = 7
        Me.Label41.Text = "Over 1 Hour"
        '
        'Label40
        '
        Me.Label40.Location = New System.Drawing.Point(272, 32)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(72, 16)
        Me.Label40.TabIndex = 6
        Me.Label40.Text = "First 15 Min."
        '
        'Label39
        '
        Me.Label39.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label39.Location = New System.Drawing.Point(336, 16)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(176, 16)
        Me.Label39.TabIndex = 5
        Me.Label39.Text = "Feline (ml)"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label38
        '
        Me.Label38.BackColor = System.Drawing.Color.DarkRed
        Me.Label38.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label38.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label38.ForeColor = System.Drawing.Color.White
        Me.Label38.Location = New System.Drawing.Point(80, 47)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(64, 16)
        Me.Label38.TabIndex = 4
        Me.Label38.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label37
        '
        Me.Label37.BackColor = System.Drawing.Color.DarkRed
        Me.Label37.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label37.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label37.ForeColor = System.Drawing.Color.White
        Me.Label37.Location = New System.Drawing.Point(80, 32)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(64, 16)
        Me.Label37.TabIndex = 3
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label36
        '
        Me.Label36.Location = New System.Drawing.Point(8, 48)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(72, 16)
        Me.Label36.TabIndex = 2
        Me.Label36.Text = "Over 1 Hour"
        '
        'Label35
        '
        Me.Label35.Location = New System.Drawing.Point(8, 32)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(72, 16)
        Me.Label35.TabIndex = 1
        Me.Label35.Text = "First 15 Min."
        '
        'Label34
        '
        Me.Label34.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label34.Location = New System.Drawing.Point(80, 16)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(176, 16)
        Me.Label34.TabIndex = 0
        Me.Label34.Text = "Canine (ml)"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label52
        '
        Me.Label52.BackColor = System.Drawing.Color.MediumBlue
        Me.Label52.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label52.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label52.ForeColor = System.Drawing.Color.White
        Me.Label52.Location = New System.Drawing.Point(280, 216)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(144, 16)
        Me.Label52.TabIndex = 26
        Me.Label52.Text = "Number of Pills/Dose"
        Me.Label52.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label53
        '
        Me.Label53.BackColor = System.Drawing.Color.MediumSpringGreen
        Me.Label53.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label53.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label53.Location = New System.Drawing.Point(280, 232)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(144, 16)
        Me.Label53.TabIndex = 27
        Me.Label53.Text = "Pending..."
        Me.Label53.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 5000
        Me.ToolTip1.InitialDelay = 100
        Me.ToolTip1.ReshowDelay = 100
        Me.ToolTip1.ShowAlways = True
        '
        'GroupBox11
        '
        Me.GroupBox11.Controls.Add(Me.Label70)
        Me.GroupBox11.Controls.Add(Me.Label64)
        Me.GroupBox11.Controls.Add(Me.Label63)
        Me.GroupBox11.Controls.Add(Me.TextBox18)
        Me.GroupBox11.Controls.Add(Me.Label62)
        Me.GroupBox11.Controls.Add(Me.TextBox17)
        Me.GroupBox11.Controls.Add(Me.TextBox16)
        Me.GroupBox11.Controls.Add(Me.Label61)
        Me.GroupBox11.Controls.Add(Me.Label60)
        Me.GroupBox11.Controls.Add(Me.Label59)
        Me.GroupBox11.Controls.Add(Me.Label58)
        Me.GroupBox11.Controls.Add(Me.Label57)
        Me.GroupBox11.Controls.Add(Me.Label56)
        Me.GroupBox11.Controls.Add(Me.TextBox15)
        Me.GroupBox11.Controls.Add(Me.TextBox14)
        Me.GroupBox11.Controls.Add(Me.Label55)
        Me.GroupBox11.Controls.Add(Me.Label54)
        Me.GroupBox11.Location = New System.Drawing.Point(632, 8)
        Me.GroupBox11.Name = "GroupBox11"
        Me.GroupBox11.Size = New System.Drawing.Size(168, 240)
        Me.GroupBox11.TabIndex = 29
        Me.GroupBox11.TabStop = False
        Me.GroupBox11.Text = "Conversions"
        '
        'Label70
        '
        Me.Label70.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label70.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label70.Location = New System.Drawing.Point(128, 192)
        Me.Label70.Name = "Label70"
        Me.Label70.Size = New System.Drawing.Size(24, 16)
        Me.Label70.TabIndex = 16
        Me.Label70.Text = "="
        Me.Label70.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label64
        '
        Me.Label64.Location = New System.Drawing.Point(128, 136)
        Me.Label64.Name = "Label64"
        Me.Label64.Size = New System.Drawing.Size(16, 16)
        Me.Label64.TabIndex = 15
        Me.Label64.Text = "ml"
        '
        'Label63
        '
        Me.Label63.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label63.Location = New System.Drawing.Point(80, 136)
        Me.Label63.Name = "Label63"
        Me.Label63.Size = New System.Drawing.Size(48, 16)
        Me.Label63.TabIndex = 14
        Me.Label63.Text = "Cnvrt"
        '
        'TextBox18
        '
        Me.TextBox18.Location = New System.Drawing.Point(80, 112)
        Me.TextBox18.Name = "TextBox18"
        Me.TextBox18.Size = New System.Drawing.Size(40, 20)
        Me.TextBox18.TabIndex = 13
        Me.TextBox18.TabStop = False
        Me.TextBox18.Text = "000"
        '
        'Label62
        '
        Me.Label62.Location = New System.Drawing.Point(16, 112)
        Me.Label62.Name = "Label62"
        Me.Label62.Size = New System.Drawing.Size(64, 16)
        Me.Label62.TabIndex = 12
        Me.Label62.Text = "Ounces (fl)"
        '
        'TextBox17
        '
        Me.TextBox17.Location = New System.Drawing.Point(80, 208)
        Me.TextBox17.Name = "TextBox17"
        Me.TextBox17.Size = New System.Drawing.Size(40, 20)
        Me.TextBox17.TabIndex = 11
        Me.TextBox17.TabStop = False
        Me.TextBox17.Text = "000.0"
        '
        'TextBox16
        '
        Me.TextBox16.Location = New System.Drawing.Point(80, 176)
        Me.TextBox16.Name = "TextBox16"
        Me.TextBox16.Size = New System.Drawing.Size(40, 20)
        Me.TextBox16.TabIndex = 10
        Me.TextBox16.TabStop = False
        Me.TextBox16.Text = "000.0"
        '
        'Label61
        '
        Me.Label61.Location = New System.Drawing.Point(8, 208)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(80, 16)
        Me.Label61.TabIndex = 9
        Me.Label61.Text = "Temprature C"
        '
        'Label60
        '
        Me.Label60.Location = New System.Drawing.Point(8, 176)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(80, 16)
        Me.Label60.TabIndex = 8
        Me.Label60.Text = "Temprature F"
        '
        'Label59
        '
        Me.Label59.Location = New System.Drawing.Point(128, 88)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(16, 16)
        Me.Label59.TabIndex = 7
        Me.Label59.Text = "ml"
        '
        'Label58
        '
        Me.Label58.Location = New System.Drawing.Point(128, 40)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(16, 16)
        Me.Label58.TabIndex = 6
        Me.Label58.Text = "ml"
        '
        'Label57
        '
        Me.Label57.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label57.Location = New System.Drawing.Point(80, 88)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(48, 16)
        Me.Label57.TabIndex = 5
        Me.Label57.Text = "Cnvrt"
        '
        'Label56
        '
        Me.Label56.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label56.Location = New System.Drawing.Point(80, 40)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(48, 16)
        Me.Label56.TabIndex = 4
        Me.Label56.Text = "Cnvrt"
        '
        'TextBox15
        '
        Me.TextBox15.Location = New System.Drawing.Point(80, 64)
        Me.TextBox15.Name = "TextBox15"
        Me.TextBox15.Size = New System.Drawing.Size(40, 20)
        Me.TextBox15.TabIndex = 3
        Me.TextBox15.TabStop = False
        Me.TextBox15.Text = "000"
        '
        'TextBox14
        '
        Me.TextBox14.Location = New System.Drawing.Point(80, 16)
        Me.TextBox14.Name = "TextBox14"
        Me.TextBox14.Size = New System.Drawing.Size(40, 20)
        Me.TextBox14.TabIndex = 2
        Me.TextBox14.TabStop = False
        Me.TextBox14.Text = "000"
        '
        'Label55
        '
        Me.Label55.Location = New System.Drawing.Point(16, 64)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(64, 16)
        Me.Label55.TabIndex = 1
        Me.Label55.Text = "Teaspoon/s"
        '
        'Label54
        '
        Me.Label54.Location = New System.Drawing.Point(8, 16)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(72, 16)
        Me.Label54.TabIndex = 0
        Me.Label54.Text = "Tablespoon/s"
        '
        'GroupBox13
        '
        Me.GroupBox13.Controls.Add(Me.Label76)
        Me.GroupBox13.Controls.Add(Me.Label75)
        Me.GroupBox13.Controls.Add(Me.Label74)
        Me.GroupBox13.Controls.Add(Me.Label73)
        Me.GroupBox13.Controls.Add(Me.Label71)
        Me.GroupBox13.Controls.Add(Me.Label69)
        Me.GroupBox13.Controls.Add(Me.Label80)
        Me.GroupBox13.Controls.Add(Me.Label79)
        Me.GroupBox13.Controls.Add(Me.Label78)
        Me.GroupBox13.Controls.Add(Me.Label77)
        Me.GroupBox13.Location = New System.Drawing.Point(632, 256)
        Me.GroupBox13.Name = "GroupBox13"
        Me.GroupBox13.Size = New System.Drawing.Size(168, 120)
        Me.GroupBox13.TabIndex = 31
        Me.GroupBox13.TabStop = False
        Me.GroupBox13.Text = "Seizure Drugs "
        '
        'Label76
        '
        Me.Label76.Location = New System.Drawing.Point(136, 96)
        Me.Label76.Name = "Label76"
        Me.Label76.Size = New System.Drawing.Size(24, 16)
        Me.Label76.TabIndex = 10
        Me.Label76.Text = "mg"
        '
        'Label75
        '
        Me.Label75.Location = New System.Drawing.Point(136, 72)
        Me.Label75.Name = "Label75"
        Me.Label75.Size = New System.Drawing.Size(24, 16)
        Me.Label75.TabIndex = 9
        Me.Label75.Text = "mg"
        '
        'Label74
        '
        Me.Label74.Location = New System.Drawing.Point(136, 40)
        Me.Label74.Name = "Label74"
        Me.Label74.Size = New System.Drawing.Size(24, 16)
        Me.Label74.TabIndex = 8
        Me.Label74.Text = "ml"
        '
        'Label73
        '
        Me.Label73.Location = New System.Drawing.Point(136, 16)
        Me.Label73.Name = "Label73"
        Me.Label73.Size = New System.Drawing.Size(24, 16)
        Me.Label73.TabIndex = 7
        Me.Label73.Text = "mg"
        '
        'Label71
        '
        Me.Label71.BackColor = System.Drawing.SystemColors.Window
        Me.Label71.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label71.Location = New System.Drawing.Point(96, 40)
        Me.Label71.Name = "Label71"
        Me.Label71.Size = New System.Drawing.Size(40, 16)
        Me.Label71.TabIndex = 5
        '
        'Label69
        '
        Me.Label69.BackColor = System.Drawing.SystemColors.Window
        Me.Label69.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label69.Location = New System.Drawing.Point(96, 96)
        Me.Label69.Name = "Label69"
        Me.Label69.Size = New System.Drawing.Size(40, 16)
        Me.Label69.TabIndex = 4
        '
        'Label80
        '
        Me.Label80.BackColor = System.Drawing.SystemColors.Window
        Me.Label80.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label80.Location = New System.Drawing.Point(96, 72)
        Me.Label80.Name = "Label80"
        Me.Label80.Size = New System.Drawing.Size(40, 16)
        Me.Label80.TabIndex = 3
        '
        'Label79
        '
        Me.Label79.Location = New System.Drawing.Point(8, 72)
        Me.Label79.Name = "Label79"
        Me.Label79.Size = New System.Drawing.Size(80, 40)
        Me.Label79.TabIndex = 2
        Me.Label79.Text = "Phenobarbital 3.5mg/kg PO BID Dosage"
        Me.Label79.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label78
        '
        Me.Label78.BackColor = System.Drawing.SystemColors.Window
        Me.Label78.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label78.Location = New System.Drawing.Point(96, 16)
        Me.Label78.Name = "Label78"
        Me.Label78.Size = New System.Drawing.Size(40, 16)
        Me.Label78.TabIndex = 1
        '
        'Label77
        '
        Me.Label77.Location = New System.Drawing.Point(8, 16)
        Me.Label77.Name = "Label77"
        Me.Label77.Size = New System.Drawing.Size(80, 40)
        Me.Label77.TabIndex = 0
        Me.Label77.Text = "Diazepam (IV)  0.375mg/kg   @ 5mg/ml"
        Me.Label77.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label81
        '
        Me.Label81.BackColor = System.Drawing.Color.MediumBlue
        Me.Label81.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label81.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label81.ForeColor = System.Drawing.Color.White
        Me.Label81.Location = New System.Drawing.Point(280, 120)
        Me.Label81.Name = "Label81"
        Me.Label81.Size = New System.Drawing.Size(144, 16)
        Me.Label81.TabIndex = 33
        Me.Label81.Text = "Dosage (Teaspoon/s)"
        Me.Label81.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label82
        '
        Me.Label82.BackColor = System.Drawing.Color.MediumSpringGreen
        Me.Label82.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label82.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label82.Location = New System.Drawing.Point(280, 136)
        Me.Label82.Name = "Label82"
        Me.Label82.Size = New System.Drawing.Size(144, 16)
        Me.Label82.TabIndex = 34
        Me.Label82.Text = "Pending..."
        Me.Label82.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'StatusBar2
        '
        Me.StatusBar2.Location = New System.Drawing.Point(0, 459)
        Me.StatusBar2.Name = "StatusBar2"
        Me.StatusBar2.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.StatusBarPanel4, Me.StatusBarPanel5, Me.StatusBarPanel6, Me.StatusBarPanel7})
        Me.StatusBar2.ShowPanels = True
        Me.StatusBar2.Size = New System.Drawing.Size(806, 24)
        Me.StatusBar2.SizingGrip = False
        Me.StatusBar2.TabIndex = 36
        Me.StatusBar2.Text = "StatusBar2"
        '
        'StatusBarPanel4
        '
        Me.StatusBarPanel4.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.StatusBarPanel4.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.StatusBarPanel4.MinWidth = 161
        Me.StatusBarPanel4.Text = "Epinephrine (ml):  0000"
        Me.StatusBarPanel4.ToolTipText = "0.1mg/kg @ 1:1000"
        Me.StatusBarPanel4.Width = 201
        '
        'StatusBarPanel5
        '
        Me.StatusBarPanel5.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.StatusBarPanel5.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.StatusBarPanel5.MinWidth = 161
        Me.StatusBarPanel5.Text = "Atropine (ml):  0000"
        Me.StatusBarPanel5.ToolTipText = "0.02mg/lb @ 0.5mg/ml"
        Me.StatusBarPanel5.Width = 201
        '
        'StatusBarPanel6
        '
        Me.StatusBarPanel6.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.StatusBarPanel6.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.StatusBarPanel6.MinWidth = 161
        Me.StatusBarPanel6.Text = "Sodium Bicarbonate (mEq):  0000"
        Me.StatusBarPanel6.ToolTipText = "0.25mEq/lb @ 1 mEq/ml"
        Me.StatusBarPanel6.Width = 201
        '
        'StatusBarPanel7
        '
        Me.StatusBarPanel7.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.StatusBarPanel7.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.StatusBarPanel7.MinWidth = 161
        Me.StatusBarPanel7.Text = "Naloxone (ml):  0000"
        Me.StatusBarPanel7.ToolTipText = "0.03mg/kg @ 0.4mg/ml"
        Me.StatusBarPanel7.Width = 201
        '
        'GroupBox12
        '
        Me.GroupBox12.Controls.Add(Me.Label68)
        Me.GroupBox12.Controls.Add(Me.Label67)
        Me.GroupBox12.Controls.Add(Me.Label66)
        Me.GroupBox12.Controls.Add(Me.Label65)
        Me.GroupBox12.Location = New System.Drawing.Point(536, 384)
        Me.GroupBox12.Name = "GroupBox12"
        Me.GroupBox12.Size = New System.Drawing.Size(152, 72)
        Me.GroupBox12.TabIndex = 37
        Me.GroupBox12.TabStop = False
        Me.GroupBox12.Text = "Diphenhydramine"
        '
        'Label68
        '
        Me.Label68.BackColor = System.Drawing.SystemColors.Window
        Me.Label68.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label68.Location = New System.Drawing.Point(72, 48)
        Me.Label68.Name = "Label68"
        Me.Label68.Size = New System.Drawing.Size(60, 16)
        Me.Label68.TabIndex = 3
        '
        'Label67
        '
        Me.Label67.BackColor = System.Drawing.SystemColors.Window
        Me.Label67.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label67.Location = New System.Drawing.Point(72, 24)
        Me.Label67.Name = "Label67"
        Me.Label67.Size = New System.Drawing.Size(60, 16)
        Me.Label67.TabIndex = 2
        '
        'Label66
        '
        Me.Label66.Location = New System.Drawing.Point(8, 48)
        Me.Label66.Name = "Label66"
        Me.Label66.Size = New System.Drawing.Size(64, 16)
        Me.Label66.TabIndex = 1
        Me.Label66.Text = "@ 50mg/ml"
        '
        'Label65
        '
        Me.Label65.Location = New System.Drawing.Point(24, 24)
        Me.Label65.Name = "Label65"
        Me.Label65.Size = New System.Drawing.Size(48, 16)
        Me.Label65.TabIndex = 0
        Me.Label65.Text = "IV (mg)"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.LightSteelBlue
        Me.ClientSize = New System.Drawing.Size(806, 507)
        Me.Controls.Add(Me.GroupBox12)
        Me.Controls.Add(Me.StatusBar2)
        Me.Controls.Add(Me.Label82)
        Me.Controls.Add(Me.Label81)
        Me.Controls.Add(Me.GroupBox13)
        Me.Controls.Add(Me.Label53)
        Me.Controls.Add(Me.Label52)
        Me.Controls.Add(Me.GroupBox10)
        Me.Controls.Add(Me.GroupBox9)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.GroupBox8)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.GroupBox7)
        Me.Controls.Add(Me.GroupBox6)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox11)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "Stillinger's Dosage Calculator-XP"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox8.ResumeLayout(False)
        CType(Me.StatusBarPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StatusBarPanel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StatusBarPanel3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Timer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox11.ResumeLayout(False)
        Me.GroupBox13.ResumeLayout(False)
        CType(Me.StatusBarPanel4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StatusBarPanel5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StatusBarPanel6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StatusBarPanel7, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox12.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    'Calculator function, returns calculations preformed seperatly from fluid calcs.
    Public Sub calcu()
        ' Calculate the dosage based on weight, protected against a divide by zero error.
        ' Configure variable to use a double interger value, less accurate.
        Dim xweight As Double
        Dim adosage As Double
        Dim bdosage As Double
        Dim concentration As Double
        Dim cpill As Double
        Dim cpilld As Double
        Dim cdays As Double
        'Weight and dosage in kg
        If CheckBox1.Checked = True And CheckBox3.Checked = True And TextBox2.Text <> "0000" And TextBox2.Text <> "" Then
            xweight = Val(TextBox1.Text)
            adosage = Val(TextBox2.Text)
            Label2.Text = Str(xweight * adosage)
            TextBox3.Text = Label2.Text
        End If
        ' Weight and dosage in pounds
        If CheckBox2.Checked = True And CheckBox4.Checked = True And TextBox2.Text <> "0000" And TextBox2.Text <> "" Then
            xweight = Val(TextBox1.Text)
            adosage = Val(TextBox2.Text)
            Label2.Text = Str(xweight * adosage)
        End If
        'Weight in pounds, dosage in kg
        If CheckBox2.Checked = True And CheckBox3.Checked = True And TextBox1.Text <> "000.0" And TextBox2.Text <> "0000" And TextBox2.Text <> "" Then
            xweight = Val(TextBox1.Text) / 2.2
            adosage = Val(TextBox2.Text)
            Label2.Text = Str(xweight * adosage)
            TextBox3.Text = Label2.Text
        End If
        'Weight in kg, dosage in lbs
        If CheckBox1.Checked = True And CheckBox4.Checked = True And TextBox1.Text <> "000.0" And TextBox2.Text <> "0000" And TextBox2.Text <> "" Then
            xweight = Val(TextBox1.Text) * 2.2
            adosage = Val(TextBox2.Text)
            Label2.Text = Str(xweight * adosage)
            TextBox3.Text = Label2.Text
        End If
        ' mg -> ml calculation.  Protected from a divide by zero error.
        If TextBox3.Text <> "0000" And TextBox3.Text <> "" And TextBox4.Text <> "0000" And TextBox4.Text <> "" Then
            bdosage = Val(TextBox3.Text)
            concentration = Val(TextBox4.Text)
            Label4.Text = Str(bdosage / concentration)
            'teaspoon conversion
            Label82.Text = Str(Val(Label4.Text) / 5)
        End If
        'Calculate the pill stuff here.  Protect against a divide by zero error.
        If TextBox3.Text <> "0000" And TextBox3.Text <> "" And TextBox5.Text <> "0000" And TextBox5.Text <> "" Then
            cpill = Val(TextBox3.Text) / Val(TextBox5.Text)
            Label53.Text = Str(cpill)
            cdays = Val(TextBox9.Text)
            ' This turned out to be a rather harsh error trap.
            ' but keeps the calcuations accurate.
            If CheckBox7.Checked = True Then
                TextBox9.Text = "01"
                cpilld = cpill
                Label18.Text = Str(cpilld * cdays)
            ElseIf CheckBox8.Checked = True Then
                cpilld = cpill * 2
                Label18.Text = Str(cpilld * cdays)
            ElseIf CheckBox9.Checked = True Then
                cpilld = cpill * 3
                Label18.Text = Str(cpilld * cdays)
            ElseIf CheckBox10.Checked = True Then
                cpilld = cpill * 4
                Label18.Text = Str(cpilld * cdays)
            End If
        End If
    End Sub
    Public Sub fluid_calc()
        'Fluid Calculations.  Returns fluid calcs all at once.
        'Normal dosage range is 40-60ml per kg.  Program defaults 
        'to 50, the center range.
        Dim cweight As Double
        Dim edose As Double
        'Dim dropspermin As Double
        Dim dropspersecond As Double
        timergo = 0
        'Weight in kg, protected against a divide by zero error.
        If CheckBox1.Checked = True And CheckBox2.Checked = False And TextBox1.Text <> "000.0" And TextBox1.Text <> "" Then
            cweight = Val(TextBox1.Text)
            'Weight in lbs, converted to kg before final calc.
        ElseIf CheckBox1.Checked = False And CheckBox2.Checked = True And TextBox1.Text <> "000.0" And TextBox1.Text <> "" Then
            cweight = Val(TextBox1.Text / 2.2)
        End If
        'Final fluid calculation, protected against a divide by zero error.
        'no more of the over and under cancel fraction type math.  Make algabra
        'and be done with it.
        If TextBox1.Text <> "000.0" And TextBox1.Text <> "" Then
            '24 Hour Maint calc
            Label6.Text = Str(50 * cweight)
            'ml/hr pump setting calc
            Label8.Text = Str(50 * cweight / 24)
            'Oxyglobin calc
            Label10.Text = Str(10 * cweight / 24)
            Label49.Text = Str(30 * cweight)
            'Drip Rate Calc
            If TextBox13.Text = "00" Or TextBox13.Text = "" Then
                dropspersecond = 60 / (((50 * cweight / 24) / 60) * 20)
            ElseIf TextBox13.Text <> "00" Or TextBox13.Text <> "" Then
                dropspersecond = 60 / (((50 * cweight / 24) / 60) * Val(TextBox13.Text))
            End If
            Label13.Text = Str(dropspersecond)
            timergo = dropspersecond
            'Plasma dosage calc
            Label16.Text = Str(5 * cweight)
            Label51.Text = Str(5 * cweight / 24)
            'Sock dosages crystalloid fluids
            'canine
            Label37.Text = Str(30 * cweight)
            Label38.Text = Str(80 * cweight)
            'feline
            Label42.Text = Str(15 * cweight)
            Label43.Text = Str(41.5 * cweight)
            'seizure dosages
            Label78.Text = Str(0.375 * cweight)
            Label71.Text = Str(Val(Label78.Text) / 5)
            Label80.Text = Str(3.5 * cweight)
            Label69.Text = Str(Val(Label80.Text) / 2)
            'diphenhydramine dosage
            Label67.Text = cweight * 0.5
            Label68.Text = Val(Label67.Text) / 50
            'emergency drug dosages
            'epi
            edose = 0.0
            edose = cweight * 0.1
            StatusBarPanel4.Text = "Epinephrine (ml): " + Str(edose)
            'atropine
            edose = 0.0
            edose = (cweight * 2.2) * 0.02
            StatusBarPanel5.Text = "Atropine (ml): " + Str(edose / 0.5)
            'sodium bicarb
            StatusBarPanel6.Text = "Sodium Bicarbonate (mEq): " + Str((cweight * 2.2) * 0.25)
            'naloxone
            edose = 0.0
            edose = cweight * 0.03
            StatusBarPanel7.Text = "Naloxone (ml): " + Str(edose / 0.4)
            edose = 0.0
        End If
    End Sub
    Public Sub blood_trans()
        Dim bweight As Double
        Dim ppcv As Double
        Dim doner As Double
        Dim targetpcv As Double
        'protect against a divide by zero error with this statement.
        'make sure we have everything we need
        If TextBox6.Text <> "00" And TextBox6.Text <> "" And TextBox7.Text <> "00" And TextBox7.Text <> "" And TextBox8.Text <> "00" And TextBox8.Text <> "" Then
            doner = Val(TextBox6.Text)
            ppcv = Val(TextBox7.Text)
            targetpcv = Val(TextBox8.Text)
            If CheckBox5.Checked = True And CheckBox6.Checked = False Then
                ' For a woofer
                'Weight in kg
                If CheckBox1.Checked = True And CheckBox2.Checked = False Then
                    bweight = Val(TextBox1.Text)
                    Label23.Text = bweight * 80 * (targetpcv - ppcv) / doner
                    Label25.Text = bweight * 80 * (targetpcv - ppcv) / doner / 24
                    'weight in lbs
                ElseIf CheckBox1.Checked = False And CheckBox2.Checked = True Then
                    bweight = Val(TextBox1.Text)
                    Label23.Text = bweight * 40 * (targetpcv - ppcv) / doner
                    Label25.Text = bweight * 40 * (targetpcv - ppcv) / doner / 24
                End If
            ElseIf CheckBox5.Checked = False And CheckBox6.Checked = True Then
                ' for a meower
                'weight in kg
                If CheckBox1.Checked = True And CheckBox2.Checked = False Then
                    bweight = Val(TextBox1.Text)
                    Label23.Text = bweight * 70 * (targetpcv - ppcv) / doner
                    Label25.Text = bweight * 70 * (targetpcv - ppcv) / doner / 24
                    'weight in lbs
                ElseIf CheckBox2.Checked = False And CheckBox2.Checked = True Then
                    bweight = Val(TextBox1.Text)
                    Label23.Text = bweight * 30 * (targetpcv - ppcv) / doner
                    Label25.Text = bweight * 30 * (targetpcv - ppcv) / doner / 24
                End If
            End If
        End If
    End Sub
    Public Sub dexcalc()
        'Protect from a divide by zero error
        If TextBox10.Text <> "" And TextBox11.Text <> "" And TextBox12.Text <> "" Then
            Dim dex As Double
            Dim desired As Double
            Dim volume As Double
            volume = Val(TextBox10.Text)
            dex = Val(TextBox11.Text)
            desired = Val(TextBox12.Text)
            Label28.Text = Str(volume * desired / dex)
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text <> "000.0" And TextBox1.Text <> "" Then
            'do the fluid calcs again, in case something has changed.
            fluid_calc()
            'do the dosage calcs
            calcu()
            'do the blood transfusion 
            blood_trans()
            'do dextrose calc
            dexcalc()
            'set up for the next dosage
            TextBox1.Focus()
        ElseIf TextBox1.Text = "000.0" Or TextBox1.Text = "" Then
            MsgBox("YOU MUST PROVIDE A WEIGHT", MsgBoxStyle.OKOnly, "Calculation Warning: WEIGHT REQUIRED")
            TextBox1.Focus()
        End If
    End Sub
    Private Sub TextBox1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.LostFocus
        If TextBox1.Text <> "000.0" And TextBox1.Text <> "" Then
            fluid_calc()
            dexcalc()
        End If
    End Sub
    'This is how the effect is created for the checkboxes.  Used to lock down variables. 
    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            CheckBox2.Checked = False
            CheckBox2.Enabled = False
        ElseIf CheckBox1.Checked = False Then
            CheckBox2.Enabled = True
            CheckBox2.Checked = True
        End If
        fluid_calc()
    End Sub
    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            CheckBox1.Checked = False
            CheckBox1.Enabled = False
        ElseIf CheckBox2.Checked = False Then
            CheckBox1.Enabled = True
            CheckBox1.Checked = True
        End If
        fluid_calc()
    End Sub
    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked = True Then
            CheckBox4.Checked = False
            CheckBox4.Enabled = False
        ElseIf CheckBox3.Checked = False Then
            CheckBox4.Enabled = True
            CheckBox4.Checked = True
        End If
        calcu()
    End Sub
    Private Sub CheckBox4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked = True Then
            CheckBox3.Checked = False
            CheckBox3.Enabled = False
        ElseIf CheckBox4.Checked = False Then
            CheckBox3.Enabled = True
            CheckBox3.Checked = True
        End If
        calcu()
    End Sub
    Private Sub CheckBox5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox5.CheckedChanged
        If CheckBox5.Checked = True Then
            CheckBox6.Checked = False
            CheckBox6.Enabled = False
            TextBox8.Text = "45.5"
            Label32.Text = "Normal PCV: 37 - 55%"
            Button1.Focus()
        ElseIf CheckBox5.Checked = False Then
            CheckBox6.Enabled = True
            CheckBox6.Checked = True
            TextBox8.Text = "37"
            Label32.Text = "Normal PCV: 30 - 45%"
            Button1.Focus()
        End If
        blood_trans()
    End Sub
    Private Sub CheckBox6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox6.CheckedChanged
        If CheckBox6.Checked = True Then
            CheckBox5.Checked = False
            CheckBox5.Enabled = False
            TextBox8.Text = "37"
            Label32.Text = "Normal PCV: 30 - 45%"
            Button1.Focus()
        ElseIf CheckBox6.Checked = False Then
            CheckBox5.Enabled = True
            CheckBox5.Checked = True
            TextBox8.Text = "45.5"
            Label32.Text = "Normal PCV: 37 - 55%"
            Button1.Focus()
        End If
        blood_trans()
    End Sub
    Private Sub CheckBox7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox7.CheckedChanged
        If CheckBox7.Checked = True Then
            CheckBox8.Checked = False
            CheckBox9.Checked = False
            CheckBox10.Checked = False
            CheckBox8.Enabled = False
            CheckBox9.Enabled = False
            CheckBox10.Enabled = False
        ElseIf CheckBox7.Checked = False Then
            CheckBox8.Checked = False
            CheckBox9.Checked = False
            CheckBox10.Checked = False
            CheckBox8.Enabled = True
            CheckBox9.Enabled = True
            CheckBox10.Enabled = True
        End If
    End Sub
    Private Sub CheckBox8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox8.CheckedChanged
        If CheckBox8.Checked = True Then
            CheckBox7.Checked = False
            CheckBox9.Checked = False
            CheckBox10.Checked = False
            CheckBox7.Enabled = False
            CheckBox9.Enabled = False
            CheckBox10.Enabled = False
        ElseIf CheckBox8.Checked = False Then
            CheckBox7.Checked = False
            CheckBox9.Checked = False
            CheckBox10.Checked = False
            CheckBox7.Enabled = True
            CheckBox9.Enabled = True
            CheckBox10.Enabled = True
        End If
    End Sub
    Private Sub CheckBox9_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox9.CheckedChanged
        If CheckBox9.Checked = True Then
            CheckBox7.Checked = False
            CheckBox8.Checked = False
            CheckBox10.Checked = False
            CheckBox7.Enabled = False
            CheckBox8.Enabled = False
            CheckBox10.Enabled = False
        ElseIf CheckBox9.Checked = False Then
            CheckBox7.Checked = False
            CheckBox8.Checked = False
            CheckBox10.Checked = False
            CheckBox7.Enabled = True
            CheckBox8.Enabled = True
            CheckBox10.Enabled = True
        End If
    End Sub
    Private Sub CheckBox10_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox10.CheckedChanged
        If CheckBox10.Checked = True Then
            CheckBox7.Checked = False
            CheckBox8.Checked = False
            CheckBox9.Checked = False
            CheckBox7.Enabled = False
            CheckBox8.Enabled = False
            CheckBox9.Enabled = False
        ElseIf CheckBox10.Checked = False Then
            CheckBox7.Checked = False
            CheckBox8.Checked = False
            CheckBox9.Checked = False
            CheckBox7.Enabled = True
            CheckBox8.Enabled = True
            CheckBox9.Enabled = True
        End If
    End Sub
    Private Sub StatusBar1_PanelClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.StatusBarPanelClickEventArgs) Handles StatusBar1.PanelClick
        'Note you may not remove this function or form from the code.
        Dim gpl As New Form2
        gpl.Show()
    End Sub
    Private Sub Timer1_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles Timer1.Elapsed
        If CheckBox11.Checked = True And Label13.Text <> "" And timergo <> 0 Then
            Timer1.Interval = timergo * 1000
            Beep()
        End If
    End Sub
    Private Sub CheckBox11_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox11.CheckedChanged
        If CheckBox11.Checked = False Then
            timergo = 0
            Timer1.Interval = 1000
        End If
        Button1.Focus()
    End Sub
    Private Sub TextBox13_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox13.LostFocus
        fluid_calc()
    End Sub
    Private Sub GroupBox9_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox9.Enter
        MsgBox("SUBTRACT CALCULATED AMOUNT FROM BAG FIRST, REPLACE WITH CALCULATED AMOUNT OF DEXTROSE SOLUTION", MsgBoxStyle.OKOnly, "Calculation Warning")
    End Sub
    Private Sub TextBox2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox2.LostFocus
        calcu()
    End Sub
    Private Sub Label49_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label49.MouseHover
        ToolTip1.SetToolTip(Label49, "30ml x Weight in kg")
    End Sub
    Private Sub Label6_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label6.MouseHover
        ToolTip1.SetToolTip(Label6, "50ml x Weight in kg")
    End Sub
    Private Sub Label8_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label8.MouseHover
        ToolTip1.SetToolTip(Label8, "50 x Weight in kg / 24")
    End Sub
    Private Sub Label13_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label13.MouseHover
        ToolTip1.SetToolTip(Label13, "((ml per hour/60)x15)/60")
    End Sub
    Private Sub Label10_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label10.MouseHover
        ToolTip1.SetToolTip(Label10, "10ml x Weight in kg / 24")
    End Sub
    Private Sub Label16_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label16.MouseHover
        ToolTip1.SetToolTip(Label16, "5 x Weight in kg")
    End Sub
    Private Sub Label51_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label51.MouseHover
        ToolTip1.SetToolTip(Label51, "5 x Weight in kg / 24")
    End Sub
    Private Sub Label37_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label37.MouseHover
        ToolTip1.SetToolTip(Label37, "30 x Weight in kg")
    End Sub
    Private Sub Label38_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label38.MouseHover
        ToolTip1.SetToolTip(Label38, "80 x Weight in kg")
    End Sub
    Private Sub Label42_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label42.MouseHover
        ToolTip1.SetToolTip(Label42, "15 x Weight in kg")
    End Sub
    Private Sub Label43_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label43.MouseHover
        ToolTip1.SetToolTip(Label43, "41.5 x Weight in kg")
    End Sub
    Private Sub TextBox1_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.MouseHover
        ToolTip1.SetToolTip(TextBox1, "Enter weight in kg or lbs ** REQUIRED **")
    End Sub
    Private Sub TextBox2_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox2.MouseHover
        ToolTip1.SetToolTip(TextBox2, "Enter dosage from Plumbs or package insert")
    End Sub
    Private Sub TextBox3_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox3.MouseHover
        ToolTip1.SetToolTip(TextBox3, "Enter Doctors order if needed")
    End Sub
    Private Sub TextBox4_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox4.MouseHover
        ToolTip1.SetToolTip(TextBox4, "Enter concentration from the bottle")
    End Sub
    Private Sub TextBox5_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox5.MouseHover
        ToolTip1.SetToolTip(TextBox5, "Enter pill concentration")
    End Sub
    Private Sub Label67_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label67.MouseHover
        ToolTip1.SetToolTip(Label67, "Weight in kg x 0.5")
    End Sub
    Private Sub Label56_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label56.Click
        If TextBox14.Text <> "000" And TextBox14.Text <> "" Then
            'table spoon to ml
            Label56.Text = Str(Val(TextBox14.Text) * 15)
            Button1.Focus()
        ElseIf TextBox14.Text = "" Then
            TextBox14.Text = "000"
            Label56.Text = "Cnvrt"
        End If
    End Sub
    Private Sub Label57_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label57.Click
        If TextBox15.Text <> "000" And TextBox15.Text <> "" Then
            'tea spoon to ml
            Label57.Text = Str(Val(TextBox15.Text) * 5)
            Button1.Focus()
        ElseIf TextBox14.Text = "" Then
            TextBox15.Text = "000"
            Label57.Text = "Cnvrt"
        End If
    End Sub
    Private Sub Label63_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label63.Click
        If TextBox18.Text <> "000" And TextBox18.Text <> "" Then
            'oz to ml
            Label63.Text = Str(Val(TextBox18.Text) * 30)
            Button1.Focus()
        ElseIf TextBox18.Text = "" Then
            TextBox18.Text = "000"
            Label63.Text = "Cnvrt"
        End If
    End Sub
    Private Sub Label70_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label70.Click
        If TextBox16.Text <> "000.0" And TextBox16.Text <> "" And TextBox17.Text = "000.0" And TextBox17.Text <> "" Then
            'Convert from F to C
            TextBox17.Text = Str((Val(TextBox16.Text) - 32) * 5 / 9)
            Button1.Focus()
        ElseIf TextBox17.Text <> "000.0" And TextBox17.Text <> "" And TextBox16.Text = "000.0" And TextBox16.Text <> "" Then
            'Convert from C to F
            TextBox16.Text = Str((Val(TextBox17.Text) * 9 / 5) + 32)
            Button1.Focus()
        End If
    End Sub
End Class
