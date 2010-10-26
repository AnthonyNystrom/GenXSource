using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using NuGenSVisualLib.UI.Lighting;
using System.Xml.Serialization;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using NuGenSVisualLib.Settings;

namespace NuGenSVisualLib.Rendering
{
    /// <summary>
    /// Encapsulates a description of the general lighting conditions
    /// </summary>
    public class GeneralLightingDesc : IPropertyTreeNodeClass
    {
        bool enabled;
        System.Drawing.Color ambient;
        System.Drawing.Color specular;
        bool fixedLightPositions;
//        LightWrapper[] lights;
        string loadedFromFile;

        public GeneralLightingDesc() { }

        public GeneralLightingDesc(GeneralLightingDesc toClone)
        {
            this.enabled = toClone.enabled;
            this.ambient = toClone.ambient;
            this.specular = specular;
            this.fixedLightPositions = toClone.fixedLightPositions;
            this.loadedFromFile = toClone.loadedFromFile;
        }

        #region Properties

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public System.Drawing.Color Ambient
        {
            get { return ambient; }
            set { ambient = value; }
        }

        public System.Drawing.Color Specular
        {
            get { return specular; }
            set { specular = value; }
        }

        public bool FixedLightPositions
        {
            get { return fixedLightPositions; }
            set { fixedLightPositions = value; }
        }

//        [XmlElementAttribute("Light")]
//        public LightWrapper[] Lights
//        {
//            get
//            {
//                return lights;
//            }
//            set
//            {
//                lights = value;
//            }
//        }

        [XmlIgnore()]
        public string LoadedFromFile
        {
            get { return loadedFromFile; }
            set { loadedFromFile = value; }
        }

//        public void ApplyToDevice(Device device)
//        {
//            if (device.RenderState.Lighting = enabled)
//            {
//                for (int i = 0; i < lights.Length; i++)
//                {
//                    if (lights[i] != null && lights[i].Enabled)
//                    {
//                        device.Lights[i].Type = LightType.Directional;
//                        device.Lights[i].Direction = new Vector3(lights[i].DirectionX, lights[i].DirectionY, lights[i].DirectionZ);
//                        device.Lights[i].Ambient = Color.DarkGray;
//                        device.Lights[i].Diffuse = Color.White;
//                        device.Lights[i].Specular = Color.White;
//                        device.Lights[i].Update();
//                        device.Lights[i].Enabled = true;
//                    }
//                    else
//                        device.Lights[i].Enabled = false;
//                }
//            }
//            device.RenderState.SpecularEnable = false;
//        }

//        public void UpdateLightDirections(Device device, Matrix rotation)
//        {
//            if (enabled && !fixedLightPositions)
//            {
//                for (int i = 0; i < lights.Length; i++)
//                {
//                    if (lights[i] != null && lights[i].Enabled)
//                    {
//                        device.Lights[i].Enabled = false;
//                        Vector3 dir = new Vector3(lights[i].DirectionX, lights[i].DirectionY, lights[i].DirectionZ);
//                        dir.TransformCoordinate(rotation);
//                        dir.Normalize();
//
//                        device.Lights[i].Direction = dir;
//                        device.Lights[i].Enabled = true;
//                    }
//                }
//            }
        //        }

        #endregion
    }
}