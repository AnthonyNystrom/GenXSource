/*
* $RCSfile: AxisAngle4f.java,v $
*
* Copyright (c) 2005 Sun Microsystems, Inc. All rights reserved.
*
* Use is subject to license terms.
*
* $Revision: 1.3 $
* $Date: 2005/02/18 16:28:07 $
* $State: Exp $
*/
using System;
namespace javax.vecmath
{
	
	/// <summary> A four-element axis angle represented by single-precision floating point 
	/// x,y,z,angle components.  An axis angle is a rotation of angle (radians)
	/// about the vector (x,y,z).
	/// 
	/// </summary>
	[Serializable]
	public class AxisAngle4f : System.ICloneable
	{
		
		// Compatible with 1.1
		internal const long serialVersionUID = - 163246355858070601L;
		
		/// <summary> The x coordinate.</summary>
		public float x;
		
		/// <summary> The y coordinate.</summary>
		public float y;
		
		/// <summary> The z coordinate.</summary>
		public float z;
		
		/// <summary> The angle of rotation in radians.</summary>
		public float angle;
		
		internal const double EPS = 0.000001;
		
		/// <summary> Constructs and initializes a AxisAngle4f from the specified xyzw coordinates.</summary>
		/// <param name="x">the x coordinate
		/// </param>
		/// <param name="y">the y coordinate
		/// </param>
		/// <param name="z">the z coordinate
		/// </param>
		/// <param name="angle"> the angle of rotation in radians
		/// </param>
		public AxisAngle4f(float x, float y, float z, float angle)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.angle = angle;
		}
		
		
		/// <summary> Constructs and initializes an AxisAngle4f from the array of length 4. </summary>
		/// <param name="a">the array of length 4 containing x,y,z,angle in order
		/// </param>
		public AxisAngle4f(float[] a)
		{
			this.x = a[0];
			this.y = a[1];
			this.z = a[2];
			this.angle = a[3];
		}
		
		
		/// <summary> Constructs and initializes an AxisAngle4f from the specified 
		/// AxisAngle4f.
		/// </summary>
		/// <param name="a1">the AxisAngle4f containing the initialization x y z angle data
		/// </param>
		public AxisAngle4f(AxisAngle4f a1)
		{
			this.x = a1.x;
			this.y = a1.y;
			this.z = a1.z;
			this.angle = a1.angle;
		}
		
		
		/// <summary> Constructs and initializes an AxisAngle4f from the specified AxisAngle4d.</summary>
		/// <param name="a1">the AxisAngle4d containing the initialization x y z angle data
		/// </param>
		public AxisAngle4f(AxisAngle4d a1)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.x = (float) a1.x;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.y = (float) a1.y;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.z = (float) a1.z;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.angle = (float) a1.angle;
		}
		
		
		/// <summary> Constructs and initializes an AxisAngle4f from the specified 
		/// axis and angle.
		/// </summary>
		/// <param name="axis">the axis
		/// </param>
		/// <param name="angle">the angle of rotation in radians
		/// 
		/// </param>
		/// <since> Java 3D 1.2
		/// </since>
		public AxisAngle4f(Vector3f axis, float angle)
		{
			this.x = axis.x;
			this.y = axis.y;
			this.z = axis.z;
			this.angle = angle;
		}
		
		
		/// <summary> Constructs and initializes an AxisAngle4f to (0,0,1,0).</summary>
		public AxisAngle4f()
		{
			this.x = 0.0f;
			this.y = 0.0f;
			this.z = 1.0f;
			this.angle = 0.0f;
		}
		
		
		/// <summary> Sets the value of this axis-angle to the specified x,y,z,angle. </summary>
		/// <param name="x">the x coordinate
		/// </param>
		/// <param name="y">the y coordinate
		/// </param>
		/// <param name="z">the z coordinate
		/// </param>
		/// <param name="angle"> the angle of rotation in radians
		/// </param>
		public void  set_Renamed(float x, float y, float z, float angle)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.angle = angle;
		}
		
		
		/// <summary> Sets the value of this axis-angle to the specified values in the
		/// array of length 4.
		/// </summary>
		/// <param name="a">the array of length 4 containing x,y,z,angle in order
		/// </param>
		public void  set_Renamed(float[] a)
		{
			this.x = a[0];
			this.y = a[1];
			this.z = a[2];
			this.angle = a[3];
		}
		
		
		/// <summary> Sets the value of this axis-angle to the value of axis-angle a1.</summary>
		/// <param name="a1">the axis-angle to be copied
		/// </param>
		public void  set_Renamed(AxisAngle4f a1)
		{
			this.x = a1.x;
			this.y = a1.y;
			this.z = a1.z;
			this.angle = a1.angle;
		}
		
		
		/// <summary> Sets the value of this axis-angle to the value of axis-angle a1.</summary>
		/// <param name="a1">the axis-angle to be copied
		/// </param>
		public void  set_Renamed(AxisAngle4d a1)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.x = (float) a1.x;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.y = (float) a1.y;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.z = (float) a1.z;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.angle = (float) a1.angle;
		}
		
		
		/// <summary> Sets the value of this AxisAngle4f to the specified 
		/// axis and angle.
		/// </summary>
		/// <param name="axis">the axis
		/// </param>
		/// <param name="angle">the angle of rotation in radians
		/// 
		/// </param>
		/// <since> Java 3D 1.2
		/// </since>
		public void  set_Renamed(Vector3f axis, float angle)
		{
			this.x = axis.x;
			this.y = axis.y;
			this.z = axis.z;
			this.angle = angle;
		}
		
		
		/// <summary> Copies the value of this axis-angle into the array a.</summary>
		/// <param name="a">the array 
		/// </param>
		public void  get_Renamed(float[] a)
		{
			a[0] = this.x;
			a[1] = this.y;
			a[2] = this.z;
			a[3] = this.angle;
		}
		
		
		/// <summary> Sets the value of this axis-angle to the rotational equivalent
		/// of the passed quaternion.
		/// If the specified quaternion has no rotational component, the value
		/// of this AxisAngle4f is set to an angle of 0 about an axis of (0,1,0).
		/// </summary>
		/// <param name="q1"> the Quat4f
		/// </param>
		public void  set_Renamed(Quat4f q1)
		{
			double mag = q1.x * q1.x + q1.y * q1.y + q1.z * q1.z;
			
			if (mag > EPS)
			{
				mag = System.Math.Sqrt(mag);
				double invMag = 1.0 / mag;
				
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				x = (float) (q1.x * invMag);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				y = (float) (q1.y * invMag);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				z = (float) (q1.z * invMag);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				angle = (float) (2.0 * System.Math.Atan2(mag, q1.w));
			}
			else
			{
				x = 0.0f;
				y = 1.0f;
				z = 0.0f;
				angle = 0.0f;
			}
		}
		
		
		/// <summary> Sets the value of this axis-angle to the rotational equivalent
		/// of the passed quaternion.
		/// If the specified quaternion has no rotational component, the value
		/// of this AxisAngle4f is set to an angle of 0 about an axis of (0,1,0).
		/// </summary>
		/// <param name="q1"> the Quat4d
		/// </param>
		public void  set_Renamed(Quat4d q1)
		{
			double mag = q1.x * q1.x + q1.y * q1.y + q1.z * q1.z;
			
			if (mag > EPS)
			{
				mag = System.Math.Sqrt(mag);
				double invMag = 1.0 / mag;
				
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				x = (float) (q1.x * invMag);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				y = (float) (q1.y * invMag);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				z = (float) (q1.z * invMag);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				angle = (float) (2.0 * System.Math.Atan2(mag, q1.w));
			}
			else
			{
				x = 0.0f;
				y = 1.0f;
				z = 0.0f;
				angle = 0.0f;
			}
		}
		
		
		/// <summary> Sets the value of this axis-angle to the rotational component of
		/// the passed matrix.
		/// If the specified matrix has no rotational component, the value
		/// of this AxisAngle4f is set to an angle of 0 about an axis of (0,1,0).
		/// </summary>
		/// <param name="m1">the matrix4f
		/// </param>
		public void  set_Renamed(Matrix4f m1)
		{
			Matrix3f m3f = new Matrix3f();
			
			m1.get_Renamed(m3f);
			
			x = m3f.m21 - m3f.m12;
			y = m3f.m02 - m3f.m20;
			z = m3f.m10 - m3f.m01;
			double mag = x * x + y * y + z * z;
			
			if (mag > EPS)
			{
				mag = System.Math.Sqrt(mag);
				double sin = 0.5 * mag;
				double cos = 0.5 * (m3f.m00 + m3f.m11 + m3f.m22 - 1.0);
				
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				angle = (float) System.Math.Atan2(sin, cos);
				double invMag = 1.0 / mag;
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				x = (float) (x * invMag);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				y = (float) (y * invMag);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				z = (float) (z * invMag);
			}
			else
			{
				x = 0.0f;
				y = 1.0f;
				z = 0.0f;
				angle = 0.0f;
			}
		}
		
		
		/// <summary> Sets the value of this axis-angle to the rotational component of
		/// the passed matrix.
		/// If the specified matrix has no rotational component, the value
		/// of this AxisAngle4f is set to an angle of 0 about an axis of (0,1,0).
		/// </summary>
		/// <param name="m1">the matrix4d
		/// </param>
		public void  set_Renamed(Matrix4d m1)
		{
			Matrix3d m3d = new Matrix3d();
			
			m1.get_Renamed(m3d);
			
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			x = (float) (m3d.m21 - m3d.m12);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			y = (float) (m3d.m02 - m3d.m20);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			z = (float) (m3d.m10 - m3d.m01);
			double mag = x * x + y * y + z * z;
			
			if (mag > EPS)
			{
				mag = System.Math.Sqrt(mag);
				double sin = 0.5 * mag;
				double cos = 0.5 * (m3d.m00 + m3d.m11 + m3d.m22 - 1.0);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				angle = (float) System.Math.Atan2(sin, cos);
				
				double invMag = 1.0 / mag;
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				x = (float) (x * invMag);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				y = (float) (y * invMag);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				z = (float) (z * invMag);
			}
			else
			{
				x = 0.0f;
				y = 1.0f;
				z = 0.0f;
				angle = 0.0f;
			}
		}
		
		
		/// <summary> Sets the value of this axis-angle to the rotational component of
		/// the passed matrix.
		/// If the specified matrix has no rotational component, the value
		/// of this AxisAngle4f is set to an angle of 0 about an axis of (0,1,0).
		/// </summary>
		/// <param name="m1">the matrix3f
		/// </param>
		public void  set_Renamed(Matrix3f m1)
		{
			x = (float) (m1.m21 - m1.m12);
			y = (float) (m1.m02 - m1.m20);
			z = (float) (m1.m10 - m1.m01);
			double mag = x * x + y * y + z * z;
			if (mag > EPS)
			{
				mag = System.Math.Sqrt(mag);
				double sin = 0.5 * mag;
				double cos = 0.5 * (m1.m00 + m1.m11 + m1.m22 - 1.0);
				
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				angle = (float) System.Math.Atan2(sin, cos);
				
				double invMag = 1.0 / mag;
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				x = (float) (x * invMag);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				y = (float) (y * invMag);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				z = (float) (z * invMag);
			}
			else
			{
				x = 0.0f;
				y = 1.0f;
				z = 0.0f;
				angle = 0.0f;
			}
		}
		
		
		/// <summary> Sets the value of this axis-angle to the rotational component of
		/// the passed matrix.
		/// If the specified matrix has no rotational component, the value
		/// of this AxisAngle4f is set to an angle of 0 about an axis of (0,1,0).
		/// </summary>
		/// <param name="m1">the matrix3d
		/// </param>
		public void  set_Renamed(Matrix3d m1)
		{
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			x = (float) (m1.m21 - m1.m12);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			y = (float) (m1.m02 - m1.m20);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			z = (float) (m1.m10 - m1.m01);
			double mag = x * x + y * y + z * z;
			
			if (mag > EPS)
			{
				mag = System.Math.Sqrt(mag);
				double sin = 0.5 * mag;
				double cos = 0.5 * (m1.m00 + m1.m11 + m1.m22 - 1.0);
				
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				angle = (float) System.Math.Atan2(sin, cos);
				
				double invMag = 1.0 / mag;
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				x = (float) (x * invMag);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				y = (float) (y * invMag);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				z = (float) (z * invMag);
			}
			else
			{
				x = 0.0f;
				y = 1.0f;
				z = 0.0f;
				angle = 0.0f;
			}
		}
		
		
		/// <summary> Returns a string that contains the values of this AxisAngle4f.
		/// The form is (x,y,z,angle).
		/// </summary>
		/// <returns> the String representation
		/// </returns>
		public override System.String ToString()
		{
			return "(" + this.x + ", " + this.y + ", " + this.z + ", " + this.angle + ")";
		}
		
		
		/// <summary> Returns true if all of the data members of AxisAngle4f a1 are
		/// equal to the corresponding data members in this AxisAngle4f.
		/// </summary>
		/// <param name="a1"> the axis-angle with which the comparison is made
		/// </param>
		/// <returns>  true or false
		/// </returns>
		public bool equals(AxisAngle4f a1)
		{
			try
			{
				return (this.x == a1.x && this.y == a1.y && this.z == a1.z && this.angle == a1.angle);
			}
			catch (System.NullReferenceException e2)
			{
				return false;
			}
		}
		
		/// <summary> Returns true if the Object o1 is of type AxisAngle4f and all of the
		/// data members of o1 are equal to the corresponding data members in
		/// this AxisAngle4f.
		/// </summary>
		/// <param name="o1"> the object with which the comparison is made
		/// </param>
		/// <returns>  true or false
		/// </returns>
		public  override bool Equals(System.Object o1)
		{
			try
			{
				AxisAngle4f a2 = (AxisAngle4f) o1;
				return (this.x == a2.x && this.y == a2.y && this.z == a2.z && this.angle == a2.angle);
			}
			catch (System.NullReferenceException e2)
			{
				return false;
			}
			catch (System.InvalidCastException e1)
			{
				return false;
			}
		}
		
		/// <summary> Returns true if the L-infinite distance between this axis-angle
		/// and axis-angle a1 is less than or equal to the epsilon parameter, 
		/// otherwise returns false.  The L-infinite
		/// distance is equal to 
		/// MAX[abs(x1-x2), abs(y1-y2), abs(z1-z2), abs(angle1-angle2)].
		/// </summary>
		/// <param name="a1"> the axis-angle to be compared to this axis-angle 
		/// </param>
		/// <param name="epsilon"> the threshold value  
		/// </param>
		public virtual bool epsilonEquals(AxisAngle4f a1, float epsilon)
		{
			float diff;
			
			diff = x - a1.x;
			if ((diff < 0?- diff:diff) > epsilon)
				return false;
			
			diff = y - a1.y;
			if ((diff < 0?- diff:diff) > epsilon)
				return false;
			
			diff = z - a1.z;
			if ((diff < 0?- diff:diff) > epsilon)
				return false;
			
			diff = angle - a1.angle;
			if ((diff < 0?- diff:diff) > epsilon)
				return false;
			
			return true;
		}
		
		
		/// <summary> Returns a hash code value based on the data values in this
		/// object.  Two different AxisAngle4f objects with identical data values
		/// (i.e., AxisAngle4f.equals returns true) will return the same hash
		/// code value.  Two objects with different data members may return the
		/// same hash value, although this is not likely.
		/// </summary>
		/// <returns> the integer hash code value
		/// </returns>
        //public override int GetHashCode()
        //{
        //    long bits = 1L;
        //    bits = 31L * bits + (long) VecMathUtil.floatToIntBits(x);
        //    bits = 31L * bits + (long) VecMathUtil.floatToIntBits(y);
        //    bits = 31L * bits + (long) VecMathUtil.floatToIntBits(z);
        //    bits = 31L * bits + (long) VecMathUtil.floatToIntBits(angle);
        //    return (int) (bits ^ (bits >> 32));
        //}
		
		/// <summary> Creates a new object of the same class as this object.
		/// 
		/// </summary>
		/// <returns> a clone of this instance.
		/// </returns>
		/// <exception cref="OutOfMemoryError">if there is not enough memory.
		/// </exception>
		/// <seealso cref="java.lang.Cloneable">
		/// </seealso>
		/// <since> Java 3D 1.3
		/// </since>
		public virtual System.Object Clone()
		{
			// Since there are no arrays we can just use Object.clone()
			try
			{
				return base.MemberwiseClone();
			}
			//UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				// this shouldn't happen, since we are Cloneable
				throw new System.ApplicationException();
			}
		}
	}
}