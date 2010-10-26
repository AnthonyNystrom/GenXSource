using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;

namespace NuGenUnify
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();
		}

        void TopBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	System.Windows.Application.Current.Shutdown();
        }

        private void btnMaximize_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	    IntPtr windowHandle;
				System.Drawing.Rectangle workingArea;
				Screen screen;
			
				windowHandle = new WindowInteropHelper(this).Handle;
				screen = Screen.FromHandle(windowHandle);
				workingArea = screen.WorkingArea;
			
				Left = workingArea.Left;
				Top = workingArea.Top;
				Width = workingArea.Width;
				Height = workingArea.Height;
        }

        private void btnMinimize_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	 WindowState= WindowState.Minimized;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	System.Windows.MessageBox.Show(fileMainAssembly.FileName);
        }

        private void btnSettings_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dlg = new ProgressDialog();
            dlg.Owner = this;
            dlg.ShowDialog();
        	var form = new SettingsWindow();
            form.Owner = this;
			form.ShowDialog();
        }

        private void btnMerge_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ProgressDialog();
            dlg.Owner = this;
            object[] args = new object[] { lstAssemblies.Assemblies, fileMainAssembly.FileName, fnaOutput.FileName , fnaLog.FileName};
            dlg.RunWorkerThread(args, Merge);
        }

        BackgroundWorker worker =null;
        private void Merge(object sender, DoWorkEventArgs e)
        {
            object[] args = (object[] )e.Argument;

            worker = (BackgroundWorker)sender;
            ILMergeManager merger = new ILMergeManager();
            merger.OtherAssemblies = (List<string>)args[0];
            merger.PrimaryAssembly = (string)args[1];
            merger.OutputFile = (string)args[2];
            merger.LogFile = (string)args[3];

            merger.StatusChanged += new StatusEventHandler(merger_StatusChanged);
            if (!merger.CheckILMerge())
                throw new Exception("Can't found ILMerge utility.");
               merger.Merge();
        }

        void merger_StatusChanged(object sender, StatusEventArgs e)
        {
            worker.ReportProgress(int.MinValue, e.Status);
        }
	}
}