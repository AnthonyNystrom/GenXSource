using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NuGenSVisualLib.Settings;
using Microsoft.DirectX.Direct3D;

namespace ChemDevEnv
{
    public partial class OptionsWindow : Form
    {
        Dictionary<string, object> globals;
        HashTableSettings settings;

        string[][] adaptersInfo;

        public OptionsWindow(HashTableSettings settings)
        {
            InitializeComponent();
            this.globals = HashTableSettings.globalTable;
            ScanAdapters();
            //SetSettings(settings);
        }

        private void ScanAdapters()
        {
            adaptersInfo = new string[Manager.Adapters.Count][];
            int adapterIdx = 0;
            foreach (AdapterInformation adapter in Manager.Adapters)
            {
                // get name
                string adapterName = string.Format("{0}:{1}", adapter.Information.DeviceName,
                                                   adapter.Information.Description);
                // get info
                string driver = string.Format("Driver : {0} [{1}]", adapter.Information.DriverName,
                                              adapter.Information.DriverVersion.ToString());
                Caps caps = Manager.GetDeviceCaps(adapter.Adapter, DeviceType.Hardware);
                string vs = string.Format("Vertex Shader : {0}", caps.VertexShaderVersion.ToString());
                string ps = string.Format("Pixel Shader : {0}", caps.PixelShaderVersion.ToString());

                adaptersInfo[adapterIdx++] = new string[] { driver, vs, ps };

                uiViewAdaptersListBox.Items.Add(adapterName);
            }
        }

        public void SetSettings(HashTableSettings settings)
        {
            // configure all controls with settings
            this.settings = settings;
            this.globals = HashTableSettings.globalTable;

            // general
            uiGenUIClrSchemeComboBox.SelectedIndex = (byte)globals["CDE.UI.Office2007ClrScheme"];

            uiStartupWinMax.Checked = ((string)globals["CDE.Startup.WindowSize"] == "100");
            uiStartupWin70.Checked = ((string)globals["CDE.Startup.WindowSize"] == "70");
            uiStartupWin50.Checked = ((string)globals["CDE.Startup.WindowSize"] == "50");

            // view
            uiViewAdaptersListBox.SelectedIndex = (byte)globals["CDI.Adapter"];

            uiViewBgClrBtn.SelectedColor = (Color)globals["View3D.Bg.Clr"];
            uiViewFontPicker.SelectedValue = new FontFamily((string)globals["View3D.Font.Family"]);
            uiViewFontAA.Checked = (bool)globals["View3D.Font.AntiAlias"];
        }

        private void GatherSettings()
        {
            // general
            globals["CDE.UI.Office2007ClrScheme"] = (byte)uiGenUIClrSchemeComboBox.SelectedIndex;

            string value = null;
            if (uiStartupWinMax.Checked)
                value = "100";
            else if (uiStartupWin70.Checked)
                value = "70";
            else if (uiStartupWin50.Checked)
                value = "50";
            globals["CDE.Startup.WindowSize"] = value;

            // view
            globals["CDI.Adapter"] = (byte)uiViewAdaptersListBox.SelectedIndex;

            globals["View3D.Bg.Clr"] = uiViewBgClrBtn.SelectedColor;
            globals["View3D.Font.Family"] = ((FontFamily)uiViewFontPicker.SelectedValue).Name;
            globals["View3D.Font.AntiAlias"] = uiViewFontAA.Checked;

            //settings.ExportToXml("c:/temp.xml");
        }

        #region Events

        private void uiButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            GatherSettings();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        
        private void uiViewAdaptersListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            uiViewAdapterInfoListBox.Items.Clear();
            if (uiViewAdaptersListBox.SelectedIndex >= 0 && uiViewAdaptersListBox.SelectedIndex < adaptersInfo.Length)
            uiViewAdapterInfoListBox.Items.AddRange(adaptersInfo[uiViewAdaptersListBox.SelectedIndex]);
        }
        #endregion
    }
}