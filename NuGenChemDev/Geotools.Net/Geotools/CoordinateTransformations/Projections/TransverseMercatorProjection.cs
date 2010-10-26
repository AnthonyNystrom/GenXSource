/*
 *  Copyright (C) 2002 Urban Science Applications, Inc. 
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
using System;
using Geotools.Utilities;

namespace Geotools.CoordinateTransformations
{
	/// <summary>
	/// Summary description for MathTransform.
	/// </summary>
	/// <remarks>
	/// <para>Universal (UTM) and Modified (MTM) Transverses Mercator projections. This
	/// is a cylindrical projection, in which the cylinder has been rotated 90°.
	/// Instead of being tangent to the equator (or to an other standard latitude),
	/// it is tangent to a central meridian. Deformation are more important as we
	/// are going futher from the central meridian. The Transverse Mercator
	/// projection is appropriate for region wich have a greater extent north-south
	/// than east-west.</para>
	/// 
	/// <para>Reference: John P. Snyder (Map Projections - A Working Manual,
	///            U.S. Geological Survey Professional Paper 1395, 1987)</para>
	/// </remarks>
	internal class TransverseMercatorProjection : MapProjection
	{
	
		/* Variables common to all subroutines in this code file
  -----------------------------------------------------*/
		static double r_major;		/* major axis 				*/
		static double r_minor;		/* minor axis 				*/
		static double scale_factor;	/* scale factor				*/
		static double lon_center;	/* Center longitude (projection center) */
		static double lat_origin;	/* center latitude			*/
		static double e0,e1,e2,e3;	/* eccentricity constants		*/
		static double e,es,esp;		/* eccentricity constants		*/
		static double ml0;		/* small value m			*/
		static double false_northing;	/* y offset in meters			*/
		static double false_easting;	/* x offset in meters			*/
		//static double ind;		/* spherical flag			*/

		/// <summary>
		/// Creates an instance of an TransverseMercatorProjection projection object.
		/// </summary>
		/// <param name="parameters">List of parameters to initialize the projection.</param>
		public TransverseMercatorProjection(ParameterList parameters): this(parameters, false)
		{
			
		}
		/// <summary>
		/// Creates an instance of an TransverseMercatorProjection projection object.
		/// </summary>
		/// <param name="parameters">List of parameters to initialize the projection.</param>
		/// <param name="inverse">Flag indicating wether is a forward/projection (false) or an inverse projection (true).</param>
		/// <remarks>
		/// <list type="bullet">
		/// <listheader><term>Items</term><description>Descriptions</description></listheader>
		/// <item><term>semi_major</term><description>Your Description</description></item>
		/// <item><term>semi_minor</term><description>Your Description</description></item>
		/// <item><term>scale_factor_at_natural_origin</term><description>Your Description</description></item>
		/// <item><term>longitude_of_natural_origin</term><description>Your Description</description></item>
		/// <item><term>latitude_of_natural_origin</term><description>Your Description</description></item>
		/// <item><term>false_easting</term><description>Your Description</description></item>
		/// <item><term>false_northing</term><description>Your Description</description></item>
		/// </list>
		/// </remarks>
		public TransverseMercatorProjection(ParameterList parameters, bool inverse): base(parameters, inverse)
		{
			double r_maj=parameters.GetDouble("semi_major");			/* major axis			*/
			double r_min=parameters.GetDouble("semi_minor");			/* minor axis			*/
			double scale_fact=parameters.GetDouble("scale_factor_at_natural_origin");		/* scale factor			*/
			double center_lon=Degrees.ToRadians(parameters.GetDouble("longitude_of_natural_origin"));		/* center longitude		*/
			double center_lat=Degrees.ToRadians(parameters.GetDouble("latitude_of_natural_origin"));		/* center latitude		*/
			double false_east=parameters.GetDouble("false_easting");	/* x offset in meters		*/
			double false_north=parameters.GetDouble("false_northing");		/* y offset in meters		*/

			double temp;			/* temporary variable		*/

			/* Place parameters in static storage for common use
			  -------------------------------------------------*/
			r_major = r_maj;
			r_minor = r_min;
			scale_factor = scale_fact;
			lon_center = center_lon;
			lat_origin = center_lat;
			false_northing = false_north;
			false_easting = false_east;

			temp = r_minor / r_major;
			es = 1.0 - SQUARE(temp);
			e = Math.Sqrt(es);
			e0 = e0fn(es);
			e1 = e1fn(es);
			e2 = e2fn(es);
			e3 = e3fn(es);
			ml0 = r_major * mlfn(e0, e1, e2, e3, lat_origin);
			esp = es / (1.0 - es);

		}

		
		public override void DegreesToMeters(double lon, double lat,out double x, out double y)
		{
			lon = Degrees.ToRadians(lon);
			lat = Degrees.ToRadians(lat);

			double delta_lon=0.0;	/* Delta longitude (Given longitude - center 	*/
			double sin_phi, cos_phi;/* sin and cos value				*/
			double al, als;		/* temporary values				*/
			double c, t, tq;	/* temporary values				*/
			double con, n, ml;	/* cone constant, small m			*/
		
			

			delta_lon = adjust_lon(lon - lon_center);
			sincos(lat, out sin_phi, out cos_phi);


			al  = cos_phi * delta_lon;
			als = SQUARE(al);
			c   = esp * SQUARE(cos_phi);
			tq  = Math.Tan(lat);
			t   = SQUARE(tq);
			con = 1.0 - es * SQUARE(sin_phi);
			n   = r_major / Math.Sqrt(con);
			ml  = r_major * mlfn(e0, e1, e2, e3, lat);

			x  = scale_factor * n * al * (1.0 + als / 6.0 * (1.0 - t + c + als / 20.0 *
				(5.0 - 18.0 * t + SQUARE(t) + 72.0 * c - 58.0 * esp))) + false_easting;

			y  = scale_factor * (ml - ml0 + n * tq * (als * (0.5 + als / 24.0 *
				(5.0 - t + 9.0 * c + 4.0 * SQUARE(c) + als / 30.0 * (61.0 - 58.0 * t
				+ SQUARE(t) + 600.0 * c - 330.0 * esp))))) + false_northing;

			return;//(OK);
		}

		public override void MetersToDegrees(double x, double y,out double lon, out double lat)
		{
			double con,phi;		/* temporary angles				*/
			double delta_phi;	/* difference between longitudes		*/
			long i;			/* counter variable				*/
			double sin_phi, cos_phi, tan_phi;	/* sin cos and tangent values	*/
			double c, cs, t, ts, n, r, d, ds;	/* temporary variables		*/
			//double f, h, g, temp;			/* temporary variables		*/
			long max_iter = 6;			/* maximun number of iterations	*/

	
			x = x - false_easting;
			y = y - false_northing;

			con = (ml0 + y / scale_factor) / r_major;
			phi = con;
			for (i=0;;i++)
			{
				delta_phi = ((con + e1 * Math.Sin(2.0*phi) - e2 * Math.Sin(4.0*phi) + e3 * Math.Sin(6.0*phi))
					/ e0) - phi;
				/*
				   delta_phi = ((con + e1 * sin(2.0*phi) - e2 * sin(4.0*phi)) / e0) - phi;
				*/
				phi += delta_phi;
				if (Math.Abs(delta_phi) <= EPSLN) break;
				if (i >= max_iter) 
				{ 
					throw new TransformException("Latitude failed to converge"); 
					//return(95);
				}
			}
			if (Math.Abs(phi) < HALF_PI)
			{
				sincos(phi, out sin_phi, out cos_phi);
				tan_phi = Math.Tan(phi);
				c    = esp * SQUARE(cos_phi);
				cs   = SQUARE(c);
				t    = SQUARE(tan_phi);
				ts   = SQUARE(t);
				con  = 1.0 - es * SQUARE(sin_phi); 
				n    = r_major / Math.Sqrt(con);
				r    = n * (1.0 - es) / con;
				d    = x / (n * scale_factor);
				ds   = SQUARE(d);
				lat = phi - (n * tan_phi * ds / r) * (0.5 - ds / 24.0 * (5.0 + 3.0 * t + 
					10.0 * c - 4.0 * cs - 9.0 * esp - ds / 30.0 * (61.0 + 90.0 * t +
					298.0 * c + 45.0 * ts - 252.0 * esp - 3.0 * cs)));
				lon = adjust_lon(lon_center + (d * (1.0 - ds / 6.0 * (1.0 + 2.0 * t +
					c - ds / 20.0 * (5.0 - 2.0 * c + 28.0 * t - 3.0 * cs + 8.0 * esp +
					24.0 * ts))) / cos_phi));
			}
			else
			{
				lat = HALF_PI * sign(y);
				lon = lon_center;
			}
			lon = Radians.ToDegrees(lon);
			lat = Radians.ToDegrees(lat);
		}
			
		


		/// <summary>
		/// Returns the inverse of this projection.
		/// </summary>
		/// <returns>IMathTransform that is the reverse of the current projection.</returns>
		public override IMathTransform GetInverse()
		{
			if (_inverse==null)
			{
				_inverse = new TransverseMercatorProjection(this._parameters, ! _isInverse);
			}
			return _inverse;
		}


	}
}
