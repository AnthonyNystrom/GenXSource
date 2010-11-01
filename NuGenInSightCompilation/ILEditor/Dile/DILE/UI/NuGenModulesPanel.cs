using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration;
using Dile.Debug;
using Dile.Disassemble;
using Dile.Metadata;
using System.Diagnostics;
using System.IO;
using Janus.Windows.GridEX;

namespace Dile.UI
{
	public partial class NuGenModulesPanel : NuGenBasePanel
	{
		private ToolStripItem addModuleMenuItem;
		private ToolStripItem AddModuleMenuItem
		{
			get
			{
				return addModuleMenuItem;
			}
			set
			{
				addModuleMenuItem = value;
			}
		}

		public NuGenModulesPanel()
		{
			InitializeComponent();
			NuGenSettings.Instance.DisplayHexaNumbersChanged += new NoArgumentsDelegate(Instance_DisplayHexaNumbersChanged);

			modulesGrid.Initialize();
			modulesGrid.RowContextMenu.Opening += new CancelEventHandler(RowContextMenu_Opening);

			AddModuleMenuItem = new ToolStripMenuItem("Add module to the project");
			AddModuleMenuItem.Click += new EventHandler(AddModuleMenuItem_Click);
			modulesGrid.RowContextMenu.Items.Insert(0, AddModuleMenuItem);
		}

		private void FormatNumberInCell(GridEXCell cell)
		{
			cell.Value = NuGenHelperFunctions.FormatNumber(NuGenHelperFunctions.TaggedObjects[cell.Value.ToString()]);
		}

		private void AssignNumberValueToCell<T>(GridEXCell cell, T tag)
		{
            NuGenHelperFunctions.TaggedObjects.Add(cell.Value.ToString(), tag);			
			cell.Value = NuGenHelperFunctions.FormatNumber(tag);
		}

		private void ShowModules()
		{
			modulesGrid.BeginGridUpdate();
			modulesGrid.ClearItems();

			List<ModuleWrapper> modules = NuGenDebugEventHandler.Instance.EventObjects.Process.GetModules();			
			for (int index = 0; index < modules.Count; index++)
			{
				ModuleWrapper module = modules[index];
                GridEXRow row = modulesGrid.AddItem();

				AddModuleToGrid(module, row);
			}

			modulesGrid.EndGridUpdate();
		}

		private void AddModuleToGrid(ModuleWrapper module, GridEXRow row)
		{
			AssignNumberValueToCell<uint>(row.Cells[0], module.GetToken());
			AssignNumberValueToCell<ulong>(row.Cells[1], module.GetBaseAddress());
			AssignNumberValueToCell<uint>(row.Cells[2], module.GetSize());

			row.Cells[3].Value = module.IsDynamic();
			row.Cells[4].Value = module.IsInMemory();

			string moduleName = module.GetName();

			try
			{
				row.Cells[5].Value = Path.GetFileName(moduleName);
			}
			catch
			{
			}

			row.Cells[6].Value = moduleName;

			AssemblyWrapper assembly = module.GetAssembly();
			row.Cells[7].Value = assembly.GetName();

			AppDomainWrapper appDomain = assembly.GetAppDomain();
			row.Cells[8].Value = appDomain.GetName();
		}

		public void AddModule(ModuleWrapper module)
		{
			modulesGrid.BeginGridUpdate();

            GridEXRow moduleRow = modulesGrid.AddItem();
			AddModuleToGrid(module, moduleRow);

			modulesGrid.EndGridUpdate();
		}

		public void AddModules(ModuleWrapper[] modules)
		{
			modulesGrid.BeginGridUpdate();

            int rowsCount = modulesGrid.RowCount;

			for (int index = 0; index < modules.Length; index++)
			{
				ModuleWrapper module = modules[index];
                GridEXRow row = modulesGrid.AddItem();				

				AddModuleToGrid(module, row);
			}

			modulesGrid.EndGridUpdate();
		}

		private void Instance_DisplayHexaNumbersChanged()
		{
			if (modulesGrid.RowCount != 0)
			{
				for(int i = 0; i<modulesGrid.RowCount; i++)
				{
					FormatNumberInCell(modulesGrid.GetRow(i).Cells[0]);
                    FormatNumberInCell(modulesGrid.GetRow(i).Cells[1]);
                    FormatNumberInCell(modulesGrid.GetRow(i).Cells[2]);
				}
			}
		}

		private void modulesGrid_CellDoubleClick(object sender, EventArgs e)
		{
			if (modulesGrid.SelectedItems.Count > 0)
			{
				GridEXRow selectedRow = modulesGrid.SelectedItems[0].GetRow();
				string moduleName = (string)selectedRow.Cells["Name"].Value;

				if (!NuGenProject.Instance.IsAssemblyLoaded(moduleName))
				{
					string message = string.Format("Would you like to add the {0} assembly to the project?", moduleName);

					if (MessageBox.Show(message, "DILE - Load assembly?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						AddSelectedModuleToProject(selectedRow, moduleName);
					}
				}
			}
		}

		private void AddModuleMenuItem_Click(object sender, EventArgs e)
		{
			GridEXRow selectedRow = modulesGrid.SelectedItems[0].GetRow();
			string moduleName = (string)selectedRow.Cells["Name"].Value;

			AddSelectedModuleToProject(selectedRow, moduleName);
		}

		private void RowContextMenu_Opening(object sender, CancelEventArgs e)
		{
			if (modulesGrid.SelectedItems.Count > 0)
			{
				GridEXRow selectedRow = modulesGrid.SelectedItems[0].GetRow();
				string moduleName = (string)selectedRow.Cells["Name"].Value;

				AddModuleMenuItem.Visible = !NuGenProject.Instance.IsAssemblyLoaded(moduleName);
			}
		}

		private void AddSelectedModuleToProject(GridEXRow selectedRow, string moduleName)
		{
			ModuleWrapper inMemoryModule = NuGenHelperFunctions.TaggedObjects[moduleName] as ModuleWrapper;

			if (inMemoryModule != null)
			{
				NuGenAssemblyLoader.Instance.LoadInMemoryModule(inMemoryModule);
			}
			else
			{
				NuGenAssemblyLoader.Instance.Start(new string[] { moduleName });
			}
		}        
	}
}