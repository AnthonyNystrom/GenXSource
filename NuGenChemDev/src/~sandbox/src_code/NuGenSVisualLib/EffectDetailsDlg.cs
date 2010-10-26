using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NuGenSVisualLib.Rendering.Effects;
using NuGenSVisualLib.Settings;

namespace NuGenSVisualLib
{
    public partial class EffectDetailsDlg : Form
    {
        public EffectDetailsDlg()
        {
            InitializeComponent();
        }

        public void SetEffect(RenderingEffect effect)
        {
            label2.Text = effect.Name;
            label3.Text = effect.EfxType.ToString();
            label1.Text = effect.Description;

            // load image
            string base_path = (string)HashTableSettings.Instance["Base.Path"];

            pictureBox1.Image = Image.FromFile(string.Format(@"{0}Media\UI\previews\effects\{1}-big.jpg", base_path, effect.Name));
        }
    }
}