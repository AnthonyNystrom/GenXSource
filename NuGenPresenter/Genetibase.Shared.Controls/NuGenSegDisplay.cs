/* -----------------------------------------------
 * NuGenSegDisplay.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ComponentModel;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a 7-segment digital display.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSegDisplay), "Resources.NuGenIcon.png")]
	[DefaultEvent("Click")]
	[DefaultProperty("Value")]
	[Description("Description_SegDisplay")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenSegDisplayDesigner")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSegDisplay : Control
	{
		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(10)]
		[NuGenSRCategory("Category_Appearance")]
		public int SegmentLength
		{
			get
			{
				return SL;
			}
			set
			{
				SL = value;
				Anpassen();
				this.Refresh();
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		public new bool AutoSize
		{
			get
			{
				return b_autosize;
			}
			set
			{
				b_autosize = value;
				Anpassen();
				this.Refresh();
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		public float Brightness
		{
			get
			{
				return stf.Width;
			}
			set
			{
				stf.Width = value;
				Anpassen();
				this.Refresh();
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		public int Value
		{
			get
			{
				return ZL;
			}
			set
			{
				ZL = Math.Min(Math.Max(value, 0), 10);
				this.Refresh();
			}
		}

		/// <summary>
		/// Gets or sets the offset between two segments.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(2)]
		[NuGenSRCategory("Category_Apparance")]
		[NuGenSRDescription("Description_SegDisplay_Offset")]
		public int Offset
		{
			get
			{
				return AS;
			}
			set
			{
				AS = value;
				Anpassen();
				this.Refresh();
			}
		}

		/// <summary>
		/// </summary>
		[NuGenSRCategory("Category_Appearance")]
		public Color ColorLEDOn
		{
			get
			{
				return actcol;
			}
			set
			{
				actcol = value;
				this.Refresh();
			}
		}

		/// <summary>
		/// </summary>
		[NuGenSRCategory("Category_Appearance")]
		public Color ColorLEDOff
		{
			get
			{
				return inactcol;
			}
			set
			{
				inactcol = value;
				this.Refresh();
			}
		}

		private static readonly Size _defaultSize = new Size(40, 40);

		/// <summary>
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return _defaultSize;
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			Anpassen();
			this.Refresh();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.Clear(Color.Black);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			Malen(ZL, e.Graphics);
		}

		private void Anpassen()
		{
			int Rand, x, y;
			if (b_autosize)
			{
				int br = Math.Min(this.Height / 9, this.Width / 5);
				stf.Width = br;
				SL = Math.Min(this.Width - (int)stf.Width, (int)((this.Height - stf.Width) / 2));
				AS = (int)(stf.Width * 0.6);
			}
			Rand = (int)Math.Ceiling(stf.Width / 2d);
			x = (int)((this.Width - (2 * Rand + SL)) / 2);
			y = (int)((this.Height - 2 * (Rand + SL)) / 2);

			Punkte[0] = new Point(x + Rand, y + Rand + SL - AS);
			Punkte[1] = new Point(x + Rand, y + Rand + AS);

			Punkte[2] = new Point(x + Rand + AS, y + Rand);
			Punkte[3] = new Point(x + Rand + SL - AS, y + Rand);

			Punkte[4] = new Point(x + Rand + SL, y + Rand + AS);
			Punkte[5] = new Point(x + Rand + SL, y + Rand + SL - AS);

			Punkte[6] = new Point(x + Rand + SL - AS, y + Rand + SL);
			Punkte[7] = new Point(x + Rand + AS, y + Rand + SL);

			Punkte[8] = new Point(x + Rand, y + Rand + SL + AS);
			Punkte[9] = new Point(x + Rand, y + Rand + 2 * SL - AS);

			Punkte[10] = new Point(x + Rand + AS, y + Rand + 2 * SL);
			Punkte[11] = new Point(x + Rand + SL - AS, y + Rand + 2 * SL);

			Punkte[12] = new Point(x + Rand + SL, y + Rand + SL + AS);
			Punkte[13] = new Point(x + Rand + SL, y + Rand + 2 * SL - AS);
		}

		private void Malen(int num, Graphics gr)
		{
			int nm = 1 << num;
			for (int i = 0; i < 7; i++)
			{
				stf.Color = (prüfnummern[i] & nm) != 0 ? actcol : inactcol;
				gr.DrawLine(stf, Punkte[i * 2], Punkte[i * 2 + 1]);
			}
		}

		private Color actcol = Color.Red, inactcol = Color.FromArgb(40, 0, 0);
		private Pen stf = new Pen(Color.Red);
		private int[] prüfnummern = new int[] { 881, 941, 927, 892, 325, 365, 1019 };
		private Point[] Punkte = new Point[16];
		private int SL, ZL, AS;
		private bool b_autosize;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSegDisplay"/> class.
		/// </summary>
		public NuGenSegDisplay()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint |
				ControlStyles.UserPaint |
				ControlStyles.DoubleBuffer, true);
			stf.EndCap = LineCap.Triangle;
			stf.StartCap = LineCap.Triangle;
			stf.Width = 3;
			SL = 10;
			ZL = 0;
			AS = 2;
			Anpassen();
		}
	}
}
