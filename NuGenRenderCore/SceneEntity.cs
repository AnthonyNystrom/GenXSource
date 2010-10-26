using System;
using System.Drawing;
using Genetibase.NuGenRenderCore.Rendering;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using OcTree;

namespace Genetibase.NuGenRenderCore.Scene
{
    public abstract class SceneEntity : OcTreeItem, IWorldEntity
    {
        protected BoundingBox bBox;
        protected Device gDevice;
        protected SceneEntity parent;
        protected IWorldEntity[] children;
        bool selected;
        int internalSelectedValue;

        Matrix toWorldMatrix, toWorldAnsestoryMatrix, toWorldMatrixTotal;
        private IEntity[] dependants;

        Vector3 translation;

        protected SceneEntity(Vector3 centre, Vector3 dimensions)
            : base(centre, dimensions)
        {
            internalSelectedValue = -1;

            bBox = new BoundingBox();
            bBox.Centre = centre;
            bBox.Extent = dimensions * 0.5f;
            bBox.Dimensions = dimensions;
            bBox.Position = centre - bBox.Extent;

            toWorldAnsestoryMatrix = Matrix.Identity;
            Scale = new Vector3(1, 1, 1);
            translation = centre;

            UpdateWorldMatrix();
        }

        public void Move(Vector3 location)
        {
            //Vector3 rDistance = location - (Vector3)Centre;

            // update tree data
            translation = location;

            // update matrix
            UpdateWorldMatrix();
            Vector4 cent = Vector3.Transform(Vector3.Empty, toWorldMatrixTotal);
            Centre = new Vector3(cent.X, cent.Y, cent.Z);

            // update bounds
            //bBox.MoveTo((Vector3)Centre);

            // update children
            if (children != null)
            {
                foreach (IWorldEntity entity in children)
                {
                    entity.ParentUpdated(toWorldMatrixTotal);
                }
            }
            // update dependants
            if (dependants != null)
            {
                foreach (IEntity entity in dependants)
                {
                    //entity.DependancyMoved(this, rDistance);
                    entity.DependancyTreeUpdated(this, toWorldMatrixTotal);
                }
            }
        }

        public void Rotate(Vector3 rotation)
        {
        }

        public void Scaling(Vector3 scaling)
        {
            Vector3 rScaling = scaling - (Vector3)Scale;

            // update bounds
            /*bBox.Extent = new Vector3(bBox.Extent.X * scaling.X, bBox.Extent.Y * scaling.Y,
                                                      bBox.Extent.Z * scaling.Z);
            bBox.Position = bBox.Centre - new Vector3(bBox.Extent.X * scaling.X, bBox.Extent.Y * scaling.Y,
                                                      bBox.Extent.Z * scaling.Z);*/

            Scale = scaling;

            UpdateWorldMatrix();

            // update children
            if (children != null)
            {
                foreach (IWorldEntity entity in children)
                {
                    entity.ParentUpdated(toWorldMatrixTotal);
                }
            }
            // update dependants
            if (dependants != null)
            {
                foreach (IEntity entity in dependants)
                {
                    entity.DependancyScaled(this, rScaling);
                }
            }
        }

        private void UpdateWorldMatrix()
        {
            toWorldMatrix = Matrix.Scaling((Vector3)Scale) * Matrix.Translation(translation);

            if (parent != null)
                toWorldMatrixTotal = toWorldMatrix * toWorldAnsestoryMatrix;
            else
                toWorldMatrixTotal = toWorldMatrix;
        }

        public virtual bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public virtual int InternalSelectedValue
        {
            get { return internalSelectedValue; }
            set { internalSelectedValue = value; }
        }

        public Matrix ToWorldMatrix
        {
            get { return toWorldMatrixTotal; }
        }

        protected void SetChildren(IWorldEntity[] children)
        {
            this.children = children;
            foreach (IWorldEntity child in children)
            {
                child.Parent = this;
                child.ParentUpdated(toWorldMatrixTotal);
            }
        }

        #region IWorldEntity Members

        public virtual void PreRender(GraphicsPipeline gPipeline)
        {
            gPipeline.Push();
            gPipeline.WorldMatrix = toWorldMatrixTotal * gPipeline.WorldMatrix;
        }

        public virtual void Render(GraphicsPipeline gPipeline)
        {
            if (selected)
            {
                gPipeline.BeginSceneKeepState();
                BoundingBox.DrawWireframe(gDevice, bBox, Color.Yellow.ToArgb());
                gPipeline.EndSceneKeepState();
            }
        }

        public virtual void PostRender(GraphicsPipeline gPipeline)
        {
            gPipeline.Pop();
        }

        public virtual void Init(DeviceInterface devIf, SceneManager sManager)
        {
            gDevice = devIf.Device;
        }

        public virtual void DeInit(DeviceInterface devIf, SceneManager sManager)
        { }

        public void ParentUpdated(Matrix toWorld)
        {
            toWorldAnsestoryMatrix = toWorld;
            UpdateWorldMatrix();

            // update children
            if (children != null)
            {
                foreach (IWorldEntity entity in children)
                {
                    entity.ParentUpdated(toWorldMatrixTotal);
                }
            }
            // update dependants
            if (dependants != null)
            {
                foreach (IEntity entity in dependants)
                {
                    entity.DependancyTreeUpdated(this, toWorldMatrixTotal);
                }
            }
        }

        public void AddChild(IWorldEntity child)
        {
            throw new NotImplementedException();
        }

        public void RemoveChild(IWorldEntity child)
        {
            throw new NotImplementedException();
        }

        public BoundingBox BoundingBox
        {
            get { return bBox; }
        }

        public IWorldEntity Parent
        {
            get { return parent; }
            set { parent = (SceneEntity)value; }
        }

        public IWorldEntity[] Children
        {
            get { return children; }
        }

        public SceneEntity SceneParent
        {
            get { return parent; }
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
            entity.DependancyTreeUpdated(this, toWorldMatrixTotal);
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
    }
}