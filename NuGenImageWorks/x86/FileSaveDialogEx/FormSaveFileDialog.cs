//  Copyright (c) 2006, Gustavo Franco
//  Email:  gustavo_franco@hotmail.com
//  All rights reserved.

//  Redistribution and use in source and binary forms, with or without modification, 
//  are permitted provided that the following conditions are met:

//  Redistributions of source code must retain the above copyright notice, 
//  this list of conditions and the following disclaimer. 
//  Redistributions in binary form must reproduce the above copyright notice, 
//  this list of conditions and the following disclaimer in the documentation 
//  and/or other materials provided with the distribution. 

//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE. IT CAN BE DISTRIBUTED FREE OF CHARGE AS LONG AS THIS HEADER 
//  REMAINS UNCHANGED.

using System;
using System.IO;
using System.Data;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Collections.Generic;
using Genetibase.UI.NuGenImageWorks;

using Genetibase.UI.FileSaveDialogEx;

namespace Genetibase.UI.FileSaveDialogEx
{
    
    public partial class FormSaveFileDialog : SaveFileDialogEx
    {
        MainForm frmMain = null;

        #region Constructors
        public FormSaveFileDialog(MainForm frmMain)
        {
            this.frmMain = frmMain;
            InitializeComponent();
        }

        public FormSaveFileDialog()
        {
            InitializeComponent();
        }
        #endregion

        #region Overrides
        public override void OnFileNameChanged(string filePath)
        {
        }

        public override void OnFolderNameChanged(string folderName)
        {        
        }

        public override void OnClosingDialog()
        {
            if (pbxPreview.Image != null)
                pbxPreview.Image.Dispose();
        }
        #endregion

        

        private void pnlBgColor_Click(object sender, EventArgs e)
        {
            ColorDialog clrDlg = new ColorDialog();

            DialogResult res = clrDlg.ShowDialog();

            if (res != DialogResult.OK)
                return;

            Application.DoEvents();

            this.pnlBgColor.Image = null;
            this.pnlBgColor.BackColor = clrDlg.Color;

            Extra.Enable = true;
            Extra.BackgroundColor = clrDlg.Color;

            Bitmap b = frmMain.FilterEffects(false);

            if (this.pbxPreview.Image != null)
            {
                Image prev = this.pbxPreview.Image;
                this.pbxPreview.Image = null;
                prev.Dispose();
            }

            this.pbxPreview.Image = b;

        }

        private void FormSaveFileDialog_Load(object sender, EventArgs e)
        {
            Bitmap b = frmMain.FilterEffects(false);
            this.pbxPreview.Image = b;           
        }
    }
}