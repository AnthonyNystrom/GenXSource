using System;
using System.Text;
using System.Windows.Browser;

namespace MapServiceAPI
{
    public class GoogleMapJS:IMapServiceJS
    {
        private string sl_control;
        public GoogleMapJS(string silverLightControlID)
        {
            sl_control = silverLightControlID;
        }
        #region IMapServiceJS Members

        public void MapInit(Map mapJsObj)
        {
            HtmlPage.Window.Eval("var " + mapJsObj.Id + " = new GMap2(document.getElementById('" + mapJsObj.Id + "'));");
            HtmlPage.Window.Eval("var " + sl_control + " = document.getElementById('" + sl_control + "');");
            HtmlPage.Window.Eval(mapJsObj.Id + ".enableScrollWheelZoom();");
            
            HtmlPage.Window.Eval(mapJsObj.Id + ".ViewChangeHandler = function(){var center = "+mapJsObj.Id+".getCenter(); " + sl_control + ".Content.MapEvent.OnMapViewChange('" + mapJsObj.Id + "',center.lat(),center.lng());}");
            HtmlPage.Window.Eval(mapJsObj.Id + ".ZoomChangeHandler = function(oldLevel, newLevel){" + sl_control + ".Content.MapEvent.OnZoom('" + mapJsObj.Id + "',newLevel);}");
            HtmlPage.Window.Eval(mapJsObj.Id + ".ClickHandler = function(overlay, point){try{" + sl_control + ".Content.MapEvent.OnMapClick('" + mapJsObj.Id + "',point.lat(),point.lng());}catch(ex){} "
                + "try{overlay.ClickHandler('" + mapJsObj.Id + "');}catch(ex){}}");
            HtmlPage.Window.Eval("GEvent.addListener(" + mapJsObj.Id + ", 'moveend', " + mapJsObj.Id + ".ViewChangeHandler);");
            HtmlPage.Window.Eval("GEvent.addListener(" + mapJsObj.Id + ", 'zoomend', " + mapJsObj.Id + ".ZoomChangeHandler);");
            HtmlPage.Window.Eval("GEvent.addListener(" + mapJsObj.Id + ", 'click', " + mapJsObj.Id + ".ClickHandler);");
        }




        public void MapSetStyle(MapStyle style, Map mapJsObj)
        {
            switch (style)
            {
                case MapStyle.HYBRID:
                    HtmlPage.Window.Eval(mapJsObj.Id + ".setMapType(G_HYBRID_MAP);");
                    break;
                case MapStyle.STREET:
                    HtmlPage.Window.Eval(mapJsObj.Id + ".setMapType(G_NORMAL_MAP);");
                    break;
                case MapStyle.SATELLITE:
                    HtmlPage.Window.Eval(mapJsObj.Id + ".setMapType(G_SATELLITE_MAP);");
                    break;
                default:
                    break;
            }
        }
        public void MapSetCenterZoom(LatLng center, Int32 zoom, Map mapJsObj)
        {
            HtmlPage.Window.Eval(mapJsObj.Id + ".setCenter(new GLatLng(" + center.ToString() + ")," + zoom.ToString() + ");");
        }
        public void MapAddShape(MapShapes shape, Map mapJsObj)
        {
            HtmlPage.Window.Eval(mapJsObj.Id + ".addOverlay(" + shape.Id + ");");
        }
        public void MapDeleteShape(MapShapes shape, Map mapJsObj)
        {
            HtmlPage.Window.Eval(mapJsObj.Id + ".removeOverlay(" + shape.Id + ");");
        }

        public void MapUnload()
        {
            HtmlPage.Window.Eval("GUnload();");
        }
        private void ShapeInitClickHandler(string objId)
        {
            HtmlPage.Window.Eval(objId + ".ClickHandler = function(mapid){" + sl_control + ".Content.MapEvent.OnShapeClick('" + objId + "',mapid);}");
        }
        public void ShapeMarker(MapShapes shape, LatLng point, String title, String iconSource)
        {
            String icon = "G_DEFAULT_ICON";
            if (iconSource != null)
            {
                icon = "new GIcon(G_DEFAULT_ICON,'" + iconSource + "')";
            }
            String evalString = shape.Id + " = new GMarker(new GLatLng(" + point.ToString() + "),{title: '" + title + "',icon:" + icon + "});";
            HtmlPage.Window.Eval(evalString);
            ShapeInitClickHandler(shape.Id);
        }
        public void ShapePolyLine(MapShapes shape, LatLng[] points, System.Windows.Media.Color color, Int32 weight, Double opacity)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("var " + shape.Id + " = new GPolyline([");
            for (int i = 0; i < points.Length - 1; i++)
            {
                sb.Append("new GLatLng(" + points[i].ToString() + ")");
                sb.Append(",");
            }
            sb.Append("new GLatLng(" + points[points.Length - 1].ToString() + ")],");
            sb.Append("'" + Utility.ColorToRGB(color) + "'," + weight + "," + opacity + ");");
            HtmlPage.Window.Eval(sb.ToString());
            ShapeInitClickHandler(shape.Id);
        }
        public void ShapeMarkerSetPoint(MapShapes shape, LatLng point)
        {
            HtmlPage.Window.Eval(shape.Id + ".setLatLng(new GLatLng(" + point.ToString() + "));");
        }

        public void ShapeDispose(MapShapes shape)
        {
            HtmlPage.Window.Eval(shape.Id + " = null;");
        }

        #endregion
    }
}
