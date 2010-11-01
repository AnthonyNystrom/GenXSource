using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Genetibase.Chem.NuGenSChem;

namespace Sketch.UI
{
	public partial class MainWnd : Form
	{

		public MainWnd()
		{
			InitializeComponent();
            mainControl.Init(null, false); 
		}

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!mainControl.IsStreamMode)
            {
                if (mainControl.IsDirty())
                {
                    if (MessageBox.Show(this, "Current structure has been modified. Exit without saving?",
                        "Quit", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        return;
                    }
                }
            }
            else
            {
                mainControl.WriteStream();
            }
            mainControl.Dispose();
            this.Close(); 
            return;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            mainControl.Width = this.Width;
        }

        public virtual void DirtyChanged(bool isdirty)
        {
            System.String str = Text;
            if (str[0] == '*')
                str = str.Substring(1);
            if (isdirty)
                str = "*" + str;
            Text = str;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainControl.MolData().NumAtoms() > 0)
            {
                if (MessageBox.Show(this, "Clear current structure and start anew?", "New", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                    return;
            }
            mainControl.Clear();
            
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Init(null, false); 
            mw.Show(); 
        }
      
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            System.Windows.Forms.FileDialog chooser = new OpenFileDialog();
            chooser.InitialDirectory = Directory.GetCurrentDirectory();

            // chooser.setDragEnabled(false);
            //UPGRADE_ISSUE: Method 'javax.swing.JFileChooser.setFileSelectionMode' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJFileChoosersetFileSelectionMode_int'"
            // chooser.setFileSelectionMode(0);
            //UPGRADE_ISSUE: Method 'javax.swing.JFileChooser.setFileFilter' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJFileChoosersetFileFilter_javaxswingfilechooserFileFilter'"
            chooser.Filter = "Molecular Structures|*.el;*.mol;*.sdf|CML Files|*.xml;*.cml|All Files|*.*";
            //UPGRADE_ISSUE: Method 'javax.swing.JFileChooser.setAccessory' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJFileChoosersetAccessory_javaxswingJComponent'"
            // chooser.setAccessory(new FileMolPreview(chooser));
            //UPGRADE_TODO: The equivalent in .NET for method 'javax.swing.JFileChooser.showOpenDialog' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            //UPGRADE_TODO: The equivalent in .NET for field 'javax.swing.JFileChooser.APPROVE_OPTION' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            if ((int)chooser.ShowDialog(this) != (int)System.Windows.Forms.DialogResult.OK)
                return;

            mainControl.Open(chooser.FileName); 
            
            
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainControl.Save(); 
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FileDialog chooser = new SaveFileDialog();
            chooser.InitialDirectory = Directory.GetCurrentDirectory();

            // chooser.setDragEnabled(false);
            //UPGRADE_ISSUE: Method 'javax.swing.JFileChooser.setFileSelectionMode' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJFileChoosersetFileSelectionMode_int'"
            // chooser.setFileSelectionMode(0);
            //UPGRADE_ISSUE: Method 'javax.swing.JFileChooser.setFileFilter' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJFileChoosersetFileFilter_javaxswingfilechooserFileFilter'"
            chooser.Filter = "SketchEl Files | *.el";
            //UPGRADE_ISSUE: Method 'javax.swing.JFileChooser.setAccessory' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJFileChoosersetAccessory_javaxswingJComponent'"
            // TODO: Again, figure out what this does in java  
            // chooser.setAccessory(new FileMolPreview(chooser));

            //UPGRADE_TODO: The equivalent in .NET for method 'javax.swing.JFileChooser.showSaveDialog' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            //UPGRADE_TODO: The equivalent in .NET for field 'javax.swing.JFileChooser.APPROVE_OPTION' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            if ((int)chooser.ShowDialog(this) != (int)System.Windows.Forms.DialogResult.OK)
                return;

             mainControl.SaveAs(chooser.FileName); 
           
        }

        private void asMODToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FileDialog chooser = new SaveFileDialog();
            chooser.InitialDirectory = Directory.GetCurrentDirectory();
            // chooser.setDragEnabled(false);
            //UPGRADE_ISSUE: Method 'javax.swing.JFileChooser.setFileSelectionMode' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJFileChoosersetFileSelectionMode_int'"
            // chooser.setFileSelectionMode(0);
            chooser.Filter = "MDL MOL Files | *.mol";

            // TODO: What does this do? 
            // chooser.setAccessory(new FileMolPreview(chooser));

            if ((int)chooser.ShowDialog(this) != (int)System.Windows.Forms.DialogResult.OK)
                return;

            mainControl.ExportMOL(chooser.FileName); 
        }

        private void asCMLXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileDialog chooser = new SaveFileDialog();
            chooser.InitialDirectory = Directory.GetCurrentDirectory();

            // chooser.setDragEnabled(false);
            //UPGRADE_ISSUE: Method 'javax.swing.JFileChooser.setFileSelectionMode' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJFileChoosersetFileSelectionMode_int'"
            // chooser.setFileSelectionMode(0);
            //UPGRADE_ISSUE: Method 'javax.swing.JFileChooser.setFileFilter' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJFileChoosersetFileFilter_javaxswingfilechooserFileFilter'"
            chooser.Filter = "XML Files | *.xml";
            //UPGRADE_ISSUE: Method 'javax.swing.JFileChooser.setAccessory' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJFileChoosersetAccessory_javaxswingJComponent'"
            // TODO: again, ... 
            // chooser.setAccessory(new FileMolPreview(chooser));
            //UPGRADE_TODO: The equivalent in .NET for method 'javax.swing.JFileChooser.showSaveDialog' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            //UPGRADE_TODO: The equivalent in .NET for field 'javax.swing.JFileChooser.APPROVE_OPTION' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            if ((int)chooser.ShowDialog(this) != (int)System.Windows.Forms.DialogResult.OK)
                return;

            mainControl.ExportCML(chooser.FileName); 
            
        }

        private void cursorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainControl.actionPerformed(sender, e); 
        }


	}
}