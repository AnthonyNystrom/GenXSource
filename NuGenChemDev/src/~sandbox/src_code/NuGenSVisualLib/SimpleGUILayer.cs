using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Resources;

namespace NuGenSVisualLib.Rendering.Layers
{
    interface ILayer : IDisposable
    {
        Point Position { get; }
        Size Dimensions { get; }
        void Draw();
        bool Enabled { get; }
        bool Visible { get; }
        void Resize(int width, int height);
    }

    abstract class SimpleGUILayer : ILayer
    {
        Point position;
        Size dimensions;
        bool enabled, visible;

        protected DeviceInterface devIf;
        protected Device device;
        List<GUILayerItem> itemsList;
        QuadTree<GUILayerItem> itemsLayout;
        protected List<ISharableResource> checkedOutResources;
        ILayoutManager layoutManager;

        public SimpleGUILayer(DeviceInterface devIf, Point position, Size dimensions)
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

        public abstract void LoadResources();

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

        public void Show(bool visible)
        {
            this.visible = visible;
        }

        #region IDisposable Members

        public virtual void Dispose()
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
        }

        public bool Visible
        {
            get { return visible; }
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

    abstract class GUILayerItem : IQuadTreeItem, IDisposable
    {
        protected bool enabled, visible;
        protected Point center, origin;
        protected int radius;
        protected Size dimensions;
        protected bool wantMouseOver, wantMouseClicks;

        protected Device device;

        protected LayoutRules layoutRules;
        protected bool isMouseDown;

        public GUILayerItem(Point origin, Size dimensions)
        {
            enabled = true;
            visible = true;
            this.origin = origin;
            this.dimensions = dimensions;
            layoutRules = new LayoutRules();
            layoutRules.XAxisPositionRule = LayoutRules.Positioning.Absolute;
            layoutRules.XAxisPositionValueType = LayoutRules.ValueType.Pixel;
            layoutRules.XAxisPositionValue = origin.X;
            layoutRules.YAxisPositionRule = LayoutRules.Positioning.Absolute;
            layoutRules.YAxisPositionValueType = LayoutRules.ValueType.Pixel;
            layoutRules.YAxisPositionValue = origin.Y;

            int xDist = dimensions.Width / 2;
            int yDist = dimensions.Height / 2;
            center = new Point(origin.X + xDist, origin.Y + yDist);
            radius = (int)Math.Sqrt((xDist * xDist) + (yDist * yDist));
        }

        #region Properties

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

        public Point Center
        {
            get { return center; }
            set { center = value; }
        }

        public Point Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Size Dimensions
        {
            get { return dimensions; }
        }

        public int Radius
        {
            get { return radius; }
        }

        public bool WantMouseOver
        {
            get { return wantMouseOver; }
            set { wantMouseOver = value; }
        }

        public bool WantMouseClicks
        {
            get { return wantMouseClicks; }
            set { wantMouseClicks = value; }
        }

        public LayoutRules LayoutRules
        {
            get { return layoutRules; }
        }
        #endregion

        #region Events
        
        public event EventHandler MouseEnter;
        public event EventHandler MouseLeave;
        public event MouseEventHandler MouseClick;
        #endregion

        public virtual void OnMouseEnter() { if (MouseEnter != null) MouseEnter(this, null); }
        public virtual void OnMouseLeave() { if (MouseLeave != null) MouseLeave(this, null); }
        public virtual void OnMouseDown(MouseEventArgs e) { isMouseDown = true; }
        public virtual void OnMouseUp(MouseEventArgs e) { if (isMouseDown && MouseClick != null) MouseClick(this, e); isMouseDown = false; }

        public virtual void Init(Device device) { this.device = device; }
        public virtual void Update() { }
        public abstract void Draw();

        public virtual void Dispose() { }
    }

    class GUIImage : GUILayerItem
    {
        protected TextureResource texture;
        protected CustomVertex.TransformedTextured[] texQuad;

        public GUIImage(Point origin, Size dimensions, TextureResource texture)
            : base(origin, dimensions)
        {
            this.texture = texture;
        }

        public override void Init(Device device)
        {
            base.Init(device);

            texQuad = new CustomVertex.TransformedTextured[4];
            Update();
        }

        public override void Draw()
        {
            device.VertexFormat = CustomVertex.TransformedTextured.Format;
            device.Indices = null;
            device.SetTexture(0, texture.Texture);

            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, texQuad);
        }

        public override void Update()
        {
            texQuad[0] = new CustomVertex.TransformedTextured(origin.X, origin.Y + dimensions.Height, 0, 1, 0, 0);
            texQuad[1] = new CustomVertex.TransformedTextured(origin.X + dimensions.Width, origin.Y + dimensions.Height, 0, 1, 1, 0);
            texQuad[2] = new CustomVertex.TransformedTextured(origin.X, origin.Y, 0, 1, 0, 1);
            texQuad[3] = new CustomVertex.TransformedTextured(origin.X + dimensions.Width, origin.Y, 0, 1, 1, 1);
        }
    }

    class GUIIcon : GUIImage
    {
        TextureResource.Icon icon;
        bool highlight;
        bool isHighlighting;

        public GUIIcon(Point origin, Size dimensions, TextureResource.Icon icon,
                       bool highlight, bool enabled)
            : base(origin, dimensions, icon.Texture)
        {
            this.icon = icon;
            this.highlight = highlight;
            wantMouseOver = true;
            this.enabled = enabled;
        }

        public override void Update()
        {
            texQuad[0] = new CustomVertex.TransformedTextured(origin.X, origin.Y, 0, 1, 0, 0);
            texQuad[1] = new CustomVertex.TransformedTextured(origin.X + dimensions.Width, origin.Y, 0, 1, 0, 0);
            texQuad[2] = new CustomVertex.TransformedTextured(origin.X, origin.Y + dimensions.Height, 0, 1, 0, 0);
            texQuad[3] = new CustomVertex.TransformedTextured(origin.X + dimensions.Width, origin.Y + dimensions.Height, 0, 1, 0, 0);
        }

        public override void Draw()
        {
            device.VertexFormat = CustomVertex.TransformedTextured.Format;
            device.Indices = null;
            device.SetTexture(0, texture.Texture);

            device.TextureState[0].ColorOperation = TextureOperation.Modulate;
            device.TextureState[0].AlphaOperation = TextureOperation.Modulate;
            device.TextureState[0].ColorArgument0 = TextureArgument.TextureColor;
            device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;

            device.RenderState.AlphaBlendEnable = true;
            device.RenderState.SourceBlend = Blend.SourceAlpha;
            device.RenderState.DestinationBlend = Blend.InvSourceAlpha;

            RectangleF coords = icon.FaceCoords;
            if (enabled)
            {
                if (highlight && isHighlighting && icon.HighlightCoords != null)
                    coords = icon.HighlightCoords;
            }
            else if (icon.DisabledCoords != null)
                coords = icon.DisabledCoords;

            texQuad[0].Tu = coords.Left; texQuad[0].Tv = coords.Top;
            texQuad[1].Tu = coords.Right; texQuad[1].Tv = coords.Top;
            texQuad[2].Tu = coords.Left; texQuad[2].Tv = coords.Bottom;
            texQuad[3].Tu = coords.Right; texQuad[3].Tv = coords.Bottom;

            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, texQuad);

            device.RenderState.AlphaBlendEnable = false;
        }

        public override void OnMouseEnter()
        {
            base.OnMouseEnter();
            if (highlight)
                isHighlighting = true;
        }

        public override void OnMouseLeave()
        {
            base.OnMouseLeave();
            isHighlighting = false;
        }
    }

    class GUILabel : GUILayerItem
    {
        string text;
        Microsoft.DirectX.Direct3D.Font font;
        System.Drawing.Font fontType;
        Color clr;

        public GUILabel(string text, System.Drawing.Font font,
                        Color clr, Point position, Size dimensions)
            : base(position, dimensions)
        {
            this.text = text;
            fontType = font;
            this.clr = clr;
        }

        public override void Init(Device device)
        {
            base.Init(device);

            font = new Microsoft.DirectX.Direct3D.Font(device, fontType);
            // update dimensions
            //font.MeasureString(null, text, DrawTextFormat.RightToLeftReading, clr);
            //this.dimensions = new Size();
        }

        public override void Draw()
        {
            font.DrawText(null, text, origin.X, origin.Y, clr);
        }
    }
}