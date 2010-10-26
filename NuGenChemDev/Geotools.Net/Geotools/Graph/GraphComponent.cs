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
using System.Diagnostics;
using System.Text;
using Geotools.Geometries;
#endregion

namespace Geotools.Graph
{
	/// <summary>
	/// A GraphComponent is the parent class for the objects'
	/// that form a graph.  Each GraphComponent can carry a
	/// Label.
	/// </summary>
	internal abstract class GraphComponent
	{
		
		protected Label _label;

		/// <summary>
		/// Idicates if this component has already been included in the result.
		/// </summary>
		private bool _isInResult = false;
		private bool _isCovered = false;
		private bool _isCoveredSet = false;
		private bool _isVisited = false;

		#region Constructors
		public GraphComponent() 
		{
		}

		public GraphComponent(Label label) 
		{
			_label = label;
		}
		#endregion

		#region Properties

		public Label Label
		{ 
			get
			{
				return _label;
			}
			set
			{
				_label = value;
			}
		}

		/// <summary>
		/// Get or sets a flag that indicates if this component has already been included in the result.
		/// </summary>
		public bool IsInResult
		{ 
			get
			{
				return _isInResult;
			}
			set
			{
				_isInResult = value;
			}
		}

		public bool SetCovered
		{
			get
			{
				return _isCovered;
			}
			set
			{
				_isCovered = value;
				_isCoveredSet = true;
			}
		}

		public bool IsCovered
		{ 
			get
			{
				return _isCovered;
			}
		}

		public bool IsCoveredSet
		{ 
			get
			{
				return _isCoveredSet;
			}
		}

		public bool IsVisited
		{ 
			get
			{
				return _isVisited;
			}
			set
			{
				_isVisited = value;	
			}
		}
		/// <summary>
		/// An isolated component is one that does not intersect or touch any other
		/// component.  This is the case if the label has valid locations for only a
		/// single Geometry. Returns true if this component is isolated.
		/// </summary>
		abstract public bool IsIsolated();
	
		#endregion

		#region Methods
		/// <summary>
		/// Returns a coordinate in this component (or null, if there are none).
		/// </summary>
		/// <returns></returns>
		abstract public Coordinate GetCoordinate();

		/// <summary>
		/// Compute the contribution to an IM for this component.
		/// </summary>
		/// <param name="im"></param>
		abstract protected void ComputeIM(IntersectionMatrix im);

		/// <summary>
		/// Update the IM with the contribution for this component.  A component only contributes
		/// if it has a labeling for both parent geometries.
		/// </summary>
		/// <param name="im"></param>
		public void UpdateIM(IntersectionMatrix im)
		{
			if ( _label.GetGeometryCount() < 2 )
			{
				throw new InvalidOperationException("Found partial label.");
			}
			ComputeIM(im);
		}

		/// <summary>
		/// Returns a string representation of this object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append( "IsInResult:" + _isInResult );
			sb.Append( "IsCovered:" + _isCovered );
			sb.Append( "IsCoveredSet:" + _isCoveredSet );
			sb.Append( "IsVisited:" + _isVisited );
			sb.Append( _label.ToString() );
			return sb.ToString();
		} // public override string ToString()
		#endregion

	} // public abstract class GraphComponent
}
