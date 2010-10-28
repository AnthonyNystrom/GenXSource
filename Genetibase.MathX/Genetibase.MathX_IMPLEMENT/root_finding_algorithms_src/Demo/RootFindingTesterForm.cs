using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RootFinding;


namespace RootFinding
{
	public partial class RootFindingTesterForm : Form
	{
		private List<Type> m_ClassesToTest=null;
		MathExpressionParser m_Parser=null;

		public RootFindingTesterForm() {
			InitializeComponent();
			m_Parser=new MathExpressionParser(Application.ExecutablePath);
		}

		protected override void OnLoad(EventArgs e) {
			ClassesLoader.Load("RootFinding.dll",typeof(RootFinder),out m_ClassesToTest);
			if(m_ClassesToTest.Contains(typeof(NewtonRootFinder))) m_ClassesToTest.Remove(typeof(NewtonRootFinder));
			base.OnLoad(e);
		}

		private void OnButtonGenerateDistributionClick(object sender,EventArgs e) {
			double xmin,xmax;
			UnaryFunction func=null;
			this.RichTextBoxTestsResult.Clear();
			try {
				string[] szrange=this.textBox2.Text.Split(new char[] { ';' });
				xmin=double.Parse(szrange[0]);
				xmax=double.Parse(szrange[1]);
			} catch{
				this.RichTextBoxTestsResult.AppendText("Range invalide");
				return;
			}
			try {
				// Parse the function
				this.RichTextBoxTestsResult.AppendText("Function compilation ...");
				func=m_Parser.Parse(this.textBox1.Text.Remove(0,5));
				// Test it
				double t=func((xmax-xmin)/2.0);
				this.RichTextBoxTestsResult.AppendText("ok\n");
			} catch(Exception ex) {
				this.RichTextBoxTestsResult.AppendText("error :\n"+ex.Message);
				return;
			}
			try {
				// Draw the chart
				this.RichTextBoxTestsResult.AppendText("Drawing the chart ...");
				RefreshChart(func,xmin,xmax);
				this.RichTextBoxTestsResult.AppendText("ok\n\n");
			} catch(Exception ex) {
				this.RichTextBoxTestsResult.AppendText("error :\n"+ex.Message);
				return;
			}
			foreach(Type type in m_ClassesToTest) {
				RootFinder finder=(RootFinder)Activator.CreateInstance(type,new object[] { func});
				Compute(finder,xmin,xmax);
			}
		}

		private void Compute(RootFinder finder,double xmin, double xmax) {
			try {
				System.Diagnostics.Stopwatch watch=new System.Diagnostics.Stopwatch();
				this.RichTextBoxTestsResult.AppendText(finder.GetType().Name.Replace("RootFinder","'s method\n"));
				finder.Iterations=(int)this.NumericUpDownDistributionSize.Value;
				double accuracy=0.0;
				if(double.TryParse(this.textBox3.Text,out accuracy)) finder.Accuracy=accuracy;
				watch.Reset();
				watch.Start();
				double root=finder.Solve(xmin,xmax,false);
				watch.Stop();
				double duration=(double)watch.ElapsedTicks/(double)System.Diagnostics.Stopwatch.Frequency;
				this.RichTextBoxTestsResult.AppendText("    root is "+root.ToString()+"\n");
				this.RichTextBoxTestsResult.AppendText("    computation time "+duration.ToString("#0.000000")+" s\n\n");
			} catch(Exception ex) {
				this.RichTextBoxTestsResult.AppendText(ex.Message+"\n\n");
			}
		}

		private void RefreshChart(UnaryFunction func,double xmin, double xmax) {
			this.Chart.GraphPane.CurveList.Clear();
			double pas=(xmax-xmin)/100.0;
			ZedGraph.PointPairList points=new ZedGraph.PointPairList();
			for(int i=0; i<100; i++) points.Add(new ZedGraph.PointPair(i*pas,func(i*pas)));
			this.Chart.GraphPane.AddCurve(this.textBox1.Text, points, Color.Red, ZedGraph.SymbolType.None);
			this.Chart.AxisChange();
			this.Chart.Refresh();
		}
	}
}