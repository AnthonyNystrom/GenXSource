using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using NuGenSVisualLib.Settings;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Chem.Materials;

namespace NuGenSVisualLib.Rendering.Chem
{
    /// <summary>
    /// Encapsulates the description of an atoms shading
    /// </summary>
    public class AtomShadingDesc : IPropertyTreeNodeClass
    {
        private FillMode fillMode;
        private bool symbolText;
        private IMoleculeMaterialLookup moleculeMaterials;
        private bool draw = true;
        private bool blendSymbolText;

        public AtomShadingDesc() { }

        public AtomShadingDesc(AtomShadingDesc toClone)
        {
            this.fillMode = toClone.fillMode;
            this.symbolText = toClone.symbolText;
            this.moleculeMaterials = toClone.moleculeMaterials;
            this.Draw = toClone.Draw;
        }

        #region Properties

        public bool Draw
        {
            get { return draw; }
            set { draw = value; }
        }

        public FillMode FillMode
        {
            get { return fillMode; }
            set { fillMode = value; }
        }

        public bool SymbolText
        {
            get { return symbolText; }
            set { symbolText = value; }
        }

        public bool BlendSymbolText
        {
            get { return blendSymbolText; }
            set { blendSymbolText = value; }
        }

        [XmlIgnore]
        public IMoleculeMaterialLookup MoleculeMaterials
        {
            get { return moleculeMaterials; }
            set { moleculeMaterials = value; }
        }

        #endregion
    }
}
