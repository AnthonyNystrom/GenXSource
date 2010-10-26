/* -----------------------------------------------
 * NuGenLinearGradientColorTable.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Genetibase.UI.NuGenInterface.Rendering
{
	/// <summary>
	/// Encapsulates linear gradient parameters.
	/// </summary>
	public class NuGenLinearGradientColorTable
	{
		#region Properties.Public

		/*
		 * EndColor
		 */

		private Color _EndColor;

		/// <summary>
		/// Gets or sets gradient end color.
		/// </summary>
		public Color EndColor
		{
			get
			{
				return _EndColor;
			}
			set
			{
				_EndColor = value;
			}
		}

		/*
		 * GradientAngle
		 */

		private int _GradientAngle;

		/// <summary>
		/// Gets or sets gradient angle.
		/// </summary>
		public int GradientAngle
		{
			get
			{
				return _GradientAngle;
			}
			set
			{
				_GradientAngle = value;
			}
		}

		/*
		 * IsEmpty
		 */

		/// <summary>
		/// Gets the value indicating whether this
		/// <see cref="T:Genetibase.UI.NuGenInterface.Rendering.NuGenLinearGradientColorTable"/>
		/// is not initialized.
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return this.StartColor.IsEmpty && this.EndColor.IsEmpty;
			}
		}

		/*
		 * StartColor
		 */

		private Color _StartColor;

		/// <summary>
		/// Gets or sets gradient start color.
		/// </summary>
		public Color StartColor
		{
			get
			{
				return _StartColor;
			}
			set
			{
				_StartColor = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLinearGradientColorTable"/> class.
		/// </summary>
		public NuGenLinearGradientColorTable()
			: this(Color.Empty, Color.Empty)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLinearGradientColorTable"/> class.
		/// </summary>
		/// <param name="startColor">Specifies gradient start color.</param>
		/// <param name="endColor">Specifies gradient end color.</param>
		public NuGenLinearGradientColorTable(Color startColor, Color endColor)
			: this(startColor, endColor, 90)
		{	
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLinearGradientColorTable"/> class.
		/// </summary>
		/// <param name="startColorString">Specifies start color in the #FFFFFF format.</param>
		/// <param name="endColorString">Specifies end color in the #FFFFFF format.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="startColorString"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="endColorString"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="startColorString"/> is an empty string.
		/// -or-
		/// <paramref name="endColorString"/> is an empty string.
		/// </exception>
		/// <exception cref="T:System.FormatException">
		/// <paramref name="startColorString"/> is not in the correct format
		/// -or-
		/// <paramref name="endColorString"/> is not in the correct format.
		/// </exception>
		public NuGenLinearGradientColorTable(string startColorString, string endColorString)
		{
			if (string.IsNullOrEmpty(startColorString))
			{
				throw new ArgumentNullException("startColorString");
			}

			if (string.IsNullOrEmpty(endColorString))
			{
				throw new ArgumentNullException("endColorString");
			}

			this.StartColor = ColorTranslator.FromHtml(startColorString);
			_EndColor = ColorTranslator.FromHtml(endColorString);

			this.GradientAngle = 90;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLinearGradientColorTable"/> class.
		/// </summary>
		/// <param name="startColor">The start color.</param>
		/// <param name="endColor">The end color.</param>
		/// <param name="gradientAngle">The gradient angle.</param>
		public NuGenLinearGradientColorTable(Color startColor, Color endColor, int gradientAngle)
		{
			this.StartColor = startColor;
			this.EndColor = endColor;
			this.GradientAngle = gradientAngle;
		}

		#endregion
	}
}
