using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Genetibase.NuGenRenderOptics.MDX1.Rasterization;

namespace NuGenRenderOptics
{
    class RayGroup
    {
        bool divide;
        double maxLength;
        Rectangle area;

        ISceneManager scene;
        FrameData frameData;
        CameraView view;

//        Vector2D extent;

        internal IRayDispatch dispatch;

        public RayGroup(bool divide, double maxLength,
                        Rectangle area, ISceneManager scene, IRayDispatch dispatch,
                        FrameData frameData, CameraView view)
        {
            this.divide = divide;
            this.maxLength = maxLength;
            this.area = area;

            this.scene = scene;
            this.dispatch = dispatch;
            this.frameData = frameData;
            this.view = view;
        }

        #region Properties

        public double MaxLength
        {
            get { return maxLength; }
        }

        public Rectangle Area
        {
            get { return area; }
        }
        #endregion

        public void Trace()
        {
            // project into scene as inital rejection test
            if (scene.TestForContents(view, area, maxLength))
            {
                // sub-divide task area until all 4 are positive for matches
                if (!divide)
                {
                    // no divisions so run the task immediately
                    // just run all rays here for now
                    double xScale = 0.5 / view.Area.Width;//area.Width;
                    double yScale = 0.5 / view.Area.Height;//area.Height;
                    double yIdx = -0.25 + (xScale * 0.5) + (yScale * area.Top);
                    double filterShiftX = xScale * 0.5;
                    double filterShiftY = yScale * 0.5;

                    for (int y = area.Top; y < area.Bottom; y++)
                    {
                        double xIdx = -0.25 + (yScale * 0.5) + (xScale * area.Left);
                        for (int x = area.Left; x < area.Right; x++)
                        {
                            Vector3D offset = ((view.XUV * xIdx) + (view.YUV * yIdx));
                            Vector3D rayDir = offset + view.Direction;
                            rayDir.Normalize();
                            Ray ray = new Ray(view.Centre, rayDir, 1, 0, maxLength, scene);
                            RGBA_D value = ray.Trace(ray);

                            // do anti-alias pass/pixel
                            // TODO: Do more if on left and/or top edge
                            RGBA_D aaValue = RGBA_D.Empty;
                            if (frameData.AALayer != null)
                            {
                                offset = ((view.XUV * (xIdx + filterShiftX)) + (view.YUV * (yIdx + filterShiftY)));
                                rayDir = offset + view.Direction;
                                rayDir.Normalize();
                                ray = new Ray(view.Centre, rayDir, 1, 0, maxLength, scene);
                                aaValue = ray.Trace(ray);

                                if (aaValue != RGBA_D.Empty)
                                {
                                    int index = ((y + 1) * (frameData.Width + 2)) + (x + 1);
                                    lock (frameData)
                                    {
                                        // pack to bytes
                                        float val = ((byte)aaValue.R | (byte)aaValue.G << 8 | (byte)aaValue.B << 16 | (byte)value.A << 24);
                                        frameData.AALayer[index] = val;
                                    }
                                }
                            }

                            value.Normalize();
                            aaValue.Normalize();

                            if (value != RGBA_D.Empty)
                            {
                                // write to frame buffer
                                int index = (y * frameData.Width) + x;
                                lock (frameData)
                                {
                                    // pack to bytes
                                    float val = ((byte)value.R | (byte)value.G << 8 | (byte)value.B << 16 | (byte)value.A << 24);
                                    frameData.Data[index] = val;
                                }
                            }
                            /*else
                            {
                                int index = (y * frameData.Width) + x;
                                lock (frameData)
                                {
                                    // pack to bytes
                                    float val = ((byte)area.Left /2 | (byte)area.Top /2 << 8 | (byte)0 << 16 | (byte)255 << 24);
                                    frameData.Data[index] = val;
                                }
                            }*/
                            xIdx += xScale;
                        }
                        yIdx += yScale;
                    }

                    // pass AA area on to control

                    // just create group outline for now
                    /*for (int x = area.Left; x < area.Right; x++)
                    {
                        lock (frameData)
                        {
                            // pack to bytes
                            frameData.Data[x] = (255 | (byte)0 << 255 | (byte)0 << 255 | (byte)255 << 24);
                        }
                    }
                    int index2 = view.Area.Width * (area.Height - 1);
                    for (int x = area.Left; x < area.Right; x++)
                    {
                        lock (frameData)
                        {
                            // pack to bytes
                            frameData.Data[index2 + x] = (255 | (byte)0 << 255 | (byte)0 << 255 | (byte)255 << 24);
                        }
                    }*/

                    dispatch.RaysTraced(area.Width * area.Height);
                }
                else
                    SubDivideTask(area, maxLength, 0);
            }
            dispatch.ExecutionComplete();
        }

        private void SubDivideTask(Rectangle area, double length, int level)
        {
            level++;

            // divide in quaters into 2x2
            byte matches = 0;
            int halfWidth = area.Width / 2;
            int halfHeight = area.Height / 2;

            Rectangle tlArea = new Rectangle(area.X, area.Y, halfWidth, halfHeight);
            if (scene.TestForContents(view, tlArea, length))
                matches |= 1;

            Rectangle trArea = new Rectangle(area.X + halfWidth, area.Y, halfWidth, halfHeight);
            if (scene.TestForContents(view, trArea, length))
                matches |= 2;

            Rectangle blArea = new Rectangle(area.X, area.Y + halfHeight, halfWidth, halfHeight);
            if (scene.TestForContents(view, blArea, length))
                matches |= 4;

            Rectangle brArea = new Rectangle(area.X + halfWidth, area.Y + halfHeight, halfWidth, halfHeight);
            if (scene.TestForContents(view, brArea, length))
                matches |= 8;

            if (level == 2)
            {
                // max depth so no more division
                if ((matches & 1) > 0)
                    dispatch.QueueDispatch(new RayGroup(false, length, tlArea, scene, null, frameData, view));
                if ((matches & 2) > 0)
                    dispatch.QueueDispatch(new RayGroup(false, length, trArea, scene, null, frameData, view));
                if ((matches & 4) > 0)
                    dispatch.QueueDispatch(new RayGroup(false, length, blArea, scene, null, frameData, view));
                if ((matches & 8) > 0)
                    dispatch.QueueDispatch(new RayGroup(false, length, brArea, scene, null, frameData, view));
            }
            else
            {
                // sub-divide all matches
                if ((matches & 1) > 0)
                    SubDivideTask(tlArea, length, level);
                if ((matches & 2) > 0)
                    SubDivideTask(trArea, length, level);
                if ((matches & 4) > 0)
                    SubDivideTask(blArea, length, level);
                if ((matches & 8) > 0)
                    SubDivideTask(brArea, length, level);
            }
        }
    }
}
