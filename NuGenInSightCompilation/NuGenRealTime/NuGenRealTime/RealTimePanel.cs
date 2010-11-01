using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Genetibase.UI.NuGenMeters;
using System.Runtime.Remoting;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Diagnostics;

namespace NuGenRealTime
{
    public partial class RealTimePanel : UserControl
    {
        public RealTimePanel()
        {
            InitializeComponent();
            AssociateButtons();

            this.nuGenPercentMemoryTimeGraph2.ValueChanged += new Genetibase.Shared.NuGenTargetEventHandler(nuGenPercentMemoryTimeGraph1_ValueChanged);
        }

        void nuGenPercentMemoryTimeGraph1_ValueChanged(object sender, Genetibase.Shared.NuGenTargetEventArgs e)
        {
            System.Diagnostics.PerformanceCounter counter = new System.Diagnostics.PerformanceCounter("Memory", "Available KBytes");
            this.label6.Text = "Free Memory: " + counter.NextValue().ToString() + " KBytes";
            counter = new System.Diagnostics.PerformanceCounter("System", "Processes");
            this.label8.Text = "Running Processes :" + counter.NextValue().ToString();
        }

        public List<String> GraphInstances
        {
            get
            {
                if (uiTab1.SelectedIndex != 1)
                {
                    return new List<string>();
                }

                NuGenGraphBase graph = null;

                foreach (Control c in uiTabPage2.Controls)
                {
                    if (c is NuGenGraphBase)
                    {
                        graph = (NuGenGraphBase)c;
                    }
                }

                if (graph == null)
                {
                    return new List<string>();
                }

                List<string> instances = new List<string>();
                System.Diagnostics.PerformanceCounterCategory cat = new System.Diagnostics.PerformanceCounterCategory(graph.CategoryName);
                foreach (String instance in cat.GetInstanceNames())
                {
                    instances.Add(instance);
                }

                return instances;
            }
        }

        public String GraphInstance
        {
            get
            {
                if (uiTab1.SelectedIndex != 1)
                {
                    return "";
                }

                NuGenGraphBase graph = null;

                foreach (Control c in uiTabPage2.Controls)
                {
                    if (c is NuGenGraphBase)
                    {
                        graph = (NuGenGraphBase)c;
                    }
                }

                if (graph == null)
                {
                    return "";
                }
                else
                {
                    return graph.InstanceName;
                }
            }

            set
            {
                if (uiTab1.SelectedIndex != 1)
                {
                    return;
                }

                NuGenGraphBase graph = null;

                foreach (Control c in uiTabPage2.Controls)
                {
                    if (c is NuGenGraphBase)
                    {
                        graph = (NuGenGraphBase)c;
                    }
                }

                if (graph == null)
                {
                    return;
                }
                else
                {
                    graph.InstanceName = value;
                }
            }
            
        }

        public String Instance
        {
            get
            {
                return nuGenWorkingSetGraph1.InstanceName;
            }

            set
            {
                nuGenWorkingSetGraph1.InstanceName = value;
                nuGenPercentProcessorTimeGraph5.InstanceName = value;
                nuGenIoDataBytesPerSecGraph1.InstanceName = value;
                nuGenVirtualBytesGraph3.InstanceName = value;

                if (GraphInstances.Contains(value))
                {
                    GraphInstance = value;
                }
            }
        }

        private void AssociateButtons()
        {
            #region JobObject

            #region General

            ButtonMeterMap.AssociateButton(button1, "Genetibase.UI.NuGenMeters.JobObject.NuGenCurrentPercentKernelModeTimeGraph");
			button1.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem2, "Genetibase.UI.NuGenMeters.JobObject.NuGenCurrentPercentProcessorTimeGraph");
			buttonItem2.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem11, "Genetibase.UI.NuGenMeters.JobObject.NuGenCurrentPercentUserModeTimeGraph");
			buttonItem11.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem12, "Genetibase.UI.NuGenMeters.JobObject.NuGenPagesPerSecGraph");
			buttonItem12.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem16, "Genetibase.UI.NuGenMeters.JobObject.NuGenProcessCountActiveGraph");
			buttonItem16.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem17, "Genetibase.UI.NuGenMeters.JobObject.NuGenProcessCountTerminatedGraph");
			buttonItem17.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem18, "Genetibase.UI.NuGenMeters.JobObject.NuGenProcessCountTotalGraph");
			buttonItem18.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem19, "Genetibase.UI.NuGenMeters.JobObject.NuGenThisPeriodMSecKernelModeGraph");
			buttonItem19.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem20, "Genetibase.UI.NuGenMeters.JobObject.NuGenThisPeriodMSecProcessorGraph");
			buttonItem20.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem21, "Genetibase.UI.NuGenMeters.JobObject.NuGenThisPeriodMSecUserModeGraph");
			buttonItem21.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem22, "Genetibase.UI.NuGenMeters.JobObject.NuGenTotalMSecKernelModeGraph");
			buttonItem22.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem23, "Genetibase.UI.NuGenMeters.JobObject.NuGenTotalMSecProcessorGraph");
			buttonItem23.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem24, "Genetibase.UI.NuGenMeters.JobObject.NuGenTotalMSecUserModeGraph");
			buttonItem24.Click += new EventHandler(this.buttonClick);

            #endregion

            #region Details

            ButtonMeterMap.AssociateButton(buttonItem3, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenCreatingProcessIdGraph");
			buttonItem3.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem25, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenElapsedTimeGraph");
			buttonItem25.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem26, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenHandleCountGraph");
			buttonItem26.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem27, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenIdProcessGraph");
			buttonItem27.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem28, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenIoDataBytesPerSecGraph");
			buttonItem28.Click += new EventHandler(this.buttonClick);

            //ButtonMeterMap.AssociateButton(buttonItem29, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenIoDataOperationsPerSecGraph");
			//buttonItem29.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem30, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenIoOtherBytesPerSecGraph");
			buttonItem30.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem31, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenIoOtherOperationsPerSecGraph");
			buttonItem31.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem32, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenIoReadBytesPerSecGraph");
			buttonItem32.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem33, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenIoReadOperationsPerSecGraph");
			buttonItem33.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem34, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenIoWriteBytesPerSecGraph");
			buttonItem34.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem1, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenIoWriteOperationsPerSecGraph");
			buttonItem1.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem35, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenPageFaultsPerSecGraph");
			buttonItem35.Click += new EventHandler(this.buttonClick);

            //ButtonMeterMap.AssociateButton(buttonItem36, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenPageFileBytesGraph");
			//buttonItem36.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem37, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenPageFileBytesPeakGraph");
			buttonItem37.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem38, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenPercentPrivilegedTimeGraph");
			buttonItem38.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem39, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenPercentProcessorTimeGraph");
			buttonItem39.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem40, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenPercentUserTimeGraph");
			buttonItem40.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem41, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenPoolNonpagedBytesGraph");
			buttonItem41.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem42, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenPoolPagedBytesGraph");
			buttonItem42.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem43, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenPriorityBaseGraph");
			buttonItem43.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem44, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenPrivateBytesGraph");
			buttonItem44.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem45, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenThreadCountGraph");
			buttonItem45.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem46, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenVirtualBytesGraph");
			buttonItem46.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem47, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenVirtualBytesPeakGraph");
			buttonItem47.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem48, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenWorkingSetGraph");
			buttonItem48.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem49, "Genetibase.UI.NuGenMeters.JobObjectDetails.NuGenWorkingSetPeakGraph");
			buttonItem49.Click += new EventHandler(this.buttonClick);
            #endregion

            #endregion

            #region Hard Drives

            #region Physical Disk

            ButtonMeterMap.AssociateButton(buttonItem50, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenAvgDiskBytesPerReadGraph");
			buttonItem50.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem51, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenAvgDiskBytesPerTransferGraph");
			buttonItem51.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem52, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenAvgDiskBytesPerWriteGraph");
			buttonItem52.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem53, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenAvgDiskQueueLengthGraph");
			buttonItem53.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem54, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenAvgDiskSecPerReadGraph");
			buttonItem54.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem55, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenAvgDiskSecPerTransferGraph");
			buttonItem55.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem56, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenAvgDiskSecPerWriteGraph");
			buttonItem56.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem57, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenAvgDiskWriteQueueLengthGraph");
			buttonItem57.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem58, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenCurrentDiskQueueLengthGraph");
			buttonItem58.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem59, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenDiskBytesPerSecGraph");
			buttonItem59.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem60, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenDiskReadBytesPerSecGraph");
			buttonItem60.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem61, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenDiskReadsPerSecGraph");
			buttonItem61.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem62, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenDiskTransfersPerSecGraph");
			buttonItem62.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem63, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenDiskWriteBytesPerSecGraph");
			buttonItem63.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem64, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenDiskWritesPerSecGraph");
			buttonItem64.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem65, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenPercentDiskReadTimeGraph");
			buttonItem65.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem66, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenPercentDiskTimeGraph");
			buttonItem66.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem67, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenPercentDiskWriteTimeGraph");
			buttonItem67.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem68, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenPercentIdleTimeGraph");
			buttonItem68.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem69, "Genetibase.UI.NuGenMeters.PhysicalDisk.NuGenSplitIoPerSecGraph");
			buttonItem69.Click += new EventHandler(this.buttonClick);

            #endregion

            #region Logical Disk

            ButtonMeterMap.AssociateButton(buttonItem70, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenAvgDiskBytesPerReadGraph");
			buttonItem70.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem71, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenAvgDiskBytesPerTransferGraph");
			buttonItem71.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem72, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenAvgDiskBytesPerWriteGraph");
			buttonItem72.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem73, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenAvgDiskQueueLengthGraph");
			buttonItem73.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem74, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenAvgDiskSecPerReadGraph");
			buttonItem74.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem75, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenAvgDiskSecPerTransferGraph");
			buttonItem75.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem76, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenAvgDiskSecPerWriteGraph");
			buttonItem76.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem77, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenAvgDiskWriteQueueLengthGraph");
			buttonItem77.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem78, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenCurrentDiskQueueLengthGraph");
			buttonItem78.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem79, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenDiskBytesPerSecGraph");
			buttonItem79.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem80, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenDiskReadBytesPerSecGraph");
			buttonItem80.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem81, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenDiskReadsPerSecGraph");
			buttonItem81.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem82, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenDiskTransfersPerSecGraph");
			buttonItem82.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem83, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenDiskWriteBytesPerSecGraph");
			buttonItem83.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem84, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenDiskWritesPerSecGraph");
			buttonItem84.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem85, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenPercentDiskReadTimeGraph");
			buttonItem85.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem86, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenPercentDiskTimeGraph");
			buttonItem86.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem87, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenPercentDiskWriteTimeGraph");
			buttonItem87.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem88, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenPercentIdleTimeGraph");
			buttonItem88.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem89, "Genetibase.UI.NuGenMeters.LogicalDisk.NuGenSplitIoPerSecGraph");
			buttonItem89.Click += new EventHandler(this.buttonClick);

            #endregion

            #endregion

            #region Threads and Processes

            #region Processor

            ButtonMeterMap.AssociateButton(buttonItem90, "Genetibase.UI.NuGenMeters.Processor.NuGenC1TransitionsPerSecGraph");
			buttonItem90.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem91, "Genetibase.UI.NuGenMeters.Processor.NuGenC2TransitionsPerSecGraph");
			buttonItem91.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem92, "Genetibase.UI.NuGenMeters.Processor.NuGenC3TransitionsPerSecGraph");
			buttonItem92.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem93, "Genetibase.UI.NuGenMeters.Processor.NuGenDpcRateGraph");
			buttonItem93.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem94, "Genetibase.UI.NuGenMeters.Processor.NuGenDPCsQueuedPerSecGraph");
			buttonItem94.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem95, "Genetibase.UI.NuGenMeters.Processor.NuGenInterruptsPerSecGraph");
			buttonItem95.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem96, "Genetibase.UI.NuGenMeters.Processor.NuGenPercentC1TimeGraph");
			buttonItem96.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem97, "Genetibase.UI.NuGenMeters.Processor.NuGenPercentC2TimeGraph");
			buttonItem97.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem98, "Genetibase.UI.NuGenMeters.Processor.NuGenPercentC3TimeGraph");
			buttonItem98.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem99, "Genetibase.UI.NuGenMeters.Processor.NuGenPercentDpcTimeGraph");
			buttonItem99.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem100, "Genetibase.UI.NuGenMeters.Processor.NuGenPercentIdleTimeGraph");
			buttonItem100.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem101, "Genetibase.UI.NuGenMeters.Processor.NuGenPercentInterruptTimeGraph");
			buttonItem101.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem102, "Genetibase.UI.NuGenMeters.Processor.NuGenPercentPrivilegedTimeGraph");
			buttonItem102.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem103, "Genetibase.UI.NuGenMeters.Processor.NuGenPercentProcessorTimeGraph");
			buttonItem103.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem104, "Genetibase.UI.NuGenMeters.Processor.NuGenPercentUserTimeGraph");
			buttonItem104.Click += new EventHandler(this.buttonClick);

            #endregion

            #region Processes

            ButtonMeterMap.AssociateButton(buttonItem105, "Genetibase.UI.NuGenMeters.Process.NuGenCreatingProcessIdGraph");
			buttonItem105.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem106, "Genetibase.UI.NuGenMeters.Process.NuGenElapsedTimeGraph");
			buttonItem106.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem107, "Genetibase.UI.NuGenMeters.Process.NuGenHandleCountGraph");
			buttonItem107.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem108, "Genetibase.UI.NuGenMeters.Process.NuGenIdProcessGraph");
			buttonItem108.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem109, "Genetibase.UI.NuGenMeters.Process.NuGenIoDataBytesPerSecGraph");
			buttonItem109.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem110, "Genetibase.UI.NuGenMeters.Process.NuGenIoDataOperationsPerSecGraph");
			buttonItem110.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem111, "Genetibase.UI.NuGenMeters.Process.NuGenIoOtherBytesPerSecGraph");
			buttonItem111.Click += new EventHandler(this.buttonClick);

            //ButtonMeterMap.AssociateButton(buttonItem112, "Genetibase.UI.NuGenMeters.Process.NuGenIoOtherOperationsPerSecGraph");
			//buttonItem112.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem113, "Genetibase.UI.NuGenMeters.Process.NuGenIoReadBytesPerSecGraph");
			buttonItem113.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem114, "Genetibase.UI.NuGenMeters.Process.NuGenIoReadOperationsPerSecGraph");
			buttonItem114.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem115, "Genetibase.UI.NuGenMeters.Process.NuGenIoWriteBytesPerSecGraph");
			buttonItem115.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem116, "Genetibase.UI.NuGenMeters.Process.NuGenIoWriteOperationsPerSecGraph");
			buttonItem116.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem117, "Genetibase.UI.NuGenMeters.Process.NuGenPageFaultsPerSecGraph");
			buttonItem117.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem118, "Genetibase.UI.NuGenMeters.Process.NuGenPageFileBytesGraph");
			buttonItem118.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem119, "Genetibase.UI.NuGenMeters.Process.NuGenPageFileBytesPeakGraph");
			buttonItem119.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem120, "Genetibase.UI.NuGenMeters.Process.NuGenPercentPrivilegedTimeGraph");
			buttonItem120.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem121, "Genetibase.UI.NuGenMeters.Process.NuGenPercentProcessorTimeGraph");
			buttonItem121.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem122, "Genetibase.UI.NuGenMeters.Process.NuGenPercentUserTimeGraph");
			buttonItem122.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem123, "Genetibase.UI.NuGenMeters.Process.NuGenPoolNonpagedBytesGraph");
			buttonItem123.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem124, "Genetibase.UI.NuGenMeters.Process.NuGenPoolPagedBytesGraph");
			buttonItem124.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem125, "Genetibase.UI.NuGenMeters.Process.NuGenPriorityBaseGraph");
			buttonItem125.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem126, "Genetibase.UI.NuGenMeters.Process.NuGenPrivateBytesGraph");
			buttonItem126.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem127, "Genetibase.UI.NuGenMeters.Process.NuGenThreadCountGraph");
			buttonItem127.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem128, "Genetibase.UI.NuGenMeters.Process.NuGenVirtualBytesGraph");
			buttonItem128.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem129, "Genetibase.UI.NuGenMeters.Process.NuGenVirtualBytesPeakGraph");
			buttonItem129.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem130, "Genetibase.UI.NuGenMeters.Process.NuGenWorkingSetGraph");
			buttonItem130.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem131, "Genetibase.UI.NuGenMeters.Process.NuGenWorkingSetPeakGraph");
			buttonItem131.Click += new EventHandler(this.buttonClick);

            #endregion

            #region Threads

            ButtonMeterMap.AssociateButton(buttonItem132, "Genetibase.UI.NuGenMeters.Thread.NuGenContextSwithcesPerSecGraph");
			buttonItem132.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem133, "Genetibase.UI.NuGenMeters.Thread.NuGenElapsedTimeGraph");
			buttonItem133.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem134, "Genetibase.UI.NuGenMeters.Thread.NuGenIdProcessGraph");
			buttonItem134.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem135, "Genetibase.UI.NuGenMeters.Thread.NuGenIdThreadGraph");
			buttonItem135.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem136, "Genetibase.UI.NuGenMeters.Thread.NuGenPercentPrivilegedTimeGraph");
			buttonItem136.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem137, "Genetibase.UI.NuGenMeters.Thread.NuGenPercentProcessorTimeGraph");
			buttonItem137.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem138, "Genetibase.UI.NuGenMeters.Thread.NuGenPercentUserTimeGraph");
			buttonItem138.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem139, "Genetibase.UI.NuGenMeters.Thread.NuGenPriorityBaseGraph");
			buttonItem139.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem140, "Genetibase.UI.NuGenMeters.Thread.NuGenPriorityCurrentGraph");
			buttonItem140.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem141, "Genetibase.UI.NuGenMeters.Thread.NuGenStartAddressGraph");
			buttonItem141.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem142, "Genetibase.UI.NuGenMeters.Thread.NuGenThreadStateGraph");
			buttonItem142.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem143, "Genetibase.UI.NuGenMeters.Thread.NuGenThreadWaitReasonGraph");
			buttonItem143.Click += new EventHandler(this.buttonClick);

            #endregion

            #endregion

            #region .NET CLR

            #region Exceptions


            ButtonMeterMap.AssociateButton(buttonItem144, "Genetibase.UI.NuGenMeters.NetClrExceptions.NuGenNumberOfExcepsThrownGraph");
			buttonItem144.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem145, "Genetibase.UI.NuGenMeters.NetClrExceptions.NuGenNumberOfExcepsThrownPerSecGraph");
			buttonItem145.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem146, "Genetibase.UI.NuGenMeters.NetClrExceptions.NuGenNumberOfFiltersPerSecGraph");
			buttonItem146.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem147, "Genetibase.UI.NuGenMeters.NetClrExceptions.NuGenNumberOfFinallysPerSecGraph");
			buttonItem147.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem148, "Genetibase.UI.NuGenMeters.NetClrExceptions.NuGenThrowToCatchDepthGraph");
			buttonItem148.Click += new EventHandler(this.buttonClick);

            #endregion

            #region Interop

            ButtonMeterMap.AssociateButton(buttonItem149, "Genetibase.UI.NuGenMeters.NetClrInterop.NuGenNumberOfCCWsGraph");
			buttonItem149.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem150, "Genetibase.UI.NuGenMeters.NetClrInterop.NuGenNumberOfMarshallingGraph");
			buttonItem150.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem151, "Genetibase.UI.NuGenMeters.NetClrInterop.NuGenNumberOfStubsGraph");
			buttonItem151.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem152, "Genetibase.UI.NuGenMeters.NetClrInterop.NuGenNumberOfTlbExportsPerSecGraph");
			buttonItem152.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem153, "Genetibase.UI.NuGenMeters.NetClrInterop.NuGenNumberOfTlbImportsPerSecGraph");
			buttonItem153.Click += new EventHandler(this.buttonClick);


            #endregion

            #region Just-In-Time

            ButtonMeterMap.AssociateButton(buttonItem154, "Genetibase.UI.NuGenMeters.NetClrJit.NuGenILBytesJittedPerSecGraph");
			buttonItem154.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem155, "Genetibase.UI.NuGenMeters.NetClrJit.NuGenNumberOfILBytesJittedGraph");
			buttonItem155.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem156, "Genetibase.UI.NuGenMeters.NetClrJit.NuGenNumberOfMethodsJittedGraph");
			buttonItem156.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem157, "Genetibase.UI.NuGenMeters.NetClrJit.NuGenPercentTimeInJitGraph");
			buttonItem157.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem158, "Genetibase.UI.NuGenMeters.NetClrJit.NuGenStandardJitFailuresGraph");
			buttonItem158.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem159, "Genetibase.UI.NuGenMeters.NetClrJit.NuGenTotalNumberOfILBytesJittedGraph");
			buttonItem159.Click += new EventHandler(this.buttonClick);


            #endregion

            #region Loading

            ButtonMeterMap.AssociateButton(buttonItem160, "Genetibase.UI.NuGenMeters.NetClrLoading.NuGenAssemblySearchLengthGraph");
			buttonItem160.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem161, "Genetibase.UI.NuGenMeters.NetClrLoading.NuGenBytesInLoaderHeapGraph");
			buttonItem161.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem162, "Genetibase.UI.NuGenMeters.NetClrLoading.NuGenCurrentAppDomainsGraph");
			buttonItem162.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem163, "Genetibase.UI.NuGenMeters.NetClrLoading.NuGenCurrentAssembliesGraph");
			buttonItem163.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem164, "Genetibase.UI.NuGenMeters.NetClrLoading.NuGenCurrentClassesLoadedGraph");
			buttonItem164.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem165, "Genetibase.UI.NuGenMeters.NetClrLoading.NuGenPercentTimeLoadingGraph");
			buttonItem165.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem166, "Genetibase.UI.NuGenMeters.NetClrLoading.NuGenRateOfAppDomainsGraph");
			buttonItem166.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem167, "Genetibase.UI.NuGenMeters.NetClrLoading.NuGenRateOfAppDomainsUnloadedGraph");
			buttonItem167.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem168, "Genetibase.UI.NuGenMeters.NetClrLoading.NuGenRateOfAssembliesGraph");
			buttonItem168.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem169, "Genetibase.UI.NuGenMeters.NetClrLoading.NuGenRateOfClassesLoadedGraph");
			buttonItem169.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem170, "Genetibase.UI.NuGenMeters.NetClrLoading.NuGenRateOfLoadFailuresGraph");
			buttonItem170.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem171, "Genetibase.UI.NuGenMeters.NetClrLoading.NuGenTotalAppDomainsGraph");
			buttonItem171.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem172, "Genetibase.UI.NuGenMeters.NetClrLoading.NuGenTotalAssembliesGraph");
			buttonItem172.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem173, "Genetibase.UI.NuGenMeters.NetClrLoading.NuGenTotalClassesLoadedGraph");
			buttonItem173.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem174, "Genetibase.UI.NuGenMeters.NetClrLoading.NuGenTotalNumberOfLoadFailuresGraph");
			buttonItem174.Click += new EventHandler(this.buttonClick);

            #endregion

            #region Locks and Threads

            ButtonMeterMap.AssociateButton(buttonItem175, "Genetibase.UI.NuGenMeters.NetClrLocksAndThreads.NuGenContentionRatePerSecGraph");
			buttonItem175.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem176, "Genetibase.UI.NuGenMeters.NetClrLocksAndThreads.NuGenCurrentQueueLengthGraph");
			buttonItem176.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem177, "Genetibase.UI.NuGenMeters.NetClrLocksAndThreads.NuGenNumberOfCurrentLogicalThreadsGraph");
			buttonItem177.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem178, "Genetibase.UI.NuGenMeters.NetClrLocksAndThreads.NuGenNumberOfCurrentPhysicalThreadsGraph");
			buttonItem178.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem179, "Genetibase.UI.NuGenMeters.NetClrLocksAndThreads.NuGenNumberOfCurrentRecognizedThreadsGraph");
			buttonItem179.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem180, "Genetibase.UI.NuGenMeters.NetClrLocksAndThreads.NuGenQueueLengthPeakGraph");
			buttonItem180.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem181, "Genetibase.UI.NuGenMeters.NetClrLocksAndThreads.NuGenQueueLengthPerSecGraph");
			buttonItem181.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem182, "Genetibase.UI.NuGenMeters.NetClrLocksAndThreads.NuGenRateOfRecognizedThreadsPerSecGraph");
			buttonItem182.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem183, "Genetibase.UI.NuGenMeters.NetClrLocksAndThreads.NuGenTotalNumberOfContentionsGraph");
			buttonItem183.Click += new EventHandler(this.buttonClick);

            #endregion

            #region Memory

            ButtonMeterMap.AssociateButton(buttonItem184, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenAllocatedBytesPerSecGraph");
			buttonItem184.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem185, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenFinalizationSurvivorsGraph");
			buttonItem185.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem186, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenGen0HeapSizeGraph");
			buttonItem186.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem187, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenGen0PromotedBytesPerSecGraph");
			buttonItem187.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem188, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenGen2HeapSizeGraph");
			buttonItem188.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem189, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenLargeObjectHeapSizeGraph");
			buttonItem189.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem190, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenNumberOfBytesInAllHeapsGraph");
			buttonItem190.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem191, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenNumberOfGCHandlesGraph");
			buttonItem191.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem192, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenNumberOfGen0CollectionsGraph");
			buttonItem192.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem193, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenNumberOfGen1CollectionsGraph");
			buttonItem193.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem194, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenNumberOfGen2CollectionsGraph");
			buttonItem194.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem195, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenNumberOfInducedGCGraph");
			buttonItem195.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem196, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenNumberOfPinnedObjectsGraph");
			buttonItem196.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem197, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenNumberOfSinkBlocksInUseGraph");
			buttonItem197.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem198, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenNumberOfTotalCommittedBytesGraph");
			buttonItem198.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem199, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenNumberOfTotalReservedBytesGraph");
			buttonItem199.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem200, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenPercentTimeInGCGraph");
			buttonItem200.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem201, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenPromotedFinalizationMemoryFromGen0Graph");
			buttonItem201.Click += new EventHandler(this.buttonClick);

            //Vista Issue: Commented out for the moment because this causes a crash on Vista
            //ButtonMeterMap.AssociateButton(buttonItem202, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenPromotedFinalizationMemoryFromGen1Graph");
			//buttonItem202.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem203, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenPromotedMemoryFromGen0Graph");
			buttonItem203.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem204, "Genetibase.UI.NuGenMeters.NetClrMemory.NuGenPromotedMemoryFromGen1Graph");
			buttonItem204.Click += new EventHandler(this.buttonClick);


            #endregion

            #region Remoting

            ButtonMeterMap.AssociateButton(buttonItem205, "Genetibase.UI.NuGenMeters.NetClrRemoting.NuGenChannelsGraph");
			buttonItem205.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem206, "Genetibase.UI.NuGenMeters.NetClrRemoting.NuGenContextBoundClassesLoadedGraph");
			buttonItem206.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem207, "Genetibase.UI.NuGenMeters.NetClrRemoting.NuGenContextBoundObjectsAllocPerSecGraph");
			buttonItem207.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem208, "Genetibase.UI.NuGenMeters.NetClrRemoting.NuGenContextProxiesGraph");
			buttonItem208.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem209, "Genetibase.UI.NuGenMeters.NetClrRemoting.NuGenContextsGraph");
			buttonItem209.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem210, "Genetibase.UI.NuGenMeters.NetClrRemoting.NuGenRemoteCallsPerSecGraph");
			buttonItem210.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem211, "Genetibase.UI.NuGenMeters.NetClrRemoting.NuGenTotalRemoteCallsGraph");
			buttonItem211.Click += new EventHandler(this.buttonClick);

            #endregion

            #region Security

            ButtonMeterMap.AssociateButton(buttonItem212, "Genetibase.UI.NuGenMeters.NetClrSecurity.NuGenNumberOfLinkTimeChecksGraph");
			buttonItem212.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem213, "Genetibase.UI.NuGenMeters.NetClrSecurity.NuGenPercentTimeInRTChecksGraph");
			buttonItem213.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem214, "Genetibase.UI.NuGenMeters.NetClrSecurity.NuGenPercentTimeInSigAuthenticatingGraph");
			buttonItem214.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem215, "Genetibase.UI.NuGenMeters.NetClrSecurity.NuGenStackWalkDepthGraph");
			buttonItem215.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem216, "Genetibase.UI.NuGenMeters.NetClrSecurity.NuGenTotalRuntimeChecksGraph");
			buttonItem216.Click += new EventHandler(this.buttonClick);


            #endregion

            #endregion

            #region Other

            #region Paging File

            ButtonMeterMap.AssociateButton(buttonItem220, "Genetibase.UI.NuGenMeters.PagingFile.NuGenPercentUsageGraph");
			buttonItem220.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem221, "Genetibase.UI.NuGenMeters.PagingFile.NuGenPercentUsageGraph");
			buttonItem221.Click += new EventHandler(this.buttonClick);

            #endregion

            #region NBT Connection

            ButtonMeterMap.AssociateButton(buttonItem7, "Genetibase.UI.NuGenMeters.NbtConnection.NuGenBytesReceivedPerSecGraph");
			buttonItem7.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem218, "Genetibase.UI.NuGenMeters.NbtConnection.NuGenBytesSentPerSecGraph");
			buttonItem218.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem219, "Genetibase.UI.NuGenMeters.NbtConnection.NuGenBytesTotalPerSecGraph");
			buttonItem219.Click += new EventHandler(this.buttonClick);

            #endregion

            #region Print Queue 222-233

            ButtonMeterMap.AssociateButton(buttonItem222, "Genetibase.UI.NuGenMeters.PrintQueue.NuGenAddNetworkPrinterCallsGraph");
			buttonItem222.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem223, "Genetibase.UI.NuGenMeters.PrintQueue.NuGenBytesPrintedPerSecGraph");
			buttonItem223.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem224, "Genetibase.UI.NuGenMeters.PrintQueue.NuGenEnumerateNetworkPrinterCallsGraph");
			buttonItem224.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem225, "Genetibase.UI.NuGenMeters.PrintQueue.NuGenJobErrorsGraph");
			buttonItem225.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem226, "Genetibase.UI.NuGenMeters.PrintQueue.NuGenJobSpoolingGraph");
			buttonItem226.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem227, "Genetibase.UI.NuGenMeters.PrintQueue.NuGenMaxJobsSpoolingGraph");
			buttonItem227.Click += new EventHandler(this.buttonClick);

            //ButtonMeterMap.AssociateButton(buttonItem228, "Genetibase.UI.NuGenMeters.PrintQueue.NuGenMaxReferencesGraph");
			//buttonItem228.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem229, "Genetibase.UI.NuGenMeters.PrintQueue.NuGenNotReadyErrorsGraph");
			buttonItem229.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem230, "Genetibase.UI.NuGenMeters.PrintQueue.NuGenOutOfPaperErrorsGraph");
			buttonItem230.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem231, "Genetibase.UI.NuGenMeters.PrintQueue.NuGenReferencesGraph");
			buttonItem231.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem232, "Genetibase.UI.NuGenMeters.PrintQueue.NuGenTotalJobsPrintedGraph");
			buttonItem232.Click += new EventHandler(this.buttonClick);

            ButtonMeterMap.AssociateButton(buttonItem233, "Genetibase.UI.NuGenMeters.PrintQueue.NuGenTotalPagesPrintedGraph");
			buttonItem233.Click += new EventHandler(this.buttonClick);

            #endregion

            #endregion
        }

        private delegate void ClickEvent(object sender, EventArgs e);
        private ButtonItem lastButton;
        ClickEvent lastClickEvent;

        public delegate void GraphChangedDelegate(String[] instances, String currentInstance);
        public event GraphChangedDelegate GraphChanged;

        private void buttonClick(object sender, EventArgs e)
        {
            ButtonItem button = (ButtonItem)sender;

            label14.Text = button.Text;
            lastButton = button;
            lastClickEvent = buttonClick;                            

            //The following code is to dynamically generate a class from a string
            //Then it dynamically compiles a block of code to pass that object to "SetGraph"
            String name = Assembly.CreateQualifiedName("Genetibase.UI.NuGenMeters.Full", ButtonMeterMap.GetGraph(button));
            Type t = Type.GetType(name);

            SetGraph((NuGenGraphGeneric)Activator.CreateInstance(t));

            uiTab1.SelectedIndex = 1;
        }

        public String NetworkInstance
        {
            get
            {
                PerformanceCounterCategory cat = new PerformanceCounterCategory("Network Interface");
                return cat.GetInstanceNames()[0];
            }
        }

        public void SetGraph(NuGenGraphGeneric graph)
        {
            uiTabPage2.Controls.Clear();

            Janus.Windows.EditControls.UIGroupBox uiGroupBox3 = new Janus.Windows.EditControls.UIGroupBox();
            uiGroupBox3.BackgroundStyle = Janus.Windows.EditControls.BackgroundStyle.ExplorerBarGroupBackground;
            uiGroupBox3.Location = new System.Drawing.Point(35, 35);
            uiGroupBox3.Name = "uiGroupBox3";
            uiGroupBox3.Size = new System.Drawing.Size(uiTabPage2.Width - 70, uiTabPage2.Height - 70);
            uiGroupBox3.TabIndex = 2;
            uiGroupBox3.Text = "Graph";
            uiGroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            uiGroupBox3.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;

            graph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            graph.BackgroundTransparency = 0;
            graph.ForegroundTransparency = 0;
            graph.GridTransparency = 0;
            graph.Location = new System.Drawing.Point(15, 75);
            graph.MachineName = ".";
            graph.Size = new System.Drawing.Size(uiGroupBox3.Width - 30, uiGroupBox3.Height - 90);
            graph.TabIndex = 2;

            uiGroupBox3.Controls.Add(graph);
            uiGroupBox3.Controls.Add(this.label14);
            uiTabPage2.Controls.Add(uiGroupBox3);

            String[] instances = new PerformanceCounterCategory(graph.CategoryName).GetInstanceNames();            

            foreach (String instance in instances)
            {
                if (instance == Instance)
                {
                    graph.InstanceName = Instance;                    
                }
            }

            if (GraphChanged != null)
                GraphChanged(new PerformanceCounterCategory(graph.CategoryName).GetInstanceNames(), graph.InstanceName);
        }


        public void ShowHome()
        {
            this.uiTab1.SelectedIndex = 0;            
        }

        private void uiTab1_SelectedTabChanged(object sender, Janus.Windows.UI.Tab.TabEventArgs e)
        {

        }
    }
}
