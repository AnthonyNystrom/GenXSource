using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NGVChem;
namespace NGVChem
{
    public partial class PDBOpenDialogControl : FileDialogControlBase
    {
        private List<String> dfPDBList = new List<string>();
        public PDBOpenDialogControl():base()
        {
            InitializeComponent();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            String pdbStr = this.txtPDBID.Text;
            if (!String.IsNullOrEmpty(pdbStr))
            {
                String[] pdbFileArray = pdbStr.Split(new String[] { ",", " ", ";" }, StringSplitOptions.RemoveEmptyEntries);
                dfPDBList.AddRange(pdbFileArray);
            }
            CloseDlg();
           
        }
        public List<String> DownloadPDBList
        {
            get
            {
                return this.dfPDBList;
            }
        }
    }
}
