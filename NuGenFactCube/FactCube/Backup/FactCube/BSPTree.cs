using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;

namespace Genetibase.FactCube
{
	/// <summary>
	/// BSP tree
	/// </summary>
	/// <typeparam name="T">Item type</typeparam>
	public class BSPTree
	{
		public delegate double PartitionHandler( double length, Vector3D normal );

		private BSPTree parent;
		private Plane partition;
		private List<IBSPItem> items = new List<IBSPItem>();
		private BSPTree front;
		private BSPTree back;
		private BoundingBox bounds;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="normal">Normal</param>
		public BSPTree( IList<IBSPItem> items ) : this( null, items, null )
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="normal">Normal</param>
		public BSPTree( IList<IBSPItem> items, PartitionHandler partitionHandler ) : this( null, items, partitionHandler )
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="plane">Plane</param>
		/// <param name="normal">Normal</param>
		protected BSPTree( BSPTree parent, IList<IBSPItem> items, PartitionHandler partitionHandler )
		{
			this.parent = parent;
			bounds = GetBounds( items );
			partition = DeterminePartition( items, bounds, partitionHandler );

			if ( items.Count == 1 )
			{
				this.items.Add( items[ 0 ] );
			}
			else
			{
				List<IBSPItem> backItems = new List<IBSPItem>();
				List<IBSPItem> frontItems = new List<IBSPItem>();

				foreach ( IBSPItem item in items )
				{
					Add( item, backItems, frontItems );
				}

				if ( backItems.Count > 0 ) back = new BSPTree( this, backItems, partitionHandler );
				if ( frontItems.Count > 0 ) front = new BSPTree( this, frontItems, partitionHandler );
			}
		}

		/// <summary>
		/// Get bounds
		/// </summary>
		/// <param name="items">Items</param>
		/// <returns>Bounds</returns>
		protected virtual BoundingBox GetBounds( IList<IBSPItem> items )
		{
            BoundingBox bounds = new BoundingBox();

			foreach ( IBSPItem item in items )
			{
                bounds.Merge( item.Bounds );
			}

            return bounds;
		}

		/// <summary>
		/// Determine partition
		/// </summary>
		/// <param name="items">Items</param>
		/// <param name="bounds">Bounds</param>
		/// <returns>Partition</returns>
		protected virtual Plane DeterminePartition( IList<IBSPItem> items, BoundingBox bounds, PartitionHandler partitionHandler )
		{
			Vector3D size = bounds.Max - bounds.Min;
			double length;
			Vector3D normal;

			if ( size.Z >= size.X && size.Z >= size.Y )
			{
				length = size.Z;
				normal = new Vector3D( 0, 0, 1 );
			}
			else if ( size.X >= size.Y && size.X >= size.Z )
			{
				length = size.X;
				normal = new Vector3D( 1, 0, 0 );
			}
			else
			{
				length = size.Y;
				normal = new Vector3D( 0, 1, 0 );
			}

			double position = ( partitionHandler != null ) ? partitionHandler( length, normal ) : 0.5;

			return new Plane( bounds.Min + ( size * position ), normal );
		}

		/// <summary>
		/// Add item
		/// </summary>
		/// <param name="item">Item</param>
		protected void Add( IBSPItem item, IList<IBSPItem> backItems, IList<IBSPItem> frontItems )
		{
			IBSPItem frontItem;
			IBSPItem backItem;

			switch ( item.Intersects( partition, out frontItem, out backItem ) )
			{
				case PlaneIntersectionType.Back: backItems.Add( item ); break;
				case PlaneIntersectionType.Front: frontItems.Add( item ); break;
				case PlaneIntersectionType.Intersecting:
				{
					if ( frontItem != null && backItem != null )
					{
						backItems.Add( frontItem );
						frontItems.Add( backItem );
					}

					break;
				}
			}
		}

		/// <summary>
		/// Get parent
		/// </summary>
		public BSPTree Parent { get { return parent; } }

		/// <summary>
		/// Get bounds
		/// </summary>
		public BoundingBox Bounds { get { return bounds; } }

		/// <summary>
		/// Get partition plane
		/// </summary>
		public Plane Partition { get { return partition; } }

		/// <summary>
		/// Get back tree
		/// </summary>
		public BSPTree Back { get { return back; } }

		/// <summary>
		/// Get front tree
		/// </summary>
		public BSPTree Front { get { return front; } }

		/// <summary>
		/// Get items
		/// </summary>
		public IList<IBSPItem> Items { get { return items; } }

		/// <summary>
		/// Find leaf closest to point
		/// </summary>
		/// <param name="point">Point</param>
		public BSPTree FindLeaf( Point3D point )
		{
			BSPTree last = this;
			BSPTree tree = this;

			while ( tree != null )
			{
				tree = tree.FindNext( point );
				if ( tree != null ) last = tree;
			}

			return last;
		}

		/// <summary>
		/// Find leaf closest to point
		/// </summary>
		/// <param name="point">Point</param>
		public BSPTree FindNext( Point3D point )
		{
			switch ( partition.Classify( point ) )
			{
				case Halfspace.Negative: return back;
				case Halfspace.Positive: return front;
			}

			return null;
		}
	}
}