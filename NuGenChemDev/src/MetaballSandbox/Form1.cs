using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NuGenSVisualLib.Maths.Volumes;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MetaballSandbox
{
    public partial class Form1 : Form
    {
        Device device;
        VertexBuffer vBuffer;
        int numPoints;
        int numTris;
        Matrix rot;
        float angle = 0f;

        public Form1()
        {
            InitializeComponent();

            GenericVolumeScene volumeScene = new GenericVolumeScene(new IVolume[] {
                                                        //new BoxVolume(new Vector3(), 50, 50, 50)
                                                        //new BoxVolume(new Vector3(70, 0 , 0), 30, 30, 30)
                                                        //new Metaball(new Vector3(0, 0, 0), 15f),
                                                        //new Metaball(new Vector3(110, 0, 0), 10f),
                                                        //new Metaball(new Vector3(75, 75, 0), 8f)
                                                        });
            //pictureBox1.Image = IsosurfaceGenerator2D.GenerateBitmapSurface(pictureBox1.Width, pictureBox1.Height, 1.0f, 1.0f, volumeScene);

            PresentParameters pParams = new PresentParameters();
            //pParams.EnableAutoDepthStencil = true;
            pParams.Windowed = true;
            pParams.SwapEffect = SwapEffect.Discard;

            device = new Device(0, DeviceType.Hardware, this.pictureBox1, CreateFlags.SoftwareVertexProcessing/*.HardwareVertexProcessing*/, pParams);
            device.RenderState.Lighting = false;

            //Vector3[] points = IsosurfaceGenerator3D.GenerateSimplePointField(volumeScene, new Vector3(), 400, 20);
            Vector3[] points;
            int[] triangles;
            Color[] colors;
            IsosurfaceGenerator3D.GenerateSimpleMesh(volumeScene, new Vector3(), 400, 30, false, out triangles, out points, out colors);

            vBuffer = new VertexBuffer(typeof(CustomVertex.PositionOnly), triangles.Length, device, Usage.None, CustomVertex.PositionOnly.Format, Pool.Managed);
            CustomVertex.PositionOnly[] vertices = (CustomVertex.PositionOnly[])vBuffer.Lock(0, LockFlags.None);
            for (int vIdx = 0; vIdx < triangles.Length; vIdx++)
            {
                vertices[vIdx].Position = points[triangles[vIdx]];
            }
            vBuffer.Unlock();
            numPoints = triangles.Length;
            numTris = numPoints / 3;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void Render()
        {
            device.Clear(ClearFlags.Target, Color.Blue, 0, 1);
            device.BeginScene();

            device.Transform.Projection = Matrix.PerspectiveFovLH(45f, (float)pictureBox1.Width / (float)pictureBox1.Height, 0.1f, 1000f);
            device.Transform.View = Matrix.LookAtLH(new Vector3(300, 300, 300), new Vector3(), new Vector3(0, 1, 0));
            device.Transform.World = rot;

            device.RenderState.Lighting = false;
            device.RenderState.FillMode = FillMode.Solid;
            //device.RenderState.CullMode = Cull.None;

            device.VertexFormat = CustomVertex.PositionOnly.Format;
            device.SetStreamSource(0, vBuffer, 0);
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, numTris);//34
            //device.DrawPrimitives(PrimitiveType.PointList, 0, numPoints);

            //device.RenderState.Lighting = true;
            //device.RenderState.FillMode = FillMode.WireFrame;
            //device.DrawPrimitives(PrimitiveType.TriangleList, 0, numTris);

            device.EndScene();
            device.Present();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            angle += 5;
            if (angle >= 360f)
                angle -= 360f;
            rot = Matrix.RotationZ((float)((angle / 180f) * Math.PI));

            Render();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }
    }
}