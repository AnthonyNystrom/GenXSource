using System;
using System.Collections.Generic;
using System.Text;
using NuGenSVisualLib.Rendering;
using Org.OpenScience.CDK.Interfaces;
using Microsoft.DirectX;

namespace NuGenSVisualLib.Rendering.TwoD
{
    /// <summary>
    /// Encapsulates a chemistry related 2D rendering source
    /// </summary>
    public class ChemRenderingSource2D : RenderingSource2D, IChemRenderingSource
    {
        protected IChemFile file;

        public ChemRenderingSource2D(IChemFile file)
        {
            this.file = file;
        }

        #region IChemRenderingSource Members

        public IChemFile ChemFile
        {
            get { return file; }
        }

        #endregion

        public override void SourceModified()
        {
            CalcBounds2D();   
        }

        private void CalcBounds2D()
        {
            Vector2 max = new Vector2(), min = new Vector2();
            bool first = true;

            foreach (IChemSequence sequence in file.ChemSequences)
            {
                foreach (IChemModel model in sequence.ChemModels)
                {
                    if (model.SetOfMolecules != null)
                    {
                        foreach (IMolecule molecule in model.SetOfMolecules.Molecules)
                        {
                            foreach (IAtom atom in molecule.Atoms)
                            {
                                if (first)
                                {
                                    min.X = max.X = (float)atom.X3d;
                                    min.Y = max.Y = (float)atom.Y3d;
                                    first = false;
                                }
                                else
                                {
                                    if (atom.X3d > max.X)
                                        max.X = (float)atom.X3d;
                                    else if (atom.X3d < min.X)
                                        min.X = (float)atom.X3d;

                                    if (atom.Y3d > max.Y)
                                        max.Y = (float)atom.Y3d;
                                    else if (atom.Y3d < min.Y)
                                        min.Y = (float)atom.Y3d;
                                }
                            }
                        }
                    }

                }
            }

            // calc origin
            Vector2 origin = new Vector2(max.X - min.X, max.Y - min.Y);
            origin *= 0.5f;
            origin.X += min.X;
            origin.Y += min.Y;

            this.bounds.min = min;
            this.bounds.max = max;

            this.origin = origin;
        }

        public string Title
        {
            get
            {
                return file.ID;
            }
        }
    }
}