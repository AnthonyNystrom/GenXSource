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
using Geotools.Geometries;
#endregion

namespace Geotools.Operation.Valid
{
	/// <summary>
	/// Summary description for TopologyValidationError.
	/// </summary>
	public class TopologyValidationError
	{
		public static int Error                   = 0;
		public static int RepeatedPoint          = 1;
		public static int HoleOutsideShell      = 2;
		public static int NestedHoles            = 3;
		public static int DisconnectedInterior   = 4;
		public static int SelfIntersection       = 5;
		public static int RingSelfIntersection  = 6;
		public static int NestedShells           = 7;
		public static int DuplicateRings         = 8;
		public static int TooFewPoints				=9;

		// these messages must synch up with the indexes above
		private static string[] _errMsg = {
											 "Topology Validation Error",
											 "Repeated Point",
											 "Hole lies outside shell",
											 "Holes are nested",
											 "Interior is disconnected",
											 "Self-intersection",
											 "Ring Self-intersection",
											 "Nested shells",
											 "Duplicate Rings",
											 "Too few points in geometry component"
										 };

		private int _errorType;
		private Coordinate _pt;


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the TopologyValidationError class.
		/// </summary>
		public TopologyValidationError(int errorType, Coordinate pt)
		{
			this._errorType = errorType;
			this._pt = (Coordinate) pt.Clone();
		}
		public TopologyValidationError(int errorType)
		{
			this._errorType = errorType;
		}

		#endregion

		#region Properties
		public Coordinate Coordinate
		{
			get
			{
				return _pt; 
			}
		}
		#endregion

		#region Methods
		

		public String GetMessage()
		{
			return _errMsg[_errorType]; 
		}

		public int GteErrorType()
		{
			return _errorType;
		}
		public override string ToString()
		{
			return GetMessage() + " at or near point " + _pt;
		}
		#endregion

	}
}
