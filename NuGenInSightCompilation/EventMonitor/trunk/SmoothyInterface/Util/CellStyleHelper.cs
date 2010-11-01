using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SmoothyInterface.Util
{
	public class CellStyleHelper
	{
		public static DataGridViewCellStyle GetColorStyle(System.Drawing.Color color)
		{
			DataGridViewCellStyle style = new DataGridViewCellStyle();
			style.BackColor = color;
			return style;
		}
	}
}
