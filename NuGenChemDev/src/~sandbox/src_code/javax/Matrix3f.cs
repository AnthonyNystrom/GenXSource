/*
* $RCSfile: Matrix3f.java,v $
*
* Copyright (c) 2005 Sun Microsystems, Inc. All rights reserved.
*
* Use is subject to license terms.
*
* $Revision: 1.3 $
* $Date: 2005/02/18 16:28:09 $
* $State: Exp $
*/
using System;
namespace javax.vecmath
{
	
	/// <summary> A single precision floating point 3 by 3 matrix.
	/// Primarily to support 3D rotations.
	/// 
	/// </summary>
	[Serializable]
	public class Matrix3f : System.ICloneable
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Performs an SVD normalization of this matrix to calculate
		/// and return the uniform scale factor. If the matrix has non-uniform 
		/// scale factors, the largest of the x, y, and z scale factors will 
		/// be returned. This matrix is not modified.
		/// </summary>
		/// <returns>  the scale factor of this matrix
		/// </returns>
		/// <summary> Sets the scale component of the current matrix by factoring
		/// out the current scale (by doing an SVD) and multiplying by 
		/// the new scale.
		/// </summary>
		/// <param name="scale"> the new scale amount
		/// </param>
		virtual public float Scale
		{
			get
			{
				
				double[] tmp_rot = new double[9]; // scratch matrix
				double[] tmp_scale = new double[3]; // scratch matrix
				getScaleRotate(tmp_scale, tmp_rot);
				
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				return ((float) Matrix3d.max3(tmp_scale));
			}
			
			set
			{
				double[] tmp_rot = new double[9]; // scratch matrix
				double[] tmp_scale = new double[3]; // scratch matrix
				
				getScaleRotate(tmp_scale, tmp_rot);
				
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				this.m00 = (float) (tmp_rot[0] * value);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				this.m01 = (float) (tmp_rot[1] * value);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				this.m02 = (float) (tmp_rot[2] * value);
				
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				this.m10 = (float) (tmp_rot[3] * value);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				this.m11 = (float) (tmp_rot[4] * value);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				this.m12 = (float) (tmp_rot[5] * value);
				
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				this.m20 = (float) (tmp_rot[6] * value);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				this.m21 = (float) (tmp_rot[7] * value);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				this.m22 = (float) (tmp_rot[8] * value);
			}
			
		}
		
		// Compatible with 1.1
		internal const long serialVersionUID = 329697160112089834L;
		
		/// <summary> The first matrix element in the first row.</summary>
		public float m00;
		
		/// <summary> The second matrix element in the first row.</summary>
		public float m01;
		
		/// <summary> The third matrix element in the first row.</summary>
		public float m02;
		
		/// <summary> The first matrix element in the second row.</summary>
		public float m10;
		
		/// <summary> The second matrix element in the second row.</summary>
		public float m11;
		
		/// <summary> The third matrix element in the second row.</summary>
		public float m12;
		
		/// <summary> The first matrix element in the third row.</summary>
		public float m20;
		
		/// <summary> The second matrix element in the third row.</summary>
		public float m21;
		
		/// <summary> The third matrix element in the third row.</summary>
		public float m22;
		/*
		double[]    tmp       = new double[9];  // scratch matrix
		double[]    tmp_rot   = new double[9];  // scratch matrix
		double[]    tmp_scale = new double[3];  // scratch matrix
		*/
		private const double EPS = 1.0e-8;
		
		
		
		/// <summary> Constructs and initializes a Matrix3f from the specified nine values.</summary>
		/// <param name="m00">the [0][0] element
		/// </param>
		/// <param name="m01">the [0][1] element
		/// </param>
		/// <param name="m02">the [0][2] element
		/// </param>
		/// <param name="m10">the [1][0] element
		/// </param>
		/// <param name="m11">the [1][1] element
		/// </param>
		/// <param name="m12">the [1][2] element
		/// </param>
		/// <param name="m20">the [2][0] element
		/// </param>
		/// <param name="m21">the [2][1] element
		/// </param>
		/// <param name="m22">the [2][2] element
		/// </param>
		public Matrix3f(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
		{
			this.m00 = m00;
			this.m01 = m01;
			this.m02 = m02;
			
			this.m10 = m10;
			this.m11 = m11;
			this.m12 = m12;
			
			this.m20 = m20;
			this.m21 = m21;
			this.m22 = m22;
		}
		
		/// <summary> Constructs and initializes a Matrix3f from the specified 
		/// nine-element array.   this.m00 =v[0], this.m01=v[1], etc.
		/// </summary>
		/// <param name="v">the array of length 9 containing in order
		/// </param>
		public Matrix3f(float[] v)
		{
			this.m00 = v[0];
			this.m01 = v[1];
			this.m02 = v[2];
			
			this.m10 = v[3];
			this.m11 = v[4];
			this.m12 = v[5];
			
			this.m20 = v[6];
			this.m21 = v[7];
			this.m22 = v[8];
		}
		
		/// <summary>  Constructs a new matrix with the same values as the 
		/// Matrix3d parameter.
		/// </summary>
		/// <param name="m1"> the source matrix
		/// </param>
		public Matrix3f(Matrix3d m1)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m00 = (float) m1.m00;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m01 = (float) m1.m01;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m02 = (float) m1.m02;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m10 = (float) m1.m10;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m11 = (float) m1.m11;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m12 = (float) m1.m12;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m20 = (float) m1.m20;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m21 = (float) m1.m21;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m22 = (float) m1.m22;
		}
		
		
		/// <summary>  Constructs a new matrix with the same values as the
		/// Matrix3f parameter.
		/// </summary>
		/// <param name="m1"> the source matrix
		/// </param>
		public Matrix3f(Matrix3f m1)
		{
			this.m00 = m1.m00;
			this.m01 = m1.m01;
			this.m02 = m1.m02;
			
			this.m10 = m1.m10;
			this.m11 = m1.m11;
			this.m12 = m1.m12;
			
			this.m20 = m1.m20;
			this.m21 = m1.m21;
			this.m22 = m1.m22;
		}
		
		
		/// <summary> Constructs and initializes a Matrix3f to all zeros.</summary>
		public Matrix3f()
		{
			this.m00 = (float) 0.0;
			this.m01 = (float) 0.0;
			this.m02 = (float) 0.0;
			
			this.m10 = (float) 0.0;
			this.m11 = (float) 0.0;
			this.m12 = (float) 0.0;
			
			this.m20 = (float) 0.0;
			this.m21 = (float) 0.0;
			this.m22 = (float) 0.0;
		}
		
		/// <summary> Returns a string that contains the values of this Matrix3f.</summary>
		/// <returns> the String representation
		/// </returns>
		public override System.String ToString()
		{
			return this.m00 + ", " + this.m01 + ", " + this.m02 + "\n" + this.m10 + ", " + this.m11 + ", " + this.m12 + "\n" + this.m20 + ", " + this.m21 + ", " + this.m22 + "\n";
		}
		
		/// <summary> Sets this Matrix3f to identity.</summary>
		public void  setIdentity()
		{
			this.m00 = (float) 1.0;
			this.m01 = (float) 0.0;
			this.m02 = (float) 0.0;
			
			this.m10 = (float) 0.0;
			this.m11 = (float) 1.0;
			this.m12 = (float) 0.0;
			
			this.m20 = (float) 0.0;
			this.m21 = (float) 0.0;
			this.m22 = (float) 1.0;
		}
		
		/// <summary> Sets the specified element of this matrix3f to the value provided.</summary>
		/// <param name="row">the row number to be modified (zero indexed)
		/// </param>
		/// <param name="column">the column number to be modified (zero indexed)
		/// </param>
		/// <param name="value">the new value
		/// </param>
		public void  setElement(int row, int column, float value_Renamed)
		{
			switch (row)
			{
				
				case 0: 
					switch (column)
					{
						
						case 0: 
							this.m00 = value_Renamed;
							break;
						
						case 1: 
							this.m01 = value_Renamed;
							break;
						
						case 2: 
							this.m02 = value_Renamed;
							break;
						
						default: 
							throw new /*ArrayIndexOutOfBounds*/Exception();//VecMathI18N.getString("Matrix3f0"));
						
					}
					break;
				
				
				case 1: 
					switch (column)
					{
						
						case 0: 
							this.m10 = value_Renamed;
							break;
						
						case 1: 
							this.m11 = value_Renamed;
							break;
						
						case 2: 
							this.m12 = value_Renamed;
							break;
						
						default:
                            throw new Exception();//Exception();//ArrayIndexOutOfBoundsException(VecMathI18N.getString("Matrix3f0"));
						
					}
					break;
				
				
				case 2: 
					switch (column)
					{
						
						case 0: 
							this.m20 = value_Renamed;
							break;
						
						case 1: 
							this.m21 = value_Renamed;
							break;
						
						case 2: 
							this.m22 = value_Renamed;
							break;
						
						default:

                            throw new Exception();//Exception();//ArrayIndexOutOfBoundsException(VecMathI18N.getString("Matrix3f0"));
						
					}
					break;
				
				
				default:
                    throw new Exception(); //Exception();//ArrayIndexOutOfBoundsException(VecMathI18N.getString("Matrix3f0"));
				
			}
		}
		
		/// <summary> Copies the matrix values in the specified row into the vector parameter. </summary>
		/// <param name="row"> the matrix row
		/// </param>
		/// <param name="v">   the vector into which the matrix row values will be copied
		/// </param>
		public void  getRow(int row, Vector3f v)
		{
			if (row == 0)
			{
				v.x = m00;
				v.y = m01;
				v.z = m02;
			}
			else if (row == 1)
			{
				v.x = m10;
				v.y = m11;
				v.z = m12;
			}
			else if (row == 2)
			{
				v.x = m20;
				v.y = m21;
				v.z = m22;
			}
			else
			{
                throw new Exception(); //Exception();//ArrayIndexOutOfBoundsException(VecMathI18N.getString("Matrix3f1"));
			}
		}
		
		/// <summary> Copies the matrix values in the specified row into the array parameter. </summary>
		/// <param name="row"> the matrix row
		/// </param>
		/// <param name="v">   the array into which the matrix row values will be copied 
		/// </param>
		public void  getRow(int row, float[] v)
		{
			if (row == 0)
			{
				v[0] = m00;
				v[1] = m01;
				v[2] = m02;
			}
			else if (row == 1)
			{
				v[0] = m10;
				v[1] = m11;
				v[2] = m12;
			}
			else if (row == 2)
			{
				v[0] = m20;
				v[1] = m21;
				v[2] = m22;
			}
			else
			{
                throw new Exception(); //Exception();//ArrayIndexOutOfBoundsException(VecMathI18N.getString("Matrix3f1"));
			}
		}
		
		/// <summary> Copies the matrix values in the specified column into the vector 
		/// parameter.
		/// </summary>
		/// <param name="column"> the matrix column
		/// </param>
		/// <param name="v">   the vector into which the matrix row values will be copied
		/// </param>
		public void  getColumn(int column, Vector3f v)
		{
			if (column == 0)
			{
				v.x = m00;
				v.y = m10;
				v.z = m20;
			}
			else if (column == 1)
			{
				v.x = m01;
				v.y = m11;
				v.z = m21;
			}
			else if (column == 2)
			{
				v.x = m02;
				v.y = m12;
				v.z = m22;
			}
			else
			{
                throw new Exception(); //Exception();//ArrayIndexOutOfBoundsException(VecMathI18N.getString("Matrix3f3"));
			}
		}
		
		/// <summary> Copies the matrix values in the specified column into the array 
		/// parameter.
		/// </summary>
		/// <param name="column"> the matrix column
		/// </param>
		/// <param name="v">   the array into which the matrix row values will be copied
		/// </param>
		public void  getColumn(int column, float[] v)
		{
			if (column == 0)
			{
				v[0] = m00;
				v[1] = m10;
				v[2] = m20;
			}
			else if (column == 1)
			{
				v[0] = m01;
				v[1] = m11;
				v[2] = m21;
			}
			else if (column == 2)
			{
				v[0] = m02;
				v[1] = m12;
				v[2] = m22;
			}
			else
			{
                throw new Exception(); //ArrayIndexOutOfBoundsException(VecMathI18N.getString("Matrix3f3"));
			}
		}
		
		/// <summary> Retrieves the value at the specified row and column of this
		/// matrix.
		/// </summary>
		/// <param name="row">the row number to be retrieved (zero indexed)
		/// </param>
		/// <param name="column">the column number to be retrieved (zero indexed)
		/// </param>
		/// <returns> the value at the indexed element.
		/// </returns>
		public float getElement(int row, int column)
		{
			switch (row)
			{
				
				case 0: 
					switch (column)
					{
						
						case 0: 
							return (this.m00);
						
						case 1: 
							return (this.m01);
						
						case 2: 
							return (this.m02);
						
						default: 
							break;
						
					}
					break;
				
				case 1: 
					switch (column)
					{
						
						case 0: 
							return (this.m10);
						
						case 1: 
							return (this.m11);
						
						case 2: 
							return (this.m12);
						
						default: 
							break;
						
					}
					break;
				
				
				case 2: 
					switch (column)
					{
						
						case 0: 
							return (this.m20);
						
						case 1: 
							return (this.m21);
						
						case 2: 
							return (this.m22);
						
						default: 
							break;
						
					}
					break;
				
				
				default: 
					break;
				
			}
			throw new Exception();//ArrayIndexOutOfBoundsException(VecMathI18N.getString("Matrix3f5"));
		}
		
		/// <summary> Sets the specified row of this matrix3f to the three values provided.</summary>
		/// <param name="row">the row number to be modified (zero indexed)
		/// </param>
		/// <param name="x">the first column element
		/// </param>
		/// <param name="y">the second column element
		/// </param>
		/// <param name="z">the third column element
		/// </param>
		public void  setRow(int row, float x, float y, float z)
		{
			switch (row)
			{
				
				case 0: 
					this.m00 = x;
					this.m01 = y;
					this.m02 = z;
					break;
				
				
				case 1: 
					this.m10 = x;
					this.m11 = y;
					this.m12 = z;
					break;
				
				
				case 2: 
					this.m20 = x;
					this.m21 = y;
					this.m22 = z;
					break;
				
				
				default: 
					throw new Exception();//ArrayIndexOutOfBoundsException(VecMathI18N.getString("Matrix3f6"));
				
			}
		}
		
		/// <summary> Sets the specified row of this matrix3f to the Vector provided.</summary>
		/// <param name="row">the row number to be modified (zero indexed)
		/// </param>
		/// <param name="v">the replacement row
		/// </param>
		public void  setRow(int row, Vector3f v)
		{
			switch (row)
			{
				
				case 0: 
					this.m00 = v.x;
					this.m01 = v.y;
					this.m02 = v.z;
					break;
				
				
				case 1: 
					this.m10 = v.x;
					this.m11 = v.y;
					this.m12 = v.z;
					break;
				
				
				case 2: 
					this.m20 = v.x;
					this.m21 = v.y;
					this.m22 = v.z;
					break;
				
				
				default: 
					throw new Exception();//ArrayIndexOutOfBoundsException(VecMathI18N.getString("Matrix3f6"));
				
			}
		}
		
		/// <summary> Sets the specified row of this matrix3f to the three values provided.</summary>
		/// <param name="row">the row number to be modified (zero indexed)
		/// </param>
		/// <param name="v">the replacement row
		/// </param>
		public void  setRow(int row, float[] v)
		{
			switch (row)
			{
				
				case 0: 
					this.m00 = v[0];
					this.m01 = v[1];
					this.m02 = v[2];
					break;
				
				
				case 1: 
					this.m10 = v[0];
					this.m11 = v[1];
					this.m12 = v[2];
					break;
				
				
				case 2: 
					this.m20 = v[0];
					this.m21 = v[1];
					this.m22 = v[2];
					break;
				
				
				default: 
					throw new Exception();//ArrayIndexOutOfBoundsException(VecMathI18N.getString("Matrix3f6"));
				
			}
		}
		
		/// <summary> Sets the specified column of this matrix3f to the three values provided.</summary>
		/// <param name="column">the column number to be modified (zero indexed)
		/// </param>
		/// <param name="x">the first row element
		/// </param>
		/// <param name="y">the second row element
		/// </param>
		/// <param name="z">the third row element
		/// </param>
		public void  setColumn(int column, float x, float y, float z)
		{
			switch (column)
			{
				
				case 0: 
					this.m00 = x;
					this.m10 = y;
					this.m20 = z;
					break;
				
				
				case 1: 
					this.m01 = x;
					this.m11 = y;
					this.m21 = z;
					break;
				
				
				case 2: 
					this.m02 = x;
					this.m12 = y;
					this.m22 = z;
					break;
				
				
				default: 
					throw new Exception();//ArrayIndexOutOfBoundsException(VecMathI18N.getString("Matrix3f9"));
				
			}
		}
		
		/// <summary> Sets the specified column of this matrix3f to the vector provided.</summary>
		/// <param name="column">the column number to be modified (zero indexed)
		/// </param>
		/// <param name="v">the replacement column
		/// </param>
		public void  setColumn(int column, Vector3f v)
		{
			switch (column)
			{
				
				case 0: 
					this.m00 = v.x;
					this.m10 = v.y;
					this.m20 = v.z;
					break;
				
				
				case 1: 
					this.m01 = v.x;
					this.m11 = v.y;
					this.m21 = v.z;
					break;
				
				
				case 2: 
					this.m02 = v.x;
					this.m12 = v.y;
					this.m22 = v.z;
					break;
				
				
				default: 
					throw new Exception();//ArrayIndexOutOfBoundsException(VecMathI18N.getString("Matrix3f9"));
				
			}
		}
		
		/// <summary> Sets the specified column of this matrix3f to the three values provided.</summary>
		/// <param name="column">the column number to be modified (zero indexed)
		/// </param>
		/// <param name="v">the replacement column
		/// </param>
		public void  setColumn(int column, float[] v)
		{
			switch (column)
			{
				
				case 0: 
					this.m00 = v[0];
					this.m10 = v[1];
					this.m20 = v[2];
					break;
				
				
				case 1: 
					this.m01 = v[0];
					this.m11 = v[1];
					this.m21 = v[2];
					break;
				
				
				case 2: 
					this.m02 = v[0];
					this.m12 = v[1];
					this.m22 = v[2];
					break;
				
				
				default: 
					throw new Exception();//ArrayIndexOutOfBoundsException(VecMathI18N.getString("Matrix3f9"));
				
			}
		}
		
		/// <summary>  Adds a scalar to each component of this matrix.</summary>
		/// <param name="scalar"> the scalar adder
		/// </param>
		public void  add(float scalar)
		{
			m00 += scalar;
			m01 += scalar;
			m02 += scalar;
			m10 += scalar;
			m11 += scalar;
			m12 += scalar;
			m20 += scalar;
			m21 += scalar;
			m22 += scalar;
		}
		
		/// <summary>  Adds a scalar to each component of the matrix m1 and places
		/// the result into this.  Matrix m1 is not modified.
		/// </summary>
		/// <param name="scalar"> the scalar adder.
		/// </param>
		/// <param name="m1"> the original matrix values
		/// </param>
		public void  add(float scalar, Matrix3f m1)
		{
			this.m00 = m1.m00 + scalar;
			this.m01 = m1.m01 + scalar;
			this.m02 = m1.m02 + scalar;
			this.m10 = m1.m10 + scalar;
			this.m11 = m1.m11 + scalar;
			this.m12 = m1.m12 + scalar;
			this.m20 = m1.m20 + scalar;
			this.m21 = m1.m21 + scalar;
			this.m22 = m1.m22 + scalar;
		}
		
		/// <summary> Sets the value of this matrix to the matrix sum of matrices m1 and m2.</summary>
		/// <param name="m1">the first matrix
		/// </param>
		/// <param name="m2">the second matrix
		/// </param>
		public void  add(Matrix3f m1, Matrix3f m2)
		{
			this.m00 = m1.m00 + m2.m00;
			this.m01 = m1.m01 + m2.m01;
			this.m02 = m1.m02 + m2.m02;
			
			this.m10 = m1.m10 + m2.m10;
			this.m11 = m1.m11 + m2.m11;
			this.m12 = m1.m12 + m2.m12;
			
			this.m20 = m1.m20 + m2.m20;
			this.m21 = m1.m21 + m2.m21;
			this.m22 = m1.m22 + m2.m22;
		}
		
		/// <summary> Sets the value of this matrix to the matrix sum of itself and 
		/// matrix m1.
		/// </summary>
		/// <param name="m1">the other matrix
		/// </param>
		public void  add(Matrix3f m1)
		{
			this.m00 += m1.m00;
			this.m01 += m1.m01;
			this.m02 += m1.m02;
			
			this.m10 += m1.m10;
			this.m11 += m1.m11;
			this.m12 += m1.m12;
			
			this.m20 += m1.m20;
			this.m21 += m1.m21;
			this.m22 += m1.m22;
		}
		
		/// <summary> Sets the value of this matrix to the matrix difference
		/// of matrices m1 and m2.
		/// </summary>
		/// <param name="m1">the first matrix
		/// </param>
		/// <param name="m2">the second matrix
		/// </param>
		public void  sub(Matrix3f m1, Matrix3f m2)
		{
			this.m00 = m1.m00 - m2.m00;
			this.m01 = m1.m01 - m2.m01;
			this.m02 = m1.m02 - m2.m02;
			
			this.m10 = m1.m10 - m2.m10;
			this.m11 = m1.m11 - m2.m11;
			this.m12 = m1.m12 - m2.m12;
			
			this.m20 = m1.m20 - m2.m20;
			this.m21 = m1.m21 - m2.m21;
			this.m22 = m1.m22 - m2.m22;
		}
		
		/// <summary> Sets the value of this matrix to the matrix difference
		/// of itself and matrix m1 (this = this - m1).
		/// </summary>
		/// <param name="m1">the other matrix
		/// </param>
		public void  sub(Matrix3f m1)
		{
			this.m00 -= m1.m00;
			this.m01 -= m1.m01;
			this.m02 -= m1.m02;
			
			this.m10 -= m1.m10;
			this.m11 -= m1.m11;
			this.m12 -= m1.m12;
			
			this.m20 -= m1.m20;
			this.m21 -= m1.m21;
			this.m22 -= m1.m22;
		}
		
		/// <summary> Sets the value of this matrix to its transpose.</summary>
		public void  transpose()
		{
			float temp;
			
			temp = this.m10;
			this.m10 = this.m01;
			this.m01 = temp;
			
			temp = this.m20;
			this.m20 = this.m02;
			this.m02 = temp;
			
			temp = this.m21;
			this.m21 = this.m12;
			this.m12 = temp;
		}
		
		/// <summary> Sets the value of this matrix to the transpose of the argument matrix.</summary>
		/// <param name="m1">the matrix to be transposed
		/// </param>
		public void  transpose(Matrix3f m1)
		{
			if (this != m1)
			{
				this.m00 = m1.m00;
				this.m01 = m1.m10;
				this.m02 = m1.m20;
				
				this.m10 = m1.m01;
				this.m11 = m1.m11;
				this.m12 = m1.m21;
				
				this.m20 = m1.m02;
				this.m21 = m1.m12;
				this.m22 = m1.m22;
			}
			else
				this.transpose();
		}
		
		/// <summary> Sets the value of this matrix to the matrix conversion of the
		/// (single precision) quaternion argument.
		/// </summary>
		/// <param name="q1">the quaternion to be converted
		/// </param>
		public void  set_Renamed(Quat4f q1)
		{
			this.m00 = 1.0f - 2.0f * q1.y * q1.y - 2.0f * q1.z * q1.z;
			this.m10 = 2.0f * (q1.x * q1.y + q1.w * q1.z);
			this.m20 = 2.0f * (q1.x * q1.z - q1.w * q1.y);
			
			this.m01 = 2.0f * (q1.x * q1.y - q1.w * q1.z);
			this.m11 = 1.0f - 2.0f * q1.x * q1.x - 2.0f * q1.z * q1.z;
			this.m21 = 2.0f * (q1.y * q1.z + q1.w * q1.x);
			
			this.m02 = 2.0f * (q1.x * q1.z + q1.w * q1.y);
			this.m12 = 2.0f * (q1.y * q1.z - q1.w * q1.x);
			this.m22 = 1.0f - 2.0f * q1.x * q1.x - 2.0f * q1.y * q1.y;
		}
		
		/// <summary> Sets the value of this matrix to the matrix conversion of the
		/// (single precision) axis and angle argument.
		/// </summary>
		/// <param name="a1">the axis and angle to be converted
		/// </param>
		public void  set_Renamed(AxisAngle4f a1)
		{
			float mag = (float) Math.Sqrt(a1.x * a1.x + a1.y * a1.y + a1.z * a1.z);
			if (mag < EPS)
			{
				m00 = 1.0f;
				m01 = 0.0f;
				m02 = 0.0f;
				
				m10 = 0.0f;
				m11 = 1.0f;
				m12 = 0.0f;
				
				m20 = 0.0f;
				m21 = 0.0f;
				m22 = 1.0f;
			}
			else
			{
				mag = 1.0f / mag;
				float ax = a1.x * mag;
				float ay = a1.y * mag;
				float az = a1.z * mag;
				
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				float sinTheta = (float) System.Math.Sin((float) a1.angle);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				float cosTheta = (float) System.Math.Cos((float) a1.angle);
				float t = (float) 1.0 - cosTheta;
				
				float xz = ax * az;
				float xy = ax * ay;
				float yz = ay * az;
				
				m00 = t * ax * ax + cosTheta;
				m01 = t * xy - sinTheta * az;
				m02 = t * xz + sinTheta * ay;
				
				m10 = t * xy + sinTheta * az;
				m11 = t * ay * ay + cosTheta;
				m12 = t * yz - sinTheta * ax;
				
				m20 = t * xz - sinTheta * ay;
				m21 = t * yz + sinTheta * ax;
				m22 = t * az * az + cosTheta;
			}
		}
		
		/// <summary> Sets the value of this matrix to the matrix conversion of the
		/// (double precision) axis and angle argument.
		/// </summary>
		/// <param name="a1">the axis and angle to be converted
		/// </param>
		public void  set_Renamed(AxisAngle4d a1)
		{
			double mag = Math.Sqrt(a1.x * a1.x + a1.y * a1.y + a1.z * a1.z);
			if (mag < EPS)
			{
				m00 = 1.0f;
				m01 = 0.0f;
				m02 = 0.0f;
				
				m10 = 0.0f;
				m11 = 1.0f;
				m12 = 0.0f;
				
				m20 = 0.0f;
				m21 = 0.0f;
				m22 = 1.0f;
			}
			else
			{
				mag = 1.0 / mag;
				double ax = a1.x * mag;
				double ay = a1.y * mag;
				double az = a1.z * mag;
				
				double sinTheta = Math.Sin(a1.angle);
				double cosTheta = Math.Cos(a1.angle);
				double t = 1.0 - cosTheta;
				
				double xz = ax * az;
				double xy = ax * ay;
				double yz = ay * az;
				
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				m00 = (float) (t * ax * ax + cosTheta);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				m01 = (float) (t * xy - sinTheta * az);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				m02 = (float) (t * xz + sinTheta * ay);
				
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				m10 = (float) (t * xy + sinTheta * az);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				m11 = (float) (t * ay * ay + cosTheta);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				m12 = (float) (t * yz - sinTheta * ax);
				
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				m20 = (float) (t * xz - sinTheta * ay);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				m21 = (float) (t * yz + sinTheta * ax);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				m22 = (float) (t * az * az + cosTheta);
			}
		}
		
		/// <summary> Sets the value of this matrix to the matrix conversion of the
		/// (single precision) quaternion argument.
		/// </summary>
		/// <param name="q1">the quaternion to be converted
		/// </param>
		public void  set_Renamed(Quat4d q1)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m00 = (float) (1.0 - 2.0 * q1.y * q1.y - 2.0 * q1.z * q1.z);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m10 = (float) (2.0 * (q1.x * q1.y + q1.w * q1.z));
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m20 = (float) (2.0 * (q1.x * q1.z - q1.w * q1.y));
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m01 = (float) (2.0 * (q1.x * q1.y - q1.w * q1.z));
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m11 = (float) (1.0 - 2.0 * q1.x * q1.x - 2.0 * q1.z * q1.z);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m21 = (float) (2.0 * (q1.y * q1.z + q1.w * q1.x));
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m02 = (float) (2.0 * (q1.x * q1.z + q1.w * q1.y));
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m12 = (float) (2.0 * (q1.y * q1.z - q1.w * q1.x));
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m22 = (float) (1.0 - 2.0 * q1.x * q1.x - 2.0 * q1.y * q1.y);
		}
		
		/// <summary>  Sets the values in this Matrix3f equal to the row-major 
		/// array parameter (ie, the first three elements of the 
		/// array will be copied into the first row of this matrix, etc.). 
		/// </summary>
		/// <param name="m"> the single precision array of length 9 
		/// </param>
		public void  set_Renamed(float[] m)
		{
			m00 = m[0];
			m01 = m[1];
			m02 = m[2];
			
			m10 = m[3];
			m11 = m[4];
			m12 = m[5];
			
			m20 = m[6];
			m21 = m[7];
			m22 = m[8];
		}
		
		/// <summary> Sets the value of this matrix to the value of the Matrix3f 
		/// argument. 
		/// </summary>
		/// <param name="m1">the source matrix3f 
		/// </param>
		public void  set_Renamed(Matrix3f m1)
		{
			
			this.m00 = m1.m00;
			this.m01 = m1.m01;
			this.m02 = m1.m02;
			
			this.m10 = m1.m10;
			this.m11 = m1.m11;
			this.m12 = m1.m12;
			
			this.m20 = m1.m20;
			this.m21 = m1.m21;
			this.m22 = m1.m22;
		}
		
		
		/// <summary> Sets the value of this matrix to the float value of the Matrix3d 
		/// argument. 
		/// </summary>
		/// <param name="m1">the source matrix3d 
		/// </param>
		public void  set_Renamed(Matrix3d m1)
		{
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m00 = (float) m1.m00;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m01 = (float) m1.m01;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m02 = (float) m1.m02;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m10 = (float) m1.m10;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m11 = (float) m1.m11;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m12 = (float) m1.m12;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m20 = (float) m1.m20;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m21 = (float) m1.m21;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m22 = (float) m1.m22;
		}
		
		
		/// <summary> Sets the value of this matrix to the matrix inverse
		/// of the passed matrix m1.
		/// </summary>
		/// <param name="m1">the matrix to be inverted
		/// </param>
		public void  invert(Matrix3f m1)
		{
			invertGeneral(m1);
		}
		
		/// <summary> Inverts this matrix in place.</summary>
		public void  invert()
		{
			invertGeneral(this);
		}
		
		/// <summary> General invert routine.  Inverts m1 and places the result in "this".
		/// Note that this routine handles both the "this" version and the
		/// non-"this" version.
		/// 
		/// Also note that since this routine is slow anyway, we won't worry
		/// about allocating a little bit of garbage.
		/// </summary>
		private void  invertGeneral(Matrix3f m1)
		{
			double[] temp = new double[9];
			double[] result = new double[9];
			int[] row_perm = new int[3];
			int i, r, c;
			
			// Use LU decomposition and backsubstitution code specifically
			// for floating-point 3x3 matrices.
			
			// Copy source matrix to t1tmp 
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			temp[0] = (double) m1.m00;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			temp[1] = (double) m1.m01;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			temp[2] = (double) m1.m02;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			temp[3] = (double) m1.m10;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			temp[4] = (double) m1.m11;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			temp[5] = (double) m1.m12;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			temp[6] = (double) m1.m20;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			temp[7] = (double) m1.m21;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			temp[8] = (double) m1.m22;
			
			
			// Calculate LU decomposition: Is the matrix singular? 
			if (!luDecomposition(temp, row_perm))
			{
				// Matrix has no inverse 
                throw new Exception();//SingularMatrixException(VecMathI18N.getString("Matrix3f12"));
			}
			
			// Perform back substitution on the identity matrix 
			for (i = 0; i < 9; i++)
				result[i] = 0.0;
			result[0] = 1.0; result[4] = 1.0; result[8] = 1.0;
			luBacksubstitution(temp, row_perm, result);
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m00 = (float) result[0];
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m01 = (float) result[1];
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m02 = (float) result[2];
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m10 = (float) result[3];
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m11 = (float) result[4];
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m12 = (float) result[5];
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m20 = (float) result[6];
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m21 = (float) result[7];
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m22 = (float) result[8];
		}
		
		/// <summary> Given a 3x3 array "matrix0", this function replaces it with the 
		/// LU decomposition of a row-wise permutation of itself.  The input 
		/// parameters are "matrix0" and "dimen".  The array "matrix0" is also 
		/// an output parameter.  The vector "row_perm[3]" is an output 
		/// parameter that contains the row permutations resulting from partial 
		/// pivoting.  The output parameter "even_row_xchg" is 1 when the 
		/// number of row exchanges is even, or -1 otherwise.  Assumes data 
		/// type is always double.
		/// 
		/// This function is similar to luDecomposition, except that it
		/// is tuned specifically for 3x3 matrices.
		/// 
		/// </summary>
		/// <returns> true if the matrix is nonsingular, or false otherwise.
		/// </returns>
		//
		// Reference: Press, Flannery, Teukolsky, Vetterling, 
		//	      _Numerical_Recipes_in_C_, Cambridge University Press, 
		//	      1988, pp 40-45.
		//
		internal static bool luDecomposition(double[] matrix0, int[] row_perm)
		{
			
			double[] row_scale = new double[3];
			
			// Determine implicit scaling information by looping over rows 
			{
				int i, j;
				int ptr, rs;
				double big, temp;
				
				ptr = 0;
				rs = 0;
				
				// For each row ... 
				i = 3;
				while (i-- != 0)
				{
					big = 0.0;
					
					// For each column, find the largest element in the row 
					j = 3;
					while (j-- != 0)
					{
						temp = matrix0[ptr++];
						temp = System.Math.Abs(temp);
						if (temp > big)
						{
							big = temp;
						}
					}
					
					// Is the matrix singular? 
					if (big == 0.0)
					{
						return false;
					}
					row_scale[rs++] = 1.0 / big;
				}
			}
			
			{
				int j;
				int mtx;
				
				mtx = 0;
				
				// For all columns, execute Crout's method 
				for (j = 0; j < 3; j++)
				{
					int i, imax, k;
					int target, p1, p2;
					double sum, big, temp;
					
					// Determine elements of upper diagonal matrix U 
					for (i = 0; i < j; i++)
					{
						target = mtx + (3 * i) + j;
						sum = matrix0[target];
						k = i;
						p1 = mtx + (3 * i);
						p2 = mtx + j;
						while (k-- != 0)
						{
							sum -= matrix0[p1] * matrix0[p2];
							p1++;
							p2 += 3;
						}
						matrix0[target] = sum;
					}
					
					// Search for largest pivot element and calculate
					// intermediate elements of lower diagonal matrix L.
					big = 0.0;
					imax = - 1;
					for (i = j; i < 3; i++)
					{
						target = mtx + (3 * i) + j;
						sum = matrix0[target];
						k = j;
						p1 = mtx + (3 * i);
						p2 = mtx + j;
						while (k-- != 0)
						{
							sum -= matrix0[p1] * matrix0[p2];
							p1++;
							p2 += 3;
						}
						matrix0[target] = sum;
						
						// Is this the best pivot so far? 
						if ((temp = row_scale[i] * System.Math.Abs(sum)) >= big)
						{
							big = temp;
							imax = i;
						}
					}
					
					if (imax < 0)
					{
                        throw new Exception();//RuntimeException(VecMathI18N.getString("Matrix3f13"));
					}
					
					// Is a row exchange necessary? 
					if (j != imax)
					{
						// Yes: exchange rows 
						k = 3;
						p1 = mtx + (3 * imax);
						p2 = mtx + (3 * j);
						while (k-- != 0)
						{
							temp = matrix0[p1];
							matrix0[p1++] = matrix0[p2];
							matrix0[p2++] = temp;
						}
						
						// Record change in scale factor 
						row_scale[imax] = row_scale[j];
					}
					
					// Record row permutation 
					row_perm[j] = imax;
					
					// Is the matrix singular 
					if (matrix0[(mtx + (3 * j) + j)] == 0.0)
					{
						return false;
					}
					
					// Divide elements of lower diagonal matrix L by pivot 
					if (j != (3 - 1))
					{
						temp = 1.0 / (matrix0[(mtx + (3 * j) + j)]);
						target = mtx + (3 * (j + 1)) + j;
						i = 2 - j;
						while (i-- != 0)
						{
							matrix0[target] *= temp;
							target += 3;
						}
					}
				}
			}
			
			return true;
		}
		
		/// <summary> Solves a set of linear equations.  The input parameters "matrix1",
		/// and "row_perm" come from luDecompostionD3x3 and do not change
		/// here.  The parameter "matrix2" is a set of column vectors assembled
		/// into a 3x3 matrix of floating-point values.  The procedure takes each
		/// column of "matrix2" in turn and treats it as the right-hand side of the
		/// matrix equation Ax = LUx = b.  The solution vector replaces the
		/// original column of the matrix.
		/// 
		/// If "matrix2" is the identity matrix, the procedure replaces its contents
		/// with the inverse of the matrix from which "matrix1" was originally
		/// derived.
		/// </summary>
		//
		// Reference: Press, Flannery, Teukolsky, Vetterling, 
		//	      _Numerical_Recipes_in_C_, Cambridge University Press, 
		//	      1988, pp 44-45.
		//
		internal static void  luBacksubstitution(double[] matrix1, int[] row_perm, double[] matrix2)
		{
			
			int i, ii, ip, j, k;
			int rp;
			int cv, rv;
			
			//	rp = row_perm;
			rp = 0;
			
			// For each column vector of matrix2 ... 
			for (k = 0; k < 3; k++)
			{
				//	    cv = &(matrix2[0][k]);
				cv = k;
				ii = - 1;
				
				// Forward substitution 
				for (i = 0; i < 3; i++)
				{
					double sum;
					
					ip = row_perm[rp + i];
					sum = matrix2[cv + 3 * ip];
					matrix2[cv + 3 * ip] = matrix2[cv + 3 * i];
					if (ii >= 0)
					{
						//		    rv = &(matrix1[i][0]);
						rv = i * 3;
						for (j = ii; j <= i - 1; j++)
						{
							sum -= matrix1[rv + j] * matrix2[cv + 3 * j];
						}
					}
					else if (sum != 0.0)
					{
						ii = i;
					}
					matrix2[cv + 3 * i] = sum;
				}
				
				// Backsubstitution 
				//	    rv = &(matrix1[3][0]);
				rv = 2 * 3;
				matrix2[cv + 3 * 2] /= matrix1[rv + 2];
				
				rv -= 3;
				matrix2[cv + 3 * 1] = (matrix2[cv + 3 * 1] - matrix1[rv + 2] * matrix2[cv + 3 * 2]) / matrix1[rv + 1];
				
				rv -= 3;
				matrix2[cv + 4 * 0] = (matrix2[cv + 3 * 0] - matrix1[rv + 1] * matrix2[cv + 3 * 1] - matrix1[rv + 2] * matrix2[cv + 3 * 2]) / matrix1[rv + 0];
			}
		}
		/// <summary> Computes the determinant of this matrix.</summary>
		/// <returns> the determinant of this matrix
		/// </returns>
		public float determinant()
		{
			float total;
			total = this.m00 * (this.m11 * this.m22 - this.m12 * this.m21) + this.m01 * (this.m12 * this.m20 - this.m10 * this.m22) + this.m02 * (this.m10 * this.m21 - this.m11 * this.m20);
			return total;
		}
		
		/// <summary> Sets the value of this matrix to a scale matrix with
		/// the passed scale amount.
		/// </summary>
		/// <param name="scale">the scale factor for the matrix
		/// </param>
		public void  set_Renamed(float scale)
		{
			this.m00 = scale;
			this.m01 = (float) 0.0;
			this.m02 = (float) 0.0;
			
			this.m10 = (float) 0.0;
			this.m11 = scale;
			this.m12 = (float) 0.0;
			
			this.m20 = (float) 0.0;
			this.m21 = (float) 0.0;
			this.m22 = scale;
		}
		
		/// <summary> Sets the value of this matrix to a counter clockwise rotation 
		/// about the x axis.
		/// </summary>
		/// <param name="angle">the angle to rotate about the X axis in radians
		/// </param>
		public void  rotX(float angle)
		{
			float sinAngle, cosAngle;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			sinAngle = (float) System.Math.Sin((double) angle);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			cosAngle = (float) System.Math.Cos((double) angle);
			
			this.m00 = (float) 1.0;
			this.m01 = (float) 0.0;
			this.m02 = (float) 0.0;
			
			this.m10 = (float) 0.0;
			this.m11 = cosAngle;
			this.m12 = - sinAngle;
			
			this.m20 = (float) 0.0;
			this.m21 = sinAngle;
			this.m22 = cosAngle;
		}
		
		/// <summary> Sets the value of this matrix to a counter clockwise rotation 
		/// about the y axis.
		/// </summary>
		/// <param name="angle">the angle to rotate about the Y axis in radians
		/// </param>
		public void  rotY(float angle)
		{
			float sinAngle, cosAngle;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			sinAngle = (float) System.Math.Sin((double) angle);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			cosAngle = (float) System.Math.Cos((double) angle);
			
			this.m00 = cosAngle;
			this.m01 = (float) 0.0;
			this.m02 = sinAngle;
			
			this.m10 = (float) 0.0;
			this.m11 = (float) 1.0;
			this.m12 = (float) 0.0;
			
			this.m20 = - sinAngle;
			this.m21 = (float) 0.0;
			this.m22 = cosAngle;
		}
		
		/// <summary> Sets the value of this matrix to a counter clockwise rotation 
		/// about the z axis.
		/// </summary>
		/// <param name="angle">the angle to rotate about the Z axis in radians
		/// </param>
		public void  rotZ(float angle)
		{
			float sinAngle, cosAngle;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			sinAngle = (float) System.Math.Sin((double) angle);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			cosAngle = (float) System.Math.Cos((double) angle);
			
			this.m00 = cosAngle;
			this.m01 = - sinAngle;
			this.m02 = (float) 0.0;
			
			this.m10 = sinAngle;
			this.m11 = cosAngle;
			this.m12 = (float) 0.0;
			
			this.m20 = (float) 0.0;
			this.m21 = (float) 0.0;
			this.m22 = (float) 1.0;
		}
		
		/// <summary> Multiplies each element of this matrix by a scalar.</summary>
		/// <param name="scalar"> the scalar multiplier
		/// </param>
		public void  mul(float scalar)
		{
			m00 *= scalar;
			m01 *= scalar;
			m02 *= scalar;
			
			m10 *= scalar;
			m11 *= scalar;
			m12 *= scalar;
			
			m20 *= scalar;
			m21 *= scalar;
			m22 *= scalar;
		}
		
		/// <summary> Multiplies each element of matrix m1 by a scalar and places
		/// the result into this.  Matrix m1 is not modified.
		/// </summary>
		/// <param name="scalar"> the scalar multiplier
		/// </param>
		/// <param name="m1"> the original matrix
		/// </param>
		public void  mul(float scalar, Matrix3f m1)
		{
			this.m00 = scalar * m1.m00;
			this.m01 = scalar * m1.m01;
			this.m02 = scalar * m1.m02;
			
			this.m10 = scalar * m1.m10;
			this.m11 = scalar * m1.m11;
			this.m12 = scalar * m1.m12;
			
			this.m20 = scalar * m1.m20;
			this.m21 = scalar * m1.m21;
			this.m22 = scalar * m1.m22;
		}
		
		/// <summary> Sets the value of this matrix to the result of multiplying itself
		/// with matrix m1.
		/// </summary>
		/// <param name="m1">the other matrix
		/// </param>
		public void  mul(Matrix3f m1)
		{
			float m00, m01, m02, m10, m11, m12, m20, m21, m22;
			
			m00 = this.m00 * m1.m00 + this.m01 * m1.m10 + this.m02 * m1.m20;
			m01 = this.m00 * m1.m01 + this.m01 * m1.m11 + this.m02 * m1.m21;
			m02 = this.m00 * m1.m02 + this.m01 * m1.m12 + this.m02 * m1.m22;
			
			m10 = this.m10 * m1.m00 + this.m11 * m1.m10 + this.m12 * m1.m20;
			m11 = this.m10 * m1.m01 + this.m11 * m1.m11 + this.m12 * m1.m21;
			m12 = this.m10 * m1.m02 + this.m11 * m1.m12 + this.m12 * m1.m22;
			
			m20 = this.m20 * m1.m00 + this.m21 * m1.m10 + this.m22 * m1.m20;
			m21 = this.m20 * m1.m01 + this.m21 * m1.m11 + this.m22 * m1.m21;
			m22 = this.m20 * m1.m02 + this.m21 * m1.m12 + this.m22 * m1.m22;
			
			this.m00 = m00; this.m01 = m01; this.m02 = m02;
			this.m10 = m10; this.m11 = m11; this.m12 = m12;
			this.m20 = m20; this.m21 = m21; this.m22 = m22;
		}
		
		/// <summary> Sets the value of this matrix to the result of multiplying
		/// the two argument matrices together.
		/// </summary>
		/// <param name="m1">the first matrix
		/// </param>
		/// <param name="m2">the second matrix
		/// </param>
		public void  mul(Matrix3f m1, Matrix3f m2)
		{
			if (this != m1 && this != m2)
			{
				this.m00 = m1.m00 * m2.m00 + m1.m01 * m2.m10 + m1.m02 * m2.m20;
				this.m01 = m1.m00 * m2.m01 + m1.m01 * m2.m11 + m1.m02 * m2.m21;
				this.m02 = m1.m00 * m2.m02 + m1.m01 * m2.m12 + m1.m02 * m2.m22;
				
				this.m10 = m1.m10 * m2.m00 + m1.m11 * m2.m10 + m1.m12 * m2.m20;
				this.m11 = m1.m10 * m2.m01 + m1.m11 * m2.m11 + m1.m12 * m2.m21;
				this.m12 = m1.m10 * m2.m02 + m1.m11 * m2.m12 + m1.m12 * m2.m22;
				
				this.m20 = m1.m20 * m2.m00 + m1.m21 * m2.m10 + m1.m22 * m2.m20;
				this.m21 = m1.m20 * m2.m01 + m1.m21 * m2.m11 + m1.m22 * m2.m21;
				this.m22 = m1.m20 * m2.m02 + m1.m21 * m2.m12 + m1.m22 * m2.m22;
			}
			else
			{
				float m00, m01, m02, m10, m11, m12, m20, m21, m22;
				
				m00 = m1.m00 * m2.m00 + m1.m01 * m2.m10 + m1.m02 * m2.m20;
				m01 = m1.m00 * m2.m01 + m1.m01 * m2.m11 + m1.m02 * m2.m21;
				m02 = m1.m00 * m2.m02 + m1.m01 * m2.m12 + m1.m02 * m2.m22;
				
				m10 = m1.m10 * m2.m00 + m1.m11 * m2.m10 + m1.m12 * m2.m20;
				m11 = m1.m10 * m2.m01 + m1.m11 * m2.m11 + m1.m12 * m2.m21;
				m12 = m1.m10 * m2.m02 + m1.m11 * m2.m12 + m1.m12 * m2.m22;
				
				m20 = m1.m20 * m2.m00 + m1.m21 * m2.m10 + m1.m22 * m2.m20;
				m21 = m1.m20 * m2.m01 + m1.m21 * m2.m11 + m1.m22 * m2.m21;
				m22 = m1.m20 * m2.m02 + m1.m21 * m2.m12 + m1.m22 * m2.m22;
				
				this.m00 = m00; this.m01 = m01; this.m02 = m02;
				this.m10 = m10; this.m11 = m11; this.m12 = m12;
				this.m20 = m20; this.m21 = m21; this.m22 = m22;
			}
		}
		
		/// <summary>  Multiplies this matrix by matrix m1, does an SVD normalization 
		/// of the result, and places the result back into this matrix.
		/// this = SVDnorm(this*m1).
		/// </summary>
		/// <param name="m1">the matrix on the right hand side of the multiplication
		/// </param>
		public void  mulNormalize(Matrix3f m1)
		{
			
			double[] tmp = new double[9]; // scratch matrix
			double[] tmp_rot = new double[9]; // scratch matrix
			double[] tmp_scale = new double[3]; // scratch matrix
			
			tmp[0] = this.m00 * m1.m00 + this.m01 * m1.m10 + this.m02 * m1.m20;
			tmp[1] = this.m00 * m1.m01 + this.m01 * m1.m11 + this.m02 * m1.m21;
			tmp[2] = this.m00 * m1.m02 + this.m01 * m1.m12 + this.m02 * m1.m22;
			
			tmp[3] = this.m10 * m1.m00 + this.m11 * m1.m10 + this.m12 * m1.m20;
			tmp[4] = this.m10 * m1.m01 + this.m11 * m1.m11 + this.m12 * m1.m21;
			tmp[5] = this.m10 * m1.m02 + this.m11 * m1.m12 + this.m12 * m1.m22;
			
			tmp[6] = this.m20 * m1.m00 + this.m21 * m1.m10 + this.m22 * m1.m20;
			tmp[7] = this.m20 * m1.m01 + this.m21 * m1.m11 + this.m22 * m1.m21;
			tmp[8] = this.m20 * m1.m02 + this.m21 * m1.m12 + this.m22 * m1.m22;
			
			Matrix3d.compute_svd(tmp, tmp_scale, tmp_rot);
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m00 = (float) (tmp_rot[0]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m01 = (float) (tmp_rot[1]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m02 = (float) (tmp_rot[2]);
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m10 = (float) (tmp_rot[3]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m11 = (float) (tmp_rot[4]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m12 = (float) (tmp_rot[5]);
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m20 = (float) (tmp_rot[6]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m21 = (float) (tmp_rot[7]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m22 = (float) (tmp_rot[8]);
		}
		
		/// <summary>  Multiplies matrix m1 by matrix m2, does an SVD normalization 
		/// of the result, and places the result into this matrix.
		/// this = SVDnorm(m1*m2).
		/// </summary>
		/// <param name="m1"> the matrix on the left hand side of the multiplication
		/// </param>
		/// <param name="m2"> the matrix on the right hand side of the multiplication
		/// </param>
		public void  mulNormalize(Matrix3f m1, Matrix3f m2)
		{
			
			double[] tmp = new double[9]; // scratch matrix
			double[] tmp_rot = new double[9]; // scratch matrix
			double[] tmp_scale = new double[3]; // scratch matrix
			
			
			tmp[0] = m1.m00 * m2.m00 + m1.m01 * m2.m10 + m1.m02 * m2.m20;
			tmp[1] = m1.m00 * m2.m01 + m1.m01 * m2.m11 + m1.m02 * m2.m21;
			tmp[2] = m1.m00 * m2.m02 + m1.m01 * m2.m12 + m1.m02 * m2.m22;
			
			tmp[3] = m1.m10 * m2.m00 + m1.m11 * m2.m10 + m1.m12 * m2.m20;
			tmp[4] = m1.m10 * m2.m01 + m1.m11 * m2.m11 + m1.m12 * m2.m21;
			tmp[5] = m1.m10 * m2.m02 + m1.m11 * m2.m12 + m1.m12 * m2.m22;
			
			tmp[6] = m1.m20 * m2.m00 + m1.m21 * m2.m10 + m1.m22 * m2.m20;
			tmp[7] = m1.m20 * m2.m01 + m1.m21 * m2.m11 + m1.m22 * m2.m21;
			tmp[8] = m1.m20 * m2.m02 + m1.m21 * m2.m12 + m1.m22 * m2.m22;
			
			Matrix3d.compute_svd(tmp, tmp_scale, tmp_rot);
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m00 = (float) (tmp_rot[0]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m01 = (float) (tmp_rot[1]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m02 = (float) (tmp_rot[2]);
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m10 = (float) (tmp_rot[3]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m11 = (float) (tmp_rot[4]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m12 = (float) (tmp_rot[5]);
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m20 = (float) (tmp_rot[6]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m21 = (float) (tmp_rot[7]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m22 = (float) (tmp_rot[8]);
		}
		
		/// <summary>  Multiplies the transpose of matrix m1 times the transpose of matrix
		/// m2, and places the result into this.
		/// </summary>
		/// <param name="m1"> the matrix on the left hand side of the multiplication
		/// </param>
		/// <param name="m2"> the matrix on the right hand side of the multiplication
		/// </param>
		public void  mulTransposeBoth(Matrix3f m1, Matrix3f m2)
		{
			if (this != m1 && this != m2)
			{
				this.m00 = m1.m00 * m2.m00 + m1.m10 * m2.m01 + m1.m20 * m2.m02;
				this.m01 = m1.m00 * m2.m10 + m1.m10 * m2.m11 + m1.m20 * m2.m12;
				this.m02 = m1.m00 * m2.m20 + m1.m10 * m2.m21 + m1.m20 * m2.m22;
				
				this.m10 = m1.m01 * m2.m00 + m1.m11 * m2.m01 + m1.m21 * m2.m02;
				this.m11 = m1.m01 * m2.m10 + m1.m11 * m2.m11 + m1.m21 * m2.m12;
				this.m12 = m1.m01 * m2.m20 + m1.m11 * m2.m21 + m1.m21 * m2.m22;
				
				this.m20 = m1.m02 * m2.m00 + m1.m12 * m2.m01 + m1.m22 * m2.m02;
				this.m21 = m1.m02 * m2.m10 + m1.m12 * m2.m11 + m1.m22 * m2.m12;
				this.m22 = m1.m02 * m2.m20 + m1.m12 * m2.m21 + m1.m22 * m2.m22;
			}
			else
			{
				float m00, m01, m02, m10, m11, m12, m20, m21, m22; // vars for temp result matrix 
				
				m00 = m1.m00 * m2.m00 + m1.m10 * m2.m01 + m1.m20 * m2.m02;
				m01 = m1.m00 * m2.m10 + m1.m10 * m2.m11 + m1.m20 * m2.m12;
				m02 = m1.m00 * m2.m20 + m1.m10 * m2.m21 + m1.m20 * m2.m22;
				
				m10 = m1.m01 * m2.m00 + m1.m11 * m2.m01 + m1.m21 * m2.m02;
				m11 = m1.m01 * m2.m10 + m1.m11 * m2.m11 + m1.m21 * m2.m12;
				m12 = m1.m01 * m2.m20 + m1.m11 * m2.m21 + m1.m21 * m2.m22;
				
				m20 = m1.m02 * m2.m00 + m1.m12 * m2.m01 + m1.m22 * m2.m02;
				m21 = m1.m02 * m2.m10 + m1.m12 * m2.m11 + m1.m22 * m2.m12;
				m22 = m1.m02 * m2.m20 + m1.m12 * m2.m21 + m1.m22 * m2.m22;
				
				this.m00 = m00; this.m01 = m01; this.m02 = m02;
				this.m10 = m10; this.m11 = m11; this.m12 = m12;
				this.m20 = m20; this.m21 = m21; this.m22 = m22;
			}
		}
		
		
		/// <summary>  Multiplies matrix m1 times the transpose of matrix m2, and
		/// places the result into this.
		/// </summary>
		/// <param name="m1"> the matrix on the left hand side of the multiplication 
		/// </param>
		/// <param name="m2"> the matrix on the right hand side of the multiplication
		/// </param>
		public void  mulTransposeRight(Matrix3f m1, Matrix3f m2)
		{
			if (this != m1 && this != m2)
			{
				this.m00 = m1.m00 * m2.m00 + m1.m01 * m2.m01 + m1.m02 * m2.m02;
				this.m01 = m1.m00 * m2.m10 + m1.m01 * m2.m11 + m1.m02 * m2.m12;
				this.m02 = m1.m00 * m2.m20 + m1.m01 * m2.m21 + m1.m02 * m2.m22;
				
				this.m10 = m1.m10 * m2.m00 + m1.m11 * m2.m01 + m1.m12 * m2.m02;
				this.m11 = m1.m10 * m2.m10 + m1.m11 * m2.m11 + m1.m12 * m2.m12;
				this.m12 = m1.m10 * m2.m20 + m1.m11 * m2.m21 + m1.m12 * m2.m22;
				
				this.m20 = m1.m20 * m2.m00 + m1.m21 * m2.m01 + m1.m22 * m2.m02;
				this.m21 = m1.m20 * m2.m10 + m1.m21 * m2.m11 + m1.m22 * m2.m12;
				this.m22 = m1.m20 * m2.m20 + m1.m21 * m2.m21 + m1.m22 * m2.m22;
			}
			else
			{
				float m00, m01, m02, m10, m11, m12, m20, m21, m22; // vars for temp result matrix 
				
				m00 = m1.m00 * m2.m00 + m1.m01 * m2.m01 + m1.m02 * m2.m02;
				m01 = m1.m00 * m2.m10 + m1.m01 * m2.m11 + m1.m02 * m2.m12;
				m02 = m1.m00 * m2.m20 + m1.m01 * m2.m21 + m1.m02 * m2.m22;
				
				m10 = m1.m10 * m2.m00 + m1.m11 * m2.m01 + m1.m12 * m2.m02;
				m11 = m1.m10 * m2.m10 + m1.m11 * m2.m11 + m1.m12 * m2.m12;
				m12 = m1.m10 * m2.m20 + m1.m11 * m2.m21 + m1.m12 * m2.m22;
				
				m20 = m1.m20 * m2.m00 + m1.m21 * m2.m01 + m1.m22 * m2.m02;
				m21 = m1.m20 * m2.m10 + m1.m21 * m2.m11 + m1.m22 * m2.m12;
				m22 = m1.m20 * m2.m20 + m1.m21 * m2.m21 + m1.m22 * m2.m22;
				
				this.m00 = m00; this.m01 = m01; this.m02 = m02;
				this.m10 = m10; this.m11 = m11; this.m12 = m12;
				this.m20 = m20; this.m21 = m21; this.m22 = m22;
			}
		}
		
		/// <summary>  Multiplies the transpose of matrix m1 times matrix m2, and 
		/// places the result into this.  
		/// </summary>
		/// <param name="m1"> the matrix on the left hand side of the multiplication  
		/// </param>
		/// <param name="m2"> the matrix on the right hand side of the multiplication 
		/// </param>
		public void  mulTransposeLeft(Matrix3f m1, Matrix3f m2)
		{
			if (this != m1 && this != m2)
			{
				this.m00 = m1.m00 * m2.m00 + m1.m10 * m2.m10 + m1.m20 * m2.m20;
				this.m01 = m1.m00 * m2.m01 + m1.m10 * m2.m11 + m1.m20 * m2.m21;
				this.m02 = m1.m00 * m2.m02 + m1.m10 * m2.m12 + m1.m20 * m2.m22;
				
				this.m10 = m1.m01 * m2.m00 + m1.m11 * m2.m10 + m1.m21 * m2.m20;
				this.m11 = m1.m01 * m2.m01 + m1.m11 * m2.m11 + m1.m21 * m2.m21;
				this.m12 = m1.m01 * m2.m02 + m1.m11 * m2.m12 + m1.m21 * m2.m22;
				
				this.m20 = m1.m02 * m2.m00 + m1.m12 * m2.m10 + m1.m22 * m2.m20;
				this.m21 = m1.m02 * m2.m01 + m1.m12 * m2.m11 + m1.m22 * m2.m21;
				this.m22 = m1.m02 * m2.m02 + m1.m12 * m2.m12 + m1.m22 * m2.m22;
			}
			else
			{
				float m00, m01, m02, m10, m11, m12, m20, m21, m22; // vars for temp result matrix 
				
				m00 = m1.m00 * m2.m00 + m1.m10 * m2.m10 + m1.m20 * m2.m20;
				m01 = m1.m00 * m2.m01 + m1.m10 * m2.m11 + m1.m20 * m2.m21;
				m02 = m1.m00 * m2.m02 + m1.m10 * m2.m12 + m1.m20 * m2.m22;
				
				m10 = m1.m01 * m2.m00 + m1.m11 * m2.m10 + m1.m21 * m2.m20;
				m11 = m1.m01 * m2.m01 + m1.m11 * m2.m11 + m1.m21 * m2.m21;
				m12 = m1.m01 * m2.m02 + m1.m11 * m2.m12 + m1.m21 * m2.m22;
				
				m20 = m1.m02 * m2.m00 + m1.m12 * m2.m10 + m1.m22 * m2.m20;
				m21 = m1.m02 * m2.m01 + m1.m12 * m2.m11 + m1.m22 * m2.m21;
				m22 = m1.m02 * m2.m02 + m1.m12 * m2.m12 + m1.m22 * m2.m22;
				
				this.m00 = m00; this.m01 = m01; this.m02 = m02;
				this.m10 = m10; this.m11 = m11; this.m12 = m12;
				this.m20 = m20; this.m21 = m21; this.m22 = m22;
			}
		}
		
		/// <summary> Performs singular value decomposition normalization of this matrix.   </summary>
		public void  normalize()
		{
			
			double[] tmp_rot = new double[9]; // scratch matrix
			double[] tmp_scale = new double[3]; // scratch matrix
			getScaleRotate(tmp_scale, tmp_rot);
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m00 = (float) tmp_rot[0];
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m01 = (float) tmp_rot[1];
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m02 = (float) tmp_rot[2];
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m10 = (float) tmp_rot[3];
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m11 = (float) tmp_rot[4];
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m12 = (float) tmp_rot[5];
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m20 = (float) tmp_rot[6];
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m21 = (float) tmp_rot[7];
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m22 = (float) tmp_rot[8];
		}
		
		/// <summary> Perform singular value decomposition normalization of matrix m1 
		/// and place the normalized values into this.   
		/// </summary>
		/// <param name="m1"> the matrix values to be normalized
		/// </param>
		public void  normalize(Matrix3f m1)
		{
			double[] tmp = new double[9]; // scratch matrix
			double[] tmp_rot = new double[9]; // scratch matrix
			double[] tmp_scale = new double[3]; // scratch matrix
			
			tmp[0] = m1.m00;
			tmp[1] = m1.m01;
			tmp[2] = m1.m02;
			
			tmp[3] = m1.m10;
			tmp[4] = m1.m11;
			tmp[5] = m1.m12;
			
			tmp[6] = m1.m20;
			tmp[7] = m1.m21;
			tmp[8] = m1.m22;
			
			Matrix3d.compute_svd(tmp, tmp_scale, tmp_rot);
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m00 = (float) (tmp_rot[0]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m01 = (float) (tmp_rot[1]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m02 = (float) (tmp_rot[2]);
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m10 = (float) (tmp_rot[3]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m11 = (float) (tmp_rot[4]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m12 = (float) (tmp_rot[5]);
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m20 = (float) (tmp_rot[6]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m21 = (float) (tmp_rot[7]);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			this.m22 = (float) (tmp_rot[8]);
		}
		
		/// <summary> Perform cross product normalization of this matrix.   </summary>
		public void  normalizeCP()
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float mag = 1.0f / (float) System.Math.Sqrt(m00 * m00 + m10 * m10 + m20 * m20);
			m00 = m00 * mag;
			m10 = m10 * mag;
			m20 = m20 * mag;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			mag = 1.0f / (float) System.Math.Sqrt(m01 * m01 + m11 * m11 + m21 * m21);
			m01 = m01 * mag;
			m11 = m11 * mag;
			m21 = m21 * mag;
			
			m02 = m10 * m21 - m11 * m20;
			m12 = m01 * m20 - m00 * m21;
			m22 = m00 * m11 - m01 * m10;
		}
		
		/// <summary> Perform cross product normalization of matrix m1 and place the 
		/// normalized values into this.   
		/// </summary>
		/// <param name="m1"> Provides the matrix values to be normalized
		/// </param>
		public void  normalizeCP(Matrix3f m1)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float mag = 1.0f / (float) System.Math.Sqrt(m1.m00 * m1.m00 + m1.m10 * m1.m10 + m1.m20 * m1.m20);
			m00 = m1.m00 * mag;
			m10 = m1.m10 * mag;
			m20 = m1.m20 * mag;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			mag = 1.0f / (float) System.Math.Sqrt(m1.m01 * m1.m01 + m1.m11 * m1.m11 + m1.m21 * m1.m21);
			m01 = m1.m01 * mag;
			m11 = m1.m11 * mag;
			m21 = m1.m21 * mag;
			
			m02 = m10 * m21 - m11 * m20;
			m12 = m01 * m20 - m00 * m21;
			m22 = m00 * m11 - m01 * m10;
		}
		
		/// <summary> Returns true if all of the data members of Matrix3f m1 are
		/// equal to the corresponding data members in this Matrix3f.
		/// </summary>
		/// <param name="m1"> the matrix with which the comparison is made
		/// </param>
		/// <returns>  true or false
		/// </returns>
		public bool equals(Matrix3f m1)
		{
			try
			{
				
				return (this.m00 == m1.m00 && this.m01 == m1.m01 && this.m02 == m1.m02 && this.m10 == m1.m10 && this.m11 == m1.m11 && this.m12 == m1.m12 && this.m20 == m1.m20 && this.m21 == m1.m21 && this.m22 == m1.m22);
			}
			catch (System.NullReferenceException e2)
			{
				return false;
			}
		}
		
		/// <summary> Returns true if the Object o1 is of type Matrix3f and all of the
		/// data members of o1 are equal to the corresponding data members in
		/// this Matrix3f.
		/// </summary>
		/// <param name="o1"> the object with which the comparison is made
		/// </param>
		/// <returns>  true or false
		/// </returns>
		public  override bool Equals(System.Object o1)
		{
			try
			{
				
				Matrix3f m2 = (Matrix3f) o1;
				return (this.m00 == m2.m00 && this.m01 == m2.m01 && this.m02 == m2.m02 && this.m10 == m2.m10 && this.m11 == m2.m11 && this.m12 == m2.m12 && this.m20 == m2.m20 && this.m21 == m2.m21 && this.m22 == m2.m22);
			}
			catch (System.InvalidCastException e1)
			{
				return false;
			}
			catch (System.NullReferenceException e2)
			{
				return false;
			}
		}
		
		/// <summary> Returns true if the L-infinite distance between this matrix 
		/// and matrix m1 is less than or equal to the epsilon parameter, 
		/// otherwise returns false.  The L-infinite
		/// distance is equal to 
		/// MAX[i=0,1,2 ; j=0,1,2 ; abs(this.m(i,j) - m1.m(i,j)]
		/// </summary>
		/// <param name="m1"> the matrix to be compared to this matrix
		/// </param>
		/// <param name="epsilon"> the threshold value  
		/// </param>
		public virtual bool epsilonEquals(Matrix3f m1, float epsilon)
		{
			bool status = true;
			
			if (System.Math.Abs(this.m00 - m1.m00) > epsilon)
				status = false;
			if (System.Math.Abs(this.m01 - m1.m01) > epsilon)
				status = false;
			if (System.Math.Abs(this.m02 - m1.m02) > epsilon)
				status = false;
			
			if (System.Math.Abs(this.m10 - m1.m10) > epsilon)
				status = false;
			if (System.Math.Abs(this.m11 - m1.m11) > epsilon)
				status = false;
			if (System.Math.Abs(this.m12 - m1.m12) > epsilon)
				status = false;
			
			if (System.Math.Abs(this.m20 - m1.m20) > epsilon)
				status = false;
			if (System.Math.Abs(this.m21 - m1.m21) > epsilon)
				status = false;
			if (System.Math.Abs(this.m22 - m1.m22) > epsilon)
				status = false;
			
			return (status);
		}
		
		
		/// <summary> Returns a hash code value based on the data values in this
		/// object.  Two different Matrix3f objects with identical data values
		/// (i.e., Matrix3f.equals returns true) will return the same hash
		/// code value.  Two objects with different data members may return the
		/// same hash value, although this is not likely.
		/// </summary>
		/// <returns> the integer hash code value
		/// </returns>
        //public override int GetHashCode()
        //{
        //    long bits = 1L;
        //    bits = 31L * bits + (long) VecMathUtil.floatToIntBits(m00);
        //    bits = 31L * bits + (long) VecMathUtil.floatToIntBits(m01);
        //    bits = 31L * bits + (long) VecMathUtil.floatToIntBits(m02);
        //    bits = 31L * bits + (long) VecMathUtil.floatToIntBits(m10);
        //    bits = 31L * bits + (long) VecMathUtil.floatToIntBits(m11);
        //    bits = 31L * bits + (long) VecMathUtil.floatToIntBits(m12);
        //    bits = 31L * bits + (long) VecMathUtil.floatToIntBits(m20);
        //    bits = 31L * bits + (long) VecMathUtil.floatToIntBits(m21);
        //    bits = 31L * bits + (long) VecMathUtil.floatToIntBits(m22);
        //    return (int) (bits ^ (bits >> 32));
        //}
		
		
		/// <summary>  Sets this matrix to all zeros.</summary>
		public void  setZero()
		{
			m00 = 0.0f;
			m01 = 0.0f;
			m02 = 0.0f;
			
			m10 = 0.0f;
			m11 = 0.0f;
			m12 = 0.0f;
			
			m20 = 0.0f;
			m21 = 0.0f;
			m22 = 0.0f;
		}
		
		/// <summary> Negates the value of this matrix: this = -this.</summary>
		public void  negate()
		{
			this.m00 = - this.m00;
			this.m01 = - this.m01;
			this.m02 = - this.m02;
			
			this.m10 = - this.m10;
			this.m11 = - this.m11;
			this.m12 = - this.m12;
			
			this.m20 = - this.m20;
			this.m21 = - this.m21;
			this.m22 = - this.m22;
		}
		
		/// <summary>  Sets the value of this matrix equal to the negation of
		/// of the Matrix3f parameter.
		/// </summary>
		/// <param name="m1"> the source matrix
		/// </param>
		public void  negate(Matrix3f m1)
		{
			this.m00 = - m1.m00;
			this.m01 = - m1.m01;
			this.m02 = - m1.m02;
			
			this.m10 = - m1.m10;
			this.m11 = - m1.m11;
			this.m12 = - m1.m12;
			
			this.m20 = - m1.m20;
			this.m21 = - m1.m21;
			this.m22 = - m1.m22;
		}
		
		/// <summary> Multiply this matrix by the tuple t and place the result
		/// back into the tuple (t = this*t).
		/// </summary>
		/// <param name="t"> the tuple to be multiplied by this matrix and then replaced
		/// </param>
		public void  transform(Tuple3f t)
		{
			float x, y, z;
			x = m00 * t.x + m01 * t.y + m02 * t.z;
			y = m10 * t.x + m11 * t.y + m12 * t.z;
			z = m20 * t.x + m21 * t.y + m22 * t.z;
			t.set_Renamed(x, y, z);
		}
		
		/// <summary> Multiply this matrix by the tuple t and and place the result 
		/// into the tuple "result" (result = this*t).
		/// </summary>
		/// <param name="t"> the tuple to be multiplied by this matrix
		/// </param>
		/// <param name="result"> the tuple into which the product is placed
		/// </param>
		public void  transform(Tuple3f t, Tuple3f result)
		{
			float x, y, z;
			x = m00 * t.x + m01 * t.y + m02 * t.z;
			y = m10 * t.x + m11 * t.y + m12 * t.z;
			result.z = m20 * t.x + m21 * t.y + m22 * t.z;
			result.x = x;
			result.y = y;
		}
		
		/// <summary> perform SVD (if necessary to get rotational component </summary>
		internal virtual void  getScaleRotate(double[] scales, double[] rot)
		{
			
			double[] tmp = new double[9]; // scratch matrix
			tmp[0] = m00;
			tmp[1] = m01;
			tmp[2] = m02;
			tmp[3] = m10;
			tmp[4] = m11;
			tmp[5] = m12;
			tmp[6] = m20;
			tmp[7] = m21;
			tmp[8] = m22;
			Matrix3d.compute_svd(tmp, scales, rot);
			
			return ;
		}
		
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
			Matrix3f m1 = null;
			try
			{
				m1 = (Matrix3f) base.MemberwiseClone();
			}
			//UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				// this shouldn't happen, since we are Cloneable
				throw new System.ApplicationException();
			}
			return m1;
		}
	}
}