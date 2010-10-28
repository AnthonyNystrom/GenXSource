#region Copyright 2001-2006 Christoph Daniel Rüegg [GNU Public License]
/*
NeuroBox, a library for neural network generation, propagation and training
Copyright (c) 2001-2006, Christoph Daniel Rueegg, http://cdrnet.net/. All rights reserved.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
*/
#endregion

using System;
using NeuroBox;

namespace NeuroBox.PatternMatching
{
	/// <summary>
	/// Event Arguments for events refering to a pattern.
	/// </summary>
	public class PatternEventArgs: EventArgs
	{
		Pattern pattern;
		/// <summary>
		/// Instanciates a new event arg.
		/// </summary>
		/// <param name="pattern">The pattern to refer to.</param>
		public PatternEventArgs(Pattern pattern)
		{
			this.pattern = pattern;
		}
		/// <summary>
		/// The pattern this event refers to.
		/// </summary>
		public Pattern Pattern
		{
			get {return this.pattern;}
		}
	}

	/// <summary>
	/// Event Arguments for events refering to a pattern and its position.
	/// </summary>
	public class PatternPositionEventArgs: EventArgs
	{
		Pattern pattern;
		int position;
		/// <summary>
		/// Instanciates a new event arg.
		/// </summary>
		/// <param name="pattern">The pattern to refer to.</param>
		/// <param name="position">The position of the pattern.</param>
		public PatternPositionEventArgs(Pattern pattern, int position)
		{
			this.pattern = pattern;
			this.position = position;
		}
		/// <summary>
		/// The pattern this event refers to.
		/// </summary>
		public Pattern Pattern
		{
			get {return pattern;}
		}
		/// <summary>
		/// The position of the pattern.
		/// </summary>
		public int Position
		{
			get {return position;}
		}
	}
}
