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
        public Node Parse(XmlNode XMLNode, Types mTypes, EntityManager mEntities,
                        bool bAll, StyleAttributes styleAttributes)
        {
            return Parse(XMLNode, mTypes, mEntities, bAll, styleAttributes, false);
        }

        public Node Parse(XmlNode XMLNode, Types mTypes, EntityManager mEntities, bool recurse, StyleAttributes styleAttributes, bool bParentShift)
        {
            bool hasSelect = false;
            bool hasSelectRight = false;
            Node result = null;
            if (!bParentShift)
            {
                xmlTagName = XMLNode.LocalName;
                namespaceURI = XMLNode.NamespaceURI;
            }
            
            int numAttrs = 0;
            
            if ((recurse && (XMLNode.Attributes != null)) && !bParentShift)
            {
                StyleAttributes attributes = ParseMStyle(XMLNode, style_);
                if (attributes != null)
                {
                    if (style_ == null)
                    {
                        style_ = new StyleAttributes();
                    }
                    attributes.CopyTo(style_);
                }
            
                numAttrs = XMLNode.Attributes.Count;
                if (numAttrs > 0)
                {
                    if (attrs == null)
                    {
                        attrs = new AttributeList();
                    }

                    for (int i = 0; i < numAttrs; i++)
                    {
                        
                        if (XMLNode.Attributes[i].Name == "nugenCursor")
                        {
                            result = this;
                            hasSelect = true;
                        }
                        else if (XMLNode.Attributes[i].Name == "nugenCursorEnd")
                        {
                            result = this;
                            result.IsAppend = true;
                            hasSelectRight = true;
                        }
                        else
                        {
                            attrs.Add(new Attribute(XMLNode.Attributes[i].Name, XMLNode.Attributes[i].Value, XMLNode.Attributes[i].NamespaceURI));
                        }
                    }

                    if (hasSelect)
                    {
                        XMLNode.Attributes.RemoveNamedItem("nugenCursor");
                    }
                    if (hasSelectRight)
                    {
                        XMLNode.Attributes.RemoveNamedItem("nugenCursorEnd");
                    }
                }
            }

            if ((XMLNode.NodeType == XmlNodeType.Element) && !bParentShift)
            {
                if (type_ == null)
                {
                    type_ = mTypes[xmlTagName];
                }
                if ((hasSelect && (type_.type == ElementType.Mi)) &&
                    (literalText != null))
                {
                    InternalMark = literalText.Length;
                }
            }

            if (recurse && XMLNode.HasChildNodes)
            {
                XmlNodeList list = XMLNode.ChildNodes;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].NodeType == XmlNodeType.Text)
                    {
                        if ((type_.type == ElementType.Mtext) || (type_.type == ElementType.Ms))
                        {
                            literalText += list[i].Value;
                            continue;
                        }
                        
                        if (type_.type == ElementType.Mn)
                        {
                            literalText += list[i].Value.Trim();
                            continue;
                        }
                        
                        if (type_.type == ElementType.Mi)
                        {
                            literalText += list[i].Value.Trim();
                            continue;
                        }

                        if (type_.type != ElementType.Mo)
                        {
                            continue;
                        }

                        string entityChar = list[i].Value.Trim();
                        bool isGlyph = false;
                        try
                        {
                            Glyph glyph;

                            if (! (((((entityChar != "(") && (entityChar != ")")) && ((entityChar != "[") && (entityChar != "]"))) &&
                                    (((entityChar != "{") && (entityChar != "}")) && ((entityChar != "|") && (entityChar != "||")))) &&
                                   (((entityChar != "+") && (entityChar != "-")) && ((entityChar != "=") && (entityChar != "/")))))
                            {
                                string entityName = "";


                                switch (entityChar)
                                {
                                    case "(":
                                    {
                                        entityName = "lpar";
                                        break;
                                    }
                                    case ")":
                                    {
                                        entityName = "rpar";
                                        break;
                                    }
                                    case "[":
                                    {
                                        entityName = "lbrack";
                                        break;
                                    }
                                    case "]":
                                    {
                                        entityName = "rbrack";
                                        break;
                                    }
                                    case "{":
                                    {
                                        entityName = "lbrace";
                                        break;
                                    }
                                    case "}":
                                    {
                                        entityName = "rbrace";
                                        break;
                                    }
                                    case "|":
                                    {
                                        entityName = "verbar";
                                        break;
                                    }
                                    case "||":
                                    {
                                        entityName = "Verbar";
                                        break;
                                    }
                                    case "+":
                                    {
                                        entityName = "plus";
                                        break;
                                    }
                                    case "-":
                                    {
                                        entityName = "minus";
                                        break;
                                    }
                                    case "=":
                                    {
                                        entityName = "equals";
                                        break;
                                    }
                                    case "/":
                                    {
                                        entityName = "sol";
                                        break;
                                    }
                                }

                                glyph = mEntities.ByName(entityName);
                                if (glyph != null)
                                {
                                    Node glyphNode = new Node();
                                    glyphNode.type_ = mTypes["entity"];
                                    glyphNode.literalText = "" + glyph.CharValue;
                                    glyphNode.fontFamily = glyph.FontFamily;
                                    glyphNode.glyph = glyph;
                                    glyphNode.xmlTagName = glyph.Name;
                                    AdoptChild(glyphNode);
                                    
                                    isGlyph = true;
                                }
                            }
                        }
                        catch
                        {
                        }

                        if (!isGlyph)
                        {
                            literalText += entityChar;
                        }
                        continue;
                    }

                    if (list[i].NodeType == XmlNodeType.SignificantWhitespace)
                    {
                        continue;
                    }

                    if (list[i].NodeType == XmlNodeType.Whitespace)
                    {
                        if ((type_.type == ElementType.Mtext) || (type_.type == ElementType.Ms))
                        {
                            literalText += " ";
                        }
                        continue;
                    }

                    if (list[i].NodeType == XmlNodeType.Element)
                    {
                        if ((list[i].NamespaceURI == "http://www.w3.org/1998/Math/MathML") && (list[i].LocalName == "mstyle"))
                        {
                            Node mstyl = ParseMstyle(list[i], mTypes, mEntities, recurse, styleAttributes);
                            if (mstyl != null)
                            {
                                result = mstyl;
                            }
                        }
                        else
                        {
                            Node n = new Node(XMLNode.Name, styleAttributes);
                            n.type_ = mTypes[list[i].LocalName];

                            if (AdoptChild(n))
                            {
                                Node sn = n.Parse(list[i], mTypes,  mEntities, recurse, styleAttributes, false);
                                if (sn != null)
                                {
                                    result = sn;
                                }
                            }
                        }

                        continue;
                    }
                
                    if (list[i].NodeType == XmlNodeType.EntityReference)
                    {
                        Node n = new Node();
                        n.type_ = mTypes["entity"];
                        if ((type_.type == ElementType.Mtext) ||
                            (type_.type == ElementType.Ms))
                        {
                            Glyph glyph = mEntities.ByName(list[i].LocalName);
                            if (glyph != null)
                            {
                                char c = Convert.ToChar(Convert.ToUInt32(glyph.Code, 0x10));
                                if (char.IsWhiteSpace(c) || char.IsControl(c))
                                {
                                    literalText = literalText + " ";
                                }
                                else
                                {
                                    literalText = literalText + c;
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                Glyph glyph = mEntities.ByName(list[i].LocalName);
                                if (glyph != null)
                                {
                                    n.literalText = "";
                                    n.literalText = n.literalText + glyph.CharValue;
                                    n.fontFamily = glyph.FontFamily;
                                    n.glyph = glyph;
                                    n.xmlTagName = list[i].LocalName;
                                }
                                else
                                {
                                    n.literalText = "?";
                                    n.xmlTagName = list[i].LocalName;
                                }
                                AdoptChild(n);
                            }
                            catch
                            {
                                n.literalText = "?";
                                n.xmlTagName = list[i].LocalName;
                                AdoptChild(n);
                            }
                        }
                    }
                }
            }
            return result;
        }

        public Node ParseMstyle(XmlNode XMLNode, Types mTypes, EntityManager mEntities, bool bAll, StyleAttributes styleAttributes)
        {
            StyleAttributes s = null;
            if ((XMLNode.Attributes == null) || (XMLNode.Attributes.Count <= 0))
            {
                return Parse(XMLNode, mTypes, mEntities, bAll, styleAttributes, true);
            }
            Node node = new Node();
            node.type_ = mTypes["mstyle"];
            node.attrs = new AttributeList();
            for (int i = 0; i < XMLNode.Attributes.Count; i++)
            {
                node.attrs.Add(new Attribute(XMLNode.Attributes[i].Name, XMLNode.Attributes[i].Value, ""));
            }
            StyleAttributes fromNode = new StyleAttributes();
            s = new StyleAttributes();
            fromNode = AttributeBuilder.StyleAttrsFromNode(node, true);
            if (fromNode != null)
            {
                if (styleAttributes != null)
                {
                    node.style_ = new StyleAttributes();
                    fromNode.CopyTo(node.style_);
                    node.style_.canOverride = true;
                    s = node.CascadeOverride(styleAttributes);
                }
                else
                {
                    fromNode.CopyTo(s);
                }
            }
            else
            {
                if (styleAttributes != null)
                    styleAttributes.CopyTo(s);
            }
            s.canOverride = true;
            XMLNode.Attributes.RemoveAll();
            return Parse(XMLNode, mTypes, mEntities, bAll, s, true);
        }

        public StyleAttributes ParseMStyle(XmlNode xmlNode, StyleAttributes baseStyle)
        {
            bool hasStyleAttrs = false;
            bool hasColor = false;
            bool hasBackground = false;
            bool hasMathsize = false;
            bool hasVariant = false;
            StyleAttributes r = null;
            
            int count = 0;

            if (((xmlNode != null) &&
                 (((xmlNode.Name == "mi") || (xmlNode.Name == "mo")) ||
                  (((xmlNode.Name == "mn") || (xmlNode.Name == "ms")) || (xmlNode.Name == "mtext")))) &&
                (xmlNode.Attributes != null))
            {
                try
                {
                    count = xmlNode.Attributes.Count;

                    for (int i = 0; i < count; i++)
                    {
                        string name = xmlNode.Attributes[i].Name.Trim().ToLower();
                        
                        if (((name == "mathvariant") || (name == "mathcolor")) ||
                            ((name == "mathbackground") || (name == "mathsize")))
                        {
                            hasStyleAttrs = true;
                        }
                        if (name == "mathvariant")
                        {
                            hasVariant = true;
                        }
                        if (name == "mathcolor")
                        {
                            hasColor = true;
                        }
                        if (name == "mathbackground")
                        {
                            hasBackground = true;
                        }
                        if (name == "mathsize")
                        {
                            hasMathsize = true;
                        }
                    }
                }
                catch
                {
                }
            }

            if (hasStyleAttrs)
            {
                try
                {
                    Node n = new Node();
                    n.attrs = new AttributeList();
                    for (int i = 0; i < count; i++)
                    {
                        n.attrs.Add(new Attribute(xmlNode.Attributes[i].Name, xmlNode.Attributes[i].Value, ""));
                    }

                    StyleAttributes nodeStyleAttrs = AttributeBuilder.StyleAttrsFromNode(n);
                    if (nodeStyleAttrs != null)
                    {
                        nodeStyleAttrs.canOverride = true;
                        
                        r = new StyleAttributes();
                        if (baseStyle != null)
                        {
                            n.style_ = new StyleAttributes();
                            nodeStyleAttrs.CopyTo(n.style_);
                            r = n.CascadeOverride(baseStyle);
                        }
                        else
                        {
                            nodeStyleAttrs.CopyTo(r);
                        }
                        r.canOverride = true;
                    }
                    if (hasMathsize)
                    {
                        xmlNode.Attributes.RemoveNamedItem("mathsize", "");
                    }
                    if (hasVariant)
                    {
                        xmlNode.Attributes.RemoveNamedItem("mathvariant", "");
                    }
                    if (hasColor)
                    {
                        xmlNode.Attributes.RemoveNamedItem("mathcolor", "");
                    }
                    if (hasBackground)
                    {
                        xmlNode.Attributes.RemoveNamedItem("mathbackground", "");
                    }
                }
                catch
                {
                }
            }
            return r;
        }

        public StyleAttributes CascadeOverride(StyleAttributes baseAttrs)
        {
            StyleAttributes own = null;
            StyleAttributes result = null;

            if (baseAttrs != null)
            {
                result = new StyleAttributes();
                baseAttrs.CopyTo(result);
            }

            own = style_;
            if (own != null)
            {
                if (result == null)
                {
                    result = new StyleAttributes();
                    own.CopyTo(result);
                    return result;
                }

                if (!own.canOverride)
                {
                    return result;
                }

                if (own.displayStyle != DisplayStyle.AUTOMATIC)
                {
                    result.displayStyle = own.displayStyle;
                }
                
                if (own.scriptLevel != ScriptLevel.NONE)
                {
                    result.scriptLevel = own.scriptLevel;
                }
                
                if (own.hasColor)
                {
                    result.color = own.color;
                    result.hasColor = true;
                }
                
                if (own.hasBackground)
                {
                    result.background = own.background;
                    result.hasBackground = true;
                }
                
                if (own.hasSize)
                {
                    result.scale = own.scale;
                    result.size = own.size;
                    result.hasSize = true;
                }
                
                if (own.IsStyled)
                {
                    result.isBold = own.isBold;
                    result.isItalic = own.isItalic;
                }
                
                result.isUnderline = false;
                
                if (own.isNormal)
                {
                    result.isNormal = true;
                }
                
                if (own.isScript)
                {
                    result.isScript = true;
                }
                
                if (own.isSans)
                {
                    result.isSans = true;
                }
                
                if (own.isFractur)
                {
                    result.isFractur = true;
                }
                
                if (own.isDoubleStruck)
                {
                    result.isDoubleStruck = true;
                }
                
                if (own.isMonospace)
                {
                    result.isMonospace = true;
                }
            }

            return result;
        }
    }
}