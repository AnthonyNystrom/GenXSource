using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GMap
{
    public partial class ucGMap : UserControl
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public ucGMap()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the web browser control
        /// </summary>
        public void InitMap()
        {
            string htmlLocation = Application.StartupPath + "\\GoogleMap.html";

            // Make sure we are not in design view  before attempting to load the html file
            if (!System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper().Equals("DEVENV"))
            {
                if (System.IO.File.Exists(htmlLocation))
                {
                    gMapBrowser.ObjectForScripting = this;
                    gMapBrowser.Url = new Uri(htmlLocation);
                }
                else
                {
                    throw new Exception("Google Map HTML file not found!");
                }
            }
        }

        /// <summary>
        /// Add a marker to the map
        /// </summary>
        public void AddMarker(int markernumber, double latitude, double longitude, string description)
        {
            gMapBrowser.Document.InvokeScript("addMarker", 
                new object[] {
                    markernumber,
                    latitude,
                    longitude,
                    description });
        }

        /// <summary>
        /// Set center of map
        /// </summary>
        public void SetCenter(double latitude, double longitude)
        {
            gMapBrowser.Document.InvokeScript("setCenter",
                new object[] {
                    latitude,
                    longitude });
        }

        /// <summary>
        /// Clear map
        /// </summary>
        public void ClearMap()
        {
            gMapBrowser.Document.InvokeScript("clearMap");
        }
    }
}
