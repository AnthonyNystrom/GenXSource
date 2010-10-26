using System;
using System.Drawing;

namespace Genetibase.VisUI.UI
{
    public interface ILayer : IDisposable
    {
        Point Position { get; }
        Size Dimensions { get; }
        void Draw();
        bool Enabled { get; set; }
        bool Visible { get; set; }
        void Resize(int width, int height);
    }
}