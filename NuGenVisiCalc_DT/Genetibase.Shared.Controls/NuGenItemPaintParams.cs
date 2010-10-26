/* -----------------------------------------------
 * NuGenItemPaintParams.cs
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
	public class NuGenItemPaintParams : NuGenPaintParams
	{
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
		 * ContentAlign
		 */

		private ContentAlignment _contentAlign;

		/// <summary>
		/// </summary>
		public ContentAlignment ContentAlign
		{
			get
			{
				return _contentAlign;
			}
			set
			{
				_contentAlign = value;
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

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenItemPaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenItemPaintParams(Graphics g)
			: base(g)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenItemPaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="initializeFrom"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenItemPaintParams(NuGenPaintParams initializeFrom)
			: base(initializeFrom)
		{
		}
	}
}
