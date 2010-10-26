using System.Windows.Forms;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.VisUI.Rendering;

namespace NuGenVisUI
{
    public partial class GraphicsProfileInfoDlg : Form
    {
        public GraphicsProfileInfoDlg(GraphicsProfile profile)
        {
            InitializeComponent();

            label2.Text = profile.Name;
            label4.Text = profile.Desc;

            GraphicsDeviceRequirements req = profile.RecommendedVariation;
            AddItem("Hardware TnL", req.HardwareTnL, 0);
            AddItem("Device Type", req.DeviceType, 0);
            AddItem("Display Format", req.DisplayFormat, 0);
            AddItem("Multi-Sample", req.MultiSample, 0);
            AddItem("Pixel Shader", req.PixelShader, 0);
            AddItem("Vertex Shader", req.VertexShader, 0);
            AddItem("Pure Device", req.Pure, 0);

            req = profile.MinReqs;
            AddItem("Hardware TnL", req.HardwareTnL, 1);
            AddItem("Device Type", req.DeviceType, 1);
            AddItem("Display Format", req.DisplayFormat, 1);
            AddItem("Multi-Sample", req.MultiSample, 1);
            AddItem("Pixel Shader", req.PixelShader, 1);
            AddItem("Vertex Shader", req.VertexShader, 1);
            AddItem("Pure Device", req.Pure, 1);
        }

        private void AddItem(string name, object value, int group)
        {
            ListViewItem item;
            if (value != null)
                item = new ListViewItem(new string[] { name, value.ToString() }, listView1.Groups[group]);
            else
                item = new ListViewItem(new string[] { name, "-" }, listView1.Groups[group]);
            listView1.Items.Add(item);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}