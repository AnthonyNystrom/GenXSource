using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Genetibase.MathX.NugenCCalc.Adapters ;

namespace Genetibase.MathX.NugenCCalc.Design
{
	/// <summary>
	/// Summary description for SeriesListBox.
	/// </summary>
	[ToolboxItem(false)]
	public class SeriesListBox : ListBox
	{

		public class SeriesObjectCollection : ObjectCollection
		{
			private SeriesListBox _owner = null;
			public SeriesObjectCollection(SeriesListBox owner)
				: base(owner)
			{                
				_owner = owner;
			}

			public SeriesObjectCollection(SeriesListBox owner, object[] value)
				: base(owner, value)
			{
				_owner = owner;
			}

			public SeriesObjectCollection(SeriesListBox owner, ObjectCollection value)
				: base(owner,value)
			{
				_owner = owner;
			}
			
		}


        private SeriesObjectCollection _items = null;
        private System.ComponentModel.IContainer components;

		public SeriesListBox() : base()
		{           
			if (!DesignMode)
			{
				DrawMode = DrawMode.OwnerDrawVariable;
			}
            InitializeComponent();
		}

		// need to redraw rows when resize
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Invalidate();
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (e.Index < 0 || e.Index >= Items.Count)
				return;

			if (this.Items.Count > 0)
			{               
				DrawItem(Items[e.Index], e);                
			}
		}

		protected new void DrawItem(object item, DrawItemEventArgs e)
		{ 
			if (item is Series)
			{
				Series series = (Series)item;
				Font itemFont = Font;
                

				System.Drawing.Drawing2D.LinearGradientBrush bb =
					new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, 
					BackColor,Color.Black, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
				Pen p = new Pen(bb, 1);

				if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
				{
					e.Graphics.FillRectangle(bb, e.Bounds);
				}
				else 
					e.DrawBackground();

				//e.Graphics.DrawString(series.Index.ToString() + " " + series.Label, itemFont, SystemBrushes.ControlText, e.Bounds.Left + 8, e.Bounds.Top);
				e.Graphics.DrawString(series.Index.ToString(), itemFont, SystemBrushes.ControlText, e.Bounds.Left, e.Bounds.Top);
				e.Graphics.DrawString(series.Label, itemFont, SystemBrushes.ControlText, e.Bounds.Left + 20, e.Bounds.Top);

				e.Graphics.DrawLine(p, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);

				Point statusImagePoint = new Point(e.Bounds.Left+12, e.Bounds.Top + 3);

				Bitmap image = new Bitmap(8,8);
				Graphics graphics = Graphics.FromImage(image);
				graphics.FillRectangle(new SolidBrush(series.Color), new Rectangle(0,0,image.Width, image.Height));
				e.Graphics.DrawImageUnscaled(image, statusImagePoint);
			}
		}


		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SeriesListBox));
			this.SuspendLayout();
			// 
			// SeriesListBox
			// 
			this.BackColor = System.Drawing.Color.AliceBlue;
			this.ResumeLayout(false);
		}
	}
}
