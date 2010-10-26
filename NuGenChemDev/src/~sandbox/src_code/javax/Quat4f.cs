/*
* $RCSfile: Quat4f.java,v $
*
* Copyright (c) 2005 Sun Microsystems, Inc. All rights reserved.
*
* Use is subject to license terms.
*
* $Revision: 1.3 $
* $Date: 2005/02/18 16:28:14 $
* $State: Exp $
*/
using System;
namespace javax.vecmath
{
	
	/// <summary> A 4 element unit quaternion represented by single precision floating 
	/// point x,y,z,w coordinates.  The quaternion is always normalized.
	/// 
	/// </summary>
	[Serializable]
	public class Quat4f:Tuple4f
	{
		
		// Combatible with 1.1
		internal const long serialVersionUID = 2675933778405442383L;
		
		internal const double EPS = 0.000001;
		internal const double EPS2 = 1.0e-30;
		internal const double PIO2 = 1.57079632679;
		
		/// <summary> Constructs and initializes a Quat4f from the specified xyzw coordinates.</summary>
		/// <param name="x">the x coordinate
		/// </param>
		/// <param name="y">the y coordinate
		/// </param>
		/// <param name="z">the z coordinate
		/// </param>
		/// <param name="w">the w scalar component
		/// </param>
		public Quat4f(float x, float y, float z, float w)
		{
			float mag;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			mag = (float) (1.0 / System.Math.Sqrt(x * x + y * y + z * z + w * w));
			this.x = x * mag;
			this.y = y * mag;
			this.z = z * mag;
			this.w = w * mag;
		}
		
		/// <summary> Constructs and initializes a Quat4f from the array of length 4. </summary>
		/// <param name="q">the array of length 4 containing xyzw in order
		/// </param>
		public Quat4f(float[] q)
		{
			float mag;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			mag = (float) (1.0 / System.Math.Sqrt(q[0] * q[0] + q[1] * q[1] + q[2] * q[2] + q[3] * q[3]));
			x = q[0] * mag;
			y = q[1] * mag;
			z = q[2] * mag;
			w = q[3] * mag;
		}
		
		
		/// <summary> Constructs and initializes a Quat4f from the specified Quat4f.</summary>
		/// <param name="q1">the Quat4f containing the initialization x y z w data
		/// </param>
		public Quat4f(Quat4f q1):base(q1)
		{
		}
		
		/// <summary> Constructs and initializes a Quat4f from the specified Quat4d.</summary>
		/// <param name="q1">the Quat4d containing the initialization x y z w data
		/// </param>
		public Quat4f(Quat4d q1):base(q1)
		{
		}
		
		
		/// <summary> Constructs and initializes a Quat4f from the specified Tuple4f. </summary>
		/// <param name="t1">the Tuple4f containing the initialization x y z w data 
		/// </param>
		public Quat4f(Tuple4f t1)
		{
			float mag;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			mag = (float) (1.0 / System.Math.Sqrt(t1.x * t1.x + t1.y * t1.y + t1.z * t1.z + t1.w * t1.w));
			x = t1.x * mag;
			y = t1.y * mag;
			z = t1.z * mag;
			w = t1.w * mag;
		}
		
		
		/// <summary> Constructs and initializes a Quat4f from the specified Tuple4d.  </summary>
		/// <param name="t1">the Tuple4d containing the initialization x y z w data 
		/// </param>
		public Quat4f(Tuple4d t1)
		{
			double mag;
			mag = 1.0 / System.Math.Sqrt(t1.x * t1.x + t1.y * t1.y + t1.z * t1.z + t1.w * t1.w);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			x = (float) (t1.x * mag);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			y = (float) (t1.y * mag);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			z = (float) (t1.z * mag);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			w = (float) (t1.w * mag);
		}
		
		
		/// <summary> Constructs and initializes a Quat4f to (0.0,0.0,0.0,0.0).</summary>
		public Quat4f():base()
		{
		}
		
		
		/// <summary> Sets the value of this quaternion to the conjugate of quaternion q1.</summary>
		/// <param name="q1">the source vector
		/// </param>
		public void  conjugate(Quat4f q1)
		{
			this.x = - q1.x;
			this.y = - q1.y;
			this.z = - q1.z;
			this.w = q1.w;
		}
		
		/// <summary> Sets the value of this quaternion to the conjugate of itself.</summary>
		public void  conjugate()
		{
			this.x = - this.x;
			this.y = - this.y;
			this.z = - this.z;
		}
		
		
		/// <summary> Sets the value of this quaternion to the quaternion product of
		/// quaternions q1 and q2 (this = q1 * q2).  
		/// Note that this is safe for aliasing (e.g. this can be q1 or q2).
		/// </summary>
		/// <param name="q1">the first quaternion
		/// </param>
		/// <param name="q2">the second quaternion
		/// </param>
		public void  mul(Quat4f q1, Quat4f q2)
		{
			if (this != q1 && this != q2)
			{
				this.w = q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z;
				this.x = q1.w * q2.x + q2.w * q1.x + q1.y * q2.z - q1.z * q2.y;
				this.y = q1.w * q2.y + q2.w * q1.y - q1.x * q2.z + q1.z * q2.x;
				this.z = q1.w * q2.z + q2.w * q1.z + q1.x * q2.y - q1.y * q2.x;
			}
			else
			{
				float x, y, w;
				
				w = q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z;
				x = q1.w * q2.x + q2.w * q1.x + q1.y * q2.z - q1.z * q2.y;
				y = q1.w * q2.y + q2.w * q1.y - q1.x * q2.z + q1.z * q2.x;
				this.z = q1.w * q2.z + q2.w * q1.z + q1.x * q2.y - q1.y * q2.x;
				this.w = w;
				this.x = x;
				this.y = y;
			}
		}
		
		
		/// <summary> Sets the value of this quaternion to the quaternion product of
		/// itself and q1 (this = this * q1).  
		/// </summary>
		/// <param name="q1">the other quaternion
		/// </param>
		public void  mul(Quat4f q1)
		{
			float x, y, w;
			
			w = this.w * q1.w - this.x * q1.x - this.y * q1.y - this.z * q1.z;
			x = this.w * q1.x + q1.w * this.x + this.y * q1.z - this.z * q1.y;
			y = this.w * q1.y + q1.w * this.y - this.x * q1.z + this.z * q1.x;
			this.z = this.w * q1.z + q1.w * this.z + this.x * q1.y - this.y * q1.x;
			this.w = w;
			this.x = x;
			this.y = y;
		}
		
		
		/// <summary> Multiplies quaternion q1 by the inverse of quaternion q2 and places
		/// the value into this quaternion.  The value of both argument quaternions 
		/// is preservered (this = q1 * q2^-1).
		/// </summary>
		/// <param name="q1">the first quaternion
		/// </param>
		/// <param name="q2">the second quaternion
		/// </param>
		public void  mulInverse(Quat4f q1, Quat4f q2)
		{
			Quat4f tempQuat = new Quat4f(q2);
			
			tempQuat.inverse();
			this.mul(q1, tempQuat);
		}
		
		
		
		/// <summary> Multiplies this quaternion by the inverse of quaternion q1 and places
		/// the value into this quaternion.  The value of the argument quaternion
		/// is preserved (this = this * q^-1).
		/// </summary>
		/// <param name="q1">the other quaternion
		/// </param>
		public void  mulInverse(Quat4f q1)
		{
			Quat4f tempQuat = new Quat4f(q1);
			
			tempQuat.inverse();
			this.mul(tempQuat);
		}
		
		
		
		/// <summary> Sets the value of this quaternion to quaternion inverse of quaternion q1.</summary>
		/// <param name="q1">the quaternion to be inverted
		/// </param>
		public void  inverse(Quat4f q1)
		{
			float norm;
			
			norm = 1.0f / (q1.w * q1.w + q1.x * q1.x + q1.y * q1.y + q1.z * q1.z);
			this.w = norm * q1.w;
			this.x = (- norm) * q1.x;
			this.y = (- norm) * q1.y;
			this.z = (- norm) * q1.z;
		}
		
		
		/// <summary> Sets the value of this quaternion to the quaternion inverse of itself.</summary>
		public void  inverse()
		{
			float norm;
			
			norm = 1.0f / (this.w * this.w + this.x * this.x + this.y * this.y + this.z * this.z);
			this.w *= norm;
			this.x *= (- norm);
			this.y *= (- norm);
			this.z *= (- norm);
		}
		
		
		/// <summary> Sets the value of this quaternion to the normalized value
		/// of quaternion q1.
		/// </summary>
		/// <param name="q1">the quaternion to be normalized.
		/// </param>
		public void  normalize(Quat4f q1)
		{
			float norm;
			
			norm = (q1.x * q1.x + q1.y * q1.y + q1.z * q1.z + q1.w * q1.w);
			
			if (norm > 0.0f)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				norm = 1.0f / (float) System.Math.Sqrt(norm);
				this.x = norm * q1.x;
				this.y = norm * q1.y;
				this.z = norm * q1.z;
				this.w = norm * q1.w;
			}
			else
			{
				this.x = (float) 0.0;
				this.y = (float) 0.0;
				this.z = (float) 0.0;
				this.w = (float) 0.0;
			}
		}
		
		
		/// <summary> Normalizes the value of this quaternion in place.</summary>
		public void  normalize()
		{
			float norm;
			
			norm = (this.x * this.x + this.y * this.y + this.z * this.z + this.w * this.w);
			
			if (norm > 0.0f)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				norm = 1.0f / (float) System.Math.Sqrt(norm);
				this.x *= norm;
				this.y *= norm;
				this.z *= norm;
				this.w *= norm;
			}
			else
			{
				this.x = (float) 0.0;
				this.y = (float) 0.0;
				this.z = (float) 0.0;
				this.w = (float) 0.0;
			}
		}
		
		
		/// <summary> Sets the value of this quaternion to the rotational component of
		/// the passed matrix.
		/// </summary>
		/// <param name="m1">the Matrix4f
		/// </param>
		public void  set_Renamed(Matrix4f m1)
		{
			float ww = 0.25f * (m1.m00 + m1.m11 + m1.m22 + m1.m33);
			
			if (ww >= 0)
			{
				if (ww >= EPS2)
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					this.w = (float) System.Math.Sqrt((double) ww);
					ww = 0.25f / this.w;
					this.x = (m1.m21 - m1.m12) * ww;
					this.y = (m1.m02 - m1.m20) * ww;
					this.z = (m1.m10 - m1.m01) * ww;
					return ;
				}
			}
			else
			{
				this.w = 0;
				this.x = 0;
				this.y = 0;
				this.z = 1;
				return ;
			}
			
			this.w = 0;
			ww = (- 0.5f) * (m1.m11 + m1.m22);
			
			if (ww >= 0)
			{
				if (ww >= EPS2)
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					this.x = (float) System.Math.Sqrt((double) ww);
					ww = 1.0f / (2.0f * this.x);
					this.y = m1.m10 * ww;
					this.z = m1.m20 * ww;
					return ;
				}
			}
			else
			{
				this.x = 0;
				this.y = 0;
				this.z = 1;
				return ;
			}
			
			this.x = 0;
			ww = 0.5f * (1.0f - m1.m22);
			
			if (ww >= EPS2)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				this.y = (float) System.Math.Sqrt((double) ww);
				this.z = m1.m21 / (2.0f * this.y);
				return ;
			}
			
			this.y = 0;
			this.z = 1;
		}
		
		
		/// <summary> Sets the value of this quaternion to the rotational component of
		/// the passed matrix.
		/// </summary>
		/// <param name="m1">the Matrix4d
		/// </param>
		public void  set_Renamed(Matrix4d m1)
		{
			double ww = 0.25 * (m1.m00 + m1.m11 + m1.m22 + m1.m33);
			
			if (ww >= 0)
			{
				if (ww >= EPS2)
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					this.w = (float) System.Math.Sqrt(ww);
					ww = 0.25 / this.w;
					this.x = (float) ((m1.m21 - m1.m12) * ww);
					this.y = (float) ((m1.m02 - m1.m20) * ww);
					this.z = (float) ((m1.m10 - m1.m01) * ww);
					return ;
				}
			}
			else
			{
				this.w = 0;
				this.x = 0;
				this.y = 0;
				this.z = 1;
				return ;
			}
			
			this.w = 0;
			ww = (- 0.5) * (m1.m11 + m1.m22);
			if (ww >= 0)
			{
				if (ww >= EPS2)
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					this.x = (float) System.Math.Sqrt(ww);
					ww = 0.5 / this.x;
					this.y = (float) (m1.m10 * ww);
					this.z = (float) (m1.m20 * ww);
					return ;
				}
			}
			else
			{
				this.x = 0;
				this.y = 0;
				this.z = 1;
				return ;
			}
			
			this.x = 0;
			ww = 0.5 * (1.0 - m1.m22);
			if (ww >= EPS2)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				this.y = (float) System.Math.Sqrt(ww);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				this.z = (float) (m1.m21 / (2.0 * (double) (this.y)));
				return ;
			}
			
			this.y = 0;
			this.z = 1;
		}
		
		
		/// <summary> Sets the value of this quaternion to the rotational component of
		/// the passed matrix.
		/// </summary>
		/// <param name="m1">the Matrix3f
		/// </param>
		public void  set_Renamed(Matrix3f m1)
		{
			float ww = 0.25f * (m1.m00 + m1.m11 + m1.m22 + 1.0f);
			
			if (ww >= 0)
			{
				if (ww >= EPS2)
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					this.w = (float) System.Math.Sqrt((double) ww);
					ww = 0.25f / this.w;
					this.x = (m1.m21 - m1.m12) * ww;
					this.y = (m1.m02 - m1.m20) * ww;
					this.z = (m1.m10 - m1.m01) * ww;
					return ;
				}
			}
			else
			{
				this.w = 0;
				this.x = 0;
				this.y = 0;
				this.z = 1;
				return ;
			}
			
			this.w = 0;
			ww = (- 0.5f) * (m1.m11 + m1.m22);
			if (ww >= 0)
			{
				if (ww >= EPS2)
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					this.x = (float) System.Math.Sqrt((double) ww);
					ww = 0.5f / this.x;
					this.y = m1.m10 * ww;
					this.z = m1.m20 * ww;
					return ;
				}
			}
			else
			{
				this.x = 0;
				this.y = 0;
				this.z = 1;
				return ;
			}
			
			this.x = 0;
			ww = 0.5f * (1.0f - m1.m22);
			if (ww >= EPS2)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				this.y = (float) System.Math.Sqrt((double) ww);
				this.z = m1.m21 / (2.0f * this.y);
				return ;
			}
			
			this.y = 0;
			this.z = 1;
		}
		
		
		/// <summary> Sets the value of this quaternion to the rotational component of
		/// the passed matrix.
		/// </summary>
		/// <param name="m1">the Matrix3d
		/// </param>
		public void  set_Renamed(Matrix3d m1)
		{
			double ww = 0.25 * (m1.m00 + m1.m11 + m1.m22 + 1.0f);
			
			if (ww >= 0)
			{
				if (ww >= EPS2)
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					this.w = (float) System.Math.Sqrt(ww);
					ww = 0.25 / this.w;
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					this.x = (float) ((m1.m21 - m1.m12) * ww);
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					this.y = (float) ((m1.m02 - m1.m20) * ww);
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					this.z = (float) ((m1.m10 - m1.m01) * ww);
					return ;
				}
			}
			else
			{
				this.w = 0;
				this.x = 0;
				this.y = 0;
				this.z = 1;
				return ;
			}
			
			this.w = 0;
			ww = (- 0.5) * (m1.m11 + m1.m22);
			if (ww >= 0)
			{
				if (ww >= EPS2)
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					this.x = (float) System.Math.Sqrt(ww);
					ww = 0.5 / this.x;
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					this.y = (float) (m1.m10 * ww);
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					this.z = (float) (m1.m20 * ww);
					return ;
				}
			}
			else
			{
				this.x = 0;
				this.y = 0;
				this.z = 1;
				return ;
			}
			
			this.x = 0;
			ww = 0.5 * (1.0 - m1.m22);
			if (ww >= EPS2)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				this.y = (float) System.Math.Sqrt(ww);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				this.z = (float) (m1.m21 / (2.0 * (double) (this.y)));
				return ;
			}
			
			this.y = 0;
			this.z = 1;
		}
		
		
		/// <summary> Sets the value of this quaternion to the equivalent rotation
		/// of the AxisAngle argument.
		/// </summary>
		/// <param name="a"> the AxisAngle to be emulated
		/// </param>
		public void  set_Renamed(AxisAngle4f a)
		{
			float mag, amag;
			// Quat = cos(theta/2) + sin(theta/2)(roation_axis) 
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			amag = (float) System.Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
			if (amag < EPS)
			{
				w = 0.0f;
				x = 0.0f;
				y = 0.0f;
				z = 0.0f;
			}
			else
			{
				amag = 1.0f / amag;
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				mag = (float) System.Math.Sin(a.angle / 2.0);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				w = (float) System.Math.Cos(a.angle / 2.0);
				x = a.x * amag * mag;
				y = a.y * amag * mag;
				z = a.z * amag * mag;
			}
		}
		
		
		/// <summary> Sets the value of this quaternion to the equivalent rotation
		/// of the AxisAngle argument.
		/// </summary>
		/// <param name="a"> the AxisAngle to be emulated
		/// </param>
		public void  set_Renamed(AxisAngle4d a)
		{
			float mag, amag;
			// Quat = cos(theta/2) + sin(theta/2)(roation_axis) 
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			amag = (float) (1.0 / System.Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z));
			
			if (amag < EPS)
			{
				w = 0.0f;
				x = 0.0f;
				y = 0.0f;
				z = 0.0f;
			}
			else
			{
				amag = 1.0f / amag;
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				mag = (float) System.Math.Sin(a.angle / 2.0);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				w = (float) System.Math.Cos(a.angle / 2.0);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				x = (float) a.x * amag * mag;
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				y = (float) a.y * amag * mag;
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				z = (float) a.z * amag * mag;
			}
		}
		
		
		/// <summary>  Performs a great circle interpolation between this quaternion
		/// and the quaternion parameter and places the result into this
		/// quaternion.
		/// </summary>
		/// <param name="q1"> the other quaternion
		/// </param>
		/// <param name="alpha"> the alpha interpolation parameter
		/// </param>
		public void  interpolate(Quat4f q1, float alpha)
		{
			// From "Advanced Animation and Rendering Techniques"
			// by Watt and Watt pg. 364, function as implemented appeared to be 
			// incorrect.  Fails to choose the same quaternion for the double
			// covering. Resulting in change of direction for rotations.
			// Fixed function to negate the first quaternion in the case that the
			// dot product of q1 and this is negative. Second case was not needed. 
			
			double dot, s1, s2, om, sinom;
			
			dot = x * q1.x + y * q1.y + z * q1.z + w * q1.w;
			
			if (dot < 0)
			{
				// negate quaternion
				q1.x = - q1.x; q1.y = - q1.y; q1.z = - q1.z; q1.w = - q1.w;
				dot = - dot;
			}
			
			if ((1.0 - dot) > EPS)
			{
				om = System.Math.Acos(dot);
				sinom = System.Math.Sin(om);
				s1 = System.Math.Sin((1.0 - alpha) * om) / sinom;
				s2 = System.Math.Sin(alpha * om) / sinom;
			}
			else
			{
				s1 = 1.0 - alpha;
				s2 = alpha;
			}
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			w = (float) (s1 * w + s2 * q1.w);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			x = (float) (s1 * x + s2 * q1.x);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			y = (float) (s1 * y + s2 * q1.y);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			z = (float) (s1 * z + s2 * q1.z);
		}
		
		
		
		/// <summary>  Performs a great circle interpolation between quaternion q1
		/// and quaternion q2 and places the result into this quaternion. 
		/// </summary>
		/// <param name="q1"> the first quaternion
		/// </param>
		/// <param name="q2"> the second quaternion
		/// </param>
		/// <param name="alpha"> the alpha interpolation parameter 
		/// </param>
		public void  interpolate(Quat4f q1, Quat4f q2, float alpha)
		{
			// From "Advanced Animation and Rendering Techniques"
			// by Watt and Watt pg. 364, function as implemented appeared to be 
			// incorrect.  Fails to choose the same quaternion for the double
			// covering. Resulting in change of direction for rotations.
			// Fixed function to negate the first quaternion in the case that the
			// dot product of q1 and this is negative. Second case was not needed. 
			
			double dot, s1, s2, om, sinom;
			
			dot = q2.x * q1.x + q2.y * q1.y + q2.z * q1.z + q2.w * q1.w;
			
			if (dot < 0)
			{
				// negate quaternion
				q1.x = - q1.x; q1.y = - q1.y; q1.z = - q1.z; q1.w = - q1.w;
				dot = - dot;
			}
			
			if ((1.0 - dot) > EPS)
			{
				om = System.Math.Acos(dot);
				sinom = System.Math.Sin(om);
				s1 = System.Math.Sin((1.0 - alpha) * om) / sinom;
				s2 = System.Math.Sin(alpha * om) / sinom;
			}
			else
			{
				s1 = 1.0 - alpha;
				s2 = alpha;
			}
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			w = (float) (s1 * q1.w + s2 * q2.w);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			x = (float) (s1 * q1.x + s2 * q2.x);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			y = (float) (s1 * q1.y + s2 * q2.y);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			z = (float) (s1 * q1.z + s2 * q2.z);
		}
	}
}