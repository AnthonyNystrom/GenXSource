/* -----------------------------------------------
 * NuGenItemParams.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	internal class NuGenItemParams
	{
		#region Properties.Public

		/*
		 * BackgroundColor
		 */

		private Color _backgroundColor;

		/// <summary>
		/// </summary>
		public Color BackgroundColor
		{
			get
			{
				return _backgroundColor;
			}
			set
			{
				_backgroundColor = value;
			}
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

		private Color _foreColor;

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
				_image = value;
			}
		}

		/*
		 * Graphics
		 */

		private Graphics _graphics;

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
		 * State
		 */

		private DrawItemState _state;

		/// <summary>
		/// </summary>
		public DrawItemState State
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
		 * RightToLeft
		 */

		private RightToLeft _rightToLeft;

		/// <summary>
		/// </summary>
		public RightToLeft RightToLeft
		{
			get
			{
				return _rightToLeft;
			}
			set
			{
				_rightToLeft = value;
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

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenItemParams"/> struct.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenItemParams(Graphics g)
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
