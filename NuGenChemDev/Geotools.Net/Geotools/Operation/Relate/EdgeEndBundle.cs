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


#region Using
using System;
using System.IO;
using System.Collections;
using System.Text;
using Geotools.Graph;
using Geotools.Geometries;
#endregion

namespace Geotools.Operation.Relate
{
	/// <summary>
	/// Summary description for EdgeEndBundle.
	/// </summary>
	internal class EdgeEndBundle : EdgeEnd
	{
		private ArrayList _edgeEnds = new ArrayList();

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the EdgeEndBundle class.
		/// </summary>
		public EdgeEndBundle( EdgeEnd e ) : base( e.Edge, e.Coordinate, e.DirectedCoordinate, new Label( e.Label ) )
		{
			Insert( e );
		}
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public ArrayList EdgeEnds
		{
			get
			{
				return _edgeEnds;
			}
		}
		#endregion

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public void Insert( EdgeEnd e )
		{
			// Assert: start point is the same
			// Assert: direction is the same
			_edgeEnds.Add( e );
		} // public void Insert( EdgeEnd e )

		/// <summary>
		/// This computes the overall edge label for the set of edges in this EdgeStubBundle.
		/// It essentially merges the ON and side labels for each edge. These labels must be compatible.
		/// </summary>
		public override void ComputeLabel()
		{
			// create the label.  If any of the edges belong to areas,
			// the label must be an area label
			bool isArea = false;
			foreach ( object obj in _edgeEnds ) 
			{
				EdgeEnd e = (EdgeEnd)obj;
				if ( e.Label.IsArea() )
				{
					isArea = true;
				}
			}
			if ( isArea )
			{
				_label = new Label( Location.Null, Location.Null, Location.Null );
			}
			else
			{
				_label = new Label( Location.Null );
			}

			// compute the On label, and the side labels if present
			for ( int i = 0; i < 2; i++ ) 
			{
				ComputeLabelOn( i );
				if ( isArea )
				{
					ComputeLabelSides(i);
				}
			} // for ( int i = 0; i < 2; i++ )

		} // public override void ComputeLabel()

		/// <summary>
		/// Compute the overall ON location for the list of EdgeStubs.
		/// </summary>
		/// <remarks>
		/// (This is essentially equivalent to computing the self-overlay of a single Geometry)
		/// edgeStubs can be either on the boundary (eg Polygon edge)
		/// OR in the interior (e.g. segment of a LineString)of their parent Geometry.
		/// In addition, GeometryCollections use the mod-2 rule to determine
		/// whether a segment is on the boundary or not.
		///	Finally, in GeometryCollections it can still occur that an edge is both
		/// on the boundary and in the interior (e.g. a LineString segment lying on
		/// top of a Polygon edge.) In this case as usual the Boundary is given precendence.
		/// These observations result in the following rules for computing the ON location:
		/// <list type="bullet">
		/// <item><term>if there are an odd number of Bdy edges, the attribute is Bdy</term></item>
		/// <item><term>if there are an even number >= 2 of Bdy edges, the attribute is Int</term></item>
		/// <item><term>if there are any Int edges, the attribute is Int</term></item>
		/// <item><term>otherwise, the attribute is NULL.</term></item>
		/// </list>
		/// </remarks>
		/// <param name="geomIndex"></param>
		private void ComputeLabelOn( int geomIndex )
		{
			// compute the ON location value
			int boundaryCount = 0;
			bool foundInterior = false;
			int loc;

			foreach ( object obj in _edgeEnds ) 
			{
				EdgeEnd e = (EdgeEnd) obj;
				loc = e.Label.GetLocation( geomIndex );
				if ( loc == Location.Boundary )
				{
					boundaryCount++;
				}
				if ( loc == Location.Interior )
				{
					foundInterior = true;
				}
			} // foreach ( object obj in _edgeEnds )

			loc = Location.Null;
			if ( foundInterior )
			{
				loc = Location.Interior;
			}
			if ( boundaryCount > 0 ) 
			{
				loc = GeometryGraph.DetermineBoundary( boundaryCount );
			}
			_label.SetLocation( geomIndex, loc );
		} // private void ComputeLabelOn(int geomIndex)

		/// <summary>
		/// Compute the labelling for each side.
		/// </summary>
		/// <param name="geomIndex"></param>
		private void ComputeLabelSides( int geomIndex )
		{
			ComputeLabelSide( geomIndex, Position.Left );
			ComputeLabelSide( geomIndex, Position.Right );
		} // private void ComputeLabelSides( int geomIndex )

		/// <summary>
		/// To compute the summary label for a side, the algorithm is:
		/// FOR all edges
		///     IF any edge's location is INTERIOR for the side, side location = INTERIOR
		///     ELSE IF there is at least one EXTERIOR attribute, side location = EXTERIOR
		///     ELSE  side location = NULL
		///  Note that it is possible for two sides to have apparently contradictory information
		///  i.e. one edge side may indicate that it is in the interior of a geometry, while
		///  another edge side may indicate the exterior of the same geometry.  This is
		///  not an incompatibility - GeometryCollections may contain two Polygons that touch
		///  along an edge.  This is the reason for Interior-primacy rule above - it
		///  results in the summary label having the Geometry interior on both sides.
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <param name="side"></param>
		private void ComputeLabelSide( int geomIndex, int side )
		{
			foreach ( object obj in _edgeEnds ) 
			{
				EdgeEnd e = (EdgeEnd) obj;
				if ( e.Label.IsArea() ) 
				{
					int loc = e.Label.GetLocation( geomIndex, side );
					if ( loc == Location.Interior ) 
					{
						_label.SetLocation( geomIndex, side, Location.Interior );
						return;
					}
					else if ( loc == Location.Exterior )
					{
						_label.SetLocation( geomIndex, side, Location.Exterior );
					}
				} //if ( e.Label.IsArea() )
			} //foreach ( object obj in _edgeEnds )
		} // private void ComputeLabelSide( int geomIndex, int side )

		/// <summary>
		/// Update the IM with the contribution for the computed label for the EdgeStubs.
		/// </summary>
		/// <param name="im"></param>
		public void UpdateIM(IntersectionMatrix im)
		{
			Edge.UpdateIM( _label, im );
		} // void UpdateIM(IntersectionMatrix im)

		/// <summary>
		/// Returns a string representation of this object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append( "EdgeEndBundle--> Label: " + _label );
			foreach ( object obj in _edgeEnds ) 
			{
				EdgeEnd ee = (EdgeEnd) obj;
				sb.Append( ee.ToString() );
			}
			return sb.ToString();
		} // public override string ToString()

		public override bool Equals( object obj )
		{
			bool returnValue = false;
			EdgeEndBundle eeb = obj as EdgeEndBundle;
			if ( eeb != null )
			{
				if ( _edgeEnds.Count != eeb.EdgeEnds.Count ) return false;
				for ( int i = 0; i < _edgeEnds.Count; i++ )
				{
					if ( !((EdgeEnd)_edgeEnds[i]).Equals( eeb.EdgeEnds[i] ) ) return false;
				}
				// now need to check base class
				if ( !base.Equals( obj ) ) return false;
				returnValue = true;
			}
			return returnValue;
		} // public override bool Equals( object obj )

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		} // public override int GetHashCode()

		#endregion

	} // public class EdgeEndBundle : EdgeEnd
}
