
#region Using
using System;
using System.Collections;
using Geotools.Geometries;
#endregion

namespace Geotools.Index
{
	/// <summary>
	/// Summary description for ISpatialIndex.
	/// </summary>
	internal interface ISpatialIndex
	{
		/// <summary>
		/// Adds a spatial item to the index with the given envelope
		/// </summary>
		/// <param name="itemEnv"></param>
		/// <param name="item"></param>
		void Insert(Envelope itemEnv, object item);

		/// <summary>
		/// Queries the index for all items whose envelopes intersect the given search envelope
		/// </summary>
		/// <param name="searchEnv">The envelope to query for.</param>
		/// <returns>A list of the items found by the query.</returns>
		ArrayList Query( Envelope searchEnv );			
	}
}
