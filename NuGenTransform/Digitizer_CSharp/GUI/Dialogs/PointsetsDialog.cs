using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenTransform
{
    public partial class PointsetsDialog : Form
    {
        private List<NuGenPointSet> pointSets = new List<NuGenPointSet>();
        private bool curve;

        public PointsetsDialog(List<NuGenPointSet> pointSets, bool curve)
        {
            InitializeComponent();

            if (curve)
                listLabel.Text = "Curves";
            else
                listLabel.Text = "Measures";

            this.curve = curve;

            foreach (NuGenPointSet pointSet in pointSets)
            {
                NuGenPointSet newPointSet = new NuGenPointSet();
                newPointSet.Style = pointSet.Style;
                newPointSet.Name = pointSet.Name;
                this.pointSets.Add(newPointSet);
            }

            this.pointSets = pointSets;

            foreach (NuGenPointSet pointSet in pointSets)
            {
                listBox1.Items.Add(pointSet.Name);
            }

            this.MaximumSize = Size;
        }

        private void PointsetsDialog_Load(object sender, EventArgs e)
        {

        }

        private NuGenPointSet GetPointsFromName(string name)
        {
            foreach (NuGenPointSet pointset in pointSets)
            {
                if (pointset.Name.Equals(name))
                {
                    return pointset;
                }
            }

            return new NuGenPointSet();
        }

        private void propertiesButton_Click(object sender, EventArgs e)
        {
            NuGenPointSet pointSet = GetPointsFromName((string)listBox1.SelectedItem);

            PointSettingsDialog dlg = new PointSettingsDialog(pointSet.Style, false);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pointSet.Style = dlg.Style;
            }
        }

        private void delButton_Click(object sender, EventArgs e)
        {
            pointSets.Remove(GetPointsFromName((string)listBox1.SelectedItem));
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            NameDialog dlg = new NameDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                NuGenPointSet pointSet = new NuGenPointSet();
                pointSet.Name = dlg.PointSetName;
                pointSet.Style = curve ? NuGenDefaultSettings.GetInstance().DefaultCurveStyle : NuGenDefaultSettings.GetInstance().DefaultMeasureStyle;

                pointSets.Add(pointSet);
            }

            UpdateListBox();
        }

        private void UpdateListBox()
        {
            foreach(NuGenPointSet pointSet in pointSets)
            {
                if(!listBox1.Items.Contains((string)pointSet.Name))
                {
                    listBox1.Items.Add((string)pointSet.Name);
                }
            }
        }

        public void SetPointsets(List<NuGenPointSet> list)
        {
            bool found = false;

            foreach (NuGenPointSet pointSet in pointSets)
            {
                foreach (NuGenPointSet outPointSet in list)
                {
                    if (outPointSet.Name.Equals(pointSet.Name))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    list.Add(pointSet);
                    found = false;
                }
            }

            foreach (NuGenPointSet pointSet in list)
            {
                pointSet.Style = GetPointsFromName(pointSet.Name).Style;
            }
        }
    }

    public class NameDialog : Form
    {
        private Label label;
        private TextBox textBox;
        private Button okButton;
        private Button cancelButton;

        public NameDialog()
        {
            this.Size = new Size(300, 200);

            label = new Label();
            label.Text = "Name : ";
            label.Location = new Point(25, 35);

            textBox = new TextBox();
            textBox.Width = 100;
            textBox.Location = new Point(125, 35);

            okButton = new Button();
            okButton.Text = "OK";
            okButton.Location = new Point(25, 125);
            okButton.DialogResult = DialogResult.OK;

            cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.Location = new Point(125, 125);
            cancelButton.DialogResult = DialogResult.Cancel;
            
            Controls.Add(label);
            Controls.Add(textBox);
            Controls.Add(okButton);
            Controls.Add(cancelButton);
        }

        public string PointSetName
        {
            get
            {
                return textBox.Text;
            }
        }
    }
}