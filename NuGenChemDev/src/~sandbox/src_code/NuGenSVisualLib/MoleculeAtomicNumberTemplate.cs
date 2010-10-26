using System;
using System.Collections.Generic;
using System.Text;
using Org.OpenScience.CDK.Config;
using Org.OpenScience.CDK;
using System.Drawing;

namespace NuGenSVisualLib.Rendering.Chem.Materials
{
    class MoleculeAtomicNumberTemplate : MoleculeMaterialsModule
    {
        public MoleculeAtomicNumberTemplate()
            : base("AtomicNumber")
        { }

        public override void LoadModuleSettings(NuGenSVisualLib.Settings.HashTableSettings settings)
        {
            // ignore series

            // elements
            ElementPTFactory ptElements = ElementPTFactory.Instance;
            foreach (PeriodicTableElement element in ptElements)
            {
                int r = 0;
                int g = 0;
                int b = 0;

                // calc group (103/35)
                int group = (int)((float)element.AtomicNumber / 35f);

                // calc shade
                int index = element.AtomicNumber - (group * 35);
                int clr1, clr2, clr3;
                int amt = (int)((float)index * 5.55f);
                if (index < 18)
                {
                    // light
                    clr1 = 255;
                    clr2 = clr3 = 100 - amt;
                }
                else
                {
                    // dark
                    clr1 = 255 - amt;
                    clr2 = clr3 = 0;
                }

                if (group == 0)
                {
                    r = clr1;
                    g = clr2;
                    b = clr3;
                }
                else if (group == 1)
                {
                    r = clr2;
                    g = clr1;
                    b = clr3;
                }
                else
                {
                    r = clr2;
                    g = clr3;
                    b = clr1;
                }

                Color baseColor = Color.FromArgb(r, g, b);
                elements[element.Symbol] = new MoleculeMaterialTemplate(new AtomMaterial(baseColor), null);
            }
        }
    }
}
