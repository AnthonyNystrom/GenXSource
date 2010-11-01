/* -----------------------------------------------
 * NuGenNavigationButton.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[DefaultProperty("Text")]
	[DesignTimeVisible(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public sealed class NuGenNavigationButton : Component
	{
		#region Properties.Public

		/*
		 * Allowed
		 */

		private bool _allowed = true;

		/// <summary></summary>
		[DefaultValue(true)]
		public bool Allowed
		{
			get
			{
				return _allowed;
			}
			set
			{
				_allowed = value;

				if (!_allowed)
				{
					this.Visible = false;
				}
			}
		}

		/*
		 * Bounds
		 */

		private Rectangle _bounds;

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary></summary>
		[DefaultValue(null)]
		public Image Image
		{
			get
			{
				return _image;
			}
			set
			{
				_image = value;
			}
		}

		/*
		 * Parent
		 */

		private NuGenNavigationBar _parent;

		/// <summary></summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public NuGenNavigationBar Parent
		{
			get
			{
				return _parent;
			}
			set
			{
				_parent = value;
			}
		}

		/*
		 * Selected
		 */

		private bool _selected;

		/// <summary></summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary></summary>
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

		/*
		 * Visible
		 */

		private bool _visible = true;

		/// <summary></summary>
		[DefaultValue(true)]
		public bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				_visible = value;

				if (!_visible)
				{
					_bounds = Rectangle.Empty;
				}
			}
		}

		#endregion

		#region Properties.Internal

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

		#endregion

		#region Methods.Protected.Overridden

		/*
			 * ToString
			 */

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			return this.Text;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenNavigationButton"/> class.
		/// </summary>
		public NuGenNavigationButton()
		{
		}

		#endregion
	}
}
