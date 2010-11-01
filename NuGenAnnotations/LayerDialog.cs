using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Genetibase.NuGenAnnotation;

namespace Genetibase.NuGenAnnotation
{
	public partial class LayerDialog : Form
	{
		public List<LayerEdit> layerList = new List<LayerEdit>();

		public LayerDialog(Layers _layers)
		{
			InitializeComponent();
			for (int i = 0; i < _layers.Count; i++)
			{
				LayerEdit le = new LayerEdit();
				le.LayerName = ((Layer)_layers[i]).LayerName;
				le.LayerVisible = ((Layer)_layers[i]).IsVisible;
				le.LayerActive = ((Layer)_layers[i]).IsActive;

				layerList.Add(le);
			}
			dgvLayers.DataSource = layerList;
			dgvLayers.Columns[0].HeaderText = "Layer Name";
			dgvLayers.Columns[1].HeaderText = "New Layer";
			dgvLayers.Columns[2].HeaderText = "Active";
			dgvLayers.Columns[3].HeaderText = "Deleted";
			dgvLayers.Columns[4].HeaderText = "Visible";
		}

		private void btnAddLayer_Click(object sender, EventArgs e)
		{
			LayerEdit le = new LayerEdit();
			le.LayerName = "New Layer";
			le.LayerNew = true;
			layerList.Add(le);
			dgvLayers.DataSource = null;
			dgvLayers.DataSource = layerList;
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			int active = 0;
			for (int i = 0; i < layerList.Count; i++)
				if (layerList[i].LayerActive == true)
					active++;
			if (active > 1)
				MessageBox.Show("There can be only one Active layer at a time\nCorrect this by only checking the Active box on one layer.");
			else
				this.Close();
		}
	}
}