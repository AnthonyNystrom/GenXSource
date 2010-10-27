using System;
using System.Windows.Browser;


namespace MapServiceAPI
{
    public class MapEvents
    {
        public MapEvents()
        {
            HtmlPage.RegisterScriptableObject("MapEvent", this);
        }

        [ScriptableMember]
        public void OnMapViewChange(string mapid, string lat, string lng)
        {
            if (MapViewChange != null)
            {
                MapEventArgs mapEvent = new MapEventArgs();
                mapEvent.MapId = mapid;
                mapEvent.Point = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
                MapViewChange(null, mapEvent);
            }
        }
        public event EventHandler<MapEventArgs> MapViewChange;

        [ScriptableMember]
        public void OnShapeClick(String shapeid,String mapid)
        {
            if (ShapeClick != null)
            {
                MapEventArgs mapEvent = new MapEventArgs();
                mapEvent.MapId = mapid;
                mapEvent.ShapeId = shapeid;
                ShapeClick(null, mapEvent);
            }
        }
        public event EventHandler<MapEventArgs> ShapeClick;

        [ScriptableMember]
        public void OnZoom(string mapid, string zoom)
        {
            if (ZoomChange != null)
            {
                MapEventArgs mapEvent = new MapEventArgs();
                mapEvent.MapId = mapid;
                mapEvent.Zoom = Convert.ToInt32(zoom);
                ZoomChange(null, mapEvent);
            }
        }
        public event EventHandler<MapEventArgs> ZoomChange;

        [ScriptableMember]
        public void OnMapClick(string mapid, string lat, string lng)
        {
            if (MapClick != null)
            {
                MapEventArgs mapEvent = new MapEventArgs();
                mapEvent.MapId = mapid;
                mapEvent.Point = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
                MapViewChange(null, mapEvent);
            }
        }
        public event EventHandler<MapEventArgs> MapClick;
        
        
    }
    public class MapEventArgs : EventArgs
    {
        public String MapId;
        public String ShapeId;
        public LatLng Point;
        public Int32 Zoom;
    }
}
