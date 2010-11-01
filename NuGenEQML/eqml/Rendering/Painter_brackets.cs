namespace Rendering
{
    using Attrs;
    using Boxes;
    using Nodes;
    using Operators;
    
    using Fonts;
    using Facade;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.IO;

    public partial class Painter
    {
        private bool IsParen(string sUnicode)
        {
            if (((((sUnicode != "00028") && (sUnicode != "00029")) && ((sUnicode != "0007B") && (sUnicode != "0007D"))) && (((sUnicode != "0005B") && (sUnicode != "0005D")) && ((sUnicode != "0007C") && (sUnicode != "02016")))) && ((((sUnicode != "0230A") && (sUnicode != "0230B")) && ((sUnicode != "02308") && (sUnicode != "02309"))) && ((sUnicode != "02329") && (sUnicode != "0232A"))))
            {
                return false;
            }
            return true;
        }

        public bool DrawFence(Node node, string sText, Color color)
        {
            return this.DrawBracketed(true, node, sText, color);
        }

        public bool DrawBracketed(Node node, Color color)
        {
            return this.DrawBracketed(false, node, "", color);
        }

        private bool DrawBracketed(bool isFence, Node node, string sText, Color color)
        {
            int fHeight = 0;
            string fString = "";
            if (isFence)
            {
                if (sText.Length == 1)
                {
                    fString = ((ushort)sText[0]).ToString("X5");
                }
                if (!this.IsParen(fString))
                {
                    node.type_.type = ElementType.Mo;
                    try
                    {
                        BoxRect boxRect = this.MeasureTextRect(node, sText, node.scriptLevel_, node.style_);
                        int x = node.box.X + this.RoundFontSize(this.operatorFontSize(node, node.style_));
                        int y = (node.box.Y + node.box.Baseline) - boxRect.baseline;
                        this.DrawQuote(node, x, y, sText, node.scriptLevel_, node.style_, color);
                    }
                    catch
                    {
                    }
                }
            }
            else if (((node.firstChild != null) && (node.firstChild.type_.type == ElementType.Entity)) && (node.firstChild.glyph != null))
            {
                fString = node.firstChild.glyph.Code;
            }
            if (!this.IsParen(fString))
            {
                return false;
            }
            if (isFence)
            {
                Node n= null;
                n= this.CreateChildGlyph(fString, node);
                if ((n!= null) && (n.box != null))
                {
                    int normBase = this.MeasureBaseline(node, node.style_, "X");
                    if (this.IsAboveBaseline(node.box.Height, node.box.Baseline, normBase))
                    {
                        n.box.X = node.box.X;
                        n.box.Y = (node.box.Y + node.box.Baseline) - n.box.Baseline;
                        this.DrawString(n, node.style_, color);
                        return true;
                    }
                }
            }
            if (!isFence)
            {
                int w = node.firstChild.box.Width;
                int h = node.firstChild.box.Height;
                int b = node.firstChild.box.Baseline;
                int dx = node.firstChild.box.Dx;
                int dy = node.firstChild.box.Dy;
                try
                {
                    this.MeasureBox(node.firstChild, node.style_);
                    fHeight = node.firstChild.box.Height;
                }
                catch
                {
                }
                node.firstChild.box.Width = w;
                node.firstChild.box.Height = h;
                node.firstChild.box.Baseline = b;
                node.firstChild.box.Dx = dx;
                node.firstChild.box.Dy = dy;
                if (node.box.Height == fHeight)
                {
                    int oldX = node.firstChild.box.X;
                    try
                    {
                        node.firstChild.box.X = node.firstChild.box.X + this.Fence2Width(false, node, node.firstChild.glyph.Code);
                        this.DrawString(node.firstChild, node.style_, color);
                    }
                    catch
                    {
                    }
                    node.firstChild.box.X = oldX;
                    return true;
                }
            }
            
            int x1 = (this.oX + node.box.X) + this.Fence2Width(true, node, fString);
            int y1 = (this.oY + node.box.Y) + 1;
            int y2 = node.box.Height - 1;
            int charVal = 230;
            int char_extender = 0xe7;
            int tPart = 0xe8;
            int bPart = 0xed;
            bool isRight = false;
            if (fString != null)
            {
                switch (fString)
                {
                    case "00028":
                    {
                        charVal = 230;
                        char_extender = 0xe7;
                        tPart = 0xe8;
                        break;
                    }
                    case "0007B":
                    {
                        charVal = 0xec;
                        char_extender = 0xef;
                        tPart = 0xee;
                        bPart = 0xed;
                        break;
                    }
                    case "0005B":
                    {
                        charVal = 0xe9;
                        char_extender = 0xea;
                        tPart = 0xeb;
                        break;
                    }
                    case "0230A":
                    {
                        charVal = 0xea;
                        char_extender = 0xea;
                        tPart = 0xeb;
                        break;
                    }
                    case "02308":
                    {
                        charVal = 0xe9;
                        char_extender = 0xea;
                        tPart = 0xea;
                        break;
                    }
                    case "0007C":
                    {
                        charVal = 0xef;
                        char_extender = 0xef;
                        tPart = 0xef;
                        break;
                    }
                    case "02016":
                    {
                        charVal = 0x4c;
                        char_extender = 0x4c;
                        tPart = 0x4c;
                        break;
                    }
                    case "00029":
                    {
                        isRight = true;
                        charVal = 0xf6;
                        char_extender = 0xf7;
                        tPart = 0xf8;
                        break;
                    }
                    case "0007D":
                    {
                        isRight = true;
                        charVal = 0xfc;
                        char_extender = 0xef;
                        tPart = 0xfe;
                        bPart = 0xfd;
                        break;
                    }
                    case "0230B":
                    {
                        isRight = true;
                        charVal = 250;
                        char_extender = 250;
                        tPart = 0xfb;
                        break;
                    }
                    case "02309":
                    {
                        isRight = true;
                        charVal = 0xf9;
                        char_extender = 250;
                        tPart = 250;
                        break;
                    }
                    case "0005D":
                    {
                        isRight = true;
                        charVal = 0xf9;
                        char_extender = 250;
                        tPart = 0xfb;
                        break;
                    }
                    case "0232A":
                    {
                        isRight = true;
                        break;
                    }
                }
            }
            string s = "" + Convert.ToChar(charVal);
            SizeF sizeF = this.graphics_.MeasureString(s, this.GetBarFont(node));
            int ww = (int) Math.Round((double) sizeF.Width);
            float cor = 0f;
            float hScale = 0f;
            int hAmount = 0;
            try
            {
                if (this.GetBarFont(node).Name != "ESSTIXEight")
                {
                    FontFamilyInfo familyInfo = this.entityManager.GetFontFamilyInfo(this.GetBarFont(node).Name);
                    if ((familyInfo != null) && familyInfo.NeedCorrectY)
                    {
                        cor = ((float) familyInfo.CorrectY) / 100f;
                        hScale = sizeF.Height * cor;
                        hAmount = (int) Math.Round((double) hScale);
                    }
                }
                else
                {
                    cor = -0.02f;
                    hScale = sizeF.Height * cor;
                    hAmount = (int) Math.Round((double) hScale);
                }
            }
            catch
            {
            }
            y1 += hAmount;
            int char_top = (int) Math.Round((double) (0.05f * sizeF.Height));
            int char_bottom = (int) Math.Round((double) (0.78f * sizeF.Height));
            double margin = this.operatorFontSize(node, node.style_);
            if (fString == "00029")
            {
                x1 += (int) Math.Round((double) (sizeF.Height * 0.06f));
            }
            else if (((fString == "0005B") || (fString == "0230A")) || (fString == "02308"))
            {
                x1 += (int) Math.Round((double) (sizeF.Height * 0.03f));
            }
            else if (fString == "0007C")
            {
                x1 -= (int) Math.Round((double) (sizeF.Width * 0.165f));
            }
            else if (fString == "02016")
            {
                x1 += this.RoundFontSize(margin);
                y1 += this.RoundFontSize(0.5 * margin);
            }
            if (isRight)
            {
                if (fString == "0232A")
                {
                    try
                    {
                        GraphicsPath graphicsPath = new GraphicsPath();
                        graphicsPath.AddLine(x1 + (int) Math.Round(0.2 * node.box.Height), this.oY + node.box.Y + node.box.Height / 2, x1 + (2 * this.RoundFontSize(0.5 * margin)), this.oY + node.box.Y);
                        graphicsPath.AddLine(x1 + (2 * this.RoundFontSize(0.5 * margin)), this.oY + node.box.Y, x1 + this.RoundFontSize(0.5 * margin), this.oY + node.box.Y);
                        graphicsPath.AddLine(x1 + this.RoundFontSize(0.5 * margin), this.oY + node.box.Y, (x1 + (int) Math.Round(0.2 * node.box.Height)) - this.RoundFontSize(1.5 * margin), this.oY + node.box.Y + node.box.Height / 2);
                        graphicsPath.AddLine((int) ((x1 + (int) Math.Round(0.2 * node.box.Height)) - this.RoundFontSize(1.5 * margin)), (int) (this.oY + node.box.Y + node.box.Height / 2), (int) (x1 + this.RoundFontSize(0.5 * margin)), (int) (this.oY + node.box.Y + node.box.Height));
                        graphicsPath.AddLine((int) (x1 + this.RoundFontSize(0.5 * margin)), (int) (this.oY + node.box.Y + node.box.Height), (int) (x1 + (2 * this.RoundFontSize(0.5 * margin))), (int) (this.oY + node.box.Y + node.box.Height));
                        graphicsPath.AddLine((int) (x1 + (2 * this.RoundFontSize(0.5 * margin))), (int) (this.oY + node.box.Y + node.box.Height), (int) (x1 + (int) Math.Round(0.2 * node.box.Height)), (int) (this.oY + node.box.Y + node.box.Height / 2));
                        this.graphics_.FillPath(new SolidBrush(color), graphicsPath);
                        return true;
                    }
                    catch
                    {
                        return true;
                    }
                }
                if (fString == "0007D")
                {
                    int yy = (y1 - (int) Math.Round((double) (0.05f * sizeF.Height))) + (int) Math.Round((double) (0.78f * sizeF.Height));
                    int xx = ((y1 + (y2 / 2)) - (int) Math.Round((double) (0.05f * sizeF.Height))) - (((int) Math.Round((double) (0.78f * sizeF.Height)) - (int) Math.Round((double) (0.05f * sizeF.Height))) / 2) + (int) Math.Round((double) (0.05f * sizeF.Height));
                    this.graphics_.SetClip(new RectangleF((float) (x1 - 5), (float) (y1 - 5), (float) (ww + 20), (float) ((((((y1 + (y2 / 2)) - (int) Math.Round((double) (0.05f * sizeF.Height))) - (((int) Math.Round((double) (0.78f * sizeF.Height)) - (int) Math.Round((double) (0.05f * sizeF.Height))) / 2) + (int) Math.Round((double) (0.05f * sizeF.Height))) - y1) + 5) + this.Floor(margin * 0.5, 2))));
                    this.graphics_.DrawString(s, this.GetBarFont(node), new SolidBrush(color), (float) x1, (float) (y1 - (int) Math.Round((double) (0.05f * sizeF.Height))), this.typographicsFormat);
                    this.graphics_.ResetClip();
                    s = "" + Convert.ToChar(tPart);
                    this.graphics_.SetClip(new RectangleF((float) (x1 - 5), (float) (((y1 + (y2 / 2)) - (int) Math.Round((double) (0.05f * sizeF.Height))) - (((int) Math.Round((double) (0.78f * sizeF.Height)) - (int) Math.Round((double) (0.05f * sizeF.Height))) / 2) + (int) Math.Round((double) (0.78f * sizeF.Height))), (float) (ww + 20), (float) (((y1 + y2) - (((y1 + (y2 / 2)) - (int) Math.Round((double) (0.05f * sizeF.Height))) - (((int) Math.Round((double) (0.78f * sizeF.Height)) - (int) Math.Round((double) (0.05f * sizeF.Height))) / 2) + (int) Math.Round((double) (0.78f * sizeF.Height)))) + 20)));
                    this.graphics_.DrawString(s, this.GetBarFont(node), new SolidBrush(color), (float) x1, (float) ((y1 + y2) - (int) Math.Round((double) (0.78f * sizeF.Height))), this.typographicsFormat);
                    this.graphics_.ResetClip();
                    s = "" + Convert.ToChar(bPart);
                    this.graphics_.DrawString(s, this.GetBarFont(node), new SolidBrush(color), (float) x1, (float) (((y1 + (y2 / 2)) - (int) Math.Round((double) (0.05f * sizeF.Height))) - (((int) Math.Round((double) (0.78f * sizeF.Height)) - (int) Math.Round((double) (0.05f * sizeF.Height))) / 2)), this.typographicsFormat);
                    if (xx > yy)
                    {
                        this.DrawStringClipped(margin, x1, yy, xx + 1, char_extender, ww, char_top, char_bottom, this.GetBarFont(node), color);
                    }
                    yy = ((y1 + (y2 / 2)) - (int) Math.Round((double) (0.05f * sizeF.Height))) - (((int) Math.Round((double) (0.78f * sizeF.Height)) - (int) Math.Round((double) (0.05f * sizeF.Height))) / 2) + (int) Math.Round((double) (0.78f * sizeF.Height));
                    xx = ((y1 + y2) - (int) Math.Round((double) (0.78f * sizeF.Height))) + (int) Math.Round((double) (0.05f * sizeF.Height));
                    if (xx > yy)
                    {
                        this.DrawStringClipped(margin, x1, yy, xx, char_extender, ww, char_top, char_bottom, this.GetBarFont(node), color);
                    }
                }
                else
                {
                    this.graphics_.SetClip(new RectangleF((float) (x1 - this.RoundFontSize(margin)), (float) (y1 - 5), (float) (ww + (2 * this.RoundFontSize(margin))), (float) ((y2 / 2) + 5)));
                    this.graphics_.DrawString(s, this.GetBarFont(node), new SolidBrush(color), (float) x1, (float) (y1 - (int) Math.Round((double) (0.05f * sizeF.Height))), this.typographicsFormat);
                    this.graphics_.ResetClip();
                    s = "" + Convert.ToChar(tPart);
                    this.graphics_.SetClip(new RectangleF((float) (x1 - this.RoundFontSize(margin)), (float) (y1 + (y2 / 2)), (float) (ww + (2 * this.RoundFontSize(margin))), (float) ((y2 / 2) + 20)));
                    this.graphics_.DrawString(s, this.GetBarFont(node), new SolidBrush(color), (float) x1, (float) ((y1 + y2) - (int) Math.Round((double) (0.78f * sizeF.Height))), this.typographicsFormat);
                    this.graphics_.ResetClip();
                    this.DrawStringClipped(margin, x1, ((y1 - (int) Math.Round((double) (0.05f * sizeF.Height))) + (int) Math.Round((double) (0.78f * sizeF.Height))) - 2, (((y1 + y2) - (int) Math.Round((double) (0.78f * sizeF.Height))) + (int) Math.Round((double) (0.05f * sizeF.Height))) + 1, char_extender, ww, char_top, char_bottom, this.GetBarFont(node), color);
                }
            }
            else
            {
                if (fString == "02329")
                {
                    try
                    {
                        int boxH = node.box.Height;
                        int y = this.oY + node.box.Y;
                        int b = (int) Math.Round(0.2 * boxH);
                        
                        int xx = this.RoundFontSize(0.5 * margin);
                        int xxx = this.RoundFontSize(1.5 * margin);
                        int c = boxH / 2;
                        GraphicsPath graphicsPath = new GraphicsPath();
                        graphicsPath.AddLine(x1, y + c, (x1 + b) - (2 * xx), y);
                        graphicsPath.AddLine((x1 + b) - (2 * xx), y, (x1 + b) - xx, y);
                        graphicsPath.AddLine((x1 + b) - xx, y, x1 + xxx, y + c);
                        graphicsPath.AddLine((int) (x1 + xxx), (int) (y + c), (int) ((x1 + b) - xx), (int) (y + boxH));
                        graphicsPath.AddLine((int) (x1 + b), (int) (y + boxH), (int) ((x1 + b) - (2 * xx)), (int) (y + boxH));
                        graphicsPath.AddLine((x1 + b) - (2 * xx), y + boxH, x1, y + c);
                        this.graphics_.FillPath(new SolidBrush(color), graphicsPath);
                        return true;
                    }
                    catch
                    {
                        return true;
                    }
                }
                if (fString == "0007B") // LEFT CURLY BRACKET
                {
                    int yMarg = ((y1 + (y2 / 2)) - (int) Math.Round((double) (0.05f * sizeF.Height))) - (((int) Math.Round((double) (0.78f * sizeF.Height)) - (int) Math.Round((double) (0.05f * sizeF.Height))) / 2);
                    int yy = (y1 - (int) Math.Round((double) (0.05f * sizeF.Height))) + (int) Math.Round((double) (0.78f * sizeF.Height));
                    int hh = yMarg + (int) Math.Round((double) (0.05f * sizeF.Height));
                    this.graphics_.SetClip(new RectangleF((float) (x1 - 5), (float) (y1 - 5), (float) (ww + 10), (float) ((((yMarg - y1) + (int) Math.Round((double) (0.05f * sizeF.Height))) + 5) + this.Floor(margin * 0.5, 2))));
                    this.graphics_.DrawString(s, this.GetBarFont(node), new SolidBrush(color), (float) x1, (float) (y1 - (int) Math.Round((double) (0.05f * sizeF.Height))), this.typographicsFormat);
                    this.graphics_.ResetClip();
                    s = "" + Convert.ToChar(tPart);
                    this.graphics_.SetClip(new RectangleF((float) (x1 - 5), (float) (yMarg + (int) Math.Round((double) (0.78f * sizeF.Height))), (float) (ww + 10), (float) (((y1 + y2) - (yMarg + (int) Math.Round((double) (0.78f * sizeF.Height)))) + 20)));
                    this.graphics_.DrawString(s, this.GetBarFont(node), new SolidBrush(color), (float) x1, (float) ((y1 + y2) - (int) Math.Round((double) (0.78f * sizeF.Height))), this.typographicsFormat);
                    this.graphics_.ResetClip();
                    s = "" + Convert.ToChar(bPart);
                    this.graphics_.DrawString(s, this.GetBarFont(node), new SolidBrush(color), (float) x1, (float) yMarg, this.typographicsFormat);
                    if (hh > yy)
                    {
                        this.DrawStringClipped(margin, x1, yy, hh + 1, char_extender, ww, char_top, char_bottom, this.GetBarFont(node), color);
                    }
                    yy = yMarg + (int) Math.Round((double) (0.78f * sizeF.Height));
                    hh = ((y1 + y2) - (int) Math.Round((double) (0.78f * sizeF.Height))) + (int) Math.Round((double) (0.05f * sizeF.Height));
                    if (hh > yy)
                    {
                        this.DrawStringClipped(margin, x1, yy, hh, char_extender, ww, char_top, char_bottom, this.GetBarFont(node), color);
                    }
                }
                else
                {
                    this.graphics_.SetClip(new RectangleF((float) x1, (float) (y1 - 5), (float) ww, (float) ((y2 / 2) + 5)));
                    this.graphics_.DrawString(s, this.GetBarFont(node), new SolidBrush(color), (float) x1, (float) (y1 - (int) Math.Round((double) (0.05f * sizeF.Height))), this.typographicsFormat);
                    this.graphics_.ResetClip();
                    s = "" + Convert.ToChar(tPart);
                    this.graphics_.SetClip(new RectangleF((float) x1, (float) (y1 + (y2 / 2)), (float) ww, (float) ((y2 / 2) + 20)));
                    this.graphics_.DrawString(s, this.GetBarFont(node), new SolidBrush(color), (float) x1, (float) ((y1 + y2) - (int) Math.Round((double) (0.78f * sizeF.Height))), this.typographicsFormat);
                    this.graphics_.ResetClip();
                    this.DrawStringClipped(margin, x1, ((y1 - (int) Math.Round((double) (0.05f * sizeF.Height))) + (int) Math.Round((double) (0.78f * sizeF.Height))) - 2, (((y1 + y2) - (int) Math.Round((double) (0.78f * sizeF.Height))) + (int) Math.Round((double) (0.05f * sizeF.Height))) + 1, char_extender, ww, char_top, char_bottom, this.GetBarFont(node), color);
                }
            }
 
            return true;
        }
    }
}