namespace NuGenRenderOptics
{
    class PlaneSceneObject : OpticalSceneObject
    {
        PlaneD plane;

        public PlaneSceneObject(Vector3D origin, Vector3D normal, MaterialShader shader, double radius)
            : base(origin, shader, radius)
        {
            plane = PlaneD.FromPointNormal(origin, normal);
        }

        public override bool GetIntersect(Vector3D p1, Vector3D p2, Vector3D uv, out Vector3D iPos, out double iDist,
                                          out uint subIdx)
        {
            subIdx = 0;
            if (plane.GetIntersectionWithLine(p1, uv, out iPos))//plane.GetIntersectionWithLimitedLine(p1, p2, out iPos))
            {
                // calc distance
                iDist = (iPos - p1).Length();
                return true;
            }
            iDist = -1;
            return false;
        }

        public override Vector3D GetNormal(Vector3D pos, uint subIdx)
        {
            return plane.PlaneNormal;
        }

        public override Vector2D GetTexCoord(Vector3D p, uint subIdx)
        {
            throw new System.NotImplementedException();
        }
    }
}