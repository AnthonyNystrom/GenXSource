using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MathNet.Symbolics.Core;
using MathNet.Symbolics.Workplace;
using MathNet.Symbolics.Backend;
using MathNet.Symbolics.Backend.Containers;

namespace Yttrium.Whiteboard.UI
{
    public partial class UserControl1 : UserControl
    {
        private Entity _currentEntity;
        private EntityTable _table;

        public event EventHandler CompiledEntitySelected;
        public event EventHandler Canceled;

        public UserControl1()
        {
            InitializeComponent();
        }

        public EntityTable Entities
        {
            get { return _table; }
            set { _table = value; }
        }

        public Entity Entity
        {
            get { return _currentEntity; }
        }

        public int NumberOfInputs
        {
            get { return (int)udInputs.Value; }
        }

        public int NumberOfBuses
        {
            get { return (int)udBuses.Value; }
        }

        public void UpdateEntities()
        {
            cmbEntities.Items.Clear();
            foreach (Entity et in _table.SelectAll())
                cmbEntities.Items.Add(et);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (CompiledEntitySelected != null)
                CompiledEntitySelected(this, EventArgs.Empty);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (Canceled != null)
                Canceled(this, EventArgs.Empty);
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
    }
}
