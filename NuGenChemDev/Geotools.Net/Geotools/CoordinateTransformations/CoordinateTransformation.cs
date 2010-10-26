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
using Geotools.CoordinateReferenceSystems;

namespace Geotools.CoordinateTransformations
{
	/// <summary>
	/// Describes a coordinate transformation.
	/// </summary>
	/// <remarks>
	/// This interface only describes a coordinate transformation, it does not actually perform the transform operation on points.  To transform points you must use a math transform.
	/// </remarks>
	public class CoordinateTransformation : MathTransform,ICoordinateTransformation
	{
		/// <summary>
		///  Initializes a new instance of the VerticalDatum class.
		/// </summary>
		/// <param name="transformType">The type of transform.</param>
		/// <param name="targetCS">The target coordinate system.</param>
		/// <param name="sourceCS">The source coordinate system.</param>
		/// <param name="mathTransform">The object that actually does the transformation.</param>
		/// <param name="authorityCode">The authority code.</param>
		/// <param name="authority">The authority.</param>
		/// <param name="name">The name of the transform.</param>
		/// <param name="areaOfUse">The area of use.</param>
		/// <param name="remarks">Remarks about this transformation.</param>
		/// <param name="abbreviation">The abbreviation for this transformation.</param>
		public CoordinateTransformation(	TransformType transformType,
												ICoordinateSystem targetCS,
												ICoordinateSystem sourceCS,
												IMathTransform mathTransform,
												string authorityCode,
												string authority,
												string name,
												string areaOfUse,
												string remarks,
												string abbreviation)
			:base(remarks, authority, authorityCode, name, "", "")
		{
			_transformType = transformType;
			_targetCS = targetCS;
			_mathTransform = mathTransform;
			_areaOfUse = areaOfUse;
			_sourceCS = sourceCS;
			_abbreviation = abbreviation;
			_authority=authority;
			_authorityCode=authorityCode;
			_remarks = remarks;
			_alias="";
			_name=name;
		}
		
		TransformType _transformType;
		ICoordinateSystem _targetCS;
		ICoordinateSystem _sourceCS;
		IMathTransform _mathTransform;
		string _areaOfUse;
		string _abbreviation;
		string _name;
		string _remarks;
		string _authorityCode;
		string _authority;
		string _alias;
		
		#region Implementation of ICoordinateTransformation


		/// <summary>
		/// Semantic type of transform. For example, a datum transformation or a coordinate conversion.
		/// </summary>
		public TransformType TransformType
		{
			get
			{
				return _transformType;
			}
		}

		

		/// <summary>
		/// Gets the target coordinate system.
		/// </summary>
		public ICoordinateSystem TargetCS
		{
			get
			{
				return _targetCS;
			}
		}

		

		/// <summary>
		/// Gets math transform.
		/// </summary>
		public IMathTransform MathTransform
		{
			get
			{
				return _mathTransform;
			}
		}

		

		

		/// <summary>
		/// Gets the human readable description of domain in source coordinate system.
		/// </summary>
		public string AreaOfUse
		{
			get
			{
				return _areaOfUse;
			}
		}

		/// <summary>
		/// Gets the source coordinate system.
		/// </summary>
		public ICoordinateSystem SourceCS
		{
			get
			{
				return _sourceCS;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public string Abbreviation
		{
			get
			{
				return _abbreviation;
			}
		}
		public string Alias
		{
			get
			{
				return _alias;
			}
		}
		
		public string Remarks
		{
			get
			{
				return _remarks;
			}
		}
		public string Authority
		{
			get
			{
				return _authority;
			}
		}
		public string AuthorityCode
		{
			get
			{
				return _authorityCode;
			}
		}
		#endregion
	}
}
