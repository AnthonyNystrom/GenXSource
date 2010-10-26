/* -----------------------------------------------
 * TextChange.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Genetibase.Shared;

namespace Genetibase.Windows.Controls.Data.Text
{
	/// <summary>
	/// </summary>
	public class TextChange
	{
		#region Properties

		/*
		 * Delta
		 */

		/// <summary>
		/// </summary>
		public Int32 Delta
		{
			get
			{
				return (this.NewText.Length - this.OldText.Length);
			}
		}

		/*
		 * NewEnd
		 */

		/// <summary>
		/// </summary>
		public Int32 NewEnd
		{
			get
			{
				return (this.Position + this.NewText.Length);
			}
		}

		/*
		 * NewText
		 */

		private String _newText;

		/// <summary>
		/// </summary>
		public String NewText
		{
			get
			{
				return _newText;
			}
			private set
			{
				_newText = value;
			}
		}

		/*
		 * OldEnd
		 */

		/// <summary>
		/// </summary>
		public Int32 OldEnd
		{
			get
			{
				return (this.Position + this.OldText.Length);
			}
		}

		/*
		 * OldText
		 */

		private String _oldText;

		/// <summary>
		/// </summary>
		public String OldText
		{
			get
			{
				return _oldText;
			}
			private set
			{
				_oldText = value;
			}
		}

		/*
		 * Position
		 */

		private NuGenNonNegativeInt32 _positionInternal;

		private NuGenNonNegativeInt32 PositionInternal
		{
			get
			{
				if (_positionInternal == null)
				{
					_positionInternal = new NuGenNonNegativeInt32();
				}

				return _positionInternal;
			}
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <paramref name="value"/> should be non-negative.
		/// </exception>
		public Int32 Position
		{
			get
			{
				return this.PositionInternal.Value;
			}
			private set
			{
				this.PositionInternal.Value = value;
			}
		}

		#endregion

		#region Methods

		/*
		 * Catenate
		 */

		private static void Catenate(ref String left, String right)
		{
			if (right.Length > 0)
			{
				left = (left.Length == 0) ? right : (left + right);
			}
		}

		/*
		 * Merge
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="normalizedChanges"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="buffer"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The amount of items in <paramref name="normalizedChanges"/> is less than 1.
		/// </exception>
		public static TextChange Merge(IList<TextChange> normalizedChanges, TextBuffer buffer)
		{
			if (normalizedChanges == null)
			{
				throw new ArgumentNullException("normalizedChanges");
			}

			if (normalizedChanges.Count < 1)
			{
				throw new ArgumentOutOfRangeException("normalizedChanges.Count");
			}

			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}

			Int32 position = normalizedChanges[0].Position;
			var builder = new StringBuilder();
			var builder2 = new StringBuilder();

			for (var i = 0; i < normalizedChanges.Count; i++)
			{
				TextChange change = normalizedChanges[i];
				builder.Append(change.OldText);
				builder2.Append(change.NewText);

				if ((i + 1) < normalizedChanges.Count)
				{
					Int32 length = normalizedChanges[i + 1].Position - normalizedChanges[i].NewEnd;
					String text = buffer.GetText(normalizedChanges[i].NewEnd, length);
					builder.Append(text);
					builder2.Append(text);
				}
			}

			return new TextChange(position, builder.ToString(), builder2.ToString());
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="changes"/> is <see langword="null"/>.</para>
		/// </exception>
		public static IList<TextChange> Normalize(IList<TextChange> changes)
		{
			if (changes == null)
			{
				throw new ArgumentNullException("changes");
			}

			if (changes.Count == 1)
			{
				return changes;
			}

			TextChange[] changeArray = StableSort(changes);
			var num = 0;
			var index = 0;
			var num3 = 1;

			while (num3 < changeArray.Length)
			{
				TextChange change = changeArray[index];
				TextChange change2 = changeArray[num3];
				Int32 num4 = change2.Position - change.OldEnd;

				if (num4 > 0)
				{
					change.Position += num;
					num += change.Delta;
					index = num3;
					num3 = index + 1;
				}
				else
				{
					Catenate(ref change._newText, change2.NewText);

					if (num4 == 0)
					{
						Catenate(ref change._oldText, change2.OldText);
					}
					else if (change.OldEnd < change2.OldEnd)
					{
						var startIndex = change.OldEnd - change2.Position;
						Catenate(ref change._oldText, change2.OldText.Substring(startIndex));
					}

					changeArray[num3] = null;
					num3++;
				}
			}

			TextChange textChange = changeArray[index];
			textChange.Position += num;

			IList<TextChange> normalizedList = new List<TextChange>();

			foreach (var item in changeArray)
			{
				if (item != null)
				{
					normalizedList.Add(item);
				}
			}

			return normalizedList;
		}

		/*
		 * StableSort
		 */

		private static TextChange[] StableSort(IList<TextChange> changes)
		{
			var array = new TextChange[changes.Count];
			changes.CopyTo(array, 0);

			for (var i = 0; i < array.Length - 1; i++)
			{
				Int32 position = array[i].Position;
				var index = i;

				for (var j = i + 1; j < array.Length; j++)
				{
					if (array[j].Position < position)
					{
						position = array[j].Position;
						index = j;
					}
				}

				NuGenArgument.Exchange<TextChange>(ref array[i], ref array[index]);
			}

			return array;
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="TextChange"/> class.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <para><paramref name="position"/> should be non-negative.</para>
		/// </exception>
		public TextChange(Int32 position, String oldText, String newText)
		{
			this.Position = position;
			this.OldText = oldText == null ? "" : oldText;
			this.NewText = newText == null ? "" : newText;
		}
	}
}
