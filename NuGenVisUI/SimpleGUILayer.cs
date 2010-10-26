using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Resources;
using Microsoft.DirectX.Direct3D;
using Resource=Genetibase.NuGenRenderCore.Resources.Resource;

namespace Genetibase.VisUI.UI
{
    public class SimpleGUILayer : Resource, ILayer
    {
        readonly Point position;
        Size dimensions;
        bool enabled;
        bool visible;

        protected DeviceInterface devIf;
        protected Device device;
        readonly List<GUILayerItem> itemsList;
        QuadTree<GUILayerItem> itemsLayout;
        protected List<ISharableResource> checkedOutResources;
        readonly ILayoutManager layoutManager;

        public SimpleGUILayer(DeviceInterface devIf, Point position, Size dimensions,
                              string id, IResource[] dependants, ISharableResource[] dependancies)
            : base(id, dependants, dependancies)
        {
            this.devIf = devIf;
            device = devIf.Device;
            this.position = position;
            this.dimensions = dimensions;
            enabled = true;
            visible = true;
            itemsList = new List<GUILayerItem>();
            itemsLayout = new QuadTree<GUILayerItem>(dimensions.Width, dimensions.Height);
            checkedOutResources = new List<ISharableResource>();
            layoutManager = new LayoutManager();
        }

        public virtual void LoadResources()
        { }

        public virtual void UnloadResources()
        {
            // dispose of items
            foreach (GUILayerItem item in itemsList)
            {
                item.Dispose();
            }
            itemsList.Clear();
            itemsLayout.Clear();
        }

        public void AddItem(GUILayerItem item)
        {
            layoutManager.PlaceItem(item, this);

            item.Init(device);
            itemsList.Add(item);
            itemsLayout.Insert(item);
        }

        public void AddItem(GUILayerItem item, EventHandler mouseEnter, EventHandler mouseLeave,
                            MouseEventHandler mouseClick)
        {
            layoutManager.PlaceItem(item, this);

            item.Init(device);
            itemsList.Add(item);
            itemsLayout.Insert(item);

            if (mouseEnter != null)
                item.MouseEnter += mouseEnter;
            if (mouseLeave != null)
                item.MouseLeave += mouseLeave;
            if (mouseClick != null)
            {
                item.MouseClick += mouseClick;
                item.WantMouseClicks = true;
            }
        }

        public void RemoveItem(GUILayerItem item)
        {
            itemsList.Remove(item);
            itemsLayout.Remove(item);
            item.Dispose();
        }

        public void Show(bool visible)
        {
            this.visible = visible;
        }

        #region IDisposable Members

        public override void Dispose()
        {
            // check back in any shared resources
            foreach (ISharableResource rz in checkedOutResources)
            {
                rz.Owner.Checkin(rz);
            }
            checkedOutResources.Clear();
        }
        #endregion

        #region ILayer Members
        public Point Position
        {
            get { return position; }
        }

        public Size Dimensions
        {
            get { return dimensions; }
        }

        public virtual void Draw()
        {
            // draw all items
            device.BeginScene();
            foreach (GUILayerItem item in itemsList)
            {
                if (item.Visible)
                    item.Draw();
            }
            device.EndScene();
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        #endregion

        public GUILayerItem TraceCollisionPointer(Point location)
        {
            List<GUILayerItem> items = null;
            itemsLayout.CheckRadius(location.X, location.Y, ref items);
            if (items != null && items.Count > 0)
            {
                // choose last item found as it's drawn above any overlapping items in this layer
                GUILayerItem item = items[items.Count - 1];
                items.Clear();
                return item;
            }
            return null;
        }

        public void Resize(int width, int height)
        {
            dimensions = new Size(width, height);
            itemsLayout.Clear();
            itemsLayout = new QuadTree<GUILayerItem>(dimensions.Width, dimensions.Height);
            foreach (GUILayerItem item in itemsList)
            {
                layoutManager.PlaceItem(item, this);
                item.Update();
                itemsLayout.Insert(item);
            }
        }
    }
}