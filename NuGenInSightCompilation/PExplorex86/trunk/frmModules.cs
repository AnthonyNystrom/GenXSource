
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

namespace Genetibase.Debug
{
    public partial class frmModules : Form
    {
        public frmModules()
        {
            InitializeComponent();
        }
        private Process m_ParentProcess;

        private List<ProcessModule> _mcolModules = new List<ProcessModule>();

        public Process ParentProcess
        {
            get
            {
                return ParentProcess;
            }
            set
            {
                m_ParentProcess = value;

                if (m_ParentProcess == null)
                {
                    _mcolModules = null;
                }

            }

        }

        private void EnumModules()
        {
            try
            {
                this.lvModules.ClearItems();                

                if (!(_mcolModules == null))
                {
                    _mcolModules = new List<ProcessModule>();
                }

                foreach (ProcessModule m in m_ParentProcess.Modules)
                {
                    this.lvModules.AddItem(new object[] { m.ModuleName });                    
                    try
                    {
                        _mcolModules.Add(m);
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message, exp.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, exp.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RefreshModules()
        {
            this.uiStatusBar1.Panels[0].Text = "Process = " + m_ParentProcess.ProcessName;
            this.lvModDetail.ClearItems();
            EnumModules();
        }

        private void mnuClose_Click(object sender, System.EventArgs e)
        {
            this.Hide();
        }

        private void EnumModule(ProcessModule m)
        {
            this.lvModDetail.ClearItems();
            try
            {
                lvModDetail.AddItem(new object[] { "Base Address", m.BaseAddress.ToInt32().ToString("x").ToLower() });
                lvModDetail.AddItem(new object[] { "Entry Point Address", m.EntryPointAddress.ToInt32().ToString("x").ToLower() });
                lvModDetail.AddItem(new object[] { "File Name", m.FileName });
                lvModDetail.AddItem(new object[] { "File Version", m.FileVersionInfo.FileVersion.ToString() });
                lvModDetail.AddItem(new object[] { "File Description", m.FileVersionInfo.FileDescription.ToString() });
                lvModDetail.AddItem(new object[] { "Memory Size", m.ModuleMemorySize.ToString("N0") });
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, exp.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lvModules_SelectionChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (lvModules.SelectedItems.Count == 1)
                {
                    string strMode = lvModules.GetValue("Name") as String;
                    ProcessModule m = ((ProcessModule)(_mcolModules[lvModules.Row]));
                     EnumModule(m);
                }                
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, exp.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void splVert_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }
    }
}