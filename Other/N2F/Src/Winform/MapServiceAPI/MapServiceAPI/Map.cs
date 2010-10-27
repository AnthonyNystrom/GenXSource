using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;
using System.Collections.Generic;

namespace MapServiceAPI
{
    
    public class Map
    {
        IMapServiceJS mapServiceJS;
        static MapEvents mapEvents = new MapEvents();
        static int mapIdCount;

        MapShapeCollection mapShapeList;

        #region Map properties

        String mapId;
        public String Id
        {
            get { return mapId; }
        }

        LatLng center = new LatLng(43.75, -99.71);
        public LatLng Center
        {
            get { return center; }
            set
            {
                mapServiceJS.MapSetCenterZoom(value, zoom,this);
                center = value;
            }
        }

        Int32 zoom = 4;
        public Int32 Zoom
        {
            get { return zoom; }
            set
            {
                mapServiceJS.MapSetCenterZoom(center, value,this);
                zoom = value;
            }
        }

        MapStyle mapStyle = MapStyle.STREET;
        public MapStyle MapStyle
        {
            get { return mapStyle; }
            set
            {
                mapServiceJS.MapSetStyle(value,this);
                mapStyle = value;
            }
        }
        
        #endregion
 
        public Map(Rectangle control, IMapServiceJS mapServiceJS)
        {
            if (mapServiceJS == null)
            {
                throw new Exception("mapServiceJS cannot be null");
            }
            this.mapServiceJS = mapServiceJS;

            mapId = "map_" + mapIdCount.ToString("0000");
            mapIdCount++;
            mapShapeList = new MapShapeCollection();
            mapEvents.MapViewChange += new EventHandler<MapEventArgs>(mapEvents_MapViewChange);
            mapEvents.MapClick += new EventHandler<MapEventArgs>(mapEvents_MapClick);
            mapEvents.ZoomChange += new EventHandler<MapEventArgs>(mapEvents_ZoomChange);
            mapEvents.ShapeClick += new EventHandler<MapEventArgs>(mapEvents_ShapeClick);
            InitMapOverlay(control);

            Redraw();
        }
        public event EventHandler OnShapeClick;
        void mapEvents_ShapeClick(object sender, MapEventArgs e)
        {
            if (e.MapId == mapId) {
                //fire map event with shape
                if (OnShapeClick != null)
                {
                    OnShapeClick(mapShapeList[e.ShapeId], null);
                }
            }
        }
        public event EventHandler OnZoom;
        void mapEvents_ZoomChange(object sender, MapEventArgs e)
        {
            if (e.MapId == mapId)
            {
                //Set new zoom
                zoom = e.Zoom;
                //fire map zoom
                if (OnZoom != null)
                {
                    OnZoom(this, null);
                }
            }
        }
        public event EventHandler OnClick;
        void mapEvents_MapClick(object sender, MapEventArgs e)
        {
            if (e.MapId == mapId)
            {
                if (OnClick != null)
                {
                    OnClick(this, null);
                }
            }
        }


        public event EventHandler OnViewChange;
        void mapEvents_MapViewChange(object sender, MapEventArgs e)
        {
            if (e.MapId == mapId)
            {
                //Maped moved set new center
                center = e.Point;
                //fire map move
                if (OnViewChange != null)
                {
                    OnViewChange(this, null);
                }
            }
        }
        
        public void AddShape(MapShapes shape)
        {
            mapShapeList.Add(shape);
            mapServiceJS.MapAddShape(shape, this);
        }

        public void DeleteShape(MapShapes shape)
        {
            mapShapeList.Remove(shape);
            mapServiceJS.MapDeleteShape(shape,this);
        }

        public void Redraw()
        {
            mapServiceJS.MapInit(this);
            mapServiceJS.MapSetCenterZoom(center, zoom, this);
            mapServiceJS.MapSetStyle(mapStyle,this);

            foreach (MapShapes shape in mapShapeList.Values)
            {
                shape.Redraw();
                mapServiceJS.MapAddShape(shape,this);
            }
        }

        private void InitMapOverlay(Rectangle control)
        {
            HtmlElement ele = HtmlPage.Document.CreateElement("div");
            ele.Id = mapId;
            ele.SetStyleAttribute("width", control.Width + "px");
            ele.SetStyleAttribute("height", (control.Height-12) + "px");
            ele.SetStyleAttribute("position", "absolute");
            ele.SetStyleAttribute("top", control.Margin.Top + "px");
            ele.SetStyleAttribute("left", control.Margin.Left + "px");

            HtmlPage.Document.Body.AppendChild(ele);

            HtmlElement info = HtmlPage.Document.CreateElement("div");
            info.Id = "infoBox";
            info.SetAttribute("height", 12 + "px");
            info.SetStyleAttribute("position", "absolute");
            info.SetStyleAttribute("top", (control.Margin.Top+ control.Height -12) + "px");
            info.SetStyleAttribute("left", control.Margin.Left + "px");
            info.SetAttribute("innerHTML", "<div style = 'font-size:x-small'><a  href='http://www.codeplex.com/mapserviceapi' target='_blank'>MapServiceAPI 0.1</a> ©2008 Troy Bennett</div>");
            HtmlPage.Document.Body.AppendChild(info);
        }
        ~Map()
        {
            //mapServiceJS.MapUnload();
        }
    }
}
