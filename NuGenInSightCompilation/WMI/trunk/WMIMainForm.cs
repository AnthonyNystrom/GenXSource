using System;
using System.Collections;
using System.Management;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Janus.Windows.GridEX;
using System.Threading;

namespace GetHardwareInfo
{
    public partial class WMIMainForm : Form
    {
        public WMIMainForm()
        {
            InitializeComponent();
            
            cmbxOption.SelectedValue = "Win32_Processor";
            cmbxMemory.SelectedValue = "Win32_CacheMemory";
            cmbxNetwork.SelectedValue = "Win32_NetworkAdapter";
            cmbxSystemInfo.SelectedValue = "Win32_Account";
            cmbxUserAccount.SelectedValue = "Win32_Account";
            cmbxUtility.SelectedValue = "Win32_Account";
            cmbxStorage.SelectedValue = "Win32_LogicalDisk";

            lstDisplayHardware.FirstRow = 0;
        }

        private void InsertInfo(string Key, ref GridEX grid, bool DontInsertNull)
        {
            Cursor = Cursors.WaitCursor;
            foreach (Control c in Controls)
            {
                c.Enabled = false;
            }

            InsertInfoRun(Key, grid, DontInsertNull);

            foreach (Control c in Controls)
            {
                c.Enabled = true;
            }

            grid.FirstRow = 0;

            Cursor = Cursors.Default;
        }

        private static void InsertInfoRun(string Key, GridEX grid, bool DontInsertNull)
        {
            Cursor.Current = Cursors.WaitCursor;
            grid.ClearItems();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + Key);

            DateTime start = DateTime.Now;

            bool cancel = false;

            Thread t = new Thread( new ThreadStart(delegate() {
                bool run = true;

                while (run)
                {
                    TimeSpan elapsed = DateTime.Now.Subtract(start);

                    if (elapsed.Seconds > 5)
                    {
                        DialogResult res = MessageBox.Show("This operation could take a while, would you like to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (res == DialogResult.Yes)
                        {
                            run = false;
                        }
                        else
                        {
                            cancel = true;
                            run = false;                            
                        }
                    }

                    Thread.Sleep(100);
                }
            }));
            t.Start();


            try
            {
                foreach (ManagementObject share in searcher.Get())
                {
                    foreach (PropertyData PC in share.Properties)
                    {
                        if (cancel)
                        {
                            return;
                        }

                        if (PC.Value != null && PC.Value.ToString() != "")
                        {
                            grid.AddItem(new object[] { PC.Name, PC.Value, share["Name"].ToString() });
                        }
                        else
                        {
                            if (!DontInsertNull)
                                grid.AddItem(new object[] { PC.Name, "No Information available", share["Name"].ToString() });
                            else
                                continue;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                t.Abort();
                Cursor.Current = Cursors.Default;

                MessageBox.Show("Data could not be loaded: " + exp.Message +"\n\n" +
                "Some data is unavailable in certain hardware or software configurations. " +
                "If you have any concerns please contact the manufacturer of the device you " +
                "were trying to obtain information for", exp.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (t.ThreadState != ThreadState.Aborted)
            {
                t.Abort();
            }

            Cursor.Current = Cursors.Default;
        }

        private void RemoveNullValue(ref GridEX grid)
        {
            foreach (GridEXRow row in grid.GetDataRows())
            {
                foreach (GridEXCell cell in row.Cells)
                {
                    if ((String)(cell.Value) == "No Information available")
                    {
                        row.Delete();
                        continue;
                    }
                }
            }
        }

        private void RemoveNullValue(ref ListView lst)
        {
            foreach (ListViewItem item in lst.Items)
                if (item.SubItems[1].Text == "No Information available")
                    item.Remove();
        }


        #region Control events ...

        private void cmbxNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            InsertInfo(cmbxNetwork.SelectedValue.ToString(), ref lstNetwork, chkNetwork.Checked);
        }

        private void cmbxSystemInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            InsertInfo(cmbxSystemInfo.SelectedValue.ToString(), ref lstSystemInfo, chkSystemInfo.Checked);
        }

        private void cmbxUtility_SelectedIndexChanged(object sender, EventArgs e)
        {
            InsertInfo(cmbxUtility.SelectedValue.ToString(), ref lstUtility, chkUtility.Checked);
        }

        private void cmbxUserAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            InsertInfo(cmbxUserAccount.SelectedValue.ToString(), ref lstUserAccount, chkUserAccount.Checked);
        }

        private void cmbxStorage_SelectedIndexChanged(object sender, EventArgs e)
        {
            InsertInfo(cmbxStorage.SelectedValue.ToString(), ref lstStorage, chkDataStorage.Checked);
        }

        private void cmbxDeveloper_SelectedIndexChanged(object sender, EventArgs e)
        {
            InsertInfo(cmbxDeveloper.SelectedValue.ToString(), ref lstDeveloper, chkDeveloper.Checked);
        }

        private void cmbxMemory_SelectedIndexChanged(object sender, EventArgs e)
        {
            InsertInfo(cmbxMemory.SelectedValue.ToString(), ref lstMemory, chkMemory.Checked);
        }

        private void chkHardware_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHardware.Checked)
                RemoveNullValue(ref lstDisplayHardware);
            else
                InsertInfo(cmbxOption.SelectedValue.ToString(), ref lstDisplayHardware, chkHardware.Checked);
        }

        private void cmbxOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            InsertInfo(cmbxOption.SelectedValue.ToString(), ref lstDisplayHardware, chkHardware.Checked);
        }

        private void chkDataStorage_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDataStorage.Checked)
                RemoveNullValue(ref lstStorage);
            else
                InsertInfo(cmbxStorage.SelectedValue.ToString(), ref lstStorage, chkDataStorage.Checked);
        }

        private void chkMemory_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMemory.Checked)
                RemoveNullValue(ref lstMemory);
            else
                InsertInfo(cmbxMemory.SelectedValue.ToString(), ref lstStorage, false);
        }

        private void chkSystemInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSystemInfo.Checked)
                RemoveNullValue(ref lstSystemInfo);
            else
                InsertInfo(cmbxSystemInfo.SelectedValue.ToString(), ref lstSystemInfo, false);
        }

        private void chkNetwork_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNetwork.Checked)
                RemoveNullValue(ref lstNetwork);
            else
                InsertInfo(cmbxNetwork.SelectedValue.ToString(), ref lstNetwork, false);
        }

        private void chkUserAccount_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUserAccount.Checked)
                RemoveNullValue(ref lstUserAccount);
            else
                InsertInfo(cmbxUserAccount.SelectedValue.ToString(), ref lstUserAccount, false);
        }

        private void chkDeveloper_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDeveloper.Checked)
                RemoveNullValue(ref lstDeveloper);
            else
                InsertInfo(cmbxDeveloper.SelectedValue.ToString(), ref lstDeveloper, false);
        }

        private void chkUtility_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUtility.Checked)
                RemoveNullValue(ref lstUtility);
            else
                InsertInfo(cmbxUtility.SelectedValue.ToString(), ref lstUtility, false);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("http://www.ShiraziOnline.net");
        }

        #endregion

        private void gridEX1_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {

        }

       
    }
}