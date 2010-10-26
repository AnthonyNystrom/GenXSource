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
using System.Windows.Shapes;

namespace NuGenUnify
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NuGenUnify.Properties.Settings.Default.Save();
        	Close();
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	DragMove();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            NuGenUnify.Properties.Settings.Default.Reset();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            chkClosed.Focus();
        }
    }
}
