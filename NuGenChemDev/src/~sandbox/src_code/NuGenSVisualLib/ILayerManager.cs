using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace NuGenSVisualLib.Rendering.Layers
{
    interface ILayerStack : IEnumerable<ILayer>
    {
        uint LayerCount { get; }
        IEnumerator<ILayer> Layers { get; }
        void InsertLayer(ILayer layer, uint position);
        void RemoveLayer(uint position);
        void RemoveLayer(ILayer layer);
    }

    class LayerStack : ILayerStack
    {
        readonly List<ILayer> layers;

        public LayerStack()
        {
            layers = new List<ILayer>();
        }

        #region ILayerStack Members

        public uint LayerCount
        {
            get { return (uint)layers.Count; }
        }

        public IEnumerator<ILayer> Layers
        {
            get { return layers.GetEnumerator(); }
        }

        public void InsertLayer(ILayer layer, uint level)
        {
            if (level == uint.MaxValue)
                layers.Insert(layers.Count, layer);
            else if (level == uint.MinValue)
                layers.Insert(0, layer);
            else
                layers.Insert((int)level, layer);
        }

        public void RemoveLayer(uint position)
        {
            layers.RemoveAt((int)position);
        }

        public void RemoveLayer(ILayer layer)
        {
            layers.Remove(layer);
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            layers.Clear();
        }
        #endregion

        #region IEnumerable<ILayer> Members

        public IEnumerator<ILayer> GetEnumerator()
        {
            return layers.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return layers.GetEnumerator();
        }
        #endregion
    }

    class LayoutRules
    {
        public enum Positioning
        {
            Near, Far, Center, Absolute
        }

        public enum Fill
        {
            Absolute, Stretch
        }

        public enum ValueType
        {
            Pixel,
            Percentage
        }

        public Positioning XAxisPositionRule, YAxisPositionRule;
        public ValueType XAxisPositionValueType, YAxisPositionValueType;
        public int XAxisPositionValue, YAxisPositionValue;

        public Fill XAxisFillRule, YAxisFillRule;
        public ValueType XAxisFillValueType, YAxisFillValueType;
        public int XAxisFillValue, YAxisFillValue;
    }

    interface ILayoutManager
    {
        void PlaceItem(GUILayerItem item, ILayer layer);
    }

    class LayoutManager : ILayoutManager
    {
        #region ILayoutManager Members

        public void PlaceItem(GUILayerItem item, ILayer layer)
        {
            LayoutRules rules = item.LayoutRules;

            // position
            int xValue = 0;
            switch (rules.XAxisPositionRule)
            {
                case LayoutRules.Positioning.Near:
                case LayoutRules.Positioning.Absolute:
                    xValue = rules.XAxisPositionValue;
                    break;
                case LayoutRules.Positioning.Far:
                    if (rules.XAxisPositionValueType == LayoutRules.ValueType.Percentage)
                        xValue = 100 - rules.XAxisPositionValue;
                    else
                        xValue = layer.Dimensions.Width + rules.XAxisPositionValue;
                    break;
                case LayoutRules.Positioning.Center:
                    if (rules.XAxisPositionValueType == LayoutRules.ValueType.Percentage)
                        xValue = 50 + rules.XAxisPositionValue;
                    else
                        xValue = (layer.Dimensions.Width / 2) + rules.XAxisPositionValue;
                    break;
            }
            if (rules.XAxisPositionValueType == LayoutRules.ValueType.Percentage)
                xValue = (int)(100f * (xValue / (float)layer.Dimensions.Width));
            xValue += layer.Position.X;

            int yValue = 0;
            switch (rules.YAxisPositionRule)
            {
                case LayoutRules.Positioning.Near:
                case LayoutRules.Positioning.Absolute:
                    yValue = rules.YAxisPositionValue;
                    break;
                case LayoutRules.Positioning.Far:
                    if (rules.YAxisPositionValueType == LayoutRules.ValueType.Percentage)
                        yValue = 100 - rules.YAxisPositionValue;
                    else
                        yValue = layer.Dimensions.Height + rules.YAxisPositionValue;
                    break;
                case LayoutRules.Positioning.Center:
                    if (rules.YAxisPositionValueType == LayoutRules.ValueType.Percentage)
                        yValue = 50 + rules.YAxisPositionValue;
                    else
                        yValue = (layer.Dimensions.Height / 2) + rules.YAxisPositionValue;
                    break;
            }
            if (rules.YAxisPositionValueType == LayoutRules.ValueType.Percentage)
                yValue = (int)(100f * (yValue / (float)layer.Dimensions.Height));
            yValue += layer.Position.Y;

            item.Origin = new Point(xValue, yValue);
            item.Center = new Point(xValue + (item.Dimensions.Width / 2), yValue + (item.Dimensions.Height / 2));
        }
        #endregion

        public static GUILayerItem AlignItem(GUILayerItem item, LayoutRules.Positioning xPos,
                                             LayoutRules.Positioning yPos)
        {
            item.LayoutRules.XAxisPositionRule = xPos;
            item.LayoutRules.YAxisPositionRule = yPos;
            return item;
        }
    }
}