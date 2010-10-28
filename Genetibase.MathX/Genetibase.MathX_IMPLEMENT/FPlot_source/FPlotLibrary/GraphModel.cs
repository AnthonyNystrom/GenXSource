using System;
using System.IO;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
using ICSharpCode.SharpZipLib.GZip;

namespace FPlotLibrary {

	/// <summary>
	/// A class containing an array of PlotItems.
	/// </summary>
	[Serializable]
	public class ItemList {
		GraphModel model;
		ArrayList list = new ArrayList();
		/// <summary>
		/// Constructor setting the parent model for the ItemList.
		/// </summary>
		/// <param name="model"></param>
		public ItemList(GraphModel model) {
			this.model = model;
		}
		/// <summary>
		/// Adds a item to the list.
		/// </summary>
		/// <param name="item">The item to be inserted.</param>
		/// <returns>The index of the inserted item.</returns>
		public virtual int Add(PlotItem item) {
			item.model = model;
			return list.Add(item);
		}
		/// <summary>
		/// Inserts a item into the list at a given index.
		/// </summary>
		/// <param name="index">The index where to insert the item.</param>
		/// <param name="item">The item to insert.</param>
		public virtual void Insert(int index, PlotItem item) {
			item.model = model;
			list.Insert(index, item);
		}
		/// <summary>
		/// Returns the index of a given item.
		/// </summary>
		/// <param name="item">The item to return the index of.</param>
		/// <returns>The index of the item.</returns>
		public virtual int IndexOf(PlotItem item) {
			return list.IndexOf(item);
		}
		/// <summary>
		/// Removes a item from the list.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		public virtual void Remove(PlotItem item) {
			list.Remove(item);
		}
		/// <summary>
		/// The number of items in the list.
		/// </summary>
		public int Count {
			get {return list.Count;}
		}
		/// <summary>
		/// The indexer of the list.
		/// </summary>
		public PlotItem this [int i] {
			get{ return (PlotItem)list[i]; }
			set{ value.model = model; list[i] = value; }
		}
		/// <summary>
		/// Returns a deep copy.
		/// </summary>
		/// <param name="model">The model the copy belongs to.</param>
		public ItemList Clone(GraphModel model) {
			ItemList l = new ItemList(model);
			for (int i = 0; i < Count; i++) {
				l.Add(this[i].Clone());
			}
			return l;
		}
	}

	/// <summary>
	/// The Model of a <see cref="GraphControl">GraphControl</see>. The Model contains all data such as
	/// functions, plotting area, scale-style, etc.
	/// </summary>
	[Serializable]
	public class GraphModel {
		/// <summary>
		/// A handler delegate that handles the Invalidate event.
		/// </summary>
		public delegate void InvalidateEventHandler(GraphModel model);
		/// <summary>
		/// The style with which to display numbers.
		/// </summary>
		public enum NumberStyle {
			/// <summary>
			/// Uses either fixedpoint or scientific notation depending on the number.
			/// </summary>
			Normal,
			/// <summary>
			/// Use always fixedpoint notation.
			/// </summary>
			Fixedpoint,
			/// <summary>
			/// Use always scientific notation.
			/// </summary>
			Scientific};

		[NonSerialized]
		private static double ln10 = Math.Log(10);
		/// <summary>
		/// The event that can be raised, when the model data is changed. This event is not raised
		/// automatically.
		/// </summary>
		[field: NonSerialized]
		public event InvalidateEventHandler Invalidated;
		/// <summary>
		/// The filename where to store the model.
		/// </summary>
		[NonSerialized]
		public string Filename;
		/// <summary>
		/// The width of the main raster spaces.
		/// </summary>
		public double rx, ry, rz;
		private double X0, Y0, X1, Y1, Z0, Z1;
		/// <summary>
		/// Indicates wether to draw a x axis.
		/// </summary>
		public bool xAxis;
		/// <summary>
		/// Indicates wether to draw a y axis.
		/// </summary>
		public bool yAxis;
		/// <summary>
		/// Indicates wether to draw a x raster.
		/// </summary>
		public bool xRaster;
		/// <summary>
		/// Indicates wether to draw a y raster.
		/// </summary>
		public bool yRaster;
		/// <summary>
		/// Indicates wether to draw x grid-lines.
		/// </summary>
		public bool xGrid;
		/// <summary>
		/// Indicates wether to draw y grid-lines.
		/// </summary>
		public bool yGrid;
		/// <summary>
		/// Indicates wether a x-scale should be drawn.
		/// </summary>
		public bool xScale;
		/// <summary>
		/// Indicates wether a y-scale should be drawn.
		/// </summary>
		public bool yScale;
		/// <summary>
		/// Indicates wether a z-scale should be drawn.
		/// </summary>
		public bool zScale;
		/// <summary>
		/// Indicates wether the plotting area should be drawn inside a box.
		/// </summary>
		public bool Border;
		/// <summary>
		/// Indicates if the y-scale should be fixed to the x-scale.
		/// </summary>
		public bool FixYtoX;
		/// <summary>
		/// Indicates if a legend should be drawn.
		/// </summary>
		public bool Legend;
		/// <summary>
		/// Indicates if a border should be drawn around the legend.
		/// </summary>
		public bool LegendBorder;
		/// <summary>
		/// Indicates how many digits should be displayed in the scale
		/// </summary>
		public int xDigits, yDigits, zDigits;
		/// <summary>
		/// Indicates the style with which the numbers in the scale should be displayed.
		/// </summary>
		public NumberStyle xNumberStyle, yNumberStyle, zNumberStyle;
		/// <summary>
		/// The font to use for the scales.
		/// </summary>
		public Font ScaleFont;
		/// <summary>
		/// The color to use for all scales.
		/// </summary>
		public Color ScaleColor;
		/// <summary>
		/// The line-witdth to draw the scale with.
		/// </summary>
		public float ScaleLineWidth = 1;
		/// <summary>
		/// The items to plot in the plotting area.
		/// </summary>
		public ItemList Items;
		/// <summary>
		/// The library code to use.
		/// </summary>
		public SourceLibrary Library;
		/// <summary>
		/// The Assemblies to import for compilations.
		/// </summary>
		public string[] CompilerImports;
		/// <summary>
		/// The compiler options to use.
		/// </summary>
		public ListDictionary CompilerOptions;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public GraphModel() {
			X0 = Y0 = Z0 = -1; X1 = Y1 = Z1 = rx = ry = rz = 1;
			xScale = yScale = zScale = xRaster = yRaster = Border = LegendBorder = true;
			xGrid = yGrid = xAxis = yAxis = Legend = FixYtoX = false; 
			xDigits = yDigits = zDigits = 3;
			xNumberStyle = yNumberStyle = zNumberStyle = NumberStyle.Normal;
			ScaleFont = new System.Drawing.Font("Arial", 8);
			ScaleColor = Color.Black;
			Items = new ItemList(this);
			Library = new SourceLibrary(this);
			CompilerImports = new string[0];
			CompilerOptions = new ListDictionary();
			CompilerOptions.Add("target", "library");
			CompilerOptions.Add("o", true);
			ResetRaster();
			this.Filename = "Noname.fplot";
		}
		
		/// <summary>
		/// Copies from another model with a deep copy.
		/// </summary>
		/// <param name="m">The model to copy from.</param>
		public void CopyFrom(GraphModel m)
		{
			X0 = m.X0;	X1 = m.X1; Y0 = m.Y0; Y1 = m.Y1; Z0 = m.Z0; Z1 = m.Z1;
			rx = m.rx; ry = m.ry; rz = m.rz;

			xAxis = m.xAxis; yAxis = m.yAxis; xRaster = m.xRaster; yRaster = m.yRaster;
			xGrid = m.xGrid; yGrid = m.yGrid;
			xScale = m.xScale; yScale = m.yScale; zScale = m.zScale;
			Border = m.Border; FixYtoX = m.FixYtoX; Legend = m.Legend; LegendBorder = m.LegendBorder;

			xDigits = m.xDigits; yDigits = m.yDigits; zDigits = m.zDigits;
			xNumberStyle = m.xNumberStyle; yNumberStyle = m.yNumberStyle; zNumberStyle = m.zNumberStyle;
			ScaleFont = m.ScaleFont;
			ScaleColor = m.ScaleColor;
			Filename = (string)m.Filename.Clone();

			Items = m.Items.Clone(this);
			Library = (SourceLibrary)m.Library.Clone();
			CompilerImports = (string[])m.CompilerImports.Clone();

			//Deep Copy of CompilerOptions
			CompilerOptions.Clear();
			IEnumerator keys, values;
			keys = m.CompilerOptions.Keys.GetEnumerator();
			values = m.CompilerOptions.Values.GetEnumerator();
			keys.Reset(); values.Reset();
			for (int i = 0; i < m.CompilerOptions.Count; i++) {
				keys.MoveNext(); values.MoveNext();
				CompilerOptions.Add(keys.Current, values.Current);
			}
			Modified = true;
		}
		
		/// <summary>
		/// Creates a deep copy.
		/// </summary>
		public object Clone() {
			GraphModel m = new GraphModel();
			m.CopyFrom(this);
			return m;
		} 
		
		/// <summary>
		/// Get or sets the left border of the plotting area.
		/// </summary>
		public double x0 {
			get{lock(this) return X0;}
			set{lock(this) {X0 = value; Modified = true;}}
		}

		/// <summary>
		/// Get or sets the right border of the plotting area.
		/// </summary>
		public double x1 {
			get{lock(this) return X1;}
			set{lock(this) {X1 = value; Modified = true;}}
		}

		/// <summary>
		/// Get or sets the lower border of the plotting area.
		/// </summary>
		public double y0 {
			get{lock(this) return Y0;}
			set{lock(this) {Y0 = value; Modified = true;}}
		}

		/// <summary>
		/// Get or sets the upper border of the plotting area.
		/// </summary>
		public double y1 {
			get{lock(this) return Y1;}
			set{lock(this) {Y1 = value; Modified = true;}}
		}

		/// <summary>
		/// Get or sets the lower z-border of the plotting area.
		/// </summary>
		public double z0 {
			get{lock(this) return Z0;}
			set{
				lock(this) {
					Z0 = value; 
					for (int i = 0; i < Items.Count; i++) {
						Items[i].Modified = Items[i].Modified || Items[i] is Function2D;	
					}
				}
			}
		}

		/// <summary>
		/// Get or sets the upper z-border of the plotting area.
		/// </summary>
		public double z1 {
			get{lock(this) return Z1;}
			set{
				lock(this) {
					Z1 = value;
					for (int i = 0; i < Items.Count; i++) {
						Items[i].Modified = Items[i].Modified || Items[i] is Function2D;	
					}
				}
			}
		}

		/// <summary>
		/// Indicates if the model was modified.
		/// </summary>
		public bool Modified {
			get{
				bool b = false;	
				lock(this) {
					for(int i = 0; i < Items.Count; i++) {
						b = b || Items[i].Modified;
					}
				}
				return b;
			}
			set {
				lock(this) {
					for (int i = 0; i < Items.Count; i++) {
						Items[i].Modified = value;
					}
				}
			}
		}

		/// <summary>
		/// Loads a Model from a file.
		/// </summary>
		/// <param name="filename">The file to load from.</param>
		public static GraphModel LoadFromFile(string filename) {
			object o = null;
			GraphModel m = null;

			BinaryFormatter formatter = new BinaryFormatter();
			using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None)) {
				using (GZipInputStream zipstream = new GZipInputStream(stream)) {
					try {
						o = formatter.Deserialize(zipstream);
					}
					catch { o = null; }
				}
			}
			if (o != null) {
				m = (GraphModel)o;
				m.Filename = filename;
				m.Modified = true;
			}
			return m;
		}

		/// <summary>
		/// Saves the model to a file using <c>filename</c>.
		/// </summary>
		public void SaveToFile() {
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(Filename, FileMode.Create, FileAccess.Write, FileShare.None);
			GZipOutputStream zipstream = new GZipOutputStream(stream);
			formatter.Serialize(zipstream, this);
			zipstream.Close();
			stream.Close();
		}
		
		/// <summary>
		/// Raises the Invalidated event. 
		/// </summary>
		public void Invalidate() {
			OnInvalidate();
		}

		/// <summary>
		/// Raises the Invalidated event.
		/// </summary>
		protected void OnInvalidate() {
			if (Invalidated != null) Invalidated(this);
		}

		/// <summary>
		/// Resets the raster with to default values.
		/// </summary>
		public void ResetRaster() {
			double w = x1-x0;
			double h = y1-y0;
			double wz = z1-z0;
    
			double dx = Math.Exp(Math.Floor(Math.Log(w)/ln10)*ln10);
			double dy = Math.Exp(Math.Floor(Math.Log(h)/ln10)*ln10);
			double dz = Math.Exp(Math.Floor(Math.Log(wz)/ln10)*ln10);

			if (w/dx > 5) rx = dx;
			else if (w/dx > 3) rx = dx/2;
			else rx = dx/5;

			if (h/dy > 5) ry = dy;
			else if (h/dy > 3) ry = dy/2;
			else ry = dy/5;

			if (wz/dz > 5) rz = dz;
			else if (wz/dz > 3) rz = dz/2;
			else rz = dz/5;
		}

		/// <summary>
		/// Sets the range of the plotting area. Throws an <c>System.ArgumentException</c> if the parameters
		/// are invalid.
		/// </summary>
		/// <param name="x0">The left border of the plotting area.</param>
		/// <param name="x1">The right border of the plotting area.</param>
		/// <param name="y0">The lower border of the plotting area.</param>
		/// <param name="y1">The upper border of the plotting area.</param>
		/// <param name="z0">The lower z-border of the plotting area.</param>
		/// <param name="z1">The upper z-border of the plotting area.</param>
		/// <param name="Width">The Width of the plotting area in pixels.</param>
		/// <param name="Height">The Height of the plotting area in pixels.</param>
		public void SetRange(double x0, double x1, double y0, double y1, double z0, double z1, int Width, int Height) {
			if ((x0 >= x1) || (y0 >= y1)) {
				if ((x0 == x1) || (y0 == y1)) {
					throw new System.ArgumentException("unable to zoom any further");
				} 
				else {
					throw new System.ArgumentException("x0 and y0 must be smaller than x1 and y1");
				}
			}
			if (z0 >= z1) throw new System.ArgumentException("z0 must be smaller than z1");

			this.x0 = x0;
			this.x1 = x1;
			if ((this.FixYtoX) && (Width > 0) && (Height > 0)) {
				double dx = (x1-x0)/Width;
				double h = dx*Height;
				this.y0 = (y1+y0-h)/2;
				this.y1 = (y1+y0+h)/2;
			} 
			else {
				this.y0 = y0;
				this.y1 = y1;
			}
			this.z0 = z0;
			this.z1 = z1;
			ResetRaster();
		}

		/// <summary>
		/// Sets the range of the plotting area.
		/// </summary>
		/// <param name="x0">The left border of the plotting area.</param>
		/// <param name="x1">The right border of the plotting area.</param>
		/// <param name="y0">The lower border of the plotting area.</param>
		/// <param name="y1">The upper border of the plotting area.</param>
		/// <param name="Width">The Width of the plotting area in pixels.</param>
		/// <param name="Height">The Height of the plotting area in pixels.</param>
		public void SetRange(double x0, double x1, double y0, double y1, int Width, int Height) {
			SetRange(x0, x1, y0, y1, z0, z1, Width, Height);
		}

		/// <summary>
		/// Returns a format string used by the ToString method.
		/// </summary>
		/// <param name="digits">The number of digits to display.</param>
		/// <param name="style">The number-style</param>
		/// <returns>A format string</returns>
		public static string NumberFormat(int digits, GraphModel.NumberStyle style) {
			switch (style) {
			case GraphModel.NumberStyle.Normal:
				return "G"+digits.ToString();
			case GraphModel.NumberStyle.Fixedpoint:
				return "F"+digits.ToString();
			case GraphModel.NumberStyle.Scientific:
				return "E"+digits.ToString();
			default:
				return "G3";
			}
		}
	}
}
