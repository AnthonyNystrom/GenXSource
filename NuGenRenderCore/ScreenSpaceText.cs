using System.Drawing;
using Genetibase.NuGenRenderCore.Rendering;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Font=Microsoft.DirectX.Direct3D.Font;

namespace Genetibase.NuGenRenderCore.Scene
{
    public class ScreenSpaceText : ScreenSpaceEntity
    {
        private Device gDevice;
        private Font font;

        private readonly int fontSize;
        private readonly FontWeight fontWeight;
        private readonly string fontFace;
        private string text;
        private readonly Color textClr;

        private Point textPos;
        private readonly Vector3 worldPos;
        private Rectangle textRect;
        private Size textCentre;


        public ScreenSpaceText(string text, Color textClr, string fontFace,
                               FontWeight fontWeight, int fontSize,
                               Vector3 worldPos)
        {
            this.text = text;
            this.textClr = textClr;
            this.fontFace = fontFace;
            this.fontWeight = fontWeight;
            this.fontSize = fontSize;
            this.worldPos = worldPos;
        }

        public ScreenSpaceText(string text, Color textClr, string fontFace,
                               FontWeight fontWeight, int fontSize,
                               Vector3 worldPos, IWorldEntity dependancy)
        {
            this.text = text;
            this.textClr = textClr;
            this.fontFace = fontFace;
            this.fontWeight = fontWeight;
            this.fontSize = fontSize;
            this.worldPos = worldPos;

            Dependancy = dependancy;
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                MeasureText();
            }
        }

        void MeasureText()
        {
            textRect = font.MeasureString(null, text, DrawTextFormat.None, Color.White);
            textCentre = new Size(textRect.Width / 2, textRect.Height / 2);
        }

        private void UpdatePos()
        {
            // update text pos
            Vector3 pos = Vector3.Project(worldPos, viewportCached, projCached, viewCached, localWorld * worldCached);
            textPos = new Point((int)pos.X, (int)pos.Y) - textCentre;
        }

        public override void DependancyTreeUpdated(IEntity dependancy, Matrix toWorld)
        {
            base.DependancyTreeUpdated(dependancy, toWorld);
            UpdatePos();
        }

        #region IScreenSpaceEntity Members

        public override void Init(DeviceInterface devIf, SceneManager sManager)
        {
            gDevice = devIf.Device;

            // create font
            font = new Font(gDevice, fontSize, 0, fontWeight, 1, false,
                            CharacterSet.Ansi, Precision.Default, FontQuality.ClearType,
                            PitchAndFamily.DefaultPitch, fontFace);
            MeasureText();
        }

        public override void Update(Matrix world, Matrix view, Matrix proj, Viewport viewport)
        {
            // updated cached
            worldCached = world;
            viewCached = view;
            projCached = proj;
            viewportCached = viewport;

            UpdatePos();
        }

        public override void Render()
        {
            font.DrawText(null, text, textPos, textClr);
        }
        #endregion

        #region IDisposable Members

        public override void Dispose()
        {
            if (font != null)
                font.Dispose();
        }
        #endregion
    }
}