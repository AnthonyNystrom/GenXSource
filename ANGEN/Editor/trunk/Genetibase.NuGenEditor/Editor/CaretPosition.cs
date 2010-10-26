/* -----------------------------------------------
 * CaretPosition.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Windows.Controls.Editor.Text.View;
using System.Globalization;

namespace Genetibase.Windows.Controls.Editor
{
	internal class CaretPosition : ICaretPosition
	{
		private Int32 _characterIndex;
		private CaretPlacement _placement;
		private Int32 _textInsertionIndex;

		internal CaretPosition(Int32 characterIndex, Int32 textInsertionIndex, CaretPlacement caretPlacement)
		{
			if (characterIndex < 0)
			{
				throw new ArgumentOutOfRangeException("characterIndex");
			}
			if (textInsertionIndex < 0)
			{
				throw new ArgumentOutOfRangeException("textInsertionIndex");
			}
			_characterIndex = characterIndex;
			_textInsertionIndex = textInsertionIndex;
			_placement = caretPlacement;
		}

		public override Boolean Equals(Object obj)
		{
			if (obj != null)
			{
				CaretPosition position = obj as CaretPosition;
				if (position == null)
				{
					return false;
				}
				if ((position.CharacterIndex == this.CharacterIndex) && (position.TextInsertionIndex == this.TextInsertionIndex))
				{
					return (position.Placement == this.Placement);
				}
			}
			return false;
		}

		public override Int32 GetHashCode()
		{
			return ((this.CharacterIndex.GetHashCode() * this.TextInsertionIndex.GetHashCode()) * this.Placement.GetHashCode());
		}

		public override String ToString()
		{
			if (this.Placement == CaretPlacement.LeftOfCharacter)
			{
				return String.Format(CultureInfo.InvariantCulture, "|{0}", new Object[] { this.CharacterIndex });
			}
			return String.Format(CultureInfo.InvariantCulture, "{0}|", new Object[] { this.CharacterIndex });
		}

		public Int32 CharacterIndex
		{
			get
			{
				return _characterIndex;
			}
		}

		public CaretPlacement Placement
		{
			get
			{
				return _placement;
			}
		}

		public Int32 TextInsertionIndex
		{
			get
			{
				return _textInsertionIndex;
			}
		}
	}
}
