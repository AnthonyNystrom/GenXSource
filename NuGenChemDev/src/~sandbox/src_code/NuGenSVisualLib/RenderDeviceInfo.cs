using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NuGenSVisualLib.Rendering.Devices;

namespace NuGenSVisualLib.Rendering.Devices
{
    partial class RenderDeviceInfo : Form
    {
        public RenderDeviceInfo(OutputSettings settings, OutputCaps caps)
        {
            InitializeComponent();

            // populate list
            AddItem("Adapter Desc", caps.AdapterDetails.Description);
            AddItem("Driver Name", caps.AdapterDetails.DriverName);
            AddItem("Driver Version", caps.AdapterDetails.DriverVersion.ToString());
            AddItem("T&L Support", caps.HardwareTnL.ToString());
            AddItem("Device Format", settings.DeviceFormat.ToString());
            AddItem("Depth Format", settings.DepthFormat.ToString());
            AddItem("Vertex Shader", caps.VertexShaderVersion.ToString());
            AddItem("Pixel Shader", caps.FragmentShaderVersion.ToString());
        }

        private void AddItem(string name, string value)
        {
            ListViewItem item = new ListViewItem(new string[] { name, value });
            listView1.Items.Add(item);
        }
    }
}