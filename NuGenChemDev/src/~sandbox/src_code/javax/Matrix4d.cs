/*
Copyright (C) 1997,1998,1999
Kenji Hiranabe, Eiwa System Management, Inc.

This program is free software.
Implemented by Kenji Hiranabe(hiranabe@esm.co.jp),
conforming to the Java(TM) 3D API specification by Sun Microsystems.

Permission to use, copy, modify, distribute and sell this software
and its documentation for any purpose is hereby granted without fee,
provided that the above copyright notice appear in all copies and
that both that copyright notice and this permission notice appear
in supporting documentation. Kenji Hiranabe and Eiwa System Management,Inc.
makes no representations about the suitability of this software for any
purpose.  It is provided "AS IS" with NO WARRANTY.*/
using System;
namespace javax.vecmath
{
	
	/// <summary> A double precision floating point 4 by 4 matrix.</summary>
	/// <version>  specification 1.1, implementation $Revision: 1.3 $, $Date: 2001/10/13 04:09:46 $
	/// </version>
	/// <author>  Kenji hiranabe
	/// </author>
	[Serializable]
	public class Matrix4d
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Performs an SVD normalization of this matrix to calculate and return the
		/// uniform scale factor. This matrix is not modified.
		/// </summary>
		/// <returns> the scale factor of this matrix
		/// </returns>
		/// <summary> Sets the scale component of the current matrix by factoring out the
		/// current scale (by doing an SVD) from the rotational component and
		/// multiplying by the new scale.
		/// </summary>
		/// <param name="scale">the new scale amount
		/// </param>
		virtual public double Scale
		{
			get
			{
				return SVD(null);
			}
			
			set
			{
				SVD(null, this);
				mulRotationScale(value);
			}
			
		}
		/*
		* $Log: Matrix4d.java,v $
		* Revision 1.3  2001/10/13 04:09:46  arty
		* checkins for the walk demo.
		* Includes classes written in SML.
		*
		* Revision 1.15  1999/10/05  07:03:50  hiranabe
		* copyright change
		*
		* Revision 1.15  1999/10/05  07:03:50  hiranabe
		* copyright change
		*
		* Revision 1.14  1999/06/12  03:27:36  hiranabe
		* minor internal notation change
		*
		* Revision 1.13  1999/06/12  03:15:11  hiranabe
		* SVD normlizatin is done for each axis
		*
		* Revision 1.12  1999/03/04  09:16:33  hiranabe
		* small bug fix and copyright change
		*
		* Revision 1.11  1998/10/14  00:49:10  hiranabe
		* API1.1 Beta02
		*
		* Revision 1.10  1998/07/27  04:33:08  hiranabe
		* transpose(M m1) bug. It acted as the same as 'set'.
		*
		* Revision 1.9  1998/07/27  04:28:13  hiranabe
		* API1.1Alpha01 ->API1.1Alpha03
		*
		* Revision 1.8  1998/04/17  10:30:46  hiranabe
		* null check for equals
		*
		* Revision 1.7  1998/04/10  04:52:14  hiranabe
		* API1.0 -> API1.1 (added constructors, methods)
		*
		* Revision 1.6  1998/04/09  08:18:15  hiranabe
		* minor comment change
		*
		* Revision 1.5  1998/04/09  07:05:18  hiranabe
		* API 1.1
		*
		* Revision 1.4  1998/04/08  06:01:08  hiranabe
		* bug fix of set(m,t,s). thanks > t.m.child@surveying.salford.ac.uk
		*
		* Revision 1.3  1998/01/05  06:29:31  hiranabe
		* copyright 98
		*
		* Revision 1.2  1997/12/10  06:08:05  hiranabe
		* toString   '\n' -> "line.separator"
		*
		* Revision 1.1  1997/11/26  03:00:44  hiranabe
		* Initial revision
		*
		*/
		
		/// <summary> The first element of the first row.</summary>
		public double m00;
		
		/// <summary> The second element of the first row.</summary>
		public double m01;
		
		/// <summary> third element of the first row.</summary>
		public double m02;
		
		/// <summary> The fourth element of the first row.</summary>
		public double m03;
		
		/// <summary> The first element of the second row.</summary>
		public double m10;
		
		/// <summary> The second element of the second row.</summary>
		public double m11;
		
		/// <summary> The third element of the second row.</summary>
		public double m12;
		
		/// <summary> The fourth element of the second row.</summary>
		public double m13;
		
		/// <summary> The first element of the third row.</summary>
		public double m20;
		
		/// <summary> The second element of the third row.</summary>
		public double m21;
		
		/// <summary> The third element of the third row.</summary>
		public double m22;
		
		/// <summary> The fourth element of the third row.</summary>
		public double m23;
		
		/// <summary> The first element of the fourth row.</summary>
		public double m30;
		
		/// <summary> The second element of the fourth row.</summary>
		public double m31;
		
		/// <summary> The third element of the fourth row.</summary>
		public double m32;
		
		/// <summary> The fourth element of the fourth row.</summary>
		public double m33;
		
		/// <summary> 
		/// Constructs and initializes a Matrix4d from the specified 16 values.
		/// </summary>
		/// <param name="m00">the [0][0] element
		/// </param>
		/// <param name="m01">the [0][1] element
		/// </param>
		/// <param name="m02">the [0][2] element
		/// </param>
		/// <param name="m03">the [0][3] element
		/// </param>
		/// <param name="m10">the [1][0] element
		/// </param>
		/// <param name="m11">the [1][1] element
		/// </param>
		/// <param name="m12">the [1][2] element
		/// </param>
		/// <param name="m13">the [1][3] element
		/// </param>
		/// <param name="m20">the [2][0] element
		/// </param>
		/// <param name="m21">the [2][1] element
		/// </param>
		/// <param name="m22">the [2][2] element
		/// </param>
		/// <param name="m23">the [2][3] element
		/// </param>
		/// <param name="m30">the [3][0] element
		/// </param>
		/// <param name="m31">the [3][1] element
		/// </param>
		/// <param name="m32">the [3][2] element
		/// </param>
		/// <param name="m33">the [3][3] element
		/// </param>
		public Matrix4d(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13, double m20, double m21, double m22, double m23, double m30, double m31, double m32, double m33)
		{
			set_Renamed(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
		}
		
		/// <summary> Constructs and initializes a Matrix4d from the specified 16
		/// element array.  this.m00 =v[0], this.m01=v[1], etc.
		/// </summary>
		/// <param name="v">the array of length 16 containing in order
		/// </param>
		public Matrix4d(double[] v)
		{
			set_Renamed(v);
		}
		
		/// <summary> Constructs and initializes a Matrix4d from the quaternion,
		/// translation, and scale values; the scale is applied only to the
		/// rotational components of the matrix (upper 3x3) and not to the
		/// translational components.
		/// </summary>
		/// <param name="q1"> The quaternion value representing the rotational component
		/// </param>
		/// <param name="t1"> The translational component of the matrix
		/// </param>
		/// <param name="s"> The scale value applied to the rotational components
		/// </param>
		public Matrix4d(Quat4d q1, Vector3d t1, double s)
		{
			set_Renamed(q1, t1, s);
		}
		
		/// <summary> Constructs and initializes a Matrix4d from the quaternion,
		/// translation, and scale values; the scale is applied only to the
		/// rotational components of the matrix (upper 3x3) and not to the
		/// translational components.
		/// </summary>
		/// <param name="q1"> The quaternion value representing the rotational component
		/// </param>
		/// <param name="t1"> The translational component of the matrix
		/// </param>
		/// <param name="s"> The scale value applied to the rotational components
		/// </param>
		public Matrix4d(Quat4f q1, Vector3d t1, double s)
		{
			set_Renamed(q1, t1, s);
		}
		
		
		/// <summary> Constructs a new matrix with the same values as the Matrix4d parameter.</summary>
		/// <param name="m1">The source matrix.
		/// </param>
		public Matrix4d(Matrix4d m1)
		{
			set_Renamed(m1);
		}
		
		/// <summary> Constructs a new matrix with the same values as the Matrix4f parameter.</summary>
		/// <param name="m1">The source matrix.
		/// </param>
		public Matrix4d(Matrix4f m1)
		{
			set_Renamed(m1);
		}
		
		/// <summary> Constructs and initializes a Matrix4d from the rotation matrix,
		/// translation, and scale values; the scale is applied only to the
		/// rotational components of the matrix (upper 3x3) and not to the
		/// translational components.
		/// </summary>
		/// <param name="m1"> The rotation matrix representing the rotational components
		/// </param>
		/// <param name="t1"> The translational components of the matrix
		/// </param>
		/// <param name="s"> The scale value applied to the rotational components
		/// </param>
		public Matrix4d(Matrix3f m1, Vector3d t1, double s)
		{
			// why no set(Matrix3f, Vector3d, double) ?
			// set(Matrix3f, Vector3f, float) is there.
			// feel inconsistent.
			set_Renamed(m1);
			mulRotationScale(s);
			setTranslation(t1);
			m33 = 1.0;
		}
		
		/// <summary> Constructs and initializes a Matrix4d from the rotation matrix,
		/// translation, and scale values; the scale is applied only to the
		/// rotational components of the matrix (upper 3x3) and not to the
		/// translational components.
		/// </summary>
		/// <param name="m1"> The rotation matrix representing the rotational components
		/// </param>
		/// <param name="t1"> The translational components of the matrix
		/// </param>
		/// <param name="s"> The scale value applied to the rotational components
		/// </param>
		public Matrix4d(Matrix3d m1, Vector3d t1, double s)
		{
			set_Renamed(m1, t1, s);
		}
		
		/// <summary> Constructs and initializes a Matrix4d to all zeros.</summary>
		public Matrix4d()
		{
			setZero();
		}
		
		/// <summary> Returns a string that contains the values of this Matrix4d.</summary>
		/// <returns> the String representation
		/// </returns>
		public override System.String ToString()
		{
			System.String nl = System.Environment.NewLine;
			return "[" + nl + "  [" + m00 + "\t" + m01 + "\t" + m02 + "\t" + m03 + "]" + nl + "  [" + m10 + "\t" + m11 + "\t" + m12 + "\t" + m13 + "]" + nl + "  [" + m20 + "\t" + m21 + "\t" + m22 + "\t" + m23 + "]" + nl + "  [" + m30 + "\t" + m31 + "\t" + m32 + "\t" + m33 + "] ]";
		}
		
		/// <summary> Sets this Matrix4d to identity.</summary>
		public void  setIdentity()
		{
			m00 = 1.0; m01 = 0.0; m02 = 0.0; m03 = 0.0;
			m10 = 0.0; m11 = 1.0; m12 = 0.0; m13 = 0.0;
			m20 = 0.0; m21 = 0.0; m22 = 1.0; m23 = 0.0;
			m30 = 0.0; m31 = 0.0; m32 = 0.0; m33 = 1.0;
		}
		
		/// <summary> Sets the specified element of this matrix4d to the value provided.</summary>
		/// <param name="row"> the row number to be modified (zero indexed)
		/// </param>
		/// <param name="column"> the column number to be modified (zero indexed)
		/// </param>
		/// <param name="value">the new value
		/// </param>
		public void  setElement(int row, int column, double value_Renamed)
		{
			if (row == 0)
				if (column == 0)
					m00 = value_Renamed;
				else if (column == 1)
					m01 = value_Renamed;
				else if (column == 2)
					m02 = value_Renamed;
				else if (column == 3)
					m03 = value_Renamed;
				else
					throw new System.IndexOutOfRangeException("column must be 0 to 3 and is " + column);
			else if (row == 1)
				if (column == 0)
					m10 = value_Renamed;
				else if (column == 1)
					m11 = value_Renamed;
				else if (column == 2)
					m12 = value_Renamed;
				else if (column == 3)
					m13 = value_Renamed;
				else
					throw new System.IndexOutOfRangeException("column must be 0 to 3 and is " + column);
			else if (row == 2)
				if (column == 0)
					m20 = value_Renamed;
				else if (column == 1)
					m21 = value_Renamed;
				else if (column == 2)
					m22 = value_Renamed;
				else if (column == 3)
					m23 = value_Renamed;
				else
					throw new System.IndexOutOfRangeException("column must be 0 to 3 and is " + column);
			else if (row == 3)
				if (column == 0)
					m30 = value_Renamed;
				else if (column == 1)
					m31 = value_Renamed;
				else if (column == 2)
					m32 = value_Renamed;
				else if (column == 3)
					m33 = value_Renamed;
				else
					throw new System.IndexOutOfRangeException("column must be 0 to 3 and is " + column);
			else
				throw new System.IndexOutOfRangeException("row must be 0 to 2 and is " + row);
		}
		
		/// <summary> Retrieves the value at the specified row and column of this matrix.</summary>
		/// <param name="row"> the row number to be retrieved (zero indexed)
		/// </param>
		/// <param name="column"> the column number to be retrieved (zero indexed)
		/// </param>
		/// <returns> the value at the indexed element
		/// </returns>
		public double getElement(int row, int column)
		{
			if (row == 0)
				if (column == 0)
					return m00;
				else if (column == 1)
					return m01;
				else if (column == 2)
					return m02;
				else if (column == 3)
					return m03;
				else
					throw new System.IndexOutOfRangeException("column must be 0 to 3 and is " + column);
			else if (row == 1)
				if (column == 0)
					return m10;
				else if (column == 1)
					return m11;
				else if (column == 2)
					return m12;
				else if (column == 3)
					return m13;
				else
					throw new System.IndexOutOfRangeException("column must be 0 to 3 and is " + column);
			else if (row == 2)
				if (column == 0)
					return m20;
				else if (column == 1)
					return m21;
				else if (column == 2)
					return m22;
				else if (column == 3)
					return m23;
				else
					throw new System.IndexOutOfRangeException("column must be 0 to 3 and is " + column);
			else if (row == 3)
				if (column == 0)
					return m30;
				else if (column == 1)
					return m31;
				else if (column == 2)
					return m32;
				else if (column == 3)
					return m33;
				else
					throw new System.IndexOutOfRangeException("column must be 0 to 3 and is " + column);
			else
				throw new System.IndexOutOfRangeException("row must be 0 to 3 and is " + row);
		}
		
		/// <summary> Performs an SVD normalization of this matrix in order to acquire the
		/// normalized rotational component; the values are placed into the Matrix3d parameter.
		/// </summary>
		/// <param name="m1">matrix into which the rotational component is placed
		/// </param>
		public void  get_Renamed(Matrix3d m1)
		{
			SVD(m1, null);
		}
		
		/// <summary> Performs an SVD normalization of this matrix in order to acquire the
		/// normalized rotational component; the values are placed into the Matrix3f parameter.
		/// </summary>
		/// <param name="m1">matrix into which the rotational component is placed
		/// </param>
		public void  get_Renamed(Matrix3f m1)
		{
			SVD(m1);
		}
		
		/// <summary> Performs an SVD normalization of this matrix to calculate the rotation
		/// as a 3x3 matrix, the translation, and the scale. None of the matrix values are modified.
		/// </summary>
		/// <param name="m1">The normalized matrix representing the rotation
		/// </param>
		/// <param name="t1">The translation component
		/// </param>
		/// <returns> The scale component of this transform
		/// </returns>
		public double get_Renamed(Matrix3d m1, Vector3d t1)
		{
			get_Renamed(t1);
			return SVD(m1, null);
		}
		
		/// <summary> Performs an SVD normalization of this matrix to calculate the rotation
		/// as a 3x3 matrix, the translation, and the scale. None of the matrix values are modified.
		/// </summary>
		/// <param name="m1">The normalized matrix representing the rotation
		/// </param>
		/// <param name="t1">The translation component
		/// </param>
		/// <returns> The scale component of this transform
		/// </returns>
		public double get_Renamed(Matrix3f m1, Vector3d t1)
		{
			get_Renamed(t1);
			return SVD(m1);
		}
		
		/// <summary> Performs an SVD normalization of this matrix in order to acquire the
		/// normalized rotational component; the values are placed into
		/// the Quat4f parameter.
		/// </summary>
		/// <param name="q1">quaternion into which the rotation component is placed
		/// </param>
		public void  get_Renamed(Quat4f q1)
		{
			q1.set_Renamed(this);
			q1.normalize();
		}
		
		/// <summary> Performs an SVD normalization of this matrix in order to acquire the
		/// normalized rotational component; the values are placed into
		/// the Quat4f parameter.
		/// </summary>
		/// <param name="q1">quaternion into which the rotation component is placed
		/// </param>
		public void  get_Renamed(Quat4d q1)
		{
			q1.set_Renamed(this);
			q1.normalize();
		}
		
		/// <summary> Retrieves the translational components of this matrix.</summary>
		/// <param name="trans">the vector that will receive the translational component
		/// </param>
		public void  get_Renamed(Vector3d trans)
		{
			trans.x = m03;
			trans.y = m13;
			trans.z = m23;
		}
		
		/// <summary> Gets the upper 3x3 values of this matrix and places them into the matrix m1.</summary>
		/// <param name="m1">The matrix that will hold the values
		/// </param>
		public void  getRotationScale(Matrix3f m1)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			m1.m00 = (float) m00;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			m1.m01 = (float) m01;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			m1.m02 = (float) m02;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			m1.m10 = (float) m10;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			m1.m11 = (float) m11;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			m1.m12 = (float) m12;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			m1.m20 = (float) m20;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			m1.m21 = (float) m21;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			m1.m22 = (float) m22;
		}
		
		/// <summary> Gets the upper 3x3 values of this matrix and places them into the matrix m1.</summary>
		/// <param name="m1">The matrix that will hold the values
		/// </param>
		public void  getRotationScale(Matrix3d m1)
		{
			m1.m00 = m00; m1.m01 = m01; m1.m02 = m02;
			m1.m10 = m10; m1.m11 = m11; m1.m12 = m12;
			m1.m20 = m20; m1.m21 = m21; m1.m22 = m22;
		}
		
		/// <summary> Replaces the upper 3x3 matrix values of this matrix with the values in the matrix m1.</summary>
		/// <param name="m1">The matrix that will be the new upper 3x3
		/// </param>
		public void  setRotationScale(Matrix3d m1)
		{
			m00 = m1.m00; m01 = m1.m01; m02 = m1.m02;
			m10 = m1.m10; m11 = m1.m11; m12 = m1.m12;
			m20 = m1.m20; m21 = m1.m21; m22 = m1.m22;
		}
		
		/// <summary> Replaces the upper 3x3 matrix values of this matrix with the values in the matrix m1.</summary>
		/// <param name="m1">The matrix that will be the new upper 3x3
		/// </param>
		public void  setRotationScale(Matrix3f m1)
		{
			m00 = m1.m00; m01 = m1.m01; m02 = m1.m02;
			m10 = m1.m10; m11 = m1.m11; m12 = m1.m12;
			m20 = m1.m20; m21 = m1.m21; m22 = m1.m22;
		}
		
		/// <summary> Sets the specified row of this matrix4d to the four values provided.</summary>
		/// <param name="row"> the row number to be modified (zero indexed)
		/// </param>
		/// <param name="x">the first column element
		/// </param>
		/// <param name="y">the second column element
		/// </param>
		/// <param name="z">the third column element
		/// </param>
		/// <param name="w">the fourth column element
		/// </param>
		public void  setRow(int row, double x, double y, double z, double w)
		{
			if (row == 0)
			{
				m00 = x;
				m01 = y;
				m02 = z;
				m03 = w;
			}
			else if (row == 1)
			{
				m10 = x;
				m11 = y;
				m12 = z;
				m13 = w;
			}
			else if (row == 2)
			{
				m20 = x;
				m21 = y;
				m22 = z;
				m23 = w;
			}
			else if (row == 3)
			{
				m30 = x;
				m31 = y;
				m32 = z;
				m33 = w;
			}
			else
			{
				throw new System.IndexOutOfRangeException("row must be 0 to 3 and is " + row);
			}
		}
		
		/// <summary> Sets the specified row of this matrix4d to the Vector provided.</summary>
		/// <param name="row">the row number to be modified (zero indexed)
		/// </param>
		/// <param name="v">the replacement row
		/// </param>
		public void  setRow(int row, Vector4d v)
		{
			if (row == 0)
			{
				m00 = v.x;
				m01 = v.y;
				m02 = v.z;
				m03 = v.w;
			}
			else if (row == 1)
			{
				m10 = v.x;
				m11 = v.y;
				m12 = v.z;
				m13 = v.w;
			}
			else if (row == 2)
			{
				m20 = v.x;
				m21 = v.y;
				m22 = v.z;
				m23 = v.w;
			}
			else if (row == 3)
			{
				m30 = v.x;
				m31 = v.y;
				m32 = v.z;
				m33 = v.w;
			}
			else
			{
				throw new System.IndexOutOfRangeException("row must be 0 to 3 and is " + row);
			}
		}
		
		/// <summary> Sets the specified row of this matrix4d to the four values provided.</summary>
		/// <param name="row">the row number to be modified (zero indexed)
		/// </param>
		/// <param name="v">the replacement row
		/// </param>
		public void  setRow(int row, double[] v)
		{
			if (row == 0)
			{
				m00 = v[0];
				m01 = v[1];
				m02 = v[2];
				m03 = v[3];
			}
			else if (row == 1)
			{
				m10 = v[0];
				m11 = v[1];
				m12 = v[2];
				m13 = v[3];
			}
			else if (row == 2)
			{
				m20 = v[0];
				m21 = v[1];
				m22 = v[2];
				m23 = v[3];
			}
			else if (row == 3)
			{
				m30 = v[0];
				m31 = v[1];
				m32 = v[2];
				m33 = v[3];
			}
			else
			{
				throw new System.IndexOutOfRangeException("row must be 0 to 3 and is " + row);
			}
		}
		
		/// <summary> Copies the matrix values in the specified row into the
		/// vector parameter.
		/// </summary>
		/// <param name="row">the matrix row
		/// </param>
		/// <param name="v">The vector into which the matrix row values will be copied
		/// </param>
		public void  getRow(int row, Vector4d v)
		{
			if (row == 0)
			{
				v.x = m00;
				v.y = m01;
				v.z = m02;
				v.w = m03;
			}
			else if (row == 1)
			{
				v.x = m10;
				v.y = m11;
				v.z = m12;
				v.w = m13;
			}
			else if (row == 2)
			{
				v.x = m20;
				v.y = m21;
				v.z = m22;
				v.w = m23;
			}
			else if (row == 3)
			{
				v.x = m30;
				v.y = m31;
				v.z = m32;
				v.w = m33;
			}
			else
			{
				throw new System.IndexOutOfRangeException("row must be 0 to 3 and is " + row);
			}
		}
		
		/// <summary> Copies the matrix values in the specified row into the
		/// array parameter.
		/// </summary>
		/// <param name="row">the matrix row
		/// </param>
		/// <param name="v">The array into which the matrix row values will be copied
		/// </param>
		public void  getRow(int row, double[] v)
		{
			if (row == 0)
			{
				v[0] = m00;
				v[1] = m01;
				v[2] = m02;
				v[3] = m03;
			}
			else if (row == 1)
			{
				v[0] = m10;
				v[1] = m11;
				v[2] = m12;
				v[3] = m13;
			}
			else if (row == 2)
			{
				v[0] = m20;
				v[1] = m21;
				v[2] = m22;
				v[3] = m23;
			}
			else if (row == 3)
			{
				v[0] = m30;
				v[1] = m31;
				v[2] = m32;
				v[3] = m33;
			}
			else
			{
				throw new System.IndexOutOfRangeException("row must be 0 to 3 and is " + row);
			}
		}
		
		/// <summary> Sets the specified column of this matrix4d to the four values provided.</summary>
		/// <param name="column">the column number to be modified (zero indexed)
		/// </param>
		/// <param name="x">the first row element
		/// </param>
		/// <param name="y">the second row element
		/// </param>
		/// <param name="z">the third row element
		/// </param>
		/// <param name="w">the fourth row element
		/// </param>
		public void  setColumn(int column, double x, double y, double z, double w)
		{
			if (column == 0)
			{
				m00 = x;
				m10 = y;
				m20 = z;
				m30 = w;
			}
			else if (column == 1)
			{
				m01 = x;
				m11 = y;
				m21 = z;
				m31 = w;
			}
			else if (column == 2)
			{
				m02 = x;
				m12 = y;
				m22 = z;
				m32 = w;
			}
			else if (column == 3)
			{
				m03 = x;
				m13 = y;
				m23 = z;
				m33 = w;
			}
			else
			{
				throw new System.IndexOutOfRangeException("column must be 0 to 3 and is " + column);
			}
		}
		
		/// <summary> Sets the specified column of this matrix4d to the vector provided.</summary>
		/// <param name="column">the column number to be modified (zero indexed)
		/// </param>
		/// <param name="v">the replacement column
		/// </param>
		public void  setColumn(int column, Vector4d v)
		{
			if (column == 0)
			{
				m00 = v.x;
				m10 = v.y;
				m20 = v.z;
				m30 = v.w;
			}
			else if (column == 1)
			{
				m01 = v.x;
				m11 = v.y;
				m21 = v.z;
				m31 = v.w;
			}
			else if (column == 2)
			{
				m02 = v.x;
				m12 = v.y;
				m22 = v.z;
				m32 = v.w;
			}
			else if (column == 3)
			{
				m03 = v.x;
				m13 = v.y;
				m23 = v.z;
				m33 = v.w;
			}
			else
			{
				throw new System.IndexOutOfRangeException("column must be 0 to 3 and is " + column);
			}
		}
		
		/// <summary> Sets the specified column of this matrix4d to the four values provided. </summary>
		/// <param name="column"> the column number to be modified (zero indexed) 
		/// </param>
		/// <param name="v">      the replacement column 
		/// </param>
		public void  setColumn(int column, double[] v)
		{
			if (column == 0)
			{
				m00 = v[0];
				m10 = v[1];
				m20 = v[2];
				m30 = v[3];
			}
			else if (column == 1)
			{
				m01 = v[0];
				m11 = v[1];
				m21 = v[2];
				m31 = v[3];
			}
			else if (column == 2)
			{
				m02 = v[0];
				m12 = v[1];
				m22 = v[2];
				m32 = v[3];
			}
			else if (column == 3)
			{
				m03 = v[0];
				m13 = v[1];
				m23 = v[2];
				m33 = v[3];
			}
			else
			{
				throw new System.IndexOutOfRangeException("column must be 0 to 3 and is " + column);
			}
		}
		/// <summary> Copies the matrix values in the specified column into the
		/// vector parameter.
		/// </summary>
		/// <param name="column">the matrix column
		/// </param>
		/// <param name="v">The vector into which the matrix column values will be copied
		/// </param>
		public void  getColumn(int column, Vector4d v)
		{
			if (column == 0)
			{
				v.x = m00;
				v.y = m10;
				v.z = m20;
				v.w = m30;
			}
			else if (column == 1)
			{
				v.x = m01;
				v.y = m11;
				v.z = m21;
				v.w = m31;
			}
			else if (column == 2)
			{
				v.x = m02;
				v.y = m12;
				v.z = m22;
				v.w = m32;
			}
			else if (column == 3)
			{
				v.x = m03;
				v.y = m13;
				v.z = m23;
				v.w = m33;
			}
			else
			{
				throw new System.IndexOutOfRangeException("column must be 0 to 3 and is " + column);
			}
		}
		
		/// <summary> Copies the matrix values in the specified column into the
		/// array parameter.
		/// </summary>
		/// <param name="column">the matrix column
		/// </param>
		/// <param name="v">The array into which the matrix column values will be copied
		/// </param>
		public void  getColumn(int column, double[] v)
		{
			if (column == 0)
			{
				v[0] = m00;
				v[1] = m10;
				v[2] = m20;
				v[3] = m30;
			}
			else if (column == 1)
			{
				v[0] = m01;
				v[1] = m11;
				v[2] = m21;
				v[3] = m31;
			}
			else if (column == 2)
			{
				v[0] = m02;
				v[1] = m12;
				v[2] = m22;
				v[3] = m32;
			}
			else if (column == 3)
			{
				v[0] = m03;
				v[1] = m13;
				v[2] = m23;
				v[3] = m33;
			}
			else
			{
				throw new System.IndexOutOfRangeException("column must be 0 to 3 and is " + column);
			}
		}
		
		/// <summary> Adds a scalar to each component of this matrix.</summary>
		/// <param name="scalar">The scalar adder.
		/// </param>
		public void  add(double scalar)
		{
			m00 += scalar; m01 += scalar; m02 += scalar; m03 += scalar;
			m10 += scalar; m11 += scalar; m12 += scalar; m13 += scalar;
			m20 += scalar; m21 += scalar; m22 += scalar; m23 += scalar;
			m30 += scalar; m31 += scalar; m32 += scalar; m33 += scalar;
		}
		
		/// <summary> Adds a scalar to each component of the matrix m1 and places
		/// the result into this. Matrix m1 is not modified.
		/// </summary>
		/// <param name="scalar">The scalar adder.
		/// </param>
		/// <parm>  m1 The original matrix values. </parm>
		public void  add(double scalar, Matrix4d m1)
		{
			set_Renamed(m1);
			add(scalar);
		}
		
		
		/// <summary> Sets the value of this matrix to the matrix sum of matrices m1 and m2. </summary>
		/// <param name="m1">the first matrix 
		/// </param>
		/// <param name="m2">the second matrix 
		/// </param>
		public void  add(Matrix4d m1, Matrix4d m2)
		{
			// note this is alias safe.
			set_Renamed(m1.m00 + m2.m00, m1.m01 + m2.m01, m1.m02 + m2.m02, m1.m03 + m2.m03, m1.m10 + m2.m10, m1.m11 + m2.m11, m1.m12 + m2.m12, m1.m13 + m2.m13, m1.m20 + m2.m20, m1.m21 + m2.m21, m1.m22 + m2.m22, m1.m23 + m2.m23, m1.m30 + m2.m30, m1.m31 + m2.m31, m1.m32 + m2.m32, m1.m33 + m2.m33);
		}
		
		/// <summary> Sets the value of this matrix to sum of itself and matrix m1. </summary>
		/// <param name="m1">the other matrix 
		/// </param>
		public void  add(Matrix4d m1)
		{
			m00 += m1.m00; m01 += m1.m01; m02 += m1.m02; m03 += m1.m03;
			m10 += m1.m10; m11 += m1.m11; m12 += m1.m12; m13 += m1.m13;
			m20 += m1.m20; m21 += m1.m21; m22 += m1.m22; m23 += m1.m23;
			m30 += m1.m30; m31 += m1.m31; m32 += m1.m32; m33 += m1.m33;
		}
		
		/// <summary> Sets the value of this matrix to the matrix difference
		/// of matrices m1 and m2. 
		/// </summary>
		/// <param name="m1">the first matrix 
		/// </param>
		/// <param name="m2">the second matrix 
		/// </param>
		public void  sub(Matrix4d m1, Matrix4d m2)
		{
			// note this is alias safe.
			set_Renamed(m1.m00 - m2.m00, m1.m01 - m2.m01, m1.m02 - m2.m02, m1.m03 - m2.m03, m1.m10 - m2.m10, m1.m11 - m2.m11, m1.m12 - m2.m12, m1.m13 - m2.m13, m1.m20 - m2.m20, m1.m21 - m2.m21, m1.m22 - m2.m22, m1.m23 - m2.m23, m1.m30 - m2.m30, m1.m31 - m2.m31, m1.m32 - m2.m32, m1.m33 - m2.m33);
		}
		
		/// <summary> Sets the value of this matrix to the matrix difference of itself
		/// and matrix m1 (this = this - m1). 
		/// </summary>
		/// <param name="m1">the other matrix 
		/// </param>
		public void  sub(Matrix4d m1)
		{
			m00 -= m1.m00; m01 -= m1.m01; m02 -= m1.m02; m03 -= m1.m03;
			m10 -= m1.m10; m11 -= m1.m11; m12 -= m1.m12; m13 -= m1.m13;
			m20 -= m1.m20; m21 -= m1.m21; m22 -= m1.m22; m23 -= m1.m23;
			m30 -= m1.m30; m31 -= m1.m31; m32 -= m1.m32; m33 -= m1.m33;
		}
		
		/// <summary> Sets the value of this matrix to its transpose. </summary>
		public void  transpose()
		{
			double tmp = m01;
			m01 = m10;
			m10 = tmp;
			
			tmp = m02;
			m02 = m20;
			m20 = tmp;
			
			tmp = m03;
			m03 = m30;
			m30 = tmp;
			
			tmp = m12;
			m12 = m21;
			m21 = tmp;
			
			tmp = m13;
			m13 = m31;
			m31 = tmp;
			
			tmp = m23;
			m23 = m32;
			m32 = tmp;
		}
		
		/// <summary> Sets the value of this matrix to the transpose of the argument matrix</summary>
		/// <param name="m1">the matrix to be transposed 
		/// </param>
		public void  transpose(Matrix4d m1)
		{
			// alias-safe
			set_Renamed(m1);
			transpose();
		}
		
		/// <summary> Sets the values in this Matrix4d equal to the row-major array parameter
		/// (ie, the first four elements of the array will be copied into the first
		/// row of this matrix, etc.).
		/// </summary>
		public void  set_Renamed(double[] m)
		{
			m00 = m[0]; m01 = m[1]; m02 = m[2]; m03 = m[3];
			m10 = m[4]; m11 = m[5]; m12 = m[6]; m13 = m[7];
			m20 = m[8]; m21 = m[9]; m22 = m[10]; m23 = m[11];
			m30 = m[12]; m31 = m[13]; m32 = m[14]; m33 = m[15];
		}
		
		/// <summary> Sets the rotational component (upper 3x3) of this matrix to the matrix
		/// values in the single precision Matrix3f argument; the other elements of
		/// this matrix are initialized as if this were an identity matrix
		/// (ie, affine matrix with no translational component).
		/// </summary>
		/// <param name="m1">the 3x3 matrix
		/// </param>
		public void  set_Renamed(Matrix3f m1)
		{
			m00 = m1.m00; m01 = m1.m01; m02 = m1.m02; m03 = 0.0;
			m10 = m1.m10; m11 = m1.m11; m12 = m1.m12; m13 = 0.0;
			m20 = m1.m20; m21 = m1.m21; m22 = m1.m22; m23 = 0.0;
			m30 = 0.0; m31 = 0.0; m32 = 0.0; m33 = 1.0;
		}
		
		/// <summary> Sets the rotational component (upper 3x3) of this matrix to the matrix
		/// values in the double precision Matrix3d argument; the other elements of
		/// this matrix are initialized as if this were an identity matrix
		/// (ie, affine matrix with no translational component).
		/// </summary>
		/// <param name="m1">the 3x3 matrix
		/// </param>
		public void  set_Renamed(Matrix3d m1)
		{
			m00 = m1.m00; m01 = m1.m01; m02 = m1.m02; m03 = 0.0;
			m10 = m1.m10; m11 = m1.m11; m12 = m1.m12; m13 = 0.0;
			m20 = m1.m20; m21 = m1.m21; m22 = m1.m22; m23 = 0.0;
			m30 = 0.0; m31 = 0.0; m32 = 0.0; m33 = 1.0;
		}
		
		/// <summary> Sets the value of this matrix to the matrix conversion of the
		/// (double precision) quaternion argument. 
		/// </summary>
		/// <param name="q1">the quaternion to be converted 
		/// </param>
		public void  set_Renamed(Quat4d q1)
		{
			setFromQuat(q1.x, q1.y, q1.z, q1.w);
		}
		
		/// <summary> Sets the value of this matrix to the matrix conversion of the
		/// double precision axis and angle argument. 
		/// </summary>
		/// <param name="a1">the axis and angle to be converted 
		/// </param>
		public void  set_Renamed(AxisAngle4d a1)
		{
			setFromAxisAngle(a1.x, a1.y, a1.z, a1.angle);
		}
		/// <summary> Sets the value of this matrix to the matrix conversion of the
		/// single precision quaternion argument. 
		/// </summary>
		/// <param name="q1">the quaternion to be converted 
		/// </param>
		public void  set_Renamed(Quat4f q1)
		{
			setFromQuat(q1.x, q1.y, q1.z, q1.w);
		}
		/// <summary> Sets the value of this matrix to the matrix conversion of the
		/// single precision axis and angle argument. 
		/// </summary>
		/// <param name="a1">the axis and angle to be converted 
		/// </param>
		public void  set_Renamed(AxisAngle4f a1)
		{
			setFromAxisAngle(a1.x, a1.y, a1.z, a1.angle);
		}
		
		
		/// <summary> Sets the value of this matrix from the rotation expressed by the
		/// quaternion q1, the translation t1, and the scale s.
		/// </summary>
		/// <param name="q1"> the rotation expressed as a quaternion
		/// </param>
		/// <param name="t1"> the translation
		/// </param>
		/// <param name="s"> the scale value
		/// </param>
		public void  set_Renamed(Quat4d q1, Vector3d t1, double s)
		{
			set_Renamed(q1);
			mulRotationScale(s);
			m03 = t1.x;
			m13 = t1.y;
			m23 = t1.z;
		}
		
		/// <summary> Sets the value of this matrix from the rotation expressed by the
		/// quaternion q1, the translation t1, and the scale s.
		/// </summary>
		/// <param name="q1"> the rotation expressed as a quaternion
		/// </param>
		/// <param name="t1"> the translation
		/// </param>
		/// <param name="s"> the scale value
		/// </param>
		public void  set_Renamed(Quat4f q1, Vector3d t1, double s)
		{
			set_Renamed(q1);
			mulRotationScale(s);
			m03 = t1.x;
			m13 = t1.y;
			m23 = t1.z;
		}
		
		/// <summary> Sets the value of this matrix from the rotation expressed by the
		/// quaternion q1, the translation t1, and the scale s.
		/// </summary>
		/// <param name="q1"> the rotation expressed as a quaternion
		/// </param>
		/// <param name="t1"> the translation
		/// </param>
		/// <param name="s"> the scale value
		/// </param>
		public void  set_Renamed(Quat4f q1, Vector3f t1, float s)
		{
			set_Renamed(q1);
			mulRotationScale(s);
			m03 = t1.x;
			m13 = t1.y;
			m23 = t1.z;
		}
		
		/// <summary> Sets the value of this matrix to a copy of the
		/// passed matrix m1.
		/// </summary>
		/// <param name="m1">the matrix to be copied
		/// </param>
		public void  set_Renamed(Matrix4d m1)
		{
			m00 = m1.m00; m01 = m1.m01; m02 = m1.m02; m03 = m1.m03;
			m10 = m1.m10; m11 = m1.m11; m12 = m1.m12; m13 = m1.m13;
			m20 = m1.m20; m21 = m1.m21; m22 = m1.m22; m23 = m1.m23;
			m30 = m1.m30; m31 = m1.m31; m32 = m1.m32; m33 = m1.m33;
		}
		/// <summary> Sets the value of this matrix to the double value of the
		/// passed matrix4f.
		/// </summary>
		/// <param name="m1">the matrix4f
		/// </param>
		public void  set_Renamed(Matrix4f m1)
		{
			m00 = m1.m00; m01 = m1.m01; m02 = m1.m02; m03 = m1.m03;
			m10 = m1.m10; m11 = m1.m11; m12 = m1.m12; m13 = m1.m13;
			m20 = m1.m20; m21 = m1.m21; m22 = m1.m22; m23 = m1.m23;
			m30 = m1.m30; m31 = m1.m31; m32 = m1.m32; m33 = m1.m33;
		}
		
		
		/// <summary> Sets the value of this matrix to the matrix inverse
		/// of the passed matrix m1. 
		/// </summary>
		/// <param name="m1">the matrix to be inverted 
		/// </param>
		public void  invert(Matrix4d m1)
		{
			set_Renamed(m1);
			invert();
		}
		
		/// <summary> Sets the value of this matrix to its inverse.</summary>
		public void  invert()
		{
			double s = determinant();
			if (s == 0.0)
				return ;
			s = 1 / s;
			// alias-safe way.
			// less *,+,- calculation than expanded expression.
			set_Renamed(m11 * (m22 * m33 - m23 * m32) + m12 * (m23 * m31 - m21 * m33) + m13 * (m21 * m32 - m22 * m31), m21 * (m02 * m33 - m03 * m32) + m22 * (m03 * m31 - m01 * m33) + m23 * (m01 * m32 - m02 * m31), m31 * (m02 * m13 - m03 * m12) + m32 * (m03 * m11 - m01 * m13) + m33 * (m01 * m12 - m02 * m11), m01 * (m13 * m22 - m12 * m23) + m02 * (m11 * m23 - m13 * m21) + m03 * (m12 * m21 - m11 * m22), m12 * (m20 * m33 - m23 * m30) + m13 * (m22 * m30 - m20 * m32) + m10 * (m23 * m32 - m22 * m33), m22 * (m00 * m33 - m03 * m30) + m23 * (m02 * m30 - m00 * m32) + m20 * (m03 * m32 - m02 * m33), m32 * (m00 * m13 - m03 * m10) + m33 * (m02 * m10 - m00 * m12) + m30 * (m03 * m12 - m02 * m13), m02 * (m13 * m20 - m10 * m23) + m03 * (m10 * m22 - m12 * m20) + m00 * (m12 * m23 - m13 * m22), m13 * (m20 * m31 - m21 * m30) + m10 * (m21 * m33 - m23 * m31) + m11 * (m23 * m30 - m20 * m33), m23 * (m00 * m31 - m01 * m30) + m20 * (m01 * m33 - m03 * m31) + m21 * (m03 * m30 - m00 * m33), m33 * (m00 * m11 - m01 * m10) + m30 * (m01 * m13 - m03 * m11) + m31 * (m03 * m10 - m00 * m13), m03 * (m11 * m20 - m10 * m21) + m00 * (m13 * m21 - m11 * m23) + m01 * (m10 * m23 - m13 * m20), m10 * (m22 * m31 - m21 * m32) + m11 * (m20 * m32 - m22 * m30) + m12 * (m21 * m30 - m20 * m31), m20 * (m02 * m31 - m01 * m32) + m21 * (m00 * m32 - m02 * m30) + m22 * (m01 * m30 - m00 * m31), m30 * (m02 * m11 - m01 * m12) + m31 * (m00 * m12 - m02 * m10) + m32 * (m01 * m10 - m00 * m11), m00 * (m11 * m22 - m12 * m21) + m01 * (m12 * m20 - m10 * m22) + m02 * (m10 * m21 - m11 * m20));
			
			mul(s);
		}
		
		/// <summary> Computes the determinant of this matrix. </summary>
		/// <returns> the determinant of the matrix 
		/// </returns>
		public double determinant()
		{
			// less *,+,- calculation than expanded expression.
			return (m00 * m11 - m01 * m10) * (m22 * m33 - m23 * m32) - (m00 * m12 - m02 * m10) * (m21 * m33 - m23 * m31) + (m00 * m13 - m03 * m10) * (m21 * m32 - m22 * m31) + (m01 * m12 - m02 * m11) * (m20 * m33 - m23 * m30) - (m01 * m13 - m03 * m11) * (m20 * m32 - m22 * m30) + (m02 * m13 - m03 * m12) * (m20 * m31 - m21 * m30);
		}
		
		/// <summary> Sets the value of this matrix to a scale matrix with the
		/// passed scale amount. 
		/// </summary>
		/// <param name="scale">the scale factor for the matrix 
		/// </param>
		public void  set_Renamed(double scale)
		{
			m00 = scale; m01 = 0.0; m02 = 0.0; m03 = 0.0;
			m10 = 0.0; m11 = scale; m12 = 0.0; m13 = 0.0;
			m20 = 0.0; m21 = 0.0; m22 = scale; m23 = 0.0;
			m30 = 0.0; m31 = 0.0; m32 = 0.0; m33 = 1.0;
		}
		
		/// <summary> Sets the value of this matrix to a translate matrix by the
		/// passed translation value.
		/// </summary>
		/// <param name="v1">the translation amount
		/// </param>
		public void  set_Renamed(Vector3d v1)
		{
			setIdentity();
			setTranslation(v1);
		}
		
		/// <summary> Sets the value of this matrix to a scale and translation matrix;
		/// scale is not applied to the translation and all of the matrix
		/// values are modified.
		/// </summary>
		/// <param name="scale">the scale factor for the matrix
		/// </param>
		/// <param name="v1">the translation amount
		/// </param>
		public void  set_Renamed(double scale, Vector3d v1)
		{
			set_Renamed(scale);
			setTranslation(v1);
		}
		
		/// <summary> Sets the value of this matrix to a scale and translation matrix;
		/// the translation is scaled by the scale factor and all of the
		/// matrix values are modified.
		/// </summary>
		/// <param name="v1">the translation amount
		/// </param>
		/// <param name="scale">the scale factor for the matrix
		/// </param>
		public void  set_Renamed(Vector3d v1, double scale)
		{
			m00 = scale; m01 = 0.0; m02 = 0.0; m03 = scale * v1.x;
			m10 = 0.0; m11 = scale; m12 = 0.0; m13 = scale * v1.y;
			m20 = 0.0; m21 = 0.0; m22 = scale; m23 = scale * v1.z;
			m30 = 0.0; m31 = 0.0; m32 = 0.0; m33 = 1.0;
		}
		
		/// <summary> Sets the value of this matrix from the rotation expressed by the
		/// rotation matrix m1, the translation t1, and the scale s. The translation
		/// is not modified by the scale.
		/// </summary>
		/// <param name="m1">The rotation component
		/// </param>
		/// <param name="t1">The translation component
		/// </param>
		/// <param name="scale">The scale component
		/// </param>
		public void  set_Renamed(Matrix3f m1, Vector3f t1, float scale)
		{
			setRotationScale(m1);
			mulRotationScale(scale);
			setTranslation(t1);
			m33 = 1.0;
		}
		
		/// <summary> Sets the value of this matrix from the rotation expressed by the
		/// rotation matrix m1, the translation t1, and the scale s. The translation
		/// is not modified by the scale.
		/// </summary>
		/// <param name="m1">The rotation component
		/// </param>
		/// <param name="t1">The translation component
		/// </param>
		/// <param name="scale">The scale component
		/// </param>
		public void  set_Renamed(Matrix3d m1, Vector3d t1, double scale)
		{
			setRotationScale(m1);
			mulRotationScale(scale);
			setTranslation(t1);
			m33 = 1.0;
		}
		
		/// <summary> Modifies the translational components of this matrix to the values of
		/// the Vector3d argument; the other values of this matrix are not modified.
		/// </summary>
		/// <param name="trans">the translational component
		/// </param>
		public void  setTranslation(Vector3d trans)
		{
			m03 = trans.x;
			m13 = trans.y;
			m23 = trans.z;
		}
		
		
		/// <summary> Sets the value of this matrix to a rotation matrix about the x axis
		/// by the passed angle. 
		/// </summary>
		/// <param name="angle">the angle to rotate about the X axis in radians 
		/// </param>
		public void  rotX(double angle)
		{
			double c = System.Math.Cos(angle);
			double s = System.Math.Sin(angle);
			m00 = 1.0; m01 = 0.0; m02 = 0.0; m03 = 0.0;
			m10 = 0.0; m11 = c; m12 = - s; m13 = 0.0;
			m20 = 0.0; m21 = s; m22 = c; m23 = 0.0;
			m30 = 0.0; m31 = 0.0; m32 = 0.0; m33 = 1.0;
		}
		
		/// <summary> Sets the value of this matrix to a rotation matrix about the y axis
		/// by the passed angle. 
		/// </summary>
		/// <param name="angle">the angle to rotate about the Y axis in radians 
		/// </param>
		public void  rotY(double angle)
		{
			double c = System.Math.Cos(angle);
			double s = System.Math.Sin(angle);
			m00 = c; m01 = 0.0; m02 = s; m03 = 0.0;
			m10 = 0.0; m11 = 1.0; m12 = 0.0; m13 = 0.0;
			m20 = - s; m21 = 0.0; m22 = c; m23 = 0.0;
			m30 = 0.0; m31 = 0.0; m32 = 0.0; m33 = 1.0;
		}
		
		/// <summary> Sets the value of this matrix to a rotation matrix about the z axis
		/// by the passed angle. 
		/// </summary>
		/// <param name="angle">the angle to rotate about the Z axis in radians 
		/// </param>
		public void  rotZ(double angle)
		{
			double c = System.Math.Cos(angle);
			double s = System.Math.Sin(angle);
			m00 = c; m01 = - s; m02 = 0.0; m03 = 0.0;
			m10 = s; m11 = c; m12 = 0.0; m13 = 0.0;
			m20 = 0.0; m21 = 0.0; m22 = 1.0; m23 = 0.0;
			m30 = 0.0; m31 = 0.0; m32 = 0.0; m33 = 1.0;
		}
		
		/// <summary> Multiplies each element of this matrix by a scalar.</summary>
		/// <param name="scalar">The scalar multiplier.
		/// </param>
		public void  mul(double scalar)
		{
			m00 *= scalar; m01 *= scalar; m02 *= scalar; m03 *= scalar;
			m10 *= scalar; m11 *= scalar; m12 *= scalar; m13 *= scalar;
			m20 *= scalar; m21 *= scalar; m22 *= scalar; m23 *= scalar;
			m30 *= scalar; m31 *= scalar; m32 *= scalar; m33 *= scalar;
		}
		
		/// <summary> Multiplies each element of matrix m1 by a scalar and places the result
		/// into this. Matrix m1 is not modified.
		/// </summary>
		/// <param name="scalar">The scalar multiplier.
		/// </param>
		/// <param name="m1">The original matrix.
		/// </param>
		public void  mul(double scalar, Matrix4d m1)
		{
			set_Renamed(m1);
			mul(scalar);
		}
		
		/// <summary> Sets the value of this matrix to the result of multiplying itself
		/// with matrix m1. 
		/// </summary>
		/// <param name="m1">the other matrix 
		/// </param>
		public void  mul(Matrix4d m1)
		{
			mul(this, m1);
		}
		
		/// <summary> Sets the value of this matrix to the result of multiplying
		/// the two argument matrices together. 
		/// </summary>
		/// <param name="m1">the first matrix 
		/// </param>
		/// <param name="m2">the second matrix 
		/// </param>
		public void  mul(Matrix4d m1, Matrix4d m2)
		{
			// alias-safe way
			set_Renamed(m1.m00 * m2.m00 + m1.m01 * m2.m10 + m1.m02 * m2.m20 + m1.m03 * m2.m30, m1.m00 * m2.m01 + m1.m01 * m2.m11 + m1.m02 * m2.m21 + m1.m03 * m2.m31, m1.m00 * m2.m02 + m1.m01 * m2.m12 + m1.m02 * m2.m22 + m1.m03 * m2.m32, m1.m00 * m2.m03 + m1.m01 * m2.m13 + m1.m02 * m2.m23 + m1.m03 * m2.m33, m1.m10 * m2.m00 + m1.m11 * m2.m10 + m1.m12 * m2.m20 + m1.m13 * m2.m30, m1.m10 * m2.m01 + m1.m11 * m2.m11 + m1.m12 * m2.m21 + m1.m13 * m2.m31, m1.m10 * m2.m02 + m1.m11 * m2.m12 + m1.m12 * m2.m22 + m1.m13 * m2.m32, m1.m10 * m2.m03 + m1.m11 * m2.m13 + m1.m12 * m2.m23 + m1.m13 * m2.m33, m1.m20 * m2.m00 + m1.m21 * m2.m10 + m1.m22 * m2.m20 + m1.m23 * m2.m30, m1.m20 * m2.m01 + m1.m21 * m2.m11 + m1.m22 * m2.m21 + m1.m23 * m2.m31, m1.m20 * m2.m02 + m1.m21 * m2.m12 + m1.m22 * m2.m22 + m1.m23 * m2.m32, m1.m20 * m2.m03 + m1.m21 * m2.m13 + m1.m22 * m2.m23 + m1.m23 * m2.m33, m1.m30 * m2.m00 + m1.m31 * m2.m10 + m1.m32 * m2.m20 + m1.m33 * m2.m30, m1.m30 * m2.m01 + m1.m31 * m2.m11 + m1.m32 * m2.m21 + m1.m33 * m2.m31, m1.m30 * m2.m02 + m1.m31 * m2.m12 + m1.m32 * m2.m22 + m1.m33 * m2.m32, m1.m30 * m2.m03 + m1.m31 * m2.m13 + m1.m32 * m2.m23 + m1.m33 * m2.m33);
		}
		
		/// <summary> Multiplies the transpose of matrix m1 times the transpose of matrix m2,
		/// and places the result into this.
		/// </summary>
		/// <param name="m1">The matrix on the left hand side of the multiplication
		/// </param>
		/// <param name="m2">The matrix on the right hand side of the multiplication
		/// </param>
		public void  mulTransposeBoth(Matrix4d m1, Matrix4d m2)
		{
			mul(m2, m1);
			transpose();
		}
		
		/// <summary> Multiplies matrix m1 times the transpose of matrix m2, and places the
		/// result into this.
		/// </summary>
		/// <param name="m1">The matrix on the left hand side of the multiplication
		/// </param>
		/// <param name="m2">The matrix on the right hand side of the multiplication
		/// </param>
		public void  mulTransposeRight(Matrix4d m1, Matrix4d m2)
		{
			// alias-safe way.
			set_Renamed(m1.m00 * m2.m00 + m1.m01 * m2.m01 + m1.m02 * m2.m02 + m1.m03 * m2.m03, m1.m00 * m2.m10 + m1.m01 * m2.m11 + m1.m02 * m2.m12 + m1.m03 * m2.m13, m1.m00 * m2.m20 + m1.m01 * m2.m21 + m1.m02 * m2.m22 + m1.m03 * m2.m23, m1.m00 * m2.m30 + m1.m01 * m2.m31 + m1.m02 * m2.m32 + m1.m03 * m2.m33, m1.m10 * m2.m00 + m1.m11 * m2.m01 + m1.m12 * m2.m02 + m1.m13 * m2.m03, m1.m10 * m2.m10 + m1.m11 * m2.m11 + m1.m12 * m2.m12 + m1.m13 * m2.m13, m1.m10 * m2.m20 + m1.m11 * m2.m21 + m1.m12 * m2.m22 + m1.m13 * m2.m23, m1.m10 * m2.m30 + m1.m11 * m2.m31 + m1.m12 * m2.m32 + m1.m13 * m2.m33, m1.m20 * m2.m00 + m1.m21 * m2.m01 + m1.m22 * m2.m02 + m1.m23 * m2.m03, m1.m20 * m2.m10 + m1.m21 * m2.m11 + m1.m22 * m2.m12 + m1.m23 * m2.m13, m1.m20 * m2.m20 + m1.m21 * m2.m21 + m1.m22 * m2.m22 + m1.m23 * m2.m23, m1.m20 * m2.m30 + m1.m21 * m2.m31 + m1.m22 * m2.m32 + m1.m23 * m2.m33, m1.m30 * m2.m00 + m1.m31 * m2.m01 + m1.m32 * m2.m02 + m1.m33 * m2.m03, m1.m30 * m2.m10 + m1.m31 * m2.m11 + m1.m32 * m2.m12 + m1.m33 * m2.m13, m1.m30 * m2.m20 + m1.m31 * m2.m21 + m1.m32 * m2.m22 + m1.m33 * m2.m23, m1.m30 * m2.m30 + m1.m31 * m2.m31 + m1.m32 * m2.m32 + m1.m33 * m2.m33);
		}
		
		
		/// <summary> Multiplies the transpose of matrix m1 times matrix m2, and places the
		/// result into this.
		/// </summary>
		/// <param name="m1">The matrix on the left hand side of the multiplication
		/// </param>
		/// <param name="m2">The matrix on the right hand side of the multiplication
		/// </param>
		public void  mulTransposeLeft(Matrix4d m1, Matrix4d m2)
		{
			// alias-safe way.
			set_Renamed(m1.m00 * m2.m00 + m1.m10 * m2.m10 + m1.m20 * m2.m20 + m1.m30 * m2.m30, m1.m00 * m2.m01 + m1.m10 * m2.m11 + m1.m20 * m2.m21 + m1.m30 * m2.m31, m1.m00 * m2.m02 + m1.m10 * m2.m12 + m1.m20 * m2.m22 + m1.m30 * m2.m32, m1.m00 * m2.m03 + m1.m10 * m2.m13 + m1.m20 * m2.m23 + m1.m30 * m2.m33, m1.m01 * m2.m00 + m1.m11 * m2.m10 + m1.m21 * m2.m20 + m1.m31 * m2.m30, m1.m01 * m2.m01 + m1.m11 * m2.m11 + m1.m21 * m2.m21 + m1.m31 * m2.m31, m1.m01 * m2.m02 + m1.m11 * m2.m12 + m1.m21 * m2.m22 + m1.m31 * m2.m32, m1.m01 * m2.m03 + m1.m11 * m2.m13 + m1.m21 * m2.m23 + m1.m31 * m2.m33, m1.m02 * m2.m00 + m1.m12 * m2.m10 + m1.m22 * m2.m20 + m1.m32 * m2.m30, m1.m02 * m2.m01 + m1.m12 * m2.m11 + m1.m22 * m2.m21 + m1.m32 * m2.m31, m1.m02 * m2.m02 + m1.m12 * m2.m12 + m1.m22 * m2.m22 + m1.m32 * m2.m32, m1.m02 * m2.m03 + m1.m12 * m2.m13 + m1.m22 * m2.m23 + m1.m32 * m2.m33, m1.m03 * m2.m00 + m1.m13 * m2.m10 + m1.m23 * m2.m20 + m1.m33 * m2.m30, m1.m03 * m2.m01 + m1.m13 * m2.m11 + m1.m23 * m2.m21 + m1.m33 * m2.m31, m1.m03 * m2.m02 + m1.m13 * m2.m12 + m1.m23 * m2.m22 + m1.m33 * m2.m32, m1.m03 * m2.m03 + m1.m13 * m2.m13 + m1.m23 * m2.m23 + m1.m33 * m2.m33);
		}
		
		
		/// <summary> Returns true if all of the data members of Matrix4d m1 are
		/// equal to the corresponding data members in this Matrix4d. 
		/// </summary>
		/// <param name="m1">The matrix with which the comparison is made. 
		/// </param>
		/// <returns> true or false 
		/// </returns>
		public bool equals(Matrix4d m1)
		{
			return m1 != null && m00 == m1.m00 && m01 == m1.m01 && m02 == m1.m02 && m03 == m1.m03 && m10 == m1.m10 && m11 == m1.m11 && m12 == m1.m12 && m13 == m1.m13 && m20 == m1.m20 && m21 == m1.m21 && m22 == m1.m22 && m23 == m1.m23 && m30 == m1.m30 && m31 == m1.m31 && m32 == m1.m32 && m33 == m1.m33;
		}
		
		/// <summary> Returns true if the Object o1 is of type Matrix4d and all of the data
		/// members of t1 are equal to the corresponding data members in this
		/// Matrix4d.
		/// </summary>
		/// <param name="o1">the object with which the comparison is made.
		/// </param>
		public  override bool Equals(System.Object o1)
		{
			return o1 != null && (o1 is Matrix4d) && equals((Matrix4d) o1);
		}
		
		/// <summary> Returns true if the L-infinite distance between this matrix and matrix
		/// m1 is less than or equal to the epsilon parameter, otherwise returns
		/// false. The L-infinite distance is equal to MAX[i=0,1,2,3 ; j=0,1,2,3 ;
		/// abs(this.m(i,j) - m1.m(i,j)]
		/// </summary>
		/// <param name="m1">The matrix to be compared to this matrix
		/// </param>
		/// <param name="epsilon">the threshold value
		/// </param>
		/// <deprecated> As of Java3D API1.1 Beta02
		/// </deprecated>
		public virtual bool epsilonEquals(Matrix4d m1, float epsilon)
		{
			// why epsilon is float ??
			return System.Math.Abs(m00 - m1.m00) <= epsilon && System.Math.Abs(m01 - m1.m01) <= epsilon && System.Math.Abs(m02 - m1.m02) <= epsilon && System.Math.Abs(m03 - m1.m03) <= epsilon && System.Math.Abs(m10 - m1.m10) <= epsilon && System.Math.Abs(m11 - m1.m11) <= epsilon && System.Math.Abs(m12 - m1.m12) <= epsilon && System.Math.Abs(m13 - m1.m13) <= epsilon && System.Math.Abs(m20 - m1.m20) <= epsilon && System.Math.Abs(m21 - m1.m21) <= epsilon && System.Math.Abs(m22 - m1.m22) <= epsilon && System.Math.Abs(m23 - m1.m23) <= epsilon && System.Math.Abs(m30 - m1.m30) <= epsilon && System.Math.Abs(m31 - m1.m31) <= epsilon && System.Math.Abs(m32 - m1.m32) <= epsilon && System.Math.Abs(m33 - m1.m33) <= epsilon;
		}
		
		/// <summary> Returns true if the L-infinite distance between this matrix and matrix
		/// m1 is less than or equal to the epsilon parameter, otherwise returns
		/// false. The L-infinite distance is equal to MAX[i=0,1,2,3 ; j=0,1,2,3 ;
		/// abs(this.m(i,j) - m1.m(i,j)]
		/// </summary>
		/// <param name="m1">The matrix to be compared to this matrix
		/// </param>
		/// <param name="epsilon">the threshold value
		/// </param>
		public virtual bool epsilonEquals(Matrix4d m1, double epsilon)
		{
			return System.Math.Abs(m00 - m1.m00) <= epsilon && System.Math.Abs(m01 - m1.m01) <= epsilon && System.Math.Abs(m02 - m1.m02) <= epsilon && System.Math.Abs(m03 - m1.m03) <= epsilon && System.Math.Abs(m10 - m1.m10) <= epsilon && System.Math.Abs(m11 - m1.m11) <= epsilon && System.Math.Abs(m12 - m1.m12) <= epsilon && System.Math.Abs(m13 - m1.m13) <= epsilon && System.Math.Abs(m20 - m1.m20) <= epsilon && System.Math.Abs(m21 - m1.m21) <= epsilon && System.Math.Abs(m22 - m1.m22) <= epsilon && System.Math.Abs(m23 - m1.m23) <= epsilon && System.Math.Abs(m30 - m1.m30) <= epsilon && System.Math.Abs(m31 - m1.m31) <= epsilon && System.Math.Abs(m32 - m1.m32) <= epsilon && System.Math.Abs(m33 - m1.m33) <= epsilon;
		}
		
		/// <summary> Returns a hash number based on the data values in this
		/// object.  Two different Matrix4d objects with identical data values
		/// (ie, returns true for equals(Matrix4d) ) will return the same hash
		/// number.  Two objects with different data members may return the
		/// same hash value, although this is not likely.
		/// </summary>
		/// <returns> the integer hash value 
		/// </returns>
        //public override int GetHashCode()
        //{
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    long bits = Double.doubleToLongBits(m00);
        //    int hash = (int) (bits ^ (bits >> 32));
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    bits = Double.doubleToLongBits(m01);
        //    hash ^= (int) (bits ^ (bits >> 32));
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    bits = Double.doubleToLongBits(m02);
        //    hash ^= (int) (bits ^ (bits >> 32));
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    bits = Double.doubleToLongBits(m03);
        //    hash ^= (int) (bits ^ (bits >> 32));
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    bits = Double.doubleToLongBits(m10);
        //    hash ^= (int) (bits ^ (bits >> 32));
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    bits = Double.doubleToLongBits(m11);
        //    hash ^= (int) (bits ^ (bits >> 32));
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    bits = Double.doubleToLongBits(m12);
        //    hash ^= (int) (bits ^ (bits >> 32));
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    bits = Double.doubleToLongBits(m13);
        //    hash ^= (int) (bits ^ (bits >> 32));
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    bits = Double.doubleToLongBits(m20);
        //    hash ^= (int) (bits ^ (bits >> 32));
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    bits = Double.doubleToLongBits(m21);
        //    hash ^= (int) (bits ^ (bits >> 32));
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    bits = Double.doubleToLongBits(m22);
        //    hash ^= (int) (bits ^ (bits >> 32));
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    bits = Double.doubleToLongBits(m23);
        //    hash ^= (int) (bits ^ (bits >> 32));
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    bits = Double.doubleToLongBits(m30);
        //    hash ^= (int) (bits ^ (bits >> 32));
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    bits = Double.doubleToLongBits(m31);
        //    hash ^= (int) (bits ^ (bits >> 32));
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    bits = Double.doubleToLongBits(m32);
        //    hash ^= (int) (bits ^ (bits >> 32));
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    bits = Double.doubleToLongBits(m33);
        //    hash ^= (int) (bits ^ (bits >> 32));
			
        //    return hash;
        //}
		
		/// <summary> Transform the vector vec using this Matrix4d and place the
		/// result into vecOut.
		/// </summary>
		/// <param name="vec">the double precision vector to be transformed
		/// </param>
		/// <param name="vecOut">the vector into which the transformed values are placed
		/// </param>
		public void  transform(Tuple4d vec, Tuple4d vecOut)
		{
			// alias-safe
			vecOut.set_Renamed(m00 * vec.x + m01 * vec.y + m02 * vec.z + m03 * vec.w, m10 * vec.x + m11 * vec.y + m12 * vec.z + m13 * vec.w, m20 * vec.x + m21 * vec.y + m22 * vec.z + m23 * vec.w, m30 * vec.x + m31 * vec.y + m32 * vec.z + m33 * vec.w);
		}
		
		/// <summary> Transform the vector vec using this Matrix4d and place the
		/// result back into vec.
		/// </summary>
		/// <param name="vec">the double precision vector to be transformed
		/// </param>
		public void  transform(Tuple4d vec)
		{
			transform(vec, vec);
		}
		
		/// <summary> Transform the vector vec using this Matrix4d and place the
		/// result into vecOut.
		/// </summary>
		/// <param name="vec">the single precision vector to be transformed
		/// </param>
		/// <param name="vecOut">the vector into which the transformed values are placed
		/// </param>
		public void  transform(Tuple4f vec, Tuple4f vecOut)
		{
			// alias-safe
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			vecOut.set_Renamed((float) (m00 * vec.x + m01 * vec.y + m02 * vec.z + m03 * vec.w), (float) (m10 * vec.x + m11 * vec.y + m12 * vec.z + m13 * vec.w), (float) (m20 * vec.x + m21 * vec.y + m22 * vec.z + m23 * vec.w), (float) (m30 * vec.x + m31 * vec.y + m32 * vec.z + m33 * vec.w));
		}
		
		/// <summary> Transform the vector vec using this Matrix4d and place the
		/// result back into vec.
		/// </summary>
		/// <param name="vec">the single precision vector to be transformed
		/// </param>
		public void  transform(Tuple4f vec)
		{
			transform(vec, vec);
		}
		
		/// <summary> Transforms the point parameter with this Matrix4d and places the result
		/// into pointOut. The fourth element of the point input paramter is assumed
		/// to be one.
		/// </summary>
		/// <param name="point">the input point to be transformed.
		/// </param>
		/// <param name="pointOut">the transformed point
		/// </param>
		public void  transform(Point3d point, Point3d pointOut)
		{
			pointOut.set_Renamed(m00 * point.x + m01 * point.y + m02 * point.z + m03, m10 * point.x + m11 * point.y + m12 * point.z + m13, m20 * point.x + m21 * point.y + m22 * point.z + m23);
		}
		
		
		/// <summary> Transforms the point parameter with this Matrix4d and
		/// places the result back into point.  The fourth element of the
		/// point input paramter is assumed to be one.
		/// </summary>
		/// <param name="point">the input point to be transformed.
		/// </param>
		public void  transform(Point3d point)
		{
			transform(point, point);
		}
		
		/// <summary> Transforms the point parameter with this Matrix4d and places the result
		/// into pointOut. The fourth element of the point input paramter is assumed
		/// to be one.
		/// </summary>
		/// <param name="point">the input point to be transformed.
		/// </param>
		/// <param name="pointOut">the transformed point
		/// </param>
		public void  transform(Point3f point, Point3f pointOut)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			pointOut.set_Renamed((float) (m00 * point.x + m01 * point.y + m02 * point.z + m03), (float) (m10 * point.x + m11 * point.y + m12 * point.z + m13), (float) (m20 * point.x + m21 * point.y + m22 * point.z + m23));
		}
		
		/// <summary> Transforms the point parameter with this Matrix4d and
		/// places the result back into point.  The fourth element of the
		/// point input paramter is assumed to be one.
		/// </summary>
		/// <param name="point">the input point to be transformed.
		/// </param>
		public void  transform(Point3f point)
		{
			transform(point, point);
		}
		
		/// <summary> Transforms the normal parameter by this Matrix4d and places the value
		/// into normalOut.  The fourth element of the normal is assumed to be zero.
		/// </summary>
		/// <param name="normal">the input normal to be transformed.
		/// </param>
		/// <param name="normalOut">the transformed normal
		/// </param>
		public void  transform(Vector3d normal, Vector3d normalOut)
		{
			normalOut.set_Renamed(m00 * normal.x + m01 * normal.y + m02 * normal.z, m10 * normal.x + m11 * normal.y + m12 * normal.z, m20 * normal.x + m21 * normal.y + m22 * normal.z);
		}
		
		/// <summary> Transforms the normal parameter by this transform and places the value
		/// back into normal.  The fourth element of the normal is assumed to be zero.
		/// </summary>
		/// <param name="normal">the input normal to be transformed.
		/// </param>
		public void  transform(Vector3d normal)
		{
			transform(normal, normal);
		}
		
		/// <summary> Transforms the normal parameter by this Matrix4d and places the value
		/// into normalOut.  The fourth element of the normal is assumed to be zero.
		/// </summary>
		/// <param name="normal">the input normal to be transformed.
		/// </param>
		/// <param name="normalOut">the transformed normal
		/// </param>
		public void  transform(Vector3f normal, Vector3f normalOut)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			normalOut.set_Renamed((float) (m00 * normal.x + m01 * normal.y + m02 * normal.z), (float) (m10 * normal.x + m11 * normal.y + m12 * normal.z), (float) (m20 * normal.x + m21 * normal.y + m22 * normal.z));
		}
		
		/// <summary> Transforms the normal parameter by this transform and places the value
		/// back into normal.  The fourth element of the normal is assumed to be zero.
		/// </summary>
		/// <param name="normal">the input normal to be transformed.
		/// </param>
		public void  transform(Vector3f normal)
		{
			transform(normal, normal);
		}
		
		/// <summary> Sets the rotational component (upper 3x3) of this matrix to the matrix
		/// values in the double precision Matrix3d argument; the other elements of
		/// this matrix are unchanged; a singular value decomposition is performed
		/// on this object's upper 3x3 matrix to factor out the scale, then this
		/// object's upper 3x3 matrix components are replaced by the passed rotation
		/// components, and then the scale is reapplied to the rotational
		/// components.
		/// </summary>
		/// <param name="m1">double precision 3x3 matrix
		/// </param>
		public void  setRotation(Matrix3d m1)
		{
			double scale = SVD(null, null);
			setRotationScale(m1);
			mulRotationScale(scale);
		}
		
		/// <summary> Sets the rotational component (upper 3x3) of this matrix to the matrix
		/// values in the single precision Matrix3f argument; the other elements of
		/// this matrix are unchanged; a singular value decomposition is performed
		/// on this object's upper 3x3 matrix to factor out the scale, then this
		/// object's upper 3x3 matrix components are replaced by the passed rotation
		/// components, and then the scale is reapplied to the rotational
		/// components.
		/// </summary>
		/// <param name="m1">single precision 3x3 matrix
		/// </param>
		public void  setRotation(Matrix3f m1)
		{
			double scale = SVD(null, null);
			setRotationScale(m1);
			mulRotationScale(scale);
		}
		
		/// <summary> Sets the rotational component (upper 3x3) of this matrix to the matrix
		/// equivalent values of the quaternion argument; the other elements of this
		/// matrix are unchanged; a singular value decomposition is performed on
		/// this object's upper 3x3 matrix to factor out the scale, then this
		/// object's upper 3x3 matrix components are replaced by the matrix
		/// equivalent of the quaternion, and then the scale is reapplied to the
		/// rotational components.
		/// </summary>
		/// <param name="q1">the quaternion that specifies the rotation
		/// </param>
		public void  setRotation(Quat4f q1)
		{
			double scale = SVD(null, null);
			
			// save other values
			double tx = m03;
			double ty = m13;
			double tz = m23;
			double w0 = m30;
			double w1 = m31;
			double w2 = m32;
			double w3 = m33;
			
			set_Renamed(q1);
			mulRotationScale(scale);
			
			// set back
			m03 = tx;
			m13 = ty;
			m23 = tz;
			m30 = w0;
			m31 = w1;
			m32 = w2;
			m33 = w3;
		}
		
		/// <summary> Sets the rotational component (upper 3x3) of this matrix to the matrix
		/// equivalent values of the quaternion argument; the other elements of this
		/// matrix are unchanged; a singular value decomposition is performed on
		/// this object's upper 3x3 matrix to factor out the scale, then this
		/// object's upper 3x3 matrix components are replaced by the matrix
		/// equivalent of the quaternion, and then the scale is reapplied to the
		/// rotational components.
		/// </summary>
		/// <param name="q1">the quaternion that specifies the rotation
		/// </param>
		public void  setRotation(Quat4d q1)
		{
			double scale = SVD(null, null);
			// save other values
			double tx = m03;
			double ty = m13;
			double tz = m23;
			double w0 = m30;
			double w1 = m31;
			double w2 = m32;
			double w3 = m33;
			
			set_Renamed(q1);
			mulRotationScale(scale);
			
			// set back
			m03 = tx;
			m13 = ty;
			m23 = tz;
			m30 = w0;
			m31 = w1;
			m32 = w2;
			m33 = w3;
		}
		
		/// <summary> Sets the rotational component (upper 3x3) of this matrix to the matrix
		/// equivalent values of the axis-angle argument; the other elements of this
		/// matrix are unchanged; a singular value decomposition is performed on
		/// this object's upper 3x3 matrix to factor out the scale, then this
		/// object's upper 3x3 matrix components are replaced by the matrix
		/// equivalent of the axis-angle, and then the scale is reapplied to the
		/// rotational components.
		/// </summary>
		/// <param name="a1">the axis-angle to be converted (x, y, z, angle)
		/// </param>
		public void  setRotation(AxisAngle4d a1)
		{
			double scale = SVD(null, null);
			// save other values
			double tx = m03;
			double ty = m13;
			double tz = m23;
			double w0 = m30;
			double w1 = m31;
			double w2 = m32;
			double w3 = m33;
			
			set_Renamed(a1);
			mulRotationScale(scale);
			
			// set back
			m03 = tx;
			m13 = ty;
			m23 = tz;
			m30 = w0;
			m31 = w1;
			m32 = w2;
			m33 = w3;
		}
		
		/// <summary> Sets this matrix to all zeros.</summary>
		public void  setZero()
		{
			m00 = 0.0; m01 = 0.0; m02 = 0.0; m03 = 0.0;
			m10 = 0.0; m11 = 0.0; m12 = 0.0; m13 = 0.0;
			m20 = 0.0; m21 = 0.0; m22 = 0.0; m23 = 0.0;
			m30 = 0.0; m31 = 0.0; m32 = 0.0; m33 = 0.0;
		}
		
		/// <summary> Negates the value of this matrix: this = -this.</summary>
		public void  negate()
		{
			m00 = - m00; m01 = - m01; m02 = - m02; m03 = - m03;
			m10 = - m10; m11 = - m11; m12 = - m12; m13 = - m13;
			m20 = - m20; m21 = - m21; m22 = - m22; m23 = - m23;
			m30 = - m30; m31 = - m31; m32 = - m32; m33 = - m33;
		}
		
		/// <summary> Sets the value of this matrix equal to the negation of of the Matrix4d
		/// parameter.
		/// </summary>
		/// <param name="m1">The source matrix
		/// </param>
		public void  negate(Matrix4d m1)
		{
			set_Renamed(m1);
			negate();
		}
		
		/// <summary> Sets 16 values	</summary>
		private void  set_Renamed(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13, double m20, double m21, double m22, double m23, double m30, double m31, double m32, double m33)
		{
			this.m00 = m00; this.m01 = m01; this.m02 = m02; this.m03 = m03;
			this.m10 = m10; this.m11 = m11; this.m12 = m12; this.m13 = m13;
			this.m20 = m20; this.m21 = m21; this.m22 = m22; this.m23 = m23;
			this.m30 = m30; this.m31 = m31; this.m32 = m32; this.m33 = m33;
		}
		
		/// <summary> Performs SVD on this matrix and gets scale and rotation.
		/// Rotation is placed into rot.
		/// </summary>
		/// <param name="rot3">the rotation factor(Matrix3d).
		/// </param>
		/// <param name="rot4">the rotation factor(Matrix4d) only upper 3x3 elements are changed.
		/// </param>
		/// <returns> scale factor
		/// </returns>
		private double SVD(Matrix3d rot3, Matrix4d rot4)
		{
			// this is a simple svd.
			// Not complete but fast and reasonable.
			// See comment in Matrix3d.
			
			double s = System.Math.Sqrt((m00 * m00 + m10 * m10 + m20 * m20 + m01 * m01 + m11 * m11 + m21 * m21 + m02 * m02 + m12 * m12 + m22 * m22) / 3.0);
			
			if (rot3 != null)
			{
				this.getRotationScale(rot3);
				
				// zero-div may occur.
				double n = 1 / System.Math.Sqrt(m00 * m00 + m10 * m10 + m20 * m20);
				rot3.m00 *= n;
				rot3.m10 *= n;
				rot3.m20 *= n;
				
				n = 1 / System.Math.Sqrt(m01 * m01 + m11 * m11 + m21 * m21);
				rot3.m01 *= n;
				rot3.m11 *= n;
				rot3.m21 *= n;
				
				n = 1 / System.Math.Sqrt(m02 * m02 + m12 * m12 + m22 * m22);
				rot3.m02 *= n;
				rot3.m12 *= n;
				rot3.m22 *= n;
			}
			
			if (rot4 != null)
			{
				if (rot4 != this)
					rot4.setRotationScale(this); // private method
				
				// zero-div may occur.
				double n = 1 / System.Math.Sqrt(m00 * m00 + m10 * m10 + m20 * m20);
				rot4.m00 *= n;
				rot4.m10 *= n;
				rot4.m20 *= n;
				
				n = 1 / System.Math.Sqrt(m01 * m01 + m11 * m11 + m21 * m21);
				rot4.m01 *= n;
				rot4.m11 *= n;
				rot4.m21 *= n;
				
				n = 1 / System.Math.Sqrt(m02 * m02 + m12 * m12 + m22 * m22);
				rot4.m02 *= n;
				rot4.m12 *= n;
				rot4.m22 *= n;
			}
			
			return s;
		}
		
		/// <summary> Performs SVD on this matrix and gets the scale and the pure rotation.
		/// The pure rotation is placed into rot.
		/// </summary>
		/// <param name="rot">the rotation factor.
		/// </param>
		/// <returns> scale factor
		/// </returns>
		private float SVD(Matrix3f rot)
		{
			// this is a simple svd.
			// Not complete but fast and reasonable.
			// See comment in Matrix3d.
			
			double s = System.Math.Sqrt((m00 * m00 + m10 * m10 + m20 * m20 + m01 * m01 + m11 * m11 + m21 * m21 + m02 * m02 + m12 * m12 + m22 * m22) / 3.0);
			
			// zero-div may occur.
			double t = (s == 0.0?0.0:1.0 / s);
			
			if (rot != null)
			{
				this.getRotationScale(rot);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				rot.mul((float) t);
			}
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (float) s;
		}
		
		/// <summary> Multiplies 3x3 upper elements of this matrix by a scalar.
		/// The other elements are unchanged.
		/// </summary>
		private void  mulRotationScale(double scale)
		{
			m00 *= scale; m01 *= scale; m02 *= scale;
			m10 *= scale; m11 *= scale; m12 *= scale;
			m20 *= scale; m21 *= scale; m22 *= scale;
		}
		
		/// <summary> Sets only 3x3 upper elements of this matrix to that of m1.
		/// The other elements are unchanged.
		/// </summary>
		private void  setRotationScale(Matrix4d m1)
		{
			m00 = m1.m00; m01 = m1.m01; m02 = m1.m02;
			m10 = m1.m10; m11 = m1.m11; m12 = m1.m12;
			m20 = m1.m20; m21 = m1.m21; m22 = m1.m22;
		}
		
		/// <summary> Modifies the translational components of this matrix to the values of
		/// the Vector3f argument; the other values of this matrix are not modified.
		/// </summary>
		/// <param name="trans">the translational component
		/// </param>
		private void  setTranslation(Vector3f trans)
		{
			m03 = trans.x;
			m13 = trans.y;
			m23 = trans.z;
		}
		private void  setFromQuat(double x, double y, double z, double w)
		{
			double n = x * x + y * y + z * z + w * w;
			double s = (n > 0.0)?(2.0 / n):0.0;
			
			double xs = x * s, ys = y * s, zs = z * s;
			double wx = w * xs, wy = w * ys, wz = w * zs;
			double xx = x * xs, xy = x * ys, xz = x * zs;
			double yy = y * ys, yz = y * zs, zz = z * zs;
			
			setIdentity();
			m00 = 1.0 - (yy + zz); m01 = xy - wz; m02 = xz + wy;
			m10 = xy + wz; m11 = 1.0 - (xx + zz); m12 = yz - wx;
			m20 = xz - wy; m21 = yz + wx; m22 = 1.0 - (xx + yy);
		}
		private void  setFromAxisAngle(double x, double y, double z, double angle)
		{
			// Taken from Rick's which is taken from Wertz. pg. 412
			// Bug Fixed and changed into right-handed by hiranabe
			double n = System.Math.Sqrt(x * x + y * y + z * z);
			// zero-div may occur
			n = 1 / n;
			x *= n;
			y *= n;
			z *= n;
			double c = System.Math.Cos(angle);
			double s = System.Math.Sin(angle);
			double omc = 1.0 - c;
			m00 = c + x * x * omc;
			m11 = c + y * y * omc;
			m22 = c + z * z * omc;
			
			double tmp1 = x * y * omc;
			double tmp2 = z * s;
			m01 = tmp1 - tmp2;
			m10 = tmp1 + tmp2;
			
			tmp1 = x * z * omc;
			tmp2 = y * s;
			m02 = tmp1 + tmp2;
			m20 = tmp1 - tmp2;
			
			tmp1 = y * z * omc;
			tmp2 = x * s;
			m12 = tmp1 - tmp2;
			m21 = tmp1 + tmp2;
		}
	}
}