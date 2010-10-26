using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace NuGenSVisualLib
{
    interface IQuadTreeItem
    {
        Point Center { get; }
        Point Origin { get; }
        Size Dimensions { get; }
        int Radius { get; }
    }

    class QuadTree<T> : QuadTreeNode<T> where T : IQuadTreeItem
    {
        uint itemsCount;

        public QuadTree(int width, int height)
            : base(new Point(0,0), new Size(width, height))
        {
            itemsCount = 0;
        }

        public override bool Insert(T item)
        {
            if (base.Insert(item))
            {
                itemsCount++;
                return true;
            }
            return false;
        }

        public override bool Remove(T item)
        {
            if (base.Remove(item))
            {
                itemsCount--;
                return true;
            }
            return false;
        }

        public override void Clear()
        {
            base.Clear();
            itemsCount = 0;
        }

        public uint ItemsCount
        {
            get { return itemsCount; }
        }
    }

    class QuadTreeNode<T> where T : IQuadTreeItem
    {
        public const uint maxItemsPerNode = 4;

        QuadTreeNode<T>[] subNodes;
        List<T> nodeItems;

        Point position, positionUpper;
        Size dimensions;

        public QuadTreeNode(Point position, Size dimensions)
        {
            this.position = position;
            this.dimensions = dimensions;
            this.positionUpper = position + dimensions;

            subNodes = new QuadTreeNode<T>[4];
            nodeItems = new List<T>();
        }

        public QuadTreeNode(QuadTreeNode<T> parent, int subNodeNum)
        {
            this.dimensions = new Size(parent.dimensions.Width / 2, parent.dimensions.Height / 2);
            switch (subNodeNum)
            {
                case 0:
                    this.position = parent.position;
                    this.position.Y += dimensions.Height;
                    break;
                case 1:
                    this.position = parent.position + dimensions;
                    break;
                case 2:
                    this.position = parent.position;
                    break;
                case 3:
                    this.position = parent.position;
                    this.position.X += dimensions.Width;
                    break;
            }
            this.positionUpper = position + dimensions;

            subNodes = new QuadTreeNode<T>[4];
            nodeItems = new List<T>();
        }

        public virtual bool Insert(T item)
        {
            // try to put into local items first
            if (nodeItems.Count < maxItemsPerNode)
            {
                nodeItems.Add(item);
                return true;
            }
            else
            {
                // add into subnode
                Size halfDim = new Size(dimensions.Width / 2, dimensions.Height / 2);
                int startNode = -1, endNode = -2;
                if (item.Origin.X < position.X + halfDim.Width)
                {
                    if (item.Origin.Y < position.Y + halfDim.Height)
                        startNode = 2;
                    else if (item.Origin.Y < position.Y + dimensions.Height)
                        startNode = 0;
                    else
                        startNode = -1;
                }
                else if (item.Origin.X < position.X + dimensions.Width)
                {
                    if (item.Origin.Y < position.Y + halfDim.Height)
                        startNode = 4;
                    else if (item.Origin.Y < position.Y + dimensions.Height)
                        startNode = 1;
                    else
                        startNode = -1;
                }

                if (item.Origin.X + item.Dimensions.Width < position.X + halfDim.Width)
                {
                    if (item.Origin.Y + item.Dimensions.Height < position.Y + halfDim.Height)
                        endNode = 2;
                    else if (item.Origin.Y + item.Dimensions.Height < position.Y + dimensions.Height)
                        endNode = 0;
                    else
                        endNode = -2;
                }
                else if (item.Origin.X + item.Dimensions.Width < position.X + dimensions.Width)
                {
                    if (item.Origin.Y < position.Y + halfDim.Height)
                        endNode = 4;
                    else if (item.Origin.Y + item.Dimensions.Height < position.Y + dimensions.Height)
                        endNode = 1;
                    else
                        endNode = -2;
                }
                else
                    endNode = -2;

                if (startNode == endNode)
                {
                    // is within a single sub-node - add to that
                    if (subNodes[startNode] == null)
                        subNodes[startNode] = new QuadTreeNode<T>(this, startNode);
                    subNodes[startNode].Insert(item);
                    return true;
                }
                else
                {
                    // add anyways - too big for sub-nodes
                    nodeItems.Add(item);
                    return true;
                }
            }
        }

        public virtual bool Remove(T item)
        {
            return false;
        }

        public virtual void Clear()
        {
            if (nodeItems != null)
                nodeItems.Clear();
            for (int i = 0; i < 4; i++)
            {
                if (subNodes[i] != null)
                    subNodes[i].Clear();
            }
        }

        public void CheckRadius(int x, int y, ref List<T> items)
        {
            // check items
            foreach (T item in nodeItems)
            {
                int xDist = item.Center.X - x;
                int yDist = item.Center.Y - y;
                double length = Math.Sqrt((xDist * xDist) + (yDist * yDist));
                if (length < item.Radius)
                {
                    // check absolute dimensions now
                    if (x > item.Origin.X && y > item.Origin.Y &&
                        x < item.Origin.X + item.Dimensions.Width &&
                        y < item.Origin.Y + item.Dimensions.Height)
                    {
                        if (items == null)
                            items = new List<T>();
                        items.Add(item);
                    }
                }
            }
            // check sub-nodes
            Size halfDim = new Size(dimensions.Width / 2, dimensions.Height / 2);
            int subNode = -1;
            if (x < position.X + halfDim.Width)
            {
                if (y < position.Y + halfDim.Height)
                    subNode = 2;
                else
                    subNode = 0;
            }
            else
            {
                if (y < position.Y + halfDim.Height)
                    subNode = 3;
                else
                    subNode = 1;
            }
            if (subNodes[subNode] != null)
                subNodes[subNode].CheckRadius(x, y, ref items);
        }

//        public T[] CheckRadius(T item, int radius)
//        {
//            return null;
//        }

//        public T[] CheckItemCollisions(T item)
//        {
//            return null;
//        }
    }
}