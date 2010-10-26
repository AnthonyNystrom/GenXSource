using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NuGenSVisualLib.Rendering.Chem;
using Org.OpenScience.CDK;
using Org.OpenScience.CDK.Config;
using NuGenSVisualLib.Rendering.Chem.Materials;

namespace NuGenSVisualLib
{
    partial class PeriodicTableControl : UserControl
    {
        MoleculeMaterialsModule material;
        int ptWidth, ptHeight, elWidth, elHeight;
        Size tableOrigin;

        PeriodicTableElement selectedElement;

        public event EventHandler OnElementSelect;

        public PeriodicTableControl()
        {
            InitializeComponent();

            this.BackColor = Color.White;
        }

        public PeriodicTableElement SelectedElement
        {
            get { return selectedElement; }
        }

        public MoleculeMaterialsModule MaterialsModule
        {
            get { return material; }
        }

        public void SetMaterialsModule(MoleculeMaterialsModule materialsModule)
        {
            material = materialsModule;
            GenerateTable();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            material = new MoleculeDefaultMaterials();
            material.LoadModuleSettings(null);


            pictureBox1.Image = material.DrawPTToBitmap(this.Width, this.Height,
                                                        true, true, true, true, true,
                                                        out ptWidth, out ptHeight, out elWidth,
                                                        out elHeight, out tableOrigin);
        }

        public void GenerateTable()
        {
            material.DrawPT((Bitmap)pictureBox1.Image, elWidth, elHeight, true, true, true, true);
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            selectedElement = null;

            if (!Enabled)
                return;

            // test hit on element
            int group = 19 * (ptWidth / (e.X - tableOrigin.Width));
            int period = 8 * (ptHeight / (e.Y - tableOrigin.Height));

            if (group > 1 && period > 1 && group < 20 && period < 9)
            {
                // look for element fitting desc
                foreach (PeriodicTableElement element in ElementPTFactory.Instance)
                {
                    if (element.Group.Length > 0)
                    {
                        if (group == int.Parse(element.Group) &&
                            period == int.Parse(element.Period))
                        {
                            selectedElement = element;
                            break;
                        }
                    }
                }
            }

            if (OnElementSelect != null)
                OnElementSelect(this, null);
        }
    }
}
