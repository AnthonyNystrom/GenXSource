/* -----------------------------------------------
 * NuGenTabLayoutBuilder.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Controls
{
	/// <summary>
	/// Provides helper methods to build tab buttons and pages layout.
	/// </summary>
	public class NuGenTabLayoutBuilder
	{
		#region Declarations.Fields

		private static Rectangle _selectedTabButtonOffset = new Rectangle(-2, -2, 4, 4);

		#endregion

		#region Properties.Protected

		/*
		 * DefaultTabButtonSize
		 */

		private Size _defaultTabButtonSize = Size.Empty;

		/// <summary>
		/// </summary>
		protected Size DefaultTabButtonSize
		{
			get
			{
				return _defaultTabButtonSize;
			}
		}

		/*
		 * TabButtons
		 */

		private List<NuGenTabButton> _tabButtons = null;

		/// <summary>
		/// </summary>
		protected List<NuGenTabButton> TabButtons
		{
			get
			{
				return _tabButtons;
			}
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// </summary>
		public void RebuildLayout(Rectangle tabStripBounds)
		{
			if (tabStripBounds == Rectangle.Empty)
			{
				return;
			}

			NuGenTabButton selectedTabButton = null;
			int currentLeftOffset = tabStripBounds.Left;
			int defaultWidth = this.TabButtons.Count * this.DefaultTabButtonSize.Width;
			int tabButtonWidth = this.DefaultTabButtonSize.Width;

			if (defaultWidth > tabStripBounds.Width)
			{
				tabButtonWidth = tabStripBounds.Width / this.TabButtons.Count;
			}

			foreach (NuGenTabButton tabButton in this.TabButtons)
			{
				tabButton.Width = tabButtonWidth;
				tabButton.Height = tabStripBounds.Height;
				tabButton.Top = tabStripBounds.Top;
				tabButton.Left = currentLeftOffset;
				
				currentLeftOffset += tabButton.Width;

				if (tabButton.Selected)
				{
					Debug.Assert(selectedTabButton == null, "selectedTabButton == null");
					selectedTabButton = tabButton;
				}
			}

			/* Selected */

			if (selectedTabButton != null && selectedTabButton.Enabled)
			{
				selectedTabButton.Top += _selectedTabButtonOffset.Top;
				selectedTabButton.Left += _selectedTabButtonOffset.Left;
				selectedTabButton.Width += _selectedTabButtonOffset.Width;
				selectedTabButton.Height += _selectedTabButtonOffset.Height;

				selectedTabButton.BringToFront();
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabLayoutBuilder"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///	<para>
		///		<paramref name="tabButtons"/> is <see langword="null"/>.
		///	</para>
		/// </exception>
		public NuGenTabLayoutBuilder(List<NuGenTabButton> tabButtons, Size defaultTabButtonSize)
		{
			if (tabButtons == null)
			{
				throw new ArgumentNullException("tabButtons");
			}

			_defaultTabButtonSize = defaultTabButtonSize;
			_tabButtons = tabButtons;
		}

		#endregion
	}
}
