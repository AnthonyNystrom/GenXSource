/* -----------------------------------------------
 * NuGenItemParams.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
		 * Sender
		 */

		private object _sender;

		/// <summary>
		/// </summary>
		public object Sender
		{
			get
			{
				return _sender;
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
		///		<paramref name="sender"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenItemParams(object sender, Graphics g, Rectangle bounds, DrawItemState state)
		{
			if (sender == null)
			{
				throw new ArgumentNullException("sender");
			}

			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			_sender = sender;
			_graphics = g;
			_bounds = bounds;
			_state = state;
		}

		#endregion
	}
}
