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
using System.Diagnostics;
using Geotools.Geometries;
#endregion

namespace Geotools.Index.Chain
{
	/// <summary>
	/// MonotoneChains are a way of partitioning the segments of an edge to
	/// allow for fast searching of intersections.
	/// </summary>
	/// <remarks>
	/// They have the following properties:
	/// <list type="bullet">
	/// <listheader><term>Items</term><description>Descriptions</description></listheader>
	/// <item><term>Property 1 means that there is no need to test pairs of segments from within
	/// the same monotone chain for intersection.</term><description>Your Description</description></item>
	/// <item><term>Property 2 allows
	/// binary search to be used to find the intersection points of two monotone chains.
	/// For many types of real-world data, these properties eliminate a large number of
	/// segment comparisons, producing substantial speed gains.</term><description>Your Description</description></item>
	/// </list>
	/// <para>One of the goals of this implementation of MonotoneChains is to be
	/// as space and time efficient as possible. One design choice that aids this
	/// is that a MonotoneChain is based on a subarray of a list of points.
	/// This means that new arrays of points (potentially very large) do not
	/// have to be allocated.
	/// </para>
	/// MonotoneChains support the following kinds of queries:
	/// <list type="table">
	/// <listheader><term>Items</term><description>Descriptions</description></listheader>
	/// <item><term>Envelope select</term><description>determine all the segments in the chain which
	/// intersect a given envelope</description></item>
	/// <item><term>Overlap</term><description>determine all the pairs of segments in two chains whose
	/// envelopes overlap</description></item>
	/// </list>
	/// <para>
	/// This implementation of MonotoneChains uses the concept of internal iterators
	/// to return the resultsets for the above queries.
	/// This has time and space advantages, since it
	/// is not necessary to build lists of instantiated objects to represent the segments
	/// returned by the query.
	/// However, it does mean that the queries are not thread-safe.
	///</para>
	///</remarks>
	internal class MonotoneChain
	{
		private Coordinates _pts;
		private int _start, _end;
		private Envelope _env = null;
		// these envelopes are created once and reused
		Envelope _env1 = new Envelope();
		Envelope _env2 = new Envelope();

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the MonotoneChain class.
		/// </summary>
		public MonotoneChain( Coordinates pts, int start, int end )
		{
			_pts    = pts;
			_start  = start;
			_end    = end;
		} // public MonotoneChain( Coordinates pts, int start, int end )

		#endregion

		#region Properties
		public int StartIndex 
		{
			get
			{
				return _start;
			}
		}
		public int EndIndex  
		{ 
			get
			{
				return _end; 
			}
		}
		#endregion

		#region Methods

		/// <summary>
		/// Returns the envelope
		/// </summary>
		/// <returns></returns>
		public Envelope GetEnvelope()
		{
			if (_env == null) 
			{
				Coordinate p0 = _pts[_start];
				Coordinate p1 = _pts[_end];
				_env = new Envelope(p0, p1);
			}
			return _env;
		} // public Envelope GetEnvelope()

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="ls"></param>
		public void GetLineSegment( int index, ref LineSegment ls )
		{
			if ( ls == null )
			{
				throw new ArgumentNullException( "ls", "LineSegment parameter is null when object was expected");
			}
			if ( index < 0 || index >= _pts.Count )
			{
				throw new IndexOutOfRangeException( "Coordinates index was out of range:"+index.ToString() );
			}
			ls.P0 = _pts[index];
			ls.P1 = _pts[index + 1];
		} // public void GetLineSegment( int index, ref LineSegment ls )

		/// <summary>
		/// Return the subsequence of coordinates forming this chain.
		/// Allocates a new array to hold the Coordinates
		/// </summary>
		/// <returns></returns>
		public Coordinates GetCoordinates()
		{
			Coordinates coords = new Coordinates();
			for (int i = _start; i <= _end; i++) 
			{
				coords.Add( _pts[i] );
			}
			return coords;
		} // public Coordinates GetCoordinates()

		/// <summary>
		/// Determine all the line segments in the chain whose envelopes overlap
		/// the searchEnvelope, and process them
		/// </summary>
		/// <param name="searchEnv"></param>
		/// <param name="mcs"></param>
		public void Select( Envelope searchEnv, MonotoneChainSelectAction mcs )
		{
			ComputeSelect( searchEnv, _start, _end, mcs );
		} // public void Select( Envelope searchEnv, MonotoneChainSelectAction mcs )

		private void ComputeSelect(
			Envelope searchEnv,
			int start0, int end0,
			MonotoneChainSelectAction mcs )
		{
			Coordinate p0 = _pts[start0];
			Coordinate p1 = _pts[end0];
			_env1.Initialize( p0, p1 );

			//Trace.WriteLine( "trying:" + p0.ToString() + p1.ToString() + " [ " + start0.ToString() + ", " + end0.ToString() + " ]");

			// terminating condition for the recursion
			if (end0 - start0 == 1) 
			{
				//Trace.WriteLine("ComputeSelect:" + p0.ToString() + p1.ToString() );
				mcs.Select( this, start0 );
				return;
			}

			// nothing to do if the envelopes don't overlap
			if ( !searchEnv.Intersects( _env1 ) )
			{
				return;
			}

			// the chains overlap, so split each in half and iterate  (binary search)
			int mid = ( start0 + end0 ) / 2;

			// Assert: mid != start or end (since we checked above for end - start <= 1)
			// check terminating conditions before recursing
			if ( start0 < mid ) 
			{
				ComputeSelect( searchEnv, start0, mid, mcs );
			}
			if ( mid < end0 ) 
			{
				ComputeSelect( searchEnv, mid, end0, mcs );
			}
		} // private void ComputeSelect(...

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mc"></param>
		/// <param name="mco"></param>
		public void ComputeOverlaps( MonotoneChain mc, MonotoneChainOverlapAction mco )
		{
			ComputeOverlaps( _start, _end, mc, mc.StartIndex, mc.EndIndex, mco );
		} // public void ComputeOverlaps( MonotoneChain mc, MonotoneChainOverlapAction mco )


		private void ComputeOverlaps(
			int start0, int end0,
			MonotoneChain mc,
			int start1, int end1,
			MonotoneChainOverlapAction mco)
		{
			Coordinate p00 = _pts[start0];
			Coordinate p01 = _pts[end0];
			Coordinate p10 = mc.GetCoordinates()[start1];
			Coordinate p11 = mc.GetCoordinates()[end1];
			//Trace.WriteLine( "ComputeIntersectsForChain:" + p00.ToString() + p01.ToString() + p10.ToString() + p11.ToString() );

			// terminating condition for the recursion
			if ( end0 - start0 == 1 && end1 - start1 == 1 ) 
			{
				mco.Overlap( this, start0, mc, start1 );
				return;
			}
			// nothing to do if the envelopes of these chains don't overlap
			_env1.Initialize( p00, p01 );
			_env2.Initialize( p10, p11 );
			if ( !_env1.Intersects( _env2 ) ) return;

			// the chains overlap, so split each in half and iterate  (binary search)
			int mid0 = ( start0 + end0 ) / 2;
			int mid1 = ( start1 + end1 ) / 2;

			// Assert: mid != start or end (since we checked above for end - start <= 1)
			// check terminating conditions before recursing
			if ( start0 < mid0 ) 
			{
				if ( start1 < mid1 )
				{
					ComputeOverlaps( start0, mid0, mc, start1,  mid1, mco );
				}
				if (mid1 < end1)
				{
					ComputeOverlaps( start0, mid0, mc, mid1,    end1, mco );
				}
			} // if ( start0 < mid0 )
			if ( mid0 < end0 ) 
			{
				if ( start1 < mid1 )
				{
					ComputeOverlaps(mid0,   end0, mc, start1,  mid1, mco);
				}
				if ( mid1 < end1 )   
				{
					ComputeOverlaps(mid0,   end0, mc, mid1,    end1, mco);
				}
			} // if ( mid0 < end0 )
		} // private void ComputeOverlaps(...
		#endregion

	} // public class MonotoneChain
}
