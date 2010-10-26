using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using System.Windows.Media.Media3D;

namespace NuGenCRBase.AvalonBridge
{
    abstract class ABLight
    {
        protected Color clr;

        public Color Clr
        {
            get { return clr; }
        }

        public abstract void SetupDx(Microsoft.DirectX.Direct3D.Light light);
        public abstract Visual3D ToAvalonObj();
    }

    class ABDirectionalLight : ABLight
    {
        protected Vector3 direction;

        public ABDirectionalLight(Vector3 direction, Color color)
        {
            this.direction = direction;
            this.clr = color;
        }

        public Vector3 Direction
        {
            get { return direction; }
        }

        public override void SetupDx(Microsoft.DirectX.Direct3D.Light light)
        {
            light.DiffuseColor = ColorValue.FromColor(clr);
            light.Direction = direction;
            light.Type = LightType.Directional;
            light.Enabled = true;
            //light.Update();
        }

        public override Visual3D ToAvalonObj()
        {
            ModelVisual3D model = new ModelVisual3D();
            DirectionalLight light = new DirectionalLight(System.Windows.Media.Color.FromArgb(clr.A, clr.R, clr.G, clr.B),
                                                          new Vector3D(direction.X, direction.Y, direction.Z));
            model.Content = light;
            return model;
        }
    }
}