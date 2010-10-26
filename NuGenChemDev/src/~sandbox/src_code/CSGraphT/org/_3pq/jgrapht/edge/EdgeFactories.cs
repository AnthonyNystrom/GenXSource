/* ==========================================
* JGraphT : a free Java graph-theory library
* ==========================================
*
* Project Info:  http://jgrapht.sourceforge.net/
* Project Lead:  Barak Naveh (http://sourceforge.net/users/barak_naveh)
*
* (C) Copyright 2003-2004, by Barak Naveh and Contributors.
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
/* ------------------
* EdgeFactories.java
* ------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: EdgeFactories.java,v 1.6 2004/11/18 21:44:24 barak_naveh Exp $
*
* Changes
* -------
* 24-Jul-2003 : Initial revision (BN);
* 04-Aug-2003 : Renamed from EdgeFactoryFactory & made utility class (BN);
* 03-Nov-2003 : Made edge factories serializable (BN);
*
*/
using System;
using Edge = org._3pq.jgrapht.Edge;
using EdgeFactory = org._3pq.jgrapht.EdgeFactory;
namespace org._3pq.jgrapht.edge
{
	
	/// <summary> This utility class is a container of various {@link
	/// org._3pq.jgrapht.EdgeFactory} classes.
	/// 
	/// <p>
	/// Classes included here do not have substantial logic. They are grouped
	/// together in this container in order to avoid clutter.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Jul 16, 2003
	/// </since>
	public sealed class EdgeFactories
	{
		private EdgeFactories()
		{
		} // ensure non-instantiability.
		
		/// <summary> An EdgeFactory for producing directed edges.
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// 
		/// </author>
		/// <since> Jul 14, 2003
		/// </since>
		[Serializable]
		public class DirectedEdgeFactory:AbstractEdgeFactory
		{
			private const long serialVersionUID = 3618135658586388792L;
			
			/// <seealso cref="EdgeFactory.createEdge(Object, Object)">
			/// </seealso>
			public override Edge createEdge(System.Object source, System.Object target)
			{
				return new DirectedEdge(source, target);
			}
		}
		
		
		/// <summary> An EdgeFactory for producing directed edges with weights.
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// 
		/// </author>
		/// <since> Jul 14, 2003
		/// </since>
		[Serializable]
		public class DirectedWeightedEdgeFactory:AbstractEdgeFactory
		{
			private const long serialVersionUID = 3257002163870775604L;
			
			/// <seealso cref="EdgeFactory.createEdge(Object, Object)">
			/// </seealso>
			public override Edge createEdge(System.Object source, System.Object target)
			{
				return new DirectedWeightedEdge(source, target);
			}
		}
		
		
		/// <summary> An EdgeFactory for producing undirected edges.
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// 
		/// </author>
		/// <since> Jul 14, 2003
		/// </since>
		[Serializable]
		public class UndirectedEdgeFactory:AbstractEdgeFactory
		{
			private const long serialVersionUID = 3257007674431189815L;
			
			/// <seealso cref="EdgeFactory.createEdge(Object, Object)">
			/// </seealso>
			public override Edge createEdge(System.Object source, System.Object target)
			{
				return new UndirectedEdge(source, target);
			}
		}
		
		
		/// <summary> An EdgeFactory for producing undirected edges with weights.
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// 
		/// </author>
		/// <since> Jul 14, 2003
		/// </since>
		[Serializable]
		public class UndirectedWeightedEdgeFactory:AbstractEdgeFactory
		{
			private const long serialVersionUID = 4048797883346269237L;
			
			/// <seealso cref="EdgeFactory.createEdge(Object, Object)">
			/// </seealso>
			public override Edge createEdge(System.Object source, System.Object target)
			{
				return new UndirectedWeightedEdge(source, target);
			}
		}
		
		
		/// <summary> A base class for edge factories.
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// 
		/// </author>
		/// <since> Nov 3, 2003
		/// </since>
		//UPGRADE_NOTE: The access modifier for this class or class field has been changed in order to prevent compilation errors due to the visibility level. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1296'"
		[Serializable]
		abstract public class AbstractEdgeFactory : EdgeFactory
		{
			public abstract org._3pq.jgrapht.Edge createEdge(System.Object param1, System.Object param2);
		}
	}
}