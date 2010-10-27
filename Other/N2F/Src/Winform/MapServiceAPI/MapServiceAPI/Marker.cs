using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Browser;

namespace MapServiceAPI
{
    public class Marker : MapShapes
    {
        LatLng point;
        
        public LatLng Point
        {
            get { return point; }
            set { SetPoint(value); }
        }
        String title;

        public String Title
        {
            get { return title; }
        }

        String iconSource;

        public Marker(LatLng point, String title, String iconSource, IMapServiceJS mapServiceJS):base(mapServiceJS)
        {
            this.point = point;
            this.title = title;
            this.iconSource = iconSource;
            Redraw();
        }

        public void SetPoint(LatLng point)
        {
            this.point = point;
            mapServiceJS.ShapeMarkerSetPoint(this, point);
        }

        public override void Redraw()
        {
            mapServiceJS.ShapeMarker(this, point, title, iconSource);
            base.Redraw();
        }
    }
}
