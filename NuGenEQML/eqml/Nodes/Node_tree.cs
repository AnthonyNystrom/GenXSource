namespace Nodes
{
    using Attrs;
    using Rendering;
    using Boxes;
    using Nodes;
    
    using Fonts;
    using Facade;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Globalization;
    using System.Xml;

    public partial class Node
    {
        public void EmbraceParent()
        {
            if (firstChild != null)
            {
                NodesList list = GetChildrenNodes();
                for (Node n = list.Next(); n != null; n = list.Next())
                {
                    n.parent_ = this;
                    if (n.firstChild != null)
                    {
                        n.EmbraceParent();
                    }
                }
            }
        }

        public void UpdateLevel()
        {
            if (firstChild != null)
            {
                NodesList list = GetChildrenNodes();
                for (Node node = list.Next(); node != null; node = list.Next())
                {
                    node.level = level + 1;
                    if (node.firstChild != null)
                    {
                        node.UpdateLevel();
                    }
                }
            }
        }

        public void UpdateChildrenIndices()
        {
            NodesList list = GetChildrenNodes();
            Node node = list.Next();
            int index = 0;
            while (node != null)
            {
                node.childIndex = index;
                node = list.Next();
                index++;
            }
            numChildren = index;
        }

    }
}