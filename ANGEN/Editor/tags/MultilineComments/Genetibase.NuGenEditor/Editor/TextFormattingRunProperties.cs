/* -----------------------------------------------
 * TextFormattingRunProperties.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace Genetibase.Windows.Controls.Editor
{
	/// <summary>
	/// </summary>
	public class TextFormattingRunProperties : TextRunProperties
	{
		#region Properties.Public

		/// <summary>
		/// </summary>
		public override Brush BackgroundBrush
		{
			get
			{
				return _backgroundBrush;
			}
		}

		/// <summary>
		/// Gets the culture information for the text run.
		/// </summary>
		/// <value></value>
		/// <returns>A value of <see cref="T:System.Globalization.CultureInfo"/> that represents the culture of the text run.</returns>
		public override CultureInfo CultureInfo
		{
			get
			{
				return _cultureInfo;
			}
		}

		/// <summary>
		/// </summary>
		public static TextFormattingRunProperties DefaultProperties
		{
			get
			{
				return _defaultProperties;
			}
		}

		/// <summary>
		/// Gets the text size in points, which is then used for font hinting.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Double"/> that represents the text size in points. The default is 12 pt.</returns>
		public override Double FontHintingEmSize
		{
			get
			{
				return _hintingSize;
			}
		}

		/// <summary>
		/// Gets the text size in points for the text run.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Double"/> that represents the text size in points. The default is 12 pt.</returns>
		public override Double FontRenderingEmSize
		{
			get
			{
				return _size;
			}
		}

		/// <summary>
		/// Gets the brush that is used to paint the foreground color of the text run.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Media.Brush"/> value that represents the foreground color.</returns>
		public override Brush ForegroundBrush
		{
			get
			{
				return _foregroundBrush;
			}
		}

		/// <summary>
		/// </summary>
		public Color ForegroundColor
		{
			get
			{
				return _foreground;
			}
		}

		/// <summary>
		/// Gets the collection of  <see cref="T:System.Windows.TextDecoration"/> objects used for the text run.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.TextDecorationCollection"/> value.</returns>
		public override TextDecorationCollection TextDecorations
		{
			get
			{
				return _textDecorations;
			}
		}

		/// <summary>
		/// Gets the collection of <see cref="T:System.Windows.Media.TextEffect"/> objects used for the text run.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Media.TextEffectCollection"/> value.</returns>
		public override TextEffectCollection TextEffects
		{
			get
			{
				return _textEffects;
			}
		}

		/// <summary>
		/// Gets the typeface for the text run.
		/// </summary>
		/// <value></value>
		/// <returns>A value of <see cref="T:System.Windows.Media.Typeface"/>.</returns>
		public override Typeface Typeface
		{
			get
			{
				return _typeface;
			}
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// </summary>
		public Boolean IsEquivalent(TextFormattingRunProperties other)
		{
			if ((!_altered && !other._altered) && (_typeface.Equals(other._typeface) && (_size == other._size)))
			{
				return (_foreground == other._foreground);
			}
			return false;
		}

		/// <summary>
		/// </summary>
		public Boolean SameSize(TextFormattingRunProperties other)
		{
			if (!_altered)
			{
				return (_size == other._size);
			}
			return false;
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///		<para><paramref name="brush"/> is <see langword="null"/>.</para>
		///	</exception>
		public void SetBackgroundBrush(Brush brush)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			_backgroundBrush = brush;
			_altered = true;
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///		<para><paramref name="cultureInfo"/> is <see langword="null"/>.</para>
		/// </exception>
		public void SetCultureInfo(CultureInfo cultureInfo)
		{
			if (cultureInfo == null)
			{
				throw new ArgumentNullException("cultureInfo");
			}
			_cultureInfo = cultureInfo;
			_altered = true;
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <para><paramref name="hintingSize"/> is <see langword="null"/>.</para>
		/// </exception>
		public void SetFontHintingEmSize(Double hintingSize)
		{
			if (hintingSize < 0)
			{
				throw new ArgumentOutOfRangeException("hintingSize");
			}
			_hintingSize = hintingSize;
			_altered = true;
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<para><paramref name="renderingSize"/> is <see langword="null"/>.</para>
		///	</exception>
		public void SetFontRenderingEmSize(Double renderingSize)
		{
			if (renderingSize < 0)
			{
				throw new ArgumentOutOfRangeException("renderingSize");
			}
			_size = renderingSize;
			_altered = true;
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="brush"/> is <see langword="null"/>.</para>
		/// </exception>
		public void SetForegroundBrush(Brush brush)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			_foregroundBrush = brush;
			_altered = true;
		}

		/// <summary>
		/// </summary>
		public void SetForegroundColor(Color color)
		{
			_foreground = color;
			_foregroundBrush = new SolidColorBrush(color);
			_altered = true;
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="textDecorations"/> is <see langword="null"/>.</para>
		/// </exception>
		public void SetTextDecorations(TextDecorationCollection textDecorations)
		{
			if (textDecorations == null)
			{
				throw new ArgumentNullException("textDecorations");
			}
			_textDecorations = textDecorations;
			_altered = true;
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="textEffects"/> is <see langword="null"/>.</para>
		/// </exception>
		public void SetTextEffects(TextEffectCollection textEffects)
		{
			if (textEffects == null)
			{
				throw new ArgumentNullException("textEffects");
			}
			_textEffects = textEffects;
			_altered = true;
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// </exception>
		public void SetTypeface(Typeface typeface)
		{
			if (typeface == null)
			{
				throw new ArgumentNullException("typeface");
			}
			_typeface = typeface;
			_altered = true;
		}

		#endregion

		private Boolean _altered;
		private Brush _backgroundBrush;
		private CultureInfo _cultureInfo;
		private static TextFormattingRunProperties _defaultProperties = new TextFormattingRunProperties(new Typeface("Lucida console"), 12, Colors.Black);
		private Color _foreground;
		private Brush _foregroundBrush;
		private Double _hintingSize;
		private Double _size;
		private TextDecorationCollection _textDecorations;
		private TextEffectCollection _textEffects;
		private Typeface _typeface;

		/// <summary>
		/// Initializes a new instance of the <see cref="TextFormattingRunProperties"/> class.
		/// </summary>
		public TextFormattingRunProperties(Typeface typeface, Double size, Color foreground)
		{
			_typeface = typeface;
			_size = _hintingSize = size;
			_foreground = foreground;
			_foregroundBrush = new SolidColorBrush(foreground);
			_backgroundBrush = Brushes.Transparent;
			_textDecorations = new TextDecorationCollection();
			_textEffects = new TextEffectCollection();
			_cultureInfo = CultureInfo.CurrentCulture;
			if (_foregroundBrush.CanFreeze)
			{
				_foregroundBrush.Freeze();
			}
			if (_backgroundBrush.CanFreeze)
			{
				_backgroundBrush.Freeze();
			}
			if (_textEffects.CanFreeze)
			{
				_textEffects.Freeze();
			}
			if (_textDecorations.CanFreeze)
			{
				_textDecorations.Freeze();
			}
		}
	}
}
