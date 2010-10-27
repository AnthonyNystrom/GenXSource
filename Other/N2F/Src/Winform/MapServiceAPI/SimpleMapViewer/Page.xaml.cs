using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MapServiceAPI;

namespace SimpleMapViewer
{
    public partial class Page : UserControl
    {
        Map theMap;
        GoogleMapJS googleMapJS;
        int counter;
        Marker sosMarker;
        System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer();
        public Page()
        {
            InitializeComponent();
            googleMapJS = new GoogleMapJS("silverlightControl");
            theMap = new Map(MapBox, googleMapJS);
            theMap.OnShapeClick += new EventHandler(theMap_OnShapeClick);
            theMap.OnViewChange += new EventHandler(theMap_OnViewChange);
            theMap.OnZoom += new EventHandler(theMap_OnZoom);
        }

        void theMap_OnZoom(object sender, EventArgs e)
        {
            UpdateMapViewInfo();
        }

        void theMap_OnViewChange(object sender, EventArgs e)
        {
            UpdateMapViewInfo();
        }
        void UpdateMapViewInfo()
        {
            TextBoxZoom.Text = theMap.Zoom.ToString();
            TextBoxLatitude.Text = theMap.Center.Latitude.ToString();
            TextBoxLongitude.Text = theMap.Center.Longitude.ToString();
        }
        void theMap_OnShapeClick(object sender, EventArgs e)
        {
            TextBoxClicked.Text = ((Marker)sender).Title + " Deleted!";
            theMap.DeleteShape((MapShapes)sender);
        }

        private void ButtonAddPin_Click(object sender, RoutedEventArgs e)
        {
            Marker pin = new Marker(theMap.Center, "pin "+counter, null,googleMapJS);
            pin.IsClickable = true;
            theMap.AddShape(pin);
            counter++;
            
        }

        private void ButtonRedraw_Click(object sender, RoutedEventArgs e)
        {
            theMap.Redraw();
        }

        private void RadioButtonStreets_Checked(object sender, RoutedEventArgs e)
        {
            theMap.MapStyle = MapStyle.STREET;
        }

        private void RadioButtonHybrid_Checked(object sender, RoutedEventArgs e)
        {
            theMap.MapStyle = MapStyle.HYBRID;
        }

        private void RadioButtonSatellight_Checked(object sender, RoutedEventArgs e)
        {
            theMap.MapStyle = MapStyle.SATELLITE;
        }

        private void StartSoS()
        {
            sosMarker = new Marker(new LatLng(45.5878549, -122.602958), "Zoom!", null,googleMapJS);
            theMap.AddShape(sosMarker);

            
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += new EventHandler(dt_Tick);
            dt.Start();
        }

        void dt_Tick(object sender, EventArgs e)
        {
            if (sosMarker.Point.Latitude < 33)
            {
                dt.Stop();
                return;
            }
            sosMarker.SetPoint(sosMarker.Point.FromCourse(180, 0.173866*4));
        }

        private void ButtonSos_Click(object sender, RoutedEventArgs e)
        {
            if (!dt.IsEnabled)
            {
                StartSoS();
            }
        }
    }
}
