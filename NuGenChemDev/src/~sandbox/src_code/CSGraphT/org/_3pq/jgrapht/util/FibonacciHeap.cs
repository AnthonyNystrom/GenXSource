/* ==========================================
* JGraphT : a free Java graph-theory library
* ==========================================
*
* Project Info:  http://jgrapht.sourceforge.net/
* Project Lead:  Barak Naveh (barak_naveh@users.sourceforge.net)
*
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* This library is free software; you can redistribute it and/or modify it
* under the terms of the GNU Lesser General Public License as published by
* the Free Software Foundation; either version 2.1 of the License, or
* (at your option) any later version.
*
* This library is distributed in the hope that it will be useful, but
* WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
* or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
* License for more details.
*
* You should have received a copy of the GNU Lesser General Public License
* along with this library; if not, write to the Free Software Foundation, Inc.,
* 59 Temple Place, Suite 330, Boston, MA 02111-1307, USA.
*/
/* --------------------------
* FibonnaciHeap.java
* --------------------------
* (C) Copyright 1999-2003, by Nathan Fiedler and Contributors.
*
* Original Author:  Nathan Fiedler
* Contributor(s):   John V. Sichi
*
* $Id: FibonacciHeap.java,v 1.1 2003/09/04 18:15:14 perfecthash Exp $
*
* Changes
* -------
* 03-Sept-2003 : Adapted from Nathan Fiedler (JVS);
*
*      Name    Date            Description
*      ----    ----            -----------
*      nf      08/31/97        Initial version
*      nf      09/07/97        Removed FibHeapData interface
*      nf      01/20/01        Added synchronization
*      nf      01/21/01        Made Node an inner class
*      nf      01/05/02        Added clear(), renamed empty() to
*                              isEmpty(), and renamed printHeap()
*                              to toString()
*      nf      01/06/02        Removed all synchronization
*
*/
using System;
using CSGraphT;

namespace org._3pq.jgrapht.util
{
	
	/// <summary> This class implements a Fibonacci heap data structure. Much of the code in
	/// this class is based on the algorithms in the "Introduction to Algorithms"
	/// by Cormen, Leiserson, and Rivest in Chapter 21. The amortized running time
	/// of most of these methods is O(1), making it a very fast data structure.
	/// Several have an actual running time of O(1). removeMin() and delete() have
	/// O(log n) amortized running times because they do the heap consolidation. If
	/// you attempt to store nodes in this heap with key values of -Infinity
	/// (Double.NEGATIVE_INFINITY) the <code>delete()</code> operation may fail to
	/// remove the correct element.
	/// 
	/// <p>
	/// <b>Note that this implementation is not synchronized.</b> If multiple
	/// threads access a set concurrently, and at least one of the threads modifies
	/// the set, it <i>must</i> be synchronized externally. This is typically
	/// accomplished by synchronizing on some object that naturally encapsulates
	/// the set.
	/// </p>
	/// 
	/// <p>
	/// This class was originally developed by Nathan Fiedler for the GraphMaker
	/// project.  It was imported to JGraphT with permission, courtesy of Nathan
	/// Fiedler.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Nathan Fiedler
	/// </author>
	public class FibonacciHeap
	{
		/// <summary> Tests if the Fibonacci heap is empty or not. Returns true if the heap is
		/// empty, false otherwise.
		/// 
		/// <p>
		/// Running time: O(1) actual
		/// </p>
		/// 
		/// </summary>
		/// <returns> true if the heap is empty, false otherwise
		/// </returns>
		virtual public bool Empty
		{
			get
			{
				return m_min == null;
			}
			
		}
		/// <summary>Points to the minimum node in the heap. </summary>
		private Node m_min;
		
		/// <summary>Number of nodes in the heap. </summary>
		private int m_n;
		
		/// <summary> Constructs a FibonacciHeap object that contains no elements.</summary>
		public FibonacciHeap()
		{
		} // FibonacciHeap
		
		
		// isEmpty
		
		/// <summary> Removes all elements from this heap.</summary>
		public virtual void  clear()
		{
			m_min = null;
			m_n = 0;
		}
		
		
		// clear
		
		/// <summary> Decreases the key value for a heap node, given the new value to take on.
		/// The structure of the heap may be changed and will not be consolidated.
		/// 
		/// <p>
		/// Running time: O(1) amortized
		/// </p>
		/// 
		/// </summary>
		/// <param name="x">node to decrease the key of
		/// </param>
		/// <param name="k">new key value for node x
		/// 
		/// </param>
		/// <exception cref="IllegalArgumentException">Thrown if k is larger than x.key
		/// value.
		/// </exception>
		public virtual void  decreaseKey(Node x, double k)
		{
			if (k > x.m_key)
			{
				throw new System.ArgumentException("decreaseKey() got larger key value");
			}
			
			x.m_key = k;
			
			Node y = x.m_parent;
			
			if ((y != null) && (x.m_key < y.m_key))
			{
				cut(x, y);
				cascadingCut(y);
			}
			
			if (x.m_key < m_min.m_key)
			{
				m_min = x;
			}
		}
		
		
		// decreaseKey
		
		/// <summary> Deletes a node from the heap given the reference to the node. The trees
		/// in the heap will be consolidated, if necessary. This operation may fail
		/// to remove the correct element if there are nodes with key value
		/// -Infinity.
		/// 
		/// <p>
		/// Running time: O(log n) amortized
		/// </p>
		/// 
		/// </summary>
		/// <param name="x">node to remove from heap
		/// </param>
		public virtual void  delete(Node x)
		{
			// make x as small as possible
			decreaseKey(x, System.Double.NegativeInfinity);
			
			// remove the smallest, which decreases n also
			removeMin();
		}
		
		
		// delete
		
		/// <summary> Inserts a new data element into the heap. No heap consolidation is
		/// performed at this time, the new node is simply inserted into the root
		/// list of this heap.
		/// 
		/// <p>
		/// Running time: O(1) actual
		/// </p>
		/// 
		/// </summary>
		/// <param name="node">new node to insert into heap
		/// </param>
		/// <param name="key">key value associated with data object
		/// </param>
		public virtual void  insert(Node node, double key)
		{
			node.m_key = key;
			
			// concatenate node into min list
			if (m_min != null)
			{
				node.m_left = m_min;
				node.m_right = m_min.m_right;
				m_min.m_right = node;
				node.m_right.m_left = node;
				
				if (key < m_min.m_key)
				{
					m_min = node;
				}
			}
			else
			{
				m_min = node;
			}
			
			m_n++;
		}
		
		
		// insert
		
		/// <summary> Returns the smallest element in the heap. This smallest element is the
		/// one with the minimum key value.
		/// 
		/// <p>
		/// Running time: O(1) actual
		/// </p>
		/// 
		/// </summary>
		/// <returns> heap node with the smallest key
		/// </returns>
		public virtual Node min()
		{
			return m_min;
		}
		
		
		// min
		
		/// <summary> Removes the smallest element from the heap. This will cause the trees in
		/// the heap to be consolidated, if necessary.
		/// 
		/// <p>
		/// Running time: O(log n) amortized
		/// </p>
		/// 
		/// </summary>
		/// <returns> node with the smallest key
		/// </returns>
		public virtual Node removeMin()
		{
			Node z = m_min;
			
			if (z != null)
			{
				int numKids = z.m_degree;
				Node x = z.m_child;
				Node tempRight;
				
				// for each child of z do...
				while (numKids > 0)
				{
					tempRight = x.m_right;
					
					// remove x from child list
					x.m_left.m_right = x.m_right;
					x.m_right.m_left = x.m_left;
					
					// add x to root list of heap
					x.m_left = m_min;
					x.m_right = m_min.m_right;
					m_min.m_right = x;
					x.m_right.m_left = x;
					
					// set parent[x] to null
					x.m_parent = null;
					x = tempRight;
					numKids--;
				}
				
				// remove z from root list of heap
				z.m_left.m_right = z.m_right;
				z.m_right.m_left = z.m_left;
				
				if (z == z.m_right)
				{
					m_min = null;
				}
				else
				{
					m_min = z.m_right;
					consolidate();
				}
				
				// decrement size of heap
				m_n--;
			}
			
			return z;
		}
		
		
		// removeMin
		
		/// <summary> Returns the size of the heap which is measured in the number of elements
		/// contained in the heap.
		/// 
		/// <p>
		/// Running time: O(1) actual
		/// </p>
		/// 
		/// </summary>
		/// <returns> number of elements in the heap
		/// </returns>
		public virtual int size()
		{
			return m_n;
		}
		
		
		// size
		
		/// <summary> Joins two Fibonacci heaps into a new one. No heap consolidation is
		/// performed at this time. The two root lists are simply joined together.
		/// 
		/// <p>
		/// Running time: O(1) actual
		/// </p>
		/// 
		/// </summary>
		/// <param name="h1">first heap
		/// </param>
		/// <param name="h2">second heap
		/// 
		/// </param>
		/// <returns> new heap containing h1 and h2
		/// </returns>
		public static FibonacciHeap union(FibonacciHeap h1, FibonacciHeap h2)
		{
			FibonacciHeap h = new FibonacciHeap();
			
			if ((h1 != null) && (h2 != null))
			{
				h.m_min = h1.m_min;
				
				if (h.m_min != null)
				{
					if (h2.m_min != null)
					{
						h.m_min.m_right.m_left = h2.m_min.m_left;
						h2.m_min.m_left.m_right = h.m_min.m_right;
						h.m_min.m_right = h2.m_min;
						h2.m_min.m_left = h.m_min;
						
						if (h2.m_min.m_key < h1.m_min.m_key)
						{
							h.m_min = h2.m_min;
						}
					}
				}
				else
				{
					h.m_min = h2.m_min;
				}
				
				h.m_n = h1.m_n + h2.m_n;
			}
			
			return h;
		}
		
		
		// union
		
		/// <summary> Creates a String representation of this Fibonacci heap.
		/// 
		/// </summary>
		/// <returns> String of this.
		/// </returns>
		public override System.String ToString()
		{
			if (m_min == null)
			{
				return "FibonacciHeap=[]";
			}
			
			// create a new stack and put root on it
			System.Collections.ArrayList stack = new System.Collections.ArrayList();
			stack.Add(m_min);
			
			System.Text.StringBuilder buf = new System.Text.StringBuilder(512);
			buf.Append("FibonacciHeap=[");
			
			// do a simple breadth-first traversal on the tree
			while (!(stack.Count == 0))
			{
				Node curr = (Node) SupportClass.StackSupport.Pop(stack);
				buf.Append(curr);
				buf.Append(", ");
				
				if (curr.m_child != null)
				{
					stack.Add(curr.m_child);
				}
				
				Node start = curr;
				curr = curr.m_right;
				
				while (curr != start)
				{
					buf.Append(curr);
					buf.Append(", ");
					
					if (curr.m_child != null)
					{
						stack.Add(curr.m_child);
					}
					
					curr = curr.m_right;
				}
			}
			
			buf.Append(']');
			
			return buf.ToString();
		}
		
		
		// toString
		
		/// <summary> Performs a cascading cut operation. This cuts y from its parent and then
		/// does the same for its parent, and so on up the tree.
		/// 
		/// <p>
		/// Running time: O(log n); O(1) excluding the recursion
		/// </p>
		/// 
		/// </summary>
		/// <param name="y">node to perform cascading cut on
		/// </param>
		protected internal virtual void  cascadingCut(Node y)
		{
			Node z = y.m_parent;
			
			// if there's a parent...
			if (z != null)
			{
				// if y is unmarked, set it marked
				if (!y.m_mark)
				{
					y.m_mark = true;
				}
				else
				{
					// it's marked, cut it from parent
					cut(y, z);
					
					// cut its parent as well
					cascadingCut(z);
				}
			}
		}
		
		
		// cascadingCut
		
		/// <summary> Consolidates the trees in the heap by joining trees of equal degree
		/// until there are no more trees of equal degree in the root list.
		/// 
		/// <p>
		/// Running time: O(log n) amortized
		/// </p>
		/// </summary>
		protected internal virtual void  consolidate()
		{
			int arraySize = m_n + 1;
			Node[] array = new Node[arraySize];
			
			// Initialize degree array
			for (int i = 0; i < arraySize; i++)
			{
				array[i] = null;
			}
			
			// Find the number of root nodes.
			int numRoots = 0;
			Node x = m_min;
			
			if (x != null)
			{
				numRoots++;
				x = x.m_right;
				
				while (x != m_min)
				{
					numRoots++;
					x = x.m_right;
				}
			}
			
			// For each node in root list do...
			while (numRoots > 0)
			{
				// Access this node's degree..
				int d = x.m_degree;
				Node next = x.m_right;
				
				// ..and see if there's another of the same degree.
				while (array[d] != null)
				{
					// There is, make one of the nodes a child of the other.
					Node y = array[d];
					
					// Do this based on the key value.
					if (x.m_key > y.m_key)
					{
						Node temp = y;
						y = x;
						x = temp;
					}
					
					// Node y disappears from root list.
					link(y, x);
					
					// We've handled this degree, go to next one.
					array[d] = null;
					d++;
				}
				
				// Save this node for later when we might encounter another
				// of the same degree.
				array[d] = x;
				
				// Move forward through list.
				x = next;
				numRoots--;
			}
			
			// Set min to null (effectively losing the root list) and
			// reconstruct the root list from the array entries in array[].
			m_min = null;
			
			for (int i = 0; i < arraySize; i++)
			{
				if (array[i] != null)
				{
					// We've got a live one, add it to root list.
					if (m_min != null)
					{
						// First remove node from root list.
						array[i].m_left.m_right = array[i].m_right;
						array[i].m_right.m_left = array[i].m_left;
						
						// Now add to root list, again.
						array[i].m_left = m_min;
						array[i].m_right = m_min.m_right;
						m_min.m_right = array[i];
						array[i].m_right.m_left = array[i];
						
						// Check if this is a new min.
						if (array[i].m_key < m_min.m_key)
						{
							m_min = array[i];
						}
					}
					else
					{
						m_min = array[i];
					}
				}
			}
		}
		
		
		// consolidate
		
		/// <summary> The reverse of the link operation: removes x from the child list of y.
		/// This method assumes that min is non-null.
		/// 
		/// <p>
		/// Running time: O(1)
		/// </p>
		/// 
		/// </summary>
		/// <param name="x">child of y to be removed from y's child list
		/// </param>
		/// <param name="y">parent of x about to lose a child
		/// </param>
		protected internal virtual void  cut(Node x, Node y)
		{
			// remove x from childlist of y and decrement degree[y]
			x.m_left.m_right = x.m_right;
			x.m_right.m_left = x.m_left;
			y.m_degree--;
			
			// reset y.child if necessary
			if (y.m_child == x)
			{
				y.m_child = x.m_right;
			}
			
			if (y.m_degree == 0)
			{
				y.m_child = null;
			}
			
			// add x to root list of heap
			x.m_left = m_min;
			x.m_right = m_min.m_right;
			m_min.m_right = x;
			x.m_right.m_left = x;
			
			// set parent[x] to nil
			x.m_parent = null;
			
			// set mark[x] to false
			x.m_mark = false;
		}
		
		
		// cut
		
		/// <summary> Make node y a child of node x.
		/// 
		/// <p>
		/// Running time: O(1) actual
		/// </p>
		/// 
		/// </summary>
		/// <param name="y">node to become child
		/// </param>
		/// <param name="x">node to become parent
		/// </param>
		protected internal virtual void  link(Node y, Node x)
		{
			// remove y from root list of heap
			y.m_left.m_right = y.m_right;
			y.m_right.m_left = y.m_left;
			
			// make y a child of x
			y.m_parent = x;
			
			if (x.m_child == null)
			{
				x.m_child = y;
				y.m_right = y;
				y.m_left = y;
			}
			else
			{
				y.m_left = x.m_child;
				y.m_right = x.m_child.m_right;
				x.m_child.m_right = y;
				y.m_right.m_left = y;
			}
			
			// increase degree[x]
			x.m_degree++;
			
			// set mark[y] false
			y.m_mark = false;
		}
		
		// link
		
		/// <summary> Implements a node of the Fibonacci heap. It holds the information
		/// necessary for maintaining the structure of the heap. It also holds the
		/// reference to the key value (which is used to determine the heap
		/// structure).  Additional Node data should be stored in a subclass.
		/// 
		/// </summary>
		/// <author>  Nathan Fiedler
		/// </author>
		public class Node
		{
			/// <summary> Obtain the key for this node.
			/// 
			/// </summary>
			/// <returns> the key
			/// </returns>
			virtual public double Key
			{
				get
				{
					return m_key;
				}
				
			}
			/// <summary>first child node </summary>
			internal Node m_child;
			
			/// <summary>left sibling node </summary>
			internal Node m_left;
			
			/// <summary>parent node </summary>
			internal Node m_parent;
			
			/// <summary>right sibling node </summary>
			internal Node m_right;
			
			/// <summary> true if this node has had a child removed since this node was added
			/// to its parent
			/// </summary>
			internal bool m_mark;
			
			/// <summary>key value for this node </summary>
			internal double m_key;
			
			/// <summary>number of children of this node (does not count grandchildren) </summary>
			internal int m_degree;
			
			/// <summary> Default constructor.  Initializes the right and left pointers,
			/// making this a circular doubly-linked list.
			/// 
			/// </summary>
			/// <param name="key">initial key for node
			/// </param>
			public Node(double key)
			{
				m_right = this;
				m_left = this;
				m_key = key;
			}
			
			
			/// <summary> Return the string representation of this object.
			/// 
			/// </summary>
			/// <returns> string representing this object
			/// </returns>
			public override System.String ToString()
			{
				if (true)
				{
					return m_key.ToString();
				}
				else
				{
					System.Text.StringBuilder buf = new System.Text.StringBuilder();
					buf.Append("Node=[parent = ");
					
					if (m_parent != null)
					{
						buf.Append(m_parent.m_key.ToString());
					}
					else
					{
						buf.Append("---");
					}
					
					buf.Append(", key = ");
					buf.Append(m_key.ToString());
					buf.Append(", degree = ");
					buf.Append(System.Convert.ToString(m_degree));
					buf.Append(", right = ");
					
					if (m_right != null)
					{
						buf.Append(m_right.m_key.ToString());
					}
					else
					{
						buf.Append("---");
					}
					
					buf.Append(", left = ");
					
					if (m_left != null)
					{
						buf.Append(m_left.m_key.ToString());
					}
					else
					{
						buf.Append("---");
					}
					
					buf.Append(", child = ");
					
					if (m_child != null)
					{
						buf.Append(m_child.m_key.ToString());
					}
					else
					{
						buf.Append("---");
					}
					
					buf.Append(']');
					
					return buf.ToString();
				}
			}
			
			// toString
		}
		
		// Node
	}
	
	
	// FibonacciHeap
}