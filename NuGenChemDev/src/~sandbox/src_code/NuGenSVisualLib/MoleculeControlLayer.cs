using System;
using System.Collections.Generic;
using System.Text;
using NuGenSVisualLib.Rendering.Pipelines;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Resources;
using System.Drawing;
using System.Windows.Forms;

namespace NuGenSVisualLib.Rendering.Layers.Chem
{
    class MoleculeControlLayer : SimpleGUILayer
    {
        bool showXYZIcons;
        int xyzAxisOptions;
        TextureResource iconsTexture;
        TextureResource.Icon xAxisIcon, yAxisIcon, zAxisIcon;
        TextureResource.Icon tickIcon, crossIcon;

        public MoleculeControlLayer(DeviceInterface devIf, Point position, Size dimensions)
            : base(devIf, position, dimensions)
        {
        }

        public override void LoadResources()
        {
            // load icons
            iconsTexture = (TextureResource)devIf.GetSharedResource("file://Media/UI/InViewIconsX16.png", ref checkedOutResources, devIf);
            // ^need??
            xAxisIcon = (TextureResource.Icon)devIf.GetSharedResource("file://Media/UI/InViewIconsX16.png:areas:icon:xAxis", ref checkedOutResources, devIf);
            yAxisIcon = (TextureResource.Icon)devIf.GetSharedResource("file://Media/UI/InViewIconsX16.png:areas:icon:yAxis", ref checkedOutResources, devIf);
            zAxisIcon = (TextureResource.Icon)devIf.GetSharedResource("file://Media/UI/InViewIconsX16.png:areas:icon:zAxis", ref checkedOutResources, devIf);
            tickIcon = (TextureResource.Icon)devIf.GetSharedResource("file://Media/UI/InViewIconsX16.png:areas:icon:tick", ref checkedOutResources, devIf);
            crossIcon = (TextureResource.Icon)devIf.GetSharedResource("file://Media/UI/InViewIconsX16.png:areas:icon:cross", ref checkedOutResources, devIf);

            // create GUI items
            // TODO: Needs layout manager support
            AddItem(new GUIIcon(new Point(10, 10), new Size(16, 16), xAxisIcon, true, true));
            AddItem(new GUIIcon(new Point(32, 10), new Size(16, 16), yAxisIcon, true, true));
            AddItem(new GUIIcon(new Point(54, 10), new Size(16, 16), zAxisIcon, true, true));

            AddItem(LayoutManager.AlignItem(new GUIIcon(new Point(-48, -50),
                                                        new Size(16, 16), tickIcon, true, false),
                                            LayoutRules.Positioning.Far,
                                            LayoutRules.Positioning.Far),
                    null, null, OnTickClick);
            AddItem(LayoutManager.AlignItem(new GUIIcon(new Point(-32, -50),
                                                        new Size(16, 16), crossIcon, true, false),
                                            LayoutRules.Positioning.Far,
                                            LayoutRules.Positioning.Far));
        }

        public override void UnloadResources()
        {
        }

        public override void Dispose()
        {
        }

        public bool ShowXYZIcons
        {
            get { return showXYZIcons; }
            set { showXYZIcons = value; }
        }

        public int XyzAxisOptions
        {
            get { return xyzAxisOptions; }
            set { xyzAxisOptions = value; }
        }

        void OnTickClick(object sender, MouseEventArgs args)
        {
        }
    }
}