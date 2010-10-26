using System;
using System.Collections.Generic;
using System.Text;

namespace NuGenSVisualLib.Rendering.TwoD
{
    public class RenderingView2D : IRenderingView
    {
        protected float scale;
        protected int centreX, centreY;

        public float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        public int CentreX
        {
            get
            {
                return centreX;
            }
            set
            {
                centreX = value;
            }
        }

        public int CentreY
        {
            get
            {
                return centreY;
            }
            set
            {
                centreY = value;
            }
        }
    }
}
