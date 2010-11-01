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
                public void DrawString(Node node, StyleAttributes styleAttributes, Color color)
        {
            this.DrawString(node, styleAttributes, node.literalText, color);
        }

        public void DrawString(Node node, StyleAttributes styleAttributes, string text, Color color)
        {
            this.DrawString(node, styleAttributes, node.box.X, node.box.Y, node.box.Dx, node.box.Dy, text, color);
        }

        public void DrawString(int x, int y, string text, Color color, System.Drawing.Font font)
        {
            try
            {
                if (text.Length > 0)
                {
                    PointF tf1 = new PointF((float) (x + this.oX), (float) (y + this.oY));
                    this.graphics_.DrawString(text, font, new SolidBrush(color), tf1, this.typographicsFormat);
                }
            }
            catch
            {
            }
        }

        public void DrawString(Node node, StyleAttributes styleAttributes, int x, int y, int dx, int dy, string text, Color color)
        {
            Brush brush1;

            if (styleAttributes != null)
            {
                if (node.NotBlack() || (((node.type_.type == ElementType.Entity) && (node.parent_ != null)) && node.parent_.NotBlack()))
                {
                    brush1 = new SolidBrush(styleAttributes.color);
                }
                else
                {
                    brush1 = new SolidBrush(color);
                }
            }
            else if (color != Color.Black)
            {
                brush1 = new SolidBrush(color);
            }
            else
            {
                brush1 = this.blackBrush;
            }
            System.Drawing.Font font1 = this.GetSuitableFont(node, styleAttributes);
            
            PointF tf1 = new PointF((float) ((this.oX + x) + dx), (float) ((this.oY + y) + dy));
            if (((node.glyph != null) && node.glyph.IsVisible) && ((node.glyph.Category.Name != "Spaces") && (node.glyph.FontFamily.Length == 0)))
            {
                brush1 = new SolidBrush(Color.Red);
                this.graphics_.DrawString("?", font1, brush1, tf1, this.typographicsFormat);
                return;
            }
            if ((text == "{"))
            {
                try
                {
                    double num3 = this.MeasureWidth(node, node.style_, text);
                    double num4 = 0.1;
                    double num5 = num4 * num3;
                    tf1.X -= (int) Math.Round(num5);
                }
                catch
                {
                }
            }
            if (((node.type_.type == ElementType.Entity) && (node.glyph != null)) && (node.glyph.Code == "0222C"))
            {
                this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                tf1.X += node.box.Width / 3;
                this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                goto Label_1B94;
            }
            if (((node.type_.type == ElementType.Entity) && (node.glyph != null)) && (node.glyph.Code == "000A8"))
            {
                string text1 = "";
                text1 = text + text;
                this.graphics_.DrawString(text1, font1, brush1, tf1, this.typographicsFormat);
                goto Label_1B94;
            }
            if (((node.type_.type == ElementType.Entity) && (node.glyph != null)) && (node.glyph.Code == "0222D"))
            {
                this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                tf1.X += node.box.Width / 4;
                this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                tf1.X += node.box.Width / 4;
                this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                goto Label_1B94;
            }
            if (((node.type_.type != ElementType.Entity) || (node.glyph == null)) || (((((node.glyph.Code != "02192") && (node.glyph.Code != "02190")) && ((node.glyph.Code != "02194") && (node.glyph.Code != "0F576"))) && (((node.glyph.Code != "0F577") && (node.glyph.Code != "0F578")) && ((node.glyph.Code != "021C4") && (node.glyph.Code != "021C6")))) && ((((node.glyph.Code != "021CC") && (node.glyph.Code != "021CB")) && ((node.glyph.Code != "021C0") && (node.glyph.Code != "021C1"))) && ((((node.glyph.Code != "021BC") && (node.glyph.Code != "021BD")) && ((node.glyph.Code != "0005E") && (node.glyph.Code != "000AF"))) && (((node.glyph.Code != "0FE37") && (node.glyph.Code != "0FE38")) && (node.glyph.Code != "00332"))))))
            {
                this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                goto Label_1B94;
            }
            float single1 = node.box.Width;
            float single2 = node.box.Height;
            this.graphics_.SetClip(new Rectangle((int) tf1.X, ((int) tf1.Y) - 20, node.box.Width, node.box.Height + 40));
            try
            {
                PointF tf2;
                SizeF ef1;
                if (node.glyph.Code == "0FE37")
                {
                    ef1 = this.graphics_.MeasureString("D", font1, tf1, this.typographicsFormat);
                    float single3 = ef1.Width;
                    ef1 = this.graphics_.MeasureString(text, font1, tf1, this.typographicsFormat);
                    float single4 = ef1.Width;
                    ef1 = this.graphics_.MeasureString("F", font1, tf1, this.typographicsFormat);
                    float single5 = ef1.Width;
                    ef1 = this.graphics_.MeasureString("C", font1, tf1, this.typographicsFormat);
                    float single6 = ef1.Width;
                    float single7 = 0f;
                    float single8 = 0f;
                    single8 = tf1.X;
                    single6 -= 2f;
                    this.graphics_.DrawString("D", font1, brush1, tf1, this.typographicsFormat);
                    tf2 = tf1;
                    tf2.X += single3 - 2f;
                    single7 = single3 - 2f;
                    int num6 = ((int) (((single1 / 2f) - (single4 / 2f)) - single3)) + 4;
                    this.graphics_.SetClip(new Rectangle((int) tf2.X, ((int) tf2.Y) - 20, num6, node.box.Height + 40));
                    try
                    {
                        while (single7 < ((single1 / 2f) - (single4 / 2f)))
                        {
                            this.graphics_.DrawString("C", font1, brush1, tf2, this.typographicsFormat);
                            tf2.X += single6;
                            single7 += single6;
                        }
                    }
                    catch
                    {
                    }
                    this.graphics_.ResetClip();
                    tf2 = tf1;
                    tf2.X += ((single1 / 2f) + (single4 / 2f)) - 2f;
                    single7 = ((single1 / 2f) + (single4 / 2f)) - 2f;
                    this.graphics_.SetClip(new Rectangle((int) tf2.X, ((int) tf2.Y) - 20, num6, node.box.Height + 40));
                    try
                    {
                        while (single7 < (single1 - single5))
                        {
                            this.graphics_.DrawString("C", font1, brush1, tf2, this.typographicsFormat);
                            tf2.X += single6;
                            single7 += single6;
                        }
                    }
                    catch
                    {
                    }
                    this.graphics_.ResetClip();
                    tf1.X = (single8 + (single1 / 2f)) - (single4 / 2f);
                    this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                    tf1.X = (single8 + single1) - single5;
                    this.graphics_.DrawString("F", font1, brush1, tf1, this.typographicsFormat);
                }
                else if (node.glyph.Code == "0FE38")
                {
                    ef1 = this.graphics_.MeasureString("?", font1, tf1, this.typographicsFormat);
                    float single9 = ef1.Width;
                    ef1 = this.graphics_.MeasureString(text, font1, tf1, this.typographicsFormat);
                    float single10 = ef1.Width;
                    ef1 = this.graphics_.MeasureString("A", font1, tf1, this.typographicsFormat);
                    float single11 = ef1.Width;
                    ef1 = this.graphics_.MeasureString("C", font1, tf1, this.typographicsFormat);
                    float single12 = ef1.Width;
                    float single13 = 0f;
                    float single14 = 0f;
                    single14 = tf1.X;
                    single12 -= 2f;
                    this.graphics_.DrawString("?", font1, brush1, tf1, this.typographicsFormat);
                    tf2 = tf1;
                    tf2.X += single9 - 2f;
                    single13 = single9 - 2f;
                    int num7 = ((int) (((single1 / 2f) - (single10 / 2f)) - single9)) + 4;
                    this.graphics_.SetClip(new Rectangle((int) tf2.X, ((int) tf2.Y) - 20, num7, node.box.Height + 40));
                    try
                    {
                        while (single13 < ((single1 / 2f) - (single10 / 2f)))
                        {
                            this.graphics_.DrawString("C", font1, brush1, tf2, this.typographicsFormat);
                            tf2.X += single12;
                            single13 += single12;
                        }
                    }
                    catch
                    {
                    }
                    this.graphics_.ResetClip();
                    tf2 = tf1;
                    tf2.X += ((single1 / 2f) + (single10 / 2f)) - 2f;
                    single13 = ((single1 / 2f) + (single10 / 2f)) - 2f;
                    this.graphics_.SetClip(new Rectangle((int) tf2.X, ((int) tf2.Y) - 20, num7, node.box.Height + 40));
                    try
                    {
                        while (single13 < (single1 - single11))
                        {
                            this.graphics_.DrawString("C", font1, brush1, tf2, this.typographicsFormat);
                            tf2.X += single12;
                            single13 += single12;
                        }
                    }
                    catch
                    {
                    }
                    this.graphics_.ResetClip();
                    tf1.X = (single14 + (single1 / 2f)) - (single10 / 2f);
                    this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                    tf1.X = (single14 + single1) - single11;
                    this.graphics_.DrawString("A", font1, brush1, tf1, this.typographicsFormat);
                }
                else if (node.glyph.Code == "021C4")
                {
                    ef1 = this.graphics_.MeasureString("&", font1, tf1, this.typographicsFormat);
                    float single15 = ef1.Width;
                    ef1 = this.graphics_.MeasureString("(", font1, tf1, this.typographicsFormat);
                    float single16 = ef1.Width;
                    ef1 = this.graphics_.MeasureString(text, font1, tf1, this.typographicsFormat);
                    float single17 = ef1.Width;
                    float single18 = 0f;
                    this.graphics_.DrawString("&", font1, brush1, tf1, this.typographicsFormat);
                    tf2 = tf1;
                    tf2.X += single15;
                    for (single18 = single15; single18 < (single1 - single17); single18 += single16)
                    {
                        this.graphics_.DrawString("(", font1, brush1, tf2, this.typographicsFormat);
                        tf2.X += single16;
                    }
                    tf1.X = (tf1.X + single1) - single17;
                    this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                }
                else if (node.glyph.Code == "021C6")
                {
                    ef1 = this.graphics_.MeasureString(text, font1, tf1, this.typographicsFormat);
                    float single19 = ef1.Width;
                    ef1 = this.graphics_.MeasureString("(", font1, tf1, this.typographicsFormat);
                    float single20 = ef1.Width;
                    ef1 = this.graphics_.MeasureString(",", font1, tf1, this.typographicsFormat);
                    float single21 = ef1.Width;
                    float single22 = 0f;
                    this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                    tf2 = tf1;
                    tf2.X += single19;
                    for (single22 = single19; single22 < (single1 - single21); single22 += single20)
                    {
                        this.graphics_.DrawString("(", font1, brush1, tf2, this.typographicsFormat);
                        tf2.X += single20;
                    }
                    tf1.X = (tf1.X + single1) - single21;
                    this.graphics_.DrawString(",", font1, brush1, tf1, this.typographicsFormat);
                }
                else if (node.glyph.Code == "021CC")
                {
                    ef1 = this.graphics_.MeasureString("2", font1, tf1, this.typographicsFormat);
                    float single23 = ef1.Width;
                    ef1 = this.graphics_.MeasureString("(", font1, tf1, this.typographicsFormat);
                    float single24 = ef1.Width;
                    ef1 = this.graphics_.MeasureString(text, font1, tf1, this.typographicsFormat);
                    float single25 = ef1.Width;
                    float single26 = 0f;
                    this.graphics_.DrawString("2", font1, brush1, tf1, this.typographicsFormat);
                    tf2 = tf1;
                    tf2.X += single23;
                    for (single26 = single23; single26 < (single1 - single25); single26 += single24)
                    {
                        this.graphics_.DrawString("(", font1, brush1, tf2, this.typographicsFormat);
                        tf2.X += single24;
                    }
                    tf1.X = (tf1.X + single1) - single25;
                    this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                }
                else if (node.glyph.Code == "021CB")
                {
                    ef1 = this.graphics_.MeasureString(text, font1, tf1, this.typographicsFormat);
                    float single27 = ef1.Width;
                    ef1 = this.graphics_.MeasureString("(", font1, tf1, this.typographicsFormat);
                    float single28 = ef1.Width;
                    ef1 = this.graphics_.MeasureString("3", font1, tf1, this.typographicsFormat);
                    float single29 = ef1.Width;
                    float single30 = 0f;
                    this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                    tf2 = tf1;
                    tf2.X += single27;
                    for (single30 = single27; single30 < (single1 - single29); single30 += single28)
                    {
                        this.graphics_.DrawString("(", font1, brush1, tf2, this.typographicsFormat);
                        tf2.X += single28;
                    }
                    tf1.X = (tf1.X + single1) - single29;
                    this.graphics_.DrawString("3", font1, brush1, tf1, this.typographicsFormat);
                }
                else if ((node.glyph.Code == "02192") || (node.glyph.Code == "0F577"))
                {
                    ef1 = this.graphics_.MeasureString("$", font1, tf1, this.typographicsFormat);
                    float single31 = ef1.Width;
                    ef1 = this.graphics_.MeasureString(text, font1, tf1, this.typographicsFormat);
                    float single32 = ef1.Width;
                    float single33 = 0f;
                    single31 -= 1f;
                    tf2 = tf1;
                    while (single33 < (single1 - single32))
                    {
                        this.graphics_.DrawString("$", font1, brush1, tf2, this.typographicsFormat);
                        tf2.X += single31;
                        single33 += single31;
                    }
                    tf1.X = (tf1.X + single1) - single32;
                    this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                }
                else
                {
                    if (node.glyph.Code == "0005E")
                    {
                        tf2 = tf1;
                        try
                        {
                            ef1 = this.graphics_.MeasureString("m", font1, tf1, this.typographicsFormat);
                            float single34 = ef1.Width;
                            if (single1 > (single34 * 1.1))
                            {
                                float single35 = single1 / 2f;
                                single35 = (float) Math.Round((double) single35);
                                float single36 = single2 / 3f;
                                single36 = (float) Math.Round((double) single36);
                                if (single36 < 1f)
                                {
                                    single36 = 1f;
                                }
                                PointF tf3 = new PointF(tf2.X, tf2.Y + single2);
                                PointF tf4 = new PointF(tf2.X + single35, tf2.Y);
                                PointF tf5 = new PointF(tf2.X + single1, tf2.Y + single2);
                                PointF tf6 = new PointF(tf2.X + single35, tf2.Y + single36);
                                GraphicsPath path1 = new GraphicsPath();
                                path1.AddLine(tf3, tf4);
                                path1.AddLine(tf4, tf5);
                                path1.AddLine(tf5, tf6);
                                path1.AddLine(tf6, tf3);
                                this.graphics_.FillPath(brush1, path1);
                                goto Label_1B71;
                            }
                            this.graphics_.DrawString(text, font1, brush1, tf2, this.typographicsFormat);
                            goto Label_1B71;
                        }
                        catch
                        {
                            goto Label_1B71;
                        }
                    }
                    if (node.glyph.Code == "000AF")
                    {
                        ef1 = this.graphics_.MeasureString("$", font1, tf1, this.typographicsFormat);
                        float single37 = ef1.Width;
                        ef1 = this.graphics_.MeasureString(text, font1, tf1, this.typographicsFormat);
                        float single38 = ef1.Width;
                        float single39 = 0f;
                        tf2 = tf1;
                        while (single39 < (single1 - single38))
                        {
                            this.graphics_.DrawString("$", font1, brush1, tf2, this.typographicsFormat);
                            tf2.X += single37;
                            single39 += single37;
                        }
                        tf1.X = (tf1.X + single1) - single38;
                        this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                    }
                    else if (node.glyph.Code == "00332")
                    {
                        ef1 = this.graphics_.MeasureString("$", font1, tf1, this.typographicsFormat);
                        float single40 = ef1.Width;
                        ef1 = this.graphics_.MeasureString(text, font1, tf1, this.typographicsFormat);
                        float single41 = ef1.Width;
                        float single42 = 0f;
                        tf2 = tf1;
                        while (single42 < (single1 - single41))
                        {
                            this.graphics_.DrawString("$", font1, brush1, tf2, this.typographicsFormat);
                            tf2.X += single40;
                            single42 += single40;
                        }
                        tf1.X = (tf1.X + single1) - single41;
                        this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                    }
                    else if ((node.glyph.Code == "021C0") || (node.glyph.Code == "021C1"))
                    {
                        ef1 = this.graphics_.MeasureString("$", font1, tf1, this.typographicsFormat);
                        float single43 = ef1.Width;
                        ef1 = this.graphics_.MeasureString(text, font1, tf1, this.typographicsFormat);
                        float single44 = ef1.Width;
                        float single45 = 0f;
                        tf2 = tf1;
                        while (single45 < (single1 - single44))
                        {
                            this.graphics_.DrawString("$", font1, brush1, tf2, this.typographicsFormat);
                            tf2.X += single43;
                            single45 += single43;
                        }
                        tf1.X = (tf1.X + single1) - single44;
                        this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                    }
                    else if ((node.glyph.Code == "021BC") || (node.glyph.Code == "021BD"))
                    {
                        ef1 = this.graphics_.MeasureString(text, font1, tf1, this.typographicsFormat);
                        float single46 = ef1.Width;
                        ef1 = this.graphics_.MeasureString("$", font1, tf1, this.typographicsFormat);
                        float single47 = ef1.Width;
                        float single48 = 0f;
                        this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                        tf2 = tf1;
                        tf2.X = (tf2.X + single1) - single47;
                        for (single48 = single1; single48 > single46; single48 -= single47)
                        {
                            this.graphics_.DrawString("$", font1, brush1, tf2, this.typographicsFormat);
                            tf2.X -= single47;
                        }
                    }
                    else if ((node.glyph.Code == "02190") || (node.glyph.Code == "0F576"))
                    {
                        ef1 = this.graphics_.MeasureString(text, font1, tf1, this.typographicsFormat);
                        float single49 = ef1.Width;
                        ef1 = this.graphics_.MeasureString("$", font1, tf1, this.typographicsFormat);
                        float single50 = ef1.Width;
                        float single51 = 0f;
                        this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                        single50 -= 1f;
                        tf2 = tf1;
                        tf2.X = (tf2.X + single1) - single50;
                        for (single51 = single1; single51 > single49; single51 -= single50)
                        {
                            this.graphics_.DrawString("$", font1, brush1, tf2, this.typographicsFormat);
                            tf2.X -= single50;
                        }
                    }
                    else if ((node.glyph.Code == "0F578") || (node.glyph.Code == "02194"))
                    {
                        ef1 = this.graphics_.MeasureString("!", font1, tf1, this.typographicsFormat);
                        float single52 = ef1.Width;
                        ef1 = this.graphics_.MeasureString("$", font1, tf1, this.typographicsFormat);
                        float single53 = ef1.Width;
                        ef1 = this.graphics_.MeasureString(text, font1, tf1, this.typographicsFormat);
                        float single54 = ef1.Width;
                        float single55 = 0f;
                        this.graphics_.DrawString("!", font1, brush1, tf1, this.typographicsFormat);
                        single53 -= 1f;
                        single52 -= 1f;
                        tf2 = tf1;
                        tf2.X += single52;
                        for (single55 = single52; single55 < (single1 - single54); single55 += single53)
                        {
                            this.graphics_.DrawString("$", font1, brush1, tf2, this.typographicsFormat);
                            tf2.X += single53;
                        }
                        tf1.X = (tf1.X + single1) - single54;
                        this.graphics_.DrawString(text, font1, brush1, tf1, this.typographicsFormat);
                    }
                }
            }
            catch
            {
            }
        Label_1B71:
            this.graphics_.ResetClip();
        Label_1B94:
            if (!node.IsUnderlined())
            {
                return;
            }
            int num8 = (int) this.Leading((float) node.box.Height, font1);
            this.graphics_.DrawLine(new Pen(brush1), this.oX + node.box.X, ((this.oY + node.box.Y) + node.box.Height) - (num8 / 2), (this.oX + node.box.X) + node.box.Width, ((this.oY + node.box.Y) + node.box.Height) - (num8 / 2));
        }
    }
}