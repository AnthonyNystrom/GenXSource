using System.Drawing;
using System.Windows.Forms;
using Genetibase.NuGenRenderOptics.MDX1.Rasterization;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ViewRasterizer view = new ViewRasterizer();
            view.ViewArea = new Rectangle(0, 0, 256, 256);
            view.CollectData();

            pictureBox1.Image = view.Rasterize();
        }
    }
}