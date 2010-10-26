using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Org.OpenScience.CDK.Interfaces;
using NuGenSVisualLib.Rendering.Chem.Structures;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Rendering.Pipelines;
using NuGenSVisualLib.Rendering.Effects;

namespace NuGenSVisualLib.Rendering.Chem.Schemes
{
    class SpriteSchemeSettings : MoleculeSchemeSettings
    {
        bool absoluteSizes;
        float spriteSizes = 0.4f;

        public bool AbsoluteSizes
        {
            get { return absoluteSizes; }
            set { absoluteSizes = value; }
        }

        public float SpriteSizes
        {
            get { return spriteSizes; }
            set { spriteSizes = value; }
        }

        /// <summary>
        /// Initializes a new instance of the SpriteSchemeSettings class.
        /// </summary>
        public SpriteSchemeSettings()
            : base(null, null, new LevelOfDetailRange(0, 1))
        { }

        public override MoleculeRenderingScheme GetScheme(Device device)
        {
            return new SpriteRenderingScheme(device);
        }

        public override MoleculeSchemeSUI GetSUI()
        {
            return null;// new SpriteSchemeSUI(this);
        }

        public override object Clone()
        {
            SpriteSchemeSettings settings = new SpriteSchemeSettings();
            settings.atomLodRange = bondLodRange;
            settings.renderingEffectType = renderingEffectType;
            settings.absoluteSizes = absoluteSizes;
            settings.spriteSizes = spriteSizes;
            settings.atomLOD = atomLOD;
            return settings;
        }
    }

    class SpriteRenderingScheme : MoleculeRenderingScheme
    {
        public SpriteRenderingScheme(Device device)
            : base(device, "Sprite", "", true, false, false)
        {
            this.deviceReqs = new OutputRequirements(MultiSampleType.None, DeviceType.NullReference, Format.Unknown,
                                                     true, DepthFormat.Unknown, false, null);
        }

        public override void SetStructureData(ChemEntityStructure[] structures)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetOutputDescription(CompleteOutputDescription coDesc)
        {
            //SpriteSchemeSettings settings = (SpriteSchemeSettings)coDesc.SchemeSettings;

            atomsBufferCreators = new AtomGeometryCreator[] { new AtomSpriteBufferCreator() };
            atomsBufferCreators[0].SetupForCreation(null);

            lightBonds = false;
            lightAtoms = true;

            this.coDesc = coDesc;
        }
    }
}