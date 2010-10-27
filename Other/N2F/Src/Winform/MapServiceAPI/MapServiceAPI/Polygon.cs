using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Browser;
using System.Windows.Media;

namespace MapServiceAPI
{
    public class Polygon : MapShapes
    {
        LatLng[] points;
        string strokeColor;
        int strokeWeight;
        double strokeOpacity;
        string fillColor;
        double fillOpacity;

        public Polygon(LatLng[] points, Color strokeColor, int strokeWeight, double strokeOpacity, Color fillColor, double fillOpacity,IMapServiceJS mapServiceJS):base(mapServiceJS)
        {
            this.strokeColor = Utility.ColorToRGB(strokeColor);
            this.strokeWeight = strokeWeight;
            this.strokeOpacity = strokeOpacity;
            this.fillColor = Utility.ColorToRGB(fillColor);
            this.fillOpacity = fillOpacity;
            this.points = points;
            Redraw();
        }

        public override void Redraw()
        {
            /*
            switch (Map.MapService)
            {
                case MapService.VirtualEarth:
                    throw new NotImplementedException();
                    break;
                case MapService.GoogleMap:
                    StringBuilder sb = new StringBuilder();
                    sb.Append("var " + Id + " = new GPolygon([");
                    for (int i = 0; i < points.Length - 1; i++)
                    {
                        sb.Append("new GLatLng(" + points[i].ToString() + ")");
                        sb.Append(",");
                    }
                    sb.Append("new GLatLng(" + points[points.Length - 1].ToString() + ")],");
                    sb.Append("'" + strokeColor + "'," + strokeWeight + "," + strokeOpacity + ",");
                    sb.Append("'" + fillColor + "'," + fillOpacity + ");");

                    HtmlPage.Window.Eval(sb.ToString());
                    break;
                default:
                    break;
            }
            base.Redraw();*/
        }
    }

}
