using System;
using System.Threading;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.MathX.NugenCCalc;
using Genetibase.MathX.Core;

namespace Genetibase.MathX.NugenCCalc.Design.Controls
{
	public class ExpressionView : PropertyView
	{
		private static string CSharpUsingSection = "using System; using Genetibase.MathX.Core; using System.Globalization; using System.Collections;using System.Diagnostics;using System.IO;using System.Reflection;using System.Text;";
		private static string OkBuiltResult = "---------------------- Done ----------------------" + Environment.NewLine + Environment.NewLine + "Build: 1 succeeded, 0 failed, 0 skipped";
		private static int ShowTimeResult = 10000;
		private System.Threading.Timer timer = null;

		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cbCodeLang;
		private System.Windows.Forms.Button btCheckExpression;
		private System.Windows.Forms.Panel panelExpression;
		private System.Windows.Forms.RichTextBox rtbBottom;
		private System.Windows.Forms.RichTextBox rtbBody;
		private System.Windows.Forms.RichTextBox rtbHead;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cbFunctionType;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel resultPanel;
		private System.Windows.Forms.RichTextBox rtbResult;
		private System.Windows.Forms.Label label3;

		public ExpressionView()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label4 = new System.Windows.Forms.Label();
			this.cbCodeLang = new System.Windows.Forms.ComboBox();
			this.btCheckExpression = new System.Windows.Forms.Button();
			this.panelExpression = new System.Windows.Forms.Panel();
			this.rtbBottom = new System.Windows.Forms.RichTextBox();
			this.rtbBody = new System.Windows.Forms.RichTextBox();
			this.rtbHead = new System.Windows.Forms.RichTextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbFunctionType = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.resultPanel = new System.Windows.Forms.Panel();
			this.rtbResult = new System.Windows.Forms.RichTextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.panelExpression.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.resultPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Location = new System.Drawing.Point(8, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 16);
			this.label4.TabIndex = 12;
			this.label4.Text = "Code Language";
			// 
			// cbCodeLang
			// 
			this.cbCodeLang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbCodeLang.DisplayMember = "C#";
			this.cbCodeLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbCodeLang.Items.AddRange(new object[] {
															"C#",
															"VB.NET"});
			this.cbCodeLang.Location = new System.Drawing.Point(120, 56);
			this.cbCodeLang.Name = "cbCodeLang";
			this.cbCodeLang.Size = new System.Drawing.Size(72, 21);
			this.cbCodeLang.TabIndex = 14;
			this.cbCodeLang.SelectedIndexChanged += new System.EventHandler(this.cbCodeLang_SelectedIndexChanged);
			// 
			// btCheckExpression
			// 
			this.btCheckExpression.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCheckExpression.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btCheckExpression.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btCheckExpression.Location = new System.Drawing.Point(400, 360);
			this.btCheckExpression.Name = "btCheckExpression";
			this.btCheckExpression.Size = new System.Drawing.Size(112, 24);
			this.btCheckExpression.TabIndex = 10;
			this.btCheckExpression.Text = "Check expression";
			this.btCheckExpression.Click += new System.EventHandler(this.btCheckExpression_Click);
			// 
			// panelExpression
			// 
			this.panelExpression.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panelExpression.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panelExpression.Controls.Add(this.rtbBottom);
			this.panelExpression.Controls.Add(this.rtbBody);
			this.panelExpression.Controls.Add(this.rtbHead);
			this.panelExpression.Location = new System.Drawing.Point(0, 112);
			this.panelExpression.Name = "panelExpression";
			this.panelExpression.Size = new System.Drawing.Size(512, 240);
			this.panelExpression.TabIndex = 9;
			this.panelExpression.Resize += new System.EventHandler(this.panelExpression_Resize);
			// 
			// rtbBottom
			// 
			this.rtbBottom.BackColor = System.Drawing.SystemColors.ScrollBar;
			this.rtbBottom.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtbBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.rtbBottom.Enabled = false;
			this.rtbBottom.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.rtbBottom.Location = new System.Drawing.Point(0, 216);
			this.rtbBottom.Name = "rtbBottom";
			this.rtbBottom.Size = new System.Drawing.Size(508, 20);
			this.rtbBottom.TabIndex = 2;
			this.rtbBottom.Text = "}";
			// 
			// rtbBody
			// 
			this.rtbBody.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtbBody.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbBody.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.rtbBody.Location = new System.Drawing.Point(0, 32);
			this.rtbBody.Name = "rtbBody";
			this.rtbBody.Size = new System.Drawing.Size(508, 204);
			this.rtbBody.TabIndex = 1;
			this.rtbBody.Text = "";
			this.rtbBody.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rtbBody_MouseDown);
			// 
			// rtbHead
			// 
			this.rtbHead.BackColor = System.Drawing.SystemColors.ScrollBar;
			this.rtbHead.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtbHead.Dock = System.Windows.Forms.DockStyle.Top;
			this.rtbHead.Enabled = false;
			this.rtbHead.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.rtbHead.Location = new System.Drawing.Point(0, 0);
			this.rtbHead.Name = "rtbHead";
			this.rtbHead.Size = new System.Drawing.Size(508, 32);
			this.rtbHead.TabIndex = 0;
			this.rtbHead.Text = "function Calculate (double x) {";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.cbFunctionType);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.cbCodeLang);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(0, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(512, 96);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Expression properties";
			// 
			// cbFunctionType
			// 
			this.cbFunctionType.DisplayMember = "Explicit";
			this.cbFunctionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbFunctionType.Location = new System.Drawing.Point(120, 24);
			this.cbFunctionType.Name = "cbFunctionType";
			this.cbFunctionType.Size = new System.Drawing.Size(168, 21);
			this.cbFunctionType.TabIndex = 10;
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 9;
			this.label2.Text = "Function type";
			// 
			// resultPanel
			// 
			this.resultPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.resultPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.resultPanel.Controls.Add(this.rtbResult);
			this.resultPanel.Controls.Add(this.label3);
			this.resultPanel.Location = new System.Drawing.Point(8, 256);
			this.resultPanel.Name = "resultPanel";
			this.resultPanel.Size = new System.Drawing.Size(504, 96);
			this.resultPanel.TabIndex = 13;
			this.resultPanel.Visible = false;
			// 
			// rtbResult
			// 
			this.rtbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.rtbResult.Location = new System.Drawing.Point(0, 16);
			this.rtbResult.Name = "rtbResult";
			this.rtbResult.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
			this.rtbResult.Size = new System.Drawing.Size(504, 80);
			this.rtbResult.TabIndex = 1;
			this.rtbResult.Text = "";
			this.rtbResult.Leave += new System.EventHandler(this.rtbResult_Leave);
			this.rtbResult.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rtbResult_MouseDown);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.label3.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label3.Location = new System.Drawing.Point(0, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 0;
			this.label3.Text = "Output";
			// 
			// ExpressionView
			// 
			this.Controls.Add(this.resultPanel);
			this.Controls.Add(this.btCheckExpression);
			this.Controls.Add(this.panelExpression);
			this.Controls.Add(this.groupBox1);
			this.Name = "ExpressionView";
			this.Size = new System.Drawing.Size(512, 392);
			this.panelExpression.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.resultPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		public override void SaveData()
		{
			try
			{
				_component.FunctionParameters.Code = String.Format("{0} {1} {2}", this.rtbHead.Text, this.rtbBody.Text, this.rtbBottom.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}


		public NugenCCalcBase Component
		{
			get
			{
				return _component;
			}
			set
			{
				if (_component != value)
				{
					_component = value;
					InitFunctionTypes();
					GetExpressionValue();
					UpdateFunctionView();
				}
			}
		}


		private void GetExpressionValue()
		{	
			if (_component.FunctionParameters == null)
				return;


			this.rtbBody.Text = _component.FunctionParameters.CodeBody;


			if (_component.FunctionParameters is Function2DParameters)
			{
				if (_component.FunctionParameters is Implicit2DParameters)
					this.cbFunctionType.SelectedIndex = 1;

				if (_component.FunctionParameters is Parametric2DParameters)
					this.cbFunctionType.SelectedIndex = 2;

				if (_component.FunctionParameters is Explicit2DParameters)
					this.cbFunctionType.SelectedIndex = 0;
			}
			if (_component.FunctionParameters is Function3DParameters)
			{
				if (_component.FunctionParameters is Implicit3DParameters)
					this.cbFunctionType.SelectedIndex = 1;

				if (_component.FunctionParameters is Parametric3DParameters)
					this.cbFunctionType.SelectedIndex = 2;

				if (_component.FunctionParameters is Explicit3DParameters)
					this.cbFunctionType.SelectedIndex = 0;
				if (_component.FunctionParameters is ParametricSurfaceParameters)
					this.cbFunctionType.SelectedIndex = 3;
			}

			if (_component.FunctionParameters.CodeLanguage == CodeLanguage.CSharp)
				this.cbCodeLang.SelectedIndex = 0;
			else
				this.cbCodeLang.SelectedIndex = 1;
			this.cbFunctionType.SelectedIndexChanged += new System.EventHandler(this.cbFunctionType_SelectedIndexChanged);

		}

		public void InitFunctionTypes()
		{
			this.cbFunctionType.Items.Clear();
			this.cbFunctionType.Items.AddRange(new object[] {
																"Explicit",
																"Implicit",
																"Parametric"});
			if (_component.FunctionParameters is Function3DParameters)
			{
				this.cbFunctionType.Items.Add("Parametric Surface");
			}
		}

		private void cbFunctionType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (_component.FunctionParameters is Function2DParameters)
			{
				switch (this.cbFunctionType.Text)
				{
					case "Explicit":
						_component.FunctionParameters = new Explicit2DParameters();
						break;
					case "Implicit":
						_component.FunctionParameters = new Implicit2DParameters();
						break;
					case "Parametric":
						_component.FunctionParameters = new Parametric2DParameters();
						break;
				}
			}
			if (_component.FunctionParameters is Function3DParameters)
			{
				switch (this.cbFunctionType.Text)
				{
					case "Explicit":
						_component.FunctionParameters = new Explicit3DParameters();
						break;
					case "Implicit":
						_component.FunctionParameters = new Implicit3DParameters();
						break;
					case "Parametric":
						_component.FunctionParameters = new Parametric3DParameters();
						break;
					case "Parametric Surface":
						_component.FunctionParameters = new ParametricSurfaceParameters();
						break;
				}
			}
			_component.FunctionParameters.SourceType = SourceType.CodeExpression;
			if (this.cbCodeLang.Text == "C#")
				_component.FunctionParameters.CodeLanguage = CodeLanguage.CSharp;
			else
				_component.FunctionParameters.CodeLanguage = CodeLanguage.VBNET;

			UpdateFunctionView();
		}

		private void cbCodeLang_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.cbCodeLang.Text == "C#")
				_component.FunctionParameters.CodeLanguage = CodeLanguage.CSharp;
			else
				_component.FunctionParameters.CodeLanguage = CodeLanguage.VBNET;
			UpdateFunctionView();
		}

		private void UpdateFunctionView()
		{
			if (this.cbCodeLang.Text == "C#")
			{
                this.rtbHead.Text = GetFunctionHeader(_component.FunctionParameters, CodeLanguage.CSharp);
                this.rtbBottom.Text = GetFunctionFooter(_component.FunctionParameters, CodeLanguage.CSharp);
			}
			else
			{	
                this.rtbHead.Text = GetFunctionHeader(_component.FunctionParameters, CodeLanguage.VBNET);
                this.rtbBottom.Text = GetFunctionFooter(_component.FunctionParameters, CodeLanguage.VBNET);

			}
		}



		private void ShowResult(string result)
		{
			this.resultPanel.SetBounds(0, this.panelExpression.Location.Y + this.panelExpression.Height /2, this.panelExpression.Width, this.panelExpression.Height /2, System.Windows.Forms.BoundsSpecified.All);
			this.resultPanel.Visible = true;
			this.rtbResult.Text = result;
            timer = new System.Threading.Timer(new TimerCallback(HideResult), null, ShowTimeResult, Timeout.Infinite);
		}

        private delegate void HideResultDelegate(object o);

        private void HideResult(object o)
        {
            if (InvokeRequired)
            {
                Invoke(new HideResultDelegate(HideResult), new Object[] { null});
            }
            else
                this.resultPanel.Visible = false;
        }

		private void btCheckExpression_Click(object sender, System.EventArgs e)
		{
			String function = "";
			switch (this.cbCodeLang.Text)
			{
				case "C#":
					function = String.Format("{5} class Nugencomponent{3} {0}{1}{2}{4}", this.rtbHead.Text, this.rtbBody.Text, this.rtbBottom.Text, "{", "}", CSharpUsingSection);
					break;
				case "VB.NET":
					function = String.Format("Class Nugencomponent \n {0} \n {1} \n {2} \n End Class", this.rtbHead.Text, this.rtbBody.Text, this.rtbBottom.Text);
					break;
			}

			String resultString = "";

			CodeExpressionChecker.CheckCode(_component.FunctionParameters.CodeLanguage, function, ref resultString);
			if (resultString != "")
				ShowResult(resultString);
			else
				ShowResult(OkBuiltResult);
		}



		private void rtbBody_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			HideResult(null);
		}


		private void panelExpression_Resize(object sender, System.EventArgs e)
		{
			this.resultPanel.SetBounds(0, this.panelExpression.Location.Y + this.panelExpression.Height /2, this.panelExpression.Width, this.panelExpression.Height /2, System.Windows.Forms.BoundsSpecified.All);
		}

		private void rtbResult_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			timer.Change(Timeout.Infinite, Timeout.Infinite);
		}

		private void rtbResult_Leave(object sender, System.EventArgs e)
		{
			timer.Change(ShowTimeResult, -1);
		}

        private string GetFunctionHeader(FunctionParameters function, CodeLanguage codeLanguage)
        {
            Type functionType = function.GetType(); ;
            FunctionCodeAttribute[] attributes =
                (FunctionCodeAttribute[])functionType.GetCustomAttributes(
                typeof(FunctionCodeAttribute), false);
            switch (codeLanguage)
            { 
                case CodeLanguage.CSharp:
                    return (attributes.Length > 0) ? attributes[0].SharpHeader : "";
                case CodeLanguage.VBNET:
                    return (attributes.Length > 0) ? attributes[0].VBHeader : "";
            }
            return "";
        }

        private string GetFunctionFooter(FunctionParameters function, CodeLanguage codeLanguage)
        {
            Type functionType = function.GetType(); ;
            FunctionCodeAttribute[] attributes =
                (FunctionCodeAttribute[])functionType.GetCustomAttributes(
                typeof(FunctionCodeAttribute), false);
            switch (codeLanguage)
            {
                case CodeLanguage.CSharp:
                    return (attributes.Length > 0) ? attributes[0].SharpFooter : "";
                case CodeLanguage.VBNET:
                    return (attributes.Length > 0) ? attributes[0].VBFooter : "";
            }
            return "";
        }


	}
}

