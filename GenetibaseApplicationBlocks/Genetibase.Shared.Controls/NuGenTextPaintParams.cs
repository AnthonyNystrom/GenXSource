/* -----------------------------------------------
 * NuGenTextPaintParams.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public class NuGenTextPaintParams : NuGenPaintParams
	{
		#region Properties.Public

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

		/*
		 * TextAlign
		 */

		private ContentAlignment _textAlign;

		/// <summary>
		/// </summary>
		public ContentAlignment TextAlign
		{
			get
			{
				return _textAlign;
			}
			set
			{
				_textAlign = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTextPaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="sender"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="text"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenTextPaintParams(object sender, Graphics g, Rectangle bounds, NuGenControlState state, string text)
			: base(sender, g, bounds, state)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}

			_text = text;
		}

		#endregion
	}
}
