/* -----------------------------------------------
 * NuGenControlImageDescriptor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Drawing
{
	/// <summary>
	/// Encapsulates Image, ImageIndex, and ImageList associated data and logic.
	/// </summary>
	public class NuGenControlImageDescriptor : NuGenEventInitiator, INuGenControlImageDescriptor
	{
		#region Properties.Public

		/*
		 * Image
		 */

		private Image _image;

		/// <summary>
		/// </summary>
		public Image Image
		{
			get
			{
				if (_image == null && _imageList != null)
				{
					int imageIndex = _imageIndex.Value;

					if (imageIndex >= 0)
					{
						return _imageList.Images[imageIndex];
					}
				}

				return _image;
			}
			set
			{
				if (_image != value)
				{
					_image = value;

					if (_image != null)
					{
						this.ImageIndex = -1;
						this.ImageList = null;
					}

					this.OnImageChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _imageChanged = new object();

		/// <summary>
		/// </summary>
		public event EventHandler ImageChanged
		{
			add
			{
				this.Events.AddHandler(_imageChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_imageChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="Genetibase.Shared.Drawing.NuGenControlImageDescriptor.ImageChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnImageChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_imageChanged, e);
		}

		/*
		 * ImageIndex
		 */

		private NuGenInt32 _imageIndex = new NuGenInt32(-1, int.MaxValue);

		/// <summary>
		/// </summary>
		public int ImageIndex
		{
			get
			{
				int value = _imageIndex.Value;

				if (
					value != -1
					&& _imageList != null
					&& value >= _imageList.Images.Count
					)
				{
					return _imageList.Images.Count - 1;
				}

				return value;
			}
			set
			{
				if (_imageIndex.Value != value)
				{
					if (value != -1)
					{
						_image = null;
					}

					_imageIndex.Value = value;
					this.OnImageIndexChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _imageIndexChanged = new object();

		/// <summary>
		/// </summary>
		public event EventHandler ImageIndexChanged
		{
			add
			{
				this.Events.AddHandler(_imageIndexChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_imageIndexChanged, value);
			}
		}

		/// <summary>
		/// Will bubble
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnImageIndexChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_imageIndexChanged, e);
		}

		/*
		 * ImageList
		 */

		private ImageList _imageList;

		/// <summary>
		/// </summary>
		public ImageList ImageList
		{
			get
			{
				return _imageList;
			}
			set
			{
				if (_imageList != value)
				{
					if (_imageList != null)
					{
						_imageList.Disposed -= ImageList_Disposed;
						_imageList.RecreateHandle -= ImageList_RecreateHandle;
					}

					_imageList = value;

					if (_imageList != null)
					{
						_image = null;
						_imageList.Disposed += ImageList_Disposed;
						_imageList.RecreateHandle += ImageList_RecreateHandle;
					}

					this.OnImageListChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _imageListChanged = new object();

		/// <summary>
		/// </summary>
		public event EventHandler ImageListChanged
		{
			add
			{
				this.Events.AddHandler(_imageListChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_imageListChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="Genetibase.Shared.Drawing.NuGenControlImageDescriptor.ImageListChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnImageListChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_imageListChanged, e);
		}

		private void ImageList_Disposed(object sender, EventArgs e)
		{
			this.ImageList = null;
		}

		private void ImageList_RecreateHandle(object sender, EventArgs e)
		{
			this.OnImageListChanged(EventArgs.Empty);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenControlImageDescriptor"/> class.
		/// </summary>
		public NuGenControlImageDescriptor()
		{
		}

		#endregion
	}
}
