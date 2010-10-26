using System.Collections;
using System.Collections.Generic;
using Microsoft.DirectX;

namespace Genetibase.VisUI.Maths
{
    public class PureQuadTreeLeafEnumerator : IEnumerator<PureQuadTreeNode>
    {
        PureQuadTreeNode node;
        readonly PureQuadTree root;

        Stack<byte> traceStack;
        byte currentTrace;

        /// <summary>
        /// Initializes a new instance of the PureQuadTreeLeafEnumerator class.
        /// </summary>
        /// <param name="root"></param>
        public PureQuadTreeLeafEnumerator(PureQuadTree root)
        {
            this.root = root;
            traceStack = new Stack<byte>();
        }

        public bool IsLeaf
        {
            get { return node == null || node.Children == null; }
        }

        #region IEnumerator<PureQuadTreeNode> Members

        PureQuadTreeNode IEnumerator<PureQuadTreeNode>.Current
        {
            get { return node; }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        { }
        #endregion

        #region IEnumerator Members

        public bool MoveNext()
        {
            // FIXME: [13-01-2007] PQT Enumeration

            // locate next leaf
            if (IsLeaf)
            {
                if (TrySilbing())
                    return true;
                while (MoveUpBranch())
                {
                    if (TrySilbing())
                        return true;
                }
                // cant go up any further!
                return false;
            }

            // find next by branching down
            while (BranchDown())
            {
                if (IsLeaf)
                    return true;
            }
            return false;
        }

        private bool BranchDown()
        {
            if (currentTrace > 3)
                return false;
            traceStack.Push(currentTrace);
            currentTrace = 0;
            node = node.Children[currentTrace];
            return true;
        }

        private bool MoveUpBranch()
        {
            if (traceStack.Count == 0)
                return false;
            currentTrace = traceStack.Pop();
            node = node.Parent;
            return true;
        }

        private bool TrySilbing()
        {
            if (node == null)
            {
                node = root;
                return true;
            }
            if (currentTrace < 3 && node.Parent != null)
            {
                node = node.Parent.Children[++currentTrace];
                if (!IsLeaf)
                {
                    while (BranchDown())
                    {
                        if (IsLeaf)
                            return true;
                    }
                }
                return true;
            }
            return false;
        }

        public void Reset()
        {
            node = root;
        }

        public object Current
        {
            get { return ((IEnumerator<PureQuadTreeNode>)this).Current; }
        }
        #endregion
    }

    public class PureQuadTree : PureQuadTreeNode, IEnumerable<PureQuadTreeNode>
    {
        public PureQuadTree(Vector2 location, Vector2 size)
            : base(location, size, 0, null, 0)
        { }

        public IEnumerator<PureQuadTreeNode> GetLeafEnumerator()
        {
            return new PureQuadTreeLeafEnumerator(this);
        }

        #region IEnumerable<PureQuadTreeNode> Members

        IEnumerator<PureQuadTreeNode> IEnumerable<PureQuadTreeNode>.GetEnumerator()
        {
            return new PureQuadTreeLeafEnumerator(this);
        }
        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<PureQuadTreeNode>) this).GetEnumerator();
        }
        #endregion
    }

    public class PureQuadTreeNode
    {
        private readonly PureQuadTreeNode parent;
        private PureQuadTreeNode[] children;
        private PureQuadTreeNode[] childrenCached;
        private readonly ulong code;

        private readonly Vector2 location;
        private readonly Vector2 centre;
        private readonly Vector2 size;
        private readonly ushort level;
        private ushort depth;

        /// <summary>
        /// Initializes a new instance of the PureQuadTreeNode class.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="size"></param>
        /// <param name="level"></param>
        /// <param name="parent"></param>
        /// <param name="childNum"></param>
        public PureQuadTreeNode(Vector2 location, Vector2 size, ushort level,
                                PureQuadTreeNode parent, uint childNum)
        {
            this.location = location;
            this.parent = parent;
            this.size = size;
            this.level = level;
            depth = 0;

            centre = location + (size * 0.5f);

            if (parent != null)
                code = parent.code | (childNum << (level * 2));
            else
                code = childNum;
        }

        #region Properties

        public PureQuadTreeNode[] Children
        {
            get { return children; }
        }

        public ulong Code
        {
            get { return code; }
        }

        public Vector2 Location
        {
            get { return location; }
        }

        public Vector2 Centre
        {
            get { return centre; }
        }

        public Vector2 Size
        {
            get { return size; }
        }

        public ushort Level
        {
            get { return level; }
        }

        public PureQuadTreeNode Parent
        {
            get { return parent; }
        }

        public ushort Depth
        {
            get { return depth; }
        }

        public ushort ChildNum
        {
            get { return (ushort)(code >> (level * 2)); }
        }
        #endregion

        public void Fork(ushort numLevels, bool useCache)
        {
            Fork(numLevels, useCache, false); 
        }

        protected void Fork(ushort numLevels, bool useCache, bool branching)
        {
            if (childrenCached == null)
            {
                Vector2 childSize = size * 0.5f;

                children = new PureQuadTreeNode[4];
                children[0] = new PureQuadTreeNode(location, childSize, (ushort)(level + 1), this, 0);
                children[1] = new PureQuadTreeNode(new Vector2(location.X + childSize.X, location.Y), childSize, (ushort)(level + 1), this, 1);
                children[2] = new PureQuadTreeNode(new Vector2(location.X, location.Y + childSize.Y), childSize, (ushort)(level + 1), this, 2);
                children[3] = new PureQuadTreeNode(location + childSize, childSize, (ushort)(level + 1), this, 3);
            }
            else if (useCache)
                children = childrenCached;

            if (numLevels > 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    children[i].Fork((ushort)(numLevels - 1), useCache);
                }
            }
            else if (useCache && childrenCached != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    children[i].Chop(useCache);
                }
            }

            depth = numLevels;
        }

        public void Chop(bool cache)
        {
            if (children != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    children[i].Chop(cache);
                }
                if (cache)
                    childrenCached = children;
                children = null;
            }
            depth = 0;
        }

        protected void IncreaseDepth(ushort value)
        {
            depth += value;
            if (parent != null)
                parent.IncreaseDepth(value);
        }

        protected void DecreaseDepth(ushort value)
        {
            depth -= value;
            if (parent != null)
                parent.DecreaseDepth(value);
        }
    }
}