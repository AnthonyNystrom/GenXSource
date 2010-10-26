using System;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.NuGenRenderCore.Resources;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.VisUI.UI
{
    public abstract class GUILayerItem : IQuadTreeItem, IDisposable
    {
        protected bool enabled, visible;
        protected Point center, origin;
        protected int radius;
        protected Size dimensions;
        protected bool wantMouseOver, wantMouseClicks;

        protected Device gDevice;

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

        public virtual void Init(Device device) { this.gDevice = device; }
        public virtual void Update() { }
        public abstract void Draw();

        public virtual void Dispose() { }
    }
}