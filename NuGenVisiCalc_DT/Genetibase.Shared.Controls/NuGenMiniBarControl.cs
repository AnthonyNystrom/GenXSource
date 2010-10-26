/* -----------------------------------------------
 * NuGenMiniBarControl.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[DesignTimeVisibleAttribute(false)]
	[Designer("Genetibase.Shared.Controls.Design.NuGenMiniBarControlDesigner")]
	[ToolboxItemAttribute(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public abstract class NuGenMiniBarControl : Component
	{
		/// <summary>
		/// </summary>
		public abstract NuGenMiniBarButtonState Action(Point mouse, MouseButtons but, NuGenMiniBarUpdateAction act);

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public abstract NuGenMiniBarButtonState NState
		{
			get;
		}

		private NuGenMiniBar _owner;

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public NuGenMiniBar Owner
		{
			get
			{
				return _owner;
			}
			set
			{
				_owner = value;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public Rectangle ClientRectangle
		{
			get
			{
				return new Rectangle(this.Location, this.Size);
			}
		}
		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Point Location
		{
			get
			{
				return new Point(this.X, this.Y);
			}
			set
			{
				this.SetBounds(value.X, value.Y, 0, 0, BoundsSpecified.Location);
			}
		}

		/// <summary>
		/// </summary>
		[RefreshProperties(RefreshProperties.Repaint)]
		public Size Size
		{
			get
			{
				return new Size(this.Width, this.Height);
			}
			set
			{
				this.SetBounds(0, 0, value.Width, value.Height, BoundsSpecified.Size);
			}
		}

		private int _height = 13;

		/// <summary>
		/// </summary>
		[RefreshProperties(RefreshProperties.Repaint)]
		public int Height
		{
			get
			{
				return _height;
			}
			set
			{
				this.SetBounds(0, 0, 0, value, BoundsSpecified.Height);
			}
		}

		private int _width = 13;

		/// <summary>
		/// </summary>
		[RefreshProperties(RefreshProperties.Repaint)]
		public int Width
		{
			get
			{
				return _width;
			}
			set
			{
				this.SetBounds(0, 0, value, 0, BoundsSpecified.Width);
			}
		}

		private int _x;

		/// <summary>
		/// </summary>
		[RefreshProperties(RefreshProperties.Repaint)]
		public int X
		{
			get
			{
				return _x;
			}
			set
			{
				this.SetBounds(value, 0, 0, 0, BoundsSpecified.X);
			}
		}

		private int _y;

		/// <summary>
		/// </summary>
		[RefreshProperties(RefreshProperties.Repaint)]
		public int Y
		{
			get
			{
				return _y;
			}
			set
			{
				this.SetBounds(0, value, 0, 0, BoundsSpecified.Y);
			}
		}

		/// <summary>
		/// </summary>
		protected virtual void SetBounds(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if ((specified & BoundsSpecified.X) != BoundsSpecified.None)
			{
				_x = x;
			}

			if ((specified & BoundsSpecified.Y) != BoundsSpecified.None)
			{
				_y = y;
			}

			if ((specified & BoundsSpecified.Width) != BoundsSpecified.None)
			{
				_width = width;
			}

			if ((specified & BoundsSpecified.Height) != BoundsSpecified.None)
			{
				_height = height;
			}
		}
	}
}
