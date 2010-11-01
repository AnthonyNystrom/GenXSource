/* -----------------------------------------------
 * NuGenAspectRatio.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using System;
using System.Drawing;

namespace Genetibase.ApplicationBlocks.ImageExport
{
	/// <summary>
	/// Provides functionality to track changes on the initial size. If width (height) is changed and
	/// <see cref="MaintainAspectRatio"/> is <see langword="true"/>, height (width)
	/// is recalculated according to the <see cref="Ratio"/>. <see cref="Ratio"/> is calculated as
	/// <see cref="P:System.Drawing.Size.Width"/> / <see cref="P:System.Drawing.Size.Height"/>. Default value for <see cref="Ratio"/>
	/// is <c>1.0</c>.
	/// </summary>
	internal class NuGenRatioSizeTracker : NuGenEventInitiator
	{
		#region Properties.Public

		/*
		 * Height
		 */

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

		/// <summary>
		/// Raises the <see cref="HeightChanged"/> event.
		/// </summary>
		protected virtual void OnHeightChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_heightChanged, e);
		}

		/*
		 * MaintainAspectRatio
		 */

		private bool _maintainAspectRatio = true;

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

		/*
		 * Ratio
		 */

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
			protected set
			{
				_ratio = value;
			}
		}

		/*
		 * Size
		 */

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

		/*
		 * Width
		 */

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

		/// <summary>
		/// Raises the <see cref="WidthChanged"/> event.
		/// </summary>
		protected virtual void OnWidthChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_widthChanged, e);
		}

		#endregion

		#region Methods.Public.Overriden

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			return string.Format("Size: {0} Ratio: {1} MaintainAspectRatio: {2}", this.Size, this.Ratio, this.MaintainAspectRatio);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRatioSizeTracker"/> class.
		/// </summary>
		/// <param name="size">Specifies the initial size to track.</param>
		public NuGenRatioSizeTracker(Size size)
			: this(size, true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRatioSizeTracker"/> class.
		/// </summary>
		/// <param name="size">Specifies the initial size to track.</param>
		/// <param name="maintainAspectRatio">Specifies the value indicating whether to recalculate tracked
		/// size width (height) according to the <see cref="P:Ratio"/> or leave as is.</param>
		public NuGenRatioSizeTracker(Size size, bool maintainAspectRatio)
		{
			this.MaintainAspectRatio = maintainAspectRatio;
			this.Size = size;
			this.Ratio = (size.Width == 0 || size.Height == 0)
				? 1.0
				: ((double)size.Width / (double)size.Height);
		}

		#endregion
	}
}
