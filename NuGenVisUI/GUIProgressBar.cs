using System.Drawing;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.VisUI.UI
{
    class GUIProgressBar : GUILayerItem
    {
        protected CustomVertex.TransformedColored[] clrQuadProgress;
        protected CustomVertex.TransformedColored[] clrQuadEmptyProgress;
        Color clr;
        int progress;

        public GUIProgressBar(Point origin, Size dimensions, Color clr)
            : base(origin, dimensions)
        {
            this.clr = clr;
            progress = 0;
        }

        public override void Init(Device device)
        {
            base.Init(device);

            clrQuadProgress = new CustomVertex.TransformedColored[4];
            clrQuadEmptyProgress = new CustomVertex.TransformedColored[4];
            Update();
        }

        public override void Update()
        {
            int clrInt = clr.ToArgb();
            int clrGrey = Color.Gray.ToArgb();
            float progressX = (dimensions.Width / 100f) * progress;

            clrQuadProgress[0] = new CustomVertex.TransformedColored(origin.X, origin.Y + dimensions.Height, 0, 1, clrInt);
            clrQuadProgress[1] = new CustomVertex.TransformedColored(origin.X, origin.Y, 0, 1, clrInt);

            clrQuadProgress[2] = new CustomVertex.TransformedColored(origin.X + progressX, origin.Y + dimensions.Height, 0, 1, clrInt);
            clrQuadProgress[3] = new CustomVertex.TransformedColored(origin.X + progressX, origin.Y, 0, 1, clrInt);

            clrQuadEmptyProgress[0] = new CustomVertex.TransformedColored(origin.X + progressX, origin.Y + dimensions.Height, 0, 1, clrGrey);
            clrQuadEmptyProgress[1] = new CustomVertex.TransformedColored(origin.X + progressX, origin.Y, 0, 1, clrGrey);

            clrQuadEmptyProgress[2] = new CustomVertex.TransformedColored(origin.X + dimensions.Width, origin.Y + dimensions.Height, 0, 1, clrGrey);
            clrQuadEmptyProgress[3] = new CustomVertex.TransformedColored(origin.X + dimensions.Width, origin.Y, 0, 1, clrGrey);
        }

        public override void Draw()
        {
            gDevice.SetTexture(0, null);
            gDevice.VertexFormat = CustomVertex.TransformedColored.Format;
            gDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, clrQuadProgress);
            gDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, clrQuadEmptyProgress);
        }

        public int Progress
        {
            get { return progress; }
            set { progress = value <= 100 ? value : 100; Update(); }
        }
    }
}