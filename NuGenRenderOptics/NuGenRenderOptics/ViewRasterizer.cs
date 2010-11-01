using System;
using System.Drawing;
using System.Drawing.Imaging;
using Genetibase.NuGenRenderOptics.MDX1.HeightFields;
using NuGenRenderOptics;

namespace Genetibase.NuGenRenderOptics.MDX1.Rasterization
{
    struct ARGB_32Bit
    {
        public byte B, G, R, A;
    }

    class FrameData
    {
        public readonly float[] Data;
        public readonly float[] AALayer;
        public readonly int Width;
        public readonly int Height;
        public readonly Color Bg;

        public FrameData(int width, int height, bool aa, Color bg)
        {
            Data = new float[width * height];
            AALayer = aa ? new float[(width + 2) * (height + 2)] : null;
            Width = width;
            Height = height;
            Bg = bg;
        }

        public void ApplyAAFilter()
        {
            if (AALayer == null)
                return;
            // NOTE: just do standard box sample for now - needs to be moved
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    float s1 = AALayer[y * (Height + 2) + x];
                    float s2 = AALayer[y * (Height + 2) + x + 1];
                    float s3 = AALayer[(y + 1) * (Height + 2) + x];
                    float s4 = AALayer[(y + 1) * (Height + 2) + x + 1];

                    byte aa1 = (byte)((int)s1 >> 24);
                    byte aa2 = (byte)((int)s2 >> 24);
                    byte aa3 = (byte)((int)s3 >> 24);
                    byte aa4 = (byte)((int)s4 >> 24);
                    byte aa0 = (byte)((uint)Data[y * Height + x] >> 24);

                    /*float aa1f = aa1 / 255f;
                    float aa2f = aa2 / 255f;
                    float aa3f = aa3 / 255f;
                    float aa4f = aa4 / 255f;
                    float aa0f = aa0 / 255f;

                    byte r = (byte)(((byte)(((uint)s1 & 0x000000FF) * aa1f) + ((byte)((uint)s2 & 0x000000FF) * aa2f) +
                                     (byte)(((uint)s3 & 0x000000FF) * aa3f) + ((byte)((uint)s4 & 0x000000FF) * aa4f) +
                                     (byte)((uint)Data[y * Height + x] & 0x000000FF) * aa0f) / 5);
                    byte g = (byte)(((byte)(((int)s1 >> 8) * aa1f) + (byte)(((int)s2 >> 8) * aa2f) +
                                     (byte)(((int)s3 >> 8) * aa3f) + (byte)(((int)s4 >> 8) * aa4f) +
                                     (byte)(((int)Data[y * Height + x] >> 8) * aa0f)) / 5);
                    byte b = (byte)(((byte)(((int)s1 >> 16) * aa1f) + (byte)(((int)s2 >> 16) * aa2f) +
                                     (byte)(((int)s3 >> 16) * aa3f) + (byte)(((int)s4 >> 16) * aa4f) +
                                     (byte)(((int)Data[y * Height + x] >> 16)) * aa0f) / 5);
                    byte a = (byte)((aa1 + aa2 + aa3 + aa4 + aa0) / 5);*/

                    // FIXME: Specular highlights inverted for reason
                    byte r = (byte)(((byte)((uint)s1 & 0x000000FF) + ((byte)(uint)s2 & 0x000000FF) +
                                     (byte)((uint)s3 & 0x000000FF) + ((byte)(uint)s4 & 0x000000FF) +
                                     (byte)((uint)Data[y * Height + x] & 0x000000FF)) / 5);
                    byte g = (byte)(((byte)((int)s1 >> 8) + (byte)((int)s2 >> 8) +
                                     (byte)((int)s3 >> 8) + (byte)((int)s4 >> 8) +
                                     (byte)((int)Data[y * Height + x] >> 8)) / 5);
                    byte b = (byte)(((byte)((int)s1 >> 16) + (byte)((int)s2 >> 16) +
                                     (byte)((int)s3 >> 16) + (byte)((int)s4 >> 16) +
                                     (byte)((int)Data[y * Height + x] >> 16)) / 5);
                    byte a = (byte)((aa1 + aa2 + aa3 + aa4 + aa0) / 5);

                    // TODO: Alpha blending correctly with RGB?
                    // encode back into data
                    Data[(y * Height) + x] = (r | g << 8 | b << 16 | a << 24);
                }
            }
        }
    }

    class CameraView
    {
        public Vector3D Centre;
        public Vector3D Direction;
        public Vector3D[] Corners;
        public Vector3D XUV, YUV;
        public PlaneD[] Frustum;
        public Rectangle Area;
        public Vector2D ViewArea;
        public Vector2D ProjectionScale;
    }

    public class ViewRasterizer
    {
        Rectangle viewArea;

        RayDispatcher rayDispatch;

        FrameData buffer;

        string stats;

        Image texture;

        public ViewRasterizer()
        {
            rayDispatch = new RayDispatcher(4);
        }

        public Rectangle ViewArea
        {
            get
            {
                return viewArea;
            }
            set
            {
                viewArea = value;
            }
        }

        public int Camera
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public void CollectData()
        {
            //Rectangle3D rect = new Rectangle3D(new Vector3D(-5, 0, -5), new Vector3D(5, 0, 5), new PlaneD(new Vector3D(0, 1, 0), 0));
            //Vector3D iPoint;
            //bool result = rect.IntersectWithLine(new Vector3D(5, 5, 0), new Vector3D(0, -1, 0), out iPoint);

            // create frame buffer to store data
            buffer = new FrameData(viewArea.Width, viewArea.Height, false, Color.Black);

            CameraView cView = new CameraView();
            cView.Centre = new Vector3D(3, 5, 3);
            cView.Direction = cView.Centre - new Vector3D(0, 0, 0);
            cView.Direction.Normalize();
            cView.XUV = Vector3D.Cross(cView.Direction, Vector3D.Up);
            cView.XUV.Normalize();
            cView.YUV = Vector3D.Cross(cView.XUV, cView.Direction);
            cView.YUV.Normalize();
            cView.ViewArea = new Vector2D(1, 1);
            Vector3D halfX = cView.XUV * 0.5 * cView.ViewArea.X;
            Vector3D halfY = cView.YUV * 0.5 * cView.ViewArea.Y;
            cView.Corners = new Vector3D[] { cView.Centre - halfX + halfY, // top left
                                             cView.Centre + halfX + halfY, // top right
                                             cView.Centre - halfX - halfY, // bottom left
                                             cView.Centre + halfX - halfY // bottom right
                                           };
            cView.Frustum = new PlaneD[] { PlaneD.FromPointNormal(cView.Corners[0], -cView.XUV),
                                           PlaneD.FromPointNormal(cView.Corners[1], cView.XUV),
                                           PlaneD.FromPointNormal(cView.Corners[2], cView.YUV),
                                           PlaneD.FromPointNormal(cView.Corners[3], -cView.YUV)
                                         };
            cView.Area = viewArea;
            cView.ProjectionScale = new Vector2D(0.2, 0.2);

            texture = Bitmap.FromFile("c:/checkers2.bmp");
            //Bitmap texture2 = (Bitmap)Bitmap.FromFile("c:/newheightmap.jpg");

            // setup temp scene
            SimpleSceneManager scene = new SimpleSceneManager(new RGBA_D(0.2, 0.2, 0.2, 1));

            /*scene.AddObject(new TriangleGroupSceneObject(new Vector3D(0, 0, 2),
                                                         new MaterialShader(new RGBA_D(1, 1, 1, 1),
                                                                            15, 0.1, 0, 0, 1),
                                                         1.1));*/
            /*scene.AddObject(new SphereSceneObject(new Vector3D(0, 0, 0),
                                                  new MaterialShader(new RGBA_D(0, 0, 1, 1),
                                                                     15, 0, 0, 0, 1),
                                                  1));*/
            /*scene.AddObject(new SphereSceneObject(new Vector3D(0, 0, 0),
                                                  new MaterialShader(new RGBA_D(1, 0, 0, 1),
                                                                     15, 1, 0, 0, 1),
                                                  1));
            scene.AddObject(new SphereSceneObject(new Vector3D(-3, -1, -3),
                                                  new MaterialShader(new RGBA_D(0.2, 1, 0.1, 1),
                                                                     15, 0, 0, 0, 1, texture2),
                                                  1));
            scene.AddObject(new SphereSceneObject(new Vector3D(2, 0, 0),
                                                  new MaterialShader(new RGBA_D(1, 1, 0, 1),
                                                                     10, 0.1, 0, 0, 1),
                                                  1));
            scene.AddObject(new SphereSceneObject(new Vector3D(0.5, 1, 0),
                                                  new MaterialShader(new RGBA_D(1, 1, 1, 1),
                                                                     5, 0, 0, 0, 1, texture),
                                                  0.8));
            
            scene.AddObject(new PlaneSceneObject(new Vector3D(0, -5, 0), new Vector3D(0, 1, 0),
                                                 new MaterialShader(new RGBA_D(0.1, 0.1, 0.1, 1.0),
                                                                    5, 0, 0, 0, 1), -1));
            */
            scene.AddObject(new HeightFieldObject(new Vector3D(),
                                                  new MaterialShader(new RGBA_D(1, 1, 1, 1),
                                                                     5, 0, 0, 0, 1, texture),
                                                  new Vector2D(2, 2), (Bitmap)texture));
            /*
            IOpticalSceneObject obj;
            Vector3D iPos;
            double iDist;
            uint subIdx;
            scene.GetFirstIntersection(new Vector3D(10, 0.5, 0), new Vector3D(1, 0, 0), 100, out obj, out iPos, out iDist, out subIdx);
            */
            // collect data
            DateTime start = DateTime.Now;
            RayGroup initialGroup = new RayGroup(true, 100,
                                                 cView.Area, scene, null, buffer, cView);
            rayDispatch.DispatchRayGroupReq(initialGroup);

            rayDispatch.WaitForCompletion();
            DateTime end = DateTime.Now;
            TimeSpan time = end - start;

            string aaFilter = buffer.AALayer != null ? "true[5_point_star]" : "false"; 
            stats = string.Format("threads={0} rays={1} pixels={2} time={3}ms AA={4}", rayDispatch.ProcessedThreads,
                                                                                rayDispatch.ProcessedRays,
                                                                                viewArea.Width * viewArea.Height,
                                                                                time.TotalMilliseconds,
                                                                                aaFilter);
        }

        public Bitmap Rasterize()
        {
            if (buffer.AALayer != null)
                buffer.ApplyAAFilter();

            Bitmap bitmap = new Bitmap(buffer.Width, buffer.Height);
            BitmapData data = bitmap.LockBits(viewArea, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                ARGB_32Bit* pixels = (ARGB_32Bit*)data.Scan0.ToPointer();
                int dataIdx = 0;
                for (int y = 0; y < buffer.Height; y++)
                {
                    for (int x = 0; x < buffer.Width; x++)
                    {
                        float value = buffer.Data[dataIdx++];
                        pixels->B = (byte)((int)value >> 16);
                        pixels->G = (byte)((int)value >> 8);
                        pixels->R = (byte)((int)value & 0xFF);
                        pixels->A = (byte)((int)value >> 24);

                        // add bg if alpha < 255
                        if (pixels->A == 0)
                        {
                            pixels->B = buffer.Bg.B;
                            pixels->G = buffer.Bg.G;
                            pixels->R = buffer.Bg.R;
                            pixels->A = buffer.Bg.A;
                        }
                        else if (pixels->A != 255)
                        {
                            double alpha = (double)pixels->A / 255;
                            double ra = 1 - alpha;
                            pixels->B = (byte)((buffer.Bg.B * ra) + (pixels->B * alpha));
                            pixels->G = (byte)((buffer.Bg.G * ra) + (pixels->G * alpha));
                            pixels->R = (byte)((buffer.Bg.R * ra) + (pixels->R * alpha));
                            pixels->A = buffer.Bg.A; // maybe add alpha also
                        }

                        pixels++;
                    }
                }
            }
            bitmap.UnlockBits(data);

            Graphics g = Graphics.FromImage(bitmap);
            g.DrawString(stats, new Font("Tahoma", 6), Brushes.White, 0, 0);

            int size = buffer.Width / 4;
            g.DrawImage(texture, buffer.Width - size, buffer.Height - size, size, size);
            // put alpha back into text
            //SizeF size = g.MeasureString(stats, new Font("Tahona", 6));
            //data = bitmap.LockBits(new Rectangle(0, 0, (int)size.Width, (int)size.Height),
            //                       ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            //unsafe
            //{
            //    for (int y = 0; y < size.Height; y++)
            //    {
            //        ARGB_32Bit* pixels = (ARGB_32Bit*)data.Scan0.ToPointer() + (y * viewArea.Width);
            //        for (int x = 0; x < size.Width; x++)
            //        {
            //            if (pixels->A == 0/* && (pixels->R != 0 || pixels->G != 0 || pixels->B != 0)*/)
            //                pixels->A = 255;
            //            pixels++;
            //        }
            //    }
            //}
            //bitmap.UnlockBits(data);

            //bitmap.Save("c:/output-HM.png", ImageFormat.Png);
            return bitmap;
        }

//        public Bitmap Test()
//        {
//            Bitmap bitmap = new Bitmap(128, 128);
//            Graphics g = Graphics.FromImage(bitmap);
//
//            Vector3D dir = new Vector3D(1, 0, 0);
//            Vector3D cross = new Vector3D(0, 1, 0);
//            float angle = 0.2f;
//
//            int yPos = 0;
//            float angleScale = -1;
//            for (int i = 0; i < 10; i++)
//            {
//                Vector3D lineDir = Vector3D.Normalize(dir + (cross * (angle * angleScale)));
//                g.DrawLine(Pens.Red, 0, yPos, (int)(lineDir.X * 100), (int)(yPos + (lineDir.Y * 100)));
//                yPos += 10;
//                angleScale += 0.1f;
//            }
//
//            g.Dispose();
//            return bitmap;
//        }
    }
}