using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Microsoft.DirectX;

namespace NuGenSVisualLib.Recording
{
    /// <summary>
    /// Encapsulates a movement in 3d
    /// </summary>
    abstract class Movement3D
    {
        protected long timeIndex;
        [XmlAttribute("tIdx")]
        public long TimeIndex { get { return timeIndex; } set { timeIndex = value; } }
        public Movement3D(long timeIndex) { this.timeIndex = timeIndex; }
    }

    [XmlRoot("rot")]
    class Rotation3D : Movement3D
    {
        float x, y, z;
        [XmlAttribute("x")]
        public float X { get { return x; } set { x = value; } }
        [XmlAttribute("y")]
        public float Y { get { return y; } set { y = value; } }
        [XmlAttribute("z")]
        public float Z { get { return z; } set { z = value; } }
        public Rotation3D(long timeIndex, float x, float y, float z) : base(timeIndex) { this.x = x; this.y = y; this.z = z; }
    }

    //[XmlRoot("zoom")]
    //class Zoom3D : Movement3D
    //{
    //    float zoom;
    //    public float Zoom { get { return zoom; } }
    //}

    /// <summary>
    /// Encapsulates a movement recording of a view
    /// </summary>
    [XmlRoot("viewRecording")]
    class ViewRecording
    {
        TimeSpan duration;
        List<Movement3D> movements;
        string file;
        bool molecule;

        public ViewRecording() { }
        public ViewRecording(ViewRecorder recorder)
        {
            this.movements = recorder.movements;
            this.duration = TimeSpan.FromTicks(DateTime.Now.Ticks - recorder.startTick);
        }
        
        [XmlElement("duration")]
        public TimeSpan Duration
        {
            get { return duration; }
        }

        [XmlArray("movements")]
        [XmlArrayItem("data")]
        List<Movement3D> Movements
        {
            get { return movements; }
        }

        public class ViewRecorder
        {
            internal long startTick;
            internal Matrix startRotation;
            internal List<Movement3D> movements;
            float rx, ry, rz;

            public ViewRecorder()
            {
                startTick = DateTime.Now.Ticks;
                movements = new List<Movement3D>();
            }

            public void RecordRotation(float x, float y, float z)
            {
                long time = startTick - DateTime.Now.Ticks;

                float dx = x - rx;
                float dy = y - ry;
                float dz = z - rz;

                rx = x;
                ry = y;
                rz = z;

                movements.Add(new Rotation3D(time, dx, dy, dz));
            }

            public void RecordZoom(float w)
            {
            }
        }
    }
}