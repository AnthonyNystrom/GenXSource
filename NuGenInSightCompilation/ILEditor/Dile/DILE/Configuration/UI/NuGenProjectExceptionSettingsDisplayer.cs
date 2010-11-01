using System;
using System.Collections.Generic;
using System.Text;

using Dile.Controls;
using Dile.Disassemble;
using System.Drawing;
using System.Windows.Forms;
using Janus.Windows.GridEX;

namespace Dile.Configuration.UI
{
	public class NuGenProjectExceptionSettingsDisplayer : NuGenBaseSettingsDisplayer
	{
		private NuGenCustomDataGridView exceptionsGrid = null;
		private NuGenCustomDataGridView ExceptionsGrid
		{
			get
			{
				return exceptionsGrid;
			}
			set
			{
				exceptionsGrid = value;
			}
		}

		private void CreateExceptionGrid(TableLayoutPanel panel)
		{
			ExceptionsGrid = new NuGenCustomDataGridView();
            ExceptionsGrid.GroupByBoxVisible = false;
            ExceptionsGrid.VisualStyle = VisualStyle.Office2007;
			ExceptionsGrid.Dock = DockStyle.Fill;
            ExceptionsGrid.RootTable = new GridEXTable();

            GridEXColumn skipColumn = new GridEXColumn();
            skipColumn.ColumnType = ColumnType.CheckBox;
			skipColumn.DataMember = "Skip";
			skipColumn.Caption = "Skip";
			ExceptionsGrid.RootTable.Columns.Add(skipColumn);

            GridEXColumn assemblyPathColumn = new GridEXColumn();
            assemblyPathColumn.DataMember = "AssemblyPath";
			assemblyPathColumn.Caption = "Assembly";			
			ExceptionsGrid.RootTable.Columns.Add(assemblyPathColumn);

            GridEXColumn tokenColumn = new GridEXColumn();
            tokenColumn.DataMember = "ExceptionClassTokenString";
			tokenColumn.Caption = "Exception Token";			
			ExceptionsGrid.RootTable.Columns.Add(tokenColumn);

            GridEXColumn classNameColumn = new GridEXColumn();
			classNameColumn.DataMember = "ExceptionClassName";
			classNameColumn.Caption = "Exception Class";			
			ExceptionsGrid.RootTable.Columns.Add(classNameColumn);

			GridEXColumn methodTokenColumn = new GridEXColumn();
			methodTokenColumn.DataMember = "ThrowingMethodTokenString";
			ExceptionsGrid.RootTable.Columns.Add(methodTokenColumn);

			GridEXColumn methodNameColumn = new GridEXColumn();
			methodNameColumn.DataMember = "ThrowingMethodName";
			methodNameColumn.Caption = "Method Name";
			ExceptionsGrid.RootTable.Columns.Add(methodNameColumn);

			GridEXColumn ipColumn = new GridEXColumn();
			ipColumn.DataMember = "IPAsString";
			ipColumn.Caption = "IP";
			ExceptionsGrid.RootTable.Columns.Add(ipColumn);

			BindingSource bindingSource = new BindingSource(NuGenProject.Instance.Exceptions, string.Empty);
			ExceptionsGrid.DataSource = bindingSource;

			panel.Controls.Add(ExceptionsGrid);

			ExceptionsGrid.Initialize();
		}

		protected override void CreateControls(TableLayoutPanel panel)
		{
			panel.ColumnCount = 1;
			panel.RowCount = 1;

			CreateExceptionGrid(panel);
		}

		public override void ReadSettings()
		{
		}

		public override string ToString()
		{
			return "Exception handling";
		}
	}
}