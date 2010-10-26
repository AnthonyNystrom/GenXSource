/* -----------------------------------------------
 * NuGenAspectRatio.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using System;
using System.Drawing;

namespace Genetibase.ApplicationBlocks.ImageExportInternals
{
	/// <summary>
	/// Provides functionality to track changes on the initial size. If width (height) is changed and
	/// <see cref="MaintainAspectRatio"/> is <see langword="true"/>, height (width)
	/// is recalculated according to the <see cref="Ratio"/>. <see cref="Ratio"/> is calculated as
	/// <see cref="P:System.Drawing.Size.Width"/> / <see cref="P:System.Drawing.Size.Height"/>. Default value for <see cref="Ratio"/>
	/// is <c>1.0</c>.
	/// </summary>
	internal sealed class NuGenRatioSizeTracker : NuGenEventInitiator
	{
		private int _height;

		/// <summary>
		/// Gets or sets tracked size height.
		/// </summary>
		public int Height
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

					if (this.MaintainAspectRatio)
					{
						int previousWidth = _width;
						int newWidth = (int)(value * this.Ratio);

						if (previousWidth != newWidth)
						{
							_width = newWidth;
							this.OnWidthChanged(EventArgs.Empty);
						}
					}
				}
			}
		}

		private static readonly object _heightChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Width"/> property changes.
		/// </summary>
		public event EventHandler HeightChanged
		{
			add
			{
				this.Events.AddHandler(_heightChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_heightChanged, value);
			}
		}

		private void OnHeightChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_heightChanged, e);
		}

		private bool _maintainAspectRatio;

		/// <summary>
		/// Gets or sets the value indicating whether to recalculate tracked size width (height) according
		/// to the <see cref="Ratio"/> or leave as is.
		/// </summary>
		public bool MaintainAspectRatio
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

		private double _ratio = 1.0;

		/// <summary>
		/// Gets the ratio that is specified in the constructor by the passed <see cref="Size"/> structure
		/// and is used to recalculate tracked size width (height).
		/// </summary>
		public double Ratio
		{
			get
			{
				return _ratio;
			}
		}

		/// <summary>
		/// Gets tracked size.
		/// </summary>
		public Size Size
		{
			get
			{
				return new Size(this.Width, this.Height);
			}
			set
			{
				this.Width = value.Width;
				this.Height = value.Height;
			}
		}

		private int _width;

		/// <summary>
		/// Gets or sets tracked size width.
		/// </summary>
		public int Width
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

					if (this.MaintainAspectRatio)
					{
						int previousHeight = _height;
						int newHeight = (int)(value / this.Ratio);

						if (previousHeight != newHeight)
						{
							_height = newHeight;
							this.OnHeightChanged(EventArgs.Empty);
						}
					}	
				}
			}
		}

		private static readonly object _widthChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Width"/> property changes.
		/// </summary>
		public event EventHandler WidthChanged
		{
			add
			{
				this.Events.AddHandler(_widthChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_widthChanged, value);
			}
		}

		private void OnWidthChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_widthChanged, e);
		}

		public override string ToString()
		{
			return string.Format("Size: {0} Ratio: {1} MaintainAspectRatio: {2}", this.Size, this.Ratio, this.MaintainAspectRatio);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRatioSizeTracker"/> class.
		/// </summary>
		/// <param name="size">Specifies the initial size to track.</param>
		public NuGenRatioSizeTracker(Size size)
		{
			this.Size = size;
			_ratio = (size.Width == 0 || size.Height == 0)
				? 1.0
				: ((double)size.Width / (double)size.Height)
				;
		}
	}
}
