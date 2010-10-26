/* -----------------------------------------------
 * TextViewInputEventArgs.cs
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
	public class TextViewInputEventArgs : EventArgs
	{
		/// <summary>
		/// </summary>
		public Boolean Handled
		{
			get;
			set;
		}

		/// <summary>
		/// </summary>
		public TextInputState State
		{
			get;
			private set;
		}

		/// <summary>
		/// </summary>
		public String Text
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextViewInputEventArgs"/> class.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Specified <paramref name="state"/> does not belong to <see cref="TextInputState"/>.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="text"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Specified <paramref name="text"/> cannot be zero-length if <paramref name="state"/> = <see cref="F:Genetibase.Windows.Controls.Editor.Text.View.TextInputState.Provisional"/>.
		/// </exception>
		public TextViewInputEventArgs(TextInputState state, String text)
		{
			if ((state != TextInputState.Provisional) && (state != TextInputState.Final))
			{
				throw new ArgumentOutOfRangeException("state");
			}

			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			
			if ((state == TextInputState.Provisional) && (text.Length == 0))
			{
				throw new ArgumentException("Provisional TextInput can not be zero-length.");
			}
			
			this.State = state;
			this.Text = text;
		}
	}
}
