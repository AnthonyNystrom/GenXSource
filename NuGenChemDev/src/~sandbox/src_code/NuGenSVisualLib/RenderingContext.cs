using System;
using System.Collections.Generic;
using System.Text;
using NuGenSVisualLib.Rendering;
using System.Windows.Forms;
using System.Drawing;

namespace NuGenSVisualLib.Rendering
{
    /// <summary>
    /// Encapsulates a rendering platform
    /// </summary>
    public abstract class RenderingContext : IRenderingContext
    {
        protected Control targetRenderArea;
        protected IRenderingSource renderSource;
        protected IRenderingView view;
        protected Color background;

        #region IRenderingContext Members

        public Control TargetRenderArea
        {
            get
            {
                return targetRenderArea;
            }
            set
            {
                targetRenderArea = value;
            }
        }

        public virtual IRenderingSource RenderSource
        {
            get
            {
                return renderSource;
            }
            set
            {
                renderSource = value;
            }
        }

        public virtual IRenderingView View
        {
            get
            {
                return view;
            }
            set
            {
                view = value;
            }
        }

        public abstract void Render(Object param);
        public abstract void Dispose();

        public Color BackColor
        {
            get
            {
                return background;
            }
            set
            {
                background = value;
            }
        }

        public abstract void OnResize(int width, int height);

        #endregion
    }
}
