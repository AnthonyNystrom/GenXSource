namespace Boxes
{
    using Rendering;
    using Nodes;
    using System;

    public class BoxBuilder
    {
        public static void MakeBox(Node node, Painter g)
        {
            BoxBuilder.MakeBox(node, g, 0);
        }

        private static void MakeBox(Node node, Painter g, int width)
        {
            if (node.parent_ != null)
            {
                node.isVisible = node.parent_.isVisible;
            }
            if (node.type_ == null)
            {
                return;
            }
            if ((node.parent_ != null) && !node.parent_.isVisible)
            {
                node.box = new Box_none();
                node.isVisible = false;
            }
            else
            {
                switch (node.type_.type)
                {
                    case ElementType.Math:
                    {
                        node.box = new Box_mrow();
                        break;
                    }
                    case ElementType.Mi:
                    {
                        node.box = new Box_Text();
                        node.tokenType = Tokens.ID;
                        if (node.literalText != null)
                        {
                            string trimmed = node.literalText.Trim();
                            if (trimmed.Length <= 1)
                            {
                                if (trimmed.Length == 1)
                                {
                                    node.tokenType = Tokens.GLYPH;
                                }
                            }
                            else
                            {
                                node.tokenType = Tokens.TEXT;
                            }
                        }
                        break;
                    }
                    case ElementType.Mn:
                    {
                        node.box = new Box_Text();
                        node.tokenType = Tokens.NUMBER;
                        break;
                    }
                    case ElementType.Mo:
                    {
                        node.box = new Box_Mo();
                        break;
                    }
                    case ElementType.Mtext:
                    {
                        node.box = new Box_Text();
                        node.tokenType = Tokens.TEXT;
                        break;
                    }
                    case ElementType.Mspace:
                    {
                        node.box = new Box_Mspace();
                        break;
                    }
                    case ElementType.Ms:
                    {
                        node.box = new Box_Ms();
                        node.tokenType = Tokens.TEXT;
                        break;
                    }
                    case ElementType.Mglyph:
                    {
                        node.box = new Box_Mglyph();
                        node.isGlyph = true;
                        break;
                    }
                    case ElementType.Mfrac:
                    {
                        node.box = new Box_Mfrac();
                        break;
                    }
                    case ElementType.Msqrt:
                    {
                        node.box = new Box_Msqrt();
                        break;
                    }
                    case ElementType.Mroot:
                    {
                        node.box = new Box_mroot();
                        break;
                    }
                    case ElementType.Mrow:
                    {
                        node.box = new Box_mrow();
                        break;
                    }
                    case ElementType.Mfenced:
                    {
                        node.box = new Box_mfenced();
                        break;
                    }
                    case ElementType.Mstyle:
                    {
                        node.box = new Box_mstyle();
                        break;
                    }
                    case ElementType.Mphantom:
                    {
                        node.box = new Box_mphantom();
                        break;
                    }
                    case ElementType.Merror:
                    {
                        node.box = new Box_Text();
                        break;
                    }
                    case ElementType.Msup:
                    case ElementType.Msub:
                    case ElementType.Msubsup:
                    {
                        node.box = new Box_msub();
                        break;
                    }
                    case ElementType.Mover:
                    {
                        node.box = new Box_mover();
                        break;
                    }
                    case ElementType.Munder:
                    {
                        node.box = new Box_munder();
                        break;
                    }
                    case ElementType.Munderover:
                    {
                        node.box = new Box_munderover();
                        break;
                    }
                    case ElementType.Mmultiscripts:
                    {
                        node.box = new Box_multiscripts();
                        break;
                    }
                    case ElementType.Mprescripts:
                    {
                        node.box = new Box_none();
                        node.isVisible = false;
                        break;
                    }
                    case ElementType.Mnone:
                    {
                        node.box = new Box_none();
                        node.isVisible = false;
                        break;
                    }
                    case ElementType.Mtable:
                    {
                        node.box = new Box_mtable();
                        break;
                    }
                    case ElementType.Mtr:
                    {
                        node.box = new Box_mtr();
                        node.skip = true;
                        break;
                    }
                    case ElementType.Mtd:
                    {
                        node.box = new Box_mtd();
                        node.skip = true;
                        break;
                    }
                    case ElementType.Mlabeledtr:
                    {
                        node.box = new Box_mlabeledtr();
                        node.skip = true;
                        break;
                    }
                    case ElementType.Maction:
                    {
                        node.box = new Box_maction();
                        break;
                    }
                    
                    case ElementType.Entity:
                    {
                        node.box = new Box_entity();
                        node.isGlyph = true;
                        break;
                    }

                    default:
                        node.box = new Box_mrow();
                        break;
                }
            }
        
            node.box.Painter = g;
        }

    }
}

