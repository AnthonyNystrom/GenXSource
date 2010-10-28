Imports Eval3

Public Class Form1
   Inherits System.Windows.Forms.Form

   Private mInitializing As Boolean
   WithEvents mFormula3 As Eval3.opCode
   Dim ev As Evaluator
   Public arr() As Double = New Double() {1.2, 3.4, 5.6, 7.8}

   Public Sub New()
      MyBase.New()
      mInitializing = True
      ev = New Eval3.Evaluator
      ev.AddEnvironmentFunctions(Me)
      ev.AddEnvironmentFunctions(New EvalFunctions)

      A = New Eval3.EvalVariable(0.0, GetType(Double))
      B = New Eval3.EvalVariable(0.0, GetType(Double))
      C = New Eval3.EvalVariable(0.0, GetType(Double))

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      A.Value = CDbl(updownA.Value)
      B.Value = CDbl(updownB.Value)
      C.Value = CDbl(updownC.Value)

      mInitializing = False
   End Sub

#Region " Windows Form Designer generated code "


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
   Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
   Friend WithEvents cbSamples As System.Windows.Forms.ComboBox
   Friend WithEvents tbExpression As System.Windows.Forms.TextBox
   Friend WithEvents btnEvaluate As System.Windows.Forms.Button
   Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
   Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
   Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
   Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
   Friend WithEvents btnEvaluate2 As System.Windows.Forms.Button
   Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
   Friend WithEvents Label1 As System.Windows.Forms.Label
   Friend WithEvents tbExpressionRed As System.Windows.Forms.TextBox
   Friend WithEvents tbExpressionGreen As System.Windows.Forms.TextBox
   Friend WithEvents Label2 As System.Windows.Forms.Label
   Friend WithEvents Label3 As System.Windows.Forms.Label
   Friend WithEvents tbExpressionBlue As System.Windows.Forms.TextBox
   Friend WithEvents Label4 As System.Windows.Forms.Label
   Friend WithEvents cbAuto As System.Windows.Forms.CheckBox
   Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
   Friend WithEvents updownA As System.Windows.Forms.NumericUpDown
   Friend WithEvents updownB As System.Windows.Forms.NumericUpDown
   Friend WithEvents updownC As System.Windows.Forms.NumericUpDown
   Friend WithEvents Label5 As System.Windows.Forms.Label
   Friend WithEvents Label6 As System.Windows.Forms.Label
   Friend WithEvents Label7 As System.Windows.Forms.Label
   Friend WithEvents Label12 As System.Windows.Forms.Label
   Friend WithEvents Label8 As System.Windows.Forms.Label
   Friend WithEvents tbExpression3 As System.Windows.Forms.TextBox
   Friend WithEvents lblResults3 As System.Windows.Forms.Label
   Friend WithEvents LogBox3 As System.Windows.Forms.TextBox
   Friend WithEvents btnEvaluate3 As System.Windows.Forms.Button
   Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
   Friend WithEvents RichTextControl1 As RichText.RichTextControl
   Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
   Friend WithEvents tbRichtextSrc As System.Windows.Forms.TextBox
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.tbExpression = New System.Windows.Forms.TextBox
      Me.TextBox2 = New System.Windows.Forms.TextBox
      Me.btnEvaluate = New System.Windows.Forms.Button
      Me.cbSamples = New System.Windows.Forms.ComboBox
      Me.TabControl1 = New System.Windows.Forms.TabControl
      Me.TabPage1 = New System.Windows.Forms.TabPage
      Me.TabPage2 = New System.Windows.Forms.TabPage
      Me.cbAuto = New System.Windows.Forms.CheckBox
      Me.Label1 = New System.Windows.Forms.Label
      Me.PictureBox1 = New System.Windows.Forms.PictureBox
      Me.tbExpressionRed = New System.Windows.Forms.TextBox
      Me.ComboBox1 = New System.Windows.Forms.ComboBox
      Me.btnEvaluate2 = New System.Windows.Forms.Button
      Me.tbExpressionGreen = New System.Windows.Forms.TextBox
      Me.Label2 = New System.Windows.Forms.Label
      Me.Label3 = New System.Windows.Forms.Label
      Me.tbExpressionBlue = New System.Windows.Forms.TextBox
      Me.Label4 = New System.Windows.Forms.Label
      Me.TabPage3 = New System.Windows.Forms.TabPage
      Me.btnEvaluate3 = New System.Windows.Forms.Button
      Me.LogBox3 = New System.Windows.Forms.TextBox
      Me.Label5 = New System.Windows.Forms.Label
      Me.updownA = New System.Windows.Forms.NumericUpDown
      Me.tbExpression3 = New System.Windows.Forms.TextBox
      Me.updownB = New System.Windows.Forms.NumericUpDown
      Me.updownC = New System.Windows.Forms.NumericUpDown
      Me.Label6 = New System.Windows.Forms.Label
      Me.Label7 = New System.Windows.Forms.Label
      Me.Label12 = New System.Windows.Forms.Label
      Me.Label8 = New System.Windows.Forms.Label
      Me.lblResults3 = New System.Windows.Forms.Label
      Me.TabPage4 = New System.Windows.Forms.TabPage
      Me.tbRichtextSrc = New System.Windows.Forms.TextBox
      Me.ComboBox2 = New System.Windows.Forms.ComboBox
      Me.RichTextControl1 = New RichText.RichTextControl
      Me.TabControl1.SuspendLayout()
      Me.TabPage1.SuspendLayout()
      Me.TabPage2.SuspendLayout()
      Me.TabPage3.SuspendLayout()
      CType(Me.updownA, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.updownB, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.updownC, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.TabPage4.SuspendLayout()
      Me.SuspendLayout()
      '
      'tbExpression
      '
      Me.tbExpression.Location = New System.Drawing.Point(8, 32)
      Me.tbExpression.Name = "tbExpression"
      Me.tbExpression.Size = New System.Drawing.Size(328, 20)
      Me.tbExpression.TabIndex = 0
      Me.tbExpression.Text = "4+5"
      '
      'TextBox2
      '
      Me.TextBox2.Location = New System.Drawing.Point(8, 56)
      Me.TextBox2.Multiline = True
      Me.TextBox2.Name = "TextBox2"
      Me.TextBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both
      Me.TextBox2.Size = New System.Drawing.Size(408, 336)
      Me.TextBox2.TabIndex = 0
      Me.TextBox2.Text = ""
      '
      'btnEvaluate
      '
      Me.btnEvaluate.Location = New System.Drawing.Point(344, 32)
      Me.btnEvaluate.Name = "btnEvaluate"
      Me.btnEvaluate.Size = New System.Drawing.Size(72, 23)
      Me.btnEvaluate.TabIndex = 2
      Me.btnEvaluate.Text = "Evaluate"
      '
      'cbSamples
      '
      Me.cbSamples.Items.AddRange(New Object() {"123+345", "(2+3)*(4-2)", "120+5%", "now", "Round(now - Date(2006,1,1))+"" days since new year""", "anumber*5", "arr(1)+arr(2)", "theForm.Controls(0).name", "theForm.Left", "1+2 >= 3", """ab""+""c"" = ""abc""", "IIF(true,""a"",""b"")", "IIF(false,1,2)", "MIN(4,3,1,2)", "MAX(1,2)"})
      Me.cbSamples.Location = New System.Drawing.Point(8, 8)
      Me.cbSamples.Name = "cbSamples"
      Me.cbSamples.Size = New System.Drawing.Size(408, 21)
      Me.cbSamples.TabIndex = 3
      Me.cbSamples.Text = "<enter an expression or select a sample>"
      '
      'TabControl1
      '
      Me.TabControl1.Controls.Add(Me.TabPage1)
      Me.TabControl1.Controls.Add(Me.TabPage2)
      Me.TabControl1.Controls.Add(Me.TabPage3)
      Me.TabControl1.Controls.Add(Me.TabPage4)
      Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
      Me.TabControl1.Location = New System.Drawing.Point(5, 5)
      Me.TabControl1.Name = "TabControl1"
      Me.TabControl1.SelectedIndex = 0
      Me.TabControl1.Size = New System.Drawing.Size(432, 422)
      Me.TabControl1.TabIndex = 5
      '
      'TabPage1
      '
      Me.TabPage1.Controls.Add(Me.tbExpression)
      Me.TabPage1.Controls.Add(Me.cbSamples)
      Me.TabPage1.Controls.Add(Me.btnEvaluate)
      Me.TabPage1.Controls.Add(Me.TextBox2)
      Me.TabPage1.Location = New System.Drawing.Point(4, 22)
      Me.TabPage1.Name = "TabPage1"
      Me.TabPage1.Size = New System.Drawing.Size(424, 396)
      Me.TabPage1.TabIndex = 0
      Me.TabPage1.Text = "Single expression"
      '
      'TabPage2
      '
      Me.TabPage2.Controls.Add(Me.cbAuto)
      Me.TabPage2.Controls.Add(Me.Label1)
      Me.TabPage2.Controls.Add(Me.PictureBox1)
      Me.TabPage2.Controls.Add(Me.tbExpressionRed)
      Me.TabPage2.Controls.Add(Me.ComboBox1)
      Me.TabPage2.Controls.Add(Me.btnEvaluate2)
      Me.TabPage2.Controls.Add(Me.tbExpressionGreen)
      Me.TabPage2.Controls.Add(Me.Label2)
      Me.TabPage2.Controls.Add(Me.Label3)
      Me.TabPage2.Controls.Add(Me.tbExpressionBlue)
      Me.TabPage2.Controls.Add(Me.Label4)
      Me.TabPage2.Location = New System.Drawing.Point(4, 22)
      Me.TabPage2.Name = "TabPage2"
      Me.TabPage2.Size = New System.Drawing.Size(424, 396)
      Me.TabPage2.TabIndex = 1
      Me.TabPage2.Text = "heavier evaluation"
      '
      'cbAuto
      '
      Me.cbAuto.Checked = True
      Me.cbAuto.CheckState = System.Windows.Forms.CheckState.Checked
      Me.cbAuto.Location = New System.Drawing.Point(344, 64)
      Me.cbAuto.Name = "cbAuto"
      Me.cbAuto.Size = New System.Drawing.Size(64, 24)
      Me.cbAuto.TabIndex = 10
      Me.cbAuto.Text = "Auto"
      '
      'Label1
      '
      Me.Label1.Location = New System.Drawing.Point(8, 112)
      Me.Label1.Name = "Label1"
      Me.Label1.Size = New System.Drawing.Size(344, 16)
      Me.Label1.TabIndex = 9
      Me.Label1.Text = "Label1"
      '
      'PictureBox1
      '
      Me.PictureBox1.Location = New System.Drawing.Point(56, 128)
      Me.PictureBox1.Name = "PictureBox1"
      Me.PictureBox1.Size = New System.Drawing.Size(256, 256)
      Me.PictureBox1.TabIndex = 8
      Me.PictureBox1.TabStop = False
      '
      'tbExpressionRed
      '
      Me.tbExpressionRed.Location = New System.Drawing.Point(56, 32)
      Me.tbExpressionRed.Name = "tbExpressionRed"
      Me.tbExpressionRed.Size = New System.Drawing.Size(280, 20)
      Me.tbExpressionRed.TabIndex = 4
      Me.tbExpressionRed.Text = "x*15"
      '
      'ComboBox1
      '
      Me.ComboBox1.Items.AddRange(New Object() {"Sample1", "Sample2", "Sample3", "Sample4"})
      Me.ComboBox1.Location = New System.Drawing.Point(8, 8)
      Me.ComboBox1.Name = "ComboBox1"
      Me.ComboBox1.Size = New System.Drawing.Size(408, 21)
      Me.ComboBox1.TabIndex = 6
      Me.ComboBox1.Text = "<enter an expression or select a sample>"
      '
      'btnEvaluate2
      '
      Me.btnEvaluate2.Location = New System.Drawing.Point(344, 32)
      Me.btnEvaluate2.Name = "btnEvaluate2"
      Me.btnEvaluate2.Size = New System.Drawing.Size(72, 23)
      Me.btnEvaluate2.TabIndex = 5
      Me.btnEvaluate2.Text = "Evaluate"
      '
      'tbExpressionGreen
      '
      Me.tbExpressionGreen.Location = New System.Drawing.Point(56, 56)
      Me.tbExpressionGreen.Name = "tbExpressionGreen"
      Me.tbExpressionGreen.Size = New System.Drawing.Size(280, 20)
      Me.tbExpressionGreen.TabIndex = 4
      Me.tbExpressionGreen.Text = "cos(x*y*4900)"
      '
      'Label2
      '
      Me.Label2.Location = New System.Drawing.Point(8, 35)
      Me.Label2.Name = "Label2"
      Me.Label2.Size = New System.Drawing.Size(40, 16)
      Me.Label2.TabIndex = 9
      Me.Label2.Text = "Red"
      Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      '
      'Label3
      '
      Me.Label3.Location = New System.Drawing.Point(8, 59)
      Me.Label3.Name = "Label3"
      Me.Label3.Size = New System.Drawing.Size(40, 16)
      Me.Label3.TabIndex = 9
      Me.Label3.Text = "Green"
      Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      '
      'tbExpressionBlue
      '
      Me.tbExpressionBlue.Location = New System.Drawing.Point(56, 80)
      Me.tbExpressionBlue.Name = "tbExpressionBlue"
      Me.tbExpressionBlue.Size = New System.Drawing.Size(280, 20)
      Me.tbExpressionBlue.TabIndex = 4
      Me.tbExpressionBlue.Text = "y*15"
      '
      'Label4
      '
      Me.Label4.Location = New System.Drawing.Point(8, 83)
      Me.Label4.Name = "Label4"
      Me.Label4.Size = New System.Drawing.Size(40, 16)
      Me.Label4.TabIndex = 9
      Me.Label4.Text = "Blue"
      Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      '
      'TabPage3
      '
      Me.TabPage3.Controls.Add(Me.btnEvaluate3)
      Me.TabPage3.Controls.Add(Me.LogBox3)
      Me.TabPage3.Controls.Add(Me.Label5)
      Me.TabPage3.Controls.Add(Me.updownA)
      Me.TabPage3.Controls.Add(Me.tbExpression3)
      Me.TabPage3.Controls.Add(Me.updownB)
      Me.TabPage3.Controls.Add(Me.updownC)
      Me.TabPage3.Controls.Add(Me.Label6)
      Me.TabPage3.Controls.Add(Me.Label7)
      Me.TabPage3.Controls.Add(Me.Label12)
      Me.TabPage3.Controls.Add(Me.Label8)
      Me.TabPage3.Controls.Add(Me.lblResults3)
      Me.TabPage3.Location = New System.Drawing.Point(4, 22)
      Me.TabPage3.Name = "TabPage3"
      Me.TabPage3.Size = New System.Drawing.Size(424, 396)
      Me.TabPage3.TabIndex = 2
      Me.TabPage3.Text = "Dynamic Formulas"
      '
      'btnEvaluate3
      '
      Me.btnEvaluate3.Location = New System.Drawing.Point(344, 16)
      Me.btnEvaluate3.Name = "btnEvaluate3"
      Me.btnEvaluate3.Size = New System.Drawing.Size(72, 23)
      Me.btnEvaluate3.TabIndex = 7
      Me.btnEvaluate3.Text = "Evaluate"
      '
      'LogBox3
      '
      Me.LogBox3.Location = New System.Drawing.Point(8, 184)
      Me.LogBox3.Multiline = True
      Me.LogBox3.Name = "LogBox3"
      Me.LogBox3.ScrollBars = System.Windows.Forms.ScrollBars.Both
      Me.LogBox3.Size = New System.Drawing.Size(408, 200)
      Me.LogBox3.TabIndex = 6
      Me.LogBox3.Text = "Notice how the formula is refreshed only when involved variables are modified." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10)
      '
      'Label5
      '
      Me.Label5.Location = New System.Drawing.Point(32, 48)
      Me.Label5.Name = "Label5"
      Me.Label5.Size = New System.Drawing.Size(88, 20)
      Me.Label5.TabIndex = 5
      Me.Label5.Text = "a"
      Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      '
      'updownA
      '
      Me.updownA.Location = New System.Drawing.Point(128, 48)
      Me.updownA.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
      Me.updownA.Name = "updownA"
      Me.updownA.Size = New System.Drawing.Size(72, 20)
      Me.updownA.TabIndex = 1
      Me.updownA.Value = New Decimal(New Integer() {23, 0, 0, 0})
      '
      'tbExpression3
      '
      Me.tbExpression3.Location = New System.Drawing.Point(128, 16)
      Me.tbExpression3.Name = "tbExpression3"
      Me.tbExpression3.Size = New System.Drawing.Size(208, 20)
      Me.tbExpression3.TabIndex = 0
      Me.tbExpression3.Text = "a+2*b"
      '
      'updownB
      '
      Me.updownB.Location = New System.Drawing.Point(128, 80)
      Me.updownB.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
      Me.updownB.Name = "updownB"
      Me.updownB.Size = New System.Drawing.Size(72, 20)
      Me.updownB.TabIndex = 1
      Me.updownB.Value = New Decimal(New Integer() {50, 0, 0, 0})
      '
      'updownC
      '
      Me.updownC.Location = New System.Drawing.Point(128, 112)
      Me.updownC.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
      Me.updownC.Name = "updownC"
      Me.updownC.Size = New System.Drawing.Size(72, 20)
      Me.updownC.TabIndex = 1
      Me.updownC.Value = New Decimal(New Integer() {150, 0, 0, 0})
      '
      'Label6
      '
      Me.Label6.Location = New System.Drawing.Point(32, 80)
      Me.Label6.Name = "Label6"
      Me.Label6.Size = New System.Drawing.Size(88, 20)
      Me.Label6.TabIndex = 5
      Me.Label6.Text = "b"
      Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      '
      'Label7
      '
      Me.Label7.Location = New System.Drawing.Point(32, 112)
      Me.Label7.Name = "Label7"
      Me.Label7.Size = New System.Drawing.Size(88, 20)
      Me.Label7.TabIndex = 5
      Me.Label7.Text = "c"
      Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      '
      'Label12
      '
      Me.Label12.Location = New System.Drawing.Point(32, 16)
      Me.Label12.Name = "Label12"
      Me.Label12.Size = New System.Drawing.Size(88, 20)
      Me.Label12.TabIndex = 5
      Me.Label12.Text = "Formula"
      Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      '
      'Label8
      '
      Me.Label8.Location = New System.Drawing.Point(32, 152)
      Me.Label8.Name = "Label8"
      Me.Label8.Size = New System.Drawing.Size(88, 20)
      Me.Label8.TabIndex = 5
      Me.Label8.Text = "Result"
      Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      '
      'lblResults3
      '
      Me.lblResults3.Location = New System.Drawing.Point(128, 152)
      Me.lblResults3.Name = "lblResults3"
      Me.lblResults3.Size = New System.Drawing.Size(272, 20)
      Me.lblResults3.TabIndex = 5
      Me.lblResults3.Text = "Label5"
      Me.lblResults3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
      '
      'TabPage4
      '
      Me.TabPage4.Controls.Add(Me.tbRichtextSrc)
      Me.TabPage4.Controls.Add(Me.ComboBox2)
      Me.TabPage4.Controls.Add(Me.RichTextControl1)
      Me.TabPage4.Location = New System.Drawing.Point(4, 22)
      Me.TabPage4.Name = "TabPage4"
      Me.TabPage4.Size = New System.Drawing.Size(424, 396)
      Me.TabPage4.TabIndex = 3
      Me.TabPage4.Text = "RichText"
      '
      'tbRichtextSrc
      '
      Me.tbRichtextSrc.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.tbRichtextSrc.Location = New System.Drawing.Point(8, 32)
      Me.tbRichtextSrc.Multiline = True
      Me.tbRichtextSrc.Name = "tbRichtextSrc"
      Me.tbRichtextSrc.ScrollBars = System.Windows.Forms.ScrollBars.Both
      Me.tbRichtextSrc.Size = New System.Drawing.Size(408, 136)
      Me.tbRichtextSrc.TabIndex = 5
      Me.tbRichtextSrc.Text = ""
      '
      'ComboBox2
      '
      Me.ComboBox2.Items.AddRange(New Object() {"Sample 1", "Sample 2", "Sample 3", "Sample 4", "Sample 5"})
      Me.ComboBox2.Location = New System.Drawing.Point(8, 8)
      Me.ComboBox2.Name = "ComboBox2"
      Me.ComboBox2.Size = New System.Drawing.Size(408, 21)
      Me.ComboBox2.TabIndex = 4
      Me.ComboBox2.Text = "<enter an expression or select a sample>"
      '
      'RichTextControl1
      '
      Me.RichTextControl1.Location = New System.Drawing.Point(8, 176)
      Me.RichTextControl1.Name = "RichTextControl1"
      Me.RichTextControl1.Size = New System.Drawing.Size(408, 208)
      Me.RichTextControl1.TabIndex = 0
      Me.RichTextControl1.XML = "<p>The first line</p><p>The second line</p><p>The third line</p>"
      '
      'Form1
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(442, 432)
      Me.Controls.Add(Me.TabControl1)
      Me.DockPadding.All = 5
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
      Me.Name = "Form1"
      Me.Text = "Expression Evaluator 100% Managed VB.net"
      Me.TabControl1.ResumeLayout(False)
      Me.TabPage1.ResumeLayout(False)
      Me.TabPage2.ResumeLayout(False)
      Me.TabPage3.ResumeLayout(False)
      CType(Me.updownA, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.updownB, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.updownC, System.ComponentModel.ISupportInitialize).EndInit()
      Me.TabPage4.ResumeLayout(False)
      Me.ResumeLayout(False)

   End Sub

#End Region

   Public ReadOnly Property [me]() As Form1
      Get
         Return Me
      End Get
   End Property



   Private Sub btnEvaluate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEvaluate.Click

      Dim lCode As opCode = Nothing
      Try
         TextBox2.AppendText(tbExpression.Text & vbCrLf)
         'Dim t As New Timer
         lCode = ev.Parse(tbExpression.Text)
         'TextBox2.AppendText("Parsed in " & t.ms & " ms" & vbCrLf)
      Catch ex As Exception
         TextBox2.AppendText(ex.Message & vbCrLf)
         Return
      End Try
      Try
         Dim t As New Timer
         Dim res As Object = lCode.value
         'TextBox2.AppendText("Run in " & t.ms & " ms" & vbCrLf)
         If res Is Nothing Then
            TextBox2.AppendText("Result=<nothing>" & vbCrLf)
         Else
            TextBox2.AppendText("Result=" & ev.ConvertToString(res) & " (" & res.GetType.Name & ")" & vbCrLf)
         End If
      Catch ex As Exception
         TextBox2.AppendText(ex.Message & vbCrLf)
      End Try
      TextBox2.AppendText(vbCrLf)
   End Sub

   Private Sub cbSamples_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSamples.SelectedIndexChanged
      tbExpression.Text = cbSamples.Text
      btnEvaluate_Click(sender, e)
   End Sub

   Public ReadOnly Property theForm() As Form1
      Get
         Return Me
      End Get
   End Property

   Public X As Double
   Public Y As Double

   Private Sub btnEvaluate2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEvaluate2.Click
      Dim lCodeR As opCode = Nothing
      Dim lCodeG As opCode = Nothing
      Dim lCodeB As opCode = Nothing
      Try
         lCodeR = ev.Parse(tbExpressionRed.Text)
         lCodeG = ev.Parse(tbExpressionGreen.Text)
         lCodeB = ev.Parse(tbExpressionBlue.Text)
      Catch ex As Exception
         Label1.Text = ex.Message
         Return
      End Try
      Try
         Dim t As New Timer

         Dim bm As Bitmap = DirectCast(PictureBox1.Image, Bitmap)
         If bm Is Nothing Then
            bm = New Bitmap(256, 256)
            PictureBox1.Image = bm
         End If
         Dim mult As Double = 2 * Math.PI / 256.0
         Dim r As Double
         Dim g As Double
         Dim b As Double

         For Xi As Integer = 0 To 255
            X = (Xi - 128) * mult
            For Yi As Integer = 0 To 255
               Y = (Yi - 128) * mult
               Try
                  r = DirectCast(lCodeR.value, Double)
                  g = DirectCast(lCodeG.value, Double)
                  b = DirectCast(lCodeB.value, Double)
                  If r < 0 OrElse Double.IsNaN(r) Then r = 0 Else If r > 1 Then r = 1
                  If g < 0 OrElse Double.IsNaN(g) Then g = 0 Else If g > 1 Then g = 1
                  If b < 0 OrElse Double.IsNaN(b) Then b = 0 Else If b > 1 Then b = 1
               Catch ex As System.OverflowException
                  ' ignore (same as previous pixel)
               End Try
               bm.SetPixel(Xi, Yi, Color.FromArgb(CInt(r * 255), CInt(g * 255), CInt(b * 255)))
            Next
            If (Xi And 7) = 7 Then
               PictureBox1.Refresh()
            End If
         Next
         Label1.Text = "196,608 evaluations run in " & t.ms() & " ms"
      Catch ex As Exception
         Label1.Text = ex.Message
      End Try
   End Sub


   Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
      mInitializing = True
      Select Case ComboBox1.SelectedIndex
         Case 1
            tbExpressionRed.Text = "mod(round(4*x-y*2),2)-x"
            tbExpressionGreen.Text = "mod(abs(x+2*y),0.75)*10+y/5"
            tbExpressionBlue.Text = "round(sin(sqrt(x*x+y*y))*3/5)+x/3"
         Case 2
            tbExpressionRed.Text = "1-round(x/y*0.5)"
            tbExpressionGreen.Text = "1-round(y/x*0.4)"
            tbExpressionBlue.Text = "round(sin(sqrt(x*x+y*y)*10))"
         Case 3
            tbExpressionRed.Text = "cos(x/2)/2"
            tbExpressionGreen.Text = "cos(y/2)/3"
            tbExpressionBlue.Text = "round(sin(sqrt(x*x*x+y*y)*10))"
         Case 4
            tbExpressionRed.Text = "x*15"
            tbExpressionGreen.Text = "cos(x*y*4900)"
            tbExpressionBlue.Text = "y*15"
         Case Else
            tbExpressionRed.Text = String.Empty
            tbExpressionGreen.Text = String.Empty
            tbExpressionBlue.Text = String.Empty
      End Select
      btnEvaluate2_Click(sender, e)
   End Sub

   Private Sub tbExpressionBlue_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbExpressionBlue.TextChanged, tbExpressionRed.TextChanged, tbExpressionGreen.TextChanged
      If mInitializing Then Exit Sub
      If cbAuto.Checked Then
         btnEvaluate2_Click(sender, e)
      End If
   End Sub

   Private Sub cbAuto_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbAuto.CheckedChanged
      If mInitializing Then Exit Sub
      If cbAuto.Checked Then btnEvaluate2_Click(sender, e)
   End Sub

   'Public Event ValueChanged(ByVal Sender As Object, ByVal e As System.EventArgs) Implements Eval3.iEvalValue.ValueChanged

   ' Note that these 3 variables are visible from within the evaluator 
   ' without needing any assessor 
   Public A As Eval3.EvalVariable
   Public B As Eval3.EvalVariable
   Public C As Eval3.EvalVariable

   Private Sub updownA_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles updownA.ValueChanged
      A.Value = CDbl(updownA.Value)
   End Sub

   Private Sub updownB_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles updownB.ValueChanged
      B.Value = CDbl(updownB.Value)
   End Sub

   Private Sub updownC_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles updownC.ValueChanged
      C.Value = CDbl(updownC.Value)
   End Sub

   Private Sub mFormula3_ValueChanged(ByVal Sender As Object, ByVal e As System.EventArgs) Handles mFormula3.ValueChanged
      Dim v As String = Evaluator.ConvertToString(mFormula3.value)
      lblResults3.Text = v
      LogBox3.AppendText(Now.ToLongTimeString() & ": " & v & vbCrLf)
   End Sub

   Private Sub btnEvaluate3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEvaluate3.Click
      Try
         If tbExpression3.Text.Length = 0 Then Exit Sub
         mFormula3 = ev.Parse(tbExpression3.Text)
         mFormula3_ValueChanged(sender, e)
      Catch ex As Exception
         lblResults3.Text = ex.Message
      End Try
   End Sub

   Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
      btnEvaluate3_Click(sender, e)
   End Sub

   Public Class class1
      Public field1 As Double = 2.3

      Public Function method2() As Double
         Return 3.4
      End Function

      Public ReadOnly Property prop3() As Integer
         Get
            Return 4.5
         End Get
      End Property

      Public arr As Double() = {3, 1, 4, 1, 5, 9}
   End Class


   Public Class class2
      Implements iVariableBag


      Public Function GetVariable(ByVal varname As String) As Eval3.iEvalValue Implements Eval3.iVariableBag.GetVariable
         Select Case varname
            Case "dyn1"
               Return New Eval3.EvalVariable(1.1, GetType(Double))
         End Select
      End Function
   End Class



   Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      'Dim ev As New Eval3.Evaluator
      'ev.AddEnvironmentFunctions(New class1)
      'MsgBox(ev.Parse("arr(0)").value.ToString)

      'ev.AddEnvironmentFunctions(New class2)
      'Dim code As opCode = ev.Parse("dyn1*field1")
      'MsgBox(code.value & " " & code.value)


      btnEvaluate_Click(Nothing, Nothing)
      btnEvaluate2_Click(Nothing, Nothing)
      btnEvaluate3_Click(Nothing, Nothing)

   End Sub

   Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
      Dim t As String
      Select Case ComboBox2.SelectedIndex
         Case 1
            t = "<p font=""arial,15"">This</p>" & vbCrLf & _
               "<div orientation=""vertical"" > " & vbCrLf & _
               "<p font=""times,23,underline"" color=""blue"">is</p>" & vbCrLf & _
               "<p font="",23"">a</p><p font=""Verdana,13"">versatile</p>" & vbCrLf & _
               "</div>" & vbCrLf & _
               "<p font="",20"">component</p>" & vbCrLf & _
               "<div orientation=""vertical"" > " & vbCrLf & _
               "<p font="",12"">isn't</p>" & vbCrLf & _
               "<p font="",34,italic"" color=""red"">it ?</p>" & vbCrLf & _
               "</div>" & vbCrLf
         Case 2
            t = "<p>hello2</p>"
         Case 3
            t = "<p>hello3</p>"
         Case 4
            t = "<p>hello4</p>"
         Case Else
            t = "<p>hello5</p>"
      End Select
      tbRichtextSrc.Text = t
   End Sub

   Private Sub tbRichtextSrc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbRichtextSrc.TextChanged
      RichTextControl1.XML = tbRichtextSrc.Text

   End Sub

End Class
