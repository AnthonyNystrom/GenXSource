using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for AssemblyListControl.xaml
    /// </summary>
    public partial class AssemblyListControl : UserControl
    {
        public AssemblyListControl()
        {
            InitializeComponent();
        }

        private void btnAddAsssembly_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Exe files (*.exe)|*.exe|Dll files (*.dll)|*.dll|All files (*.*)|*.*";
            fileDialog.Multiselect = true;
            if (fileDialog.ShowDialog().GetValueOrDefault(false))
                AddAssembly(fileDialog.FileNames);
		}

        private void AddAssembly(string[] fileNames)
        {
            foreach (string fileName in fileNames)
            {
                ListViewItem item = new ListViewItem();
                item.Content = System.IO.Path.GetFileName(fileName);
                item.ToolTip = fileName;
                lvAssemblies.Items.Add(item);
            }
        }

        private void btnRemoveAsssembly_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            while (lvAssemblies.SelectedItems.Count > 0)
            {
                lvAssemblies.Items.Remove(lvAssemblies.SelectedItem);
            }
		}

        public List<string> Assemblies
        {
            get
            {
                List<string> result = new List<string>();
                foreach (ListViewItem item in lvAssemblies.Items)
                    result.Add((string)item.Content);
                    return result;
            }
        }
    }
}
