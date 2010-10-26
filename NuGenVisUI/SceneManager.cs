using System;
using System.Collections.Generic;
using Genetibase.NuGenRenderCore.Rendering;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Scene;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using OcTree;

namespace Genetibase.VisUI.Rendering
{
    public class GenericSceneManager<T> : SceneManager where T : SceneEntity
    {
        readonly OcTree<T> sceneGraph;

        GraphicsDeviceSettings outSettings;

        readonly List<IScreenSpaceEntity> screenEntities;
        readonly List<ViewSpaceEntity> postSceneViewEntities;
        readonly List<IWorldEntity> postSceneWorldEntities;
        public Dictionary<uint, T> sceneGraphEntities;

        readonly SortedList<float, ViewSpaceEntity> zCompareViewEntities;

        public GenericSceneManager(Device device, GraphicsDeviceSettings outSettings,
                                   Vector3 origin, int size)
            : base(device)
        {
            this.outSettings = outSettings;

            sceneGraph = new OcTree<T>(size, origin);
            screenEntities = new List<IScreenSpaceEntity>();
            postSceneWorldEntities = new List<IWorldEntity>();
            postSceneViewEntities = new List<ViewSpaceEntity>();
            zCompareViewEntities = new SortedList<float, ViewSpaceEntity>();
            sceneGraphEntities = sceneGraph.SceneItems;
        }

        public void AddSceneEntity(T entity)
        {
            sceneGraph.Insert(entity);
            //sceneGraphEntities.Add(0, entity);
        }

        public void AddScreenSpaceEntity(IScreenSpaceEntity entity)
        {
            screenEntities.Add(entity);
        }

        private void RemoveScreenSpaceEntity(IScreenSpaceEntity entity)
        {
            screenEntities.Remove(entity);
        }

        private void RemoveSceneEntity(T entity)
        {
            sceneGraph.Remove(entity);
        }

        public T TraceRay(Vector3 rayOrigin, Vector3 rayDir, out int internalValue)
        {
            OcTree<T>.OcTreeResult result = sceneGraph.RayIntersectFirst(rayOrigin, rayDir);
            if (result != null)
            {
                internalValue = result.InternalValue;
                return (T)result.Item;
            }
            internalValue = -1;
            return null;
        }

        #region SceneManager Members

        public override void OnSourceDataModified()
        {
            throw new System.NotImplementedException();
        }

        public override void RenderSceneFrame(GraphicsPipeline gPipeline, int width, int height)
        {
            // render scene graph
            // TODO: sceneGraph.StreamAllInFrustum
            gPipeline.PushAll();
            foreach (KeyValuePair<uint, T> item in sceneGraphEntities)
            {
                SceneEntity entity = item.Value;
                entity.PreRender(gPipeline);
                entity.Render(gPipeline);
                entity.PostRender(gPipeline);
            }
            gPipeline.PopAll();

            // render post-scene world entities
            gPipeline.PushAll();
            foreach (IWorldEntity entity in postSceneWorldEntities)
            {
                entity.Render(gPipeline);
            }
            gPipeline.PopAll();

            Matrix wvMat = gPipeline.WorldMatrix * gPipeline.ViewMatrix;
            foreach (ViewSpaceEntity entity in postSceneViewEntities)
            {
                entity.UpdateView(gPipeline.WorldMatrix, gPipeline.ViewMatrix);
                // z-compare
                float z = 1000 - Vector3.Transform(entity.BoundingBox.Centre, wvMat).Z;
                try
                {
                    zCompareViewEntities.Add(z, entity);
                }
                catch { zCompareViewEntities.Add(z - 0.001f, entity); } // TODO: more robust feature?
            }

            foreach (KeyValuePair<float, ViewSpaceEntity> entity in zCompareViewEntities)
            {
                entity.Value.Render(gPipeline);
            }
            zCompareViewEntities.Clear();

            // render post-scene screen entities
            foreach (IScreenSpaceEntity entity in screenEntities)
            {
                // TODO: Detect matrix changes
                entity.Update(gPipeline.WorldMatrix, gPipeline.ViewMatrix, gPipeline.ProjectionMatrix, gDevice.Viewport);
                entity.Render();
            }
        }

        public override void AddEntity(object entity)
        {
            if (entity is T)
                AddSceneEntity((T)entity);
            else if (entity is IScreenSpaceEntity)
                AddScreenSpaceEntity((IScreenSpaceEntity)entity);
            else 
                throw new Exception("Unknown entity type");
        }

        public override void RemoveEntity(object entity)
        {
            if (entity is T)
                RemoveSceneEntity((T)entity);
            else if (entity is IScreenSpaceEntity)
                RemoveScreenSpaceEntity((IScreenSpaceEntity)entity);
            else
                throw new Exception("Unknown entity type");
        }
        #endregion
    }
}