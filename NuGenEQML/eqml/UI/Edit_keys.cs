namespace UI
{
    using Microsoft.Win32;
    using Attrs;
    using Facade;
    using JpegComment;
    using Boxes;
    using Nodes;
    using Operators;
    using Fonts;
    using UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;

    internal partial class CoreControl : UserControl
    {
        protected override bool ProcessDialogKey (Keys keyData)
        {
            if (builder_.GetCurrentlySelectedNode () == null)
            {
                return true;
            }
            
                switch (keyData)
                {
                    case (Keys.Control | Keys.F):
                    {
                        InsertFraction();
                        return true;
                    }
                    case (Keys.Control | Keys.H):
                    {
                        Command_SupScript();
                        return true;
                    }

                    case (Keys.Control | Keys.J):
                    {
                        Command_SubSupScript();
                        return true;
                    }
                    case (Keys.Control | Keys.L):
                    {
                        Command_SubScript();
                        return true;
                    }
                    case (Keys.Control | Keys.P):
                    {
                        ShowPropertiesDialog();
                        ReRender();
                        return true;
                    }
                    case (Keys.Control | Keys.R):
                    {
                        InsertSqrt();
                        return true;
                    }
                    case (Keys.Control | Keys.T):
                    {
                        InsertMatrix();
                        return true;
                    }

                    case (Keys.Control | Keys.Add):
                    {
                        FontSize += 1f;
                        return true;
                    }

                    case (Keys.Control | Keys.Subtract):
                    {
                        if (FontSize >= 2f)
                        {
                            FontSize -= 1f;
                        }
                        return true;
                    }
                    case (Keys.Control | Keys.Shift | Keys.A):
                    {
                        ReRender();
                        InsertAction();
                        ReRender();
                        return true;
                    }

                    case (Keys.Control | Keys.Shift | Keys.F):
                    {
                        InsertFenced();
                        ReRender();
                        return true;
                    }
                    case (Keys.Alt | Keys.O):
                    {
                        InsertEntity_Open_IdentifierDictionary_Dialog(false);
                        return true;
                    }
                    case (Keys.Alt | Keys.S):
                    {
                        InsertEntity_Open_IdentifierDictionary_Dialog(true);
                        return true;
                    }
                    case (Keys.Alt | Keys.Control | Keys.Y):
                    {
                        StyleProperties();
                        ReRender();
                        return true;
                    }
                    case Keys.Back:
                    {
                        builder_.Back();
                        needUpdate = true;
                        ReRender();
                        return true;
                    }
                    case Keys.Return:
                    {
                        if (builder_.EnterPressed_NeedSplit())
                        {
                            needUpdate = true;
                            ReRender();
                            return true;
                        }
                        break;
                    }
                    case Keys.Space:
                    {
                        if (builder_.Space())
                        {
                            ReRender();
                        }
                        return true;
                    }
                    case Keys.Prior:
                    {
                        GotoPrev();
                        return true;
                    }
                    case Keys.Next:
                    {
                        GotoNext();
                        return true;
                    }
                    case Keys.End:
                    {
                        try
                        {
                            if (GotoLast())
                            {
                                needUpdate = true;
                                ReRender();
                            }
                        }
                        catch
                        {
                        }
                        return true;
                    }
                    case Keys.Home:
                    {
                        try
                        {
                            if (GoHome())
                            {
                                needUpdate = true;
                                ReRender();
                            }
                        }
                        catch
                        {
                        }
                        return true;
                    }
                    case Keys.Left:
                    {
                        if (builder_.GoLeft(true))
                        {
                            needUpdate = true;
                            ReRender();
                        }
                        return true;
                    }
                    case Keys.Up:
                    {
                        if (builder_.WordUp())
                        {
                            needUpdate = true;
                            ReRender();
                            return true;
                        }
                        break;
                    }
                    case Keys.Right:
                    {
                        if (builder_.GoRight(true))
                        {
                            needUpdate = true;

                            ReRender();
                        }
                        return true;
                    }
                    case Keys.Down:
                    {
                        if (builder_.GoDown())
                        {
                            needUpdate = true;
                            ReRender();
                            return true;
                        }
                        break;
                    }

                    case Keys.Delete:
                    {
                        Delete();
                        ReRender();
                        return true;
                    }
                    case (Keys.Shift | Keys.End):
                    {
                        builder_.SelectToEnd();
                        needUpdate = true;
                        ReRender();
                        return true;
                    }
                    case (Keys.Shift | Keys.Home):
                    {
                        builder_.SelectToStart();
                        needUpdate = true;
                        ReRender();
                        return true;
                    }
                    case (Keys.Shift | Keys.Left):
                    {
                        builder_.MoveSelectionPoint(NodesBuilder.SelectionDirection.Left);
                        needUpdate = true;

                        ReRender();
                        return true;
                    }
                    case (Keys.Shift | Keys.Up):
                    {
                        builder_.MoveSelectionPoint(NodesBuilder.SelectionDirection.Up);

                        needUpdate = true;

                        ReRender();
                        return true;
                    }
                    case (Keys.Shift | Keys.Right):
                    {
                        builder_.MoveSelectionPoint(NodesBuilder.SelectionDirection.Right);

                        needUpdate = true;

                        ReRender();
                        return true;
                    }
                    case (Keys.Shift | Keys.Down):
                    {
                        builder_.MoveSelectionPoint(NodesBuilder.SelectionDirection.Down);

                        needUpdate = true;

                        ReRender();
                        return true;
                    }
                    case (Keys.Shift | Keys.Insert):
                    case (Keys.Control | Keys.V):
                        Paste();
                        return true;

                    case (Keys.Control | Keys.Space):
                    {
                        builder_.InvisibleTimes();
                        ReRender();
                        return true;
                    }
                    case (Keys.Control | Keys.Left):
                    {
                        if (builder_.WordLeft())
                        {
                            needUpdate = true;
                            ReRender();
                        }
                        return true;
                    }
                    case (Keys.Control | Keys.Up):
                    {
                        if (builder_.WordUp())
                        {
                            needUpdate = true;
                            ReRender();
                            return true;
                        }
                        break;
                    }
                    case (Keys.Control | Keys.Right):
                    {
                        if (builder_.WordRight())
                        {
                            needUpdate = true;
                            ReRender();
                        }
                        return true;
                    }
                    case (Keys.Control | Keys.Down):
                    {
                        if (builder_.GoDown())
                        {
                            needUpdate = true;
                            ReRender();
                            return true;
                        }

                        break;
                    }

                    case (Keys.Control | Keys.A):
                    {
                        try
                        {
                            if (builder_.SelectAll())
                            {
                                needUpdate = true;
                                ReRender();
                            }
                        }
                        catch
                        {
                        }
                        return true;
                    }

                    case (Keys.Control | Keys.C):
                    case (Keys.Control | Keys.Insert):
                        Copy();
                        return true;
                        
                    case (Keys.Control | Keys.X):
                    {
                        Cut();
                        return true;
                    }
                    case (Keys.Control | Keys.Y):
                    {
                        Redo();
                        return true;
                    }
                    case (Keys.Control | Keys.Z):
                    {
                        Undo();
                        return true;
                    }
                    case (Keys.Control | Keys.Shift | Keys.Up):
                    {
                        builder_.MoveSelectionPoint(NodesBuilder.SelectionDirection.Up);

                        needUpdate = true;

                        ReRender();
                        return true;
                    }
                    case (Keys.Control | Keys.Shift | Keys.Down):
                    {
                        builder_.MoveSelectionPoint(NodesBuilder.SelectionDirection.Down);

                        needUpdate = true;

                        ReRender();
                        return true;
                    }
                }
            
            return base.ProcessDialogKey (keyData);
        }
    }
}