using System;
using System.Collections;
using System.Collections.Generic;
using Next2Friends.Swoosher.Media3D;

namespace Next2Friends.Swoosher
{
    /// <summary>
    /// BSP tree
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public class BSPTree<T> where T : IBSPItem
    {
        public delegate double PartitionHandler( double length, Vector3D normal );

        private BSPTree<T> parent;
        private Plane partition;
        private List<T> items = new List<T>();
        private BSPTree<T> front;
        private BSPTree<T> back;
        private BoundingBox bounds;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="normal">Normal</param>
        public BSPTree( ICollection<T> items )
            : this( null, items, null )
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="normal">Normal</param>
        public BSPTree( ICollection<T> items, PartitionHandler partitionHandler )
            : this( null, items, partitionHandler )
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="plane">Plane</param>
        /// <param name="normal">Normal</param>
        protected BSPTree( BSPTree<T> parent, ICollection<T> items, PartitionHandler partitionHandler )
        {
			this.parent = parent;
            bounds = GetBounds( items );
            partition = DeterminePartition( items, bounds, partitionHandler );

			if ( items.Count == 1 )
			{
				this.items.AddRange( items );
			}
			else
			{
				var backItems = new List<T>();
				var frontItems = new List<T>();

				foreach ( var item in items ) Add( item, backItems, frontItems );

				if ( backItems.Count > 0 ) back = new BSPTree<T>( this, backItems, partitionHandler );
				if ( frontItems.Count > 0 ) front = new BSPTree<T>( this, frontItems, partitionHandler );
			}
        }

        /// <summary>
        /// Get bounds
        /// </summary>
        /// <param name="items">Items</param>
        /// <returns>Bounds</returns>
        protected virtual BoundingBox GetBounds( IEnumerable<T> items )
        {
            BoundingBox bounds = new BoundingBox();

            foreach ( var item in items )
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
        protected virtual Plane DeterminePartition( IEnumerable<T> items, BoundingBox bounds, PartitionHandler partitionHandler )
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

            float position = (float)( ( partitionHandler != null ) ? partitionHandler( length, normal ) : 0.5 );

            return new Plane( (Vector3D)( bounds.Min + ( size * position ) ), normal );
        }

        /// <summary>
        /// Add item
        /// </summary>
        /// <param name="item">Item</param>
        protected void Add( T item, IList<T> backItems, IList<T> frontItems )
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
							backItems.Add( (T)frontItem );
							frontItems.Add( (T)backItem );
						}
						else backItems.Add( item );

                        break;
                    }
            }
        }

        /// <summary>
        /// Get parent
        /// </summary>
        public BSPTree<T> Parent { get { return parent; } }

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
        public BSPTree<T> Back { get { return back; } }

        /// <summary>
        /// Get front tree
        /// </summary>
        public BSPTree<T> Front { get { return front; } }

        /// <summary>
        /// Get items
        /// </summary>
        public IList<T> Items { get { return items; } }

        /// <summary>
        /// Find leaf closest to point
        /// </summary>
        /// <param name="point">Point</param>
		public BSPTree<T> FindLeaf( Point3D point )
        {
            BSPTree<T> last = this;
            BSPTree<T> tree = this;

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
		public BSPTree<T> FindNext( Point3D point )
        {
            switch ( partition.Classify( point ) )
            {
                case Halfspace.Negative: return back;
                case Halfspace.Positive: return front;
            }

            return null;
        }

        #region IEnumerable<T> Members

        /// <summary>
        /// Get item enumerator based on viewpoint
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
		public IEnumerable<T> Select( Point3D viewpoint )
        {
            Halfspace halfspace = partition.Classify( viewpoint );

            if ( halfspace == Halfspace.Negative )
            {
                if ( front != null ) foreach ( var item in front.Select( viewpoint ) ) yield return item;
            }
            else if ( back != null ) foreach ( var item in back.Select( viewpoint ) ) yield return item;

            foreach ( var item in items ) yield return item;

            if ( halfspace == Halfspace.Negative )
            {
                if ( back != null ) foreach ( var item in back.Select( viewpoint ) ) yield return item;
            }
            else if ( front != null ) foreach ( var item in front.Select( viewpoint ) ) yield return item;
        }

        /// <summary>
        /// Get item enumerator
        /// </summary>
        /// <returns>New enumerator</returns>
        public IEnumerable<T> Select()
        {
            if ( back != null ) foreach ( var item in back.Select() ) yield return item;
            if ( items != null ) foreach ( var item in items ) yield return item;
            if ( front != null ) foreach ( var item in front.Select() ) yield return item;
        }

        #endregion
    }
}