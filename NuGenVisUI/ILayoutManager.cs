using System.Drawing;

namespace Genetibase.VisUI.UI
{
    public class LayoutRules
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
                        xValue = layer.Dimensions.Width + rules.XAxisPositionValue - item.Dimensions.Width;
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
                        yValue = layer.Dimensions.Height + rules.YAxisPositionValue - item.Dimensions.Height;
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