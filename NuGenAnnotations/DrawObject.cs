using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Drawing.Drawing2D;

namespace Genetibase.NuGenAnnotation
{
	/// <summary>
	/// Base class for all draw objects
	/// </summary>
	[Serializable]
	public abstract class DrawObject : Control,IComparable
	{
		#region Members

		// Object properties
		private bool selected;
		private Color color;
		private Color fillColor;
		private bool filled;
		private int penWidth;
		private Pen drawpen;
		private Brush drawBrush;
		private DrawingPens.PenType _penType;
		private FillBrushes.BrushType _brushType;

		// Last used property values (may be kept in the Registry)
		private static Color lastUsedColor = Color.Black;
		private static int lastUsedPenWidth = 1;

		// Entry names for serialization
		private const string entryColor = "Color";
		private const string entryPenWidth = "PenWidth";
		private const string entryPen = "DrawPen";
		private const string entryBrush = "DrawBrush";
		private const string entryFillColor = "FillColor";
		private const string entryFilled = "Filled";
		private const string entryZOrder = "ZOrder";
		private bool dirty;

		private int _id;
		private int _zOrder;

		#endregion Members

		#region Properties

		/// <summary>
		/// ZOrder is the order the objects will be drawn in - lower the ZOrder, the closer the to top the object is.
		/// </summary>
		public int ZOrder
		{
			get { return _zOrder; }
			set { _zOrder = value; }
		}
		/// <summary>
		/// Object ID used for Undo Redo functions
		/// </summary>
		public int ID
		{
			get { return _id; }
			set { _id = value; }
		}
		/// <summary>
		/// Set to true whenever the object changes
		/// </summary>
		public bool Dirty
		{
			get { return dirty; }
			set { dirty = value; }
		}
		/// <summary>
		/// Draw object filled?
		/// </summary>
		public bool Filled
		{
			get { return filled; }
			set { filled = value; }
		}

		/// <summary>
		/// Selection flag
		/// </summary>
		public bool Selected
		{
			get { return selected; }
			set { selected = value; }
		}

		/// <summary>
		/// Fill Color
		/// </summary>
		public Color FillColor
		{
			get { return fillColor; }
			set { fillColor = value; }
		}
		/// <summary>
		/// Border (line) Color
		/// </summary>
		public Color Color
		{
			get { 	return color; }
			set { color = value; }
		}

		/// <summary>
		/// Pen width
		/// </summary>
		public int PenWidth
		{
			get { return penWidth; }
			set { penWidth = value; }
		}

		public FillBrushes.BrushType BrushType
		{
			get { return _brushType; }
			set { _brushType = value; }
		}
	
		/// <summary>
		/// Brush used to paint object
		/// </summary>
		public Brush DrawBrush
		{
			get { return drawBrush; }
			set { drawBrush = value; }
		}

		public DrawingPens.PenType PenType
		{
			get { return _penType; }
			set { _penType = value; }
		}
	
		/// <summary>
		/// Pen used to draw object
		/// </summary>
		public Pen DrawPen
		{
			get { return drawpen; }
			set { drawpen = value; }
		}

		/// <summary>
		/// Number of handles
		/// </summary>
		public virtual int HandleCount
		{
			get { return 0; }
		}

		/// <summary>
		/// Last used color
		/// </summary>
		public static Color LastUsedColor
		{
			get { return lastUsedColor; }
			set { lastUsedColor = value; }
		}

		/// <summary>
		/// Last used pen width
		/// </summary>
		public static int LastUsedPenWidth
		{
			get { return lastUsedPenWidth; }
			set { lastUsedPenWidth = value; }
		}

		#endregion Properties

		#region Virtual Functions

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public abstract DrawObject Clone();
		/// <summary>
		/// Draw object
		/// </summary>
		/// <param name="g"></param>
		public virtual void Draw(Graphics g)
		{
			_id = this.GetHashCode();
		}

		/// <summary>
		/// Get handle point by 1-based number
		/// </summary>
		/// <param name="handleNumber"></param>
		/// <returns></returns>
		public virtual Point GetHandle(int handleNumber)
		{
			return new Point(0, 0);
		}

		/// <summary>
		/// Get handle rectangle by 1-based number
		/// </summary>
		/// <param name="handleNumber"></param>
		/// <returns></returns>
		public virtual Rectangle GetHandleRectangle(int handleNumber)
		{
			Point point = GetHandle(handleNumber);
			// Take into account width of pen
			return new Rectangle(point.X - (penWidth + 3), point.Y - (penWidth + 3), 7 + penWidth, 7 + penWidth);
		}

		/// <summary>
		/// Draw tracker for selected object
		/// </summary>
		/// <param name="g"></param>
		public virtual void DrawTracker(Graphics g)
		{
			if (!Selected)
				return;

			SolidBrush brush = new SolidBrush(Color.Black);

			for (int i = 1; i <= HandleCount; i++)
			{
				g.FillRectangle(brush, GetHandleRectangle(i));
			}

			brush.Dispose();
		}

		/// <summary>
		/// Hit test.
		/// Return value: -1 - no hit
		///                0 - hit anywhere
		///                > 1 - handle number
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public virtual int HitTest(Point point)
		{
			return -1;
		}


		/// <summary>
		/// Test whether point is inside of the object
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		protected virtual bool PointInObject(Point point)
		{
			return false;
		}


		/// <summary>
		/// Get cursor for the handle
		/// </summary>
		/// <param name="handleNumber"></param>
		/// <returns></returns>
		public virtual Cursor GetHandleCursor(int handleNumber)
		{
			return Cursors.Default;
		}

		/// <summary>
		/// Test whether object intersects with rectangle
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns></returns>
		public virtual bool IntersectsWith(Rectangle rectangle)
		{
			return false;
		}

		/// <summary>
		/// Move object
		/// </summary>
		/// <param name="deltaX"></param>
		/// <param name="deltaY"></param>
		public new virtual void Move(int deltaX, int deltaY)
		{
		}

		/// <summary>
		/// Move handle to the point
		/// </summary>
		/// <param name="point"></param>
		/// <param name="handleNumber"></param>
		public virtual void MoveHandleTo(Point point, int handleNumber)
		{
		}

		/// <summary>
		/// Dump (for debugging)
		/// </summary>
		public virtual void Dump()
		{
			Trace.WriteLine("");
			Trace.WriteLine(this.GetType().Name);
			Trace.WriteLine("Selected = " + selected.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Normalize object.
		/// Call this function in the end of object resizing.
		/// </summary>
		public virtual void Normalize()
		{
		}


		/// <summary>
		/// Save object to serialization stream
		/// </summary>
		/// <param name="info">The data being written to disk</param>
		/// <param name="orderNumber">Index of the Layer being saved</param>
		/// <param name="objectIndex">Index of the object on the Layer</param>
		public virtual void SaveToStream(SerializationInfo info, int orderNumber, int objectIndex)
		{
			info.AddValue(
				String.Format(CultureInfo.InvariantCulture,
					"{0}{1}-{2}",
					entryColor, orderNumber, objectIndex),
				Color.ToArgb());

			info.AddValue(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}-{2}",
				entryPenWidth, orderNumber, objectIndex),
				PenWidth);

			info.AddValue(
				string.Format(CultureInfo.InvariantCulture,
				"{0}{1}-{2}",
				entryPen, orderNumber, objectIndex),
				PenType);

			info.AddValue(
				string.Format(CultureInfo.InvariantCulture,
				"{0}{1}-{2}",
				entryBrush, orderNumber, objectIndex),
				BrushType);
			
			info.AddValue(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}-{2}",
				entryFillColor, orderNumber, objectIndex),
				FillColor.ToArgb());

			info.AddValue(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}-{2}",
				entryFilled, orderNumber, objectIndex),
				Filled);

			info.AddValue(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}-{2}",
				entryZOrder, orderNumber, objectIndex),
				ZOrder);
		}

		/// <summary>
		/// Load object from serialization stream
		/// </summary>
		/// <param name="info"></param>
		/// <param name="orderNumber"></param>
		public virtual void LoadFromStream(SerializationInfo info, int orderNumber, int objectData)
		{
			int n = info.GetInt32(
				String.Format(CultureInfo.InvariantCulture,
					"{0}{1}-{2}",
					entryColor, orderNumber, objectData));

			Color = Color.FromArgb(n);

			PenWidth = info.GetInt32(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}-{2}",
				entryPenWidth, orderNumber, objectData));

			PenType = (DrawingPens.PenType)info.GetValue(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}-{2}",
				entryPen, orderNumber, objectData),
				typeof(DrawingPens.PenType));

			BrushType = (FillBrushes.BrushType)info.GetValue(
				string.Format(CultureInfo.InvariantCulture,
				"{0}{1}-{2}",
				entryBrush, orderNumber, objectData),
				typeof(FillBrushes.BrushType));

			n = info.GetInt32(
				String.Format(CultureInfo.InvariantCulture,
					"{0}{1}-{2}",
					entryFillColor, orderNumber, objectData));

			FillColor = Color.FromArgb(n);

			Filled = info.GetBoolean(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}-{2}",
				entryFilled, orderNumber, objectData));
			//if(Curr
			ZOrder = info.GetInt32(
				String.Format(CultureInfo.InvariantCulture,
					"{0}{1}-{2}",
					entryZOrder, orderNumber, objectData));

			// Set the Pen and the Brush, if defined
			if (PenType != DrawingPens.PenType.Generic)
				DrawPen = DrawingPens.SetCurrentPen(PenType);
			if (BrushType != FillBrushes.BrushType.NoBrush)
				DrawBrush = FillBrushes.SetCurrentBrush(BrushType);
		}

		#endregion Virtual Functions

		#region Other functions

		/// <summary>
		/// Initialization
		/// </summary>
		protected void Initialize()
		{
		}
		/// <summary>
		/// Copy fields from this instance to cloned instance drawObject.
		/// Called from Clone functions of derived classes.
		/// </summary>
		protected void FillDrawObjectFields(DrawObject drawObject)
		{
			drawObject.selected = this.selected;
			drawObject.color = this.color;
			drawObject.penWidth = this.penWidth;
			drawObject.ID = this.ID;
			drawObject._brushType = this._brushType;
			drawObject._penType = this._penType;
			drawObject.drawBrush = this.drawBrush;
			drawObject.drawpen = this.drawpen;
			drawObject.fillColor = this.fillColor;
		}

		#endregion Other functions

		#region IComparable Members
		/// <summary>
		/// Returns -1, 0, +1 to represent the relative order of the object being compared with this object
		/// </summary>
		/// <param name="obj">DrawObject being compared</param>
		/// <returns>-1 if the object is less (further back) than this object.
		///				0 if the object is equal to this object (same level graphically).
		///				1 if the object is greater (closer to the front) than this object.</returns>
		public int CompareTo(object obj)
		{
			DrawObject d = obj as DrawObject;
			int x = 0;
			if (d.ZOrder == this.ZOrder)
				x = 0;
			else if (d.ZOrder > this.ZOrder)
				x = -1;
			else
				x = 1;

			return x;
		}

		#endregion IComparable Members
	}
}
