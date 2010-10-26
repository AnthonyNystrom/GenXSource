using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace NuGenSVisualLib.Rendering.TwoD
{
    public abstract class RenderingContext2D : RenderingContext
    {
        public abstract void Render(Graphics g);

        public override void Render(object param)
        {
            Render((Graphics)param);
        }
    }
}
