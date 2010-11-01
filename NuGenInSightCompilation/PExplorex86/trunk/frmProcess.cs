
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.Threading;

namespace Genetibase.Debug
{
    public partial class frmProcess : Form
    {
        // Collection to hold processes for faster retrieval
        private ArrayList mcolProcesses = new ArrayList();

        // Child form reference to show module detail
        private frmModules mfrmMod;

        // string constants for display in listviews
        private const string PID_NA = "N/A";
        private const string PROCESS_NAME_TOTAL = "_Total (0x0)";
        private const string PROCESS_IDLE = "Idle";
        private const string PROCESS_SYSTEM = "System";


        public frmProcess()
        {
            //This call is required by the Windows Form Designer.
            InitializeComponent();
            //Add any initialization after the InitializeComponent() call
            // So that we only need to set the title of the application once,
            // we use the AssemblyInfo class (defined in the AssemblyInfo.cs file)
            // to read the AssemblyTitle attribute.

            AssemblyInfo ainfo = new AssemblyInfo();
        }

        #region " Standard Menu Code "

        // This code simply shows the About form.
        //private void mnuAbout_Click(object sender, System.EventArgs e)
        //{

        //    // Open the About form in Dialog Mode
        //    frmAbout frm = new frmAbout();
        //    frm.ShowDialog(this);
        //    frm.Dispose();
        //}

        // This code will close the form.
        private void mnuExit_Click(object sender, System.EventArgs e)
        {
            // Close the current form
            this.Close();
        }

        #endregion

        private void frmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Clean up the child form if it's there
            if (!(mfrmMod == null))
            {
                try
                {
                    mfrmMod.Owner = null;
                    mfrmMod.ParentProcess = null;
                    mfrmMod.Close();
                    mfrmMod.Dispose();
                    mfrmMod = null;
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message, exp.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmMain_Load(object sender, System.EventArgs e)
        {
            // Load the list of processes
            EnumProcesses();
        }

        private void lvProcesses_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Handle the selection changing.
            try
            {
                if (lvProcesses.SelectedItems.Count == 1)
                {
                    // Get the process id from the first subitem column
                    string strProcessId = lvProcesses.SelectedItems[0].GetRow().Cells["PID"].Value as String;
                    int ItemIndex = lvProcesses.SelectedItems[0].GetRow().Position;
                    // Check to see if we got our fake 'total' process
                    if (strProcessId == PID_NA)
                    {
                        this.lvProcessDetail.ClearItems();
                        this.lvThreads.ClearItems();
                        return;
                    }

                    Process p;

                    p = ((Process)(mcolProcesses[ItemIndex]));

                    // Get the most current data
                    p.Refresh();

                    // Get the process detail
                    EnumProcess(p);

                    // Get the thread detail
                    EnumThreads(p);
                  //  MessageBox.Show(p.StartInfo.WorkingDirectory.ToString() );
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, exp.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }

        private void mnuModules_Click(object sender, System.EventArgs e)
        {
            // Load the child form to display module information
            frmModules mfrmMod = null;
            try
            {
                if (lvProcesses.SelectedItems.Count == 1)
                {
                    string strProcessId = lvProcesses.SelectedItems[0].GetRow().Cells["PID"].Value as String;
                    int ItemIndex = lvProcesses.SelectedItems[0].GetRow().Position;
                    // Check to see if we got our fake 'total' process
                    if (strProcessId == PID_NA)
                    {
                        return;
                    }
                    Process p;
                    p = ((Process)(mcolProcesses[ItemIndex]));

                    // Don't enumerate the idle process.
                    // You will receive an access denied error.

                    if (p.ProcessName == PROCESS_IDLE)
                    {
                        MessageBox.Show(PROCESS_IDLE);
                        return;
                    }

                    // null to show
                    if (p.ProcessName == PROCESS_SYSTEM)
                    {
                        MessageBox.Show(PROCESS_SYSTEM);
                   //     return;
                    }

                    p.Refresh();

                    // Finally check to see if we can even 
                    // Get the module count.
                    // if not, no point in going further.

                    try
                    {
                        int i = p.Modules.Count;
                    }
                    catch
                    {
                        MessageBox.Show("Sorry, you are not authorized to read this information.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    // if the form is not available, load it

                    if (mfrmMod == null)
                    {
                        mfrmMod = new frmModules();
                    }

                    // Pass the selected process
                    mfrmMod.ParentProcess = p;

                    // Get the module data
                    mfrmMod.RefreshModules();

                    // Show the form
                    mfrmMod.ShowDialog(this);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, exp.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuRefresh_Click(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            EnumProcesses();
            Cursor.Current = Cursors.Default;
        }

        private void AddNameValuePair(string Item, string voidItem)
        {
            // Helper procedure to add name/value pairs to a listview control
            lvProcessDetail.AddItem(new object[] { Item, voidItem });            
        }

        private void EnumThreads(Process p)
        {
            // Get the thread information for the process.
            // This information is about the physical Win32 threads
            // not System.Threading.Thread threads.

            string strProcessId = string.Empty;

            try
            {
                this.lvThreads.ClearItems();

                if (strProcessId == PID_NA)
                {
                    this.lvThreads.AddItem(new object[] { PID_NA });
                }
                else
                {
                    //ProcessThread t;

                    // Timespans for individual thread times
                    TimeSpan tpt;
                    TimeSpan tppt;
                    TimeSpan tupt;

                    foreach (ProcessThread t in p.Threads)
                    {
                        try
                        {
                            // Get thread time and store
                            tppt = t.PrivilegedProcessorTime;
                            tupt = t.UserProcessorTime;
                            tpt = t.TotalProcessorTime;

                            // % User Processor Time for thread
                            string strPUPT;
                            if (tupt.Ticks == 0)
                            {
                                strPUPT = "0%";
                            }
                            else
                            {
                                decimal decPUPT = decimal.Divide(tupt.Ticks, tpt.Ticks);
                                strPUPT = decPUPT.ToString("#0%");
                            }


                            // % Privileged Processor Time for thread
                            string strPPPT;
                            if (tppt.Ticks == 0)
                            {
                                strPPPT = "0%";
                            }
                            else
                            {
                                Decimal decPPPT = decimal.Divide(tppt.Ticks, tpt.Ticks);
                                strPPPT = decPPPT.ToString("#0%");
                            }


                            string strTPT;
                            strTPT = (tpt.Days.ToString("00") + "." + tpt.Hours.ToString("00") + ":" + tpt.Minutes.ToString("00") + ":" + tpt.Seconds.ToString("00"));

                            string pBoost = "N/A";
                            string pLevel = "N/A";

                            try
                            {
                                pBoost = t.PriorityBoostEnabled.ToString();                                
                            }
                            catch
                            {                                
                            }

                            try
                            {
                                pLevel = t.PriorityLevel.ToString();                                
                            }
                            catch
                            {
                            }

                            lvThreads.AddItem(new object[] {t.Id.ToString(),
                                                            t.BasePriority.ToString(),
                                                            t.CurrentPriority.ToString(),
                                                            pBoost,
                                                            pLevel,
                                                            strPPPT,
                                                            t.StartAddress.ToInt32().ToString("x").ToLower(),
                                                            t.StartTime.ToShortDateString() + " " + t.StartTime.ToShortTimeString(),
                                                            strTPT,
                                                            strPUPT});
                        }
                        catch
                        {
                            this.lvThreads.AddItem(new object[] { "N/A", "N/A" });
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, exp.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnumProcess(Process p)
        {
            lvProcessDetail.ClearItems();

            if (p.ProcessName == PROCESS_IDLE)
            {
             //   return;
            }

            const string NA = "Not Authorized";

            try
            {
                AddNameValuePair("Start Time", p.StartTime.ToLongDateString() + " " + p.StartTime.ToLongTimeString());
                AddNameValuePair("Responding", p.Responding.ToString());

                try
                {
                    AddNameValuePair("Handle", p.Handle.ToString());
                }
                catch
                {
                    AddNameValuePair("Handle", NA);
                }

                AddNameValuePair("Handle Count", p.HandleCount.ToString("N0"));

                try
                {
                    AddNameValuePair("Main Window Handle", p.MainWindowHandle.ToString());
                }
                catch
                {
                    AddNameValuePair("Main Window Handle", NA);
                }

                try
                {
                    AddNameValuePair("Main Window Title", p.MainWindowTitle);
                }
                catch
                {
                    AddNameValuePair("Main Window Title", NA);
                }

                try
                {
                    // Check to make sure we have a valid reference.
                    // The System process is most notable for not exposing 
                    // any module data.

                    if (p.MainModule == null)
                    {
                        AddNameValuePair("Main Module", string.Empty);
                    }
                    else
                    {
                        AddNameValuePair("Main Module", p.MainModule.ModuleName);
                    }
                }
                catch
                {
                    AddNameValuePair("Main Module", NA);
                }

                try
                {
                    AddNameValuePair("Module Count", p.Modules.Count.ToString("N0"));
                }
                catch
                {
                    AddNameValuePair("Module Count", NA);
                }

                AddNameValuePair("Base Priority", p.BasePriority.ToString());

                try
                {
                    AddNameValuePair("Priority Boost Enabled", p.PriorityBoostEnabled.ToString());
                }
                catch
                {
                    AddNameValuePair("Priority Boost Enabled", NA);

                }

                try
                {
                    AddNameValuePair("Priority Class", p.PriorityClass.ToString());
                }
                catch
                {
                    AddNameValuePair("Priority Class", NA);
                }

                try
                {
                    AddNameValuePair("Processor Affinity", p.ProcessorAffinity.ToInt32().ToString());
                }
                catch
                {
                    AddNameValuePair("Processor Affinity", NA);
                }

                AddNameValuePair("Thread Count", p.Threads.Count.ToString());

                try
                {
                    AddNameValuePair("Min Working Set", p.MinWorkingSet.ToInt32().ToString("N0"));
                }
                catch
                {
                    AddNameValuePair("Min Working Set", NA);
                }

                try
                {
                    AddNameValuePair("Max Working Set", p.MaxWorkingSet.ToInt32().ToString("N0"));
                }
                catch
                {
                    AddNameValuePair("Max Working Set", NA);
                }

                AddNameValuePair("Working Set", p.WorkingSet64.ToString("N0"));
                AddNameValuePair("Peak Working Set", p.PeakWorkingSet64.ToString("N0"));
                AddNameValuePair("private Memory Size", p.PrivateMemorySize64.ToString("N0"));
                AddNameValuePair("Nonpaged System Memory Size", p.NonpagedSystemMemorySize64.ToString("N0"));

                AddNameValuePair("Paged Memory Size", p.PagedMemorySize64.ToString("N0"));

                AddNameValuePair("Peak Paged Memory Size", p.PeakPagedMemorySize64.ToString("N0"));

                AddNameValuePair("Virtual Memory Size", p.VirtualMemorySize64.ToString("N0"));

                AddNameValuePair("Peak Virtual Memory Size", p.PeakVirtualMemorySize64.ToString("N0"));
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, exp.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnumProcesses()
        {

            // Timespans for individual process information
            TimeSpan tpt;
            TimeSpan tppt;
            TimeSpan tupt;

            // Timespans for machine
            TimeSpan mtpt = new TimeSpan();
            TimeSpan mtppt = new TimeSpan();
            TimeSpan mtupt = new TimeSpan();

            // Enumerate all processes
            try
            {
                Process[] Processes;

                if (!(mcolProcesses == null))
                {
                    mcolProcesses = new ArrayList();
                }

                if (this.lvProcesses.RowCount > 0)
                {
                    this.lvProcesses.ClearItems();
                    this.lvProcessDetail.ClearItems();
                    this.lvThreads.ClearItems();
                }

                Processes = Process.GetProcesses();

                foreach (Process p in Processes)
                {
                    try
                    {
                        mcolProcesses.Add(p);
                        // Get processor time and store
                        tppt = p.PrivilegedProcessorTime;
                        tupt = p.UserProcessorTime;
                        tpt = p.TotalProcessorTime;

                        // Add the current process times to total times.
                        mtpt = mtpt.Add(tpt);
                        mtppt = mtppt.Add(tppt);
                        mtupt = mtupt.Add(tupt);

                        // % User Processor Time

                        decimal dblPUPT = Decimal.Divide(tupt.Ticks, tpt.Ticks);
                        string strPUPT = dblPUPT.ToString("#0%");


                        // % Privileged Processor Time
                        decimal dblPPPT = Decimal.Divide(tppt.Ticks, tpt.Ticks);
                        string strPPPT = dblPPPT.ToString("#0%");
                        string strTPT;

                        strTPT = (tpt.Days.ToString("00") + "." + tpt.Hours.ToString("00") + ":" + tpt.Minutes.ToString("00") + ":" + tpt.Seconds.ToString("00"));

                        this.lvProcesses.AddItem(new object[] { p.ProcessName + " : (0x" + (p.Id.ToString("x")).ToLower() + ")", p.Id.ToString(), strTPT, strPPPT, strPUPT });

                        //lSingleItem = this.lvProcesses.Items.Add(p.ProcessName /*+ " " + p.StartInfo.WorkingDirectory*/  + " : (0x" + (p.Id.ToString("x")).ToLower() + ")");
                        //lSingleItem.SubItems.Add(p.Id.ToString());
                        //lSingleItem.SubItems.Add(strTPT);
                        //lSingleItem.SubItems.Add(strPPPT);
                        //lSingleItem.SubItems.Add(strPUPT);
                    }
                    catch {
                        //
                        //handles access to the system idle process.
                        //the 2.0 framework does not support access to the system idle process.
                        //
                    }
                }

                // % Total User Processor Time
                decimal mdecPUPT = Decimal.Divide(mtupt.Ticks, mtpt.Ticks);
                string mstrPUPT = mdecPUPT.ToString("#0%");

                // % Total Privileged Processor Time
                decimal mdecPPPT = decimal.Divide(mtppt.Ticks, mtpt.Ticks);
                string mstrPPPT = mdecPPPT.ToString("#0%");
                string mstrTPT;

                mstrTPT = (mtpt.Days.ToString("00") + "." + mtpt.Hours.ToString("00") + ":" + mtpt.Minutes.ToString("00") + ":" + mtpt.Seconds.ToString("00"));

                this.lvProcesses.AddItem(new object[] {PROCESS_NAME_TOTAL, PID_NA, mstrTPT, mstrPPPT, mstrPUPT });

                // Add entry for all processes			
                //lSingleItem = this.lvProcesses.Items.Add(PROCESS_NAME_TOTAL);
                //lSingleItem.SubItems.Add(PID_NA);
                //lSingleItem.SubItems.Add(mstrTPT);
                //lSingleItem.SubItems.Add(mstrPPPT);
                //lSingleItem.SubItems.Add(mstrPUPT);

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, exp.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ctxViewMods_Click(object sender, EventArgs e)
        {
         
            this.mnuModules_Click(sender, e);
        }

        private void lvProcesses_DoubleClick(object sender, EventArgs e)
        {
            this.mnuModules_Click(sender, e);
        }

        private void ctxKillProc_Click(object sender, EventArgs e)
        {
             // Handle the selection changing.
            try
            {
                if (lvProcesses.SelectedItems.Count == 1)
                {
                    // Get the process id from the first subitem column
                    string strProcessId = lvProcesses.SelectedItems[0].GetRow().Cells["PID"].Value as String;
                    int ItemIndex = lvProcesses.SelectedItems[0].GetRow().Position;
                    // Check to see if we got our fake 'total' process
                    if (strProcessId == PID_NA)
                    {
                        this.lvProcessDetail.ClearItems();
                        this.lvThreads.ClearItems();
                        return;
                    }
                    Process process;
                    process = ((Process)(mcolProcesses[ItemIndex]));
                    MessageBox.Show(process.MainModule.FileName.ToString());
                    // Get the most current data
                    //process.Refresh();
                //    process.Close();
                    process.Kill();
               
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Kill Process Failed");
            }
        }

        private void mnuRunAs_Click(object sender, EventArgs e)
        {
            frmLogin frmLog = new frmLogin();
            frmLog.ShowDialog();
            
        }

        private void uiCommandManager1_CommandClick(object sender, Janus.Windows.UI.CommandBars.CommandEventArgs e)
        {
            switch (e.Command.Text)
            {
                case "Run As":
                    mnuRunAs_Click(sender, null); break;
                case "View Modules":
                    mnuModules_Click(sender, null); break;
                case "Refresh":
                    mnuRefresh_Click(sender, null); break;
            }
        }

      
    }
}