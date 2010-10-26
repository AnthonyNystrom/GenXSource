using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Font=System.Drawing.Font;

namespace Genetibase.VisUI.UI
{
    class GUILabel : GUILayerItem
    {
        readonly string text;
        Microsoft.DirectX.Direct3D.Font font;
        readonly Font fontType;
        readonly Color clr;

        public GUILabel(string text, Font font,
                        Color clr, Point position, Size dimensions)
            : base(position, dimensions)
        {
            this.text = text;
            fontType = font;
            this.clr = clr;
        }

        public override void Init(Device device)
        {
            base.Init(device);

            font = new Microsoft.DirectX.Direct3D.Font(device, fontType);
            // update dimensions
            if (dimensions.IsEmpty)
            {
                Rectangle strRect = font.MeasureString(null, text, DrawTextFormat.Left, clr);
                dimensions = new Size(strRect.Width, strRect.Height);
            }
        }

        public override void Draw()
        {
            font.DrawText(null, text, new Rectangle(origin, dimensions), DrawTextFormat.Left, clr);
        }
    }
}