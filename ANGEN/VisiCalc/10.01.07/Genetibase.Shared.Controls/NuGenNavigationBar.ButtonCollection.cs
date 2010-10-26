/* -----------------------------------------------
 * NuGenNavigationBar.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Collections;

using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;

namespace Genetibase.Shared.Controls
{
	partial class NuGenNavigationBar
	{
		private sealed class ButtonCollection : IList<NuGenNavigationButton>
		{
			#region IList<NuGenNavigationButton> Members

			NuGenNavigationButton IList<NuGenNavigationButton>.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			#endregion

			#region ICollection<NuGenNavigationButton> Members

			bool ICollection<NuGenNavigationButton>.IsReadOnly
			{
				get
				{
					return false;
				}
			}

			#endregion

			#region IEnumerable Members

			IEnumerator IEnumerable.GetEnumerator()
			{
				return _list.GetEnumerator();
			}

			#endregion

			#region Properties.Public

			public int Count
			{
				get
				{
					return _list.Count;
				}
			}

			#endregion

			#region Properties.Indexer

			public NuGenNavigationButton this[int index]
			{
				get
				{
					return _list[index];
				}
			}

			public NuGenNavigationButton this[string text]
			{
				get
				{
					foreach (NuGenNavigationButton button in _list)
					{
						if (button.Text == text)
						{
							return button;
						}
					}

					return null;
				}
			}

			public NuGenNavigationButton this[Point mouseLocation]
			{
				get
				{
					foreach (NuGenNavigationButton button in _list)
					{
						if (button.Bounds.Contains(mouseLocation))
						{
							return button;
						}
					}

					return null;
				}
			}

			public NuGenNavigationButton this[int x, int y]
			{
				get
				{
					return this[new Point(x, y)];
				}
			}

			#endregion

			#region Methods.Public

			public void Add(NuGenNavigationButton navigationButtonToAdd)
			{
				if (navigationButtonToAdd == null)
				{
					throw new ArgumentNullException("navigationButtonToAdd");
				}

				_list.Add(navigationButtonToAdd);
				_buttonBlock.OnNavigationButtonAdded(
					new NuGenCollectionEventArgs<NuGenNavigationButton>(_list.Count - 1, navigationButtonToAdd)
				);
			}

			public void AddRange(ButtonCollection navigationButtonsToAdd)
			{
				if (navigationButtonsToAdd == null)
				{
					throw new ArgumentNullException("navigationButtonsToAdd");
				}

				foreach (NuGenNavigationButton button in navigationButtonsToAdd)
				{
					this.Add(button);
				}
			}

			public bool Contains(NuGenNavigationButton navigationButton)
			{
				return _list.Contains(navigationButton);
			}

			public void Clear()
			{
				_list.Clear();
			}

			public void CopyTo(NuGenNavigationButton[] destination, int zeroBasedIndexToStartAt)
			{
				_list.CopyTo(destination, zeroBasedIndexToStartAt);
			}

			public IEnumerator<NuGenNavigationButton> GetEnumerator()
			{
				return _list.GetEnumerator();
			}

			public int GetVisibleCount()
			{
				int count = 0;

				foreach (NuGenNavigationButton button in _list)
				{
					if (button.Visible && button.Allowed)
					{
						count++;
					}
				}

				return count;
			}

			public int IndexOf(NuGenNavigationButton navigationButton)
			{
				return _list.IndexOf(navigationButton);
			}

			public void Insert(int index, NuGenNavigationButton navigationButtonToInsert)
			{
				if (navigationButtonToInsert == null)
				{
					throw new ArgumentNullException("navigationButtonToInsert");
				}

				_list.Insert(index, navigationButtonToInsert);
				_buttonBlock.OnNavigationButtonAdded(
					new NuGenCollectionEventArgs<NuGenNavigationButton>(index, navigationButtonToInsert)
				);
			}

			public bool Remove(NuGenNavigationButton navigationButtonToRemove)
			{
				int index = _list.IndexOf(navigationButtonToRemove);

				if (_list.Remove(navigationButtonToRemove))
				{
					_buttonBlock.OnNavigationButtonRemoved(
						new NuGenCollectionEventArgs<NuGenNavigationButton>(index, navigationButtonToRemove)
					);

					return true;
				}

				return false;
			}

			public void RemoveAt(int zeroBasedIndex)
			{
				_list.RemoveAt(zeroBasedIndex);
			}

			#endregion

			private List<NuGenNavigationButton> _list;
			private ButtonBlock _buttonBlock;

			public ButtonCollection(ButtonBlock buttonBlock)
			{
				if (buttonBlock == null)
				{
					throw new ArgumentNullException("buttonBlock");
				}

				_buttonBlock = buttonBlock;
				_list = new List<NuGenNavigationButton>();
			}
		}
	}
}
