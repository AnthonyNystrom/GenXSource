using System.Drawing;
using Microsoft.DirectX.Direct3D;

namespace NuGenSVisualLib.Rendering
{
    class TextTexture
    {
        private Texture texture;
        private Size size;

        public Texture Texture
        {
            get { return texture; }
        }

        public Size Size
        {
            get { return size; }
        }

        public static TextTexture DrawTextToTexture(string text, System.Drawing.Font font, Device device,
                                                    int width, int height)
        {
            Bitmap img = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(img);

            SizeF tSz = g.MeasureString(text, font);
            int x = (int)((width - tSz.Width) / 2.0f);
            int y = (int)((height - tSz.Height) / 2.0f);

            g.DrawString(text, font, Brushes.White, x, y);

            TextTexture tex = new TextTexture();
            tex.texture = Texture.FromBitmap(device, img, Usage.None, Pool.Managed);
            tex.size = new Size(width, height);

            g.Dispose();
            img.Dispose();

            return tex;
        }

        public static TextTexture DrawTextToTexture(string text, System.Drawing.Font font, Device device)
        {
            Graphics g;
            Bitmap img = new Bitmap(32,32);
            g = Graphics.FromImage(img);
            SizeF textSize = g.MeasureString(text, font);

            g.Dispose();
            img.Dispose();

            return DrawTextToTexture(text, font, device, (int)textSize.Width + 1, (int)textSize.Height + 1);
        }
    }
}