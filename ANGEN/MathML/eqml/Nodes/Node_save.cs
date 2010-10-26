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
        public void SavePure(XmlDocument xmlDoc, XmlNode XMLNode, string sXMLEncoding)
        {
            SaveToXml(xmlDoc, XMLNode, sXMLEncoding, null, "");
        }

        public void SaveToXml(XmlDocument xmlDoc, XmlNode XMLNode, string sXMLEncoding)
        {
            SaveToXml(xmlDoc, XMLNode, sXMLEncoding, null);
        }

        public void SaveToXml(XmlDocument xmlDoc, XmlNode XMLNode, string sXMLEncoding, Selection Selection_Collection)
        {
            SaveToXml(xmlDoc, XMLNode, sXMLEncoding, Selection_Collection, namespaceURI);
        }

        public void SaveToXml(XmlDocument xmlDoc, XmlNode XMLNode, string sXMLEncoding, Selection Selection_Collection, string nspace)
        {
            NodesList list;
            XmlNode targetXmlNode = null;
            XmlNode selNode = null;
            bool ownSel = false;
            if ((Selection_Collection != null) && (this == Selection_Collection.parent))
            {
                ownSel = true;
                targetXmlNode = XMLNode;
            }
            
            if (type_ == null)
            {
                return;
            }

            if (!ownSel)
            {
                if (type_.type == ElementType.Entity)
                {
                    targetXmlNode = xmlDoc.CreateNode(XmlNodeType.EntityReference, xmlTagName, nspace);
                }
                else if ((type_.xmlTag != null) && (type_.xmlTag.Length > 0))
                {
                    {
                        targetXmlNode = xmlDoc.CreateNode(XmlNodeType.Element, type_.xmlTag, nspace);

                        if (((type_.type == ElementType.Ms) ||
                             (type_.type == ElementType.Mtext)) &&
                            (((targetXmlNode != null) && (literalText != null)) && (literalText.Length > 0)))
                        {
                            string s = literalText;
                            if (Selection_Collection != null)
                            {
                                if ((Selection_Collection.First != null) && (Selection_Collection.First == this))
                                {
                                    if ((Selection_Collection.Last != null) && (Selection_Collection.Last == this))
                                    {
                                        s = s.Substring(Selection_Collection.caret,
                                                            Selection_Collection.literalLength -
                                                            Selection_Collection.caret);
                                    }
                                    else
                                    {
                                        s = s.Substring(Selection_Collection.caret,
                                                            s.Length - Selection_Collection.caret);
                                    }
                                }
                                else if (((Selection_Collection.Last != null) && (Selection_Collection.Last == this)) &&
                                         (Selection_Collection.First != this))
                                {
                                    s = s.Substring(0, Selection_Collection.literalLength);
                                }
                            }
                            selNode = xmlDoc.CreateTextNode(s);
                            targetXmlNode.AppendChild(selNode);
                        }
                        else if (((type_.type != ElementType.Mglyph) && (targetXmlNode != null)) &&
                                 ((literalText != null) && (literalText.Length > 0)))
                        {
                            string s = literalText;
                            if (Selection_Collection != null)
                            {
                                if ((Selection_Collection.First != null) && (Selection_Collection.First == this))
                                {
                                    if ((Selection_Collection.Last != null) && (Selection_Collection.Last == this))
                                    {
                                        s =
                                            s.Substring(Selection_Collection.caret,
                                                            Selection_Collection.literalLength -
                                                            Selection_Collection.caret);
                                    }
                                    else
                                    {
                                        s =
                                            s.Substring(Selection_Collection.caret,
                                                            s.Length - Selection_Collection.caret);
                                    }
                                }
                                else if (((Selection_Collection.Last != null) && (Selection_Collection.Last == this)) &&
                                         (Selection_Collection.First != this))
                                {
                                    s = s.Substring(0, Selection_Collection.literalLength);
                                }
                            }
                            selNode = xmlDoc.CreateTextNode(s);
                            targetXmlNode.AppendChild(selNode);
                        }
                    }
                }
            }
            if (targetXmlNode == null)
            {
                return;
            }
            
            if (XMLNode == null)
            {
                string xml = "";
                if (sXMLEncoding == "UTF-16")
                {
                    xml = "<?xml version='1.0' encoding='UTF-16'?>";
                }
                else if (sXMLEncoding == "UTF-8")
                {
                    xml = "<?xml version='1.0' encoding='UTF-8'?>";
                }
                else if (sXMLEncoding.Length > 0)
                {
                    xml = "<?xml version='1.0' encoding='" + sXMLEncoding + "'?>";
                }
                else
                {
                    xml = "<?xml version='1.0'?>";
                }
                xml = xml + "<root/>";
                xmlDoc.LoadXml(xml);
                if ((type_.type == ElementType.Math) && displayStyle)
                {
                    XmlAttribute attribute = xmlDoc.CreateAttribute("", "display", "");
                    attribute.Value = "block";
                    targetXmlNode.Attributes.Append(attribute);
                }
                
                xmlDoc.ReplaceChild(targetXmlNode, xmlDoc.DocumentElement);
            }
            else if ((!ownSel && !false) && !false)
            {
                {
                    XMLNode.AppendChild(targetXmlNode);
                }
                if ((type_.type == ElementType.Math) && displayStyle)
                {
                    XmlAttribute attribute = xmlDoc.CreateAttribute("", "display", "");
                    attribute.Value = "block";
                    targetXmlNode.Attributes.Append(attribute);
                    XMLNode.AppendChild(targetXmlNode);
                }
            }
            if (((((Selection_Collection == null) || (this != Selection_Collection.parent)))) && ((!HasChildren())))
            {
                return;
            }
            if ((Selection_Collection != null) && (this == Selection_Collection.parent))
            {
                list = Selection_Collection.nodesList;
            }
            else
            {
                list = GetChildrenNodes();
            }
            
            list.Reset();
            Node next = list.Next();
            int level = 0;
            bool sameSel = false;
            bool isPrev = false;
            bool isNext = false;
            XmlNode curXmlNode = targetXmlNode;
            bool ok = true;
            while ((next != null) && ok)
            {
                sameSel = false;
                isPrev = false;
                isNext = false;

                if (next.type_.type != ElementType.Entity)
                {
                    if ((Selection_Collection != null) && (next == Selection_Collection.First))
                    {
                        if (next.IsSameStyleParent())
                        {
                            sameSel = true;
                        }
                    }
                    else if (next.prevSibling == null)
                    {
                        if (next.IsSameStyleParent())
                        {
                            sameSel = true;
                        }
                    }
                    else if (!next.IsSameStyle(next.prevSibling))
                    {
                        if (next.IsSameStyleParent() && next.prevSibling.IsSameStyleParent())
                        {
                            isNext = true;
                        }
                        else
                        {
                            if (next.prevSibling.IsSameStyleParent())
                            {
                                isPrev = true;
                            }
                            if (next.IsSameStyleParent())
                            {
                                sameSel = true;
                            }
                        }
                    }
                }
                if (isNext)
                {
                    if (next.prevSibling.IsSameStyleParent() && (level > 0))
                    {
                        curXmlNode = curXmlNode.ParentNode;
                        level--;
                    }
                    sameSel = true;
                }
                if (sameSel && next.IsSameStyleParent())
                {
                    Node snode;
                    if (next.style_ == null)
                    {
                        snode = new Node();
                        next.style_ = new StyleAttributes();
                    }
                    if ((next.parent_ != null) && (next.parent_.style_ != null))
                    {
                        snode = new Node(next.parent_.style_);
                    }
                    else
                    {
                        snode = new Node(new StyleAttributes());
                    }
                    XmlNode mstyleNode = null;
                    mstyleNode = xmlDoc.CreateNode(XmlNodeType.Element, "mstyle", nspace);
                    AttributeBuilder.CascadeStyles(next.parent_, snode, next.style_);
                    if (snode.attrs != null)
                    {
                        snode.attrs.Reset();
                        for (Attribute i = snode.attrs.Next(); i != null; i = snode.attrs.Next())
                        {
                            XmlAttribute attr = xmlDoc.CreateAttribute("", i.name, "");
                            attr.Value = i.val;
                            mstyleNode.Attributes.Append(attr);
                        }
                        snode.attrs.Reset();
                    }
                    curXmlNode.AppendChild(mstyleNode);
                    curXmlNode = mstyleNode;
                    level++;
                }
                if (isPrev && next.prevSibling.IsSameStyleParent())
                {
                    curXmlNode = curXmlNode.ParentNode;
                    level--;
                }

                next.SaveToXml(xmlDoc, curXmlNode, sXMLEncoding, Selection_Collection, nspace);
                next = list.Next();
                if (((Selection_Collection != null) && (Selection_Collection.Last != null)) &&
                    ((next == Selection_Collection.Last) && (Selection_Collection.literalLength == 0)))
                {
                    ok = false;
                }
            }
        }
    }
}