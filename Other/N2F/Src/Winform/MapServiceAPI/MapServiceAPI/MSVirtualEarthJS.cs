using System;
using System.Text;
using System.Windows.Browser;

namespace MapServiceAPI
{/*
    public class MSVirtualEarthJS:IMapServiceJS
    {
        #region IMapServiceJS Members

        public void MapInit(string mapJsObj, string sl_control)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("silverLightControl = document.getElementById('silverlightControl');");
            sb.Append("map = new VEMap('myMap'); map.LoadMap();");
            sb.Append("function ClickDispatch(e){if(e.elementID){var shape = map.GetShapeByID(e.elementID);if(shape.ClickHandler){shape.ClickHandler();}}}");
            sb.Append("map.AttachEvent('onclick',ClickDispatch);");
            HtmlPage.Window.Eval(sb.ToString());
        }
        public void MapViewChangeInit(string mapJsObj, string sl_contro)
        {
        }
        public void MapShapeClickInit(string mapJsObj)
        {
        }
        public void MapSetStyle(MapStyle style, string mapJsObj)
        {
            switch (style)
            {
                case MapStyle.HYBRID:
                    break;
                case MapStyle.STREET:
                    break;
                case MapStyle.SATELLITE:
                    break;
                default:
                    break;
            }
        }
        public void MapSetCenterZoom(LatLng center, Int32 zoom, string mapJsObj)
        {
            HtmlPage.Window.Eval("map.SetCenterAndZoom(new VELatLong(" + center.ToString() + ")," + zoom.ToString() + ");");
        }
        public void MapAddShape(MapShapes shape, string mapJsObj)
        {
            HtmlPage.Window.Eval("map.AddShape(" + shape.Id + ");");
        }
        public void MapDeleteShape(MapShapes shape, string mapJsObj)
        {
            HtmlPage.Window.Eval("map.DeleteShape(" + shape.Id + ");");
        }
        public void MapUnload() { }

        public void ShapeMarker(MapShapes shape, LatLng point, String title, String iconSource)
        {
            HtmlPage.Window.Eval("var " + shape.Id + " = new VEShape(VEShapeType.Pushpin, new VELatLong(" + point.ToString() + "));");
            HtmlPage.Window.Eval(shape.Id + ".SetTitle('" + title + "');");
            if (iconSource != null)
            {
                HtmlPage.Window.Eval(shape.Id + ".SetCustomIcon(\"<img src='" + iconSource + "' />\");");
            }
        }
        public void ShapeMarkerSetPoint(MapShapes shape, LatLng point)
        {
            HtmlPage.Window.Eval(shape.Id + ".SetPoints(new VELatLong(" + point.ToString() + "));");
        }
        public void ShapeInitClickHandler(MapShapes shape)
        {
            HtmlPage.Window.Eval(shape.Id + ".ClickHandler = function(){silverLightControl.Content.MapEvent.OnShapeClick('" + shape.Id + "');}");
        }
        public void ShapeDispose(MapShapes shape)
        {
            HtmlPage.Window.Eval(shape.Id + " = null;");
        }

        #endregion
    }*/
}
