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
using System.Collections;
using Geotools.CoordinateTransformations;
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	/// A linear ring is a line string whose start and end points have the same coordinates.
	/// </summary>
	public class LinearRing : LineString, ILinearRing
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance LinearRing with the given points.
		/// </summary>
		/// <param name="points">
		/// Points forming a closed and simple linestring, or null or an empty array 
		/// to create the empty geometry. This array must not contain null elements. 
		/// Consecutive  points may not be equal.
		/// </param>
		/// <param name="precisionModel">
		/// The specification of the grid of allowable points for this LinearRing.
		/// </param>
		/// <param name="SRID">
		/// The ID of the Spatial Reference System used by this LinearRing.
		/// </param>
		internal LinearRing(Coordinates points, PrecisionModel precisionModel, int SRID) 
			: base(points,precisionModel,SRID)
		{
			if (!IsEmpty() && !base.IsClosed()) 
			{
				// uses base.IsClosed() since this.IsClosed() always returns true.
				throw new ArgumentException("Points must form a closed linestring.");
			}
			if ((points != null) && (points.Count >= 1 && points.Count <= 3)) 
			{
				throw new ArgumentException("Points must contain 0 or >3 elements.");
			}
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
		
		/// <summary>
		/// By definition a linear ring cannot intersect (except the first and last point) except
		/// for the first and end point, so this property will always return true. 
		/// </summary>
		/// <returns>True if the linear ring is simple.</returns>
		public override bool IsSimple()
		{
			return true;
		}

		///<summary>
		///  Returns the type of this geometry.  
		///</summary>
		///<returns>Returns the type of this geometry.</returns>
		public override string GetGeometryType()
		{
			return "LinearRing";
		}

		/// <summary>
		/// By definition a linar ring is closed so always return true. This is checked in the
		/// constructor.
		/// </summary>
		/// <returns>True if the linear ring is closed.</returns>
		public override bool IsClosed()
		{
			return true;
		}

		/// <summary>
		/// Returns a deep copy of a LinearRing.
		/// </summary>
		/// <returns>A deep copy LinearRing.</returns>
		public override Geometry Clone() 
		{
			LineString ls =  (LineString)base.Clone();
			LinearRing lr = _geometryFactory.CreateLinearRing(ls.GetCoordinates() );
			return lr;// return the clone
		}

		/// <summary>
		/// Creates a projected linear ring.
		/// </summary>
		/// <param name="coordinateTransform">The coordinate transformation to use.</param>
		/// <returns>A projected linear ring.</returns>
		public override Geometry Project(ICoordinateTransformation  coordinateTransform)
		{
			if (coordinateTransform==null)
			{
				throw new ArgumentNullException("coordinateTransform");
			}
			if (!(coordinateTransform.MathTransform is Geotools.CoordinateTransformations.MapProjection))
			{
				throw new ArgumentException("CoordinateTransform must be a MapProjection.");
			}

			int sourceSRID = int.Parse(coordinateTransform.SourceCS.AuthorityCode);
			int targetSRID = int.Parse(coordinateTransform.TargetCS.AuthorityCode);
			MapProjection projection = (MapProjection)coordinateTransform.MathTransform;
			int newSRID = GetNewSRID(coordinateTransform);

			Coordinates projectedCoordinates = new Coordinates();
			double x=0.0;
			double y=0.0;
			Coordinate projectedCoordinate;
			Coordinate external;
			Coordinate coordinate;
			for(int i=0; i < _points.Count; i++)
			{
				coordinate = _points[i];
				external = _geometryFactory.PrecisionModel.ToExternal( coordinate );
				if (this._SRID==sourceSRID)
				{
					projection.MetersToDegrees(external.X, external.Y, out x, out y);	
				}
				else if (this._SRID==targetSRID)
				{
					
					projection.DegreesToMeters(external.X, external.Y, out x, out y);
				}
				projectedCoordinate = _geometryFactory.PrecisionModel.ToInternal(new Coordinate( x, y) );
				projectedCoordinates.Add( projectedCoordinate );
			}
			return new LinearRing( projectedCoordinates, this.PrecisionModel, newSRID);
		}
		#endregion

	}
}
