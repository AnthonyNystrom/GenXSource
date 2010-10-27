namespace FacScan
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Windows.Forms;

    public class DrawImageClass
    {
        public DrawImageClass()
        {
            this.fOption = new ImageOptionForm();
            this.initcolor();
        }

        public DrawImageClass(ImageOptionForm f)
        {
            this.fOption = new ImageOptionForm();
            this.initcolor();
            this.fOption = f;
        }

        public ImageList creatSymbolList(int number)
        {
            ImageList list1 = new ImageList();
            int num1 = 0x12;
            try
            {
                Bitmap bitmap1 = new Bitmap(num1, num1);
                Graphics graphics1 = Graphics.FromImage(bitmap1);
                list1.ColorDepth = ColorDepth.Depth32Bit;
                list1.ImageSize = new Size(num1, num1);
                for (int num2 = 0; num2 < number; num2++)
                {
                    Point[] pointArray4;
                    bitmap1 = new Bitmap(num1, num1);
                    graphics1 = Graphics.FromImage(bitmap1);
                    graphics1.SmoothingMode = SmoothingMode.AntiAlias;
                    Pen pen1 = new Pen(this._color[num2 % this._color.Length], 2f);
                    Pen pen2 = new Pen(this._color[(num2 + 10) % this._color.Length], 2f);
                    SolidBrush brush1 = new SolidBrush(this._color[num2 % this._color.Length]);
                    SolidBrush brush2 = new SolidBrush(this._color[(num2 + 10) % this._color.Length]);
                    switch ((num2 % 6))
                    {
                        case 0:
                            graphics1.FillEllipse(brush1, 1, 1, num1 - 2, num1 - 2);
                            goto Label_02D7;

                        case 1:
                            graphics1.FillRectangle(brush1, 1, 1, num1 - 2, num1 - 2);
                            goto Label_02D7;

                        case 2:
                        {
                            pointArray4 = new Point[] { new Point(num1 / 2, 2), new Point(2, num1 / 2), new Point(num1 / 2, num1 - 2), new Point(num1 - 2, num1 / 2), new Point(num1 / 2, 2) };
                            Point[] pointArray1 = pointArray4;
                            graphics1.DrawLines(pen1, pointArray1);
                            goto Label_02D7;
                        }
                        case 3:
                        {
                            pointArray4 = new Point[] { new Point(num1 / 2, 2), new Point(2, num1 - 2), new Point(num1 - 2, num1 - 2), new Point(num1 / 2, 2) };
                            Point[] pointArray2 = pointArray4;
                            graphics1.DrawLines(pen1, pointArray2);
                            goto Label_02D7;
                        }
                        case 4:
                            graphics1.DrawEllipse(pen1, 2, 2, (num1 / 2) - 2, num1 - 4);
                            graphics1.DrawEllipse(pen2, num1 / 2, 2, (num1 / 2) - 2, num1 - 4);
                            goto Label_02D7;

                        case 5:
                            break;

                        default:
                            goto Label_02D7;
                    }
                    pointArray4 = new Point[] { new Point(0, 0), new Point(0, num1 - 1), new Point(num1 - 1, 0), new Point(0, 0) };
                    Point[] pointArray3 = pointArray4;
                    graphics1.FillRectangle(brush1, 0, 0, num1, num1);
                    graphics1.FillPolygon(brush2, pointArray3);
                Label_02D7:
                    list1.Images.Add(bitmap1);
                    list1.Draw(graphics1, 0, 0, num2);
                }
                graphics1.Dispose();
                bitmap1.Dispose();
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Creat Symbol Error: " + exception1.Message, "Error");
            }
            return list1;
        }

        public void DrawAngleString(Graphics g, float x, float y, float angle, Font font, string text)
        {
            try
            {
                GraphicsPath path1 = new GraphicsPath();
                path1.AddString(text, font.FontFamily, 1, font.Size, new PointF(x, y), new StringFormat(StringFormatFlags.NoClip));
                Matrix matrix1 = new Matrix();
                matrix1.RotateAt(angle, new PointF(x, y));
                path1.Transform(matrix1);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.FillPath(Brushes.Black, path1);
                path1.Dispose();
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Draw Angle String Error: " + exception1.Message, "Error");
            }
        }

        public Color[] initcolor()
        {
            this._color = new Color[] { 
                Color.AliceBlue, Color.AntiqueWhite, Color.Aqua, Color.Aquamarine, Color.Azure, Color.Beige, Color.Bisque, Color.Black, Color.BlanchedAlmond, Color.Blue, Color.BlueViolet, Color.Brown, Color.BurlyWood, Color.CadetBlue, Color.Chartreuse, Color.Chocolate, 
                Color.Coral, Color.CornflowerBlue, Color.Cornsilk, Color.Crimson, Color.Cyan, Color.DarkBlue, Color.DarkCyan, Color.DarkGoldenrod, Color.DarkGray, Color.DarkGreen, Color.DarkKhaki, Color.DarkMagenta, Color.DarkOliveGreen, Color.DarkOrange, Color.DarkOrchid, Color.DarkRed, 
                Color.DarkSalmon, Color.DarkSeaGreen, Color.DarkSlateBlue, Color.DarkSlateGray, Color.DarkTurquoise, Color.DarkViolet, Color.DeepPink, Color.DeepSkyBlue, Color.DimGray, Color.DodgerBlue, Color.Firebrick, Color.FloralWhite, Color.ForestGreen, Color.Fuchsia, Color.Gainsboro, Color.GhostWhite, 
                Color.Gold, Color.Goldenrod, Color.Gray, Color.Green, Color.GreenYellow, Color.Honeydew, Color.HotPink, Color.IndianRed, Color.Indigo, Color.Ivory, Color.Khaki, Color.Lavender, Color.LavenderBlush, Color.LawnGreen, Color.LemonChiffon, Color.LightBlue, 
                Color.LightCoral, Color.LightCyan, Color.LightGoldenrodYellow, Color.LightGray, Color.LightGreen, Color.LightPink, Color.LightSalmon, Color.LightSeaGreen, Color.LightSkyBlue, Color.LightSlateGray, Color.LightSteelBlue, Color.LightYellow, Color.Lime, Color.LimeGreen, Color.Linen, Color.Magenta, 
                Color.Maroon, Color.MediumAquamarine, Color.MediumBlue, Color.MediumOrchid, Color.MediumPurple, Color.MediumSeaGreen, Color.MediumSlateBlue, Color.MediumSpringGreen, Color.MediumTurquoise, Color.MediumVioletRed, Color.MidnightBlue, Color.MintCream, Color.MistyRose, Color.Moccasin, Color.NavajoWhite, Color.Navy, 
                Color.OldLace, Color.Olive, Color.OliveDrab, Color.Orange, Color.OrangeRed, Color.Orchid, Color.PaleGoldenrod, Color.PaleGreen, Color.PaleTurquoise, Color.PaleVioletRed, Color.PapayaWhip, Color.PeachPuff, Color.Peru, Color.Pink, Color.Plum, Color.PowderBlue, 
                Color.Purple, Color.Red, Color.RosyBrown, Color.RoyalBlue, Color.SaddleBrown, Color.Salmon, Color.SandyBrown, Color.SeaGreen, Color.SeaShell, Color.Sienna, Color.Silver, Color.SkyBlue, Color.SlateBlue, Color.SlateGray, Color.Snow, Color.SpringGreen, 
                Color.SteelBlue, Color.Tan, Color.Teal, Color.Thistle, Color.Tomato, Color.Transparent, Color.Turquoise, Color.Violet, Color.Wheat, Color.White, Color.WhiteSmoke, Color.Yellow, Color.YellowGreen
             };
            return this._color;
        }

        public void SaveImage(SaveFileDialog sfDlg, Bitmap bmp)
        {
            try
            {
                sfDlg.Filter = "Bit Map (*.BMP)|*.BMP|Jpeg (*.JPG)|*.JPG|GIF (*.GIF)|*.GIF|PNG (*.PNG)|*.PNG|TIFF (*.TIF)|*.TIF|WMF (*.WMF)|*.WMF|All files (*.*)|*.*";
                ImageFormat format1 = ImageFormat.Bmp;
                if (sfDlg.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                string text1 = sfDlg.FileName;
                string text2 = text1.Substring(text1.IndexOf(".") + 1);
                if ((((text2 != "BMP") && (text2 != "JPG")) && ((text2 != "GIF") && (text2 != "TIF"))) && ((text2 != "WMF") && (text2 != "PNG")))
                {
                    return;
                }
                switch (text2)
                {
                    case "BMP":
                        format1 = ImageFormat.Bmp;
                        goto Label_011E;

                    case "JPG":
                        format1 = ImageFormat.Jpeg;
                        goto Label_011E;

                    case "GIF":
                        format1 = ImageFormat.Gif;
                        goto Label_011E;

                    case "TIF":
                        format1 = ImageFormat.Tiff;
                        goto Label_011E;

                    case "WMF":
                        format1 = ImageFormat.Wmf;
                        break;

                    case "PNG":
                        format1 = ImageFormat.Png;
                        break;
                }
            Label_011E:
                bmp.Save(text1, format1);
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Save Image Error: " + exception1.Message, "Error!");
            }
        }


        public Color[] color
        {
            get
            {
                return this._color;
            }
        }


        private Color[] _color;
        private ImageOptionForm fOption;
    }
}

