using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace Dile.Configuration.UI
{
	public abstract class NuGenBaseSettingsDisplayer
	{
		private List<Control> createdControls;
		protected List<Control> CreatedControls
		{
			get
			{
				return createdControls;
			}
			set
			{
				createdControls = value;
			}
		}

		private List<ColumnStyle> columnStyles;
		protected List<ColumnStyle> ColumnStyles
		{
			get
			{
				return columnStyles;
			}
			set
			{
				columnStyles = value;
			}
		}

		private List<RowStyle> rowStyles;
		protected List<RowStyle> RowStyles
		{
			get
			{
				return rowStyles;
			}
			set
			{
				rowStyles = value;
			}
		}

		private bool isInitialized = false;
		public bool IsInitialized
		{
			get
			{
				return isInitialized;
			}
			private set
			{
				isInitialized = value;
			}
		}

		private int rowCount;
		private int RowCount
		{
			get
			{
				return rowCount;
			}
			set
			{
				rowCount = value;
			}
		}

		private int columnCount;
		private int ColumnCount
		{
			get
			{
				return columnCount;
			}
			set
			{
				columnCount = value;
			}
		}

		protected abstract void CreateControls(TableLayoutPanel panel);

		protected virtual void DisplayCreatedControls(TableLayoutPanel panel)
		{
			panel.RowCount = RowCount;
			panel.ColumnCount = ColumnCount;
			NuGenHelperFunctions.CopyListElements(CreatedControls, panel.Controls);
			NuGenHelperFunctions.CopyListElements(ColumnStyles, panel.ColumnStyles);
			NuGenHelperFunctions.CopyListElements(RowStyles, panel.RowStyles);
		}

		public void DisplayControls(TableLayoutPanel panel)
		{
			panel.Visible = false;

			if (IsInitialized)
			{
				DisplayCreatedControls(panel);
			}
			else
			{
				CreateControls(panel);
				ColumnCount = panel.ColumnCount;
				RowCount = panel.RowCount;

				if (panel.Controls.Count > 0)
				{
					CreatedControls = new List<Control>(panel.Controls.Count);
					NuGenHelperFunctions.CopyListElements(panel.Controls, CreatedControls);
				}

				if (panel.ColumnStyles.Count > 0)
				{
					ColumnStyles = new List<ColumnStyle>(panel.ColumnStyles.Count);
					NuGenHelperFunctions.CopyListElements(panel.ColumnStyles, ColumnStyles);
				}

				if (panel.RowStyles.Count > 0)
				{
					RowStyles = new List<RowStyle>(panel.RowStyles.Count);
					NuGenHelperFunctions.CopyListElements(panel.RowStyles, RowStyles);
				}

				IsInitialized = true;
			}

			panel.Visible = true;
		}

		public abstract void ReadSettings();
	}
}