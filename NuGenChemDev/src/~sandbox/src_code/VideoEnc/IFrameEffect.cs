using System;
using System.Collections.Generic;
using System.Text;

namespace NuGenVideoEnc.Effects
{
    public interface IFrameEffect
    {
        string Name { get; }
        bool Enabled { get; }
        void FramePassthrough(int[] data, int frame, long time);
        int StartFrameIndex { get; }
        int EndFrameIndex { get; }

        void SetupEffect(int numFrames, int width, int height, int depth);
    }

    abstract class FrameEffect : IFrameEffect
    {
        private string name;

        protected bool enabled;
        protected int startFrameIndex;
        protected int endFrameIndex;

        public FrameEffect(string name) { this.name = name; }

        #region IFrameEffect Members

        public string Name
        {
            get { return name; }
        }

        public bool Enabled
        {
            get { return enabled; }
        }

        public int StartFrameIndex { get { return startFrameIndex; } }
        public int EndFrameIndex { get { return endFrameIndex; } }

        public abstract void FramePassthrough(int[] data, int frame, long time);
        public abstract void SetupEffect(int numFrames, int width, int height, int depth);

        #endregion
    }
}