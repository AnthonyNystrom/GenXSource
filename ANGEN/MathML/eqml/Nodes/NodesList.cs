namespace Nodes
{
    using System;
    using System.Collections;

    public class NodesList
    {
        public NodesList()
        {
            this.count_ = 0;
            this.list_ = new ArrayList();
            this.iterator = 0;
            this.count_ = 0;
        }

        public void Reset()
        {
            this.iterator = 0;
        }

        public void Add(Node Node)
        {
            this.list_.Add(Node);
            this.count_++;
        }

        public Node Get(int n)
        {
            if (n < this.count_)
            {
                return (Node) this.list_[n];
            }
            return null;
        }

        public Node Next()
        {
            if (this.iterator < this.count_)
            {
                Node node = (Node) this.list_[this.iterator];
                this.iterator++;
                return node;
            }
            return null;
        }


        public int Count
        {
            get
            {
                if (this.list_ != null)
                {
                    return this.list_.Count;
                }
                return 0;
            }
        }


        private int iterator;
        private int count_;
        private ArrayList list_;
    }
}

