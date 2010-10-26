namespace Facade
{
    using Attrs;
    using Facade;
    using Rendering;
    using JpegComment;
    using Boxes;
    using Nodes;
    using MathTable;
    using Operators;
    
    using Fonts;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml;

    public partial class NodesBuilder
    {
        public void insertMathML (string xml)
        {
            this.insertMathML (true, xml);
        }

        public void insertMathML (bool isInsert, string xml)
        {
            this.insertMathML (isInsert, xml, false);
        }

        public void insertMathML (string xml, bool isPaste)
        {
            this.insertMathML (true, xml, isPaste);
        }

        public void insertMathML (bool isInsert, string xml, bool isPaste)
        {
            try
            {
                this.tryAddMathXML (isInsert, xml, isPaste);
            }
            catch
            {
            }
            finally
            {
                try
                {
                    this.InsertHappened ();
                }
                catch
                {
                }
            }
        }

        private void tryAddMathXML (bool isInsert, string xml, bool isPaste)
        {
            string s = "";
            bool hasS = false;
            bool ok = false;
            try
            {
                if (this.HasSelection)
                {
                    xml = xml.Trim ();
                    if ((xml.IndexOf ("<math", 0, 5) != -1) &&
                        (((xml.IndexOf (" nugenCursor=\"") != -1) || (xml.IndexOf (" nugenCursor='") != -1)) ||
                         ((xml.IndexOf (" nugenCursorEnd=\"") != -1) || (xml.IndexOf (" nugenCursorEnd='") != -1))))
                    {
                        Selection selection = this.CaptureSelection ();
                        if ((selection != null))
                        {
                            XmlDocument doc = new XmlDocument ();
                            if (this.SaveToXml (doc, selection))
                            {
                                string outerxml = doc.OuterXml;
                                int startIndex = outerxml.IndexOf ("<math");
                                if (startIndex != -1)
                                {
                                    outerxml = outerxml.Substring (startIndex, outerxml.Length - startIndex);
                                    outerxml = outerxml.Trim ();
                                    if ((outerxml.IndexOf ("<math", 0, 5) != -1) &&
                                        (outerxml.Substring (outerxml.Length - 7, 7) == "</math>"))
                                    {
                                        s = outerxml;
                                        hasS = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            if (isInsert)
            {
                this.OnInsert (true);
            }

            bool emptyRow = false;
          
            Node selectedNode = this.GetCurrentlySelectedNode ();
            
            if (selectedNode == null)
            {
                return;
            }
            if (selectedNode.type_ == null)
            {
                return;
            }
            
            if (!this.IsEditable ())
            {
                return;
            }
            NodeClass nodeClass = this.GetNodeClass (selectedNode);
            selectedNode = this.GetCurrentlySelectedNode ();
            bool wasSplit = false;
            selectedNode = this.CarriageReturn (selectedNode, ref wasSplit);
            if (wasSplit)
            {
                this.SelectNode (selectedNode, false);
            }

            XmlNode xmlRoot = this.LoadXml (xml, new XmlDocument ());
            
            if (xmlRoot == null)
            {
                return;
            }
            
            if (!this.IsMultiline)
            {
                Node wasSelected = selectedNode;
                if (this.CreateTopLevelTable ())
                {
                    nodeClass = this.GetNodeClass (selectedNode);
                    selectedNode = this.GetCurrentlySelectedNode ();
                }
                else
                {
                    return;
                }
            }
            
            if ((nodeClass == NodeClass.unknown) )
            {
                return;
            }

            bool wasSelect = false;
            Node lastRow = null;
            if (!((xmlRoot == null) || !xmlRoot.HasChildNodes))
            {
                Node selRow = selectedNode;
                int count = 0;
                count = xmlRoot.ChildNodes.Count;
                int rCount = 0;
                if (((selRow.type_ != null) && (selRow.type_.type == ElementType.Mrow)) && !selRow.HasChildren())
                {
                    emptyRow = true;
                    rCount = 1;
                }
                if ((((selRow.parent_ != null)) &&
                     ((selRow.parent_.type_.maxChilds != -1) &&
                      (((selRow.parent_.numChildren - rCount) + count) >= selRow.parent_.type_.maxChilds))) ||
                    ((selRow.parent_.type_.type == ElementType.Mmultiscripts) ||
                     (selRow.parent_.type_.type == ElementType.Maction)))
                {
                    selRow = this.WrapInRowInplace(selRow);
                }

                if (selRow.IsAppend)
                {
                    Node row = selRow;
                    Node lastCell = null;
                    if (isPaste && (xmlRoot.Name == "math"))
                    {
                        Node newNode = new Node();
                        newNode.Parse(xmlRoot, this.types_, this.entityManager, true, null);
                        if ((newNode.type_ != null) && newNode.HasChildren())
                        {
                            NodesList list = newNode.GetChildrenNodes();
                            Node n = list.Next();
                            lastRow = row;
                            for (int i = 0; (row != null) && (n != null); i++)
                            {
                                row.AppendNode(n);
                                lastCell = n;
                                n = list.Next();
                                row = row.nextSibling;
                            }
                            if (lastCell != null)
                            {
                                if (lastCell.nextSibling != null)
                                {
                                    this.SelectNode(lastCell.nextSibling, false);
                                }
                                else
                                {
                                    this.SelectNode(lastCell, true);
                                }
                                wasSelect = true;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            XmlNode x = xmlRoot.ChildNodes[i];
                            Node n = new Node();
                            n.Parse(x, this.types_, this.entityManager, false, null);
                            if (n.type_ != null)
                            {
                                row.AppendNode(n);

                                lastCell = n.Parse(x, this.types_, this.entityManager, true, null);
                                if (lastCell != null)
                                {
                                    this.SelectNode(lastCell, lastCell.IsAppend);
                                    wasSelect = true;
                                    ok = true;
                                }
                                else if (i == 0)
                                {
                                    lastRow = n;
                                }
                            }
                            row = n;
                        }
                    }
                }
                else
                {
                    Node newSelectedNode = null;
                    if (isPaste && (xmlRoot.Name == "math"))
                    {
                        Node newNode = new Node();
                        newNode.Parse(xmlRoot, this.types_, this.entityManager, true, null);
                        if ((newNode.type_ != null) && newNode.HasChildren())
                        {
                            NodesList list = newNode.GetChildrenNodes();
                            Node n = list.Next();
                            for (int i = 0; (selRow != null) && (n != null); i++)
                            {
                                selRow.PrependNode(n);
                                n = list.Next();
                            }
                            this.SelectNode(selRow, false);
                            wasSelect = true;
                        }
                    }
                    else if (xmlRoot.Name == "math")
                    {
                        for (int i = 0; i < count; i++)
                        {
                            XmlNode x = xmlRoot.ChildNodes[i];
                            Node newMode = new Node();
                            newMode.Parse(x, this.types_, this.entityManager, false, null);
                            if (newMode.type_ != null)
                            {
                                selRow.PrependNode(newMode);

                                newSelectedNode =
                                    newMode.Parse(x, this.types_, this.entityManager, true, null);
                                if (newSelectedNode != null)
                                {
                                    this.SelectNode(newSelectedNode, newSelectedNode.IsAppend);
                                    ok = true;
                                    wasSelect = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Node newNode = new Node();
                            XmlNode x = xmlRoot.ChildNodes[i];
                        
                            this.InsertFromXml(selRow, x, newNode, ref newSelectedNode, ref wasSelect);
                        }
                    }
                }

                if (!wasSelect && (lastRow != null))
                {
                    this.SelectNode(lastRow, false);
                }
                if (emptyRow)
                {
                    if (selRow == this.selectedNode)
                    {
                        this.SelectNeighbor(selRow);
                    }
                    this.Tear(selRow);
                }
            }
            
            if ((!hasS || !ok) || ((this.selectedNode.type_.type != ElementType.Mrow) || (this.selectedNode.numChildren != 0)))
            {
                return;
            }
            Node lastSelected = null;
            Node firstSelected = null;
            Node singleSelected = null;
            try
            {
                Node curPrev = null;
                Node curNext = null;
                Node cur = null;
                cur = this.selectedNode;
                cur.InternalMark = 0;
                curPrev = cur.prevSibling;
                curNext = cur.nextSibling;
                singleSelected = cur.parent_;
                
                Node targetSelected = null;
                this.tryAddMathXML (false, s, false);
                try
                {
                    targetSelected = singleSelected.GetChildrenNodes ().Get (this.selectedNode.childIndex);
                }
                catch
                {
                }
                if (targetSelected != null)
                {
                    if ((targetSelected.type_.type == ElementType.Mrow) && (targetSelected.firstChild != null))
                    {
                        cur = targetSelected;
                        lastSelected = cur.firstChild;
                        firstSelected = cur.lastChild;
                    }
                    else if (((curPrev != null) && !curPrev.tagDeleted) && ((curNext != null) && !curNext.tagDeleted))
                    {
                        lastSelected = curPrev.nextSibling;
                        firstSelected = curNext.prevSibling;
                    }
                    else if ((curPrev != null) && !curPrev.tagDeleted)
                    {
                        lastSelected = curPrev.nextSibling;
                        firstSelected = singleSelected.lastChild;
                    }
                    else if ((curNext != null) && !curNext.tagDeleted)
                    {
                        lastSelected = singleSelected.firstChild;
                        firstSelected = curNext.prevSibling;
                    }
                }
            }
            catch
            {
            }
            if ((lastSelected != null) && (firstSelected != null))
            {
                this.SelectNode (firstSelected, true);
                this.multiSelectNode = lastSelected;
                this.hasSelection = true;
            }
            else if (singleSelected != null)
            {
                this.SelectNode (singleSelected, false);
            }
        }
    }
}