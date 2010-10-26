/* -----------------------------------------------
 * NuGenScrollEventArgs.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Encapsulates event parameters.
	/// </summary>
	public class NuGenScrollEventArgs
	{
		#region Properties

		/*
		 * ScrollBarStyle
		 */

		/// <summary>
		/// Determines the style of the scroll bar to fire the event.
		/// </summary>
		private NuGenScrollBarStyle scrollBarStyle;

		/// <summary>
		/// Gets the style of the scroll bar to fire the event.
		/// </summary>
		public NuGenScrollBarStyle ScrollBarStyle
		{
			get { return this.scrollBarStyle; }
		}

		/*
		 * ScrollPos
		 */

		/// <summary>
		/// Determines the current position of the scroll box.
		/// </summary>
		private int scrollPos = 0;

		/// <summary>
		/// Gets the current position of the scroll box.
		/// </summary>
		public int ScrollPos
		{
			get { return this.scrollPos; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenScrollEventArgs"/> class.
		/// </summary>
		/// <param name="scrollBarStyle">The style of the scroll bar to fire the event.</param>
		/// <param name="scrollPos">The scroll box position.</param>
		public NuGenScrollEventArgs(NuGenScrollBarStyle scrollBarStyle, int scrollPos)
		{
			this.scrollBarStyle = scrollBarStyle;
			this.scrollPos = scrollPos;
		}

		#endregion
	}
}
