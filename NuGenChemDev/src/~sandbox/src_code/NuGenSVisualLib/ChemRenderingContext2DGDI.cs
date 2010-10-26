using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Org.OpenScience.CDK.Interfaces;
using System.Drawing.Drawing2D;

namespace NuGenSVisualLib.Rendering.TwoD
{
    /// <summary>
    /// Encapsulates a 2D rendering platform using GDI in a chemistry context
    /// </summary>
    //public class ChemRenderingContext2DGDI : RenderingContext2DGDI, IChemRenderingContext
    //{
    //    public override void Render(Graphics g)
    //    {
    //        g.Clear(background);

    //        ChemRenderingSource2D renderSource = (ChemRenderingSource2D)this.renderSource;

    //        // Draw source chemistry
    //        if (renderSource != null)
    //        {
    //            foreach (IChemSequence sequence in renderSource.ChemFile.ChemSequences)
    //            {
    //                foreach (IChemModel model in sequence.ChemModels)
    //                {
    //                    DrawModel(model, g);
    //                }
    //            }
    //        }

    //        g.Flush();
    //    }

    //    private void DrawModel(IChemModel model, Graphics g)
    //    {
    //        //g.Transform.Translate(-50, -50);
    //        if (model.SetOfMolecules != null)
    //        {
    //            foreach (IMolecule molecule in model.SetOfMolecules.Molecules)
    //            {
    //                foreach (IBond bond in molecule.Bonds)
    //                {
    //                }
    //                foreach (IAtom atom in molecule.Atoms)
    //                {
    //                    int x = -(int)(atom.X3d * 10f);
    //                    int y = -(int)(atom.Z3d * 10f);
    //                    g.FillEllipse(Brushes.Blue, x, y, 3, 3);
    //                }
    //            }
    //        }
    //    }

    //    public override void Dispose()
    //    {
    //        throw new Exception("The method or operation is not implemented.");
    //    }

    //    public override void OnResize(int width, int height)
    //    {
    //        throw new Exception("The method or operation is not implemented.");
    //    }
    //}
}
