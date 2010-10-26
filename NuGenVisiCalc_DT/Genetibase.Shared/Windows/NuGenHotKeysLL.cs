/* -----------------------------------------------
 * NuGenHotKeysLL.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Exposes <see cref="NuGenHotKeys"/> functionality to process the key codes retrieved using a low-level windows hook.
	/// </summary>
	public class NuGenHotKeysLL : NuGenHotKeys
	{
		private IList<Keys> _sequence;

		/// <summary>
		/// </summary>
		public void KeyDown(Keys keyCode)
		{
			_sequence.Add(keyCode);
			Keys keySequence = Keys.None;

			foreach (Keys key in _sequence)
			{
				keySequence |= key;
			}

			this.Process(keySequence);
		}

		/// <summary>
		/// </summary>
		public void KeyUp(Keys keyCode)
		{
			_sequence.Clear();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenHotKeysLL"/> class.
		/// </summary>
		public NuGenHotKeysLL()
		{
			_sequence = new List<Keys>();
		}
	}
}
