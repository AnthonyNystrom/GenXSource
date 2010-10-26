using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using NuGenSVisualLib.Settings;

namespace NuGenSVisualLib.Rendering.Chem
{
    /// <summary>
    /// Encapsulates the general shading for structures
    /// </summary>
    public class GeneralStructuresShadingDesc
    {
        private StructuresShadingDesc defaultShadingDesc;
        private RibbonsShadingDesc ribbonsShadingDesc;

        public GeneralStructuresShadingDesc()
        {
            defaultShadingDesc = new StructuresShadingDesc();
            ribbonsShadingDesc = new RibbonsShadingDesc();
        }
        
        public RibbonsShadingDesc RibbonsShadingDesc
        {
            get { return ribbonsShadingDesc; }
            set { ribbonsShadingDesc = value; }
        }

        public StructuresShadingDesc DefaultShadingDesc
        {
            get { return defaultShadingDesc; }
            set { defaultShadingDesc = value; }
        }
    }

    public class StructuresShadingDesc
    {
        protected bool translucent;
        protected bool useClrAxis;
        protected ColorAxis3D clrAxis;
        protected Color defaultClr;
        protected bool doubleSided;

        public Dictionary<string, bool> PropertiesInUse;

        public StructuresShadingDesc()
        {
            PropertiesInUse = new Dictionary<string, bool>();
            //PropertiesInUse.Add("UseClrAxis", true);
            //clrAxis = new ColorAxis3D();
            //clrAxis.YEnabled = true;
            //clrAxis.Ya = Color.Blue;
            //clrAxis.Yb = Color.White;
            //defaultClr = Color.DarkGray;
        }

        public bool Translucent
        {
            get { return translucent; }
            set { translucent = value; }
        }

        public bool UseClrAxis
        {
            get { return useClrAxis; }
            set { useClrAxis = value; }
        }

        public ColorAxis3D Clrs
        {
            get { return clrAxis; }
            set { clrAxis = value; }
        }

        public Color DefaultClr
        {
            get { return defaultClr; }
            set { defaultClr = value; }
        }

        public bool DoubleSided
        {
            get { return doubleSided; }
            set { doubleSided = value; }
        }
    }

    public class ColorAxis3D
    {
        public Color Xa, Xb;
        public Color Ya, Yb;
        public Color Za, Zb;

        public bool XEnabled;
        public bool YEnabled;
        public bool ZEnabled;
    }

    /// <summary>
    /// Encapsulates the shading for ribbon structures
    /// </summary>
    public class RibbonsShadingDesc : StructuresShadingDesc
    {
        public enum Shading
        {
            Solid,
            Edges
        }

        private Shading shading;

        public Shading ShadingType
        {
            get { return shading; }
            set { shading = value; }
        }
    }
}