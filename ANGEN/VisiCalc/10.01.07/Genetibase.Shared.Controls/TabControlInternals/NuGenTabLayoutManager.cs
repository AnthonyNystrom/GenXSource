/* -----------------------------------------------
 * NuGenTabLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.TabControlInternals
{
	/// <summary>
	/// </summary>
	public class NuGenTabLayoutManager : NuGenControlLayoutManager, INuGenTabLayoutManager
	{
		#region Declarations.Fields

		private static readonly int _closeButtonSize = 16;
		private static readonly int _offset = 5;
		private static readonly int _textOffset = 4;

		#endregion

		#region INuGenTabLayoutManager.RegisterLayoutBuilder

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="tabButtons"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public virtual NuGenTabLayoutBuilder RegisterLayoutBuilder(List<NuGenTabButton> tabButtons)
		{
			if (tabButtons == null)
			{
				throw new ArgumentNullException("tabButtons");
			}

			return new NuGenTabLayoutBuilder(tabButtons, new Size(250, 24));
		}

		#endregion

		#region INuGenTabLayoutManager.GetCloseButtonBounds

		/// <summary>
		/// </summary>
		public virtual Rectangle GetCloseButtonBounds(Rectangle bounds)
		{
			return new Rectangle(
				bounds.Right - (_closeButtonSize + 4),
				bounds.Top + (bounds.Height - _closeButtonSize) / 2,
				_closeButtonSize,
				_closeButtonSize
			);
		}

		#endregion

		#region INuGenTabLayoutManager.GetContentRectangle

		/// <summary>
		/// </summary>
		public virtual Rectangle GetContentRectangle(Rectangle clientRectangle, Rectangle closeButtonRectangle)
		{
			Rectangle contentRectangle = clientRectangle;

			if (closeButtonRectangle != Rectangle.Empty)
			{
				contentRectangle = Rectangle.FromLTRB(
					clientRectangle.Left,
					clientRectangle.Top,
					closeButtonRectangle.Right,
					clientRectangle.Bottom
				);
			}

			return Rectangle.Inflate(contentRectangle, -_offset, 0);
		}

		#endregion

		#region INuGenTabLayoutManager.GetTabPageBounds

		/// <summary>
		/// </summary>
		public virtual Rectangle GetTabPageBounds(Rectangle tabControlBounds, Rectangle tabStripBounds)
		{
			return new Rectangle(
				tabControlBounds.Left,
				tabControlBounds.Top + tabStripBounds.Height + 3,
				tabControlBounds.Width,
				tabControlBounds.Height - tabStripBounds.Bottom
			);
		}

		#endregion

		#region INuGenTabLayoutManager.GetTabStripBounds

		/// <summary>
		/// </summary>
		public virtual Rectangle GetTabStripBounds(Rectangle tabControlBounds)
		{
			return new Rectangle(
				tabControlBounds.Left + 2,
				tabControlBounds.Top + 3,
				tabControlBounds.Width - 6,
				24
			);
		}

		#endregion

		#region INuGenTabLayoutManager.GetTextBounds

		/// <summary>
		/// </summary>
		public new Rectangle GetTextBounds(NuGenBoundsParams textBoundsParams)
		{
			Rectangle textBounds = base.GetTextBounds(textBoundsParams);
			return Rectangle.Inflate(textBounds, -_textOffset, 0);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabLayoutManager"/> class.
		/// </summary>
		public NuGenTabLayoutManager()
		{
		}

		#endregion
	}
}
