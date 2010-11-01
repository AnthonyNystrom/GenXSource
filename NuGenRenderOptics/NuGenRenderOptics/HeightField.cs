using System;
using System.Drawing;
using System.Drawing.Imaging;
using Genetibase.NuGenRenderOptics.MDX1.Rasterization;
using NuGenRenderOptics;

namespace Genetibase.NuGenRenderOptics.MDX1.HeightFields
{
    class HeightField
    {
        float[] values;
        int width, height;

        public HeightField(int width, int height)
        {
            values = new float[width * height];
            this.width = width;
            this.height = height;
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public float Sample(float x, float y)
        {
            return SampleActual((int)(x * width), (int)(y * height));
        }

        public float Sample(double x, double y)
        {
            return SampleActual((int)(x * width), (int)(y * height));
        }

        public float Sample(Vector2D coord)
        {
            if (coord.X > 1)
                coord.X = 1;
            else if (coord.X < 0)
                coord.X = 0;
            if (coord.Y > 1)
                coord.Y = 1;
            else if (coord.Y < 0)
                coord.Y = 0;

            return SampleActual((int)(coord.X * (width - 1)), (int)(coord.Y * (height - 1)));
        }

        public float SampleShiftX(double x, double y, int shift)
        {
            int xPos = (int)(x * (width - 1)) + shift;
            if (xPos >= width || xPos < 0)
                return SampleActual((int)(x * (width - 1)), (int)(y * (height - 1)));
            return SampleActual(xPos, (int)(y * (height - 1)));
        }

        public float SampleShiftY(double x, double y, int shift)
        {
            int yPos = (int)(y * (height - 1)) + shift;
            if (yPos >= height || yPos < 0)
                return SampleActual((int)(x * (width - 1)), (int)(y * (height - 1)));
            return SampleActual((int)(x * (width - 1)), yPos);
        }

        public float SampleActual(int x, int y)
        {
            // straight sample
            return values[(y * height) + x];
        }

        public float SampleActualByX(float x, int y)
        {
            int xa = (int)x;
            float xr = x - xa;

            // take 2 samples on x-axis
            int yIndex = y * height;
            float v1 = values[yIndex + xa];
            float v2 = values[yIndex + xa + 1];

            return v1 + ((v2 - v1) * xr);
        }

        public float SampleActualByY(int x, float y)
        {
            int ya = (int)y;
            float yr = y - ya;

            // take 2 samples on y-axis
            float v1 = values[ya * height + x];
            float v2 = values[(ya + 1) * height + x];

            return v1 + ((v2 - v1) * yr);
        }

        public static HeightField FromBitmap(Bitmap bitmap)
        {
            HeightField hField = new HeightField(bitmap.Width, bitmap.Height);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                ARGB_32Bit* pixels = (ARGB_32Bit*)data.Scan0.ToPointer();
                int dataIdx = 0;
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        hField.values[dataIdx++] = (pixels->R + pixels->G + pixels->B) / 765f;
                        pixels++;
                    }
                }
            }

            bitmap.UnlockBits(data);
            return hField;
        }

        public bool SampleViaRay(Vector3D start, double startDist, Vector3D dir,
                                 Vector3D end, double endDist,
                                 out Vector3D iPos, out double iDist)
        {
            // determine start pos in bitmap and dir scale
            Vector3D pos = start * (width - 1);
            Vector3D endPos = end * (width - 1);
            double lineScale = (endDist - startDist) / width;
            Vector3D scale = (endPos - pos) * (1f / width);

            for (int i = 0; i < width; i++)
            {
                // sample
                float value = SampleActual((int)pos.X, (int)pos.Z) * 127;
                if (value > pos.Y)
                {
                    iPos = pos;
                    iDist = i * lineScale;
                    return true;
                }
                pos += scale;
            }
            iPos = Vector3D.Empty;
            iDist = -1;
            return false;
        }
    }

    class HeightFieldQuadTree : HeightFieldQTNode
    {
        float size;

        
    }

    class HeightFieldQTNode
    {
        protected HeightFieldQTNode[] subNodes;
        protected float maxHeight;
        protected PointF position;
        protected float size;

        public bool IntersectRay(Vector3D start, Vector3D end, Vector3D dir)
        {
            // Note: Ignore opt for now
            // clip ray
            if (start.X < position.X)
                start.X = position.X;
            else if (start.X > position.X + size)
                start.X = position.X + size;
            if (start.Y < position.Y)
                start.Y = position.Y;
            else if (start.Y > position.Y + size)
                start.Y = position.Y + size;

            if (end.X < position.X)
                end.X = position.X;
            else if (end.X > position.X + size)
                end.X = position.X + size;
            if (end.Y < position.Y)
                end.Y = position.Y;
            else if (end.Y > position.Y + size)
                end.Y = position.Y + size;

            // look at subnodes
            if (subNodes != null)
            {
                // determine where start and end are
                float hSize = size / 2;
                int startIdx;
                bool startX, startY;
                if (start.X - position.X < hSize)
                {
                    // left
                    startX = false;
                    if (start.Z - position.Y < hSize)
                    {
                        // bottom
                        startIdx = 2;
                        startY = false;
                    }
                    else
                    {
                        // top
                        startIdx = 0;
                        startY = true;
                    }
                }
                else
                {
                    // right
                    startX = true;
                    if (start.Z - position.Y < hSize)
                    {
                        // bottom
                        startIdx = 3;
                        startY = false;
                    }
                    else
                    {
                        // top
                        startIdx = 1;
                        startY = true;
                    }
                }

                int endIdx;
                bool endX, endY;
                if (end.X - position.X < hSize)
                {
                    // left
                    endX = false;
                    if (end.Z - position.Y < hSize)
                    {
                        // bottom
                        endIdx = 2;
                        endY = false;
                    }
                    else
                    {
                        // top
                        endIdx = 0;
                        endY = true;
                    }
                }
                else
                {
                    // right
                    endX = true;
                    if (end.Z - position.Y < hSize)
                    {
                        // bottom
                        endIdx = 3;
                        endY = false;
                    }
                    else
                    {
                        // top
                        endIdx = 1;
                        endY = true;
                    }
                }

                // see what borders are crossed to see how many subNodes need looking at, and in which order
                // look at start first
                bool result;
                if (subNodes[startIdx].maxHeight > start.Y)
                {
                    result = subNodes[startIdx].IntersectRay(start, end, dir);
                    if (result)
                        return true;
                }
                // look at up to 2 others
                if (startIdx != endIdx)
                {
                    if (endX != startX)
                    {
                        // find intersection point

                    }
                }
            }

            return false;
        }
    }
}