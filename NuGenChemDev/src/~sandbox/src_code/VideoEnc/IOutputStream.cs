using System;
using System.Collections.Generic;
using System.Text;
using NuGenVideoEnc.Effects;

namespace NuGenVideoEnc
{
    public interface IOutputStream
    {
        int Width { get; }
        int Height { get; }
        int Depth { get; }
        int FramesPerSecond { get; }

        long CurrentFrameNum { get; }
        TimeSpan EstimatedCurrnetDuration { get; }

        void PutFrame(byte[] data);
        void PutFrame(int[] data);
        void Close(out int totalBytes);
    }

    abstract class OutputStream : IOutputStream
    {
        int width;
        int height;
        int depth;
        int fps;
        long currentFrameNum;
        IFrameEffect[] effects;

        public OutputStream(int width, int height, int depth, int fps)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
            this.fps = fps;
        }

        #region IOutputStream Members

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public int Depth
        {
            get { return depth; }
        }

        public int FramesPerSecond
        {
            get { return fps; }
        }

        public long CurrentFrameNum
        {
            get { return fps; }
        }

        public TimeSpan EstimatedCurrnetDuration
        {
            get
            {
                if (currentFrameNum > 0)
                {
                    double seconds = (float)currentFrameNum / (float)fps;
                    return TimeSpan.FromSeconds(seconds);
                }
                return TimeSpan.Zero;
            }
        }

        public virtual void PutFrame(byte[] data)
        {
        }

        public virtual void PutFrame(int[] data)
        {
            if (effects != null)
            {
                // apply effects
                foreach (IFrameEffect effect in effects)
                {
                    if (effect.Enabled)
                        effect.FramePassthrough(data, 0, 0);
                }
            }
        }

        public abstract void Close(out int totalBytes);

        #endregion
    }
}