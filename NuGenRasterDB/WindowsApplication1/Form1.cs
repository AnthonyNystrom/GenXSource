using System;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.RasterDatabase;
using Genetibase.RasterDatabase.Geometry;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        RectangleGroupQuadTree tree;
        Image src, src2;
        int depth;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            src = Bitmap.FromFile(@"f:\output.bmp");
            src2 = Bitmap.FromFile(@"C:\Users\Andrew\Pictures\sense.jpg");

            RasterDatabase db = new RasterDatabase();
            DataLayer layer;
            db.AddLayer(layer = new DataLayer("DiffuseMap", 32, "X8R8G8B8"));
            layer.AddArea(new DataArea(new Rectangle(0, 0, 960, 1200), new RectangleF(0, 0, 1, 1), src));
            layer.AddArea(new DataArea(new Rectangle(1000, 50, 501, 501), new RectangleF(0, 0, 1, 1), src2));

            tree = layer.BuildQuadTree(512);

            DrawDepth();
        }

        private void DrawDepth()
        {
            RectangleGroupQuadTree.GroupNode[] nodes;
            tree.GetNodes(++depth, out nodes);
            int size = 1;
            if (depth > 1)
                size = (int)Math.Pow(2, depth - 1);
            int area = size * 512;
            int maxArea = ((int)Math.Pow(2, tree.Depth - 1)) * 512;

            Image img = new Bitmap(area, area);
            Graphics g = Graphics.FromImage(img);

            float scale = (float)area / maxArea;
            g.ScaleTransform(scale, scale);

            //g.FillRectangle(Brushes.Black, 0, 0, maxArea, maxArea);

            //g.DrawImage(src, 0, 0);

            foreach (RectangleGroupQuadTree.GroupNode node in nodes)
            {
                foreach (DataArea dataArea in node.Rectangles)
                {
                    Rectangle srcArea = new Rectangle((int)(dataArea.TexCoords.X * src.Width), (int)(dataArea.TexCoords.Y * src.Height),
                                                      (int)(dataArea.TexCoords.Width * src.Width), (int)(dataArea.TexCoords.Height * src.Height));
                    g.DrawImage((Bitmap)dataArea.Data, dataArea.Area, srcArea, GraphicsUnit.Pixel);
                }
            }
            
            foreach (RectangleGroupQuadTree.GroupNode node in nodes)
            {
                g.DrawRectangle(Pens.Red, node.NodeArea);
                foreach (DataArea dataArea in node.Rectangles)
                {
                    g.DrawRectangle(Pens.Yellow, dataArea.Area);
                }
            }

            g.Flush();
            g.Dispose();

            pictureBox1.Image = img;

            //img.Save("c:/output-" + depth + ".png", ImageFormat.Png);

            if (depth >= tree.Depth)
                depth = 0;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DrawDepth();
        }
    }
}