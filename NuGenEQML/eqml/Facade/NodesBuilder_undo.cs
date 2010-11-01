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
        private bool DoUndo ()
        {
            if ((this.undo.Depth > 0))
            {
                NodesInfo info = this.undo.Pop ();
                if (info != null)
                {
                    this.HasSelection = false;
                    this.rootNode_ = info.RootNode;
                    this.multiSelectNode = null;
                    this.currentCaret = 0;
                    this.selectedNode = info.SelectedNode;
                    this.selectedNode.InternalMark = info.Mark;
                    this.lastSelectedNode = info.LastSelected;
                    return true;
                }
            }
            return false;
        }

        private bool DoRedo ()
        {
            if ((this.redo.Depth > 0))
            {
                NodesInfo info = this.redo.Pop ();
                if (info != null)
                {
                    this.HasSelection = false;
                    this.rootNode_ = info.RootNode;
                    this.multiSelectNode = null;
                    this.currentCaret = 0;
                    this.selectedNode = info.SelectedNode;
                    this.selectedNode.InternalMark = info.Mark;
                    this.lastSelectedNode = info.LastSelected;
                    return true;
                }
            }
            return false;
        }

        public bool HasUndo ()
        {
            if (this.undo.Depth > 0)
            {
                return true;
            }
            return false;
        }

        public bool HasRedo ()
        {
            if ((this.redo.Depth > 0))
            {
                return true;
            }
            return false;
        }

        public bool Redo ()
        {
            if ((this.redo.Depth > 0))
            {
                this.undo.Push (new NodesInfo (this.rootNode_, this.selectedNode, this.selectedNode.InternalMark, this.lastSelectedNode));
                if (this.DoRedo ())
                {
                    this.CanUndo = true;
                    return true;
                }
            }
            return false;
        }

        public bool Undo ()
        {
            try
            {
                if (this.undo.Depth > 0)
                {
                    this.CaptureRedo();
                    if (this.DoUndo())
                    {
                        this.CanUndo = true;
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch
            {
            }
            return false;
        }

        private void UndoCallback (object sender, EventArgs e)
        {
            try
            {
                this.UndoRedo (this, new EventArgs ());
            }
            catch
            {
            }
        }

        private void RedoCallback (object sender, EventArgs e)
        {
            try
            {
                this.UndoRedo (this, new EventArgs ());
            }
            catch
            {
            }
        }

        private void CaptureUndo ()
        {
            this.redo.Clear();
            this.undo.Push(new NodesInfo(this.rootNode_, this.selectedNode, this.selectedNode.InternalMark,
                                              this.lastSelectedNode));
        }

        private void CaptureRedo ()
        {
            this.redo.Push(new NodesInfo(this.rootNode_, this.selectedNode, this.selectedNode.InternalMark, this.lastSelectedNode));
        }
    }
}