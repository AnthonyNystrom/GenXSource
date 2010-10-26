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


#region Using
using System;
using Geotools.CoordinateReferenceSystems;
using Geotools.Positioning;

using Geotools.Utilities;
#endregion




namespace Geotools.CoordinateTransformations
{
	/// <summary>
	/// Projections inherit from this abstract class to get access to useful mathmatical functions.
	/// </summary>
	internal abstract class MapProjection : MathTransform, IProjection
	{
		protected bool _isInverse = false;
		protected bool _isSpherical = false;
		protected double _e;
		protected double _es;
		protected double _semiMajor;
		protected double _semiMinor;
		
		protected ParameterList _parameters;
		protected MathTransform _inverse;

		protected MapProjection( ParameterList parameters, bool isInverse) : this(parameters)
		{
			_isInverse = isInverse;
		}
		
		protected MapProjection( ParameterList parameters)
		{
			_parameters = parameters;
			//todo. Should really convert to the correct linear units??
			this._semiMajor			= _parameters.GetDouble("semi_major");
			this._semiMinor			= _parameters.GetDouble("semi_minor");
		
			this._isSpherical		= (_semiMajor == _semiMinor);
			this._es = 1.0 - (_semiMinor * _semiMinor ) / ( _semiMajor * _semiMajor);
			this._e  = Math.Sqrt(_es);
		}
	
		#region Implementation of IProjection
		public ProjectionParameter GetParameter(int Index)
		{
			throw new NotImplementedException();
		}
		

		public int NumParameters
		{
			get
			{
				return 0;
			}
		}

		

		public string ClassName
		{
			get
			{
				return null;
			}
		}
		#endregion

		#region IMathTransform

		public abstract void MetersToDegrees(double dX, double dY,out double dLongitude, out double dLatitude);
		public abstract void DegreesToMeters(double dLongitude, double dLatitude,out double dX, out double dY);

		public override CoordinatePoint Transform(CoordinatePoint cp)
		{
			CoordinatePoint projectedPoint = new CoordinatePoint();
			projectedPoint.Ord = new double[2];
			if (_isInverse==false)
			{
				double x=0.0;
				double y=0.0;
				double longitude = (double)cp.Ord.GetValue(0);
				double latitude = (double)cp.Ord.GetValue(1);
				
				this.DegreesToMeters(longitude, latitude, out x, out y);
				
				
				projectedPoint.Ord.SetValue(x,0);
				projectedPoint.Ord.SetValue(y,1);
			}
			else
			{
				double x=(double)cp.Ord.GetValue(0);
				double y=(double)cp.Ord.GetValue(1);
				double longitude = 0.0 ;
				double latitude =0.0; 
				
				this.MetersToDegrees(x, y, out longitude, out latitude);

				projectedPoint.Ord.SetValue(longitude,0);
				projectedPoint.Ord.SetValue(latitude,1);
			}
			return projectedPoint;
		}

		public override double[] TransformList(double[] ord)
		{
			if (ord.Length % 2!=0)
			{
				throw new ArgumentException("Array must have an even number of parameters.");
			}
			double[] result = new double[ord.Length];
			double x;
			double y;
			for(int i=0; i<ord.Length; i=i+2)
			{
				x=result[i];
				y=result[i+1];
				CoordinatePoint point = new CoordinatePoint();
				point.Ord = new double[2];
				point.Ord.SetValue(x,0);
				point.Ord.SetValue(y,1);
				CoordinatePoint projectedPoint;	
				if (_isInverse==false)
				{
					projectedPoint = Transform( point);
				}
				else
				{
					projectedPoint = GetInverse().Transform(point);
				}
				result[i] = (double)projectedPoint.Ord.GetValue(0);
				result[i+1] = (double)projectedPoint.Ord.GetValue(1);
			}
			return result;
		}

	




		#endregion

		#region Helper mathmatical functions

		// defines some usefull constants that are used in the projection routines
		/// <summary>
		/// PI
		/// </summary>
		protected const double PI = Math.PI;
		/// <summary>
		/// Half of PI
		/// </summary>
		protected const double HALF_PI = (PI*0.5);
		/// <summary>
		/// PI * 2
		/// </summary>
		protected const double TWO_PI = (PI*2.0);
		/// <summary>
		/// EPSLN
		/// </summary>
		protected const double EPSLN = 1.0e-10;
		/// <summary>
		/// R2D
		/// </summary>
		protected const double R2D = 57.2957795131;
		/// <summary>
		/// D2R
		/// </summary>
		protected const double D2R = 1.745329251994328e-2;
		/// <summary>
		/// S2R
		/// </summary>
		protected const double S2R = 4.848136811095359e-6;
		/// <summary>
		/// MAX_VAL
		/// </summary>
		protected const double MAX_VAL = 4;
		/// <summary>
		/// prjMAXLONG
		/// </summary>
		protected const double prjMAXLONG = 2147483647;
		/// <summary>
		/// DBLLONG
		/// </summary>
		protected const double DBLLONG = 4.61168601e18;

		/// <summary>
		/// Returns the square of a number.
		/// </summary>
		/// <param name="x">The number to be squared.</param>
		protected static double SQUARE(double x)
		{
			return x * x;   /* x**2 */
		}
		/// <summary>
		/// Returns the cube of a number.
		/// </summary>
		/// <param name="x"> </param>
		protected static double CUBE(double x)
		{
			return x * x * x;   /* x**3 */
		}
		/// <summary>
		/// Returns the quad of a number.
		/// </summary>
		/// <param name="x"> </param>
		protected static double QUAD(double x)
		{
			return x * x * x * x ;  /* x**4 */
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="A"></param>
		/// <param name="B"></param>
		/// <returns></returns>
		protected static double GMAX(ref double A,ref double B)
		{
			return ((A) > (B) ? (A) : (B)); /* assign maximum of a and b */
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="A"></param>
		/// <param name="B"></param>
		/// <returns></returns>
		protected static double GMIN(ref double A,ref double B)
		{
			return ((A) < (B) ? (A) : (B)); /* assign minimum of a and b */
		}

		/// <summary>
		/// IMOD
		/// </summary>
		/// <param name="A"></param>
		/// <param name="B"></param>
		/// <returns></returns>
		protected static double IMOD(double A, double B)
		{
			return (A) - (((A) / (B)) * (B)); /* Integer mod function */

		}
		
		///<summary>
		///Function to return the sign of an argument
		///</summary>
		protected static double sign(double x)
		{ 
			if (x < 0.0) 
				return(-1); 
			else 
				return(1);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		protected static double adjust_lon(double x) 
		{
			long count = 0;
			for(;;)
			{
				if (Math.Abs(x)<=PI)
					break;
				else
					if (((long) Math.Abs(x / Math.PI)) < 2)
					x = x-(sign(x) *TWO_PI);
				else
					if (((long) Math.Abs(x / TWO_PI)) < prjMAXLONG)
				{
					x = x-(((long)(x / TWO_PI))*TWO_PI);
				}
				else
					if (((long) Math.Abs(x / (prjMAXLONG * TWO_PI))) < prjMAXLONG)
				{
					x = x-(((long)(x / (prjMAXLONG * TWO_PI))) * (TWO_PI * prjMAXLONG));
				}
				else
					if (((long) Math.Abs(x / (DBLLONG * TWO_PI))) < prjMAXLONG)
				{
					x = x-(((long)(x / (DBLLONG * TWO_PI))) * (TWO_PI * DBLLONG));
				}
				else
					x = x-(sign(x) *TWO_PI);
				count++;
				if (count > MAX_VAL)
					break;
			}
			return(x);
		}
		/// <summary>
		/// Function to compute the constant small m which is the radius of
		/// a parallel of latitude, phi, divided by the semimajor axis.
		/// </summary>
		protected static double msfnz (double eccent, double sinphi, double cosphi)
		{
			double con;

			con = eccent * sinphi;
			return((cosphi / (Math.Sqrt(1.0 - con * con))));
		}
		
		/// <summary>
		/// Function to compute constant small q which is the radius of a 
		/// parallel of latitude, phi, divided by the semimajor axis. 
		/// </summary>
		protected static double qsfnz (double eccent, double sinphi, double cosphi)
		{
			double con;

			if (eccent > 1.0e-7)
			{
				con = eccent * sinphi;
				return (( 1.0- eccent * eccent) * (sinphi /(1.0 - con * con) - (.5/eccent)*
					Math.Log((1.0 - con)/(1.0 + con))));
			}
			else
				return(2.0 * sinphi);
		}

		/// <summary>
		/// Function to calculate the sine and cosine in one call.  Some computer
		/// systems have implemented this function, resulting in a faster implementation
		/// than calling each function separately.  It is provided here for those
		/// computer systems which don`t implement this function
		/// </summary>
		protected static void sincos(double val, out double sin_val, out double cos_val) 

		{ 
			sin_val = Math.Sin(val); 
			cos_val = Math.Cos(val);
		}
		/// <summary>
		/// Function to compute the constant small t for use in the forward
		/// computations in the Lambert Conformal Conic and the Polar
		/// Stereographic projections.
		/// </summary>

		protected static double tsfnz(	double eccent,
			double phi,
			double sinphi
			)
		{
			double con;
			double com;

			con = eccent * sinphi;
			com = .5 * eccent; 
			con = Math.Pow(((1.0 - con) / (1.0 + con)),com);
			return (Math.Tan(.5 * (HALF_PI - phi))/con);
		}
		/// <summary>
		/// To convert degrees to radians, multiply degrees by pi/180. 
		///	conversion factor  0.01746031746032 = pi/180
		/// </summary>
		protected static double Degrees2Radians(double deg)
		{
			return (0.01746031746032 * deg);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rad"></param>
		/// <returns></returns>
		protected static double Radians2Degrees(double rad)
		{
			return ( 57.27272727273 * rad);
		}

		/// <summary>
		/// 
		/// 
		/// </summary>
		/// <param name="eccent"></param>
		/// <param name="qs"></param>
		/// <param name="flag"></param>
		/// <returns></returns>
		protected static double phi1z(double eccent,double qs,out long flag)
		{
			double eccnts;
			double dphi;
			double con;
			double com;
			double sinpi;
			double cospi;
			double phi;
			flag=0;
			//double asinz();
			long i;

			phi = asinz(.5 * qs);
			if (eccent < EPSLN) 
				return(phi);
			eccnts = eccent * eccent; 
			for (i = 1; i <= 25; i++)
			{
				sincos(phi,out sinpi,out cospi);
				con = eccent * sinpi; 
				com = 1.0 - con * con;
				dphi = .5 * com * com / cospi * (qs / (1.0 - eccnts) - sinpi / com + 
					.5 / eccent * Math.Log((1.0 - con) / (1.0 + con)));
				phi = phi + dphi;
				if (Math.Abs(dphi) <= 1e-7)
					return(phi);
			}
			//p_error ("Convergence error","phi1z-conv");
			//ASSERT(FALSE);
			throw new TransformException("Convergence error.");
	

		}

		///<summary>
		///Function to eliminate roundoff errors in asin
		///</summary>

		protected static double asinz (double con)
		{
			if (Math.Abs(con) > 1.0)
			{
				if (con > 1.0)
					con = 1.0;
				else
					con = -1.0;
			}
			return(Math.Asin(con));
		}

 
		/// <summary>Function to compute the latitude angle, phi2, for the inverse of the
		///   Lambert Conformal Conic and Polar Stereographic projections.
		///   </summary>

		protected static double phi2z(double eccent,double ts,out long flag)

			/* Spheroid eccentricity		*/
			/* Constant value t			*/
			/* Error flag number			*/
		
		{
			double eccnth;
			double phi;
			double con;
			double dphi;
			double sinpi;
			long i;

			flag = 0;
			eccnth = .5 * eccent;
			phi = HALF_PI - 2 * Math.Atan(ts);
			for (i = 0; i <= 15; i++)
			{
				sinpi = Math.Sin(phi);
				con = eccent * sinpi;
				dphi = HALF_PI - 2 * Math.Atan(ts *(Math.Pow(((1.0 - con)/(1.0 + con)),eccnth))) -  phi;
				phi += dphi; 
				if (Math.Abs(dphi) <= .0000000001)
					return(phi);
			}
			throw new TransformException("Convergence error - phi2z-conv");
		}


		///<summary>
		///Functions to compute the constants e0, e1, e2, and e3 which are used
		///in a series for calculating the distance along a meridian.  The
		///input x represents the eccentricity squared.
		///</summary>
		protected static double e0fn(double x)
		{
			return(1.0-0.25*x*(1.0+x/16.0*(3.0+1.25*x)));
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		protected static double e1fn(double x)
		{
			return(0.375*x*(1.0+0.25*x*(1.0+0.46875*x)));
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		protected static double e2fn(double x)
		{
			return(0.05859375*x*x*(1.0+0.75*x));
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		protected static double e3fn(double x)
		{
			return(x*x*x*(35.0/3072.0));
		}

		/// <summary>
		/// Function to compute the constant e4 from the input of the eccentricity
		/// of the spheroid, x.  This constant is used in the Polar Stereographic
		/// projection.
		/// </summary>
		protected static double e4fn(double x)

		{
			double con;
			double com;
			con = 1.0 + x;
			com = 1.0 - x;
			return (Math.Sqrt((Math.Pow(con,con))*(Math.Pow(com,com))));
		}

		/// <summary>
		/// Function computes the value of M which is the distance along a meridian
		/// from the Equator to latitude phi.
		/// </summary>
		protected static double mlfn(double e0,double e1,double e2,double e3,double phi) 
		{
			return(e0*phi-e1*Math.Sin(2.0*phi)+e2*Math.Sin(4.0*phi)-e3*Math.Sin(6.0*phi));
		}

		/// <summary>
		/// Function to calculate UTM zone number--NOTE Longitude entered in DEGREES!!!
		/// </summary>
		protected static long calc_utm_zone(double lon)
		{ 
			return((long)(((lon + 180.0) / 6.0) + 1.0)); 
		}
	
		#endregion

		#region Static Methods;
		/// <summary>
		/// Converts a longitude value in degrees to radians.
		/// </summary>
		/// <param name="x">The value in degrees to convert to radians.</param>
		/// <param name="edge">If true, -180 and +180 are valid, otherwise they are considered out of range.</param>
		/// <returns></returns>
		static protected double LongitudeToRadians( double x, bool edge) 
		{
			if (edge ? (x>=Longitude.MinimumValue && x<=Longitude.MaximumValue) : (x>Longitude.MinimumValue && x<Longitude.MaximumValue))
			{
				return Degrees.ToRadians(x);
			}
			throw new ArgumentOutOfRangeException("x",x," not a valid longitude in degrees.");
		}

  
		/// <summary>
		/// Converts a latitude value in degrees to radians.
		/// </summary>
		/// <param name="y">The value in degrees to to radians.</param>
		/// <param name="edge">If true, -90 and +90 are valid, otherwise they are considered out of range.</param>
		/// <returns></returns>
		static protected double LatitudeToRadians(double y, bool edge)
		{
			if (edge ? (y>=Latitude.MinimumValue && y<=Latitude.MaximumValue) : (y>Latitude.MinimumValue && y<Latitude.MaximumValue))
			{
				return Degrees.ToRadians(y );
			}
			throw new ArgumentOutOfRangeException("x",y," not a valid latitude in degrees.");
		}
		#endregion

		public string Abbreviation
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		public string Alias
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		public string Authority
		{
			get
			{
				throw new NotImplementedException();
			}

		}
		public string AuthorityCode
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		public string Name
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		public string Remarks
		{
			get
			{
				throw new NotImplementedException();
			}
		}
	}
}
