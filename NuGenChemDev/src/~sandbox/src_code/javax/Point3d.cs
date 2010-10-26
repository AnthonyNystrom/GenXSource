/*
* $RCSfile: Point3d.java,v $
*
* Copyright (c) 2005 Sun Microsystems, Inc. All rights reserved.
*
* Use is subject to license terms.
*
* $Revision: 1.2 $
* $Date: 2005/02/18 16:28:10 $
* $State: Exp $
*/
using System;
namespace javax.vecmath
{
	
	/// <summary> A 3 element point that is represented by double precision floating point 
	/// x,y,z coordinates.
	/// 
	/// </summary>
	[Serializable]
	public class Point3d:Tuple3d
	{
		
		// Compatible with 1.1
		internal const long serialVersionUID = 5718062286069042927L;
		
		/// <summary> Constructs and initializes a Point3d from the specified xyz coordinates.</summary>
		/// <param name="x">the x coordinate
		/// </param>
		/// <param name="y">the y coordinate
		/// </param>
		/// <param name="z">the z coordinate
		/// </param>
		public Point3d(double x, double y, double z):base(x, y, z)
		{
		}
		
		
		/// <summary> Constructs and initializes a Point3d from the array of length 3.</summary>
		/// <param name="p">the array of length 3 containing xyz in order
		/// </param>
		public Point3d(double[] p):base(p)
		{
		}
		
		
		/// <summary> Constructs and initializes a Point3d from the specified Point3d.</summary>
		/// <param name="p1">the Point3d containing the initialization x y z data
		/// </param>
		public Point3d(Point3d p1):base(p1)
		{
		}
		
		
		/// <summary> Constructs and initializes a Point3d from the specified Point3f.</summary>
		/// <param name="p1">the Point3f containing the initialization x y z data
		/// </param>
		public Point3d(Point3f p1):base(p1)
		{
		}
		
		
		/// <summary> Constructs and initializes a Point3d from the specified Tuple3f. </summary>
		/// <param name="t1">the Tuple3f containing the initialization x y z data 
		/// </param>
		public Point3d(Tuple3f t1):base(t1)
		{
		}
		
		
		/// <summary> Constructs and initializes a Point3d from the specified Tuple3d.  </summary>
		/// <param name="t1">the Tuple3d containing the initialization x y z data 
		/// </param>
		public Point3d(Tuple3d t1):base(t1)
		{
		}
		
		
		/// <summary> Constructs and initializes a Point3d to (0,0,0).</summary>
		public Point3d():base()
		{
		}
		
		
		/// <summary> Returns the square of the distance between this point and point p1.</summary>
		/// <param name="p1">the other point 
		/// </param>
		/// <returns> the square of the distance
		/// </returns>
		public double distanceSquared(Point3d p1)
		{
			double dx, dy, dz;
			
			dx = this.x - p1.x;
			dy = this.y - p1.y;
			dz = this.z - p1.z;
			return (dx * dx + dy * dy + dz * dz);
		}
		
		
		/// <summary> Returns the distance between this point and point p1.</summary>
		/// <param name="p1">the other point
		/// </param>
		/// <returns> the distance 
		/// </returns>
		public double distance(Point3d p1)
		{
			double dx, dy, dz;
			
			dx = this.x - p1.x;
			dy = this.y - p1.y;
			dz = this.z - p1.z;
			return System.Math.Sqrt(dx * dx + dy * dy + dz * dz);
		}
		
		
		/// <summary> Computes the L-1 (Manhattan) distance between this point and
		/// point p1.  The L-1 distance is equal to:
		/// abs(x1-x2) + abs(y1-y2) + abs(z1-z2).
		/// </summary>
		/// <param name="p1">the other point
		/// </param>
		/// <returns>  the L-1 distance
		/// </returns>
		public double distanceL1(Point3d p1)
		{
			return System.Math.Abs(this.x - p1.x) + System.Math.Abs(this.y - p1.y) + System.Math.Abs(this.z - p1.z);
		}
		
		
		/// <summary> Computes the L-infinite distance between this point and
		/// point p1.  The L-infinite distance is equal to
		/// MAX[abs(x1-x2), abs(y1-y2), abs(z1-z2)].
		/// </summary>
		/// <param name="p1">the other point
		/// </param>
		/// <returns>  the L-infinite distance
		/// </returns>
		public double distanceLinf(Point3d p1)
		{
			double tmp;
			tmp = System.Math.Max(System.Math.Abs(this.x - p1.x), System.Math.Abs(this.y - p1.y));
			
			return System.Math.Max(tmp, System.Math.Abs(this.z - p1.z));
		}
		
		
		/// <summary>  Multiplies each of the x,y,z components of the Point4d parameter 
		/// by 1/w and places the projected values into this point. 
		/// </summary>
		/// <param name="p1"> the source Point4d, which is not modified 
		/// </param>
		public void  project(Point4d p1)
		{
			double oneOw;
			
			oneOw = 1 / p1.w;
			x = p1.x * oneOw;
			y = p1.y * oneOw;
			z = p1.z * oneOw;
		}
	}
}