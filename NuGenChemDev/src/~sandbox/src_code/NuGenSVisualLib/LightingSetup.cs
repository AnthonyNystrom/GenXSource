using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using System.IO;
using System.Xml;

namespace NuGenSVisualLib.Rendering.Lighting
{
    public abstract class Light
    {
        private string name;
        private Color clr;
        private bool enabled;
        private bool castShadows;

        public Color Clr
        {
            get { return clr; }
            set { clr = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool CastShadows
        {
            get { return castShadows; }
            set { castShadows = value; }
        }

        public abstract void ToXml(XmlWriter writer);
        protected void FromXmlNode(XmlElement lightNode)
        {
            this.name = lightNode.Attributes["name"].InnerText;
            this.enabled = (lightNode.Attributes["enabled"].InnerText == "true");
            XmlNode clrNode = lightNode.SelectSingleNode("color");
            if (clrNode != null)
            {
                if (clrNode.InnerText != null && clrNode.InnerText.Length > 0)
                    this.clr = Color.FromName(clrNode.InnerText);
                else
                {
                    int r = int.Parse(clrNode.Attributes["r"].InnerText);
                    int g = int.Parse(clrNode.Attributes["g"].InnerText);
                    int b = int.Parse(clrNode.Attributes["b"].InnerText);
                    this.clr = Color.FromArgb(r, g, b);
                }
            }
        }

        public override string ToString()
        {
            return name;
        }
    }

    public class DirectionalLight : Light
    {
        private Vector3 direction;

        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public static DirectionalLight FromXml(XmlElement lightNode)
        {
            DirectionalLight light = new DirectionalLight();
            light.FromXmlNode(lightNode);

            XmlNode direction = lightNode.SelectSingleNode("direction");
            if (direction != null)
            {
                float x = float.Parse(direction.Attributes["x"].InnerText);
                float y = float.Parse(direction.Attributes["y"].InnerText);
                float z = float.Parse(direction.Attributes["z"].InnerText);
                light.direction = new Vector3(x, y, z);
            }

            return light;
        }

        public override void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("directional");

            writer.WriteStartElement("direction");
            writer.WriteAttributeString("x", direction.X.ToString());
            writer.WriteAttributeString("y", direction.Y.ToString());
            writer.WriteAttributeString("z", direction.Z.ToString());
            writer.WriteEndElement();

            KnownColor kClr = Clr.ToKnownColor();
            if (kClr != 0)
                writer.WriteElementString("clr", kClr.ToString());
            else
            {
                writer.WriteStartElement("clr");
                writer.WriteAttributeString("r", Clr.R.ToString());
                writer.WriteAttributeString("g", Clr.G.ToString());
                writer.WriteAttributeString("b", Clr.B.ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }

    public class LightingSetup
    {
        public string name;
        public List<Light> lights;

        public LightingSetup()
        {
            lights = new List<Light>();
        }

        public static LightingSetup[] FromXml(Stream stream)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            return FromXml(doc);
        }

        public static LightingSetup[] FromXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return FromXml(doc);
        }

        public static LightingSetup[] FromXml(XmlDocument xml)
        {
            XmlNodeList setupNodes = xml.SelectNodes("configuration/lightingPresets/lightingSetup");
            if (setupNodes != null && setupNodes.Count > 0)
            {
                LightingSetup[] setups = new LightingSetup[setupNodes.Count];
                for (int i = 0; i < setupNodes.Count; i++)
                {
                    setups[i] = new LightingSetup();
                    setups[i].name = setupNodes[i].Attributes["name"].InnerText;
                    foreach (XmlNode node in setupNodes[i].ChildNodes)
                    {
                        if (node is XmlElement)
                        {
                            if (node.Name == "light")
                            {
                                string type = node.Attributes["type"].InnerText;
                                if (type == "directional")
                                    setups[i].lights.Add(DirectionalLight.FromXml((XmlElement)node));
                            }
                        }
                    }
                }
                return setups;
            }
            return null;
        }
    }
}