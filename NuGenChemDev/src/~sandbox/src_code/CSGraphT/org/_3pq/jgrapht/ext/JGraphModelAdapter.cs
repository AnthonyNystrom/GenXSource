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
/* -----------------------
* JGraphModelAdapter.java
* -----------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   Erik Postma
*
* $Id: JGraphModelAdapter.java,v 1.22 2005/07/17 05:40:49 perfecthash Exp $
*
* Changes
* -------
* 02-Aug-2003 : Initial revision (BN);
* 10-Aug-2003 : Adaptation to new event model (BN);
* 06-Nov-2003 : Allowed non-listenable underlying JGraphT graph (BN);
* 12-Dec-2003 : Added CellFactory support (BN);
* 27-Jan-2004 : Added support for JGraph->JGraphT change propagation (EP);
* 29-Jan-2005 : Added support for JGraph dangling edges (BN);
*
*/
using System;
using DirectedGraph = org._3pq.jgrapht.DirectedGraph;
using EdgeFactory = org._3pq.jgrapht.EdgeFactory;
using Graph = org._3pq.jgrapht.Graph;
using ListenableGraph = org._3pq.jgrapht.ListenableGraph;
using GraphEdgeChangeEvent = org._3pq.jgrapht.event_Renamed.GraphEdgeChangeEvent;
using GraphListener = org._3pq.jgrapht.event_Renamed.GraphListener;
using GraphVertexChangeEvent = org._3pq.jgrapht.event_Renamed.GraphVertexChangeEvent;
//UPGRADE_TODO: The type 'org.jgraph.event_Renamed.GraphModelEvent' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using GraphModelEvent = org.jgraph.event_Renamed.GraphModelEvent;
//UPGRADE_TODO: The type 'org.jgraph.event_Renamed.GraphModelEvent.GraphModelChange' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using GraphModelChange = org.jgraph.event_Renamed.GraphModelEvent.GraphModelChange;
//UPGRADE_TODO: The type 'org.jgraph.event_Renamed.GraphModelListener' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using GraphModelListener = org.jgraph.event_Renamed.GraphModelListener;
//UPGRADE_TODO: The type 'org.jgraph.graph.AttributeMap' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AttributeMap = org.jgraph.graph.AttributeMap;
//UPGRADE_TODO: The type 'org.jgraph.graph.ConnectionSet' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using ConnectionSet = org.jgraph.graph.ConnectionSet;
//UPGRADE_TODO: The type 'org.jgraph.graph.DefaultEdge' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using DefaultEdge = org.jgraph.graph.DefaultEdge;
//UPGRADE_TODO: The type 'org.jgraph.graph.DefaultGraphCell' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using DefaultGraphCell = org.jgraph.graph.DefaultGraphCell;
//UPGRADE_TODO: The type 'org.jgraph.graph.DefaultGraphModel' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using DefaultGraphModel = org.jgraph.graph.DefaultGraphModel;
//UPGRADE_TODO: The type 'org.jgraph.graph.DefaultPort' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using DefaultPort = org.jgraph.graph.DefaultPort;
//UPGRADE_TODO: The type 'org.jgraph.graph.GraphCell' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using GraphCell = org.jgraph.graph.GraphCell;
//UPGRADE_TODO: The type 'org.jgraph.graph.GraphConstants' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using GraphConstants = org.jgraph.graph.GraphConstants;
//UPGRADE_TODO: The type 'org.jgraph.graph.Port' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Port = org.jgraph.graph.Port;
namespace org._3pq.jgrapht.ext
{
	
	/// <summary> An adapter that reflects a JGraphT graph as a JGraph graph. This adapter is
	/// useful when using JGraph in order to visualize JGraphT graphs. For more
	/// about JGraph see <a href="http://jgraph.sourceforge.net">
	/// http://jgraph.sourceforge.net</a>
	/// 
	/// <p>
	/// Modifications made to the underlying JGraphT graph are reflected to this
	/// JGraph model if and only if the underlying JGraphT graph is a {@link
	/// org._3pq.jgrapht.ListenableGraph}. If the underlying JGraphT graph is
	/// <i>not</i> ListenableGraph, then this JGraph model represent a snapshot if
	/// the graph at the time of its creation.
	/// </p>
	/// 
	/// <p>
	/// Changes made to this JGraph model are also reflected back to the underlying
	/// JGraphT graph. To avoid confusion, variables are prefixed according to the
	/// JGraph/JGraphT object(s) they are referring to.
	/// </p>
	/// 
	/// <p>
	/// <b>KNOWN BUGS:</b> There is a small issue to be aware of. JGraph allows
	/// 'dangling edges' incident with just one vertex; JGraphT doesn't. Such a
	/// configuration can arise when adding an edge or removing a vertex. The code
	/// handles this by removing the newly-added dangling edge or removing all
	/// edges incident with the vertex before actually removing the vertex,
	/// respectively. This works very well, only it doesn't play all that nicely
	/// with the undo-manager in the JGraph: for the second situation where you
	/// remove a vertex incident with some edges, if you undo the removal, the
	/// vertex is 'unremoved' but the edges aren't.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Aug 2, 2003
	/// </since>
	
	/*
	* FUTURE WORK: Now that the adapter supports JGraph dangling edges, it is
	* possible, with a little effort, to eliminate the "known bugs" above. Some
	* todo and fixme marks in the code indicate where the possible improvements
	* could be made to realize that.
	*/
	public class JGraphModelAdapter:DefaultGraphModel
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Returns the default edge attributes used for creating new JGraph edges.
		/// 
		/// </summary>
		/// <returns> the default edge attributes used for creating new JGraph edges.
		/// </returns>
		/// <summary> Sets the default edge attributes used for creating new JGraph edges.
		/// 
		/// </summary>
		/// <param name="defaultEdgeAttributes">the default edge attributes to set.
		/// </param>
		virtual public AttributeMap DefaultEdgeAttributes
		{
			get
			{
				return m_defaultEdgeAttributes;
			}
			
			set
			{
				m_defaultEdgeAttributes = value;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Returns the default vertex attributes used for creating new JGraph
		/// vertices.
		/// 
		/// </summary>
		/// <returns> the default vertex attributes used for creating new JGraph
		/// vertices.
		/// </returns>
		/// <summary> Sets the default vertex attributes used for creating new JGraph
		/// vertices.
		/// 
		/// </summary>
		/// <param name="defaultVertexAttributes">the default vertex attributes to set.
		/// </param>
		virtual public AttributeMap DefaultVertexAttributes
		{
			get
			{
				return m_defaultVertexAttributes;
			}
			
			set
			{
				m_defaultVertexAttributes = value;
			}
			
		}
		private const long serialVersionUID = 3256722883706302515L;
		
		/// <summary> The following m_(jCells|jtElement)Being(Added|Removed) sets are used to
		/// prevent bouncing of events between the JGraph and JGraphT listeners.
		/// They ensure that their respective add/remove operations are done
		/// exactly once. Here is an example of how m_jCellsBeingAdded is used when
		/// an edge is added to a JGraph graph:
		/// <pre>
		/// 1. First, we add the desired edge to m_jCellsBeingAdded to indicate
		/// that the edge is being inserted internally.
		/// 2.    Then we invoke the JGraph 'insert' operation.
		/// 3.    The JGraph listener will detect the newly inserted edge.
		/// 4.    It checks if the edge is contained in m_jCellsBeingAdded.
		/// 5.    If yes,
		/// it just removes it and does nothing else.
		/// if no,
		/// it knows that the edge was inserted externally and performs
		/// the insertion.
		/// 6. Lastly, we remove the edge from the m_jCellsBeingAdded.
		/// </pre>
		/// 
		/// <p>
		/// Step 6 is not always required but we do it anyway as a safeguard against
		/// the rare case where the edge to be added is already contained in the
		/// graph and thus NO event will be fired. If 6 is not done, a junk edge
		/// will remain in the m_jCellsBeingAdded set.
		/// </p>
		/// 
		/// <p>
		/// The other sets are used in a similar manner to the above. Apparently,
		/// All that complication could be eliminated if JGraph and JGraphT had
		/// both allowed operations that do not inform listeners...
		/// </p>
		/// </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'm_jCellsBeingAdded '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_TODO: Class 'java.util.HashSet' was converted to 'SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashSet'"
		internal SupportClass.SetSupport m_jCellsBeingAdded = new SupportClass.HashSetSupport();
		//UPGRADE_NOTE: Final was removed from the declaration of 'm_jCellsBeingRemoved '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_TODO: Class 'java.util.HashSet' was converted to 'SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashSet'"
		internal SupportClass.SetSupport m_jCellsBeingRemoved = new SupportClass.HashSetSupport();
		//UPGRADE_NOTE: Final was removed from the declaration of 'm_jtElementsBeingAdded '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_TODO: Class 'java.util.HashSet' was converted to 'SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashSet'"
		internal SupportClass.SetSupport m_jtElementsBeingAdded = new SupportClass.HashSetSupport();
		//UPGRADE_NOTE: Final was removed from the declaration of 'm_jtElementsBeingRemoved '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_TODO: Class 'java.util.HashSet' was converted to 'SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashSet'"
		internal SupportClass.SetSupport m_jtElementsBeingRemoved = new SupportClass.HashSetSupport();
		//UPGRADE_NOTE: Final was removed from the declaration of 'm_cellFactory '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private JGraphModelAdapter.CellFactory m_cellFactory;
		
		/// <summary>Maps JGraph edges to JGraphT edges </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'm_cellToEdge '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		private System.Collections.IDictionary m_cellToEdge = new System.Collections.Hashtable();
		
		/// <summary>Maps JGraph vertices to JGraphT vertices </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'm_cellToVertex '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		private System.Collections.IDictionary m_cellToVertex = new System.Collections.Hashtable();
		private AttributeMap m_defaultEdgeAttributes;
		private AttributeMap m_defaultVertexAttributes;
		
		/// <summary>Maps JGraphT edges to JGraph edges </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'm_edgeToCell '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		private System.Collections.IDictionary m_edgeToCell = new System.Collections.Hashtable();
		
		/// <summary>Maps JGraphT vertices to JGraph vertices </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'm_vertexToCell '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		private System.Collections.IDictionary m_vertexToCell = new System.Collections.Hashtable();
		//UPGRADE_NOTE: Final was removed from the declaration of 'm_jtGraph '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private ShieldedGraph m_jtGraph;
		
		/// <summary> Constructs a new JGraph model adapter for the specified JGraphT graph.
		/// 
		/// </summary>
		/// <param name="jGraphTGraph">the JGraphT graph for which JGraph model adapter to
		/// be created. <code>null</code> is NOT permitted.
		/// </param>
		public JGraphModelAdapter(Graph jGraphTGraph):this(jGraphTGraph, createDefaultVertexAttributes(), createDefaultEdgeAttributes(jGraphTGraph))
		{
		}
		
		
		/// <summary> Constructs a new JGraph model adapter for the specified JGraphT graph.
		/// 
		/// </summary>
		/// <param name="jGraphTGraph">the JGraphT graph for which JGraph model adapter to
		/// be created. <code>null</code> is NOT permitted.
		/// </param>
		/// <param name="defaultVertexAttributes">a default map of JGraph attributes to
		/// format vertices. <code>null</code> is NOT permitted.
		/// </param>
		/// <param name="defaultEdgeAttributes">a default map of JGraph attributes to
		/// format edges. <code>null</code> is NOT permitted.
		/// </param>
		public JGraphModelAdapter(Graph jGraphTGraph, AttributeMap defaultVertexAttributes, AttributeMap defaultEdgeAttributes):this(jGraphTGraph, defaultVertexAttributes, defaultEdgeAttributes, new DefaultCellFactory())
		{
		}
		
		
		/// <summary> Constructs a new JGraph model adapter for the specified JGraphT graph.
		/// 
		/// </summary>
		/// <param name="jGraphTGraph">the JGraphT graph for which JGraph model adapter to
		/// be created. <code>null</code> is NOT permitted.
		/// </param>
		/// <param name="defaultVertexAttributes">a default map of JGraph attributes to
		/// format vertices. <code>null</code> is NOT permitted.
		/// </param>
		/// <param name="defaultEdgeAttributes">a default map of JGraph attributes to
		/// format edges. <code>null</code> is NOT permitted.
		/// </param>
		/// <param name="cellFactory">a {@link CellFactory} to be used to create the JGraph
		/// cells. <code>null</code> is NOT permitted.
		/// 
		/// </param>
		/// <throws>  IllegalArgumentException </throws>
		public JGraphModelAdapter(Graph jGraphTGraph, AttributeMap defaultVertexAttributes, AttributeMap defaultEdgeAttributes, JGraphModelAdapter.CellFactory cellFactory):base()
		{
			
			if (jGraphTGraph == null || defaultVertexAttributes == null || defaultEdgeAttributes == null || cellFactory == null)
			{
				throw new System.ArgumentException("null is NOT permitted");
			}
			
			m_jtGraph = new ShieldedGraph(this, jGraphTGraph);
			DefaultVertexAttributes = defaultVertexAttributes;
			DefaultEdgeAttributes = defaultEdgeAttributes;
			m_cellFactory = cellFactory;
			
			if (jGraphTGraph is ListenableGraph)
			{
				ListenableGraph g = (ListenableGraph) jGraphTGraph;
				g.GraphListenerDelegateVar += new org._3pq.jgrapht.event.GraphListenerDelegate(new JGraphTListener(this).edgeAdded);
				g.GraphListenerDelegateVar += new org._3pq.jgrapht.event.GraphListenerDelegate(new JGraphTListener(this).edgeRemoved);
				g.GraphListenerDelegate2Var += new org._3pq.jgrapht.event.GraphListenerDelegate2(new JGraphTListener(this).vertexAdded);
				g.GraphListenerDelegate2Var += new org._3pq.jgrapht.event.GraphListenerDelegate2(new JGraphTListener(this).vertexRemoved);
			}
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = jGraphTGraph.vertexSet().GetEnumerator(); i.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				handleJGraphTAddedVertex(i.Current);
			}
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = jGraphTGraph.edgeSet().GetEnumerator(); i.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				handleJGraphTAddedEdge((org._3pq.jgrapht.Edge) i.Current);
			}
			
			this.addGraphModelListener(new JGraphListener(this));
		}
		
		/// <summary> Creates and returns a map of attributes to be used as defaults for edge
		/// attributes, depending on the specified graph.
		/// 
		/// </summary>
		/// <param name="jGraphTGraph">the graph for which default edge attributes to be
		/// created.
		/// 
		/// </param>
		/// <returns> a map of attributes to be used as default for edge attributes.
		/// </returns>
		public static AttributeMap createDefaultEdgeAttributes(Graph jGraphTGraph)
		{
			AttributeMap map = new AttributeMap();
			
			if (jGraphTGraph is DirectedGraph)
			{
				GraphConstants.setLineEnd(map, GraphConstants.ARROW_TECHNICAL);
				GraphConstants.setEndFill(map, true);
				GraphConstants.setEndSize(map, 10);
			}
			
			//UPGRADE_ISSUE: Method 'java.awt.Color.decode' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtColordecode_javalangString'"
			GraphConstants.setForeground(map, Color.decode("#25507C"));
			GraphConstants.setFont(map, GraphConstants.DEFAULTFONT.deriveFont((int) System.Drawing.FontStyle.Bold, 12));
			//UPGRADE_ISSUE: Method 'java.awt.Color.decode' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtColordecode_javalangString'"
			GraphConstants.setLineColor(map, Color.decode("#7AA1E6"));
			
			return map;
		}
		
		
		/// <summary> Creates and returns a map of attributes to be used as defaults for
		/// vertex attributes.
		/// 
		/// </summary>
		/// <returns> a map of attributes to be used as defaults for vertex
		/// attributes.
		/// </returns>
		public static AttributeMap createDefaultVertexAttributes()
		{
			AttributeMap map = new AttributeMap();
			//UPGRADE_ISSUE: Method 'java.awt.Color.decode' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtColordecode_javalangString'"
			System.Drawing.Color c = Color.decode("#FF9900");
			
			GraphConstants.setBounds(map, new System.Drawing.RectangleF((float) 50, (float) 50, (float) 90, (float) 30));
			GraphConstants.setBorder(map, System.Windows.Forms.Border3DStyle.Raised);
			GraphConstants.setBackground(map, c);
			GraphConstants.setForeground(map, System.Drawing.Color.White);
			GraphConstants.setFont(map, GraphConstants.DEFAULTFONT.deriveFont((int) System.Drawing.FontStyle.Bold, 12));
			GraphConstants.setOpaque(map, true);
			
			return map;
		}
		
		
		/// <summary> Returns the cell factory used to create the JGraph cells.
		/// 
		/// </summary>
		/// <returns> the cell factory used to create the JGraph cells.
		/// </returns>
		public virtual JGraphModelAdapter.CellFactory getCellFactory()
		{
			return m_cellFactory;
		}
		
		
		/// <summary> Returns the JGraph edge cell that corresponds to the specified JGraphT
		/// edge. If no corresponding cell found, returns <code>null</code>.
		/// 
		/// </summary>
		/// <param name="jGraphTEdge">a JGraphT edge of the JGraphT graph.
		/// 
		/// </param>
		/// <returns> the JGraph edge cell that corresponds to the specified JGraphT
		/// edge, or <code>null</code> if no corresponding cell found.
		/// </returns>
		public virtual DefaultEdge getEdgeCell(org._3pq.jgrapht.Edge jGraphTEdge)
		{
			return (DefaultEdge) m_edgeToCell[jGraphTEdge];
		}
		
		
		/// <summary> Returns the JGraph vertex cell that corresponds to the specified JGraphT
		/// vertex. If no corresponding cell found, returns <code>null</code>.
		/// 
		/// </summary>
		/// <param name="jGraphTVertex">a JGraphT vertex of the JGraphT graph.
		/// 
		/// </param>
		/// <returns> the JGraph vertex cell that corresponds to the specified JGraphT
		/// vertex, or <code>null</code> if no corresponding cell found.
		/// </returns>
		public virtual DefaultGraphCell getVertexCell(System.Object jGraphTVertex)
		{
			return (DefaultGraphCell) m_vertexToCell[jGraphTVertex];
		}
		
		
		/// <summary> Returns the JGraph port cell that corresponds to the specified JGraphT
		/// vertex. If no corresponding port found, returns <code>null</code>.
		/// 
		/// </summary>
		/// <param name="jGraphTVertex">a JGraphT vertex of the JGraphT graph.
		/// 
		/// </param>
		/// <returns> the JGraph port cell that corresponds to the specified JGraphT
		/// vertex, or <code>null</code> if no corresponding cell found.
		/// </returns>
		public virtual DefaultPort getVertexPort(System.Object jGraphTVertex)
		{
			DefaultGraphCell vertexCell = getVertexCell(jGraphTVertex);
			
			if (vertexCell == null)
			{
				return null;
			}
			else
			{
				return (DefaultPort) vertexCell.getChildAt(0);
			}
		}
		
		
		/// <summary> Applies the specified attributes to the model, as in the {@link
		/// DefaultGraphModel#edit(java.util.Map, org.jgraph.graph.ConnectionSet,
		/// org.jgraph.graph.ParentMap, javax.swing.undo.UndoableEdit[])} method.
		/// 
		/// </summary>
		/// <param name="attrs">the attributes to be applied to the model.
		/// 
		/// </param>
		/// <deprecated> this method will be deleted in the future. Use
		/// DefaultGraphModel#edit instead.
		/// </deprecated>
		public virtual void  edit(System.Collections.IDictionary attrs)
		{
			edit(attrs, null, null, null);
		}
		
		
		/// <summary> Adds/removes an edge to/from the underlying JGraphT graph according to
		/// the change in the specified JGraph edge. If both vertices are
		/// connected, we ensure to have a corresponding JGraphT edge. Otherwise,
		/// we ensure NOT to have a corresponding JGraphT edge.
		/// 
		/// <p>
		/// This method is to be called only for edges that have already been
		/// changed in the JGraph graph.
		/// </p>
		/// 
		/// </summary>
		/// <param name="jEdge">the JGraph edge that has changed.
		/// </param>
		internal virtual void  handleJGraphChangedEdge(org.jgraph.graph.Edge jEdge)
		{
			if (isDangling(jEdge))
			{
				if (m_cellToEdge.Contains(jEdge))
				{
					// a non-dangling edge became dangling -- remove the JGraphT
					// edge by faking as if the edge is removed from the JGraph.
					// TODO: Consider keeping the JGraphT edges outside the graph
					// to avoid loosing user data, such as weights.
					handleJGraphRemovedEdge(jEdge);
				}
				else
				{
					// a dangling edge is still dangling -- just ignore.
				}
			}
			else
			{
				// edge is not dangling
				if (m_cellToEdge.Contains(jEdge))
				{
					// edge already has a corresponding JGraphT edge.
					// check if any change to its endpoints.
					org._3pq.jgrapht.Edge jtEdge = (org._3pq.jgrapht.Edge) m_cellToEdge[jEdge];
					
					System.Object jSource = getSourceVertex(this, jEdge);
					System.Object jTarget = getTargetVertex(this, jEdge);
					
					System.Object jtSource = m_cellToVertex[jSource];
					System.Object jtTarget = m_cellToVertex[jTarget];
					
					if (jtEdge.Source == jtSource && jtEdge.Target == jtTarget)
					{
						// no change in edge's endpoints -- nothing to do.
					}
					else
					{
						// edge's end-points have changed -- need to refresh the
						// JGraphT edge. Refresh by faking as if the edge has been
						// removed from JGraph and then added again.
						// ALSO HERE: consider an alternative that maintains user data
						handleJGraphRemovedEdge(jEdge);
						handleJGraphInsertedEdge(jEdge);
					}
				}
				else
				{
					// a new edge
					handleJGraphInsertedEdge(jEdge);
				}
			}
		}
		
		
		/// <summary> Adds to the underlying JGraphT graph an edge that corresponds to the
		/// specified JGraph edge. If the specified JGraph edge is a dangling edge,
		/// it is NOT added to the underlying JGraphT graph.
		/// 
		/// <p>
		/// This method is to be called only for edges that have already been added
		/// to the JGraph graph.
		/// </p>
		/// 
		/// </summary>
		/// <param name="jEdge">the JGraph edge that has been added.
		/// </param>
		internal virtual void  handleJGraphInsertedEdge(org.jgraph.graph.Edge jEdge)
		{
			if (isDangling(jEdge))
			{
				// JGraphT forbid dangling edges so we cannot add the edge yet.
				// If later the edge becomes connected, we will add it.
			}
			else
			{
				System.Object jSource = getSourceVertex(this, jEdge);
				System.Object jTarget = getTargetVertex(this, jEdge);
				
				System.Object jtSource = m_cellToVertex[jSource];
				System.Object jtTarget = m_cellToVertex[jTarget];
				
				org._3pq.jgrapht.Edge jtEdge = m_jtGraph.EdgeFactory.createEdge(jtSource, jtTarget);
				
				bool added = m_jtGraph.addEdge(jtEdge);
				
				if (added)
				{
					m_cellToEdge[jEdge] = jtEdge;
					m_edgeToCell[jtEdge] = jEdge;
				}
				else
				{
					// Adding failed because user is using a JGraphT graph the
					// forbids parallel edges.
					// For consistency, we remove the edge from the JGraph too.
					internalRemoveCell(jEdge);
					System.Console.Error.WriteLine("Warning: an edge was deleted because the underlying " + "JGraphT graph refused to create it. " + "This situation can happen when a constraint of the " + "underlying graph is violated, e.g., an attempt to add " + "a parallel edge or a self-loop to a graph that forbids " + "them. To avoid this message, make sure to use a " + "suitable underlying JGraphT graph.");
				}
			}
		}
		
		
		/// <summary> Adds to the underlying JGraphT graph a vertex corresponding to the
		/// specified JGraph vertex. In JGraph, two vertices with the same user
		/// object are in principle allowed; in JGraphT, this would lead to
		/// duplicate vertices, which is not allowed. So if such vertex already
		/// exists, the specified vertex is REMOVED from the JGraph graph and a a
		/// warning is printed.
		/// 
		/// <p>
		/// This method is to be called only for vertices that have already been
		/// added to the JGraph graph.
		/// </p>
		/// 
		/// </summary>
		/// <param name="jVertex">the JGraph vertex that has been added.
		/// </param>
		internal virtual void  handleJGraphInsertedVertex(GraphCell jVertex)
		{
			System.Object jtVertex;
			
			if (jVertex is DefaultGraphCell)
			{
				jtVertex = ((DefaultGraphCell) jVertex).getUserObject();
			}
			else
			{
				// FIXME: Why toString? Explain if for a good reason otherwise fix.
				jtVertex = jVertex.toString();
			}
			
			if (m_vertexToCell.Contains(jtVertex))
			{
				// We have to remove the new vertex, because it would lead to
				// duplicate vertices. We can't use ShieldedGraph.removeVertex for
				// that, because it would remove the wrong (existing) vertex.
				System.Console.Error.WriteLine("Warning: detected two JGraph vertices with " + "the same JGraphT vertex as user object. It is an " + "indication for a faulty situation that should NOT happen." + "Removing vertex: " + jVertex);
				internalRemoveCell(jVertex);
			}
			else
			{
				m_jtGraph.addVertex(jtVertex);
				
				m_cellToVertex[jVertex] = jtVertex;
				m_vertexToCell[jtVertex] = jVertex;
			}
		}
		
		
		/// <summary> Removes the edge corresponding to the specified JGraph edge from the
		/// JGraphT graph. If the specified edge is not contained in {@link
		/// #m_cellToEdge}, it is silently ignored.
		/// 
		/// <p>
		/// This method is to be called only for edges that have already been
		/// removed from the JGraph graph.
		/// </p>
		/// 
		/// </summary>
		/// <param name="jEdge">the JGraph edge that has been removed.
		/// </param>
		internal virtual void  handleJGraphRemovedEdge(org.jgraph.graph.Edge jEdge)
		{
			if (m_cellToEdge.Contains(jEdge))
			{
				org._3pq.jgrapht.Edge jtEdge = (org._3pq.jgrapht.Edge) m_cellToEdge[jEdge];
				
				m_jtGraph.removeEdge(jtEdge);
				
				m_cellToEdge.Remove(jEdge);
				m_edgeToCell.Remove(jtEdge);
			}
		}
		
		
		/// <summary> Removes the vertex corresponding to the specified JGraph vertex from the
		/// JGraphT graph. If the specified vertex is not contained in {@link
		/// #m_cellToVertex}, it is silently ignored.
		/// 
		/// <p>
		/// If any edges are incident with this vertex, we first remove them from
		/// the both graphs, because otherwise the JGraph graph would leave them
		/// intact and the JGraphT graph would throw them out. TODO: Revise this
		/// behavior now that we gracefully tolerate dangling edges. It might be
		/// possible to remove just the JGraphT edges. The JGraph edges will be
		/// left dangling, as a result.
		/// </p>
		/// 
		/// <p>
		/// This method is to be called only for vertices that have already been
		/// removed from the JGraph graph.
		/// </p>
		/// 
		/// </summary>
		/// <param name="jVertex">the JGraph vertex that has been removed.
		/// </param>
		internal virtual void  handleJGraphRemovedVertex(GraphCell jVertex)
		{
			if (m_cellToVertex.Contains(jVertex))
			{
				System.Object jtVertex = m_cellToVertex[jVertex];
				System.Collections.IList jtIncidentEdges = m_jtGraph.edgesOf(jtVertex);
				
				if (!(jtIncidentEdges.Count == 0))
				{
					// We can't just call removeAllEdges with this list: that
					// would throw a ConcurrentModificationException. So we create
					// a shallow copy.
					// This also triggers removal of the corresponding JGraph edges.
					m_jtGraph.removeAllEdges(new System.Collections.ArrayList(jtIncidentEdges));
				}
				
				m_jtGraph.removeVertex(jtVertex);
				
				m_cellToVertex.Remove(jVertex);
				m_vertexToCell.Remove(jtVertex);
			}
		}
		
		
		/// <summary> Adds the specified JGraphT edge to be reflected by this graph model. To
		/// be called only for edges that already exist in the JGraphT graph.
		/// 
		/// </summary>
		/// <param name="jtEdge">a JGraphT edge to be reflected by this graph model.
		/// </param>
		internal virtual void  handleJGraphTAddedEdge(org._3pq.jgrapht.Edge jtEdge)
		{
			DefaultEdge edgeCell = m_cellFactory.createEdgeCell(jtEdge);
			m_edgeToCell[jtEdge] = edgeCell;
			m_cellToEdge[edgeCell] = jtEdge;
			
			ConnectionSet cs = new ConnectionSet();
			cs.connect(edgeCell, getVertexPort(jtEdge.Source), getVertexPort(jtEdge.Target));
			
			internalInsertCell(edgeCell, createEdgeAttributeMap(edgeCell), cs);
		}
		
		
		/// <summary> Adds the specified JGraphT vertex to be reflected by this graph model.
		/// To be called only for edges that already exist in the JGraphT graph.
		/// 
		/// </summary>
		/// <param name="jtVertex">a JGraphT vertex to be reflected by this graph model.
		/// </param>
		internal virtual void  handleJGraphTAddedVertex(System.Object jtVertex)
		{
			DefaultGraphCell vertexCell = m_cellFactory.createVertexCell(jtVertex);
			vertexCell.add(new DefaultPort());
			
			m_vertexToCell[jtVertex] = vertexCell;
			m_cellToVertex[vertexCell] = jtVertex;
			
			internalInsertCell(vertexCell, createVertexAttributeMap(vertexCell), null);
		}
		
		
		/// <summary> Removes the specified JGraphT vertex from being reflected by this graph
		/// model. To be called only for vertices that have already been removed
		/// from the JGraphT graph.
		/// 
		/// </summary>
		/// <param name="jtVertex">a JGraphT vertex to be removed from being reflected by
		/// this graph model.
		/// </param>
		internal virtual void  handleJGraphTRemoveVertex(System.Object jtVertex)
		{
			System.Object tempObject;
			tempObject = m_vertexToCell[jtVertex];
			m_vertexToCell.Remove(jtVertex);
			DefaultGraphCell vertexCell = (DefaultGraphCell) tempObject;
			m_cellToVertex.Remove(vertexCell);
			
			internalRemoveCell(vertexCell);
			
			// FIXME: Why remove childAt(0)? Explain if correct, otherwise fix.
			if (vertexCell.getChildCount() > 0)
			{
				remove(new System.Object[]{vertexCell.getChildAt(0)});
			}
		}
		
		
		/// <summary> Removes the specified JGraphT edge from being reflected by this graph
		/// model. To be called only for edges that have already been removed from
		/// the JGraphT graph.
		/// 
		/// </summary>
		/// <param name="jtEdge">a JGraphT edge to be removed from being reflected by this
		/// graph model.
		/// </param>
		internal virtual void  handleJGraphTRemovedEdge(org._3pq.jgrapht.Edge jtEdge)
		{
			System.Object tempObject;
			tempObject = m_edgeToCell[jtEdge];
			m_edgeToCell.Remove(jtEdge);
			DefaultEdge edgeCell = (DefaultEdge) tempObject;
			m_cellToEdge.Remove(edgeCell);
			internalRemoveCell(edgeCell);
		}
		
		
		/// <summary> Tests if the specified JGraph edge is 'dangling', that is having at
		/// least one endpoint which is not connected to a vertex.
		/// 
		/// </summary>
		/// <param name="jEdge">the JGraph edge to be tested for being dangling.
		/// 
		/// </param>
		/// <returns> <code>true</code> if the specified edge is dangling, otherwise
		/// <code>false</code>.
		/// </returns>
		private bool isDangling(org.jgraph.graph.Edge jEdge)
		{
			System.Object jSource = getSourceVertex(this, jEdge);
			System.Object jTarget = getTargetVertex(this, jEdge);
			
			return !m_cellToVertex.Contains(jSource) || !m_cellToVertex.Contains(jTarget);
		}
		
		
		private AttributeMap createEdgeAttributeMap(DefaultEdge edgeCell)
		{
			AttributeMap attrs = new AttributeMap();
			attrs.put(edgeCell, DefaultEdgeAttributes.clone());
			
			return attrs;
		}
		
		
		private AttributeMap createVertexAttributeMap(GraphCell vertexCell)
		{
			AttributeMap attrs = new AttributeMap();
			attrs.put(vertexCell, DefaultVertexAttributes.clone());
			
			return attrs;
		}
		
		
		/// <summary> Inserts the specified cell into the JGraph graph model.
		/// 
		/// </summary>
		/// <param name="cell">
		/// </param>
		/// <param name="attrs">
		/// </param>
		/// <param name="cs">
		/// </param>
		private void  internalInsertCell(GraphCell cell, AttributeMap attrs, ConnectionSet cs)
		{
			m_jCellsBeingAdded.Add(cell);
			insert(new System.Object[]{cell}, attrs, cs, null, null);
			m_jCellsBeingAdded.Remove(cell);
		}
		
		
		/// <summary> Removed the specified cell from the JGraph graph model.
		/// 
		/// </summary>
		/// <param name="cell">
		/// </param>
		private void  internalRemoveCell(GraphCell cell)
		{
			m_jCellsBeingRemoved.Add(cell);
			remove(new System.Object[]{cell});
			m_jCellsBeingRemoved.Remove(cell);
		}
		
		/// <summary> Creates the JGraph cells that reflect the respective JGraphT elements.
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// 
		/// </author>
		/// <since> Dec 12, 2003
		/// </since>
		public interface CellFactory
		{
			/// <summary> Creates an edge cell that contains its respective JGraphT edge.
			/// 
			/// </summary>
			/// <param name="jGraphTEdge">a JGraphT edge to be contained.
			/// 
			/// </param>
			/// <returns> an edge cell that contains its respective JGraphT edge.
			/// </returns>
			DefaultEdge createEdgeCell(org._3pq.jgrapht.Edge jGraphTEdge);
			
			
			/// <summary> Creates a vertex cell that contains its respective JGraphT vertex.
			/// 
			/// </summary>
			/// <param name="jGraphTVertex">a JGraphT vertex to be contained.
			/// 
			/// </param>
			/// <returns> a vertex cell that contains its respective JGraphT vertex.
			/// </returns>
			DefaultGraphCell createVertexCell(System.Object jGraphTVertex);
		}
		
		/// <summary> A simple default cell factory.
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// 
		/// </author>
		/// <since> Dec 12, 2003
		/// </since>
		[Serializable]
		public class DefaultCellFactory : JGraphModelAdapter.CellFactory
		{
			private const long serialVersionUID = 3690194343461861173L;
			
			/// <seealso cref="org._3pq.jgrapht.ext.JGraphModelAdapter.CellFactory.createEdgeCell(org._3pq.jgrapht.Edge)">
			/// </seealso>
			public virtual DefaultEdge createEdgeCell(org._3pq.jgrapht.Edge jGraphTEdge)
			{
				return new DefaultEdge(jGraphTEdge);
			}
			
			
			/// <seealso cref="org._3pq.jgrapht.ext.JGraphModelAdapter.CellFactory.createVertexCell(Object)">
			/// </seealso>
			public virtual DefaultGraphCell createVertexCell(System.Object jGraphTVertex)
			{
				return new DefaultGraphCell(jGraphTVertex);
			}
		}
		
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'JGraphListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/// <summary> <p>
		/// Inner class listening to the GraphModel. If something is changed in the
		/// GraphModel, this Listener gets notified and propagates the change back
		/// to the JGraphT graph, if it didn't originate there.
		/// </p>
		/// 
		/// <p>
		/// If this change contains changes that would make this an illegal JGraphT
		/// graph, like adding an edge that is incident with only one vertex, the
		/// illegal parts of the change are undone.
		/// </p>
		/// </summary>
		[Serializable]
		private class JGraphListener : GraphModelListener
		{
			public JGraphListener(JGraphModelAdapter enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(JGraphModelAdapter enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private JGraphModelAdapter enclosingInstance;
			public JGraphModelAdapter Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			private const long serialVersionUID = 3544673988098865209L;
			
			/// <summary> This method is called for all JGraph changes.
			/// 
			/// </summary>
			/// <param name="e">
			/// </param>
			public virtual void  graphChanged(GraphModelEvent e)
			{
				// We first remove edges that have to be removed, then we
				// remove vertices, then we add vertices and finally we add
				// edges. Otherwise, things might go wrong: for example, if we
				// would first remove vertices and then edges, removal of the
				// vertices might induce 'automatic' removal of edges. If we
				// later attempt to re-remove these edges, we get confused.
				GraphModelChange change = e.getChange();
				
				System.Object[] removedCells = change.getRemoved();
				
				if (removedCells != null)
				{
					handleRemovedEdges(filterEdges(removedCells));
					handleRemovedVertices(filterVertices(removedCells));
				}
				
				System.Object[] insertedCells = change.getInserted();
				
				if (insertedCells != null)
				{
					handleInsertedVertices(filterVertices(insertedCells));
					handleInsertedEdges(filterEdges(insertedCells));
				}
				
				// Now handle edges that became 'dangling' or became connected.
				System.Object[] changedCells = change.getChanged();
				
				if (changedCells != null)
				{
					handleChangedEdges(filterEdges(changedCells));
				}
			}
			
			
			/// <summary> Filters a list of edges out of an array of JGraph GraphCell objects.
			/// Other objects are thrown away.
			/// 
			/// </summary>
			/// <param name="cells">Array of cells to be filtered.
			/// 
			/// </param>
			/// <returns> a list of edges.
			/// </returns>
			private System.Collections.IList filterEdges(System.Object[] cells)
			{
				System.Collections.IList jEdges = new System.Collections.ArrayList();
				
				for (int i = 0; i < cells.Length; i++)
				{
					if (cells[i] is org.jgraph.graph.Edge)
					{
						jEdges.Add(cells[i]);
					}
				}
				
				return jEdges;
			}
			
			
			/// <summary> Filters a list of vertices out of an array of JGraph GraphCell
			/// objects. Other objects are thrown away.
			/// 
			/// </summary>
			/// <param name="cells">Array of cells to be filtered.
			/// 
			/// </param>
			/// <returns> a list of vertices.
			/// </returns>
			private System.Collections.IList filterVertices(System.Object[] cells)
			{
				System.Collections.IList jVertices = new System.Collections.ArrayList();
				
				for (int i = 0; i < cells.Length; i++)
				{
					System.Object cell = cells[i];
					
					if (cell is org.jgraph.graph.Edge)
					{
						// ignore -- we don't care about edges.
					}
					else if (cell is Port)
					{
						// ignore -- we don't care about ports.
					}
					else if (cell is DefaultGraphCell)
					{
						DefaultGraphCell graphCell = (DefaultGraphCell) cell;
						
						// If a DefaultGraphCell has a Port as a child, it is a vertex.
						// Note: do not change the order of following conditions;
						// the code uses the short-circuit evaluation of ||.
						if (graphCell.isLeaf() || graphCell.getFirstChild() is Port)
						{
							jVertices.Add(cell);
						}
					}
					else if (cell is GraphCell)
					{
						// If it is not a DefaultGraphCell, it doesn't have
						// children.
						jVertices.Add(cell);
					}
				}
				
				return jVertices;
			}
			
			
			private void  handleChangedEdges(System.Collections.IList jEdges)
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				for (System.Collections.IEnumerator i = jEdges.GetEnumerator(); i.MoveNext(); )
				{
					//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
					org.jgraph.graph.Edge jEdge = (org.jgraph.graph.Edge) i.Current;
					
					handleJGraphChangedEdge(jEdge);
				}
			}
			
			
			private void  handleInsertedEdges(System.Collections.IList jEdges)
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				for (System.Collections.IEnumerator i = jEdges.GetEnumerator(); i.MoveNext(); )
				{
					//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
					org.jgraph.graph.Edge jEdge = (org.jgraph.graph.Edge) i.Current;
					
					System.Boolean tempBoolean;
					tempBoolean = Enclosing_Instance.m_jCellsBeingAdded.Contains(jEdge);
					Enclosing_Instance.m_jCellsBeingAdded.Remove(jEdge);
					if (!tempBoolean)
					{
						handleJGraphInsertedEdge(jEdge);
					}
				}
			}
			
			
			private void  handleInsertedVertices(System.Collections.IList jVertices)
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				for (System.Collections.IEnumerator i = jVertices.GetEnumerator(); i.MoveNext(); )
				{
					//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
					GraphCell jVertex = (GraphCell) i.Current;
					
					System.Boolean tempBoolean;
					tempBoolean = Enclosing_Instance.m_jCellsBeingAdded.Contains(jVertex);
					Enclosing_Instance.m_jCellsBeingAdded.Remove(jVertex);
					if (!tempBoolean)
					{
						handleJGraphInsertedVertex(jVertex);
					}
				}
			}
			
			
			private void  handleRemovedEdges(System.Collections.IList jEdges)
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				for (System.Collections.IEnumerator i = jEdges.GetEnumerator(); i.MoveNext(); )
				{
					//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
					org.jgraph.graph.Edge jEdge = (org.jgraph.graph.Edge) i.Current;
					
					System.Boolean tempBoolean;
					tempBoolean = Enclosing_Instance.m_jCellsBeingRemoved.Contains(jEdge);
					Enclosing_Instance.m_jCellsBeingRemoved.Remove(jEdge);
					if (!tempBoolean)
					{
						handleJGraphRemovedEdge(jEdge);
					}
				}
			}
			
			
			private void  handleRemovedVertices(System.Collections.IList jVertices)
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				for (System.Collections.IEnumerator i = jVertices.GetEnumerator(); i.MoveNext(); )
				{
					//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
					GraphCell jVertex = (GraphCell) i.Current;
					
					System.Boolean tempBoolean;
					tempBoolean = Enclosing_Instance.m_jCellsBeingRemoved.Contains(jVertex);
					Enclosing_Instance.m_jCellsBeingRemoved.Remove(jVertex);
					if (!tempBoolean)
					{
						handleJGraphRemovedVertex(jVertex);
					}
				}
			}
		}
		
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'JGraphTListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/// <summary> A listener on the underlying JGraphT graph. This listener is used to
		/// keep the JGraph model in sync. Whenever one of the event handlers is
		/// called, it first checks whether the change is due to a previous change
		/// in the JGraph model. If it is, then no action is taken.
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// 
		/// </author>
		/// <since> Aug 2, 2003
		/// </since>
		[Serializable]
		private class JGraphTListener : GraphListener
		{
			public JGraphTListener(JGraphModelAdapter enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(JGraphModelAdapter enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private JGraphModelAdapter enclosingInstance;
			public JGraphModelAdapter Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			private const long serialVersionUID = 3616724963609360440L;
			
			/// <seealso cref="GraphListener.edgeAdded(GraphEdgeChangeEvent)">
			/// </seealso>
			public virtual void  edgeAdded(System.Object event_sender, GraphEdgeChangeEvent e)
			{
				org._3pq.jgrapht.Edge jtEdge = e.getEdge();
				
				System.Boolean tempBoolean;
				tempBoolean = Enclosing_Instance.m_jtElementsBeingAdded.Contains(jtEdge);
				Enclosing_Instance.m_jtElementsBeingAdded.Remove(jtEdge);
				if (!tempBoolean)
				{
					Enclosing_Instance.handleJGraphTAddedEdge(jtEdge);
				}
			}
			
			
			/// <seealso cref="GraphListener.edgeRemoved(GraphEdgeChangeEvent)">
			/// </seealso>
			public virtual void  edgeRemoved(System.Object event_sender, GraphEdgeChangeEvent e)
			{
				org._3pq.jgrapht.Edge jtEdge = e.getEdge();
				
				System.Boolean tempBoolean;
				tempBoolean = Enclosing_Instance.m_jtElementsBeingRemoved.Contains(jtEdge);
				Enclosing_Instance.m_jtElementsBeingRemoved.Remove(jtEdge);
				if (!tempBoolean)
				{
					Enclosing_Instance.handleJGraphTRemovedEdge(jtEdge);
				}
			}
			
			
			/// <seealso cref="org._3pq.jgrapht.event.VertexSetListener.vertexAdded(GraphVertexChangeEvent)">
			/// </seealso>
			public virtual void  vertexAdded(System.Object event_sender, GraphVertexChangeEvent e)
			{
				System.Object jtVertex = e.getVertex();
				
				System.Boolean tempBoolean;
				tempBoolean = Enclosing_Instance.m_jtElementsBeingAdded.Contains(jtVertex);
				Enclosing_Instance.m_jtElementsBeingAdded.Remove(jtVertex);
				if (!tempBoolean)
				{
					Enclosing_Instance.handleJGraphTAddedVertex(jtVertex);
				}
			}
			
			
			/// <seealso cref="org._3pq.jgrapht.event.VertexSetListener.vertexRemoved(GraphVertexChangeEvent)">
			/// </seealso>
			public virtual void  vertexRemoved(System.Object event_sender, GraphVertexChangeEvent e)
			{
				System.Object jtVertex = e.getVertex();
				
				System.Boolean tempBoolean;
				tempBoolean = Enclosing_Instance.m_jtElementsBeingRemoved.Contains(jtVertex);
				Enclosing_Instance.m_jtElementsBeingRemoved.Remove(jtVertex);
				if (!tempBoolean)
				{
					Enclosing_Instance.handleJGraphTRemoveVertex(jtVertex);
				}
			}
		}
		
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'ShieldedGraph' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/// <summary> A wrapper around a JGraphT graph that ensures a few atomic operations.</summary>
		private class ShieldedGraph
		{
			private void  InitBlock(JGraphModelAdapter enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private JGraphModelAdapter enclosingInstance;
			virtual internal EdgeFactory EdgeFactory
			{
				get
				{
					return m_graph.EdgeFactory;
				}
				
			}
			public JGraphModelAdapter Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_NOTE: Final was removed from the declaration of 'm_graph '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			private Graph m_graph;
			
			internal ShieldedGraph(JGraphModelAdapter enclosingInstance, Graph graph)
			{
				InitBlock(enclosingInstance);
				m_graph = graph;
			}
			
			
			internal virtual bool addEdge(org._3pq.jgrapht.Edge jtEdge)
			{
				Enclosing_Instance.m_jtElementsBeingAdded.Add(jtEdge);
				
				bool added = m_graph.addEdge(jtEdge);
				Enclosing_Instance.m_jtElementsBeingAdded.Remove(jtEdge);
				
				return added;
			}
			
			
			internal virtual void  addVertex(System.Object jtVertex)
			{
				Enclosing_Instance.m_jtElementsBeingAdded.Add(jtVertex);
				m_graph.addVertex(jtVertex);
				Enclosing_Instance.m_jtElementsBeingAdded.Remove(jtVertex);
			}
			
			
			internal virtual System.Collections.IList edgesOf(System.Object vertex)
			{
				return m_graph.edgesOf(vertex);
			}
			
			
			internal virtual bool removeAllEdges(System.Collections.ICollection edges)
			{
				return m_graph.removeAllEdges(edges);
			}
			
			
			internal virtual void  removeEdge(org._3pq.jgrapht.Edge jtEdge)
			{
				Enclosing_Instance.m_jtElementsBeingRemoved.Add(jtEdge);
				m_graph.removeEdge(jtEdge);
				Enclosing_Instance.m_jtElementsBeingRemoved.Remove(jtEdge);
			}
			
			
			internal virtual void  removeVertex(System.Object jtVertex)
			{
				Enclosing_Instance.m_jtElementsBeingRemoved.Add(jtVertex);
				m_graph.removeVertex(jtVertex);
				Enclosing_Instance.m_jtElementsBeingRemoved.Remove(jtVertex);
			}
		}
	}
}