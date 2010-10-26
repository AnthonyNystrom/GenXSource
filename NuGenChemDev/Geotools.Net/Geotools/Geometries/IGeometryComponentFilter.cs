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

#region Using
using System;
#endregion

namespace Geotools.Geometries
{
	/// <summary>A Component filter can either record information 
	/// about each Component or change the Component in some way. Component filters 
	/// implement the interface ComponentFilter.
	/// </summary>
	/// <remarks>
	/// Geometry classes support the concept of applying a Component filter to every 
	/// Component in the Geometry. (ComponentFilter is an example of 
	/// the Gang-of-Four Visitor pattern). Component filters can be used to 
	/// implement such things as Component transformations, centroid and envelope 
	/// computation, and many other functions.
	///</remarks>
	public interface IGeometryComponentFilter
	{
		/// <summary>
		/// Performs an operation with or on this component.
		/// </summary>
		/// <param name="geom">The geometry to which the filter is applied.</param>
		void Filter(Geometry geom);
	}
}
