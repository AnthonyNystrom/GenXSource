using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Netron.Diagramming.Core;
namespace Netron.Cobalt
{
    public partial class DocumentProperties : Form
    {
        #region Fields
        private DiagramControlBase control;
        #endregion


        public DocumentProperties( DiagramControlBase control)
        {
            if(control == null)
                throw new InconsistencyException("The diagram control cannot be 'null'.");
            this.control = control;
           
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            #region Document information
            DocumentInformation info = control.Document.Information;
            this.documentInformation1.Author = info.Author;
            this.documentInformation1.Title = info.Title;
            this.documentInformation1.CreationDate = DateTime.Parse(info.CreationDate);
            this.documentInformation1.Description = info.Description;
            #endregion

        }

        private void TheCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TheOKButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            this.Close();
        }

        private void SaveSettings()
        {
            #region Document information
            DocumentInformation info = control.Document.Information;
            info.Author = this.documentInformation1.Author;
            info.Title = this.documentInformation1.Title;
            info.Description = this.documentInformation1.Description;
            #endregion
        }

    }
}