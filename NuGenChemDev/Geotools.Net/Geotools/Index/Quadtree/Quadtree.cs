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
using System.Collections;
using Geotools.Geometries;
#endregion

namespace Geotools.Index.Quadtree
{
	
	/**
	 * A Quadtree is a spatial index structure for efficient querying
	 * of 2D rectangles.  If other kinds of spatial objects
	 * need to be indexed they can be represented by their
	 * envelopes
	 * <p>
	 * The quadtree structure is used to provide a primary filter
	 * for range rectangle querys.  The query() method returns a list of
	 * all objects which <i>may</i> intersect the query rectangle.  Note that
	 * it may return objects which do not in fact intersect.
	 * A secondary filter is required to test for exact intersection.
	 * Of course, this secondary filter may consist of other tests besides
	 * intersection, such as testing other kinds of spatial relationships.
	 *
	 * <p>
	 * This implementation does not require specifying the extent of the inserted
	 * items beforehand.  It will automatically expand to accomodate any extent
	 * of dataset.
	 * <p>
	 * This data structure is also known as an <i>MX-CIF quadtree</i>
	 * following the usage of Samet and others.
	 */
	internal class Quadtree: ISpatialIndex
	{
		private Root _root;

		//  Statistics
		//
		// minExtent is the minimum envelope extent of all items
		// inserted into the tree so far. It is used as a heuristic value
		// to construct non-zero envelopes for features with zero X and/or Y extent.
		// Start with a non-zero extent, in case the first feature inserted has
		// a zero extent in both directions.  This value may be non-optimal, but
		// only one feature will be inserted with this value.
		private double _minExtent = 1.0;

		#region Constructors
		public Quadtree()
		{
			_root = new Root();
		}

		#endregion

		#region Methods
		//public Quad getRoot() { return root; }
		public int Depth()
		{
			//I don't think it's possible for root to be null. Perhaps we should
			//remove the check. [Jon Aquino]
			if ( _root != null ) return _root.Depth();
			return 0;
		}

		public int Size()
		{
			if ( _root != null ) return _root.Size();
			return 0;
		}

		public void Insert(Envelope itemEnv, object item)
		{
			CollectStats(itemEnv);
			Envelope insertEnv = EnsureExtent(itemEnv, _minExtent);
			_root.Insert(insertEnv, item);
		}

		public ArrayList Query(Envelope searchEnv)
		{
			// the items that are matched are the items in quads which
			// overlap the search envelope
			ArrayList foundItems = new ArrayList();
			_root.AddAllItemsFromOverlapping(searchEnv, foundItems);
			return foundItems;
		}

		/// <summary>
		/// Returns a list of all items in the Quadtree.
		/// </summary>
		/// <returns></returns>
		public ArrayList QueryAll()
		{
			ArrayList foundItems = new ArrayList();
			_root.AddAllItems(foundItems);
			return foundItems;
		}

		private void CollectStats(Envelope itemEnv)
		{
			double delX = itemEnv.Width;
			if (delX < _minExtent && delX > 0.0)
			{
				_minExtent = delX;
			}

			double delY = itemEnv.Width;
			if (delY < _minExtent && delY > 0.0)
			{
				_minExtent = delY;
			}
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Ensure that the envelope for the inserted item has non-zero extents.
		/// Use the current minExtent to pad the envelope, if necessary.
		/// </summary>
		/// <param name="itemEnv"></param>
		/// <param name="minExtent"></param>
		/// <returns></returns>
		public static Envelope EnsureExtent( Envelope itemEnv, double minExtent )
		{
			//The names "ensureExtent" and "minExtent" are misleading -- sounds like
			//this method ensures that the extents are greater than minExtent.
			//Perhaps we should rename them to "ensurePositiveExtent" and "defaultExtent".
			//[Jon Aquino]
			double minx = itemEnv.MinX;
			double maxx = itemEnv.MaxX;
			double miny = itemEnv.MinY;
			double maxy = itemEnv.MaxY;
			// has a non-zero extent
			if (minx != maxx && miny != maxy) return itemEnv;

			// pad one or both extents
			if (minx == maxx) 
			{
				minx = minx - minExtent / 2.0;
				maxx = minx + minExtent / 2.0;
			}
			if (miny == maxy) 
			{
				miny = miny - minExtent / 2.0;
				maxy = miny + minExtent / 2.0;
			}
			return new Envelope(minx, maxx, miny, maxy);
		}
		#endregion

	}

}
