namespace NuGenRenderOptics
{
    internal interface IOpticalSceneObject
    {
        Vector3D Origin
        {
            get;
            set;
        }

        MaterialShader Shader
        {
            get;
            set;
        }

        double Radius
        {
            get;
            set;
        }

        RGBA_D Shade(Ray ray, Vector3D pos, uint subIdx, out Ray reflection, out Ray refraction, ISceneManager scene);
        bool GetIntersect(Vector3D p1, Vector3D p2, Vector3D uv, out Vector3D iPos, out double iDist, out uint subIdx);
        Vector2D GetTexCoord(Vector3D p, uint subIdx);
        Vector3D GetNormal(Vector3D pos, uint subIdx);
    }
}
