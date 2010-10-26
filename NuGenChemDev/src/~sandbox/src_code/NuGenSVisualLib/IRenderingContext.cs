using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace NuGenSVisualLib.Rendering
{
    /// <summary>
    /// Encapsulates a rendering context for the target source to be rendered into the target render area.
    /// </summary>
    public interface IRenderingContext : IDisposable
    {
        Control TargetRenderArea
        {
            get;
            set;
        }

        IRenderingSource RenderSource
        {
            get;
            set;
        }

        IRenderingView View
        {
            get;
            set;
        }

        Color BackColor
        {
            get;
            set;
        }

        void Render(Object param);
        void OnResize(int width, int height);
    }
}
