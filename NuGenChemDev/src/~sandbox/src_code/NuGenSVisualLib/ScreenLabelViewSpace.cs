using System;
using System.Collections.Generic;
using System.Text;
using NuGenSVisualLib.Rendering.Chem.Structures;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace NuGenSVisualLib.Rendering.Entities
{
    class ScreenLabelVSpaceEntity : IScreenSpaceEntity
    {
        Device device;
        string text;
        Vector3 position;
        Microsoft.DirectX.Direct3D.Font font;
        Point pos2d;
        BoundingBox bBox;

        public ScreenLabelVSpaceEntity(string text, Vector3 position)
        {
            this.text = text;
            this.position = position;
            bBox = new BoundingBox(position, position);
        }

        public void Render()
        {
            font.DrawText(null, text, pos2d, Color.Red);   
        }

        public void Init(Device device)
        {
            this.device = device;

            font = new Microsoft.DirectX.Direct3D.Font(device, 14, 0, FontWeight.SemiBold,
                                                       1, false, CharacterSet.Ansi,
                                                       Precision.Default, FontQuality.AntiAliased,
                                                       PitchAndFamily.DefaultPitch, "Segoe UI");
        }

        public void Update(Matrix world, Matrix view, Matrix proj)
        {
            Vector3 pos = Vector3.Project(position, device.Viewport, proj, view, world);
            pos2d = new Point((int)pos.X, (int)pos.Y);
        }

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
