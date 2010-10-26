using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace NuGenSVisualLib.Rendering.Chem.Schemes
{
    partial class BallAndStickSchemeSUI : UserControl, MoleculeSchemeSUI
    {
        BallAndStickSchemeSettings settings;
        event EventHandler<SchemeSUIChangeHandler> OnValueChanged;
        object lockObj;

        public BallAndStickSchemeSUI(BallAndStickSchemeSettings settings)
        {
            InitializeComponent();

            this.settings = settings;
        }

        #region MoleculeSchemeSUI Members

        public void UpdateValues()
        {
            integerUpDown1.Value = settings.StickThickness;
            integerUpDown2.Value = settings.Glow;
            uiCheckBox1.Checked = settings.ElectronChargeCloud;
        }

        public void SetChangeEvent(object lockObj, EventHandler<SchemeSUIChangeHandler> handler)
        {
            this.lockObj = lockObj;
            OnValueChanged += handler;
        }

        #endregion

        private void integerUpDown1_ValueChanged(object sender, EventArgs e)
        {
            lock (lockObj)
            {
                settings.StickThickness = integerUpDown1.Value;
            }
            if (OnValueChanged != null)
                OnValueChanged(this, new SchemeSUIChangeHandler(false, true));
        }

        private void integerUpDown2_ValueChanged(object sender, EventArgs e)
        {
            lock (lockObj)
            {
                settings.Glow = integerUpDown2.Value;
            }
            if (OnValueChanged != null)
                OnValueChanged(this, new SchemeSUIChangeHandler(true, false));
        }

        private void uiCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            lock (lockObj)
            {
                settings.ElectronChargeCloud = uiCheckBox1.Checked;
            }
            if (OnValueChanged != null)
                OnValueChanged(this, new SchemeSUIChangeHandler(true, false));
        }
    }
}
