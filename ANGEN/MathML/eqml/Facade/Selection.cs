namespace Facade
{
    using Nodes;
    using System;

    public class Selection
    {
        public Selection()
        {
            this.nodesList = null;
            
            this.parent = null;
            this.caret = 0;
            this.literalLength = 0;
            this.swap = false;
            this.nodesList = new NodesList();
        }

        public void Add(Node node)
        {
            this.nodesList.Add(node);
        }

        public bool NotEmpty()
        {
            bool notEmpty = false;
            try
            {
                if ((((this.literalLength >= 0) && (this.caret >= 0)) && 
                    ((this.parent != null) && (this.nodesList != null))) && (this.nodesList.Count >= 0))
                {
                    return true;
                }
            }
            catch
            {
                notEmpty = false;
            }
            return notEmpty;
        }


        public Node First
        {
            get
            {
                if ((this.nodesList != null) && (this.nodesList.Count > 0))
                {
                    return this.nodesList.Get(0);
                }
                return null;
            }
        }

        public Node Last
        {
            get
            {
                if ((this.nodesList != null) && (this.nodesList.Count > 0))
                {
                    return this.nodesList.Get(this.nodesList.Count - 1);
                }
                return null;
            }
        }
        
        public NodesList nodesList;
        public Node parent;
        public int caret;
        public int literalLength;
        public bool swap;
    }
}

