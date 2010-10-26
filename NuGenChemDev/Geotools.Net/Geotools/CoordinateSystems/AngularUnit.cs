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

using System;

namespace Geotools.CoordinateReferenceSystems
{
	/// <summary>
	/// The AngularUnit class holds the standard information stored with angular units.
	/// </summary>
	public class AngularUnit : AbstractInformation, IAngularUnit
	{
		double _radiansPerUnit;

		/// <summary>
		/// Initializes a new instance of the AngularUnit class with a value for the RadiansPerUnit property.
		/// </summary>
		/// <param name="radiansPerUnit">The number of radians per angular unit.</param>
		internal AngularUnit(double radiansPerUnit ) : this(radiansPerUnit, "", "", "", "", "", "")
		{
		}

		/// <summary>
		/// Initializes a new instance of the AngularUnit class with the specified parameters.
		/// </summary>
		/// <param name="radiansPerUnit">The number of radians per AngularUnit.</param>
		/// <param name="remarks">The provider-supplied remarks.</param>
		/// <param name="authority">The authority-specific identification code.</param>
		/// <param name="authorityCode">The authority-specific identification code.</param>
		/// <param name="name">The name.</param>
		/// <param name="alias">The alias.</param>
		/// <param name="abbreviation">The abbreviation.</param>
		internal AngularUnit(	double radiansPerUnit,
							string remarks,
							string authority,
							string authorityCode,
							string name,
							string alias,
							string abbreviation)	
			: base(remarks, authority, authorityCode, name, alias, abbreviation)
		{
			_radiansPerUnit = radiansPerUnit;
		}
		#region Implementation of IAngularUnit

		/// <summary>
		///	Gets the number of radians per unit.	
		/// </summary>
		public double RadiansPerUnit
		{
			get
			{
				return _radiansPerUnit;
			}
		}
		#endregion


	}
}
