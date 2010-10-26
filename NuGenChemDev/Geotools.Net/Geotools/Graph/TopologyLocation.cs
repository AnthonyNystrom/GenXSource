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
using System.Collections;
using System.Text;
using Geotools.Geometries;
#endregion

namespace Geotools.Graph
{
	/// <summary>	
	///	A TopologyLocation is the labelling of a
	///	GraphComponent's topological relationship to a single Geometry.	
	///	</summary>
	///	<remarks>
	///	&lt;para&gt;
	///	If the parent component is an area edge, each side and the edge itself
	///	have a topological location.  These location are named
	///	&lt;list type="table"&gt;
	///	&lt;listheader&gt;&lt;term&gt;Items&lt;/term&gt;&lt;description&gt;Descriptions&lt;/description&gt;&lt;/listheader&gt;
	///	&lt;item&gt;&lt;term&gt;On &lt;/term&gt;&lt;description&gt;On the edge.&lt;/description&gt;&lt;/item&gt;
	///	&lt;item&gt;&lt;term&gt;Left&lt;/term&gt;&lt;description&gt;left-hand side of the edge.&lt;/description&gt;&lt;/item&gt;
	///	&lt;item&gt;&lt;term&gt;Right&lt;/term&gt;&lt;description&gt;Right-hand side.&lt;/description&gt;&lt;/item&gt;
	///	&lt;/list&gt;
	///	&lt;/para&gt;&lt;para&gt;
	///	If the parent component is a line edge or node, there is a single
	///	topological relationship attribute, TopologyLocation.On.
	///	&lt;/para&gt;&lt;para&gt;
	///	The possible values of a toplogical location are {Location.Null, Location.Exterior, Location.Boundary, Location.Interior}
	///	&lt;/para&gt;
	///	&lt;para&gt;
	///	The labelling is stored in an array location[j] where
	///	where j has the values On, Left, Right
	///	&lt;/para&gt;
	///	</remarks>

	internal class TopologyLocation
	{
		private int[] _location;

		#region Constructors
		/// <summary>
		/// Constructs a TopologyLocation object.
		/// </summary>
		/// <param name="location"></param>
		public TopologyLocation(int[] location)
		{
			Initialize( location.Length );
		}

		/// <summary>
		/// Constructs a TopologyLocation object.
		/// </summary>
		/// <param name="on"></param>
		/// <param name="left"></param>
		/// <param name="right"></param>
		public TopologyLocation(int on, int left, int right) 
		{
			Initialize(3);
			_location[Position.On] = on;
			_location[Position.Left] = left;
			_location[Position.Right] = right;
		}

		/// <summary>
		/// Constructs a TopologyLocation object.
		/// </summary>
		/// <param name="on"></param>
		public TopologyLocation(int on) 
		{
			Initialize(1);
			_location[Position.On] = on;
		}

		/// <summary>
		/// Constructs a TopologyLocation object.
		/// </summary>
		/// <param name="gl"></param>
		public TopologyLocation(TopologyLocation gl) 
		{
			if (gl != null) 
			{			
				Initialize( gl.Locations.Length );

				for (int i = 0; i < _location.Length; i++) 
				{
					_location[i] = gl.Locations[i];
				}
			}
		}

		/// <summary>
		/// Initialize this object.
		/// </summary>
		/// <param name="size"></param>
		private void Initialize(int size)
		{
			_location = new int[size];
			SetAllLocations( Location.Null );
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the Locations.
		/// </summary>
		public int[] Locations
		{ 
			get
			{
				return _location; 
			}
		}

		/// <summary>
		/// Determines if area type geometry.
		/// </summary>
		public bool IsArea 
		{
			get
			{
				return _location.Length > 1;
			}
		}

		/// <summary>
		/// Determines if line type geometry.
		/// </summary>
		public bool IsLine 
		{ 
			get
			{
				return _location.Length == 1;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Return true if all locations are Null.
		/// </summary>
		/// <returns>True if all locations are null.</returns>
		public bool IsNull()
		{
			for (int i = 0; i < _location.Length; i++) 
			{
				if (_location[i] != Location.Null) return false;
			}
			return true;
		}

		/// <summary>
		/// Return true if any locations are NULL
		/// </summary>
		public bool IsAnyNull()
		{
			foreach(int i in _location)
			{
				if (i == Location.Null)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Gets the location at the specified index.
		/// </summary>
		/// <param name="posIndex"></param>
		/// <returns></returns>
		public int Get( int posIndex )
		{
			if ( posIndex < _location.Length )
			{
				return _location[posIndex];
			}
			return Location.Null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="le"></param>
		/// <param name="locIndex"></param>
		/// <returns></returns>
		public bool IsEqualOnSide( TopologyLocation le, int locIndex )
		{
				return _location[locIndex] == le.Locations[locIndex];
		}

		/// <summary>
		/// Flips the left and right locations.
		/// </summary>
		public void Flip()
		{
			if (_location.Length <= 1)
			{
				return;
			}
			int temp = _location[Position.Left];
			_location[Position.Left] = _location[Position.Right];
			_location[Position.Right] = temp;
		}

		/// <summary>
		/// Sets all location to the specified value.
		/// </summary>
		/// <param name="locValue"></param>
		public void SetAllLocations( int locValue )
		{
			for (int i = 0; i < _location.Length; i++) 
			{
				_location[i] = locValue;
			}
		}


		/// <summary>
		/// Set all numm values to specified locValue.
		/// </summary>
		/// <param name="locValue"></param>
		public void SetAllLocationsIfNull( int locValue )
		{
			for (int i = 0; i < _location.Length; i++) 
			{
				if ( _location[i] == Location.Null )
				{
					_location[i] = locValue;
				}
			}
		}

		/// <summary>
		/// Sets the the location to locValue at the specified locInde.
		/// </summary>
		/// <param name="locIndex"></param>
		/// <param name="locValue"></param>
		public void SetLocation( int locIndex, int locValue )
		{
				_location[locIndex] = locValue;
		}

		/// <summary>
		/// Sets the location for Position.On to locValue.
		/// </summary>
		/// <param name="locValue"></param>
		public void SetLocation( int locValue )
		{
			SetLocation( Position.On, locValue);		// SetLocation( int locIndex, int locValue ) will check for valid index.
		}

		/// <summary>
		/// Sets all location to supplied on, left, and right,m
		/// </summary>
		/// <param name="on"></param>
		/// <param name="left"></param>
		/// <param name="right"></param>
		public void SetLocations( int on, int left, int right ) 
		{
			_location[Position.On] = on;
			_location[Position.Left] = left;
			_location[Position.Right] = right;
		}

		/// <summary>
		/// Sets the locations from TopologyLocation.
		/// </summary>
		/// <param name="gl"></param>
		public void SetLocations( TopologyLocation gl ) 
		{
			for (int i = 0; i < gl.Locations.Length; i++) 
			{
				_location[i] = gl.Locations[i];
			}
		}
		
		/// <summary>
		/// Returns true if all locatios are equal to loc.
		/// </summary>
		/// <param name="loc"></param>
		/// <returns></returns>
		public bool AllPositionsEqual( int loc )
		{
			for (int i = 0; i < _location.Length; i++) 
			{
				if (_location[i] != loc) return false;
			}
			return true;
		}

		/// <summary>
		/// Merge updates only the NULL attributes of this object ith the attributes of another.
		/// </summary>
		/// <param name="gl">The object to merge attributes with.</param>
		public void Merge(TopologyLocation gl)
		{
			// if the src is an Area label & and the dest is not, increase the dest to be an Area
			if (gl.Locations.Length > _location.Length) 
			{
				int [] newLoc = new int[3];
				newLoc[Position.On] = _location[Position.On];
				newLoc[Position.Left] = Location.Null;
				newLoc[Position.Right] = Location.Null;
				_location = newLoc;
			}
			for (int i = 0; i < _location.Length; i++) 
			{
				if (_location[i] == Location.Null && i < gl.Locations.Length)
				{
					_location[i] = gl.Locations[i];
				}
			}
		}

		/// <summary>
		/// Returns string representation of this object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder buf = new StringBuilder();
			if ( _location.Length > 1 ) 
			{
				buf.Append( Location.ToLocationSymbol(_location[Position.Left]) );
			}
			buf.Append( Location.ToLocationSymbol(_location[Position.On]) );
			if (_location.Length > 1)
			{
				buf.Append( Location.ToLocationSymbol(_location[Position.Right]) );
			}
			return buf.ToString();
		}

		#endregion
	}
}
