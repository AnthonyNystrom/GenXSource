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
* VisioExporter.java
* ------------------
* (C) Copyright 2003, by Avner Linder and Contributors.
*
* Original Author:  Avner Linder
* Contributor(s):   Barak Naveh
*
* $Id: VisioExporter.java,v 1.2 2004/05/31 19:47:36 barak_naveh Exp $
*
* Changes
* -------
* 27-May-2004 : Initial Version (AL);
*
*/
using System;
using Edge = org._3pq.jgrapht.Edge;
using Graph = org._3pq.jgrapht.Graph;
namespace org._3pq.jgrapht.ext
{
	
	/// <summary> Exports a graph to a csv format that can be imported into MS Visio.
	/// 
	/// <p>
	/// <b>Tip:</b> By default, the exported graph doesn't show link directions. To
	/// show link directions:<br>
	/// 
	/// <ol>
	/// <li>
	/// Select All (Ctrl-A)
	/// </li>
	/// <li>
	/// Right Click the selected items
	/// </li>
	/// <li>
	/// Format/Line...
	/// </li>
	/// <li>
	/// Line ends: End: (choose an arrow)
	/// </li>
	/// </ol>
	/// </p>
	/// 
	/// </summary>
	/// <author>  Avner Linder
	/// </author>
	public class VisioExporter
	{
		public class AnonymousClassVertexNameProvider : VisioExporter.VertexNameProvider
		{
			public virtual System.String getVertexName(System.Object vertex)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				return vertex.ToString();
			}
		}
		//UPGRADE_NOTE: Final was removed from the declaration of 'DEFAULT_VERTEX_NAME_PROVIDER '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'DEFAULT_VERTEX_NAME_PROVIDER' was moved to static method 'org._3pq.jgrapht.ext.VisioExporter'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static readonly VisioExporter.VertexNameProvider DEFAULT_VERTEX_NAME_PROVIDER;
		
		private VisioExporter.VertexNameProvider m_vertexNameProvider;
		
		/// <summary> Creates a new VisioExporter object with the specified naming policy.
		/// 
		/// </summary>
		/// <param name="vertexNameProvider">the vertex name provider to be used for naming
		/// the Visio shapes.
		/// </param>
		public VisioExporter(VisioExporter.VertexNameProvider vertexNameProvider)
		{
			m_vertexNameProvider = vertexNameProvider;
		}
		
		
		/// <summary> Creates a new VisioExporter object.</summary>
		public VisioExporter():this(DEFAULT_VERTEX_NAME_PROVIDER)
		{
		}
		
		/// <summary> Exports the specified graph into a Visio csv file format.
		/// 
		/// </summary>
		/// <param name="output">the print stream to which the graph to be exported.
		/// </param>
		/// <param name="g">the graph to be exported.
		/// </param>
		public virtual void  export(System.IO.Stream output, Graph g)
		{
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.PrintStream' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.IO.StreamWriter out_Renamed = new System.IO.StreamWriter(output);
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = g.vertexSet().GetEnumerator(); i.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				exportVertex(out_Renamed, i.Current);
			}
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = g.edgeSet().GetEnumerator(); i.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				exportEdge(out_Renamed, (Edge) i.Current);
			}
			
			out_Renamed.Flush();
		}
		
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.PrintStream' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private void  exportEdge(System.IO.StreamWriter out_Renamed, Edge edge)
		{
			System.String sourceName = m_vertexNameProvider.getVertexName(edge.Source);
			System.String targetName = m_vertexNameProvider.getVertexName(edge.Target);
			
			out_Renamed.Write("Link,");
			
			// create unique ShapeId for link
			out_Renamed.Write(sourceName);
			out_Renamed.Write("-->");
			out_Renamed.Write(targetName);
			
			// MasterName and Text fields left blank
			out_Renamed.Write(",,,");
			out_Renamed.Write(sourceName);
			out_Renamed.Write(",");
			out_Renamed.Write(targetName);
			out_Renamed.Write("\n");
		}
		
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.PrintStream' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private void  exportVertex(System.IO.StreamWriter out_Renamed, System.Object vertex)
		{
			System.String name = m_vertexNameProvider.getVertexName(vertex);
			
			out_Renamed.Write("Shape,");
			out_Renamed.Write(name);
			out_Renamed.Write(",,"); // MasterName field left empty
			out_Renamed.Write(name);
			out_Renamed.Write("\n");
		}
		
		/// <summary> Assigns a display name for each of the graph vertices.</summary>
		public interface VertexNameProvider
		{
			/// <summary> Returns the shape name for the vertex as to be appeared in the Visio
			/// diagram.
			/// 
			/// </summary>
			/// <param name="vertex">the vertex
			/// 
			/// </param>
			/// <returns> vertex display name for Visio shape.
			/// </returns>
			System.String getVertexName(System.Object vertex);
		}
		static VisioExporter()
		{
			DEFAULT_VERTEX_NAME_PROVIDER = new AnonymousClassVertexNameProvider();
		}
	}
}