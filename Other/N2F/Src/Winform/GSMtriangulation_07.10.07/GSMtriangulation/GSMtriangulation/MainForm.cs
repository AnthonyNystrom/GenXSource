// Program    : GSM Triangulation
// Date       : July 10, 2008
// Description: This program determines the latitude and longitude coordinates of GSM
// cell towers based upon their Location Area Code and Cell ID.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace GSMtriangulation
{
    public partial class MainForm : Form
    {
        const int NUMCELLS = 7;
        
        gsmMapping gsmMapping1; // Google GSM mapping

        int[] mcc = new int[NUMCELLS]; // Mobile Country Code
        int[] mnc = new int[NUMCELLS]; // Mobile Network Code
        int[] lac = new int[NUMCELLS]; // Location Area Code
        int[] cid = new int[NUMCELLS]; // Cell ID

        double[] lat = new double[NUMCELLS]; // Found latitude
        double[] lon = new double[NUMCELLS]; // Found longitude

        bool[] datavalid = new bool[NUMCELLS]; // Input data is valid flag

        int numValidCells;  // Number of Valid Cells
        double avgLat;      // Average latitude of valid cells
        double avgLon;      // Average longitude of valid cells


        public MainForm()
        {
            InitializeComponent();

            gsmMapping1 = new gsmMapping();

        }

        private void btnFindLocations_Click(object sender, EventArgs e)
        {
            // Clear result textboxes.
            txtLat0.Text = null; txtLat1.Text = null; txtLat2.Text = null; txtLat3.Text = null; txtLat4.Text = null; txtLat5.Text = null; txtLat6.Text = null;
            txtLon0.Text = null; txtLon1.Text = null; txtLon2.Text = null; txtLon3.Text = null; txtLon4.Text = null; txtLon5.Text = null; txtLon6.Text = null;

            // Initialize variables.
            for (int i = 0; i < NUMCELLS; i++)
            {
                datavalid[i] = true; // Assume input is OK unless proven otherwise.
                lat[i] = 0;
                lon[i] = 0;
            }

            numValidCells = 0;

            // Get our input data
            if (!Int32.TryParse(txtMCC0.Text, out mcc[0])) datavalid[0] = false;
            if (!Int32.TryParse(txtMCC1.Text, out mcc[1])) datavalid[1] = false;
            if (!Int32.TryParse(txtMCC2.Text, out mcc[2])) datavalid[2] = false;
            if (!Int32.TryParse(txtMCC3.Text, out mcc[3])) datavalid[3] = false;
            if (!Int32.TryParse(txtMCC4.Text, out mcc[4])) datavalid[4] = false;
            if (!Int32.TryParse(txtMCC5.Text, out mcc[5])) datavalid[5] = false;
            if (!Int32.TryParse(txtMCC6.Text, out mcc[6])) datavalid[6] = false;

            if (!Int32.TryParse(txtMNC0.Text, out mnc[0])) datavalid[0] = false;
            if (!Int32.TryParse(txtMNC1.Text, out mnc[1])) datavalid[1] = false;
            if (!Int32.TryParse(txtMNC2.Text, out mnc[2])) datavalid[2] = false;
            if (!Int32.TryParse(txtMNC3.Text, out mnc[3])) datavalid[3] = false;
            if (!Int32.TryParse(txtMNC4.Text, out mnc[4])) datavalid[4] = false;
            if (!Int32.TryParse(txtMNC5.Text, out mnc[5])) datavalid[5] = false;
            if (!Int32.TryParse(txtMNC6.Text, out mnc[6])) datavalid[6] = false;

            if (!Int32.TryParse(txtLAC0.Text, out lac[0])) datavalid[0] = false;
            if (!Int32.TryParse(txtLAC1.Text, out lac[1])) datavalid[1] = false;
            if (!Int32.TryParse(txtLAC2.Text, out lac[2])) datavalid[2] = false;
            if (!Int32.TryParse(txtLAC3.Text, out lac[3])) datavalid[3] = false;
            if (!Int32.TryParse(txtLAC4.Text, out lac[4])) datavalid[4] = false;
            if (!Int32.TryParse(txtLAC5.Text, out lac[5])) datavalid[5] = false;
            if (!Int32.TryParse(txtLAC6.Text, out lac[6])) datavalid[6] = false;

            if (!Int32.TryParse(txtCID0.Text, out cid[0])) datavalid[0] = false;
            if (!Int32.TryParse(txtCID1.Text, out cid[1])) datavalid[1] = false;
            if (!Int32.TryParse(txtCID2.Text, out cid[2])) datavalid[2] = false;
            if (!Int32.TryParse(txtCID3.Text, out cid[3])) datavalid[3] = false;
            if (!Int32.TryParse(txtCID4.Text, out cid[4])) datavalid[4] = false;
            if (!Int32.TryParse(txtCID5.Text, out cid[5])) datavalid[5] = false;
            if (!Int32.TryParse(txtCID6.Text, out cid[6])) datavalid[6] = false;


            double sumLat = 0; double sumLon = 0;
            // Get our latitude and longitude coordinates.
            for (int i = 0; i < NUMCELLS; i++)
            {
                if (datavalid[i] && gsmMapping1.SetLocation(mcc[i], mnc[i], lac[i], cid[i]))
                {
                    lat[i] = gsmMapping1.GetLatitude();
                    lon[i] = gsmMapping1.GetLongitude();

                    numValidCells++;
                    sumLat += lat[i];
                    sumLon += lon[i];

                    switch (i)
                    {
                        case 0:
                            txtLat0.Text = Convert.ToString(lat[0]);
                            txtLon0.Text = Convert.ToString(lon[0]);
                            break;
                        case 1:
                            txtLat1.Text = Convert.ToString(lat[1]);
                            txtLon1.Text = Convert.ToString(lon[1]);
                            break;
                        case 2:
                            txtLat2.Text = Convert.ToString(lat[2]);
                            txtLon2.Text = Convert.ToString(lon[2]);
                            break;
                        case 3:
                            txtLat3.Text = Convert.ToString(lat[3]);
                            txtLon3.Text = Convert.ToString(lon[3]);
                            break;
                        case 4:
                            txtLat4.Text = Convert.ToString(lat[4]);
                            txtLon4.Text = Convert.ToString(lon[4]);
                            break;
                        case 5:
                            txtLat5.Text = Convert.ToString(lat[5]);
                            txtLon5.Text = Convert.ToString(lon[5]);
                            break;
                        case 6:
                            txtLat6.Text = Convert.ToString(lat[6]);
                            txtLon6.Text = Convert.ToString(lon[6]);
                            break;
                    } //end switch
                } // end if
            } // end for

            avgLat = sumLat / numValidCells;
            avgLon = sumLon / numValidCells;
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            ucGMap1.InitMap();
        }


        private void btnAddMarkers_Click(object sender, EventArgs e)
        {
            ucGMap1.ClearMap();
            ucGMap1.SetCenter(avgLat, avgLon);

            for (int i = 0; i < NUMCELLS; i++)
            {
                if (datavalid[i])
                {
                    string description = "Cell " + i + "<br />";
                    description += lat[i] + ", " + lon[i] + "<br />";
                    ucGMap1.AddMarker(i, lat[i], lon[i], description);
                }
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            HelpForm HelpForm1 = new HelpForm();
            HelpForm1.ShowDialog(this);
        }
    }
}
