namespace Facade
{
    using Attrs;
    using Nodes;
    using System;

    public class NodesInfo
    {
        public NodesInfo(Node rootNode, Node selectedNode, int selectedNode_Caret, Node lastSelectedNode)
        {
            this.selected_ = null;
            this.root_ = null;
            this.lastSel_ = null;
            this.mark_ = 0;
            this.root_ = this.create(rootNode, selectedNode, lastSelectedNode);
            this.root_.UpdateLevel();
            this.mark_ = selectedNode_Caret;
        }

        private Node create(Node node, Node selectedNode, Node lastSelectedNode)
        {
            Node n = new Node();
            
            n.tagDeleted = node.tagDeleted;
            n.tokenType = node.tokenType;
            n.xmlTagName = node.xmlTagName;
            n.namespaceURI = node.namespaceURI;
            n.isVisible = node.isVisible;
            n.isGlyph = node.isGlyph;
            n.skip = node.skip;
            
            n.literalText = node.literalText;
            n.literalCaret = node.literalCaret;
            n.literalStart = node.literalStart;
            n.yOffset = node.yOffset;
            n.displayStyle = node.displayStyle;
            
            n.glyph = node.glyph;
            
            n.scriptLevel_ = node.scriptLevel_;
            
            n.type_ = node.type_;
            if (node.attrs != null)
            {
                n.attrs = new AttributeList();
                node.attrs.CopyTo(n.attrs);
            }
            n.FontStyle = node.FontStyle;
            if (node.style_ != null)
            {
                n.style_ = new StyleAttributes();
                node.style_.CopyTo(n.style_);
            }
            if (node == selectedNode)
            {
                this.selected_ = n;
            }
            if (node == lastSelectedNode)
            {
                this.lastSel_ = n;
            }
            if (node.HasChildren())
            {
                NodesList list = node.GetChildrenNodes();
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    Node c = list.Get(i);
                    Node child = this.create(c, selectedNode, lastSelectedNode);
                    if (child != null)
                    {
                        n.AdoptChild(child);
                    }
                }
            }
            return n;
        }


        public Node RootNode
        {
            get
            {
                return this.root_;
            }
        }

        public Node SelectedNode
        {
            get
            {
                return this.selected_;
            }
        }

        public int Mark
        {
            get
            {
                return this.mark_;
            }
        }

        public Node LastSelected
        {
            get
            {
                return this.lastSel_;
            }
        }


        private Node selected_;
        private Node root_;
        private Node lastSel_;
        private int mark_;
    }
}

