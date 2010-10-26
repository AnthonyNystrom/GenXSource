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
using Geotools.Graph;
using Geotools.Algorithms;
#endregion

namespace Geotools.Operation
{
	/// <summary>
	/// Summary description for Operation.
	/// </summary>
	internal class GeometryGraphOperation
	{
		public static CGAlgorithms _cga = new RobustCGAlgorithms();
		public static LineIntersector _li = new RobustLineIntersector();

		// The operation's arguments are placed into an array so they can be accessed by index.
		protected GeometryGraph[] _arg = null;  // the arg(s) of the operation  TODO array of GeometryGraph...

		protected bool _makePrecise;

		/// <summary>
		/// Constructs an Operation object.
		/// </summary>
		/// <param name="g0"></param>
		/// <param name="g1"></param>
		public GeometryGraphOperation(Geometry g0, Geometry g1) 
		{
			SetComputationPrecision( g0.PrecisionModel );

			_arg = new GeometryGraph[2];
			_arg[0] = new GeometryGraph(0, g0);
			_arg[1] = new GeometryGraph(1, g1);
		}

		/// <summary>
		/// Constructs an Operation object.
		/// </summary>
		/// <param name="g0"></param>
		public GeometryGraphOperation(Geometry g0) 
		{
			SetComputationPrecision( g0.PrecisionModel );

			_arg = new GeometryGraph[1];
			_arg[0] = new GeometryGraph(0, g0);;
		}

		/// <summary>
		/// Returns the associated geometry of the specified geometry graph index.
		/// </summary>
		/// <param name="index">Index of the geometry graph array.</param>
		/// <returns>Returns the associated geometry.</returns>
		public Geometry GetArgGeometry(int index) 
		{ 
			return _arg[ index ].Geometry; 
		} // public Geometry GetArgGeometry(int index)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pm"></param>
		protected void SetComputationPrecision( PrecisionModel pm )
		{
			_makePrecise = !pm.IsFloating();
			_li.MakePrecise = _makePrecise;
		}	

	} // public class GeometryGraphOperation
}
