using System.Drawing;
using Genetibase.NuGenRenderCore.Resources;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.VisUI.UI
{
    class GUIImage : GUILayerItem
    {
        protected TextureResource texture;
        protected CustomVertex.TransformedTextured[] texQuad;

        public GUIImage(Point origin, Size dimensions, TextureResource texture)
            : base(origin, dimensions)
        {
            this.texture = texture;
        }

        public override void Init(Device device)
        {
            base.Init(device);

            texQuad = new CustomVertex.TransformedTextured[4];
            Update();
        }

        public override void Draw()
        {
            gDevice.VertexFormat = CustomVertex.TransformedTextured.Format;
            gDevice.SetTexture(0, texture.Texture);

            gDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, texQuad);
            gDevice.SetTexture(0, null);
        }

        public override void Update()
        {
            texQuad[0] = new CustomVertex.TransformedTextured(origin.X, origin.Y + dimensions.Height, 0, 1, 0, 0);
            texQuad[1] = new CustomVertex.TransformedTextured(origin.X + dimensions.Width, origin.Y + dimensions.Height, 0, 1, 1, 0);
            texQuad[2] = new CustomVertex.TransformedTextured(origin.X, origin.Y, 0, 1, 0, 1);
            texQuad[3] = new CustomVertex.TransformedTextured(origin.X + dimensions.Width, origin.Y, 0, 1, 1, 1);
        }
    }

    class GUIIcon : GUIImage
    {
        TextureResource.Icon icon;
        bool highlight;
        bool isHighlighting;

        public GUIIcon(Point origin, Size dimensions, TextureResource.Icon icon,
                       bool highlight, bool enabled)
            : base(origin, dimensions, icon.Texture)
        {
            this.icon = icon;
            this.highlight = highlight;
            wantMouseOver = true;
            this.enabled = enabled;
        }

        public override void Update()
        {
            texQuad[0] = new CustomVertex.TransformedTextured(origin.X, origin.Y, 0, 1, 0, 0);
            texQuad[1] = new CustomVertex.TransformedTextured(origin.X + dimensions.Width, origin.Y, 0, 1, 0, 0);
            texQuad[2] = new CustomVertex.TransformedTextured(origin.X, origin.Y + dimensions.Height, 0, 1, 0, 0);
            texQuad[3] = new CustomVertex.TransformedTextured(origin.X + dimensions.Width, origin.Y + dimensions.Height, 0, 1, 0, 0);
        }

        public override void Draw()
        {
            gDevice.VertexFormat = CustomVertex.TransformedTextured.Format;
            gDevice.Indices = null;
            gDevice.SetTexture(0, texture.Texture);

            gDevice.TextureState[0].ColorOperation = TextureOperation.Modulate;
            gDevice.TextureState[0].AlphaOperation = TextureOperation.Modulate;
            gDevice.TextureState[0].ColorArgument0 = TextureArgument.TextureColor;
            gDevice.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;

            gDevice.RenderState.AlphaBlendEnable = true;
            gDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            gDevice.RenderState.DestinationBlend = Blend.InvSourceAlpha;

            RectangleF coords = icon.FaceCoords;
            if (enabled)
            {
                if (highlight && isHighlighting && icon.HighlightCoords != null)
                    coords = icon.HighlightCoords;
            }
            else if (icon.DisabledCoords != null)
                coords = icon.DisabledCoords;

            texQuad[0].Tu = coords.Left; texQuad[0].Tv = coords.Top;
            texQuad[1].Tu = coords.Right; texQuad[1].Tv = coords.Top;
            texQuad[2].Tu = coords.Left; texQuad[2].Tv = coords.Bottom;
            texQuad[3].Tu = coords.Right; texQuad[3].Tv = coords.Bottom;

            gDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, texQuad);

            gDevice.RenderState.AlphaBlendEnable = false;
        }

        public override void OnMouseEnter()
        {
            base.OnMouseEnter();
            if (highlight)
                isHighlighting = true;
        }

        public override void OnMouseLeave()
        {
            base.OnMouseLeave();
            isHighlighting = false;
        }
    }
}