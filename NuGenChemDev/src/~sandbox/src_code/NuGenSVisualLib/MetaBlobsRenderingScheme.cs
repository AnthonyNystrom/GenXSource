using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Devices;
using Org.OpenScience.CDK.Interfaces;
using NuGenSVisualLib.Rendering.Chem.Structures;
using NuGenSVisualLib.Rendering.Pipelines;
using NuGenSVisualLib.Rendering.Effects;

namespace NuGenSVisualLib.Rendering.Chem.Schemes
{
    class MetaBlobsSchemeSettings : MoleculeSchemeSettings
    {
        int blobSize;

        public int BlobSize
        {
            get { return blobSize; }
            set { blobSize = value; }
        }

        public MetaBlobsSchemeSettings()
            : base(null, null, new LevelOfDetailRange(0, 0))
        { }

        public override MoleculeRenderingScheme GetScheme(Device device)
        {
            return new MetaBlobsRenderingScheme(device);
        }

        public override MoleculeSchemeSUI GetSUI()
        {
            return null;
        }

        public override object Clone()
        {
            MetaBlobsSchemeSettings settings = new MetaBlobsSchemeSettings();
            settings.atomLOD = atomLOD;
            settings.atomLodRange = atomLodRange;
            settings.renderingEffectType = renderingEffectType;
            settings.blobSize = blobSize;
            return settings;
        }

        public override RenderingEffectSettings[] GetRequiredEffects()
        {
            return new RenderingEffectSettings[] { new MetaBlobsEffectSettings() };
        }
    }

    class MetaBlobsRenderingScheme : MoleculeRenderingScheme
    {
        public MetaBlobsRenderingScheme(Device device)
            : base(device, "Meta Blobs", "", true, false, false)
        {
            this.deviceReqs = new OutputRequirements(MultiSampleType.None, DeviceType.NullReference, Format.Unknown,
                                                     true, DepthFormat.Unknown, false, null);
        }

        public override void SetStructureData(ChemEntityStructure[] structures)
        { }

        public override void SetOutputDescription(CompleteOutputDescription coDesc)
        {
            //MetaBlobsSchemeSettings settings = (MetaBlobsSchemeSettings)coDesc.SchemeSettings;

            atomsBufferCreators = new AtomGeometryCreator[] { new AtomBlobBufferCreator() };
            atomsBufferCreators[0].SetupForCreation(null);

            this.coDesc = coDesc;
        }
    }
}