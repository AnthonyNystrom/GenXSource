using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Genetibase.NuGenDEMVis.Data;

namespace Genetibase.NuGenDEMVis.UI
{
    public partial class DataProfilePreviewControl : UserControl
    {
        Image dataPreviewImg;
        DataProfile profile;
        DataProfile.SubProfile subProfile;

        public DataProfilePreviewControl()
        {
            InitializeComponent();
        }

        public DataProfile Profile
        {
            get { return profile; }
        }

        public DataProfile.SubProfile SubProfile
        {
            get { return subProfile; }
        }

        public void Setup(DataProfile dataProfile, DataProfile.SubProfile dataSubProfile)
        {
            profile = dataProfile;
            subProfile = dataSubProfile;

            RedrawPreviewImg();
        }

        private void DisposeResources()
        {
            if (dataPreviewImg != null)
            {
                dataPreviewImg.Dispose();
                dataPreviewImg = null;
            }
        }

        protected override void OnResize(System.EventArgs e)
        {
            RedrawPreviewImg();
        }

        private void RedrawPreviewImg()
        {
            DisposeResources();

            dataPreviewImg = new Bitmap(Width, Height);

            Graphics g = Graphics.FromImage(dataPreviewImg);
            g.CompositingQuality = CompositingQuality.HighQuality;

            int sampleDistance = 255 / 32;
            int sampleSpacing = Width / 32;
            float heightScale = Height / 255f;
            Point lastPos = new Point(0, 0);
            LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(0, Height), Color.Black, Color.White);
            for (int i = 0; i < 33; i++)
            {
                int sampleValue = sampleDistance * i;
                int sampleHeight = (int)(heightScale * sampleValue);

                int x = sampleSpacing * i;
                if (i != 0)
                {
                    g.FillPolygon(brush, new Point[] { lastPos, new Point(x, sampleHeight), new Point(x, Height), new Point(lastPos.X, Height) });
                    g.DrawLine(Pens.Black, lastPos.X, lastPos.Y, x, sampleHeight);
                }
                lastPos = new Point(x, sampleHeight);
            }
            g.Dispose();

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (dataPreviewImg != null)
                e.Graphics.DrawImage(dataPreviewImg, (Width / 2) - (dataPreviewImg.Width / 2),
                                     (Height / 2) - (dataPreviewImg.Height / 2));
        }
    }
}