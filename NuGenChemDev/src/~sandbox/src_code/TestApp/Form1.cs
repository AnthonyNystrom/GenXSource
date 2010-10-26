using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace TestApp
{
    public partial class Form1 : Form
    {
        //Device device;

        //VertexBuffer vBuffer;
        //Texture texture;
        //int numVerts;

        //float xRot = 0.0f, yRot = 0.0f;

        //int detail;

        class TestItem : OcTree.OcTreeItem
        {
            public TestItem(Vector3 position, Vector3 dim, float radius)
                : base(position, dim, radius)
            {
            }
        }

        public Form1()
        {
            InitializeComponent();

            bool results = Geometry.SphereBoundProbe(new Vector3(5, 0, 0), 1, new Vector3(0, 0, 0), new Vector3(1, 0, 0));

            OcTree.OcTree<TestItem> tree = new OcTree.OcTree<TestItem>(20);
            tree.Insert(new TestItem(new Vector3(5, 5, 0), new Vector3(10, 10, 10), 10));
            tree.Insert(new TestItem(new Vector3(5, 5, 0), new Vector3(3, 3, 3), 3));
            tree.Insert(new TestItem(new Vector3(5, 5, 5), new Vector3(0.1f, 0.1f, 0.1f), 0.1f));
            tree.Insert(new TestItem(new Vector3(5, 1, 15), new Vector3(1.6f, 1.6f, 1.6f), 1.6f));

            tree.Insert(new TestItem(new Vector3(1, 0, 0), new Vector3(1, 1, 1), 1));
            tree.Insert(new TestItem(new Vector3(15, 15, 0), new Vector3(1, 1, 1), 1));
            tree.Insert(new TestItem(new Vector3(5, 15, 5), new Vector3(1, 1, 1), 1));
            tree.Insert(new TestItem(new Vector3(18, 1, 1), new Vector3(3, 5, 6), 6));


            //object hit = tree.RayIntersectFirst(new Vector3(0, 0, 0), new Vector3(1, 0, 0));

            //PresentParameters pParams = new PresentParameters();
            //pParams.SwapEffect = SwapEffect.Copy;
            //pParams.Windowed = true;
            //pParams.AutoDepthStencilFormat = DepthFormat.D16;
            //pParams.EnableAutoDepthStencil = true;

            //device = new Device(0, DeviceType.Hardware, this.panel1, CreateFlags.SoftwareVertexProcessing, pParams);
            //device.RenderState.Lighting = false;

            //detail = 12;
            //int numPoints = detail * (detail / 2) * 6;

            //CustomVertex.PositionNormalTextured[] points = new CustomVertex.PositionNormalTextured[numPoints];
            //CreateSphere2(new Vector3(), 2, detail, points);

            //vBuffer = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), numPoints, device, Usage.None, CustomVertex.PositionNormalTextured.Format, Pool.Managed);
            //vBuffer.SetData(points, 0, LockFlags.None);
            //numVerts = numPoints;//((8 * 4) + 2) * 4;
            
            /*SphereMathHelper.SphereN sphereN = SphereMathHelper.CalcSphereWNormals(10, 10, 1, new Vector3(), true);

            vBuffer = new VertexBuffer(typeof(CustomVertex.PositionTextured), sphereN.Positions.Length, device, Usage.None, CustomVertex.PositionTextured.Format, Pool.Managed);
            CustomVertex.PositionTextured[] sphere = (CustomVertex.PositionTextured[])vBuffer.Lock(0, LockFlags.None);
            for (int i = 0; i < sphereN.Positions.Length; i++)
            {
                sphere[i].Position = sphereN.Positions[i];
                sphere[i].Tu = sphereN.TexCoords[i].X;
                sphere[i].Tv = sphereN.TexCoords[i].Y;
            }
            vBuffer.Unlock();
            
            numVerts = sphereN.Positions.Length;*/

            //texture = TextureLoader.FromStream(device, NoiseTextureBuilder.BuildSphericalTexture());
            //FromFile(device, "c:/earth.bmp");
        }

//        static double TWOPI = Math.PI * 2;
//        static double PID2 = Math.PI / 2;

//        void CreateSphere(Vector3 c, double r, int n, CustomVertex.PositionNormalTextured[] points)
//        {
//            int i, j;
//            double theta1, theta2, theta3;
//            Vector3 e, p;
//
//            if (r < 0)
//                r = -r;
//            if (n < 0)
//                n = -n;
//
//            int vIdx = 0;
//            for (j = 0; j < n / 2; j++)
//            {
//                theta1 = j * TWOPI / n - PID2;
//                theta2 = (j + 1) * TWOPI / n - PID2;
//
//                for (i = 0; i <= n; i++)
//                {
//                    theta3 = i * TWOPI / n;
//
//                    e.X = (float)(Math.Cos(theta2) * Math.Cos(theta3));
//                    e.Y = (float)Math.Sin(theta2);
//                    e.Z = (float)(Math.Cos(theta2) * Math.Sin(theta3));
//                    p.X = (float)(c.X + r * e.X);
//                    p.Y = (float)(c.Y + r * e.Y);
//                    p.Z = (float)(c.Z + r * e.Z);
//
//                    points[vIdx].Normal = new Vector3(e.X, e.Y, e.Z);
//                    points[vIdx].Tu = (float)(i / (double)n);
//                    points[vIdx].Tv = (float)(2 * (j + 1) / (double)n);
//
//                    points[vIdx].Position = new Vector3(p.X, p.Y, p.Z);
//                    vIdx++;
//
//                    e.X = (float)(Math.Cos(theta1) * Math.Cos(theta3));
//                    e.Y = (float)Math.Sin(theta1);
//                    e.Z = (float)(Math.Cos(theta1) * Math.Sin(theta3));
//                    p.X = (float)(c.X + r * e.X);
//                    p.Y = (float)(c.Y + r * e.Y);
//                    p.Z = (float)(c.Z + r * e.Z);
//
//                    points[vIdx].Normal = new Vector3(e.X, e.Y, e.Z);
//                    points[vIdx].Tu = (float)(i / (double)n);
//                    points[vIdx].Tv = (float)(2 * j / (double)n);
//
//                    points[vIdx].Position = new Vector3(p.X, p.Y, p.Z);
//                    vIdx++;
//                }
//            }
//        }

//        void CreateSphere2(Vector3 c, double r, int n, CustomVertex.PositionNormalTextured[] points)
//        {
//            double theta1, theta2, theta3;
//            Vector3 e, p;
//
//            if (r < 0)
//                r = -r;
//            if (n < 0)
//                n = -n;
//
//            int vIdx = 0;
//            for (int j = 0; j < n / 2; j++)
//            {
//                theta1 = j * TWOPI / n - PID2;
//                theta2 = (j + 1) * TWOPI / n - PID2;
//
//                for (int i = 0; i <= n; i++)
//                {
//                    theta3 = i * TWOPI / n;
//
//                    if (i > 0)
//                    {
//                        e.X = (float)(Math.Cos(theta2) * Math.Cos(theta3));
//                        e.Y = (float)Math.Sin(theta2);
//                        e.Z = (float)(Math.Cos(theta2) * Math.Sin(theta3));
//                        p.X = (float)(c.X + r * e.X);
//                        p.Y = (float)(c.Y + r * e.Y);
//                        p.Z = (float)(c.Z + r * e.Z);
//
//                        // end of T1
//                        points[vIdx].Normal = new Vector3(e.X, e.Y, e.Z);
//                        points[vIdx].Tu = (float)(i / (double)n);
//                        points[vIdx].Tv = (float)(2 * (j + 1) / (double)n);
//                        points[vIdx].Position = new Vector3(p.X, p.Y, p.Z);
//                        vIdx++;
//
//                        e.X = (float)(Math.Cos(theta1) * Math.Cos(theta3));
//                        e.Y = (float)Math.Sin(theta1);
//                        e.Z = (float)(Math.Cos(theta1) * Math.Sin(theta3));
//                        p.X = (float)(c.X + r * e.X);
//                        p.Y = (float)(c.Y + r * e.Y);
//                        p.Z = (float)(c.Z + r * e.Z);
//
//                        // T2
//                        points[vIdx].Normal = points[vIdx - 1].Normal;
//                        points[vIdx].Tu = points[vIdx - 1].Tu;
//                        points[vIdx].Tv = points[vIdx - 1].Tv;
//                        points[vIdx].Position = points[vIdx - 1].Position;
//                        vIdx++;
//
//                        points[vIdx].Normal = new Vector3(e.X, e.Y, e.Z);
//                        points[vIdx].Tu = (float)(i / (double)n);
//                        points[vIdx].Tv = (float)(2 * j / (double)n);
//                        points[vIdx].Position = new Vector3(p.X, p.Y, p.Z);
//                        vIdx++;
//
//                        points[vIdx].Normal = points[vIdx - 5].Normal;
//                        points[vIdx].Tu = points[vIdx - 5].Tu;
//                        points[vIdx].Tv = points[vIdx - 5].Tv;
//                        points[vIdx].Position = points[vIdx - 5].Position;
//                        vIdx++;
//
//                        // start of T1
//                        if (i < n /*- 1*/)
//                        {
//                            points[vIdx].Normal = points[vIdx - 2].Normal;
//                            points[vIdx].Tu = points[vIdx - 2].Tu;
//                            points[vIdx].Tv = points[vIdx - 2].Tv;
//                            points[vIdx].Position = points[vIdx - 2].Position;
//                            vIdx++;
//
//                            points[vIdx].Normal = points[vIdx - 4].Normal;
//                            points[vIdx].Tu = points[vIdx - 4].Tu;
//                            points[vIdx].Tv = points[vIdx - 4].Tv;
//                            points[vIdx].Position = points[vIdx - 4].Position;
//                            vIdx++;
//                        }
//                    }
//                    else
//                    {
//                        e.X = (float)(Math.Cos(theta1) * Math.Cos(theta3));
//                        e.Y = (float)Math.Sin(theta1);
//                        e.Z = (float)(Math.Cos(theta1) * Math.Sin(theta3));
//                        p.X = (float)(c.X + r * e.X);
//                        p.Y = (float)(c.Y + r * e.Y);
//                        p.Z = (float)(c.Z + r * e.Z);
//
//                        points[vIdx].Normal = new Vector3(e.X, e.Y, e.Z);
//                        points[vIdx].Tu = (float)(i / (double)n);
//                        points[vIdx].Tv = (float)(2 * j / (double)n);
//                        points[vIdx].Position = new Vector3(p.X, p.Y, p.Z);
//                        vIdx++;
//
//                        e.X = (float)(Math.Cos(theta2) * Math.Cos(theta3));
//                        e.Y = (float)Math.Sin(theta2);
//                        e.Z = (float)(Math.Cos(theta2) * Math.Sin(theta3));
//                        p.X = (float)(c.X + r * e.X);
//                        p.Y = (float)(c.Y + r * e.Y);
//                        p.Z = (float)(c.Z + r * e.Z);
//
//                        points[vIdx].Normal = new Vector3(e.X, e.Y, e.Z);
//                        points[vIdx].Tu = (float)(i / (double)n);
//                        points[vIdx].Tv = (float)(2 * (j + 1) / (double)n);
//                        points[vIdx].Position = new Vector3(p.X, p.Y, p.Z);
//                        vIdx++;
//                    }
//                }
//            }
//        }

//        private void panel1_Paint(object sender, PaintEventArgs e)
//        {
//            RenderDevice();
//        }

//        private void RenderDevice()
//        {
//            if (device != null)
//            {
//                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Blue, 1.0f, 0);
//
//                device.Transform.Projection = Matrix.PerspectiveFovLH(45f, (float)this.panel1.Width / (float)this.panel1.Height, 0.1f, 100f);
//                device.Transform.View = Matrix.LookAtLH(new Vector3(4, -4, 1), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
//                device.Transform.World = Matrix.RotationY(xRot) * Matrix.RotationX(yRot);
//
//                device.BeginScene();
//                device.SetTexture(0, texture);
//                device.RenderState.FillMode = FillMode.WireFrame;
//                //device.RenderState.CullMode = Cull.None;
//
//                device.VertexFormat = CustomVertex.PositionNormalTextured.Format;
//                device.SetStreamSource(0, vBuffer, 0);
//                /*for (int i = 0; i < detail / 2; i++)
//                {
//                    device.DrawPrimitives(PrimitiveType.TriangleStrip, i * ((detail * 2) + 2), detail * 2);
//                }*/
//                device.DrawPrimitives(PrimitiveType.TriangleList, 0, numVerts / 3);
//
//                device.EndScene();
//                device.Present();
//            }
//        }

//        private void panel1_MouseMove(object sender, MouseEventArgs e)
//        {
//            xRot = (float)(((float)e.X / (float)panel1.Width) * Math.PI * 2f);
//            yRot = (float)(((float)e.Y / (float)panel1.Height) * Math.PI * 2f);
//            RenderDevice();
//        }
    }
}