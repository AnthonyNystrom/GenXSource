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
        public Painter()
        {
            this.fonts_ = null;
            this.operators = null;
            this.oX = 0;
            this.oY = 0;
            this.textSize = 12f;
            this.textSize = 10f;
            this.oX = 0;
            this.oY = 0;
        }

        public void SetOrigin(int oX, int oY)
        {
            this.oX = oX;
            this.oY = oY;
        }

        public void SetupPainting(Graphics g, bool bAntiAlias)
        {
            this.graphics_ = g;
            try
            {
                this.typographicsFormat = new StringFormat(StringFormat.GenericTypographic);
            }
            catch
            {
            }
            try
            {
                if (bAntiAlias)
                {
                    this.graphics_.TextRenderingHint = TextRenderingHint.AntiAlias;
                }
                else
                {
                    this.graphics_.TextRenderingHint = TextRenderingHint.SystemDefault;
                }
            }
            catch
            {
            }
            try
            {
                this.graphics_.InterpolationMode = InterpolationMode.HighQualityBilinear;
            }
            catch
            {
            }
            try
            {
                this.graphics_.SmoothingMode = SmoothingMode.HighQuality;
            }
            catch
            {
            }
        }

        private void createResources()
        {
            try
            {
                this.mono0 = this.MakeFont(null, 1, Category.MONOSPACE, 0, null);
                this.mono1 = this.MakeFont(null, 1, Category.MONOSPACE, 1, null);
                this.mono2 = this.MakeFont(null, 1, Category.MONOSPACE, 2, null);
                
                this.symbol0 = this.MakeFont(null, 1, Category.SYMBOL, 0, null);
                this.symbol1 = this.MakeFont(null, 1, Category.SYMBOL, 1, null);
                this.symbol2 = this.MakeFont(null, 1, Category.SYMBOL, 2, null);

                this.bold0 = this.MakeFont(null, 1, Category.BOLD, 0, null);
                this.bold1 = this.MakeFont(null, 1, Category.BOLD, 1, null);
                this.bold2 = this.MakeFont(null, 1, Category.BOLD, 2, null);
                
                this.operator0 = this.MakeFont(null, 1, Category.OPERATOR, 0, null);
                this.operator1 = this.MakeFont(null, 1, Category.OPERATOR, 1, null);
                this.operator2 = this.MakeFont(null, 1, Category.OPERATOR, 2, null);
                
                
                
                this.unknown0 = this.MakeFont(null, 1, Category.UNKNOWN, 0, null);
                this.unknown1 = this.MakeFont(null, 1, Category.UNKNOWN, 1, null);
                this.unknown2 = this.MakeFont(null, 1, Category.UNKNOWN, 2, null);

                this.number0 = this.MakeFont(null, 1, Category.NUMBER, 0, null);
                this.number1 = this.MakeFont(null, 1, Category.NUMBER, 1, null);
                this.number2 = this.MakeFont(null, 1, Category.NUMBER, 2, null);
                
                this.outlinePen = new Pen(Color.LightGray);
                this.blackPen = new Pen(Color.Black, 2f);
                this.blackBrush = Brushes.Black;
            }
            catch
            {
            }
        }

        public void Rectangle(Node node)
        {
            this.Rectangle(node, 0, Color.Black);
        }

        public void Rectangle(Node node, Color color)
        {
            this.Rectangle(node, 0, color);
        }

        public void Rectangle(Node node, int n, Color color)
        {
            this.graphics_.DrawRectangle(new Pen(color), (this.oX + node.box.X) - n, (this.oY + node.box.Y) - n, node.box.Width + (2 * n), node.box.Height + (2 * n));
        }

        public void Rectangle(int x, int y, int w, int h, Color color)
        {
            this.graphics_.DrawRectangle(new Pen(color, 1f), this.oX + x, this.oY + y, w, h);
        }

        public void DrawNodeRect(Node node, Color color)
        {
            this.graphics_.DrawRectangle(new Pen(color), this.oX + node.box.X, this.oY + node.box.Y, node.box.Width, node.box.Height);
        }

        public void FillBackground(Node node)
        {
            if (node.HasChildren())
            {
                NodesList nodesList = node.GetChildrenNodes();
                Node n = nodesList.Next();
                int x = 0;
                int y = 0;
                int w = 0;
                int h = 0;
                while (n != null)
                {
                    if (n.NotOnWhite())
                    {
                        x = n.box.X;
                        y = n.box.Y;
                        w = n.box.Width;
                        h = n.box.Height;
                        int count = 0;
                        while (((n.nextSibling != null) && n.nextSibling.NotOnWhite()) && (n.nextSibling.Background == n.Background))
                        {
                            n = n.nextSibling;
                            if (n.box.Y < y)
                            {
                                h += y - n.box.Y;
                                y = n.box.Y;
                            }
                            if ((n.box.X + n.box.Width) > (x + w))
                            {
                                w = (n.box.X + n.box.Width) - x;
                            }
                            if ((n.box.Y + n.box.Height) > (y + h))
                            {
                                h = (n.box.Y + n.box.Height) - y;
                            }
                            count++;
                        }
                        if (count > 0)
                        {
                            this.FillRectangle(x, y, w, h, n.Background);
                        }
                    }
                    if (n != null)
                    {
                        n = nodesList.Next();
                    }
                }
            }
        }

        public void FillRectangle(Node node)
        {
            Color background = Color.White;
            if (node.style_ != null)
            {
                background = node.style_.background;
            }
            
            
            if (((background != Color.White) || (((node.parent_ != null) && (node.parent_.style_ != null)) && (node.parent_.style_.background != Color.White))) || false)
            {
                this.graphics_.FillRectangle(new SolidBrush(background), this.oX + node.box.X, this.oY + node.box.Y, node.box.Width + 1, node.box.Height);
            }
        }

        public void FillRectangle(Node node, Color color)
        {
            this.graphics_.FillRectangle(new SolidBrush(color), this.oX + node.box.X, this.oY + node.box.Y, node.box.Width, node.box.Height);
        }

        public void FillRectangle(int x, int y, int w, int h, Color color)
        {
            this.graphics_.FillRectangle(new SolidBrush(color), this.oX + x, this.oY + y, w, h);
        }

        public void setFonts(Fonts.FontCollection FontCollection)
        {
            this.fonts_ = FontCollection;
        }

        public void OutlineRect(Node node)
        {
            this.graphics_.DrawRectangle(this.outlinePen, this.oX + node.box.X, this.oY + node.box.Y, node.box.Width, node.box.Height);
        }

        public int LineThickness(Node node)
        {
            int r = 1;
            try
            {
                r = Convert.ToInt32(Math.Round((double) (this.FontSize(node, node.style_) * 0.06)));
                return Math.Max(r, 1);
            }
            catch
            {
				return r;
            }
        }

        public int DoubleLineThickness(Node node)
        {
            try
            {
                return Math.Max(Convert.ToInt32(Math.Round((double) (this.FontSize(node, node.style_) * 0.12))), 1);
            }
            catch
            {
				return 1;
            }
        }

        public float DpiX()
        {
            return this.graphics_.DpiX;
        }

        public float FontSize(System.Drawing.Font font)
        {
            try
            {
                return (0.85f * font.GetHeight(this.graphics_.DpiX));
            }
            catch
            {
				return 1f;
            }
        }

        public float FontSize(Node node, StyleAttributes styleAttributes)
        {
            float r = 1f;
            try
            {
                System.Drawing.Font f = this.GetSuitableFont(node, node.style_);
                if (f != null)
                {
                    r = this.FontSize(f);
                }
            }
            catch
            {
            }
            return r;
        }

        public float CalcSpaceHeight(Space space, float ems)
        {
            float r = 0f;

            switch (space)
            {
                case Space.VeryVeryThin:
                {
                    r = 0.0555556f;
                    break;
                }
                case Space.VeryThin:
                {
                    r = 0.111111f;
                    break;
                }
                case Space.Thin:
                {
                    r = 0.166667f;
                    break;
                }
                case Space.Medium:
                {
                    r = 0.222222f;
                    break;
                }
                case Space.Thick:
                {
                    r = 0.277778f;
                    break;
                }
                case Space.VeryThick:
                {
                    r = 0.333333f;
                    break;
                }
                case Space.VeryVeryThick:
                    r = 0.388889f;
                    break;
            }

            return r*ems;
        }

        public float CalcSpaceHeight(Space Space, Node node, StyleAttributes styleAttributes)
        {
            
            try
            {
                return this.CalcSpaceHeight(Space, this.FontSize(node, styleAttributes));
            }
            catch
            {
				return 0f;
            }
        }

        public float CalcSpaceHeight(Space Space, float fontSize, string sFontFamily)
        {
            try
            {
                return this.CalcSpaceHeight(Space, this.FontSize(new System.Drawing.Font(sFontFamily, fontSize)));
            }
            catch
            {
				return 0f;
            }
        }

        public int CenterHeight(Node node)
        {
            int r = 0;
            try
            {
                Node n = new Node();
                n.type_ = new global::Nodes.Type("entity", ElementType.Entity, 0, 0);
                n.glyph = this.entityManager.ByName("plus");
                n.namespaceURI = node.namespaceURI;
                n.literalText = n.literalText + n.glyph.CharValue;
                n.fontFamily = n.glyph.FontFamily;
                n.box = new Box_entity();
                n.style_ = node.style_;
                n.scriptLevel_ = node.scriptLevel_;
                n.level = node.level;
                n.displayStyle = node.displayStyle;
                
                this.MeasureBox(n, node.style_);
                r = n.box.Baseline - (n.box.Height / 2);
                n = null;
            }
            catch
            {
                try
                {
                    int b = this.MeasureBaseline(node, node.style_, "X");
                    int h = this.MeasureHeight(node, node.style_, "X");
                    return (int) Math.Round((h - (2 * (h - b))) * 0.44);
                }
                catch
                {
                    return r;
                }
            }
            return r;
        }

        public bool IsAboveBaseline(int h, int b, int normBase)
        {
            int v = (int) Math.Round((double) (normBase * 1.2));
            if ((b <= v) && ((h - b) <= v))
            {
                return true;
            }
            return false;
        }

        private int RoundFontSize(double dValue)
        {
            int r = 0;
            try
            {
                r = Convert.ToInt32(Math.Round(dValue));
                return Math.Max(r, 1);
            }
            catch
            {
				return r;
            }
        }

        private int Floor(double dValue)
        {
            try
            {
                return Convert.ToInt32(Math.Round(dValue));
            }
            catch
            {
				return 0;
            }
        }

        private int Floor(double dValue, int nMin)
        {
            try
            {
                return Math.Max(Convert.ToInt32(Math.Round(dValue)), nMin);
            }
            catch
            {
				return 0;
            }
        }

        public int Fence2Width(bool isBig, Node opNode, string unicode)
        {
            double fontSize = this.operatorFontSize(opNode, opNode.style_);
            if (isBig)
            {
                switch (unicode)
                {
                    case "00028":
                    case "0005B":
                    case "0230A":
                    case "02308":
                        return this.RoundFontSize(0.7*fontSize);

                    case "00029":
                    case "0005D":
                    case "0230B":
                        return this.RoundFontSize(0.3*fontSize);

                    case "02309":
                        return this.RoundFontSize(0.4*fontSize);

                    case "0007B":
                    case "02329":
                    case "0007C":
                    case "02016":
                        return this.RoundFontSize(fontSize);

                    case "0232A":
                    default:
                        return 0;
                }
            }

            if (unicode == null)
            {
                return 0;
            }

            if (unicode == "0007C")
            {
                return this.RoundFontSize(fontSize);
            }
            else if (unicode == "02016")
            {
                return this.RoundFontSize(1.5*fontSize);
            }

            return 0;
        }

        public int FenceWidth(bool bBIG, Node opNode, string sUnicode)
        {
            int val = 0;
            double fontSize = this.operatorFontSize(opNode, opNode.style_);

            if (bBIG)
            {
                switch (sUnicode)
                {
                    case "00028":
                    case "0005B":
                    case "0230A":
                    case "02308":
                        return this.RoundFontSize(0.3*fontSize);

                    case "00029":
                    case "0005D":
                    case "0230B":
                    case "02309":
                    case "0007D":
                        return this.RoundFontSize(0.7*fontSize);

                    case "0232A":
                    case "0007C":
                    case "02016":
                        return this.RoundFontSize(fontSize);

                    case "0007B":
                    case "02329":
                    default:
                        return val;
                }
            }
            else
            {
                if (sUnicode == null)
                {
                    return val;
                }

                if (sUnicode == "0007C")
                {
                    return this.RoundFontSize(fontSize);
                }
                else if (sUnicode == "02016")
                {
                    return this.RoundFontSize(1.5*fontSize);
                }

                return val;
            }
        }

        public int OpWidth(bool bMFence, Node opNode, string sText)
        {
            sText = sText.Trim();
            if (sText.Length == 1)
            {
                string unicode = ((ushort)sText[0]).ToString("X5");
                return this.FencedWidth(bMFence, opNode, unicode);
            }
            if (bMFence)
            {
                try
                {
                    double opSize = this.operatorFontSize(opNode, opNode.style_);
                    BoxRect rect = null;
                    ElementType realType = opNode.type_.type;
                    opNode.type_.type = ElementType.Mo;
                    try
                    {
                        rect = this.MeasureTextRect(opNode, sText, opNode.scriptLevel_, opNode.style_);
                    }
                    catch
                    {
                        opNode.type_.type = realType;
                        return 0;
                    }
                    opNode.type_.type = realType;
                    if (rect != null)
                    {
                        rect.width += this.RoundFontSize(opSize);
                        rect.width += this.RoundFontSize(opSize);
                        return rect.width;
                    }
                }
                catch
                {
                }
            }
            return 0;
        }

        public int FencedWidth(bool isFence, Node opNode, string unicode)
        {
            int w = 0;
            this.roundOpFontSize(opNode, opNode.style_);
            if (!isFence)
            {
                w = opNode.box.Width;
            }
            int b = this.MeasureBaseline(opNode, opNode.style_, "X");
            bool isStretch = true;
            try
            {
                if (opNode.attrs != null)
                {
                    Nodes.Attribute attribute = opNode.attrs.Get("stretchy");
                    if (attribute != null)
                    {
                        isStretch = Convert.ToBoolean(attribute.val);
                    }
                }
            }
            catch
            {
            }
            if (!isStretch || this.IsAboveBaseline(opNode.box.Height, opNode.box.Baseline, b))
            {
                if (isFence)
                {
                    Node n = this.CreateChildGlyph(unicode, opNode);
                    w = n.box.Width;
                }
                else
                {
                    w = opNode.box.Width;
                }
                w += this.Fence2Width(false, opNode, unicode);
                return (w + this.FenceWidth(false, opNode, unicode));
            }
            
            if ((unicode == "02329") || (unicode == "0232A"))
            {
                w = (int) Math.Round(0.2 * opNode.box.Height);
                w += this.Fence2Width(true, opNode, unicode);
                return (w + this.FenceWidth(true, opNode, unicode));
            }
            float scale = 1f;
            float ww = 0f;
            System.Drawing.Font font1 = this.GetBarFont(opNode);
            int ext = 230;
            if ((unicode == "00028") || (unicode == "00029"))
            {
                ext = 230;
                scale = 0.6f;
            }
            else if (unicode == "0007B")
            {
                ext = 0xec;
                scale = 0.55f;
            }
            else if (unicode == "0007D")
            {
                ext = 0xec;
                scale = 0.55f;
            }
            else if ((((unicode == "0005B") || (unicode == "0005D")) || ((unicode == "0230A") || (unicode == "0230B"))) || ((unicode == "02308") || (unicode == "02309")))
            {
                ext = 0xe9;
                scale = 0.5f;
            }
            else if (unicode == "0007C")
            {
                ext = 0xef;
                scale = 0.2f;
            }
            else if (unicode == "02016")
            {
                ext = 0xef;
                scale = 0.8f;
            }
            string s = "" + Convert.ToChar(ext);
            SizeF sizeF = this.graphics_.MeasureString(s, font1);
            ww = (int) Math.Round((double) sizeF.Width);
            w = (int) Math.Round((double) (ww * scale));
            w += this.Fence2Width(true, opNode, unicode);
            return (w + this.FenceWidth(true, opNode, unicode));
        }

        

        public bool IsStretchy(Node node)
        {
            if (((node.firstChild == null) || (node.firstChild.type_.type != ElementType.Entity)) || ((node.firstChild.glyph == null) || !this.IsParen(node.firstChild.glyph.Code)))
            {
                return false;
            }
            if ((node.attrs != null) && (node.attrs.GetValue("stretchy").Trim().ToUpper() == "FALSE"))
            {
                return false;
            }
            return true;
        }

        public void SetEntityManager(EntityManager MathMLEntityManager)
        {
            this.entityManager = MathMLEntityManager;
        }

        private Node CreateChildGlyph(string sUnicode, Node node)
        {
            Node entityNode = null;
            try
            {
                entityNode = new Node();
                Node operatorNode = new Node();
                operatorNode.type_ = new global::Nodes.Type("mo", ElementType.Mo, 0, 0);
                entityNode.type_ = new global::Nodes.Type("entity", ElementType.Entity, 0, 0);
                Glyph glyph = null;
                glyph = this.entityManager.ByUnicode(sUnicode);
                if (glyph != null)
                {
                    entityNode.literalText = "";
                    entityNode.literalText = entityNode.literalText + glyph.CharValue;
                    entityNode.fontFamily = glyph.FontFamily;
                    entityNode.glyph = glyph;
                    entityNode.xmlTagName = glyph.Name;
                    entityNode.box = new Box_entity();
                    operatorNode.box = new Box_Mo();
                    node.CopyProperties(operatorNode);
                    entityNode.scriptLevel_ = node.scriptLevel_;
                    
                    operatorNode.AdoptChild(entityNode);
                    this.MeasureBox(entityNode, node.style_);
                }
            }
            catch
            {
                entityNode = null;
            }
            return entityNode;
        }

        private void DrawStringClipped(double margin, int x1, int y1, int y2, int char_extender, int char_w, int char_top,
                         int char_bottom, System.Drawing.Font font, Color color)
        {
            string s = "" + Convert.ToChar(char_extender);
            int h = char_bottom - char_top;
            this.graphics_.SetClip(new RectangleF((float) (x1 - this.RoundFontSize(margin)), (float) y1, (float) (char_w + this.RoundFontSize(2 * margin)), (float) ((y2 - y1) + 2)));
            for (int i = y1; i <= (y2 + 2); i += h)
            {
                this.graphics_.DrawString(s, font, new SolidBrush(color), (float) x1, (float) ((i - char_top) - this.Floor(margin)), this.typographicsFormat);
            }
            this.graphics_.ResetClip();
        }

        public System.Drawing.Font GetBarFont(Node node)
        {
            bool isAngled = false;
            if ((((node.type_.type == ElementType.Mo) && (node.firstChild != null)) &&
                 ((node.firstChild.type_.type == ElementType.Entity) && (node.firstChild.glyph != null))) &&
                (((node.firstChild.glyph.Code == "02016") || (node.firstChild.glyph.Code == "02329")) ||
                 (node.firstChild.glyph.Code == "0232A")))
            {
                isAngled = true;
            }
            double sc = 1;
            System.Drawing.FontStyle style = System.Drawing.FontStyle.Regular;
            if (isAngled)
            {
                if (node.style_ != null)
                {
                    sc = node.style_.scale;
                    if (node.style_.isBold)
                    {
                        style |= System.Drawing.FontStyle.Bold;
                    }
                }
                switch (node.scriptLevel_)
                {
                    case 0:
                        return new System.Drawing.Font("ESSTIXSeven", (float) (this.textSize * sc), style);

                    case 1:
                        return new System.Drawing.Font("ESSTIXSeven", (float) ((this.textSize * 0.71) * sc), style);

                    case 2:
                        return new System.Drawing.Font("ESSTIXSeven", (float) ((this.textSize * 0.5) * sc), style);
                }
                return new System.Drawing.Font("ESSTIXSeven", this.textSize);
            }
           
            if (node.style_ != null)
            {
                sc = node.style_.scale;
                if (node.style_.isBold)
                {
                    style |= System.Drawing.FontStyle.Bold;
                }
            }
            switch (node.scriptLevel_)
            {
                case 0:
                    return new System.Drawing.Font("Symbol", (float) (this.textSize * sc), style);

                case 1:
                    return new System.Drawing.Font("Symbol", (float) ((this.textSize * 0.71) * sc), style);

                case 2:
                    return new System.Drawing.Font("Symbol", (float) ((this.textSize * 0.5) * sc), style);
            }
            return new System.Drawing.Font("Symbol", this.textSize);
        }

        public void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {
            this.DrawLine(x1, y1, x2, y2, 1, color);
        }

        public void DrawLine(int x1, int y1, int x2, int y2, int linethickness, Color color)
        {
            this.graphics_.DrawLine(new Pen(color, (float) linethickness), this.oX + x1, this.oY + y1, this.oX + x2, this.oY + y2);
        }

        public void DrawDashLine(int x1, int y1, int x2, int y2, Color color)
        {
            this.DrawDashLine(x1, y1, x2, y2, 1, color);
        }

        public void DrawDashLine(int x1, int y1, int x2, int y2, int linethickness, Color color)
        {
            Pen pen = new Pen(color, (float) linethickness);
            pen.DashStyle = DashStyle.Dash;
            pen.DashPattern = new float[] { 3f, 3f };
            this.graphics_.DrawLine(pen, this.oX + x1, this.oY + y1, this.oX + x2, this.oY + y2);
        }

        public void SetOperators(OperatorDictionary OperatorDictionary)
        {
            this.operators = OperatorDictionary;
        }

        public void Polyline(Point[] points, Color color)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X += this.oX;
                points[i].Y += this.oY;
            }
            this.graphics_.DrawPolygon(new Pen(color, 1f), points);
            this.graphics_.FillPolygon(new SolidBrush(color), points, FillMode.Winding);
        }

        private bool IsAlphaCaseless(string text)
        {
            bool r = true;
            int len = text.Length;
            for (int i = 0; i < len; i++)
            {
                if (((text[i] < 'a') || (text[i] > 'z')) && ((text[i] < 'A') || (text[i] > 'Z')))
                {
                    r = false;
                }
            }
            return r;
        }

        private bool IsAlpha(string text)
        {
            bool r = true;
            int len = text.Length;
            for (int i = 0; i < len; i++)
            {
                if ((text[i] < 'A') || (text[i] > 'Z'))
                {
                    r = false;
                }
            }
            return r;
        }

        public int roundOpFontSize(Node node, StyleAttributes styleAttributes)
        {
            int roundFontSize = 0;
            try
            {
                roundFontSize = this.RoundFontSize(this.operatorFontSize(node, styleAttributes));
            }
            catch
            {
            }
            return Math.Max(roundFontSize, 1);
        }

        public double operatorFontSize(Node node, StyleAttributes styleAttributes)
        {
            try
            {
                return (this.FontSize(this.GetSuitableFont(node, node.style_)) * 0.07741);
            }
            catch
            {
				return 0;
            }
        }

        public void MeasureBox(Node node, StyleAttributes styleAttributes)
        {
            this.MeasureBox(node, styleAttributes, node.literalText);
        }

        public void MeasureBox(Node node, StyleAttributes styleAttributes, string text)
        {
            SizeF sizeF;
            float height = 0f;
            float width = 0f;
            float baseline = 0f;
            float dy = 0f;
            int dY = 0;
            int xCorr = 0;
            if ((node.type_.type == ElementType.Entity) && (node.glyph != null))
            {
                if (node.glyph.Code == "0F578")
                {
                    text = "$" + text;
                }
                else if (node.glyph.Code == "0F576")
                {
                    text = text + "$";
                }
                else if (node.glyph.Code == "0F577")
                {
                    text = "$" + text;
                }
                else if (node.glyph.Code == "021C4")
                {
                    text = "$" + text;
                }
                else if (node.glyph.Code == "021C6")
                {
                    text = "$" + text;
                }
                else if (node.glyph.Code == "021CC")
                {
                    text = "$" + text;
                }
                else if (node.glyph.Code == "021CB")
                {
                    text = "$" + text;
                }
                else if (node.glyph.Code == "0FE37")
                {
                    text = "D" + text + "F";
                }
                else if (node.glyph.Code == "0FE38")
                {
                    text = "?" + text + "A";
                }
                else if (node.glyph.Code == "000A8")
                {
                    text = text + text;
                }
            }
            PointF origin = new PointF(0f, 0f);
            System.Drawing.Font f = this.GetSuitableFont(node, styleAttributes);
            if (((node.glyph != null) && node.glyph.IsVisible) && ((node.glyph.Category.Name != "Spaces") && (node.glyph.FontFamily.Length == 0)))
            {
                sizeF = this.graphics_.MeasureString("?", f, origin, this.typographicsFormat);
            }
            else
            {
                bool isX = false;
                bool isF = false;
                if ((((node.type_ != null)) && ((f != null) && f.Italic)) && (text.Length > 0))
                {
                    char ch = text[text.Length - 1];
                    if (char.IsLetter(ch))
                    {
                        if (char.IsUpper(ch))
                        {
                            if ((((ch == 'X') || (ch == 'Y')) || ((ch == 'H') || (ch == 'I'))) || ((((ch == 'J') || (ch == 'M')) || ((ch == 'N') || (ch == 'T'))) || (((ch == 'U') || (ch == 'V')) || (ch == 'W'))))
                            {
                                isX = true;
                            }
                        }
                        else if (ch == 'f')
                        {
                            isF = true;
                        }
                    }
                }
                if (isX)
                {
                    sizeF = this.graphics_.MeasureString(text, f, origin, this.typographicsFormat);
                    sizeF.Width += this.roundOpFontSize(node, node.style_);
                }
                else if (isF)
                {
                    sizeF = this.graphics_.MeasureString(text + "|", f, origin, this.typographicsFormat);
                }
                else
                {
                    sizeF = this.graphics_.MeasureString(text, f, origin, this.typographicsFormat);
                }
            }
            width = sizeF.Width;
            height = sizeF.Height;
            FontFamilyInfo familyInfo = this.entityManager.GetFontFamilyInfo(f.Name);
            if (((familyInfo != null) && familyInfo.NeedCorrectX) && (((familyInfo.FontFamily != "ESSTIXFive") || (node.glyph == null)) || (node.glyph.Code != "0210F")))
            {
                if (f.Name == "ESSTIXEight")
                {
                    xCorr = (int) Math.Round((double) (height * (((float) familyInfo.CorrectX) / 100f)));
                    xCorr = Math.Max(xCorr, 2);
                }
                else
                {
                    xCorr = (int) Math.Round((double) (height * (((float) familyInfo.CorrectX) / 100f)));
                }
            }
            if (((familyInfo != null) && familyInfo.NeedCorrectW) && (((familyInfo.FontFamily != "ESSTIXFive") || (node.glyph == null)) || (node.glyph.Code != "0210F")))
            {
                width += height * (((float) familyInfo.CorrectW) / 100f);
            }
            if ((familyInfo != null) && familyInfo.NeedCorrectY)
            {
                dy = height * (((float) familyInfo.CorrectY) / 100f);
            }
            float leading = 0f;
            if (styleAttributes == null)
            {
                leading = this.Leading(height, f, f.Style);
            }
            else
            {
                leading = this.Leading(height, f);
            }
            baseline = height - leading;
            if ((familyInfo != null) && familyInfo.NeedCorrectB)
            {
                baseline += height * (((float) familyInfo.CorrectB) / 100f);
            }
            if ((familyInfo != null) && familyInfo.NeedCorrectH)
            {
                height *= ((float) familyInfo.CorrectH) / 100f;
            }
            int h = 0;
            int b = 0;
            int w = 0;
            if ((node.type_.type == ElementType.Entity) && (node.glyph != null))
            {
                if (node.glyph.Code == "0222B")
                {
                    baseline *= 1.1f;
                }
                else if (node.glyph.Code == "0222C")
                {
                    width = 1.5f * width;
                    baseline *= 1.1f;
                }
                else if (node.glyph.Code == "0222D")
                {
                    width = 2f * width;
                    baseline *= 1.1f;
                }
                else if (((node.glyph.Code == "0222E") || (node.glyph.Code == "02232")) || (node.glyph.Code == "02233"))
                {
                    baseline *= 1.1f;
                }
                else if (node.glyph.Code == "0222F")
                {
                    baseline *= 1.1f;
                }
                else if (node.glyph.Code == "02230")
                {
                    baseline *= 1.1f;
                }
            }
            try
            {
                if ((((node.type_.type == ElementType.Entity) && (node.glyph != null)) && (this.IsParen(node.glyph.Code) && (familyInfo != null))) && (familyInfo.FontFamily == "Times New Roman"))
                {
                    dy -= height * 0.0626953f;
                }
            }
            catch
            {
            }
            baseline -= MeasureBaseline(this.graphics_, "Times New Roman", f);
            h = (int) Math.Round((double) height);
            b = (int) Math.Round((double) baseline);
            w = (int) Math.Round((double) width);
            dY = (int) Math.Round((double) dy);
            node.box.Height = h;
            node.box.Baseline = b;
            node.box.Width = w;
            node.box.Dy = dY;
            node.box.Dx = xCorr;
        }

        public int MeasureBaseline(Node node, StyleAttributes styleAttributes, string text)
        {
            int r = 0;
            if ((node != null) && (node.box != null))
            {
                int w = node.box.Width;
                int h = node.box.Height;
                int b = node.box.Baseline;
                int dy = node.box.Dy;
                int dx = node.box.Dx;
                try
                {
                    this.MeasureBox(node, styleAttributes, text);
                    r = node.box.Baseline;
                }
                catch
                {
                }
                node.box.Width = w;
                node.box.Height = h;
                node.box.Baseline = b;
                node.box.Dx = dx;
                node.box.Dy = dy;
            }
            return r;
        }

        public int MeasureHeight(Node node, StyleAttributes styleAttributes, string text)
        {
            int r = 0;
            if ((node != null) && (node.box != null))
            {
                int w = node.box.Width;
                int h = node.box.Height;
                int baseline = node.box.Baseline;
                int dy = node.box.Dy;
                int dx = node.box.Dx;
                try
                {
                    this.MeasureBox(node, styleAttributes, text);
                    r = node.box.Height;
                }
                catch
                {
                }
                node.box.Width = w;
                node.box.Height = h;
                node.box.Baseline = baseline;
                node.box.Dx = dx;
                node.box.Dy = dy;
            }
            return r;
        }

        public void SetFontSize(float textSize)
        {
            try
            {
                this.textSize = textSize;
                this.createResources();
            }
            catch
            {
            }
        }

        public int MeasureWidth(Node node, StyleAttributes styleAttributes, string text)
        {
            int w = 0;
            bool startSpace = false;
            bool endSpace = false;
            if ((text.Length > 0) && char.IsWhiteSpace(text[0]))
            {
                startSpace = true;
            }
            if ((text.Length > 0) && char.IsWhiteSpace(text[text.Length - 1]))
            {
                endSpace = true;
            }
            if (startSpace || endSpace)
            {
                w = this.MeasureWidth(node, styleAttributes, "I");
                if (startSpace && endSpace)
                {
                    text = "I" + text + "I";
                }
                else if (startSpace)
                {
                    text = "I" + text;
                }
                else if (endSpace)
                {
                    text = text + "I";
                }
            }
            int r = 0;
            if ((node != null) && (node.box != null))
            {
                int ww = node.box.Width;
                int h = node.box.Height;
                int b = node.box.Baseline;
                int dy = node.box.Dy;
                int dx = node.box.Dx;
                try
                {
                    this.MeasureBox(node, styleAttributes, text);
                    r = node.box.Width;
                }
                catch
                {
                }
                if (startSpace || endSpace)
                {
                    if (startSpace && endSpace)
                    {
                        r -= 2 * w;
                    }
                    else if (startSpace)
                    {
                        r -= w;
                    }
                    else if (endSpace)
                    {
                        r -= w;
                    }
                }
                node.box.Width = ww;
                node.box.Height = h;
                node.box.Baseline = b;
                node.box.Dx = dx;
                node.box.Dy = dy;
            }
            return r;
        }

        public Rectangle CalcRect(Node node, StyleAttributes styleAttributes)
        {
            Rectangle r = new Rectangle(0, 0, 0, 0);
            try
            {
                Size size = this.graphics_.MeasureString("X", this.GetSuitableFont(node, styleAttributes), new PointF(0f, 0f), this.typographicsFormat).ToSize();
                r.Width = size.Width;
                r.Height = size.Height;
            }
            catch
            {
            }
            return r;
        }

        public BoxRect MeasureTextRect(Node node, string text, int scriptLevel, StyleAttributes styleAttributes)
        {
            BoxRect rect = new BoxRect();
            int maxb = 0;
            int maxd = 0;
            try
            {
                text = text.Replace("&amp;", "&");
                text = text.Replace("&quot;", "\"");
                text = text.Replace("&apos;", "'");
                text = text.Replace("&gt;", ">");
                text = text.Replace("&lt;", "<");
                if (text != null)
                {
                    int len = text.Length;
                    for (int i = 0; i < len; i++)
                    {
                        if (char.IsLetterOrDigit(text[i]) || char.IsPunctuation(text[i]))
                        {
                            rect.width += this.MeasureWidth(node, styleAttributes, "" + text[i]);
                            int h = this.MeasureHeight(node, styleAttributes, "" + text[i]);
                            int b = this.MeasureBaseline(node, styleAttributes, "" + text[i]);
                            if (b > maxb)
                            {
                                maxb = b;
                            }
                            if ((h - b) > maxd)
                            {
                                maxd = h - b;
                            }
                        }
                        else
                        {
                            string hex = "";
                            hex = Convert.ToString(text[i], 0x10).ToUpper();
                            hex = hex.PadLeft(5, '0');
                            Glyph glyph = this.entityManager.ByUnicode(hex);
                            if (glyph != null)
                            {
                                node.glyph = glyph;
                                if (this.MakeGlyphFont(node, scriptLevel, styleAttributes) != null)
                                {
                                    rect.width += this.MeasureWidth(node, styleAttributes, "" + glyph.CharValue);
                                    int h = this.MeasureHeight(node, styleAttributes, "" + glyph.CharValue);
                                    int b = this.MeasureBaseline(node, styleAttributes, "" + glyph.CharValue);
                                    if (b > maxb)
                                    {
                                        maxb = b;
                                    }
                                    if ((h - b) > maxd)
                                    {
                                        maxd = h - b;
                                    }
                                }
                                node.glyph = null;
                            }
                        }
                    }
                }
            }
            catch
            {
                rect = new BoxRect(10, 10, 5);
            }
            rect.height = Math.Max(maxb + maxd, 10);
            rect.baseline = Math.Max(maxb, 8);
            return rect;
        }

        
        private float Leading(float h, System.Drawing.Font font)
        {
            return this.Leading(h, font, font.Style);
        }

        private float Leading(float h, System.Drawing.Font font, System.Drawing.FontStyle style)
        {
            try
            {
                return ((h * font.FontFamily.GetCellDescent(style)) / font.FontFamily.GetEmHeight(style));
            }
            catch
            {
                return 0f;
            }
        }

        public void DrawQuote(Node node, int x, int y, string text, int scriptLevel, StyleAttributes styleAttributes, Color color)
        {
            try
            {
                Brush brush;
                int width = 0;
                text = text.Replace("&amp;", "&");
                text = text.Replace("&quot;", "\"");
                text = text.Replace("&apos;", "'");
                text = text.Replace("&gt;", ">");
                text = text.Replace("&lt;", "<");
                if (styleAttributes != null)
                {
                    if (node.NotBlack() || (((node.type_.type == ElementType.Entity) && (node.parent_ != null)) && node.parent_.NotBlack()))
                    {
                        brush = new SolidBrush(styleAttributes.color);
                    }
                    else
                    {
                        brush = new SolidBrush(color);
                    }
                }
                else if (color != Color.Black)
                {
                    brush = new SolidBrush(color);
                }
                else
                {
                    brush = this.blackBrush;
                }
                if (text == null)
                {
                    return;
                }
                int len = text.Length;
                for (int i = 0; i < len; i++)
                {
                    
                    if (char.IsLetterOrDigit(text[i]) || char.IsPunctuation(text[i]))
                    {
                        string s = "" + text[i];
                        PointF point = new PointF((float) ((this.oX + x) + width), (float) (this.oY + y));
                        this.graphics_.DrawString(s, this.GetSuitableFont(node, styleAttributes), brush, point, this.typographicsFormat);
                        width += this.MeasureWidth(node, styleAttributes, s);
                    }
                    else
                    {
                        string s = Convert.ToString(Convert.ToInt32(text[i]), 0x10).ToUpper();
                        s = s.PadLeft(5, '0');
                        Glyph glyph = this.entityManager.ByUnicode(s);
                        if (glyph != null)
                        {
                            node.glyph = glyph;
                            System.Drawing.Font f = this.MakeGlyphFont(node, scriptLevel, styleAttributes);
                            if (f != null)
                            {
                                PointF point = new PointF((float) ((this.oX + x) + width), (float) (this.oY + y));
                                string gs = "" + glyph.CharValue;
                                if (color == Color.Black)
                                {
                                    this.graphics_.DrawString(gs, f, brush, point, this.typographicsFormat);
                                }
                                else
                                {
                                    this.graphics_.DrawString(gs, f, new SolidBrush(color), point, this.typographicsFormat);
                                }
                                width += this.MeasureWidth(node, styleAttributes, gs);
                            }
                            node.glyph = null;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public System.Drawing.Font GetSuitableFont(Node node, StyleAttributes styleAttributes)
        {
            if (node.glyph != null)
            {
                return this.MakeGlyphFont(node, node.scriptLevel_, styleAttributes);
            }
            if (node.type_.type == ElementType.Mn)
            {
                return this.MakeFont(node, 0, Category.NUMBER, node.scriptLevel_, styleAttributes);
            }
            if (((node.tokenType == Tokens.TEXT) || (node.type_.type == ElementType.Entity)) || (node.type_.type == ElementType.Mo))
            {
                return this.MakeFont(node, 0, Category.OPERATOR, node.scriptLevel_, styleAttributes);
            }
            if (node.type_.type == ElementType.Mglyph)
            {
                return this.MakeFont(node, 0, Category.GLYPH, node.scriptLevel_, styleAttributes);
            }
            return this.MakeFont(node, 0, Category.UNKNOWN, node.scriptLevel_, styleAttributes);
        }

        public System.Drawing.Font MakeGlyphFont(Node node, int scriptLevel, StyleAttributes styleAttributes)
        {
            System.Drawing.Font r = null;
            bool hasMax = false;
            bool hasMin = false;
            double scale = 1;
            System.Drawing.FontStyle style = System.Drawing.FontStyle.Regular;
            if (styleAttributes != null)
            {
                scale = styleAttributes.scale;
                if (styleAttributes.isBold)
                {
                    style |= System.Drawing.FontStyle.Bold;
                }
                if (styleAttributes.isItalic)
                {
                    style |= System.Drawing.FontStyle.Italic;
                }
            }
            if (node.glyph != null)
            {
                if ((node.parent_ != null) && (node.parent_.type_.type == ElementType.Mi))
                {
                    if (styleAttributes != null)
                    {
                        if (styleAttributes.isTop)
                        {
                            style |= System.Drawing.FontStyle.Italic;
                        }
                        else if (((!styleAttributes.isNormal && !styleAttributes.isBold) && (!styleAttributes.isDoubleStruck && !styleAttributes.isFractur)) && ((!styleAttributes.isMonospace && !styleAttributes.isSans) && !styleAttributes.isScript))
                        {
                            style |= System.Drawing.FontStyle.Italic;
                        }
                    }
                    else
                    {
                        style |= System.Drawing.FontStyle.Italic;
                    }
                }
                string fontName = "Symbol";
                if (node.glyph.FontFamily.Length > 0)
                {
                    fontName = node.glyph.FontFamily;
                    if (node.glyph.FontFamily == "ESSTIXNine")
                    {
                        if ((style & System.Drawing.FontStyle.Italic) == System.Drawing.FontStyle.Italic)
                        {
                            style &= ~System.Drawing.FontStyle.Italic;
                        }
                        else
                        {
                            fontName = "ESSTIXTen";
                        }
                    }
                    else if ((node.glyph.FontFamily == "ESSTIXTen") && ((style & System.Drawing.FontStyle.Italic) == System.Drawing.FontStyle.Italic))
                    {
                        fontName = "ESSTIXNine";
                        style &= ~System.Drawing.FontStyle.Italic;
                    }
                }
                if ((node.type_.type == ElementType.Entity) && (node.glyph != null))
                {
                    bool notAuto = false;
                    bool hasDisplayStyle = true;
                    bool dispTrue = false;
                    if (node.parent_ != null)
                    {
                        hasDisplayStyle = node.parent_.displayStyle;
                    }
                    if (((node.parent_ != null) && (node.parent_.type_.type == ElementType.Mo)) && (node.parent_.attrs != null))
                    {
                        string largeop = node.parent_.attrs.GetValue("largeop");
                        if (node.parent_.attrs.Get("maxsize") != null)
                        {
                            hasMax = true;
                        }
                        if (node.parent_.attrs.Get("minsize") != null)
                        {
                            hasMin = true;
                        }
                        switch (largeop.Trim().ToUpper())
                        {
                            case "TRUE":
                            {
                                notAuto = true;
                                dispTrue = true;
                                break;
                            }
                            case "FALSE":
                            {
                                notAuto = false;
                                dispTrue = true;
                                break;
                            }
                        }
                    }
                    if ((dispTrue && notAuto) && hasDisplayStyle)
                    {
                        scale *= 1.5;
                    }
                    else if ((!dispTrue && hasDisplayStyle) && (((((node.glyph.Code == "0222B") || (node.glyph.Code == "0222C")) || ((node.glyph.Code == "0222D") || (node.glyph.Code == "0222E"))) || (((node.glyph.Code == "02232") || (node.glyph.Code == "02233")) || ((node.glyph.Code == "0222F") || (node.glyph.Code == "02230")))) || ((((node.glyph.Code == "02211") || (node.glyph.Code == "0220F")) || ((node.glyph.Code == "02210") || (node.glyph.Code == "022C2"))) || (((node.glyph.Code == "022C3") || (node.glyph.Code == "022C0")) || (((node.glyph.Code == "022C1") || (node.glyph.Code == "02294")) || (node.glyph.Code == "0228E"))))))
                    {
                        scale *= 1.5;
                    }
                }
                FontFamily fam = null;
                if (this.fonts_ != null)
                {
                    fam = this.fonts_.Get(fontName);
                }
                if (fam != null)
                {
                    switch (node.scriptLevel_)
                    {
                        case 0:
                        {
                            r = new System.Drawing.Font(fam, (float) (this.textSize * scale), style);
                            if (hasMin || hasMax)
                            {
                                r = new System.Drawing.Font(fam, (float) ((this.textSize * this.FontScale(r, node.parent_)) * scale), style);
                            }
                            break;
                        }
                        case 1:
                        {
                            r = new System.Drawing.Font(fam, (float) ((this.textSize * 0.71) * scale), style);
                            if (hasMin || hasMax)
                            {
                                r = new System.Drawing.Font(fam, (float) (((this.textSize * this.FontScale(r, node.parent_)) * 0.71) * scale), style);
                            }
                            break;
                        }
                        case 2:
                        {
                            r = new System.Drawing.Font(fam, (float) ((this.textSize * 0.5) * scale), style);
                            if (hasMin || hasMax)
                            {
                                r = new System.Drawing.Font(fam, (float) (((this.textSize * this.FontScale(r, node.parent_)) * 0.5) * scale), style);
                            }
                            break;
                        }
                    }
                }
                else
                {
                    switch (node.scriptLevel_)
                    {
                        case 0:
                        {
                            r = new System.Drawing.Font(fontName, (float) (this.textSize * scale), style);
                            if (hasMin || hasMax)
                            {
                                r = new System.Drawing.Font(fontName, (float) ((this.textSize * this.FontScale(r, node.parent_)) * scale), style);
                            }
                            break;
                        }
                        case 1:
                        {
                            r = new System.Drawing.Font(fontName, (float) ((this.textSize * 0.71) * scale), style);
                            if (hasMin || hasMax)
                            {
                                r = new System.Drawing.Font(fontName, (float) (((this.textSize * this.FontScale(r, node.parent_)) * 0.71) * scale), style);
                            }
                            break;
                        }
                        case 2:
                        {
                            r = new System.Drawing.Font(fontName, (float) ((this.textSize * 0.5) * scale), style);
                            if (hasMin || hasMax)
                            {
                                r = new System.Drawing.Font(fontName, (float) (((this.textSize * this.FontScale(r, node.parent_)) * 0.5) * scale), style);
                            }
                            break;
                        }
                    }
                }
            }
        
            if (r == null)
            {
                r = new System.Drawing.Font("Symbol", this.textSize);
            }
            return r;
        }

        private double FontScale(System.Drawing.Font font, Node moNode)
        {
            double r = 1;
            try
            {
                float defaultSize = this.FontSize(font);
                float dpi = this.DpiX();
                int minSize = -1;
                int maxSize = -1;
                bool hasMaxSize = false;
                bool hasMinSize = false;
                if ((moNode.attrs != null) && (moNode.attrs.Get("maxsize") != null))
                {
                    hasMaxSize = true;
                    maxSize = AttributeBuilder.SizeByAttr(defaultSize, dpi, moNode, "maxsize", (double) defaultSize);
                }
                if ((moNode.attrs != null) && (moNode.attrs.Get("minsize") != null))
                {
                    hasMinSize = true;
                    minSize = AttributeBuilder.SizeByAttr(defaultSize, dpi, moNode, "minsize", (double) defaultSize);
                }
                int intDefault = Convert.ToInt32(Math.Round(this.FontSize(font)));
                if (hasMaxSize && (intDefault > maxSize))
                {
                    return (maxSize / this.FontSize(font));
                }
                if (!hasMinSize || (intDefault >= minSize))
                {
                    return r;
                }
                if (this.IsStretchy(moNode))
                {
                    if (minSize < defaultSize)
                    {
                        r = minSize / this.FontSize(font);
                    }
                    return r;
                }
                return (minSize / this.FontSize(font));
            }
            catch
            {
				return r;
            }
        }


        private Pen outlinePen;
        private Pen blackPen;
        private System.Drawing.Font symbol0;
        private System.Drawing.Font symbol1;
        private System.Drawing.Font symbol2;
        private System.Drawing.Font bold0;
        private System.Drawing.Font bold1;
        private System.Drawing.Font bold2;
        private Fonts.FontCollection fonts_;
        public EntityManager entityManager;
        private System.Drawing.Font mono0;
        private System.Drawing.Font mono1;
        private System.Drawing.Font mono2;
        private System.Drawing.Font operator0;
        private System.Drawing.Font operator1;
        public OperatorDictionary operators;
        private System.Drawing.Font operator2;
        private System.Drawing.Font unknown0;
        private System.Drawing.Font unknown1;
        private System.Drawing.Font unknown2;
        private System.Drawing.Font number0;
        private System.Drawing.Font number1;
        private System.Drawing.Font number2;
        private Brush blackBrush;
        private StringFormat typographicsFormat;
        private Graphics graphics_;
        private int oX;
        private int oY;
        private float textSize;
    }
}

