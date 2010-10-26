/* -----------------------------------------------
 * NuGenWmHandlerList.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Represents a collection of WM_X identifiers and appropriate <see cref="NuGenWmHandler"/> delegates.
	/// </summary>
	public class NuGenWmHandlerList
	{
		#region Properties.Public

		/*
		 * Count
		 */

		/// <summary>
		/// </summary>
		public int Count
		{
			get
			{
				return this.MessageMap.Count;
			}
		}

		/*
		 * Indexer
		 */

		/// <summary>
		/// </summary>
		public NuGenWmHandler this[int index]
		{
			get
			{
				if (this.MessageMap.ContainsKey(index))
				{
					return this.MessageMap[index];
				}

				return null;
			}
			set
			{
				this.MessageMap[index] = value;
			}
		}

		#endregion

		#region Properties.Protected

		private Dictionary<int, NuGenWmHandler> _messageMap;

		/// <summary>
		/// </summary>
		protected Dictionary<int, NuGenWmHandler> MessageMap
		{
			get
			{
				if (_messageMap == null)
				{
					_messageMap = new Dictionary<int, NuGenWmHandler>();
				}

				return _messageMap;
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * AddWmHandler
		 */

		/// <summary>
		/// </summary>
		/// <param name="wmId"></param>
		/// <param name="wmHandler"></param>
		public void AddWmHandler(int wmId, NuGenWmHandler wmHandler)
		{
			if (this.MessageMap.ContainsKey(wmId))
			{
				NuGenWmHandler @delegate = this.MessageMap[wmId];
				this.MessageMap[wmId] = (NuGenWmHandler)NuGenWmHandler.Combine(@delegate, wmHandler);
			}
			else
			{
				this.MessageMap.Add(wmId, wmHandler);
			}
		}

        /*
         * GetEnumerator
         */

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<int> GetEnumerator()
        {
            Debug.Assert(this.MessageMap != null, "this.MessageMap != null");

            foreach (int wmId in this.MessageMap.Keys)
            {
                yield return wmId;
            }
        }

		/*
		 * RemoveWmHandler
		 */

		/// <summary>
		/// </summary>
		/// <param name="wmId"></param>
		/// <param name="wmHandler"></param>
		public void RemoveWmHandler(int wmId, NuGenWmHandler wmHandler)
		{
			if (this.MessageMap.ContainsKey(wmId))
			{
				NuGenWmHandler @delegate = this.MessageMap[wmId];

				if (@delegate != null)
				{
					this.MessageMap[wmId] = (NuGenWmHandler)NuGenWmHandler.Remove(@delegate, wmHandler);
				}
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWmHandlerList"/> class.
		/// </summary>
		public NuGenWmHandlerList()
		{

		}
	}
}
