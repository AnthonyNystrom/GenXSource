using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Org.OpenScience.CDK.Interfaces;
using NuGenSVisualLib.Rendering.Chem.Structures;
using NuGenSVisualLib.Rendering.Pipelines;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Rendering.Effects;

namespace NuGenSVisualLib.Rendering.Chem.Schemes
{
    class BallAndStickSchemeSettings : MoleculeSchemeSettings
    {
        int stickThickness;
        bool drawAtoms = true;
        int glow;
        bool electronChargeCloud;

        public int StickThickness
        {
            get { return stickThickness; }
            set { stickThickness = value; }
        }
        
        public int Glow
        {
            get { return glow; }
            set { glow = value; }
        }

        public bool ElectronChargeCloud
        {
            get { return electronChargeCloud; }
            set { electronChargeCloud = value; }
        }

        public BallAndStickSchemeSettings()
            : base(null, new LevelOfDetailRange(0, 2), new LevelOfDetailRange(0, 5))
        { }

        #region MoleculeSchemeSettings Overrides
        public override MoleculeRenderingScheme GetScheme(Device device)
        {
            return new BallAndStickRenderingScheme(device);
        }

        public override MoleculeSchemeSUI GetSUI()
        {
            return new BallAndStickSchemeSUI(this);
        }

        public override object Clone()
        {
            BallAndStickSchemeSettings settings = new BallAndStickSchemeSettings();
            settings.bondLodRange = bondLodRange;
            settings.renderingEffectType = renderingEffectType;
            settings.stickThickness = stickThickness;
            settings.glow = glow;
            settings.electronChargeCloud = electronChargeCloud;

            settings.drawAtoms = drawAtoms;
            settings.atomLOD = atomLOD;
            settings.bondLOD = bondLOD;
            settings.atomLodRange = atomLodRange;
            return settings;
        }
        #endregion
    }

    class BallAndStickRenderingScheme : MoleculeRenderingScheme
    {
        public BallAndStickRenderingScheme(Device device)
            : base(device, "Ball and Stick", "", true, true, true)
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
            bool updateAtomLOD = true;
            bool updateBondLOD = true;
            bool updateSymbols = true;
            bool updateGlow = true;

            bool updateBondSize = true;

            bool updateBonds = true;
            bool updateAtoms = true;
            bool updateScheme = true;

            if (this.coDesc != null)
            {
                this.coDesc = coDesc;
                updateAtoms = coDesc.CheckChange("AtomShadingDesc.");
                if (updateAtoms)
                {
                    updateSymbols = coDesc.CheckChange("AtomShadingDesc.", "SymbolText");
                    updateGlow = coDesc.CheckChange("SchemeSettings.", "Glow");
                }
                updateBonds = coDesc.CheckChange("BondShadingDesc.");

                updateScheme = coDesc.CheckChange("SchemeSettings.");
                if (updateScheme)
                {
                    updateAtomLOD = coDesc.CheckChange("SchemeSettings.", "AtomLOD");
                    updateBondLOD = coDesc.CheckChange("SchemeSettings.", "BondLOD");
                    
                    updateBondSize = coDesc.CheckChange("SchemeSettings.", "StickThickness");
                }
            }
            else
                this.coDesc = coDesc;
            BallAndStickSchemeSettings settings = (BallAndStickSchemeSettings)coDesc.SchemeSettings;

            int atomDetail1 = 4;
            int atomDetail2 = 1;
            if (updateAtoms || updateScheme)
            {
                /*if (updateAtomLOD)
                {*/
                    switch (settings.AtomLOD)
                    {
                        case 0:
                            atomDetail1 = 4;
                            atomDetail2 = 1;
                            break;
                        case 1:
                            atomDetail1 = 8;
                            atomDetail2 = 6;
                            break;
                        case 2:
                            atomDetail1 = 12;
                            atomDetail2 = 9;
                            break;
                        case 3:
                            atomDetail1 = 16;
                            atomDetail2 = 12;
                            break;
                        case 4:
                            atomDetail1 = 24;
                            atomDetail2 = 18;
                            break;
                        case 5:
                            atomDetail1 = 32;
                            atomDetail2 = 26;
                            break;
                    }
                //}
                int bufCount = 1;
                if (settings.ElectronChargeCloud)
                    bufCount++;
                if (settings.Glow > 0)
                    bufCount++;
                if (coDesc.AtomShadingDesc.SymbolText)
                    bufCount++;

                // TODO: Reconfigure previous creators
                atomsBufferCreators = new AtomGeometryCreator[bufCount];
                atomsBufferCreators[0] = new AtomSphereBufferCreator(0.2f, atomDetail1, atomDetail2);
                bufCount = 1;
                if (settings.Glow > 0)
                    atomsBufferCreators[bufCount++] = new AtomSpriteBufferCreator();
                if (settings.ElectronChargeCloud)
                    atomsBufferCreators[bufCount++] = new AtomMetaballsBufferCreator(true);
                if (coDesc.AtomShadingDesc.SymbolText)
                    atomsBufferCreators[bufCount++] = new AtomSymbolSpriteBufferCreator();

                foreach (AtomGeometryCreator creator in atomsBufferCreators)
                {
                    creator.SetupForCreation(null);
                }
            }

            if (updateBonds || updateScheme)
            {
                if (updateBondSize)
                {
                    if (settings.StickThickness == 0 && (bondsBufferCreators == null || !(bondsBufferCreators[0] is BondLinesBufferCreator)))
                        bondsBufferCreators = new BondGeometryCreator[] { new BondLinesBufferCreator() };
                    else if (settings.StickThickness > 0 && (bondsBufferCreators == null || !(bondsBufferCreators[0] is BondThickLinesBufferCreator)))
                        bondsBufferCreators = new BondGeometryCreator[] { new BondThickLinesBufferCreator(5) };
                    bondsBufferCreators[0].SetupForCreation(null);
                }
            }

            lightBonds = false;
            lightAtoms = true;

            this.coDesc = coDesc;
        }
    }
}