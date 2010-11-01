using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenTransform
{
    public partial class GeometryDisplayDialog : Form
    {
        public GeometryDisplayDialog(string[,] rows, int nRows)
        {
            InitializeComponent();            

            for (int i = 0; i < nRows; i++)
            {
                string[] curRow = new string[dataGridView1.ColumnCount];

                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    try
                    {
                        curRow[j] = rows[i, j];
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        curRow[j] = null;
                    }
                }

                dataGridView1.Rows.Add(curRow);
            }

            dataGridView1.Width = dataGridView1.Rows[0].Cells.Count * dataGridView1.Rows[0].Cells[0].Size.Width + 44;
            dataGridView1.Height = dataGridView1.Rows.Count * dataGridView1.Rows[0].Cells[0].Size.Height + 20;
            if (dataGridView1.Height > 600)
                dataGridView1.Height = 600;           
            this.MaximumSize= new System.Drawing.Size(dataGridView1.Width + 65, dataGridView1.Height + 65);
        }

        public static void Show(List<NuGenGeometryWindowItem> info, Point loc, Size size)
        {
            Size s = TableSize(info);            
            string[,] rows = new string[s.Height, s.Width];

            int nRows = s.Height;

            foreach (NuGenGeometryWindowItem item in info)
            {
                rows[item.Row, item.Column] = item.Entry;
            }

            GeometryDisplayDialog dlg = new GeometryDisplayDialog(rows, nRows);
            dlg.Location = loc;
            dlg.Size = size;
            dlg.Show();
        }

        public static Size TableSize(List<NuGenGeometryWindowItem> info)
        {
            int rMax = 0;
            int cMax = 0;
            foreach (NuGenGeometryWindowItem item in info)
            {
                if (item.Row > rMax)
                    rMax = item.Row;

                if (item.Column > cMax)
                    cMax = item.Column;
            }

            return new Size(cMax + 1, rMax + 1);
        }
    }
}