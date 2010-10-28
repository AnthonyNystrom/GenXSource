
using System;
using System.Collections;

namespace Genetibase.MathX.Utils
{

	public class NuGenCache
	{

		public NuGenCache(int capacity)
		{
			this.capacity = capacity;
		}

		public object this [object key]
		{
			get 
			{
				return Get(key); 
			}
			set 
			{
				Insert(key, value); 
			}
		}

		public int Size
		{
			get 
			{
				return size; 
			}
		}

		public bool Contains(object key)
		{
			return hashtable.ContainsKey(key);
		}

		public void Insert(object key, object val)
		{
						
			// if key exists replace it and return
			if (hashtable.ContainsKey(key))
			{
				((NuGenCacheEntry)hashtable[key]).val = val;
				return;
			}

			// remove lru entry if element count equals capacity
			if (size == capacity) RemoveFromHead();

			NuGenCacheEntry e = new NuGenCacheEntry(key, val);
			AddToTail(e);
			hashtable[key] = e;
		}

		public object Get(object key)
		{
						
			if (!hashtable.ContainsKey(key)) return null;
			NuGenCacheEntry e = (NuGenCacheEntry)hashtable[key];
			MoveToTail(e);
			return e.val;
		}

		protected void RemoveFromHead()
		{
						
			if (size == 0) return;

			size--;

			hashtable.Remove(head.key);

			if (size == 0)
			{
				head = tail = null;
			}

			else 
			{
				head = head.next;
			}
		}

		protected void AddToTail(NuGenCacheEntry e)
		{
			size++;

			if (tail == null)
			{
				head = tail = e;
				e.prev = null;
				e.next = null;
				return;
			}

			tail.next = e;
			e.prev = tail;
			e.next = null;
			tail = e;
		}

		protected void MoveToTail(NuGenCacheEntry e)
		{
						
			// tail element
			if (e == tail) return;

			// head element
			if (e == head)
			{
				head = head.next;
				tail.next = e;
				e.prev = tail;
				e.next = null;
				tail = e;
				return;
			}
			// inner element
			e.prev.next = e.next;
			e.next.prev = e.prev;
			tail.next = e;
			e.prev = tail;
			e.next = null;
			tail = e;
		}

		protected class NuGenCacheEntry
		{

			public NuGenCacheEntry(object key, object val)
			{
				this.key = key;
				this.val = val;
				prev = next = null;
			}

			public object val;
			public object key;
			public NuGenCacheEntry next;
			public NuGenCacheEntry prev;
		}

		NuGenCacheEntry head = null;
		NuGenCacheEntry tail = null;

		protected int capacity;
		protected int size = 0;
		protected Hashtable hashtable = new Hashtable();

	}

}
