using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.NuGenDEMVis.UI
{
    public partial class DataSourceControl : UserControl
    {
        public class DataSourcePreviewInfo
        {
            public DataSourceImageGroup[] Groups;
            public string Type;
            public string DataType;
            public Size Dimensions;
        }

        public class DataSourcePreviewImage
        {
            public Image ShrunkImage;
            public string Name;
            public Size Size;

            /// <summary>
            /// Initializes a new instance of the DataSourceImage class.
            /// </summary>
            /// <param name="shrunkImage"></param>
            /// <param name="name"></param>
            /// <param name="size"></param>
            public DataSourcePreviewImage(Image shrunkImage, string name, Size size)
            {
                ShrunkImage = shrunkImage;
                Name = name;
                Size = size;
            }
        }

        public class DataSourceImageGroup
        {
            public DataSourcePreviewImage[] DataSources;
            public int Radius;

            /// <summary>
            /// Initializes a new instance of the DataSourceImageGroup class.
            /// </summary>
            /// <param name="dataSources"></param>
            public DataSourceImageGroup(DataSourcePreviewImage[] dataSources)
            {
                DataSources = dataSources;

                // calc radius
                Radius = int.MinValue;
                foreach (DataSourcePreviewImage src in dataSources)
                {
                    int length = (int)Math.Sqrt(((src.Size.Width / 2f) * (src.Size.Width / 2f)) + ((src.Size.Height / 2f) * (src.Size.Height / 2f)));
                    if (length > Radius)
                        Radius = length;
                }
            }
        }

        private DataSourceImageGroup[] dataSrcGroups;
        private DataSourcePreviewInfo dataSrcInfo;

        public DataSourceControl()
        {
            InitializeComponent();

            /*DataSourcePreviewImage[] imgs = new DataSourcePreviewImage[] { new DataSourcePreviewImage(Image.FromFile(@"C:\Users\Public\Pictures\Sample Pictures\Forest.jpg"), "Heights", new Size(64, 64)), new DataSourcePreviewImage(null, "Heights", new Size(64, 64)), new DataSourcePreviewImage(null, "Heights", new Size(64, 64)) };
            DataSourceImageGroup[] dsg = new DataSourceImageGroup[] { new DataSourceImageGroup(imgs), new DataSourceImageGroup(imgs)  };

            DataSourceGroups = dsg;*/
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(pictureBox1.Height * 3, pictureBox1.Height);
            pictureBox1.Location = new Point(Width - 5 - pictureBox1.Width, pictureBox1.Location.Y);
        }

        public DataSourceImageGroup[] DataSourceGroups
        {
            get { return dataSrcGroups; }
            set { dataSrcGroups = value; RedrawGroupsBitmap(); }
        }

        public DataSourcePreviewInfo DataSource
        {
            get { return dataSrcInfo; }
            set { dataSrcInfo = value;
                if (dataSrcInfo != null)
                {
                    RefreshValues();
                    DataSourceGroups = dataSrcInfo.Groups;
                }
            }
        }

        private void RefreshValues()
        {
            if (dataSrcInfo == null)
                return;

            label2.Text = dataSrcInfo.Type;
            label4.Text = string.Format("{0}x{1}", dataSrcInfo.Dimensions.Width, dataSrcInfo.Dimensions.Height);
            label6.Text = dataSrcInfo.DataType;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            RedrawGroupsBitmap();
        }

        private void RedrawGroupsBitmap()
        {
            if (dataSrcGroups == null)
                return;

            // create canvas
            Bitmap dest = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(dest);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // measure stats
            Size srcSize = new Size(0, int.MinValue);
            foreach (DataSourceImageGroup group in dataSrcGroups)
            {
                if (group.Radius > srcSize.Height)
                    srcSize.Height = group.Radius * 2;
                srcSize.Width += group.Radius * 2;
            }
            int spacing = srcSize.Width / (dataSrcGroups.Length + 1);

            // draw each group
            /*if (srcSize.Width > srcSize.Height)
                srcSize.Height = srcSize.Width;
            else if (srcSize.Width < srcSize.Height)
                srcSize.Width = srcSize.Height;*/

            float scaleX = (float)dest.Width / srcSize.Width;
            float scaleY = (float)dest.Height / srcSize.Height;
            g.ScaleTransform(1, 1);
            int xPos = 0;
            Font font = new Font(Font.FontFamily, 8);
            foreach (DataSourceImageGroup group in dataSrcGroups)
            {
                xPos += spacing;

                float angle = 0;
                for (int i = group.DataSources.Length - 1; i >= 0; i--)
                {
                    angle += -22.5f;

                    int xT = (int)(xPos * scaleX);
                    int yT = (int)((srcSize.Height / 2) * scaleY);
                    g.TranslateTransform(xT, yT);
                    Rectangle rect = new Rectangle(-group.DataSources[i].Size.Width / 2,
                                                   -group.DataSources[i].Size.Height / 2,
                                                   group.DataSources[i].Size.Width,
                                                   group.DataSources[i].Size.Height);
                    if (i == 0)
                    {
                        if (group.DataSources[i].ShrunkImage != null)
                            g.DrawImage(group.DataSources[i].ShrunkImage, rect);
                        else
                        {
                            g.FillRectangle(Brushes.White, rect);
                            g.DrawRectangle(Pens.Black, rect);
                        }
                    }
                    else
                    {
                        g.RotateTransform(angle);
                        if (group.DataSources[i].ShrunkImage != null)
                            g.DrawImage(group.DataSources[i].ShrunkImage, rect);
                        else
                        {
                            g.FillRectangle(Brushes.White, rect);
                            g.DrawRectangle(Pens.Black, rect);
                        }
                        g.RotateTransform(-angle);
                    }
                    if (group.DataSources[i].Name != null && group.DataSources[i].Name.Length > 0)
                    {
                        SizeF strSz = g.MeasureString(group.DataSources[i].Name, font);
                        g.DrawString(group.DataSources[i].Name, font, Brushes.Black, -strSz.Width / 2, -strSz.Height / 2);
                    }
                    g.TranslateTransform(-xT, -yT);
                }
            }

            g.Flush();
            g.Dispose();
            pictureBox1.Image = dest;
        }
    }
}