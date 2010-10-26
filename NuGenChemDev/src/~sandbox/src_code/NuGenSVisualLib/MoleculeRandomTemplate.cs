using System;
using System.Collections.Generic;
using System.Text;
using NuGenSVisualLib.Settings;
using System.Drawing;
using Org.OpenScience.CDK.Config;
using Org.OpenScience.CDK;

namespace NuGenSVisualLib.Rendering.Chem.Materials
{
    class MoleculeRandomTemplate : MoleculeMaterialsModule
    {
        public MoleculeRandomTemplate()
            : base("Random")
        { }

        public override void LoadModuleSettings(HashTableSettings settings)
        {
            // Load random template
            Random ran = new Random();

            // series
            foreach (string serie in serieNames)
            {
                int r = (int)(ran.NextDouble() * 255f);
                int g = (int)(ran.NextDouble() * 255f);
                int b = (int)(ran.NextDouble() * 255f);
                Color baseColor = Color.FromArgb(r, g, b);

                series[serie] = new AtomMaterial(baseColor);
            }

            // elements
            ElementPTFactory ptElements = ElementPTFactory.Instance;
            foreach (PeriodicTableElement element in ptElements)
            {
                int r = (int)(ran.NextDouble() * 255f);
                int g = (int)(ran.NextDouble() * 255f);
                int b = (int)(ran.NextDouble() * 255f);
                Color baseColor = Color.FromArgb(r, g, b);

                IMoleculeMaterial serieMat = null;
                series.TryGetValue(element.ChemicalSerie, out serieMat);
                elements[element.Symbol] = new MoleculeMaterialTemplate(new AtomMaterial(baseColor), serieMat);
            }
        }
    }
}