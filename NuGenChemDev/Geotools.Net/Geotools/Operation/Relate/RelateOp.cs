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
using Geotools.Geometries;
#endregion

namespace Geotools.Operation.Relate
{
	/// <summary>
	/// Summary description for RelateOp.
	/// </summary>
	internal class RelateOp : GeometryGraphOperation
	{	
		private RelateComputer _relate = null;

		#region Constructors
		/// <summary>
		/// Constructs a RelateOp object.
		/// </summary>
		/// <param name="g0"></param>
		/// <param name="g1"></param>
		public RelateOp(Geometry g0, Geometry g1) : base( g0, g1 )
		{
			_relate = new RelateComputer( _arg  );
		}
		#endregion

		#region Static Methods
		/// <summary>
		/// Static method used to calculate the intersection matrix.  This is the same as instantiating the RelateOp object and calling the
		/// GetIntersectionMatrix method.
		/// </summary>
		/// <param name="a">Geometry a to be used in the relation computation.</param>
		/// <param name="b">Geometry b to be used in the relation computation.</param>
		/// <returns>Returns the Intersection Matrix.</returns>
		public static IntersectionMatrix Relate( Geometry a, Geometry b )
		{
			RelateOp relOp = new RelateOp( a, b );
			IntersectionMatrix im = relOp.GetIntersectionMatrix();
			return im;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Returns the Intersection Matrix
		/// </summary>
		/// <returns></returns>
		public IntersectionMatrix GetIntersectionMatrix()
		{
			return _relate.ComputeIM();
		}
		#endregion
	
	}
}
