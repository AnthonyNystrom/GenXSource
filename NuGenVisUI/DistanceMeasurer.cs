using System.Drawing;
using Genetibase.NuGenRenderCore.Rendering;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Scene;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.VisUI.Entities.Helpers
{
    public class DistanceMeasurer : ScreenSpaceEntity
    {
        private SceneEntity point1;
        private SceneEntity point2;
        ScreenSpaceText distanceText;

        Line line;
        Vector2[] linePoints;

        /// <summary>
        /// Initializes a new instance of the DistanceMeasurer class.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        public DistanceMeasurer(SceneEntity point1, SceneEntity point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }

        public SceneEntity Point1
        {
            get { return point1; }
            set { point1 = value; }
        }

        public SceneEntity Point2
        {
            get { return point2; }
            set { point2 = value; }
        }

        private void UpdatePoints()
        {
            Vector3 pos = Vector3.Project((Vector3)Point1.Centre, viewportCached, projCached, viewCached, worldCached);
            linePoints[0] = new Vector2(pos.X, pos.Y);
            pos = Vector3.Project((Vector3)Point2.Centre, viewportCached, projCached, viewCached, worldCached);
            linePoints[1] = new Vector2(pos.X, pos.Y);
        }

        #region IScreenSpaceEntity Members

        public override void Init(DeviceInterface devIf, SceneManager sManager)
        {
            // init line
            line = new Line(devIf.Device);
            line.Antialias = true;
            line.Width = 2;
            
            linePoints = new Vector2[2];

            // init text
            distanceText = new ScreenSpaceText(".m", Color.Yellow, "Tahoma", FontWeight.Normal, 10, new Vector3());
            sManager.AddEntity(distanceText);
        }

        public override void Update(Matrix world, Matrix view, Matrix proj, Viewport viewport)
        {
            worldCached = world;
            viewCached = view;
            projCached = proj;
            viewportCached = viewport;

            UpdatePoints();
        }

        public override void Render()
        {
            // draw line
            line.Begin();
            line.Draw(linePoints, Color.YellowGreen);
            line.End();
        }
        #endregion

        #region IDisposable Members

        public override void Dispose()
        {
        }
        #endregion

        public override void DependancyTreeUpdated(IEntity depentancy, Matrix toWorld)
        {
            throw new System.NotImplementedException();
        }

        public override void DependancyMoved(IEntity depentancy, Vector3 rDistance)
        {
            throw new System.NotImplementedException();
        }

        public override void DependancyScaled(IEntity dependancy, Vector3 rScale)
        {
            throw new System.NotImplementedException();
        }
    }
}