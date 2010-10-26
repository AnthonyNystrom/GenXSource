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
	/// A set of quantities from which other quantities are calculated.
	/// </summary>
	/// <remarks>
	///	The datum may be a textual description and/or a set of parameters describing the
	/// relationship of a coordinate system to some predefined physical locations
	/// (such as center of mass) and physical directions (such as axis of spin).
	/// It can be defined as a set of real points on the earth that have coordinates.
	/// For example a datum can be thought of as a set of parameters defining completely
	/// the origin and orientation of a coordinate system with respect to the earth.
	/// The definition of the datum may also include the temporal behavior (such
	/// as the rate of change of the orientation of the coordinate axes).
	/// </remarks>
	public class Datum : AbstractInformation, IDatum
	{
		DatumType _datumType;


		/// <summary>
		/// Initializes a new instane of Datun.
		/// </summary>
		/// <param name="name">The name of the datum.</param>
		/// <param name="datumType">The type of the datum.</param>
		internal Datum(string name, DatumType datumType) : this (datumType, "", "", "", name, "","")
		{
		}

		/// <summary>
		/// Initializes a new instance of the Datum class with the specified properties.
		/// </summary>
		/// <param name="datumType">The datum type.</param>
		internal Datum(DatumType datumType) : this (datumType, "", "", "", "", "","")
		{
		}

		/// <summary>
		/// Initializes a new instance of the EventLog class with the specified properties.
		/// </summary>
		/// <param name="datumType"></param>
		/// <param name="remarks">The provider-supplied remarks.</param>
		/// <param name="authority">The authority-specific identification code.</param>
		/// <param name="authorityCode">The authority-specific identification code.</param>
		/// <param name="name">The name.</param>
		/// <param name="alias">The alias.</param>
		/// <param name="abbreviation">The abbreviation.</param>
		public Datum(	DatumType datumType,
				string remarks,
				string authorityCode,
				string authority, 
				string name,
				string alias,
				string abbreviation ) : base(remarks,authorityCode, authority, name, alias, abbreviation )
		{
			_datumType = datumType;
		}


		#region Implementation of IDatum
		/// <summary>
		/// 
		/// </summary>
		public DatumType DatumType
		{
			get
			{
				return _datumType;
			}
		}
		#endregion
	}
}
