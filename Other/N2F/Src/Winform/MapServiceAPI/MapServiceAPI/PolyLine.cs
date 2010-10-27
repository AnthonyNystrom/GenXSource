using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;
using System.Windows.Browser;



namespace MapServiceAPI
{
    public class PolyLine : MapShapes
    {
        LatLng[] points;
        Color strokeColor;
        int strokeWeight;
        double strokeOpacity;

        public PolyLine(LatLng[] points, Color color, int strokeWeight, double strokeOpacity, IMapServiceJS mapServiceJS):base(mapServiceJS)
        {
            this.strokeColor = color;
            this.strokeWeight = strokeWeight;
            this.strokeOpacity = strokeOpacity;
            this.points = points;
            Redraw();
        }


        public override void Redraw()
        {
            mapServiceJS.ShapePolyLine(this, points, strokeColor, strokeWeight, strokeOpacity);
            base.Redraw();
        }
    }
}
