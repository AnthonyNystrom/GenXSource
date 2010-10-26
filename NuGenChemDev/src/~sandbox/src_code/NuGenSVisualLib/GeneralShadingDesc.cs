using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Settings;


namespace NuGenSVisualLib.Rendering.Shading
{
    /// <summary>
    /// Encapsulates a description for the general shading
    /// </summary>
    [XmlRootAttribute(ElementName = "generalShadingDesc", IsNullable = false)]
    public class GeneralShadingDesc : IPropertyTreeNodeClass
    {
        private MultiSampleType antiAliasing;
        private bool wireframe;
        private System.Drawing.Color bgColor;
        private Texture bgImg;
        private Size bgImgSize;
        private ShadingModes shadingMode;
        private string backgroundImgFilename;
        private ImageAlignment backgroundImgAlign;

        public enum ShadingModes
        {
            Flat,
            Smooth
        }

        public enum ImageAlignment
        {
            Centre,
            Tile,
            Stretch
        }

        public GeneralShadingDesc()
        { }

        public GeneralShadingDesc(GeneralShadingDesc toClone)
        {
            this.antiAliasing = toClone.antiAliasing;
            this.wireframe = toClone.wireframe;
            this.bgColor = toClone.bgColor;
            this.bgImg = toClone.bgImg;
            this.bgImgSize = toClone.bgImgSize;
            this.shadingMode = toClone.shadingMode;
            this.backgroundImgFilename = toClone.backgroundImgFilename;
            this.backgroundImgAlign = toClone.backgroundImgAlign;
        }

        #region Properties

        public MultiSampleType AntiAliasing
        {
            get { return antiAliasing; }
            set { antiAliasing = value; }
        }

        public bool Wireframe
        {
            get { return wireframe; }
            set { wireframe = value; }
        }

        public System.Drawing.Color BackgroundColor
        {
            get {return bgColor; }
            set { bgColor = value; }
        }

        [XmlIgnore]
        public Texture BackgroundImg
        {
            get { return bgImg; }
            set { bgImg = value; }
        }

        public Size BackgroundImgSize
        {
            get { return bgImgSize; }
            set { bgImgSize = value; }
        }

        public string BackgroundImgFilename
        {
            get { return backgroundImgFilename; }
            set { backgroundImgFilename = value; }
        }

        public ImageAlignment BackgroundImgAlignment
        {
            get { return backgroundImgAlign; }
            set { backgroundImgAlign = value; }
        }

        public ShadingModes ShadingMode
        {
            get { return shadingMode; }
            set { shadingMode = value; }
        }

        #endregion
    }
}
