using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using NuGenSVisualLib.Settings;
using NuGenSVisualLib.Rendering.Chem.Materials;

namespace NuGenSVisualLib.Rendering.Chem
{
    /// <summary>
    /// Encapsulates the description of a bonds shading
    /// </summary>
    public class BondShadingDesc : IPropertyTreeNodeClass
    {
        public enum BondSpacings
        {
            AtoB = 0,
            CenterSpace = 1
        }

        public enum BondEndTypes
        {
            Open = 0,
            Closed = 1,
            Point = 2,
            Rounded = 3,
            RoundedAtEndsOnly = 4
        }

        private bool wireframe;
        private IMoleculeMaterialLookup moleculeMaterials;
        private bool shadeBothColors;
        private bool bondOrderAsMultipleBonds;
        private int width;
        private bool blendEndClrs = false;
        private BondSpacings spacing = BondSpacings.AtoB;
        private BondEndTypes endType = BondEndTypes.Point;
        private bool draw = true;

        public BondShadingDesc() { }

        public BondShadingDesc(BondShadingDesc toClone)
        {
            this.wireframe = toClone.wireframe;
            this.moleculeMaterials = toClone.moleculeMaterials;
            this.shadeBothColors = toClone.shadeBothColors;
            this.bondOrderAsMultipleBonds = toClone.bondOrderAsMultipleBonds;
            this.width = toClone.width;
            this.draw = toClone.draw;
        }

        #region Properties
        public bool Wireframe
        {
            get { return wireframe; }
            set { wireframe = value; }
        }

        public bool ShadeBothColors
        {
            get { return shadeBothColors; }
            set { shadeBothColors = value; }
        }

        /// <summary>
        /// If the bond order should be displayed as multiple bonds
        /// </summary>
        public bool BondOrderAsMultipleBonds
        {
            get { return bondOrderAsMultipleBonds; }
            set { bondOrderAsMultipleBonds = value; }
        }

        [XmlIgnore]
        public IMoleculeMaterialLookup MoleculeMaterials
        {
            get { return moleculeMaterials; }
            set { moleculeMaterials = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// If the end colours should be blended or distinct
        /// </summary>
        public bool BlendEndClrs
        {
            get { return blendEndClrs; }
            set { blendEndClrs = value; }
        }

        public BondSpacings Spacing
        {
            get { return spacing; }
            set { spacing = value; }
        }

        public BondEndTypes EndType
        {
            get { return endType; }
            set { endType = value; }
        }

        public bool Draw
        {
            get { return draw; }
            set { draw = value; }
        }
        #endregion
    }
}