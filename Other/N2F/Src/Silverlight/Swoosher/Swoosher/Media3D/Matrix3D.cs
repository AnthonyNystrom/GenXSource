using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Next2Friends.Swoosher.Media3D
{
	public struct Matrix3D// : IFormattable
	{
		private const int c_identityHashCode = 0;
		private double _m11;
		private double _m12;
		private double _m13;
		private double _m14;
		private double _m21;
		private double _m22;
		private double _m23;
		private double _m24;
		private double _m31;
		private double _m32;
		private double _m33;
		private double _m34;
		private double _offsetX;
		private double _offsetY;
		private double _offsetZ;
		private double _m44;
		private bool _isNotKnownToBeIdentity;
		private static readonly Matrix3D s_identity;
		public Matrix3D( double m11, double m12, double m13, double m14, double m21, double m22, double m23, double m24, double m31, double m32, double m33, double m34, double offsetX, double offsetY, double offsetZ, double m44 )
		{
			this._m11 = m11;
			this._m12 = m12;
			this._m13 = m13;
			this._m14 = m14;
			this._m21 = m21;
			this._m22 = m22;
			this._m23 = m23;
			this._m24 = m24;
			this._m31 = m31;
			this._m32 = m32;
			this._m33 = m33;
			this._m34 = m34;
			this._offsetX = offsetX;
			this._offsetY = offsetY;
			this._offsetZ = offsetZ;
			this._m44 = m44;
			this._isNotKnownToBeIdentity = true;
		}

		public static Matrix3D Identity
		{
			get
			{
				return s_identity;
			}
		}
		public void SetIdentity()
		{
			this = s_identity;
		}

		public bool IsIdentity
		{
			get
			{
				if ( this.IsDistinguishedIdentity )
				{
					return true;
				}
				if ( ( ( ( ( this._m11 == 1 ) && ( this._m12 == 0 ) ) && ( ( this._m13 == 0 ) && ( this._m14 == 0 ) ) ) && ( ( ( this._m21 == 0 ) && ( this._m22 == 1 ) ) && ( ( this._m23 == 0 ) && ( this._m24 == 0 ) ) ) ) && ( ( ( ( this._m31 == 0 ) && ( this._m32 == 0 ) ) && ( ( this._m33 == 1 ) && ( this._m34 == 0 ) ) ) && ( ( ( this._offsetX == 0 ) && ( this._offsetY == 0 ) ) && ( ( this._offsetZ == 0 ) && ( this._m44 == 1 ) ) ) ) )
				{
					this.IsDistinguishedIdentity = true;
					return true;
				}
				return false;
			}
		}
		public void Prepend( Matrix3D matrix )
		{
			this = matrix * this;
		}

		public void Append( Matrix3D matrix )
		{
			this *= matrix;
		}

		public void Rotate( Quaternion quaternion )
		{
			Point3D center = new Point3D();
			this *= CreateRotationMatrix( ref quaternion, ref center );
		}

		public void RotatePrepend( Quaternion quaternion )
		{
			Point3D center = new Point3D();
			this = CreateRotationMatrix( ref quaternion, ref center ) * this;
		}

		public void RotateAt( Quaternion quaternion, Point3D center )
		{
			this *= CreateRotationMatrix( ref quaternion, ref center );
		}

		public void RotateAtPrepend( Quaternion quaternion, Point3D center )
		{
			this = CreateRotationMatrix( ref quaternion, ref center ) * this;
		}

		public void Scale( Vector3D scale )
		{
			if ( this.IsDistinguishedIdentity )
			{
				this.SetScaleMatrix( ref scale );
			}
			else
			{
				this._m11 *= scale.X;
				this._m12 *= scale.Y;
				this._m13 *= scale.Z;
				this._m21 *= scale.X;
				this._m22 *= scale.Y;
				this._m23 *= scale.Z;
				this._m31 *= scale.X;
				this._m32 *= scale.Y;
				this._m33 *= scale.Z;
				this._offsetX *= scale.X;
				this._offsetY *= scale.Y;
				this._offsetZ *= scale.Z;
			}
		}

		public void ScalePrepend( Vector3D scale )
		{
			if ( this.IsDistinguishedIdentity )
			{
				this.SetScaleMatrix( ref scale );
			}
			else
			{
				this._m11 *= scale.X;
				this._m12 *= scale.X;
				this._m13 *= scale.X;
				this._m14 *= scale.X;
				this._m21 *= scale.Y;
				this._m22 *= scale.Y;
				this._m23 *= scale.Y;
				this._m24 *= scale.Y;
				this._m31 *= scale.Z;
				this._m32 *= scale.Z;
				this._m33 *= scale.Z;
				this._m34 *= scale.Z;
			}
		}

		public void ScaleAt( Vector3D scale, Point3D center )
		{
			if ( this.IsDistinguishedIdentity )
			{
				this.SetScaleMatrix( ref scale, ref center );
			}
			else
			{
				double num = this._m14 * center.X;
				this._m11 = num + ( scale.X * ( this._m11 - num ) );
				num = this._m14 * center.Y;
				this._m12 = num + ( scale.Y * ( this._m12 - num ) );
				num = this._m14 * center.Z;
				this._m13 = num + ( scale.Z * ( this._m13 - num ) );
				num = this._m24 * center.X;
				this._m21 = num + ( scale.X * ( this._m21 - num ) );
				num = this._m24 * center.Y;
				this._m22 = num + ( scale.Y * ( this._m22 - num ) );
				num = this._m24 * center.Z;
				this._m23 = num + ( scale.Z * ( this._m23 - num ) );
				num = this._m34 * center.X;
				this._m31 = num + ( scale.X * ( this._m31 - num ) );
				num = this._m34 * center.Y;
				this._m32 = num + ( scale.Y * ( this._m32 - num ) );
				num = this._m34 * center.Z;
				this._m33 = num + ( scale.Z * ( this._m33 - num ) );
				num = this._m44 * center.X;
				this._offsetX = num + ( scale.X * ( this._offsetX - num ) );
				num = this._m44 * center.Y;
				this._offsetY = num + ( scale.Y * ( this._offsetY - num ) );
				num = this._m44 * center.Z;
				this._offsetZ = num + ( scale.Z * ( this._offsetZ - num ) );
			}
		}

		public void ScaleAtPrepend( Vector3D scale, Point3D center )
		{
			if ( this.IsDistinguishedIdentity )
			{
				this.SetScaleMatrix( ref scale, ref center );
			}
			else
			{
				double num3 = center.X - ( center.X * scale.X );
				double num2 = center.Y - ( center.Y * scale.Y );
				double num = center.Z - ( center.Z * scale.Z );
				this._offsetX += ( ( this._m11 * num3 ) + ( this._m21 * num2 ) ) + ( this._m31 * num );
				this._offsetY += ( ( this._m12 * num3 ) + ( this._m22 * num2 ) ) + ( this._m32 * num );
				this._offsetZ += ( ( this._m13 * num3 ) + ( this._m23 * num2 ) ) + ( this._m33 * num );
				this._m44 += ( ( this._m14 * num3 ) + ( this._m24 * num2 ) ) + ( this._m34 * num );
				this._m11 *= scale.X;
				this._m12 *= scale.X;
				this._m13 *= scale.X;
				this._m14 *= scale.X;
				this._m21 *= scale.Y;
				this._m22 *= scale.Y;
				this._m23 *= scale.Y;
				this._m24 *= scale.Y;
				this._m31 *= scale.Z;
				this._m32 *= scale.Z;
				this._m33 *= scale.Z;
				this._m34 *= scale.Z;
			}
		}

		public void Translate( Vector3D offset )
		{
			if ( this.IsDistinguishedIdentity )
			{
				this.SetTranslationMatrix( ref offset );
			}
			else
			{
				this._m11 += this._m14 * offset.X;
				this._m12 += this._m14 * offset.Y;
				this._m13 += this._m14 * offset.Z;
				this._m21 += this._m24 * offset.X;
				this._m22 += this._m24 * offset.Y;
				this._m23 += this._m24 * offset.Z;
				this._m31 += this._m34 * offset.X;
				this._m32 += this._m34 * offset.Y;
				this._m33 += this._m34 * offset.Z;
				this._offsetX += this._m44 * offset.X;
				this._offsetY += this._m44 * offset.Y;
				this._offsetZ += this._m44 * offset.Z;
			}
		}

		public void TranslatePrepend( Vector3D offset )
		{
			if ( this.IsDistinguishedIdentity )
			{
				this.SetTranslationMatrix( ref offset );
			}
			else
			{
				this._offsetX += ( ( this._m11 * offset.X ) + ( this._m21 * offset.Y ) ) + ( this._m31 * offset.Z );
				this._offsetY += ( ( this._m12 * offset.X ) + ( this._m22 * offset.Y ) ) + ( this._m32 * offset.Z );
				this._offsetZ += ( ( this._m13 * offset.X ) + ( this._m23 * offset.Y ) ) + ( this._m33 * offset.Z );
				this._m44 += ( ( this._m14 * offset.X ) + ( this._m24 * offset.Y ) ) + ( this._m34 * offset.Z );
			}
		}

		public static Matrix3D operator *( Matrix3D matrix1, Matrix3D matrix2 )
		{
			if ( matrix1.IsDistinguishedIdentity )
			{
				return matrix2;
			}
			if ( matrix2.IsDistinguishedIdentity )
			{
				return matrix1;
			}
			return new Matrix3D( ( ( ( matrix1._m11 * matrix2._m11 ) + ( matrix1._m12 * matrix2._m21 ) ) + ( matrix1._m13 * matrix2._m31 ) ) + ( matrix1._m14 * matrix2._offsetX ), ( ( ( matrix1._m11 * matrix2._m12 ) + ( matrix1._m12 * matrix2._m22 ) ) + ( matrix1._m13 * matrix2._m32 ) ) + ( matrix1._m14 * matrix2._offsetY ), ( ( ( matrix1._m11 * matrix2._m13 ) + ( matrix1._m12 * matrix2._m23 ) ) + ( matrix1._m13 * matrix2._m33 ) ) + ( matrix1._m14 * matrix2._offsetZ ), ( ( ( matrix1._m11 * matrix2._m14 ) + ( matrix1._m12 * matrix2._m24 ) ) + ( matrix1._m13 * matrix2._m34 ) ) + ( matrix1._m14 * matrix2._m44 ), ( ( ( matrix1._m21 * matrix2._m11 ) + ( matrix1._m22 * matrix2._m21 ) ) + ( matrix1._m23 * matrix2._m31 ) ) + ( matrix1._m24 * matrix2._offsetX ), ( ( ( matrix1._m21 * matrix2._m12 ) + ( matrix1._m22 * matrix2._m22 ) ) + ( matrix1._m23 * matrix2._m32 ) ) + ( matrix1._m24 * matrix2._offsetY ), ( ( ( matrix1._m21 * matrix2._m13 ) + ( matrix1._m22 * matrix2._m23 ) ) + ( matrix1._m23 * matrix2._m33 ) ) + ( matrix1._m24 * matrix2._offsetZ ), ( ( ( matrix1._m21 * matrix2._m14 ) + ( matrix1._m22 * matrix2._m24 ) ) + ( matrix1._m23 * matrix2._m34 ) ) + ( matrix1._m24 * matrix2._m44 ), ( ( ( matrix1._m31 * matrix2._m11 ) + ( matrix1._m32 * matrix2._m21 ) ) + ( matrix1._m33 * matrix2._m31 ) ) + ( matrix1._m34 * matrix2._offsetX ), ( ( ( matrix1._m31 * matrix2._m12 ) + ( matrix1._m32 * matrix2._m22 ) ) + ( matrix1._m33 * matrix2._m32 ) ) + ( matrix1._m34 * matrix2._offsetY ), ( ( ( matrix1._m31 * matrix2._m13 ) + ( matrix1._m32 * matrix2._m23 ) ) + ( matrix1._m33 * matrix2._m33 ) ) + ( matrix1._m34 * matrix2._offsetZ ), ( ( ( matrix1._m31 * matrix2._m14 ) + ( matrix1._m32 * matrix2._m24 ) ) + ( matrix1._m33 * matrix2._m34 ) ) + ( matrix1._m34 * matrix2._m44 ), ( ( ( matrix1._offsetX * matrix2._m11 ) + ( matrix1._offsetY * matrix2._m21 ) ) + ( matrix1._offsetZ * matrix2._m31 ) ) + ( matrix1._m44 * matrix2._offsetX ), ( ( ( matrix1._offsetX * matrix2._m12 ) + ( matrix1._offsetY * matrix2._m22 ) ) + ( matrix1._offsetZ * matrix2._m32 ) ) + ( matrix1._m44 * matrix2._offsetY ), ( ( ( matrix1._offsetX * matrix2._m13 ) + ( matrix1._offsetY * matrix2._m23 ) ) + ( matrix1._offsetZ * matrix2._m33 ) ) + ( matrix1._m44 * matrix2._offsetZ ), ( ( ( matrix1._offsetX * matrix2._m14 ) + ( matrix1._offsetY * matrix2._m24 ) ) + ( matrix1._offsetZ * matrix2._m34 ) ) + ( matrix1._m44 * matrix2._m44 ) );
		}

		public static Matrix3D Multiply( Matrix3D matrix1, Matrix3D matrix2 )
		{
			return ( matrix1 * matrix2 );
		}

		public Point3D Transform( Point3D point )
		{
			this.MultiplyPoint( ref point );
			return point;
		}

		public void Transform( ref Point3D[] points )
		{
			if ( points != null )
			{
				for ( int i = 0; i < points.Length; i++ )
				{
					this.MultiplyPoint( ref points[ i ] );
				}
			}
		}

		public Point4D Transform( Point4D point )
		{
			this.MultiplyPoint( ref point );
			return point;
		}

		public void Transform( ref Point4D[] points )
		{
			if ( points != null )
			{
				for ( int i = 0; i < points.Length; i++ )
				{
					this.MultiplyPoint( ref points[ i ] );
				}
			}
		}

		public Vector3D Transform( Vector3D vector )
		{
			this.MultiplyVector( ref vector );
			return vector;
		}

		public void Transform( ref Vector3D[] vectors )
		{
			if ( vectors != null )
			{
				for ( int i = 0; i < vectors.Length; i++ )
				{
					this.MultiplyVector( ref vectors[ i ] );
				}
			}
		}

		public bool IsAffine
		{
			get
			{
				if ( this.IsDistinguishedIdentity )
				{
					return true;
				}
				if ( ( ( this._m14 == 0 ) && ( this._m24 == 0 ) ) && ( this._m34 == 0 ) )
				{
					return ( this._m44 == 1 );
				}
				return false;
			}
		}
		public double Determinant
		{
			get
			{
				if ( this.IsDistinguishedIdentity )
				{
					return 1;
				}
				if ( this.IsAffine )
				{
					return this.GetNormalizedAffineDeterminant();
				}
				double num6 = ( this._m13 * this._m24 ) - ( this._m23 * this._m14 );
				double num5 = ( this._m13 * this._m34 ) - ( this._m33 * this._m14 );
				double num4 = ( this._m13 * this._m44 ) - ( this._offsetZ * this._m14 );
				double num3 = ( this._m23 * this._m34 ) - ( this._m33 * this._m24 );
				double num2 = ( this._m23 * this._m44 ) - ( this._offsetZ * this._m24 );
				double num = ( this._m33 * this._m44 ) - ( this._offsetZ * this._m34 );
				double num10 = ( ( this._m22 * num5 ) - ( this._m32 * num6 ) ) - ( this._m12 * num3 );
				double num9 = ( ( this._m12 * num2 ) - ( this._m22 * num4 ) ) + ( this._offsetY * num6 );
				double num8 = ( ( this._m32 * num4 ) - ( this._offsetY * num5 ) ) - ( this._m12 * num );
				double num7 = ( ( this._m22 * num ) - ( this._m32 * num2 ) ) + ( this._offsetY * num3 );
				return ( ( ( ( this._offsetX * num10 ) + ( this._m31 * num9 ) ) + ( this._m21 * num8 ) ) + ( this._m11 * num7 ) );
			}
		}
		public bool HasInverse
		{
			get
			{
				return !DoubleUtil.IsZero( this.Determinant );
			}
		}
		public void Invert()
		{
			if ( !this.InvertCore() )
			{
				throw new InvalidOperationException( "Not invertible" );
			}
		}

		public double M11
		{
			get
			{
				if ( this.IsDistinguishedIdentity )
				{
					return 1;
				}
				return this._m11;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._m11 = value;
			}
		}
		public double M12
		{
			get
			{
				return this._m12;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._m12 = value;
			}
		}
		public double M13
		{
			get
			{
				return this._m13;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._m13 = value;
			}
		}
		public double M14
		{
			get
			{
				return this._m14;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._m14 = value;
			}
		}
		public double M21
		{
			get
			{
				return this._m21;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._m21 = value;
			}
		}
		public double M22
		{
			get
			{
				if ( this.IsDistinguishedIdentity )
				{
					return 1;
				}
				return this._m22;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._m22 = value;
			}
		}
		public double M23
		{
			get
			{
				return this._m23;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._m23 = value;
			}
		}
		public double M24
		{
			get
			{
				return this._m24;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._m24 = value;
			}
		}
		public double M31
		{
			get
			{
				return this._m31;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._m31 = value;
			}
		}
		public double M32
		{
			get
			{
				return this._m32;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._m32 = value;
			}
		}
		public double M33
		{
			get
			{
				if ( this.IsDistinguishedIdentity )
				{
					return 1;
				}
				return this._m33;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._m33 = value;
			}
		}
		public double M34
		{
			get
			{
				return this._m34;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._m34 = value;
			}
		}
		public double OffsetX
		{
			get
			{
				return this._offsetX;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._offsetX = value;
			}
		}
		public double OffsetY
		{
			get
			{
				return this._offsetY;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._offsetY = value;
			}
		}
		public double OffsetZ
		{
			get
			{
				return this._offsetZ;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._offsetZ = value;
			}
		}
		public double M44
		{
			get
			{
				if ( this.IsDistinguishedIdentity )
				{
					return 1;
				}
				return this._m44;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._m44 = value;
			}
		}
		internal void SetScaleMatrix( ref Vector3D scale )
		{
			this._m11 = scale.X;
			this._m22 = scale.Y;
			this._m33 = scale.Z;
			this._m44 = 1;
			this.IsDistinguishedIdentity = false;
		}

		internal void SetScaleMatrix( ref Vector3D scale, ref Point3D center )
		{
			this._m11 = scale.X;
			this._m22 = scale.Y;
			this._m33 = scale.Z;
			this._m44 = 1;
			this._offsetX = center.X - ( center.X * scale.X );
			this._offsetY = center.Y - ( center.Y * scale.Y );
			this._offsetZ = center.Z - ( center.Z * scale.Z );
			this.IsDistinguishedIdentity = false;
		}

		internal void SetTranslationMatrix( ref Vector3D offset )
		{
			this._m11 = this._m22 = this._m33 = this._m44 = 1;
			this._offsetX = offset.X;
			this._offsetY = offset.Y;
			this._offsetZ = offset.Z;
			this.IsDistinguishedIdentity = false;
		}

		internal static Matrix3D CreateRotationMatrix( ref Quaternion quaternion, ref Point3D center )
		{
			Matrix3D matrixd = s_identity;
			matrixd.IsDistinguishedIdentity = false;
			double num12 = quaternion.X + quaternion.X;
			double num2 = quaternion.Y + quaternion.Y;
			double num = quaternion.Z + quaternion.Z;
			double num11 = quaternion.X * num12;
			double num10 = quaternion.X * num2;
			double num9 = quaternion.X * num;
			double num8 = quaternion.Y * num2;
			double num7 = quaternion.Y * num;
			double num6 = quaternion.Z * num;
			double num5 = quaternion.W * num12;
			double num4 = quaternion.W * num2;
			double num3 = quaternion.W * num;
			matrixd._m11 = 1 - ( num8 + num6 );
			matrixd._m12 = num10 + num3;
			matrixd._m13 = num9 - num4;
			matrixd._m21 = num10 - num3;
			matrixd._m22 = 1 - ( num11 + num6 );
			matrixd._m23 = num7 + num5;
			matrixd._m31 = num9 + num4;
			matrixd._m32 = num7 - num5;
			matrixd._m33 = 1 - ( num11 + num8 );
			if ( ( ( center.X != 0 ) || ( center.Y != 0 ) ) || ( center.Z != 0 ) )
			{
				matrixd._offsetX = ( ( ( -center.X * matrixd._m11 ) - ( center.Y * matrixd._m21 ) ) - ( center.Z * matrixd._m31 ) ) + center.X;
				matrixd._offsetY = ( ( ( -center.X * matrixd._m12 ) - ( center.Y * matrixd._m22 ) ) - ( center.Z * matrixd._m32 ) ) + center.Y;
				matrixd._offsetZ = ( ( ( -center.X * matrixd._m13 ) - ( center.Y * matrixd._m23 ) ) - ( center.Z * matrixd._m33 ) ) + center.Z;
			}
			return matrixd;
		}

		internal void MultiplyPoint( ref Point3D point )
		{
			if ( !this.IsDistinguishedIdentity )
			{
				double x = point.X;
				double y = point.Y;
				double z = point.Z;
				point.X = ( ( ( x * this._m11 ) + ( y * this._m21 ) ) + ( z * this._m31 ) ) + this._offsetX;
				point.Y = ( ( ( x * this._m12 ) + ( y * this._m22 ) ) + ( z * this._m32 ) ) + this._offsetY;
				point.Z = ( ( ( x * this._m13 ) + ( y * this._m23 ) ) + ( z * this._m33 ) ) + this._offsetZ;
				if ( !this.IsAffine )
				{
					double num4 = ( ( ( x * this._m14 ) + ( y * this._m24 ) ) + ( z * this._m34 ) ) + this._m44;
					point.X /= num4;
					point.Y /= num4;
					point.Z /= num4;
				}
			}
		}

		internal void MultiplyPoint( ref Point4D point )
		{
			if ( !this.IsDistinguishedIdentity )
			{
				double x = point.X;
				double y = point.Y;
				double z = point.Z;
				double w = point.W;
				point.X = ( ( ( x * this._m11 ) + ( y * this._m21 ) ) + ( z * this._m31 ) ) + ( w * this._offsetX );
				point.Y = ( ( ( x * this._m12 ) + ( y * this._m22 ) ) + ( z * this._m32 ) ) + ( w * this._offsetY );
				point.Z = ( ( ( x * this._m13 ) + ( y * this._m23 ) ) + ( z * this._m33 ) ) + ( w * this._offsetZ );
				point.W = ( ( ( x * this._m14 ) + ( y * this._m24 ) ) + ( z * this._m34 ) ) + ( w * this._m44 );
			}
		}

		internal void MultiplyVector( ref Vector3D vector )
		{
			if ( !this.IsDistinguishedIdentity )
			{
				double x = vector.X;
				double y = vector.Y;
				double z = vector.Z;
				vector.X = ( ( x * this._m11 ) + ( y * this._m21 ) ) + ( z * this._m31 );
				vector.Y = ( ( x * this._m12 ) + ( y * this._m22 ) ) + ( z * this._m32 );
				vector.Z = ( ( x * this._m13 ) + ( y * this._m23 ) ) + ( z * this._m33 );
			}
		}

		internal double GetNormalizedAffineDeterminant()
		{
			double num3 = ( this._m12 * this._m23 ) - ( this._m22 * this._m13 );
			double num2 = ( this._m32 * this._m13 ) - ( this._m12 * this._m33 );
			double num = ( this._m22 * this._m33 ) - ( this._m32 * this._m23 );
			return ( ( ( this._m31 * num3 ) + ( this._m21 * num2 ) ) + ( this._m11 * num ) );
		}

		internal bool NormalizedAffineInvert()
		{
			double num11 = ( this._m12 * this._m23 ) - ( this._m22 * this._m13 );
			double num10 = ( this._m32 * this._m13 ) - ( this._m12 * this._m33 );
			double num9 = ( this._m22 * this._m33 ) - ( this._m32 * this._m23 );
			double num8 = ( ( this._m31 * num11 ) + ( this._m21 * num10 ) ) + ( this._m11 * num9 );
			if ( DoubleUtil.IsZero( num8 ) )
			{
				return false;
			}
			double num20 = ( this._m21 * this._m13 ) - ( this._m11 * this._m23 );
			double num19 = ( this._m11 * this._m33 ) - ( this._m31 * this._m13 );
			double num18 = ( this._m31 * this._m23 ) - ( this._m21 * this._m33 );
			double num7 = ( this._m11 * this._m22 ) - ( this._m21 * this._m12 );
			double num6 = ( this._m11 * this._m32 ) - ( this._m31 * this._m12 );
			double num5 = ( this._m11 * this._offsetY ) - ( this._offsetX * this._m12 );
			double num4 = ( this._m21 * this._m32 ) - ( this._m31 * this._m22 );
			double num3 = ( this._m21 * this._offsetY ) - ( this._offsetX * this._m22 );
			double num2 = ( this._m31 * this._offsetY ) - ( this._offsetX * this._m32 );
			double num17 = ( ( this._m23 * num5 ) - ( this._offsetZ * num7 ) ) - ( this._m13 * num3 );
			double num16 = ( ( this._m13 * num2 ) - ( this._m33 * num5 ) ) + ( this._offsetZ * num6 );
			double num15 = ( ( this._m33 * num3 ) - ( this._offsetZ * num4 ) ) - ( this._m23 * num2 );
			double num14 = num7;
			double num13 = -num6;
			double num12 = num4;
			double num = 1 / num8;
			this._m11 = num9 * num;
			this._m12 = num10 * num;
			this._m13 = num11 * num;
			this._m21 = num18 * num;
			this._m22 = num19 * num;
			this._m23 = num20 * num;
			this._m31 = num12 * num;
			this._m32 = num13 * num;
			this._m33 = num14 * num;
			this._offsetX = num15 * num;
			this._offsetY = num16 * num;
			this._offsetZ = num17 * num;
			return true;
		}

		internal bool InvertCore()
		{
			if ( !this.IsDistinguishedIdentity )
			{
				if ( this.IsAffine )
				{
					return this.NormalizedAffineInvert();
				}
				double num7 = ( this._m13 * this._m24 ) - ( this._m23 * this._m14 );
				double num6 = ( this._m13 * this._m34 ) - ( this._m33 * this._m14 );
				double num5 = ( this._m13 * this._m44 ) - ( this._offsetZ * this._m14 );
				double num4 = ( this._m23 * this._m34 ) - ( this._m33 * this._m24 );
				double num3 = ( this._m23 * this._m44 ) - ( this._offsetZ * this._m24 );
				double num2 = ( this._m33 * this._m44 ) - ( this._offsetZ * this._m34 );
				double num12 = ( ( this._m22 * num6 ) - ( this._m32 * num7 ) ) - ( this._m12 * num4 );
				double num11 = ( ( this._m12 * num3 ) - ( this._m22 * num5 ) ) + ( this._offsetY * num7 );
				double num10 = ( ( this._m32 * num5 ) - ( this._offsetY * num6 ) ) - ( this._m12 * num2 );
				double num9 = ( ( this._m22 * num2 ) - ( this._m32 * num3 ) ) + ( this._offsetY * num4 );
				double num8 = ( ( ( this._offsetX * num12 ) + ( this._m31 * num11 ) ) + ( this._m21 * num10 ) ) + ( this._m11 * num9 );
				if ( DoubleUtil.IsZero( num8 ) )
				{
					return false;
				}
				double num24 = ( ( this._m11 * num4 ) - ( this._m21 * num6 ) ) + ( this._m31 * num7 );
				double num23 = ( ( this._m21 * num5 ) - ( this._offsetX * num7 ) ) - ( this._m11 * num3 );
				double num22 = ( ( this._m11 * num2 ) - ( this._m31 * num5 ) ) + ( this._offsetX * num6 );
				double num21 = ( ( this._m31 * num3 ) - ( this._offsetX * num4 ) ) - ( this._m21 * num2 );
				num7 = ( this._m11 * this._m22 ) - ( this._m21 * this._m12 );
				num6 = ( this._m11 * this._m32 ) - ( this._m31 * this._m12 );
				num5 = ( this._m11 * this._offsetY ) - ( this._offsetX * this._m12 );
				num4 = ( this._m21 * this._m32 ) - ( this._m31 * this._m22 );
				num3 = ( this._m21 * this._offsetY ) - ( this._offsetX * this._m22 );
				num2 = ( this._m31 * this._offsetY ) - ( this._offsetX * this._m32 );
				double num20 = ( ( this._m13 * num4 ) - ( this._m23 * num6 ) ) + ( this._m33 * num7 );
				double num19 = ( ( this._m23 * num5 ) - ( this._offsetZ * num7 ) ) - ( this._m13 * num3 );
				double num18 = ( ( this._m13 * num2 ) - ( this._m33 * num5 ) ) + ( this._offsetZ * num6 );
				double num17 = ( ( this._m33 * num3 ) - ( this._offsetZ * num4 ) ) - ( this._m23 * num2 );
				double num16 = ( ( this._m24 * num6 ) - ( this._m34 * num7 ) ) - ( this._m14 * num4 );
				double num15 = ( ( this._m14 * num3 ) - ( this._m24 * num5 ) ) + ( this._m44 * num7 );
				double num14 = ( ( this._m34 * num5 ) - ( this._m44 * num6 ) ) - ( this._m14 * num2 );
				double num13 = ( ( this._m24 * num2 ) - ( this._m34 * num3 ) ) + ( this._m44 * num4 );
				double num = 1 / num8;
				this._m11 = num9 * num;
				this._m12 = num10 * num;
				this._m13 = num11 * num;
				this._m14 = num12 * num;
				this._m21 = num21 * num;
				this._m22 = num22 * num;
				this._m23 = num23 * num;
				this._m24 = num24 * num;
				this._m31 = num13 * num;
				this._m32 = num14 * num;
				this._m33 = num15 * num;
				this._m34 = num16 * num;
				this._offsetX = num17 * num;
				this._offsetY = num18 * num;
				this._offsetZ = num19 * num;
				this._m44 = num20 * num;
			}
			return true;
		}

		private static Matrix3D CreateIdentity()
		{
			return new Matrix3D( 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 ) { IsDistinguishedIdentity = true };
		}

		private bool IsDistinguishedIdentity
		{
			get
			{
				return !this._isNotKnownToBeIdentity;
			}
			set
			{
				this._isNotKnownToBeIdentity = !value;
			}
		}
		public static bool operator ==( Matrix3D matrix1, Matrix3D matrix2 )
		{
			if ( matrix1.IsDistinguishedIdentity || matrix2.IsDistinguishedIdentity )
			{
				return ( matrix1.IsIdentity == matrix2.IsIdentity );
			}
			if ( ( ( ( ( matrix1.M11 == matrix2.M11 ) && ( matrix1.M12 == matrix2.M12 ) ) && ( ( matrix1.M13 == matrix2.M13 ) && ( matrix1.M14 == matrix2.M14 ) ) ) && ( ( ( matrix1.M21 == matrix2.M21 ) && ( matrix1.M22 == matrix2.M22 ) ) && ( ( matrix1.M23 == matrix2.M23 ) && ( matrix1.M24 == matrix2.M24 ) ) ) ) && ( ( ( ( matrix1.M31 == matrix2.M31 ) && ( matrix1.M32 == matrix2.M32 ) ) && ( ( matrix1.M33 == matrix2.M33 ) && ( matrix1.M34 == matrix2.M34 ) ) ) && ( ( ( matrix1.OffsetX == matrix2.OffsetX ) && ( matrix1.OffsetY == matrix2.OffsetY ) ) && ( matrix1.OffsetZ == matrix2.OffsetZ ) ) ) )
			{
				return ( matrix1.M44 == matrix2.M44 );
			}
			return false;
		}

		public static bool operator !=( Matrix3D matrix1, Matrix3D matrix2 )
		{
			return !Equals( matrix1, matrix2 );
		}

		public static bool Equals( Matrix3D matrix1, Matrix3D matrix2 )
		{
			if ( matrix1.IsDistinguishedIdentity || matrix2.IsDistinguishedIdentity )
			{
				return ( matrix1.IsIdentity == matrix2.IsIdentity );
			}
			if ( ( ( ( matrix1.M11.Equals( matrix2.M11 ) && matrix1.M12.Equals( matrix2.M12 ) ) && ( matrix1.M13.Equals( matrix2.M13 ) && matrix1.M14.Equals( matrix2.M14 ) ) ) && ( ( matrix1.M21.Equals( matrix2.M21 ) && matrix1.M22.Equals( matrix2.M22 ) ) && ( matrix1.M23.Equals( matrix2.M23 ) && matrix1.M24.Equals( matrix2.M24 ) ) ) ) && ( ( ( matrix1.M31.Equals( matrix2.M31 ) && matrix1.M32.Equals( matrix2.M32 ) ) && ( matrix1.M33.Equals( matrix2.M33 ) && matrix1.M34.Equals( matrix2.M34 ) ) ) && ( ( matrix1.OffsetX.Equals( matrix2.OffsetX ) && matrix1.OffsetY.Equals( matrix2.OffsetY ) ) && matrix1.OffsetZ.Equals( matrix2.OffsetZ ) ) ) )
			{
				return matrix1.M44.Equals( matrix2.M44 );
			}
			return false;
		}

		public override bool Equals( object o )
		{
			if ( ( o == null ) || !( o is Matrix3D ) )
			{
				return false;
			}
			Matrix3D matrixd = (Matrix3D)o;
			return Equals( this, matrixd );
		}

		public bool Equals( Matrix3D value )
		{
			return Equals( this, value );
		}

		public override int GetHashCode()
		{
			if ( this.IsDistinguishedIdentity )
			{
				return 0;
			}
			return ( ( ( ( ( ( ( ( ( ( ( ( ( ( ( this.M11.GetHashCode() ^ this.M12.GetHashCode() ) ^ this.M13.GetHashCode() ) ^ this.M14.GetHashCode() ) ^ this.M21.GetHashCode() ) ^ this.M22.GetHashCode() ) ^ this.M23.GetHashCode() ) ^ this.M24.GetHashCode() ) ^ this.M31.GetHashCode() ) ^ this.M32.GetHashCode() ) ^ this.M33.GetHashCode() ) ^ this.M34.GetHashCode() ) ^ this.OffsetX.GetHashCode() ) ^ this.OffsetY.GetHashCode() ) ^ this.OffsetZ.GetHashCode() ) ^ this.M44.GetHashCode() );
		}

/*		public static Matrix3D Parse( string source )
		{
			Matrix3D identity;
			IFormatProvider cultureInfo = CultureInfo.GetCultureInfo( "en-us" );
			TokenizerHelper helper = new TokenizerHelper( source, cultureInfo );
			string str = helper.NextTokenRequired();
			if ( str == "Identity" )
			{
				identity = Identity;
			}
			else
			{
				identity = new Matrix3D( Convert.ToDouble( str, cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ) );
			}
			helper.LastTokenRequired();
			return identity;
		}

		public override string ToString()
		{
			return this.ConvertToString( null, null );
		}

		public string ToString( IFormatProvider provider )
		{
			return this.ConvertToString( null, provider );
		}

		string IFormattable.ToString( string format, IFormatProvider provider )
		{
			return this.ConvertToString( format, provider );
		}

		internal string ConvertToString( string format, IFormatProvider provider )
		{
			if ( this.IsIdentity )
			{
				return "Identity";
			}
			char numericListSeparator = TokenizerHelper.GetNumericListSeparator( provider );
			return string.Format( provider, "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}{0}{5:" + format + "}{0}{6:" + format + "}{0}{7:" + format + "}{0}{8:" + format + "}{0}{9:" + format + "}{0}{10:" + format + "}{0}{11:" + format + "}{0}{12:" + format + "}{0}{13:" + format + "}{0}{14:" + format + "}{0}{15:" + format + "}{0}{16:" + format + "}", new object[] { 
            numericListSeparator, this._m11, this._m12, this._m13, this._m14, this._m21, this._m22, this._m23, this._m24, this._m31, this._m32, this._m33, this._m34, this._offsetX, this._offsetY, this._offsetZ, 
            this._m44
         } );
		}
*/
		static Matrix3D()
		{
			s_identity = CreateIdentity();
		}
	}
}
