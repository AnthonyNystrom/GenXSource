using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Rendering.Chem.Structures;
using NuGenSVisualLib.Rendering.Effects;

namespace NuGenSVisualLib.Rendering.Chem.Schemes
{
    class MetaballSchemeSettings : MoleculeSchemeSettings
    {
        /// <summary>
        /// Initializes a new instance of the MetaballSchemeSettings class.
        /// </summary>
        public MetaballSchemeSettings()
            : base(null, null, new LevelOfDetailRange(0, 3))
        { }

        public override MoleculeRenderingScheme GetScheme(Device device)
        {
            return new MetaballRenderingScheme(device);
        }

        public override MoleculeSchemeSUI GetSUI()
        {
            return null;
        }

        public override object Clone()
        {
            MetaballSchemeSettings settings = new MetaballSchemeSettings();
            settings.atomLodRange = atomLodRange;
            settings.renderingEffectType = renderingEffectType;
            settings.atomLOD = atomLOD;
            return settings;
        }
    }

    class MetaballRenderingScheme : MoleculeRenderingScheme
    {
        public MetaballRenderingScheme(Device device)
            : base(device, "Metaballs", "", true, false, false)
        {
            this.deviceReqs = new OutputRequirements();
        }

        public override void SetStructureData(ChemEntityStructure[] structures)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetOutputDescription(CompleteOutputDescription coDesc)
        {
            //MetaballSchemeSettings settings = (MetaballSchemeSettings)coDesc.SchemeSettings;

            atomsBufferCreators = new AtomGeometryCreator[] { new AtomMetaballsBufferCreator(false) };
            atomsBufferCreators[0].SetupForCreation(null);

            bondsBufferCreators = null;

            lightBonds = false;
            lightAtoms = true;

            this.coDesc = coDesc;
        }
    }
}
