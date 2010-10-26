using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NuGenSVisualLib.Logging;

namespace NuGenSVisualLib
{
    partial class UleDlg : Form
    {
        MoleculeLoadingResults results;

        public UleDlg(MoleculeLoadingResults results, LoadingProgress progress)
        {
            InitializeComponent();

            this.results = results;

            LogDataTreeNodeFormatting logFormatting = new LogDataTreeNodeFormatting();
            progress.CollateLogData(LogItem.ItemLevel.UserInfo, logFormatting);
            treeView1.Nodes.Add(logFormatting.GetRoot());
        }

        private void UleDlg_Load(object sender, EventArgs e)
        {
            // load icon
            Icon icon = null;
            string message;
            bool success = false;
            switch (results.Result)
            {
                case MoleculeLoadingResults.Results.Success:
                    //icon = SystemIcons.Information;
                    message = "File loaded with success";
                    success = true;
                    break;
                case MoleculeLoadingResults.Results.Warnings:
                    icon = SystemIcons.Warning;
                    message = "File loaded with some warnings";
                    success = true;
                    break;
                case MoleculeLoadingResults.Results.Problems:
                    icon = SystemIcons.Asterisk;
                    message = "File not loaded due to problems";
                    break;
                case MoleculeLoadingResults.Results.Errors:
                    icon = SystemIcons.Error;
                    message = "File not loaded due to errors";
                    break;
                default:
                    message = "";
                    break;
            }
            if (icon != null)
                pictureBox1.Image = icon.ToBitmap();
            label14.Text = message;

            if (success)
            {
                label3.Text = results.FileFormat.FormatName;
                label5.Text = results.NumSequences.ToString();
                label7.Text = results.NumModels.ToString();
                label9.Text = results.NumMolecules.ToString();
                label11.Text = results.NumAtoms.ToString();
                label13.Text = results.NumBonds.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}