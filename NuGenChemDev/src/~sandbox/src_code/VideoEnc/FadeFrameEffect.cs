using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace NuGenVideoEnc.Effects
{
    /// <summary>
    /// Encapsulates a fade effect
    /// </summary>
    class FadeFrameEffect : FrameEffect
    {
        protected uint duration;
        protected Color startClr;
        protected Color endClr;
        protected FadeType fadeType;

        public enum FadeType
        {
            In,
            Out,
            OutIn
        }

        public FadeFrameEffect(FadeType fadeType) : base("Fade") { this.fadeType = fadeType; }

        public override void FramePassthrough(int[] data, int frame, long time)
        {
            // ignore what frame for now

        }

        public override void SetupEffect(int numFrames, int width, int height, int depth)
        {
            
        }
    }
}