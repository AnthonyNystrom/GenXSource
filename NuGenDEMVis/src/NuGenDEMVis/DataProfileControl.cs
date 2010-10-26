using System.Windows.Forms;
using Genetibase.NuGenDEMVis.Data;
using Janus.Windows.EditControls;

namespace Genetibase.NuGenDEMVis.UI
{
    public partial class DataProfileControl : UserControl
    {
        DataProfile[] profiles;
        DataProfile selectedProfile;
        DataProfile.SubProfile selectedSubProfile;

        public DataProfileControl()
        {
            InitializeComponent();
        }

        #region Properties

        public DataProfile[] Profiles
        {
            get { return profiles; }
            set { profiles = value; UpdateValues(); }
        }

        public DataProfile SelectedProfile
        {
            get { return selectedProfile; }
        }

        public DataProfile.SubProfile SelectedSubProfile
        {
            get { return selectedSubProfile; }
        }
        #endregion

        private void UpdateValues()
        {
            if (profiles == null)
                return;
            foreach (DataProfile profile in profiles)
            {
                uiComboBox1.Items.Add(new UIComboBoxItem(profile.Name, profile));
            }
            uiComboBox1.SelectedIndex = 0;
        }

        private void uiComboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            uiComboBox2.Items.Clear();
            if (uiComboBox1.SelectedItem != null)
            {
                selectedProfile = (DataProfile)uiComboBox1.SelectedItem.Value;
                textBox1.Text = selectedProfile.Desc;

                foreach (DataProfile.SubProfile sProfile in selectedProfile.SubProfiles)
                {
                    uiComboBox2.Items.Add(new UIComboBoxItem(sProfile.Name, sProfile));
                }
                uiComboBox2.SelectedIndex = 0;
            }
            else
                textBox1.Text = "";
        }

        private void uiComboBox2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (uiComboBox2.SelectedItem != null && uiComboBox1.SelectedItem != null)
            {
                selectedSubProfile = (DataProfile.SubProfile)uiComboBox2.SelectedItem.Value;
                textBox2.Text = selectedSubProfile.Desc;

                dataProfilePreviewControl1.Setup((DataProfile)uiComboBox1.SelectedItem.Value, selectedSubProfile);
            }
            else
                textBox2.Text = "";
        }
    }
}