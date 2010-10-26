/* -----------------------------------------------
 * NuGenTreeNodeCollection.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Genetibase.Controls.Collections
{
	/// <summary>
	/// Represents a proxy for the <see cref="T:NuGenTreeView"/> and <see cref="T:NuGenTreeNode"/> to
	/// operate with custom node types without explicit boxing/unboxing (it is necessary to preserve
	/// the native collection to process the tree properly). It is recommended to hide the native collection
	/// and manage it according to the events bubbled by this <see cref="T:NuGenTreeNodeCollection"/>. Do use
	/// calls to the base class (e.g. base.Nodes.Add, not this.Nodes.Add) to prevent
	/// <see cref="T:System.StackOverflowException"/>.
	/// </summary>
	public class NuGenTreeNodeCollection : NuGenEventInitiator, IEnumerable
	{
		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator GetEnumerator()
		{
			NuGenEnumeratorRequestedEventArgs eventArgs = new NuGenEnumeratorRequestedEventArgs();
			this.OnEnumeratorRequested(eventArgs);
			return eventArgs.RequestedEnumerator;
		}

		private static readonly object eventEnumeratorRequested = new object();

		/// <summary>
		/// The handler should set the value for the <see cref="P:RequestedEnumerator"/> property.
		/// </summary>
		public event EventHandler<NuGenEnumeratorRequestedEventArgs> EnumeratorRequested
		{
			add
			{
				this.Events.AddHandler(eventEnumeratorRequested, value);
			}
			remove
			{
				this.Events.RemoveHandler(eventEnumeratorRequested, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:EnumeratorRequested"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnEnumeratorRequested(NuGenEnumeratorRequestedEventArgs e)
		{
			this.InvokeActionT<NuGenEnumeratorRequestedEventArgs>(eventEnumeratorRequested, e);
		}

		#endregion

		#region Properties.Public.Count

		/*
		 * Count
		 */

		/// <summary>
		/// Gets the number of nodes in the collection.
		/// </summary>
		public int Count
		{
			get
			{
				NuGenItemsCountRequestedEventArgs eventArgs = new NuGenItemsCountRequestedEventArgs();
				this.OnNodeCountRequested(eventArgs);
				return eventArgs.Count;
			}
		}

		private static readonly object eventNodeCountRequested = new object();

		/// <summary>
		/// The handler should set the value for the <see cref="P:Count"/> property to indicate the number
		/// of nodes contained within the native collection.
		/// </summary>
		public event EventHandler<NuGenItemsCountRequestedEventArgs> NodeCountRequested
		{
			add
			{
				this.Events.AddHandler(eventNodeCountRequested, value);
			}
			remove
			{
				this.Events.RemoveHandler(eventNodeCountRequested, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:NodeCountRequested"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnNodeCountRequested(NuGenItemsCountRequestedEventArgs e)
		{
			this.InvokeActionT<NuGenItemsCountRequestedEventArgs>(eventNodeCountRequested, e);
		}

		#endregion

		#region Properties.Indexer

		/// <summary>
		/// </summary>
		public NuGenTreeNode this[int nodeIndex]
		{
			get
			{
				NuGenIndexedTreeNodeEventArgs eventArgs = new NuGenIndexedTreeNodeEventArgs(nodeIndex);
				this.OnNodeByIndexRequested(eventArgs);
				return eventArgs.TreeNode;
			}
			set
			{
				NuGenIndexedTreeNodeEventArgs eventArgs = new NuGenIndexedTreeNodeEventArgs(nodeIndex, value);
				this.OnNodeByIndexAdjusted(eventArgs);
			}
		}

		private static readonly object eventNodeByIndexRequested = new object();

		/// <summary>
		/// The handler should set the value for the <see cref="P:TreeNode"/> property according to the
		/// specified index.
		/// </summary>
		public event EventHandler<NuGenIndexedTreeNodeEventArgs> NodeByIndexRequested
		{
			add
			{
				this.Events.AddHandler(eventNodeByIndexRequested, value);
			}
			remove
			{
				this.Events.RemoveHandler(eventNodeByIndexRequested, value);
			}
		}

		/// <summary>
		/// Will bubble <see cref="E:NodeByIndexRequested"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnNodeByIndexRequested(NuGenIndexedTreeNodeEventArgs e)
		{
			this.InvokeActionT<NuGenIndexedTreeNodeEventArgs>(eventNodeByIndexRequested, e);
		}

		private static readonly object eventNodeByIndexAdjusted = new object();

		/// <summary>
		/// The handler should set the specified value i.e. <see cref="P:TreeNode"/> for the item at the
		/// specified index in the collection.
		/// </summary>
		public event EventHandler<NuGenIndexedTreeNodeEventArgs> NodeByIndexAdjusted
		{
			add
			{
				this.Events.AddHandler(eventNodeByIndexAdjusted, value);
			}
			remove
			{
				this.Events.RemoveHandler(eventNodeByIndexAdjusted, value);
			}
		}
		
		/// <summary>
		/// Will bubble <see cref="E:NodeByIndexAdjusted"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnNodeByIndexAdjusted(NuGenIndexedTreeNodeEventArgs e)
		{
			this.InvokeActionT<NuGenIndexedTreeNodeEventArgs>(eventNodeByIndexAdjusted, e);
		}

		#endregion

		#region Methods.Public.Add

		/// <summary>
		/// </summary>
		/// <param name="treeNodeToAdd"></param>
		/// <returns></returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="treeNodeToAdd"/> is <see langword="null"/>.
		/// </exception>
		public int AddNode(NuGenTreeNode treeNodeToAdd)
		{
			NuGenAddTreeNodeEventArgs eventArgs = new NuGenAddTreeNodeEventArgs(treeNodeToAdd);
			this.OnNodeAdded(eventArgs);
			return eventArgs.TreeNodeIndex;
		}

		private static readonly object eventNodeAdded = new object();

		/// <summary>
		/// A new <see cref="T:NuGenTreeNode"/> should be added to the collection. The handler should set
		/// the value for the <see cref="P:TreeNodeIndex"/> property that indicates the index the node has
		/// been added at.
		/// </summary>
		public event EventHandler<NuGenAddTreeNodeEventArgs> NodeAdded
		{
			add
			{
				this.Events.AddHandler(eventNodeAdded, value);
			}
			remove
			{
				this.Events.RemoveHandler(eventNodeAdded, value);
			}
		}

		/// <summary>
		/// Will bubble <see cref="E:NodeAdded"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnNodeAdded(NuGenAddTreeNodeEventArgs e)
		{
			this.InvokeActionT<NuGenAddTreeNodeEventArgs>(eventNodeAdded, e);
		}

		#endregion

		#region Methods.Public.AddRange

		/// <summary>
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="treeNodeRangeToAdd"/> is <see langword="null"/>.
		/// </exception>
		public void AddNodeRange(NuGenTreeNode[] treeNodeRangeToAdd)
		{
			this.OnNodeRangeAdded(new NuGenAddTreeNodeRangeEventArgs(treeNodeRangeToAdd));
		}

		private static readonly object eventNodeRangeAdded = new object();

		/// <summary>
		/// The handler should add the specified <see cref="T:NuGenTreeNode"/> range to the collection.
		/// </summary>
		public event EventHandler<NuGenAddTreeNodeRangeEventArgs> NodeRangeAdded
		{
			add
			{
				this.Events.AddHandler(eventNodeRangeAdded, value);
			}
			remove
			{
				this.Events.RemoveHandler(eventNodeRangeAdded, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:NodeRangeAdded"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnNodeRangeAdded(NuGenAddTreeNodeRangeEventArgs e)
		{
			this.InvokeActionT<NuGenAddTreeNodeRangeEventArgs>(eventNodeRangeAdded, e);
		}

		#endregion

		#region Methods.Public.Clear

		/// <summary>
		/// Clear underlying collection.
		/// </summary>
		public void Clear()
		{
			this.OnClearNodesRequested(new NuGenItemsClearRequestedEventArgs());
		}

		private static readonly object eventClearNodesRequested = new object();

		/// <summary>
		/// The handler should clear its collection.
		/// </summary>
		public event EventHandler<NuGenItemsClearRequestedEventArgs> ClearNodesRequested
		{
			add
			{
				this.Events.AddHandler(eventClearNodesRequested, value);
			}
			remove
			{
				this.Events.RemoveHandler(eventClearNodesRequested, value);
			}
		}

		/// <summary>
		/// Will bubble <see cref="E:ClearNodesRequested"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnClearNodesRequested(NuGenItemsClearRequestedEventArgs e)
		{
			this.InvokeActionT<NuGenItemsClearRequestedEventArgs>(eventClearNodesRequested, e);
		}

		#endregion

		#region Methods.Public.Contains

		/// <summary>
		/// </summary>
		/// <param name="treeNodeToCheck">Can be <see langword="null"/>.</param>
		/// <returns></returns>
		public bool Contains(NuGenTreeNode treeNodeToCheck)
		{
			NuGenContainsItemRequestedEventArgs eventArgs = new NuGenContainsItemRequestedEventArgs(treeNodeToCheck);
			this.OnContainsNodeRequested(eventArgs);
			return eventArgs.ContainsNode;
		}

		private static readonly object eventContainsNodeRequested = new object();

		/// <summary>
		/// The handler should set the value for the <see cref="P:ContainsNode"/> property indicating whether
		/// the <see cref="P:NodeToCheck"/> is contained within the collection.
		/// </summary>
		public event EventHandler<NuGenContainsItemRequestedEventArgs> ContainsNodeRequested
		{
			add
			{
				this.Events.AddHandler(eventContainsNodeRequested, value);
			}
			remove
			{
				this.Events.RemoveHandler(eventContainsNodeRequested, value);
			}
		}

		/// <summary>
		/// Will bubble <see cref="E:ContainsNodeRequested"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnContainsNodeRequested(NuGenContainsItemRequestedEventArgs e)
		{
			this.InvokeActionT<NuGenContainsItemRequestedEventArgs>(eventContainsNodeRequested, e);
		}

		#endregion

		#region Methods.Public.Insert

		/// <summary>
		/// </summary>
		/// <param name="indexToInsertAt"></param>
		/// <param name="treeNodeToInsert"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="treeNodeToInsert"/> is <see langword="null"/>.
		/// </exception>
		public void InsertNode(int indexToInsertAt, NuGenTreeNode treeNodeToInsert)
		{
			if (treeNodeToInsert == null)
			{
				throw new ArgumentNullException("treeNodeToInsert");
			}
			else
			{
				this.OnNodeInserted(new NuGenAddTreeNodeEventArgs(treeNodeToInsert, indexToInsertAt));
			}
		}

		private static readonly object eventNodeInserted = new object();

		/// <summary>
		/// A new <see cref="T:NuGenTreeNode"/> should be inserted into the native collection.
		/// </summary>
		public event EventHandler<NuGenAddTreeNodeEventArgs> NodeInserted
		{
			add
			{
				this.Events.AddHandler(eventNodeInserted, value);
			}
			remove
			{
				this.Events.RemoveHandler(eventNodeInserted, value);
			}
		}

		/// <summary>
		/// Will bubble <see cref="E:NodeInserted"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnNodeInserted(NuGenAddTreeNodeEventArgs e)
		{
			this.InvokeActionT<NuGenAddTreeNodeEventArgs>(eventNodeInserted, e);
		}

		#endregion

		#region Methods.Public.Remove

		/// <summary>
		/// </summary>
		/// <param name="treeNodeToRemove"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="treeNodeToRemove"/> is <see langword="null"/>.
		/// </exception>
		public void RemoveNode(NuGenTreeNode treeNodeToRemove)
		{
			if (treeNodeToRemove == null)
			{
				throw new ArgumentNullException("treeNodeToRemove");
			}
			else
			{
				this.OnNodeRemoved(new NuGenRemoveTreeNodeEventArgs(treeNodeToRemove));
			}
		}

		private static readonly object eventNodeRemoved = new object();

		/// <summary>
		/// A new <see cref="T:NuGenTreeNode"/> should be removed from the native collection.
		/// </summary>
		public event EventHandler<NuGenRemoveTreeNodeEventArgs> NodeRemoved
		{
			add
			{
				this.Events.AddHandler(eventNodeRemoved, value);
			}
			remove
			{
				this.Events.RemoveHandler(eventNodeRemoved, value);
			}
		}

		/// <summary>
		/// Will bubble <see cref="E:NodeRemoved"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnNodeRemoved(NuGenRemoveTreeNodeEventArgs e)
		{
			this.InvokeActionT<NuGenRemoveTreeNodeEventArgs>(eventNodeRemoved, e);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeNodeCollection"/> class.
		/// </summary>
		public NuGenTreeNodeCollection()
		{
		}

		#endregion
	}
}
