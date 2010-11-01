using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Disassemble;

namespace Dile.UI
{
	public partial class NuGenQuickSearchSettingsForm : Form
	{
		private bool updatingControls = false;
		private bool UpdatingControls
		{
			get
			{
				return updatingControls;
			}
			set
			{
				updatingControls = value;
			}
		}

		public NuGenQuickSearchSettingsForm()
		{
			InitializeComponent();
		}

		private void InitializeSearchOptionsListBox()
		{
			searchOptionsListBox.BeginUpdate();
			UpdatingControls = true;

			searchOptionsListBox.Items.Clear();

			foreach (SearchOptions enumValue in Enum.GetValues(typeof(SearchOptions)))
			{
				if (enumValue != SearchOptions.None)
				{
					searchOptionsListBox.Items.Add(enumValue, false);
				}
			}

			searchOptionsListBox.Sorted = true;

			UpdatingControls = false;
			searchOptionsListBox.EndUpdate();
		}

		private void InitializeAssembliesListBox()
		{
			assembliesListBox.BeginUpdate();
			UpdatingControls = true;

			assembliesListBox.Items.Clear();

			foreach (NuGenAssembly assembly in NuGenProject.Instance.Assemblies)
			{
				assembliesListBox.Items.Add(new NuGenAssemblySearchOptions(assembly));
			}

			assembliesListBox.Sorted = true;

			UpdatingControls = false;
			assembliesListBox.EndUpdate();
		}

		private void QuickFinderSettingsForm_Load(object sender, EventArgs e)
		{
			if (NuGenProject.Instance.Assemblies.Count > 0)
			{
				InitializeSearchOptionsListBox();
				InitializeAssembliesListBox();

				assembliesListBox.SelectedIndex = 0;
			}
		}

		private void UpdateSearchOptions()
		{
			foreach (NuGenAssemblySearchOptions assemblySearchOptions in assembliesListBox.Items)
			{
				assemblySearchOptions.UpdateAssemblySearchOptions();
			}

			NuGenProject.Instance.IsSaved = false;
		}

		private void DisplayAssemblySearchOptions(NuGenAssemblySearchOptions assemblySearchOptions)
		{
			searchOptionsListBox.BeginUpdate();
			UpdatingControls = true;

			for (int index = 0; index < searchOptionsListBox.Items.Count; index++)
			{
				SearchOptions searchOption = (SearchOptions)searchOptionsListBox.Items[index];
				searchOptionsListBox.SetItemChecked(index, assemblySearchOptions.IsSearchOptionChosen(searchOption));
			}

			UpdatingControls = false;
			searchOptionsListBox.EndUpdate();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			UpdateSearchOptions();
			DialogResult = DialogResult.OK;
		}

		private NuGenAssemblySearchOptions GetSelectedAssemblySearchOptions()
		{
			NuGenAssemblySearchOptions result = null;

			if (assembliesListBox.SelectedIndex > -1)
			{
				result = (NuGenAssemblySearchOptions)assembliesListBox.Items[assembliesListBox.SelectedIndex];
			}

			return result;
		}

		private void assembliesListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!UpdatingControls)
			{
				NuGenAssemblySearchOptions selectedAssemblySearchOptions = GetSelectedAssemblySearchOptions();

				if (selectedAssemblySearchOptions != null)
				{
					DisplayAssemblySearchOptions(GetSelectedAssemblySearchOptions());
				}
			}
		}

		private void searchOptionsListBox_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (!UpdatingControls)
			{
				NuGenAssemblySearchOptions selectedAssemblySearchOptions = GetSelectedAssemblySearchOptions();

				if (selectedAssemblySearchOptions != null)
				{
					SearchOptions selectedSearchOptions = (SearchOptions)searchOptionsListBox.Items[e.Index];

					if (e.NewValue == CheckState.Checked)
					{
						selectedAssemblySearchOptions.SetSearchOption(selectedSearchOptions);
					}
					else
					{
						selectedAssemblySearchOptions.ClearSearchOption(selectedSearchOptions);
					}
				}
			}
		}
	}
}