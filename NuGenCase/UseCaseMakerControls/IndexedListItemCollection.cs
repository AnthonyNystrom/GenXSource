using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace UseCaseMakerControls
{
	/// <summary>
	/// Descrizione di riepilogo per IndexedListItemCollection.
	/// </summary>
	public class IndexedListItemCollection : IList, ICollection, IEnumerable
	{
		#region Private Enumerators and Constants
		#endregion

		#region Public Enumerators and Constants
		#endregion

		#region Class Members
		private ArrayList items = new ArrayList();
		private IndexedList parent = null;
		#endregion

		#region Constructors
		public IndexedListItemCollection(IndexedList parent)
		{
			this.parent = parent;
		}
		#endregion

		#region Events
		#endregion

		#region Public Properties
		/// <summary>
		/// Returns the number of elements in the MenuItemCollection
		/// </summary>
		[ Browsable(false) ]
		public int Count
		{
			get
			{
				return items.Count;
			}
		}

		[ Browsable(false) ]
		public bool IsSynchronized
		{
			get
			{
				return items.IsSynchronized;
			}
		}

		[ Browsable(false) ]
		public object SyncRoot
		{
			get
			{
				return items.SyncRoot;
			}
		}

		public IndexedListItem this[int index]
		{
			get
			{
				return (IndexedListItem) items[index];
			}
			set
			{
				items[index] = value;
			}
		}

		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (IndexedListItem)value;
			}
		}

		[ Browsable(false) ]
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		[ Browsable(false) ]
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}
		#endregion

		#region Public Methods
		public int Add(IndexedListItem item)
		{
			Guid GUID = Guid.NewGuid();
			
			item.ItemLabel.Name = "index_" + GUID.ToString();
			item.ItemLabel.BorderStyle = BorderStyle.None;
			item.ItemLabel.BackColor = this.parent.IndexBackColor;
			item.ItemLabel.Click += new EventHandler(parent.OnItemLabelClick);

			item.ItemRichTextBox.CaseSensitive = true;
			item.ItemRichTextBox.Name = "text_" + GUID.ToString();
			item.ItemRichTextBox.BorderStyle = BorderStyle.None;
			item.ItemRichTextBox.BackColor = this.parent.TextBackColor;
			item.ItemRichTextBox.GotFocus += new EventHandler(parent.OnItemRichTextBoxClick);

			item.ItemRichTextBox.Separators = this.parent.Separators;
			item.ItemRichTextBox.HighlightDescriptors = this.parent.HighlightDescriptors;

			item.SelectedChange += new SelectedChangeEventHandler(item_SelectedChange);
			item.MouseOverToken += new MouseOverTokenEventHandler(parent.OnMouseOverToken);
			item.ItemTextChanged += new ItemTextChangedEventHandler(parent.OnItemTextChanged);
			item.ItemClick += new ItemClickEventHandler(parent.OnItemClick);
			item.ItemTextEnter += new ItemTextEnterEventHandler(parent.OnItemTextEnter);
			item.ItemTextSelectionChanged += new ItemTextSelectionChangedEventHandler(parent.OnItemTextSelectionChanged);

			parent.Controls.Add(item.ItemLabel);
			parent.Controls.Add(item.ItemRichTextBox);

			int result = items.Add(item);

			parent.PerformLayout();

			return result;
		}

		int IList.Add(object item)
		{
			return this.Add((IndexedListItem)item);
		}

		public void Clear()
		{
			items.Clear();
			parent.Controls.Clear();
			parent.PerformLayout();
		}

		public bool Contains(IndexedListItem item)
		{
			return items.Contains(item);
		}

		bool IList.Contains(object item)
		{
			return this.Contains((IndexedListItem)item);
		}

		public int IndexOf(IndexedListItem item)
		{
			return items.IndexOf(item);
		}

		int IList.IndexOf(object item)
		{
			return this.IndexOf((IndexedListItem)item);
		}

		public void Insert(int index, IndexedListItem item)
		{
			Guid GUID = Guid.NewGuid();
			
			item.ItemLabel.Name = "index_" + GUID.ToString();
			item.ItemLabel.BorderStyle = BorderStyle.None;
			item.ItemLabel.BackColor = parent.IndexBackColor;
			item.ItemLabel.Click += new EventHandler(parent.OnItemLabelClick);

			item.ItemRichTextBox.CaseSensitive = true;
			item.ItemRichTextBox.Name = "text_" + GUID.ToString();
			item.ItemRichTextBox.BorderStyle = BorderStyle.None;
			item.ItemRichTextBox.BackColor = parent.TextBackColor;
			item.ItemRichTextBox.GotFocus += new EventHandler(parent.OnItemRichTextBoxClick);

			item.ItemRichTextBox.Separators = this.parent.Separators;
			item.ItemRichTextBox.HighlightDescriptors = this.parent.HighlightDescriptors;

			item.SelectedChange += new SelectedChangeEventHandler(item_SelectedChange);
			item.MouseOverToken += new MouseOverTokenEventHandler(parent.OnMouseOverToken);
			item.ItemTextChanged += new ItemTextChangedEventHandler(parent.OnItemTextChanged);

			parent.Controls.Add(item.ItemLabel);
			parent.Controls.Add(item.ItemRichTextBox);
			parent.Controls.SetChildIndex(item.ItemLabel,index * 2);
			parent.Controls.SetChildIndex(item.ItemRichTextBox,(index * 2) + 1);

			items.Insert(index, item);

			parent.PerformLayout();
		}

		void IList.Insert(int index, object item)
		{
			this.Insert(index,(IndexedListItem)item);
		}

		public void Remove(IndexedListItem item)
		{
			items.Remove(item);
			parent.Controls.Remove(item.ItemLabel);
			parent.Controls.Remove(item.ItemRichTextBox);
			parent.SelectedIndex = -1;
			parent.PerformLayout();
		}

		void IList.Remove(object item)
		{
			this.Remove((IndexedListItem)item);
		}

		public void RemoveAt(int index)
		{
			parent.Controls.Remove(((IndexedListItem)this.items[index]).ItemLabel);
			parent.Controls.Remove(((IndexedListItem)this.items[index]).ItemRichTextBox);
			items.RemoveAt(index);
			parent.SelectedIndex = -1;
			parent.PerformLayout();
		}

		public void CopyTo(Array array, int index)
		{
			items.CopyTo(array, index);
		}

		public IEnumerator GetEnumerator()
		{
			return items.GetEnumerator();
		}
		#endregion

		#region Events Handling
		protected void item_SelectedChange(object sender, EventArgs e)
		{
			parent.SelectedChange(sender);
		}
		#endregion

		#region Protected Methods
		#endregion

		#region Private Methods
		#endregion
	}
}
