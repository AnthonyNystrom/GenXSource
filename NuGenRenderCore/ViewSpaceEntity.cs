using System;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Scene;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenRenderCore.Rendering
{
    public abstract class ViewSpaceEntity : IWorldEntity
    {
        IEntity[] dependants;
        protected Device gDevice;
        protected BoundingBox bBox;
        private IWorldEntity parent;
        private IWorldEntity[] children;

        public abstract void UpdateView(Matrix world, Matrix view);

        #region IWorldEntity Members

        public abstract void PreRender(GraphicsPipeline gPipeline);
        public abstract void Render(GraphicsPipeline gPipeline);
        public abstract void PostRender(GraphicsPipeline gPipeline);
        public abstract void Init(DeviceInterface devIf, SceneManager sManager);
        public abstract void DeInit(DeviceInterface devIf, SceneManager sManager);

        public void ParentUpdated(Matrix toWorld)
        {
            throw new NotImplementedException();
        }

        public void AddChild(IWorldEntity child)
        {
            throw new NotImplementedException();
        }

        public void RemoveChild(IWorldEntity child)
        {
            throw new NotImplementedException();
        }

        public abstract void ParentUpdated();

        public abstract BoundingBox BoundingBox
        {
            get;
        }

        public IWorldEntity Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public IWorldEntity[] Children
        {
            get { return children; }
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

        public void DependancyTreeUpdated(IEntity depentancy, Matrix toWorld)
        {
            throw new NotImplementedException();
        }

        public void DependancyMoved(IEntity depentancy, Vector3 rDistance)
        {
            throw new NotImplementedException();
        }

        public void DependancyScaled(IEntity dependancy, Vector3 rScale)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members
        public abstract void Dispose();
        #endregion
    }
}