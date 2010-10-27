/* ------------------------------------------------
 * ListModelBase.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace Next2Friends.CrossPoster.Client.Logic
{
    class ListModel<T>
    {
        protected IList<T> _list;

        public ListModel()
        {
            _list = new List<T>();
        }

        public event EventHandler<ListEventArgs<T>> ItemAdded;
        private void InvokeItemAdded(ListEventArgs<T> e)
        {
            if (ItemAdded != null)
                ItemAdded(this, e);
        }

        public event EventHandler<ListEventArgs<T>> ItemRemoved;
        private void InvokeItemRemoved(ListEventArgs<T> e)
        {
            if (ItemRemoved != null)
                ItemRemoved(this, e);
        }

        public Int32 ItemCount
        {
            get { return _list.Count; }
        }

        public T GetDescriptorAt(Int32 index)
        {
            return _list[index];
        }

        public ReadOnlyCollection<T> GetItems()
        {
            return new ReadOnlyCollection<T>(_list);
        }

        public void AddItem(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            _list.Add(item);
            InvokeItemAdded(new ListEventArgs<T>(item));
        }

        public void RemoveBlog(T item)
        {
            if (_list.Remove(item))
                InvokeItemRemoved(new ListEventArgs<T>(item));
        }
    }
}
