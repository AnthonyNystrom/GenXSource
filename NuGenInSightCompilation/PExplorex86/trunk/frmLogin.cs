/*
   Copyright (c) 2006 Talha Tariq [ talha.tariq@gmail.com ] 
   All rights are reserved.

   Permission to use, copy, modify, and distribute this software 
   for any purpose and without any fee is hereby granted, 
   provided this notice is included in its entirety in the 
   documentation and in the source files.
  
   This software and any related documentation is provided "as is" 
   without any warranty of any kind, either express or implied, 
   including, without limitation, the implied warranties of 
   merchantibility or fitness for a particular purpose. The entire 
   risk arising out of use or performance of the software remains 
   with you. 
   
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using System.Security;
using System.Windows.Forms;

namespace Genetibase.Debug
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
            tbApplicationName.Text = openFileDialog.FileName;
          //  btRun.Enabled = true;
        }

        private void btRun_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo(tbApplicationName.Text);
            psi.UserName = tbUserName.Text;
            for(int i=0; i<tbPassword.TextLength; i++)
            {
                psi.Password.AppendChar(tbPassword.Text[i]);
            }
            psi.Domain = tbDomain.Text;
            if(cbUserProfile.Checked)
                psi.LoadUserProfile = true;
            psi.UseShellExecute = false;
            try
            {
                Process.Start(psi);
                this.Dispose();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Process Creation Failed");
            }
            finally
            {
                
            }
            
            
        }
    }
}