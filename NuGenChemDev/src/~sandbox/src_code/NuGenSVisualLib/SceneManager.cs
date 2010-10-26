using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Chem;
using Org.OpenScience.CDK.Interfaces;
using NuGenSVisualLib.Rendering.Effects;
using NuGenSVisualLib.Rendering.Pipelines;
using NuGenSVisualLib.Rendering;
using NuGenSVisualLib.Settings;
using NuGenSVisualLib.Rendering.Lighting;
using System.Drawing;
using Microsoft.DirectX;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Rendering.Chem.Schemes;
using OcTree;
using NuGenSVisualLib.Rendering.Chem.Structures;
using NuGenSVisualLib.Rendering.ThreeD;
using NuGenSVisualLib.Rendering.Entities;
using System.Collections;

namespace NuGenSVisualLib
{
    class ViewRenderTarget
    {
        public enum ViewProjection
        {
            Perspective,
            Orthographic
        }

        private ViewProjection Projection;
        private float near, far;
        private int width, height;
        private float fov;

        Matrix proj, view;

        public ViewRenderTarget(ViewProjection projection, float near, float far,
                                int width, int height, float fov, float extent)
        {
            Projection = projection;
            this.near = near;
            this.far = far;
            this.width = width;
            this.height = height;
            this.fov = fov;

            // create matrices
            if (projection == ViewProjection.Perspective)
                proj = Matrix.PerspectiveLH(width, height, near, far);
            else if (projection == ViewProjection.Orthographic)
                proj = Matrix.OrthoLH(width, height, near, far);

            view = Matrix.LookAtLH(new Vector3(-extent, extent, extent), new Vector3(), new Vector3(0, 1, 0));
        }
    }

    /// <summary>
    /// Encapsulates the management of a scene
    /// </summary>
    abstract class SceneManager
    {
        protected Device device;

        public SceneManager(Device device)
        {
            this.device = device;
        }

        public abstract void OnSourceDataModified();

        public abstract void RenderSceneFrame(GraphicsPipeline3D pipeline, int width, int height);
    }

    class MoleculeSceneManager : SceneManager
    {
        OcTree<ChemEntity> sceneGraph;

        MoleculeRenderingScheme scheme;
        RenderingEffect effect;
        PostProcessingRenderingEffect ppEffect;

        CompleteOutputDescription coDesc;
        OutputSettings outSettings;

        ViewRenderTarget[] extraRenderTargets;

        List<IScreenSpaceEntity> screenEntities;
        List<ViewSpaceEntity> postSceneViewEntities;
        List<IEntity> postSceneWorldEntities;
        public Dictionary<uint, ChemEntity> sceneGraphEntities;

        SortedList<float, ViewSpaceEntity> zCompareViewEntities;

        public MoleculeSceneManager(Device device, OutputSettings outSettings)
            : base(device)
        {
            
            screenEntities = new List<IScreenSpaceEntity>();
            postSceneWorldEntities = new List<IEntity>();
            postSceneViewEntities = new List<ViewSpaceEntity>();
            zCompareViewEntities = new SortedList<float, ViewSpaceEntity>();

            //effect = new ShadowMappingEffect(device, HashTableSettings.Instance, 1);//PPixelLightEffect(device, HashTableSettings.Instance, 1);
            effect = new PPixelLightEffect(device, HashTableSettings.Instance, 1);
            effect.LoadResources();

            LightingSetup setup = new LightingSetup();
            DirectionalLight light = new DirectionalLight();
            light.Clr = Color.White;
            light.Direction = new Vector3(1, -1, -1);
            light.Enabled = true;
            setup.lights.Add(light);

            effect.SetupWithLights(setup);
            effect.SetupForDevice(this.outSettings = outSettings);
            
            /*ppEffect = new BloomEffect(device, HashTableSettings.Instance);
            ppEffect.LoadResources();
            ppEffect.SetupWithLights(setup);*/
            effect.SetupForDevice(this.outSettings);
        }

        public void OnNewDataSource(IAtom[] atoms, IBond[] bonds, Vector3 origin, Bounds3D bounds)
        {
            sceneGraph = new OcTree<ChemEntity>((int)bounds.radius * 2, -bounds.min);
            /*screenEntities.Add(new BondAngle(device, atoms[0], atoms[1], atoms[2]));
            screenEntities[0].Init(device);

            postSceneWorldEntities.Add(new BoundingBoxEntity(new BoundingBox(bounds.min, bounds.max), false, Color.LightGray.ToArgb()));
            postSceneWorldEntities[0].Init(device);
            postSceneWorldEntities.Add(new SphereAxis3D());
            postSceneWorldEntities[1].Init(device);*/
            
            sceneGraphEntities = sceneGraph.SceneItems;

            // create molecule entities
            foreach (IAtom atom in atoms)
            {
                AtomEntity aentity = AtomEntity.BuildEntity(atom);
                sceneGraph.Insert(aentity);
                /*ScreenLabelVSpaceEntity entity = new ScreenLabelVSpaceEntity(atom.ID + ":" + aentity.UId.ToString(), (Vector3)aentity.Position3D);
                entity.Init(device);
                screenEntities.Add(entity);*/
                /*AtomSymbolEntity entity = new AtomSymbolEntity(aentity);
                entity.Init(device);
                postSceneViewEntities.Add(entity);*/
            }
            
            //foreach (IBond bond in bonds)
            //{
                //sceneGraph.Insert(new BondE);
            //}

            scheme.SetOutputDescription(coDesc);
            scheme.SetupScene(origin, bounds.radius);

            // pass throught scheme & effects
            IGeometryCreator[] schStreams = scheme.GetAtomStreams();
            DataFields[] efxFields = null;
            //effect.DesiredData(out efxFields, false);

            DataFields[][] allStreams = new DataFields[1 + (efxFields != null ? 1 : 0 )][];
            //if (efxFields != null)
            //    allStreams[1] = efxFields;
            if (scheme.HandlesAtoms && atoms != null)
            {
                GeomDataBufferStream[] geomStream = new GeomDataBufferStream[schStreams.Length];
                for (int i = 0; i < geomStream.Length; i++)
                {
                    allStreams[0] = schStreams[i].Fields;
                    GeomDataTransformer.CreateBufferStream(allStreams, out geomStream[i]);
                }
                // fill buffer stream
                scheme.SetAtomData(atoms, geomStream);
            }

            schStreams = scheme.GetBondStreams();
            if (scheme.HandlesBonds && bonds != null)
            {
                GeomDataBufferStream[] geomStream = new GeomDataBufferStream[schStreams.Length];
                for (int i = 0; i < geomStream.Length; i++)
                {
                    allStreams[0] = schStreams[i].Fields;
                    GeomDataTransformer.CreateBufferStream(allStreams, out geomStream[i]);
                }
                // fill buffer stream
                scheme.SetBondData(bonds, geomStream);
            }
        }

        public override void OnSourceDataModified()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UpdateAtoms(CompleteOutputDescription coDesc, IAtom[] atoms)
        {
            if (scheme.HandlesAtoms)
            {
                scheme.SetOutputDescription(coDesc);

                // pass throught scheme & effects
                IGeometryCreator[] schStreams = scheme.GetAtomStreams();
                DataFields[][] allStreams = new DataFields[1][];

                GeomDataBufferStream[] geomStream = new GeomDataBufferStream[schStreams.Length];
                for (int i = 0; i < geomStream.Length; i++)
                {
                    allStreams[0] = schStreams[i].Fields;
                    GeomDataTransformer.CreateBufferStream(allStreams, out geomStream[i]);
                }
                // fill buffer stream
                scheme.SetAtomData(atoms, geomStream);
            }
        }

        public void SetScheme(MoleculeRenderingScheme scheme)
        {
            this.scheme = scheme;
        }

        public void SetOutputDesc(CompleteOutputDescription desc)
        {
            this.coDesc = new CompleteOutputDescription(desc);
            scheme.SetOutputDescription(desc);
        }

        public void SetEffect(RenderingEffect effect)
        {
            this.effect = effect;
            effect.LoadResources();

            LightingSetup setup = new LightingSetup();
            DirectionalLight light = new DirectionalLight();
            light.Clr = Color.White;
            light.Direction = new Vector3(1, 1, -1);
            light.Enabled = true;
            setup.lights.Add(light);
            effect.SetupWithLights(setup);
            effect.SetupForDevice(outSettings);
        }

        public override void RenderSceneFrame(GraphicsPipeline3D pipeline, int width, int height)
        {
            //pipeline.PushAll();

            //pipeline.PopAll();

            //pipeline.ProjectionMatrix = Matrix.OrthoLH(width, height, 0f, 2f);
            //pipeline.ViewMatrix = Matrix.Identity;
            //pipeline.WorldMatrix = Matrix.Identity;

            if (ppEffect != null)
            {
                // do alpha pass first
                ppEffect.PreFramePass(-1);
                if (ppEffect.RenderScene(-1))
                {
                    // render scene w/effect
                    device.RenderState.Lighting = false;
                    // update frame data
                    scheme.UpdateFrameViewData(pipeline);
                    if (effect != null)
                    {
                        if (effect is GeometryRenderingEffect)
                        {
                            GeometryRenderingEffect gEffect = (GeometryRenderingEffect)effect;
                            gEffect.RenderFrame(pipeline, scheme, true);
                        }
                        /*else if (effect is ScreenSpaceRenderingEffect)
                        {
                            ScreenSpaceRenderingEffect sEffect = (ScreenSpaceRenderingEffect)effect;
                            sEffect.UpdateFrameData(scheme.atomsBufferData, pipeline);

                            sEffect.RenderFrame(pipeline);
                            return;
                        }*/
                    }
                    /*device.RenderState.Lighting = false;
                    scheme.RenderAtoms(true, effect == null);
                    scheme.RenderBonds(true, (effect == null || !scheme.LightBonds));*/
                }
                ppEffect.PostFramePass(-1);

                for (int i = 0; i < ppEffect.NumPassesRequired; i++)
                {
                    ppEffect.PreFramePass(i);
                    if (ppEffect.RenderScene(i))
                    {
                        // render scene w/effect
                        device.RenderState.Lighting = true;
                        // update frame data
                        scheme.UpdateFrameViewData(pipeline);
                        if (effect != null)
                        {
                            if (effect is GeometryRenderingEffect)
                            {
                                GeometryRenderingEffect gEffect = (GeometryRenderingEffect)effect;
                                gEffect.RenderFrame(pipeline, scheme, false);
                            }
                            else if (effect is ScreenSpaceRenderingEffect)
                            {
                                ScreenSpaceRenderingEffect sEffect = (ScreenSpaceRenderingEffect)effect;
                                sEffect.UpdateFrameData(scheme.atomsBufferData, pipeline);

                                sEffect.RenderFrame(pipeline);
                                return;
                            }
                        }
                        device.RenderState.Lighting = false;
                        scheme.RenderAtoms(true, effect == null);
                        scheme.RenderBonds(true, (effect == null || !scheme.LightBonds));
                    }
                    ppEffect.PostFramePass(i);
                }
            }
            else
            {
                device.RenderState.Lighting = true;

                // update frame data
                scheme.UpdateFrameViewData(pipeline);

                if (effect != null)
                {
                    if (effect is GeometryRenderingEffect)
                    {
                        GeometryRenderingEffect gEffect = (GeometryRenderingEffect)effect;
                        gEffect.RenderFrame(pipeline, scheme, false);
                    }
                    else if (effect is ScreenSpaceRenderingEffect)
                    {
                        ScreenSpaceRenderingEffect sEffect = (ScreenSpaceRenderingEffect)effect;
                        sEffect.UpdateFrameData(scheme.atomsBufferData, pipeline);

                        sEffect.RenderFrame(pipeline);
                        return;
                    }
                    //else if (effect.EfxType == RenderingEffect.EffectType.ScreenSpace)
                    //{
                    //    // TODO: Check for matrix changes for update check
                    //    effect.UpdateFrameData(scheme.atomsBufferData, pipeline);
                    //    effect.RenderFrame(pipeline, scheme);
                    //}
                }

                // render as fixed pipeline (/ do FP pass)

                // TODO: Setup lights

                device.RenderState.Lighting = false;

                scheme.RenderAtoms(true, effect == null);
                scheme.RenderBonds(true, (effect == null || !scheme.LightBonds));
            }

            // render post-scene world entities
            foreach (IEntity entity in postSceneWorldEntities)
            {
                entity.Render();
            }

            Matrix wvMat = pipeline.WorldMatrix * pipeline.ViewMatrix;
            foreach (ViewSpaceEntity entity in postSceneViewEntities)
            {
                entity.UpdateView(pipeline.WorldMatrix, pipeline.ViewMatrix);
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
                entity.Value.Render();
            }
            zCompareViewEntities.Clear();

            // render post-scene screen entities
            foreach (IScreenSpaceEntity entity in screenEntities)
            {
                // TODO: Detect matrix changes
                entity.Update(pipeline.WorldMatrix, pipeline.ViewMatrix, pipeline.ProjectionMatrix);
                entity.Render();
            }
        }

        public void UpdateBonds(CompleteOutputDescription latestCoDesc, IBond[] bonds)
        {
            if (scheme.HandlesBonds)
            {
                scheme.SetOutputDescription(coDesc);

                DataFields[][] allStreams = new DataFields[1][];
                IGeometryCreator[] schStreams = scheme.GetBondStreams();
                GeomDataBufferStream[] geomStream = new GeomDataBufferStream[schStreams.Length];
                
                for (int i = 0; i < geomStream.Length; i++)
                {
                    allStreams[0] = schStreams[i].Fields;
                    GeomDataTransformer.CreateBufferStream(allStreams, out geomStream[i]);
                }
                // fill buffer stream
                scheme.SetBondData(bonds, geomStream);
            }
        }

        public ChemEntity TraceRayInScene(Vector3 rayOrigin, Vector3 rayDir)
        {
            ChemEntity entity = (ChemEntity)sceneGraph.RayIntersectFirst(rayOrigin, rayDir);
            /*if (entity != null)
            {
                AtomSelectionEntity selEntity = new AtomSelectionEntity((AtomEntity)entity);
                selEntity.Init(device);
                postSceneViewEntities.Add(selEntity);
            }*/
            /*Ray3D ray = new Ray3D(rayOrigin, rayDir, 20f);
            postSceneWorldEntities.Add(ray);
            ray.Init(device);*/
            return entity;
        }

        public void TraceFrustumInScene(Matrix viewProjection)
        {
            Frustum frustum = new Frustum(viewProjection);
            ChemEntity[] entities = sceneGraph.GetAllInsideFrustum(frustum);
            if (entities != null)
            {
                foreach (ChemEntity entity in entities)
                {
                    AtomSelectionEntity selEntity = new AtomSelectionEntity((AtomEntity)entity);
                    selEntity.Init(device);
                    postSceneViewEntities.Add(selEntity);
                }
            }
        }

        public void RemoveFromPostSceneView(ICollection entities)
        {
            foreach (ViewSpaceEntity entity in entities)
            {
                postSceneViewEntities.Remove(entity);
            }
        }

        public void AddToPostSceneView(ViewSpaceEntity entity)
        {
            postSceneViewEntities.Add(entity);
        }

        public void RemoveFromPostSceneView(ICollection entities, bool[] inUse)
        {
            int idx = 0;
            foreach (ViewSpaceEntity entity in entities)
            {
                if (!inUse[idx++])
                    postSceneViewEntities.Remove(entity);
            }
        }

        public void AddToPostScene(BoundingBoxEntity entity)
        {
            postSceneWorldEntities.Add(entity);
        }

        public void RemoveFromPostScene(List<IEntity> entities)
        {
            foreach (IEntity entity in entities)
            {
                postSceneWorldEntities.Remove(entity);
            }
        }
    }
}