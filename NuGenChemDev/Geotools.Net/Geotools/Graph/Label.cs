/*
 *  Copyright (C) 2002 Urban Science Applications, Inc. (translated from Java Topology Suite, 
 *  Copyright 2001 Vivid Solutions)
 *
 *  This library is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  This library is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 */

#region Using statements
using System;
using System.Text;
using Geotools.Geometries;
#endregion

namespace Geotools.Graph
{
	/// <summary>
	/// A Label indicates the topological relationship of a component
	/// of a topology graph to a given Geometry.
  	/// This class supports labels for relationships to two Geometrys,
  	/// which is sufficient for algorithms for binary operations.
  	/// </summary>
  	/// <remarks>
  	/// <para>Topology graphs support the concept of labeling nodes and edges in the graph.
  	/// The label of a node or edge specifies its topological relationship to one or
  	/// more geometries.  (In fact, since JTS operations have only two arguments labels
  	/// are required for only two geometries).  A label for a node or edge has one or
  	/// two elements, depending on whether the node or edge occurs in one or both of the
  	/// input Geometrys.  Elements contain attributes which categorize the
  	/// topological location of the node or edge relative to the parent
  	/// Geometry; that is, whether the node or edge is in the interior,
  	/// boundary or exterior of the Geometry.  Attributes have a value
  	/// from the Set {Interior, Boundary, Exterior}.  In a node each
  	/// element has  a single attribute &lt;On&gt;.  For an edge each element has a
  	/// triplet of attributes &lt;Left, On, Right&gt;.</para>
  	/// <para>
  	/// It is up to the client code to associate the 0 and 1 TopologyLocations
  	/// with specific geometries.</para>
	/// </remarks>
	internal class Label
	{
		/// <summary>
		/// Topological relationship to one or more geometries.  A label for a node or edge has one or
		/// two elements, depending on whether the node or edge occurs in one or both of the input geometies.
		/// </summary>
		protected TopologyLocation[] _elt = new TopologyLocation[2];

		#region Constructors
		/// <summary>
		/// Construct a Label with a single location for both Geometries. Initialize the locations to Null
		/// </summary>
		/// <param name="onLoc"></param>
		public Label( int onLoc )
		{
			_elt[0] = new TopologyLocation(onLoc);
			_elt[1] = new TopologyLocation(onLoc);
		}
		
		/// <summary>
		/// Construct a Label with a single location for both Geometries. Initialize the location for
		/// the Geometry index.
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <param name="onLoc"></param>
		public Label( int geomIndex, int onLoc )
		{
			_elt[0] = new TopologyLocation(Location.Null);
			_elt[1] = new TopologyLocation(Location.Null);
			_elt[geomIndex].SetLocation(onLoc);
		}
		
		/// <summary>
		/// Construct a Label with On, Left and Right locations for both Geometries. Initialize the locations
		/// for both Geometries to the given values.
		/// </summary>
		/// <param name="onLoc"></param>
		/// <param name="leftLoc"></param>
		/// <param name="rightLoc"></param>
		public Label( int onLoc, int leftLoc, int rightLoc )
		{
			_elt[0] = new TopologyLocation(onLoc, leftLoc, rightLoc);
			_elt[1] = new TopologyLocation(onLoc, leftLoc, rightLoc);
		}

		/// <summary>
		/// Construct a Label with On, Left and Right locations for both Geometries.
		/// Initialize the locations for the given Geometry index.
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <param name="onLoc"></param>
		/// <param name="leftLoc"></param>
		/// <param name="rightLoc"></param>
		public Label( int geomIndex, int onLoc, int leftLoc, int rightLoc )
		{
			_elt[0] = new TopologyLocation(Location.Null, Location.Null, Location.Null);
			_elt[1] = new TopologyLocation(Location.Null, Location.Null, Location.Null);
			_elt[geomIndex].SetLocations(onLoc, leftLoc, rightLoc);
		}

		/// <summary>
		/// Construct a Label with the same values as the argument for the given Geometry index.
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <param name="gl"></param>
		public Label( int geomIndex, TopologyLocation gl )
		{

			_elt[0] = new TopologyLocation( gl.Locations );
			_elt[1] = new TopologyLocation( gl.Locations );
			_elt[geomIndex].SetLocations( gl );
		}

		/// <summary>
		/// Construct a Label with the same values as the argument Label.
		/// </summary>
		/// <param name="lbl"></param>
		public Label( Label lbl )
		{
			_elt[0] = new TopologyLocation(lbl.LabelElements[0]);
			_elt[1] = new TopologyLocation(lbl.LabelElements[1]);
		}

		#endregion

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		public TopologyLocation[] LabelElements
		{
			get
			{
				return _elt;
			}
		}
		#endregion

		#region Static Methods
		/// <summary>
		/// Converts a Label to a Line label (that is, one with no side Locations)
		/// </summary>
		/// <param name="label"></param>
		/// <returns></returns>
		public static Label ToLineLabel(Label label)
		{
			Label lineLabel = new Label( Location.Null );
			for (int i = 0; i < 2; i++) 
			{
				lineLabel.SetLocation( i, label.GetLocation(i) );
			}
			return lineLabel;
		}
		#endregion

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		public void Flip()
		{
			_elt[0].Flip();
			_elt[1].Flip(); 
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <param name="posIndex"></param>
		/// <returns></returns>
		public int GetLocation( int geomIndex, int posIndex )
		{ 
			return _elt[geomIndex].Get( posIndex );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <returns></returns>
		public int GetLocation( int geomIndex )
		{ 
			return _elt[geomIndex].Get( Position.On );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <param name="posIndex"></param>
		/// <param name="location"></param>
		public void SetLocation( int geomIndex, int posIndex, int location )
		{
			_elt[geomIndex].SetLocation( posIndex, location );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <param name="location"></param>
		public void SetLocation( int geomIndex, int location )
		{
			_elt[geomIndex].SetLocation( Position.On, location );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <param name="location"></param>
		public void SetAllLocations( int geomIndex, int location )
		{
			_elt[geomIndex].SetAllLocations(location);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <param name="location"></param>
		public void SetAllLocationsIfNull( int geomIndex, int location )
		{
			_elt[geomIndex].SetAllLocationsIfNull(location);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="location"></param>
		public void SetAllLocationsIfNull(int location)
		{
			SetAllLocationsIfNull(0, location);
			SetAllLocationsIfNull(1, location);
		}

		/// <summary>
		/// Merge this label with another one. Merging updates any null attributes of this label
		/// with the attributes from label.
		/// </summary>
		/// <param name="lbl"></param>
		public void Merge( Label lbl )
		{
			for (int i = 0; i < 2; i++) 
			{
				if (_elt[i] == null && lbl.LabelElements[i] != null) 
				{
					_elt[i] = new TopologyLocation( lbl.LabelElements[i] );
				}
				else 
				{
					_elt[i].Merge( lbl.LabelElements[i] );
				}
			}
		}

		private void SetGeometryLocation(int geomIndex, TopologyLocation tl)
		{
			if (tl == null) return;

			_elt[geomIndex].SetLocations(tl);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int GetGeometryCount()
		{
			int count = 0;
			if ( !_elt[0].IsNull() ) count++;
			if ( !_elt[1].IsNull() ) count++;
			return count;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <returns></returns>
		public bool IsNull(int geomIndex)
		{
			return _elt[geomIndex].IsNull();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <returns></returns>
		public bool IsAnyNull(int geomIndex) 
		{
			return _elt[geomIndex].IsAnyNull(); 
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsArea()   
		{
			return _elt[0].IsArea || _elt[1].IsArea; 
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <returns></returns>
		public bool IsArea(int geomIndex)  
		{
			return _elt[geomIndex].IsArea;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <returns></returns>
		public bool IsLine(int geomIndex)  
		{
			return _elt[geomIndex].IsLine;  
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lbl"></param>
		/// <param name="side"></param>
		/// <returns></returns>
		public bool IsEqualOnSide(Label lbl, int side)
		{
			return _elt[0].IsEqualOnSide( lbl.LabelElements[0], side )
				   &&  _elt[1].IsEqualOnSide( lbl.LabelElements[1], side );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <param name="loc"></param>
		/// <returns></returns>
		public bool AllPositionsEqual(int geomIndex, int loc)
		{
			return _elt[geomIndex].AllPositionsEqual( loc );
		}

		/// <summary>
		/// Converts one GeometryLocation to a Line location.
		/// </summary>
		/// <param name="geomIndex"></param>
		public void ToLine(int geomIndex)
		{
			if ( _elt[geomIndex].IsArea )
			{
				_elt[geomIndex] = new TopologyLocation( _elt[geomIndex].Locations[0] );
			}
		}

		/// <summary>
		/// Returns a string representation of this object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder buf = new StringBuilder();
			if ( _elt[0] != null ) 
			{
				buf.Append("a:");
				buf.Append( _elt[0].ToString() );
			}
			if ( _elt[1] != null) 
			{
				buf.Append(" b:");
				buf.Append( _elt[1].ToString() );
			}

			return buf.ToString();
		} // public override String ToString()

		#endregion
	}
}
