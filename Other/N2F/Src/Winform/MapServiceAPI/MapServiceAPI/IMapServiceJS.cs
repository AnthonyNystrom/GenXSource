using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MapServiceAPI
{

    public interface IMapServiceJS
    {
        void MapInit(Map mapJsObj);
        void MapSetStyle(MapStyle style, Map mapJsObj);
        void MapSetCenterZoom(LatLng center, Int32 zoom, Map mapJsObj);
        void MapAddShape(MapShapes shape, Map mapJsObj);
        void MapDeleteShape(MapShapes shape, Map mapJsObj);
        void MapUnload();

        void ShapeMarker(MapShapes shape, LatLng point, String title, String iconSource);
        void ShapeMarkerSetPoint(MapShapes shape, LatLng point);
        void ShapePolyLine(MapShapes shape, LatLng[] points, System.Windows.Media.Color color, Int32 weight, Double opacity);
        void ShapeDispose(MapShapes shape);
    }
}
