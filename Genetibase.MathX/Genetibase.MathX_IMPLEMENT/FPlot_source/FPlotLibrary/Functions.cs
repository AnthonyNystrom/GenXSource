using Microsoft.CSharp;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace FPlotLibrary {

	/// <summary>
	/// The EventHandler for a calculation step.
	/// </summary>
	public delegate void CalcStepEventHandler(CalcThread t);

	/// <summary>
	/// The class that holds the thread that calculates the function values in the background.
	/// </summary>
	public class CalcThread {
		/// <exclude/>
		public const int delay = 500;
		/// <summary>
		/// The width of one pixel in the current plotting area.
		/// </summary>
		public double dx;
		/// <summary>
		/// The height of one pixel in the current plotting area.
		/// </summary>
		public double dy;
		/// <summary>
		/// The z-range to display.
		/// </summary>
		public double dz;
		/// <summary>
		/// The width of the plotting area in pixels.
		/// </summary>
		public int w;
		/// <summary>
		/// The height of the plotting area in pixels.
		/// </summary>
		public int h;
		/// <exclude/>
		public int t, nt, progress, maxprogress;
		/// <summary>
		/// A value that indicates if the calculation thread should abort because of a newer recalculation.
		/// </summary>
		public bool stop;
		/// <summary>
		/// A value that indicates if the calculation thread has finished.
		/// </summary>
		public bool done;
		/// <summary>
		/// The <see cref="GraphControl">GraphControl</see> associated with the <see cref="CalcThread">CalcThread</see>.
		/// </summary>
		public GraphControl Graph;
		/// <summary>
		/// The calculatig <see cref="Thread">Thread</see>.
		/// </summary>
		public Thread thread;
		/// <summary>
		/// The brush used to paint the items of the <see cref="GraphControl">GraphControl</see>.
		/// </summary>
		public SolidBrush brush = new SolidBrush(Color.White);
		/// <summary>
		/// The pen used to paint the items of the <see cref="GraphControl">GraphControl</see>.
		/// </summary>
		public Pen pen = new Pen(Color.Black);
		/// <summary>
		/// An event that is raised every 0.5 seconds during calculation. This event is used by the <see cref="GraphControl">GraphControl</see>
		/// to update a Windows.Forms <see cref="ProgressBar">ProgressBar</see>.
		/// </summary>
		public event CalcStepEventHandler Step;
		private delegate void CalcInvoke();

		/// <summary>
		/// The constructor of the CalcThread.
		/// </summary>
		public CalcThread(GraphControl graph) {
			Graph = graph;
			w = -1; h = -1;
			stop = true;
			done = true;
		}

		/// <summary>
		/// Copies from another CalcThread using a deep copy.
		/// </summary>
		public void CopyFrom(CalcThread t) {
			w = t.w; h = t.h; dx = t.dx; dy = t.dy; dz = t.dz;
			//Step = t.Step;		
		}

		private void DoStep() {
			if (Step != null) Step(this);
		}

		/// <summary>
		/// Calls the Step event delegate, using <c>Graph.Invoke</c>, e.g. in a thread safe way. Note that 
		/// because of the use of <c>Graph.Invoke</c> the Step event is only raised,
		/// if the <see cref="GraphControl">GraphControl</see> has a parent control that is linked to a main form.
		/// </summary>
		public void OnStep() {
			progress++;
			if (System.Environment.TickCount - t > delay) {
				t = System.Environment.TickCount;
				try {
					Graph.Invoke(new CalcInvoke(DoStep), null);
				} catch {}
			}
		}

		/// <summary>
		/// Creates a deep copy of the CalcThread.
		/// </summary>
		public CalcThread Clone(GraphControl graph) {
			CalcThread t = new CalcThread(graph);
			t.CopyFrom(this);
			return t;
		}

		/// <summary>
		/// The main routine of the thread.
		/// </summary>
		public void DoCalc() {
			done = false;
			t = System.Environment.TickCount;
			nt = 0;
			progress = 0;
			for (int i = 0; i < Graph.Model.Items.Count; i++) {
				if (stop) break;
				Graph.Model.Items[i].Calc(this);
			}
			done = true;
			if (!stop && Graph.AsyncDraw) {
				Graph.Model.Invalidate();
			}
		}

		/// <summary>
		/// This routine starts a new calc thread depending on wether the plotting area or the plotted
		/// items have changed.
		/// </summary>
		/// <param name="b">The size of the plotting area.</param>
		public void Start(Rectangle b) {
			bool calcall = ((w != b.Width) || (h != b.Height));
			if (Graph.Model.Modified || calcall && b.Width > 0 && b.Height > 0) {
				stop = true;
				if (thread != null) {
					try {
						thread.Abort();
					} catch{}
					thread.Join(3000);
				}
				if (calcall) Graph.Model.Modified = true;
				w = b.Width; h = b.Height;
				dx = (Graph.Model.x1 - Graph.Model.x0)/w;
				dy = (Graph.Model.y1 - Graph.Model.y0)/h;
				dz = (Graph.Model.z1 - Graph.Model.z0);
				progress = 0;
				maxprogress = 1;
				for (int i = 0; i < Graph.Model.Items.Count; i++) {
					Graph.Model.Items[i].StartCalc(this);
				}
				if (Graph.Bar != null) {
					Graph.Bar.Maximum = maxprogress;
					Graph.Bar.Value = 0;
				}
				stop = false;
				thread = new Thread(new ThreadStart(DoCalc));
				thread.Name = "Calc Thread";
				done = false;
				thread.Start();
				Thread.Sleep(0);
			}
		}
	}

	/// <summary>
	/// Base class of all plottable items.
	/// </summary>
	[Serializable]
	public class PlotItem: IDeserializationCallback {
		/// <exclude/>
		protected const string initsrc = "using System;namespace FPlotLibrary.Code{class Item";
		/// <exclude/>
		protected const string initsrcdraw = "using System;using System.Drawing;namespace FPlotLibrary.Code{class Item";
		/// <exclude/>
		protected const string initsrcdata = "using System;using System.IO;namespace FPlotLibrary.Code{class Item";

		/// <exclude/>
		protected const string tempPath = "FPlotTempDir";

		/// <exclude/>
		protected Color color = Color.Black;
		/// <summary>
		/// The line style used to draw the item.
		/// </summary>
		public System.Drawing.Drawing2D.DashStyle lineStyle = System.Drawing.Drawing2D.DashStyle.Solid;
		/// <summary>
		/// The line width used to draw the item. 
		/// </summary>
		public float lineWidth = 1;
		/// <summary>
		/// The name of the item.
		/// </summary>
		public string name = "";
		/// <summary>
		/// The <see cref="GraphModel">GraphModel</see> the item belongs to.
		/// </summary>
		public GraphModel model;
		/// <exclude/>
		[NonSerialized]
		protected bool recalc = true;
		/// <exclude/>
		[NonSerialized]
		protected bool modified = true;
		/// <exclude/>
		[NonSerialized]
		public PlotItem code;
		/// <summary>
		/// The compilation errors that occured during compilation.
		/// </summary>
		[NonSerialized]
		public string[] errors = new string[1] {"No errors found."};
		/// <summary>
		/// If true compilation was successfull.
		/// </summary>
		[NonSerialized]
		public bool compiled = false;
		/// <exclude/>
		[NonSerialized]
		protected static int dllIndex = 0;
		[NonSerialized]
		private static int ticks = System.Environment.TickCount;
		/// <exclude/>
		[NonSerialized]
		public static string libraryName = null;

		/// <summary>
		/// The color the item is drawn with.
		/// </summary>
		public virtual Color Color {
			get{return color;}
			set{color = value;}
		}
		
		/// <exclude/>
		public virtual bool Recalc {
			get{
				bool b;
				lock(this) {b = recalc;}
				return b;
			}
			set{lock(this) recalc = value;}
		}

		/// <summary>
		/// Set <c>Modified</c> to true if you modified an item. All standart items automatically set <c>Modified</c> if you modify them.
		/// </summary>
		public virtual bool Modified {
			get{
				bool b;
				lock(this) {b = modified;}
				return b;
			}
			set{lock(this) modified = value;}
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public PlotItem() {}

		/// <summary>
		/// Initializes a <c>PlotItem</c> and sets the <c>GraphModel</c> the item belongs to.
		/// </summary>
		public PlotItem(GraphModel m) {
			model = m;
		}

		/// <summary>
		/// Copies from another PlotItem with a deep copy.
		/// </summary>
		public virtual void CopyFrom(PlotItem item) {
			model = item.model;
			name = (string)item.name.Clone();
			color = item.color;
			lineStyle = item.lineStyle;
			lineWidth = item.lineWidth;
			recalc = item.recalc;
			code = item.code;
			errors = (string[])errors.Clone();
		}

		/// <summary>
		/// Returns a deep copy of the <c>PlotItem</c>.
		/// </summary>
		public virtual PlotItem Clone() {
			PlotItem item = new PlotItem();
			item.CopyFrom(this);
			return item;
		}

		private string[] GetImports() {
			int add = 2;
			if (libraryName != null) add = 3;
			int n = 0;
			if (model != null && model.CompilerImports != null) n = model.CompilerImports.Length;
			string[] imp = new string[n+add];
			for (int i=0; i<n; i++) {
				imp[i+add] = model.CompilerImports[i];
			}
			imp[0] = this.GetType().Module.FullyQualifiedName; //import FPlotLibrary.dll
			imp[1] = "System.Drawing.dll";
			if (libraryName != null) imp[2] = libraryName; //import library
			return imp;
		}

		/// <exclude/>
		protected virtual string GetSource() {return initsrc+dllIndex;}

		/// <exclude/>
		protected string DLLName() {
			string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), tempPath);
			return System.IO.Path.Combine(path, "Code" + ticks.ToString("X") + "." + dllIndex + ".dll");	 
		}

		private void DeleteDLLs() {
			if (dllIndex == 0) {
				string path = System.IO.Path.GetDirectoryName(DLLName());
				System.IO.Directory.CreateDirectory(path);
				string[] files = System.IO.Directory.GetFiles(path, "*.*");
				foreach (string f in files) {
					try {
						System.IO.File.Delete(f);
					} catch {}
				}
				dllIndex++;
			}
		}

		/// <summary>
		/// Compiles the sourcecode of the <c>PlotItem</c>.
		/// </summary>
		/// <param name="load">Specifies if the code should also be loaded, or if only to check the 
		/// items source for compilation errors.</param>
		public virtual void Compile(bool load) {
			DeleteDLLs();
	
			ListDictionary opts;
			if (model != null) opts = model.CompilerOptions;
			else {
				opts = new ListDictionary();
				opts.Add("target", "library");
				opts.Add("o", true);
			}

			CompilerError[] err = Compiler.Compile(new string[1]{GetSource()}, new string[1] {"Source"}, DLLName(),
				GetImports(), opts);

			compiled = true;
			if (err.Length > 0) {
				errors = new string[err.Length];
				for (int i = 0; i < err.Length; i++) {
					compiled = compiled && (err[i].ErrorLevel != ErrorLevel.Error) &&
						(err[i].ErrorLevel != ErrorLevel.FatalError);
					if (err[i].ErrorLevel == ErrorLevel.Warning) { errors[i] = "warning at line "; }
					else { errors[i] = "line "; }
					errors[i] += err[i].SourceLine + ": " + err[i].ErrorMessage;
				}
			} else {
				errors = new string[1] {"No errors found."};
			}



			if (compiled && load) {
				Assembly a = Assembly.LoadFrom(DLLName());
				Type t = a.GetType("FPlotLibrary.Code.Item"+dllIndex.ToString(), true, false);
				object o = t.GetConstructor(Type.EmptyTypes).Invoke(null);
				dllIndex++;
				code = (PlotItem)o;
				modified = recalc = true;
			} else if (load) {
				modified = recalc = true;
				code = this;
			}
		}

		/// <summary>
		/// Used internally. Called by the calculation thread. Calculates the items data in the background. 
		/// </summary>
		/// <param name="t"></param>
		public virtual void Calc(CalcThread t) {Recalc = false;}

		/// <summary>
		/// Used internally. Called to initialize the process of calculating the items data.
		/// </summary>
		public virtual void StartCalc(CalcThread t) {Modified = false; Recalc = false;}

		/// <summary>
		/// Draws the <c>PlotItem</c>.
		/// </summary>
		/// <param name="g">A graphics object that is used for drawing.</param>
		/// <param name="t">The <see cref="CalcThread">CalcThread</see> that calculated the items data.</param>
		public virtual void Draw(Graphics g, CalcThread t) {}

		/// <summary>
		/// Automatically compiles the item after it is deserialized.
		/// </summary>
		public virtual void OnDeserialization(object sender) {
			Compile(true);
		}

		/// <summary>
		/// A shortcut for Math.Abs, that can be used within the PlotItems source.
		/// </summary>
		public double abs(double x) { return Math.Abs(x); }
		/// <summary>
		/// A shortcut for Math.Abs, that can be used within the PlotItems source.
		/// </summary>
		public int abs(int x) { return Math.Abs(x); }
		/// <summary>
		/// A shortcut for Math.Acos, that can be used within the PlotItems source.
		/// </summary>
		public double acos(double x) { return Math.Acos(x); }
		/// <summary>
		/// A shortcut for Math.Asin, that can be used within the PlotItems source.
		/// </summary>
		public double asin(double x) { return Math.Asin(x); }
		/// <summary>
		/// A shortcut for Math.Atan, that can be used within the PlotItems source.
		/// </summary>
		public double atan(double x) { return Math.Atan(x); }
		/// <summary>
		/// A shortcut for Math.Ceiling, that can be used within the PlotItems source.
		/// </summary>
		public double ceiling(double x) { return Math.Ceiling(x); }
		/// <summary>
		/// A shortcut for Math.Cos, that can be used within the PlotItems source.
		/// </summary>
		public double cos(double x) { return Math.Cos(x); }
		/// <summary>
		/// A shortcut for Math.Cosh, that can be used within the PlotItems source.
		/// </summary>
		public double cosh(double x) { return Math.Cosh(x); }
		/// <summary>
		/// A shortcut for Math.Exp, that can be used within the PlotItems source.
		/// </summary>
		public double exp(double x) { return Math.Exp(x); }
		/// <summary>
		/// A shortcut for Math.Floor, that can be used within the PlotItems source.
		/// </summary>
		public double floor(double x) { return Math.Floor(x); }
		/// <summary>
		/// A shortcut for Math.Log, that can be used within the PlotItems source.
		/// </summary>
		public double log(double x) { return Math.Log(x); }
		/// <summary>
		/// A shortcut for Math.Log10, that can be used within the PlotItems source.
		/// </summary>
		public double log10(double x) { return Math.Log10(x); }
		/// <summary>
		/// A shortcut for Math.Max, that can be used within the PlotItems source.
		/// </summary>
		public double max(double x, double y) { return Math.Max(x, y); }
		/// <summary>
		/// A shortcut for Math.Min, that can be used within the PlotItems source.
		/// </summary>
		public double min(double x, double y) { return Math.Min(x, y); }
		/// <summary>
		/// A shortcut for Math.Max, that can be used within the PlotItems source.
		/// </summary>
		public int max(int x, int y) { return Math.Max(x, y); }
		/// <summary>
		/// A shortcut for Math.Min, that can be used within the PlotItems source.
		/// </summary>
		public int min(int x, int y) { return Math.Min(x, y); }
		/// <summary>
		/// A shortcut for Math.Pow, that can be used within the PlotItems source.
		/// </summary>
		public double pow(double x, double y) { return Math.Pow(x, y); }
		/// <summary>
		/// A shortcut for Math.Sin, that can be used within the PlotItems source.
		/// </summary>
		public double sin(double x) { return Math.Sin(x); }
		/// <summary>
		/// A shortcut for Math.Sinh, that can be used within the PlotItems source.
		/// </summary>
		public double sinh(double x) { return Math.Sinh(x); }
		/// <summary>
		/// A shortcut for Math.Sqrt, that can be used within the PlotItems source.
		/// </summary>
		public double sqrt(double x) { return Math.Sqrt(x); }
		/// <summary>
		/// A shortcut for Math.Tan, that can be used within the PlotItems source.
		/// </summary>
		public double tan(double x) { return Math.Tan(x); }
		/// <summary>
		/// A shortcut for Math.Tanh, that can be used within the PlotItems source.
		/// </summary>
		public double tanh(double x) { return Math.Tanh(x); }
		/// <summary>
		/// A shortcut for Math.PI, that can be used within the PlotItems source.
		/// </summary>
		public const double pi = Math.PI;
		/// <summary>
		/// A shortcut for Math.E, that can be used within the PlotItems source.
		/// </summary>
		public const double e = Math.E;

	}

	/// <summary>
	/// The base class for function items.
	/// </summary>
	[Serializable]
	public class FunctionItem: PlotItem {
		/// <summary>
		/// The number of function evaluations. Each function evaluation increments this value by one.
		/// </summary>
		[NonSerialized]
		public int neval = 0;
		
		/// <summary>
		/// The C# sourcecode of the function. 
		/// </summary>
		public string source = "return 0";
		/// <summary>
		/// Parameters used by the function.
		/// </summary>
		public SmallData p = new SmallData();
		/// <summary>
		/// The derivatives df/dp of the function and the parameters.
		/// </summary>
		public SmallData dfdp = new SmallData();

		/// <summary>
		/// Indicates if the function has been modified. This value is automatically set if <see cref="p">p</see> or the
		/// sourcecode changes.
		/// </summary>
		public override bool Modified {
			get{
				bool b;
				lock(this) {
					b = modified || p.Modified;
				}
				return b;
			}
			set{
				lock(this) {
					if (value == false) p.Modified = false;
					modified = value;
				}
			}
		}

		/// <summary>
		/// The constructor of a function item
		/// </summary>
		public FunctionItem() {}

		/// <summary>
		/// The constructor of a function item that sets the items model.
		/// </summary>
		public FunctionItem(GraphModel m): base(m) {}
		
		/// <summary>
		/// Copies from another with a deep copy.
		/// </summary>
		/// <param name="item"></param>
		public override void CopyFrom(PlotItem item) {
			base.CopyFrom(item);
			source = (string)((FunctionItem)item).source.Clone();
			p = (SmallData)((FunctionItem)item).p.Clone();
		}

		/// <summary>
		/// Returns a deep copy of this item.
		/// </summary>
		public override PlotItem Clone() {
			FunctionItem item = new FunctionItem();
			item.CopyFrom(this);
			return item;
		}
		
		/// <summary>
		/// Compiles the function item.
		/// </summary>
		/// <param name="load">Specifies if the compiled code should be loaded or if the item should 
		/// only be checked for compiler errors.</param>
		public override void Compile(bool load) {
			base.Compile(load);
			if (compiled) {
				((FunctionItem)code).p = p;
				((FunctionItem)code).dfdp = dfdp;
			}
		}

		/// <summary>
		/// Ensures that either there is no derivative information in the function (no reference to dfdp)
		/// or that the lenghts of the array p and dfdp are the same. 
		/// </summary>
		protected void CheckdfdpLength() {
			if ((dfdp.Length > 0) && (p.Length != dfdp.Length)) {
				MessageBox.Show("The number of parameters (p) must match\n the number of derivatives (dfdp) in the function.");
				dfdp.Length = p.Length;
			}
		}

	}

	/// <summary>
	/// This class represents a ordinary one dimensional function of the form
	/// <code>
	/// double f(double x) {
	///	  ...
	///	}
	/// </code>
	/// </summary>
	[Serializable]
	public class Function1D: FunctionItem {
		/// <summary>
		/// A delegate to the code that evaluates the function.
		/// </summary>
		public delegate double Code(double x);
		/// <summary>
		/// A delegate to the code that evaluates the function.
		/// </summary>
		[NonSerialized]
		public Code f;
		//draw buffer
		/// <summary>
		/// The points of the function within the plotting area. This value is calculated in the background 
		/// by the <see cref="CalcThread">calculating thread</see>. Therefore the
		/// <see cref="Function1D">Function1D</see> object must be locked
		/// before cache is accessed.
		/// </summary>
		[NonSerialized]
		public PointF[] cache;
		/// <summary>
		/// The default constructor.
		/// </summary>
		public Function1D() {}
		/// <summary>
		/// A constructor that sets the items model.
		/// </summary>
		public Function1D(GraphModel m): base(m) {}
		/// <exclude/>
		public virtual double F(double x) {
			if ((f != null) && (code != this)) return f(x);
			else return double.NaN;
		}
		/// <summary>
		/// Returns the source of the function.
		/// </summary>
		protected override string GetSource() {
			return base.GetSource() + ":FPlotLibrary.Function1D{public override double F(double x){neval++;" +
				source + "}}}";
		}
		/// <summary>
		/// Compiles the function and automatically adapts the length of the <c>p</c> and <c>dfdp</c> arrays.
		/// </summary>
		public override void Compile(bool load) {
			base.Compile(true);
			f = new Code(((Function1D)code).F);
			//evaluate number of parameters p and derivatives dfdp
			p.AutoResize = dfdp.AutoResize = true;
			p.Length = dfdp.Length = 0;
			try {
				f(0);
			} catch {}
			p.AutoResize = dfdp.AutoResize = false;
			CheckdfdpLength();				
		}
		/// <summary>
		/// Copies from another function with a deep copy.
		/// </summary>
		public override void CopyFrom(PlotItem item) {
			base.CopyFrom(item);
			f = ((Function1D)item).f;
			cache = (PointF[])((Function1D)item).cache.Clone();
		}
		/// <summary>
		/// Returns a deep copy.
		/// </summary>
		public override PlotItem Clone() {
			Function1D item = new Function1D();
			item.CopyFrom(this);
			return item;
		}
		/// <summary>
		/// Caluclates the items <see cref="cache">cache</see> field to draw the function.
		/// </summary>
		public override void Calc(CalcThread t) {
			const float BIG = 1e20F;
			int X;
			float F;
			if (Recalc) {
				for (X = 0; X < t.w; X++) {
					if (t.stop) break;
					lock(this) {
						try {
							cache[X].X = X;
							F = (float)(t.h - 1 - (f(t.Graph.Model.x0 + X*t.dx) - t.Graph.Model.y0)/t.dy);
							if (float.IsInfinity(F) || float.IsNaN(F) || Math.Abs(F) > BIG) F = 0;
							cache[X].Y = F;
						} catch (ThreadAbortException ex) { throw ex;
						} catch { cache[X].Y = 0; }
					}
					t.OnStep();
				}
				Recalc = t.stop;
			}
		}

		/// <summary>
		/// Initiates the background calculation of the <see cref="cache">cache</see> field.
		/// </summary>
		public override void StartCalc(CalcThread t) {
			if (Modified) {
				Modified = false; Recalc = true;
				lock(this) {
					cache = new PointF[t.w];
					for (int x = 0; x < t.w; x++) {
						cache[x].X = x; cache[x].Y = t.h+10;
					}
				}
				t.maxprogress += t.w;
			}
		}

		/// <summary>
		/// Draws the function.
		/// </summary>
		public override void Draw(Graphics g, CalcThread t) {
			t.pen.Color = color;
			t.pen.DashStyle = lineStyle;
			t.pen.Width = lineWidth;
			lock(this) {
				try {
					g.DrawLines(t.pen, cache);
				} catch {}
			}
		}
		/// <summary>
		/// Returns if the function source calculates the derivatives <c>dfdp</c>.
		/// </summary>
		public bool HasDerivatives() {
			return (code != null) && (code != this) && (p.Length > 0) && (dfdp.Length > 0) &&
				(p.Length == dfdp.Length);
		}

		/// <summary>
		/// Returns if the function is a function that can be fitted to data.
		/// </summary>
		/// <returns></returns>
		public bool Fitable() {
			return (code != null) && (code != this) && (p.Length > 0);
		}
	}

	/// <summary>
	/// This class represents two dimensional functions of the form
	/// <code>
	/// double f(double x, double y) {
	///   ...
	/// }
	/// </code>
	/// </summary>
	[Serializable]
	public class Function2D: FunctionItem {
		/// <summary>
		/// A delegate to the evaluating code.
		/// </summary>
		public delegate double Code(double x, double y);
		/// <summary>
		/// This field indicates that a Red-Green-Blue color palette should be used instead of a monochrome
		/// color.
		/// </summary>
		public bool rgb;
		/// <summary>
		/// The calculated bitmap of the function. This value is calculated by the background thread,
		/// therefore you must lock the Function2D object before accessing <c>cache</c>.
		/// </summary>
		[NonSerialized]
		public Bitmap cache;
		/// <summary>
		/// A delegate to the evaluating code.
		/// </summary>
		[NonSerialized]
		public Code f; 
		/// <summary>
		/// The default constructor.
		/// </summary>
		public Function2D() {}
		/// <summary>
		/// A constructor setting the model of the item.
		/// </summary>
		public Function2D(GraphModel m): base(m) {}
		/// <exclude/>
		public virtual double F(double x, double y) {
			if ((f != null) && (code != this)) return f(x, y);
			else return double.NaN;
		}
		/// <summary>
		/// The color used to draw the function.
		/// </summary>
		public override Color Color {
			get{ return color; }
			set{ color = value; Modified = true; }
		}
		/// <summary>
		/// Returns the C# source of the function.
		/// </summary>
		protected override string GetSource() {
			return base.GetSource() + ":FPlotLibrary.Function2D{public override double F(double x, double y){neval++;" +
				source + "}}}";
		}
		/// <summary>
		/// Copies from another function with a deep copy.
		/// </summary>
		public override void CopyFrom(PlotItem item) {
			base.CopyFrom(item);
			f = ((Function2D)item).f;
			rgb = ((Function2D)item).rgb;
			cache = (Bitmap)((Function2D)item).cache.Clone();
		}
		/// <summary>
		/// Creates a deep copy.
		/// </summary>
		public override PlotItem Clone() {
			Function2D item = new Function2D();
			item.CopyFrom(this);
			return item;
		}
		/// <summary>
		/// Returns a RGB Color from a double value between 0 and 1. 
		/// </summary>
		public Color RGBColor(double z) {
			Color c;
			int F = ((int)(z*765)) % 766;
			if (F < 0) F = 765 + F;
			if (F <= 255) {
				c = Color.FromArgb(0, F, 255-F);
			} else if (F < 511) {
				c = Color.FromArgb(F-255, 510-F, 0);
			} else {
				c = Color.FromArgb(765-F, 0, F-510);
			}
			return c;
		}
		/// <summary>
		/// Returns a transparent color from <see cref="Color">Color</see> and a double value between 0 and 1. 
		/// </summary>
		public Color FColor(double z) {
			int F = ((int)(z*255)) % 256;
			if (F < 0) F = 255 + F;
			return Color.FromArgb(255-F, color);
		}
		/// <summary>
		/// Calculates the cache bitmap to draw the function.
		/// </summary>
		public override void Calc(CalcThread t) {
			int X, Y;
			double fx, dzinv;
			Color c;
			if (Recalc) {
				dzinv = 1/t.dz;
				for (Y = 0; Y < t.h; Y++) {
					if (t.stop) break;
					for (X = 0; X < t.w; X++) {
						if (t.stop) break;
						try {
							fx = (f(t.Graph.Model.x0 + X*t.dx, t.Graph.Model.y0 + Y*t.dy) - t.Graph.Model.z0)*dzinv;
						} catch (ThreadAbortException ex) { throw ex;
						} catch { fx = 0; }
						if (rgb) c = RGBColor(fx);
						else c = FColor(fx);
						lock(this) {
							cache.SetPixel(X, t.h - Y - 1, c);
						}
					}						
					t.OnStep();
				}
				Recalc = t.stop;
			}
		}
		/// <summary>
		/// Initiates the calculation of the function cache bitmap.
		/// </summary>
		public override void StartCalc(CalcThread t) {
			if (Modified) {
				Modified = false; Recalc = true;
				lock(this) {
					cache = new Bitmap(t.w, t.h, PixelFormat.Format32bppArgb);
					Graphics g = Graphics.FromImage(cache);
					g.Clear(Color.White);
				}
				t.maxprogress += t.h;
			}
		}
		/// <summary>
		/// Draws the function from the previously calculated cache bitmap.
		/// </summary>
		public override void Draw(Graphics g, CalcThread t) {
			lock(this) {
				g.DrawImage(cache, 0, 0, t.w, t.h);
			}
		}
		/// <summary>
		/// Compiles the function.
		/// </summary>
		/// <param name="load">Indicates if the compiled code should be loaded or if the source 
		/// should only be checked for errors.</param>
		public override void Compile(bool load) {
			base.Compile(true);
			f = new Code(((Function2D)code).F);
			//evaluate number of parameters p and derivatives dfdp
			p.AutoResize = dfdp.AutoResize = true;
			p.Length = dfdp.Length = 0;
			try {
				f(0, 0);
			} catch {}
			p.AutoResize = dfdp.AutoResize = false;
			CheckdfdpLength();
		}
	}

	/// <summary>
	/// This class represents two dimensional functions that return a Color of the form
	/// <code>
	/// System.Drawing.Color f(double x, double y) {
	///   ...
	/// }
	/// </code>
	/// </summary>
	[Serializable]
	public class FunctionColor: FunctionItem {
		/// <summary>
		/// A delegate to the evaluating code.
		/// </summary>
		public delegate Color Code(double x, double y);
		/// <summary>
		/// A delegate to the evaluating code.
		/// </summary>
		[NonSerialized]
		public Code f;	
		/// <summary>
		/// The calculated bitmap of the function. This value is calculated by the background thread,
		/// therefore you must lock the FunctionColor object before accessing <c>cache</c>.
		/// </summary>
		[NonSerialized]
		public Bitmap cache;
		/// <summary>
		/// The default constructor.
		/// </summary>
		public FunctionColor() {
			source = "return Color.White";
		}
		/// <summary>
		/// A constructor that sets the model of the function.
		/// </summary>
		/// <param name="m"></param>
		public FunctionColor(GraphModel m): base(m) {
			source = "return Color.White";
		}
		/// <exclude/>
		public virtual Color F(double x, double y) {
			if ((f != null) && (code != this)) return f(x, y);
			return Color.White;
		}
		/// <summary>
		/// Returns the C# code for the function.
		/// </summary>
		protected override string GetSource() {
			return initsrcdraw + dllIndex + ":FPlotLibrary.FunctionColor{public override System.Drawing.Color " +
				"F(double x, double y){neval++;" + source + "}}}";
		}
		/// <summary>
		/// Copies from another function with a deep copy.
		/// </summary>
		public override void CopyFrom(PlotItem item) {
			base.CopyFrom(item);
			cache = (Bitmap)((FunctionColor)item).cache.Clone();
		}
		/// <summary>
		/// Creates a deep copy.
		/// </summary>
		public override PlotItem Clone() {
			FunctionColor item = new FunctionColor();
			item.CopyFrom(this);
			return item;
		}
		/// <summary>
		/// Calculates the cache bitmap to draw the function.
		/// </summary>
		public override void Calc(CalcThread t) {
			int X, Y;
			if (Recalc) {
				Color c;
				double dzinv = 1/t.dz;
				for (Y = 0; Y < t.h; Y++) {
					if (t.stop) break;
					for (X = 0; X < t.w; X++) {
						if (t.stop) break;
						try {
							c = f(t.Graph.Model.x0 + X*t.dx, t.Graph.Model.y0 + Y*t.dy);
						} catch (ThreadAbortException ex) { throw ex;
						} catch { c = Color.White; }
						lock(this) {
							cache.SetPixel(X, t.h - Y - 1, c);
						}
					}
					t.OnStep();
				}
				Recalc = t.stop;
			}
		}
		/// <summary>
		/// Initiates the calculation of the function cache bitmap.
		/// </summary>
		public override void StartCalc(CalcThread t) {
			if (Modified) {
				Modified = false; Recalc = true;
				lock(this) {
					cache = new Bitmap(t.w, t.h, PixelFormat.Format32bppArgb);
					Graphics g = Graphics.FromImage(cache);
					g.Clear(Color.White);
				}
				t.maxprogress += t.h;
			}
		}
		/// <summary>
		/// Draws the function from the previously calculated cache bitmap.
		/// </summary>
		public override void Draw(Graphics g, CalcThread t) {
			lock(this) {
				g.DrawImage(cache, 0, 0, t.w, t.h);
			}
		}
		/// <summary>
		/// Compiles the function.
		/// </summary>
		/// <param name="load">Indicates if the compiled code should be loaded or if the source 
		/// should only be checked for errors.</param>
		public override void Compile(bool load) {
			base.Compile(true);
			f = new Code(((FunctionColor)code).F);
			//evaluate number of parameters p and derivatives dfdp
			p.AutoResize = dfdp.AutoResize = true;
			p.Length = dfdp.Length = 0;
			try {
				f(0, 0);
			} catch {}
			p.AutoResize = dfdp.AutoResize = false;
			CheckdfdpLength();
		}

	}
	/// <summary>
	/// This class represents a source library that can be compiled and imported by the other PlotItems.
	/// You can implement library routines in this class, compile them and import them in a function.
	/// </summary>
	[Serializable]
	public class SourceLibrary: PlotItem {
		/// <summary>
		/// The complete source code of the library.
		/// </summary>
		public string source = "";
		/// <summary>
		/// The default constructor.
		/// </summary>
		public SourceLibrary() {}
		/// <summary>
		/// A constructor that sets the model.
		/// </summary>
		public SourceLibrary(GraphModel m): base(m) {}
		/// <summary>
		/// Returns the source of the library.
		/// </summary>
		protected override string GetSource() {
			return source;
		}
		/// <summary>
		/// Compiles the library.
		/// </summary>
		/// <param name="load">Indicates if the compiled code should be loaded or if the source 
		/// should only be checked for errors.</param>
		public override void Compile(bool load) {
			base.Compile(false);
			if (compiled) {
				libraryName = DLLName();
				Assembly a = Assembly.LoadFrom(DLLName());
				dllIndex++;
				for (int i = 0; i < model.Items.Count; i++) {
					if (model.Items[i] != this) model.Items[i].Compile(true);
				}
			}
		}
		/// <summary>
		/// Copies from another library with a deep copy.
		/// </summary>
		public override void CopyFrom(PlotItem item) {
			base.CopyFrom(item);
			source = (string)((SourceLibrary)item).source.Clone();
		}
		/// <summary>
		/// Creates a deep copy.
		/// </summary>
		public override PlotItem Clone() {
			SourceLibrary l = new SourceLibrary();
			l.CopyFrom(this);
			return l;			
		}
	}

}
