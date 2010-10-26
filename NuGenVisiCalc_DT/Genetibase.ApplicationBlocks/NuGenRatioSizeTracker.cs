/* -----------------------------------------------
 * NuGenAspectRatio.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using System;
using System.Drawing;

namespace Genetibase.ApplicationBlocks
{
	/// <summary>
	/// Provides functionality to track changes on the initial size. If width (height) is changed and
	/// <see cref="MaintainAspectRatio"/> is <see langword="true"/>, height (width)
	/// is recalculated according to the <see cref="Ratio"/>. <see cref="Ratio"/> is calculated as
	/// <see cref="P:System.Drawing.Size.Width"/> / <see cref="P:System.Drawing.Size.Height"/>. Default value for <see cref="Ratio"/>
	/// is <c>1.0</c>.
	/// </summary>
	public sealed class NuGenRatioSizeTracker : NuGenEventInitiator
	{
		private Int32 _height;

		/// <summary>
		/// Gets or sets tracked size height.
		/// </summary>
		public Int32 Height
		{
			get
			{
				return _height;
			}
			set
			{
				if (_height != value)
				{
					_height = value;

					if (MaintainAspectRatio)
					{
						Int32 previousWidth = _width;
						Int32 newWidth = (Int32)(value * Ratio);

						if (previousWidth != newWidth)
						{
							_width = newWidth;
							OnWidthChanged(EventArgs.Empty);
						}
					}
				}
			}
		}

		private static readonly Object _heightChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="Width"/> property changes.
		/// </summary>
		public event EventHandler HeightChanged
		{
			add
			{
				Events.AddHandler(_heightChanged, value);
			}
			remove
			{
				Events.RemoveHandler(_heightChanged, value);
			}
		}

		private void OnHeightChanged(EventArgs e)
		{
			Initiator.InvokePropertyChanged(_heightChanged, e);
		}

		private Boolean _maintainAspectRatio;

		/// <summary>
		/// Gets or sets the value indicating whether to recalculate tracked size width (height) according
		/// to the <see cref="Ratio"/> or leave as is.
		/// </summary>
		public Boolean MaintainAspectRatio
		{
			get
			{
				return _maintainAspectRatio;
			}
			set
			{
				_maintainAspectRatio = value;
			}
		}

		private Double _ratio = 1.0;

		/// <summary>
		/// Gets the ratio that is specified in the constructor by the passed <see cref="Size"/> structure
		/// and is used to recalculate tracked size width (height).
		/// </summary>
		public Double Ratio
		{
			get
			{
				return _ratio;
			}
		}

		/// <summary>
		/// </summary>
		public void UpdateRatio()
		{
			_ratio = (Size.Width == 0 || Size.Height == 0) ? 1.0 : ((Double)Size.Width / (Double)Size.Height);
		}

		/// <summary>
		/// Gets tracked size.
		/// </summary>
		public Size Size
		{
			get
			{
				return new Size(Width, Height);
			}
			set
			{
				Width = value.Width;
				Height = value.Height;
			}
		}

		private Int32 _width;

		/// <summary>
		/// Gets or sets tracked size width.
		/// </summary>
		public Int32 Width
		{
			get
			{
				return _width;
			}
			set
			{
				if (_width != value)
				{
					_width = value;

					if (MaintainAspectRatio)
					{
						Int32 previousHeight = _height;
						Int32 newHeight = (Int32)(value / Ratio);

						if (previousHeight != newHeight)
						{
							_height = newHeight;
							OnHeightChanged(EventArgs.Empty);
						}
					}	
				}
			}
		}

		private static readonly Object _widthChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="Width"/> property changes.
		/// </summary>
		public event EventHandler WidthChanged
		{
			add
			{
				Events.AddHandler(_widthChanged, value);
			}
			remove
			{
				Events.RemoveHandler(_widthChanged, value);
			}
		}

		private void OnWidthChanged(EventArgs e)
		{
			Initiator.InvokePropertyChanged(_widthChanged, e);
		}

		/// <summary>
		/// Returns string representation for this <see cref="NuGenRatioSizeTracker"/>.
		/// </summary>
		/// <returns></returns>
		public override String ToString()
		{
			return String.Format("Size: {0} Ratio: {1} MaintainAspectRatio: {2}", Size, Ratio, MaintainAspectRatio);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRatioSizeTracker"/> class.
		/// </summary>
		/// <param name="size">Specifies the initial size to track.</param>
		public NuGenRatioSizeTracker(Size size)
		{
			Size = size;
			UpdateRatio();
		}
	}
}
