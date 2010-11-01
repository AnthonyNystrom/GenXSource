using System.Drawing;
using Genetibase.NuGenRenderOptics.MDX1.Rasterization;

namespace NuGenRenderOptics
{
    internal interface ISceneManager
    {
        Light[] Lights
        {
            get;
        }

        RGBA_D Ambient
        {
            get;
        }

        bool GetFirstIntersection(Vector3D origin, Vector3D dir, double maxLength, out IOpticalSceneObject obj, out Vector3D iPos, out double iDistance, out uint subIdx);
        void AddObject(IOpticalSceneObject obj);
        void RemoveObject(IOpticalSceneObject obj);
        bool TestForContents(CameraView view, Rectangle area, double length);
        //bool TestForContents(Vector3D origin, Vector3D uv, Vector2D extentStart, Vector2D extentEnd, double length, CameraView view);
    }
}
