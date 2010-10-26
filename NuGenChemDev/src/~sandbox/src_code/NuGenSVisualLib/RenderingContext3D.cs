using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace NuGenSVisualLib.Rendering.ThreeD
{
    public abstract class RenderingContext3D : RenderingContext
    {
        protected RenderingView3D view3D;
        protected RenderingSource3D renderSource3D;

        public override IRenderingView View
        {
            get { return view; }
            set
            {
                if (value is RenderingView3D)
                    view = view3D = (RenderingView3D)value;
            }
        }

        public RenderingView3D View3D
        {
            get { return view3D; }
            set
            {
               view = view3D = value;
            }
        }

        public override IRenderingSource RenderSource
        {
            get { return renderSource; }
            set
            {
                if (value is RenderingSource3D)
                    renderSource = renderSource3D = (RenderingSource3D)value;
            }
        }

        public RenderingSource3D RenderSource3D
        {
            get { return renderSource3D; }
            set
            {
                renderSource = renderSource3D = value;
                if (value != null)
                    View3D = RenderingView3D.FromSource(value);
            }
        }
    }
}