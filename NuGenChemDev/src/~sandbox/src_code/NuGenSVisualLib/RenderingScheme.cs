using System;
using System.Collections.Generic;
using System.Text;
using NuGenSVisualLib.Rendering.Pipelines;
using Org.OpenScience.CDK.Interfaces;
using NuGenSVisualLib.Rendering.Chem.Structures;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Settings;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Rendering.Effects;
using NuGenSVisualLib.Rendering.Chem.Schemes;
using Microsoft.DirectX;

namespace NuGenSVisualLib.Rendering.Chem
{
    public class SchemeSUIChangeHandler : EventArgs
    {
        public bool AtomChange;
        public bool BondChange;

        public SchemeSUIChangeHandler(bool atomChange, bool bondChange)
        {
            AtomChange = atomChange;
            BondChange = bondChange;
        }
    }

    public interface MoleculeSchemeSUI
    {
        void UpdateValues();
        void SetChangeEvent(object lockObj, EventHandler<SchemeSUIChangeHandler> handler);

    }

    /// <summary>
    /// Encapsulates settings specific to the molecule scheme
    /// </summary>
    public abstract class MoleculeSchemeSettings : IPropertyTreeNodeClass, ICloneable
    {
        PropertyTree parent;
        protected Type renderingEffectType;
        protected LevelOfDetailRange bondLodRange, atomLodRange;
        protected ushort atomLOD;
        protected ushort bondLOD;

        #region Properties
        public Type RenderingEffectType
        {
            get { return renderingEffectType; }
            set { renderingEffectType = value; }
        }

        public LevelOfDetailRange BondLODRange
        {
            get { return bondLodRange; }
            set { bondLodRange = value; }
        }

        public LevelOfDetailRange AtomLODRange
        {
            get { return atomLodRange; }
            set { atomLodRange = value; }
        }

        public ushort AtomLOD
        {
            get { return atomLOD; }
            set { atomLOD = value; }
        }

        public ushort BondLOD
        {
            get { return bondLOD; }
            set { bondLOD = value; }
        }
        #endregion

        public MoleculeSchemeSettings(Type renderingEffectType, LevelOfDetailRange bondLodRange,
                                      LevelOfDetailRange atomLodRange)
        {
            this.renderingEffectType = renderingEffectType;
            this.bondLodRange = bondLodRange;
            this.atomLodRange = atomLodRange;
            if (atomLodRange != null)
                this.atomLOD = atomLodRange.Min;
            if (bondLodRange != null)
                this.bondLOD = bondLodRange.Min;
        }

        public abstract MoleculeRenderingScheme GetScheme(Device device);
        public abstract MoleculeSchemeSUI GetSUI();
        public virtual RenderingEffectSettings[] GetRequiredEffects() { return null; }

        #region ICloneable Members

        public abstract object Clone();

        #endregion
    }

    /// <summary>
    /// Encapsulates a scheme to create geometry etc. for a renderable item(s)
    /// </summary>
    public abstract class RenderingScheme : IDisposable
    {
        private string name;
        private string description;
        protected OutputRequirements deviceReqs;

        protected Vector3 sceneOrigin;
        protected float sceneRadius;

        public RenderingScheme(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public string Name
        {
            get { return name; }
        }

        public string Description
        {
            get { return description; }
        }

        public OutputRequirements DeviceRequirements
        {
            get { return deviceReqs; }
        }

        public Vector3 SceneOrigin
        {
            get { return sceneOrigin; }
        }

        public float SceneRadius
        {
            get { return sceneRadius; }
        }

        public abstract void UpdateFrameViewData(GraphicsPipeline3D pipeline);

        public virtual void SetupScene(Vector3 origin, float radius)
        {
            this.sceneOrigin = origin;
            this.sceneRadius = radius;
        }
    
        #region IDisposable Members

        public abstract void Dispose();

        #endregion
    }

    /// <summary>
    /// Encapsulates a scheme of creating geometry to represent a molecule
    /// </summary>
    public abstract class MoleculeRenderingScheme : RenderingScheme
    {
        private bool handlesAtoms, handlesBonds, handlesStructures;

        public Device device;
        protected BondGeometryCreator[] bondsBufferCreators;
        protected AtomGeometryCreator[] atomsBufferCreators;
        protected BufferedGeometryData[] bondsBufferData;
        internal BufferedGeometryData[] atomsBufferData;
        protected CompleteOutputDescription coDesc;

        protected bool lightBonds;
        protected bool lightAtoms;

        public virtual void SetAtomData(IAtom[] atoms, GeomDataBufferStream[] geomStreams)
        {
            if (atomsBufferData != null)
            {
                foreach (BufferedGeometryData buffer in atomsBufferData)
                {
                    if (buffer != null)
                        buffer.Dispose();
                }
            }
            atomsBufferData = new BufferedGeometryData[geomStreams.Length];
            // fill each geometry stream with atom data
            for (int i = 0; i < geomStreams.Length; i++)
            {
                atomsBufferCreators[i].CreateGeometryForObjects(device, atoms, geomStreams[i], 0,
                                                                ref atomsBufferData[i], coDesc);
            }
        }

        public virtual void SetBondData(IBond[] bonds, GeomDataBufferStream[] geomStreams)
        {
            if (bondsBufferData != null)
            {
                foreach (BufferedGeometryData buffer in bondsBufferData)
                {
                    if (buffer != null)
                        buffer.Dispose();
                }
            }
            bondsBufferData = new BufferedGeometryData[geomStreams.Length];
            // fill each geometry stream with atom data
            for (int i = 0; i < geomStreams.Length; i++)
            {
                bondsBufferCreators[i].CreateGeometryForObjects(device, bonds, geomStreams[i], 0,
                                                                ref bondsBufferData[i], coDesc);
            }
        }

        public abstract void SetStructureData(ChemEntityStructure[] structures);
        public abstract void SetOutputDescription(CompleteOutputDescription coDesc);

        public virtual IGeometryCreator[] GetAtomStreams()
        {
            return (IGeometryCreator[])atomsBufferCreators;
        }

        public virtual IGeometryCreator[] GetBondStreams()
        {
            return (IGeometryCreator[])bondsBufferCreators;
        }
        
        public virtual void Clear()
        {
            // TODO: Dispose creators
            if (atomsBufferData != null)
            {
                foreach (BufferedGeometryData data in atomsBufferData)
                {
                    data.Dispose();
                }
            }
            if (bondsBufferData != null)
            {
                foreach (BufferedGeometryData data in bondsBufferData)
                {
                    data.Dispose();
                }
            }
        }

        public MoleculeRenderingScheme(Device device, string name, string description,
                                       bool handlesAtoms, bool handlesBonds,
                                       bool handlesStructures)
            : base(name, description)
        {
            this.device = device;
            this.handlesAtoms = handlesAtoms;
            this.handlesBonds = handlesBonds;
            this.handlesStructures = handlesStructures;
        }

        #region Properties

        public bool HandlesAtoms
        {
            get { return handlesAtoms; }
        }

        public bool HandlesBonds
        {
            get { return handlesBonds; }
        }

        public bool HandlesStructures
        {
            get { return handlesStructures; }
        }

        public bool LightBonds
        {
            get { return lightBonds; }
        }

        public bool LightAtoms
        {
            get { return lightAtoms; }
        }

        #endregion

        public override void Dispose()
        {
            Clear();
        }

        public override void UpdateFrameViewData(GraphicsPipeline3D pipeline)
        {
        }
        
        public virtual void RenderAtoms(bool fixedPipeline, bool force)
        {
            if (atomsBufferData != null /*&& coDesc.AtomShadingDesc.Draw*/)
            {
                // render atom sets
                foreach (BufferedGeometryData atomsSet in atomsBufferData)
                {
                    if (atomsSet == null)
                        continue;
                    // only 1 structual inst set for now
                    if (atomsSet.iBuffers[0].Desc == BufferedGeometryData.IndexData.Description.Geometry)
                    {
                        if (fixedPipeline)
                        {
                            if (!force && atomsSet.Light)
                                continue;
                        }
                        else if (!atomsSet.Light)
                            continue;

                        device.BeginScene();

                        // plain geometry
                        device.RenderState.FillMode = atomsSet.iBuffers[0].Fill;
                        if (atomsSet.iBuffers[0].Fill == FillMode.Point)
                        {
                            device.SetStreamSource(0, atomsSet.vBuffers[0].Buffer, 0, atomsSet.vBuffers[0].Stride);
                            device.VertexFormat = atomsSet.vBuffers[0].Format;
                            device.Indices = null;
                            device.DrawPrimitives(atomsSet.iBuffers[0].PrimType, 0, atomsSet.vBuffers[0].NumElements);
                        }
                        else
                        {
                            device.SetStreamSource(0, atomsSet.vBuffers[0].Buffer, 0, atomsSet.vBuffers[0].Stride);
                            device.VertexFormat = atomsSet.vBuffers[0].Format;
                            device.Indices = atomsSet.iBuffers[0].Buffer;
                            if (device.Indices == null)
                                device.DrawPrimitives(atomsSet.iBuffers[0].PrimType, 0, atomsSet.iBuffers[0].NumPrimitives);
                            else
                            {
                                device.DrawIndexedPrimitives(atomsSet.iBuffers[0].PrimType, 0, 0,
                                                             atomsSet.vBuffers[0].NumElements,
                                                             0, atomsSet.iBuffers[0].NumPrimitives);
                            }
                        }

                        device.EndScene();
                    }
                    else if (atomsSet.iBuffers[0].Desc == BufferedGeometryData.IndexData.Description.Sprites)
                    {
                        if (!fixedPipeline)
                            continue;

                        if (atomsSet.iBuffers[0].Buffer == null)
                        {
                            device.BeginScene();

                            // NOTE: needs to turn off lighting at effect level also
                            device.RenderState.Lighting = false;
                            //device.RenderState.DiffuseMaterialSource = ColorSource.Color1;

                            device.RenderState.PointSpriteEnable = true;
                            device.RenderState.PointScaleEnable = true;

                            device.RenderState.PointSize = 0.4f;
                            device.RenderState.PointScaleB = 1.0f;

                            // just set 1 texture for now
                            device.SetTexture(0, atomsSet.iBuffers[0].Textures[0]);

                            device.RenderState.ZBufferWriteEnable = false;
                            device.RenderState.ZBufferEnable = false;

                            device.TextureState[0].ColorOperation = TextureOperation.Modulate;
                            device.TextureState[0].AlphaOperation = TextureOperation.Modulate;
                            device.TextureState[0].ColorArgument0 = TextureArgument.TextureColor;
                            device.TextureState[0].ColorArgument1 = TextureArgument.Diffuse;

                            device.RenderState.AlphaBlendEnable = true;
                            device.RenderState.SourceBlend = Blend.SourceAlpha;
                            device.RenderState.DestinationBlend = Blend.InvSourceAlpha;

                            device.Indices = null;
                            device.VertexFormat = atomsSet.vBuffers[0].Format;
                            device.SetStreamSource(0, atomsSet.vBuffers[0].Buffer, 0);

                            device.DrawPrimitives(PrimitiveType.PointList, 0, atomsSet.iBuffers[0].NumPrimitives);

                            device.RenderState.ZBufferWriteEnable = true;
                            device.RenderState.ZBufferEnable = true;

                            device.RenderState.AlphaBlendEnable = false;

                            device.RenderState.PointSpriteEnable = false;
                            device.RenderState.PointScaleEnable = false;

                            device.EndScene();
                        }
                        else
                        {
                            // NOTE: needs to turn off lighting at effect level also
                            device.RenderState.Lighting = false;
                            //device.RenderState.DiffuseMaterialSource = ColorSource.Color1;

                            device.RenderState.PointSpriteEnable = true;
                            device.RenderState.PointScaleEnable = true;

                            device.RenderState.PointSize = 0.4f;
                            device.RenderState.PointScaleB = 1.0f;

                            device.TextureState[0].ColorOperation = TextureOperation.Modulate;
                            device.TextureState[0].AlphaOperation = TextureOperation.Modulate;
                            device.TextureState[0].ColorArgument0 = TextureArgument.TextureColor;
                            device.TextureState[0].ColorArgument1 = TextureArgument.Diffuse;

                            device.RenderState.AlphaBlendEnable = false;
                            device.RenderState.SourceBlend = Blend.SourceAlpha;
                            device.RenderState.DestinationBlend = Blend.InvSourceAlpha;

                            device.VertexFormat = atomsSet.vBuffers[0].Format;
                            device.SetStreamSource(0, atomsSet.vBuffers[0].Buffer, 0);

                            device.RenderState.ZBufferWriteEnable = false;
                            device.RenderState.ZBufferEnable = false;

                            for (int i = 0; i < atomsSet.iBuffers.Length; i++)
                            {
                                device.BeginScene();

                                // just set 1 texture for now
                                device.SetTexture(0, null);// atomsSet.iBuffers[i].Textures[0]);

                                device.Indices = atomsSet.iBuffers[i].Buffer;

                                device.DrawIndexedPrimitives(PrimitiveType.PointList, 0, 0, atomsSet.vBuffers[0].NumElements, 0, atomsSet.iBuffers[0].NumPrimitives);

                                device.EndScene();
                            }

                            device.RenderState.ZBufferWriteEnable = true;
                            device.RenderState.ZBufferEnable = true;

                            device.RenderState.AlphaBlendEnable = false;

                            device.RenderState.PointSpriteEnable = false;
                            device.RenderState.PointScaleEnable = false;
                        }
                    }
                }
            }
        }

        public virtual void RenderBonds(bool fixedPipeline, bool force)
        {
            if (bondsBufferData != null && coDesc.BondShadingDesc.Draw)
            {
                if (!force && fixedPipeline)
                    return;
                device.BeginScene();

                device.SetStreamSource(0, bondsBufferData[0].vBuffers[0].Buffer, 0, bondsBufferData[0].vBuffers[0].Stride);
                device.VertexFormat = bondsBufferData[0].vBuffers[0].Format;
                device.Indices = null;
                device.DrawPrimitives(bondsBufferData[0].iBuffers[0].PrimType, 0, bondsBufferData[0].iBuffers[0].NumPrimitives);

                device.EndScene();
            }
        }
    }
}