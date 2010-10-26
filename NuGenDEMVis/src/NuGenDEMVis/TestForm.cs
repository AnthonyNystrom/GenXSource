using System.Drawing;
using System.Windows.Forms;
using Genetibase.NuGenDEMVis.Graph;
using Microsoft.DirectX;

namespace Genetibase.NuGenDEMVis
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();

            Bitmap img = (Bitmap)Bitmap.FromFile("c:/test.png");
            CrossSectionGraph graph = CrossSectionGraph.GenerateFromSampler(null, new Rectangle(0, 0, 256, 256),
                                                                            new Vector3(0, 0, 0),
                                                                            new Vector3(256, 0, 256),
                                                                            16, img, CrossSectionGraph.SamplingMode.OnGrid);

            pictureBox1.Image = img;
            Bitmap graphImg = new Bitmap(256, 256);
            graph.DrawGraph(graphImg);

            pictureBox2.Image = graphImg;

            /*BSpline spline = new BSpline(new PointF[] { new PointF(10, 10), new PointF(20, 70), new PointF(50, 50), new PointF(100, 50), new PointF(100, 100)  });
            Bitmap bitmap = new Bitmap(300, 300);
            spline.Draw(bitmap, 30, Color.Black, 2);
            pictureBox1.Image = bitmap;*/
        }
    }
}