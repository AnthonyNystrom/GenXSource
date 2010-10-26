/* -----------------------------------------------
 * NuGenTabButtonPaintParams.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace Genetibase.Shared.Controls.TabControlInternals
{
	/// <summary>
	/// </summary>
	public class NuGenTabButtonPaintParams
	{
		#region Properties.Public

		/*
		 * Bounds
		 */

		private Rectangle _bounds = Rectangle.Empty;

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
		 * Font
		 */

		private Font _font;

		/// <summary>
		/// </summary>
		public Font Font
		{
			get
			{
				return _font;
			}
			set
			{
				_font = value;
			}
		}

		/*
		 * ForeColor
		 */

		private Color _foreColor = SystemColors.ControlText;

		/// <summary>
		/// </summary>
		public Color ForeColor
		{
			get
			{
				return _foreColor;
			}
			set
			{
				_foreColor = value;
			}
		}

		/*
		 * Graphics
		 */

		private Graphics _graphics = null;

		/// <summary>
		/// </summary>
		public Graphics Graphics
		{
			get
			{
				return _graphics;
			}
		}

		/*
		 * Image
		 */

		private Image _image = null;

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
				_image = value;
			}
		}

		/*
		 * ImageBounds
		 */

		private Rectangle _imageBounds = Rectangle.Empty;

		/// <summary>
		/// </summary>
		public Rectangle ImageBounds
		{
			get
			{
				return _imageBounds;
			}
			set
			{
				_imageBounds = value;
			}
		}

		/*
		 * State
		 */

		private TabItemState _state = TabItemState.Normal;

		/// <summary>
		/// </summary>
		public TabItemState State
		{
			get
			{
				return _state;
			}
			set
			{
				_state = value;
			}
		}

		/*
		 * Text
		 */

		private string _text = "";

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

		/*
		 * TextBounds
		 */

		private Rectangle _textBounds = Rectangle.Empty;

		/// <summary>
		/// </summary>
		public Rectangle TextBounds
		{
			get
			{
				return _textBounds;
			}
			set
			{
				_textBounds = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabButtonPaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenTabButtonPaintParams(Graphics g)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			_graphics = g;
		}

		#endregion
	}
}
