using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Controls;

namespace NuGenCRBase.AvalonBridge
{
    class ABScene3D : IDisposable
    {
        public ABModel3D[] Models = null;
        public ABCamera[] Cameras = null;
        public int CurrentCamera = -1;
        public ABLight[] Lights = null;

        public float BoundingSphere;
        public ABVolume3 BoundingCubeVolume;
        public Vector3 Origin;

        public ABScene3D()
        {
        }

        public ModelVisual3D[] ToAvalonObjs()
        {
            if (Models != null)
            {
                List<ModelVisual3D> modelList = new List<ModelVisual3D>();
                // add models
                foreach (ABModel3D model in Models)
                {
                    modelList.AddRange(model.ToAvalonObj());
                }
                return modelList.ToArray();
            }
            return null;
        }

        public Viewport3D ToAvalonObj(bool completeViewport)
        {
            Viewport3D viewport = new Viewport3D();

	        if (Models != null)
	        {
		        // add models
		        foreach (ABModel3D model in Models)
                {
                    ModelVisual3D[] models = model.ToAvalonObj();
                    foreach (ModelVisual3D mdl in models)
                    {
                        viewport.Children.Add(mdl);
                    }
		        }
	        }
            
            if (Cameras != null)
            {
                // add camera
                Point3D pos = new Point3D(Cameras[0].Position.X, Cameras[0].Position.Y, Cameras[0].Position.Z);
                Vector3 lookDir = Cameras[0].Target - Cameras[0].Position;
                lookDir.Normalize();

                viewport.Camera = new PerspectiveCamera(pos,
                                                        new Vector3D(lookDir.X, lookDir.Y, lookDir.Z),
                                                        new Vector3D(0, 1, 0),
                                                        45f);
            }

            if (Lights != null)
            {
                // add lights
                foreach (ABLight light in Lights)
                {
                    viewport.Children.Add(light.ToAvalonObj());
                }
            }

	        return viewport;
        }

        public static ABScene3D FromAvalonObj(Model3D avalonObj)
        {
            return null;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (Models != null)
            {
                foreach (ABModel3D model in Models)
                {
                    model.Dispose();
                }
            }
        }

        #endregion

        public void CalcBounds()
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            foreach (ABModel3D model in Models)
            {
                model.CalcBounds();

                if (model.BoundingCubeVolume.Min.X < min.X)
                    min.X = model.BoundingCubeVolume.Min.X;
                if (model.BoundingCubeVolume.Min.Y < min.Y)
                    min.Y = model.BoundingCubeVolume.Min.Y;
                if (model.BoundingCubeVolume.Min.Z < min.Z)
                    min.Z = model.BoundingCubeVolume.Min.Z;

                if (model.BoundingCubeVolume.Max.X > max.X)
                    max.X = model.BoundingCubeVolume.Max.X;
                if (model.BoundingCubeVolume.Max.Y > max.Y)
                    max.Y = model.BoundingCubeVolume.Max.Y;
                if (model.BoundingCubeVolume.Max.Z > max.Z)
                    max.Z = model.BoundingCubeVolume.Max.Z;
            }
            this.BoundingCubeVolume.Max = max;
            this.BoundingCubeVolume.Min = min;

            Vector3 extent = max - min;
            Origin = min + (extent * 0.5f);
            BoundingSphere = (extent * 0.5f).Length();
        }
    }
}