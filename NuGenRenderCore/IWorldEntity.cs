using System;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Scene;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenRenderCore.Rendering
{
    public class BoundingBox
    {
        //protected Vector3[] points;
        protected Vector3 dimensions;
        protected Vector3 centre;
        protected Vector3 position;
        protected Vector3 extent;

        public BoundingBox(float[] points)
        {
            //this.points = points;

            dimensions = new Vector3();
            dimensions.X = points[1] - points[0];
            dimensions.Y = points[3] - points[2];
            dimensions.Z = points[5] - points[4];

            centre = new Vector3();
            centre.X = points[0] + (dimensions.X / 2.0f);
            centre.Y = points[2] + (dimensions.Y / 2.0f);
            centre.Z = points[4] + (dimensions.Z / 2.0f);
        }

        public BoundingBox(Vector3 min, Vector3 max)
        {
            position = min;
            dimensions = max - min;

            extent = dimensions * 0.5f;
            centre = position + extent;
        }

        public BoundingBox()
        { }

        public Vector3 Centre
        {
            get { return centre; }
            set { centre = value; }
        }

        /// <summary>
        /// The total distance in each axis, away from position
        /// </summary>
        public Vector3 Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        /// <summary>
        /// The lowest bound in each axis
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// The extent the bounds reach away from the centre in each axis
        /// </summary>
        public Vector3 Extent
        {
            get { return extent; }
            set { extent = value; }
        }

        public static void DrawWireframe(Device device, BoundingBox box, int clr)
        {
            CustomVertex.PositionColored[] lines = new CustomVertex.PositionColored[24];
            // bottom
            lines[0] = new CustomVertex.PositionColored(/*box.position*/new Vector3(), clr);
            lines[1] = new CustomVertex.PositionColored(/*box.position.X +*/ box.dimensions.X, /*box.position.Y*/0, /*box.position.Z*/0, clr);

            lines[2] = lines[1];
            lines[3] = new CustomVertex.PositionColored(lines[2].X, /*box.position.Y*/0, /*box.position.Z +*/ box.dimensions.Z, clr);

            lines[4] = lines[3];
            lines[5] = new CustomVertex.PositionColored(/*box.position.X*/0, /*box.position.Y*/0, /*box.position.Z +*/ box.dimensions.Z, clr);

            lines[6] = lines[5];
            lines[7] = lines[0];

            // top
            for (int i = 8; i < 16; i++)
            {
            	lines[i] = lines[i - 8];
                lines[i].Y = /*box.position.Y +*/ box.dimensions.Y;
            }

            // connectors
            lines[16] = lines[0];
            lines[17] = lines[8];

            lines[18] = lines[2];
            lines[19] = lines[10];

            lines[20] = lines[4];
            lines[21] = lines[12];

            lines[22] = lines[6];
            lines[23] = lines[14];

            // render lines
            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.Indices = null;
            device.DrawUserPrimitives(PrimitiveType.LineList, 12, lines);
        }

        public void MoveTo(Vector3 centre)
        {
            this.centre = centre;
            position = centre - extent;
        }
    }

    public interface IEntity : IDisposable
    {
        IEntity[] Dependants { get; }
        void AddDependant(IEntity entity);
        void RemoveDependant(IEntity entity);

        void DependancyTreeUpdated(IEntity dependancy, Matrix toWorld);
        void DependancyMoved(IEntity dependancy, Vector3 rDistance);
        void DependancyScaled(IEntity dependancy, Vector3 rScale);
    }

    /// <summary>
    /// Encapsulates a renderable entity
    /// </summary>
    public interface IWorldEntity : IEntity
    {
        void PreRender(GraphicsPipeline gPipeline);
        void Render(GraphicsPipeline gPipeline);
        void PostRender(GraphicsPipeline gPipeline);
        void Init(DeviceInterface devIf, SceneManager sManager);
        void DeInit(DeviceInterface devIf, SceneManager sManager);
        
        void ParentUpdated(Matrix toWorld);
        void AddChild(IWorldEntity child);
        void RemoveChild(IWorldEntity child);

        BoundingBox BoundingBox { get; }
        IWorldEntity Parent { get; set; }
        IWorldEntity[] Children { get; }
    }

    public interface IScreenSpaceEntity : IEntity
    {
        void Init(DeviceInterface devIf, SceneManager sManager);
        void Update(Matrix world, Matrix view, Matrix proj, Viewport viewport);
        void Render();

        IWorldEntity Dependancy { get; set; }
    }

    public abstract class ScreenSpaceEntity : IScreenSpaceEntity
    {
        private IWorldEntity dependancy;
        private IEntity[] dependants;

        protected Matrix worldCached, viewCached, projCached, localWorld;
        protected Viewport viewportCached;

        #region IScreenSpaceEntity Members

        public abstract void Init(DeviceInterface devIf, SceneManager sManager);
        public abstract void Update(Matrix world, Matrix view, Matrix proj, Viewport viewport);
        public abstract void Render();

        public IWorldEntity Dependancy
        {
            get { return dependancy; }
            set
            {
                if (dependancy != null)
                    dependancy.RemoveDependant(this);
                dependancy = value;
                dependancy.AddDependant(this);
            }
        }
        #endregion

        #region IEntity Members

        public IEntity[] Dependants
        {
            get { return dependants; }
        }

        public void AddDependant(IEntity entity)
        {
            if (dependants == null)
                dependants = new IEntity[] { entity };
            else
            {
                // grow array and add to end
                IEntity[] temp = new IEntity[dependants.Length + 1];
                Array.Copy(dependants, temp, dependants.Length);
                dependants = temp;
                dependants[dependants.Length - 1] = entity;
            }
        }

        public void RemoveDependant(IEntity entity)
        {
            if (dependants.Length == 1)
                dependants = null;
            else
            {
                // reorder new array
                IEntity[] temp = new IEntity[dependants.Length - 1];
                int split = 0;
                for (int i = 0; i < dependants.Length; i++)
                {
                    if (dependants[i] == entity)
                    {
                        split = i;
                        break;
                    }
                }

                if (split > 0)
                    Array.Copy(dependants, temp, split);
                if (split < dependants.Length)
                    Array.Copy(dependants, split + 1, temp, split, dependants.Length - split);
            }
        }

        public virtual void DependancyTreeUpdated(IEntity dependancy, Matrix toWorld)
        {
            localWorld = toWorld;
        }

        public virtual void DependancyMoved(IEntity dependancy, Vector3 rDistance)
        { }

        public virtual void DependancyScaled(IEntity dependancy, Vector3 rScale)
        { }
        #endregion

        #region IDisposable Members

        public abstract void Dispose();
        #endregion
    }
}