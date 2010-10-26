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
	/// An aggregate of two coordinate systems (CRS). 
	/// </summary>
	/// <remarks>
	/// One of these is usually a CRS based on a two
	/// dimensional coordinate system such as a geographic or a projected coordinate system with a
	/// horizontal datum. The other is a vertical CRS which is a one-dimensional coordinate system with
	/// a vertical datum.
	/// </remarks>
	public class CompoundCoordinateSystem : CoordinateSystem, ICompoundCoordinateSystem
	{

		IAxisInfo[] _axisInfo;
		ICoordinateSystem _headCRS;
		ICoordinateSystem _tailCRS;


		/// <summary>
		/// Creates a compound coordinate system.
		/// </summary>
		/// <param name="headCRS">The first coordinate system.</param>
		/// <param name="tailCRS">The second coordinate system.</param>
		/// <param name="remarks">Remarks about this object.</param>
		/// <param name="authority">The name of the authority.</param>
		/// <param name="authorityCode">The code the authority uses to identidy this object.</param>
		/// <param name="name">The name of the object.</param>
		/// <param name="alias">The alias of the object.</param>
		/// <param name="abbreviation">The abbreviated name of this object.</param>
		internal CompoundCoordinateSystem(ICoordinateSystem headCRS,
										ICoordinateSystem tailCRS,
										string remarks, string authority, string authorityCode, string name, string alias, string abbreviation)
			: base(remarks, authority, authorityCode, name, alias, abbreviation)
		{
			if (headCRS==null)
			{
				throw new ArgumentNullException("headCRS");
			}
			if (tailCRS==null)
			{
				throw new ArgumentNullException("tailCRS");
			}
			
			_headCRS = headCRS;
			_tailCRS = tailCRS;
			_axisInfo = new IAxisInfo[this.Dimension];
			
			// copy axis information
			for(int i=0;i<headCRS.Dimension;i++)
			{
				_axisInfo[i]=_headCRS.GetAxis(i);
			}
			int offset=headCRS.Dimension;
			for (int i=0;i<tailCRS.Dimension;i++)
			{
				_axisInfo[i+offset]=_tailCRS.GetAxis(i);
			}
		}

		#region Implementation of ICompoundCoordinateSystem
		/// <summary>
		/// Gets the axis information for the specified dimension.
		/// </summary>
		/// <param name="dimension"></param>
		/// <returns></returns>
		public override IAxisInfo GetAxis(int dimension)
		{
			return _axisInfo[dimension];
		}

		

		/// <summary>
		/// Dimension of the coordinate system.
		/// </summary>
		public override int Dimension
		{
			get
			{
				return _headCRS.Dimension + _tailCRS.Dimension;
				//TODO:
				//return -1;
			}
		}

		/// <summary>
		/// Gets second sub-coordinate system.
		/// </summary>
		public ICoordinateSystem TailCS
		{
			get
			{
				return _tailCRS;
			}
		}

		

		/// <summary>
		/// Gets first sub-coordinate system.
		/// </summary>
		public ICoordinateSystem HeadCS
		{
			get
			{
				return _headCRS;
			}
		}
		#endregion

	}
}
