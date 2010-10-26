using System;
using System.Collections.Generic;
using System.Text;
using NuGenSVisualLib.Rendering.Devices;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Chem.Structures;
using NuGenSVisualLib.Rendering.Effects;

namespace NuGenSVisualLib.Rendering.Chem.Schemes
{
    class SpaceFillSchemeSettings : MoleculeSchemeSettings
    {
        bool translucent = false;

        public bool Translucent
        {
            get { return translucent; }
            set { translucent = value; }
        }

        public SpaceFillSchemeSettings()
            : base(null, null, new LevelOfDetailRange(0, 4))
        { }

        public override MoleculeRenderingScheme GetScheme(Device device)
        {
            return new SpaceFillScheme(device);
        }

        public override MoleculeSchemeSUI GetSUI()
        {
            return null;
        }

        public override object Clone()
        {
            SpaceFillSchemeSettings settings = new SpaceFillSchemeSettings();
            settings.atomLOD = atomLOD;
            settings.atomLodRange = atomLodRange;
            settings.renderingEffectType = renderingEffectType;
            settings.translucent = translucent;
            return settings;
        }
    }

    class SpaceFillScheme : MoleculeRenderingScheme
    {
        public SpaceFillScheme(Device device)
            : base(device, "Space Fill", "", true, false, false)
        {
            this.deviceReqs = new OutputRequirements();
        }

        public override void SetStructureData(ChemEntityStructure[] structures)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetOutputDescription(CompleteOutputDescription coDesc)
        {
            //SpaceFillSchemeSettings settings = (SpaceFillSchemeSettings)coDesc.SchemeSettings;

            atomsBufferCreators = new AtomGeometryCreator[] { new AtomSphereBufferCreator(0.5f, 16, 12) };
            atomsBufferCreators[0].SetupForCreation(null);

            bondsBufferCreators = null;

            lightAtoms = true;

            this.coDesc = coDesc;
        }
    }
}
