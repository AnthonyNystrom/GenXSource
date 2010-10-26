using System;
using System.Drawing;
using System.Xml;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Logging;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenRenderCore.Resources
{
    public class TextureResource : Resource
    {
        readonly Texture texture;
        Icon[] icons;
        readonly int width;
        readonly int height;

        public class Icon : Resource
        {
            public TextureResource Texture;
            public Rectangle FacePixels, DisabledPixels, HighlightPixels;
            public RectangleF FaceCoords, DisabledCoords, HighlightCoords;

            public Icon(string id, TextureResource texture,
                        Rectangle facePixels, RectangleF faceCoords)
                : base(id, null, null)
            {
                Texture = texture;
                FacePixels = facePixels;
                FaceCoords = faceCoords;
            }

            #region IDisposable Members

            public override void Dispose()
            { }
            #endregion
        }

        public TextureResource(string id, Texture texture, Icon[] icons,
                               int width, int height)
            : base(id, icons, null)
        {
            this.texture = texture;
            this.icons = icons;
            this.width = width;
            this.height = height;
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public Icon[] Icons
        {
            get { return icons; }
            set { icons = value; }
        }

        public Texture Texture
        {
            get { return texture; }
        }

        #region IDisposable Members

        public override void Dispose()
        {
            if (texture != null)
                texture.Dispose();
        }
        #endregion
    }

    public class ImageContentLoader : RzContentTypeLoader
    {
        public ImageContentLoader()
            : base(new string[] { "image/x-ms-bmp", "image/jpeg", "image/png", "image/dds" },
                   new string[] { ".bmp", ".jpg", ".jpeg", ".png", ".dds" },
                   new int[] { 0, 1, 1, 2, 3 })
        { }

        public override IResource LoadContent(string filePath, string rzPath, string subPath,
                                              string contentType, XmlNodeList rzNodes,
                                              out IResource[] loadedDependants,
                                              out IResource[] loadedDependancies,
                                              DeviceInterface devIf)
        {
            ILog log = devIf.CDI.GeneralLog;
            log.AddItem(new LogItem(string.Format("Starting loading texture rz ([{0}]{1})", contentType, rzPath), LogItem.ItemLevel.DebugInfo));

            // load texture first
            ImageInformation imgInf = TextureLoader.ImageInformationFromFile(filePath);
            Texture texture = TextureLoader.FromFile(devIf.Device, filePath, imgInf.Width, imgInf.Height, 1, Usage.None,
                                                     imgInf.Format, Pool.Managed, Filter.None, Filter.None, 0);

            log.AddItem(new LogItem(string.Format("Loaded texture rz ([{0}]{1})", contentType, rzPath), LogItem.ItemLevel.DebugInfo));

            // process meta-data nodes
            TextureResource texRz = new TextureResource(rzPath, texture, null,
                                                        imgInf.Width, imgInf.Height);
            TextureResource.Icon[] icons = null;
            TextureResource.Icon icon = null;
            if (rzNodes != null)
            {
                foreach (XmlElement item in rzNodes)
                {
                    if (item.Name == "areas")
                    {
                        // process sub-areas
                        ProcessSubAreas(item, texRz, out icons, rzPath + ":areas:icon:");
                    }
                }
            }
            texRz.Icons = icons;

            /*if (subPath != null)
            {
                loadedDependants = icons;
                loadedDependancies = new IResource[] { texRz };
                return 
            }
            else
            {*/
                loadedDependancies = null;
                loadedDependants = icons;
                return texRz;
            //}
        }

        private static void ProcessSubAreas(XmlNode areas, TextureResource texRz, out TextureResource.Icon[] icons,
                                            string preamble)
        {
            XmlNodeList iconAreas = areas.SelectNodes("icon");
            if (iconAreas != null && iconAreas.Count > 0)
            {
                icons = new TextureResource.Icon[iconAreas.Count];
                int iconIdx = 0;
                foreach (XmlElement icon in iconAreas)
                {
                    string id = icon.Attributes["id"].InnerText;
                    // parse coords
                    XmlNodeList coords = icon.SelectNodes("coords");
                    if (coords == null || coords.Count == 0)
                        throw new Exception("Icon found with no coords");
                    icons[iconIdx] = new TextureResource.Icon(preamble + id, texRz, Rectangle.Empty,
                                                              RectangleF.Empty);

                    foreach (XmlElement coord in coords)
                    {
                        string coordId = coord.Attributes["id"].InnerText;
                        int u = int.Parse(coord.Attributes["u"].InnerText);
                        int v = int.Parse(coord.Attributes["v"].InnerText);
                        int w = int.Parse(coord.Attributes["w"].InnerText);
                        int h = int.Parse(coord.Attributes["h"].InnerText);
                        Rectangle pixels = new Rectangle(u, v, w, h);
                        RectangleF coordsR = new RectangleF(u / (float)texRz.Width,
                                                            v / (float)texRz.Height,
                                                            w / (float)texRz.Width,
                                                            h / (float)texRz.Height);
                        if (coordId == "face")
                        {
                            icons[iconIdx].FacePixels = pixels;
                            icons[iconIdx].FaceCoords = coordsR;
                        }
                        else if (coordId == "disabled")
                        {
                            icons[iconIdx].DisabledPixels = pixels;
                            icons[iconIdx].DisabledCoords = coordsR;
                        }
                        else if (coordId == "highlight")
                        {
                            icons[iconIdx].HighlightPixels = pixels;
                            icons[iconIdx].HighlightCoords = coordsR;
                        }
                    }
                    iconIdx++;
                }
            }
            else
                icons = null;
        }
    }
}