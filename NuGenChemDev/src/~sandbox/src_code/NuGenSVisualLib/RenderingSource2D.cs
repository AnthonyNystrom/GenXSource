using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace NuGenSVisualLib.Rendering.TwoD
{
    public class Bounds2D
    {
        public Vector2 min, max;
    }

    public abstract class RenderingSource2D : NuGenSVisualLib.Rendering.IRenderingSource
    {
        protected Bounds2D bounds;
        protected Vector2 origin;

        public Bounds2D Bounds
        {
            get
            {
                return bounds;
            }
            set
            {
                bounds = value;
            }
        }

        public Vector2 Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
            }
        }

        #region IRenderingSource Members

        public abstract void SourceModified();

        #endregion
    }
}
