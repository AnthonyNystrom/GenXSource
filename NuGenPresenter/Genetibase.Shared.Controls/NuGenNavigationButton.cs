/* -----------------------------------------------
 * NuGenNavigationButton.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public sealed class NuGenNavigationButton : NuGenEventInitiator
	{
		/*
		 * Allowed
		 */

		private bool _allowed = true;

		/// <summary>
		/// </summary>
		public bool Allowed
		{
			get
			{
				return _allowed;
			}
			set
			{
				if (_allowed != value)
				{
					_allowed = value;

					if (!_allowed)
					{
						this.Visible = false;
					}

					this.OnAllowedChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _allowedChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Allowed"/> property changes.
		/// </summary>
		public event EventHandler AllowedChanged
		{
			add
			{
				this.Events.AddHandler(_allowedChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_allowedChanged, value);
			}
		}

		private void OnAllowedChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_allowedChanged, e);
		}

		/*
		 * Bounds
		 */

		private Rectangle _bounds;

		/// <summary>
		/// </summary>
		public Rectangle Bounds
		{
			get
			{
				return _bounds;
			}
			set
			{
				_bounds = value;
			}
		}

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
				return _image;
			}
			set
			{
				if (_image != value)
				{
					_image = value;
					this.OnImageChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _imageChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Image"/> property changes.
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

		private void OnImageChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_imageChanged, e);
		}

		/*
		 * Selected
		 */

		private bool _selected;

		/// <summary>
		/// </summary>
		public bool Selected
		{
			get
			{
				return _selected;
			}
			set
			{
				_selected = value;
				// TODO: See TestOutlookBar.
			}
		}

		/*
		 * Text
		 */

		private string _text;

		/// <summary>
		/// </summary>
		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
			}
		}

		private static readonly object _textChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Text"/> property changed.
		/// </summary>
		public event EventHandler TextChanged
		{
			add
			{
				this.Events.AddHandler(_textChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_textChanged, value);
			}
		}

		/*
		 * Visible
		 */

		private bool _visible = true;

		/// <summary>
		/// </summary>
		public bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				if (_visible != value)
				{
					_visible = value;

					if (!_visible)
					{
						_bounds = Rectangle.Empty;
					}

					this.OnVisibleChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _visibleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Visible"/> property changes.
		/// </summary>
		public event EventHandler VisibleChanged
		{
			add
			{
				this.Events.AddHandler(_visibleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_visibleChanged, value);
			}
		}

		private void OnVisibleChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_visibleChanged, e);
		}

		/*
		 * IsLarge
		 */

		private bool _isLarge;

		/// <summary>
		/// </summary>
		internal bool IsLarge
		{
			get
			{
				return _isLarge;
			}
			set
			{
				_isLarge = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenNavigationButton"/> class.
		/// </summary>
		public NuGenNavigationButton()
		{
		}
	}
}
