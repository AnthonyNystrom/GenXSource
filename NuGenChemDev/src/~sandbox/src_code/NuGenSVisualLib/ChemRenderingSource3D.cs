using System;
using System.Collections.Generic;
using System.Text;
using Org.OpenScience.CDK.Interfaces;
using Microsoft.DirectX;
using NuGenJmol;

namespace NuGenSVisualLib.Rendering.ThreeD
{
    /// <summary>
    /// Encapsulates a chemistry related 3D rendering source
    /// </summary>
    public class ChemRenderingSource3D : RenderingSource3D, IChemRenderingSource
    {
        protected IChemFile file;
        public NuSceneBuffer3D sceneBuffer;

        public ChemRenderingSource3D(IChemFile file)
        {
            this.file = file;
            SourceModified();
        }

        #region IChemRenderingSource Members

        public IChemFile ChemFile
        {
            get { return file; }
        }

        #endregion

        public override void SourceModified()
        {
            CalcBounds3D();
        }

        private void CalcBounds3D()
        {
            Vector3 max = new Vector3(), min = new Vector3();
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
                                    min.Z = max.Z = (float)atom.Z3d;
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

                                    if (atom.Z3d > max.Z)
                                        max.Z = (float)atom.Z3d;
                                    else if (atom.Z3d < min.Z)
                                        min.Z = (float)atom.Z3d;
                                }
                            }
                        }
                    }

                }
            }

            // calc origin
            Vector3 origin = new Vector3(max.X - min.X, max.Y - min.Y, max.Z - min.Z);
            origin *= 0.5f;
            origin.X += min.X;
            origin.Y += min.Y;
            origin.Z += min.Z;

            this.bounds = new Bounds3D();
            this.bounds.min = min;
            this.bounds.max = max;

            float toMin = (min - origin).Length();
            float toMax = (max - origin).Length();

            this.bounds.radius = toMin > toMax ? toMin : toMax;

            this.origin = origin;
        }
    }
}
