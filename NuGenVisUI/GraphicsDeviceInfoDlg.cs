using System.Windows.Forms;
using Genetibase.NuGenRenderCore.Rendering.Devices;

namespace NuGenVisUI
{
    public partial class GraphicsDeviceInfoDlg : Form
    {
        public GraphicsDeviceInfoDlg(CommonDeviceInterface cdi)
        {
            InitializeComponent();

            GraphicsDeviceCaps caps = cdi.DeviceCaps;
            
            AddItem("Adapter Desc", caps.AdapterDetails.Description);
            AddItem("Driver Name", caps.AdapterDetails.DriverName);
            AddItem("Driver Version", caps.AdapterDetails.DriverVersion.ToString());
            AddItem("T&L Support", caps.HardwareTnL.ToString());
            AddItem("Vertex Shader", caps.VertexShaderVersion.ToString());
            AddItem("Pixel Shader", caps.FragmentShaderVersion.ToString());
        }

        private void AddItem(string name, string value)
        {
            ListViewItem item = new ListViewItem(new string[] { name, value });
            listView1.Items.Add(item);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}