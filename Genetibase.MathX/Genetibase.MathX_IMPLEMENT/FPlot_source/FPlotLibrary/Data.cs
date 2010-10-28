using System;
using System.Collections;
using System.Runtime.Serialization;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace FPlotLibrary {

	/// <summary>
	/// Base class of all Data implementations. Data is basically an array of double values.
	/// The array can be set to check indices, or to automatically adapt its size upon access.
	/// </summary>
	[Serializable]
	public class Data {
		/// <exclude/>
		protected int length;
		/// <exclude/>
		protected bool autoResize = false;

		/// <exclude/>
		[NonSerialized]
		protected bool modified = true;
		/// <summary>
		/// This value indicates if the data should be copied by a deep or a shallow copy. The default value is true.
		/// </summary>
		[NonSerialized]
		public bool deepCopy = true;
		/// <summary>
		/// The Length of the array. This value can be set to adjust the length.
		/// </summary>
		public virtual int Length {get{return length;} set{length = value;}}
		/// <summary>
		/// This value indicates if the index should be checked upon access, or if the size of the array
		/// should automatically be adapted upon access. The default value is <c>false</c>.
		/// </summary>
		public virtual bool AutoResize {get{return autoResize;} set{autoResize = value;}}
		/// <summary>
		/// Indicates if the array was modified. This value is automatically set.
		/// </summary>
		public virtual bool Modified {
			get{
				bool b;
				lock(this) b = modified;
				return b;
			}
			set{lock(this) modified = value;}
		} 
		/// <summary>
		/// array indexer.
		/// </summary>
		public virtual double this[int i] {
			get{
				if (i >= length) {
					if (autoResize) length = i+1;
					else throw new System.IndexOutOfRangeException();
				} else if (i < 0) throw new System.IndexOutOfRangeException();
				return double.NaN;
			}
			set{
				if ((i < 0) || (i >= length)) throw new System.IndexOutOfRangeException();
			}
		}
		/// <summary>
		/// Copies from another data array either with a deep or a shallow copy, depending on the 
		/// <see cref="deepCopy">deepCopy</see> value. 
		/// </summary>
		public void CopyFrom(Data d) {
			Modified = d.Modified;
			deepCopy = d.deepCopy;
			if (d.deepCopy) {
				Length = d.Length;
				for (int i = 0; i < length; i++) {
					this[i] = d[i];
				}
			} else {
				length = d.length;
			}
			autoResize = d.autoResize;
		}
		/// <summary>
		/// Returns either a deep or a shallow copy, depending on the 
		/// <see cref="deepCopy">deepCopy</see> value.
		/// </summary>
		public Data Clone() {
			Data d = new Data();
			d.CopyFrom(this);
			return d;
		}
		/// <summary>
		/// Deletes the array and sets its length to zero.
		/// </summary>
		public virtual void Clear() {Length = 0;}
		/// <summary>
		/// Parses the array from a string array of double values.
		/// </summary>
		/// <param name="str">The string array to parse</param>
		/// <param name="resize">If true the size of the data array is automatically adjusted.</param>
		public virtual void Parse(string[] str, bool resize) {
			bool b = autoResize;
			autoResize = resize;
			int oldlen = Length;
			int len = str.Length;
			Clear();
			while ((len > 0) && (str[len-1] == "")) {len--;}
			if (resize) Length = len;
			else Length = oldlen;
			for (int i = 0; i < len; i++) {
				try {
					this[i] = double.Parse(str[i]);
				} catch {
					try {
						this[i] = double.NaN;
					} catch {}
				}
			}
			autoResize = b;
		}
		/// <summary>
		/// Returns a string array containing the double values of the data array.
		/// </summary>
		public virtual string[] ToText() {
			string[] str = new string[Length];
			for (int i = 0; i < Length; i++) {
				str[i] = this[i].ToString();
			}
			return str;
		}
		/// <summary>
		/// Checks the index and throws a System.IndexOutOfRangeException if the index is out of bounds.
		/// </summary>
		protected void CheckIndex(int i) {
			if ((i < 0) || (i >= Length)) throw new System.IndexOutOfRangeException();
		}
	}

	/// <summary>
	/// A class containing a small amount of data.
	/// </summary>
	[Serializable]
	public class SmallData: Data {
		private double[] data = new double[0];

		private void grow(int length, bool resize) {
			if (this.length < length) {
				if (resize) {
					if (data.Length < length) {
						double[] t = new double[length];
						for (int i = 0; i < data.Length; i++) {
							t[i] = data[i];
						}
						for (int i = data.Length; i < length; i++) {
							t[i] = double.NaN;
						}
						data = t;
					}
					this.length = length;
				} else {throw new System.IndexOutOfRangeException();}
			}
		}
		/// <summary>
		/// Copies from another data array either with a deep or shallow copy, depending on the
		/// <c>deepCopy</c>c> value.
		/// </summary>
		public void CopyFrom(SmallData d) {
			base.CopyFrom(d);
			if (!d.deepCopy) data = d.data;
		}
		/// <summary>
		/// Creates either a deep or shallow copy, depending on the <c>deepCopy</c> value.
		/// </summary>
		new public SmallData Clone() {
			SmallData d = new SmallData();
			d.CopyFrom(this);
			return d;
		}
		/// <summary>
		/// Gets or sets the length of the array.
		/// </summary>
		public override int Length {
			get {return length;}
			set {grow(value, true); length = value; Modified = true;}
		}
		/// <summary>
		/// The indexer of the array. if <c>AutoResize</c> is set to true, the length of the array is
		/// automatically adjusted.
		/// </summary>
		public override double this[int i] {
			get {grow(i+1, autoResize); return data[i];}
			set {grow(i+1, autoResize); data[i] = value; Modified = true;}
		}
	}
	/// <summary>
	/// A class containing big amounts of data.
	/// </summary>
	[Serializable]
	public class BigData: Data {
		const int N = 1024;
		ArrayList data = new ArrayList();
		int n;
		[NonSerialized]
		double[] buf;
		[NonSerialized]
		int i = -1;

		private void grow(int length, bool resize) {
			if (this.length < length) {
				if (resize) {
					while (n <= (length-1) / N) {
						double[] d = new double[N];
						for (int i = 0; i < N; i++) {
							d[i] = double.NaN;
						}
						data.Add(d);
						n++;
					}
					this.length = length;
				} else {throw new System.IndexOutOfRangeException();}
			}
		}
		/// <summary>
		/// Copies from another data array, either with a deep or shallow copy, depending on the 
		/// <c>deepCopy</c> field.
		/// </summary>
		public void CopyFrom(BigData d) {
			base.CopyFrom(d);
			if (!d.deepCopy) {
				data = d.data;
				n = d.n;
			}
		}
		/// <summary>
		/// Returns either a deep or shallow copy, depending on the <c>deepCopy</c> field.
		/// </summary>
		new public BigData Clone() {
			BigData d = new BigData();
			d.CopyFrom(this);
			return d;
		}
		/// <summary>
		/// Sets or gets the length of the data array.
		/// </summary>
		public override int Length {
			get {return length;}
			set {
				grow(value, true);
				length = value;
				int m = (length-1)/N + 1;
				if (m < n) data.RemoveRange(m, n-m);
				n = m-1;
				Modified = true;
			}
		}
		/// <summary>
		/// The indexer of the array. If <c>AutoResize</c> is set, the length of the array is automatically
		/// adjusted upon access.
		/// </summary>
		public override double this[int i] {
			get {
				grow(i+1, autoResize);
				CheckIndex(i);
				if (i/N != this.i) {
					this.i = i/N;
					buf = (double[])data[this.i];
				}
				return buf[i%N];
			}
			set {
				grow(i+1, autoResize);
				CheckIndex(i);
				if (i/N != this.i) {
					this.i = i/N;
					buf = (double[])data[this.i];
				}
				buf[i%N] = value;
				Modified = true;
			}
		}
	}
	/// <summary>
	/// Represents a column of data in a matrix with x, y, dx and dz vectors.
	/// The values of the data array can either be drawn from a <see cref="BigData">BigData</see> array or from a formula
	/// specified in the property <see cref="DataColumn.source">source</see>.
	/// </summary>
	[Serializable]
	public class DataColumn: BigData {
		private string sourcefield;
		/// <summary>
		/// The parent DataItem.
		/// </summary>
		public DataItem parent;
		/// <summary>
		/// Links to the other DataColumns in the DataItem.
		/// </summary>
		[NonSerialized]
		public Data x, y, dx, dy;
		/// <summary>
		/// The default constructor.
		/// </summary>
		public DataColumn(): base() {}
		/// <summary>
		/// A constructor setting the parent <see cref="DataItem">DataItem</see>.
		/// </summary>
		public DataColumn(DataItem parent) { this.parent = parent; }
		/// <summary>
		/// Gets or sets the formula used for this DataColumn. The formula must be a regular C# expression that can refer to the variables
		/// <c>n</c>, <c>Length</c>, <c>x[n]</c>, <c>y[n]</c>, <c>dx[n]</c> and <c>dy[n]</c>. 
		/// </summary>
		public virtual string source {
			get{return sourcefield;}
			set{
				if (value != null) { base.Length = 0; }
				sourcefield = value;
			}
		}
		/// <summary>
		/// Gets or sets the length of this DataColumn.
		/// </summary>
		public override int Length {
			get {return parent.Length;}
			set {
				if (source == null) base.Length = value;
				if (parent.Length != value) { parent.Length = value; Modified = true; }
			}
		}
		/// <summary>
		/// The indexer of the DataColumn.
		/// </summary>
		public override double this[int i] {
			get {return base[i];}
			set {
				if ((autoResize) && (i >= length)) Length = i+1;
				base[i] = value;
			}
		}
		/// <summary>
		/// Copies from another DataColumn with either a deep or shallow copy, depending on the value of
		/// the <c>deepCopy</c> field. 
		/// </summary>
		public void CopyFrom(DataColumn r) {
			base.CopyFrom(r);
			if (r.source != null) {	source = (string)r.source.Clone(); }
			x = r.x; y = r.y; dx = r.dx; dy = r.dy;
		}
		/// <summary>
		/// Creates either a deep or a shallow copy, depending on the value of the <c>deepCopy</c> field.
		/// </summary>
		/// <param name="parent">The parent <see cref="DataItem">DataItem</see> of the copy.</param>
		public virtual DataColumn Clone(DataItem parent) {
			DataColumn r = new DataColumn(parent);
			r.CopyFrom(this);
			return r;
		}
		/// <summary>
		/// Parses the double data from a string array.
		/// </summary>
		/// <param name="str">The string array to parse.</param>
		/// <param name="resize">If true the size of the array is automatically adjusted.</param>
		public override void Parse(string[] str, bool resize) {
			if (source == null) base.Parse (str, resize);
		}
	}

	/// <summary>
	/// A <see cref="PlotItem">PlotItem</see> class that implements data with the four columns x, y, dx and dy, representing
	/// x and y data and the corresponding errors.
	/// </summary>
	[Serializable]
	public class DataItem: PlotItem, ISerializable {
		private int length;
		/// <summary>
		/// A C# source string that specfies source code used to load the DataItem from a file. The source
		/// is of the following form:
		/// <code>
		/// void Load(Stream stream) {
		///   "loadsource";
		/// }
		/// </code>
		/// An example code to load ASCII data would be:
		/// <code>
		/// StreamReader r = new StreamReader(stream);
		/// int n = 0;
		/// string line;
		/// while ((line = r.ReadLine()) != null) {
		///   try {
		///    y[n] = double.Parse(line);
		///  } catch {y[n] = double.NaN;}
		///  n++;
		/// }
		/// r.Close();
		/// </code>
		/// The code can assing the x, y, dx and dy arrays and their size will be adjusted automatically.
		/// </summary>
		public string loadsource;
		/// <summary>
		/// The DataColumns x, y, dx and dy.
		/// </summary>
		public DataColumn x, y, dx, dy;
		/// <summary>
		/// If true, points are joined by a line.
		/// </summary>
		public bool lines;
		/// <summary>
		/// If true, for each point a error mark is drawn.
		/// </summary>
		public bool marks = true;

		// The image to draw
		private Bitmap cache;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public DataItem() {
			x = new DataColumn(this);
			y = new DataColumn(this);
			dx = new DataColumn(this);
			dy = new DataColumn(this);
			Length = 0;
		}

		private void UpdateLinks() {
			x.x = x; x.y = y; x.dx = dx; x.dy = dy;
			y.x = x; y.y = y; y.dx = dx; y.dy = dy;
			dx.x = x; dx.y = y; dx.dx = dx; dx.dy = dy;
			dy.x = x; dy.y = y; dy.dx = dx; dy.dy = dy;
		}

		private DataItem(SerializationInfo info, StreamingContext context): base() {
			color = (Color)info.GetValue("color", typeof(Color));
			name = info.GetString("name");
			model = (GraphModel)info.GetValue("model", typeof(GraphModel));
			length = info.GetInt32("length");
			loadsource = info.GetString("loadsource");
			lines = info.GetBoolean("lines");
			marks = info.GetBoolean("marks");
			x = (DataColumn)info.GetValue("x", typeof(DataColumn));
			y = (DataColumn)info.GetValue("y", typeof(DataColumn));
			dx = (DataColumn)info.GetValue("dx", typeof(DataColumn));
			dy = (DataColumn)info.GetValue("dy", typeof(DataColumn));
			UpdateLinks();
			x.deepCopy = y.deepCopy = dx.deepCopy = dy.deepCopy = true;
		}
		
		/// <summary>
		/// The Serialization routine of the class.
		/// </summary>
		public void GetObjectData(SerializationInfo info, StreamingContext context) {
			info.AddValue("color", color);
			info.AddValue("name", name);
			info.AddValue("model", model);
			info.AddValue("length", length);
			info.AddValue("loadsource", loadsource);
			info.AddValue("lines", lines);
			info.AddValue("marks", marks);
			bool xdeep = x.deepCopy, ydeep = y.deepCopy, dxdeep = dx.deepCopy, dydeep = dy.deepCopy;
			x.deepCopy = false; y.deepCopy = false; dx.deepCopy = false; dy.deepCopy = false;
			DataColumn rx = new DataColumn(this), ry = new DataColumn(this),
				rdx = new DataColumn(this), rdy = new DataColumn(this);
			rx.CopyFrom(x); ry.CopyFrom(y); rdx.CopyFrom(dx); rdy.CopyFrom(dy);
			info.AddValue("x", rx);
			info.AddValue("y", ry);
			info.AddValue("dx", rdx);
			info.AddValue("dy", rdy);
			x.deepCopy = xdeep; y.deepCopy = ydeep; dx.deepCopy = dxdeep; dy.deepCopy = dydeep;
		}

		/// <summary>
		/// Sets or gets the length of the data.
		/// </summary>
		public int Length {
			get {return length;}
			set {
				length = value;
				x.Length = value;
				y.Length = value;
				dx.Length = value;
				dy.Length = value;
			}
		}

		/// <summary>
		/// Indicates if the DataItem was modified. This value is automatically set, if data is changed.
		/// </summary>
		public override bool Modified {
			get {
				bool b;
				lock(this) {
					modified = modified || x.Modified || y.Modified || dx.Modified || dy.Modified;
					b = modified;
				}
				return b;
			}
			set {
				lock(this) {
					if (value == false) {
						x.Modified = y.Modified = dx.Modified = dy.Modified = false;
					}
					modified = value;
				}
			}
		}

		/// <summary>
		/// Returns the sourcecode of the loadsource and the formulas of the DataColumns.
		/// </summary>
		protected override string GetSource() {
			string src = initsrcdata + dllIndex + ":FPlotLibrary.DataItem{";
			if (x.source != null) {
				src += "class DataColumnX:DataColumn{public override double this[int n]{get{CheckIndex(n);try{return " +
					x.source + ";}catch{return double.NaN;}}set{throw new InvalidOperationException();}}" +
					"public override DataColumn Clone(DataItem parent){DataColumnX row = new DataColumnX();" +
					"row.parent = parent; return row;}}";
			}
			if (y.source != null) {
				src += "class DataColumnY:DataColumn{public override double this[int n]{get{CheckIndex(n);try{return " +
					y.source + ";}catch{return double.NaN;}}set{throw new InvalidOperationException();}}" +
					"public override DataColumn Clone(DataItem parent){DataColumnY row = new DataColumnY();" +
					"row.parent = parent; return row;}}";
			}
			if (dx.source != null) {
				src += "class DataColumnDX:DataColumn{public override double this[int n]{get{CheckIndex(n);try{return " +
					dx.source + ";}catch{return double.NaN;}}set{throw new InvalidOperationException();}}" +
					"public override DataColumn Clone(DataItem parent){DataColumnDX row = new DataColumnDX();" +
					"row.parent = parent; return row;}}";

			}
			if (dy.source != null) {
				src += "class DataColumnDY:DataColumn{public override double this[int n]{get{CheckIndex(n);try{return " +
					dy.source + ";}catch{return double.NaN;}}set{throw new InvalidOperationException();}}" +
					"public override DataColumn Clone(DataItem parent){DataColumnDY row = new DataColumnDY();" +
					"row.parent = parent; return row;}}";
			}
			src += "public Item" + dllIndex + "():base(){";
			if (x.source != null) src += "x=new DataColumnX();";
			if (y.source != null) src += "y=new DataColumnY();";
			if (dx.source != null) src += "dx=new DataColumnDX();";
			if (dy.source != null) src += "dy=new DataColumnDY();";
			src += "}public override void Load(Stream stream){" + loadsource + ";}}}";
			return src;
		}
		/// <summary>
		/// Copies from another DataItem.
		/// </summary>
		public override void CopyFrom(PlotItem item) {
			base.CopyFrom(item);
			DataItem data = (DataItem)item;
			Length = data.Length;
			x = data.x.Clone(this); y = data.y.Clone(this);
			dx = data.dx.Clone(this); dy = data.dy.Clone(this);
			UpdateLinks();
			lines = data.lines;
			marks = data.marks;
		}
		/// <summary>
		/// Creates a copy of the DataItem.
		/// </summary>
		public override PlotItem Clone() {
			DataItem item = new DataItem();
			item.CopyFrom(this);
			return item;
		}
		/// <summary>
		/// Compiles the DataItem (the formulas for the DataColumns and the loadsource).
		/// </summary>
		/// <param name="load">Indicates if to load the code after compilation or to just evalueate
		/// the compilation errors.</param>
		public override void Compile(bool load) {
			base.Compile (load);
			if (compiled) {
				DataItem c = (DataItem)code;
				if (c != null) {
					bool t;
					t = x.deepCopy;
					x.deepCopy = false;
					c.x.CopyFrom(x);
					c.x.deepCopy = x.deepCopy = t;
					t = y.deepCopy;
					y.deepCopy = false;
					c.y.CopyFrom(y);
					c.y.deepCopy = y.deepCopy = t;
					t = dx.deepCopy;
					dx.deepCopy = false;
					c.dx.CopyFrom(dx);
					c.dx.deepCopy = dx.deepCopy = t;
					t = dy.deepCopy;
					dy.deepCopy = false;
					c.dy.CopyFrom(dy);
					c.dy.deepCopy = dy.deepCopy = t;
					x = c.x; y = c.y; dx = c.dx; dy = c.dy;
					x.parent = this; y.parent = this; dx.parent = this; dy.parent = this;
				}
			}
			UpdateLinks();
		}

		/// <summary>
		/// Parses a DataItem from four string arrays.
		/// </summary>
		/// <param name="sx">The string array to parse the x values from.</param>
		/// <param name="sy">The string array to parse the y values from.</param>
		/// <param name="sdx">The string array to parse the dx values from.</param>
		/// <param name="sdy">The string array to parse the dy values from.</param>
		public void Parse(string[] sx, string[] sy, string[] sdx, string[] sdy) {
			int lx, ly, ldx, ldy;
			lx = sx.Length;
			while ((lx > 0) && (sx[lx-1] == "")) {lx--;}
			ly = sy.Length;
			while ((ly > 0) && (sy[ly-1] == "")) {ly--;}
			ldx = sdx.Length;
			while ((ldx > 0) && (sdx[ldx-1] == "")) {ldx--;}
			ldy = sdy.Length;
			while ((ldy > 0) && (sdy[ldy-1] == "")) {ldy--;}
			Length = Math.Max(Math.Max(Math.Max(lx, ly), ldx), ldy);
			x.Parse(sx, false);
			y.Parse(sy, false);
			dx.Parse(sdx, false);
			dy.Parse(sdy, false);
		}
		/// <summary>
		/// This routine loads the DataItem acording to the code in <see cref="loadsource">loadsource</see> from a Stream. 
		/// </summary>
		/// <param name="stream"></param>
		public virtual void Load(Stream stream) {
			if ((code != null) && (code != this)) {
				((DataItem)code).Load(stream);
			}
		}
		
		/// <summary>
		/// Draws the DataItem from the previously calculated cache.
		/// </summary>
		/// <param name="g">The Graphics object to draw to.</param>
		/// <param name="t">The calculating thread.</param>
		public override void Draw(Graphics g, CalcThread t) {
			lock (this) {
				if (cache != null) {
					g.DrawImage(cache, 0, 0, t.w, t.h);
				}
			}
		}

		/// <summary>
		/// Calculates the image-cache for the data in the background.
		/// </summary>
		/// <param name="t">The calculating thread.</param>
		public override void Calc(CalcThread t) {
			t.pen.Color = color;
			PointF[] points = new PointF[Length];
			Graphics g;
			lock (this) {
				g = Graphics.FromImage(cache);
			}
			float X, Y, DX, DY;
			for (int i = 0; i < Length; i++) {
				if (t.stop) break;
				try {
					X = (float)((x[i] - t.Graph.Model.x0)/t.dx);
					Y = t.h - 1 - (float)((y[i] - t.Graph.Model.y0)/t.dy);
					DX = (float)(dx[i]/t.dx)/2;
					DY = (float)(dy[i]/t.dy)/2;
					points[i].X = X; points[i].Y = Y;
					if (marks) {
						lock (this) {
							t.pen.DashStyle = DashStyle.Solid;
							t.pen.Width = lineWidth;
							if (DX < 3) DX = 5;
							else {
								g.DrawLine(t.pen, X-DX, Y-3, X-DX, Y+3);
								g.DrawLine(t.pen, X+DX, Y-3, X+DX, Y+3);
							}
							if (DY < 3) DY = 5;
							else {
								g.DrawLine(t.pen, X-3, Y-DY, X+3, Y-DY);
								g.DrawLine(t.pen, X-3, Y+DY, X+3, Y+DY);
							}
							g.DrawLine(t.pen, X-DX, Y, X+DX, Y);
							g.DrawLine(t.pen, X, Y-DY, X, Y+DY);
						}
					}
				} catch (System.Threading.ThreadAbortException ex) {
					throw ex;
				} catch {}
				t.OnStep();
			}
			if (lines && !t.stop) {
				t.pen.DashStyle = lineStyle;
				t.pen.Width = lineWidth;
				try {
					lock (this) {
						g.DrawLines(t.pen, points);
					}
				} catch (System.Threading.ThreadAbortException ex) {
					throw ex;
				} catch {}
			}
			Recalc = t.stop;
		}

		/// <summary>
		/// Initiates calculation of the DataItem.
		/// </summary>
		public override void StartCalc(CalcThread t) {
			if (Modified) {
				Modified = false; Recalc = true;
				lock(this) {
					cache = new Bitmap(t.w, t.h, PixelFormat.Format32bppArgb);
					Graphics g = Graphics.FromImage(cache);
					g.Clear(Color.FromArgb(0, Color.White));
				}
				t.maxprogress += Length/1000;
			}
		}

		/// <summary>
		/// This routine loads the DataItem acording to the code in <see cref="loadsource">loadsource</see> from a file.
		/// </summary>
		/// <param name="file">The filename of the file to load from.</param>
		public void LoadFromFile(string file) {
			Length = 0;
			x.AutoResize = y.AutoResize = dx.AutoResize = dy.AutoResize = true;
			using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read)) {
				try {
					Load(stream);
				} catch {
					MessageBox.Show("Error loading from file\n"+file);
				}
			}
			Length = Math.Max(Math.Max(Math.Max(x.Length, y.Length), dx.Length), dy.Length);
			x.deepCopy = y.deepCopy = dx.deepCopy = dy.deepCopy = true;
			x.AutoResize = y.AutoResize = dx.AutoResize = dy.AutoResize = false;
		}
	}

}