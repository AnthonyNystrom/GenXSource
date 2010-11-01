using System;
using System.Drawing;

namespace Genetibase.RasterDatabase
{
//    public struct Area : Rectangle
//    {
//        public Point position;
//        public Size dimensions;
//
//        /// <summary>
//        /// Initializes a new instance of the Area structure.
//        /// </summary>
//        /// <param name="position"></param>
//        /// <param name="dimensions"></param>
//        public Area(Point position, Size dimensions)
//        {
//            this.position = position;
//            this.dimensions = dimensions;
//        }
//
//        public bool IsInside(Area area)
//        {
//            return (area.position.X > position.X &&
//                area.position.Y > position.Y &&
//                area.position.X + area.dimensions.Width < position.X + dimensions.Width &&
//                area.position.Y + area.dimensions.Height < position.Y + dimensions.Height);
//        }
//
//        public void ExpandToContain(Area area)
//        {
//            if (area.position.X < position.X)
//                position.X = area.position.X;
//            if (area.position.Y < position.Y)
//                position.Y = area.position.Y;
//            if (area.position.X + area.dimensions.Width > position.X + dimensions.Width)
//                dimensions.Width = area.position.X + area.dimensions.Width - position.X;
//            if (area.position.Y + area.dimensions.Height > position.Y + dimensions.Height)
//                dimensions.Height = area.position.Y + area.dimensions.Height - position.Y;
//        }
//    }

    public abstract class DataArea
    {
        protected Rectangle area;
        RectangleF texCoords;
        protected Size dataSize;
        protected float minDataValue, maxDataValue, avrDataValue;

        /// <summary>
        /// Initializes a new instance of the DataArea class.
        /// </summary>
        /// <param name="area"></param>
        /// <param name="texCoords"></param>
        public DataArea(Rectangle area, RectangleF texCoords)
        {
            this.area = area;
            this.texCoords = texCoords;
        }

        #region Properties

        public Rectangle Area
        {
            get { return area; }
        }

        public RectangleF TexCoords
        {
            get { return texCoords; }
        }

        public abstract object Data
        {
            get;
        }

        public abstract ValueType this[int x, int y]
        {
            get;
        }

        public float MinDataValue
        {
            get { return minDataValue; }
        }

        public float MaxDataValue
        {
            get { return maxDataValue; }
        }

        public float AverageDataValue
        {
            get { return avrDataValue; }
        }
        
        public Size DataSize
        {
            get { return dataSize; }
        }
        #endregion

        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType())
                return base.Equals(obj);

            DataArea rect = (DataArea)obj;
            return rect.area == area && rect.texCoords == texCoords;
        }

        public abstract void CalculateStats();
//        public static readonly DataArea Empty = new DataArea(Rectangle.Empty, RectangleF.Empty, null);
    }

    public class ByteArea : DataArea
    {
        readonly byte[] data;

        public ByteArea(Rectangle area, RectangleF texCoords, byte[] data, Size dataSize)
            : base(area, texCoords)
        {
            this.data = data;
            this.dataSize = dataSize;
        }

        public override object Data
        {
            get { return data; }
        }

        public override ValueType this[int x, int y]
        {
            get
            {
                // ignore tex coords for now
                return data[(y * /*Area.Width*/dataSize.Width) + x];
            }
        }

        public override void CalculateStats()
        {
            minDataValue = float.MaxValue;
            maxDataValue = float.MinValue;
            avrDataValue = 0;
            for (int y = area.Top; y < area.Bottom; y++)
            {
                for (int x = area.Left; x < area.Right; x++)
                {
                    byte value = (byte)this[x, y];
                    if (value < minDataValue)
                        minDataValue = value;
                    if (value > maxDataValue)
                        maxDataValue = value;
                    avrDataValue += value;
                }
            }
            avrDataValue /= area.Width * area.Height;
        }
    }

    public class FloatArea : DataArea
    {
        readonly float[] data;

        public FloatArea(Rectangle area, RectangleF texCoords, float[] data, Size dataSize)
            : base(area, texCoords)
        {
            this.data = data;
            this.dataSize = dataSize;
        }

        public override object Data
        {
            get { return data; }
        }

        public override ValueType this[int x, int y]
        {
            get
            {
                // ignore tex coords for now
                return data[(y * /*Area.Width*/dataSize.Width) + x];
            }
        }

        public override void CalculateStats()
        {
            minDataValue = float.MaxValue;
            maxDataValue = float.MinValue;
            avrDataValue = 0;
            for (int y = area.Top; y < area.Bottom; y++)
            {
                for (int x = area.Left; x < area.Right; x++)
                {
                    float value = (float)this[x, y];
                    if (value < minDataValue)
                        minDataValue = value;
                    if (value > maxDataValue)
                        maxDataValue = value;
                    avrDataValue += value;
                }
            }
            avrDataValue /= area.Width * area.Height;
        }
    }

}