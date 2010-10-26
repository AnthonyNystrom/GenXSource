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

    public delegate void InvalidXMLFile(object sender, InvalidXMLArgs e);

    public partial class NodesBuilder
    {
        public event EventHandler UndoRedo;
        public event InvalidXMLFile InvalidXML;
        //
        public event InsertionHappenned InsertHappened;

        //
        public NodesBuilder (int width, OperatorDictionary operatorDictionary)
        {
            this.fonts_ = null;
            this.mathmlSchema = "";
            this.mathmlNamespace = "http://www.w3.org/1998/Math/MathML";
            
            this.painter_ = null;
            this.currentCaret = 0;
            this.horizontalRes = 0;
            this.oX = 0;
            this.oY = 0;
            this.hasSelection = false;
            this.fontSize = 12f;
            this.operators_ = null;
            this.schema = null;
            this.undo = new UndoRedoStack ();
            this.redo = new UndoRedoStack ();
            this.stretchyBrackets = true;
            
            this.operators_ = operatorDictionary;
            if (this.operators_ == null)
            {
                this.operators_ = new OperatorDictionary ();
            }
            this.undo.callback =
                (EventHandler) Delegate.Combine (this.undo.callback, new EventHandler (this.UndoCallback));
            this.redo.callback =
                (EventHandler) Delegate.Combine (this.redo.callback, new EventHandler (this.RedoCallback));
            this.clientRect = new Rectangle (0, 0, 0, 0);
            this.painter_ = new Painter ();
            this.painter_.SetFontSize (this.FontSize);
            if (this.operators_ != null)
            {
                this.painter_.SetOperators (this.operators_);
            }
            this.Width = width;
        }
        //
        private Node getSelectedOrRootIfEmpty ()
        {
            try
            {
                if ((this.selectedNode == null) || this.selectedNode.tagDeleted)
                {
                    try
                    {
                        this.selectedNode = this.GetRootNode ();
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            return this.selectedNode;
        }
        //
        public Node GetCurrentlySelectedNode ()
        {
            try
            {
                if ((this.selectedNode == null) || this.selectedNode.tagDeleted)
                {
                    try
                    {
                        this.selectedNode = this.GetRootNode ();
                    }
                    catch
                    {
                    }
                }
                if (((this.selectedNode != null) && (this.selectedNode.type_ != null)) && this.selectedNode.IsAtom())
                {
                    StyleAttributes styleAttributes = this.selectedNode.style_;
                    if ((this.selectedNode.LiteralStart < 0) && (this.painter_ != null))
                    {
                        int leftQuoteWidth = -1 * this.selectedNode.LiteralStart;
                        try
                        {
                            if (this.selectedNode.type_.type == ElementType.Ms)
                            {
                                leftQuoteWidth += ((Box_Ms) this.selectedNode.box).leftQuoteWidth;
                            }
                        }
                        catch
                        {
                        }
                        int start = this.selectedNode.box.X;
                        int end = 0;
                        if (this.selectedNode.literalText.Length < 1)
                        {
                            this.selectedNode.InternalMark = 0;
                            this.selectedNode.LiteralStart = 0;
                            return this.getSelectedOrRootIfEmpty ();
                        }
                        if (this.selectedNode.literalText.Length == 1)
                        {
                            int w =
                                this.painter_.MeasureWidth (this.selectedNode, styleAttributes,
                                                       this.selectedNode.literalText.Substring (0, 1));
                            if (leftQuoteWidth > (w / 2))
                            {
                                this.selectedNode.InternalMark = 1;
                                this.selectedNode.LiteralStart = leftQuoteWidth;
                            }
                            else
                            {
                                this.selectedNode.InternalMark = 0;
                                this.selectedNode.LiteralStart = 0;
                            }
                            return this.getSelectedOrRootIfEmpty ();
                        }
                        int lastw = 0;
                        int ww = 0;
                        for (int i = 1; i <= this.selectedNode.literalText.Length; i++)
                        {
                            ww =
                                this.painter_.MeasureWidth (this.selectedNode, styleAttributes,
                                                       this.selectedNode.literalText.Substring (0, i));
                            if (ww == 0)
                            {
                                return this.getSelectedOrRootIfEmpty ();
                            }
                            end = this.selectedNode.box.X + ww;
                            if ((start < (this.selectedNode.box.X + leftQuoteWidth)) &&
                                ((leftQuoteWidth + this.selectedNode.box.X) <= end))
                            {
                                if (((ww > lastw) &&
                                     ((start + ((ww - lastw) / 2)) < (this.selectedNode.box.X + leftQuoteWidth))) &&
                                    ((leftQuoteWidth + this.selectedNode.box.X) <= end))
                                {
                                    this.selectedNode.InternalMark = i;
                                    this.selectedNode.LiteralStart = end - this.selectedNode.box.X;
                                }
                                else
                                {
                                    this.selectedNode.InternalMark = i - 1;
                                    this.selectedNode.LiteralStart = start - this.selectedNode.box.X;
                                }
                                return this.getSelectedOrRootIfEmpty ();
                            }
                            start = end;
                            lastw = ww;
                        }
                    }
                    else
                    {
                        if (this.selectedNode.literalText != null)
                        {
                            if (this.selectedNode.literalText.Length > 0)
                            {
                                this.selectedNode.LiteralStart = this.painter_.MeasureWidth (this.selectedNode, styleAttributes,
                                                                                             this.selectedNode.literalText.Substring (0, this.selectedNode.InternalMark));
                            }
                            else
                            {
                                this.selectedNode.LiteralStart = 0;
                            }
                        }
                        return this.getSelectedOrRootIfEmpty ();
                    }
                }
            }
            catch
            {
            }
            return this.getSelectedOrRootIfEmpty ();
        }
        //
        public Node MultiSelectNode ()
        {
            return this.multiSelectNode;
        }
        //
        public int CurrentCaret ()
        {
            return this.currentCaret;
        }
        //
        public Selection CaptureSelection (Node from, int fromCaret, Node to, int toCaret, bool needSwap)
        {
            Selection r = new Selection ();
            r.swap = needSwap;
            r.parent = from.parent_;
            
            Node cur = from;
            bool done = false;
            while ((cur != null) && !done)
            {
                if (((cur == from) || (cur == to)) &&
                    (((cur.IsAtom() && (cur.parent_ != null)) && cur.isVisible)))
                {
                    if ((cur.literalText != null) && (cur.literalText.Length > 0))
                    {
                        int curCaret = 0;
                        int len = 0;
                        if ((cur == from) && (cur == to))
                        {
                            curCaret = fromCaret;
                            len = toCaret;
                            if (len < 0)
                            {
                                len = cur.LiteralLength;
                            }
                            if (curCaret > len)
                            {
                                int t = len;
                                len = curCaret;
                                curCaret = t;
                            }
                            if (len > curCaret)
                            {
                                r.caret = curCaret;
                                r.literalLength = len;
                                r.Add (cur);
                            }
                        }
                        else if (cur == from)
                        {
                            curCaret = fromCaret;
                            if (curCaret < 0)
                            {
                                curCaret = 0;
                            }
                            r.caret = curCaret;
                            r.Add (cur);
                        }
                        else if (cur == to)
                        {
                            len = toCaret;
                            if (len < 0)
                            {
                                len = cur.LiteralLength;
                            }
                            r.literalLength = len;
                            r.Add (cur);
                        }
                    }
                }
                else if ((cur == from) && (cur == to))
                {
                    int curCaret = 0;
                    int len = 0;
                    curCaret = fromCaret;
                    len = toCaret;
                    if (len < 0)
                    {
                        len = cur.LiteralLength;
                    }
                    if (curCaret > len)
                    {
                        int t = len;
                        len = curCaret;
                        curCaret = t;
                    }
                    else
                    {
                    }
                    if (len > curCaret)
                    {
                        r.caret = curCaret;
                        r.literalLength = len;
                        r.Add (cur);
                    }
                }
                else
                {
                    if (cur != to)
                    {
                        r.Add (cur);
                    }
                    if (cur == to)
                    {
                        if (toCaret == cur.LiteralLength)
                        {
                            r.literalLength = cur.LiteralLength;
                            r.Add (cur);
                            if (cur == from)
                            {
                                r.caret = 0;
                            }
                        }
                        else if (cur == from)
                        {
                            r.literalLength = from.LiteralLength;
                            r.caret = 0;
                        }
                        else if (cur.prevSibling != null)
                        {
                            r.literalLength = cur.prevSibling.LiteralLength;
                        }
                    }
                }
                if (cur == to)
                {
                    done = true;
                    continue;
                }
                cur = cur.nextSibling;
            }
            return r;
        }
        //
        public Selection CaptureSelection ()
        {
            Selection r = null;
            try
            {
                if ((this.selectedNode == null) || !this.HasSelection)
                {
                    return r;
                }
                Node start = this.GetCurrentlySelectedNode ();
                Node end = this.MultiSelectNode ();
                Node first = this.GetCurrentlySelectedNode ();
                Node last = this.MultiSelectNode ();
                if (((first.parent_ == null) || (last.parent_ == null)) || (first.parent_ != last.parent_))
                {
                    return r;
                }
                int firstMark = 0;
                int secondMark = 0;
                if (first == last)
                {
                    firstMark = first.InternalMark;
                    secondMark = this.currentCaret;
                    if (firstMark > secondMark)
                    {
                        first = end;
                        last = start;
                        firstMark = this.currentCaret;
                        secondMark = last.InternalMark;
                        return this.CaptureSelection (first, firstMark, last, secondMark, true);
                    }
                    firstMark = first.InternalMark;
                    secondMark = this.currentCaret;
                    return this.CaptureSelection (first, firstMark, last, secondMark, false);
                }
                if (first.childIndex > last.childIndex)
                {
                    first = end;
                    last = start;
                    firstMark = this.currentCaret;
                    secondMark = last.InternalMark;
                    return this.CaptureSelection (first, firstMark, last, secondMark, true);
                }
                firstMark = first.InternalMark;
                secondMark = this.currentCaret;
                return this.CaptureSelection (first, firstMark, last, secondMark, false);
            }
            catch
            {
                return null;
            }
        }
        //
        private void ClearSelection ()
        {
            this.HasSelection = false;
        }
        //
        public bool GotoLast ()
        {
            this.ClearSelection ();
            Node last = null;
            Node cur = null;
            try
            {
                if (this.selectedNode != null)
                {
                    cur = this.GetCurrentlySelectedNode ();
                    if ((cur != null))
                    {
                        bool isMulti = false;
                        if ((this.rootNode_ != null) && (this.rootNode_.type_.type == ElementType.Math)
                            )
                        {
                            if (((this.rootNode_.numChildren == 1) &&
                                 (this.rootNode_.firstChild.type_.type == ElementType.Mtable)) &&
                                (this.rootNode_.firstChild.Class == "nugentoplevel"))
                            {
                                isMulti = true;
                            }
                            if (isMulti)
                            {
                                while ((cur.parent_ != null) &&
                                       ((cur.parent_.type_.type != ElementType.Mtd) ||
                                        (cur.parent_.level != 3)))
                                {
                                    cur = cur.parent_;
                                }
                                if (((cur.parent_ == null) ||
                                     (cur.parent_.type_.type != ElementType.Mtd)) ||
                                    (cur.parent_.level != 3))
                                {
                                    return false;
                                }
                                last = cur.parent_.lastChild;
                                if (last == null)
                                {
                                    return false;
                                }
                                this.SelectNode (last, true);
                                return true;
                            }
                            last = this.rootNode_.lastChild;
                            if ((last != null) &&
                                (((last.type_.type != ElementType.Mtable) || (last.level != 1)) ||
                                 (last.Class != "nugentoplevel")))
                            {
                                this.SelectNode (last, true);
                                return true;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            
            return false;
        }
        //
        public bool GoHome ()
        {
            this.ClearSelection ();
            Node selectedNode = null;
            Node firstChild = null;
            try
            {
                
                if (this.selectedNode != null)
                {
                    selectedNode = this.GetCurrentlySelectedNode ();
                    if ((selectedNode != null))
                    {
                        bool isMultiline = false;
                        if ((this.rootNode_ != null) && (this.rootNode_.type_.type == ElementType.Math))
                        {
                            if (((this.rootNode_.numChildren == 1) &&
                                 (this.rootNode_.firstChild.type_.type == ElementType.Mtable)) &&
                                (this.rootNode_.firstChild.Class == "nugentoplevel"))
                            {
                                isMultiline = true;
                            }

                            if (isMultiline)
                            {
                                while ((selectedNode.parent_ != null) &&
                                       ((selectedNode.parent_.type_.type != ElementType.Mtd) ||
                                        (selectedNode.parent_.level != 3)))
                                {
                                    selectedNode = selectedNode.parent_;
                                }
                                if (((selectedNode.parent_ == null) ||
                                     (selectedNode.parent_.type_.type != ElementType.Mtd)) ||
                                    (selectedNode.parent_.level != 3))
                                {
                                    return false;
                                }
                                firstChild = selectedNode.parent_.firstChild;
                                if (firstChild == null)
                                {
                                    return false;
                                }
                                this.SelectNode (firstChild, false);
                                return true;
                            }
                            else
                            {
                                firstChild = this.rootNode_.firstChild;
                                if ((firstChild != null) &&
                                    (((firstChild.type_.type != ElementType.Mtable) || (firstChild.level != 1)) ||
                                     (firstChild.Class != "nugentoplevel")))
                                {
                                    this.SelectNode(firstChild, false);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return false;
        }
        //
        public bool WordRight ()
        {
            this.ClearSelection ();
            try
            {
                Node cur = this.GetCurrentlySelectedNode();
                if (this.IsMultiline)
                {
                    bool lowlevel = false;
                    while (cur.level > 4)
                    {
                        cur = cur.parent_;
                        lowlevel = true;
                    }
                    if (lowlevel)
                    {
                        if (!this.IsToplevelRowCell(cur))
                        {
                            return true;
                        }
                        if (cur.nextSibling != null)
                        {
                            this.SelectNode(cur.nextSibling, false);
                        }
                        else
                        {
                            this.SelectNode(cur, true);
                        }
                        return true;
                    }
                    if (!this.IsToplevelRowCell(cur))
                    {
                        return true;
                    }
                    if (cur.IsAppend)
                    {
                        if (cur.nextSibling != null)
                        {
                            this.SelectNode(cur.nextSibling, true);
                            return true;
                        }
                        Node row = null;
                        Node cell = null;
                        Node nextRow = null;
                        Node target = null;
                        if (cur.parent_.type_.type != ElementType.Mtd)
                        {
                            return true;
                        }
                        cell = cur.parent_;
                        if (cell.parent_.type_.type != ElementType.Mtr)
                        {
                            return true;
                        }
                        row = cell.parent_;
                        if ((row.nextSibling == null) ||
                            (row.nextSibling.type_.type != ElementType.Mtr))
                        {
                            return true;
                        }
                        nextRow = row.nextSibling;
                        if ((nextRow.firstChild == null) ||
                            (nextRow.firstChild.type_.type != ElementType.Mtd))
                        {
                            return true;
                        }
                        target = nextRow.firstChild;
                        if (target.firstChild == null)
                        {
                            return true;
                        }
                        this.SelectNode(target.firstChild, false);
                        return true;
                    }
                    if (cur.nextSibling != null)
                    {
                        this.SelectNode(cur.nextSibling, false);
                        return true;
                    }
                    this.SelectNode(cur, true);
                    return true;
                }
                bool ok = false;
                while (cur.level > 1)
                {
                    cur = cur.parent_;
                    ok = true;
                }
                if (ok)
                {
                    if (cur.level != 1)
                    {
                        return true;
                    }
                    if (cur.nextSibling != null)
                    {
                        this.SelectNode(cur.nextSibling, false);
                        return true;
                    }
                    this.SelectNode(cur, true);
                    return true;
                }
                if (cur.level == 1)
                {
                    if (cur.IsAppend)
                    {
                        if (cur.nextSibling == null)
                        {
                            return true;
                        }
                        this.SelectNode(cur.nextSibling, true);
                        return true;
                    }
                    if (cur.nextSibling != null)
                    {
                        this.SelectNode(cur.nextSibling, false);
                        return true;
                    }
                    this.SelectNode(cur, true);
                    return true;
                }
            }
            catch
            {
            }
            return true;
        }
        //
        public bool WordLeft ()
        {
            this.ClearSelection ();
            try
            {
                Node cur = this.GetCurrentlySelectedNode();
                if (this.IsMultiline)
                {
                    bool cellLevel = false;
                    while (cur.level > 4)
                    {
                        cur = cur.parent_;
                        cellLevel = true;
                    }
                    if (cellLevel)
                    {
                        if (!this.IsToplevelRowCell(cur))
                        {
                            return false;
                        }
                        this.SelectNode(cur, false);
                        return true;
                    }
                    if (!this.IsToplevelRowCell(cur))
                    {
                        return false;
                    }
                    if (cur.InternalMark == 0)
                    {
                        if (cur.prevSibling != null)
                        {
                            this.SelectNode(cur.prevSibling, false);
                            return true;
                        }
                        Node row = null;
                        Node cell = null;
                        Node prevRow = null;
                        Node target = null;
                        if (cur.parent_.type_.type != ElementType.Mtd)
                        {
                            return false;
                        }
                        cell = cur.parent_;
                        if (cell.parent_.type_.type != ElementType.Mtr)
                        {
                            return false;
                        }
                        row = cell.parent_;
                        if ((row.prevSibling == null) ||
                            (row.prevSibling.type_.type != ElementType.Mtr))
                        {
                            return false;
                        }
                        prevRow = row.prevSibling;
                        if ((prevRow.firstChild == null) ||
                            (prevRow.firstChild.type_.type != ElementType.Mtd))
                        {
                            return false;
                        }
                        target = prevRow.firstChild;
                        if (target.lastChild == null)
                        {
                            return false;
                        }
                        this.SelectNode(target.lastChild, true);
                        return true;
                    }
                    this.SelectNode(cur, false);
                    return true;
                }
                bool topLevel = false;
                while (cur.level > 1)
                {
                    cur = cur.parent_;
                    topLevel = true;
                }
                if (topLevel)
                {
                    if (cur.level != 1)
                    {
                        return false;
                    }
                    this.SelectNode(cur, false);
                    return true;
                }
                if (cur.level == 1)
                {
                    if (cur.InternalMark == 0)
                    {
                        if (cur.prevSibling == null)
                        {
                            return false;
                        }
                        this.SelectNode(cur.prevSibling, false);
                        return true;
                    }
                    this.SelectNode(cur, false);
                    return true;
                }
            }
            catch
            {
            }
            
            return false;
        }
        //
        public bool GoLeft ()
        {
            return this.GoLeft (false);
        }
        //
        public bool GoLeft (bool interactive)
        {
            if (interactive)
            {
                this.ClearSelection ();
            }
            try
            {
                if (this.selectedNode != null)
                {
                    if (this.selectedNode.isVisible && (this.selectedNode.InternalMark > 0))
                    {
                        if ((
                             ((this.selectedNode.literalText == null) || (this.selectedNode.literalText.Length == 0))) &&
                            ((this.selectedNode.HasChildren () && !this.selectedNode.firstChild.isGlyph) &&
                             (this.selectedNode.firstChild.type_.type != ElementType.Entity)))
                        {
                            Node cur = null;
                            bool valid = false;
                            cur = this.selectedNode;
                            while ((!valid && (cur != null)) &&
                                   ((cur.lastChild != null) && !cur.lastChild.isGlyph))
                            {
                                if (((cur.type_.type == ElementType.Mover) ||
                                     (cur.type_.type == ElementType.Munder)) ||
                                    (cur.type_.type == ElementType.Munderover))
                                {
                                    cur = cur.firstChild;
                                    if (!cur.skip && !cur.isGlyph)
                                    {
                                        valid = true;
                                    }
                                    continue;
                                }
                                cur = cur.lastChild;
                                if ((cur.type_.type == ElementType.Mrow) && (cur.firstChild == null))
                                {
                                    valid = true;
                                    continue;
                                }
                                if (!cur.skip && !cur.isGlyph)
                                {
                                    valid = true;
                                }
                            }
                            if (valid)
                            {
                                if ((cur.parent_ != null) && this.IsClosingBracket (cur))
                                {
                                    if ((cur.prevSibling != null) && (cur.prevSibling.prevSibling != null))
                                    {
                                        this.SelectNode (cur.prevSibling, true);
                                    }
                                    else
                                    {
                                        this.SelectNode (cur, false);
                                    }
                                }
                                else
                                {
                                    this.SelectNode (cur, true);
                                }
                                return true;
                            }
                        }
                        if (this.selectedNode.InternalMark == 1)
                        {
                            this.SelectNode (this.selectedNode, false);
                        }
                        else
                        {
                            this.selectedNode.InternalMark--;
                        }
                        return true;
                    }
                    if (this.selectedNode.prevSibling != null)
                    {
                        Node target = null;
                        bool isMaction = false;
                        if (this.selectedNode.prevSibling.type_.type == ElementType.Maction)
                        {
                            
                            Node maction = ((Box_maction) this.selectedNode.prevSibling.box).target;
                            if (maction != null)
                            {
                                target = maction;
                                isMaction = true;
                            }
                        }
                        else if ((this.selectedNode.InternalMark == 0) &&
                                 (this.selectedNode.parent_.type_.type == ElementType.Maction))
                        {
                            this.SelectNode (this.selectedNode.parent_, false);
                            return true;
                        }
                        bool ok = false;
                        if (!isMaction)
                        {
                            target = this.selectedNode.prevSibling;
                        }
                        if (((target != null) && (target.lastChild != null)) && !target.lastChild.isGlyph)
                        {
                            if ((target.parent_ != null) &&
                                (((target.type_.type == ElementType.Mover) ||
                                  (target.type_.type == ElementType.Munder)) ||
                                 (target.type_.type == ElementType.Munderover)))
                            {
                                target = target.firstChild;
                            }
                            else
                            {
                                target = target.lastChild;
                                ok = true;
                            }
                        }
                        if ((target == this.selectedNode.prevSibling) &&
                            this.NotInSameContainer (this.selectedNode.prevSibling, this.selectedNode))
                        {
                            this.SelectNode (target, false);
                            if (this.IsOpeningBracket (this.selectedNode))
                            {
                                return this.GoLeft ();
                            }

                            this.selectedNode.InternalMark = this.selectedNode.LiteralLength - 1;
                            
                            return true;
                        }
                        if (ok && this.IsClosingBracket (target))
                        {
                            if ((target.prevSibling != null) && (target.prevSibling.prevSibling != null))
                            {
                                this.SelectNode (target.prevSibling, true);
                            }
                            else
                            {
                                this.SelectNode (target, false);
                            }
                        }
                        else
                        {
                            this.SelectNode (target, true);
                        }
                        if (this.selectedNode.isVisible)
                        {
                            if (this.selectedNode.skip)
                            {
                                return this.GoLeft ();
                            }
                            
                            return true;
                        }
                        return this.GoLeft ();
                    }
                    if (this.selectedNode.parent_ != null)
                    {
                    
                        if (this.selectedNode.level == 1)
                        {
                            return false;
                        }
                        
                            try
                            {
                                if (((((this.selectedNode.parent_ != null) &&
                                       (this.selectedNode.parent_.type_.type == ElementType.Mtd)) &&
                                      ((this.selectedNode.parent_.nextSibling == null) &&
                                       (this.selectedNode.parent_.prevSibling == null))) &&
                                     (((this.selectedNode.parent_.parent_ != null) &&
                                       (this.selectedNode.parent_.parent_.type_.type ==
                                        ElementType.Mtr)) &&
                                      ((this.selectedNode.parent_.parent_.prevSibling == null) &&
                                       (this.selectedNode.parent_.parent_.parent_ != null)))) &&
                                    ((((this.selectedNode.parent_.parent_.parent_.type_.type ==
                                        ElementType.Mtable) &&
                                       (this.selectedNode.parent_.parent_.parent_.Class == "nugentoplevel")) &&
                                      ((this.selectedNode.parent_.parent_.parent_.level == 1) &&
                                       (this.selectedNode.parent_.parent_.parent_.nextSibling == null))) &&
                                     (this.selectedNode.parent_.parent_.parent_.prevSibling == null)))
                                {
                                    return false;
                                }
                            }
                            catch
                            {
                            }
                        
                        
                        this.SelectNode (this.selectedNode.parent_, false);
                        if (this.selectedNode.isVisible)
                        {
                            if (this.selectedNode.skip)
                            {
                                return this.GoLeft ();
                            }
                            return true;
                        }
                        return this.GoLeft ();
                    }
                }
            }
            catch
            {
            }
            return false;
        }
        //
        public bool GoRight ()
        {
            return this.GoRight (false);
        }
        //
        public bool GoRight (bool interactive)
        {
            if (interactive)
            {
                this.ClearSelection ();
            }
            try
            {
                if (this.selectedNode != null)
                {
                    
                    if ((this.selectedNode.InternalMark == 0) && (this.selectedNode.type_.type == ElementType.Maction))
                    {
                        
                        Node mactionTarget = ((Box_maction) this.selectedNode.box).target;
                        if (mactionTarget != null)
                        {
                            this.SelectNode (mactionTarget, false);
                            if ((this.selectedNode.firstChild != null) && this.selectedNode.skip)
                            {
                                return this.GoRight ();
                            }
                            return true;
                        }
                    }

                    if (!this.selectedNode.skip && (this.selectedNode.InternalMark < (this.selectedNode.LiteralLength - 1)))
                    {
                        this.selectedNode.InternalMark++;
                        return true;
                    }

                    if ((!this.selectedNode.IsAppend && (this.selectedNode.firstChild != null)) &&
                        !this.selectedNode.firstChild.isGlyph)
                    {
                        this.SelectNode (this.selectedNode.firstChild, false);
                        if (!this.selectedNode.isVisible)
                        {
                            return this.GoRight ();
                        }
                        
                        if ((this.selectedNode.firstChild != null) && this.selectedNode.skip)
                        {
                            return this.GoRight ();
                        }
                        if (this.IsOpeningBracket (this.selectedNode) && (this.selectedNode.nextSibling != null))
                        {
                            return this.GoRight ();
                        }
                        return true;
                    }
                    if (this.selectedNode.nextSibling != null)
                    {
                        if (this.selectedNode.IsAppend)
                        {
                            if (this.IsHorizAlignmentElement (this.selectedNode.nextSibling))
                            {
                                this.SelectNode (this.selectedNode.nextSibling, false);
                                return this.GoRight ();
                            }
                            this.SelectNext ();
                            if (this.selectedNode.isVisible)
                            {
                                if ((!this.selectedNode.IsAppend && (this.selectedNode.firstChild != null)) &&
                                    this.selectedNode.skip)
                                {
                                    return this.GoRight ();
                                }
                                if (this.IsClosingBracket (this.selectedNode))
                                {
                                    this.SelectNode (this.selectedNode, true);
                                    return this.GoRight ();
                                }
                                return true;
                            }
                            this.selectedNode.IsAppend = true;
                            return this.GoRight ();
                        }
                        if (this.IsClosingBracket (this.selectedNode.nextSibling))
                        {
                            this.SelectNode (this.selectedNode, true);
                            return true;
                        }
                        this.SelectNext ();
                        if (this.selectedNode.isVisible)
                        {
                            if ((!this.selectedNode.IsAppend && (this.selectedNode.firstChild != null)) &&
                                this.selectedNode.skip)
                            {
                                return this.GoRight ();
                            }
                            return true;
                        }
                        this.selectedNode.IsAppend = true;
                        return this.GoRight ();
                    }
                    if (this.selectedNode.nextSibling == null)
                    {
                        if (this.selectedNode.isVisible && !this.selectedNode.IsAppend)
                        {
                            if (this.IsClosingBracket (this.selectedNode) &&
                                (this.selectedNode.parent_.type_.type == ElementType.Mrow))
                            {
                                this.SelectNode (this.selectedNode, true);
                                return this.GoRight ();
                            }
                            this.SelectNode (this.selectedNode, true);
                            return true;
                        }
                        
                        if (
                             
                            (((this.selectedNode.level == 1)) &&
                             (this.selectedNode.parent_.nextSibling == null)))
                        {
                            return false;
                        }
                        
                            try
                            {
                                if (((((this.selectedNode.parent_ != null) &&
                                       (this.selectedNode.parent_.type_.type == ElementType.Mtd)) &&
                                      ((this.selectedNode.parent_.nextSibling == null) &&
                                       (this.selectedNode.parent_.prevSibling == null))) &&
                                     (((this.selectedNode.parent_.parent_ != null) &&
                                       (this.selectedNode.parent_.parent_.type_.type ==
                                        ElementType.Mtr)) &&
                                      ((this.selectedNode.parent_.parent_.nextSibling == null) &&
                                       (this.selectedNode.parent_.parent_.parent_ != null)))) &&
                                    ((((this.selectedNode.parent_.parent_.parent_.type_.type ==
                                        ElementType.Mtable) &&
                                       (this.selectedNode.parent_.parent_.parent_.Class == "nugentoplevel")) &&
                                      ((this.selectedNode.parent_.parent_.parent_.level == 1) &&
                                       (this.selectedNode.parent_.parent_.parent_.nextSibling == null))) &&
                                     (this.selectedNode.parent_.parent_.parent_.prevSibling == null)))
                                {
                                    return false;
                                }
                            }
                            catch
                            {
                            }
                        
                        if (this.selectedNode.parent_ != null)
                        {
                            this.SelectNode (this.selectedNode.parent_, true);
                            if (this.selectedNode.skip)
                            {
                                if (((!this.selectedNode.IsAppend || (this.selectedNode.parent_ == null)) ||
                                     this.selectedNode.parent_.skip) ||
                                    (((this.selectedNode.parent_.type_.type != ElementType.Mover) &&
                                      (this.selectedNode.parent_.type_.type != ElementType.Munder)) &&
                                     (this.selectedNode.parent_.type_.type != ElementType.Munderover)))
                                {
                                    return this.GoRight ();
                                }
                                if ((this.selectedNode.parent_.nextSibling != null) &&
                                    !this.selectedNode.parent_.nextSibling.skip)
                                {
                                    this.SelectNode (this.selectedNode.parent_.nextSibling, false);
                                }
                                else
                                {
                                    this.SelectNode (this.selectedNode.parent_, true);
                                }
                                return true;
                            }
                            if (((this.selectedNode.nextSibling != null) &&
                                 this.NotInSameContainer (this.selectedNode, this.selectedNode.nextSibling)) &&
                                !this.IsClosingBracket (this.selectedNode.nextSibling))
                            {
                                this.SelectNode (this.selectedNode.nextSibling, false);
                                return true;
                            }
                            
                            return true;
                        }
                    }
                }
            }
            catch
            {
            }
            return false;
        }
        //
        private bool IsHorizAlignmentElement (Node node)
        {
            try
            {
                if (
                    (((((node.type_.type != ElementType.Maction) &&
                        (node.type_.type != ElementType.Menclose)) &&
                       ((node.type_.type != ElementType.Mfenced) &&
                        (node.type_.type != ElementType.Mfrac))) &&
                      (((node.type_.type != ElementType.Ms) &&
                        (node.type_.type != ElementType.Mmultiscripts)) &&
                       ((node.type_.type != ElementType.Mtext) &&
                        (node.type_.type != ElementType.Mn)))) &&
                     (((((node.type_.type != ElementType.Mo) &&
                         (node.type_.type != ElementType.Mover)) &&
                        ((node.type_.type != ElementType.Mpadded) &&
                         (node.type_.type != ElementType.Mphantom))) &&
                       (((node.type_.type != ElementType.Mroot) &&
                         (node.type_.type != ElementType.Msqrt)) &&
                        ((node.type_.type != ElementType.Msub) &&
                         (node.type_.type != ElementType.Msubsup)))) &&
                      (((node.type_.type != ElementType.Msup) &&
                        (node.type_.type != ElementType.Mi)) &&
                       (((node.type_.type != ElementType.Mtable) &&
                         (node.type_.type != ElementType.Munder)) &&
                        (node.type_.type != ElementType.Munderover))))))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        //
        public bool WordUp ()
        {
            this.ClearSelection ();
            try
            {
                Node cur = this.GetCurrentlySelectedNode ();
                if (cur != null)
                {
                    Node upper = this.FindUpper (cur);
                    
                    if (upper != null)
                    {
                        if (upper.type_.type == ElementType.Math)
                        {
                            if (upper.firstChild == null)
                            {
                                return false;
                            }
                            this.SelectNode (upper.firstChild, false);
                            return true;
                        }
                        
                        this.SelectNode (upper, false);
                        if (this.selectedNode.skip)
                        {
                            return this.GoRight ();
                        }
                        
                        return true;
                    }
                }
            }
            catch
            {
            }
            return false;
        }
        //
        public bool GoDown ()
        {
            this.ClearSelection ();
            try
            {
                if (this.selectedNode != null)
                {
                    
                    Node cur = this.GetCurrentlySelectedNode ();
                    Node lower = this.FindLower (cur);
                    
                    
                    if (lower != null)
                    {
                        if (lower.type_.type == ElementType.Math)
                        {
                            if (lower.firstChild == null)
                            {
                                return false;
                            }
                            this.SelectNode (lower.firstChild, false);
                            return true;
                        }
                       
                        this.SelectNode (lower, false);
                     
                        if (this.selectedNode.skip)
                        {
                            return this.GoRight ();
                        }
                        
                        return true;
                    }
                }
            }
            catch
            {
            }
            
            return false;
        }
        //
        public bool MoveSelectionPoint (SelectionDirection direction)
        {
            bool r = false;
            try
            {
                try
                {
                    switch (direction)
                    {
                        case SelectionDirection.Right:
                        {
                            r = this.SelectionRight();
                            break;
                        }
                        case SelectionDirection.Left:
                        {
                            r = this.SelectionLeft();
                            break;
                        }
                        case SelectionDirection.Up:
                        {
                            r = this.SelectionUp();
                            break;
                        }
                        case SelectionDirection.Down:
                        {
                            r = this.SelectionDown();
                            break;
                        }
                    }
                }
                catch
                {
                }
            }
            catch
            {
            }
            return r;
        }
        //
        private bool SelectionRight ()
        {
            try
            {
                Node cur = this.GetCurrentlySelectedNode ();
                if (cur == null)
                {
                    this.HasSelection = false;
                    return false;
                }
                if ((cur.level == 0))
                {
                    return false;
                }
                
                if (!this.HasSelection)
                {
                    this.HasSelection = true;
                }
                if (!this.selectedNode.skip && (this.selectedNode.InternalMark < (this.selectedNode.LiteralLength - 1)))
                {
                    {
                        this.selectedNode.InternalMark++;
                    }
                    return true;
                }
                if ((cur.nextSibling != null) && cur.nextSibling.isVisible)
                {
                    if (((cur.parent_ != null)) &&
                        ((cur.parent_.type_.minChilds > 1) ||
                         (cur.parent_.type_.type == ElementType.Mmultiscripts)))
                    {
                        if (cur.IsAppend)
                        {
                            return false;
                        }
                        this.SelectNode (cur, true);
                        return true;
                    }
                    if (cur.IsAppend)
                    {
                        this.SelectNode (cur.nextSibling, false);
                        return this.SelectionRight ();
                    }
                    this.SelectNode (cur.nextSibling, false);
                    return true;
                }
                if (((cur.nextSibling == null) || !cur.nextSibling.isVisible) && !cur.IsAppend)
                {
                    this.SelectNode (cur, true);
                    return true;
                }
                if ((cur.nextSibling != null) || !cur.IsAppend)
                {
                    return false;
                }
                
                if (!this.IsToplevelRowCell (cur))
                {
                    return true;
                }
                Node next = cur.parent_.parent_.nextSibling;
                if (next == null)
                {
                    return true;
                }
                Node nextFirst = next.firstChild;
                if ((nextFirst == null) || (nextFirst.firstChild == null))
                {
                    return true;
                }
                
                Node atom = nextFirst.firstChild;
                if (atom.IsAtom())
                {
                    if (atom.LiteralLength > 0)
                    {
                        if (!this.HasSelection)
                        {
                            this.HasSelection = true;
                        }
                        this.SelectNode (atom, false);
                        atom.InternalMark = 1;
                        
                        return true;
                    }
                    return false;
                }
                if (!this.HasSelection)
                {
                    this.HasSelection = true;
                }
                this.SelectNode (atom, true);
                
                return true;
            }
            catch
            {
            }
            
            return false;
        }
        //
        private bool SelectionLeft ()
        {
            try
            {
                Node cur = this.GetCurrentlySelectedNode ();
                if (cur == null)
                {
                    this.HasSelection = false;
                    return false;
                }
                if ((cur.level == 0))
                {
                    return false;
                }
                
                if (!this.HasSelection)
                {
                    this.HasSelection = true;
                }
                if (this.selectedNode.InternalMark > 0)
                {
                    this.selectedNode.InternalMark--;
                    return true;
                }
                if ((cur.prevSibling != null) && cur.prevSibling.isVisible)
                {
                    if (((cur.parent_ != null)) &&
                        ((cur.parent_.type_.minChilds > 1) ||
                         (cur.parent_.type_.type == ElementType.Mmultiscripts)))
                    {
                        return false;
                    }
                    this.SelectNode (cur.prevSibling, false);
                    if (cur.prevSibling.IsAtom() && (cur.prevSibling.LiteralLength > 1))
                    {
                        cur.prevSibling.InternalMark = cur.prevSibling.LiteralLength - 1;
                    }
                    return true;
                }
                if ((cur.prevSibling == null) && (cur.InternalMark == 0))
                {
                    if (this.IsToplevelRowCell (cur))
                    {
                        Node leftCont = cur.parent_.parent_.prevSibling;
                        if (leftCont != null)
                        {
                            Node leftFirst = leftCont.firstChild;
                            if ((leftFirst != null) && (leftFirst.firstChild != null))
                            {
                                Node leftLast = leftFirst.lastChild;
                                if (leftLast.IsAtom())
                                {
                                    if (leftLast.LiteralLength > 1)
                                    {
                                        if (!this.HasSelection)
                                        {
                                            this.HasSelection = true;
                                        }
                                        this.SelectNode (leftLast, false);
                                        leftLast.InternalMark = leftLast.LiteralLength - 1;
                                        
                                    }
                                    else
                                    {
                                        if (!this.HasSelection)
                                        {
                                            this.HasSelection = true;
                                        }
                                        this.SelectNode (leftLast, false);
                                        
                                    }
                                }
                                else
                                {
                                    if (!this.HasSelection)
                                    {
                                        this.HasSelection = true;
                                    }
                                    this.SelectNode (leftLast, false);
                                    
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return false;
        }
        //
        private bool SelectionUp ()
        {
            try
            {
                Node cur = this.GetCurrentlySelectedNode ();
                
                if (cur == null)
                {
                    this.HasSelection = false;
                    return false;
                }
                if (!this.IsToplevelRowCell (cur))
                {
                    return false;
                }
                Node prev = cur.parent_.parent_.prevSibling;
                if (prev == null)
                {
                    return false;
                }
                Node prevFirst = prev.firstChild;
                if ((prevFirst == null) || (prevFirst.firstChild == null))
                {
                    return false;
                }
                
                Node atom = prevFirst.firstChild;
                if (atom.IsAtom())
                {
                    if (atom.LiteralLength > 0)
                    {
                        if (!this.HasSelection)
                        {
                            this.HasSelection = true;
                        }
                        this.SelectNode (atom, false);
                        atom.InternalMark = 0;
                        return false;
                    }
                    return false;
                }
                if (!this.HasSelection)
                {
                    this.HasSelection = true;
                }
                this.SelectNode (atom, false);
                return false;
            }
            catch
            {
            }
            return false;
        }
        //
        private bool SelectionDown ()
        {
            try
            {
                Node cur = this.GetCurrentlySelectedNode ();
                if (cur == null)
                {
                    this.HasSelection = false;
                    return false;
                }
                if (!this.IsToplevelRowCell (cur))
                {
                    return false;
                }
                Node next = cur.parent_.parent_.nextSibling;
                if (next == null)
                {
                    return false;
                }
                Node nextFirst = next.firstChild;
                if ((nextFirst == null) || (nextFirst.firstChild == null))
                {
                    return false;
                }
                
                Node atom = nextFirst.firstChild;
                if (atom.IsAtom())
                {
                    if (atom.LiteralLength > 0)
                    {
                        if (!this.HasSelection)
                        {
                            this.HasSelection = true;
                        }
                        this.SelectNode (atom, false);
                        atom.InternalMark = 0;
                        
                        return false;
                    }
                    return false;
                }
                if (!this.HasSelection)
                {
                    this.HasSelection = true;
                }
                this.SelectNode (atom, false);
            }
            catch
            {
            }
            return false;
        }
        //
        public bool IsClosingBracket (Node node)
        {
            try
            {
                if (((node.nextSibling != null) || (node.parent_ == null)) ||
                    ((node.parent_.type_.type != ElementType.Mrow) ||
                     (node.type_.type != ElementType.Mo)))
                {
                    return false;
                }
                if ((node.literalText != null) && (node.literalText.Length > 0))
                {
                    if (((node.literalText != ")") && (node.literalText != "}")) &&
                        ((node.literalText != "]") && (node.literalText != "|")))
                    {
                        return false;
                    }
                    return true;
                }
                if (((node.firstChild == null) || (node.firstChild.type_.type != ElementType.Entity)) ||
                    ((((node.firstChild.glyph.Code != "0007D") && (node.firstChild.glyph.Code != "0005D")) &&
                      ((node.firstChild.glyph.Code != "00029") && (node.firstChild.glyph.Code != "0230B"))) &&
                     (((node.firstChild.glyph.Code != "0232A") && (node.firstChild.glyph.Code != "0007C")) &&
                      ((node.firstChild.glyph.Code != "02016") && (node.firstChild.glyph.Code != "02309")))))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        //
        public bool IsOpeningBracket (Node node)
        {
            try
            {
                if (((node.prevSibling != null) || (node.parent_ == null)) ||
                    ((node.parent_.type_.type != ElementType.Mrow) ||
                     (node.type_.type != ElementType.Mo)))
                {
                    return false;
                }
                if ((node.literalText != null) && (node.literalText.Length > 0))
                {
                    if (((node.literalText != "(") && (node.literalText != "{")) &&
                        ((node.literalText != "[") && (node.literalText != "|")))
                    {
                        return false;
                    }
                    return true;
                }
                if (((node.firstChild == null) || (node.firstChild.type_.type != ElementType.Entity)) ||
                    ((((node.firstChild.glyph.Code != "0007B") && (node.firstChild.glyph.Code != "0005B")) &&
                      ((node.firstChild.glyph.Code != "00028") && (node.firstChild.glyph.Code != "0230A"))) &&
                     (((node.firstChild.glyph.Code != "02329") && (node.firstChild.glyph.Code != "0007C")) &&
                      ((node.firstChild.glyph.Code != "02016") && (node.firstChild.glyph.Code != "02308")))))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        //
        private Node FindLower (Node pNode)
        {
            Node n = pNode;
            
            while ((n != null) && (n.lowerNode == null))
            {
                n = n.parent_;
            }
            
            if (n != null)
            {
                return n.lowerNode;
            }

            return null;
        }
        //
        private Node FindUpper (Node pNode)
        {
            Node n = pNode;
            
            while ((n != null) && (n.upperNode == null))
            {
                n = n.parent_;
            }

            if (n != null) 
            {
                return n.upperNode;
            }

            return null;
        }
        //
        private void SelectNext ()
        {
            bool differentContainer = false;
            if ((this.selectedNode.nextSibling != null) && this.NotInSameContainer (this.selectedNode, this.selectedNode.nextSibling))
            {
                differentContainer = true;
            }
            if (this.selectedNode.IsAppend)
            {
                if ((this.selectedNode.parent_ != null) &&
                    (((this.selectedNode.parent_.type_.type == ElementType.Mover) ||
                      (this.selectedNode.parent_.type_.type == ElementType.Munder)) ||
                     (this.selectedNode.parent_.type_.type == ElementType.Munderover)))
                {
                    this.SelectNode (this.selectedNode.parent_, true);
                }
                if ((differentContainer && !this.selectedNode.skip) &&
                    (this.selectedNode.nextSibling.isVisible && !this.selectedNode.nextSibling.skip))
                {
                    if (this.selectedNode.nextSibling.LiteralLength > 1)
                    {
                        this.SelectNode (this.selectedNode.nextSibling, false);
                        this.selectedNode.InternalMark = 1;
                    }
                    else
                    {
                        this.SelectNode (this.selectedNode.nextSibling, true);
                    }
                }
                else
                {
                    this.SelectNode (this.selectedNode.nextSibling, false);
                }
            }
            else if (differentContainer)
            {
                this.SelectNode (this.selectedNode.nextSibling, false);
            }
            else
            {
                this.SelectNode (this.selectedNode, true);
            }
        }
        //
        private bool AcceptsLetters ()
        {
            Node start = this.selectedNode;
            if ((start.type_.type != ElementType.Mrow) || (start.firstChild != null))
            {
                if (((start.tokenType == Tokens.TEXT) ||
                     (start.tokenType == Tokens.GLYPH)))
                {
                    return true;
                }
                if ((((!start.IsAppend && (start.InternalMark == 0)) && (start.prevSibling != null)) &&
                     ((start.prevSibling.tokenType == Tokens.TEXT) ||
                      (start.prevSibling.tokenType == Tokens.GLYPH))) &&
                    ( this.NotInSameContainer (start.prevSibling, start)))
                {
                    start = start.prevSibling;
                    this.SelectNode (start, true);
                    return true;
                }
                if (((!start.IsAppend && (start.InternalMark == 0)) && ((start.prevSibling == null) && (start.parent_ != null))) &&
                    start.parent_.skip)
                {
                    Node parent = start.parent_;
                    while ((parent.skip && (parent.parent_ != null)) && (parent.prevSibling == null))
                    {
                        parent = parent.parent_;
                    }
                    if (((parent.prevSibling != null) &&
                         ((parent.prevSibling.tokenType == Tokens.TEXT) ||
                          (parent.prevSibling.tokenType == Tokens.GLYPH))) &&
                        (this.NotInSameContainer (parent.prevSibling, parent)))
                    {
                        start = parent.prevSibling;
                        this.SelectNode (start, true);
                        return true;
                    }
                }
            }
            return false;
        }
        //
        private bool AcceptsDigits ()
        {
            Node start = this.selectedNode;
            if ((start.type_.type != ElementType.Mrow) || (start.firstChild != null))
            {
                if (start.tokenType == Tokens.NUMBER)
                {
                    return true;
                }
                if (((start.InternalMark == 0) && (start.prevSibling != null)) &&
                    ((start.prevSibling.tokenType == Tokens.NUMBER) && this.NotInSameContainer (start.prevSibling, start)))
                {
                    start = start.prevSibling;
                    this.SelectNode (start, true);
                    return true;
                }
                if (((start.InternalMark == 0) && (start.prevSibling == null)) &&
                    ((start.parent_ != null) && start.parent_.skip))
                {
                    Node parent = start.parent_;
                    while ((parent.skip && (parent.parent_ != null)) && (parent.prevSibling == null))
                    {
                        parent = parent.parent_;
                    }
                    if (((parent.prevSibling != null) && (parent.prevSibling.tokenType == Tokens.NUMBER)) &&
                        this.NotInSameContainer (parent.prevSibling, parent))
                    {
                        start = parent.prevSibling;
                        this.SelectNode (start, true);
                        return true;
                    }
                }
            }
            return false;
        }
        //
        private bool AcceptsScript ()
        {
            Node cur = this.GetCurrentlySelectedNode ();
            if ((cur.type_.type != ElementType.Mrow) || (cur.firstChild != null))
            {
                if (this.IsMathML (cur) && (cur.IsAppend || (cur.prevSibling == null)))
                {
                    return true;
                }
                if (((cur.InternalMark == 0) && (cur.prevSibling != null)) && this.IsMathML (cur.prevSibling))
                {
                    this.SelectNode (cur.prevSibling, true);
                    return true;
                }
                if (((cur.InternalMark == 0) && (cur.prevSibling == null)) &&
                    ((cur.parent_ != null) && cur.parent_.skip))
                {
                    Node par = cur.parent_;
                    while ((par.skip && (par.parent_ != null)) && (par.prevSibling == null))
                    {
                        par = par.parent_;
                    }
                    if ((par.prevSibling != null) && this.IsMathML (par.prevSibling))
                    {
                        this.SelectNode (par.prevSibling, true);
                        return true;
                    }
                }
            }
            return false;
        }
        //
        public void OnInsert (bool canOverwrite)
        {
            if (canOverwrite && this.HasSelection)
            {
                this.DoDelete();
            }

            this.CaptureUndo();
            this.CanUndo = true;
        }
        //
        public bool ApplyFencedAttributes (Node node, FencedAttributes FencedAttributes)
        {
            try
            {
                this.OnInsert (false);
                AttributeBuilder.ApplyAttrs (node, FencedAttributes);
            }
            catch
            {
            }
            return true;
        }
        //
        public bool ApplyFractionAttrs (Node node, FractionAttributes FractionAttributes)
        {
            try
            {
                this.OnInsert (false);
                AttributeBuilder.ApplyAttrs (node, FractionAttributes);
            }
            catch
            {
            }
            return true;
        }
        //
        public bool ApplyActionAttrs (Node node, ActionAttributes ActionAttributes, string statusLine)
        {
            try
            {
                this.OnInsert (false);
                if ((ActionAttributes.actionType == ActionType.StatusLine) ||
                    (ActionAttributes.actionType == ActionType.ToolTip))
                {
                    try
                    {
                        if (node.HasChildren ())
                        {
                            Node first = node.firstChild;
                            if ((first != null) && (first.nextSibling != null))
                            {
                                first = first.nextSibling;
                                if (first.type_.type == ElementType.Mtext)
                                {
                                    first.literalText = statusLine;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                if ((node != null) && (ActionAttributes != null))
                {
                    AttributeBuilder.ApplyAttrs (node, ActionAttributes);
                }
            }
            catch
            {
            }
            return true;
        }
        //
        public bool ApplyMatrixProperties (MTable Matrix)
        {
            try
            {
                this.OnInsert (false);
                Matrix.ApplyAttrs ();
            }
            catch
            {
            }
            return true;
        }
        //
        public void clear ()
        {
            try
            {
                this.undo.Clear ();
                this.redo.Clear ();
            }
            catch
            {
            }
            this.rootNode_ = null;
            this.selectedNode = null;
            this.lastSelectedNode = null;
            this.hasSelection = false;
            this.currentCaret = 0;
            this.oX = 0;
            this.oY = 0;
            this.HasSelection = false;
        }
        //
        public void setFonts (FontCollection FontCollection)
        {
            this.fonts_ = FontCollection;
            if (this.painter_ != null)
            {
                this.painter_.setFonts (this.fonts_);
            }
        }
        //
        public bool InsertSupScript ()
        {
            this.CanUndo = true;
            Selection sel = null;
            bool can = false;
            if (this.HasSelection)
            {
                sel = this.CaptureSelection ();
                if ((sel != null))
                {
                    can = true;
                }
            }
            if (can)
            {
                this.InsertSuperScript ();
                return true;
            }
            
            this.HasSelection = false;
            if (!this.ScriptEligible ())
            {
                return false;
            }
            if (!this.AcceptsScript ())
            {
                return false;
            }
            Node cur = this.GetCurrentlySelectedNode ();
            if ((cur != null) && this.IsMathML (cur))
            {
                this.OnInsert (false);
                Node newnode = new Node ();
                cur.CopyTo (newnode);
                newnode.IsAppend = cur.IsAppend;
                newnode.InternalMark = cur.InternalMark;
                Node msup = this.MakeNode ("msup");
                cur.parent_.ReplaceChild (cur, msup);
                msup.AdoptChild (newnode);
                Node row = this.CreateRow ();
                msup.AdoptChild (row);
                this.SelectNode (row, false);
                try
                {
                    this.InsertHappened ();
                }
                catch
                {
                }
            }
            return true;
        }
        //
        public bool InsertSubSupScript ()
        {
            this.CanUndo = true;
            Selection sel = null;
            bool hasSel = false;
            if (this.HasSelection)
            {
                sel = this.CaptureSelection ();
                if ((sel != null))
                {
                    hasSel = true;
                }
            }
            if (hasSel)
            {
                this.InsertSubSup ();
                return true;
            }
            this.HasSelection = false;
            if (!this.ScriptEligible ())
            {
                return false;
            }
            if (!this.AcceptsScript ())
            {
                return false;
            }
            Node cur = this.GetCurrentlySelectedNode ();
            if ((cur != null) && this.IsMathML (cur))
            {
                this.OnInsert (false);
                Node newnode = new Node ();
                cur.CopyTo (newnode);
                newnode.IsAppend = cur.IsAppend;
                newnode.InternalMark = cur.InternalMark;
                Node subnode = this.MakeNode ("msubsup");
                cur.parent_.ReplaceChild (cur, subnode);
                subnode.AdoptChild (newnode);
                Node row = this.CreateRow ();
                subnode.AdoptChild (row);
                Node row2 = this.CreateRow ();
                subnode.AdoptChild (row2);
                this.SelectNode (row, false);
                try
                {
                    this.InsertHappened ();
                }
                catch
                {
                }
            }
            return true;
        }
        //
        public bool InsertSubScript ()
        {
            this.CanUndo = true;
            Selection selection = null;
            bool hasSel = false;
            if (this.HasSelection)
            {
                selection = this.CaptureSelection ();
                if ((selection != null))
                {
                    hasSel = true;
                }
            }
            if (hasSel)
            {
                this.InsertSubscript ();
                return true;
            }
            this.HasSelection = false;
            if (!this.ScriptEligible ())
            {
                return false;
            }
            if (!this.AcceptsScript ())
            {
                return false;
            }
            Node cur = this.GetCurrentlySelectedNode ();
            if ((cur != null) && this.IsMathML (cur))
            {
                this.OnInsert (false);
                Node newnode = new Node ();
                cur.CopyTo (newnode);
                newnode.IsAppend = cur.IsAppend;
                newnode.InternalMark = cur.InternalMark;
                Node sub = this.MakeNode ("msub");
                cur.parent_.ReplaceChild (cur, sub);
                sub.AdoptChild (newnode);
                Node row = this.CreateRow ();
                sub.AdoptChild (row);
                this.SelectNode (row, false);
                try
                {
                    this.InsertHappened ();
                }
                catch
                {
                }
            }
            return true;
        }
        //
        public void SetSchemas (string MathMLSchema)
        {
            this.mathmlSchema = MathMLSchema;
        }
        //
        public bool EnterPressed_NeedSplit ()
        {
            try
            {
                this.OnInsert (true);
                return this.EnterPressed_NeedSplit (true);
            }
            catch
            {
                return false;
            }
        }
        //
        private bool EnterPressed_NeedSplit (bool splitCell)
        {
            bool isTop = false;
            try
            {
                Node cur = this.GetCurrentlySelectedNode ();
                if (((cur == null)) || (cur.type_.type == ElementType.Math))
                {
                    return isTop;
                }
                try
                {
                    bool isLast = false;
                    bool isfirst = false;
                    Node td = null;
                    Node c = cur;
                    bool isMath = false;
                    while (((c.parent_ != null) && !isMath) && (c.type_.type != ElementType.Mtd))
                    {
                        if (c.type_.type == ElementType.Math)
                        {
                            isMath = true;
                            continue;
                        }
                        td = c;
                        c = c.parent_;
                    }
                    if ((c != null) && (c.type_.type == ElementType.Mtd))
                    {
                        Node cell = c;
                        if (cell.nextSibling == null)
                        {
                            isLast = true;
                        }
                        if (((cell.nextSibling != null) && (cell.prevSibling == null)) &&
                            ((cell.firstChild == cur) && (cur.InternalMark == 0)))
                        {
                            isfirst = true;
                        }
                        if ((cell.parent_ != null) && (cell.parent_.type_.type == ElementType.Mtr))
                        {
                            Node row = cell.parent_;
                            int numCols = row.numChildren;
                            string xml = "<math xmlns=\"http://www.w3.org/1998/Math/MathML\">";
                            xml = xml + "<mtable columnalign=\"left\">";
                            xml = xml + "<mtr>";
                            for (int i = 0; i < numCols; i++)
                            {
                                if (i == 0)
                                {
                                    xml = xml + "<mtd nugenCursor=''/>";
                                }
                                else
                                {
                                    xml = xml + "<mtd><mrow/></mtd>";
                                }
                            }
                            xml = xml + "</mtr>";
                            xml = xml + "</mtable>";
                            xml = xml + "</math>";
                            XmlDocument doc = new XmlDocument ();
                            doc.LoadXml (xml);
                            XmlNode topnode = doc.DocumentElement.FirstChild;
                            Node newNode = new Node ();
                            Node newSelected = newNode.Parse (topnode, this.types_, this.entityManager, true, null);
                            Node toSel = null;
                            if (newNode.HasChildren ())
                            {
                                newNode = newNode.firstChild;
                            }
                            if (newNode.type_.type != ElementType.Mtr)
                            {
                                return isTop;
                            }
                            if (isfirst)
                            {
                                row.PrependNode (newNode);
                                this.PropogateAttributes (row, newNode);
                                if ((newSelected.type_.type == ElementType.Mtd) && (newSelected.numChildren == 0))
                                {
                                    newSelected.AdoptChild (this.CreateRow ());
                                }
                                this.SelectNode (cur, false);
                                return isTop;
                            }
                            row.AppendNode (newNode);
                            this.PropogateAttributes (row, newNode);
                            if (newSelected == null)
                            {
                                return isTop;
                            }
                            if (splitCell && isLast)
                            {
                                if (td != null)
                                {
                                    Node cel = td;
                                    toSel = newSelected;
                                    int num = 0;
                                    if ((cel.InternalMark == 0) && (cel.prevSibling == null))
                                    {
                                        Node rrow = cel.parent_;
                                        while (rrow.numChildren > 0)
                                        {
                                            this.ReParent (rrow.firstChild.parent_, newSelected, rrow.firstChild);
                                            if (num == 0)
                                            {
                                                toSel = rrow.firstChild;
                                            }
                                            num++;
                                        }
                                        if ((rrow.type_.type == ElementType.Mtd) &&
                                            (rrow.numChildren == 0))
                                        {
                                            rrow.AdoptChild (this.CreateRow ());
                                        }
                                    }
                                    else
                                    {
                                        bool wasSplit = false;
                                        if (cel.InternalMark == 0)
                                        {
                                            cel = cel.prevSibling;
                                        }
                                        else if (!cel.IsAppend && (cel.literalText.Length > 1))
                                        {
                                            this.CarriageReturn (cel, ref wasSplit);
                                        }
                                        while (cel.nextSibling != null)
                                        {
                                            this.ReParent (cel.nextSibling.parent_, newSelected, cel.nextSibling);
                                            if (num == 0)
                                            {
                                                toSel = cel.nextSibling;
                                            }
                                            num++;
                                        }
                                    }
                                }
                                if (toSel != null)
                                {
                                    this.SelectNode (toSel, false);
                                    if (toSel.type_.type == ElementType.Mtd)
                                    {
                                        if (toSel.firstChild != null)
                                        {
                                            this.SelectNode (toSel.firstChild, false);
                                        }
                                        else
                                        {
                                            toSel.AdoptChild (this.CreateRow ());
                                            toSel.UpdateChildrenIndices ();
                                            toSel.UpdateLevel ();
                                            this.SelectNode (toSel.firstChild, false);
                                        }
                                    }
                                }
                                if (((newSelected != null) && (newSelected.type_.type == ElementType.Mtd)) &&
                                    (newSelected.numChildren == 0))
                                {
                                    newSelected.AdoptChild (this.CreateRow ());
                                    newSelected.UpdateChildrenIndices ();
                                    newSelected.UpdateLevel ();
                                    this.SelectNode (newSelected.firstChild, false);
                                }
                                return isTop;
                            }
                            if (newSelected.firstChild != null)
                            {
                                this.SelectNode (newSelected.firstChild, false);
                                return isTop;
                            }
                            newSelected.AdoptChild (this.CreateRow ());
                            if (newSelected.firstChild != null)
                            {
                                this.SelectNode (newSelected.firstChild, false);
                            }
                        }
                        return isTop;
                    }
                    this.MakeTopTable ();
                    return isTop;
                }
                finally
                {
                    this.SelectCell ();
                    isTop = true;
                }
            }
            catch
            {
            }
            return isTop;
        }
        //
        private void BackspaceCell ()
        {
            try
            {
                bool empty = false;
                Node cur = this.GetCurrentlySelectedNode ();
                Node par = cur;
                if (((cur.type_.type == ElementType.Mrow) && (cur.nextSibling == null)) &&
                    ((cur.prevSibling == null) && (cur.numChildren == 0)))
                {
                    empty = true;
                }
                while ((par.parent_ != null) && (par.type_.type != ElementType.Mtd))
                {
                    par = par.parent_;
                }
                if ((par == null) || (par.type_.type != ElementType.Mtd))
                {
                    return;
                }
                Node cell = par;
                if ((par.parent_ == null) || (par.parent_.type_.type != ElementType.Mtr))
                {
                    return;
                }
                Node row = cell.parent_;
                Node table = row.parent_;
                Node prev = row.prevSibling;
                if (prev == null)
                {
                    return;
                }
                this.OnInsert (true);
                Node prevCell = prev.firstChild;
                Node prevAtom = null;
                if ((((cell.numChildren > 0) && (prevCell != null)) &&
                     ((prevCell.numChildren == 1) && (prevCell.firstChild.type_.type == ElementType.Mrow))) &&
                    (prevCell.firstChild.numChildren == 0))
                {
                    prevAtom = prevCell.firstChild;
                }
                Node fc = null;
                this.SelectNode (prev, false);
                for (int i = 0; cell.numChildren > 0; i++)
                {
                    Node child = cell.firstChild;
                    this.ReParent (cell, prevCell, child);
                    if (i == 0)
                    {
                        fc = child;
                    }
                }
                bool isLast = false;
                if (row.nextSibling == null)
                {
                    isLast = true;
                }
                this.Tear (row, false, false);
                if (isLast)
                {
                    this.ChopUpperLowerNodes (table);
                }
                if (empty && (fc.prevSibling != null))
                {
                    if (((fc.prevSibling.type_.type == ElementType.Mrow) &&
                         (fc.prevSibling.prevSibling == null)) && (fc.prevSibling.numChildren == 0))
                    {
                        this.SelectNode (fc.prevSibling, false);
                    }
                    else
                    {
                        this.SelectNode (fc.prevSibling, true);
                    }
                    this.Tear (fc, false, false);
                }
                else
                {
                    this.SelectNode (fc, false);
                }
                if (prevAtom != null)
                {
                    this.Tear (prevAtom, false, false);
                }
            }
            finally
            {
                this.SelectCell ();
                this.ReparentCell ();
            }
        }
        //
        private void MakeTopTable ()
        {
            Node root = this.FindRoot ();
            if (root != null)
            {
                bool append = false;
                bool isLast = false;
                Node top = null;
                bool wasSplit = false;
                Node cur = this.GetCurrentlySelectedNode ();
                top = this.FindRootChild ();
                if (((top != null) && (top.parent_ != null)) && (top.parent_ == root))
                {
                    if (top != cur)
                    {
                        this.SelectNode (top, false);
                        cur = this.GetCurrentlySelectedNode ();
                    }
                    if (((cur.InternalMark > 0) && !cur.IsAppend) && ((cur.literalText != null) && (cur.literalText.Length > 1)))
                    {
                        cur = this.CarriageReturn (cur, ref wasSplit);
                    }
                    append = cur.IsAppend;
                    if (append)
                    {
                        if (cur.nextSibling == null)
                        {
                            isLast = true;
                        }
                        else if (cur == top)
                        {
                            this.SelectNode (cur.nextSibling, false);
                            cur = this.GetCurrentlySelectedNode ();
                            top = cur;
                            append = false;
                        }
                    }
                    string xml =  "<math xmlns=\"http://www.w3.org/1998/Math/MathML\">";
                    xml += "<mtable columnalign=\"left\" class=\"nugentoplevel\">";
                    xml = xml + "<mtr>";
                    xml = xml + "<mtd><mrow></mrow></mtd>";
                    xml = xml + "</mtr>";
                    xml = xml + "<mtr>";
                    xml = xml + "<mtd nugenCursor=''><mrow></mrow></mtd>";
                    xml = xml + "</mtr>";
                    xml = xml + "</mtable>";
                    xml = xml + "</math>";
                    XmlDocument doc = new XmlDocument ();
                    doc.LoadXml (xml);
                    XmlNode first = doc.DocumentElement.FirstChild;
                    Node r = new Node ();
                    Node sel = r.Parse (first, this.types_, this.entityManager, true, null);
                    Node last = r;
                    Node next = sel;
                    Node prev = next.parent_.prevSibling.firstChild;
                    if ((((last != null) && (last.type_.type == ElementType.Mtable)) &&
                         ((prev != null) && (next != null))) &&
                        (((prev.type_.type == ElementType.Mtd) &&
                          (next.type_.type == ElementType.Mtd)) && (root.numChildren > 0)))
                    {
                        root.firstChild.PrependNode (last);
                        root.UpdateChildrenIndices ();
                        root.UpdateLevel ();
                        int count = 0;
                        while (last.nextSibling != null)
                        {
                            Node ns = last.nextSibling;
                            if (!append)
                            {
                                if (ns == cur)
                                {
                                    count++;
                                }
                            }
                            else if (ns == cur.nextSibling)
                            {
                                count++;
                            }
                            if (count == 0)
                            {
                                this.ReParent (ns.parent_, prev, ns);
                                continue;
                            }
                            this.ReParent (ns.parent_, next, ns);
                        }
                        Node pc = prev.firstChild;
                        Node nc = next.firstChild;
                        if (((pc != null) && (pc.type_.type == ElementType.Mrow)) &&
                            ((pc.numChildren == 0) && (pc.nextSibling != null)))
                        {
                            this.Tear (pc, false, false);
                        }
                        if (((nc != null) && (nc.type_.type == ElementType.Mrow)) &&
                            ((nc.numChildren == 0) && (nc.nextSibling != null)))
                        {
                            this.SelectNode (nc.nextSibling, false);
                            this.Tear (nc, false, false);
                        }
                        if (isLast)
                        {
                            this.SelectNode (next.firstChild, false);
                        }
                        else
                        {
                            this.SelectNode (cur, false);
                        }
                    }
                }
            }
        }
        //
        private void ReparentCell ()
        {
            try
            {
                Node cur = this.GetCurrentlySelectedNode ();
                Node par = null;
                Node first = null;
                Node sib = null;
                Node cell = null;
                Node target = null;
                
                par = cur;
                while ((par.parent_ != null) && (par.type_.type != ElementType.Math))
                {
                    par = par.parent_;
                }
                if (par.type_.type != ElementType.Math)
                {
                    return;
                }
                first = par;
                if ((first.firstChild == null) || (first.firstChild.type_.type != ElementType.Mtable))
                {
                    return;
                }
                sib = first.firstChild;
                if (((sib.nextSibling != null) || (sib.firstChild == null)) ||
                    (sib.firstChild.type_.type != ElementType.Mtr))
                {
                    return;
                }
                cell = sib.firstChild;
                if (((cell.nextSibling != null) || (cell.prevSibling != null)) ||
                    ((cell.firstChild == null) || (cell.firstChild.type_.type != ElementType.Mtd)))
                {
                    return;
                }
                target = cell.firstChild;
                if ((target.nextSibling != null) || (target.prevSibling != null))
                {
                    return;
                }
                while (target.numChildren > 0)
                {
                    this.ReParent (target, first, target.firstChild);
                }
                this.Tear (sib, false, false);
            }
            catch
            {
            }
        }
        //
        private bool DeleteCell (Node node)
        {
            try
            {
                if ((!this.HasSelection && (node.parent_ != null)) && this.IsEmptyCell (node.parent_))
                {
                    Node row = node.parent_.parent_;
                    if (row.type_.type != ElementType.Mtr)
                    {
                        return false;
                    }
                    Node table = row.parent_;
                    if ((row.nextSibling == null) || !this.IsEmptyRow (row))
                    {
                        return false;
                    }
                    bool isFirst = false;
                    this.GoDown ();
                    if (row.prevSibling == null)
                    {
                        isFirst = true;
                    }
                    this.Tear (row, false, false);
                    if (isFirst)
                    {
                        this.ChopUpperLowerNodes (table);
                    }
                    this.ReparentCell ();
                    return true;
                }
                if (((!this.HasSelection && (node.parent_ != null)) && (node.IsAppend && (node.nextSibling == null))) &&
                    (((node.parent_.type_.type == ElementType.Mtd) &&
                      (node.parent_.nextSibling == null)) && (node.parent_.prevSibling == null)))
                {
                    
                    Node cell = null;
                    Node row = null;
                    Node nextRow = null;
                    row = node.parent_.parent_;
                    if ((row.type_.type == ElementType.Mtr) && (row.nextSibling != null))
                    {
                        nextRow = row.nextSibling;
                        if (nextRow.firstChild != null)
                        {
                            cell = nextRow.firstChild;
                            if (cell.firstChild != null)
                            {
                                this.SelectNode (cell.firstChild, false);
                                this.BackspaceCell ();
                                this.ReparentCell ();
                                return true;
                            }
                        }
                    }
                }
            }
            finally
            {
                this.SelectCell ();
            }
            return false;
        }
        //
        private Node FindRootChild ()
        {
            Node r = null;
            try
            {
                Node cur = this.GetCurrentlySelectedNode ();
                while ((cur.parent_ != null) && (cur.parent_.type_.type != ElementType.Math))
                {
                    cur = cur.parent_;
                }
                if ((cur.parent_ != null) && (cur.parent_.type_.type == ElementType.Math))
                {
                    r = cur;
                }
            }
            catch
            {
            }
            return r;
        }
        //
        private Node FindRoot ()
        {
            Node r = null;
            try
            {
                Node cur = this.GetCurrentlySelectedNode ();
                while ((cur.parent_ != null) && (cur.type_.type != ElementType.Math))
                {
                    cur = cur.parent_;
                }
                if ((cur != null) && (cur.type_.type == ElementType.Math))
                {
                    r = cur;
                }
            }
            catch
            {
            }
            return r;
        }
        //
        private bool IsEmptyCell (Node node)
        {
            bool r = false;
            try
            {
                if (node.type_.type != ElementType.Mtd)
                {
                    return r;
                }
                if (node.HasChildren ())
                {
                    if (((node.numChildren == 1) && (node.firstChild.type_.type == ElementType.Mrow)) &&
                        (node.firstChild.numChildren == 0))
                    {
                        r = true;
                    }
                    return r;
                }
                return true;
            }
            catch
            {
                return r;
            }
        }
        //
        private bool IsEmptyRow (Node trNode)
        {
            bool r = false;
            try
            {
                if (trNode.type_.type != ElementType.Mtr)
                {
                    return r;
                }
                NodesList list = trNode.GetChildrenNodes ();
                int i = 0;
                while ((i < list.Count) && this.IsEmptyCell (list.Get (i)))
                {
                    i++;
                }
                if (i == list.Count)
                {
                    r = true;
                }
            }
            catch
            {
            }
            return r;
        }
        //
        private void ReParent (Node oldParent, Node newParent, Node node)
        {
            try
            {
                Node prev = null;
                Node next = null;
                prev = node.prevSibling;
                next = node.nextSibling;
                if (prev != null)
                {
                    prev.nextSibling = next;
                }
                if (next != null)
                {
                    next.prevSibling = prev;
                }
                if (oldParent.firstChild == node)
                {
                    oldParent.firstChild = next;
                }
                if (oldParent.lastChild == node)
                {
                    oldParent.lastChild = prev;
                }
                node.parent_ = newParent;
                node.prevSibling = newParent.lastChild;
                if (newParent.lastChild != null)
                {
                    newParent.lastChild.nextSibling = node;
                }
                node.nextSibling = null;
                newParent.lastChild = node;
                if (node.prevSibling == null)
                {
                    newParent.firstChild = node;
                }
                oldParent.numChildren--;
                newParent.numChildren++;
                oldParent.UpdateChildrenIndices ();
                oldParent.UpdateLevel ();
                newParent.UpdateChildrenIndices ();
                newParent.UpdateLevel ();
            }
            catch
            {
            }
        }
        //
        private void PropogateAttributes (Node trNode1, Node trNode2)
        {
            try
            {
                if ((trNode1.type_.type != ElementType.Mtr) ||
                    (trNode2.type_.type != ElementType.Mtr))
                {
                    return;
                }
                NodesList list1 = trNode1.GetChildrenNodes ();
                NodesList list2 = trNode2.GetChildrenNodes ();
                if (list1.Count != list2.Count)
                {
                    return;
                }
                for (int i = 0; i < list1.Count; i++)
                {
                    if (i < list2.Count)
                    {
                        Node child1 = list1.Get (i);
                        Node child2 = list2.Get (i);
                        if (child1.attrs != null)
                        {
                            child1.attrs.Reset ();
                            try
                            {
                                for (int j = 0; j < child1.attrs.Count; j++)
                                {
                                    Nodes.Attribute attr1 = child1.attrs.Next ();
                                    Nodes.Attribute attr2 = new Nodes.Attribute (attr1.name, attr1.val, attr1.ns);
                                    if (child2.attrs == null)
                                    {
                                        child2.attrs = new AttributeList ();
                                    }
                                    child2.attrs.Add (attr2);
                                }
                            }
                            catch
                            {
                            }
                            child1.attrs.Reset ();
                            if (child2.attrs != null)
                            {
                                child2.attrs.Reset ();
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }
        //
        private void SelectCell ()
        {
            try
            {
                Node cur = this.GetCurrentlySelectedNode ();
                if (cur.type_.type != ElementType.Mtd)
                {
                    return;
                }
                if (cur.firstChild != null)
                {
                    this.SelectNode (cur.firstChild, false);
                }
                else
                {
                    Node row = this.CreateRow ();
                    cur.AdoptChild (row);
                    if (cur.firstChild != null)
                    {
                        this.SelectNode (cur.firstChild, false);
                    }
                }
            }
            catch
            {
            }
        }
        //
        private void ChopUpperLowerNodes (Node tableNode)
        {
            try
            {
                
                if (tableNode.firstChild != null)
                {
                    NodesList list = tableNode.firstChild.GetChildrenNodes ();
                    for (int i = 0; i < list.Count; i++)
                    {
                        list.Get (i).upperNode = null;
                    }
                }
                if (tableNode.lastChild == null)
                {
                    return;
                }
                NodesList l = tableNode.lastChild.GetChildrenNodes ();
                for (int i = 0; i < l.Count; i++)
                {
                    l.Get (i).lowerNode = null;
                }
            }
            catch
            {
            }
        }
        //
        private bool IsStretchy (Glyph entity)
        {
            return this.IsStretchy (entity.Code);
        }
        //
        private bool IsStretchy (string sUnicode)
        {
            if (((((sUnicode != "00028") && (sUnicode != "00029")) && ((sUnicode != "0007B") && (sUnicode != "0007D"))) &&
                 (((sUnicode != "0005B") && (sUnicode != "0005D")) && ((sUnicode != "0007C") && (sUnicode != "02016")))) &&
                ((((sUnicode != "0230A") && (sUnicode != "0230B")) && ((sUnicode != "02308") && (sUnicode != "02309"))) &&
                 ((sUnicode != "02329") && (sUnicode != "0232A"))))
            {
                return false;
            }
            return true;
        }
        //
        public bool Space ()
        {
            bool r = this.InsertSpace ();
            try
            {
                this.InsertHappened ();
            }
            catch
            {
            }
            return r;
        }
        //
        private bool InsertSpace ()
        {
            this.OnInsert (true);
            try
            {
                Node cur = this.GetCurrentlySelectedNode ();

                if (!this.ScriptEligible ())
                {
                    return false;
                }
                cur = this.GetCurrentlySelectedNode ();
                if (cur != null)
                {
                    
                        this.insertMathML (false,
                                   "<math xmlns='http://www.w3.org/1998/Math/MathML'><mspace nugenCursorEnd='' width='mediummathspace' height='0.2em'/></math>");
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        //
        public bool InsertChar (char sKey, bool bStretchyBrackets, bool bAutoClosingBrackets)
        {
            bool r = false;
            r = this.InsertCharacter (sKey, bStretchyBrackets, bAutoClosingBrackets);
            try
            {
                this.InsertHappened ();
            }
            catch
            {
            }
            return r;
        }
        //
        private bool InsertCharacter (char sKey, bool bStretchyBrackets, bool bAutoClosingBrackets)
        {
            Node node = null;
            bool canInsert = false;
            if (this.HasSelection)
            {
                try
                {
                    this.CaptureSelection();
                    canInsert = true;
                }
                catch
                {
                }
            }
            else
            {
                node = this.GetCurrentlySelectedNode ();
                if ((node != null) && (node.type_ != null))
                {
                    {
                        canInsert = true;
                    }
                }
            }
            node = this.GetCurrentlySelectedNode ();

            if (canInsert)
            {
                if (sKey == '+')
                {
                    this.insertMathML (
                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&plus;</mo></math>");
                    return true;
                }
                if (sKey == '-')
                {
                    this.insertMathML (
                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&minus;</mo></math>");
                    return true;
                }
                if (sKey == '<')
                {
                    this.insertMathML (
                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&lt;</mo></math>");
                    return true;
                }
                if (sKey == '>')
                {
                    this.insertMathML (
                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&gt;</mo></math>");
                    return true;
                }
                if (sKey == '&')
                {
                    this.insertMathML (
                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&amp;</mo></math>");
                    return true;
                }
                if (sKey == '%')
                {
                    this.insertMathML ("<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>%</mo></math>");
                    return true;
                }
                if (sKey == '@')
                {
                    this.insertMathML ("<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>@</mo></math>");
                    return true;
                }
                
                if (sKey == '`')
                {
                    return true;
                }
                if (sKey == '_')
                {
                    this.insertMathML (
                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&lowbar;</mo></math>");
                    return true;
                }
                if (sKey == '?')
                {
                    this.insertMathML ("<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>?</mo></math>");
                    return true;
                }
                if (sKey == '/')
                {
                    this.insertMathML (
                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&sol;</mo></math>");
                    return true;
                }
                if (sKey == '*')
                {
                    this.insertMathML (
                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&times;</mo></math>");
                    return true;
                }
                if (sKey == '=')
                {
                    this.insertMathML (
                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&equals;</mo></math>");
                    return true;
                }
                if (sKey == ',')
                {
                    this.insertMathML (
                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&comma;</mo></math>");
                    return true;
                }
                if (sKey == '!')
                {
                    this.insertMathML (
                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&excl;</mo></math>");
                    return true;
                }
                if (sKey == '"')
                {
                    this.insertMathML (
                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&quot;</mo></math>");
                    return true;
                }
                if (sKey == '\'')
                {
                    this.insertMathML (
                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&apos;</mo></math>");
                    return true;
                }
                if (sKey == '#')
                {
                    this.insertMathML (
                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&num;</mo></math>");
                    return true;
                }
                if (sKey == '~')
                {
                    this.insertMathML (
                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&sim;</mo></math>");
                    return true;
                }
                if (sKey == ':')
                {
                    this.insertMathML ("<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>:</mo></math>");
                    return true;
                }
                if (sKey == ';')
                {
                    this.insertMathML ("<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>;</mo></math>");
                    return true;
                }
                if (sKey == '\x00a7')
                {
                    this.insertMathML (
                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&sect;</mo></math>");
                    return true;
                }
                if (sKey == '|')
                {
                    if (bStretchyBrackets)
                    {
                        this.insertMathML (
                            "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>|</mo></math>");
                    }
                    else
                    {
                        this.insertMathML (
                            "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo stretchy=\"false\" nugenCursorEnd=''>|</mo></math>");
                    }
                    return true;
                }
                if (sKey == '(')
                {
                    if (bAutoClosingBrackets)
                    {
                        if (bStretchyBrackets)
                        {
                            this.InsertFenced ("lpar", "rpar", true);
                        }
                        else
                        {
                            this.InsertFenced ("lpar", "rpar", false);
                        }
                    }
                    else if (bStretchyBrackets)
                    {
                        this.insertMathML (
                            "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&lpar;</mo></math>");
                    }
                    else
                    {
                        this.insertMathML (
                            "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo stretchy=\"false\" nugenCursorEnd=''>&lpar;</mo></math>");
                    }
                    return true;
                }
                if (sKey == '{')
                {
                    if (bAutoClosingBrackets)
                    {
                        if (bStretchyBrackets)
                        {
                            this.InsertFenced ("lbrace", "rbrace", true);
                        }
                        else
                        {
                            this.InsertFenced ("lbrace", "rbrace", false);
                        }
                    }
                    else if (bStretchyBrackets)
                    {
                        this.insertMathML (
                            "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&lbrace;</mo></math>");
                    }
                    else
                    {
                        this.insertMathML (
                            "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo stretchy=\"false\" nugenCursorEnd=''>&lbrace;</mo></math>");
                    }
                    return true;
                }
                if (sKey == '[')
                {
                    if (bAutoClosingBrackets)
                    {
                        if (bStretchyBrackets)
                        {
                            this.InsertFenced ("lbrack", "rbrack", true);
                        }
                        else
                        {
                            this.InsertFenced ("lbrack", "rbrack", false);
                        }
                    }
                    else if (bStretchyBrackets)
                    {
                        this.insertMathML (
                            "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&lbrack;</mo></math>");
                    }
                    else
                    {
                        this.insertMathML (
                            "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo stretchy=\"false\" nugenCursorEnd=''>&lbrack;</mo></math>");
                    }
                    return true;
                }
                if (sKey == ')')
                {
                    if (bStretchyBrackets)
                    {
                        this.insertMathML (
                            "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&rpar;</mo></math>");
                    }
                    else
                    {
                        this.insertMathML (
                            "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo stretchy=\"false\" nugenCursorEnd=''>&rpar;</mo></math>");
                    }
                    return true;
                }
                if (sKey == '}')
                {
                    if (bStretchyBrackets)
                    {
                        this.insertMathML (
                            "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&rbrace;</mo></math>");
                    }
                    else
                    {
                        this.insertMathML (
                            "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo stretchy=\"false\" nugenCursorEnd=''>&rbrace;</mo></math>");
                    }
                    return true;
                }
                if (sKey == ']')
                {
                    if (bStretchyBrackets)
                    {
                        this.insertMathML (
                            "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&rbrack;</mo></math>");
                    }
                    else
                    {
                        this.insertMathML (
                            "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo stretchy=\"false\" nugenCursorEnd=''>&rbrack;</mo></math>");
                    }
                    return true;
                }
                if (node.type_.type != ElementType.Mtext)
                {
                    try
                    {
                        int g = Convert.ToInt32 (sKey);
                        if (g > 0x7f)
                        {
                            Glyph glyph = this.entityManager.ByUnicode (g.ToString ("X5"));
                            if (glyph != null)
                            {
                                if (glyph.Name == "deg")
                                {
                                    this.insertMathML (
                                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mi mathvariant=\"normal\" nugenCursorEnd=''>&" +
                                        glyph.Name + ";</mi></math>");
                                }
                                else
                                {
                                    this.insertMathML (
                                        "<math xmlns='http://www.w3.org/1998/Math/MathML'><mi nugenCursorEnd=''>&" +
                                        glyph.Name + ";</mi></math>");
                                }
                                return true;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            node = this.GetCurrentlySelectedNode ();
            this.OnInsert (true);
            if (!this.ScriptEligible ())
            {
                return false;
            }
            node = this.GetCurrentlySelectedNode ();
            if (node == null)
            {
                return false;
            }
            if ((node.type_.type == ElementType.Mtext) || (node.type_.type == ElementType.Ms))
            {
                if (node.InternalMark == node.LiteralLength)
                {
                    node.literalText = node.literalText + sKey;
                }
                else if (node.InternalMark > 0)
                {
                    node.literalText = node.literalText.Substring (0, node.InternalMark) + sKey +
                                       node.literalText.Substring (node.InternalMark, node.literalText.Length - node.InternalMark);
                }
                else
                {
                    node.literalText = sKey + node.literalText;
                }
                node.InternalMark++;
                return true;
            }
            
            if (char.IsDigit (sKey) || (sKey == '.'))
            {
                if (this.AcceptsDigits ())
                {
                    node = this.GetCurrentlySelectedNode ();
                    if (node.InternalMark == node.LiteralLength)
                    {
                        node.literalText = node.literalText + sKey;
                    }
                    else if (node.InternalMark > 0)
                    {
                        node.literalText = node.literalText.Substring (0, node.InternalMark) + sKey +
                                           node.literalText.Substring (node.InternalMark, node.literalText.Length - node.InternalMark);
                    }
                    else
                    {
                        node.literalText = sKey + node.literalText;
                    }
                    node.InternalMark++;
                    return true;
                }
                this.insertMathML (false,
                           "<math xmlns='http://www.w3.org/1998/Math/MathML'><mn nugenCursorEnd=''>" + sKey +
                           "</mn></math>");
                return true;
            }
            if (this.AcceptsLetters ())
            {
                node = this.GetCurrentlySelectedNode ();
                if (node.InternalMark == node.LiteralLength)
                {
                    node.literalText = node.literalText + sKey;
                }
                else if (node.InternalMark > 0)
                {
                    node.literalText = node.literalText.Substring (0, node.InternalMark) + sKey +
                                       node.literalText.Substring (node.InternalMark, node.literalText.Length - node.InternalMark);
                }
                else
                {
                    node.literalText = sKey + node.literalText;
                }
                node.InternalMark++;
                return true;
            }
            this.insertMathML (false,
                       "<math xmlns='http://www.w3.org/1998/Math/MathML'><mi nugenCursorEnd=''>" + sKey +
                       "</mi></math>");
            return true;
        }
        // 
        private void InsertFromXml (Node currentSelectedNode, XmlNode xmlChildNode, Node newNode, ref Node newSelectedNode, ref bool selected)
        {
            newNode = new Node();
            newNode.Parse(xmlChildNode, this.types_, this.entityManager, false, null, false);
            if (newNode.type_ != null)
            {
                currentSelectedNode.PrependNode(newNode);

                newSelectedNode = newNode.Parse(xmlChildNode, this.types_, this.entityManager, true, null, false);
                
                if (newSelectedNode != null)
                {
                    this.SelectNode(newSelectedNode, newSelectedNode.IsAppend);
                    selected = true;
                }
            }
        }
        //
        public void ApplyStyleToSelection (StyleAttributes styleAttributes)
        {
            try
            {
                this.OnInsert (false);
                if (this.hasSelection)
                {
                    {
                        Selection sel = this.CaptureSelection();
                        if (sel != null)
                        {
                            this.ApplyStyleToSelection(styleAttributes, sel);
                        }
                    }
                }
                else
                {
                    Node cur = this.GetCurrentlySelectedNode ();
                    if ((((cur != null) && (cur.InternalMark == 0)) &&
                         ((cur.type_ != null))) &&
                        (cur.type_.type != ElementType.Math))
                    {
                        styleAttributes.hasBackground = true;
                        styleAttributes.hasColor = true;
                        cur.SetStyle (styleAttributes);
                    }
                }
            }
            catch
            {
            }
        }
        //
        private void ApplyStyleToSelection (StyleAttributes styleAttributes, Selection selectionCollection)
        {
            try
            {
                if (((selectionCollection == null) || (selectionCollection.nodesList.Count <= 0)))
                {
                    return;
                }
                int count = selectionCollection.nodesList.Count;
                selectionCollection.nodesList.Reset ();
                for (int i = 0; i < count; i++)
                {
                    Node n = selectionCollection.nodesList.Next ();
                    if (((i <= 0) || (n != selectionCollection.Last)) || (selectionCollection.literalLength != 0))
                    {
                        styleAttributes.hasBackground = true;
                        styleAttributes.hasColor = true;
                        n.SetStyle (styleAttributes);
                    }
                }
                selectionCollection.nodesList.Reset ();
            }
            catch
            {
            }
        }
        //
        public StyleAttributes GetSelectionStyle ()
        {
            StyleAttributes style = new StyleAttributes ();
            try
            {
                if (!this.hasSelection)
                {
                    return style;
                }
                
                Selection sel = this.CaptureSelection ();
                if (sel != null)
                {
                    style = this.GetSelectionStyle (style, sel);
                }
            }
            catch
            {
            }
            return style;
        }
        //
        private StyleAttributes GetSelectionStyle (StyleAttributes styleAttributes, Selection selectionCollection)
        {
            try
            {
                if (((selectionCollection == null) || (selectionCollection.nodesList.Count <= 0)))
                {
                    return styleAttributes;
                }
                int count = selectionCollection.nodesList.Count;
                Node n = selectionCollection.nodesList.Next ();
                if (n.style_ != null)
                {
                    n.style_.CopyTo (styleAttributes);
                    return styleAttributes;
                }
                styleAttributes = new StyleAttributes ();
            }
            catch
            {
            }
            return styleAttributes;
        }
        //
        public void SetClientWidth (int w)
        {
            this.clientRect.Width = w;
        }
        //
        private bool ScriptEligible ()
        {
            Node node = this.GetCurrentlySelectedNode ();
            if ((node != null) && (node.type_ != null))
            {
                
                    if (node.type_.type == ElementType.Math)
                    {
                        if (node.firstChild != null)
                        {
                            return true;
                        }
                        return false;
                    }
                    if (((((node.type_.type == ElementType.Mo) && (node.InternalMark == 0)) &&
                          (node.literalText != null)) &&
                         (((node.literalText == "(") || (node.literalText == "{")) ||
                          ((node.literalText == "[") || ((node.literalText == "|") && (node.prevSibling == null))))) &&
                        ((node.parent_ != null) && (node.parent_.type_.type == ElementType.Mrow)))
                    {
                        this.SelectNode (node.parent_, false);
                        return true;
                    }
                    if (((((node.type_.type == ElementType.Mo) && node.IsAppend) && (node.literalText != null)) &&
                         (((node.literalText == ")") || (node.literalText == "}")) ||
                          ((node.literalText == "]") || ((node.literalText == "|") && (node.nextSibling == null))))) &&
                        ((node.parent_ != null) && (node.parent_.type_.type == ElementType.Mrow)))
                    {
                        this.SelectNode (node.parent_, true);
                        return true;
                    }
                    return true;
                
            }
            
            return false;
        }
        //
        private bool IsEditable ()
        {
            Node node = this.GetCurrentlySelectedNode ();
            if (((node != null) && (node.type_ != null)))
            {
                if (node.type_.type == ElementType.Math)
                {
                    if (node.firstChild == null)
                    {
                        return false;
                    }
                    this.SelectNode (node.firstChild, false);
                    return true;
                }
                if (((((node.type_.type == ElementType.Mo) && (node.InternalMark == 0)) && (node.literalText != null)) &&
                     (((node.literalText == "(") || (node.literalText == "{")) ||
                      ((node.literalText == "[") || ((node.literalText == "|") && (node.prevSibling == null))))) &&
                    ((node.parent_ != null) && (node.parent_.type_.type == ElementType.Mrow)))
                {
                    this.SelectNode (node.parent_, false);
                    return true;
                }
                if (((((node.type_.type == ElementType.Mo) && node.IsAppend) && (node.literalText != null)) &&
                     (((node.literalText == ")") || (node.literalText == "}")) ||
                      ((node.literalText == "]") || ((node.literalText == "|") && (node.nextSibling == null))))) &&
                    ((node.parent_ != null) && (node.parent_.type_.type == ElementType.Mrow)))
                {
                    this.SelectNode (node.parent_, true);
                    return true;
                }
                return true;
            }
            return false;
        }
        //
        private bool IsContainer (Node node)
        {
            try
            {
                if ((((node == null) || !node.isVisible) || (node.isGlyph || node.skip)))
                {
                    return false;
                }
                if ((((((node.type_.type != ElementType.Mfrac) &&
                        (node.type_.type != ElementType.Maction)) &&
                       ((node.type_.type != ElementType.Mfenced) &&
                        (node.type_.type != ElementType.Mfrac))) &&
                      (((node.type_.type != ElementType.Mmultiscripts) &&
                        (node.type_.type != ElementType.Mover)) &&
                       ((node.type_.type != ElementType.Mpadded) &&
                        (node.type_.type != ElementType.Mphantom)))) &&
                     ((node.type_.type != ElementType.Mroot) &&
                      ((node.type_.type != ElementType.Mrow) || (node.firstChild == null)))) &&
                    ((((node.type_.type != ElementType.Msqrt) &&
                       (node.type_.type != ElementType.Msub)) &&
                      ((node.type_.type != ElementType.Msubsup) &&
                       (node.type_.type != ElementType.Msup))) &&
                     (((node.type_.type != ElementType.Munder) &&
                       (node.type_.type != ElementType.Munderover)) &&
                      ((node.type_.type != ElementType.Mtable) || (node.Class == "nugentoplevel")))))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        //
        public bool Back ()
        {
            try
            {
                if (this.HasSelection)
                {
                    return this.DoDelete ();
                }
                this.CanUndo = true;
                Node node = this.GetCurrentlySelectedNode ();
                if (node == null)
                {
                    return false;
                }

                if ((((node.parent_ != null)) &&
                     ((node.parent_.type_.type == ElementType.Mtd) && (node.InternalMark == 0))) &&
                    (node.prevSibling == null))
                {
                    if ((node.parent_.prevSibling == null) && (node.parent_.nextSibling == null))
                    {
                        this.BackspaceCell ();
                        return true;
                    }
                    return this.GoLeft ();
                }

                if ((node.IsAtom() && (node.literalText != null)) &&
                    ((node.literalText.Length > 0) && (node.InternalMark > 0)))
                {
                    this.CaptureUndo ();
                    if (node.InternalMark > 1)
                    {
                        
                        if (node.literalText.Length > 2)
                        {
                            node.literalText = node.literalText.Substring (0, node.InternalMark - 1) +
                                               node.literalText.Substring (node.InternalMark, node.literalText.Length - node.InternalMark);
                            node.InternalMark = node.InternalMark - 1;
                        }
                        else
                        {
                            node.literalText = node.literalText.Substring (0, node.InternalMark - 1);
                            node.InternalMark = node.InternalMark - 1;
                        }
                    }
                    else
                    {
                        if (node.literalText.Length > 1)
                        {
                            node.literalText = node.literalText.Substring (1, node.literalText.Length - 1);
                            node.InternalMark = node.InternalMark - 1;
                        }
                        else
                        {
                            this.SelectNeighbor (node);
                            this.Tear (node);
                            return true;
                        }
                        node.InternalMark = 0;
                    }
                    return true;
                }
                if (node.InternalMark == 0)
                {
                    if (node.prevSibling != null)
                    {
                        if (this.IsContainer (node.prevSibling))
                        {
                            this.multiSelectNode = node.prevSibling;
                            this.currentCaret = 1;
                            this.hasSelection = true;
                            this.SelectNode(node.prevSibling, false);
                            this.hasSelection = true;
                            return true;
                        }
                    }
                    else if ((node.parent_ != null) && node.parent_.skip)
                    {
                        if ((node.parent_.parent_ != null) && this.IsContainer (node.parent_.parent_))
                        {
                            this.multiSelectNode = node.parent_.parent_;
                            this.currentCaret = 1;
                            this.hasSelection = true;
                            this.SelectNode(node.parent_.parent_, false);
                            this.hasSelection = true;
                            return true;
                        }
                    }
                    else if (this.IsContainer (node.parent_))
                    {
                        this.multiSelectNode = node.parent_;
                        this.currentCaret = 1;
                        this.hasSelection = true;
                        this.SelectNode(node.parent_, false);
                        this.hasSelection = true;
                        return true;
                    }
                }
                else if (node.IsAppend && this.IsContainer (node))
                {
                    this.multiSelectNode = node;
                    this.currentCaret = 1;
                    this.SelectNode (node, false);
                    this.hasSelection = true;
                    return true;
                }
                
                if (node.IsAppend)
                {
                    if (!this.CheckNode (node))
                    {
                        return false;
                    }
                    
                    this.CaptureUndo ();
                    this.SelectNeighbor (node);
                    this.Tear (node);
                    
                    return true;
                }
                if (node.prevSibling != null)
                {
                    if (!this.CheckNode (node.prevSibling))
                    {
                        return false;
                    }
                    this.SelectNode (node.prevSibling, true);
                    return this.Back ();
                }
                
                if (this.GoLeft () && node.skip)
                {
                    return this.GoLeft ();
                }
                return false;
            }
            catch
            {
            }
            
            return false;
        }
        //
        public void SetClientHeight (int h)
        {
            this.clientRect.Height = h;
        }
        //
        public bool DoDelete ()
        {
            bool r = false;
            try
            {
                this.CanUndo = true;
                Node cur = this.GetCurrentlySelectedNode ();
                
                if (this.HasSelection)
                {
                    return DeleteSelection();
                }
                
                // no selection
                if (cur.InternalMark == 0)
                {
                    try
                    {
                        if (((((cur.type_.type == ElementType.Mrow) && (cur.firstChild == null)) &&
                              ((cur.nextSibling == null) && (cur.prevSibling == null))) &&
                             (((cur.parent_ != null) && (cur.parent_.type_.type == ElementType.Mtd)) &&
                              ((cur.parent_.nextSibling == null) && (cur.parent_.prevSibling == null)))) &&
                            (((cur.parent_.parent_ != null) &&
                              (cur.parent_.parent_.type_.type == ElementType.Mtr)) &&
                             ((cur.parent_.parent_.nextSibling == null) &&
                              (cur.parent_.parent_.prevSibling != null))))
                        {
                            return this.Back ();
                        }
                    }
                    catch
                    {
                    }
                    if (this.IsContainer (cur))
                    {
                        this.multiSelectNode = cur;
                        this.currentCaret = 1;
                        this.hasSelection = true;
                        return true;
                    }
                }
                else if ((cur.IsAppend && (cur.nextSibling != null)) && this.IsContainer (cur.nextSibling))
                {
                    this.multiSelectNode = cur.nextSibling;
                    this.currentCaret = 1;
                    this.hasSelection = true;
                    this.SelectNode (cur.nextSibling, false);
                    this.hasSelection = true;
                    return true;
                }
                
                if (!this.CheckNode (cur))
                {
                    return false;
                }
                if (this.DeleteCell (cur))
                {
                    return true;
                }
                
                if ((cur.IsAtom() && (cur.literalText != null)) && (cur.literalText.Length > 0))
                {
                    this.CaptureUndo ();
                    if (cur.InternalMark < cur.LiteralLength)
                    {
                        if ((cur.InternalMark == 0) && (cur.literalText.Length == 1))
                        {
                            this.SelectNeighbor (cur);
                            this.Tear (cur);
                            
                            r = true;

                            return r;
                        }
                        
                        if (cur.InternalMark > 0)
                        {
                            cur.literalText = cur.literalText.Substring (0, cur.InternalMark) +
                                               cur.literalText.Substring (cur.InternalMark + 1,
                                                                           (cur.literalText.Length - cur.InternalMark) - 1);
                        }
                        else
                        {
                            cur.literalText = cur.literalText.Substring (1, cur.literalText.Length - 1);
                        }
                        
                        r = true;
                        if ((cur.literalText.Length != 0) && (cur.InternalMark == cur.LiteralLength))
                        {
                            if (cur.nextSibling != null)
                            {
                                this.SelectNode (cur.nextSibling, false);
                                r = true;
                            }
                            else
                            {
                                r = false;
                            }
                        }
                        return r;
                    }
                    if (cur.nextSibling != null)
                    {
                        this.SelectNode (cur.nextSibling, false);
                        return this.DoDelete ();
                    }

                    return r;
                }
                this.CaptureUndo ();
                if (cur.IsAppend)
                {
                    if (cur.nextSibling != null)
                    {
                        if ((!cur.nextSibling.isVisible || cur.nextSibling.skip) ||
                            cur.nextSibling.isGlyph)
                        {
                            return r;
                        }
                        this.SelectNode (cur.nextSibling, false);
                        
                        return this.DoDelete ();
                    }
                    
                    return r;
                }
                
                this.SelectNeighbor (cur);
                r = this.Tear (cur);
           
                
                r = true;
            }
            catch
            {
                return false;
            }
            return r;
        }
        //
        private bool DeleteSelection()
        {
            int mark = 0;
            Node c = null;
            {
                this.CaptureUndo();
                Node cur = this.GetCurrentlySelectedNode();
                Selection sel = this.CaptureSelection();
                try
                {
                    if (sel != null)
                    {
                        if (sel.nodesList != null)
                        {
                            int num = sel.nodesList.Count;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                try
                {
                    c = this.TearSelection(sel, ref mark);
                    mark = Math.Min(mark, c.LiteralLength);
                }
                catch
                {
                }
            }
            bool notOk = false;
            if ((c != null) && !c.tagDeleted)
            {
                try
                {
                    this.SelectNode(c, false);
                    this.selectedNode.InternalMark = mark;
                }
                catch
                {
                    notOk = true;
                }
            }
            else
            {
                notOk = true;
            }
            
            if (notOk)
            {
                try
                {
                    if (this.rootNode_ != null)
                    {
                        if (this.rootNode_.xmlTagName == "math")
                        {
                            c = this.rootNode_;
                            if (c.firstChild != null)
                            {
                                c = c.firstChild;
                                if (((c.type_.type == ElementType.Mtable) && (c.Class == "nugentoplevel")) &&
                                    (c.firstChild != null))
                                {
                                    c = c.firstChild;
                                    if ((c.type_.type == ElementType.Mtr) &&
                                        (c.firstChild != null))
                                    {
                                        c = c.firstChild;
                                        if ((c.type_.type == ElementType.Mtd) &&
                                            (c.firstChild != null))
                                        {
                                            c = c.firstChild;
                                        }
                                    }
                                }
                            }
                        }
                        if (c == null)
                        {
                            c = this.rootNode_;
                        }
                        this.SelectNode(c, false);
                    }
                }
                catch
                {
                }
            }
            this.HasSelection = false;
            return false;
        }
        //
        private void SelectNeighbor (Node node)
        {
            try
            {
                if (node.nextSibling != null)
                {
                    this.SelectNode (node.nextSibling, false);
                }
                else if (node.prevSibling != null)
                {
                    this.SelectNode (node.prevSibling, true);
                }
                else if (node.parent_ != null)
                {
                    this.SelectNode (node.parent_, false);
                }
            }
            catch
            {
            }
        }
        //
        private bool CheckNode (Node node)
        {
            try
            {
                if (!node.isVisible)
                {
                    return false;
                }
                
                if ( (node.type_.type == ElementType.Mtr) ||
                     (node.type_.type == ElementType.Mtd)) 
                {
                    return false;
                }
                if ((node.parent_ != null) && (node.parent_.type_.type == ElementType.Maction))
                {
                    return false;
                }
                
                if ((node.level == 0))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        //
        private Node TearSelection (Selection collection, ref int updatedMark)
        {
            Node newRow = null;
            Node c = null;
            updatedMark = 0;
            bool samePar = false;
            try
            {
                if (((((collection != null) && (collection.nodesList != null)) &&
                      ((collection.nodesList.Count > 0) && (collection.First != null))) &&
                     (((collection.Last != null) && (collection.First.parent_ != null)) &&
                      ((collection.Last.parent_ != null) && (collection.parent == collection.First.parent_)))) &&
                    (collection.parent == collection.Last.parent_))
                {
                    samePar = true;
                }
                if (samePar)
                {
                    bool ok = false;
                    if (collection.First == collection.Last)
                    {
                        if (collection.First.IsAtom())
                        {
                            if ((collection.caret > 0) || (collection.literalLength < collection.Last.LiteralLength))
                            {
                                c = collection.First;
                                updatedMark = collection.caret;
                            }
                            else
                            {
                                ok = true;
                            }
                        }
                        ok = true;
                    }
                    else if (collection.First.IsAtom())
                    {
                        if (collection.caret > 0)
                        {
                            c = collection.First;
                            updatedMark = collection.caret;
                        }
                        else
                        {
                            if (collection.First.prevSibling != null)
                            {
                                c = collection.First.prevSibling;
                                updatedMark = collection.First.prevSibling.LiteralLength;
                            }
                            ok = true;
                        }
                    }
                    else
                    {
                        ok = true;
                    }
                    if (((c == null) && ok) &&
                        ((collection.First != collection.Last) && (collection.literalLength < collection.Last.LiteralLength)))
                    {
                        c = collection.Last;
                        updatedMark = 0;
                    }
                    if (c == null)
                    {
                        if (collection.Last.nextSibling != null)
                        {
                            c = collection.Last.nextSibling;
                            updatedMark = 0;
                        }
                        else if (collection.First.prevSibling != null)
                        {
                            c = collection.First.prevSibling;
                            updatedMark = collection.First.prevSibling.LiteralLength;
                        }
                        else
                        {
                            c = collection.First.parent_;
                            updatedMark = 0;
                        }
                    }
                    Node cur = null;
                    collection.nodesList.Reset ();
                    for (cur = collection.nodesList.Next (); cur != null; cur = collection.nodesList.Next ())
                    {
                        if (((cur == collection.First) || ((cur == collection.Last) && (collection.literalLength > 0))) &&
                            (cur.IsAtom()))
                        {
                            if ((cur.literalText != null) && (cur.literalText.Length > 0))
                            {
                                int mark = 0;
                                int len = 2;
                                try
                                {
                                    if (collection.nodesList.Count == 1)
                                    {
                                        mark = collection.caret;
                                        len = collection.literalLength;
                                    }
                                    else if (cur == collection.First)
                                    {
                                        mark = collection.caret;
                                        len = cur.LiteralLength;
                                    }
                                    else if (cur == collection.Last)
                                    {
                                        mark = 0;
                                        len = collection.literalLength;
                                    }
                                    if ((mark == 0) && (len == cur.LiteralLength))
                                    {
                                        this.Tear (cur);
                                    }
                                    else if ((mark == 0) && (len < cur.LiteralLength))
                                    {
                                        cur.literalText = cur.literalText.Substring (len, cur.LiteralLength - len);
                                        cur.LiteralStart = 0;
                                    }
                                    else if ((mark > 0) && (len == cur.LiteralLength))
                                    {
                                        cur.literalText = cur.literalText.Substring (0, mark);
                                    }
                                    else
                                    {
                                        cur.literalText = cur.literalText.Substring (0, mark) +
                                                           cur.literalText.Substring (len, cur.LiteralLength - len);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                if ((cur != collection.Last) || ((cur == collection.Last) && (collection.literalLength == cur.LiteralLength)))
                                {
                                    this.Tear (cur, true, true, ref newRow);
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    collection.nodesList.Reset ();
                }
            }
            catch
            {
            }
            try
            {
                if ((((newRow != null) && !newRow.tagDeleted) && ((newRow.parent_ != null) && (newRow.type_ != null))) &&
                    ((newRow.type_.type == ElementType.Mrow) && (this.selectedNode == newRow)))
                {
                    c = newRow;
                    updatedMark = 0;
                    return c;
                }
                if ((c != null) && !c.isVisible)
                {
                    bool valid = false;
                    Node n = null;
                    n = c;
                    while ((!valid && (n.prevSibling != null)) && !n.prevSibling.tagDeleted)
                    {
                        if (n.prevSibling.isVisible && !n.prevSibling.tagDeleted)
                        {
                            valid = true;
                        }
                        n = n.prevSibling;
                    }
                    if (!valid)
                    {
                        for (n = c;
                             (!valid && (n.nextSibling != null)) && !n.nextSibling.tagDeleted;
                             n = n.nextSibling)
                        {
                            if (n.nextSibling.isVisible && !n.nextSibling.tagDeleted)
                            {
                                valid = true;
                            }
                        }
                    }
                    if ((!valid && (c.parent_ != null)) &&
                        (c.parent_.isVisible && !c.parent_.tagDeleted))
                    {
                        n = c.parent_;
                    }
                    if ((valid && (n != null)) && (n.isVisible && !n.tagDeleted))
                    {
                        c = n;
                        updatedMark = 0;
                    }
                    return c;
                }
                if ((c == null) ||
                    ((c.type_.type != ElementType.Mtd) &&
                     ((c.type_.type != ElementType.Math) || (c.parent_ != null))))
                {
                    return c;
                }
                if (c.type_.type == ElementType.Mtd)
                {
                    if (c.firstChild != null)
                    {
                        c = c.firstChild;
                        updatedMark = 0;
                    }
                    return c;
                }
                if (((c.type_.type != ElementType.Math) || (c.parent_ != null)) ||
                    (c.firstChild == null))
                {
                    return c;
                }
                if ((c.firstChild.type_.type == ElementType.Mtable) && (c.firstChild.Class == "nugentoplevel"))
                {
                    if ((((c.firstChild.firstChild != null) &&
                          (c.firstChild.firstChild.type_.type == ElementType.Mtr)) &&
                         ((c.firstChild.firstChild.firstChild != null) &&
                          (c.firstChild.firstChild.firstChild.type_.type == ElementType.Mtd))) &&
                        (c.firstChild.firstChild.firstChild.firstChild != null))
                    {
                        c = c.firstChild.firstChild.firstChild.firstChild;
                        updatedMark = 0;
                    }
                    return c;
                }
                c = c.firstChild;
                updatedMark = 0;
            }
            catch
            {
            }
            return c;
        }
        //
        private bool Tear (Node node)
        {
            return this.Tear (node, true, true);
        }
        //
        private bool Tear (Node node, bool bCheck, bool bSelect)
        {
            Node newRow = null;
            return this.Tear (node, bCheck, bSelect, ref newRow);
        }
        //
        private bool Tear (Node node, bool needCheck, bool needSelect, ref Node newRow)
        {
            bool r;
            try
            {
                try
                {
                    if (node == this.selectedNode)
                    {
                        this.SelectNeighbor (node);
                    }
                }
                catch
                {
                }
                this.CanUndo = true;
                if (needCheck && !this.CheckNode (node))
                {
                    return false;
                }
                
                Node upper = node.upperNode;
                Node lower = node.lowerNode;
                Node next = node.nextSibling;
                Node prev = node.prevSibling;
                Node par = node.parent_;
                if (upper != null)
                {
                    upper.lowerNode = null;
                }
                if (lower != null)
                {
                    lower.upperNode = null;
                }
                
                if (prev == null)
                {
                    if (next != null)
                    {
                        next.prevSibling = null;
                    }
                    par.firstChild = next;
                    if (next == null)
                    {
                        par.lastChild = next;
                    }
                }
                else if (next != null)
                {
                    prev.nextSibling = next;
                    next.prevSibling = prev;
                }
                else
                {
                    prev.nextSibling = null;
                    par.lastChild = prev;
                }
                par.UpdateChildrenIndices ();
                par.UpdateLevel ();
                this.TagAsDeleted (node);
                
                if (((par.numChildren >= par.type_.minChilds) && (node.parent_.type_.type != ElementType.Mmultiscripts)))
                {
                    return true;
                }
                
                Node row = this.CreateRow ();
                row.literalText = "";
                bool ok = false;
                if (next != null)
                {
                    ok = next.PrependNode (row);
                    if (needSelect)
                    {
                        this.SelectNode(row, false);
                        try
                        {
                            newRow = this.selectedNode;
                    }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    ok = par.AdoptChild(row);
                    if (needSelect)
                    {
                        this.SelectNode(row, false);
                        try
                        {
                            newRow = this.selectedNode;
                        }
                        catch
                        {
                        }
                    }
                }
                
                r = ok;
            }
            catch
            {
                r = false;
            }
            return r;
        }
        //
        private void TagAsDeleted (Node node)
        {
            if (node.HasChildren ())
            {
                NodesList list = node.GetChildrenNodes ();
                for (int i = 0; i < list.Count; i++)
                {
                    Node n = list.Get (i);
                    if (n != null)
                    {
                        this.TagAsDeleted (n);
                    }
                }
            }
            node.tagDeleted = true;
            node = null;
        }
        //
        private bool CreateTopLevelTable ()
        {
            try
            {
                if (!this.IsMultiline)
                {
                    Node table = this.MakeNode ("mtable");
                    Node row = this.MakeNode ("mtr");
                    Node cell = this.MakeNode ("mtd");
                    table.AdoptChild (row);
                    row.AdoptChild (cell);

                    if (table.attrs == null)
                    {
                        table.attrs = new AttributeList ();
                    }
                    table.attrs.Add ("columnalign", "left");
                    table.attrs.Add ("class", "nugentoplevel");

                    Node cur = this.selectedNode;
                    int curCaret = this.selectedNode.InternalMark;
                    this.rootNode_.firstChild.PrependNode (table);
                    while (table.nextSibling != null)
                    {
                        this.ReParent (table.nextSibling, cell);
                    }
                    this.rootNode_.UpdateLevel ();
                    if ((this.selectedNode != cur) || (this.selectedNode.InternalMark != curCaret))
                    {
                        this.SelectNode (cur, false);
                        cur.InternalMark = curCaret;
                    }
                    if (this.IsMultiline)
                    {
                        return true;
                    }
                }
            }
            catch
            {
            }
            return false;
        }
        //
        private bool ReParent (Node node, Node newParent)
        {
            try
            {
                Node oldParent = null;
                Node prev = null;
                Node next = null;
                oldParent = node.parent_;
                prev = node.prevSibling;
                next = node.nextSibling;
                if ((next == null) && (prev == null))
                {
                    oldParent.firstChild = null;
                    oldParent.lastChild = null;
                    oldParent.numChildren = 0;
                }
                else if ((next != null) && (prev != null))
                {
                    next.prevSibling = prev;
                    prev.nextSibling = next;
                    oldParent.numChildren--;
                }
                else if ((next != null) && (prev == null))
                {
                    next.prevSibling = null;
                    oldParent.firstChild = next;
                    oldParent.numChildren--;
                }
                else if ((next == null) && (prev != null))
                {
                    prev.nextSibling = null;
                    oldParent.lastChild = prev;
                    oldParent.numChildren--;
                }
                node.level = oldParent.level + 1;
                oldParent.UpdateChildrenIndices ();
                oldParent.UpdateLevel ();
                node.prevSibling = null;
                node.nextSibling = null;
                node.parent_ = null;
                newParent.AdoptChild (node);
                return true;
            }
            catch
            {
            }
            return false;
        }
        //
        public void SetupPainting (Graphics pGraphics, bool bAntiAlias, bool b256ColorMode)
        {
            this.painter_.SetupPainting (pGraphics, bAntiAlias);
        }
        //
        private bool IsToplevelRowCell (Node node)
        {
            try
            {
                if (((((node != null)) &&
                      ((node.level == 4) && (node.parent_ != null))) &&
                     (((node.parent_.type_.type == ElementType.Mtd) &&
                       (node.parent_.parent_ != null)) &&
                      ((node.parent_.parent_.type_.type == ElementType.Mtr) &&
                       (node.parent_.parent_.parent_ != null)))) &&
                    ((((node.parent_.parent_.parent_.type_.type == ElementType.Mtable) &&
                       (node.parent_.parent_.parent_.Class == "nugentoplevel")) &&
                      ((node.parent_.parent_.parent_.parent_ != null) &&
                       (node.parent_.parent_.parent_.parent_.type_.type == ElementType.Math))) &&
                     (node.parent_.parent_.parent_.parent_.parent_ == null)))
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }
        //
        public Node CarriageReturn (Node node, ref bool wasSplit)
        {
            if ((((node.literalText == null) || (node.InternalMark <= 0))) ||
                (node.InternalMark >= node.LiteralLength))
            {
                return node;
            }
            if (((node.parent_ != null)) &&
                ((node.parent_.numChildren == node.parent_.type_.maxChilds) ||
                 (node.parent_.type_.type == ElementType.Mmultiscripts)))
            {
                node = this.WrapInRowInplace (node);
            }
            Node newNode = new Node (node.xmlTagName);
            node.CopyProperties (newNode);
            newNode.InternalMark = 0;
            newNode.literalText = node.literalText.Substring (node.InternalMark, node.literalText.Length - node.InternalMark);
            node.literalText = node.literalText.Substring (0, node.InternalMark);
            node.AppendNode (newNode);
            node.parent_.UpdateChildrenIndices ();
            node.parent_.UpdateLevel ();
            newNode.InternalMark = 0;
            wasSplit = true;
            return newNode;
        }
        //
        private Node WrapInRowInplace (Node node)
        {
            Node n = new Node ();
            node.CopyTo (n);
            n.IsAppend = node.IsAppend;
            n.InternalMark = node.InternalMark;
            
            Node row = this.CreateRow ();
            node.parent_.ReplaceChild (node, row);
            row.AdoptChild (n);
            return n;
        }
        //
        public void SetOrigin (int oX, int oY)
        {
            this.oX = oX;
            this.oY = oY;
            this.painter_.SetOrigin (oX, oY);
        }
        //
        public string SaveToXML (bool bStripNamespace)
        {
            string xml = this.SaveToXML ();
            if (bStripNamespace)
            {
                try
                {
                    int indexOf = 0;
                    int endIndex = 0;
                    indexOf = xml.IndexOf ("xmlns=", 0, xml.Length);
                    if (indexOf <= 0)
                    {
                        return xml;
                    }
                    string nspace = "";
                    if (xml[indexOf + 6] == '"')
                    {
                        nspace = "\"";
                    }
                    else if (xml[indexOf + 6] == '\'')
                    {
                        nspace = "'";
                    }
                    if (nspace.Length > 0)
                    {
                        endIndex = xml.IndexOf (nspace, indexOf + 7, (xml.Length - indexOf) - 7);
                        if (endIndex <= indexOf)
                        {
                            return xml;
                        }
                        endIndex++;
                        xml = xml.Substring (0, indexOf) + xml.Substring (endIndex, xml.Length - endIndex);
                    }
                }
                catch
                {
                }
            }
            return xml;
        }
        //
        public string SaveToXML ()
        {
            try
            {
                XmlDocument doc = new XmlDocument ();
                this.rootNode_.SaveToXml (doc, null, "UTF-8");
                return doc.DocumentElement.OuterXml;
            }
            catch
            {
                return "";
            }
        }
        //
        public void PropogateEntityManager ()
        {
            if (this.entityManager != null)
            {
                this.painter_.SetEntityManager (this.entityManager);
            }
        }
        //
        public string CaptureForClipboard ()
        {
            XmlDocument doc = new XmlDocument ();
            bool ok = false;
            string r = "";
            try
            {
                if ((this.selectedNode != null) && this.HasSelection)
                {
                    {
                        Selection sel = this.CaptureSelection ();
                        if (sel != null)
                        {
                            ok = this.SaveToXml (doc, sel);
                        }
                        else
                        {
                            ok = false;
                        }
                    }
                }
            }
            catch
            {
                ok = false;
            }
            if (ok)
            {
                try
                {
                    r = doc.DocumentElement.OuterXml;
                }
                catch
                {
                    ok = false;
                }
            }
            if (!ok)
            {
                r = "";
            }
            return r;
        }
        //
        private bool SaveToXml (XmlDocument doc, Selection sel)
        {
            bool r = false;
            XmlNode root = null;
            string decl = "<?xml version='1.0' encoding='UTF-8'?>";
            try
            {
                if (sel.nodesList.Count <= 0)
                {
                    return r;
                }
                bool ok = false;


                decl = decl + "<math xmlns='http://www.w3.org/1998/Math/MathML'/>";
                doc.LoadXml(decl);
                root = doc.DocumentElement;
                ok = true;


                if (!ok)
                {
                    return r;
                }
                try
                {
                    sel.parent.SaveToXml(doc, root, "UTF-8", sel);
                    return true;
                }
                catch (Exception)
                {
                    return r;
                }
            }
            catch
            {
                r = false;
            }
            return r;
        }
        //
        public void SetEntityManager (EntityManager eman)
        {
            this.entityManager = eman;
        }
        //
        private Node MakeNode (string name)
        {
            Node node = null;
            try
            {
                node = new Node(name);
                node.type_ = this.types_[name];

                node.namespaceURI = this.mathmlNamespace;
                return node;
            }
            catch
            {
            }
            return node;
        }
        // 
        private Node CreateRow ()
        {
            return this.MakeNode ("mrow");
        }
        //
        public bool MoveToSelected (ref int nDiff_X, ref int nDiff_Y)
        {
            nDiff_X = 0;
            nDiff_Y = 0;
            try
            {
                Node n = this.selectedNode;
                if (((n != null) && n.isVisible) && !n.tagDeleted)
                {
                    Rectangle rectangle = this.bounds;
                    n.PropogateYOffset ();
                    if (n != null)
                    {
                        if ((n.box.Y <= rectangle.Y) ||
                            ((n.box.Y + n.box.Height) >= ((rectangle.Y + rectangle.Height) - 1)))
                        {
                            if (n.box.Height < rectangle.Height)
                            {
                                if ((n.box.Y + n.box.Height) > ((rectangle.Y + rectangle.Height) - 1))
                                {
                                    nDiff_Y = (n.box.Y + n.box.Height) - ((rectangle.Y + rectangle.Height) - 1);
                                }
                                else if (n.box.Y < rectangle.Y)
                                {
                                    nDiff_Y = n.box.Y - rectangle.Y;
                                }
                            }
                            else if ((n.box.Y + n.box.Height) > (rectangle.Y + rectangle.Height))
                            {
                                nDiff_Y = n.box.Y - rectangle.Y;
                            }
                            else if ((n.box.Y + n.box.Height) < (rectangle.Y + rectangle.Height))
                            {
                                nDiff_Y = (n.box.Y + n.box.Height) - (rectangle.Y + rectangle.Height);
                            }
                        }
                                                
                        if ((n.box.X <= rectangle.X) ||
                            ((n.box.X + n.box.Width) >= ((rectangle.X + rectangle.Width) - 1)))
                        {
                            if (n.box.Width < rectangle.Width)
                            {
                                if ((n.box.X + n.box.Width) > ((rectangle.X + rectangle.Width) - 1))
                                {
                                    nDiff_X = (n.box.X + n.box.Width) - ((rectangle.X + rectangle.Width) - 1);
                                }
                                else if (n.box.X < rectangle.X)
                                {
                                    nDiff_X = n.box.X - rectangle.X;
                                }
                            }
                            else if ((n.box.X + n.box.Width) > (rectangle.X + rectangle.Width))
                            {
                                nDiff_X = n.box.X - rectangle.X;
                            }
                            else if ((n.box.X + n.box.Width) < (rectangle.X + rectangle.Width))
                            {
                                nDiff_X = (n.box.X + n.box.Width) - (rectangle.X + rectangle.Width);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            if ((nDiff_X == 0) && (nDiff_Y == 0))
            {
                return true;
            }
            return false;
        }
        //
        private bool NotInSameContainer (Node node, Node node2)
        {
            if ((((node.parent_ == null) || (node2.parent_ == null)) || (node.parent_ != node2.parent_)) ||
                (((((node.parent_.type_.type != ElementType.Math) &&
                    (node.parent_.type_.type != ElementType.Mrow)) &&
                   ((node.parent_.type_.type != ElementType.Mtd) &&
                    (node.parent_.type_.type != ElementType.Mstyle))) &&
                  (((node.parent_.type_.type != ElementType.Mphantom) &&
                    (node.parent_.type_.type != ElementType.Msqrt))
                   ))))
            {
                return false;
            }
            return true;
        }
        //
        public void SetTypes (Types typeCollection)
        {
            this.types_ = typeCollection;
        }
        //
        private bool IsMathML (Node node)
        {
            if (((node == null)) ||
                (((((node.type_.type != ElementType.Mrow) &&
                    (node.type_.type != ElementType.Mi)) &&
                   ((node.type_.type != ElementType.Mn) &&
                    (node.type_.type != ElementType.Munder))) &&
                  (((node.type_.type != ElementType.Mover) &&
                    (node.type_.type != ElementType.Msqrt)) &&
                   ((node.type_.type != ElementType.Mroot) &&
                    (node.type_.type != ElementType.Munderover)))) &&
                 (((node.type_.type != ElementType.Mo) && (node.type_.type != ElementType.Msub)) &&
                  ((node.type_.type != ElementType.Msup) &&
                   (node.type_.type != ElementType.Mfenced)))))
            {
                return false;
            }
            return true;
        }
        //
        public Node getRoot ()
        {
            Node r = null;
            try
            {
                if (this.rootNode_ != null)
                {
                    r = this.rootNode_;
                }
            }
            catch
            {
            }
            return r;
        }
        //
        private XmlNode LoadXml (string xml, XmlDocument doc)
        {
            XmlNode root = null;
            string entities = "";
            try
            {
                xml = xml.Trim ();
                if (((xml != null) && (xml.Length > 0)) && ((xml[0] == '<') && (xml[xml.Length - 1] == '>')))
                {
                    try
                    {
                        entities = this.entityManager.GenerateXMLEntities (xml) + xml;
                        doc.LoadXml (entities);
                        root = doc.DocumentElement;
                    }
                    catch
                    {
                        return null;
                    }
                    return root;
                }
                return null;
            }
            catch (Exception ex)
            {
                this.InvalidXML (this, new InvalidXMLArgs (ex.Message, "", this.schema.LineNumber, this.schema.LinePos));
                return null;
            }
        }
        //
        private NodeClass GetNodeClass (Node selNode)
        {
            NodeClass result = NodeClass.unknown;
            try
            {
                if (this.IsToplevelRowCell (selNode))
                {
                    return NodeClass.mcell;
                }
                if ((selNode.type_.type != ElementType.Math) && selNode.isVisible)
                {
                    result = NodeClass.no_class;
                }
            }
            catch
            {
            }
            return result;
        }
        //
        public int WidthToMark (Node node, int mark)
        {
            int w = 0;
            if ((node.literalText != null) && (mark > 0))
            {
                StyleAttributes s = node.style_;
                w = this.painter_.MeasureWidth (node, s, node.literalText.Substring (0, mark));
                try
                {
                    if ((node.type_ != null) && (node.type_.type == ElementType.Ms))
                    {
                        w += ((Box_Ms) node.box).leftQuoteWidth;
                    }
                }
                catch
                {
                }
            }
            return w;
        }
        //
        public Rectangle rectangleToUpdate ()
        {
            return new Rectangle (0, 0, this.clientRect.Width, this.clientRect.Height);
        }
        //
        public bool Validate (string sXML)
        {
            bool result = true;
            
            try
            {
                if (this.schema == null)
                {
                    this.schema = new Schema (this.mathmlSchema);
                }
                if (this.schema != null)
                {
                    result = this.schema.Validate (sXML);
                }
                if ((this.schema != null) && this.schema.IsValid)
                {
                    this.InvalidXML (this,
                             new InvalidXMLArgs (this.schema.Message, "", this.schema.LineNumber, this.schema.LinePos));
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
        //
        public bool LoadXML (string sXML)
        {
            this.clear ();
            bool result = true;
            XmlTextReader reader = null;
            this.CanUndo = false;
            try
            {
                reader = new XmlTextReader (sXML, XmlNodeType.Element, new XmlParserContext (null, new XmlNamespaceManager (new NameTable ()), null, XmlSpace.None));
                this.LoadXML (reader);
            }
            catch (Exception )
            {
                result = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close ();
                }
            }
            return result;
        }
        //
        private void LoadXML (XmlTextReader xr)
        {
            bool bad = false;
            Exception exception = null;
            try
            {
                XmlDocument doc = new XmlDocument ();
                doc.PreserveWhitespace = true;
                string finalXML = "";
                xr.WhitespaceHandling = WhitespaceHandling.All;
                xr.MoveToContent ();
                string xml = xr.ReadOuterXml ();
                string converted = "";
                if (!this.entityManager.TryConvertCommonOperators (xml, ref converted))
                {
                    converted = xml;
                }
                finalXML = this.entityManager.GenerateXMLEntities (converted) + converted;
                doc.LoadXml (finalXML);
                XmlNode root = doc.DocumentElement;
                
                this.EndowWithRows (doc, doc.DocumentElement);
                this.rootNode_ = new Node ();
                if (root != null)
                {
                    this.rootNode_.xmlTagName = root.LocalName;
                    this.rootNode_.Parse (root, this.types_, this.entityManager, true, null,false);
                }
                Node select = null;
               
                if (this.rootNode_.xmlTagName == "math")
                {
                    select = this.rootNode_;
                    try
                    {
                        if ((select != null) && select.HasChildren ())
                        {
                            select = select.firstChild;
                            if (((select.type_.type == ElementType.Mtable) && (select.nextSibling == null)) &&
                                ((select.firstChild != null) &&
                                 (select.firstChild.type_.type == ElementType.Mtr)))
                            {
                                Node row = select.firstChild;
                                if ((row.firstChild != null) &&
                                    (row.firstChild.type_.type == ElementType.Mtd))
                                {
                                    Node cell = row.firstChild;
                                    if (cell.firstChild != null)
                                    {
                                        select = cell.firstChild;
                                    }
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                Node r = this.MakeNode ("mrow");
                                this.rootNode_.AdoptChild (r);
                                this.rootNode_.UpdateLevel ();
                                select = r;
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                
                if (select == null)
                {
                    select = this.rootNode_;
                }
                this.SelectNode (select, false);
            }
            catch (Exception ex)
            {
                bad = true;
                exception = new Exception (ex.Message, ex.InnerException);
            }
            this.CanUndo = false;
            this.undo.Clear ();
            this.redo.Clear ();
            if (bad && (exception != null))
            {
                throw exception;
            }
        }
        //
        private void EndowWithRows (XmlDocument xmlDoc, XmlNode xmlNode)
        {
            if (xmlNode.HasChildNodes)
            {
                XmlNodeList list = xmlNode.ChildNodes;
                for (int i = 0; i < list.Count; i++)
                {
                    this.EndowMathWithRow (xmlDoc, list.Item (i));
                }
            }
        }
        //
        private void EndowMathWithRow (XmlDocument xmlDoc, XmlNode xmlNode)
        {
            if ((xmlNode.NodeType == XmlNodeType.Element) && (xmlNode.LocalName == "math"))
            {
                try
                {
                    if (xmlNode.ChildNodes.Count == 0)
                    {
                        XmlNode n = xmlDoc.CreateNode (XmlNodeType.Element, "mrow", "http://www.w3.org/1998/Math/MathML");
                        if (xmlNode != null)
                        {
                            xmlNode.AppendChild (n);
                        }
                    }
                }
                catch
                {
                }
            }

            if (!xmlNode.HasChildNodes)
            {
                return;
            }

            XmlNodeList list = xmlNode.ChildNodes;
            for (int i = 0; i < list.Count; i++)
            {
                this.EndowMathWithRow (xmlDoc, list.Item (i));
            }
        }
        public void SavePure (string filename)
        {
            bool ok = true;

            XmlDocument doc = new XmlDocument ();
            
            try
            {
                this.rootNode_.SavePure(doc, null, "UTF-8");
            }
            catch (Exception)
            {
                ok = false;
            }
            
            
            if (ok)
            {
                try
                {
                    doc.Save (filename);
                }
                catch
                {
                    ok = false;
                }
            }
        }
        //
        public void Save (string filename)
        {
            bool ok = true;

            XmlDocument doc = new XmlDocument ();
            
            try
            {
                this.rootNode_.SaveToXml (doc, null, "UTF-8");
            }
            catch (Exception)
            {
                ok = false;
            }
            
            
            if (ok)
            {
                try
                {
                    doc.Save (filename);
                }
                catch
                {
                    ok = false;
                }
            }
        }
        //
        public Bitmap Export2Image (PixelFormat pixFormat, float fontSize, int nResolution, bool antiAliasing, ref int BaseLine)
        {
            int width = 0;
            int height = 0;
            int prevX = 0;
            int prevY = 0;
            Bitmap bitmap = null;
            try
            {
                prevX = this.oX;
                prevY = this.oY;
                if (this.rootNode_ == null)
                {
                    return null;
                }
                if (this.rootNode_.box == null)
                {
                    return null;
                }
                if (this.painter_ == null)
                {
                    return null;
                }
                try
                {
                    width = this.rootNode_.box.Width + 10;
                    height = this.rootNode_.box.Height + 10;
                    this.SetOrigin (5, 5);
                    
                    bitmap = new Bitmap (width, height, pixFormat);
                    bitmap.SetResolution ((float) nResolution, (float) nResolution);
                    
                    Graphics graphics = Graphics.FromImage (bitmap);
                    graphics.Clear (Color.White);

                    this.painter_.SetupPainting (graphics, antiAliasing);
                    this.painter_.SetFontSize (fontSize);
                    
                    this.MeasureAll ();
                    
                    float med = 0f;
                    float thin = 0f;
                    float vvthin = 0f;
                    int medMargin = 0;
                    int thinMargin = 0;
                    int vvThinMargin = 0;
                    
                    try
                    {
                        med = this.painter_.CalcSpaceHeight (Rendering.Space.Medium, fontSize, "Times New Roman");
                        med *= 0.9f;
                        thin = this.painter_.CalcSpaceHeight (Rendering.Space.Thin, fontSize, "Times New Roman");
                        vvthin = this.painter_.CalcSpaceHeight (Rendering.Space.VeryVeryThin, fontSize, "Times New Roman");
                        medMargin = Convert.ToInt32 (Math.Round ((double) med));
                        thinMargin = Convert.ToInt32 (Math.Round ((double) thin));
                        vvThinMargin = Convert.ToInt32 (Math.Round ((double) vvthin));
                        
                    }
                    catch
                    {
                    }
                    width = (this.rootNode_.box.Width + medMargin) + thinMargin;
                    height = this.rootNode_.box.Height + vvThinMargin + vvThinMargin;
                    BaseLine = this.rootNode_.box.Baseline + vvThinMargin;
                    this.SetOrigin (medMargin, vvThinMargin);
                    bitmap = new Bitmap (width, height, pixFormat);
                    bitmap.SetResolution ((float) nResolution, (float) nResolution);
                    graphics = Graphics.FromImage (bitmap);
                    graphics.Clear (Color.White);
                    this.painter_.SetupPainting (graphics, antiAliasing);
                    this.painter_.SetFontSize (fontSize);
                    this.MeasureAll ();
                    this.DrawRoot (new Rectangle (0, 0, width, height));
                }
                catch
                {
                }
                this.painter_.SetFontSize (this.FontSize);
                this.SetOrigin (prevX, prevY);
            }
            catch
            {
            }
            return bitmap;
        }
        //
        public string ProcessEntities (string sSource)
        {
            string replaced = "";
            if (this.entityManager.ReplaceEntities (sSource, ref replaced) && (replaced.Length > 0))
            {
                return replaced;
            }
            return sSource;
        }
        //
        private void SelectTop ()
        {
            try
            {
                Node top = null;
                try
                {
                    Node sel = this.selectedNode;
                    bool d = true;
                   
                    try
                    {
                        while (sel.parent_ != null)
                        {
                            if (sel.tagDeleted)
                            {
                                d = false;
                            }
                            sel = sel.parent_;
                        }
                    }
                    catch
                    {
                    }
                    if (((sel == null) || (sel != this.rootNode_)) || !d)
                    {
                        top = this.TopContainer ();
                    }
                    else
                    {
                        try
                        {
                            if (((this.selectedNode == null) || (this.selectedNode == this.rootNode_)) ||
                                ((this.selectedNode.type_.type == ElementType.Math) ||
                                 ((this.selectedNode.type_.type == ElementType.Mtable) &&
                                  (this.selectedNode.Class == "nugentoplevel"))))
                            {
                                top = this.TopContainer ();
                            }
                            else if (this.selectedNode.type_.type == ElementType.Mtr)
                            {
                                if ((this.selectedNode.firstChild != null) &&
                                    (this.selectedNode.firstChild.type_.type == ElementType.Mtd))
                                {
                                    Node first = this.selectedNode.firstChild;
                                    if (first.firstChild != null)
                                    {
                                        top = first.firstChild;
                                    }
                                    else
                                    {
                                        top = this.WrapInMrow (first);
                                    }
                                }
                                else
                                {
                                    top = this.TopContainer ();
                                }
                            }
                            else if (this.selectedNode.type_.type == ElementType.Mtd)
                            {
                                if (this.selectedNode.firstChild != null)
                                {
                                    top = this.selectedNode.firstChild;
                                }
                                else
                                {
                                    top = this.WrapInMrow (this.selectedNode);
                                }
                            }
                            else if (this.selectedNode.type_.type == ElementType.Entity)
                            {
                                if (this.selectedNode.parent_ != null)
                                {
                                    top = this.selectedNode.parent_;
                                }
                                else
                                {
                                    top = this.TopContainer ();
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
                if (top != null)
                {
                    this.HasSelection = false;
                    this.SelectNode (top, false);
                }
            }
            catch
            {
            }
        }
        //
        private Node WrapInMrow (Node node)
        {
            Node row = null;
            try
            {
                row = this.MakeNode ("mrow");
                node.AdoptChild (row);
                node.UpdateLevel ();
            }
            catch
            {
            }
            return row;
        }
        //
        private Node TopContainer ()
        {
            Node container = null;
            try
            {
                if (this.rootNode_.firstChild != null)
                {
                    Node first = this.rootNode_.firstChild;
                    if ((first.type_.type != ElementType.Mtable) || (first.Class != "nugentoplevel"))
                    {
                        return first;
                    }
                    if ((first.firstChild == null) || (first.firstChild.type_.type != ElementType.Mtr))
                    {
                        return container;
                    }
                    first = first.firstChild;
                    if ((first.firstChild == null) || (first.firstChild.type_.type != ElementType.Mtd))
                    {
                        return container;
                    }
                    first = first.firstChild;
                    if (first.firstChild != null)
                    {
                        return first.firstChild;
                    }
                    return this.WrapInMrow (first);
                }
                return this.WrapInMrow (this.rootNode_);
            }
            catch
            {
                return container;
            }
        }
        //
        public void MeasureAll ()
        {
            try
            {
                this.SelectTop ();
            }
            catch
            {
            }
            try
            {
                if (this.painter_ != null)
                {
                    this.rootNode_.MeasurePass(this.painter_);
                    this.rootNode_.PositionPass();
                }
            }
            catch
            {
            }
        }
        //
        public void FillBackground (Rectangle rect)
        {
            if (this.rootNode_ != null)
            {
                this.rootNode_.print (rect, PaintMode.BACKGROUND, Color.Black);
            }
        }
        //
        public void FillForeground (Rectangle rect)
        {
            if (this.rootNode_ != null)
            {
                this.rootNode_.print (rect, PaintMode.FOREGROUND, Color.Black);
            }
        }
        //
        public void DrawRoot (Rectangle rect)
        {
            if (this.rootNode_ != null)
            {
                this.FillBackground (rect);
                this.FillForeground (rect);
            }
        }
        //
        public void UpdateVertical ()
        {
            if (this.rootNode_ != null)
            {
                this.rootNode_.PropogateYOffset ();
            }
        }
        //
        public bool IsValidXML (string xml)
        {
            bool ok = true;
            XmlTextReader reader = null;
            if (xml == null)
            {
                return false;
            }
            if (xml.Length == 0)
            {
                return false;
            }
            xml = xml.Trim ();
            if (xml.Substring (0, 5) != "<math")
            {
                return false;
            }
            if (xml.Substring (xml.Length - 7, 7) != "</math>")
            {
                return false;
            }
            try
            {
                reader = new XmlTextReader (xml, XmlNodeType.Element, new XmlParserContext (null, new XmlNamespaceManager (new NameTable ()), null, XmlSpace.None));
                XmlDocument document = new XmlDocument ();
                document.PreserveWhitespace = true;
                reader.WhitespaceHandling = WhitespaceHandling.All;
                reader.MoveToContent ();
                string outerxml = reader.ReadOuterXml ();
                xml = this.entityManager.GenerateXMLEntities (outerxml) + outerxml;
                document.LoadXml (xml);
            }
            catch
            {
                ok = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close ();
                }
            }
            return ok;
        }
        //
        public bool SelectToEnd ()
        {
            try
            {
                Node multi = null;
                Node last = null;
                if (this.IsMultiline)
                {
                    bool found = false;
                    Node cur = this.GetCurrentlySelectedNode();
                    while (!this.IsToplevelRowCell(cur) && (cur.level > 3))
                    {
                        cur = cur.parent_;
                        found = true;
                    }
                    if (this.IsToplevelRowCell(cur))
                    {
                        if (found)
                        {
                            this.SelectNode(cur, false);
                            cur = this.GetCurrentlySelectedNode();
                        }
                        multi = cur;
                        last = multi.parent_.lastChild;
                    }
                }
                else
                {
                    bool found = false;
                    Node cur = this.GetCurrentlySelectedNode();
                    while (cur.level > 1)
                    {
                        cur = cur.parent_;
                        found = true;
                    }
                    if (cur.level == 1)
                    {
                        if (found)
                        {
                            this.SelectNode(cur, false);
                            cur = this.GetCurrentlySelectedNode();
                        }
                        multi = cur;
                        last = multi.parent_.lastChild;
                    }
                }
                if ((multi != null) && (last != null))
                {
                    if ((multi == last) && (multi.InternalMark == multi.LiteralLength))
                    {
                        return false;
                    }
                    this.multiSelectNode = multi;
                    this.SelectNode(last, true);
                    this.hasSelection = true;
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }
        //
        public bool SelectToStart ()
        {
            try
            {
                Node first = null;
                Node multi = null;
                if (this.IsMultiline)
                {
                    bool found = false;
                    Node cur = this.GetCurrentlySelectedNode();
                    while (!this.IsToplevelRowCell(cur) && (cur.level > 3))
                    {
                        cur = cur.parent_;
                        found = true;
                    }
                    if (this.IsToplevelRowCell(cur))
                    {
                        if (found)
                        {
                            this.SelectNode(cur, false);
                            cur = this.GetCurrentlySelectedNode();
                        }
                        multi = cur;
                        first = multi.parent_.firstChild;
                    }
                }
                else
                {
                    bool found = false;
                    Node cur = this.GetCurrentlySelectedNode();
                    while (cur.level > 1)
                    {
                        cur = cur.parent_;
                        found = true;
                    }
                    if (cur.level == 1)
                    {
                        if (found)
                        {
                            this.SelectNode(cur, false);
                            cur = this.GetCurrentlySelectedNode();
                        }
                        multi = cur;
                        first = multi.parent_.firstChild;
                    }
                }
                if ((first != null) && (multi != null))
                {
                    if ((first == multi) && (multi.InternalMark == 0))
                    {
                        return false;
                    }
                    this.multiSelectNode = multi;
                    this.SelectNode(first, false);
                    this.hasSelection = true;
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }
        //
        public bool SelectAll ()
        {
            try
            {
                Node first = null;
                Node last = null;
                if (this.IsMultiline)
                {
                    Node firstCell = null;
                    Node firstContainer = null;
                    Node lastCell = null;
                    Node lastContainer = null;
                    if (((this.rootNode_.firstChild == null) ||
                         (this.rootNode_.firstChild.type_.type != ElementType.Mtable)) ||
                        (this.rootNode_.firstChild.Class != "nugentoplevel"))
                    {
                        return false;
                    }
                    if ((this.rootNode_.firstChild.firstChild != null) &&
                        (this.rootNode_.firstChild.firstChild.type_.type == ElementType.Mtr))
                    {
                        firstCell = this.rootNode_.firstChild.firstChild;
                        if ((firstCell.firstChild != null) &&
                            (firstCell.firstChild.type_.type == ElementType.Mtd))
                        {
                            firstContainer = firstCell.firstChild;
                        }
                    }
                    if ((this.rootNode_.firstChild.lastChild != null) &&
                        (this.rootNode_.firstChild.lastChild.type_.type == ElementType.Mtr))
                    {
                        lastCell = this.rootNode_.firstChild.lastChild;
                        if ((lastCell.firstChild != null) &&
                            (lastCell.firstChild.type_.type == ElementType.Mtd))
                        {
                            lastContainer = lastCell.firstChild;
                        }
                    }
                    if ((firstContainer == null) || (lastContainer == null))
                    {
                        return false;
                    }
                    first = firstContainer.firstChild;
                    last = lastContainer.lastChild;
                    if ((first == null) || (last == null))
                    {
                        return false;
                    }
                    this.multiSelectNode = last;
                    this.currentCaret = last.LiteralLength;
                    this.selectedNode = first;
                    first.InternalMark = 0;
                    this.SelectNode(first, false);
                    this.hasSelection = true;
                    return true;
                }
                first = this.rootNode_.firstChild;
                last = this.rootNode_.lastChild;
                if ((first != null) && (last != null))
                {
                    this.multiSelectNode = last;
                    this.currentCaret = last.LiteralLength;
                    this.selectedNode = first;
                    first.InternalMark = 0;
                    this.SelectNode(first, false);
                    this.hasSelection = true;
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }
        //
        public Node GetRootNode ()
        {
            Node node = null;
            try
            {
                if (this.rootNode_ == null)
                {
                    return node;
                }
                
                if (this.rootNode_.xmlTagName == "math")
                {
                    node = this.rootNode_;
                    if (node == null)
                    {
                        return node;
                    }
                }
            }
            catch
            {
            }
            return node;
        }
        //
        public bool SelectNode (int x, int y)
        {
            this.HasSelection = false;
            Node foundNode = null;
            try
            {
                Point point = new Point (x, y);
                CaretLocation caretLocation = new CaretLocation ();
                try
                {
                    if (this.rootNode_ != null)
                    {
                        if (point.X < 0)
                        {
                            point.X = 0;
                        }
                        if (point.Y < 0)
                        {
                            point.Y = 0;
                        }
                        if (point.X > (this.rootNode_.box.X + this.rootNode_.box.Width))
                        {
                            point.X = (this.rootNode_.box.X + this.rootNode_.box.Width) - 1;
                        }
                        if (point.Y > (this.rootNode_.box.Y + this.rootNode_.box.Height))
                        {
                            point.Y = (this.rootNode_.box.Y + this.rootNode_.box.Height) - 1;
                        }
                    }
                }
                catch
                {
                }
                foundNode = this.rootNode_.NodeAtPoint (point, caretLocation);
                if ((foundNode == null) || !foundNode.isVisible)
                {
                    return false;
                }

                if (((foundNode.type_.type == ElementType.Entity) && (foundNode.parent_ != null)) &&
                    ((foundNode.parent_.type_.type == ElementType.Mi) ||
                     (foundNode.parent_.type_.type == ElementType.Mo)))
                {
                    foundNode = foundNode.parent_;
                }
                
                if ((foundNode.level == 0))
                {
                    return false;
                }

                if (((foundNode.level == 1)) &&
                    ((foundNode.type_.type == ElementType.Mtable) && (foundNode.Class == "nugentoplevel")))
                {
                    return false;
                }
                
                if (foundNode.skip)
                {
                    try
                    {
                        if ((foundNode.type_.type == ElementType.Mrow) && (foundNode.firstChild == null))
                        {
                            if (caretLocation.pos == CaretPosition.Right)
                            {
                                this.SelectNode (foundNode, true);
                            }
                            else
                            {
                                this.SelectNode (foundNode, false);
                            }
                            return true;
                        }
                        if (foundNode.type_.type == ElementType.Mtr)
                        {
                            if ((foundNode.firstChild != null) &&
                                (foundNode.firstChild.type_.type == ElementType.Mtd))
                            {
                                Node child = this.ChildNodeFromPoint (foundNode, point.X, point.Y, caretLocation);
                                if ((child != null) && (child.firstChild != null))
                                {
                                    Node target = this.ChildNodeFromPoint (child, point.X, point.Y, caretLocation);
                                    if (target != null)
                                    {
                                        if (caretLocation.pos == CaretPosition.Right)
                                        {
                                            if ((target.nextSibling != null) && target.nextSibling.isVisible)
                                            {
                                                this.SelectNode (target.nextSibling, false);
                                                return true;
                                            }
                                            this.SelectNode (target, true);
                                            return true;
                                        }
                                        this.SelectNode (target, false);
                                        return true;
                                    }
                                }
                            }
                            return false;
                        }
                        if (foundNode.type_.type == ElementType.Mtd)
                        {
                            if ((foundNode.firstChild != null) && foundNode.firstChild.isVisible)
                            {
                                Node cell = foundNode;
                                if ((cell != null) && (cell.firstChild != null))
                                {
                                    Node target = this.ChildNodeFromPoint (cell, point.X, point.Y, caretLocation);
                                    if (target != null)
                                    {
                                        if (caretLocation.pos == CaretPosition.Right)
                                        {
                                            if ((target.nextSibling != null) && target.nextSibling.isVisible)
                                            {
                                                this.SelectNode (target.nextSibling, false);
                                                return true;
                                            }
                                            this.SelectNode (target, true);
                                            return true;
                                        }
                                        this.SelectNode (target, false);
                                        return true;
                                    }
                                }
                            }
                            return false;
                        }
                    }
                    catch
                    {
                    }
                    if ((foundNode.level == 0))
                    {
                        return false;
                    }
                    this.SelectNode (foundNode, false);
                    return this.GoRight ();
                }
                if (foundNode.isGlyph)
                {
                    if ((foundNode.level == 0))
                    {
                        return false;
                    }
                    this.SelectNode (foundNode, false);
                    return this.GoLeft ();
                }
                if ((foundNode.IsAtom() && (foundNode.literalText != null)) && (foundNode.literalText.Length > 1))
                {
                    this.SelectNode (foundNode, false);
                    this.selectedNode.MarkFromPoint (point);
                    this.selectedNode.LiteralStart = this.selectedNode.LiteralStart;
                    return true;
                }
                
                if (caretLocation.pos == CaretPosition.Right)
                {
                    if ((foundNode.level == 0))
                    {
                        return false;
                    }
                    if (foundNode.nextSibling != null)
                    {
                        this.SelectNode (foundNode, false);
                        return this.GoRight ();
                    }
                    this.SelectNode (foundNode, true);
                }
                else
                {
                    if ((foundNode.level == 0))
                    {
                        return false;
                    }
                    this.SelectNode (foundNode, false);
                }
                return true;
            }
            catch
            {
            }
            
            return false;
        }
        //
        public void SelectNode (Node node, bool markAppend)
        {
            try
            {
                if (node == null)
                {
                    return;
                }
                
                this.lastSelectedNode = this.selectedNode;
                this.selectedNode = node;
                try
                {
                    if ((this.lastSelectedNode != null) && (this.selectedNode != this.lastSelectedNode))
                    {
                        this.lastSelectedNode.InternalMark = 0;
                        this.lastSelectedNode.IsAppend = false;
                    }
                }
                catch
                {
                }
                
                this.selectedNode.IsAppend = markAppend;
            }
            catch
            {
            }
            finally
            {
                try
                {
                    if (this.selectedNode.InternalMark == 0)
                    {
                        this.selectedNode.LiteralStart = 0;
                    }
                }
                catch
                {
                }
            }
        }
        //
        public void RemoveSelection ()
        {
            try
            {
                if (this.HasSelection)
                {
                    Node cur = this.GetCurrentlySelectedNode ();
                    if ((cur != this.multiSelectNode) || (cur.InternalMark != this.currentCaret))
                    {
                        return;
                    }
                    this.HasSelection = false;
                }
            }
            catch
            {
            }
        }
        //
        private Node ChildNodeFromPoint (Node Parent_Node, int x, int y, CaretLocation result)
        {
            Node node = null;
            try
            {
                bool done = false;
                int i = 0;
                NodesList childrenNodes = null;
                childrenNodes = Parent_Node.GetChildrenNodes ();
                int count = 0;
                if (childrenNodes == null)
                {
                    return node;
                }
                count = childrenNodes.Count;
                while (!done && (i < count))
                {
                    Node child = null;
                    child = childrenNodes.Get (i);
                    if ((child != null) && child.isVisible)
                    {
                        if ((x >= child.box.X) && (x <= (child.box.X + (child.box.Width / 2))))
                        {
                            node = child;
                            result.pos = CaretPosition.Left;
                            done = true;
                        }
                        else if ((x >= (child.box.X + (child.box.Width / 2))) && (x <= (child.box.X + child.box.Width)))
                        {
                            node = child;
                            result.pos = CaretPosition.Right;
                            done = true;
                        }
                    }
                    i++;
                }
                if ((!done && (Parent_Node.lastChild != null)) && Parent_Node.lastChild.isVisible)
                {
                    node = Parent_Node.lastChild;
                }
            }
            catch
            {
            }
            return node;
        }
        //
        public bool SelectionTo (int x, int y)
        {
            try
            {
                Node mcell;
                Node lastSelected = this.MultiSelectNode();
                Point point = new Point(x, y);
                CaretLocation location = new CaretLocation();
                try
                {
                    if (this.rootNode_ != null)
                    {
                        if (point.X < 0)
                        {
                            point.X = 0;
                        }
                        if (point.Y < 0)
                        {
                            point.Y = 0;
                        }
                        if (point.X > (this.rootNode_.box.X + this.rootNode_.box.Width))
                        {
                            point.X = (this.rootNode_.box.X + this.rootNode_.box.Width) - 1;
                        }
                        if (point.Y > (this.rootNode_.box.Y + this.rootNode_.box.Height))
                        {
                            point.Y = (this.rootNode_.box.Y + this.rootNode_.box.Height) - 1;
                        }
                    }
                }
                catch
                {
                }
                Node nodeatPoint = this.rootNode_.NodeAtPoint(point, location);
                if (((nodeatPoint.type_.type == ElementType.Entity) && (nodeatPoint.parent_ != null)) &&
                    ((nodeatPoint.parent_.type_.type == ElementType.Mi) ||
                     (nodeatPoint.parent_.type_.type == ElementType.Mo)))
                {
                    nodeatPoint = nodeatPoint.parent_;
                }
                if ((nodeatPoint == null) || (lastSelected == null))
                {
                    return false;
                }
                if (nodeatPoint.level == 0)
                {
                    return false;
                }
                if (((nodeatPoint.level == 1)) &&
                    ((nodeatPoint.type_.type == ElementType.Mtable) && (nodeatPoint.Class == "nugentoplevel")))
                {
                    return false;
                }
                if (this.IsToplevelRowCell(this.multiSelectNode))
                {
                    bool top = false;
                    if ((nodeatPoint.level == 2) && (nodeatPoint.type_.type == ElementType.Mtr))
                    {
                        if ((nodeatPoint.firstChild != null) && (nodeatPoint.firstChild.type_.type == ElementType.Mtd))
                        {
                            Node cell = nodeatPoint.firstChild;
                            Node target = this.ChildNodeFromPoint(cell, x, y, location);
                            if (target != null)
                            {
                                nodeatPoint = target;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if ((nodeatPoint.level == 3) && (nodeatPoint.type_.type == ElementType.Mtd))
                    {
                        Node cell = nodeatPoint;
                        Node target = this.ChildNodeFromPoint(cell, point.X, point.Y, location);
                        if (target != null)
                        {
                            nodeatPoint = target;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    
                    mcell = nodeatPoint;
                    if (mcell.level > 4)
                    {
                        while (mcell.level > 4)
                        {
                            mcell = mcell.parent_;
                        }
                    }
                    if ((((mcell.level == 4) && mcell.isVisible) &&
                         ((mcell.level == this.multiSelectNode.level) && this.IsToplevelRowCell(mcell))) &&
                        (mcell.parent_ != this.multiSelectNode.parent_))
                    {
                        top = true;
                        nodeatPoint = mcell;
                    }
                    if (top)
                    {
                        try
                        {
                            bool atom = false;
                            if ((nodeatPoint.IsAtom() && (nodeatPoint.literalText != null)) &&
                                (nodeatPoint.literalText.Length > 1))
                            {
                                if (point.X > (nodeatPoint.box.X + nodeatPoint.box.Width))
                                {
                                    this.SelectNode(nodeatPoint, true);
                                }
                                else
                                {
                                    this.SelectNode(nodeatPoint, false);
                                    this.selectedNode.MarkFromPoint(point);
                                    this.selectedNode.LiteralStart = this.selectedNode.LiteralStart;
                                }
                                atom = true;
                            }
                            else
                            {
                                if (location.pos == CaretPosition.Right)
                                {
                                    if ((nodeatPoint.nextSibling != null) && nodeatPoint.nextSibling.isVisible)
                                    {
                                        this.SelectNode(nodeatPoint.nextSibling, false);
                                    }
                                    else
                                    {
                                        this.SelectNode(nodeatPoint, true);
                                    }
                                }
                                else
                                {
                                    this.SelectNode(nodeatPoint, false);
                                }
                                atom = true;
                            }
                            if (!atom)
                            {
                                return false;
                            }

                            return true;
                        }
                        catch
                        {
                        }
                        return false;
                    }
                }
                
                if (((nodeatPoint.parent_ == null) || (lastSelected.parent_ == null)) || (lastSelected.parent_ != nodeatPoint.parent_))
                {
                    Node n = nodeatPoint;
                    while ((n.parent_ != null) && (n.parent_ != lastSelected.parent_))
                    {
                        n = n.parent_;
                    }
                    if ((n.parent_ == lastSelected.parent_) && n.isVisible)
                    {
                        nodeatPoint = n;
                        if ((point.X >= (nodeatPoint.box.X + (nodeatPoint.box.Width/2))) &&
                            (point.X <= (nodeatPoint.box.X + nodeatPoint.box.Width)))
                        {
                            location.pos = CaretPosition.Right;
                        }
                        else
                        {
                            location.pos = CaretPosition.Left;
                        }
                    }
                }
                if (((nodeatPoint.parent_ != null) && (lastSelected.parent_ != null)) && (lastSelected.parent_ == nodeatPoint.parent_))
                {
                    if ((nodeatPoint.parent_.type_.minChilds > 1) && (nodeatPoint != lastSelected))
                    {
                        return false;
                    }
                    if (!nodeatPoint.isVisible)
                    {
                        return false;
                    }
                    
                    if ((nodeatPoint.IsAtom() && (nodeatPoint.literalText != null)) && (nodeatPoint.literalText.Length > 1))
                    {
                        this.SelectNode(nodeatPoint, false);
                        this.selectedNode.MarkFromPoint(point);
                        return true;
                    }
                    if (location.pos == CaretPosition.Right)
                    {
                        if (nodeatPoint.nextSibling != null)
                        {
                            this.SelectNode(nodeatPoint, false);
                            return this.SelectionRight();
                        }
                        this.SelectNode(nodeatPoint, true);
                        return true;
                    }
                    this.SelectNode(nodeatPoint, false);
                    return true;
                }

                return false;
            }
            catch
            {
            }
            return false;
        }
        
        //
        private bool IsMultiline
        {
            get
            {
                try
                {
                    if ((((this.rootNode_ != null)) &&
                         ((this.rootNode_.type_.type == ElementType.Math) &&
                          (this.rootNode_.numChildren == 1))) &&
                        (((this.rootNode_.firstChild != null) &&
                          (this.rootNode_.firstChild.type_.type == ElementType.Mtable)) &&
                         (this.rootNode_.firstChild.Class == "nugentoplevel")))
                    {
                        return true;
                    }
                }
                catch
                {
                }
                return false;
            }
        }
        //
        public bool StretchyBrackets
        {
            get { return this.stretchyBrackets; }
            set { this.stretchyBrackets = value; }
        }
        //
        public int HorizontalRes
        {
            get { return this.horizontalRes; }
            set { this.horizontalRes = value; }
        }
        //
        public float FontSize
        {
            get { return this.fontSize; }
            set
            {
                this.fontSize = value;
                if (this.painter_ != null)
                {
                    try
                    {
                        this.painter_.SetFontSize (this.FontSize);
                    }
                    catch
                    {
                    }
                }
            }
        }
        //
        public int RootHeight
        {
            get
            {
                int r = 0;
                if (this.rootNode_ == null)
                {
                    return 0;
                }
                
                if (this.rootNode_.box != null)
                {
                    r = this.rootNode_.box.Height;
                }
                return r;
            }
        }
        //
        public int RootWidth
        {
            get
            {
                if ((this.rootNode_ != null) && (this.rootNode_.box != null))
                {
                    return this.rootNode_.box.Width;
                }
                return 0;
            }
        }
        //
        public bool CanUndo
        {
            get { return this.canUndo_; }
            set { this.canUndo_ = value; }
        }
        //
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }
        //
        public bool NotUberRootNode
        {
            get
            {
                Node n = this.GetCurrentlySelectedNode ();
                if (n != null && n.type_ != null && n.type_.type != ElementType.Math)
                        {
                            return true;
                        }
                        
                return false;
            }
        }
        //
        public bool HasSelection
        {
            get { return this.hasSelection; }
            set
            {
                this.hasSelection = value;
                
                if (!this.hasSelection)
                {
                    this.multiSelectNode = null;
                    this.currentCaret = 0;
                }
                else
                {
                    this.multiSelectNode = this.GetCurrentlySelectedNode ();
                }
            }
        }
        //
        public Node RootNode
        {
            get { return this.rootNode_; }
        }

        private int width = 800;
        private FontCollection fonts_;
        private string mathmlSchema;
        public Rectangle bounds;
        public string mathmlNamespace;
        private bool canUndo_;
        
        private Painter painter_;
        public Types types_;
        private Node rootNode_;
        private Node selectedNode;
        private Node lastSelectedNode;
        private Node multiSelectNode;
        private int currentCaret;
        private int horizontalRes;
        private int oX;
        private int oY;
        public bool hasSelection;
        private Rectangle clientRect;
        private float fontSize;
        private EntityManager entityManager;
        public OperatorDictionary operators_;
        private Schema schema;
        private UndoRedoStack undo;
        private UndoRedoStack redo;
        private bool stretchyBrackets;

        public enum SelectionDirection
        {
            Right,
            Left,
            Up,
            Down
        }
    }
}