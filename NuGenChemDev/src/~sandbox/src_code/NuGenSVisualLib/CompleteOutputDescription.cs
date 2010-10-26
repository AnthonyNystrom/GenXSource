using System;
using System.Collections.Generic;
using System.Text;
using NuGenSVisualLib.Rendering.Shading;
using NuGenSVisualLib.Rendering.Chem;
using NuGenSVisualLib.Rendering;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using NuGenSVisualLib.Settings;
using NuGenSVisualLib.Rendering.Chem.Materials;

namespace NuGenSVisualLib.Rendering
{
    /// <summary>
    /// Encapsulates a complete description of the output requested
    /// </summary>
    [XmlRootAttribute(ElementName = "CompleteOutputDescName", IsNullable = false)]
    public class CompleteOutputDescription : PropertyTree
    {
        private GeneralShadingDesc generalShadingDesc;
        private AtomShadingDesc atomShadingDesc;
        private BondShadingDesc bondShadingDesc;
        private GeneralLightingDesc generalLightingDesc;
        private GeneralStructuresShadingDesc structuresShadingDesc;

        private MoleculeSchemeSettings schemeSettings;

        public CompleteOutputDescription()
        {
            /*generalShadingDesc = new GeneralShadingDesc();
            generalLightingDesc = new GeneralLightingDesc();
            atomShadingDesc = new AtomShadingDesc();
            bondShadingDesc = new BondShadingDesc();
            structuresShadingDesc = new GeneralStructuresShadingDesc();*/
        }

        public CompleteOutputDescription(CompleteOutputDescription toClone)
        {
            generalShadingDesc = new GeneralShadingDesc(toClone.GeneralShadingDesc);
            generalLightingDesc = new GeneralLightingDesc(toClone.GeneralLightingDesc);
            atomShadingDesc = new AtomShadingDesc(toClone.AtomShadingDesc);
            bondShadingDesc = new BondShadingDesc(toClone.BondShadingDesc);
            //structuresShadingDesc = new GeneralStructuresShadingDesc(toClone.GeneralStructuresShadingDesc);
            schemeSettings = (MoleculeSchemeSettings)toClone.schemeSettings.Clone();
        }

        public static CompleteOutputDescription New()
        {
            CompleteOutputDescription coDesc = new CompleteOutputDescription();
            coDesc.generalShadingDesc = new GeneralShadingDesc();
            coDesc.generalLightingDesc = new GeneralLightingDesc();
            coDesc.atomShadingDesc = new AtomShadingDesc();
            coDesc.bondShadingDesc = new BondShadingDesc();
            coDesc.structuresShadingDesc = new GeneralStructuresShadingDesc();
            return coDesc;
        }

        #region Properties

        public GeneralShadingDesc GeneralShadingDesc
        {
            get { return generalShadingDesc; }
            set { UpdateLeafNode(generalShadingDesc, value); generalShadingDesc = value; }
        }

        public AtomShadingDesc AtomShadingDesc
        {
            get { return atomShadingDesc; }
            set { UpdateLeafNode(atomShadingDesc, value); atomShadingDesc = value; }
        }

        public BondShadingDesc BondShadingDesc
        {
            get { return bondShadingDesc; }
            set { UpdateLeafNode(bondShadingDesc, value); bondShadingDesc = value; }
        }

        public GeneralLightingDesc GeneralLightingDesc
        {
            get { return generalLightingDesc; }
            set { UpdateLeafNode(generalLightingDesc, value); generalLightingDesc = value; }
        }

        [XmlIgnore]
        public MoleculeSchemeSettings SchemeSettings
        {
            get { return schemeSettings; }
            set { UpdateLeafNode(schemeSettings, value); schemeSettings = value; }
        }

        [XmlIgnore]
        public GeneralStructuresShadingDesc GeneralStructuresShadingDesc
        {
            get { return structuresShadingDesc; }
            set { structuresShadingDesc = value; }
        }
        #endregion

        #region Xml I/O

        public void ToFile(string filename)
        {
            FileStream file = new FileStream(filename, FileMode.Create);
            XmlSerializer xs = new XmlSerializer(typeof(CompleteOutputDescription));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(file, Encoding.UTF8);

            xs.Serialize(xmlTextWriter, this);

            xmlTextWriter.Close();
            file.Close();
        }

        public static CompleteOutputDescription LoadDescription(string filename)
        {
            FileStream file = new FileStream(filename, FileMode.Open);
            CompleteOutputDescription desc = LoadDescription(file);
            file.Close();
            return desc;
        }

        public static CompleteOutputDescription LoadDescription(Stream stream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(CompleteOutputDescription));
            XmlReader xmlreader = XmlReader.Create(stream);
            return (CompleteOutputDescription)xs.Deserialize(xmlreader);
        }
        #endregion
    }
}
