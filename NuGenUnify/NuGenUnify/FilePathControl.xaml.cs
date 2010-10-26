using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace NuGenUnify
{
	/// <summary>
	/// Interaction logic for FilePathControl.xaml
	/// </summary>
	public partial class FilePathControl : UserControl
	{
		public FilePathControl()
		{
			this.InitializeComponent();
		}
		
		public string FileName
		{
			get{return txtFileName.Text;}
            set { txtFileName.Text = value; }

		}

        public string Filter
        {
            get;
            set;
        }

        public bool SaveMode
        {
            get;
            set;
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            FileDialog fileDialog;
            if (SaveMode)
                fileDialog = new SaveFileDialog();
            else fileDialog = new OpenFileDialog();
            fileDialog.Filter = Filter;

            if (fileDialog.ShowDialog().GetValueOrDefault(false))
                txtFileName.Text = fileDialog.FileName;
        }

        public bool Validate()
        {
            txtFileName.Style = (Style)FindResource("DefaultErrorStyle");
            return string.IsNullOrEmpty(txtFileName.Text);
        }

	}
}