using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Netron.Neon.WinFormsUI;

using MathNet.Symbolics.Core;
using MathNet.Symbolics.Workplace;
using MathNet.Symbolics.Backend;
using MathNet.Symbolics.Backend.Containers;
using MathNet.Symbolics.Backend.Channels;
using MathNet.Symbolics.Backend.Channels.Commands;
using MathNet.Symbolics.Presentation.WinDiagram;

using MathNet.Symbolics.StdPackage;
using MathNet.Symbolics.StdPackage.Properties;
using MathNet.Symbolics.StdPackage.Structures;

using Application = Netron.Cobalt.Application;
namespace Yttrium.Whiteboard
{
    public partial class ControlPanel : DockContent
    {
        private Project _project;
        private NetronController _ctrl;

        private Entity _currentEntity;
        private EntityTable _table;

        private MathSystem s;
        public ControlPanel()
        {
            InitializeComponent();
       
        }

        private void btnNewPort_Click(object sender, EventArgs e)
        {
            ShowSelector();
        }

        private void ShowSelector()
        {
            entitySelector.Visible = true;

            btnBuildSample.Visible = false;
            btnEvaluate.Visible = false;
            btnNewBus.Visible = false;
            btnNewPort.Visible = false;
            btnNewSignal.Visible = false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            NewPortCommand cmd = new NewPortCommand();
            cmd.EntityId = _currentEntity.EntityId;
            cmd.NumberOfInputs = (int) udInputs.Value;
            cmd.NumberOfBuses = (int) udBuses.Value;
            _ctrl.PostCommand(cmd);

            HideSelector();
        }

        private void HideSelector()
        {
            entitySelector.Visible = false;

            btnBuildSample.Visible = true;
            btnEvaluate.Visible = true;
            btnNewBus.Visible = true;
            btnNewPort.Visible = true;
            btnNewSignal.Visible = true;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _project = new Project();
            _ctrl = new NetronController(_project, Application.Diagram);

            // attach a system logger (with console output), something
            // we get for free thanks to the channels subsystem (mediator/observer).
            LogObserver lo = new LogObserver(new TextLogWriter(Console.Out));
            _project.AttachLocalObserver(lo);
            _table = _project.Context.Library.Entities;
            UpdateEntities();
            
        }

        public void UpdateEntities()
        {
            cmbEntities.Items.Clear();
            foreach (Entity et in _table.SelectAll())
                cmbEntities.Items.Add(et);
        }
        private void cmbEntities_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentEntity = cmbEntities.SelectedItem as Entity;
            if (_currentEntity.IsGeneric)
            {
                udInputs.Enabled = true;
                udBuses.Enabled = true;
            }
            else
            {
                udInputs.Enabled = false;
                udBuses.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            HideSelector();
        }

        private void btnBuildSample_Click(object sender, EventArgs e)
        {
            s = _project.CurrentSystem;
            Context c = s.Context;
            Builder b = c.Builder;
            Signal x = new Signal(c); x.Label = "x";
            x.AddConstraint(RealSetProperty.Instance);
            Signal x2 = b.Square(x); x2.Label = "x2";
            Signal sinx2 = Std.Sine(c, x2); sinx2.Label = "sinx2";
            Signal sinx2t2 = sinx2 * IntegerValue.ConstantTwo(c);
            Signal sinx2t2cosx = sinx2t2 + Std.Cosine(c, x); sinx2t2cosx.Label = "sinx2t2cosx";
            s.AddSignalTree(sinx2t2, true, true);
            
        }

        private void btnEvaluate_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 30; i++)
            {
                double[] vs = s.Evaluate(new double[1] { i });
                Application.Output.WriteLine("Yttrium output", vs[0].ToString());
            }
            DockPane pane= new DockPane(Application.Tabs.Property, DockState.DockLeft, false);

            Application.Tabs.Output.Show(pane, Application.Tabs.Property);
            //DockPane pane = new DockPane(Application.Tabs.Property, DockState.Document, true);
            
            //controlPanel.Show(pane, DockAlignment.d, 0.1);
        }

        private void btnNewSignal_Click(object sender, EventArgs e)
        {
            _ctrl.PostCommandNewSignal();
        }

        private void btnNewBus_Click(object sender, EventArgs e)
        {
            _ctrl.PostCommandNewBus();

        }
    }
}