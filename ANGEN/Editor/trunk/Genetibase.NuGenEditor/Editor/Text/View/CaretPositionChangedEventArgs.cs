/* -----------------------------------------------
 * CaretPositionChangedEventArgs.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Windows.Controls.Editor.Text.View
{
	/// <summary>
	/// </summary>
	public class CaretPositionChangedEventArgs : EventArgs
	{
		/// <summary>
		/// </summary>
		public ICaretPosition NewPosition
		{
			get;
			private set;
		}

		/// <summary>
		/// </summary>
		public ICaretPosition OldPosition
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CaretPositionChangedEventArgs"/> class.
		/// </summary>
		public CaretPositionChangedEventArgs(ICaretPosition oldPosition, ICaretPosition newPosition)
		{
			this.OldPosition = oldPosition;
			this.NewPosition = newPosition;
		}
	}
}
