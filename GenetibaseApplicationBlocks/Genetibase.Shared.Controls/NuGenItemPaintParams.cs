/* -----------------------------------------------
 * NuGenItemPaintParams.cs
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
	public class NuGenItemPaintParams : NuGenPaintParams
	{
		#region Propeties.Public

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

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenItemPaintParams"/> class.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="g"></param>
		/// <param name="bounds"></param>
		/// <param name="state"></param>
		/// <exception cref="ArgumentNullException">
		/// 	<para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="sender"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenItemPaintParams(
			object sender,
			Graphics g,
			Rectangle bounds,
			NuGenControlState state
			)
			: base(sender, g, bounds, state)
		{
		}

		#endregion
	}
}
